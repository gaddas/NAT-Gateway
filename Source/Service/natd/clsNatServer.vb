' NetGate
' Copyright (c) 2008-2009, Danail Dimitrov

' This file is part of NetGate.

' NetGate is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' NetGate is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.

' You should have received a copy of the GNU General Public License
' along with NetGate. If not, see <http://www.gnu.org/licenses/>.


Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles
Imports SharpPcap
Imports SharpPcap.Packets


Public Class natServer
    Inherits MarshalByRefObject

    Implements NetGate.remoteObjectNAT
    Implements IDisposable

    ' Make object live forever
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Public Function Test(ByVal x As Integer) As Integer Implements remoteObjectNAT.Test
        Return x
    End Function

    Public Function GetConfig() As String Implements remoteObjectNAT.GetConfig
        Dim f As New IO.FileStream("natd.conf", IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim r As New IO.StreamReader(f)
        Dim c As String
        c = r.ReadToEnd()
        r.Close()
        f.Close()

        Return c
    End Function

    Public Sub SetConfig(ByVal config As String) Implements remoteObjectNAT.SetConfig
#If DEBUG Then
        Console.WriteLine(config)
#End If

        Console.Write("Stopping reader threads...")
        m_Loop = False
        While m_Thread1.IsAlive OrElse m_Thread2.IsAlive
            Console.Write(".")
            Threading.Thread.Sleep(500)
        End While
        m_Loop = True
        Console.WriteLine("DONE!")

        Dim natdConfBak As New IO.FileInfo("natd.conf.bak")
        natdConfBak.Delete()

        Dim natdConfOld As New IO.FileInfo("natd.conf")
        natdConfOld.MoveTo("natd.conf.bak")

        Dim natdConfNew As New IO.FileStream("natd.conf", IO.FileMode.Create, IO.FileAccess.Write)
        Dim w As New IO.StreamWriter(natdConfNew)
        w.Write(config)
        w.Flush()
        w.Close()
        natdConfNew.Close()

        m_Server.m_ConfigData = config
        m_Server.m_Config = New natConfig
        m_Server.m_Config.ReadConfig(config)

        m_Thread1 = New Threading.Thread(AddressOf BeginReadGREEN)
        m_Thread1.Start()
        m_Thread2 = New Threading.Thread(AddressOf BeginReadRED)
        m_Thread2.Start()
    End Sub

    Public Function GetInterfaces() As Dictionary(Of String, String) Implements remoteObjectNAT.GetInterfaces
        Dim devices As PcapDeviceList = New SharpPcap.PcapDeviceList()
        Dim result As New Dictionary(Of String, String)

        For Each dev As PcapDevice In devices
            result.Add(dev.Description, dev.Name)
        Next
        Return result
    End Function

    Public Function GetGateway() As System.Net.IPAddress Implements remoteObjectNAT.GetGateway
        Dim result As Net.IPAddress = Nothing

        For Each i As Net.NetworkInformation.NetworkInterface In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
            For Each gateway As Net.NetworkInformation.GatewayIPAddressInformation In i.GetIPProperties().GatewayAddresses
                result = gateway.Address
            Next
        Next

        Return result
    End Function

#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region


    Public m_ConfigData As String
    Public m_Config As natConfig
    Public m_Loop As Boolean = True

    Public m_Thread1 As Threading.Thread
    Public m_Thread2 As Threading.Thread

    Public Sub New()
#If DEBUG Then
        Console.WriteLine("natServer.New()")
#End If

        ' Open our config file
        Dim f As New IO.StreamReader(New IO.FileStream("natd.conf", IO.FileMode.Open))
        m_ConfigData = f.ReadToEnd()
        f.Close()

        ' Load options from config file
        m_Config = New natConfig
        m_Config.ReadConfig(m_ConfigData)
        m_Server = Me

        m_ArpResolverGREEN = New ArpResolver(m_Config.iGREEN.hInterface.Name)
        m_ArpResolverGREEN.LocalIP = m_Config.ipGreen
        m_ArpResolverGREEN.LocalMAC = m_Config.hwGreen
        m_ArpResolverRED = New ArpResolver(m_Config.iRED.hInterface.Name)
        m_ArpResolverRED.LocalIP = m_Config.ipRed
        m_ArpResolverRED.LocalMAC = m_Config.hwRed

        m_Config.extGatewayHw = m_ArpResolverRED.Resolve(m_Config.extGatewayIP)

        m_Thread1 = New Threading.Thread(AddressOf BeginReadGREEN)
        m_Thread1.Start()
        m_Thread2 = New Threading.Thread(AddressOf BeginReadRED)
        m_Thread2.Start()

        Console.WriteLine("ready!")
    End Sub


    ' Conntrack keeps data for RED("IP:PORT") <=NAT=> GREEN(IP)
    Public m_ConntrackLock As New Threading.ReaderWriterLock
    Public m_Conntrack As New Dictionary(Of String, IPAddress)
    Public m_ConntrackTimeToLive As New Dictionary(Of String, DateTime)

    Private Function ToProtocol(ByVal p As String) As UShort
        Select Case p
            Case "TCP"
                Return FIREWALL_PROTOCOL_TCP
            Case "UDP"
                Return FIREWALL_PROTOCOL_UDP
            Case "ICMP"
                Return FIREWALL_PROTOCOL_ICMP
            Case Else
                Return FIREWALL_PROTOCOL_ANY
        End Select
    End Function

    Public Sub ConntrackPut(ByVal p As String, ByVal sAddr As IPAddress, ByVal sPort As UShort, ByVal dAddr As IPAddress, ByVal dPort As UShort)
        Dim s As String = p & ":" & sAddr.ToString & ":" & sPort
        Console.WriteLine("conntrack add {0} <- {1} ", s, dAddr.ToString)

        m_ConntrackLock.AcquireWriterLock(LOCK_TIMEOUT)
        m_Conntrack(s) = dAddr
        m_ConntrackTimeToLive(s) = Now.AddSeconds(TTL_CONNTRACK)
        m_ConntrackLock.ReleaseWriterLock()

        FirewallAddRule(True, ToProtocol(p), sAddr, IPAddress.Broadcast, sPort, IPAddress.Any, IPAddress.Broadcast, dPort)
    End Sub
    Public Sub ConntrackRemove(ByVal p As String, ByVal sAddr As IPAddress, ByVal sPort As UShort, ByVal dPort As UShort)
        Dim s As String = p & ":" & sAddr.ToString & ":" & sPort
        Console.WriteLine("conntrack del {0}", s)

        m_ConntrackLock.AcquireWriterLock(LOCK_TIMEOUT)
        m_Conntrack.Remove(s)
        m_ConntrackTimeToLive.Remove(s)
        m_ConntrackLock.ReleaseWriterLock()

        FirewallRemoveRule(True, ToProtocol(p), sAddr, IPAddress.Broadcast, sPort, IPAddress.Any, IPAddress.Broadcast, dPort)
    End Sub
    Public Function ConntrackHave(ByVal p As String, ByVal sAddr As IPAddress, ByVal sPort As UShort, ByVal dPort As UShort) As Boolean
        Dim s As String = p & ":" & sAddr.ToString & ":" & sPort
        Dim flag As Boolean
        m_ConntrackLock.AcquireReaderLock(LOCK_TIMEOUT)
        flag = m_Conntrack.ContainsKey(s)
        m_ConntrackLock.ReleaseReaderLock()
        Return flag
    End Function
    Public Function ConntrackGet(ByVal p As String, ByVal sAddr As IPAddress, ByVal sPort As UShort, ByVal dPort As UShort) As IPAddress
        Dim s As String = p & ":" & sAddr.ToString & ":" & sPort
        Dim ip As IPAddress
        m_ConntrackLock.AcquireReaderLock(LOCK_TIMEOUT)
        ip = m_Conntrack(s)
        m_ConntrackLock.ReleaseReaderLock()
        Return ip
    End Function


    Public m_ArpCache As New Dictionary(Of IPAddress, NetworkInformation.PhysicalAddress)
    Public m_ArpResolverGREEN As ArpResolver
    Public m_ArpResolverRED As ArpResolver

    Public Function GetHwAddress(ByVal ip As IPAddress, ByVal i As EthernetInterface) As NetworkInformation.PhysicalAddress
        If ip.Equals(m_Config.ipGreen) AndAlso i = EthernetInterface.GREEN Then Return m_Config.hwGreen
        If ip.Equals(m_Config.ipRed) AndAlso i = EthernetInterface.RED Then Return m_Config.hwRed

        If m_ArpCache.ContainsKey(ip) Then
            Return m_ArpCache(ip)
        Else
            Select Case i
                Case EthernetInterface.GREEN
                    m_ArpCache.Add(ip, m_ArpResolverGREEN.Resolve(ip))
                Case EthernetInterface.RED
                    m_ArpCache.Add(ip, m_ArpResolverRED.Resolve(ip))
            End Select
            Return m_ArpCache(ip)
        End If
    End Function


    Public Sub BeginReadGREEN()
        EventLog.WriteEntry("NetGate NAT", "GREEN Thread started.")

        Dim p As Packet = Nothing
        While m_Loop
            Try
                p = Nothing
                p = m_Config.iGREEN.Read


                If (p IsNot Nothing) AndAlso (TypeOf (p) Is IPPacket) Then
                    Dim ip As IPPacket = p
                    'DumpPacket(p)

                    ' IPv6 packets are not routed
                    If CType(p, IPPacket).IPVersion = IPPacket.IPVersions.IPv6 Then Continue While
                    ' Broadcasts are not routed
                    If CType(p, IPPacket).DestinationAddress.ToString.Contains("255") OrElse CType(p, IPPacket).SourceAddress.ToString.Contains("255") Then Continue While
                    ' Multicasts are not routed
                    If CType(p, IPPacket).DestinationAddress.ToString.Contains("224.0.0.") OrElse CType(p, IPPacket).SourceAddress.ToString.Contains("224.0.0.") Then Continue While

                    If ip.DestinationAddress.Equals(m_Config.ipGreen) Then
                        ChainPREROUTING(ip, EthernetInterface.GREEN, EthernetInterface.NONE)
                        ChainINPUT(ip, EthernetInterface.GREEN, EthernetInterface.NONE)
                    ElseIf ip.SourceAddress.Equals(m_Config.ipGreen) Then
                        ChainOUTPUT(ip, EthernetInterface.NONE, EthernetInterface.GREEN)
                        ChainPOSTROUTING(ip, EthernetInterface.NONE, EthernetInterface.GREEN)
                    Else
                        ChainPREROUTING(ip, EthernetInterface.GREEN, EthernetInterface.RED)
                        ChainFORWARD(ip, EthernetInterface.GREEN, EthernetInterface.RED)
                        'ChainPOSTROUTING called inside ChainFORWARD
                    End If

                Else
                    Threading.Thread.Sleep(100)
                End If
            Catch ex As Exception
                EventLog.WriteEntry("NetGate NAT", String.Format("Error on GREEN interface: {0}{1}", vbNewLine, ex.ToString))
                Console.WriteLine(String.Format("Error on GREEN interface: {0}{1}", vbNewLine, ex.ToString))
            End Try

        End While
        EventLog.WriteEntry("NetGate NAT", "GREEN Thread stopped.")
    End Sub
    Public Sub BeginReadRED()
        EventLog.WriteEntry("NetGate NAT", "RED Thread started.")
        Dim p As Packet = Nothing
        While m_Loop
            Try
                p = Nothing
                p = m_Config.iRED.Read


                If (p IsNot Nothing) AndAlso (TypeOf (p) Is IPPacket) Then
                    Dim ip As IPPacket = p
                    'DumpPacket(p)

                    ' IPv6 packets are not routed
                    If CType(p, IPPacket).IPVersion = IPPacket.IPVersions.IPv6 Then Continue While
                    ' Broadcasts are not routed
                    If CType(p, IPPacket).DestinationAddress.ToString.Contains("255") OrElse CType(p, IPPacket).SourceAddress.ToString.Contains("255") Then Continue While
                    ' Multicasts are not routed
                    If CType(p, IPPacket).DestinationAddress.ToString.Contains("224.0.0.") OrElse CType(p, IPPacket).SourceAddress.ToString.Contains("224.0.0.") Then Continue While

                    If (TypeOf (p) Is TCPPacket) AndAlso ConntrackHave("TCP", ip.SourceAddress, CType(ip, TCPPacket).SourcePort, CType(ip, TCPPacket).DestinationPort) Then
                        ChainPREROUTING(ip, EthernetInterface.RED, EthernetInterface.GREEN)
                        ChainFORWARD(ip, EthernetInterface.RED, EthernetInterface.GREEN)
                        'ChainPOSTROUTING called inside ChainFORWARD
                    ElseIf (TypeOf (p) Is UDPPacket) AndAlso ConntrackHave("UDP", ip.SourceAddress, CType(ip, UDPPacket).SourcePort, CType(ip, UDPPacket).DestinationPort) Then
                        ChainPREROUTING(ip, EthernetInterface.RED, EthernetInterface.GREEN)
                        ChainFORWARD(ip, EthernetInterface.RED, EthernetInterface.GREEN)
                        'ChainPOSTROUTING called inside ChainFORWARD

                    ElseIf ip.DestinationAddress.Equals(m_Config.ipRed) Then
                        ChainPREROUTING(ip, EthernetInterface.RED, EthernetInterface.NONE)
                        ChainINPUT(ip, EthernetInterface.RED, EthernetInterface.NONE)
                    ElseIf ip.SourceAddress.Equals(m_Config.ipRed) Then
                        ChainOUTPUT(ip, EthernetInterface.NONE, EthernetInterface.RED)
                        ChainPOSTROUTING(ip, EthernetInterface.NONE, EthernetInterface.RED)
                    Else
                        ChainPREROUTING(ip, EthernetInterface.RED, EthernetInterface.GREEN)
                        ChainFORWARD(ip, EthernetInterface.RED, EthernetInterface.GREEN)
                        'ChainPOSTROUTING called inside ChainFORWARD
                    End If

                Else
                    Threading.Thread.Sleep(100)
                End If
            Catch ex As Exception
                EventLog.WriteEntry("NetGate NAT", String.Format("Error on GREEN interface: {0}{1}", vbNewLine, ex.ToString))
                Console.WriteLine(String.Format("Error on GREEN interface: {0}{1}", vbNewLine, ex.ToString))
            End Try

        End While
        EventLog.WriteEntry("NetGate NAT", "RED Thread stopped.")
    End Sub


End Class

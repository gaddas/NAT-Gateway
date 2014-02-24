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


Imports System.Runtime.Remoting
Imports System.Runtime.InteropServices
Imports System.Diagnostics.Process
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Threading
Imports System.ComponentModel


Public Module modCommon

    Public Const LOCK_TIMEOUT As Integer = 9000

    Public m_Server As dhcpServer
    Public m_RemoteChannel As Channels.IChannel = Nothing
    Public m_RemoteFactory As remoteFactoryDHCP = Nothing
    Public m_RemoteFactoryObj As ObjRef = Nothing


    Public m_Lock As New ReaderWriterLock
    Public m_PoolFree As New List(Of IPAddress)
    Public m_PoolUsed As New List(Of IPAddress)
    Public m_PoolBad As New List(Of IPAddress)
    Public m_Leases As New Dictionary(Of IPAddress, Date)
    Public m_Clients As New Dictionary(Of IPAddress, String)
    Public m_Hosts As New Dictionary(Of IPAddress, String)

    Public m_Ping As New System.Net.NetworkInformation.Ping

    Public Sub clientFreeIP(ByVal ip As IPAddress)
        m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
        m_Clients.Remove(ip)
        m_Hosts.Remove(ip)
        m_Leases.Remove(ip)
        m_PoolUsed.Remove(ip)
        m_PoolFree.Add(ip)
        m_Lock.ReleaseWriterLock()

        EventLog.WriteEntry("NetGate DHCP", String.Format("Client released ip {0}", ip.ToString))
    End Sub
    Public Function clientGetIP(ByVal hwAddr As String, ByVal hostname As String) As IPAddress
        Dim ip As IPAddress = Nothing

        m_Lock.AcquireReaderLock(LOCK_TIMEOUT)
        If m_Clients.ContainsValue(hwAddr) Then

            For Each d As KeyValuePair(Of IPAddress, String) In m_Clients
                If d.Value = hwAddr Then
                    ip = d.Key
                    Exit For
                End If
            Next
        End If
        m_Lock.ReleaseReaderLock()


        If ip IsNot Nothing Then
            ' We got IP from clients table
            While (m_Server.m_Config.pingCheck = True) AndAlso (m_Ping.Send(ip, m_Server.m_Config.pingTimeout).Status = NetworkInformation.IPStatus.Success)
                ' IP Address is currently used by other client/static assigment
#If DEBUG Then
                Console.WriteLine("IP Address {0} is currently in use.", ip.ToString)
#End If
                ' Mark as BAD
                m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
                m_Clients.Remove(ip)
                m_Hosts.Remove(ip)
                m_Leases.Remove(ip)
                m_PoolUsed.Remove(ip)
                m_PoolBad.Add(ip)
                m_Lock.ReleaseWriterLock()

                ' Get new IP
                ip = ipPoolGetIP()

                If ip Is Nothing Then Return ip

                ' Save it and set lease
                m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
                m_Clients.Add(ip, hwAddr)
                m_Hosts.Add(ip, hostname)
                m_Leases.Add(ip, Now.AddSeconds(m_Server.m_Config.leaseTimeDefault))
                m_Lock.ReleaseWriterLock()
            End While

            ' IP Address is currently not used by other client
#If DEBUG Then
            Console.WriteLine("IP Address {0} is currently free.", ip.ToString)
#End If

        Else
            ' Only already known clients?
            If m_Server.m_Config.denyUnKnownClients Then Return Nothing

            ' Get new IP
            ip = ipPoolGetIP()

            If ip Is Nothing Then Return ip

            ' Save it and set lease
            m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
            m_Clients.Add(ip, hwAddr)
            m_Hosts.Add(ip, hostname)
            m_Leases.Add(ip, Now.AddSeconds(m_Server.m_Config.leaseTimeDefault))
            m_Lock.ReleaseWriterLock()
        End If

        m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
        m_Leases(ip) = Now.AddSeconds(m_Server.m_Config.leaseTimeDefault)
        m_Lock.ReleaseWriterLock()

        EventLog.WriteEntry("NetGate DHCP", String.Format("Assing new IP address {0} for client {1}", ip.ToString, hwAddr))

        Return ip
    End Function

    Public Function ipPoolGetIP() As IPAddress
        Dim ip As IPAddress = Nothing

        m_Lock.AcquireWriterLock(LOCK_TIMEOUT)

        If m_PoolFree.Count > 0 Then
            ip = m_PoolFree(0)
            m_PoolFree.RemoveAt(0)
            m_PoolUsed.Add(ip)
        Else
            EventLog.WriteEntry("NetGate DHCP", "WARNING! IP Pool is emty.", EventLogEntryType.Warning)
        End If

        m_Lock.ReleaseWriterLock()

        Return ip
    End Function

    Public Sub ipPoolClean()
        Dim list As New List(Of IPAddress)

        m_Lock.AcquireWriterLock(LOCK_TIMEOUT)

        ' Do leases check
        For Each ip As KeyValuePair(Of IPAddress, Date) In m_Leases
            If ip.Value < Now.AddMinutes(5) Then list.Add(ip.Key)
        Next
        For Each ip As IPAddress In List
            m_PoolFree.Add(ip)
            m_PoolUsed.Remove(ip)
            m_Leases.Remove(ip)
            m_Clients.Remove(ip)
            m_Hosts.Remove(ip)

            EventLog.WriteEntry("NetGate DHCP", "Lease for IP " & ip.ToString & " is not renewed.")
            Console.WriteLine("Lease for IP " & ip.ToString & " is not renewed.")
        Next
        List.Clear()

        ' Do bad check
        For Each ip As IPAddress In m_PoolBad
            If m_Ping.Send(ip, m_Server.m_Config.pingTimeout).Status <> NetworkInformation.IPStatus.Success Then
                list.Add(ip)
            End If
        Next
        For Each ip As IPAddress In list
            m_PoolFree.Add(ip)
            m_PoolBad.Remove(ip)

            EventLog.WriteEntry("NetGate DHCP", "Bad IP " & ip.ToString & " is not marked as BAD anymore.")
            Console.WriteLine("Bad IP " & ip.ToString & " is not marked as BAD anymore.")
        Next
        List.Clear()


        m_Lock.ReleaseWriterLock()

        stateSave()
    End Sub
    Public Sub ipPoolInitialize()
#If DEBUG Then
        Console.WriteLine("ipPoolInitialize()")
#End If

        m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
        m_PoolFree.Clear()
        m_PoolBad.Clear()

        Dim b1 As Byte() = m_Server.m_Config.ipRangeStart.GetAddressBytes()
        Dim b2 As Byte() = m_Server.m_Config.ipRangeEnd.GetAddressBytes()

        While b1(0) <= b2(0)
            While b1(1) <= b2(1)
                While b1(2) <= b2(2)
                    While b1(3) <= b2(3)
                        Dim ip As IPAddress = New IPAddress(b1)
                        m_PoolFree.Add(ip)
                        b1(3) += 1
#If DEBUG Then
                        Console.WriteLine(vbTab & ip.ToString)
#End If
                    End While
                    b1(2) += 1
                End While
                b1(1) += 1
            End While
            b1(0) += 1
        End While

        For Each ip As IPAddress In m_PoolUsed
            m_PoolFree.Remove(ip)
        Next

        For Each i As dhcpOptions.HostClient In m_Server.m_Config.knownClients
            If Not m_Clients.ContainsKey(i.ipAddr) Then
                m_Clients.Add(i.ipAddr, i.hwAddr.ToLower)
                m_Leases.Add(i.ipAddr, Now.AddMonths(1))
                m_Hosts.Add(i.ipAddr, i.hostName)
                m_PoolUsed.Add(i.ipAddr)
            End If
            m_PoolFree.Remove(i.ipAddr)
        Next

        m_Lock.ReleaseWriterLock()

        EventLog.WriteEntry("NetGate DHCP", "IP pool initialized.")
    End Sub

    Public Sub stateSave()
        Dim f As New IO.FileStream("dhcpd.dat", FileMode.Create)
        Dim b As New IO.BinaryWriter(f)

        m_Lock.AcquireReaderLock(LOCK_TIMEOUT)

#If DEBUG Then
        If m_Clients.Count <> m_Leases.Count Or m_Clients.Count <> m_Hosts.Count Or m_Clients.Count <> m_PoolUsed.Count Then
            Console.WriteLine("Error in counts!")
        End If
#End If
        b.Write(m_Clients.Count)
        For Each c As KeyValuePair(Of IPAddress, String) In m_Clients
            b.Write(c.Key.ToString)
            b.Write(c.Value)
        Next
        b.Write(m_Leases.Count)
        For Each c As KeyValuePair(Of IPAddress, Date) In m_Leases
            b.Write(c.Key.ToString)
            b.Write(c.Value.ToBinary)
        Next
        b.Write(m_Hosts.Count)
        For Each c As KeyValuePair(Of IPAddress, String) In m_Hosts
            b.Write(c.Key.ToString)
            b.Write(c.Value)
        Next
        b.Write(m_PoolUsed.Count)
        For Each c As IPAddress In m_PoolUsed
            b.Write(c.ToString)
        Next

        m_Lock.ReleaseReaderLock()

        b.Flush()
        b.Close()
        f.Close()
    End Sub
    Public Sub stateLoad()
        If Dir("dhcpd.dat") <> "" Then
            Dim f As New IO.FileStream("dhcpd.dat", FileMode.Open)
            Dim b As New IO.BinaryReader(f)
            Dim c As Integer
            Dim i As Integer

            c = b.ReadInt32
            For i = 1 To c
                m_Clients.Add(IPAddress.Parse(b.ReadString), b.ReadString)
            Next
            c = b.ReadInt32()
            For i = 1 To c
                m_Leases.Add(IPAddress.Parse(b.ReadString), DateTime.FromBinary(b.ReadInt64))
            Next
            c = b.ReadInt32()
            For i = 1 To c
                m_Hosts.Add(IPAddress.Parse(b.ReadString), b.ReadString())
            Next
            c = b.ReadInt32()
            For i = 1 To c
                m_PoolUsed.Add(IPAddress.Parse(b.ReadString))
            Next

            f.Close()
        End If
    End Sub
End Module

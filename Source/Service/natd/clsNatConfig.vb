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


Imports System.Collections.Generic
Imports System.Net
Imports SharpPcap
Imports SharpPcap.Packets


Public Class natConfig

    Public extGatewayIP As IPAddress
    Public extGatewayHw As NetworkInformation.PhysicalAddress

    Public iGREEN As New InterfacePCap
    Public iRED As New InterfacePCap

    Public ipGreen As IPAddress
    Public ipRed As IPAddress

    Public hwGreen As NetworkInformation.PhysicalAddress
    Public hwRed As NetworkInformation.PhysicalAddress

    Public m_Table As New Dictionary(Of String, natConfigTable)

    Public Sub New()
        m_Table.Add(TABLE_MANGLE, New natConfigTable)
        m_Table.Add(TABLE_NAT, New natConfigTable)
        m_Table.Add(TABLE_FILTER, New natConfigTable)
    End Sub


    Private Function GetParam(ByVal s As String, ByVal p As String) As String
        s = s & " "
        p = p & " "

        If s.IndexOf(p) = -1 Then Return ""

        s = s.Substring(s.IndexOf(p) + p.Length)
        s = s.Remove(s.IndexOf(" "))
        Return s
    End Function
    Private Function IsParam(ByVal s As String, ByVal p As String) As Boolean
        s = s & " "
        p = p & " "
        Return s.Contains(p)
    End Function
    Private Function ParseInterface(ByVal s As String) As EthernetInterface
        If s.ToUpper = "GREEN" Then Return EthernetInterface.GREEN
        If s.ToUpper = "RED" Then Return EthernetInterface.RED
        Return EthernetInterface.NONE
    End Function
    Private Function ParseProtocol(ByVal s As String) As IPProtocol.IPProtocolType
        If s.ToUpper = "TCP" Then Return IPProtocol.IPProtocolType.TCP
        If s.ToUpper = "UDP" Then Return IPProtocol.IPProtocolType.UDP
        If s.ToUpper = "ICMP" Then Return IPProtocol.IPProtocolType.ICMP
        Return IPProtocol.IPProtocolType.NONE
    End Function


    Public Sub ReadConfig(ByVal s As String)
        s = ClearComments(s)
        Dim ss As String() = Split(s, vbNewLine)

        If iGREEN.hInterface IsNot Nothing Then iGREEN.Close()
        If iRED.hInterface IsNot Nothing Then iRED.Close()

        FirewallClearRules()

        For Each l As String In ss
            l = l.Trim

            If l.StartsWith("EXTERNAL_GATEWAY") Then
                extGatewayIP = IPAddress.Parse(l.Substring(l.IndexOf(" = ") + 3))
            ElseIf l.StartsWith("INTERFACE_RED") Then
                iRED.Open(l.Substring(l.IndexOf(" = ") + 3))
            ElseIf l.StartsWith("INTERFACE_GREEN") Then
                iGREEN.Open(l.Substring(l.IndexOf(" = ") + 3))

            ElseIf l.StartsWith("natd") Then
                Dim TABLE As String = GetParam(l, "-t").ToUpper

                If IsParam(l, "-F") OrElse IsParam(l, "--flush") Then
                    m_Table(TABLE).Flush()
                    Continue For
                End If

                Dim CHAIN As String
                If IsParam(l, "-a") Then CHAIN = GetParam(l, "-a").ToUpper()
                If IsParam(l, "-A") Then CHAIN = GetParam(l, "-A").ToUpper()

                Dim newChain As New natConfigChain

                If IsParam(l, "-i") Then newChain.sourceInterface = ParseInterface(GetParam(l, "-i"))
                If IsParam(l, "--in-interface") Then newChain.sourceInterface = ParseInterface(GetParam(l, "--in-interface"))

                If IsParam(l, "-o") Then newChain.destinationInterface = ParseInterface(GetParam(l, "-o"))
                If IsParam(l, "--out-interface") Then newChain.destinationInterface = ParseInterface(GetParam(l, "--out-interface"))

                If IsParam(l, "-j") Then newChain.JUMP = GetParam(l, "-j").ToUpper
                If IsParam(l, "--jump") Then newChain.JUMP = GetParam(l, "--jump").ToUpper

                If IsParam(l, "-p") Then newChain.packetProtocol = ParseProtocol(GetParam(l, "-p"))
                If IsParam(l, "--protocol") Then newChain.packetProtocol = ParseProtocol(GetParam(l, "--protocol"))

                If IsParam(l, "--sport") Then newChain.sourcePort = GetParam(l, "--sport")
                If IsParam(l, "--source-port") Then newChain.sourcePort = GetParam(l, "--source-port")

                If IsParam(l, "--dport ") Then newChain.destinationPort = GetParam(l, "--dport")
                If IsParam(l, "--destination-port ") Then newChain.destinationPort = GetParam(l, "--destination-port")

                If IsParam(l, "-s") Then newChain.sourceAddress = IPAddress.Parse(GetParam(l, "-s"))
                If IsParam(l, "--src ") Then newChain.sourceAddress = IPAddress.Parse(GetParam(l, "--src"))
                If IsParam(l, "--source") Then newChain.sourceAddress = IPAddress.Parse(GetParam(l, "--source"))

                If IsParam(l, "-d") Then newChain.destinationAddress = IPAddress.Parse(GetParam(l, "-d"))
                If IsParam(l, "--dst") Then newChain.destinationAddress = IPAddress.Parse(GetParam(l, "--dst"))
                If IsParam(l, "--destination") Then newChain.destinationAddress = IPAddress.Parse(GetParam(l, "--destination"))

                If IsParam(l, "--ttl-dec") Then newChain.packetTTLInc = -CType(GetParam(l, "--ttl-dec"), Integer)
                If IsParam(l, "--ttl-inc") Then newChain.packetTTLInc = GetParam(l, "--ttl-inc")
                If IsParam(l, "--ttl-set") Then newChain.packetTTLSet = GetParam(l, "--ttl-set")

                If IsParam(l, "--speed") Then newChain.packetLimit = GetParam(l, "--speed")

                If IsParam(l, "--quota") Then newChain.packetQuota = GetParam(l, "--quota")

                If CHAIN = "INPUT" Or CHAIN = "OUTPUT" Then
                    Dim drop As Boolean = False
                    Dim protocol As Integer = 0
                    Dim sourceAddress As IPAddress = IPAddress.Any
                    Dim destinationAddress As IPAddress = IPAddress.Any

                    If newChain.JUMP = "DROP" Or newChain.JUMP = "REJECT" Then drop = True
                    If newChain.packetProtocol <> -1 Then protocol = newChain.packetProtocol
                    If newChain.sourceAddress IsNot Nothing Then sourceAddress = newChain.sourceAddress
                    If newChain.destinationAddress IsNot Nothing Then destinationAddress = newChain.destinationAddress

                    Console.WriteLine("Added firewall rule for {0} -> {1}", sourceAddress.ToString, destinationAddress.ToString)

                    FirewallAddRule(drop, protocol, _
                                    sourceAddress, IPAddress.Broadcast, newChain.sourcePort, _
                                    destinationAddress, IPAddress.Broadcast, newChain.destinationPort)
                Else
                    m_Table(TABLE).Chain(CHAIN).Add(newChain)
                End If

            End If
        Next

        If iGREEN.hInterface.Addresses.Count > 0 Then
            ipGreen = iGREEN.ipAddress
            hwGreen = iGREEN.hwAddress
        End If

        If iRED.hInterface.Addresses.Count > 0 Then
            ipRed = iRED.ipAddress
            hwRed = iRED.hwAddress
        End If

        EventLog.WriteEntry("NetGate NAT", "Config read.")
    End Sub
    Private Function ClearComments(ByVal txt As String) As String
        ' Load confing in memory
        Dim f As IO.Stream = New IO.MemoryStream()
        Dim w As New IO.StreamWriter(f)
        w.Write(txt)
        w.Flush()
        f.Seek(0, IO.SeekOrigin.Begin)

        ' Get streams
        Dim r As New IO.StreamReader(f)
        Dim s As String

        txt = ""
        While Not r.EndOfStream
            s = r.ReadLine()
            If s.Contains("#") Then s = s.Remove(s.IndexOf("#"))
            s = s.Replace(vbTab, " ")
            s = s.Replace("  ", " ")
            If s.Trim <> String.Empty Then txt += s & vbNewLine
        End While

        txt = txt.Replace(vbTab, " ")
        txt = txt.Replace("  ", " ")

        Return txt
    End Function

End Class






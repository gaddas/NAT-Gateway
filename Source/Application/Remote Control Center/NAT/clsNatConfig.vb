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

Public Enum EthernetInterface
    NONE
    GREEN
    RED
End Enum
Public Enum Protocol
    NONE = 0
    TCP
    UDP
    ICMP
End Enum



Public Class natConfig

    Public Const TABLE_MANGLE As String = "MANGLE"
    Public Const TABLE_NAT As String = "NAT"
    Public Const TABLE_FILTER As String = "FILTER"

    Public iGREEN As String = ""
    Public iRED As String = ""
    Public extGatewayIP As IPAddress = IPAddress.Loopback

    Public optTTL As Boolean = False
    Public optNAT As Boolean = False

    Public m_Table As New Dictionary(Of String, natConfigTable)

    Public Sub New()
        m_Table.Add(TABLE_MANGLE, New natConfigTable)
        m_Table.Add(TABLE_NAT, New natConfigTable)
        m_Table.Add(TABLE_FILTER, New natConfigTable)
    End Sub


    Public Overrides Function ToString() As String
        Dim s As String = String.Empty

        s += "INTERFACE_GREEN = " & iGREEN & " " & vbNewLine
        s += "INTERFACE_RED = " & iRED & " " & vbNewLine
        s += "EXTERNAL_GATEWAY = " & extGatewayIP.ToString & " " & vbNewLine
        s += vbNewLine

        For Each k As KeyValuePair(Of String, natConfigTable) In m_Table
            For Each t As natConfigChain In k.Value.PREROUTING
                s += "natd -t " & k.Key & " -a PREROUTING " & t.ToString
                s += vbNewLine
            Next
            For Each t As natConfigChain In k.Value.INPUT
                s += "natd -t " & k.Key & " -a INPUT " & t.ToString
                s += vbNewLine
            Next
            For Each t As natConfigChain In k.Value.FORWARD
                s += "natd -t " & k.Key & " -a FORWARD " & t.ToString
                s += vbNewLine
            Next
            For Each t As natConfigChain In k.Value.OUTPUT
                s += "natd -t " & k.Key & " -a OUTPUT " & t.ToString
                s += vbNewLine
            Next
            For Each t As natConfigChain In k.Value.POSTROUTING
                s += "natd -t " & k.Key & " -a POSTROUTING " & t.ToString
                s += vbNewLine
            Next
        Next

        s += vbNewLine
        Return s
    End Function

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
    Public Shared Function ParseProtocol(ByVal s As String) As Protocol
        If s.ToUpper = "TCP" Then Return Protocol.TCP
        If s.ToUpper = "UDP" Then Return Protocol.UDP
        If s.ToUpper = "ICMP" Then Return Protocol.ICMP
        Return Protocol.NONE
    End Function


    Public Sub ReadConfig(ByVal s As String)
        s = ClearComments(s)
        Dim ss As String() = Split(s, vbNewLine)

        For Each l As String In ss
            l = l.Trim

            If l.ToUpper.Contains("natd -t nat -A PREROUTING -i RED -o GREEN -j DNAT".ToUpper) Then
                optNAT = True
            End If
            If l.ToUpper.Contains("natd -t nat -A POSTROUTING -i GREEN -o RED -j SNAT".ToUpper) Then
                optNAT = True
            End If
            If l.ToUpper.Contains("natd -t mangle -A FORWARD -j TTL --ttl-dec 1".ToUpper) Then
                optTTL = True
            End If

            If l.StartsWith("EXTERNAL_GATEWAY") Then
                extGatewayIP = IPAddress.Parse(l.Substring(l.IndexOf(" = ") + 3))
            ElseIf l.StartsWith("INTERFACE_RED") Then
                iRED = (l.Substring(l.IndexOf(" = ") + 3))
            ElseIf l.StartsWith("INTERFACE_GREEN") Then
                iGREEN = (l.Substring(l.IndexOf(" = ") + 3))

            ElseIf l.StartsWith("natd") Then
                Dim TABLE As String = GetParam(l, "-t").ToUpper

                If IsParam(l, "-F") OrElse IsParam(l, "--flush") Then
                    m_Table(TABLE).Flush()
                    Continue For
                End If

                Dim CHAIN As String
                If IsParam(l, "-A") Then CHAIN = GetParam(l, "-A").ToUpper()
                If IsParam(l, "-a") Then CHAIN = GetParam(l, "-a").ToUpper()
                Dim newChain As New natConfigChain

                If IsParam(l, "-i") Then newChain.sourceInterface = GetParam(l, "-i")
                If IsParam(l, "--in-interface") Then newChain.sourceInterface = GetParam(l, "--in-interface")

                If IsParam(l, "-o") Then newChain.destinationInterface = GetParam(l, "-o")
                If IsParam(l, "--out-interface") Then newChain.destinationInterface = GetParam(l, "--out-interface")

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

                m_Table(TABLE).Chain(CHAIN).Add(newChain)
            End If
        Next

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
Public Class natConfigTable
    Public Function Chain(ByVal s As String) As List(Of natConfigChain)
        Select Case s.ToUpper
            Case "PREROUTING"
                Return PREROUTING
            Case "INPUT"
                Return INPUT
            Case "FORWARD"
                Return FORWARD
            Case "OUTPUT"
                Return OUTPUT
            Case "POSTROUTING"
                Return POSTROUTING
        End Select

        Return Nothing
    End Function

    Public Sub Flush()
        INPUT.Clear()
        PREROUTING.Clear()
        FORWARD.Clear()
        OUTPUT.Clear()
        POSTROUTING.Clear()
    End Sub

    Public PREROUTING As New List(Of natConfigChain)
    Public INPUT As New List(Of natConfigChain)
    Public FORWARD As New List(Of natConfigChain)
    Public OUTPUT As New List(Of natConfigChain)
    Public POSTROUTING As New List(Of natConfigChain)
End Class
Public Class natConfigChain

    Public sourceAddress As IPAddress = Nothing
    Public sourcePort As UShort = 0
    Public sourceInterface As String = String.Empty

    Public destinationAddress As IPAddress = Nothing
    Public destinationPort As UShort = 0
    Public destinationInterface As String = String.Empty

    Public packetQuota As ULong = 0
    Public packetProtocol As Protocol = Protocol.NONE

    Public packetTTLInc As Integer = 0
    Public packetTTLSet As Integer = 0
    Public packetLimit As Integer = 0

    Public JUMP As String

    Public Overrides Function ToString() As String
        Dim s As String = String.Empty

        If sourceInterface <> String.Empty Then s += "-i " & sourceInterface & " "
        If destinationInterface <> String.Empty Then s += "-o " & destinationInterface & " "

        If sourceAddress IsNot Nothing Then s += "-s " & sourceAddress.ToString & " "
        If destinationAddress IsNot Nothing Then s += "-d " & destinationAddress.ToString & " "

        If sourcePort > 0 Then s += "--sport " & sourcePort.ToString & " "
        If destinationPort > 0 Then s += "--dport " & destinationPort.ToString & " "

        If packetProtocol <> Protocol.NONE Then s += "-p " & packetProtocol.ToString & " "

        s += "-j " & JUMP & " "
        If packetQuota > 0 Then s += "--quota " & packetQuota.ToString & " "
        If packetLimit > 0 Then s += "--speed " & packetLimit.ToString & " "

        If packetTTLInc > 0 Then s += "--ttl-inc" & packetTTLInc & " "
        If packetTTLInc < 0 Then s += "--ttl-dec" & packetTTLInc & " "
        If packetTTLSet <> 0 Then s += "--ttl-set" & packetTTLSet & " "

        Return s
    End Function

End Class






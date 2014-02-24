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


Public Class dhcpOptions

    Public interfaceAddr As IPAddress = IPAddress.Parse("0.0.0.0")
    Public ipRangeStart As IPAddress = IPAddress.Parse("192.168.0.100")
    Public ipRangeEnd As IPAddress = IPAddress.Parse("192.168.0.200")
    Public leaseTimeDefault As Integer = 10800
    Public leaseTimeMax As Integer = 86400
    Public bootFileName As String = ""

    Public denyUnKnownClients As Boolean = False
    Public pingCheck As Boolean = False
    Public pingTimeout As Integer = 3

    Public options As New Dictionary(Of String, Object)
    Public knownClients As New List(Of HostClient)

    Public Class HostClient
        Public hwAddr As String
        Public ipAddr As IPAddress
        Public hostName As String
    End Class

    Public Overrides Function ToString() As String
        Dim s As String = ""

        s += String.Format("interface {0};", interfaceAddr.ToString) & vbNewLine
        s += String.Format("range {0} {1};", ipRangeStart.ToString, ipRangeEnd.ToString) & vbNewLine
        s += String.Format("default-lease-time {0};", leaseTimeDefault) & vbNewLine
        s += String.Format("max-lease-time {0};", leaseTimeMax) & vbNewLine
        s += String.Format("filename ""{0}"";", bootFileName) & vbNewLine
        s += vbNewLine
        If options.ContainsKey("server-name") Then s += String.Format("option server-name ""{0}"";", options("server-name")) & vbNewLine
        If options.ContainsKey("server-identifier") Then s += String.Format("option server-identifier {0};", options("server-identifier").ToString) & vbNewLine
        s += vbNewLine
        If options.ContainsKey("routers") Then
            s += String.Format("option routers")
            For Each ip As IPAddress In options("routers")
                s += String.Format(" {0}", ip.ToString)
            Next
            s += ";" & vbNewLine
        End If
        If options.ContainsKey("domain-name-servers") Then
            s += String.Format("option domain-name-servers")
            For Each ip As IPAddress In options("domain-name-servers")
                s += String.Format(" {0}", ip.ToString)
            Next
            s += ";" & vbNewLine
        End If
        If options.ContainsKey("ntp-servers") Then
            s += String.Format("option ntp-servers")
            For Each ip As IPAddress In options("ntp-servers")
                s += String.Format(" {0}", ip.ToString)
            Next
            s += ";" & vbNewLine
        End If
        If options.ContainsKey("time-servers") Then
            s += String.Format("option time-servers")
            For Each ip As IPAddress In options("time-servers")
                s += String.Format(" {0}", ip.ToString)
            Next
            s += ";" & vbNewLine
        End If
        If options.ContainsKey("netbios-name-servers") Then
            s += String.Format("option netbios-name-servers")
            For Each ip As IPAddress In options("netbios-name-servers")
                s += String.Format(" {0}", ip.ToString)
            Next
            s += ";" & vbNewLine
        End If

        If options.ContainsKey("subnet-mask") Then s += String.Format("option subnet-mask {0};", options("subnet-mask").ToString) & vbNewLine
        If options.ContainsKey("broadcast-address") Then s += String.Format("option broadcast-address {0};", options("broadcast-address").ToString) & vbNewLine

        If options.ContainsKey("domain-name") Then s += String.Format("option domain-name ""{0}"";", options("domain-name")) & vbNewLine
        If options.ContainsKey("ip-forwarding") Then s += String.Format("option ip-forwarding;") & vbNewLine

        If options.ContainsKey("boot-file-size") Then s += String.Format("option boot-file-size {0};", options("boot-file-size")) & vbNewLine
        If options.ContainsKey("default-udp-ttl") Then s += String.Format("option default-udp-ttl {0};", options("default-udp-ttl")) & vbNewLine
        If options.ContainsKey("default-tcp-tt") Then s += String.Format("option default-tcp-ttl {0};", options("default-tcp-tt")) & vbNewLine
        If options.ContainsKey("default-mtu") Then s += String.Format("option default-mtu {0};", options("default-mtu")) & vbNewLine
        If options.ContainsKey("arp-cache-timeout") Then s += String.Format("option arp-cache-timeout {0};", options("arp-cache-timeout")) & vbNewLine

        s += vbNewLine
        If pingCheck Then s += String.Format("ping-check;") & vbNewLine
        If pingCheck Then s += String.Format("ping-timeout {0};", pingTimeout) & vbNewLine
        s += vbNewLine
        If denyUnKnownClients Then s += String.Format("deny-unknown-clients;") & vbNewLine
        s += vbNewLine

        For Each h As HostClient In knownClients
            s += "host " & h.hostName & " {" & vbNewLine
            s += String.Format("{0}hardware-address {1};", vbTab, h.hwAddr) & vbNewLine
            s += String.Format("{0}fixed-address {1};", vbTab, h.ipAddr) & vbNewLine
            s += "}" & vbNewLine
        Next

        Return s
    End Function

    Public Sub ReadOption(ByVal s As String)
        Dim o As String() = Split(s, " ", 2)
        o(0) = o(0).ToLower

        Select Case o(0)
            Case "server-name", "domain-name"
                options.Add(o(0), CType(o(1).Replace("""", "").Trim, String))
            Case "routers", "domain-name-servers", "ntp-servers", "time-servers", "netbios-name-servers"
                Dim tmpText As String() = Split(o(1), " ")
                Dim tmpInfo As New List(Of IPAddress)

                For Each tmpStr As String In tmpText
                    If tmpStr.Trim = "" Then Continue For
                    If IPAddress.TryParse(tmpStr, Nothing) Then
                        tmpInfo.Add(IPAddress.Parse(tmpStr))
                    Else
                        Try
                            For Each ip As IPAddress In Dns.GetHostAddresses(tmpStr)
                                tmpInfo.Add(ip)
                            Next
                        Catch ex As Sockets.SocketException
                            EventLog.WriteEntry("NetGate DHCP", "Can't resolve address of " & tmpStr)
                        End Try

                    End If
                Next
                options.Add(o(0), tmpInfo)
            Case "broadcast-address", "subnet-mask", "server-identifier"
                options.Add(o(0), IPAddress.Parse(o(1)))
            Case "ip-forwarding"
                options.Add(o(0), True)
            Case "boot-file-size", "default-udp-ttl", "default-tcp-ttl", "default-mtu", "arp-cache-timeout"
                options.Add(o(0), CType(o(1), Integer))
        End Select
    End Sub
    Public Sub ReadConfig(ByVal s As String)
        s = ClearComments(s)

        ' Clear hosts from options
        If s.IndexOf("host ") >= 0 Then
            knownClients.Clear()

            ' Read hosts first
            Dim hosts As String() = Split(s.Substring(s.IndexOf("host ")), "host ")
            For Each host As String In hosts
                ' Empty entry
                If host.Trim = "" Then Continue For

                Dim h As New HostClient
                h.hostName = host.Remove(host.IndexOf(" ")).Trim

                host = host.Substring(host.IndexOf("{") + 1)
                host = host.Remove(host.LastIndexOf("}"))

                Dim clients As String() = Split(host, ";")
                ' Get client details
                For Each client As String In clients
                    client = client.Trim
                    If client.Contains("hardware-address") Then
                        h.hwAddr = client.Substring(client.IndexOf(" ")).ToUpper.Trim
                    ElseIf client.Contains("fixed-address") Then
                        h.ipAddr = IPAddress.Parse(client.Substring(client.IndexOf(" ")).Trim)
                    End If
                Next
                knownClients.Add(h)
            Next

            s = s.Remove(s.IndexOf("host "))
        End If

        ' Read options
        Dim optionsText As String() = s.Split(";")
        Dim o As String()

        For Each optionString As String In optionsText
            optionString = optionString.Trim
            If optionString = "" Then Continue For

            o = Split(optionString, " ", 2)
            o(0) = o(0).ToLower

            If o(0) = "interface" Then
                interfaceAddr = IPAddress.Parse(o(1))
            ElseIf o(0) = "range" Then
                ipRangeStart = IPAddress.Parse(o(1).Remove(o(1).IndexOf(" ")).Trim)
                ipRangeEnd = IPAddress.Parse(o(1).Substring(o(1).IndexOf(" ")).Trim)
            ElseIf o(0) = "default-lease-time" Then
                leaseTimeDefault = o(1)
            ElseIf o(0) = "max-lease-time" Then
                leaseTimeMax = o(1)
            ElseIf o(0) = "filename" Then
                bootFileName = o(1).Replace("""", "")
            ElseIf o(0) = "deny-unknown-clients" Then
                denyUnKnownClients = True
            ElseIf o(0) = "ping-check" Then
                pingCheck = True
            ElseIf o(0) = "ping-timeout" Then
                pingTimeout = o(1)
            ElseIf o(0) = "option" Then
                ReadOption(o(1))
            Else
                EventLog.WriteEntry("NetGate DHCP", "Unkwnown token: " & o(0))
            End If
        Next

        EventLog.WriteEntry("NetGate DHCP", "Config read.")
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
            txt += s
        End While

        txt = txt.Replace(vbTab, " ")
        txt = txt.Replace(vbNewLine, " ")
        txt = txt.Replace(vbCr, " ")
        txt = txt.Replace(vbLf, " ")
        txt = txt.Replace("  ", " ")

        Return txt
    End Function
End Class


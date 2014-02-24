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


Public Class dnsConfig

    Public m_Servers As New List(Of IPAddress)
    Public m_UseDefault As Boolean = True
    Public m_UseCache As Boolean = True
    Public m_CacheSize As Integer = 10000
    Public m_Hosts As New Dictionary(Of String, IPAddress)

    Public Overrides Function ToString() As String
        Dim s As String = String.Empty

        s += vbNewLine
        If Not m_UseDefault Then
            For Each ip As IPAddress In m_Servers
                s += "nameserver " & ip.ToString & ";" & vbNewLine
            Next
        End If
        s += vbNewLine
        If m_UseCache Then
            s += "cache on;" & vbNewLine
            s += "cache-size " & m_CacheSize & ";" & vbNewLine
        Else
            s += "cache off;" & vbNewLine
        End If
        s += vbNewLine
        For Each e As KeyValuePair(Of String, IPAddress) In m_Hosts
            s += "record " & e.Value.ToString & " " & e.Key & ";" & vbNewLine
        Next
        s += vbNewLine
        Return s
    End Function


    Public Sub ReadConfig(ByVal s As String)
        s = ClearComments(s)
        Dim ss As String() = Split(s, ";")

        m_Servers.Clear()
        m_Hosts.Clear()
        m_UseCache = False
        m_CacheSize = 10000
        m_UseDefault = False

        For Each l As String In ss
            l = l.Trim

            If l.StartsWith("nameserver") Then
                m_Servers.Add(IPAddress.Parse(l.Substring(l.IndexOf(" ") + 1)))
            ElseIf l.StartsWith("cache on") Then
                m_UseCache = True
            ElseIf l.StartsWith("cache off") Then
                m_UseCache = False
            ElseIf l.StartsWith("cache-size") Then
                m_CacheSize = l.Substring(l.IndexOf(" "))
            ElseIf l.StartsWith("record") Then
                Dim tmp As String() = l.Split(" ")
                m_Hosts.Add(tmp(2), IPAddress.Parse(tmp(1)))
            End If
        Next

        If m_Servers.Count = 0 Then
            m_UseDefault = True
            ' Get system default DNS
            For Each i As NetworkInformation.NetworkInterface In NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                If i.OperationalStatus = NetworkInformation.OperationalStatus.Up Then
                    Dim ips As NetworkInformation.IPAddressCollection = i.GetIPProperties().DnsAddresses
                    For Each ip As IPAddress In ips
                        m_Servers.Add(ip)
                    Next
                End If
            Next
        End If

        EventLog.WriteEntry("NetGate DNS", "Config read.")
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


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


Public Module Parser

 

    Public Server As Byte = 0

    Public Sub OnPacket(ByVal p As Packet)
        Dim r As Packet = Nothing

        Try
            Console.WriteLine(p.ToString())

            ' TODO: Packet Handling code here
            Dim dp As New dnsPacket(p)
            Dim question As String = dp.GetQuestionDomain

            m_Server.m_dnsLock.AcquireWriterLock(LOCK_TIMEOUT)

            ' Handle cached request
            If m_Server.m_Config.m_UseCache AndAlso m_Server.m_dnsCache.ContainsKey(question) Then
                r = m_Server.m_dnsCache(question)
                r.IP = dp.IP
            Else
                ' Check for hosts entry
                If m_Server.m_Config.m_Hosts.ContainsKey(question) Then
                    Dim tmp As New dnsPacket
                    tmp.IsResponse = True
                    tmp.MessageID = dp.MessageID
                    tmp.AddQuestion(question, dnsType.TYPE_ANAME, dnsClass.CLASS_IN)
                    tmp.AddAnswer(question, dnsType.TYPE_ANAME, dnsClass.CLASS_IN, m_Server.m_Config.m_Hosts(question))
                    r = tmp.Build()
                    r.IP = p.IP
                Else

                    ' Is it response?
                    If dp.IsResponse AndAlso m_Server.m_dnsQuery.ContainsKey(question) Then
                        r = New Packet
                        r.Data = p.Data
                        r.Length = p.Length
                        r.IP = m_Server.m_dnsQuery(question)
                        m_Server.m_dnsQuery.Remove(question)
                    Else
                        ' Forward request
                        r = New Packet
                        r.Data = p.Data
                        r.Length = p.Length

                        If Not m_Server.m_dnsQuery.ContainsKey(question) Then
                            m_Server.m_dnsQuery.Add(question, p.IP)
                            r.IP = CType(New IPEndPoint(m_Server.m_Config.m_Servers.Item(0), 53), EndPoint)
                        Else
                            Server += 1
                            If Server >= m_Server.m_Config.m_Servers.Count Then Server = 0
                            r.IP = CType(New IPEndPoint(m_Server.m_Config.m_Servers.Item(Server), 53), EndPoint)
                        End If


                    End If
                End If
            End If





#If DEBUG Then
            Console.WriteLine("recv packet:")
            Console.WriteLine(p.ToString)
#End If
        Catch ex As Exception
            EventLog.WriteEntry("NetGate DNS", ex.ToString, EventLogEntryType.Error)
#If DEBUG Then
            Console.WriteLine(ex.ToString)
#End If
        Finally
            m_Server.m_dnsLock.ReleaseWriterLock()
        End Try

        If r IsNot Nothing Then
#If DEBUG Then
            Console.WriteLine("sent packet:")
            Console.WriteLine(r.ToString)
#End If
            m_Server.AsyncBeginSend(r)
        End If

    End Sub



   


End Module
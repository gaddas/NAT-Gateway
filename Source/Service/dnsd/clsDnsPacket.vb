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


Imports System.Net


Public Class dnsPacket
    Inherits Packets.Packet

    Private m_Opcode As Short = 0
    Private m_ID As UShort = 0
    Private m_IsResponse As Boolean = False

    Public Class Question
        Public Domain As String
        Public Type As dnsType
        Public Classe As dnsClass
    End Class
    Public m_Questions As New List(Of Question)

    Public Class Answer
        Public Domain As String
        Public Type As dnsType
        Public Classe As dnsClass
        Public IP As Net.IPAddress
    End Class
    Public m_Answers As New List(Of Answer)

    ' Create new packet for sending
    Public Sub New()
        MyBase.New()
    End Sub

    ' Read packet from network
    Public Sub New(ByVal p As Packet)
        Me.Data = p.Data
        Me.IP = p.IP
        Me.Length = p.Length
        Me.Offset = 0
    End Sub

    ' Read requested domain name
    Public Function GetQuestionDomain() As String
        Me.Offset = 0

        ' Message ID
        Me.GetUInt16()
        ' Message Opcode/Flags
        Me.GetUInt16()
        ' QDCOUNT
        If Me.GetUInt16() <> 1 Then
            Throw New NotSupportedException("DNS Query with more than one request is not supported.")
        End If
        ' ANCOUNT
        Me.GetUInt16()
        ' NSCOUNT
        Me.GetUInt16()
        ' ARCOUNT
        Me.GetUInt16()

        Dim ss As String = Me.GetString
        Dim s As String = Me.GetString
        While s <> String.Empty
            ss += "." & s
            s = Me.GetString
        End While

        Return ss
    End Function
    Public Sub AddQuestion(ByVal dnsDomain As String, ByVal dnsType As dnsType, ByVal dnsClass As dnsClass)
        Dim q As New Question
        q.Domain = dnsDomain
        q.Type = dnsType
        q.Classe = dnsClass

        m_Questions.Add(q)
    End Sub

    Public Sub AddAnswer(ByVal dnsDomain As String, ByVal dnsType As dnsType, ByVal dnsClass As dnsClass, ByVal dnsIp As IpAddress)
        Dim a As New Answer
        a.Domain = dnsDomain
        a.Type = dnsType
        a.Classe = dnsClass
        a.IP = dnsIp

        m_Answers.Add(a)
    End Sub

    ' Read packet type
    Public Property Opcode() As Short
        Get
            Return m_Opcode
        End Get
        Set(ByVal value As Short)
            m_Opcode = value
        End Set
    End Property
    Public Property MessageID() As UShort
        Get
            Me.Offset = 0
            Return Me.GetUInt16()
        End Get
        Set(ByVal value As UShort)
            m_ID = value
        End Set
    End Property

    Public Property IsResponse() As Boolean
        Get
            Me.Offset = 0
            Me.GetUInt16()
            Dim flag As Byte = Me.GetByte
            Return (flag And &H80) = &H80
        End Get
        Set(ByVal value As Boolean)
            m_IsResponse = value
        End Set
    End Property

    Public Function Build() As Packet

        ' Overflow of Message ID Counter
        If m_RequestIdCounter > Short.MaxValue Then
            System.Threading.Interlocked.Exchange(m_RequestIdCounter, 0)
        End If

        ' Message ID
        If m_ID <> 0 Then
            Me.AddUInt16(m_ID)
        Else
            Me.AddUInt16(System.Threading.Interlocked.Increment(m_RequestIdCounter))
        End If


        ' Message Status
        '  (1 bit) Type                 - Query (0) or Response (1)
        '  (4 bit) Opcode               - A four bit field that specifies kind of query in this message.
        '  (1 bit) Authoritative Answer - this bit is valid in responses, and specifies that the responding name server is an authority for the domain name in question section.
        '  (1 bit) TrunCation           - specifies that this message was truncated due to length greater than that permitted on the transmission(channel)
        '  (1 bit) Recursion Desired
        '  (1 bit) Recursion Available
        '  (3 bit)                      - reserved
        '  (4 bit) Response code
        If m_IsResponse Then
            Me.AddUInt16(m_Opcode + &H8000)
        Else
            Me.AddUInt16(m_Opcode)
        End If


        ' (uint16) QDCOUNT an unsigned 16 bit integer specifying the number of entries in the question section.
        Me.AddUInt16(m_Questions.Count)
        ' (uint16) ANCOUNT an unsigned 16 bit integer specifying the number of resource records in the answer section.
        Me.AddUInt16(m_Answers.Count)
        ' (uint16) NSCOUNT an unsigned 16 bit integer specifying the number of name server resource records in the authority records section.
        Me.AddUInt16(0)
        ' (uint16) ARCOUNT an unsigned 16 bit integer specifying the number of resource records in the additional records section.
        Me.AddUInt16(0)

        ' Questions
        For Each q As Question In m_Questions
            Dim ss As String() = q.Domain.Split(".")

            ' QNAME a domain name represented as a sequence of labels, where each label consists of a length octet followed by that number of octets.
            For Each s As String In ss
                Me.AddString(s)
            Next
            ' The domain name terminates with the zero length octet for the null label of the root.
            Me.AddByte(0)

            ' (uint16) QTYPE a two octet code which specifies the type of the query.
            Me.AddByte(0)
            Me.AddByte(q.Type)
            ' (uint16) QCLASS a two octet code that specifies the class of the query.
            Me.AddByte(0)
            Me.AddByte(q.Classe)
        Next

        ' Answers
        For Each a As Answer In m_Answers
            Dim ss As String() = a.Domain.Split(".")

            '' ANAME a domain name represented as a sequence of labels, where each label consists of a length octet followed by that number of octets.
            'For Each s As String In ss
            '    Me.AddString(s)
            'Next
            '' The domain name terminates with the zero length octet for the null label of the root.
            'Me.AddByte(0)
            Me.AddByte(&HC0)
            Me.AddByte(&HC)


            ' (uint16) ATYPE a two octet code which specifies the type of the query.
            Me.AddByte(0)
            Me.AddByte(a.Type)
            ' (uint16) ACLASS a two octet code that specifies the class of the query.
            Me.AddByte(0)
            Me.AddByte(a.Classe)

            ' (uint32) TTL
            Me.AddUInt32(24 * 60 * 60)

            Select Case a.Type
                Case dnsType.TYPE_MX
                    ' not relevant
                    Me.AddUInt16(&HA)
                    Me.AddUInt16(&HA)
                Case dnsType.TYPE_ANAME
                    ' ip
                    Me.AddUInt16(4)
                    Me.AddIPAddress(a.IP)
            End Select

        Next


        Return Me
    End Function

End Class

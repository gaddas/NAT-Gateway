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
Imports System.Net.NetworkInformation
Imports SharpPcap
Imports SharpPcap.Packets


Public Module modCommon

    Public Const LOCK_TIMEOUT As Integer = 9000
    Public Const TTL_CONNTRACK As Integer = 300
    Public Const LIMITER_BUFFER_SIZE As Integer = 100

    Public Const TABLE_MANGLE As String = "MANGLE"
    Public Const TABLE_NAT As String = "NAT"
    Public Const TABLE_FILTER As String = "FILTER"

    Public m_Server As natServer
    Public m_RemoteChannel As Channels.IChannel = Nothing
    Public m_RemoteFactory As remoteFactoryNAT = Nothing
    Public m_RemoteFactoryObj As ObjRef = Nothing


    Public Sub DumpPacket(ByVal p As SharpPcap.Packets.Packet)
        Dim j As Integer
        Dim buffer As New System.Text.StringBuilder

        Dim bytes() As Byte = p.Bytes
        ReDim Preserve bytes(p.Bytes.Length - 1)

        Console.WriteLine(p.ToString)
        Console.WriteLine("pLen: {0} bLen: {1} dLen: {2} Header: {3}", p.PcapHeader.PacketLength, p.Bytes.Length, p.Data.Length, p.Header.Length)

        'Build string
        If bytes.Length Mod 16 = 0 Then
            For j = 0 To bytes.Length - 1 Step 16
                buffer.Append("|  ")
                buffer.Append(BitConverter.ToString(bytes, j, 16).Replace("-", " "))
                buffer.Append(" |  ")
                buffer.Append(System.Text.Encoding.ASCII.GetString(bytes, j, 16).Replace(vbTab, "?").Replace(vbBack, "?").Replace(vbCr, "?").Replace(vbFormFeed, "?").Replace(vbLf, "?"))
                buffer.Append(" |")
                buffer.Append(vbNewLine)
            Next
        Else
            For j = 0 To bytes.Length - 1 - 16 Step 16
                buffer.Append("|  ")
                buffer.Append(BitConverter.ToString(bytes, j, 16).Replace("-", " "))
                buffer.Append(" |  ")
                buffer.Append(System.Text.Encoding.ASCII.GetString(bytes, j, 16).Replace(vbTab, "?").Replace(vbBack, "?").Replace(vbCr, "?").Replace(vbFormFeed, "?").Replace(vbLf, "?"))
                buffer.Append(" |")
                buffer.Append(vbNewLine)
            Next

            buffer.Append("|  ")
            buffer.Append(BitConverter.ToString(bytes, j, bytes.Length Mod 16).Replace("-", " "))
            buffer.Append(New String(" ", (16 - bytes.Length Mod 16) * 3))
            buffer.Append(" |  ")
            buffer.Append(System.Text.Encoding.ASCII.GetString(bytes, j, bytes.Length Mod 16).Replace(vbTab, "?").Replace(vbBack, "?").Replace(vbCr, "?").Replace(vbFormFeed, "?").Replace(vbLf, "?"))
            buffer.Append(New String(" ", 16 - bytes.Length Mod 16))
            buffer.Append(" |")
            buffer.Append(vbNewLine)
        End If

        Console.WriteLine(buffer.ToString)
    End Sub



    Public Sub ChainPREROUTING(ByVal p As IPPacket, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface)
        ' Следене на връзките
        If (Not p.DestinationAddress.Equals(m_Server.m_Config.ipGreen) AndAlso Not p.SourceAddress.Equals(m_Server.m_Config.ipRed) AndAlso _
            Not p.DestinationAddress.Equals(m_Server.m_Config.ipRed) AndAlso Not p.SourceAddress.Equals(m_Server.m_Config.ipGreen)) Then

            If TypeOf (p) Is TCPPacket AndAlso CType(p, TCPPacket).Syn Then
                m_Server.ConntrackPut("TCP", p.DestinationAddress, CType(p, TCPPacket).DestinationPort, p.SourceAddress, CType(p, TCPPacket).SourcePort)
            ElseIf TypeOf (p) Is UDPPacket Then
                m_Server.ConntrackPut("UDP", p.DestinationAddress, CType(p, UDPPacket).DestinationPort, p.SourceAddress, CType(p, UDPPacket).SourcePort)
            End If
        End If


        ' Адресна транслация
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_NAT).PREROUTING
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_MANGLE).PREROUTING
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next
    End Sub


    Public Sub ChainINPUT(ByVal p As IPPacket, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface)
        'For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_MANGLE).INPUT
        '    If chain.Action(p, FromInterface, ToInterface) Then Return
        'Next
        'For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_FILTER).INPUT
        '    If chain.Action(p, FromInterface, ToInterface) Then Return
        'Next
    End Sub
    Public Sub ChainOUTPUT(ByVal p As IPPacket, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface)
        'For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_FILTER).OUTPUT
        '    If chain.Action(p, FromInterface, ToInterface) Then Return
        'Next
        'For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_MANGLE).OUTPUT
        '    If chain.Action(p, FromInterface, ToInterface) Then Return
        'Next
    End Sub
    Public Sub ChainFORWARD(ByVal p As IPPacket, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface)
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_MANGLE).FORWARD
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_FILTER).FORWARD
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_NAT).FORWARD
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next

        ChainPOSTROUTING(p, FromInterface, ToInterface)
    End Sub
    Public Sub ChainPOSTROUTING(ByVal p As IPPacket, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface)
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_NAT).POSTROUTING
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next
        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_MANGLE).POSTROUTING
            If chain.Action(p, FromInterface, ToInterface) Then Return
        Next

        ' Send packet
        If (FromInterface = EthernetInterface.GREEN And ToInterface = EthernetInterface.RED) Then
            m_Server.m_Config.iRED.Write(p)

            If TypeOf p Is TCPPacket Then
                If CType(p, TCPPacket).Fin And CType(p, TCPPacket).Ack Then m_Server.ConntrackRemove("TCP", p.DestinationAddress, CType(p, TCPPacket).DestinationPort, CType(p, TCPPacket).SourcePort)
                If CType(p, TCPPacket).Rst And CType(p, TCPPacket).Ack Then m_Server.ConntrackRemove("TCP", p.DestinationAddress, CType(p, TCPPacket).DestinationPort, CType(p, TCPPacket).SourcePort)
            End If
        End If
        If (FromInterface = EthernetInterface.RED And ToInterface = EthernetInterface.GREEN) Then
            m_Server.m_Config.iGREEN.Write(p)

            If TypeOf p Is TCPPacket Then
                If CType(p, TCPPacket).Fin And CType(p, TCPPacket).Ack Then m_Server.ConntrackRemove("TCP", p.SourceAddress, CType(p, TCPPacket).SourcePort, CType(p, TCPPacket).DestinationPort)
                If CType(p, TCPPacket).Rst And CType(p, TCPPacket).Ack Then m_Server.ConntrackRemove("TCP", p.SourceAddress, CType(p, TCPPacket).SourcePort, CType(p, TCPPacket).DestinationPort)
            End If
        End If

    End Sub

End Module

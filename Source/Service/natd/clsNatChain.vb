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


Public Enum Protocol
    TCP
    UDP
    ICMP
End Enum

Public Enum EthernetInterface
    NONE
    GREEN
    RED
End Enum

Public Class QueuedPacket
    Public FromInterface As EthernetInterface
    Public ToInterface As EthernetInterface
    Public Packet As Packet
End Class

Public Class natConfigChain

    Public sourceAddress As IPAddress = Nothing
    Public sourcePort As UShort = 0
    Public sourceInterface As EthernetInterface = EthernetInterface.NONE

    Public destinationAddress As IPAddress = Nothing
    Public destinationPort As UShort = 0
    Public destinationInterface As EthernetInterface = EthernetInterface.NONE

    Public packetQuota As ULong = 0
    Public packetProtocol As IPProtocol.IPProtocolType = IPProtocol.IPProtocolType.INVALID

    Public packetTTLInc As Integer = 0
    Public packetTTLSet As Integer = 0
    Public packetLimit As Integer = 0

    Public limiterLock As New Threading.ReaderWriterLock
    Public limiterBuffer As New List(Of QueuedPacket)
    Public limiterTraffic As Integer = 0

    Public JUMP As String

    Public Function Action(ByVal ip As IPPacket, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface) As Boolean
        ' Every packet match empty chain
        Dim flag As Boolean = True

        If (sourceInterface <> EthernetInterface.NONE) AndAlso FromInterface <> sourceInterface Then flag = False
        If (destinationInterface <> EthernetInterface.NONE) AndAlso ToInterface <> destinationInterface Then flag = False


        If TypeOf (ip) Is IPPacket AndAlso flag Then

            If packetProtocol <> IPProtocol.IPProtocolType.INVALID AndAlso ip.IPProtocol = packetProtocol Then flag = False

            If (sourceAddress IsNot Nothing) AndAlso Not ip.SourceAddress.Equals(sourceAddress) Then flag = False
            If (destinationAddress IsNot Nothing) AndAlso Not ip.DestinationAddress.Equals(destinationAddress) Then flag = False
        End If

        If TypeOf (ip) Is TCPPacket AndAlso flag Then
            Dim tcp As TCPPacket = ip

            If (sourcePort <> 0) AndAlso tcp.SourcePort <> sourcePort Then flag = False
            If (destinationPort <> 0) AndAlso tcp.DestinationPort <> destinationPort Then flag = False
        End If

        If TypeOf (ip) Is UDPPacket AndAlso flag Then
            Dim udp As UDPPacket = ip

            If (sourcePort <> 0) AndAlso udp.SourcePort <> sourcePort Then flag = False
            If (destinationPort <> 0) AndAlso udp.DestinationPort <> destinationPort Then flag = False
        End If

        If flag Then Return DoJump(ip, JUMP, FromInterface, ToInterface)
    End Function

    Public Function DoJump(ByVal ip As IPPacket, ByVal Jump As String, ByVal FromInterface As EthernetInterface, ByVal ToInterface As EthernetInterface) As Boolean
        Jump = Jump.ToUpper

        Select Case Jump
            Case "ACCEPT"
                ' Do nothing here
                Return False

            Case "DROP"
                ' We can't drop packets with SharpPcap
                Return True

            Case "LOG"
                DumpPacket(ip)
                Return False

            Case "DNAT"
                If TypeOf (ip) Is TCPPacket Then
                    m_Server.m_ConntrackLock.AcquireReaderLock(LOCK_TIMEOUT)
                    If m_Server.m_Conntrack.ContainsKey("TCP:" & ip.SourceAddress.ToString & ":" & CType(ip, TCPPacket).SourcePort) Then
                        ip.DestinationAddress = m_Server.m_Conntrack("TCP:" & ip.SourceAddress.ToString & ":" & CType(ip, TCPPacket).SourcePort)
                    End If
                    m_Server.m_ConntrackLock.ReleaseReaderLock()
                ElseIf TypeOf (ip) Is UDPPacket Then
                    m_Server.m_ConntrackLock.AcquireReaderLock(LOCK_TIMEOUT)
                    If m_Server.m_Conntrack.ContainsKey("UDP:" & ip.SourceAddress.ToString & ":" & CType(ip, UDPPacket).SourcePort) Then
                        ip.DestinationAddress = m_Server.m_Conntrack("UDP:" & ip.SourceAddress.ToString & ":" & CType(ip, UDPPacket).SourcePort)
                    End If
                    m_Server.m_ConntrackLock.ReleaseReaderLock()
                End If
                ip.DestinationHwAddress = m_Server.GetHwAddress(ip.DestinationAddress, ToInterface)
                If FromInterface = EthernetInterface.GREEN Then ip.SourceHwAddress = m_Server.m_Config.hwGreen
                If FromInterface = EthernetInterface.RED Then ip.SourceHwAddress = m_Server.m_Config.hwRed
                ip.ComputeIPChecksum(True)
                If TypeOf (ip) Is TCPPacket Then CType(ip, TCPPacket).ComputeTCPChecksum(True)
                Return False

            Case "SNAT"
                If FromInterface = EthernetInterface.GREEN Then
                    ip.SourceAddress = m_Server.m_Config.ipRed
                    ip.SourceHwAddress = m_Server.m_Config.hwRed
                ElseIf FromInterface = EthernetInterface.RED Then
                    ip.SourceAddress = m_Server.m_Config.ipGreen
                    ip.SourceHwAddress = m_Server.m_Config.hwGreen
                Else
                    Throw New ApplicationException("SNAT, but FromInterface = NONE.")
                End If
                ip.DestinationHwAddress = m_Server.m_Config.extGatewayHw
                ip.ComputeIPChecksum(True)
                If TypeOf (ip) Is TCPPacket Then CType(ip, TCPPacket).ComputeTCPChecksum(True)
                Return False

            Case "REJECT"
                ' Send ICMP REJECT
                Return True

            Case "TTL"
                If packetTTLInc <> 0 Then ip.TimeToLive += packetTTLInc
                If packetTTLSet <> 0 Then ip.TimeToLive = packetTTLSet
                ip.ComputeIPChecksum(True)
                If TypeOf (ip) Is TCPPacket Then CType(ip, TCPPacket).ComputeTCPChecksum(True)
                Return False

            Case "LIMIT"
                ' Traffic shaping
                If limiterTraffic > packetLimit Then
                    Console.WriteLine("Limit {0} reached!", packetLimit)
                    limiterLock.AcquireWriterLock(LOCK_TIMEOUT)
                    If limiterBuffer.Count > LIMITER_BUFFER_SIZE Then
                        ' Buffer is full, DROP the packet
                    Else
                        ' Put packet into Buffer
                        Dim qp As New QueuedPacket
                        qp.Packet = ip
                        qp.ToInterface = ToInterface
                        qp.FromInterface = FromInterface
                        limiterBuffer.Add(qp)
                    End If
                    limiterLock.ReleaseWriterLock()
                    Return True
                Else
                    Console.WriteLine("Limit OK!", packetLimit)
                    limiterLock.AcquireWriterLock(LOCK_TIMEOUT)
                    limiterTraffic += ip.PcapHeader.PacketLength
                    limiterLock.ReleaseWriterLock()
                    Return False
                End If

            Case "REDIRECT"
                ' Redirect PORT
                Return False

            Case Else
                Throw New ApplicationException("Unsupported JUMP: " & Jump)
        End Select
    End Function


End Class
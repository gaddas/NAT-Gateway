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


Imports SharpPcap
Imports SharpPcap.Packets


Public Class InterfacePCap
    Public sInterface As String
    Public hInterface As PcapDevice

    Public ReadOnly Property ipAddress() As Net.IPAddress
        Get
            For Each Item As Containers.PcapAddress In hInterface.Addresses
                If Item.Addr.ipAddress IsNot Nothing Then Return Item.Addr.ipAddress
            Next
            Return Net.IPAddress.None
        End Get
    End Property

    Public ReadOnly Property hwAddress() As Net.NetworkInformation.PhysicalAddress
        Get
            For Each Item As Containers.PcapAddress In hInterface.Addresses
                If Item.Addr.hardwareAddress IsNot Nothing Then Return Item.Addr.hardwareAddress
            Next
            Return Net.NetworkInformation.PhysicalAddress.None
        End Get
    End Property

    Public Sub New()
        Console.WriteLine("SharpPcap Version: {0}", SharpPcap.Version.VersionString)
    End Sub

    Public Sub Open(ByVal rawInterface As String)
        rawInterface = rawInterface.Trim
        sInterface = rawInterface
        Console.WriteLine(vbTab & "open  " & sInterface)

        If rawInterface <> "none" AndAlso rawInterface <> String.Empty Then
            hInterface = Pcap.GetPcapDevice(rawInterface)
            hInterface.Open(False, 1)
            hInterface.SetFilter("ether dst " & hwAddress.ToString)
        End If
    End Sub
    Public Sub Close()
        Console.WriteLine(vbTab & "close " & sInterface)
        hInterface.Close()
    End Sub
    Public Function Read() As Packet
        If hInterface.Opened Then
            Dim p As SharpPcap.Packets.Packet = hInterface.GetNextPacket()
            If p IsNot Nothing Then
                'Console.WriteLine(vbTab & "read  {0}, {1}", sInterface, p.PcapHeader.PacketLength)
                'Console.WriteLine(vbTab & "{2}", sInterface, p.PcapHeader.PacketLength, p.ToString)
            End If

            Return p
        Else
            Return Nothing
        End If
    End Function
    Public Sub Write(ByVal p As Packet)
        'Console.WriteLine(vbTab & "write " & sInterface)
        'Console.WriteLine(vbTab & "write " & p.ToString)
        hInterface.SendPacket(p.Bytes)
    End Sub
    Public Sub Debug()

    End Sub

End Class

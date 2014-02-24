

Imports System.Net
Imports System.Net.NetworkInformation
Imports SharpPcap
Imports SharpPcap.Packets


Public Class ArpResolver

    Public Sub New()
    End Sub

    Public Sub New(ByVal deviceName As String)
        Me.DeviceName = deviceName
    End Sub

    Private Function BuildARP(ByVal localMAC As PhysicalAddress, ByVal localIP As IPAddress) As ARPPacket
        Dim packet As New ARPPacket(14, New Byte(60 - 1) {})
        packet.ARPHwLength = 6
        packet.ARPHwType = ARPFields_Fields.ARP_ETH_ADDR_CODE
        packet.ARPProtocolLength = 4
        packet.ARPProtocolType = ARPFields_Fields.ARP_IP_ADDR_CODE
        packet.ARPSenderHwAddress = localMAC
        packet.ARPSenderProtoAddress = localIP
        packet.SourceHwAddress = localMAC
        packet.EthernetProtocol = &H806
        Return packet
    End Function

    Private Function BuildRequest(ByVal destIP As IPAddress, ByVal localMAC As PhysicalAddress, ByVal localIP As IPAddress) As ARPPacket
        Dim packet As ARPPacket = Me.BuildARP(localMAC, localIP)
        packet.ARPOperation = ARPFields_Fields.ARP_OP_REQ_CODE
        packet.ARPTargetHwAddress = PhysicalAddress.Parse("00-00-00-00-00-00")
        packet.ARPTargetProtoAddress = destIP
        packet.DestinationHwAddress = PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF")
        Return packet
    End Function

    Public Function Resolve(ByVal destIP As IPAddress) As PhysicalAddress
        If (Me.DeviceName Is Nothing) Then
            Throw New Exception("Can't resolve host: A network device must be specified")
        End If
        Return Me.Resolve(destIP, Me.DeviceName)
    End Function

    Public Function Resolve(ByVal destIP As IPAddress, ByVal deviceName As String) As PhysicalAddress
        Dim nextPacket As ARPPacket
        Dim localMAC As PhysicalAddress = Me.LocalMAC
        Dim localIP As IPAddress = Me.LocalIP
        Dim pcapDevice As PcapDevice = Pcap.GetPcapDevice(Me.DeviceName)
        If (Me.LocalMAC Is Nothing) Then
            localMAC = pcapDevice.Interface.MacAddress
        End If
        Dim p As ARPPacket = Me.BuildRequest(destIP, localMAC, localIP)
        Dim filterExpression As String = ("arp and ether dst " & localMAC.ToString)
        pcapDevice.Open(False, 20)
        pcapDevice.SetFilter(filterExpression)
        pcapDevice.SendPacket(p)

        Dim timeout As Date = Now.AddSeconds(3)
        Do
            nextPacket = DirectCast(pcapDevice.GetNextPacket, ARPPacket)
            If Date.Compare(Now, timeout) > 0 Then
                EventLog.WriteEntry("NetGate NAT", "Gateway address is not found in the network. Will use broadcast address.", EventLogEntryType.Warning)
                Exit Do
            End If
        Loop While ((nextPacket Is Nothing) OrElse Not nextPacket.ARPSenderProtoAddress.Equals(destIP))
        pcapDevice.Close()

        If nextPacket IsNot Nothing Then
            Return nextPacket.ARPSenderHwAddress
        Else
            Return Net.NetworkInformation.PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF")
        End If
    End Function


    ' Properties
    Public Property DeviceName() As String
        Get
            Return Me._deviceName
        End Get
        Set(ByVal value As String)
            Me._deviceName = value
        End Set
    End Property

    Public Property LocalIP() As IPAddress
        Get
            Return Me._localIP
        End Get
        Set(ByVal value As IPAddress)
            Me._localIP = value
        End Set
    End Property

    Public Property LocalMAC() As PhysicalAddress
        Get
            Return Me._localMAC
        End Get
        Set(ByVal value As PhysicalAddress)
            Me._localMAC = value
        End Set
    End Property


    ' Fields
    Private _deviceName As String
    Private _localIP As IPAddress
    Private _localMAC As PhysicalAddress
End Class


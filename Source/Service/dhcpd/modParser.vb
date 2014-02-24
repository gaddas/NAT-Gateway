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

 

    Public Enum BOOTP_OP As Byte
        BOOTP_OP_BOOTREQUEST = 1    ' BOOTP Request     (c->s)
        BOOTP_OP_BOOTREPLY = 2      ' BOOTP Reply       (s->c)
    End Enum
    Public Enum BOOTP_HTYPE As Byte
        BOOTP_HTYPE_ETHERNET = 1
    End Enum
    Public Enum BOOTP_HLEN As Byte
        BOOTP_HLEN_ETHERNET = 6
    End Enum
    Public Enum DHCP_OPTIONS As Byte
        DHCP_OPT_PAD = 0                ' 0x00, token padding value 			(must be skipped)	
        DHCP_OPT_NETMASK = 1            ' 0x01, subnet mask client should use 	(4 Byte mask)
        DHCP_OPT_TIMEOFFSET = 2         ' 0x02, time offset                     (uint32 seconds)
        DHCP_OPT_ROUTERS = 3            ' 0x03, Routers client should use 		(IP Addr list)
        DHCP_OPT_TIMESERVERS = 4        ' 0x04, Time Servers client should use 	(IP Addr list)
        DHCP_OPT_NAMESERVERS = 5        ' 0x05, Name Servers client should use 	(IP Addr list)
        DHCP_OPT_DNSSERVERS = 6         ' 0x06, DNS Servers client should use 	(IP Addr list)
        DHCP_OPT_LOGSERVERS = 7         ' 0x07, Log Servers                     (IP Addr list)
        DHCP_OPT_COOKIESERVERS = 8      ' 0x08, Cookie Servers                  (IP Addr list)
        DHCP_OPT_LPRSERVERS = 9         ' 0x09, LRP Servers                     (IP Addr list)
        DHCP_OPT_IMPRESSSERVERS = 10    ' 0x0A, Impress Servers                 (IP Addr list)
        DHCP_OPT_RESOURCELOCATION = 11  ' 0x0B, Resource Logation Servers       (IP Addr list)
        DHCP_OPT_HOSTNAME = 12          ' 0x0C, Host name client should use 	(String)
        DHCP_OPT_BOOTFILESIZE = 13      ' 0x0D, Boot File Size                  (uint16)
        DHCP_OPT_MERITDUMPFILE = 14     ' 0x0E, Merit Dump File Location        (String)
        DHCP_OPT_DOMAINNAME = 15        ' 0x0F, Domain name client should use 	(String)
        DHCP_OPT_IPFORWARDING = 19      ' 0x13, IP Packet Forwarding            (byte, 0/1)
        DHCP_OPT_INTERFACEMTU = 26      ' 0x1A, Default MTU for use on          (uint16)
        DHCP_OPT_DEFAULTUDPTTL = 23     ' 0x17, Default IP Time To Live         (byte)
        DHCP_OPT_BROADCASTADDR = 28     ' 0x1C, Broadcast Address               (IP Addr)
        DHCP_OPT_ARPCACHETIMEOUT = 35   ' 0x23, ARP Cache Timeout               (uint32)
        DHCP_OPT_DEFAULTTCPTTL = 37     ' 0x25, Default TCP Time To Live        (byte)
        DHCP_OPT_NTPSERVERS = 42        ' 0x2A, Network Time Protocol Server    (IP Addr list)
        DHCP_OPT_VENDORSPEC = 43        ' 0x2B, Vendor Specific Information
        DHCP_OPT_NBTSERVERS = 44        ' 0x2C, NetBIOS over TCP/IP Name Server (IP Addr list)
        DHCP_OPT_REQUESTEDIP = 50       ' 0x32, IP Address requested by client	(IP Addr)
        DHCP_OPT_LEASETIME = 51         ' 0x33, DHCP Lease Time			        (uint32 seconds)
        DHCP_OPT_DHCPMSGTYPE = 53       ' 0x35, DHCP Message Type			    (byte)
        DHCP_OPT_SERVERID = 54          ' 0x36, Server Identifier			    (IP Addr)
        DHCP_OPT_RAPAMREQLIST = 55      ' 0x37, Parameter request list		    (n OPT codes)
        DHCP_OPT_RENEWALTIME = 58       ' 0x3A, DHCP Lease Renewal Time		    (uint32 seconds)
        DHCP_OPT_REBINDTIME = 59        ' 0x3B, DHCP Lease Rebinding Time		(uint32 seconds)
        DHCP_OPT_VERSION = 60           ' 0x3C, DHCP Client Version             (HW Addr)
        DHCP_OPT_CLIENTID = 61          ' 0x3D, Client Identifier               (String)
        DHCP_OPT_SMTPSERVERS = 69       ' 0x45, Simple Mail Transport Protocol  (IP Addr list)
        DHCP_OPT_POP3SERVERS = 70       ' 0x46, Post Office Protocol            (IP Addr list)
        DHCP_OPT_NNTPSERVERS = 71       ' 0x47, Network News Transport Protocol (IP Addr list)
        DHCP_OPT_WWWSERVERS = 72        ' 0x48, Default World Wide Web          (IP Addr list)
        DHCP_OPT_FINGERSERVERS = 73     ' 0x49, Default Finger Server           (IP Addr list)
        DHCP_OPT_IRCSERVERS = 74        ' 0x4A, Default Internet Relay Chat     (IP Addr list)
        DHCP_OPT_END = 255              ' 0xFF, token end value			        (marks end of option list)
    End Enum
    Public Enum DHCP_MESSAGES As Byte
        DHCP_MSG_DISCOVER = 1           ' c->broadcast
        DHCP_MSG_OFFER = 2              ' s->c
        DHCP_MSG_REQUEST = 3            ' c->s
        DHCP_MSG_DECLINE = 4            ' c->s
        DHCP_MSG_ACK = 5                ' s->c
        DHCP_MSG_NACK = 6               ' s->c
        DHCP_MSG_RELEASE = 7            ' c->s
        DHCP_MSG_INFORM = 8             ' c->s
    End Enum

    Public Sub OnPacket(ByVal p As Packet)
        Dim r As New Packet

        Try
            Dim bpOperation As BOOTP_OP = p.GetByte
            Dim bpHType As BOOTP_HTYPE = p.GetByte                          '1 = Ethernet
            Dim bpHLength As BOOTP_HLEN = p.GetByte                         '6 = MAC
            Dim bpHops As Byte = p.GetByte
            Dim bpTransactionId As UInteger = p.GetUInt32
            Dim bpDhcpNegotationTimeElapsed As UShort = p.GetUInt16         'Seconds
            Dim bpFlags As UShort = p.GetUInt16
            Dim bpAddressClient As IPAddress = IPAddress.Parse(p.GetIPAddress)
            Dim bpAddressAssigned As IPAddress = IPAddress.Parse(p.GetIPAddress)
            Dim bpAddressNextServer As IPAddress = IPAddress.Parse(p.GetIPAddress)
            Dim bpAddressRelayAgent As IPAddress = IPAddress.Parse(p.GetIPAddress)
            Dim bpAddressHardware As String = p.GetHWAddress().ToLower
            p.GetString(16 - bpHLength)
            Dim bpServerHostname As String = p.GetString(64).Replace(Chr(0), "")
            Dim bpBootFile As String = p.GetString(128).Replace(Chr(0), "")
            Dim dhcpCookie As UInteger = p.GetUInt32                        '0x63538263

            Dim dhcpOptions As DHCP_OPTIONS
            Dim dhcpLength As Byte

            Dim optMsgType As DHCP_MESSAGES = 0
            Dim optHostname As String = ""
            Dim optVersion As String = ""
            Dim optClientID As String = ""
            Dim optNetmask As IPAddress = IPAddress.Parse("255.255.255.0")
            Dim optRequestedIp As IPAddress = IPAddress.Any
            Dim optParamRequestList As New List(Of Byte)

            Do
                dhcpOptions = p.GetByte
                dhcpLength = 0

                If dhcpOptions <> DHCP_OPTIONS.DHCP_OPT_PAD And dhcpOptions <> DHCP_OPTIONS.DHCP_OPT_END Then dhcpLength = p.GetByte

                Select Case dhcpOptions
                    Case DHCP_OPTIONS.DHCP_OPT_PAD
                        '-----------------------------------------
                    Case DHCP_OPTIONS.DHCP_OPT_NETMASK
                        optNetmask = IPAddress.Parse(p.GetIPAddress)
                    Case DHCP_OPTIONS.DHCP_OPT_HOSTNAME
                        optHostname = p.GetString(dhcpLength)
                    Case DHCP_OPTIONS.DHCP_OPT_REQUESTEDIP
                        optRequestedIp = IPAddress.Parse(p.GetIPAddress)
                    Case DHCP_OPTIONS.DHCP_OPT_DHCPMSGTYPE
                        optMsgType = p.GetByte
                    Case DHCP_OPTIONS.DHCP_OPT_RAPAMREQLIST
                        For i As Integer = 0 To dhcpLength - 1
                            optParamRequestList.Add(p.GetByte)
                        Next
                    Case DHCP_OPTIONS.DHCP_OPT_VERSION
                        optVersion = p.GetString(dhcpLength)
                    Case DHCP_OPTIONS.DHCP_OPT_CLIENTID
                        p.GetByte()
                        optClientID = p.GetHWAddress()
                    Case DHCP_OPTIONS.DHCP_OPT_END
                        '-----------------------------------------
                    Case Else
                        For i As Integer = 0 To dhcpLength - 1
                            p.GetByte()
                        Next
                End Select
            Loop While dhcpOptions <> DHCP_OPTIONS.DHCP_OPT_END

            ' =========================================================== RESPONSE ======================================
            Dim ip As IPAddress
            If optMsgType = DHCP_MESSAGES.DHCP_MSG_RELEASE Then
                clientFreeIP(bpAddressClient)
                ip = bpAddressClient
            Else
                ip = clientGetIP(bpAddressHardware, optHostname)
            End If

            ' Don't reply to client if we won't give him an IP
            If ip Is Nothing Then Return


            r.IP = New IPEndPoint(IPAddress.Broadcast, 68)

            r.AddByte(BOOTP_OP.BOOTP_OP_BOOTREPLY)
            r.AddByte(bpHType)
            r.AddByte(bpHLength)
            r.AddByte(bpHops)
            r.AddInt32(bpTransactionId)
            r.AddUInt16(bpDhcpNegotationTimeElapsed)
            r.AddUInt16(bpFlags)
            r.AddIPAddress(bpAddressClient)
            r.AddIPAddress(ip)                          'Assigned
            r.AddIPAddress(IPAddress.Parse("0.0.0.0"))      'Next Server
            r.AddIPAddress(IPAddress.Parse("0.0.0.0"))      'Relay Gateway
            r.AddHWAddress(bpAddressHardware)
            r.AddStringFixed(bpAddressHardware, 16 - bpHLength)
            If m_Server.m_Config.options.ContainsKey("server-name") Then
                r.AddStringFixed(m_Server.m_Config.options("server-name"), 64)
            Else
                r.AddStringFixed("", 64)
            End If

            r.AddStringFixed(m_Server.m_Config.bootFileName, 128)
            r.AddInt32(dhcpCookie)



            Select Case optMsgType
                Case DHCP_MESSAGES.DHCP_MSG_DISCOVER
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DHCPMSGTYPE)
                    r.AddByte(1)
                    r.AddByte(DHCP_MESSAGES.DHCP_MSG_OFFER)

                Case DHCP_MESSAGES.DHCP_MSG_REQUEST
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DHCPMSGTYPE)
                    r.AddByte(1)
                    r.AddByte(DHCP_MESSAGES.DHCP_MSG_ACK)

                Case DHCP_MESSAGES.DHCP_MSG_RELEASE

                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DHCPMSGTYPE)
                    r.AddByte(1)
                    r.AddByte(DHCP_MESSAGES.DHCP_MSG_ACK)

                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_CLIENTID)
                    r.AddByte(7)
                    r.AddByte(1)
                    r.AddHWAddress(optClientID)

            End Select

            If optMsgType = DHCP_MESSAGES.DHCP_MSG_DISCOVER Or optMsgType = DHCP_MESSAGES.DHCP_MSG_REQUEST Then
                r.AddByte(DHCP_OPTIONS.DHCP_OPT_LEASETIME)
                r.AddByte(4)
                r.AddUInt32(Fix(m_Leases(ip).Subtract(Now).TotalSeconds))

                If m_Server.m_Config.options.ContainsKey("subnet-mask") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_NETMASK)
                    r.AddByte(4)
                    r.AddIPAddress(m_Server.m_Config.options("subnet-mask"))
                End If

                If m_Server.m_Config.options.ContainsKey("time-servers") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_TIMESERVERS)
                    r.AddByte(CType(m_Server.m_Config.options("time-servers"), List(Of IPAddress)).Count * 4)
                    For Each s As IPAddress In m_Server.m_Config.options("time-servers")
                        r.AddIPAddress(s)
                    Next
                End If

                If m_Server.m_Config.options.ContainsKey("boot-file-size") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_BOOTFILESIZE)
                    r.AddUInt16(m_Server.m_Config.options("boot-file-size"))
                End If

                If m_Server.m_Config.options.ContainsKey("domain-name") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DOMAINNAME)
                    r.AddString(m_Server.m_Config.options("domain-name"))
                End If

                If m_Server.m_Config.options.ContainsKey("ip-forwarding") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_IPFORWARDING)
                    r.AddByte(1)
                    If m_Server.m_Config.options("ip-forwarding") Then r.AddByte(1) Else r.AddByte(0)
                End If

                If m_Server.m_Config.options.ContainsKey("default-udp-ttl") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DEFAULTUDPTTL)
                    r.AddByte(m_Server.m_Config.options("default-udp-ttl"))
                End If

                If m_Server.m_Config.options.ContainsKey("default-mtu") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_INTERFACEMTU)
                    r.AddUInt16(m_Server.m_Config.options("default-mtu"))
                End If

                If m_Server.m_Config.options.ContainsKey("broadcast-address") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_BROADCASTADDR)
                    r.AddByte(4)
                    r.AddIPAddress(m_Server.m_Config.options("broadcast-address"))
                End If

                If m_Server.m_Config.options.ContainsKey("arp-cache-timeout") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_ARPCACHETIMEOUT)
                    r.AddUInt32(m_Server.m_Config.options("arp-cache-timeout"))
                End If

                If m_Server.m_Config.options.ContainsKey("default-tcp-ttl") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DEFAULTTCPTTL)
                    r.AddByte(m_Server.m_Config.options("default-tcp-ttl"))
                End If

                If m_Server.m_Config.options.ContainsKey("ntp-servers") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_NTPSERVERS)
                    r.AddByte(CType(m_Server.m_Config.options("ntp-servers"), List(Of IPAddress)).Count * 4)
                    For Each s As IPAddress In m_Server.m_Config.options("ntp-servers")
                        r.AddIPAddress(s)
                    Next
                End If

                If m_Server.m_Config.options.ContainsKey("netbios-name-servers") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_NBTSERVERS)
                    r.AddByte(CType(m_Server.m_Config.options("netbios-name-servers"), List(Of IPAddress)).Count * 4)
                    For Each s As IPAddress In m_Server.m_Config.options("netbios-name-servers")
                        r.AddIPAddress(s)
                    Next
                End If

                If m_Server.m_Config.options.ContainsKey("routers") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_ROUTERS)
                    r.AddByte(CType(m_Server.m_Config.options("routers"), List(Of IPAddress)).Count * 4)
                    For Each s As IPAddress In m_Server.m_Config.options("routers")
                        r.AddIPAddress(s)
                    Next
                End If

                If m_Server.m_Config.options.ContainsKey("domain-name-servers") Then
                    r.AddByte(DHCP_OPTIONS.DHCP_OPT_DNSSERVERS)
                    r.AddByte(CType(m_Server.m_Config.options("domain-name-servers"), List(Of IPAddress)).Count * 4)
                    For Each s As IPAddress In m_Server.m_Config.options("domain-name-servers")
                        r.AddIPAddress(s)
                    Next
                End If
            End If

            If m_Server.m_Config.options.ContainsKey("server-identifier") Then
                r.AddByte(DHCP_OPTIONS.DHCP_OPT_SERVERID)
                r.AddByte(4)
                r.AddIPAddress(m_Server.m_Config.options("server-identifier"))
            End If

            r.AddByte(DHCP_OPTIONS.DHCP_OPT_END)

#If DEBUG Then
            m_Lock.AcquireWriterLock(LOCK_TIMEOUT)
            Console.WriteLine("recv packet:")
            Console.WriteLine(p.ToString)
            Console.WriteLine("sent packet:")
            Console.WriteLine(r.ToString)
            m_Lock.ReleaseWriterLock()
#End If
        Catch ex As Exception
            EventLog.WriteEntry("NetGate DHCP", ex.ToString, EventLogEntryType.Error)
#If DEBUG Then
            Console.WriteLine(ex.ToString)
#End If
        End Try
        m_Server.AsyncBeginSend(r)
    End Sub



   


End Module
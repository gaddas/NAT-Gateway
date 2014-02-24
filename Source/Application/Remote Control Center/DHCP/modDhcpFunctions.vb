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


Module modDhcpFunctions

    Public configFile As String = ""
    Public configData As New dhcpOptions

    Public Sub InitializeFromConfig()
        ClearComments()

        configData = New dhcpOptions
        configData.ReadConfig(configFile)
    End Sub
    Private Sub ClearComments()
        ' Load confing in memory
        Dim f As IO.Stream = New IO.MemoryStream()
        Dim w As New IO.StreamWriter(f)
        w.Write(configFile)
        w.Flush()
        f.Seek(0, IO.SeekOrigin.Begin)

        ' Get streams
        Dim r As New IO.StreamReader(f)
        Dim s As String

        configFile = ""
        While Not r.EndOfStream
            s = r.ReadLine()
            If s.Contains("#") Then s = s.Remove(s.IndexOf("#"))
            s = s.Replace(vbTab, " ")
            s = s.Replace("  ", " ")
            configFile += s
        End While

        configFile = configFile.Replace(vbTab, " ")
        configFile = configFile.Replace(vbNewLine, " ")
        configFile = configFile.Replace(vbCr, " ")
        configFile = configFile.Replace(vbLf, " ")
        configFile = configFile.Replace("  ", " ")
    End Sub
    Public Sub ShowConfiguration()
        ' Create config form
        Dim frmSubnetConfigFrom As New frmSubnetConfig

        ' Fill network interfaces (IP)
        frmSubnetConfigFrom.lbInterfaces.Items.Clear()
        frmSubnetConfigFrom.lbInterfaces.Items.Add("0.0.0.0")
        If "0.0.0.0" = configData.interfaceAddr.ToString Then
            frmSubnetConfigFrom.lbInterfaces.SelectedIndex = 0
        End If
        Dim ipentry As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
        Dim index As Integer
        For Each ip As IPAddress In ipentry.AddressList
            index = frmSubnetConfigFrom.lbInterfaces.Items.Add(ip.ToString)
            If ip.ToString = configData.interfaceAddr.ToString Then
                frmSubnetConfigFrom.lbInterfaces.SelectedIndex = index
            End If
        Next

        'BEGIN OPEN  ==========================================================================

        frmSubnetConfigFrom.tbBootFilename.Text = configData.bootFileName
        frmSubnetConfigFrom.tbAddrRangeStart.Text = configData.ipRangeStart.ToString
        frmSubnetConfigFrom.tbAddrRangeEnd.Text = configData.ipRangeEnd.ToString
        frmSubnetConfigFrom.tbLeaseDefault.Text = configData.leaseTimeDefault
        frmSubnetConfigFrom.tbLeaseMaximal.Text = configData.leaseTimeMax
        frmSubnetConfigFrom.cbPingCheck.Checked = configData.pingCheck
        frmSubnetConfigFrom.cbDenyUnknownClients.Checked = configData.denyUnKnownClients

        ' Add options
        For Each e As KeyValuePair(Of String, Object) In configData.options
            Select Case e.Key
                Case "server-name"
                    ' not present in GUI
                Case "domain-name"
                    frmSubnetConfigFrom.cbDomainName.Checked = True
                    frmSubnetConfigFrom.tbDomainName.Text = e.Value
                Case "routers"
                    frmSubnetConfigFrom.cbGateways.Checked = True
                    With CType(e.Value, List(Of IPAddress))
                        If .Count > 0 Then frmSubnetConfigFrom.ipGateway1.Text = .Item(0).ToString
                        If .Count > 1 Then frmSubnetConfigFrom.ipGateway2.Text = .Item(1).ToString
                    End With
                Case "domain-name-servers"
                    frmSubnetConfigFrom.cbDNS.Checked = True
                    With CType(e.Value, List(Of IPAddress))
                        If .Count > 0 Then frmSubnetConfigFrom.ipDNS1.Text = .Item(0).ToString
                        If .Count > 1 Then frmSubnetConfigFrom.ipDNS2.Text = .Item(1).ToString
                    End With
                Case "ntp-servers"
                    frmSubnetConfigFrom.cbNetworkTimeServers.Checked = True
                    With CType(e.Value, List(Of IPAddress))
                        If .Count > 0 Then frmSubnetConfigFrom.ipNetworkTimeServer1.Text = .Item(0).ToString
                    End With
                Case "time-servers"
                    frmSubnetConfigFrom.cbTimeServers.Checked = True
                    With CType(e.Value, List(Of IPAddress))
                        If .Count > 0 Then frmSubnetConfigFrom.ipTimeServer1.Text = .Item(0).ToString
                    End With
                Case "netbios-name-servers"
                    frmSubnetConfigFrom.cbNBNameServers.Checked = True
                    With CType(e.Value, List(Of IPAddress))
                        If .Count > 0 Then frmSubnetConfigFrom.ipNBNameServer1.Text = .Item(0).ToString
                    End With

                Case "broadcast-address"
                    frmSubnetConfigFrom.cbBroadcastAddr.Checked = True
                    frmSubnetConfigFrom.ipBroadcastAddr.Text = e.Value.ToString
                Case "subnet-mask"
                    frmSubnetConfigFrom.cbSubnetMask.Checked = True
                    frmSubnetConfigFrom.ipSubnetMask.Text = e.Value.ToString
                Case "server-identifier"
                    frmSubnetConfigFrom.ipServerId.Text = e.Value.ToString
                Case "ip-forwarding"
                    frmSubnetConfigFrom.cbEnableIpForward.Checked = True
                Case "boot-file-size"
                    frmSubnetConfigFrom.cbBootFileSize.Checked = True
                    frmSubnetConfigFrom.tbBootFileSize.Text = e.Value
                Case "default-udp-ttl"
                    frmSubnetConfigFrom.cbDefaultTtlForUdp.Checked = True
                    frmSubnetConfigFrom.tbDefaultTtlForUdp.Text = e.Value
                Case "default-tcp-ttl"
                    frmSubnetConfigFrom.cbDefaultTtlForTcp.Checked = True
                    frmSubnetConfigFrom.tbDefaultTtlForTcp.Text = e.Value
                Case "default-mtu"
                    frmSubnetConfigFrom.cbDefaultMtu.Checked = True
                    frmSubnetConfigFrom.tbDefaultMtu.Text = e.Value
                Case "arp-cache-timeout"
                    frmSubnetConfigFrom.cbArpCacheTimeout.Checked = True
                    frmSubnetConfigFrom.tbArpCacheTimeout.Text = e.Value
            End Select
        Next

        ' Add known clients
        For Each c As dhcpOptions.HostClient In configData.knownClients
            frmSubnetConfigFrom.lvClients.Items.Add(New ListViewItem(New String() {c.ipAddr.ToString, c.hwAddr, c.hostName}))
        Next

        'END OPEN ============================================================================

        ' Show config form
        If frmSubnetConfigFrom.ShowDialog() = DialogResult.OK Then

            'BEGIN SAVE ==========================================================================

            ' Save config
            configData.interfaceAddr = IPAddress.Parse(frmSubnetConfigFrom.lbInterfaces.SelectedItem.ToString)

            configData.bootFileName = frmSubnetConfigFrom.tbBootFilename.Text
            configData.ipRangeStart = IPAddress.Parse(frmSubnetConfigFrom.tbAddrRangeStart.Text)
            configData.ipRangeEnd = IPAddress.Parse(frmSubnetConfigFrom.tbAddrRangeEnd.Text)
            configData.leaseTimeDefault = frmSubnetConfigFrom.tbLeaseDefault.Text
            configData.leaseTimeMax = frmSubnetConfigFrom.tbLeaseMaximal.Text
            configData.pingCheck = frmSubnetConfigFrom.cbPingCheck.Checked
            configData.denyUnKnownClients = frmSubnetConfigFrom.cbDenyUnknownClients.Checked

            ' Save options
            configData.options.Clear()

            configData.options.Add("server-identifier", IPAddress.Parse(frmSubnetConfigFrom.ipServerId.Text))
            'configData.options.Add("server-name", "")

            If frmSubnetConfigFrom.cbDomainName.Checked Then
                configData.options.Add("domain-name", frmSubnetConfigFrom.tbDomainName.Text)
            End If

            If frmSubnetConfigFrom.cbGateways.Checked Then
                Dim list As New List(Of IPAddress)
                If frmSubnetConfigFrom.ipGateway1.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipGateway1.Text))
                If frmSubnetConfigFrom.ipGateway2.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipGateway2.Text))
                configData.options.Add("routers", list)
            End If

            If frmSubnetConfigFrom.cbDNS.Checked Then
                Dim list As New List(Of IPAddress)
                If frmSubnetConfigFrom.ipDNS1.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipDNS1.Text))
                If frmSubnetConfigFrom.ipDNS2.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipDNS2.Text))
                configData.options.Add("domain-name-servers", list)
            End If

            If frmSubnetConfigFrom.cbNetworkTimeServers.Checked Then
                Dim list As New List(Of IPAddress)
                If frmSubnetConfigFrom.ipNetworkTimeServer1.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipNetworkTimeServer1.Text))
                configData.options.Add("ntp-servers", list)
            End If

            If frmSubnetConfigFrom.cbTimeServers.Checked Then
                Dim list As New List(Of IPAddress)
                If frmSubnetConfigFrom.ipTimeServer1.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipTimeServer1.Text))
                configData.options.Add("time-servers", list)
            End If

            If frmSubnetConfigFrom.cbNBNameServers.Checked Then
                Dim list As New List(Of IPAddress)
                If frmSubnetConfigFrom.ipNBNameServer1.Text <> "..." Then list.Add(IPAddress.Parse(frmSubnetConfigFrom.ipNBNameServer1.Text))
                configData.options.Add("netbios-name-servers", list)
            End If

            If frmSubnetConfigFrom.cbBroadcastAddr.Checked Then
                configData.options.Add("broadcast-address", IPAddress.Parse(frmSubnetConfigFrom.ipBroadcastAddr.Text))
            End If

            If frmSubnetConfigFrom.cbSubnetMask.Checked Then
                configData.options.Add("subnet-mask", IPAddress.Parse(frmSubnetConfigFrom.ipSubnetMask.Text))
            End If

            If frmSubnetConfigFrom.cbEnableIpForward.Checked Then
                configData.options.Add("ip-forwarding", frmSubnetConfigFrom.cbEnableIpForward.Checked)
            End If

            If frmSubnetConfigFrom.cbBootFileSize.Checked Then
                configData.options.Add("boot-file-size", CType(frmSubnetConfigFrom.tbBootFileSize.Text, Integer))
            End If

            If frmSubnetConfigFrom.cbDefaultTtlForUdp.Checked Then
                configData.options.Add("default-udp-ttl", CType(frmSubnetConfigFrom.tbDefaultTtlForUdp.Text, Integer))
            End If

            If frmSubnetConfigFrom.cbDefaultTtlForTcp.Checked Then
                configData.options.Add("default-tcp-ttl", CType(frmSubnetConfigFrom.tbDefaultTtlForTcp.Text, Integer))
            End If

            If frmSubnetConfigFrom.cbDefaultMtu.Checked Then
                configData.options.Add("default-mtu", CType(frmSubnetConfigFrom.tbDefaultMtu.Text, Integer))
            End If

            If frmSubnetConfigFrom.cbArpCacheTimeout.Checked Then
                configData.options.Add("arp-cache-timeout", CType(frmSubnetConfigFrom.tbArpCacheTimeout.Text, Integer))
            End If


            ' Save known clients
            configData.knownClients.Clear()
            For Each Item As ListViewItem In frmSubnetConfigFrom.lvClients.Items
                Dim NewClient As New dhcpOptions.HostClient
                NewClient.ipAddr = IPAddress.Parse(Item.SubItems(0).Text)
                NewClient.hwAddr = Item.SubItems(1).Text
                NewClient.hostName = Item.SubItems(2).Text
                configData.knownClients.Add(NewClient)
            Next

            'END SAVE =============================================================================

            configFile = configData.ToString
            remoteDhcpObject.SetConfig(modDhcpFunctions.configFile)
            remoteDhcpObject.Reload()
        End If

        ' Clean
        frmSubnetConfigFrom.Close()
        frmSubnetConfigFrom.Dispose()
    End Sub

End Module


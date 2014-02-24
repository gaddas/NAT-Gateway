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


Imports System.Resources
Imports System.Globalization
Imports System.Reflection
Imports System.ComponentModel


Public Class frmMain

    Public Sub New()
        My.Application.ChangeUICulture(My.Settings.Language)
        My.Application.ChangeCulture(My.Settings.Language)
        Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo(My.Settings.Language)
        Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo(My.Settings.Language)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' ...
    End Sub

    Private Sub pageDHCP_btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageDHCP_btnSetup.Click
        modDhcpFunctions.configFile = remoteDhcpObject.GetConfig()
        modDhcpFunctions.InitializeFromConfig()
        modDhcpFunctions.ShowConfiguration()
    End Sub

    Private Sub frmMain_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        TimerCheckServices_Tick(sender, e)
    End Sub

    Private Sub tcMainTab_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tcMainTab.SelectedIndexChanged
        If (tcMainTab.SelectedTab Is tpDHCP) AndAlso Not dhcpRemoteStatus() Then
            MessageBox.Show(My.Resources.ThisServiceIsNotActive, My.Resources.CaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information)
            tcMainTab.SelectedTab = tpStatus
        ElseIf (tcMainTab.SelectedTab Is tpDNS) AndAlso Not dnsRemoteStatus() Then
            MessageBox.Show(My.Resources.ThisServiceIsNotActive, My.Resources.CaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information)
            tcMainTab.SelectedTab = tpStatus
        ElseIf ((tcMainTab.SelectedTab Is tpFirewall) OrElse _
               (tcMainTab.SelectedTab Is tpNAT) OrElse _
               (tcMainTab.SelectedTab Is tpTraffSh)) AndAlso Not natRemoteStatus() Then
            MessageBox.Show(My.Resources.ThisServiceIsNotActive, My.Resources.CaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information)
            tcMainTab.SelectedTab = tpStatus
        End If


        If tcMainTab.SelectedTab Is tpDHCP Then
            pageDHCP_btnRefresh_Click(sender, e)
        End If

        If tcMainTab.SelectedTab Is tpDNS Then
            modDnsFunctions.InitializeFromConfig()
        End If

        If tcMainTab.SelectedTab Is tpFirewall OrElse _
           tcMainTab.SelectedTab Is tpTraffSh OrElse _
           tcMainTab.SelectedTab Is tpNAT Then
            modNatFunctions.InitializeFromConfig()
        End If

    End Sub

    Private Sub menuItemExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemExit.Click, notifyMenuItemExit.Click
        Application.Exit()
    End Sub

    Private Sub menuItemAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemAbout.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub menuItemOpenRemote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemOpenRemote.Click
        Dim frmRemoteAddressForm As New frmRemoteAddress
        Dim addr As String
        Dim user As String
        Dim pass As String

        If frmRemoteAddressForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            addr = frmRemoteAddressForm.AddressTextBox.Text
            frmRemoteAddressForm.Hide()
            frmRemoteAddressForm.Dispose()
            frmRemoteAddressForm = Nothing
        Else
            Return
        End If

        Dim frmLoginForm As New frmLogin
        If frmLoginForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            user = frmLoginForm.UsernameTextBox.Text
            pass = frmLoginForm.PasswordTextBox.Text
            frmLoginForm.Hide()
            frmLoginForm.Dispose()
            frmLoginForm = Nothing

            If Not remoteConnect("localhost", user, pass) Then
                MessageBox.Show(My.Resources.CouldNotConnectToService, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Else
            Return
        End If
    End Sub

    Private Sub menuItemOpenLocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemOpenLocal.Click, pageStatus_btnLocalServer.Click
        Dim frmLoginForm As New frmLogin
        Dim user As String
        Dim pass As String

        If frmLoginForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            user = frmLoginForm.UsernameTextBox.Text
            pass = frmLoginForm.PasswordTextBox.Text
            frmLoginForm.Hide()
            frmLoginForm.Dispose()
            frmLoginForm = Nothing

            If Not remoteConnect("localhost", user, pass) Then
                MessageBox.Show(My.Resources.CouldNotConnectToService, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Else
            Return
        End If

    End Sub

    Private Sub TimerCheckServices_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerCheckServices.Tick, pageStatus_btnRefresh.Click, menuItemTools.Click
        Dim sFoundDhcp As Boolean = False
        Dim sFoundDns As Boolean = False
        Dim sFoundNat As Boolean = False

        For Each s As ServiceProcess.ServiceController In ServiceProcess.ServiceController.GetServices()
            If s.ServiceName = "NetGate DHCPd" Then
                sFoundDhcp = True
                menuItemDhcpInstall.Enabled = False
                menuItemDhcpUnInstall.Enabled = True
                Select Case s.Status
                    Case ServiceProcess.ServiceControllerStatus.Running
                        pageStatus_btnStartDHCP.Enabled = False
                        pageStatus_btnStopDHCP.Enabled = True
                        pageStatus_lblDHCP.ForeColor = Color.DarkGreen
                    Case Else
                        pageStatus_btnStartDHCP.Enabled = True
                        pageStatus_btnStopDHCP.Enabled = False
                        pageStatus_lblDHCP.ForeColor = Color.DarkRed
                End Select
            End If
            If s.ServiceName = "NetGate DNSd" Then
                sFoundDns = True
                menuItemDnsInstall.Enabled = False
                menuItemDnsUnInstall.Enabled = True
                Select Case s.Status
                    Case ServiceProcess.ServiceControllerStatus.Running
                        pageStatus_btnStartDNS.Enabled = False
                        pageStatus_btnStopDNS.Enabled = True
                        pageStatus_lblDNS.ForeColor = Color.DarkGreen
                    Case Else
                        pageStatus_btnStartDNS.Enabled = True
                        pageStatus_btnStopDNS.Enabled = False
                        pageStatus_lblDNS.ForeColor = Color.DarkRed
                End Select
            End If
            If s.ServiceName = "NetGate NATd" Then
                sFoundNat = True
                menuItemNatInstall.Enabled = False
                menuItemNatUnInstall.Enabled = True
                Select Case s.Status
                    Case ServiceProcess.ServiceControllerStatus.Running
                        pageStatus_btnStartNAT.Enabled = False
                        pageStatus_btnStopNAT.Enabled = True
                        pageStatus_lblNAT.ForeColor = Color.DarkGreen
                    Case Else
                        pageStatus_btnStartNAT.Enabled = True
                        pageStatus_btnStopNAT.Enabled = False
                        pageStatus_lblNAT.ForeColor = Color.DarkRed
                End Select
            End If
        Next

        If Not sFoundDhcp Then
            pageStatus_btnStartDHCP.Enabled = False
            pageStatus_btnStopDHCP.Enabled = False
            pageStatus_lblDHCP.ForeColor = Color.Gray
            menuItemDhcpInstall.Enabled = True
            menuItemDhcpUnInstall.Enabled = False
        End If
        If Not sFoundDns Then
            pageStatus_btnStartDNS.Enabled = False
            pageStatus_btnStopDNS.Enabled = False
            pageStatus_lblDNS.ForeColor = Color.Gray
            menuItemDnsInstall.Enabled = True
            menuItemDnsUnInstall.Enabled = False
        End If
        If Not sFoundNat Then
            pageStatus_btnStartNAT.Enabled = False
            pageStatus_btnStopNAT.Enabled = False
            pageStatus_lblNAT.ForeColor = Color.Gray
            menuItemNatInstall.Enabled = True
            menuItemNatUnInstall.Enabled = False
        End If

    End Sub

    Private Sub pageStatus_btnStartDHCP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageStatus_btnStartDHCP.Click
        pageStatus_btnStartDHCP.Enabled = False
        TimerCheckServices.Enabled = False
        ServiceControllerDHCP.Start()
        TimerCheckServices.Enabled = True
    End Sub

    Private Sub pageStatus_btnStopDHCP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageStatus_btnStopDHCP.Click
        pageStatus_btnStopDHCP.Enabled = False
        TimerCheckServices.Enabled = False
        ServiceControllerDHCP.Stop()
        TimerCheckServices.Enabled = True
    End Sub

    Private Sub pageStatus_btnStopDNS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageStatus_btnStopDNS.Click
        pageStatus_btnStopDNS.Enabled = False
        TimerCheckServices.Enabled = False
        ServiceControllerDNS.Stop()
        TimerCheckServices.Enabled = True
    End Sub

    Private Sub pageStatus_btnStopNAT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageStatus_btnStopNAT.Click
        pageStatus_btnStopNAT.Enabled = False
        TimerCheckServices.Enabled = False
        ServiceControllerNAT.Stop()
        TimerCheckServices.Enabled = True
    End Sub

    Private Sub pageStatus_btnStartDNS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageStatus_btnStartDNS.Click
        pageStatus_btnStartDNS.Enabled = False
        TimerCheckServices.Enabled = False
        ServiceControllerDNS.Start()
        TimerCheckServices.Enabled = True
    End Sub

    Private Sub pageStatus_btnStartNAT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageStatus_btnStartNAT.Click
        pageStatus_btnStartNAT.Enabled = False
        TimerCheckServices.Enabled = False
        ServiceControllerNAT.Start()
        TimerCheckServices.Enabled = True
    End Sub

    Private Sub menuItemManageAccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemManageAccess.Click
        frmManageAccess.ShowDialog()
    End Sub

    Private Sub notifyMenuItemShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles notifyMenuItemShow.Click, NotifyIconMain.DoubleClick
        Me.Show()
    End Sub

    Private Sub menuItemToTray_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemToTray.Click
        Me.Hide()
    End Sub


    Private Sub menuItemDhcpInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemDhcpInstall.Click
        Dim processInfo As ProcessStartInfo
        Dim process As Process

        processInfo = New ProcessStartInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory & "\InstallUtil.exe", "dhcpd.exe")
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = False
        process = process.Start(processInfo)
        process.WaitForExit(10000)
        process.Close()
    End Sub

    Private Sub menuItemDhcpUnInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemDhcpUnInstall.Click
        Dim processInfo As ProcessStartInfo
        Dim process As Process

        processInfo = New ProcessStartInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory & "\InstallUtil.exe", "/u dhcpd.exe")
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = False
        process = process.Start(processInfo)
        process.WaitForExit(10000)
        process.Close()
    End Sub

    Private Sub menuItemDnsInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemDnsInstall.Click
        Dim processInfo As ProcessStartInfo
        Dim process As Process

        processInfo = New ProcessStartInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory & "\InstallUtil.exe", "dnsd.exe")
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = False
        process = process.Start(processInfo)
        process.WaitForExit(10000)
        process.Close()
    End Sub

    Private Sub menuItemDnsUnInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemDnsUnInstall.Click
        Dim processInfo As ProcessStartInfo
        Dim process As Process

        processInfo = New ProcessStartInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory & "\InstallUtil.exe", "/u dnsd.exe")
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = False
        process = process.Start(processInfo)
        process.WaitForExit(10000)
        process.Close()
    End Sub

    Private Sub menuItemNatInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemNatInstall.Click
        Dim processInfo As ProcessStartInfo
        Dim process As Process

        processInfo = New ProcessStartInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory & "\InstallUtil.exe", "natd.exe")
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = False
        process = process.Start(processInfo)
        process.WaitForExit(10000)
        process.Close()
    End Sub

    Private Sub menuItemNatUnInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemNatUnInstall.Click
        Dim processInfo As ProcessStartInfo
        Dim process As Process

        processInfo = New ProcessStartInfo(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory & "\InstallUtil.exe", "/u natd.exe")
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = False
        process = process.Start(processInfo)
        process.WaitForExit(10000)
        process.Close()
    End Sub

    Private Sub ChangeLanguage(ByVal lang As String)
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(frmMain))
        Dim ci As New CultureInfo(lang)

        resources.ApplyResources(Me, Me.Name, ci)

        For Each c As Control In Me.Controls
            resources.ApplyResources(c, c.Name, ci)
        Next c
    End Sub

    Private Sub LoadFormResources()
        Dim crm As ComponentResourceManager
        crm = New ComponentResourceManager(GetType(frmMain))

        For Each aControl As Control In Me.Controls
            crm.ApplyResources(aControl, aControl.Name)
        Next aControl

        crm.ApplyResources(menuItemBulgarian, menuItemBulgarian.Name)
    End Sub


    Private Sub menuItemEnglish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemEnglish.Click
        My.Settings.Language = "en-US"
        MessageBox.Show(My.Resources.YouMustRestartForChangesToTakeEffect, My.Resources.CaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub menuItemBulgarian_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemBulgarian.Click
        My.Settings.Language = "bg-BG"
        MessageBox.Show(My.Resources.YouMustRestartForChangesToTakeEffect, My.Resources.CaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub pageDHCP_btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageDHCP_btnRefresh.Click
        pageDHCP_lvLeases.Items.Clear()

        Dim ys As String()

        Dim ipAddr As String
        Dim hwAddr As String
        Dim Host As String
        Dim Lease As String

        For Each xs As String In remoteDhcpObject.GetClients()
            ys = Split(xs, ";")

            ipAddr = ys(0)
            hwAddr = ys(1)
            Host = ys(2)
            Lease = ys(3)

            pageDHCP_lvLeases.Items.Add(New ListViewItem(New String() {"", ipAddr, hwAddr, Host, Lease}))
            pageDHCP_lvLeases.Items(pageDHCP_lvLeases.Items.Count - 1).ImageIndex = 2
        Next
    End Sub

    Private Sub pageDNS_CacheRecords_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.Handled = Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar)
    End Sub

    Private Sub pageDNS_CacheRecords_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If String.IsNullOrEmpty(pageDNS_txtCacheRecords.Text) Then pageDNS_txtCacheRecords.Text = "0"
        If pageDNS_txtCacheRecords.Text < 100 Then pageDNS_cbEnableCache.Checked = False
    End Sub

    Private Sub pageDNS_btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageDNS_btnApply.Click
        modDnsFunctions.BuildConfig()
    End Sub


    Private Sub pageDNS_btnAddRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageDNS_btnAddRecord.Click
        ' Create config form
        Dim frmAddDnsRecordFrom As New frmAddDnsRecord

        ' Show config form
        If frmAddDnsRecordFrom.ShowDialog() = DialogResult.OK Then
            If frmAddDnsRecordFrom.txtIP.Text <> "...." AndAlso frmAddDnsRecordFrom.txtName.Text.Trim <> String.Empty Then
                pageDNS_lvDNS.Items.Add(New ListViewItem(New String() {"", frmAddDnsRecordFrom.txtIP.Text, frmAddDnsRecordFrom.txtName.Text}))
                pageDNS_lvDNS.Items(pageDNS_lvDNS.Items.Count - 1).ImageIndex = 3
            End If
        End If

        ' Clean
        frmAddDnsRecordFrom.Close()
        frmAddDnsRecordFrom.Dispose()
    End Sub

    Private Sub pageDNS_btnRemoveRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageDNS_btnRemoveRecord.Click
        pageDNS_lvDNS.SelectedItems.Item(0).Remove()
    End Sub

    Private Sub pageDNS_btnClearCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageDNS_btnClearCache.Click
        remoteDnsObject.ClearCache()
    End Sub

    Private Sub pageNAT_btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageNAT_btnApply.Click
        If pageNAT_lvExternal.SelectedItems.Count <> 1 OrElse pageNAT_lvInternal.SelectedItems.Count <> 1 Then
            MessageBox.Show(My.Resources.YouMustSelectOneInterface, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        If pageNAT_lvExternal.SelectedItems(0).SubItems(2).Text = pageNAT_lvInternal.SelectedItems(0).SubItems(2).Text Then
            MessageBox.Show(My.Resources.YouMustSelectDifferentInterfaces, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        modNatFunctions.SaveConfig()
    End Sub

    Private Sub pageFirewall_btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageFirewall_btnApply.Click
        modNatFunctions.SaveConfig()
    End Sub

    Private Sub pageTrafficShaping_btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageTrafficShaping_btnApply.Click
        modNatFunctions.SaveConfig()
    End Sub

    Private Sub pageNAT_cbEnableNAT_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageNAT_cbEnableNAT.CheckedChanged
        modNatFunctions.configData.optNAT = pageNAT_cbEnableNAT.Checked

        If pageNAT_cbEnableNAT.Checked Then
            Dim chain1 As New natConfigChain
            chain1.JUMP = "DNAT"
            chain1.sourceInterface = "RED"
            chain1.destinationInterface = "GREEN"
            modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).PREROUTING.Add(chain1)

            Dim chain2 As New natConfigChain
            chain2.JUMP = "SNAT"
            chain2.sourceInterface = "GREEN"
            chain2.destinationInterface = "RED"
            modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).POSTROUTING.Add(chain2)
        Else
            For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).POSTROUTING
                If chain.JUMP.ToUpper = "SNAT" Then
                    modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).POSTROUTING.Remove(chain)
                    Exit For
                End If
            Next

            For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).PREROUTING
                If chain.JUMP.ToUpper = "DNAT" Then
                    modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).PREROUTING.Remove(chain)
                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub pageNAT_cbEnableTTL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageNAT_cbEnableTTL.CheckedChanged
        modNatFunctions.configData.optTTL = Not pageNAT_cbEnableTTL.Checked

        If Not pageNAT_cbEnableTTL.Checked Then
            Dim chain As New natConfigChain
            chain.packetTTLInc = -1
            chain.JUMP = "TTL"
            modNatFunctions.configData.m_Table(natConfig.TABLE_MANGLE).FORWARD.Add(chain)
        Else
            For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_MANGLE).FORWARD
                If chain.packetTTLInc = -1 Then
                    modNatFunctions.configData.m_Table(natConfig.TABLE_MANGLE).FORWARD.Remove(chain)
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub pageTrafficShaping_btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageTrafficShaping_btnRemove.Click
        If pageTrafficShaping_lvRules.SelectedItems.Count > 0 Then
            Dim srcAddr As Net.IPAddress = Nothing
            Dim srcPort As Integer = 0
            Dim dstAddr As Net.IPAddress = Nothing
            Dim dstPort As Integer = 0
            Dim protocol As Protocol = NetGate.Protocol.NONE
            Dim speed As Integer = 0


            If pageTrafficShaping_lvRules.SelectedItems(0).SubItems(1).Text.ToLower <> "any" Then srcAddr = Net.IPAddress.Parse(pageTrafficShaping_lvRules.SelectedItems(0).SubItems(1).Text)
            If pageTrafficShaping_lvRules.SelectedItems(0).SubItems(2).Text.ToLower <> "any" Then srcPort = pageTrafficShaping_lvRules.SelectedItems(0).SubItems(2).Text
            If pageTrafficShaping_lvRules.SelectedItems(0).SubItems(3).Text.ToLower <> "any" Then dstAddr = Net.IPAddress.Parse(pageTrafficShaping_lvRules.SelectedItems(0).SubItems(3).Text)
            If pageTrafficShaping_lvRules.SelectedItems(0).SubItems(4).Text.ToLower <> "any" Then dstPort = pageTrafficShaping_lvRules.SelectedItems(0).SubItems(4).Text
            If pageTrafficShaping_lvRules.SelectedItems(0).SubItems(5).Text.ToLower <> "any" Then protocol = natConfig.ParseProtocol(pageTrafficShaping_lvRules.SelectedItems(0).SubItems(5).Text)
            speed = pageTrafficShaping_lvRules.SelectedItems(0).SubItems(6).Text

            For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).FORWARD
                If chain.JUMP <> "LIMIT" Then Continue For

                If Net.IPAddress.Equals(srcAddr, chain.sourceAddress) AndAlso _
                (srcPort = chain.sourcePort) AndAlso _
                Net.IPAddress.Equals(dstAddr, chain.destinationAddress) AndAlso _
                (dstPort = chain.destinationPort) AndAlso _
                (protocol = chain.packetProtocol) AndAlso _
                (speed = chain.packetLimit) Then
                    modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).FORWARD.Remove(chain)
                    Exit For
                End If
            Next

            pageTrafficShaping_lvRules.SelectedItems(0).Remove()
        End If
    End Sub

    Private Sub pageTrafficShaping_btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageTrafficShaping_btnAdd.Click
        If pageTrafficShaping_txtProtocol.Text = String.Empty Then
            MessageBox.Show(My.Resources.ThereIsEmptyField, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        Dim chain As New natConfigChain
        chain.packetLimit = pageTrafficShaping_txtLimit.Text * 1024
        chain.JUMP = "LIMIT"

        If pageTrafficShaping_txtSrcAddr.Text.ToLower <> "any" Then chain.sourceAddress = Net.IPAddress.Parse(pageTrafficShaping_txtSrcAddr.Text)
        If pageTrafficShaping_txtDstAddr.Text.ToLower <> "any" Then chain.destinationAddress = Net.IPAddress.Parse(pageTrafficShaping_txtDstAddr.Text)
        If pageTrafficShaping_txtSrcPort.Text.ToLower <> "any" Then chain.sourcePort = pageTrafficShaping_txtSrcPort.Text
        If pageTrafficShaping_txtDstPort.Text.ToLower <> "any" Then chain.destinationPort = pageTrafficShaping_txtDstPort.Text
        If pageTrafficShaping_txtProtocol.Text.ToLower <> "any" Then chain.packetProtocol = natConfig.ParseProtocol(pageTrafficShaping_txtProtocol.Text)

        pageTrafficShaping_lvRules.Items.Add(New ListViewItem(New String() {"", pageTrafficShaping_txtSrcAddr.Text, pageTrafficShaping_txtSrcPort.Text, _
                                                                                pageTrafficShaping_txtDstAddr.Text, pageTrafficShaping_txtDstPort.Text, _
                                                                                pageTrafficShaping_txtProtocol.Text, chain.packetLimit}))

        modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).FORWARD.Add(chain)
    End Sub

    Private Sub pageFirewall_btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageFirewall_btnRemove.Click
        If pageFirewall_lvRules.SelectedItems.Count > 0 Then
            Dim srcAddr As Net.IPAddress = Nothing
            Dim srcPort As Integer = 0
            Dim dstAddr As Net.IPAddress = Nothing
            Dim dstPort As Integer = 0
            Dim protocol As Protocol = NetGate.Protocol.NONE
            Dim JUMP As String = String.Empty


            If pageFirewall_lvRules.SelectedItems(0).SubItems(1).Text.ToLower <> "any" Then srcAddr = Net.IPAddress.Parse(pageFirewall_lvRules.SelectedItems(0).SubItems(1).Text)
            If pageFirewall_lvRules.SelectedItems(0).SubItems(2).Text.ToLower <> "any" Then srcPort = pageFirewall_lvRules.SelectedItems(0).SubItems(2).Text
            If pageFirewall_lvRules.SelectedItems(0).SubItems(3).Text.ToLower <> "any" Then dstAddr = Net.IPAddress.Parse(pageFirewall_lvRules.SelectedItems(0).SubItems(3).Text)
            If pageFirewall_lvRules.SelectedItems(0).SubItems(4).Text.ToLower <> "any" Then dstPort = pageFirewall_lvRules.SelectedItems(0).SubItems(4).Text
            If pageFirewall_lvRules.SelectedItems(0).SubItems(5).Text.ToLower <> "any" Then protocol = natConfig.ParseProtocol(pageFirewall_lvRules.SelectedItems(0).SubItems(5).Text)
            JUMP = pageFirewall_lvRules.SelectedItems(0).SubItems(6).Text

            For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_FILTER).FORWARD
                If Net.IPAddress.Equals(srcAddr, chain.sourceAddress) AndAlso _
                (srcPort = chain.sourcePort) AndAlso _
                Net.IPAddress.Equals(dstAddr, chain.destinationAddress) AndAlso _
                (dstPort = chain.destinationPort) AndAlso _
                (protocol = chain.packetProtocol) AndAlso _
                (JUMP = chain.JUMP) Then
                    modNatFunctions.configData.m_Table(natConfig.TABLE_FILTER).FORWARD.Remove(chain)
                    Exit For
                End If
            Next

            pageFirewall_lvRules.SelectedItems(0).Remove()
        End If
    End Sub

    Private Sub pageFirewall_btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pageFirewall_btnAdd.Click
        If pageFirewall_txtProtocol.Text = String.Empty Then
            MessageBox.Show(My.Resources.ThereIsEmptyField, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If
        If pageFirewall_txtAction.Text = String.Empty Then
            MessageBox.Show(My.Resources.ThereIsEmptyField, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        Dim chain As New natConfigChain
        chain.JUMP = pageFirewall_txtAction.Text

        If pageFirewall_txtSrcAddr.Text.ToLower <> "any" Then chain.sourceAddress = Net.IPAddress.Parse(pageFirewall_txtSrcAddr.Text)
        If pageFirewall_txtDstAddr.Text.ToLower <> "any" Then chain.destinationAddress = Net.IPAddress.Parse(pageFirewall_txtDstAddr.Text)
        If pageFirewall_txtSrcPort.Text.ToLower <> "any" Then chain.sourcePort = pageFirewall_txtSrcPort.Text
        If pageFirewall_txtDstPort.Text.ToLower <> "any" Then chain.destinationPort = pageFirewall_txtDstPort.Text
        If pageFirewall_txtProtocol.Text.ToLower <> "any" Then chain.packetProtocol = natConfig.ParseProtocol(pageFirewall_txtProtocol.Text)

        pageFirewall_lvRules.Items.Add(New ListViewItem(New String() {"", pageFirewall_txtSrcAddr.Text, pageFirewall_txtSrcPort.Text, _
                                                                          pageFirewall_txtDstAddr.Text, pageFirewall_txtDstPort.Text, _
                                                                          pageFirewall_txtProtocol.Text, chain.JUMP}))

        modNatFunctions.configData.m_Table(natConfig.TABLE_FILTER).FORWARD.Add(chain)
    End Sub

    Private Sub pageTrafficShaping_txtLimit_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageTrafficShaping_txtLimit.Validating
        Dim tmp As Integer
        If Not Integer.TryParse(pageTrafficShaping_txtLimit.Text, tmp) Then e.Cancel = True
    End Sub

    Private Sub pageTrafficShaping_txtSrcPort_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageTrafficShaping_txtSrcPort.Validating
        If pageTrafficShaping_txtSrcPort.Text <> "any" Then
            Dim tmp As Integer
            If Not Integer.TryParse(pageTrafficShaping_txtSrcPort.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageTrafficShaping_txtSrcAddr_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageTrafficShaping_txtSrcAddr.Validating
        If pageTrafficShaping_txtSrcAddr.Text <> "any" Then
            Dim tmp As Net.IPAddress
            If Not Net.IPAddress.TryParse(pageTrafficShaping_txtSrcAddr.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageTrafficShaping_txtDstAddr_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageTrafficShaping_txtDstAddr.Validating
        If pageTrafficShaping_txtDstAddr.Text <> "any" Then
            Dim tmp As Net.IPAddress
            If Not Net.IPAddress.TryParse(pageTrafficShaping_txtDstAddr.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageTrafficShaping_txtDstPort_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageTrafficShaping_txtDstPort.Validating
        If pageTrafficShaping_txtDstPort.Text <> "any" Then
            Dim tmp As Integer
            If Not Integer.TryParse(pageTrafficShaping_txtDstPort.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageFirewall_txtSrcPort_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageFirewall_txtSrcPort.Validating
        If pageFirewall_txtSrcPort.Text <> "any" Then
            Dim tmp As Integer
            If Not Integer.TryParse(pageFirewall_txtSrcPort.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageFirewall_txtDstPort_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageFirewall_txtDstPort.Validating
        If pageFirewall_txtDstPort.Text <> "any" Then
            Dim tmp As Integer
            If Not Integer.TryParse(pageFirewall_txtDstPort.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageFirewall_txtSrcAddr_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageFirewall_txtSrcAddr.Validating
        If pageFirewall_txtSrcAddr.Text <> "any" Then
            Dim tmp As Net.IPAddress
            If Not Net.IPAddress.TryParse(pageFirewall_txtSrcAddr.Text, tmp) Then e.Cancel = True
        End If
    End Sub

    Private Sub pageFirewall_txtDstAddr_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pageFirewall_txtDstAddr.Validating
        If pageFirewall_txtDstAddr.Text <> "any" Then
            Dim tmp As Net.IPAddress
            If Not Net.IPAddress.TryParse(pageFirewall_txtDstAddr.Text, tmp) Then e.Cancel = True
        End If
    End Sub
End Class

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


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.tcMainTab = New System.Windows.Forms.TabControl
        Me.tpStatus = New System.Windows.Forms.TabPage
        Me.pageStatus_lineCenter = New System.Windows.Forms.Panel
        Me.pageStatus_lineVertical = New System.Windows.Forms.Panel
        Me.pageStatus_gbServices = New System.Windows.Forms.GroupBox
        Me.pageStatus_lblNAT = New System.Windows.Forms.Label
        Me.pageStatus_lblDNS = New System.Windows.Forms.Label
        Me.pageStatus_lblDHCP = New System.Windows.Forms.Label
        Me.pageStatus_btnLocalServer = New System.Windows.Forms.Button
        Me.pageStatus_btnRefresh = New System.Windows.Forms.Button
        Me.pageStatus_btnStopNAT = New System.Windows.Forms.Button
        Me.pageStatus_btnStartNAT = New System.Windows.Forms.Button
        Me.pageStatus_btnStopDNS = New System.Windows.Forms.Button
        Me.pageStatus_btnStopDHCP = New System.Windows.Forms.Button
        Me.pageStatus_btnStartDNS = New System.Windows.Forms.Button
        Me.pageStatus_btnStartDHCP = New System.Windows.Forms.Button
        Me.pageStatus_lblInfo = New System.Windows.Forms.Label
        Me.pageStatus_imgSideImage = New System.Windows.Forms.PictureBox
        Me.pageStatus_lineHorizontal = New System.Windows.Forms.Panel
        Me.pageStatus_lblLogo2 = New System.Windows.Forms.Label
        Me.pageStatus_lblLogo1 = New System.Windows.Forms.Label
        Me.pageStatus_imgLogo = New System.Windows.Forms.PictureBox
        Me.pageStatus_imgEurope = New System.Windows.Forms.PictureBox
        Me.tpDHCP = New System.Windows.Forms.TabPage
        Me.pageDHCP_imgSideImage = New System.Windows.Forms.PictureBox
        Me.pageDHCP_gbStatus = New System.Windows.Forms.GroupBox
        Me.pageDHCP_lvLeases = New System.Windows.Forms.ListView
        Me.chLeasesImage = New System.Windows.Forms.ColumnHeader
        Me.chLeasesIPAddr = New System.Windows.Forms.ColumnHeader
        Me.chLeasesHwAddr = New System.Windows.Forms.ColumnHeader
        Me.chLeasesHostname = New System.Windows.Forms.ColumnHeader
        Me.chLeasesLeaseTime = New System.Windows.Forms.ColumnHeader
        Me.ImageListMain = New System.Windows.Forms.ImageList(Me.components)
        Me.pageDHCP_btnRefresh = New System.Windows.Forms.Button
        Me.pageDHCP_btnSetup = New System.Windows.Forms.Button
        Me.tpDNS = New System.Windows.Forms.TabPage
        Me.pageDNS_gbResolver = New System.Windows.Forms.GroupBox
        Me.pageDNS_lvDNS = New System.Windows.Forms.ListView
        Me.chDnsIcon = New System.Windows.Forms.ColumnHeader
        Me.chDnsIp = New System.Windows.Forms.ColumnHeader
        Me.chDnsRequest = New System.Windows.Forms.ColumnHeader
        Me.pageDNS_cbLeases = New System.Windows.Forms.CheckBox
        Me.pageDNS_btnRemoveRecord = New System.Windows.Forms.Button
        Me.pageDNS_btnAddRecord = New System.Windows.Forms.Button
        Me.pageDNS_btnApply = New System.Windows.Forms.Button
        Me.pageDNS_gbCaching = New System.Windows.Forms.GroupBox
        Me.pageDNS_txtCacheRecords = New System.Windows.Forms.TextBox
        Me.pageDNS_lblRecords = New System.Windows.Forms.Label
        Me.pageDNS_lblCacheSize = New System.Windows.Forms.Label
        Me.pageDNS_btnClearCache = New System.Windows.Forms.Button
        Me.pageDNS_cbEnableCache = New System.Windows.Forms.CheckBox
        Me.pageDNS_gbForwarding = New System.Windows.Forms.GroupBox
        Me.pageDNS_lblDNSInfo = New System.Windows.Forms.Label
        Me.pageDNS_txtDNS = New System.Windows.Forms.TextBox
        Me.pageDNS_rbUseSpecifiedDNS = New System.Windows.Forms.RadioButton
        Me.pageDNS_rbUseSystemDefaultDNS = New System.Windows.Forms.RadioButton
        Me.pageDNS_imgSideImage = New System.Windows.Forms.PictureBox
        Me.tpNAT = New System.Windows.Forms.TabPage
        Me.pageNAT_cbEnableTTL = New System.Windows.Forms.CheckBox
        Me.pageNAT_cbEnableNAT = New System.Windows.Forms.CheckBox
        Me.pageNAT_btnApply = New System.Windows.Forms.Button
        Me.pageNAT_gbRouting = New System.Windows.Forms.GroupBox
        Me.pageNAT_lvInternal = New System.Windows.Forms.ListView
        Me.chRoutingInIcon = New System.Windows.Forms.ColumnHeader
        Me.chRoutingInAdapterName = New System.Windows.Forms.ColumnHeader
        Me.chRoutingInAdapterInfo = New System.Windows.Forms.ColumnHeader
        Me.pageNAT_lblInternal = New System.Windows.Forms.Label
        Me.pageNAT_lblExternal = New System.Windows.Forms.Label
        Me.pageNAT_lvExternal = New System.Windows.Forms.ListView
        Me.chRoutingExIcon = New System.Windows.Forms.ColumnHeader
        Me.chRoutingExAdapterName = New System.Windows.Forms.ColumnHeader
        Me.chRoutingExAdapterInfo = New System.Windows.Forms.ColumnHeader
        Me.pageNAT_imgSideImage = New System.Windows.Forms.PictureBox
        Me.tpFirewall = New System.Windows.Forms.TabPage
        Me.pageFirewall_lblNote = New System.Windows.Forms.Label
        Me.pageFirewall_gbRules = New System.Windows.Forms.GroupBox
        Me.pageFirewall_lvRules = New System.Windows.Forms.ListView
        Me.chFirewallIcon = New System.Windows.Forms.ColumnHeader
        Me.chFirewallSrcAddr = New System.Windows.Forms.ColumnHeader
        Me.chFirewallSrcPort = New System.Windows.Forms.ColumnHeader
        Me.chFirewallDstAddr = New System.Windows.Forms.ColumnHeader
        Me.chFirewallDstPort = New System.Windows.Forms.ColumnHeader
        Me.Protocol = New System.Windows.Forms.ColumnHeader
        Me.Action = New System.Windows.Forms.ColumnHeader
        Me.pageFirewall_btnRemove = New System.Windows.Forms.Button
        Me.pageFirewall_gbNewRule = New System.Windows.Forms.GroupBox
        Me.pageFirewall_btnAdd = New System.Windows.Forms.Button
        Me.pageFirewall_lblSrcPort = New System.Windows.Forms.Label
        Me.pageFirewall_txtSrcPort = New System.Windows.Forms.ComboBox
        Me.pageFirewall_lblAction = New System.Windows.Forms.Label
        Me.pageFirewall_lblProtocol = New System.Windows.Forms.Label
        Me.pageFirewall_lblDstPort = New System.Windows.Forms.Label
        Me.pageFirewall_lblDstAddr = New System.Windows.Forms.Label
        Me.pageFirewall_lblSrcAddr = New System.Windows.Forms.Label
        Me.pageFirewall_txtAction = New System.Windows.Forms.ComboBox
        Me.pageFirewall_txtProtocol = New System.Windows.Forms.ComboBox
        Me.pageFirewall_txtDstPort = New System.Windows.Forms.ComboBox
        Me.pageFirewall_txtDstAddr = New System.Windows.Forms.ComboBox
        Me.pageFirewall_txtSrcAddr = New System.Windows.Forms.ComboBox
        Me.pageFirewall_btnApply = New System.Windows.Forms.Button
        Me.pageFirewall_imgSideImage = New System.Windows.Forms.PictureBox
        Me.tpTraffSh = New System.Windows.Forms.TabPage
        Me.pageTrafficShaping_lblNote = New System.Windows.Forms.Label
        Me.pageTrafficShaping_gbRules = New System.Windows.Forms.GroupBox
        Me.pageTrafficShaping_lvRules = New System.Windows.Forms.ListView
        Me.chTrafficShapingIcon = New System.Windows.Forms.ColumnHeader
        Me.chTrafficShapingSrcAddr = New System.Windows.Forms.ColumnHeader
        Me.chTrafficShapingSrcPort = New System.Windows.Forms.ColumnHeader
        Me.chTrafficShapingDstAddr = New System.Windows.Forms.ColumnHeader
        Me.chTrafficShapingDstPort = New System.Windows.Forms.ColumnHeader
        Me.chTrafficShapingProtocol = New System.Windows.Forms.ColumnHeader
        Me.chTrafficShapingLimit = New System.Windows.Forms.ColumnHeader
        Me.pageTrafficShaping_btnRemove = New System.Windows.Forms.Button
        Me.pageTrafficShaping_gbAddRule = New System.Windows.Forms.GroupBox
        Me.pageTrafficShaping_lblSrcPort = New System.Windows.Forms.Label
        Me.pageTrafficShaping_txtSrcPort = New System.Windows.Forms.ComboBox
        Me.pageTrafficShaping_lblLimit = New System.Windows.Forms.Label
        Me.pageTrafficShaping_btnAdd = New System.Windows.Forms.Button
        Me.pageTrafficShaping_lblProtocol = New System.Windows.Forms.Label
        Me.pageTrafficShaping_lblDstPort = New System.Windows.Forms.Label
        Me.pageTrafficShaping_lblDstAddr = New System.Windows.Forms.Label
        Me.pageTrafficShaping_lblSrcAddr = New System.Windows.Forms.Label
        Me.pageTrafficShaping_txtLimit = New System.Windows.Forms.ComboBox
        Me.pageTrafficShaping_txtProtocol = New System.Windows.Forms.ComboBox
        Me.pageTrafficShaping_txtDstPort = New System.Windows.Forms.ComboBox
        Me.pageTrafficShaping_txtDstAddr = New System.Windows.Forms.ComboBox
        Me.pageTrafficShaping_txtSrcAddr = New System.Windows.Forms.ComboBox
        Me.pageTrafficShaping_imgSideImage = New System.Windows.Forms.PictureBox
        Me.pageTrafficShaping_btnApply = New System.Windows.Forms.Button
        Me.msMainMenu = New System.Windows.Forms.MenuStrip
        Me.menuItemFile = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemOpenLocal = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemManageAccess = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemS3 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemOpenRemote = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemS2 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemDisconnect = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemToTray = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemExit = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemTools = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemS6 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemDhcpInstall = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemDhcpUnInstall = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemS4 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemDnsInstall = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemDnsUnInstall = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemS5 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemNatInstall = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemNatUnInstall = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemPlugins = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemLanguage = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemEnglish = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemBulgarian = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemContents = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemIndex = New System.Windows.Forms.ToolStripMenuItem
        Me.menuItemS1 = New System.Windows.Forms.ToolStripSeparator
        Me.menuItemAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTipMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.NotifyIconContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.notifyMenuItemShow = New System.Windows.Forms.ToolStripMenuItem
        Me.notifyMenuItemExit = New System.Windows.Forms.ToolStripMenuItem
        Me.NotifyIconMain = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ServiceControllerDHCP = New System.ServiceProcess.ServiceController
        Me.TimerCheckServices = New System.Windows.Forms.Timer(Me.components)
        Me.ServiceControllerDNS = New System.ServiceProcess.ServiceController
        Me.ServiceControllerNAT = New System.ServiceProcess.ServiceController
        Me.tcMainTab.SuspendLayout()
        Me.tpStatus.SuspendLayout()
        Me.pageStatus_gbServices.SuspendLayout()
        CType(Me.pageStatus_imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pageStatus_imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pageStatus_imgEurope, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpDHCP.SuspendLayout()
        CType(Me.pageDHCP_imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pageDHCP_gbStatus.SuspendLayout()
        Me.tpDNS.SuspendLayout()
        Me.pageDNS_gbResolver.SuspendLayout()
        Me.pageDNS_gbCaching.SuspendLayout()
        Me.pageDNS_gbForwarding.SuspendLayout()
        CType(Me.pageDNS_imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpNAT.SuspendLayout()
        Me.pageNAT_gbRouting.SuspendLayout()
        CType(Me.pageNAT_imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpFirewall.SuspendLayout()
        Me.pageFirewall_gbRules.SuspendLayout()
        Me.pageFirewall_gbNewRule.SuspendLayout()
        CType(Me.pageFirewall_imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpTraffSh.SuspendLayout()
        Me.pageTrafficShaping_gbRules.SuspendLayout()
        Me.pageTrafficShaping_gbAddRule.SuspendLayout()
        CType(Me.pageTrafficShaping_imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.msMainMenu.SuspendLayout()
        Me.NotifyIconContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcMainTab
        '
        resources.ApplyResources(Me.tcMainTab, "tcMainTab")
        Me.tcMainTab.Controls.Add(Me.tpStatus)
        Me.tcMainTab.Controls.Add(Me.tpDHCP)
        Me.tcMainTab.Controls.Add(Me.tpDNS)
        Me.tcMainTab.Controls.Add(Me.tpNAT)
        Me.tcMainTab.Controls.Add(Me.tpFirewall)
        Me.tcMainTab.Controls.Add(Me.tpTraffSh)
        Me.tcMainTab.Multiline = True
        Me.tcMainTab.Name = "tcMainTab"
        Me.tcMainTab.SelectedIndex = 0
        '
        'tpStatus
        '
        Me.tpStatus.Controls.Add(Me.pageStatus_lineCenter)
        Me.tpStatus.Controls.Add(Me.pageStatus_lineVertical)
        Me.tpStatus.Controls.Add(Me.pageStatus_gbServices)
        Me.tpStatus.Controls.Add(Me.pageStatus_lineHorizontal)
        Me.tpStatus.Controls.Add(Me.pageStatus_lblLogo2)
        Me.tpStatus.Controls.Add(Me.pageStatus_lblLogo1)
        Me.tpStatus.Controls.Add(Me.pageStatus_imgLogo)
        Me.tpStatus.Controls.Add(Me.pageStatus_imgEurope)
        resources.ApplyResources(Me.tpStatus, "tpStatus")
        Me.tpStatus.Name = "tpStatus"
        Me.tpStatus.UseVisualStyleBackColor = True
        '
        'pageStatus_lineCenter
        '
        Me.pageStatus_lineCenter.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        resources.ApplyResources(Me.pageStatus_lineCenter, "pageStatus_lineCenter")
        Me.pageStatus_lineCenter.Name = "pageStatus_lineCenter"
        '
        'pageStatus_lineVertical
        '
        Me.pageStatus_lineVertical.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        resources.ApplyResources(Me.pageStatus_lineVertical, "pageStatus_lineVertical")
        Me.pageStatus_lineVertical.Name = "pageStatus_lineVertical"
        '
        'pageStatus_gbServices
        '
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_lblNAT)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_lblDNS)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_lblDHCP)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnLocalServer)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnRefresh)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnStopNAT)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnStartNAT)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnStopDNS)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnStopDHCP)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnStartDNS)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_btnStartDHCP)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_lblInfo)
        Me.pageStatus_gbServices.Controls.Add(Me.pageStatus_imgSideImage)
        resources.ApplyResources(Me.pageStatus_gbServices, "pageStatus_gbServices")
        Me.pageStatus_gbServices.Name = "pageStatus_gbServices"
        Me.pageStatus_gbServices.TabStop = False
        '
        'pageStatus_lblNAT
        '
        resources.ApplyResources(Me.pageStatus_lblNAT, "pageStatus_lblNAT")
        Me.pageStatus_lblNAT.ForeColor = System.Drawing.Color.Gray
        Me.pageStatus_lblNAT.Name = "pageStatus_lblNAT"
        '
        'pageStatus_lblDNS
        '
        resources.ApplyResources(Me.pageStatus_lblDNS, "pageStatus_lblDNS")
        Me.pageStatus_lblDNS.ForeColor = System.Drawing.Color.Gray
        Me.pageStatus_lblDNS.Name = "pageStatus_lblDNS"
        '
        'pageStatus_lblDHCP
        '
        resources.ApplyResources(Me.pageStatus_lblDHCP, "pageStatus_lblDHCP")
        Me.pageStatus_lblDHCP.ForeColor = System.Drawing.Color.Gray
        Me.pageStatus_lblDHCP.Name = "pageStatus_lblDHCP"
        '
        'pageStatus_btnLocalServer
        '
        resources.ApplyResources(Me.pageStatus_btnLocalServer, "pageStatus_btnLocalServer")
        Me.pageStatus_btnLocalServer.Name = "pageStatus_btnLocalServer"
        Me.pageStatus_btnLocalServer.UseVisualStyleBackColor = True
        '
        'pageStatus_btnRefresh
        '
        resources.ApplyResources(Me.pageStatus_btnRefresh, "pageStatus_btnRefresh")
        Me.pageStatus_btnRefresh.Name = "pageStatus_btnRefresh"
        Me.pageStatus_btnRefresh.UseVisualStyleBackColor = True
        '
        'pageStatus_btnStopNAT
        '
        resources.ApplyResources(Me.pageStatus_btnStopNAT, "pageStatus_btnStopNAT")
        Me.pageStatus_btnStopNAT.Name = "pageStatus_btnStopNAT"
        Me.pageStatus_btnStopNAT.UseVisualStyleBackColor = True
        '
        'pageStatus_btnStartNAT
        '
        resources.ApplyResources(Me.pageStatus_btnStartNAT, "pageStatus_btnStartNAT")
        Me.pageStatus_btnStartNAT.Name = "pageStatus_btnStartNAT"
        Me.pageStatus_btnStartNAT.UseVisualStyleBackColor = True
        '
        'pageStatus_btnStopDNS
        '
        resources.ApplyResources(Me.pageStatus_btnStopDNS, "pageStatus_btnStopDNS")
        Me.pageStatus_btnStopDNS.Name = "pageStatus_btnStopDNS"
        Me.pageStatus_btnStopDNS.UseVisualStyleBackColor = True
        '
        'pageStatus_btnStopDHCP
        '
        resources.ApplyResources(Me.pageStatus_btnStopDHCP, "pageStatus_btnStopDHCP")
        Me.pageStatus_btnStopDHCP.Name = "pageStatus_btnStopDHCP"
        Me.pageStatus_btnStopDHCP.UseVisualStyleBackColor = True
        '
        'pageStatus_btnStartDNS
        '
        resources.ApplyResources(Me.pageStatus_btnStartDNS, "pageStatus_btnStartDNS")
        Me.pageStatus_btnStartDNS.Name = "pageStatus_btnStartDNS"
        Me.pageStatus_btnStartDNS.UseVisualStyleBackColor = True
        '
        'pageStatus_btnStartDHCP
        '
        resources.ApplyResources(Me.pageStatus_btnStartDHCP, "pageStatus_btnStartDHCP")
        Me.pageStatus_btnStartDHCP.Name = "pageStatus_btnStartDHCP"
        Me.pageStatus_btnStartDHCP.UseVisualStyleBackColor = True
        '
        'pageStatus_lblInfo
        '
        resources.ApplyResources(Me.pageStatus_lblInfo, "pageStatus_lblInfo")
        Me.pageStatus_lblInfo.Name = "pageStatus_lblInfo"
        '
        'pageStatus_imgSideImage
        '
        resources.ApplyResources(Me.pageStatus_imgSideImage, "pageStatus_imgSideImage")
        Me.pageStatus_imgSideImage.Name = "pageStatus_imgSideImage"
        Me.pageStatus_imgSideImage.TabStop = False
        '
        'pageStatus_lineHorizontal
        '
        Me.pageStatus_lineHorizontal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        resources.ApplyResources(Me.pageStatus_lineHorizontal, "pageStatus_lineHorizontal")
        Me.pageStatus_lineHorizontal.Name = "pageStatus_lineHorizontal"
        '
        'pageStatus_lblLogo2
        '
        resources.ApplyResources(Me.pageStatus_lblLogo2, "pageStatus_lblLogo2")
        Me.pageStatus_lblLogo2.ForeColor = System.Drawing.Color.Navy
        Me.pageStatus_lblLogo2.Name = "pageStatus_lblLogo2"
        '
        'pageStatus_lblLogo1
        '
        resources.ApplyResources(Me.pageStatus_lblLogo1, "pageStatus_lblLogo1")
        Me.pageStatus_lblLogo1.ForeColor = System.Drawing.Color.Navy
        Me.pageStatus_lblLogo1.Name = "pageStatus_lblLogo1"
        '
        'pageStatus_imgLogo
        '
        resources.ApplyResources(Me.pageStatus_imgLogo, "pageStatus_imgLogo")
        Me.pageStatus_imgLogo.Name = "pageStatus_imgLogo"
        Me.pageStatus_imgLogo.TabStop = False
        '
        'pageStatus_imgEurope
        '
        resources.ApplyResources(Me.pageStatus_imgEurope, "pageStatus_imgEurope")
        Me.pageStatus_imgEurope.Name = "pageStatus_imgEurope"
        Me.pageStatus_imgEurope.TabStop = False
        '
        'tpDHCP
        '
        Me.tpDHCP.Controls.Add(Me.pageDHCP_imgSideImage)
        Me.tpDHCP.Controls.Add(Me.pageDHCP_gbStatus)
        Me.tpDHCP.Controls.Add(Me.pageDHCP_btnRefresh)
        Me.tpDHCP.Controls.Add(Me.pageDHCP_btnSetup)
        resources.ApplyResources(Me.tpDHCP, "tpDHCP")
        Me.tpDHCP.Name = "tpDHCP"
        Me.tpDHCP.UseVisualStyleBackColor = True
        '
        'pageDHCP_imgSideImage
        '
        resources.ApplyResources(Me.pageDHCP_imgSideImage, "pageDHCP_imgSideImage")
        Me.pageDHCP_imgSideImage.Name = "pageDHCP_imgSideImage"
        Me.pageDHCP_imgSideImage.TabStop = False
        '
        'pageDHCP_gbStatus
        '
        Me.pageDHCP_gbStatus.Controls.Add(Me.pageDHCP_lvLeases)
        resources.ApplyResources(Me.pageDHCP_gbStatus, "pageDHCP_gbStatus")
        Me.pageDHCP_gbStatus.Name = "pageDHCP_gbStatus"
        Me.pageDHCP_gbStatus.TabStop = False
        '
        'pageDHCP_lvLeases
        '
        Me.pageDHCP_lvLeases.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chLeasesImage, Me.chLeasesIPAddr, Me.chLeasesHwAddr, Me.chLeasesHostname, Me.chLeasesLeaseTime})
        Me.pageDHCP_lvLeases.FullRowSelect = True
        Me.pageDHCP_lvLeases.GridLines = True
        Me.pageDHCP_lvLeases.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        resources.ApplyResources(Me.pageDHCP_lvLeases, "pageDHCP_lvLeases")
        Me.pageDHCP_lvLeases.MultiSelect = False
        Me.pageDHCP_lvLeases.Name = "pageDHCP_lvLeases"
        Me.pageDHCP_lvLeases.SmallImageList = Me.ImageListMain
        Me.pageDHCP_lvLeases.UseCompatibleStateImageBehavior = False
        Me.pageDHCP_lvLeases.View = System.Windows.Forms.View.Details
        '
        'chLeasesImage
        '
        resources.ApplyResources(Me.chLeasesImage, "chLeasesImage")
        '
        'chLeasesIPAddr
        '
        resources.ApplyResources(Me.chLeasesIPAddr, "chLeasesIPAddr")
        '
        'chLeasesHwAddr
        '
        resources.ApplyResources(Me.chLeasesHwAddr, "chLeasesHwAddr")
        '
        'chLeasesHostname
        '
        resources.ApplyResources(Me.chLeasesHostname, "chLeasesHostname")
        '
        'chLeasesLeaseTime
        '
        resources.ApplyResources(Me.chLeasesLeaseTime, "chLeasesLeaseTime")
        '
        'ImageListMain
        '
        Me.ImageListMain.ImageStream = CType(resources.GetObject("ImageListMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListMain.Images.SetKeyName(0, "encrypted.png")
        Me.ImageListMain.Images.SetKeyName(1, "decrypted.png")
        Me.ImageListMain.Images.SetKeyName(2, "gohome.png")
        Me.ImageListMain.Images.SetKeyName(3, "middle.png")
        '
        'pageDHCP_btnRefresh
        '
        resources.ApplyResources(Me.pageDHCP_btnRefresh, "pageDHCP_btnRefresh")
        Me.pageDHCP_btnRefresh.Name = "pageDHCP_btnRefresh"
        Me.pageDHCP_btnRefresh.UseVisualStyleBackColor = True
        '
        'pageDHCP_btnSetup
        '
        resources.ApplyResources(Me.pageDHCP_btnSetup, "pageDHCP_btnSetup")
        Me.pageDHCP_btnSetup.Name = "pageDHCP_btnSetup"
        Me.pageDHCP_btnSetup.UseVisualStyleBackColor = True
        '
        'tpDNS
        '
        Me.tpDNS.Controls.Add(Me.pageDNS_gbResolver)
        Me.tpDNS.Controls.Add(Me.pageDNS_btnApply)
        Me.tpDNS.Controls.Add(Me.pageDNS_gbCaching)
        Me.tpDNS.Controls.Add(Me.pageDNS_gbForwarding)
        Me.tpDNS.Controls.Add(Me.pageDNS_imgSideImage)
        resources.ApplyResources(Me.tpDNS, "tpDNS")
        Me.tpDNS.Name = "tpDNS"
        Me.tpDNS.UseVisualStyleBackColor = True
        '
        'pageDNS_gbResolver
        '
        Me.pageDNS_gbResolver.Controls.Add(Me.pageDNS_lvDNS)
        Me.pageDNS_gbResolver.Controls.Add(Me.pageDNS_cbLeases)
        Me.pageDNS_gbResolver.Controls.Add(Me.pageDNS_btnRemoveRecord)
        Me.pageDNS_gbResolver.Controls.Add(Me.pageDNS_btnAddRecord)
        resources.ApplyResources(Me.pageDNS_gbResolver, "pageDNS_gbResolver")
        Me.pageDNS_gbResolver.Name = "pageDNS_gbResolver"
        Me.pageDNS_gbResolver.TabStop = False
        '
        'pageDNS_lvDNS
        '
        Me.pageDNS_lvDNS.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chDnsIcon, Me.chDnsIp, Me.chDnsRequest})
        Me.pageDNS_lvDNS.FullRowSelect = True
        Me.pageDNS_lvDNS.GridLines = True
        Me.pageDNS_lvDNS.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        resources.ApplyResources(Me.pageDNS_lvDNS, "pageDNS_lvDNS")
        Me.pageDNS_lvDNS.MultiSelect = False
        Me.pageDNS_lvDNS.Name = "pageDNS_lvDNS"
        Me.pageDNS_lvDNS.SmallImageList = Me.ImageListMain
        Me.pageDNS_lvDNS.UseCompatibleStateImageBehavior = False
        Me.pageDNS_lvDNS.View = System.Windows.Forms.View.Details
        '
        'chDnsIcon
        '
        resources.ApplyResources(Me.chDnsIcon, "chDnsIcon")
        '
        'chDnsIp
        '
        resources.ApplyResources(Me.chDnsIp, "chDnsIp")
        '
        'chDnsRequest
        '
        resources.ApplyResources(Me.chDnsRequest, "chDnsRequest")
        '
        'pageDNS_cbLeases
        '
        resources.ApplyResources(Me.pageDNS_cbLeases, "pageDNS_cbLeases")
        Me.pageDNS_cbLeases.Name = "pageDNS_cbLeases"
        Me.pageDNS_cbLeases.UseVisualStyleBackColor = True
        '
        'pageDNS_btnRemoveRecord
        '
        resources.ApplyResources(Me.pageDNS_btnRemoveRecord, "pageDNS_btnRemoveRecord")
        Me.pageDNS_btnRemoveRecord.Name = "pageDNS_btnRemoveRecord"
        Me.pageDNS_btnRemoveRecord.UseVisualStyleBackColor = True
        '
        'pageDNS_btnAddRecord
        '
        resources.ApplyResources(Me.pageDNS_btnAddRecord, "pageDNS_btnAddRecord")
        Me.pageDNS_btnAddRecord.Name = "pageDNS_btnAddRecord"
        Me.pageDNS_btnAddRecord.UseVisualStyleBackColor = True
        '
        'pageDNS_btnApply
        '
        resources.ApplyResources(Me.pageDNS_btnApply, "pageDNS_btnApply")
        Me.pageDNS_btnApply.Name = "pageDNS_btnApply"
        Me.pageDNS_btnApply.UseVisualStyleBackColor = True
        '
        'pageDNS_gbCaching
        '
        Me.pageDNS_gbCaching.Controls.Add(Me.pageDNS_txtCacheRecords)
        Me.pageDNS_gbCaching.Controls.Add(Me.pageDNS_lblRecords)
        Me.pageDNS_gbCaching.Controls.Add(Me.pageDNS_lblCacheSize)
        Me.pageDNS_gbCaching.Controls.Add(Me.pageDNS_btnClearCache)
        Me.pageDNS_gbCaching.Controls.Add(Me.pageDNS_cbEnableCache)
        resources.ApplyResources(Me.pageDNS_gbCaching, "pageDNS_gbCaching")
        Me.pageDNS_gbCaching.Name = "pageDNS_gbCaching"
        Me.pageDNS_gbCaching.TabStop = False
        '
        'pageDNS_txtCacheRecords
        '
        resources.ApplyResources(Me.pageDNS_txtCacheRecords, "pageDNS_txtCacheRecords")
        Me.pageDNS_txtCacheRecords.Name = "pageDNS_txtCacheRecords"
        '
        'pageDNS_lblRecords
        '
        resources.ApplyResources(Me.pageDNS_lblRecords, "pageDNS_lblRecords")
        Me.pageDNS_lblRecords.Name = "pageDNS_lblRecords"
        '
        'pageDNS_lblCacheSize
        '
        resources.ApplyResources(Me.pageDNS_lblCacheSize, "pageDNS_lblCacheSize")
        Me.pageDNS_lblCacheSize.Name = "pageDNS_lblCacheSize"
        '
        'pageDNS_btnClearCache
        '
        resources.ApplyResources(Me.pageDNS_btnClearCache, "pageDNS_btnClearCache")
        Me.pageDNS_btnClearCache.Name = "pageDNS_btnClearCache"
        Me.pageDNS_btnClearCache.UseVisualStyleBackColor = True
        '
        'pageDNS_cbEnableCache
        '
        resources.ApplyResources(Me.pageDNS_cbEnableCache, "pageDNS_cbEnableCache")
        Me.pageDNS_cbEnableCache.Checked = True
        Me.pageDNS_cbEnableCache.CheckState = System.Windows.Forms.CheckState.Checked
        Me.pageDNS_cbEnableCache.Name = "pageDNS_cbEnableCache"
        Me.pageDNS_cbEnableCache.UseVisualStyleBackColor = True
        '
        'pageDNS_gbForwarding
        '
        Me.pageDNS_gbForwarding.Controls.Add(Me.pageDNS_lblDNSInfo)
        Me.pageDNS_gbForwarding.Controls.Add(Me.pageDNS_txtDNS)
        Me.pageDNS_gbForwarding.Controls.Add(Me.pageDNS_rbUseSpecifiedDNS)
        Me.pageDNS_gbForwarding.Controls.Add(Me.pageDNS_rbUseSystemDefaultDNS)
        resources.ApplyResources(Me.pageDNS_gbForwarding, "pageDNS_gbForwarding")
        Me.pageDNS_gbForwarding.Name = "pageDNS_gbForwarding"
        Me.pageDNS_gbForwarding.TabStop = False
        '
        'pageDNS_lblDNSInfo
        '
        resources.ApplyResources(Me.pageDNS_lblDNSInfo, "pageDNS_lblDNSInfo")
        Me.pageDNS_lblDNSInfo.ForeColor = System.Drawing.SystemColors.GrayText
        Me.pageDNS_lblDNSInfo.Name = "pageDNS_lblDNSInfo"
        '
        'pageDNS_txtDNS
        '
        resources.ApplyResources(Me.pageDNS_txtDNS, "pageDNS_txtDNS")
        Me.pageDNS_txtDNS.Name = "pageDNS_txtDNS"
        '
        'pageDNS_rbUseSpecifiedDNS
        '
        resources.ApplyResources(Me.pageDNS_rbUseSpecifiedDNS, "pageDNS_rbUseSpecifiedDNS")
        Me.pageDNS_rbUseSpecifiedDNS.Name = "pageDNS_rbUseSpecifiedDNS"
        Me.pageDNS_rbUseSpecifiedDNS.UseVisualStyleBackColor = True
        '
        'pageDNS_rbUseSystemDefaultDNS
        '
        resources.ApplyResources(Me.pageDNS_rbUseSystemDefaultDNS, "pageDNS_rbUseSystemDefaultDNS")
        Me.pageDNS_rbUseSystemDefaultDNS.Checked = True
        Me.pageDNS_rbUseSystemDefaultDNS.Name = "pageDNS_rbUseSystemDefaultDNS"
        Me.pageDNS_rbUseSystemDefaultDNS.TabStop = True
        Me.pageDNS_rbUseSystemDefaultDNS.UseVisualStyleBackColor = True
        '
        'pageDNS_imgSideImage
        '
        resources.ApplyResources(Me.pageDNS_imgSideImage, "pageDNS_imgSideImage")
        Me.pageDNS_imgSideImage.Name = "pageDNS_imgSideImage"
        Me.pageDNS_imgSideImage.TabStop = False
        '
        'tpNAT
        '
        Me.tpNAT.Controls.Add(Me.pageNAT_cbEnableTTL)
        Me.tpNAT.Controls.Add(Me.pageNAT_cbEnableNAT)
        Me.tpNAT.Controls.Add(Me.pageNAT_btnApply)
        Me.tpNAT.Controls.Add(Me.pageNAT_gbRouting)
        Me.tpNAT.Controls.Add(Me.pageNAT_imgSideImage)
        resources.ApplyResources(Me.tpNAT, "tpNAT")
        Me.tpNAT.Name = "tpNAT"
        Me.tpNAT.UseVisualStyleBackColor = True
        '
        'pageNAT_cbEnableTTL
        '
        resources.ApplyResources(Me.pageNAT_cbEnableTTL, "pageNAT_cbEnableTTL")
        Me.pageNAT_cbEnableTTL.Name = "pageNAT_cbEnableTTL"
        Me.pageNAT_cbEnableTTL.UseVisualStyleBackColor = True
        '
        'pageNAT_cbEnableNAT
        '
        resources.ApplyResources(Me.pageNAT_cbEnableNAT, "pageNAT_cbEnableNAT")
        Me.pageNAT_cbEnableNAT.Checked = True
        Me.pageNAT_cbEnableNAT.CheckState = System.Windows.Forms.CheckState.Checked
        Me.pageNAT_cbEnableNAT.Name = "pageNAT_cbEnableNAT"
        Me.pageNAT_cbEnableNAT.UseVisualStyleBackColor = True
        '
        'pageNAT_btnApply
        '
        resources.ApplyResources(Me.pageNAT_btnApply, "pageNAT_btnApply")
        Me.pageNAT_btnApply.Name = "pageNAT_btnApply"
        Me.pageNAT_btnApply.UseVisualStyleBackColor = True
        '
        'pageNAT_gbRouting
        '
        Me.pageNAT_gbRouting.Controls.Add(Me.pageNAT_lvInternal)
        Me.pageNAT_gbRouting.Controls.Add(Me.pageNAT_lblInternal)
        Me.pageNAT_gbRouting.Controls.Add(Me.pageNAT_lblExternal)
        Me.pageNAT_gbRouting.Controls.Add(Me.pageNAT_lvExternal)
        resources.ApplyResources(Me.pageNAT_gbRouting, "pageNAT_gbRouting")
        Me.pageNAT_gbRouting.Name = "pageNAT_gbRouting"
        Me.pageNAT_gbRouting.TabStop = False
        '
        'pageNAT_lvInternal
        '
        Me.pageNAT_lvInternal.BackColor = System.Drawing.Color.Honeydew
        Me.pageNAT_lvInternal.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chRoutingInIcon, Me.chRoutingInAdapterName, Me.chRoutingInAdapterInfo})
        Me.pageNAT_lvInternal.FullRowSelect = True
        Me.pageNAT_lvInternal.GridLines = True
        Me.pageNAT_lvInternal.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.pageNAT_lvInternal.HideSelection = False
        resources.ApplyResources(Me.pageNAT_lvInternal, "pageNAT_lvInternal")
        Me.pageNAT_lvInternal.MultiSelect = False
        Me.pageNAT_lvInternal.Name = "pageNAT_lvInternal"
        Me.pageNAT_lvInternal.UseCompatibleStateImageBehavior = False
        Me.pageNAT_lvInternal.View = System.Windows.Forms.View.Details
        '
        'chRoutingInIcon
        '
        resources.ApplyResources(Me.chRoutingInIcon, "chRoutingInIcon")
        '
        'chRoutingInAdapterName
        '
        resources.ApplyResources(Me.chRoutingInAdapterName, "chRoutingInAdapterName")
        '
        'chRoutingInAdapterInfo
        '
        resources.ApplyResources(Me.chRoutingInAdapterInfo, "chRoutingInAdapterInfo")
        '
        'pageNAT_lblInternal
        '
        resources.ApplyResources(Me.pageNAT_lblInternal, "pageNAT_lblInternal")
        Me.pageNAT_lblInternal.Name = "pageNAT_lblInternal"
        '
        'pageNAT_lblExternal
        '
        resources.ApplyResources(Me.pageNAT_lblExternal, "pageNAT_lblExternal")
        Me.pageNAT_lblExternal.Name = "pageNAT_lblExternal"
        '
        'pageNAT_lvExternal
        '
        Me.pageNAT_lvExternal.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.pageNAT_lvExternal.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chRoutingExIcon, Me.chRoutingExAdapterName, Me.chRoutingExAdapterInfo})
        Me.pageNAT_lvExternal.FullRowSelect = True
        Me.pageNAT_lvExternal.GridLines = True
        Me.pageNAT_lvExternal.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.pageNAT_lvExternal.HideSelection = False
        resources.ApplyResources(Me.pageNAT_lvExternal, "pageNAT_lvExternal")
        Me.pageNAT_lvExternal.MultiSelect = False
        Me.pageNAT_lvExternal.Name = "pageNAT_lvExternal"
        Me.pageNAT_lvExternal.UseCompatibleStateImageBehavior = False
        Me.pageNAT_lvExternal.View = System.Windows.Forms.View.Details
        '
        'chRoutingExIcon
        '
        resources.ApplyResources(Me.chRoutingExIcon, "chRoutingExIcon")
        '
        'chRoutingExAdapterName
        '
        resources.ApplyResources(Me.chRoutingExAdapterName, "chRoutingExAdapterName")
        '
        'chRoutingExAdapterInfo
        '
        resources.ApplyResources(Me.chRoutingExAdapterInfo, "chRoutingExAdapterInfo")
        '
        'pageNAT_imgSideImage
        '
        resources.ApplyResources(Me.pageNAT_imgSideImage, "pageNAT_imgSideImage")
        Me.pageNAT_imgSideImage.Name = "pageNAT_imgSideImage"
        Me.pageNAT_imgSideImage.TabStop = False
        '
        'tpFirewall
        '
        Me.tpFirewall.Controls.Add(Me.pageFirewall_lblNote)
        Me.tpFirewall.Controls.Add(Me.pageFirewall_gbRules)
        Me.tpFirewall.Controls.Add(Me.pageFirewall_gbNewRule)
        Me.tpFirewall.Controls.Add(Me.pageFirewall_btnApply)
        Me.tpFirewall.Controls.Add(Me.pageFirewall_imgSideImage)
        resources.ApplyResources(Me.tpFirewall, "tpFirewall")
        Me.tpFirewall.Name = "tpFirewall"
        Me.tpFirewall.UseVisualStyleBackColor = True
        '
        'pageFirewall_lblNote
        '
        Me.pageFirewall_lblNote.ForeColor = System.Drawing.Color.Gray
        resources.ApplyResources(Me.pageFirewall_lblNote, "pageFirewall_lblNote")
        Me.pageFirewall_lblNote.Name = "pageFirewall_lblNote"
        '
        'pageFirewall_gbRules
        '
        Me.pageFirewall_gbRules.Controls.Add(Me.pageFirewall_lvRules)
        Me.pageFirewall_gbRules.Controls.Add(Me.pageFirewall_btnRemove)
        resources.ApplyResources(Me.pageFirewall_gbRules, "pageFirewall_gbRules")
        Me.pageFirewall_gbRules.Name = "pageFirewall_gbRules"
        Me.pageFirewall_gbRules.TabStop = False
        '
        'pageFirewall_lvRules
        '
        Me.pageFirewall_lvRules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chFirewallIcon, Me.chFirewallSrcAddr, Me.chFirewallSrcPort, Me.chFirewallDstAddr, Me.chFirewallDstPort, Me.Protocol, Me.Action})
        Me.pageFirewall_lvRules.FullRowSelect = True
        Me.pageFirewall_lvRules.GridLines = True
        Me.pageFirewall_lvRules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        resources.ApplyResources(Me.pageFirewall_lvRules, "pageFirewall_lvRules")
        Me.pageFirewall_lvRules.MultiSelect = False
        Me.pageFirewall_lvRules.Name = "pageFirewall_lvRules"
        Me.pageFirewall_lvRules.SmallImageList = Me.ImageListMain
        Me.pageFirewall_lvRules.UseCompatibleStateImageBehavior = False
        Me.pageFirewall_lvRules.View = System.Windows.Forms.View.Details
        '
        'chFirewallIcon
        '
        resources.ApplyResources(Me.chFirewallIcon, "chFirewallIcon")
        '
        'chFirewallSrcAddr
        '
        resources.ApplyResources(Me.chFirewallSrcAddr, "chFirewallSrcAddr")
        '
        'chFirewallSrcPort
        '
        resources.ApplyResources(Me.chFirewallSrcPort, "chFirewallSrcPort")
        '
        'chFirewallDstAddr
        '
        resources.ApplyResources(Me.chFirewallDstAddr, "chFirewallDstAddr")
        '
        'chFirewallDstPort
        '
        resources.ApplyResources(Me.chFirewallDstPort, "chFirewallDstPort")
        '
        'Protocol
        '
        resources.ApplyResources(Me.Protocol, "Protocol")
        '
        'Action
        '
        resources.ApplyResources(Me.Action, "Action")
        '
        'pageFirewall_btnRemove
        '
        resources.ApplyResources(Me.pageFirewall_btnRemove, "pageFirewall_btnRemove")
        Me.pageFirewall_btnRemove.Name = "pageFirewall_btnRemove"
        Me.ToolTipMain.SetToolTip(Me.pageFirewall_btnRemove, resources.GetString("pageFirewall_btnRemove.ToolTip"))
        Me.pageFirewall_btnRemove.UseVisualStyleBackColor = True
        '
        'pageFirewall_gbNewRule
        '
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_btnAdd)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_lblSrcPort)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_txtSrcPort)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_lblAction)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_lblProtocol)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_lblDstPort)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_lblDstAddr)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_lblSrcAddr)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_txtAction)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_txtProtocol)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_txtDstPort)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_txtDstAddr)
        Me.pageFirewall_gbNewRule.Controls.Add(Me.pageFirewall_txtSrcAddr)
        resources.ApplyResources(Me.pageFirewall_gbNewRule, "pageFirewall_gbNewRule")
        Me.pageFirewall_gbNewRule.Name = "pageFirewall_gbNewRule"
        Me.pageFirewall_gbNewRule.TabStop = False
        '
        'pageFirewall_btnAdd
        '
        resources.ApplyResources(Me.pageFirewall_btnAdd, "pageFirewall_btnAdd")
        Me.pageFirewall_btnAdd.Name = "pageFirewall_btnAdd"
        Me.ToolTipMain.SetToolTip(Me.pageFirewall_btnAdd, resources.GetString("pageFirewall_btnAdd.ToolTip"))
        Me.pageFirewall_btnAdd.UseVisualStyleBackColor = True
        '
        'pageFirewall_lblSrcPort
        '
        resources.ApplyResources(Me.pageFirewall_lblSrcPort, "pageFirewall_lblSrcPort")
        Me.pageFirewall_lblSrcPort.Name = "pageFirewall_lblSrcPort"
        '
        'pageFirewall_txtSrcPort
        '
        Me.pageFirewall_txtSrcPort.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.pageFirewall_txtSrcPort.FormattingEnabled = True
        Me.pageFirewall_txtSrcPort.Items.AddRange(New Object() {resources.GetString("pageFirewall_txtSrcPort.Items")})
        resources.ApplyResources(Me.pageFirewall_txtSrcPort, "pageFirewall_txtSrcPort")
        Me.pageFirewall_txtSrcPort.Name = "pageFirewall_txtSrcPort"
        '
        'pageFirewall_lblAction
        '
        resources.ApplyResources(Me.pageFirewall_lblAction, "pageFirewall_lblAction")
        Me.pageFirewall_lblAction.Name = "pageFirewall_lblAction"
        '
        'pageFirewall_lblProtocol
        '
        resources.ApplyResources(Me.pageFirewall_lblProtocol, "pageFirewall_lblProtocol")
        Me.pageFirewall_lblProtocol.Name = "pageFirewall_lblProtocol"
        '
        'pageFirewall_lblDstPort
        '
        resources.ApplyResources(Me.pageFirewall_lblDstPort, "pageFirewall_lblDstPort")
        Me.pageFirewall_lblDstPort.Name = "pageFirewall_lblDstPort"
        '
        'pageFirewall_lblDstAddr
        '
        resources.ApplyResources(Me.pageFirewall_lblDstAddr, "pageFirewall_lblDstAddr")
        Me.pageFirewall_lblDstAddr.Name = "pageFirewall_lblDstAddr"
        '
        'pageFirewall_lblSrcAddr
        '
        resources.ApplyResources(Me.pageFirewall_lblSrcAddr, "pageFirewall_lblSrcAddr")
        Me.pageFirewall_lblSrcAddr.Name = "pageFirewall_lblSrcAddr"
        '
        'pageFirewall_txtAction
        '
        Me.pageFirewall_txtAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.pageFirewall_txtAction.FormattingEnabled = True
        Me.pageFirewall_txtAction.Items.AddRange(New Object() {resources.GetString("pageFirewall_txtAction.Items"), resources.GetString("pageFirewall_txtAction.Items1"), resources.GetString("pageFirewall_txtAction.Items2"), resources.GetString("pageFirewall_txtAction.Items3")})
        resources.ApplyResources(Me.pageFirewall_txtAction, "pageFirewall_txtAction")
        Me.pageFirewall_txtAction.Name = "pageFirewall_txtAction"
        '
        'pageFirewall_txtProtocol
        '
        Me.pageFirewall_txtProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.pageFirewall_txtProtocol.FormattingEnabled = True
        Me.pageFirewall_txtProtocol.Items.AddRange(New Object() {resources.GetString("pageFirewall_txtProtocol.Items"), resources.GetString("pageFirewall_txtProtocol.Items1"), resources.GetString("pageFirewall_txtProtocol.Items2")})
        resources.ApplyResources(Me.pageFirewall_txtProtocol, "pageFirewall_txtProtocol")
        Me.pageFirewall_txtProtocol.Name = "pageFirewall_txtProtocol"
        '
        'pageFirewall_txtDstPort
        '
        Me.pageFirewall_txtDstPort.BackColor = System.Drawing.Color.Honeydew
        Me.pageFirewall_txtDstPort.FormattingEnabled = True
        Me.pageFirewall_txtDstPort.Items.AddRange(New Object() {resources.GetString("pageFirewall_txtDstPort.Items")})
        resources.ApplyResources(Me.pageFirewall_txtDstPort, "pageFirewall_txtDstPort")
        Me.pageFirewall_txtDstPort.Name = "pageFirewall_txtDstPort"
        '
        'pageFirewall_txtDstAddr
        '
        Me.pageFirewall_txtDstAddr.BackColor = System.Drawing.Color.Honeydew
        Me.pageFirewall_txtDstAddr.FormattingEnabled = True
        Me.pageFirewall_txtDstAddr.Items.AddRange(New Object() {resources.GetString("pageFirewall_txtDstAddr.Items")})
        resources.ApplyResources(Me.pageFirewall_txtDstAddr, "pageFirewall_txtDstAddr")
        Me.pageFirewall_txtDstAddr.Name = "pageFirewall_txtDstAddr"
        '
        'pageFirewall_txtSrcAddr
        '
        Me.pageFirewall_txtSrcAddr.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.pageFirewall_txtSrcAddr.FormattingEnabled = True
        Me.pageFirewall_txtSrcAddr.Items.AddRange(New Object() {resources.GetString("pageFirewall_txtSrcAddr.Items")})
        resources.ApplyResources(Me.pageFirewall_txtSrcAddr, "pageFirewall_txtSrcAddr")
        Me.pageFirewall_txtSrcAddr.Name = "pageFirewall_txtSrcAddr"
        '
        'pageFirewall_btnApply
        '
        resources.ApplyResources(Me.pageFirewall_btnApply, "pageFirewall_btnApply")
        Me.pageFirewall_btnApply.Name = "pageFirewall_btnApply"
        Me.pageFirewall_btnApply.UseVisualStyleBackColor = True
        '
        'pageFirewall_imgSideImage
        '
        resources.ApplyResources(Me.pageFirewall_imgSideImage, "pageFirewall_imgSideImage")
        Me.pageFirewall_imgSideImage.Name = "pageFirewall_imgSideImage"
        Me.pageFirewall_imgSideImage.TabStop = False
        '
        'tpTraffSh
        '
        Me.tpTraffSh.Controls.Add(Me.pageTrafficShaping_lblNote)
        Me.tpTraffSh.Controls.Add(Me.pageTrafficShaping_gbRules)
        Me.tpTraffSh.Controls.Add(Me.pageTrafficShaping_gbAddRule)
        Me.tpTraffSh.Controls.Add(Me.pageTrafficShaping_imgSideImage)
        Me.tpTraffSh.Controls.Add(Me.pageTrafficShaping_btnApply)
        resources.ApplyResources(Me.tpTraffSh, "tpTraffSh")
        Me.tpTraffSh.Name = "tpTraffSh"
        Me.tpTraffSh.UseVisualStyleBackColor = True
        '
        'pageTrafficShaping_lblNote
        '
        Me.pageTrafficShaping_lblNote.ForeColor = System.Drawing.Color.Gray
        resources.ApplyResources(Me.pageTrafficShaping_lblNote, "pageTrafficShaping_lblNote")
        Me.pageTrafficShaping_lblNote.Name = "pageTrafficShaping_lblNote"
        '
        'pageTrafficShaping_gbRules
        '
        Me.pageTrafficShaping_gbRules.Controls.Add(Me.pageTrafficShaping_lvRules)
        Me.pageTrafficShaping_gbRules.Controls.Add(Me.pageTrafficShaping_btnRemove)
        resources.ApplyResources(Me.pageTrafficShaping_gbRules, "pageTrafficShaping_gbRules")
        Me.pageTrafficShaping_gbRules.Name = "pageTrafficShaping_gbRules"
        Me.pageTrafficShaping_gbRules.TabStop = False
        '
        'pageTrafficShaping_lvRules
        '
        Me.pageTrafficShaping_lvRules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTrafficShapingIcon, Me.chTrafficShapingSrcAddr, Me.chTrafficShapingSrcPort, Me.chTrafficShapingDstAddr, Me.chTrafficShapingDstPort, Me.chTrafficShapingProtocol, Me.chTrafficShapingLimit})
        Me.pageTrafficShaping_lvRules.FullRowSelect = True
        Me.pageTrafficShaping_lvRules.GridLines = True
        Me.pageTrafficShaping_lvRules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        resources.ApplyResources(Me.pageTrafficShaping_lvRules, "pageTrafficShaping_lvRules")
        Me.pageTrafficShaping_lvRules.MultiSelect = False
        Me.pageTrafficShaping_lvRules.Name = "pageTrafficShaping_lvRules"
        Me.pageTrafficShaping_lvRules.SmallImageList = Me.ImageListMain
        Me.pageTrafficShaping_lvRules.UseCompatibleStateImageBehavior = False
        Me.pageTrafficShaping_lvRules.View = System.Windows.Forms.View.Details
        '
        'chTrafficShapingIcon
        '
        resources.ApplyResources(Me.chTrafficShapingIcon, "chTrafficShapingIcon")
        '
        'chTrafficShapingSrcAddr
        '
        resources.ApplyResources(Me.chTrafficShapingSrcAddr, "chTrafficShapingSrcAddr")
        '
        'chTrafficShapingSrcPort
        '
        resources.ApplyResources(Me.chTrafficShapingSrcPort, "chTrafficShapingSrcPort")
        '
        'chTrafficShapingDstAddr
        '
        resources.ApplyResources(Me.chTrafficShapingDstAddr, "chTrafficShapingDstAddr")
        '
        'chTrafficShapingDstPort
        '
        resources.ApplyResources(Me.chTrafficShapingDstPort, "chTrafficShapingDstPort")
        '
        'chTrafficShapingProtocol
        '
        resources.ApplyResources(Me.chTrafficShapingProtocol, "chTrafficShapingProtocol")
        '
        'chTrafficShapingLimit
        '
        resources.ApplyResources(Me.chTrafficShapingLimit, "chTrafficShapingLimit")
        '
        'pageTrafficShaping_btnRemove
        '
        resources.ApplyResources(Me.pageTrafficShaping_btnRemove, "pageTrafficShaping_btnRemove")
        Me.pageTrafficShaping_btnRemove.Name = "pageTrafficShaping_btnRemove"
        Me.ToolTipMain.SetToolTip(Me.pageTrafficShaping_btnRemove, resources.GetString("pageTrafficShaping_btnRemove.ToolTip"))
        Me.pageTrafficShaping_btnRemove.UseVisualStyleBackColor = True
        '
        'pageTrafficShaping_gbAddRule
        '
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_lblSrcPort)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_txtSrcPort)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_lblLimit)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_btnAdd)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_lblProtocol)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_lblDstPort)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_lblDstAddr)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_lblSrcAddr)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_txtLimit)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_txtProtocol)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_txtDstPort)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_txtDstAddr)
        Me.pageTrafficShaping_gbAddRule.Controls.Add(Me.pageTrafficShaping_txtSrcAddr)
        resources.ApplyResources(Me.pageTrafficShaping_gbAddRule, "pageTrafficShaping_gbAddRule")
        Me.pageTrafficShaping_gbAddRule.Name = "pageTrafficShaping_gbAddRule"
        Me.pageTrafficShaping_gbAddRule.TabStop = False
        '
        'pageTrafficShaping_lblSrcPort
        '
        resources.ApplyResources(Me.pageTrafficShaping_lblSrcPort, "pageTrafficShaping_lblSrcPort")
        Me.pageTrafficShaping_lblSrcPort.Name = "pageTrafficShaping_lblSrcPort"
        '
        'pageTrafficShaping_txtSrcPort
        '
        Me.pageTrafficShaping_txtSrcPort.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.pageTrafficShaping_txtSrcPort.FormattingEnabled = True
        Me.pageTrafficShaping_txtSrcPort.Items.AddRange(New Object() {resources.GetString("pageTrafficShaping_txtSrcPort.Items")})
        resources.ApplyResources(Me.pageTrafficShaping_txtSrcPort, "pageTrafficShaping_txtSrcPort")
        Me.pageTrafficShaping_txtSrcPort.Name = "pageTrafficShaping_txtSrcPort"
        '
        'pageTrafficShaping_lblLimit
        '
        resources.ApplyResources(Me.pageTrafficShaping_lblLimit, "pageTrafficShaping_lblLimit")
        Me.pageTrafficShaping_lblLimit.Name = "pageTrafficShaping_lblLimit"
        '
        'pageTrafficShaping_btnAdd
        '
        resources.ApplyResources(Me.pageTrafficShaping_btnAdd, "pageTrafficShaping_btnAdd")
        Me.pageTrafficShaping_btnAdd.Name = "pageTrafficShaping_btnAdd"
        Me.ToolTipMain.SetToolTip(Me.pageTrafficShaping_btnAdd, resources.GetString("pageTrafficShaping_btnAdd.ToolTip"))
        Me.pageTrafficShaping_btnAdd.UseVisualStyleBackColor = True
        '
        'pageTrafficShaping_lblProtocol
        '
        resources.ApplyResources(Me.pageTrafficShaping_lblProtocol, "pageTrafficShaping_lblProtocol")
        Me.pageTrafficShaping_lblProtocol.Name = "pageTrafficShaping_lblProtocol"
        '
        'pageTrafficShaping_lblDstPort
        '
        resources.ApplyResources(Me.pageTrafficShaping_lblDstPort, "pageTrafficShaping_lblDstPort")
        Me.pageTrafficShaping_lblDstPort.Name = "pageTrafficShaping_lblDstPort"
        '
        'pageTrafficShaping_lblDstAddr
        '
        resources.ApplyResources(Me.pageTrafficShaping_lblDstAddr, "pageTrafficShaping_lblDstAddr")
        Me.pageTrafficShaping_lblDstAddr.Name = "pageTrafficShaping_lblDstAddr"
        '
        'pageTrafficShaping_lblSrcAddr
        '
        resources.ApplyResources(Me.pageTrafficShaping_lblSrcAddr, "pageTrafficShaping_lblSrcAddr")
        Me.pageTrafficShaping_lblSrcAddr.Name = "pageTrafficShaping_lblSrcAddr"
        '
        'pageTrafficShaping_txtLimit
        '
        Me.pageTrafficShaping_txtLimit.FormattingEnabled = True
        Me.pageTrafficShaping_txtLimit.Items.AddRange(New Object() {resources.GetString("pageTrafficShaping_txtLimit.Items"), resources.GetString("pageTrafficShaping_txtLimit.Items1"), resources.GetString("pageTrafficShaping_txtLimit.Items2"), resources.GetString("pageTrafficShaping_txtLimit.Items3"), resources.GetString("pageTrafficShaping_txtLimit.Items4"), resources.GetString("pageTrafficShaping_txtLimit.Items5"), resources.GetString("pageTrafficShaping_txtLimit.Items6"), resources.GetString("pageTrafficShaping_txtLimit.Items7"), resources.GetString("pageTrafficShaping_txtLimit.Items8"), resources.GetString("pageTrafficShaping_txtLimit.Items9")})
        resources.ApplyResources(Me.pageTrafficShaping_txtLimit, "pageTrafficShaping_txtLimit")
        Me.pageTrafficShaping_txtLimit.Name = "pageTrafficShaping_txtLimit"
        '
        'pageTrafficShaping_txtProtocol
        '
        Me.pageTrafficShaping_txtProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.pageTrafficShaping_txtProtocol.FormattingEnabled = True
        Me.pageTrafficShaping_txtProtocol.Items.AddRange(New Object() {resources.GetString("pageTrafficShaping_txtProtocol.Items"), resources.GetString("pageTrafficShaping_txtProtocol.Items1"), resources.GetString("pageTrafficShaping_txtProtocol.Items2")})
        resources.ApplyResources(Me.pageTrafficShaping_txtProtocol, "pageTrafficShaping_txtProtocol")
        Me.pageTrafficShaping_txtProtocol.Name = "pageTrafficShaping_txtProtocol"
        '
        'pageTrafficShaping_txtDstPort
        '
        Me.pageTrafficShaping_txtDstPort.BackColor = System.Drawing.Color.Honeydew
        Me.pageTrafficShaping_txtDstPort.FormattingEnabled = True
        Me.pageTrafficShaping_txtDstPort.Items.AddRange(New Object() {resources.GetString("pageTrafficShaping_txtDstPort.Items")})
        resources.ApplyResources(Me.pageTrafficShaping_txtDstPort, "pageTrafficShaping_txtDstPort")
        Me.pageTrafficShaping_txtDstPort.Name = "pageTrafficShaping_txtDstPort"
        '
        'pageTrafficShaping_txtDstAddr
        '
        Me.pageTrafficShaping_txtDstAddr.BackColor = System.Drawing.Color.Honeydew
        Me.pageTrafficShaping_txtDstAddr.FormattingEnabled = True
        Me.pageTrafficShaping_txtDstAddr.Items.AddRange(New Object() {resources.GetString("pageTrafficShaping_txtDstAddr.Items")})
        resources.ApplyResources(Me.pageTrafficShaping_txtDstAddr, "pageTrafficShaping_txtDstAddr")
        Me.pageTrafficShaping_txtDstAddr.Name = "pageTrafficShaping_txtDstAddr"
        '
        'pageTrafficShaping_txtSrcAddr
        '
        Me.pageTrafficShaping_txtSrcAddr.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.pageTrafficShaping_txtSrcAddr.FormattingEnabled = True
        Me.pageTrafficShaping_txtSrcAddr.Items.AddRange(New Object() {resources.GetString("pageTrafficShaping_txtSrcAddr.Items")})
        resources.ApplyResources(Me.pageTrafficShaping_txtSrcAddr, "pageTrafficShaping_txtSrcAddr")
        Me.pageTrafficShaping_txtSrcAddr.Name = "pageTrafficShaping_txtSrcAddr"
        '
        'pageTrafficShaping_imgSideImage
        '
        resources.ApplyResources(Me.pageTrafficShaping_imgSideImage, "pageTrafficShaping_imgSideImage")
        Me.pageTrafficShaping_imgSideImage.Name = "pageTrafficShaping_imgSideImage"
        Me.pageTrafficShaping_imgSideImage.TabStop = False
        '
        'pageTrafficShaping_btnApply
        '
        resources.ApplyResources(Me.pageTrafficShaping_btnApply, "pageTrafficShaping_btnApply")
        Me.pageTrafficShaping_btnApply.Name = "pageTrafficShaping_btnApply"
        Me.pageTrafficShaping_btnApply.UseVisualStyleBackColor = True
        '
        'msMainMenu
        '
        Me.msMainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemFile, Me.menuItemTools, Me.menuItemPlugins, Me.menuItemLanguage, Me.menuItemHelp})
        resources.ApplyResources(Me.msMainMenu, "msMainMenu")
        Me.msMainMenu.Name = "msMainMenu"
        '
        'menuItemFile
        '
        Me.menuItemFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemOpenLocal, Me.menuItemManageAccess, Me.menuItemS3, Me.menuItemOpenRemote, Me.menuItemS2, Me.menuItemDisconnect, Me.menuItemToTray, Me.menuItemExit})
        Me.menuItemFile.Name = "menuItemFile"
        resources.ApplyResources(Me.menuItemFile, "menuItemFile")
        '
        'menuItemOpenLocal
        '
        resources.ApplyResources(Me.menuItemOpenLocal, "menuItemOpenLocal")
        Me.menuItemOpenLocal.Name = "menuItemOpenLocal"
        '
        'menuItemManageAccess
        '
        resources.ApplyResources(Me.menuItemManageAccess, "menuItemManageAccess")
        Me.menuItemManageAccess.Name = "menuItemManageAccess"
        '
        'menuItemS3
        '
        Me.menuItemS3.Name = "menuItemS3"
        resources.ApplyResources(Me.menuItemS3, "menuItemS3")
        '
        'menuItemOpenRemote
        '
        resources.ApplyResources(Me.menuItemOpenRemote, "menuItemOpenRemote")
        Me.menuItemOpenRemote.Name = "menuItemOpenRemote"
        '
        'menuItemS2
        '
        Me.menuItemS2.Name = "menuItemS2"
        resources.ApplyResources(Me.menuItemS2, "menuItemS2")
        '
        'menuItemDisconnect
        '
        resources.ApplyResources(Me.menuItemDisconnect, "menuItemDisconnect")
        Me.menuItemDisconnect.Name = "menuItemDisconnect"
        '
        'menuItemToTray
        '
        resources.ApplyResources(Me.menuItemToTray, "menuItemToTray")
        Me.menuItemToTray.Name = "menuItemToTray"
        '
        'menuItemExit
        '
        resources.ApplyResources(Me.menuItemExit, "menuItemExit")
        Me.menuItemExit.Name = "menuItemExit"
        '
        'menuItemTools
        '
        Me.menuItemTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemRefresh, Me.menuItemS6, Me.menuItemDhcpInstall, Me.menuItemDhcpUnInstall, Me.menuItemS4, Me.menuItemDnsInstall, Me.menuItemDnsUnInstall, Me.menuItemS5, Me.menuItemNatInstall, Me.menuItemNatUnInstall})
        Me.menuItemTools.Name = "menuItemTools"
        resources.ApplyResources(Me.menuItemTools, "menuItemTools")
        '
        'menuItemRefresh
        '
        resources.ApplyResources(Me.menuItemRefresh, "menuItemRefresh")
        Me.menuItemRefresh.Name = "menuItemRefresh"
        '
        'menuItemS6
        '
        Me.menuItemS6.Name = "menuItemS6"
        resources.ApplyResources(Me.menuItemS6, "menuItemS6")
        '
        'menuItemDhcpInstall
        '
        resources.ApplyResources(Me.menuItemDhcpInstall, "menuItemDhcpInstall")
        Me.menuItemDhcpInstall.Name = "menuItemDhcpInstall"
        '
        'menuItemDhcpUnInstall
        '
        resources.ApplyResources(Me.menuItemDhcpUnInstall, "menuItemDhcpUnInstall")
        Me.menuItemDhcpUnInstall.Name = "menuItemDhcpUnInstall"
        '
        'menuItemS4
        '
        Me.menuItemS4.Name = "menuItemS4"
        resources.ApplyResources(Me.menuItemS4, "menuItemS4")
        '
        'menuItemDnsInstall
        '
        resources.ApplyResources(Me.menuItemDnsInstall, "menuItemDnsInstall")
        Me.menuItemDnsInstall.Name = "menuItemDnsInstall"
        '
        'menuItemDnsUnInstall
        '
        resources.ApplyResources(Me.menuItemDnsUnInstall, "menuItemDnsUnInstall")
        Me.menuItemDnsUnInstall.Name = "menuItemDnsUnInstall"
        '
        'menuItemS5
        '
        Me.menuItemS5.Name = "menuItemS5"
        resources.ApplyResources(Me.menuItemS5, "menuItemS5")
        '
        'menuItemNatInstall
        '
        resources.ApplyResources(Me.menuItemNatInstall, "menuItemNatInstall")
        Me.menuItemNatInstall.Name = "menuItemNatInstall"
        '
        'menuItemNatUnInstall
        '
        resources.ApplyResources(Me.menuItemNatUnInstall, "menuItemNatUnInstall")
        Me.menuItemNatUnInstall.Name = "menuItemNatUnInstall"
        '
        'menuItemPlugins
        '
        Me.menuItemPlugins.Name = "menuItemPlugins"
        resources.ApplyResources(Me.menuItemPlugins, "menuItemPlugins")
        '
        'menuItemLanguage
        '
        Me.menuItemLanguage.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemEnglish, Me.menuItemBulgarian})
        Me.menuItemLanguage.Name = "menuItemLanguage"
        resources.ApplyResources(Me.menuItemLanguage, "menuItemLanguage")
        '
        'menuItemEnglish
        '
        resources.ApplyResources(Me.menuItemEnglish, "menuItemEnglish")
        Me.menuItemEnglish.Name = "menuItemEnglish"
        '
        'menuItemBulgarian
        '
        resources.ApplyResources(Me.menuItemBulgarian, "menuItemBulgarian")
        Me.menuItemBulgarian.Name = "menuItemBulgarian"
        '
        'menuItemHelp
        '
        Me.menuItemHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemContents, Me.menuItemIndex, Me.menuItemS1, Me.menuItemAbout})
        Me.menuItemHelp.Name = "menuItemHelp"
        resources.ApplyResources(Me.menuItemHelp, "menuItemHelp")
        '
        'menuItemContents
        '
        resources.ApplyResources(Me.menuItemContents, "menuItemContents")
        Me.menuItemContents.Name = "menuItemContents"
        '
        'menuItemIndex
        '
        resources.ApplyResources(Me.menuItemIndex, "menuItemIndex")
        Me.menuItemIndex.Name = "menuItemIndex"
        '
        'menuItemS1
        '
        Me.menuItemS1.Name = "menuItemS1"
        resources.ApplyResources(Me.menuItemS1, "menuItemS1")
        '
        'menuItemAbout
        '
        Me.menuItemAbout.Name = "menuItemAbout"
        resources.ApplyResources(Me.menuItemAbout, "menuItemAbout")
        '
        'ToolTipMain
        '
        Me.ToolTipMain.AutoPopDelay = 2000
        Me.ToolTipMain.InitialDelay = 500
        Me.ToolTipMain.ReshowDelay = 100
        '
        'NotifyIconContextMenu
        '
        Me.NotifyIconContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.notifyMenuItemShow, Me.notifyMenuItemExit})
        Me.NotifyIconContextMenu.Name = "NotifyIconContextMenu"
        resources.ApplyResources(Me.NotifyIconContextMenu, "NotifyIconContextMenu")
        '
        'notifyMenuItemShow
        '
        resources.ApplyResources(Me.notifyMenuItemShow, "notifyMenuItemShow")
        Me.notifyMenuItemShow.Name = "notifyMenuItemShow"
        '
        'notifyMenuItemExit
        '
        resources.ApplyResources(Me.notifyMenuItemExit, "notifyMenuItemExit")
        Me.notifyMenuItemExit.Name = "notifyMenuItemExit"
        '
        'NotifyIconMain
        '
        Me.NotifyIconMain.ContextMenuStrip = Me.NotifyIconContextMenu
        resources.ApplyResources(Me.NotifyIconMain, "NotifyIconMain")
        '
        'ServiceControllerDHCP
        '
        Me.ServiceControllerDHCP.ServiceName = "NetGate DHCPd"
        '
        'TimerCheckServices
        '
        Me.TimerCheckServices.Enabled = True
        Me.TimerCheckServices.Interval = 3000
        '
        'ServiceControllerDNS
        '
        Me.ServiceControllerDNS.ServiceName = "NetGate DNSd"
        '
        'ServiceControllerNAT
        '
        Me.ServiceControllerNAT.ServiceName = "NetGate NATd"
        '
        'frmMain
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tcMainTab)
        Me.Controls.Add(Me.msMainMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.tcMainTab.ResumeLayout(False)
        Me.tpStatus.ResumeLayout(False)
        Me.tpStatus.PerformLayout()
        Me.pageStatus_gbServices.ResumeLayout(False)
        CType(Me.pageStatus_imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pageStatus_imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pageStatus_imgEurope, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpDHCP.ResumeLayout(False)
        CType(Me.pageDHCP_imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pageDHCP_gbStatus.ResumeLayout(False)
        Me.tpDNS.ResumeLayout(False)
        Me.pageDNS_gbResolver.ResumeLayout(False)
        Me.pageDNS_gbResolver.PerformLayout()
        Me.pageDNS_gbCaching.ResumeLayout(False)
        Me.pageDNS_gbCaching.PerformLayout()
        Me.pageDNS_gbForwarding.ResumeLayout(False)
        Me.pageDNS_gbForwarding.PerformLayout()
        CType(Me.pageDNS_imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpNAT.ResumeLayout(False)
        Me.tpNAT.PerformLayout()
        Me.pageNAT_gbRouting.ResumeLayout(False)
        Me.pageNAT_gbRouting.PerformLayout()
        CType(Me.pageNAT_imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpFirewall.ResumeLayout(False)
        Me.pageFirewall_gbRules.ResumeLayout(False)
        Me.pageFirewall_gbNewRule.ResumeLayout(False)
        Me.pageFirewall_gbNewRule.PerformLayout()
        CType(Me.pageFirewall_imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpTraffSh.ResumeLayout(False)
        Me.pageTrafficShaping_gbRules.ResumeLayout(False)
        Me.pageTrafficShaping_gbAddRule.ResumeLayout(False)
        Me.pageTrafficShaping_gbAddRule.PerformLayout()
        CType(Me.pageTrafficShaping_imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.msMainMenu.ResumeLayout(False)
        Me.msMainMenu.PerformLayout()
        Me.NotifyIconContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tcMainTab As System.Windows.Forms.TabControl
    Friend WithEvents tpDHCP As System.Windows.Forms.TabPage
    Friend WithEvents tpDNS As System.Windows.Forms.TabPage
    Friend WithEvents msMainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents menuItemFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tpNAT As System.Windows.Forms.TabPage
    Friend WithEvents tpFirewall As System.Windows.Forms.TabPage
    Friend WithEvents tpTraffSh As System.Windows.Forms.TabPage
    Friend WithEvents menuItemHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemContents As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemIndex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemS1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents menuItemAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tpStatus As System.Windows.Forms.TabPage
    Friend WithEvents ToolTipMain As System.Windows.Forms.ToolTip
    Friend WithEvents pageDHCP_btnSetup As System.Windows.Forms.Button
    Friend WithEvents menuItemOpenLocal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemOpenRemote As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemS2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents pageDHCP_gbStatus As System.Windows.Forms.GroupBox
    Friend WithEvents menuItemManageAccess As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemS3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents NotifyIconMain As System.Windows.Forms.NotifyIcon
    Friend WithEvents NotifyIconContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents notifyMenuItemShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents notifyMenuItemExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pageStatus_gbServices As System.Windows.Forms.GroupBox
    Friend WithEvents pageStatus_lblNAT As System.Windows.Forms.Label
    Friend WithEvents pageStatus_lblDNS As System.Windows.Forms.Label
    Friend WithEvents pageStatus_lblDHCP As System.Windows.Forms.Label
    Friend WithEvents pageStatus_btnRefresh As System.Windows.Forms.Button
    Friend WithEvents pageStatus_btnStopNAT As System.Windows.Forms.Button
    Friend WithEvents pageStatus_btnStartNAT As System.Windows.Forms.Button
    Friend WithEvents pageStatus_btnStopDNS As System.Windows.Forms.Button
    Friend WithEvents pageStatus_btnStopDHCP As System.Windows.Forms.Button
    Friend WithEvents pageStatus_btnStartDNS As System.Windows.Forms.Button
    Friend WithEvents pageStatus_btnStartDHCP As System.Windows.Forms.Button
    Friend WithEvents pageDHCP_imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents pageDNS_imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents pageNAT_imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents pageFirewall_imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents pageTrafficShaping_imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents pageStatus_imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents pageStatus_imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents pageStatus_lblLogo2 As System.Windows.Forms.Label
    Friend WithEvents pageStatus_lblLogo1 As System.Windows.Forms.Label
    Friend WithEvents pageStatus_imgEurope As System.Windows.Forms.PictureBox
    Friend WithEvents pageStatus_lineHorizontal As System.Windows.Forms.Panel
    Friend WithEvents pageStatus_lineVertical As System.Windows.Forms.Panel
    Friend WithEvents pageStatus_lineCenter As System.Windows.Forms.Panel
    Friend WithEvents ServiceControllerDHCP As System.ServiceProcess.ServiceController
    Friend WithEvents TimerCheckServices As System.Windows.Forms.Timer
    Friend WithEvents ServiceControllerDNS As System.ServiceProcess.ServiceController
    Friend WithEvents ServiceControllerNAT As System.ServiceProcess.ServiceController
    Friend WithEvents pageDHCP_lvLeases As System.Windows.Forms.ListView
    Friend WithEvents chLeasesIPAddr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chLeasesHwAddr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chLeasesHostname As System.Windows.Forms.ColumnHeader
    Friend WithEvents chLeasesLeaseTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageListMain As System.Windows.Forms.ImageList
    Friend WithEvents chLeasesImage As System.Windows.Forms.ColumnHeader
    Friend WithEvents pageStatus_btnLocalServer As System.Windows.Forms.Button
    Friend WithEvents pageStatus_lblInfo As System.Windows.Forms.Label
    Friend WithEvents menuItemToTray As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemDhcpInstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemDhcpUnInstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemS4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents menuItemDnsInstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemDnsUnInstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemS5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents menuItemNatInstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemNatUnInstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemDisconnect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemS6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents menuItemLanguage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemEnglish As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemBulgarian As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pageDHCP_btnRefresh As System.Windows.Forms.Button
    Friend WithEvents menuItemPlugins As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pageNAT_gbRouting As System.Windows.Forms.GroupBox
    Friend WithEvents pageNAT_lvExternal As System.Windows.Forms.ListView
    Friend WithEvents chRoutingExIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents chRoutingExAdapterName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chRoutingExAdapterInfo As System.Windows.Forms.ColumnHeader
    Friend WithEvents pageNAT_lblInternal As System.Windows.Forms.Label
    Friend WithEvents pageNAT_lblExternal As System.Windows.Forms.Label
    Friend WithEvents pageNAT_lvInternal As System.Windows.Forms.ListView
    Friend WithEvents chRoutingInIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents chRoutingInAdapterName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chRoutingInAdapterInfo As System.Windows.Forms.ColumnHeader
    Friend WithEvents pageNAT_btnApply As System.Windows.Forms.Button
    Friend WithEvents pageNAT_cbEnableTTL As System.Windows.Forms.CheckBox
    Friend WithEvents pageDNS_gbForwarding As System.Windows.Forms.GroupBox
    Friend WithEvents pageDNS_rbUseSpecifiedDNS As System.Windows.Forms.RadioButton
    Friend WithEvents pageDNS_rbUseSystemDefaultDNS As System.Windows.Forms.RadioButton
    Friend WithEvents pageDNS_btnApply As System.Windows.Forms.Button
    Friend WithEvents pageFirewall_btnApply As System.Windows.Forms.Button
    Friend WithEvents pageTrafficShaping_btnApply As System.Windows.Forms.Button
    Friend WithEvents pageDNS_txtDNS As System.Windows.Forms.TextBox
    Friend WithEvents pageDNS_gbResolver As System.Windows.Forms.GroupBox
    Friend WithEvents pageDNS_gbCaching As System.Windows.Forms.GroupBox
    Friend WithEvents pageDNS_txtCacheRecords As System.Windows.Forms.TextBox
    Friend WithEvents pageDNS_lblRecords As System.Windows.Forms.Label
    Friend WithEvents pageDNS_lblCacheSize As System.Windows.Forms.Label
    Friend WithEvents pageDNS_btnClearCache As System.Windows.Forms.Button
    Friend WithEvents pageDNS_cbEnableCache As System.Windows.Forms.CheckBox
    Friend WithEvents pageDNS_lblDNSInfo As System.Windows.Forms.Label
    Friend WithEvents pageDNS_cbLeases As System.Windows.Forms.CheckBox
    Friend WithEvents pageDNS_lvDNS As System.Windows.Forms.ListView
    Friend WithEvents chDnsIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDnsIp As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDnsRequest As System.Windows.Forms.ColumnHeader
    Friend WithEvents pageNAT_cbEnableNAT As System.Windows.Forms.CheckBox
    Friend WithEvents pageDNS_btnRemoveRecord As System.Windows.Forms.Button
    Friend WithEvents pageDNS_btnAddRecord As System.Windows.Forms.Button
    Friend WithEvents pageTrafficShaping_gbRules As System.Windows.Forms.GroupBox
    Friend WithEvents pageTrafficShaping_gbAddRule As System.Windows.Forms.GroupBox
    Friend WithEvents pageTrafficShaping_lblLimit As System.Windows.Forms.Label
    Friend WithEvents pageTrafficShaping_lblProtocol As System.Windows.Forms.Label
    Friend WithEvents pageTrafficShaping_lblDstPort As System.Windows.Forms.Label
    Friend WithEvents pageTrafficShaping_lblDstAddr As System.Windows.Forms.Label
    Friend WithEvents pageTrafficShaping_lblSrcAddr As System.Windows.Forms.Label
    Friend WithEvents pageTrafficShaping_txtLimit As System.Windows.Forms.ComboBox
    Friend WithEvents pageTrafficShaping_txtProtocol As System.Windows.Forms.ComboBox
    Friend WithEvents pageTrafficShaping_txtDstPort As System.Windows.Forms.ComboBox
    Friend WithEvents pageTrafficShaping_txtDstAddr As System.Windows.Forms.ComboBox
    Friend WithEvents pageTrafficShaping_txtSrcAddr As System.Windows.Forms.ComboBox
    Friend WithEvents pageTrafficShaping_lblSrcPort As System.Windows.Forms.Label
    Friend WithEvents pageTrafficShaping_txtSrcPort As System.Windows.Forms.ComboBox
    Friend WithEvents pageFirewall_gbNewRule As System.Windows.Forms.GroupBox
    Friend WithEvents pageFirewall_lblSrcPort As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_txtSrcPort As System.Windows.Forms.ComboBox
    Friend WithEvents pageFirewall_lblProtocol As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_lblDstPort As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_lblDstAddr As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_lblSrcAddr As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_txtProtocol As System.Windows.Forms.ComboBox
    Friend WithEvents pageFirewall_txtDstPort As System.Windows.Forms.ComboBox
    Friend WithEvents pageFirewall_txtDstAddr As System.Windows.Forms.ComboBox
    Friend WithEvents pageFirewall_txtSrcAddr As System.Windows.Forms.ComboBox
    Friend WithEvents pageTrafficShaping_btnAdd As System.Windows.Forms.Button
    Friend WithEvents pageTrafficShaping_btnRemove As System.Windows.Forms.Button
    Friend WithEvents pageTrafficShaping_lblNote As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_gbRules As System.Windows.Forms.GroupBox
    Friend WithEvents pageFirewall_lvRules As System.Windows.Forms.ListView
    Friend WithEvents pageFirewall_btnRemove As System.Windows.Forms.Button
    Friend WithEvents pageFirewall_btnAdd As System.Windows.Forms.Button
    Friend WithEvents pageFirewall_lblAction As System.Windows.Forms.Label
    Friend WithEvents pageFirewall_txtAction As System.Windows.Forms.ComboBox
    Friend WithEvents pageFirewall_lblNote As System.Windows.Forms.Label
    Friend WithEvents chFirewallIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFirewallSrcAddr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFirewallSrcPort As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFirewallDstAddr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFirewallDstPort As System.Windows.Forms.ColumnHeader
    Friend WithEvents Protocol As System.Windows.Forms.ColumnHeader
    Friend WithEvents Action As System.Windows.Forms.ColumnHeader
    Friend WithEvents pageTrafficShaping_lvRules As System.Windows.Forms.ListView
    Friend WithEvents chTrafficShapingIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTrafficShapingSrcAddr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTrafficShapingSrcPort As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTrafficShapingDstAddr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTrafficShapingDstPort As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTrafficShapingProtocol As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTrafficShapingLimit As System.Windows.Forms.ColumnHeader

End Class

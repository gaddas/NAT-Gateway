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
Partial Class frmSubnetConfig
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSubnetConfig))
        Me.gbOptions = New System.Windows.Forms.GroupBox
        Me.imgSideImage = New System.Windows.Forms.PictureBox
        Me.tbDefaultTtlForUdp = New System.Windows.Forms.MaskedTextBox
        Me.cbDefaultTtlForUdp = New System.Windows.Forms.CheckBox
        Me.tbArpCacheTimeout = New System.Windows.Forms.MaskedTextBox
        Me.tbDefaultMtu = New System.Windows.Forms.MaskedTextBox
        Me.tbDefaultTtlForTcp = New System.Windows.Forms.MaskedTextBox
        Me.tbBootFileSize = New System.Windows.Forms.MaskedTextBox
        Me.tbDomainName = New System.Windows.Forms.TextBox
        Me.ipBroadcastAddr = New IPAddressControlLib.IPAddressControl
        Me.ipSubnetMask = New IPAddressControlLib.IPAddressControl
        Me.ipNBNameServer1 = New IPAddressControlLib.IPAddressControl
        Me.ipTimeServer1 = New IPAddressControlLib.IPAddressControl
        Me.ipNetworkTimeServer1 = New IPAddressControlLib.IPAddressControl
        Me.ipDNS2 = New IPAddressControlLib.IPAddressControl
        Me.ipDNS1 = New IPAddressControlLib.IPAddressControl
        Me.ipGateway2 = New IPAddressControlLib.IPAddressControl
        Me.ipGateway1 = New IPAddressControlLib.IPAddressControl
        Me.cbArpCacheTimeout = New System.Windows.Forms.CheckBox
        Me.cbDefaultMtu = New System.Windows.Forms.CheckBox
        Me.cbDefaultTtlForTcp = New System.Windows.Forms.CheckBox
        Me.cbEnableIpForward = New System.Windows.Forms.CheckBox
        Me.cbBootFileSize = New System.Windows.Forms.CheckBox
        Me.cbTimeServers = New System.Windows.Forms.CheckBox
        Me.cbNetworkTimeServers = New System.Windows.Forms.CheckBox
        Me.cbDNS = New System.Windows.Forms.CheckBox
        Me.cbSubnetMask = New System.Windows.Forms.CheckBox
        Me.cbBroadcastAddr = New System.Windows.Forms.CheckBox
        Me.cbDomainName = New System.Windows.Forms.CheckBox
        Me.cbNBNameServers = New System.Windows.Forms.CheckBox
        Me.cbGateways = New System.Windows.Forms.CheckBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.gbGeneral = New System.Windows.Forms.GroupBox
        Me.ipServerId = New IPAddressControlLib.IPAddressControl
        Me.lblServerID = New System.Windows.Forms.Label
        Me.cbPingCheck = New System.Windows.Forms.CheckBox
        Me.cbDenyUnknownClients = New System.Windows.Forms.CheckBox
        Me.tbLeaseMaximal = New System.Windows.Forms.MaskedTextBox
        Me.tbAddrRangeEnd = New IPAddressControlLib.IPAddressControl
        Me.tbAddrRangeStart = New IPAddressControlLib.IPAddressControl
        Me.tbLeaseDefault = New System.Windows.Forms.MaskedTextBox
        Me.lblLeaseMaximal = New System.Windows.Forms.Label
        Me.lblLeaseDefault = New System.Windows.Forms.Label
        Me.lblInterfaces = New System.Windows.Forms.Label
        Me.lbInterfaces = New System.Windows.Forms.ListBox
        Me.lblBootFilename = New System.Windows.Forms.Label
        Me.lblAddrRange = New System.Windows.Forms.Label
        Me.tbBootFilename = New System.Windows.Forms.TextBox
        Me.gbClients = New System.Windows.Forms.GroupBox
        Me.btnUserClear = New System.Windows.Forms.Button
        Me.btnUserRemove = New System.Windows.Forms.Button
        Me.btnUserAdd = New System.Windows.Forms.Button
        Me.tbNewClientHostName = New System.Windows.Forms.TextBox
        Me.ipNewClientIP = New IPAddressControlLib.IPAddressControl
        Me.lvClients = New System.Windows.Forms.ListView
        Me.lbchIP = New System.Windows.Forms.ColumnHeader
        Me.lbchMAC = New System.Windows.Forms.ColumnHeader
        Me.lbchHostname = New System.Windows.Forms.ColumnHeader
        Me.tbNewClientMac = New System.Windows.Forms.MaskedTextBox
        Me.ToolTipConfig = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button
        Me.gbOptions.SuspendLayout()
        CType(Me.imgSideImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbGeneral.SuspendLayout()
        Me.gbClients.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbOptions
        '
        Me.gbOptions.AccessibleDescription = Nothing
        Me.gbOptions.AccessibleName = Nothing
        resources.ApplyResources(Me.gbOptions, "gbOptions")
        Me.gbOptions.BackgroundImage = Nothing
        Me.gbOptions.Controls.Add(Me.imgSideImage)
        Me.gbOptions.Controls.Add(Me.tbDefaultTtlForUdp)
        Me.gbOptions.Controls.Add(Me.cbDefaultTtlForUdp)
        Me.gbOptions.Controls.Add(Me.tbArpCacheTimeout)
        Me.gbOptions.Controls.Add(Me.tbDefaultMtu)
        Me.gbOptions.Controls.Add(Me.tbDefaultTtlForTcp)
        Me.gbOptions.Controls.Add(Me.tbBootFileSize)
        Me.gbOptions.Controls.Add(Me.tbDomainName)
        Me.gbOptions.Controls.Add(Me.ipBroadcastAddr)
        Me.gbOptions.Controls.Add(Me.ipSubnetMask)
        Me.gbOptions.Controls.Add(Me.ipNBNameServer1)
        Me.gbOptions.Controls.Add(Me.ipTimeServer1)
        Me.gbOptions.Controls.Add(Me.ipNetworkTimeServer1)
        Me.gbOptions.Controls.Add(Me.ipDNS2)
        Me.gbOptions.Controls.Add(Me.ipDNS1)
        Me.gbOptions.Controls.Add(Me.ipGateway2)
        Me.gbOptions.Controls.Add(Me.ipGateway1)
        Me.gbOptions.Controls.Add(Me.cbArpCacheTimeout)
        Me.gbOptions.Controls.Add(Me.cbDefaultMtu)
        Me.gbOptions.Controls.Add(Me.cbDefaultTtlForTcp)
        Me.gbOptions.Controls.Add(Me.cbEnableIpForward)
        Me.gbOptions.Controls.Add(Me.cbBootFileSize)
        Me.gbOptions.Controls.Add(Me.cbTimeServers)
        Me.gbOptions.Controls.Add(Me.cbNetworkTimeServers)
        Me.gbOptions.Controls.Add(Me.cbDNS)
        Me.gbOptions.Controls.Add(Me.cbSubnetMask)
        Me.gbOptions.Controls.Add(Me.cbBroadcastAddr)
        Me.gbOptions.Controls.Add(Me.cbDomainName)
        Me.gbOptions.Controls.Add(Me.cbNBNameServers)
        Me.gbOptions.Controls.Add(Me.cbGateways)
        Me.gbOptions.Font = Nothing
        Me.gbOptions.Name = "gbOptions"
        Me.gbOptions.TabStop = False
        Me.ToolTipConfig.SetToolTip(Me.gbOptions, resources.GetString("gbOptions.ToolTip"))
        '
        'imgSideImage
        '
        Me.imgSideImage.AccessibleDescription = Nothing
        Me.imgSideImage.AccessibleName = Nothing
        resources.ApplyResources(Me.imgSideImage, "imgSideImage")
        Me.imgSideImage.BackgroundImage = Nothing
        Me.imgSideImage.Font = Nothing
        Me.imgSideImage.ImageLocation = Nothing
        Me.imgSideImage.Name = "imgSideImage"
        Me.imgSideImage.TabStop = False
        Me.ToolTipConfig.SetToolTip(Me.imgSideImage, resources.GetString("imgSideImage.ToolTip"))
        '
        'tbDefaultTtlForUdp
        '
        Me.tbDefaultTtlForUdp.AccessibleDescription = Nothing
        Me.tbDefaultTtlForUdp.AccessibleName = Nothing
        resources.ApplyResources(Me.tbDefaultTtlForUdp, "tbDefaultTtlForUdp")
        Me.tbDefaultTtlForUdp.BackgroundImage = Nothing
        Me.tbDefaultTtlForUdp.HideSelection = False
        Me.tbDefaultTtlForUdp.Name = "tbDefaultTtlForUdp"
        Me.ToolTipConfig.SetToolTip(Me.tbDefaultTtlForUdp, resources.GetString("tbDefaultTtlForUdp.ToolTip"))
        '
        'cbDefaultTtlForUdp
        '
        Me.cbDefaultTtlForUdp.AccessibleDescription = Nothing
        Me.cbDefaultTtlForUdp.AccessibleName = Nothing
        resources.ApplyResources(Me.cbDefaultTtlForUdp, "cbDefaultTtlForUdp")
        Me.cbDefaultTtlForUdp.BackgroundImage = Nothing
        Me.cbDefaultTtlForUdp.Font = Nothing
        Me.cbDefaultTtlForUdp.Name = "cbDefaultTtlForUdp"
        Me.ToolTipConfig.SetToolTip(Me.cbDefaultTtlForUdp, resources.GetString("cbDefaultTtlForUdp.ToolTip"))
        Me.cbDefaultTtlForUdp.UseVisualStyleBackColor = True
        '
        'tbArpCacheTimeout
        '
        Me.tbArpCacheTimeout.AccessibleDescription = Nothing
        Me.tbArpCacheTimeout.AccessibleName = Nothing
        resources.ApplyResources(Me.tbArpCacheTimeout, "tbArpCacheTimeout")
        Me.tbArpCacheTimeout.BackgroundImage = Nothing
        Me.tbArpCacheTimeout.HideSelection = False
        Me.tbArpCacheTimeout.Name = "tbArpCacheTimeout"
        Me.ToolTipConfig.SetToolTip(Me.tbArpCacheTimeout, resources.GetString("tbArpCacheTimeout.ToolTip"))
        '
        'tbDefaultMtu
        '
        Me.tbDefaultMtu.AccessibleDescription = Nothing
        Me.tbDefaultMtu.AccessibleName = Nothing
        resources.ApplyResources(Me.tbDefaultMtu, "tbDefaultMtu")
        Me.tbDefaultMtu.BackgroundImage = Nothing
        Me.tbDefaultMtu.HideSelection = False
        Me.tbDefaultMtu.Name = "tbDefaultMtu"
        Me.ToolTipConfig.SetToolTip(Me.tbDefaultMtu, resources.GetString("tbDefaultMtu.ToolTip"))
        '
        'tbDefaultTtlForTcp
        '
        Me.tbDefaultTtlForTcp.AccessibleDescription = Nothing
        Me.tbDefaultTtlForTcp.AccessibleName = Nothing
        resources.ApplyResources(Me.tbDefaultTtlForTcp, "tbDefaultTtlForTcp")
        Me.tbDefaultTtlForTcp.BackgroundImage = Nothing
        Me.tbDefaultTtlForTcp.HideSelection = False
        Me.tbDefaultTtlForTcp.Name = "tbDefaultTtlForTcp"
        Me.ToolTipConfig.SetToolTip(Me.tbDefaultTtlForTcp, resources.GetString("tbDefaultTtlForTcp.ToolTip"))
        '
        'tbBootFileSize
        '
        Me.tbBootFileSize.AccessibleDescription = Nothing
        Me.tbBootFileSize.AccessibleName = Nothing
        resources.ApplyResources(Me.tbBootFileSize, "tbBootFileSize")
        Me.tbBootFileSize.BackgroundImage = Nothing
        Me.tbBootFileSize.HideSelection = False
        Me.tbBootFileSize.Name = "tbBootFileSize"
        Me.ToolTipConfig.SetToolTip(Me.tbBootFileSize, resources.GetString("tbBootFileSize.ToolTip"))
        '
        'tbDomainName
        '
        Me.tbDomainName.AccessibleDescription = Nothing
        Me.tbDomainName.AccessibleName = Nothing
        resources.ApplyResources(Me.tbDomainName, "tbDomainName")
        Me.tbDomainName.BackgroundImage = Nothing
        Me.tbDomainName.Name = "tbDomainName"
        Me.ToolTipConfig.SetToolTip(Me.tbDomainName, resources.GetString("tbDomainName.ToolTip"))
        '
        'ipBroadcastAddr
        '
        Me.ipBroadcastAddr.AccessibleDescription = Nothing
        Me.ipBroadcastAddr.AccessibleName = Nothing
        Me.ipBroadcastAddr.AllowInternalTab = True
        resources.ApplyResources(Me.ipBroadcastAddr, "ipBroadcastAddr")
        Me.ipBroadcastAddr.AutoHeight = False
        Me.ipBroadcastAddr.BackColor = System.Drawing.SystemColors.Window
        Me.ipBroadcastAddr.BackgroundImage = Nothing
        Me.ipBroadcastAddr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipBroadcastAddr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipBroadcastAddr.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipBroadcastAddr.Name = "ipBroadcastAddr"
        Me.ipBroadcastAddr.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipBroadcastAddr, resources.GetString("ipBroadcastAddr.ToolTip"))
        '
        'ipSubnetMask
        '
        Me.ipSubnetMask.AccessibleDescription = Nothing
        Me.ipSubnetMask.AccessibleName = Nothing
        Me.ipSubnetMask.AllowInternalTab = True
        resources.ApplyResources(Me.ipSubnetMask, "ipSubnetMask")
        Me.ipSubnetMask.AutoHeight = False
        Me.ipSubnetMask.BackColor = System.Drawing.SystemColors.Window
        Me.ipSubnetMask.BackgroundImage = Nothing
        Me.ipSubnetMask.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipSubnetMask.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipSubnetMask.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipSubnetMask.Name = "ipSubnetMask"
        Me.ipSubnetMask.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipSubnetMask, resources.GetString("ipSubnetMask.ToolTip"))
        '
        'ipNBNameServer1
        '
        Me.ipNBNameServer1.AccessibleDescription = Nothing
        Me.ipNBNameServer1.AccessibleName = Nothing
        Me.ipNBNameServer1.AllowInternalTab = True
        resources.ApplyResources(Me.ipNBNameServer1, "ipNBNameServer1")
        Me.ipNBNameServer1.AutoHeight = False
        Me.ipNBNameServer1.BackColor = System.Drawing.SystemColors.Window
        Me.ipNBNameServer1.BackgroundImage = Nothing
        Me.ipNBNameServer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipNBNameServer1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipNBNameServer1.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipNBNameServer1.Name = "ipNBNameServer1"
        Me.ipNBNameServer1.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipNBNameServer1, resources.GetString("ipNBNameServer1.ToolTip"))
        '
        'ipTimeServer1
        '
        Me.ipTimeServer1.AccessibleDescription = Nothing
        Me.ipTimeServer1.AccessibleName = Nothing
        Me.ipTimeServer1.AllowInternalTab = True
        resources.ApplyResources(Me.ipTimeServer1, "ipTimeServer1")
        Me.ipTimeServer1.AutoHeight = False
        Me.ipTimeServer1.BackColor = System.Drawing.SystemColors.Window
        Me.ipTimeServer1.BackgroundImage = Nothing
        Me.ipTimeServer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipTimeServer1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipTimeServer1.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipTimeServer1.Name = "ipTimeServer1"
        Me.ipTimeServer1.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipTimeServer1, resources.GetString("ipTimeServer1.ToolTip"))
        '
        'ipNetworkTimeServer1
        '
        Me.ipNetworkTimeServer1.AccessibleDescription = Nothing
        Me.ipNetworkTimeServer1.AccessibleName = Nothing
        Me.ipNetworkTimeServer1.AllowInternalTab = True
        resources.ApplyResources(Me.ipNetworkTimeServer1, "ipNetworkTimeServer1")
        Me.ipNetworkTimeServer1.AutoHeight = False
        Me.ipNetworkTimeServer1.BackColor = System.Drawing.SystemColors.Window
        Me.ipNetworkTimeServer1.BackgroundImage = Nothing
        Me.ipNetworkTimeServer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipNetworkTimeServer1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipNetworkTimeServer1.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipNetworkTimeServer1.Name = "ipNetworkTimeServer1"
        Me.ipNetworkTimeServer1.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipNetworkTimeServer1, resources.GetString("ipNetworkTimeServer1.ToolTip"))
        '
        'ipDNS2
        '
        Me.ipDNS2.AccessibleDescription = Nothing
        Me.ipDNS2.AccessibleName = Nothing
        Me.ipDNS2.AllowInternalTab = True
        resources.ApplyResources(Me.ipDNS2, "ipDNS2")
        Me.ipDNS2.AutoHeight = False
        Me.ipDNS2.BackColor = System.Drawing.SystemColors.Window
        Me.ipDNS2.BackgroundImage = Nothing
        Me.ipDNS2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipDNS2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipDNS2.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipDNS2.Name = "ipDNS2"
        Me.ipDNS2.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipDNS2, resources.GetString("ipDNS2.ToolTip"))
        '
        'ipDNS1
        '
        Me.ipDNS1.AccessibleDescription = Nothing
        Me.ipDNS1.AccessibleName = Nothing
        Me.ipDNS1.AllowInternalTab = True
        resources.ApplyResources(Me.ipDNS1, "ipDNS1")
        Me.ipDNS1.AutoHeight = False
        Me.ipDNS1.BackColor = System.Drawing.SystemColors.Window
        Me.ipDNS1.BackgroundImage = Nothing
        Me.ipDNS1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipDNS1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipDNS1.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipDNS1.Name = "ipDNS1"
        Me.ipDNS1.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipDNS1, resources.GetString("ipDNS1.ToolTip"))
        '
        'ipGateway2
        '
        Me.ipGateway2.AccessibleDescription = Nothing
        Me.ipGateway2.AccessibleName = Nothing
        Me.ipGateway2.AllowInternalTab = True
        resources.ApplyResources(Me.ipGateway2, "ipGateway2")
        Me.ipGateway2.AutoHeight = False
        Me.ipGateway2.BackColor = System.Drawing.SystemColors.Window
        Me.ipGateway2.BackgroundImage = Nothing
        Me.ipGateway2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipGateway2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipGateway2.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipGateway2.Name = "ipGateway2"
        Me.ipGateway2.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipGateway2, resources.GetString("ipGateway2.ToolTip"))
        '
        'ipGateway1
        '
        Me.ipGateway1.AccessibleDescription = Nothing
        Me.ipGateway1.AccessibleName = Nothing
        Me.ipGateway1.AllowInternalTab = True
        resources.ApplyResources(Me.ipGateway1, "ipGateway1")
        Me.ipGateway1.AutoHeight = False
        Me.ipGateway1.BackColor = System.Drawing.SystemColors.Window
        Me.ipGateway1.BackgroundImage = Nothing
        Me.ipGateway1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipGateway1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipGateway1.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipGateway1.Name = "ipGateway1"
        Me.ipGateway1.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipGateway1, resources.GetString("ipGateway1.ToolTip"))
        '
        'cbArpCacheTimeout
        '
        Me.cbArpCacheTimeout.AccessibleDescription = Nothing
        Me.cbArpCacheTimeout.AccessibleName = Nothing
        resources.ApplyResources(Me.cbArpCacheTimeout, "cbArpCacheTimeout")
        Me.cbArpCacheTimeout.BackgroundImage = Nothing
        Me.cbArpCacheTimeout.Font = Nothing
        Me.cbArpCacheTimeout.Name = "cbArpCacheTimeout"
        Me.ToolTipConfig.SetToolTip(Me.cbArpCacheTimeout, resources.GetString("cbArpCacheTimeout.ToolTip"))
        Me.cbArpCacheTimeout.UseVisualStyleBackColor = True
        '
        'cbDefaultMtu
        '
        Me.cbDefaultMtu.AccessibleDescription = Nothing
        Me.cbDefaultMtu.AccessibleName = Nothing
        resources.ApplyResources(Me.cbDefaultMtu, "cbDefaultMtu")
        Me.cbDefaultMtu.BackgroundImage = Nothing
        Me.cbDefaultMtu.Font = Nothing
        Me.cbDefaultMtu.Name = "cbDefaultMtu"
        Me.ToolTipConfig.SetToolTip(Me.cbDefaultMtu, resources.GetString("cbDefaultMtu.ToolTip"))
        Me.cbDefaultMtu.UseVisualStyleBackColor = True
        '
        'cbDefaultTtlForTcp
        '
        Me.cbDefaultTtlForTcp.AccessibleDescription = Nothing
        Me.cbDefaultTtlForTcp.AccessibleName = Nothing
        resources.ApplyResources(Me.cbDefaultTtlForTcp, "cbDefaultTtlForTcp")
        Me.cbDefaultTtlForTcp.BackgroundImage = Nothing
        Me.cbDefaultTtlForTcp.Font = Nothing
        Me.cbDefaultTtlForTcp.Name = "cbDefaultTtlForTcp"
        Me.ToolTipConfig.SetToolTip(Me.cbDefaultTtlForTcp, resources.GetString("cbDefaultTtlForTcp.ToolTip"))
        Me.cbDefaultTtlForTcp.UseVisualStyleBackColor = True
        '
        'cbEnableIpForward
        '
        Me.cbEnableIpForward.AccessibleDescription = Nothing
        Me.cbEnableIpForward.AccessibleName = Nothing
        resources.ApplyResources(Me.cbEnableIpForward, "cbEnableIpForward")
        Me.cbEnableIpForward.BackgroundImage = Nothing
        Me.cbEnableIpForward.Font = Nothing
        Me.cbEnableIpForward.Name = "cbEnableIpForward"
        Me.ToolTipConfig.SetToolTip(Me.cbEnableIpForward, resources.GetString("cbEnableIpForward.ToolTip"))
        Me.cbEnableIpForward.UseVisualStyleBackColor = True
        '
        'cbBootFileSize
        '
        Me.cbBootFileSize.AccessibleDescription = Nothing
        Me.cbBootFileSize.AccessibleName = Nothing
        resources.ApplyResources(Me.cbBootFileSize, "cbBootFileSize")
        Me.cbBootFileSize.BackgroundImage = Nothing
        Me.cbBootFileSize.Font = Nothing
        Me.cbBootFileSize.Name = "cbBootFileSize"
        Me.ToolTipConfig.SetToolTip(Me.cbBootFileSize, resources.GetString("cbBootFileSize.ToolTip"))
        Me.cbBootFileSize.UseVisualStyleBackColor = True
        '
        'cbTimeServers
        '
        Me.cbTimeServers.AccessibleDescription = Nothing
        Me.cbTimeServers.AccessibleName = Nothing
        resources.ApplyResources(Me.cbTimeServers, "cbTimeServers")
        Me.cbTimeServers.BackgroundImage = Nothing
        Me.cbTimeServers.Font = Nothing
        Me.cbTimeServers.Name = "cbTimeServers"
        Me.ToolTipConfig.SetToolTip(Me.cbTimeServers, resources.GetString("cbTimeServers.ToolTip"))
        Me.cbTimeServers.UseVisualStyleBackColor = True
        '
        'cbNetworkTimeServers
        '
        Me.cbNetworkTimeServers.AccessibleDescription = Nothing
        Me.cbNetworkTimeServers.AccessibleName = Nothing
        resources.ApplyResources(Me.cbNetworkTimeServers, "cbNetworkTimeServers")
        Me.cbNetworkTimeServers.BackgroundImage = Nothing
        Me.cbNetworkTimeServers.Font = Nothing
        Me.cbNetworkTimeServers.Name = "cbNetworkTimeServers"
        Me.ToolTipConfig.SetToolTip(Me.cbNetworkTimeServers, resources.GetString("cbNetworkTimeServers.ToolTip"))
        Me.cbNetworkTimeServers.UseVisualStyleBackColor = True
        '
        'cbDNS
        '
        Me.cbDNS.AccessibleDescription = Nothing
        Me.cbDNS.AccessibleName = Nothing
        resources.ApplyResources(Me.cbDNS, "cbDNS")
        Me.cbDNS.BackgroundImage = Nothing
        Me.cbDNS.Font = Nothing
        Me.cbDNS.Name = "cbDNS"
        Me.ToolTipConfig.SetToolTip(Me.cbDNS, resources.GetString("cbDNS.ToolTip"))
        Me.cbDNS.UseVisualStyleBackColor = True
        '
        'cbSubnetMask
        '
        Me.cbSubnetMask.AccessibleDescription = Nothing
        Me.cbSubnetMask.AccessibleName = Nothing
        resources.ApplyResources(Me.cbSubnetMask, "cbSubnetMask")
        Me.cbSubnetMask.BackgroundImage = Nothing
        Me.cbSubnetMask.Font = Nothing
        Me.cbSubnetMask.Name = "cbSubnetMask"
        Me.ToolTipConfig.SetToolTip(Me.cbSubnetMask, resources.GetString("cbSubnetMask.ToolTip"))
        Me.cbSubnetMask.UseVisualStyleBackColor = True
        '
        'cbBroadcastAddr
        '
        Me.cbBroadcastAddr.AccessibleDescription = Nothing
        Me.cbBroadcastAddr.AccessibleName = Nothing
        resources.ApplyResources(Me.cbBroadcastAddr, "cbBroadcastAddr")
        Me.cbBroadcastAddr.BackgroundImage = Nothing
        Me.cbBroadcastAddr.Font = Nothing
        Me.cbBroadcastAddr.Name = "cbBroadcastAddr"
        Me.ToolTipConfig.SetToolTip(Me.cbBroadcastAddr, resources.GetString("cbBroadcastAddr.ToolTip"))
        Me.cbBroadcastAddr.UseVisualStyleBackColor = True
        '
        'cbDomainName
        '
        Me.cbDomainName.AccessibleDescription = Nothing
        Me.cbDomainName.AccessibleName = Nothing
        resources.ApplyResources(Me.cbDomainName, "cbDomainName")
        Me.cbDomainName.BackgroundImage = Nothing
        Me.cbDomainName.Font = Nothing
        Me.cbDomainName.Name = "cbDomainName"
        Me.ToolTipConfig.SetToolTip(Me.cbDomainName, resources.GetString("cbDomainName.ToolTip"))
        Me.cbDomainName.UseVisualStyleBackColor = True
        '
        'cbNBNameServers
        '
        Me.cbNBNameServers.AccessibleDescription = Nothing
        Me.cbNBNameServers.AccessibleName = Nothing
        resources.ApplyResources(Me.cbNBNameServers, "cbNBNameServers")
        Me.cbNBNameServers.BackgroundImage = Nothing
        Me.cbNBNameServers.Font = Nothing
        Me.cbNBNameServers.Name = "cbNBNameServers"
        Me.ToolTipConfig.SetToolTip(Me.cbNBNameServers, resources.GetString("cbNBNameServers.ToolTip"))
        Me.cbNBNameServers.UseVisualStyleBackColor = True
        '
        'cbGateways
        '
        Me.cbGateways.AccessibleDescription = Nothing
        Me.cbGateways.AccessibleName = Nothing
        resources.ApplyResources(Me.cbGateways, "cbGateways")
        Me.cbGateways.BackgroundImage = Nothing
        Me.cbGateways.Font = Nothing
        Me.cbGateways.Name = "cbGateways"
        Me.ToolTipConfig.SetToolTip(Me.cbGateways, resources.GetString("cbGateways.ToolTip"))
        Me.cbGateways.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.AccessibleDescription = Nothing
        Me.btnOK.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.BackgroundImage = Nothing
        Me.btnOK.Font = Nothing
        Me.btnOK.Name = "btnOK"
        Me.ToolTipConfig.SetToolTip(Me.btnOK, resources.GetString("btnOK.ToolTip"))
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'gbGeneral
        '
        Me.gbGeneral.AccessibleDescription = Nothing
        Me.gbGeneral.AccessibleName = Nothing
        resources.ApplyResources(Me.gbGeneral, "gbGeneral")
        Me.gbGeneral.BackgroundImage = Nothing
        Me.gbGeneral.Controls.Add(Me.ipServerId)
        Me.gbGeneral.Controls.Add(Me.lblServerID)
        Me.gbGeneral.Controls.Add(Me.cbPingCheck)
        Me.gbGeneral.Controls.Add(Me.cbDenyUnknownClients)
        Me.gbGeneral.Controls.Add(Me.tbLeaseMaximal)
        Me.gbGeneral.Controls.Add(Me.tbAddrRangeEnd)
        Me.gbGeneral.Controls.Add(Me.tbAddrRangeStart)
        Me.gbGeneral.Controls.Add(Me.tbLeaseDefault)
        Me.gbGeneral.Controls.Add(Me.lblLeaseMaximal)
        Me.gbGeneral.Controls.Add(Me.lblLeaseDefault)
        Me.gbGeneral.Controls.Add(Me.lblInterfaces)
        Me.gbGeneral.Controls.Add(Me.lbInterfaces)
        Me.gbGeneral.Controls.Add(Me.lblBootFilename)
        Me.gbGeneral.Controls.Add(Me.lblAddrRange)
        Me.gbGeneral.Controls.Add(Me.tbBootFilename)
        Me.gbGeneral.Font = Nothing
        Me.gbGeneral.Name = "gbGeneral"
        Me.gbGeneral.TabStop = False
        Me.ToolTipConfig.SetToolTip(Me.gbGeneral, resources.GetString("gbGeneral.ToolTip"))
        '
        'ipServerId
        '
        Me.ipServerId.AccessibleDescription = Nothing
        Me.ipServerId.AccessibleName = Nothing
        Me.ipServerId.AllowInternalTab = True
        resources.ApplyResources(Me.ipServerId, "ipServerId")
        Me.ipServerId.AutoHeight = False
        Me.ipServerId.BackColor = System.Drawing.SystemColors.Window
        Me.ipServerId.BackgroundImage = Nothing
        Me.ipServerId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipServerId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipServerId.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipServerId.Name = "ipServerId"
        Me.ipServerId.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipServerId, resources.GetString("ipServerId.ToolTip"))
        '
        'lblServerID
        '
        Me.lblServerID.AccessibleDescription = Nothing
        Me.lblServerID.AccessibleName = Nothing
        resources.ApplyResources(Me.lblServerID, "lblServerID")
        Me.lblServerID.Font = Nothing
        Me.lblServerID.Name = "lblServerID"
        Me.ToolTipConfig.SetToolTip(Me.lblServerID, resources.GetString("lblServerID.ToolTip"))
        '
        'cbPingCheck
        '
        Me.cbPingCheck.AccessibleDescription = Nothing
        Me.cbPingCheck.AccessibleName = Nothing
        resources.ApplyResources(Me.cbPingCheck, "cbPingCheck")
        Me.cbPingCheck.BackgroundImage = Nothing
        Me.cbPingCheck.Font = Nothing
        Me.cbPingCheck.Name = "cbPingCheck"
        Me.ToolTipConfig.SetToolTip(Me.cbPingCheck, resources.GetString("cbPingCheck.ToolTip"))
        Me.cbPingCheck.UseVisualStyleBackColor = True
        '
        'cbDenyUnknownClients
        '
        Me.cbDenyUnknownClients.AccessibleDescription = Nothing
        Me.cbDenyUnknownClients.AccessibleName = Nothing
        resources.ApplyResources(Me.cbDenyUnknownClients, "cbDenyUnknownClients")
        Me.cbDenyUnknownClients.BackgroundImage = Nothing
        Me.cbDenyUnknownClients.Font = Nothing
        Me.cbDenyUnknownClients.Name = "cbDenyUnknownClients"
        Me.ToolTipConfig.SetToolTip(Me.cbDenyUnknownClients, resources.GetString("cbDenyUnknownClients.ToolTip"))
        Me.cbDenyUnknownClients.UseVisualStyleBackColor = True
        '
        'tbLeaseMaximal
        '
        Me.tbLeaseMaximal.AccessibleDescription = Nothing
        Me.tbLeaseMaximal.AccessibleName = Nothing
        resources.ApplyResources(Me.tbLeaseMaximal, "tbLeaseMaximal")
        Me.tbLeaseMaximal.BackgroundImage = Nothing
        Me.tbLeaseMaximal.HideSelection = False
        Me.tbLeaseMaximal.Name = "tbLeaseMaximal"
        Me.ToolTipConfig.SetToolTip(Me.tbLeaseMaximal, resources.GetString("tbLeaseMaximal.ToolTip"))
        '
        'tbAddrRangeEnd
        '
        Me.tbAddrRangeEnd.AccessibleDescription = Nothing
        Me.tbAddrRangeEnd.AccessibleName = Nothing
        Me.tbAddrRangeEnd.AllowInternalTab = True
        resources.ApplyResources(Me.tbAddrRangeEnd, "tbAddrRangeEnd")
        Me.tbAddrRangeEnd.AutoHeight = False
        Me.tbAddrRangeEnd.BackColor = System.Drawing.SystemColors.Window
        Me.tbAddrRangeEnd.BackgroundImage = Nothing
        Me.tbAddrRangeEnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.tbAddrRangeEnd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbAddrRangeEnd.MinimumSize = New System.Drawing.Size(126, 20)
        Me.tbAddrRangeEnd.Name = "tbAddrRangeEnd"
        Me.tbAddrRangeEnd.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.tbAddrRangeEnd, resources.GetString("tbAddrRangeEnd.ToolTip"))
        '
        'tbAddrRangeStart
        '
        Me.tbAddrRangeStart.AccessibleDescription = Nothing
        Me.tbAddrRangeStart.AccessibleName = Nothing
        Me.tbAddrRangeStart.AllowInternalTab = True
        resources.ApplyResources(Me.tbAddrRangeStart, "tbAddrRangeStart")
        Me.tbAddrRangeStart.AutoHeight = False
        Me.tbAddrRangeStart.BackColor = System.Drawing.SystemColors.Window
        Me.tbAddrRangeStart.BackgroundImage = Nothing
        Me.tbAddrRangeStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.tbAddrRangeStart.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.tbAddrRangeStart.MinimumSize = New System.Drawing.Size(126, 20)
        Me.tbAddrRangeStart.Name = "tbAddrRangeStart"
        Me.tbAddrRangeStart.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.tbAddrRangeStart, resources.GetString("tbAddrRangeStart.ToolTip"))
        '
        'tbLeaseDefault
        '
        Me.tbLeaseDefault.AccessibleDescription = Nothing
        Me.tbLeaseDefault.AccessibleName = Nothing
        resources.ApplyResources(Me.tbLeaseDefault, "tbLeaseDefault")
        Me.tbLeaseDefault.BackgroundImage = Nothing
        Me.tbLeaseDefault.HideSelection = False
        Me.tbLeaseDefault.Name = "tbLeaseDefault"
        Me.ToolTipConfig.SetToolTip(Me.tbLeaseDefault, resources.GetString("tbLeaseDefault.ToolTip"))
        '
        'lblLeaseMaximal
        '
        Me.lblLeaseMaximal.AccessibleDescription = Nothing
        Me.lblLeaseMaximal.AccessibleName = Nothing
        resources.ApplyResources(Me.lblLeaseMaximal, "lblLeaseMaximal")
        Me.lblLeaseMaximal.Font = Nothing
        Me.lblLeaseMaximal.Name = "lblLeaseMaximal"
        Me.ToolTipConfig.SetToolTip(Me.lblLeaseMaximal, resources.GetString("lblLeaseMaximal.ToolTip"))
        '
        'lblLeaseDefault
        '
        Me.lblLeaseDefault.AccessibleDescription = Nothing
        Me.lblLeaseDefault.AccessibleName = Nothing
        resources.ApplyResources(Me.lblLeaseDefault, "lblLeaseDefault")
        Me.lblLeaseDefault.Font = Nothing
        Me.lblLeaseDefault.Name = "lblLeaseDefault"
        Me.ToolTipConfig.SetToolTip(Me.lblLeaseDefault, resources.GetString("lblLeaseDefault.ToolTip"))
        '
        'lblInterfaces
        '
        Me.lblInterfaces.AccessibleDescription = Nothing
        Me.lblInterfaces.AccessibleName = Nothing
        resources.ApplyResources(Me.lblInterfaces, "lblInterfaces")
        Me.lblInterfaces.Font = Nothing
        Me.lblInterfaces.Name = "lblInterfaces"
        Me.ToolTipConfig.SetToolTip(Me.lblInterfaces, resources.GetString("lblInterfaces.ToolTip"))
        '
        'lbInterfaces
        '
        Me.lbInterfaces.AccessibleDescription = Nothing
        Me.lbInterfaces.AccessibleName = Nothing
        resources.ApplyResources(Me.lbInterfaces, "lbInterfaces")
        Me.lbInterfaces.BackgroundImage = Nothing
        Me.lbInterfaces.FormattingEnabled = True
        Me.lbInterfaces.Name = "lbInterfaces"
        Me.lbInterfaces.Sorted = True
        Me.ToolTipConfig.SetToolTip(Me.lbInterfaces, resources.GetString("lbInterfaces.ToolTip"))
        '
        'lblBootFilename
        '
        Me.lblBootFilename.AccessibleDescription = Nothing
        Me.lblBootFilename.AccessibleName = Nothing
        resources.ApplyResources(Me.lblBootFilename, "lblBootFilename")
        Me.lblBootFilename.Font = Nothing
        Me.lblBootFilename.Name = "lblBootFilename"
        Me.ToolTipConfig.SetToolTip(Me.lblBootFilename, resources.GetString("lblBootFilename.ToolTip"))
        '
        'lblAddrRange
        '
        Me.lblAddrRange.AccessibleDescription = Nothing
        Me.lblAddrRange.AccessibleName = Nothing
        resources.ApplyResources(Me.lblAddrRange, "lblAddrRange")
        Me.lblAddrRange.Font = Nothing
        Me.lblAddrRange.Name = "lblAddrRange"
        Me.ToolTipConfig.SetToolTip(Me.lblAddrRange, resources.GetString("lblAddrRange.ToolTip"))
        '
        'tbBootFilename
        '
        Me.tbBootFilename.AccessibleDescription = Nothing
        Me.tbBootFilename.AccessibleName = Nothing
        resources.ApplyResources(Me.tbBootFilename, "tbBootFilename")
        Me.tbBootFilename.BackgroundImage = Nothing
        Me.tbBootFilename.Name = "tbBootFilename"
        Me.ToolTipConfig.SetToolTip(Me.tbBootFilename, resources.GetString("tbBootFilename.ToolTip"))
        '
        'gbClients
        '
        Me.gbClients.AccessibleDescription = Nothing
        Me.gbClients.AccessibleName = Nothing
        resources.ApplyResources(Me.gbClients, "gbClients")
        Me.gbClients.BackgroundImage = Nothing
        Me.gbClients.Controls.Add(Me.btnUserClear)
        Me.gbClients.Controls.Add(Me.btnUserRemove)
        Me.gbClients.Controls.Add(Me.btnUserAdd)
        Me.gbClients.Controls.Add(Me.tbNewClientHostName)
        Me.gbClients.Controls.Add(Me.ipNewClientIP)
        Me.gbClients.Controls.Add(Me.lvClients)
        Me.gbClients.Controls.Add(Me.tbNewClientMac)
        Me.gbClients.Font = Nothing
        Me.gbClients.Name = "gbClients"
        Me.gbClients.TabStop = False
        Me.ToolTipConfig.SetToolTip(Me.gbClients, resources.GetString("gbClients.ToolTip"))
        '
        'btnUserClear
        '
        Me.btnUserClear.AccessibleDescription = Nothing
        Me.btnUserClear.AccessibleName = Nothing
        resources.ApplyResources(Me.btnUserClear, "btnUserClear")
        Me.btnUserClear.BackgroundImage = Nothing
        Me.btnUserClear.Font = Nothing
        Me.btnUserClear.Name = "btnUserClear"
        Me.ToolTipConfig.SetToolTip(Me.btnUserClear, resources.GetString("btnUserClear.ToolTip"))
        Me.btnUserClear.UseVisualStyleBackColor = True
        '
        'btnUserRemove
        '
        Me.btnUserRemove.AccessibleDescription = Nothing
        Me.btnUserRemove.AccessibleName = Nothing
        resources.ApplyResources(Me.btnUserRemove, "btnUserRemove")
        Me.btnUserRemove.BackgroundImage = Nothing
        Me.btnUserRemove.Font = Nothing
        Me.btnUserRemove.Name = "btnUserRemove"
        Me.ToolTipConfig.SetToolTip(Me.btnUserRemove, resources.GetString("btnUserRemove.ToolTip"))
        Me.btnUserRemove.UseVisualStyleBackColor = True
        '
        'btnUserAdd
        '
        Me.btnUserAdd.AccessibleDescription = Nothing
        Me.btnUserAdd.AccessibleName = Nothing
        resources.ApplyResources(Me.btnUserAdd, "btnUserAdd")
        Me.btnUserAdd.BackgroundImage = Nothing
        Me.btnUserAdd.Font = Nothing
        Me.btnUserAdd.Name = "btnUserAdd"
        Me.ToolTipConfig.SetToolTip(Me.btnUserAdd, resources.GetString("btnUserAdd.ToolTip"))
        Me.btnUserAdd.UseVisualStyleBackColor = True
        '
        'tbNewClientHostName
        '
        Me.tbNewClientHostName.AccessibleDescription = Nothing
        Me.tbNewClientHostName.AccessibleName = Nothing
        resources.ApplyResources(Me.tbNewClientHostName, "tbNewClientHostName")
        Me.tbNewClientHostName.BackgroundImage = Nothing
        Me.tbNewClientHostName.Name = "tbNewClientHostName"
        Me.ToolTipConfig.SetToolTip(Me.tbNewClientHostName, resources.GetString("tbNewClientHostName.ToolTip"))
        '
        'ipNewClientIP
        '
        Me.ipNewClientIP.AccessibleDescription = Nothing
        Me.ipNewClientIP.AccessibleName = Nothing
        Me.ipNewClientIP.AllowInternalTab = True
        resources.ApplyResources(Me.ipNewClientIP, "ipNewClientIP")
        Me.ipNewClientIP.AutoHeight = False
        Me.ipNewClientIP.BackColor = System.Drawing.SystemColors.Window
        Me.ipNewClientIP.BackgroundImage = Nothing
        Me.ipNewClientIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ipNewClientIP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ipNewClientIP.MinimumSize = New System.Drawing.Size(126, 20)
        Me.ipNewClientIP.Name = "ipNewClientIP"
        Me.ipNewClientIP.ReadOnly = False
        Me.ToolTipConfig.SetToolTip(Me.ipNewClientIP, resources.GetString("ipNewClientIP.ToolTip"))
        '
        'lvClients
        '
        Me.lvClients.AccessibleDescription = Nothing
        Me.lvClients.AccessibleName = Nothing
        resources.ApplyResources(Me.lvClients, "lvClients")
        Me.lvClients.BackgroundImage = Nothing
        Me.lvClients.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lbchIP, Me.lbchMAC, Me.lbchHostname})
        Me.lvClients.FullRowSelect = True
        Me.lvClients.GridLines = True
        Me.lvClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvClients.HideSelection = False
        Me.lvClients.LabelEdit = True
        Me.lvClients.MultiSelect = False
        Me.lvClients.Name = "lvClients"
        Me.lvClients.ShowGroups = False
        Me.lvClients.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ToolTipConfig.SetToolTip(Me.lvClients, resources.GetString("lvClients.ToolTip"))
        Me.lvClients.UseCompatibleStateImageBehavior = False
        Me.lvClients.View = System.Windows.Forms.View.Details
        '
        'lbchIP
        '
        resources.ApplyResources(Me.lbchIP, "lbchIP")
        '
        'lbchMAC
        '
        resources.ApplyResources(Me.lbchMAC, "lbchMAC")
        '
        'lbchHostname
        '
        resources.ApplyResources(Me.lbchHostname, "lbchHostname")
        '
        'tbNewClientMac
        '
        Me.tbNewClientMac.AccessibleDescription = Nothing
        Me.tbNewClientMac.AccessibleName = Nothing
        resources.ApplyResources(Me.tbNewClientMac, "tbNewClientMac")
        Me.tbNewClientMac.BackgroundImage = Nothing
        Me.tbNewClientMac.Name = "tbNewClientMac"
        Me.ToolTipConfig.SetToolTip(Me.tbNewClientMac, resources.GetString("tbNewClientMac.ToolTip"))
        '
        'ToolTipConfig
        '
        Me.ToolTipConfig.AutoPopDelay = 2000
        Me.ToolTipConfig.InitialDelay = 500
        Me.ToolTipConfig.ReshowDelay = 100
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = Nothing
        Me.btnCancel.Name = "btnCancel"
        Me.ToolTipConfig.SetToolTip(Me.btnCancel, resources.GetString("btnCancel.ToolTip"))
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmSubnetConfig
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BackgroundImage = Nothing
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.gbClients)
        Me.Controls.Add(Me.gbGeneral)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbOptions)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSubnetConfig"
        Me.ShowInTaskbar = False
        Me.ToolTipConfig.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.gbOptions.ResumeLayout(False)
        Me.gbOptions.PerformLayout()
        CType(Me.imgSideImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbGeneral.ResumeLayout(False)
        Me.gbGeneral.PerformLayout()
        Me.gbClients.ResumeLayout(False)
        Me.gbClients.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbOptions As System.Windows.Forms.GroupBox
    Friend WithEvents cbArpCacheTimeout As System.Windows.Forms.CheckBox
    Friend WithEvents cbDefaultMtu As System.Windows.Forms.CheckBox
    Friend WithEvents cbDefaultTtlForTcp As System.Windows.Forms.CheckBox
    Friend WithEvents cbEnableIpForward As System.Windows.Forms.CheckBox
    Friend WithEvents cbBootFileSize As System.Windows.Forms.CheckBox
    Friend WithEvents cbTimeServers As System.Windows.Forms.CheckBox
    Friend WithEvents cbNetworkTimeServers As System.Windows.Forms.CheckBox
    Friend WithEvents cbDNS As System.Windows.Forms.CheckBox
    Friend WithEvents cbSubnetMask As System.Windows.Forms.CheckBox
    Friend WithEvents cbBroadcastAddr As System.Windows.Forms.CheckBox
    Friend WithEvents cbDomainName As System.Windows.Forms.CheckBox
    Friend WithEvents cbNBNameServers As System.Windows.Forms.CheckBox
    Friend WithEvents cbGateways As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents gbGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents tbLeaseMaximal As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbLeaseDefault As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lblLeaseMaximal As System.Windows.Forms.Label
    Friend WithEvents lblLeaseDefault As System.Windows.Forms.Label
    Friend WithEvents lblInterfaces As System.Windows.Forms.Label
    Friend WithEvents lbInterfaces As System.Windows.Forms.ListBox
    Friend WithEvents lblBootFilename As System.Windows.Forms.Label
    Friend WithEvents lblAddrRange As System.Windows.Forms.Label
    Friend WithEvents tbBootFilename As System.Windows.Forms.TextBox
    Friend WithEvents cbDenyUnknownClients As System.Windows.Forms.CheckBox
    Friend WithEvents ipGateway1 As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipGateway2 As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipDNS2 As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipDNS1 As IPAddressControlLib.IPAddressControl
    Friend WithEvents tbAddrRangeEnd As IPAddressControlLib.IPAddressControl
    Friend WithEvents tbAddrRangeStart As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipNBNameServer1 As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipTimeServer1 As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipNetworkTimeServer1 As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipBroadcastAddr As IPAddressControlLib.IPAddressControl
    Friend WithEvents ipSubnetMask As IPAddressControlLib.IPAddressControl
    Friend WithEvents tbDefaultTtlForUdp As System.Windows.Forms.MaskedTextBox
    Friend WithEvents cbDefaultTtlForUdp As System.Windows.Forms.CheckBox
    Friend WithEvents tbArpCacheTimeout As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbDefaultMtu As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbDefaultTtlForTcp As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbBootFileSize As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbDomainName As System.Windows.Forms.TextBox
    Friend WithEvents gbClients As System.Windows.Forms.GroupBox
    Friend WithEvents tbNewClientMac As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lvClients As System.Windows.Forms.ListView
    Friend WithEvents lbchIP As System.Windows.Forms.ColumnHeader
    Friend WithEvents lbchMAC As System.Windows.Forms.ColumnHeader
    Friend WithEvents lbchHostname As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbNewClientHostName As System.Windows.Forms.TextBox
    Friend WithEvents ipNewClientIP As IPAddressControlLib.IPAddressControl
    Friend WithEvents btnUserAdd As System.Windows.Forms.Button
    Friend WithEvents btnUserRemove As System.Windows.Forms.Button
    Friend WithEvents btnUserClear As System.Windows.Forms.Button
    Friend WithEvents ToolTipConfig As System.Windows.Forms.ToolTip
    Friend WithEvents cbPingCheck As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents imgSideImage As System.Windows.Forms.PictureBox
    Friend WithEvents lblServerID As System.Windows.Forms.Label
    Friend WithEvents ipServerId As IPAddressControlLib.IPAddressControl

End Class

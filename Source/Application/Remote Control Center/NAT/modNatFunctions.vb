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


Module modNatFunctions

    Public configFile As String = ""
    Public configData As New natConfig

    Public Sub InitializeFromConfig()
        configFile = remoteNatObject.GetConfig()
        configData.ReadConfig(configFile)

        frmMain.pageNAT_lvInternal.Items.Clear()
        frmMain.pageNAT_lvExternal.Items.Clear()

        Dim interfaces As Dictionary(Of String, String) = remoteNatObject.GetInterfaces
        For Each i As KeyValuePair(Of String, String) In interfaces
            frmMain.pageNAT_lvExternal.Items.Add(New ListViewItem(New String() {"", i.Key, i.Value}))
            frmMain.pageNAT_lvInternal.Items.Add(New ListViewItem(New String() {"", i.Key, i.Value}))

            If i.Value = configData.iGREEN Then
                frmMain.pageNAT_lvInternal.Items(frmMain.pageNAT_lvInternal.Items.Count - 1).Selected = True
            End If
            If i.Value = configData.iRED Then
                frmMain.pageNAT_lvExternal.Items(frmMain.pageNAT_lvExternal.Items.Count - 1).Selected = True
            End If
        Next

        frmMain.pageNAT_cbEnableTTL.Checked = Not configData.optTTL
        frmMain.pageNAT_cbEnableNAT.Checked = configData.optNAT

        frmMain.pageFirewall_lvRules.Items.Clear()
        For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_FILTER).FORWARD
            Dim srcAddr As String = "any"
            Dim srcPort As String = "any"
            Dim dstAddr As String = "any"
            Dim dstPort As String = "any"
            Dim protocol As String = "any"
            If chain.packetProtocol <> NetGate.Protocol.NONE Then protocol = chain.packetProtocol.ToString
            If chain.sourceAddress IsNot Nothing Then srcAddr = chain.sourceAddress.ToString
            If chain.destinationAddress IsNot Nothing Then dstAddr = chain.destinationAddress.ToString
            If chain.sourcePort <> 0 Then srcPort = chain.sourcePort
            If chain.destinationPort <> 0 Then dstPort = chain.destinationPort
            frmMain.pageFirewall_lvRules.Items.Add(New ListViewItem(New String() {"", srcAddr, srcPort, dstAddr, dstPort, protocol, chain.JUMP}))
        Next

        frmMain.pageTrafficShaping_lvRules.Items.Clear()
        For Each chain As natConfigChain In modNatFunctions.configData.m_Table(natConfig.TABLE_NAT).FORWARD
            If chain.JUMP <> "LIMIT" Then Continue For

            Dim srcAddr As String = "any"
            Dim srcPort As String = "any"
            Dim dstAddr As String = "any"
            Dim dstPort As String = "any"
            Dim protocol As String = "any"
            If chain.packetProtocol <> NetGate.Protocol.NONE Then protocol = chain.packetProtocol.ToString
            If chain.sourceAddress IsNot Nothing Then srcAddr = chain.sourceAddress.ToString
            If chain.destinationAddress IsNot Nothing Then dstAddr = chain.destinationAddress.ToString
            If chain.sourcePort <> 0 Then srcPort = chain.sourcePort
            If chain.destinationPort <> 0 Then dstPort = chain.destinationPort
            frmMain.pageTrafficShaping_lvRules.Items.Add(New ListViewItem(New String() {"", srcAddr, srcPort, dstAddr, dstPort, protocol, chain.packetLimit}))
        Next
    End Sub
    Public Sub SaveConfig()
        configData.iGREEN = frmMain.pageNAT_lvInternal.SelectedItems(0).SubItems(2).Text
        configData.iRED = frmMain.pageNAT_lvExternal.SelectedItems(0).SubItems(2).Text
        configData.extGatewayIP = remoteNatObject.GetGateway

        If configData.extGatewayIP Is Nothing Then configData.extGatewayIP = IPAddress.Loopback

        configFile = configData.ToString
        configFile = "natd -t mangle --flush" & vbNewLine & "natd -t filter --flush" & vbNewLine & "natd -t nat --flush" & vbNewLine & vbNewLine & configFile
        remoteNatObject.SetConfig(configFile)
    End Sub



End Module


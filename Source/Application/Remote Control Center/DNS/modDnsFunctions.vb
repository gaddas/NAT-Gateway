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


Module modDnsFunctions

    Public configFile As String = ""
    Public configData As New dnsConfig

    Public Sub InitializeFromConfig()
        configFile = remoteDnsObject.GetConfig()
        configData.ReadConfig(configFile)

        frmMain.pageDNS_lvDNS.Items.Clear()
        For Each h As KeyValuePair(Of String, IPAddress) In configData.m_Hosts
            frmMain.pageDNS_lvDNS.Items.Add(New ListViewItem(New String() {"", h.Value.ToString, h.Key}))
            frmMain.pageDNS_lvDNS.Items(frmMain.pageDNS_lvDNS.Items.Count - 1).ImageIndex = 3
        Next

        frmMain.pageDNS_txtDNS.Text = ""
        For Each s As IPAddress In configData.m_Servers
            frmMain.pageDNS_txtDNS.Text += s.ToString & ";"
        Next

        frmMain.pageDNS_rbUseSystemDefaultDNS.Checked = configData.m_UseDefault
        frmMain.pageDNS_rbUseSpecifiedDNS.Checked = Not configData.m_UseDefault
        frmMain.pageDNS_cbEnableCache.Checked = configData.m_UseCache
        frmMain.pageDNS_txtCacheRecords.Text = configData.m_CacheSize
    End Sub
    Public Sub BuildConfig()
        configData.m_UseDefault = frmMain.pageDNS_rbUseSystemDefaultDNS.Checked
        configData.m_UseCache = frmMain.pageDNS_cbEnableCache.Checked
        configData.m_CacheSize = frmMain.pageDNS_txtCacheRecords.Text
        configData.m_Servers.Clear()
        configData.m_Hosts.Clear()

        Dim ss As String() = frmMain.pageDNS_txtDNS.Text.Split(";")
        For Each s As String In ss
            s = s.Trim
            If s <> String.Empty Then
                configData.m_Servers.Add(IPAddress.Parse(s))
            End If
        Next

        For Each i As ListViewItem In frmMain.pageDNS_lvDNS.Items
            Dim ip As IPAddress
            Dim h As String

            ip = IPAddress.Parse(i.SubItems(1).Text)
            h = i.SubItems(2).Text
            configData.m_Hosts.Add(h, ip)
        Next

        configFile = configData.ToString
        remoteDnsObject.SetConfig(configFile)
    End Sub

End Module


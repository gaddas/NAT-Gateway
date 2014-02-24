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


Imports System.Windows.Forms


Public Class frmSubnetConfig

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If lbInterfaces.SelectedItem Is Nothing Then
            Select Case MessageBox.Show(My.Resources.YouMustSelectOneOfTheAvailbleInterfaces, My.Resources.CaptionInformation, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk)
                Case Windows.Forms.DialogResult.Cancel, Windows.Forms.DialogResult.No
                    Return
                Case Windows.Forms.DialogResult.Yes
                    lbInterfaces.SelectedIndex = 0
            End Select
        End If

        If ipServerId.Text = "..." Then
            MessageBox.Show(My.Resources.ServerIdIsMissing, My.Resources.CaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnUserAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserAdd.Click
        lvClients.Items.Add(New ListViewItem(New String() {ipNewClientIP.Text, tbNewClientMac.Text, tbNewClientHostName.Text}))
    End Sub

    Private Sub btnUserRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserRemove.Click
        For Each item As ListViewItem In lvClients.SelectedItems
            item.Remove()
        Next
    End Sub

    Private Sub btnUserClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserClear.Click
        lvClients.Items.Clear()
    End Sub
    
    Private Sub tbDefaultTtlForIp_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles tbDefaultTtlForTcp.Validating
        If Trim(tbDefaultTtlForTcp.Text) <> String.Empty AndAlso (tbDefaultTtlForTcp.Text > 255 OrElse tbDefaultTtlForTcp.Text < 1) Then
            e.Cancel = True
            tbDefaultTtlForTcp.Text = String.Empty
        End If
    End Sub

    Private Sub tbDefaultTtlForUdp_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles tbDefaultTtlForUdp.Validating
        If Trim(tbDefaultTtlForUdp.Text) <> String.Empty AndAlso (tbDefaultTtlForUdp.Text > 255 OrElse tbDefaultTtlForUdp.Text < 1) Then
            e.Cancel = True
            tbDefaultTtlForTcp.Text = String.Empty
        End If
    End Sub

    Private Sub tbDefaultMtu_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles tbDefaultMtu.Validating
        If Trim(tbDefaultMtu.Text) <> String.Empty AndAlso tbDefaultMtu.Text < 67 Then
            e.Cancel = True
            tbDefaultMtu.Text = String.Empty
        End If
    End Sub

    Private Sub gbClients_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbClients.Enter

    End Sub
End Class

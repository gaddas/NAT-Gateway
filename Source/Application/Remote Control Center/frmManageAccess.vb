Imports System.Windows.Forms

Public Class frmManageAccess

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmManageAccess_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmManageAccess_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Dim f As New IO.FileStream("remote.conf", IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim r As New IO.StreamReader(f)

        Dim s As String
        Dim user, pass As String

        While Not r.EndOfStream
            s = r.ReadLine
            If s.Trim = "" Then Continue While

            user = s.Remove(s.IndexOf("|")).ToLower
            pass = s.Substring(s.IndexOf("|") + 1)

            lvUsers.Items.Add(New ListViewItem(New String() {"", user, pass}))
            lvUsers.Items(lvUsers.Items.Count - 1).ImageIndex = 0
        End While

        r.Close()
        f.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim f As New IO.FileStream("remote.conf", IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)
        Dim w As New IO.StreamWriter(f)

        For Each i As ListViewItem In lvUsers.Items
            w.WriteLine(i.SubItems(1).Text & "|" & i.SubItems(2).Text)
        Next

        w.Flush()
        w.Close()
        f.Close()
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub txtUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged
        txtUsername.Text = txtUsername.Text.Replace("|", "")
    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged
        txtPassword.Text = txtPassword.Text.Replace("|", "")
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        lvUsers.Items.Add(New ListViewItem(New String() {"", txtUsername.Text, txtPassword.Text}))
        lvUsers.Items(lvUsers.Items.Count - 1).ImageIndex = 0
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        For Each i As ListViewItem In lvUsers.SelectedItems
            i.Remove()
        Next
    End Sub

    Private Sub frmManageAccess_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub
End Class

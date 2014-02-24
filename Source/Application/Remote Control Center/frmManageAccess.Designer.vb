<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManageAccess
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManageAccess))
        Me.lvUsers = New System.Windows.Forms.ListView
        Me.chImage = New System.Windows.Forms.ColumnHeader
        Me.chUsername = New System.Windows.Forms.ColumnHeader
        Me.chPassword = New System.Windows.Forms.ColumnHeader
        Me.ImageListUsers = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lvUsers
        '
        Me.lvUsers.AccessibleDescription = Nothing
        Me.lvUsers.AccessibleName = Nothing
        resources.ApplyResources(Me.lvUsers, "lvUsers")
        Me.lvUsers.BackgroundImage = Nothing
        Me.lvUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chImage, Me.chUsername, Me.chPassword})
        Me.lvUsers.Font = Nothing
        Me.lvUsers.FullRowSelect = True
        Me.lvUsers.GridLines = True
        Me.lvUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvUsers.LabelEdit = True
        Me.lvUsers.MultiSelect = False
        Me.lvUsers.Name = "lvUsers"
        Me.lvUsers.SmallImageList = Me.ImageListUsers
        Me.lvUsers.UseCompatibleStateImageBehavior = False
        Me.lvUsers.View = System.Windows.Forms.View.Details
        '
        'chImage
        '
        resources.ApplyResources(Me.chImage, "chImage")
        '
        'chUsername
        '
        resources.ApplyResources(Me.chUsername, "chUsername")
        '
        'chPassword
        '
        resources.ApplyResources(Me.chPassword, "chPassword")
        '
        'ImageListUsers
        '
        Me.ImageListUsers.ImageStream = CType(resources.GetObject("ImageListUsers.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListUsers.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListUsers.Images.SetKeyName(0, "identity.png")
        '
        'btnOK
        '
        Me.btnOK.AccessibleDescription = Nothing
        Me.btnOK.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.BackgroundImage = Nothing
        Me.btnOK.Font = Nothing
        Me.btnOK.Name = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.Font = Nothing
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.AccessibleDescription = Nothing
        Me.btnRemove.AccessibleName = Nothing
        resources.ApplyResources(Me.btnRemove, "btnRemove")
        Me.btnRemove.BackgroundImage = Nothing
        Me.btnRemove.Font = Nothing
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.AccessibleDescription = Nothing
        Me.btnAdd.AccessibleName = Nothing
        resources.ApplyResources(Me.btnAdd, "btnAdd")
        Me.btnAdd.BackgroundImage = Nothing
        Me.btnAdd.Font = Nothing
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'txtUsername
        '
        Me.txtUsername.AccessibleDescription = Nothing
        Me.txtUsername.AccessibleName = Nothing
        resources.ApplyResources(Me.txtUsername, "txtUsername")
        Me.txtUsername.BackgroundImage = Nothing
        Me.txtUsername.Font = Nothing
        Me.txtUsername.Name = "txtUsername"
        '
        'txtPassword
        '
        Me.txtPassword.AccessibleDescription = Nothing
        Me.txtPassword.AccessibleName = Nothing
        resources.ApplyResources(Me.txtPassword, "txtPassword")
        Me.txtPassword.BackgroundImage = Nothing
        Me.txtPassword.Font = Nothing
        Me.txtPassword.Name = "txtPassword"
        '
        'frmManageAccess
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lvUsers)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmManageAccess"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvUsers As System.Windows.Forms.ListView
    Friend WithEvents chImage As System.Windows.Forms.ColumnHeader
    Friend WithEvents chUsername As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPassword As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageListUsers As System.Windows.Forms.ImageList
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox

End Class

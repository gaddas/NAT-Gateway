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
Partial Class frmAbout
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.OKButton = New System.Windows.Forms.Button
        Me.TextBoxDescription = New System.Windows.Forms.Label
        Me.LabelVersion = New System.Windows.Forms.Label
        Me.LabelCopyright = New System.Windows.Forms.Label
        Me.LabelCompanyName = New System.Windows.Forms.Label
        Me.imgLogo = New System.Windows.Forms.PictureBox
        Me.lineCenter = New System.Windows.Forms.Panel
        Me.lineVertical = New System.Windows.Forms.Panel
        Me.lineHorizontal = New System.Windows.Forms.Panel
        Me.LabelProductName = New System.Windows.Forms.Label
        Me.lblLogo1 = New System.Windows.Forms.Label
        Me.imgEurope = New System.Windows.Forms.PictureBox
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblLink1 = New System.Windows.Forms.LinkLabel
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgEurope, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Image = CType(resources.GetObject("OKButton.Image"), System.Drawing.Image)
        Me.OKButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.OKButton.Location = New System.Drawing.Point(304, 154)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(106, 37)
        Me.OKButton.TabIndex = 3
        Me.OKButton.Text = "&OK"
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.ForeColor = System.Drawing.Color.Navy
        Me.TextBoxDescription.Location = New System.Drawing.Point(104, 138)
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.Size = New System.Drawing.Size(192, 56)
        Me.TextBoxDescription.TabIndex = 7
        Me.TextBoxDescription.Text = "TextBoxDescription"
        '
        'LabelVersion
        '
        Me.LabelVersion.ForeColor = System.Drawing.Color.Navy
        Me.LabelVersion.Location = New System.Drawing.Point(106, 93)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(145, 13)
        Me.LabelVersion.TabIndex = 8
        Me.LabelVersion.Text = "LabelVersion"
        '
        'LabelCopyright
        '
        Me.LabelCopyright.ForeColor = System.Drawing.Color.Navy
        Me.LabelCopyright.Location = New System.Drawing.Point(106, 106)
        Me.LabelCopyright.Name = "LabelCopyright"
        Me.LabelCopyright.Size = New System.Drawing.Size(145, 13)
        Me.LabelCopyright.TabIndex = 8
        Me.LabelCopyright.Text = "LabelCopyright"
        '
        'LabelCompanyName
        '
        Me.LabelCompanyName.ForeColor = System.Drawing.Color.Navy
        Me.LabelCompanyName.Location = New System.Drawing.Point(106, 119)
        Me.LabelCompanyName.Name = "LabelCompanyName"
        Me.LabelCompanyName.Size = New System.Drawing.Size(145, 13)
        Me.LabelCompanyName.TabIndex = 8
        Me.LabelCompanyName.Text = "LabelCompanyName"
        '
        'imgLogo
        '
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.imgLogo.Location = New System.Drawing.Point(-110, -125)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(246, 369)
        Me.imgLogo.TabIndex = 32
        Me.imgLogo.TabStop = False
        '
        'lineCenter
        '
        Me.lineCenter.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lineCenter.Location = New System.Drawing.Point(386, 83)
        Me.lineCenter.Name = "lineCenter"
        Me.lineCenter.Size = New System.Drawing.Size(10, 10)
        Me.lineCenter.TabIndex = 39
        '
        'lineVertical
        '
        Me.lineVertical.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lineVertical.Location = New System.Drawing.Point(390, 9)
        Me.lineVertical.Name = "lineVertical"
        Me.lineVertical.Size = New System.Drawing.Size(3, 400)
        Me.lineVertical.TabIndex = 40
        '
        'lineHorizontal
        '
        Me.lineHorizontal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lineHorizontal.Location = New System.Drawing.Point(-348, 87)
        Me.lineHorizontal.Name = "lineHorizontal"
        Me.lineHorizontal.Size = New System.Drawing.Size(800, 3)
        Me.lineHorizontal.TabIndex = 38
        '
        'LabelProductName
        '
        Me.LabelProductName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LabelProductName.ForeColor = System.Drawing.Color.Navy
        Me.LabelProductName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LabelProductName.Location = New System.Drawing.Point(66, 71)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(197, 13)
        Me.LabelProductName.TabIndex = 36
        Me.LabelProductName.Text = "Remote Control Center"
        Me.LabelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLogo1
        '
        Me.lblLogo1.AutoSize = True
        Me.lblLogo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Bold)
        Me.lblLogo1.ForeColor = System.Drawing.Color.Navy
        Me.lblLogo1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblLogo1.Location = New System.Drawing.Point(99, 36)
        Me.lblLogo1.Name = "lblLogo1"
        Me.lblLogo1.Size = New System.Drawing.Size(164, 42)
        Me.lblLogo1.TabIndex = 37
        Me.lblLogo1.Text = "NetGate"
        '
        'imgEurope
        '
        Me.imgEurope.Image = CType(resources.GetObject("imgEurope.Image"), System.Drawing.Image)
        Me.imgEurope.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.imgEurope.Location = New System.Drawing.Point(231, 0)
        Me.imgEurope.Name = "imgEurope"
        Me.imgEurope.Size = New System.Drawing.Size(191, 113)
        Me.imgEurope.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgEurope.TabIndex = 35
        Me.imgEurope.TabStop = False
        '
        'imgIcon
        '
        Me.imgIcon.Location = New System.Drawing.Point(69, 36)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(48, 40)
        Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.imgIcon.TabIndex = 41
        Me.imgIcon.TabStop = False
        '
        'lblLink1
        '
        Me.lblLink1.AutoSize = True
        Me.lblLink1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblLink1.Location = New System.Drawing.Point(12, 181)
        Me.lblLink1.Name = "lblLink1"
        Me.lblLink1.Size = New System.Drawing.Size(86, 13)
        Me.lblLink1.TabIndex = 42
        Me.lblLink1.TabStop = True
        Me.lblLink1.Text = "gaddas@abv.bg"
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(422, 203)
        Me.Controls.Add(Me.lblLink1)
        Me.Controls.Add(Me.imgIcon)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.lineCenter)
        Me.Controls.Add(Me.lineVertical)
        Me.Controls.Add(Me.lineHorizontal)
        Me.Controls.Add(Me.LabelProductName)
        Me.Controls.Add(Me.lblLogo1)
        Me.Controls.Add(Me.imgEurope)
        Me.Controls.Add(Me.LabelCompanyName)
        Me.Controls.Add(Me.LabelCopyright)
        Me.Controls.Add(Me.LabelVersion)
        Me.Controls.Add(Me.TextBoxDescription)
        Me.Controls.Add(Me.imgLogo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmAbout"
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgEurope, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents TextBoxDescription As System.Windows.Forms.Label
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents LabelCopyright As System.Windows.Forms.Label
    Friend WithEvents LabelCompanyName As System.Windows.Forms.Label
    Friend WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lineCenter As System.Windows.Forms.Panel
    Friend WithEvents lineVertical As System.Windows.Forms.Panel
    Friend WithEvents lineHorizontal As System.Windows.Forms.Panel
    Friend WithEvents LabelProductName As System.Windows.Forms.Label
    Friend WithEvents lblLogo1 As System.Windows.Forms.Label
    Friend WithEvents imgEurope As System.Windows.Forms.PictureBox
    Friend WithEvents imgIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblLink1 As System.Windows.Forms.LinkLabel

End Class

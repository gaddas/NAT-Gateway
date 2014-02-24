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


Public NotInheritable Class frmAbout

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim info As New Microsoft.VisualBasic.ApplicationServices.AssemblyInfo(System.Reflection.Assembly.GetExecutingAssembly)

        ' Set the title of the form.
        Dim ApplicationTitle As String
        If info.Title <> "" Then
            ApplicationTitle = info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Me.LabelProductName.Text = info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", info.Version.ToString)
        Me.LabelCopyright.Text = info.Copyright
        Me.LabelCompanyName.Text = info.CompanyName
        Me.TextBoxDescription.Text = info.Description
        Me.imgIcon.Image = frmMain.Icon.ToBitmap

        Me.lblLink1.Parent = Me.imgLogo
        Me.lblLink1.BackColor = Color.Transparent
        Me.lblLink1.Location = New Point(4 - Me.imgLogo.Location.X, 180 - Me.imgLogo.Location.Y)

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class

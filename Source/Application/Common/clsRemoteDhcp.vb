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

Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels


Public Interface remoteObjectDHCP

    Function GetConfig() As String
    Sub SetConfig(ByVal config As String)
    Sub Reload()

    Function GetClients() As List(Of String)

    Function Test(ByVal x As Integer) As Integer

End Interface

Public Class remoteFactoryDHCP
    Inherits MarshalByRefObject

    Public Const defaultPort As Integer = 7003
    Public Const defaultURI As String = "dhcp.rem"

    Private obj As remoteObjectDHCP = Nothing

    Public Sub New(ByVal initObj As remoteObjectDHCP)
        obj = initObj
    End Sub

    Public Function Test(ByVal x As Integer) As Integer
        Return x
    End Function

    ' Authentication check
    Public Function Login(ByVal user As String, ByVal pass As String) As remoteObjectDHCP
        Dim f As New IO.FileStream("remote.conf", IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim r As New IO.StreamReader(f)

        Dim s As String

        While Not r.EndOfStream
            s = r.ReadLine
            If s.Trim = "" Then Continue While
            If s.Remove(s.IndexOf("|")).ToLower = user.ToLower AndAlso s.Substring(s.IndexOf("|") + 1) = pass Then
                ' User Name and Password are correct
                r.Close()
                f.Close()
                Return CType(obj, remoteObjectDHCP)
            End If
        End While

        r.Close()
        f.Close()
        Return Nothing
    End Function

    ' Make object live forever
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

End Class

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


Imports System
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.Resources
Imports System.Globalization
Imports System.Reflection


Public Module modRemote

    Public serverLocal As Boolean = True
    Public remoteDhcpFactory As remoteFactoryDHCP
    Public remoteDhcpObject As remoteObjectDHCP
    Public remoteDnsFactory As remoteFactoryDNS
    Public remoteDnsObject As remoteObjectDNS
    Public remoteNatFactory As remoteFactoryNAT
    Public remoteNatObject As remoteObjectNAT


    Public Function remoteConnect(ByVal addr As String, ByVal user As String, ByVal pass As String) As Boolean
        'Try
        While ChannelServices.RegisteredChannels.Length > 0
            ChannelServices.UnregisterChannel(ChannelServices.RegisteredChannels(0))
        End While

        ChannelServices.RegisterChannel(New TcpChannel(remoteFactoryDHCP.defaultPort + 1000), False)


        If addr.ToLower = "localhost" Then
            serverLocal = True
            On Error Resume Next
            remoteDhcpFactory = Activator.GetObject(GetType(remoteFactoryDHCP), "tcp://localhost:" & remoteFactoryDHCP.defaultPort & "/" & remoteFactoryDHCP.defaultURI)
            remoteDnsFactory = Activator.GetObject(GetType(remoteFactoryDNS), "tcp://localhost:" & remoteFactoryDNS.defaultPort & "/" & remoteFactoryDNS.defaultURI)
            remoteNatFactory = Activator.GetObject(GetType(remoteFactoryNAT), "tcp://localhost:" & remoteFactoryNAT.defaultPort & "/" & remoteFactoryNAT.defaultURI)
        Else
            On Error Resume Next
            remoteDhcpFactory = Activator.GetObject(GetType(remoteFactoryDHCP), "tcp://" & addr.ToString & ":" & remoteFactoryDHCP.defaultPort & "/" & remoteFactoryDHCP.defaultURI)
            remoteDnsFactory = Activator.GetObject(GetType(remoteFactoryDNS), "tcp://" & addr.ToString & ":" & remoteFactoryDNS.defaultPort & "/" & remoteFactoryDNS.defaultURI)
            remoteNatFactory = Activator.GetObject(GetType(remoteFactoryNAT), "tcp://" & addr.ToString & ":" & remoteFactoryNAT.defaultPort & "/" & remoteFactoryNAT.defaultURI)
        End If

        'Catch ex As Exception
        '    MessageBox.Show(My.Resources.ErrorConnectingServer & ex.ToString, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

        Return dhcpRemoteConnect(user, pass) And dnsRemoteConnect(user, pass) And natRemoteConnect(user, pass)
    End Function
    Public Function dhcpRemoteConnect(ByVal user As String, ByVal pass As String) As Boolean
        Try

            If (remoteDhcpFactory IsNot Nothing) AndAlso (remoteDhcpFactory.Test(93) = 93) Then
                remoteDhcpObject = remoteDhcpFactory.Login(user, pass)
                If remoteDhcpObject Is Nothing Then
                    MessageBox.Show(My.Resources.IncorrectUsernameOrPassword, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show(My.Resources.CouldNotConnectToRemoteServer, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

        Catch ex As Exception
#If DEBUG Then
            MessageBox.Show(My.Resources.ErrorConnectingServer & ex.ToString, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
#End If
            Return False
        End Try

        Return True
    End Function
    Public Function dnsRemoteConnect(ByVal user As String, ByVal pass As String) As Boolean
        Try
            If (remoteDnsFactory IsNot Nothing) AndAlso (remoteDnsFactory.Test(93) = 93) Then
                remoteDnsObject = remoteDnsFactory.Login(user, pass)
                If remoteDnsObject Is Nothing Then
                    MessageBox.Show(My.Resources.IncorrectUsernameOrPassword, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show(My.Resources.CouldNotConnectToRemoteServer, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

        Catch ex As Exception
#If DEBUG Then
            MessageBox.Show(My.Resources.ErrorConnectingServer & ex.ToString, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
#End If
            Return False
        End Try

        Return True
    End Function
    Public Function natRemoteConnect(ByVal user As String, ByVal pass As String) As Boolean
        Try
            If (remoteNatFactory IsNot Nothing) AndAlso (remoteNatFactory.Test(93) = 93) Then
                remoteNatObject = remoteNatFactory.Login(user, pass)
                If remoteNatObject Is Nothing Then
                    MessageBox.Show(My.Resources.IncorrectUsernameOrPassword, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show(My.Resources.CouldNotConnectToRemoteServer, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

        Catch ex As Exception
#If DEBUG Then
            MessageBox.Show(My.Resources.ErrorConnectingServer & ex.ToString, My.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
#End If
            Return False
        End Try

        Return True
    End Function

    Public Function dhcpRemoteStatus() As Boolean
        Try
            Return (remoteDhcpObject IsNot Nothing AndAlso remoteDhcpObject.Test(93) = 93)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function dnsRemoteStatus() As Boolean
        Try
            Return (remoteDnsObject IsNot Nothing AndAlso remoteDnsObject.Test(93) = 93)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function natRemoteStatus() As Boolean
        Try
            Return (remoteNatObject IsNot Nothing AndAlso remoteNatObject.Test(93) = 93)
        Catch ex As Exception
            Return False
        End Try
    End Function

End Module
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


Public Class dnsd

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        Try
            evLogMain.WriteEntry("DNS Service Started.")

            IO.Directory.SetCurrentDirectory(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location))

            m_RemoteChannel = New Channels.Tcp.TcpChannel(remoteFactoryDNS.defaultPort)
            Channels.ChannelServices.RegisterChannel(m_RemoteChannel, False)

            m_Server = New dnsServer
            m_RemoteFactory = New remoteFactoryDNS(m_Server)

            RemotingServices.Marshal(CType(m_RemoteFactory, remoteFactoryDNS), remoteFactoryDNS.defaultURI)
        Catch ex As Exception
            EventLog.WriteEntry("NetGate DNS", ex.ToString, EventLogEntryType.Error)
            Console.WriteLine(ex.ToString)

            Me.Stop()
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        Try
            evLogMain.WriteEntry("DNS Service Stopped.")

            Try
                Channels.ChannelServices.UnregisterChannel(m_RemoteChannel)
                RemotingServices.Unmarshal(m_RemoteFactoryObj)
            Catch
            Finally
                m_RemoteChannel = Nothing
                m_RemoteFactory = Nothing
                m_RemoteFactoryObj = Nothing
            End Try

            Try
                m_Server.Dispose()
            Catch
            Finally
                m_Server = Nothing
            End Try
        Catch ex As Exception
            evLogMain.WriteEntry(ex.ToString)
        End Try
    End Sub

End Class

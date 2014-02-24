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


Public Class natd

    Public TimerMain As New System.Timers.Timer()
    Public TimerLimit As New System.Timers.Timer()

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        Try
            evLogMain.WriteEntry("NAT Service Started.")

            IO.Directory.SetCurrentDirectory(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location))

            m_RemoteChannel = New Channels.Tcp.TcpChannel(remoteFactoryNAT.defaultPort)
            Channels.ChannelServices.RegisterChannel(m_RemoteChannel, False)

            FirewallInitialize()
            FirewallClearRules()
            FirewallHook()
            evLogMain.WriteEntry("NAT Service firewall ready.")

            m_Server = New natServer
            m_RemoteFactory = New remoteFactoryNAT(m_Server)

            TimerLimit.Enabled = True
            TimerMain.Enabled = True
            TimerMain.Interval = 10000
            TimerLimit.Interval = 1000
            AddHandler TimerMain.Elapsed, AddressOf TimerMain_Tick
            AddHandler TimerLimit.Elapsed, AddressOf TimerLimit_Tick

            RemotingServices.Marshal(CType(m_RemoteFactory, remoteFactoryNAT), remoteFactoryNAT.defaultURI)
        Catch ex As Exception
            EventLog.WriteEntry("NetGate NAT", ex.ToString, EventLogEntryType.Error)
            Console.WriteLine(ex.ToString)

            Me.Stop()
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        Try
            m_Server.m_Loop = False

            FirewallClearRules()
            FirewallUnHook()
            FirewallClose()

            evLogMain.WriteEntry("NAT Service Stopped.")

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

    Private Sub TimerMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim list As New List(Of String)

        ' Conntrack Timeout
        m_Server.m_ConntrackLock.AcquireWriterLock(LOCK_TIMEOUT)

        For Each ttl As KeyValuePair(Of String, DateTime) In m_Server.m_ConntrackTimeToLive
            If ttl.Value < Now Then list.Add(ttl.Key)
        Next

        For Each s As String In list
            m_Server.m_Conntrack.Remove(s)
            m_Server.m_ConntrackTimeToLive.Remove(s)

            Console.WriteLine("conntrack del {0}", s)
        Next
        m_Server.m_ConntrackLock.ReleaseWriterLock()
    End Sub

    Private Sub TimerLimit_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim l As New List(Of QueuedPacket)
        Dim p As QueuedPacket = Nothing

        For Each chain As natConfigChain In m_Server.m_Config.m_Table(TABLE_NAT).FORWARD
            chain.limiterLock.AcquireWriterLock(LOCK_TIMEOUT)
            While chain.limiterBuffer.Count > 0 AndAlso chain.limiterTraffic < chain.packetLimit
                p = chain.limiterBuffer(0)
                chain.limiterBuffer.Remove(p)
                chain.limiterTraffic += p.Packet.PcapHeader.PacketLength
                l.Add(p)
                p = Nothing
            End While

            chain.limiterTraffic = 0
            chain.limiterLock.ReleaseWriterLock()
        Next

        For Each qp As QueuedPacket In l
            ChainPOSTROUTING(qp.Packet, qp.FromInterface, qp.ToInterface)
        Next
        l.Clear()
    End Sub
End Class

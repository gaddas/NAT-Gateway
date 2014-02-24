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
Imports System.Net.Sockets


Public Class dhcpServer
    Inherits MarshalByRefObject

    Implements NetGate.remoteObjectDHCP
    Implements IDisposable

    ' Make object live forever
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Public Function Test(ByVal x As Integer) As Integer Implements remoteObjectDHCP.Test
        Return x
    End Function

#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
                m_Socket.Shutdown(SocketShutdown.Both)
                m_Socket.Close()
                m_Socket = Nothing

                ' Save state
                stateSave()
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "Remote Support"

    Public Function GetConfig() As String Implements remoteObjectDHCP.GetConfig
        Dim f As New IO.FileStream("dhcpd.conf", IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim r As New IO.StreamReader(f)
        Dim c As String
        c = r.ReadToEnd()
        r.Close()
        f.Close()

        Return c
    End Function

    Public Sub Reload() Implements remoteObjectDHCP.Reload

    End Sub

    Public Sub SetConfig(ByVal config As String) Implements remoteObjectDHCP.SetConfig
#If DEBUG Then
        Console.WriteLine(config)
#End If

        Dim dhcpdConfBak As New IO.FileInfo("dhcpd.conf.bak")
        dhcpdConfBak.Delete()

        Dim dhcpdConfOld As New IO.FileInfo("dhcpd.conf")
        dhcpdConfOld.MoveTo("dhcpd.conf.bak")

        Dim dhcpdConfNew As New IO.FileStream("dhcpd.conf", IO.FileMode.Create, IO.FileAccess.Write)
        Dim w As New IO.StreamWriter(dhcpdConfNew)
        w.Write(config)
        w.Flush()
        w.Close()
        dhcpdConfNew.Close()

        m_Server.m_ConfigData = config
        m_Server.m_Config = New dhcpOptions
        m_Server.m_Config.ReadConfig(config)
    End Sub

    Public Function GetClients() As List(Of String) Implements remoteObjectDHCP.GetClients
        Dim l As New List(Of String)
        Dim s As String

        m_Lock.AcquireReaderLock(LOCK_TIMEOUT)
        For Each ip As IPAddress In m_PoolUsed
            s = String.Format("{0};{1};{2};{3}", ip.ToString, m_Clients(ip), m_Hosts(ip), Fix(m_Leases(ip).Subtract(Now).TotalMinutes) & " min")
            l.Add(s)
        Next
        m_Lock.ReleaseReaderLock()

        Return l
    End Function

#End Region


    Public m_Socket As Socket
    Public m_ConfigData As String
    Public m_Config As dhcpOptions

    Public Sub New()
        ' Open our config file
        Dim f As New IO.StreamReader(New IO.FileStream("dhcpd.conf", IO.FileMode.Open))
        m_ConfigData = f.ReadToEnd()
        f.Close()

        ' Load options from config file
        m_Config = New dhcpOptions
        m_Config.ReadConfig(m_ConfigData)
        m_Server = Me

        ' Load last state
        stateLoad()

        ReInitialize()

        ipPoolClean()

        AsyncBeginReceive()
    End Sub
    Public Sub ReInitialize()
        If m_Socket IsNot Nothing Then
            m_Socket.Close()
            m_Socket = Nothing
        End If

        ipPoolInitialize()

        m_Socket = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        m_Socket.EnableBroadcast = True
        m_Socket.Bind(New IPEndPoint(m_Config.interfaceAddr, 67))
        EventLog.WriteEntry("NetGate DHCP", (String.Format("Interface UP at: {0}:67", m_Config.interfaceAddr.ToString)))
    End Sub


    Public Sub AsyncBeginReceive()
        Dim p As New Packet
        p.Prepare()

        Try
            m_Socket.BeginReceiveFrom(p.Data, 0, Packet.BUFFER_SIZE, SocketFlags.None, p.IP, AddressOf AsyncEndReceive, p)
        Catch ex As Exception
            EventLog.WriteEntry("NetGate DHCP", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#If DEBUG Then
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub
    Public Sub AsyncEndReceive(ByVal ar As IAsyncResult)
        AsyncBeginReceive()

        Dim p As Packet = CType(ar.AsyncState, Packet)

        Try
            p.Length = m_Socket.EndReceiveFrom(ar, p.IP)
            OnPacket(p)

        Catch ex As Exception
            EventLog.WriteEntry("NetGate DHCP", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#If DEBUG Then
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub
    Public Sub AsyncBeginSend(ByVal p As Packet)
        Try
            m_Socket.BeginSendTo(p.Data, 0, p.Length, SocketFlags.None, p.IP, AddressOf AsyncEndSend, p)
        Catch ex As Exception
            EventLog.WriteEntry("NetGate DHCP", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#If DEBUG Then
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub
    Public Sub AsyncEndSend(ByVal ar As IAsyncResult)
        Dim p As Packet = CType(ar.AsyncState, Packet)

        Try
            Dim bytesSent As Integer = m_Socket.EndSendTo(ar)

            EventLog.WriteEntry("NetGate DHCP", String.Format("Sent {0} bytes to {1}", bytesSent, p.IP.ToString))
#If DEBUG Then
            Console.WriteLine(String.Format("Sent {0} bytes to {1}", bytesSent, p.IP.ToString))
#End If
        Catch ex As Exception
            EventLog.WriteEntry("NetGate DHCP", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#If DEBUG Then
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub



End Class

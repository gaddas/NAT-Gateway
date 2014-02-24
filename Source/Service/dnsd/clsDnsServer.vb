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


Public Class dnsServer
    Inherits MarshalByRefObject

    Implements NetGate.remoteObjectDNS
    Implements IDisposable

    ' Make object live forever
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Public Function Test(ByVal x As Integer) As Integer Implements remoteObjectDNS.Test
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


    Public m_Socket As Socket
    Public m_ConfigData As String
    Public m_Config As dnsConfig

    Public m_dnsLock As New Threading.ReaderWriterLock
    Public m_dnsCache As New Cachetable
    Public m_dnsQuery As New Dictionary(Of String, IPEndPoint)


    Public Sub New()
        ' Open our config file
        Dim f As New IO.StreamReader(New IO.FileStream("dnsd.conf", IO.FileMode.Open))
        m_ConfigData = f.ReadToEnd()
        f.Close()

        ' Load options from config file
        m_Config = New dnsConfig
        m_Config.ReadConfig(m_ConfigData)
        m_Server = Me
        m_Server.m_dnsCache.Size = m_Config.m_CacheSize

        m_Socket = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        m_Socket.Bind(New IPEndPoint(IPAddress.Any, 53))
        EventLog.WriteEntry("NetGate DNS", "Interface UP at: 0.0.0.0:53")

        AsyncBeginReceive()
    End Sub


    Public Sub AsyncBeginReceive()
        Dim p As New Packet
        p.Prepare()
        p.IP = CType(New IPEndPoint(IPAddress.Any, 53), EndPoint)

        Try
            m_Socket.BeginReceiveFrom(p.Data, 0, Packet.BUFFER_SIZE, SocketFlags.None, p.IP, AddressOf AsyncEndReceive, p)
        Catch ex As Exception
#If DEBUG Then
            EventLog.WriteEntry("NetGate DNS", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
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
#If DEBUG Then
            EventLog.WriteEntry("NetGate DNS", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub
    Public Sub AsyncBeginSend(ByVal p As Packet)
        Try
            m_Socket.BeginSendTo(p.Data, 0, p.Length, SocketFlags.None, p.IP, AddressOf AsyncEndSend, p)
        Catch ex As Exception
#If DEBUG Then
            EventLog.WriteEntry("NetGate DNS", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub
    Public Sub AsyncEndSend(ByVal ar As IAsyncResult)
        Dim p As Packet = CType(ar.AsyncState, Packet)

        Try
            Dim bytesSent As Integer = m_Socket.EndSendTo(ar)

            EventLog.WriteEntry("NetGate DNS", String.Format("Sent {0} bytes to {1}", bytesSent, p.IP.ToString))
#If DEBUG Then
            Console.WriteLine(String.Format("Sent {0} bytes to {1}", bytesSent, p.IP.ToString))
#End If
        Catch ex As Exception
#If DEBUG Then
            EventLog.WriteEntry("NetGate DNS", String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
            Console.WriteLine(String.Format("Connection from [{0}] cause error {1}{2}", p.IP.ToString, ex.ToString, vbNewLine))
#End If
        End Try
    End Sub


    Public Function GetConfig() As String Implements remoteObjectDNS.GetConfig
        Dim f As New IO.FileStream("dnsd.conf", IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim r As New IO.StreamReader(f)
        Dim c As String
        c = r.ReadToEnd()
        r.Close()
        f.Close()

        Return c
    End Function

    Public Sub SetConfig(ByVal config As String) Implements remoteObjectDNS.SetConfig
#If DEBUG Then
        Console.WriteLine(config)
#End If

        Dim dnsdConfBak As New IO.FileInfo("dnsd.conf.bak")
        dnsdConfBak.Delete()

        Dim dnsdConfOld As New IO.FileInfo("dnsd.conf")
        dnsdConfOld.MoveTo("dnsd.conf.bak")

        Dim dnsdConfNew As New IO.FileStream("dnsd.conf", IO.FileMode.Create, IO.FileAccess.Write)
        Dim w As New IO.StreamWriter(dnsdConfNew)
        w.Write(config)
        w.Flush()
        w.Close()
        dnsdConfNew.Close()

        m_Server.m_ConfigData = config
        m_Server.m_Config = New dnsConfig
        m_Server.m_Config.ReadConfig(config)
        m_Server.m_dnsCache.Size = m_Config.m_CacheSize
    End Sub

    Public Sub ClearCache() Implements remoteObjectDNS.ClearCache
        m_dnsLock.AcquireWriterLock(LOCK_TIMEOUT)
        m_dnsCache.Clear()
        m_dnsLock.ReleaseWriterLock()
    End Sub
End Class

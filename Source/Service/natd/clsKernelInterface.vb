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


Imports System.IO
Imports System.Net
Imports System.Collections.Generic
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles


Public Class InterfaceKernel
    Public hInterface As SafeFileHandle = Nothing
    Public sInterface As String = String.Empty


    Public Sub New()
        hInterface = Nothing
        sInterface = String.Empty
    End Sub

    Public Sub Open(ByVal rawInterface As String)
        Console.WriteLine(vbTab & "PtOpenControlChannel()")
        hInterface = PtOpenControlChannel()

        If hInterface.IsInvalid Then
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        Dim lpAdapterName As IntPtr = Marshal.StringToHGlobalUni(rawInterface)
        Dim ndisStatus As Integer = 0
        Dim bytesReceived As Int32 = 0

        Console.WriteLine(vbTab & String.Format("open  {0}", rawInterface))
        If DeviceIoControl(hInterface, _
                           IOCTL_NETGUSERIO_OPEN_ADAPTER, _
                           lpAdapterName, _
                           Text.UnicodeEncoding.Unicode.GetByteCount(rawInterface), _
                           ndisStatus, _
                           Marshal.SizeOf(ndisStatus), _
                           bytesReceived, _
                           Nothing) Then
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        Marshal.FreeHGlobal(lpAdapterName)
        sInterface = rawInterface
        Console.WriteLine(vbTab & "start " & sInterface)

        If hInterface.IsInvalid Then
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If
    End Sub
    Public Sub Close()
        Console.WriteLine(vbTab & "stop  " & sInterface)

        If Not PtCloseAdapter(hInterface) Then
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        Console.WriteLine(vbTab & "PtCloseControlChannel()")
    End Sub
    Public Function ReadFromMiniport() As Packet
        Console.WriteLine(vbTab & "read  " & sInterface)

        Return Nothing
    End Function
    Public Sub WriteToProtocol(ByVal p As Packet)
        Console.WriteLine(vbTab & "write " & sInterface)

        Dim ndisStatus As Integer = 0
        Dim bytesSent As Integer = p.Data.Length
        Dim bytesReceived As Integer = 0

        Dim gcBuffer As GCHandle = GCHandle.Alloc(p.Data, GCHandleType.Pinned)
        Dim lpBuffer As IntPtr = gcBuffer.AddrOfPinnedObject

        If DeviceIoControl(hInterface, _
                           IOCTL_NETGUSERIO_SEND_TO_PROTOCOL, _
                           lpBuffer, _
                           bytesSent, _
                           ndisStatus, _
                           Marshal.SizeOf(ndisStatus), _
                           bytesReceived, _
                           Nothing) Then
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        gcBuffer.Free()
    End Sub
    Public Sub WriteToMiniport(ByVal p As Packet)
        Console.WriteLine(vbTab & "write " & sInterface)

        Dim ndisStatus As Integer = 0
        Dim bytesSent As Integer = p.Data.Length
        Dim bytesReceived As Integer = 0

        Dim gcBuffer As GCHandle = GCHandle.Alloc(p.Data, GCHandleType.Pinned)
        Dim lpBuffer As IntPtr = gcBuffer.AddrOfPinnedObject

        If DeviceIoControl(hInterface, _
                           IOCTL_NETGUSERIO_SEND_TO_MINIPORT, _
                           lpBuffer, _
                           bytesSent, _
                           ndisStatus, _
                           Marshal.SizeOf(ndisStatus), _
                           bytesReceived, _
                           Nothing) Then
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        gcBuffer.Free()
    End Sub
    Public Sub Debug()
        Console.WriteLine(vbTab & "debug " & sInterface)

        Dim ndisStatus As Integer = 0
        Dim lpBuffer As IntPtr = Marshal.StringToHGlobalAnsi("test" & vbNewLine & Chr(0))
        Dim bytesSent As Integer = Text.UnicodeEncoding.Default.GetByteCount("test" & vbNewLine & Chr(0))
        Dim bytesReceived As Integer = 0

        If DeviceIoControl(hInterface, _
                           IOCTL_NETGUSERIO_DEBUG, _
                           lpBuffer, _
                           bytesSent, _
                           ndisStatus, _
                           Marshal.SizeOf(ndisStatus), _
                           bytesReceived, _
                           Nothing) Then
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        Marshal.FreeHGlobal(lpBuffer)
    End Sub

    Public Shared Sub Query(ByVal m_Adapters As List(Of String), ByVal m_AdaptersInfo As Dictionary(Of String, String))
        Dim hInterface As SafeFileHandle

        Console.WriteLine(vbTab & "PtOpenControlChannel()")
        hInterface = PtOpenControlChannel()

        If hInterface.IsInvalid Then
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        m_Adapters.Clear()
        m_AdaptersInfo.Clear()

        ' Allocate buffer
        Dim bytesReceived As Int32 = 1024
        Dim bytesBuffer As IntPtr = Marshal.AllocHGlobal(1024)

        ' Get Enum Devices
        If Not PtEnumerateBindings(hInterface, bytesBuffer, bytesReceived) Then
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        ' Read data 
        Dim start As Integer = 0
        Dim p As IntPtr = New IntPtr(bytesBuffer.ToInt64)
        Dim s As String = ""

        While start < bytesReceived
            s = Marshal.PtrToStringUni(p)
            start += Text.UnicodeEncoding.Unicode.GetByteCount(s) + 2
            p = IntPtrIncrement(p, Text.UnicodeEncoding.Unicode.GetByteCount(s) + 2)
#If DEBUG Then
            Console.WriteLine("read {0} bytes of {1}, remaining {2}", Text.UnicodeEncoding.Unicode.GetByteCount(s), bytesReceived, bytesReceived - start)
#End If
            If s.Trim.Length <> 0 Then m_Adapters.Add(s)
        End While

        Console.WriteLine()
        Dim adapters As New List(Of String)
        For Each ss As String In m_Adapters
            Console.WriteLine(ss)

            For Each i As System.Net.NetworkInformation.NetworkInterface In System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
                If ss.Contains(i.Id) Then
                    adapters.Add(ss)
                    m_AdaptersInfo.Add(ss, i.Description)
                    Console.WriteLine(vbTab & i.Description)
                    Exit For
                End If
            Next
        Next
        m_Adapters = adapters

        ' Free buffer
        Marshal.FreeHGlobal(bytesBuffer)

        If Not PtCloseAdapter(hInterface) Then Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        Console.WriteLine("enumerate ready.")
    End Sub

End Class

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


Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles


Module IOCTL

    Public Const FILE_ANY_ACCESS As UInteger = 0
    Public Const FILE_READ_ACCESS As UInteger = 1
    Public Const FILE_WRITE_ACCESS As UInteger = 2
    Public Const FILE_DEVICE_NETWORK As UInteger = &H12
    Public Const FILE_DEVICE_UNKNOWN As UInteger = &H22
    Public Const METHOD_BUFFERED As UInteger = 0
    Public Const METHOD_IN_DIRECT As UInteger = 1
    Public Const METHOD_OUT_DIRECT As UInteger = 2
    Public Const METHOD_NEITHER As UInteger = 3

    Public Const FILE_DEVICE_FWHOOKDRV As UInteger = &H692322
    Public IOCTL_START_IP_HOOK As UInteger = CTL_CODE(FILE_DEVICE_FWHOOKDRV, &H830 + 0, METHOD_BUFFERED, FILE_ANY_ACCESS)
    Public IOCTL_STOP_IP_HOOK As UInteger = CTL_CODE(FILE_DEVICE_FWHOOKDRV, &H830 + 1, METHOD_BUFFERED, FILE_ANY_ACCESS)
    Public IOCTL_ADD_FILTER As UInteger = CTL_CODE(FILE_DEVICE_FWHOOKDRV, &H830 + 2, METHOD_BUFFERED, FILE_WRITE_ACCESS)
    Public IOCTL_CLEAR_FILTER As UInteger = CTL_CODE(FILE_DEVICE_FWHOOKDRV, &H830 + 3, METHOD_BUFFERED, FILE_ANY_ACCESS)
    Public IOCTL_REM_FILTER As UInteger = CTL_CODE(FILE_DEVICE_FWHOOKDRV, &H830 + 4, METHOD_BUFFERED, FILE_WRITE_ACCESS)

    'http://www.pinvoke.net/default.aspx/kernel32/CreateFile.html
    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function CreateFile(ByVal lpFileName As String, _
                               <MarshalAs(UnmanagedType.U4)> ByVal dwDesiredAccess As IO.FileAccess, _
                               <MarshalAs(UnmanagedType.U4)> ByVal dwShareMode As IO.FileShare, _
                               ByVal lpSecurityAttributes As IntPtr, _
                               <MarshalAs(UnmanagedType.U4)> ByVal dwCreationDisposition As IO.FileMode, _
                               ByVal dwFlagsAndAttributes As EFileAttributes, _
                               ByVal hTemplateFile As IntPtr) As SafeFileHandle
    End Function
    Friend Enum EFileAttributes
        faReadonly = &H1    'changed from C# because ReadOnly is a keyword
        Hidden = &H2
        System = &H4
        Directory = &H10
        Archive = &H20
        Device = &H40
        Normal = &H80
        Temporary = &H100
        SparseFile = &H200
        ReparsePoint = &H400
        Compressed = &H800
        Offline = &H1000
        NotContentIndexed = &H2000
        Encrypted = &H4000
        Write_Through = &H80000000
        Overlapped = &H40000000
        NoBuffering = &H20000000
        RandomAccess = &H10000000
        SequentialScan = &H8000000
        DeleteOnClose = &H4000000
        BackupSemantics = &H2000000
        PosixSemantics = &H1000000
        OpenReparsePoint = &H200000
        OpenNoRecall = &H100000
        FirstPipeInstance = &H80000
    End Enum


    'http://www.pinvoke.net/default.aspx/kernel32/CloseHandle.html
    <DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function CloseHandle(ByVal hObject As IntPtr) As Boolean
    End Function
    <DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function CloseHandle(ByVal hObject As SafeFileHandle) As Boolean
    End Function

    'http://pinvoke.net/default.aspx/kernel32/DeviceIoControl.html
    '<DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True, CharSet:=CharSet.Auto)> _
    'Public Function DeviceIoControl(ByVal hDevice As IntPtr, _
    '                                ByVal dwIoControlCode As UInteger, _
    '                                ByVal lpInBuffer As IntPtr, _
    '                                ByVal nInBufferSize As UInteger, _
    '                                ByVal lpOutBuffer As IntPtr, _
    '                                ByVal nOutBufferSize As UInteger, _
    '                                ByRef lpBytesReturned As UInteger, _
    '                                ByVal lpOverlapped As System.Threading.NativeOverlapped) As Boolean
    'End Function

    'http://pinvoke.net/default.aspx/kernel32/DeviceIoControl.html
    <DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function DeviceIoControl(ByVal hDevice As SafeFileHandle, _
                                    ByVal dwIoControlCode As Int32, _
                                    ByVal lpInBuffer As IntPtr, _
                                    ByVal nInBufferSize As Int32, _
                                    ByVal lpOutBuffer As IntPtr, _
                                    ByVal nOutBufferSize As Int32, _
                                    ByRef lpBytesReturned As Int32, _
                                    ByVal lpOverlapped As System.Threading.NativeOverlapped) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function WriteFile(ByVal hFile As SafeFileHandle, _
                              ByVal lpBuffer As IntPtr, _
                              ByVal nNumberOfBytesToWrite As UInteger, _
                              ByVal lpNumberOfBytesWritten As UInteger, _
                              ByVal lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Public Function CTL_CODE(ByVal DeviceType As UInteger, ByVal FunctionId As UInteger, ByVal Method As UInteger, ByVal Access As UInteger) As UInteger
        Return ((DeviceType << 16) Or (Access << 14) Or (FunctionId << 2) Or Method)
    End Function

    Public Function IntPtrIncrement(ByVal p As IntPtr, ByVal inc As Integer) As IntPtr
        Return New IntPtr(p.ToInt64 + inc)
    End Function

End Module
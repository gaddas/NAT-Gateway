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
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles


Module PtUtils


    Public Function PtOpenControlChannel() As SafeFileHandle
        Dim hAdapter As SafeFileHandle

        ' Open file stream
        hAdapter = CreateFile("\\.\NetGate", _
                              FileAccess.ReadWrite, _
                              FileShare.None, _
                              Nothing, _
                              FileMode.Open, _
                              EFileAttributes.Normal, _
                              Nothing)

        Return hAdapter
    End Function

    Public Function PtCloseAdapter(ByVal hAdapter As SafeFileHandle) As Boolean
        Return CloseHandle(hAdapter)
    End Function

    Public Function PtEnumerateBindings(ByVal PtHandle As SafeFileHandle, ByVal PtBuffer As IntPtr, ByRef PtBufferLen As Integer) As Boolean
        Return DeviceIoControl(PtHandle, _
                               IOCTL_NETGUSERIO_ENUMERATE, _
                               Nothing, _
                               0, _
                               PtBuffer, _
                               PtBufferLen, _
                               PtBufferLen, _
                               Nothing)
    End Function




End Module
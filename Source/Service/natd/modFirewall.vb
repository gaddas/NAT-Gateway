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
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles


Public Module modFirewall

    ' sizeof() = 28
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure FirewallRule
        <MarshalAs(UnmanagedType.U2)> Public protocol As UShort

        <MarshalAs(UnmanagedType.U4)> Public sourceIp As UInteger
        <MarshalAs(UnmanagedType.U4)> Public destinationIp As UInteger

        <MarshalAs(UnmanagedType.U4)> Public sourceMask As UInteger
        <MarshalAs(UnmanagedType.U4)> Public destinationMask As UInteger

        <MarshalAs(UnmanagedType.U2)> Public sourcePort As UShort
        <MarshalAs(UnmanagedType.U2)> Public destinationPort As UShort

        <MarshalAs(UnmanagedType.U1)> Public drop As Boolean
    End Structure

    Public Const FIREWALL_PROTOCOL_ANY As UShort = 0
    Public Const FIREWALL_PROTOCOL_TCP As UShort = 6
    Public Const FIREWALL_PROTOCOL_UDP As UShort = 17
    Public Const FIREWALL_PROTOCOL_ICMP As UShort = 1

    Public FirewallHandle As SafeFileHandle
    Public FirewallRules As New List(Of FirewallRule)
    Public FirewallRules_Lock As New Threading.ReaderWriterLock

    Public Sub FirewallInitialize()
        If ServiceInstalled() Then
            StopService()
            InitializeService(svcAction.aUnInstall)
        End If
        
        IO.File.Copy("FwHookDrv.sys", Environment.GetFolderPath(Environment.SpecialFolder.System) & "\drivers\FwHookDrv.sys", True)

        InitializeService(svcAction.aInstall)
        If Not StartService() Then Console.WriteLine("Firewall start error.")

        FirewallHandle = CreateFile("\\.\FwHookDrv", _
                              FileAccess.ReadWrite, _
                              FileShare.None, _
                              Nothing, _
                              FileMode.Open, _
                              EFileAttributes.Normal, _
                              Nothing)

        If FirewallHandle.IsInvalid Then Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
    End Sub

    Public Sub FirewallClose()
        CloseHandle(FirewallHandle)
    End Sub

    Public Sub FirewallAddRule(ByVal drop As Boolean, ByVal protocol As UShort, _
                               ByVal sourceIp As IPAddress, ByVal sourceMask As IPAddress, ByVal sourcePort As UShort, _
                               ByVal destinationIp As IPAddress, ByVal destinationMask As IPAddress, ByVal destinationPort As UShort)
        Dim rule As New FirewallRule

        rule.protocol = protocol
        rule.drop = drop
        rule.sourcePort = IPAddress.HostToNetworkOrder(CType(sourcePort, Short))
        rule.sourceIp = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(sourceIp.ToString), Integer))
        rule.sourceMask = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(sourceMask.ToString), Integer))
        rule.destinationPort = IPAddress.HostToNetworkOrder(CType(destinationPort, Short))
        rule.destinationIp = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(destinationIp.ToString), Integer))
        rule.destinationMask = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(destinationMask.ToString), Integer))

        Dim pRule As IntPtr
        pRule = Marshal.AllocHGlobal(Marshal.SizeOf(rule))
        Marshal.StructureToPtr(rule, pRule, True)

        Dim bytesReceived As Integer
        If DeviceIoControl(FirewallHandle, _
                           IOCTL_ADD_FILTER, _
                           pRule, _
                           Marshal.SizeOf(rule), _
                           Nothing, _
                           0, _
                           bytesReceived, _
                           Nothing) Then

            FirewallRules_Lock.AcquireWriterLock(LOCK_TIMEOUT)
            FirewallRules.Add(rule)
            FirewallRules_Lock.ReleaseWriterLock()
        Else
            Marshal.FreeHGlobal(pRule)
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        Marshal.FreeHGlobal(pRule)
    End Sub

    Public Sub FirewallRemoveRule(ByVal drop As Boolean, ByVal protocol As UShort, _
                               ByVal sourceIp As IPAddress, ByVal sourceMask As IPAddress, ByVal sourcePort As UShort, _
                               ByVal destinationIp As IPAddress, ByVal destinationMask As IPAddress, ByVal destinationPort As UShort)

        Dim rule As New FirewallRule
        rule.protocol = protocol
        rule.drop = drop
        rule.sourcePort = IPAddress.HostToNetworkOrder(CType(sourcePort, Short))
        rule.sourceIp = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(sourceIp.ToString), Integer))
        rule.sourceMask = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(sourceMask.ToString), Integer))
        rule.destinationPort = IPAddress.HostToNetworkOrder(CType(destinationPort, Short))
        rule.destinationIp = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(destinationIp.ToString), Integer))
        rule.destinationMask = IPAddress.HostToNetworkOrder(CType(SharpPcap.Util.IPUtil.IpToLong(destinationMask.ToString), Integer))

        Dim id As Integer = -1
        Dim count As Integer = 0

        FirewallRules_Lock.AcquireWriterLock(LOCK_TIMEOUT)
        For Each r As FirewallRule In FirewallRules
            If (rule.sourceIp = r.sourceIp) AndAlso (rule.sourceMask = r.sourceMask) AndAlso (rule.sourcePort = r.sourcePort) AndAlso _
               (rule.destinationIp = r.destinationIp) AndAlso (rule.destinationMask = r.destinationMask) AndAlso (r.destinationPort = rule.destinationPort) AndAlso _
               (rule.protocol = r.protocol) AndAlso (rule.drop = r.drop) Then
                id = count
                Exit For
            End If
            count += 1
        Next

        If id = -1 Then
            Console.WriteLine("FirewallRemoveRule: firewall rule not found.")
        Else
            Dim pID As IntPtr
            pID = Marshal.AllocHGlobal(Marshal.SizeOf(id))
            Marshal.WriteInt32(pID, 0, id)

            Dim bytesReceived As Integer
            If DeviceIoControl(FirewallHandle, _
                               IOCTL_REM_FILTER, _
                               pID, _
                               Marshal.SizeOf(id), _
                               Nothing, _
                               0, _
                               bytesReceived, _
                               Nothing) Then

                FirewallRules.RemoveAt(id)
            Else
                Marshal.FreeHGlobal(pID)
                FirewallRules_Lock.ReleaseWriterLock()
                Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
                Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            End If

            Marshal.FreeHGlobal(pID)
        End If

        FirewallRules_Lock.ReleaseWriterLock()
    End Sub

    Public Sub FirewallRemoveRule(ByVal id As Integer)
        Dim pID As IntPtr
        pID = Marshal.AllocHGlobal(4)
        Marshal.WriteInt32(pID, 0, id)

        Dim bytesReceived As Integer
        If DeviceIoControl(FirewallHandle, _
                           IOCTL_REM_FILTER, _
                           pID, _
                           Marshal.SizeOf(pID), _
                           Nothing, _
                           0, _
                           bytesReceived, _
                           Nothing) Then

            FirewallRules_Lock.AcquireWriterLock(LOCK_TIMEOUT)
            FirewallRules.RemoveAt(id)
            FirewallRules_Lock.ReleaseWriterLock()
        Else
            Marshal.FreeHGlobal(pID)
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        Marshal.FreeHGlobal(pID)
    End Sub

    Public Sub FirewallClearRules()
        Dim bytesReceived As Integer
        If DeviceIoControl(FirewallHandle, _
                           IOCTL_CLEAR_FILTER, _
                           Nothing, _
                           0, _
                           Nothing, _
                           0, _
                           bytesReceived, _
                           Nothing) Then

            FirewallRules_Lock.AcquireWriterLock(LOCK_TIMEOUT)
            FirewallRules.Clear()
            FirewallRules_Lock.ReleaseWriterLock()
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If
    End Sub

    Public Sub FirewallHook()
        Dim bytesReceived As Integer
        If DeviceIoControl(FirewallHandle, _
                           IOCTL_START_IP_HOOK, _
                           Nothing, _
                           0, _
                           Nothing, _
                           0, _
                           bytesReceived, _
                           Nothing) Then
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If
    End Sub

    Public Sub FirewallUnHook()
        Dim bytesReceived As Integer
        If DeviceIoControl(FirewallHandle, _
                           IOCTL_STOP_IP_HOOK, _
                           Nothing, _
                           0, _
                           Nothing, _
                           0, _
                           bytesReceived, _
                           Nothing) Then
        Else
            Console.WriteLine(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Throw New ApplicationException(New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If
    End Sub


End Module

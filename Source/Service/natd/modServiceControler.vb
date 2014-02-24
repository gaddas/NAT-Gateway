Imports System.ServiceProcess
Imports System.Reflection
Imports System.Runtime.InteropServices


Module SCM

    'Service Control Manager object specific access types 
    Private Const STANDARD_RIGHTS_REQUIRED As Integer = &HF0000
    Private Const SC_MANAGER_CONNECT As Integer = &H1
    Private Const SC_MANAGER_CREATE_SERVICE As Integer = &H2
    Private Const SC_MANAGER_ENUMERATE_SERVICE As Integer = &H4
    Private Const SC_MANAGER_LOCK As Integer = &H8
    Private Const SC_MANAGER_QUERY_LOCK_STATUS As Integer = &H10
    Private Const SC_MANAGER_MODIFY_BOOT_CONFIG As Integer = &H20
    Private Const SC_MANAGER_ALL_ACCESS As Integer = (STANDARD_RIGHTS_REQUIRED Or SC_MANAGER_CONNECT Or SC_MANAGER_CREATE_SERVICE Or SC_MANAGER_ENUMERATE_SERVICE Or SC_MANAGER_LOCK Or SC_MANAGER_QUERY_LOCK_STATUS Or SC_MANAGER_MODIFY_BOOT_CONFIG)

    'Service Access types 
    Private Const SERVICE_QUERY_CONFIG As Integer = &H1
    Private Const SERVICE_CHANGE_CONFIG As Integer = &H2
    Private Const SERVICE_QUERY_STATUS As Integer = &H4
    Private Const SERVICE_ENUMERATE_DEPENDENTS As Integer = &H8
    Private Const SERVICE_START As Integer = &H10
    Private Const SERVICE_STOP As Integer = &H20
    Private Const SERVICE_PAUSE_CONTINUE As Integer = &H40
    Private Const SERVICE_INTERROGATE As Integer = &H80
    Private Const SERVICE_USER_DEFINED_CONTROL As Integer = &H100
    Private Const SERVICE_ALL_ACCESS As Integer = (STANDARD_RIGHTS_REQUIRED Or SERVICE_QUERY_CONFIG Or SERVICE_CHANGE_CONFIG Or SERVICE_QUERY_STATUS Or SERVICE_ENUMERATE_DEPENDENTS Or SERVICE_START Or SERVICE_STOP Or SERVICE_PAUSE_CONTINUE Or SERVICE_INTERROGATE Or SERVICE_USER_DEFINED_CONTROL)

    Private Const SERVICE_KERNEL_DRIVER As Integer = &H1                ' Driver service.
    Private Const SERVICE_WIN32_OWN_PROCESS As Integer = &H10           ' Service that runs in its own process.
    Private Const SERVICE_WIN32_SHARE_PROCESS As Integer = &H20         ' Service that shares a process with one or more other services.

    Private Const SERVICE_AUTO_START As Integer = &H2
    Private Const SERVICE_DEMAND_START As Integer = &H3
    Private Const SERVICE_ERROR_NORMAL As Integer = &H1
    Private Const SERVICE_CONFIG_DESCRIPTION As Integer = &H1

    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function OpenSCManager(ByVal ByValsMachName As String, ByVal sDbName As String, ByVal iAccess As Integer) As Integer
    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function CloseServiceHandle(ByVal sHandle As Integer) As Integer
    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function CreateService(ByVal hSCM As Integer, ByVal sName As String, ByVal sDisplay As String, ByVal iAccess As Integer, ByVal iSvcType As Integer, ByVal iStartType As Integer, ByVal iError As Integer, ByVal sPath As String, ByVal sGroup As String, ByVal iTag As Integer, ByVal sDepends As String, ByVal sUser As String, ByVal sPass As String) As Integer
    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function OpenService(ByVal hSCManager As Integer, ByVal lpServiceName As String, ByVal dwDesiredAccess As Integer) As Integer
    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function DeleteService(ByVal hSvc As Integer) As Boolean
    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function ChangeServiceConfig2(ByVal hService As Integer, ByVal dwInfoLevel As InfoLevel, <MarshalAs(UnmanagedType.Struct)> ByRef lpInfo As SERVICE_DESCRIPTION) As Boolean
    End Function

    Public Enum InfoLevel As Integer
        SERVICE_CONFIG_DESCRIPTION = 1
        SERVICE_CONFIG_FAILURE_ACTIONS = 2
    End Enum
    Public Enum ServiceStartType As Integer
        ServiceBootStart
        ServiceSystemStart
        ServiceAutoStart
        ServiceDemandStart
        ServiceDisabled
    End Enum
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure SERVICE_DESCRIPTION
        Public lpDescription As String
    End Structure

    Public ServiceName As String = "FwHookDrv"
    Public ServiceFile As String = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\drivers\FwHookDrv.sys"
    Public DisplayName As String = "NetGate Firewall Hook Driver"
    Public ServiceDescription As String = "NetGate Firewall Hook Driver"
    Public StartType As ServiceStartType = ServiceStartType.ServiceDemandStart


    Public Enum svcAction
        aNoAction = 0
        aInstall = 1
        aUnInstall = 2
    End Enum


    Public Sub InitializeService(ByVal action As svcAction)

        If action = svcAction.aInstall Then
            InstallService()
            Console.WriteLine("Firewall service Installed!")
        Else
            UninstallService()
            Console.WriteLine("Firewall service Uninstalled!")
        End If
    End Sub
    Public Sub InstallService()
        If ServiceInstalled() Then
            Exit Sub
        End If

        Dim hSCM As Integer = OpenSCManager(Nothing, Nothing, SC_MANAGER_ALL_ACCESS)
        If hSCM = 0 Then
            Console.WriteLine("InstallService: Unable to open Service Control Manager.  Error: " & New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Exit Sub
        End If

        Dim hSvc As Integer = CreateService(hSCM, ServiceName, DisplayName, _
                                            SERVICE_ALL_ACCESS, _
                                            SERVICE_KERNEL_DRIVER, _
                                            StartType, _
                                            SERVICE_ERROR_NORMAL, _
                                            ServiceFile, _
                                            Nothing, Nothing, Nothing, Nothing, Nothing)
        If hSvc = 0 Then
            CloseServiceHandle(hSvc)
            Console.WriteLine("Unable to install service, could not create service handle.  Error: " & New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Exit Sub
        End If

        Dim svd As SERVICE_DESCRIPTION
        svd.lpDescription = ServiceDescription

        ChangeServiceConfig2(hSvc, InfoLevel.SERVICE_CONFIG_DESCRIPTION, svd)

        CloseServiceHandle(hSvc)
        CloseServiceHandle(hSCM)
    End Sub
    Public Sub UninstallService()
        If Not ServiceInstalled() Then
            Exit Sub
        End If

        If Not StopService() Then
            Console.WriteLine("UninstallService: Could not stop service.  Error: " & New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Exit Sub
        End If

        Dim hSCM As Integer = OpenSCManager(Nothing, Nothing, SC_MANAGER_ALL_ACCESS)
        If hSCM = 0 Then
            Console.WriteLine("UninstallService: Unable to open Service Control Manager. Error: " & New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Exit Sub
        End If

        Dim hSvc As Integer = OpenService(hSCM, ServiceName, SERVICE_ALL_ACCESS)
        If hSvc = 0 Then
            CloseServiceHandle(hSCM)
            Console.WriteLine("UninstallService: Unable to open service: " & New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
            Exit Sub
        End If

        If Not DeleteService(hSvc) Then
            CloseServiceHandle(hSvc)
            CloseServiceHandle(hSCM)
            Console.WriteLine("UninstallService: Unable to delete service: " & New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error).Message)
        End If

        CloseServiceHandle(hSvc)
        CloseServiceHandle(hSCM)
    End Sub
    Public Function ServiceInstalled() As Boolean
        Dim services() As ServiceController
        services = ServiceController.GetServices()

        For Each sc As ServiceController In services
            If sc.ServiceName = ServiceName Then
                Return True
            End If
        Next

        Return False
    End Function
    Public Function StopService() As Boolean
        Dim iTimeout As Integer = 0
        Dim bTimeout As Boolean = False

        Dim svc As New ServiceController(ServiceName)
        If svc.Status = ServiceControllerStatus.Stopped Then
            Return True
        End If

        svc.Stop()
        iTimeout = 0
        While (svc.Status <> ServiceControllerStatus.Stopped)
            System.Threading.Thread.Sleep(1000)
            iTimeout += 1
            If iTimeout > 120 Then
                bTimeout = True
                Exit While
            End If
            svc.Refresh()
        End While

        Return Not bTimeout
    End Function
    Public Function StartService() As Boolean
        Dim iTimeout As Integer = 0
        Dim bTimeout As Boolean = False

        Dim svc As New ServiceController(ServiceName)
        If svc.Status = ServiceControllerStatus.Running Then
            Return True
        End If

        svc.Start()
        iTimeout = 0
        While (svc.Status <> ServiceControllerStatus.Running)
            System.Threading.Thread.Sleep(1000)
            iTimeout += 1
            If iTimeout > 120 Then
                bTimeout = True
                Exit While
            End If
            svc.Refresh()
        End While

        Return Not bTimeout
    End Function


End Module



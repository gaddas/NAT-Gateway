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
Imports System.Runtime.InteropServices
Imports System.Diagnostics.Process
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Threading
Imports System.ComponentModel


Public Module modCommon

    Public Const LOCK_TIMEOUT As Integer = 9000

    Public m_Server As dnsServer
    Public m_RemoteChannel As Channels.IChannel = Nothing
    Public m_RemoteFactory As remoteFactoryDNS = Nothing
    Public m_RemoteFactoryObj As ObjRef = Nothing

    Public m_RequestIdCounter As Integer = 1

    ' RFC1035 3.2.2/3
    Public Enum dnsType
        TYPE_NONE = 0
        TYPE_ANAME = 1
        TYPE_NS = 2
        TYPE_SOA = 6
        TYPE_MX = 15
    End Enum

    ' Internet will be the one we'll be using (IN), the others are for completeness
    ' RFC1035 3.2.4/5
    Public Enum dnsClass
        CLASS_NONE = 0
        CLASS_IN = 1
        CLASS_CS = 2
        CLASS_CH = 3
        CLASS_HS = 4
    End Enum


    ' These are the return codes the server can send back
    ' RFC1035 4.1.1
    Public Enum ResponseCode
        SUCCESS = 0                 'No error condition
        FORMAT_ERROR = 1            'The name server was unable to interpret the query.
        SERVER_FAILURE = 2          'The name server was unable to process this query due to a problem with the name server.
        NAME_ERROR = 3              'Meaningful only for responses from an authoritative name server, this code signifies that the domain name referenced in the query does not exist.
        NOT_IMPLEMENTED = 4         'The name server does not support the requested kind of query.
        REFUSED = 5                 'The name server refuses to perform the specified operation for policy reasons.
    End Enum


    ' These are the Query Types which apply to all questions in a request
    ' RFC1035 4.1.1
    Public Enum Opcode
        STANDARD_QUERY = 0
        INVERSE_QUERY = 1
        STATUS_REQUEST = 2
    End Enum

End Module

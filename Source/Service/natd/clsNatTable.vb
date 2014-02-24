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


Imports System.Collections.Generic
Imports System.Net
Imports SharpPcap
Imports SharpPcap.Packets


Public Class natConfigTable
    Public Function Chain(ByVal s As String) As List(Of natConfigChain)
        Select Case s.ToUpper
            Case "PREROUTING"
                Return PREROUTING
            Case "FORWARD"
                Return FORWARD
            Case "POSTROUTING"
                Return POSTROUTING
        End Select

        Return Nothing
    End Function

    Public Sub Flush()
        PREROUTING.Clear()
        FORWARD.Clear()
        POSTROUTING.Clear()
    End Sub

    Public PREROUTING As New List(Of natConfigChain)
    Public FORWARD As New List(Of natConfigChain)
    Public POSTROUTING As New List(Of natConfigChain)
End Class
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


Public Class Cachetable
    Inherits Dictionary(Of String, Packet)

    Private m_Size As Integer = 10000
    Public Property Size() As Integer
        Get
            Return m_Size
        End Get
        Set(ByVal value As Integer)
            m_Size = value
        End Set
    End Property

    Private m_StatsUsed As New Dictionary(Of String, Integer)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overloads Sub Add(ByVal key As String, ByVal value As Packet)
        If Me.Count >= m_Size Then
            Dim eMin As Integer = Integer.MaxValue
            Dim eIdx As String = String.Empty
            For Each e As KeyValuePair(Of String, Integer) In m_StatsUsed
                If eMin < e.Value Then
                    eMin = e.Value
                    eIdx = e.Key
                End If
            Next

            m_StatsUsed.Remove(eIdx)
            MyBase.Remove(eIdx)
        End If

        m_StatsUsed.Add(key, 1)
        MyBase.Add(key, value)
    End Sub

    Public Overloads Function ContainsKey(ByVal key As String) As Boolean
        If m_StatsUsed.ContainsKey(key) Then m_StatsUsed(key) += 1
        Return MyBase.ContainsKey(key)
    End Function

    Public Overloads Sub Clear()
        m_StatsUsed.Clear()
        MyBase.Clear()
    End Sub


End Class

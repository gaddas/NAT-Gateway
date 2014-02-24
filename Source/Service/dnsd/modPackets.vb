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
Imports System.Diagnostics.Process
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Threading
Imports System.ComponentModel


Public Module Packets


    Public Class Packet
        Public Const BUFFER_SIZE As Integer = 4096

        Public Data() As Byte
        Public Length As Integer
        Public Offset As Integer = 0
        Public IP As IPEndPoint

        Public Sub New()
            Length = 0
            Data = New Byte() {}
        End Sub
        Public Sub Prepare()
            ReDim Data(BUFFER_SIZE - 1)
        End Sub

        Public Overrides Function ToString() As String
            Dim j As Integer
            Dim buffer As New System.Text.StringBuilder

            Dim bytes() As Byte = Data
            ReDim Preserve bytes(Length - 1)

            'Build string
            If bytes.Length Mod 16 = 0 Then
                For j = 0 To bytes.Length - 1 Step 16
                    buffer.Append("|  ")
                    buffer.Append(BitConverter.ToString(bytes, j, 16).Replace("-", " "))
                    buffer.Append(" |  ")
                    buffer.Append(System.Text.Encoding.ASCII.GetString(bytes, j, 16).Replace(vbTab, "?").Replace(vbBack, "?").Replace(vbCr, "?").Replace(vbFormFeed, "?").Replace(vbLf, "?"))
                    buffer.Append(" |")
                    buffer.Append(vbNewLine)
                Next
            Else
                For j = 0 To bytes.Length - 1 - 16 Step 16
                    buffer.Append("|  ")
                    buffer.Append(BitConverter.ToString(bytes, j, 16).Replace("-", " "))
                    buffer.Append(" |  ")
                    buffer.Append(System.Text.Encoding.ASCII.GetString(bytes, j, 16).Replace(vbTab, "?").Replace(vbBack, "?").Replace(vbCr, "?").Replace(vbFormFeed, "?").Replace(vbLf, "?"))
                    buffer.Append(" |")
                    buffer.Append(vbNewLine)
                Next

                buffer.Append("|  ")
                buffer.Append(BitConverter.ToString(bytes, j, bytes.Length Mod 16).Replace("-", " "))
                buffer.Append(New String(" ", (16 - bytes.Length Mod 16) * 3))
                buffer.Append(" |  ")
                buffer.Append(System.Text.Encoding.ASCII.GetString(bytes, j, bytes.Length Mod 16).Replace(vbTab, "?").Replace(vbBack, "?").Replace(vbCr, "?").Replace(vbFormFeed, "?").Replace(vbLf, "?"))
                buffer.Append(New String(" ", 16 - bytes.Length Mod 16))
                buffer.Append(" |")
                buffer.Append(vbNewLine)
            End If

            Return buffer.ToString
        End Function

        Public Function PeekByte() As Byte
            Return Data(Offset)
        End Function

        Public Function GetByte() As Byte
            Offset = Offset + 1
            Return Data(Offset - 1)
        End Function
        Public Function GetUInt16() As UShort
            Dim num1 As UShort
            Offset += 2
            num1 += CType(Data(Offset - 1), UShort)
            num1 += CType(Data(Offset - 2), UShort) << 8
            Return num1
        End Function
        Public Function GetUInt32() As UInteger
            Dim num1 As UInteger
            Offset += 4
            num1 += CType(Data(Offset - 1), UInteger)
            num1 += CType(Data(Offset - 2), UInteger) << 8
            num1 += CType(Data(Offset - 3), UInteger) << 16
            num1 += CType(Data(Offset - 4), UInteger) << 24
            Return num1
        End Function
  
        Public Function GetString() As String
            Dim start As Integer = Offset + 1
            Dim len As Integer = Data(Offset)
            Offset = Offset + len + 1

            Return System.Text.Encoding.ASCII.GetString(Data, start, len)
        End Function

        Public Sub AddByte(ByVal buffer As Byte)
            If Data Is Nothing Then
                ReDim Preserve Data(0)
            Else
                ReDim Preserve Data(Data.Length)
            End If

            Data(Data.Length - 1) = buffer

            Length += 1
        End Sub
        Public Sub AddUInt16(ByVal buffer As UShort)
            ReDim Preserve Data(Data.Length + 1)

            Data(Data.Length - 1) = CType((buffer And 255), Byte)
            Data(Data.Length - 2) = CType(((buffer >> 8) And 255), Byte)

            Length += 2
        End Sub
        Public Sub AddUInt32(ByVal buffer As UInteger)
            ReDim Preserve Data(Data.Length + 3)

            Data(Data.Length - 1) = CType((buffer And 255), Byte)
            Data(Data.Length - 2) = CType(((buffer >> 8) And 255), Byte)
            Data(Data.Length - 3) = CType(((buffer >> 16) And 255), Byte)
            Data(Data.Length - 4) = CType(((buffer >> 24) And 255), Byte)

            Length += 4
        End Sub
        
        Public Sub AddString(ByVal buffer As String)
            Dim bytes As Byte() = System.Text.Encoding.ASCII.GetBytes(buffer.ToCharArray)

            ReDim Preserve Data(Data.Length + bytes.Length)

            Data(Data.Length - bytes.Length - 1) = buffer.Length
            For i = 0 To bytes.Length - 1
                Data(Data.Length - bytes.Length + i) = bytes(i)
            Next i

            Length += buffer.Length + 1
        End Sub

        Public Sub AddIPAddress(ByVal buffer As IPAddress)
            ReDim Preserve Data(Data.Length + 3)
            Dim bytes() As Byte = buffer.GetAddressBytes()

            Data(Data.Length - 4) = bytes(0)
            Data(Data.Length - 3) = bytes(1)
            Data(Data.Length - 2) = bytes(2)
            Data(Data.Length - 1) = bytes(3)

            Length += 4
        End Sub
    End Class


End Module
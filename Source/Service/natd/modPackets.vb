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



    Public Class MyPacket
        Implements IDisposable

        Public Data() As Byte
        Public Offset As Integer = 0

        Public m_HeaderChecksumOffset As Integer = 0
        Public m_HeaderChecksumStart As Integer = 0
        Public m_HeaderChecksumEnd As Integer = 0

        Public Sub New()
            Data = New Byte() {}
        End Sub
        Public Sub New(ByRef rawdata() As Byte)
            Data = rawdata
            rawdata.CopyTo(Data, 0)
        End Sub

        Public Sub AddBitArray(ByVal buffer As BitArray, ByVal Len As Integer)
            ReDim Preserve Data(Data.Length - 1 + Len)

            Dim bufferarray(CType((buffer.Length + 8) / 8, Byte)) As Byte

            buffer.CopyTo(bufferarray, 0)
            Array.Copy(bufferarray, 0, Data, Data.Length - Len, Len)
        End Sub
        Public Sub AddByte(ByVal buffer As Byte, Optional ByVal position As Integer = 0)
            If position <= 0 OrElse position >= Data.Length Then
                position = Data.Length
                ReDim Preserve Data(Data.Length)
            End If
            Data(position) = buffer
        End Sub
        Public Sub AddInt16(ByVal buffer As Short)
            ReDim Preserve Data(Data.Length + 1)

            Data(Data.Length - 2) = CType((buffer And 255), Byte)
            Data(Data.Length - 1) = CType(((buffer >> 8) And 255), Byte)
        End Sub
        Public Sub AddInt32(ByVal buffer As Integer, Optional ByVal position As Integer = 0)
            ReDim Preserve Data(Data.Length + 3)

            Data(Data.Length) = CType((buffer And 255), Byte)
            Data(Data.Length + 1) = CType(((buffer >> 8) And 255), Byte)
            Data(Data.Length + 2) = CType(((buffer >> 16) And 255), Byte)
            Data(Data.Length + 3) = CType(((buffer >> 24) And 255), Byte)
        End Sub
        Public Sub AddInt64(ByVal buffer As Long, Optional ByVal position As Integer = 0)
            ReDim Preserve Data(Data.Length + 7)

            Data(Data.Length) = CType((buffer And 255), Byte)
            Data(Data.Length + 1) = CType(((buffer >> 8) And 255), Byte)
            Data(Data.Length + 2) = CType(((buffer >> 16) And 255), Byte)
            Data(Data.Length + 3) = CType(((buffer >> 24) And 255), Byte)
            Data(Data.Length + 4) = CType(((buffer >> 32) And 255), Byte)
            Data(Data.Length + 5) = CType(((buffer >> 40) And 255), Byte)
            Data(Data.Length + 6) = CType(((buffer >> 48) And 255), Byte)
            Data(Data.Length + 7) = CType(((buffer >> 56) And 255), Byte)
        End Sub
        Public Sub AddDouble(ByVal buffer2 As Double)
            Dim buffer1 As Byte() = BitConverter.GetBytes(buffer2)
            ReDim Preserve Data(Data.Length + buffer1.Length - 1)
            Buffer.BlockCopy(buffer1, 0, Data, Data.Length - buffer1.Length, buffer1.Length)
        End Sub
        Public Sub AddSingle(ByVal buffer2 As Single)
            Dim buffer1 As Byte() = BitConverter.GetBytes(buffer2)
            ReDim Preserve Data(Data.Length + buffer1.Length - 1)
            Buffer.BlockCopy(buffer1, 0, Data, Data.Length - buffer1.Length, buffer1.Length)
        End Sub
        Public Sub AddUInt16(ByVal buffer As UShort)
            ReDim Preserve Data(Data.Length + 1)

            Data(Data.Length - 1) = CType((buffer And 255), Byte)
            Data(Data.Length - 2) = CType(((buffer >> 8) And 255), Byte)
        End Sub
        Public Sub AddUInt32(ByVal buffer As UInteger)
            ReDim Preserve Data(Data.Length + 3)

            Data(Data.Length - 1) = CType((buffer And 255), Byte)
            Data(Data.Length - 2) = CType(((buffer >> 8) And 255), Byte)
            Data(Data.Length - 3) = CType(((buffer >> 16) And 255), Byte)
            Data(Data.Length - 4) = CType(((buffer >> 24) And 255), Byte)
        End Sub
        Public Sub AddUInt64(ByVal buffer As ULong)
            ReDim Preserve Data(Data.Length + 7)

            Data(Data.Length - 1) = CType((buffer And 255), Byte)
            Data(Data.Length - 2) = CType(((buffer >> 8) And 255), Byte)
            Data(Data.Length - 3) = CType(((buffer >> 16) And 255), Byte)
            Data(Data.Length - 4) = CType(((buffer >> 24) And 255), Byte)
            Data(Data.Length - 5) = CType(((buffer >> 32) And 255), Byte)
            Data(Data.Length - 6) = CType(((buffer >> 40) And 255), Byte)
            Data(Data.Length - 7) = CType(((buffer >> 48) And 255), Byte)
            Data(Data.Length - 8) = CType(((buffer >> 56) And 255), Byte)
        End Sub
        Public Sub AddString(ByVal buffer As String)
            If IsDBNull(buffer) Or buffer = String.Empty Then
                Me.AddByte(0)
            Else
                Dim Bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(buffer.ToCharArray)

                ReDim Preserve Data(Data.Length + Bytes.Length)

                Dim i As Integer
                For i = 0 To Bytes.Length - 1
                    Data(Data.Length - 1 - Bytes.Length + i) = Bytes(i)
                Next i

                Data(Data.Length - 1) = 0
            End If
        End Sub
        Public Sub AddIPAddress(ByVal buffer As IPAddress)
            ReDim Preserve Data(Data.Length + 3)
            Dim bytes() As Byte = buffer.GetAddressBytes()

            Data(Data.Length - 4) = bytes(0)
            Data(Data.Length - 3) = bytes(1)
            Data(Data.Length - 2) = bytes(2)
            Data(Data.Length - 1) = bytes(3)
        End Sub
        Public Sub AddHWAddress(ByVal buffer As String)
            Dim hw As String() = buffer.Split(":")

            Dim hw0 As Byte = Convert.ToByte(hw(0), 16)
            Dim hw1 As Byte = Convert.ToByte(hw(1), 16)
            Dim hw2 As Byte = Convert.ToByte(hw(2), 16)
            Dim hw3 As Byte = Convert.ToByte(hw(3), 16)
            Dim hw4 As Byte = Convert.ToByte(hw(4), 16)
            Dim hw5 As Byte = Convert.ToByte(hw(5), 16)

            ReDim Preserve Data(Data.Length + 5)
            Data(Data.Length - 6) = hw0
            Data(Data.Length - 5) = hw1
            Data(Data.Length - 4) = hw2
            Data(Data.Length - 3) = hw3
            Data(Data.Length - 2) = hw4
            Data(Data.Length - 1) = hw5
        End Sub
        Public Sub AddStringFixed(ByVal buffer As String, ByVal strLength As Byte)
            Dim bytes As Byte() = System.Text.Encoding.ASCII.GetBytes(buffer.ToCharArray)

            ReDim Preserve bytes(strLength - 1)
            ReDim Preserve Data(Data.Length + bytes.Length - 1)

            For i = 0 To bytes.Length - 1
                Data(Data.Length - bytes.Length + i) = bytes(i)
            Next i
        End Sub
        Public Sub AddByteArray(ByVal buffer() As Byte)
            Dim tmp As Integer = Data.Length
            ReDim Preserve Data(Data.Length + buffer.Length - 1)
            Array.Copy(buffer, 0, Data, tmp, buffer.Length)
        End Sub

        Public Sub HeaderChecksumStart()
            m_HeaderChecksumStart = Data.Length
        End Sub
        Public Sub HeaderChecksumEnd()
            m_HeaderChecksumEnd = Data.Length
        End Sub
        Public Sub HeaderChecksumMark()
            m_HeaderChecksumOffset = Data.Length
            Me.AddUInt16(0)
        End Sub
        Public Sub HeaderChecksumCalc()
            Dim Sum As Long = 0
            Dim word16 As UShort

            Offset = m_HeaderChecksumStart
            While Offset < m_HeaderChecksumEnd - 1
                word16 = Me.GetUInt16
                Sum += CLng(word16)
            End While

            While (Sum >> 16) <> 0
                Sum = (Sum And &HFFFF) + (Sum >> 16)
            End While
            Sum = CType(Not Sum, UShort)

            Data(m_HeaderChecksumOffset + 1) = CType((Sum And 255), Byte)
            Data(m_HeaderChecksumOffset + 0) = CType(((Sum >> 8) And 255), Byte)
        End Sub

        Public Function GetByte() As Byte
            Offset = Offset + 1
            Return Data(Offset - 1)
        End Function
        Public Function GetInt16() As Short
            Dim num1 As Short = BitConverter.ToInt16(Data, Offset)
            Offset = (Offset + 2)
            Return num1
        End Function
        Public Function GetInt32() As Integer
            Dim num1 As Integer = BitConverter.ToInt32(Data, Offset)
            Offset = (Offset + 4)
            Return num1
        End Function
        Public Function GetInt64() As Long
            Dim num1 As Long = BitConverter.ToInt64(Data, Offset)
            Offset = (Offset + 8)
            Return num1
        End Function
        Public Function GetUInt16() As UShort
            Dim num1 As UShort
            Offset += 2
            num1 += CType(Data(Offset - 1), UInteger)
            num1 += CType(Data(Offset - 2), UInteger) << 8
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
        Public Function GetUInt64() As ULong
            Dim num1 As ULong = BitConverter.ToUInt64(Data, Offset)
            Offset = (Offset + 8)
            Return num1
        End Function
        Public Function GetFloat() As Single
            Dim single1 As Single = BitConverter.ToSingle(Data, Offset)
            Offset = (Offset + 4)
            Return single1
        End Function
        Public Function GetDouble() As Double
            Dim num1 As Double = BitConverter.ToDouble(Data, Offset)
            Offset = (Offset + 8)
            Return num1
        End Function
        Public Function GetString() As String
            Dim start As Integer = Offset
            Dim i As Integer = 0

            While Data(start + i) <> 0
                i = i + 1
                Offset = Offset + 1
            End While
            Offset = Offset + 1

            Return System.Text.Encoding.ASCII.GetString(Data, start, i)
        End Function
        Public Function GetIPAddress() As String
            Dim ip0 As Byte = Data(Offset + 3)
            Dim ip1 As Byte = Data(Offset + 2)
            Dim ip2 As Byte = Data(Offset + 1)
            Dim ip3 As Byte = Data(Offset + 0)
            Offset = (Offset + 4)
            Return String.Format("{0}.{1}.{2}.{3}", ip0, ip1, ip2, ip3)
        End Function
        Public Function GetHWAddress() As String
            Dim hw0 As Byte = Data(Offset + 5)
            Dim hw1 As Byte = Data(Offset + 4)
            Dim hw2 As Byte = Data(Offset + 3)
            Dim hw3 As Byte = Data(Offset + 2)
            Dim hw4 As Byte = Data(Offset + 1)
            Dim hw5 As Byte = Data(Offset + 0)
            Offset = (Offset + 6)
            Return String.Format("{0:x2}:{1:x2}:{2:x2}:{3:x2}:{4:x2}:{5:x2}", hw0, hw1, hw2, hw3, hw4, hw5)
        End Function
        Public Function GetString(ByVal Len As Integer) As String
            Dim start As Integer = Offset
            Offset = Offset + Len

            Return System.Text.Encoding.ASCII.GetString(Data, start, Len)
        End Function


        Public Sub Dispose() Implements System.IDisposable.Dispose
        End Sub

        Public Overrides Function ToString() As String
            Dim j As Integer
            Dim buffer As New System.Text.StringBuilder

            Dim bytes() As Byte = Data
            ReDim Preserve bytes(Data.Length - 1)

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
    End Class

End Module
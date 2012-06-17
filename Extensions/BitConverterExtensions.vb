#Region "License"
' Copyright 2012 Steven Liekens
' Contact: steven.liekens@gmail.com

' This file is part of DuneApiCodepack.

' DuneApiCodepack is free software: you can redistribute it and/or modify
' it under the terms of the GNU Lesser General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' DuneApiCodepack is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Lesser General Public License for more details.

' You should have received a copy of the GNU Lesser General Public License
' along with DuneApiCodepack.  If not, see <http://www.gnu.org/licenses/>.
#End Region ' License
Imports System.Runtime.CompilerServices

Namespace Extensions

    ''' <summary>
    ''' Builds on, but does not really extend, the <see cref="BitConverter"/> type.
    ''' This is because BitConverter is a static type, and extensions are driven by instances of types.
    ''' In other words: call these methods directly.
    ''' </summary>
    Public Module BitConverterExtensions
        Public Function ToString(value() As Byte, delimiter As Char) As String
            Return BitConverterExtensions.ToString(value, 0, value.Length, delimiter)
        End Function

        Public Function ToString(value() As Byte, startIndex As Integer, delimiter As Char) As String
            Return BitConverterExtensions.ToString(value, startIndex, value.Length, delimiter)
        End Function

        Public Function ToString(value() As Byte, startIndex As Integer, length As Integer, delimiter As Char) As String
            Dim bytes As String = BitConverter.ToString(value, startIndex, length)
            If delimiter = Nothing Then
                Return bytes.Replace("-", String.Empty)
            Else
                Return bytes.Replace("-"c, delimiter)
            End If
        End Function

    End Module

End Namespace
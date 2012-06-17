﻿Imports System.Runtime.CompilerServices

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
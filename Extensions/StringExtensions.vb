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

    Public Module StringExtensions

        ''' <summary>
        ''' Indicates whether the instance is null or a System.String.Empty string.
        ''' </summary>
        <Extension()>
        Public Function IsNullOrEmpty(text As String) As Boolean
            Return String.IsNullOrEmpty(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is not null or not a System.String.Empty string.
        ''' </summary>
        <Extension()>
        Public Function IsNotNullOrEmpty(text As String) As Boolean
            Return Not String.IsNullOrEmpty(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is null, empty or consists only of white-space characters.
        ''' </summary>
        <Extension()>
        Public Function IsNullOrWhiteSpace(text As String) As Boolean
            Return String.IsNullOrWhiteSpace(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is not null, not empty or does not consist only of white-space characters.
        ''' </summary>
        <Extension()>
        Public Function IsNotNullOrWhiteSpace(text As String) As Boolean
            Return Not String.IsNullOrWhiteSpace(text)
        End Function

        ''' <summary>
        ''' Sets the instance to a System.String.Empty string.
        ''' </summary>
        <Extension()>
        Public Sub Clear(ByRef text As String)
            text = String.Empty
        End Sub

        ''' <summary>
        ''' Returns a substring, starting from the left.
        ''' </summary>
        <Extension()>
        Public Function Left(text As String, length As Integer) As String
            Return text.Substring(0, length)
        End Function

        ''' <summary>
        ''' Returns a substring, starting from the right.
        ''' </summary>
        <Extension()>
        Public Function Right(text As String, length As Integer) As String
            Return text.Substring(text.Length - length)
        End Function

        ''' <summary>
        ''' Converts the specified string to a boolean.
        ''' </summary>
        ''' <returns>False is the specified value is "0" or Boolean.FalseString; otherwise true.</returns>
        <Extension()>
        Public Function ToBoolean(text As String) As Boolean
            Select Case text.ToLower
                Case "0", Boolean.FalseString.ToLowerInvariant
                    Return False
                Case Else
                    Return True
            End Select
        End Function

    End Module

End Namespace
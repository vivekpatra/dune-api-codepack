#Region "License"
' Copyright 2012-2013 Steven Liekens
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
    ''' Extensions for the <see cref="String"/> type.
    ''' </summary>
    Public Module StringExtensions

        ''' <summary>
        ''' Indicates whether the instance is null or a System.String.Empty string.
        ''' </summary>
        <DebuggerStepThrough>
        <Extension()>
        Public Function IsNullOrEmpty(text As String) As Boolean
            Return String.IsNullOrEmpty(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is not null or not a System.String.Empty string.
        ''' </summary>
        <DebuggerStepThrough>
        <Extension()>
        Public Function IsNotNullOrEmpty(text As String) As Boolean
            Return Not String.IsNullOrEmpty(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is null, empty or consists only of white-space characters.
        ''' </summary>
        <DebuggerStepThrough>
        <Extension()>
        Public Function IsNullOrWhiteSpace(text As String) As Boolean
            Return String.IsNullOrWhiteSpace(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is not null, not empty or does not consist only of white-space characters.
        ''' </summary>
        <DebuggerStepThrough>
        <Extension()>
        Public Function IsNotNullOrWhiteSpace(text As String) As Boolean
            Return Not String.IsNullOrWhiteSpace(text)
        End Function

        ''' <summary>
        ''' Returns a substring, starting from the left.
        ''' </summary>
        <DebuggerStepThrough>
        <Extension()>
        Public Function Left(value As String, length As Integer) As String
            Return value.Substring(0, length)
        End Function

        ''' <summary>
        ''' Returns a substring, starting from the right.
        ''' </summary>
        <DebuggerStepThrough>
        <Extension()>
        Public Function Right(value As String, length As Integer) As String
            Return value.Substring(value.Length - length)
        End Function

        <DebuggerStepThrough>
        <Extension()>
        Public Function EqualsInvariantIgnoreCase(value As String, text As String) As Boolean
            Return String.Equals(value, text, StringComparison.InvariantCultureIgnoreCase)
        End Function

        <DebuggerStepThrough>
        <Extension()>
        Public Function EqualsInvariantIgnoreCaseAny(value As String, ParamArray text() As String) As Boolean
            For Each s In text
                If String.Equals(value, s, StringComparison.InvariantCultureIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

        <DebuggerStepThrough>
        <Extension()>
        Public Function GetValueOrDefault(value As String) As String
            If value Is Nothing Then
                Return String.Empty
            Else
                Return value
            End If
        End Function

        <DebuggerStepThrough>
        <Extension()>
        Public Function GetValueOrDefault(value As String, defaultValue As String) As String
            If value Is Nothing Then
                Return defaultValue
            Else
                Return value
            End If
        End Function

    End Module

End Namespace
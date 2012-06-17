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
    ''' Extensions for the Boolean type.
    ''' </summary>
    Public Module BooleanExtensions

        ''' <summary>
        ''' Toggles the value when called.
        ''' <example>Example use case: <c>duneInstance.PlaybackMute.Toggle()</c></example>
        ''' </summary>
        <Extension()>
        Public Sub Toggle(ByRef value As Boolean)
            value = Not value
        End Sub

        ''' <summary>
        ''' Returns "1" if true; otherwise "0".
        ''' </summary>
        <Extension()>
        Public Function ToNumberString(value As Boolean) As String
            Return Math.Abs(CInt(value)).ToString(Constants.FormatProvider)
        End Function

        <Extension()>
        Public Function GetNumberString(value As Boolean) As String
            If value.IsTrue Then
                Return "1"
            Else
                Return "0"
            End If
        End Function

        ''' <summary>
        ''' Gets whether the value is True.
        ''' </summary>
        <Extension()>
        Public Function IsTrue(value As Boolean) As Boolean
            Return value = True
        End Function

        ''' <summary>
        ''' Gets whether the value is False.
        ''' </summary>
        <Extension()>
        Public Function IsFalse(value As Boolean) As Boolean
            Return value = False
        End Function

    End Module

End Namespace
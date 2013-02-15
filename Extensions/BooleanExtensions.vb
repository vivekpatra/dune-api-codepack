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
    ''' Extensions for the <see cref="Boolean"/> type.
    ''' </summary>
    Public Module BooleanExtensions

        <Extension()>
        Public Sub Toggle(ByRef value As Boolean)
            value = Not value
        End Sub

        ''' <summary>
        ''' Returns "1" if true; otherwise "0".
        ''' </summary>
        <Extension()>
        Public Function ToNumberString(value As Boolean) As String
            Return Math.Abs(CInt(value)).ToString(Globalization.CultureInfo.InvariantCulture)
        End Function

    End Module

End Namespace
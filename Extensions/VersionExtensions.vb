﻿#Region "License"
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
    ''' Extensions for the <see cref="Version"/> type.
    ''' </summary>
    Public Module VersionExtensions

        <Extension()>
        Public Function Parse(version As Version, input As String, strict As Boolean) As Version
            If strict Then
                Return System.Version.Parse(input)
            Else
                If input.Contains("."c).IsFalse Then
                    input += ".0"
                End If
                Return System.Version.Parse(input)
            End If
        End Function

        

    End Module
End Namespace

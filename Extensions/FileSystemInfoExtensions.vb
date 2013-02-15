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
Imports System.IO

Namespace Extensions

    ''' <summary>
    ''' Extensions for the FileSystemInfo type.
    ''' </summary>
    Public Module FileSystemInfoExtensions

        ''' <summary>
        ''' Gets a <see cref="System.Uri"/> instance for the specified path.
        ''' </summary>
        <Extension()>
        Public Function GetUri(entry As IO.FileSystemInfo) As Uri
            Return New Uri(entry.FullName)
        End Function

        ''' <summary>
        ''' Gets the root directory of this instance.
        ''' </summary>
        <Extension()>
        Public Function GetRoot(entry As IO.FileSystemInfo) As IO.DirectoryInfo
            Return New IO.DirectoryInfo(entry.FullName).Root
        End Function

        ''' <summary>
        ''' Creates a new instance of <see cref="IO.DirectoryInfo"/>.
        ''' </summary>
        <Extension()>
        Public Function ToDirectoryInfo(entry As IO.FileSystemInfo) As IO.DirectoryInfo
            Return New IO.DirectoryInfo(entry.FullName)
        End Function

        ''' <summary>
        ''' Creates a new instance of <see cref="IO.FileInfo"/>.
        ''' </summary>
        <Extension()>
        Public Function ToFileInfo(entry As IO.FileSystemInfo) As IO.FileInfo
            Return New IO.FileInfo(entry.FullName)
        End Function

    End Module

End Namespace

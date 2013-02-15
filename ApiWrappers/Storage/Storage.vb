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
Imports System.IO
Imports System.Net

Namespace Sources

    ''' <summary>
    ''' This is the base class for all types that represent a storage device, whether it be local or remote, accessible or inaccessible.
    ''' This class cannot be instantiated directly.
    ''' </summary>
    Public MustInherit Class StorageDevice
        Friend _host As IPAddress
        Friend _root As IO.DirectoryInfo

        Protected Sub New(host As IPAddress)
            _host = host
        End Sub

        Public ReadOnly Property Host As IPAddress
            Get
                Return _host
            End Get
        End Property

        Public ReadOnly Property Root As IO.DirectoryInfo
            Get
                Return _root
            End Get
        End Property

        Public Overridable Function GetMediaUrl() As String
            Return Root.FullName
        End Function

    End Class

End Namespace


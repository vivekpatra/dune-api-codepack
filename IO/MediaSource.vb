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

Namespace InOut

    Public Class MediaSource : Implements IMediaSource

        Private _name As String
        Private _path As String
        Private _type As ListType
        Private _size As String
        Private _contentType As Mime.ContentType

        Public Sub New(parent As String, name As String, type As ListType, size As String, contentType As Mime.ContentType)
            Me.Name = name
            Me.Type = type
            Me.Path = UnixPath.Join(parent, name)
            Me.Size = size
            Me.ContentType = contentType
        End Sub

        Public Property Path As String Implements IMediaSource.Path
            Get
                Return _path
            End Get
            Private Set(value As String)
                _path = UnixPath.Normalize(value)
            End Set
        End Property

        Public Property Name As String Implements IMediaSource.Name
            Get
                Return _name
            End Get
            Private Set(value As String)
                If Not String.IsNullOrEmpty(value) Then
                    _name = value
                End If
            End Set
        End Property

        Public Property Type As ListType Implements IMediaSource.Type
            Get
                Return _type
            End Get
            Private Set(value As ListType)
                _type = value
            End Set
        End Property

        Public Property Size As String
            Get
                Return _size
            End Get
            Private Set(value As String)
                _size = value
            End Set
        End Property

        Public Property ContentType As Mime.ContentType
            Get
                Return _contentType
            End Get
            Private Set(value As Mime.ContentType)
                _contentType = value
            End Set
        End Property

        Public Function GetMediaUrl() As String Implements IMediaSource.GetMediaUrl
            Return "storage_name://" & Me.Path.TrimStart(UnixPath.Separator)
        End Function

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

    End Class

End Namespace

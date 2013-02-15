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

Namespace IPControl

    ''' <summary>
    ''' A helper class for retrieving and comparing list types and for creating new list types.
    ''' </summary>
    Public Class ListType : Inherits NameValuePair

        Private Shared ReadOnly _folder As ListType = New ListType("folder")
        Private Shared ReadOnly _audio As ListType = New ListType("audio")
        Private Shared ReadOnly _video As ListType = New ListType("video")
        Private Shared ReadOnly _image As ListType = New ListType("image")
        Private Shared ReadOnly _playlist As ListType = New ListType("playlist")
        Private Shared ReadOnly _unknown As ListType = New ListType("unknown")

        Public Sub New(type As String)
            MyBase.New(type)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Output.ListType.Name
            End Get
        End Property

        Public Shared ReadOnly Property Folder As ListType
            Get
                Return ListType._folder
            End Get
        End Property

        Public Shared ReadOnly Property Audio As ListType
            Get
                Return ListType._audio
            End Get
        End Property

        Public Shared ReadOnly Property Video As ListType
            Get
                Return ListType._video
            End Get
        End Property

        Public Shared ReadOnly Property Image As ListType
            Get
                Return ListType._image
            End Get
        End Property

        Public Shared ReadOnly Property Playlist As ListType
            Get
                Return ListType._playlist
            End Get
        End Property

        Public Shared ReadOnly Property Unknown As ListType
            Get
                Return ListType._unknown
            End Get
        End Property

    End Class

End Namespace
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
Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    Public Class LaunchFlashLiteCommand
        Inherits Command

        Private _mediaUrl As String
        Private _flashvars As HttpQuery

        Public Sub New(target As Dune, mediaUrl As String)
            MyBase.New(target)
            _mediaUrl = mediaUrl
            _flashvars = New HttpQuery
        End Sub

        Public Sub New(target As Dune, mediaUrl As String, flashvars As HttpQuery)
            Me.New(target, mediaUrl)
            _flashvars.Add(flashvars)
        End Sub

        ''' <summary>
        ''' Gets the media URL for the requested Flash Lite application.
        ''' </summary>
        Public ReadOnly Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
        End Property

        ''' <summary>
        ''' Gets the collection of Flash Lite startup parameters.
        ''' </summary>
        Public ReadOnly Property Flashvars As HttpQuery
            Get
                Return _flashvars
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.LaunchMediaUrl)
            query.Add(Constants.StartPlaybackParameterNames.MediaUrl, "swf://" + MediaUrl)

            Return query
        End Function

        Protected Overrides Function GetQueryString() As String
            If Flashvars.Count > 0 Then
                Dim query As New Text.StringBuilder
                query.Append(MyBase.GetQueryString())
                query.Append(":::")
                query.Append(Flashvars.ToString)
                Return query.ToString
            Else
                Return MyBase.GetQueryString
            End If

        End Function
    End Class

End Namespace
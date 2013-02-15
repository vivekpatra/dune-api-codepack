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
Imports SL.DuneApiCodePack.Networking

Namespace IPControl

    Public Class LaunchFlashLiteCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(3, 0)
        Private _mediaUrl As String
        Private _flashvars As HttpQuery

        Public Sub New(mediaUrl As String)
            MyBase.New(CommandValue.LaunchMediaUrl)
            _mediaUrl = mediaUrl
            _flashvars = New HttpQuery
        End Sub

        Public Sub New(mediaUrl As String, flashvars As IDictionary(Of String, String))
            Me.New(mediaUrl)
            _flashvars = New HttpQuery(flashvars)
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets the media URL for the requested Flash Lite application.
        ''' </summary>
        Public Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
            Set(value As String)
                _mediaUrl = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets the collection of Flash Lite startup parameters.
        ''' </summary>
        Public Property Flashvars As IDictionary(Of String, String)
            Get
                Return _flashvars
            End Get
            Set(value As IDictionary(Of String, String))
                _flashvars = New HttpQuery(value)
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        Private Function GetMediaUrl() As String
            Dim builder As New Text.StringBuilder

            With builder
                .Append("swf://")
                .Append(Me.MediaUrl)
                If Me.Flashvars.Count > 0 Then
                    .Append(":::")
                    .Append(Me.Flashvars.ToString)
                End If
            End With

            Return builder.ToString
        End Function

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New StatusCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
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

    Public Class LaunchWebsiteCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(3, 0)
        Private settings As HttpQuery

        Private _website As Uri
        Private _fullscreen As Boolean?
        Private _webAppKeys As Boolean?
        Private _zoomLevel As Integer?
        Private _overscan As Boolean?
        Private _userAgent As String
        Private _backgroundColor As String
        Private _videoMode As OnScreenDisplaySize

        Public Sub New(website As Uri)
            MyBase.New(CommandValue.LaunchMediaUrl)
            Me.settings = New HttpQuery
            Me.Website = website
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        Public Property Website As Uri
            Get
                Return New Uri(Me.parameters.GetValueOrDefault(Input.MediaUrl, "http://www.dune-hd.com/"))
            End Get
            Set(value As Uri)
                If value IsNot Nothing Then
                    Me.parameters.Item(Input.MediaUrl) = value.ToString
                Else
                    Me.parameters.Remove(Input.MediaUrl)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show browser controls, such as the address bar.
        ''' </summary>
        Public Property Fullscreen As Boolean?
            Get
                Return _fullscreen
            End Get
            Set(value As Boolean?)
                _fullscreen = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether key or button presses should be handles by javascript code or by the browser.
        ''' </summary>
        Public Property WebAppKeys As Boolean?
            Get
                Return _webAppKeys
            End Get
            Set(value As Boolean?)
                _webAppKeys = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the zoom level.
        ''' </summary>
        Public Property ZoomLevel As Integer?
            Get
                Return _zoomLevel
            End Get
            Set(value As Integer?)
                _zoomLevel = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to allow overscan.
        ''' </summary>
        Public Property Overscan As Boolean?
            Get
                Return _overscan
            End Get
            Set(value As Boolean?)
                _overscan = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the browser's user-agent.
        ''' </summary>
        Public Property UserAgent As String
            Get
                Return _userAgent
            End Get
            Set(value As String)
                _userAgent = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the browser's background color.
        ''' </summary>
        Public Property BackgroundColor As String
            Get
                Return _backgroundColor
            End Get
            Set(value As String)
                _backgroundColor = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the browser's video mode.
        ''' </summary>
        Public Property VideoMode As OnScreenDisplaySize
            Get
                Return _videoMode
            End Get
            Set(value As OnScreenDisplaySize)
                _videoMode = value
                Me.parameters.Item(Input.MediaUrl) = GetMediaUrl()
            End Set
        End Property

        Private Function GetBrowserSettings() As IDictionary(Of String, String)
            Dim parameters As New HttpQuery

            If Me.Fullscreen.HasValue Then
                parameters.Add(Input.Fullscreen, Fullscreen.Value.ToNumberString)
            End If
            If Me.WebAppKeys.HasValue Then
                parameters.Add(Input.WebappKeys, WebAppKeys.Value.ToNumberString)
            End If
            If Me.ZoomLevel.HasValue Then
                parameters.Add(Input.ZoomLevel, ZoomLevel.Value.ToString)
            End If
            If Me.Overscan.HasValue Then
                parameters.Add(Input.Overscan, Overscan.Value.ToNumberString)
            End If
            If Me.UserAgent.IsNotNullOrWhiteSpace Then
                parameters.Add(Input.UserAgent, UserAgent)
            End If
            If Me.BackgroundColor.IsNotNullOrWhiteSpace Then
                parameters.Add(Input.BackgroundColor, BackgroundColor)
            End If
            If Me.VideoMode IsNot Nothing Then
                parameters.Add(Input.OnScreenDisplaySize, VideoMode.Value)
            End If

            Return parameters
        End Function

        Protected Overrides Function GetQuery(target As Dune) As IDictionary(Of String, String)
            Dim query = DirectCast(MyBase.GetQuery(target), HttpQuery)
            query.Item(Input.MediaUrl) = GetMediaUrl()

            Return query
        End Function

        Private Function GetMediaUrl() As String
            Dim builder As New Text.StringBuilder
            Dim settings = GetBrowserSettings()

            With builder
                .Append("www://")
                .Append(Me.Website.AbsoluteUri)
                If settings.Count > 0 Then
                    .Append(":::")
                    .Append(settings.ToString())
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
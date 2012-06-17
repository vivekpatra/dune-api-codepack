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

    Public Class LaunchWebsiteCommand
        Inherits Command

        Private _website As Uri
        Private _fullscreen As Boolean?
        Private _webAppKeys As Boolean?
        Private _zoomLevel As Short?
        Private _overscan As Boolean?
        Private _userAgent As String
        Private _backgroundColor As String


        Public Sub New(target As Dune, website As Uri)
            MyBase.New(target)
            _website = website
        End Sub

        ''' <summary>
        ''' Gets or sets whether to show browser controls, such as the address bar.
        ''' </summary>
        Public Property Fullscreen As Boolean?
            Get
                Return _fullscreen
            End Get
            Set(value As Boolean?)
                _fullscreen = value
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
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the zoom level.
        ''' </summary>
        Public Property ZoomLevel As Short?
            Get
                Return _zoomLevel
            End Get
            Set(value As Short?)
                _zoomLevel = value
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
            End Set
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.LaunchMediaUrl)
            query.Add(Constants.StartPlaybackParameterNames.MediaUrl, "www://" + _website.AbsoluteUri)

            Return query
        End Function

        Protected Overrides Function GetQueryString() As String
            If Fullscreen.HasValue Or WebAppKeys.HasValue Or ZoomLevel.HasValue Or Overscan.HasValue Or UserAgent.IsNotNullOrWhiteSpace Or BackgroundColor.IsNotNullOrWhiteSpace Then
                Dim parameters As New HttpQuery

                If Fullscreen.HasValue Then
                    parameters.Add(Constants.WebbrowserParameterNames.Fullscreen, Fullscreen.Value.ToNumberString)
                End If
                If WebAppKeys.HasValue Then
                    parameters.Add(Constants.WebbrowserParameterNames.WebappKeys, WebAppKeys.Value.ToNumberString)
                End If
                If ZoomLevel.HasValue Then
                    parameters.Add(Constants.WebbrowserParameterNames.ZoomLevel, ZoomLevel.Value.ToString)
                End If
                If Overscan.HasValue Then
                    parameters.Add(Constants.WebbrowserParameterNames.Overscan, Overscan.Value.ToNumberString)
                End If
                If UserAgent.IsNotNullOrWhiteSpace Then
                    parameters.Add(Constants.WebbrowserParameterNames.UserAgent, UserAgent)
                End If
                If BackgroundColor.IsNotNullOrWhiteSpace Then
                    parameters.Add(Constants.WebbrowserParameterNames.BackgroundColor, BackgroundColor)
                End If

                Dim query As New Text.StringBuilder
                query.Append(MyBase.GetQueryString())
                query.Append(":::")
                query.Append(parameters.ToString)
                Return query.ToString
            Else
                Return MyBase.GetQueryString
            End If

        End Function
    End Class

End Namespace
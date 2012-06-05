Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    Public Class LaunchWebsiteCommand
        Inherits Command

        Private _website As Uri
        Private _fullscreen As Boolean?
        Private _webAppKeys As Boolean?
        Private _zoomLevel As UShort?
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
        Public Property ZoomLevel As UShort?
            Get
                Return _zoomLevel
            End Get
            Set(value As UShort?)
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

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.LaunchMediaUrl)
            query.Add(Constants.StartPlaybackParameters.MediaUrl, "www://" + _website.AbsoluteUri)

            Return query
        End Function

        Protected Overrides Function GetQueryString() As String
            If Fullscreen.HasValue Or WebAppKeys.HasValue Or ZoomLevel.HasValue Or Overscan.HasValue Or UserAgent.IsNotNullOrWhiteSpace Or BackgroundColor.IsNotNullOrWhiteSpace Then
                Dim parameters As NameValueCollection = Web.HttpUtility.ParseQueryString(String.Empty)

                If Fullscreen.HasValue Then
                    parameters.Add(Constants.WebbrowserParameters.Fullscreen, Fullscreen.Value.ToNumberString)
                End If
                If WebAppKeys.HasValue Then
                    parameters.Add(Constants.WebbrowserParameters.WebappKeys, WebAppKeys.Value.ToNumberString)
                End If
                If ZoomLevel.HasValue Then
                    parameters.Add(Constants.WebbrowserParameters.ZoomLevel, ZoomLevel.Value.ToString)
                End If
                If Overscan.HasValue Then
                    parameters.Add(Constants.WebbrowserParameters.Overscan, Overscan.Value.ToNumberString)
                End If
                If UserAgent.IsNotNullOrWhiteSpace Then
                    parameters.Add(Constants.WebbrowserParameters.UserAgent, UserAgent)
                End If
                If BackgroundColor.IsNotNullOrWhiteSpace Then
                    parameters.Add(Constants.WebbrowserParameters.BackgroundColor, BackgroundColor)
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
Imports System.Text
Imports System.Collections.Specialized

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to send a new playback request.</summary>
    Public Class StartPlaybackCommand
        Inherits DuneCommand

        Private _type As PlaybackType
        Private _mediaUrl As String
        Private _paused As Boolean
        Private _position As TimeSpan
        Private _blackScreen As Boolean
        Private _hideOnScreenDisplay As Boolean
        Private _repeat As Boolean

        ''' <param name="dune">The target device.</param>
        ''' <param name="mediaUrl">The media URL.</param>
        ''' <remarks>Does not support playlists.</remarks>
        Public Sub New(ByRef dune As Dune, ByVal mediaUrl As String)
            MyBase.New(dune)
            Type = PlaybackType.File
            _mediaUrl = UrlConverter.FormatUrl(dune, mediaUrl)
        End Sub

        ''' <summary>
        ''' Gets or sets whether to start the playback paused.
        ''' </summary>
        Public Property Paused As Boolean
            Get
                Return _paused
            End Get
            Set(value As Boolean)
                _paused = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position to start at.
        ''' </summary>
        Public Property Position As TimeSpan
            Get
                Return _position
            End Get
            Set(value As TimeSpan)
                _position = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback type.
        ''' </summary>
        Public Property Type As PlaybackType
            Get
                Return _type
            End Get
            Set(value As PlaybackType)
                _type = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the requested media URL.
        ''' </summary>
        Public ReadOnly Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether to show a black screen.
        ''' </summary>
        Public Property BlackScreen As Boolean
            Get
                Return _blackScreen
            End Get
            Set(value As Boolean)
                _blackScreen = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to hide the OSD.
        ''' </summary>
        Public Property HideOnScreenDisplay As Boolean
            Get
                Return _hideOnScreenDisplay
            End Get
            Set(value As Boolean)
                _hideOnScreenDisplay = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to repeat the playback.
        ''' </summary>
        Public Property Repeat As Boolean
            Get
                Return _repeat
            End Get
            Set(value As Boolean)
                _repeat = value
            End Set
        End Property

        ''' <summary>
        ''' Enumeration of supported playback types.
        ''' </summary>
        ''' <remarks>"File" includes everything that is not a DVD or blu-ray iso or folder, although playlist files are not supported.</remarks>
        Public Enum PlaybackType
            File = 0
            Dvd = 1
            Bluray = 2
        End Enum

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            Select Case Type
                Case PlaybackType.File
                    query.Add("cmd", Constants.Commands.StartFilePlayback)
                Case PlaybackType.Dvd
                    query.Add("cmd", Constants.Commands.StartDvdPlayback)
                Case PlaybackType.Bluray
                    query.Add("cmd", Constants.Commands.StartBlurayPlayback)
            End Select

            query.Add(Constants.StartPlaybackParameters.MediaLocation, MediaUrl)

            If Paused Then
                query.Add(Constants.StartPlaybackParameters.PlaybackSpeed, "0")
            End If

            If Position <> Nothing Then
                query.Add(Constants.StartPlaybackParameters.PlaybackPosition, Position.TotalSeconds.ToString)
            End If

            If BlackScreen Then
                query.Add(Constants.StartPlaybackParameters.BlackScreen, "1")
            End If

            If HideOnScreenDisplay Then
                query.Add(Constants.StartPlaybackParameters.HideOnScreenDisplay, "1")
            End If

            If Repeat Then
                query.Add(Constants.StartPlaybackParameters.ActionOnFinish, Constants.ActionOnFinishSettings.RestartPlayback)
            End If

            Return query
        End Function
    End Class

End Namespace
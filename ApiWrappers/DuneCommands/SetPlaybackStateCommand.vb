Imports System.Text
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to change various playback options.</summary>
    Public Class SetPlaybackStateCommand
        Inherits Command

        Private _playbackSpeed As Short?
        Private _playbackPosition As TimeSpan?
        Private _blackScreen As Boolean?
        Private _hideOnScreenDisplay As Boolean?
        Private _repeat As Boolean?
        Private _videoEnabled As Boolean?
        Private _playbackVolume As UShort?
        Private _playbackMute As Boolean?
        Private _audioTrack As UShort?
        Private _subtitleTrack As UShort?
        Private _videoZoom As String
        Private _videoOnTop As Boolean?


        Public Sub New(target As Dune)
            MyBase.New(target)
        End Sub

#Region "Properties"

#Region "Protocol version 2"

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        Public Property PlaybackSpeed As Constants.PlaybackSpeedSettings?
            Get
                If _playbackSpeed.HasValue Then
                    Return DirectCast(_playbackSpeed.Value, Constants.PlaybackSpeedSettings)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Constants.PlaybackSpeedSettings?)
                If value.HasValue Then
                    _playbackSpeed = CShort(value)
                Else
                    _playbackSpeed = Nothing
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        Public Property PlaybackPosition As TimeSpan?
            Get
                Return _playbackPosition
            End Get
            Set(value As TimeSpan?)
                _playbackPosition = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show a black screen.
        ''' </summary>
        Public Property BlackScreen As Boolean?
            Get
                Return _blackScreen
            End Get
            Set(value As Boolean?)
                _blackScreen = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to hide the OSD.
        ''' </summary>
        Public Property HideOnScreenDisplay As Boolean?
            Get
                Return _hideOnScreenDisplay
            End Get
            Set(value As Boolean?)
                _hideOnScreenDisplay = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to repeat the playback.
        ''' </summary>
        Public Property Repeat As Boolean?
            Get
                Return _repeat
            End Get
            Set(value As Boolean?)
                _repeat = value
            End Set
        End Property

#End Region ' Protocol version 1

#Region "Protocol version 2"

        ''' <summary>
        ''' Gets or sets whether to show video output.
        ''' </summary>
        Public Property VideoEnabled As Boolean?
            Get
                Return _videoEnabled
            End Get
            Set(value As Boolean?)
                _videoEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the volume.
        ''' </summary>
        ''' <value>Must be between 0 and 100. Values above 100 are automatically reduced to 100.</value>
        Public Property PlaybackVolume As UShort?
            Get
                Return _playbackVolume
            End Get
            Set(value As UShort?)
                _playbackVolume = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to mute the playback.
        ''' </summary>
        Public Property PlaybackMute As Boolean?
            Get
                Return _playbackMute
            End Get
            Set(value As Boolean?)
                _playbackMute = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the audio track number that is used in the current playback.
        ''' </summary>
        ''' <remarks>If a file contains multiple tracks (e.g. different languages, directors commentary), this property can be used to change the track.</remarks>
        Public Property AudioTrack As UShort?
            Get
                Return _audioTrack
            End Get
            Set(value As UShort?)
                _audioTrack = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the video zoom mode.
        ''' </summary>
        Public Property VideoZoom As String
            Get
                Return _videoZoom
            End Get
            Set(value As String)
                _videoZoom = value
            End Set
        End Property

#End Region ' Protocol version 2

#Region "Protocol version 3"

        ''' <summary>
        ''' Gets or sets whether to show video on top.
        ''' </summary>
        Public Property VideoOnTop As Boolean?
            Get
                Return _videoOnTop
            End Get
            Set(value As Boolean?)
                _videoOnTop = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the active subtitle track.
        ''' </summary>
        Public Property SubtitleTrack As UShort?
            Get
                Return _subtitleTrack
            End Get
            Set(value As UShort?)
                _subtitleTrack = value
            End Set
        End Property

#End Region

#End Region ' Properties

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.SetPlaybackState)

            If PlaybackSpeed.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackSpeed, PlaybackSpeed.ToString)
            End If

            If PlaybackPosition.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackPosition, PlaybackPosition.Value.TotalSeconds.ToString)
            End If

            If BlackScreen.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.BlackScreen, BlackScreen.Value.ToNumberString)
            End If

            If HideOnScreenDisplay.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.HideOnScreenDisplay, HideOnScreenDisplay.Value.ToNumberString)
            End If

            If Repeat.HasValue Then
                If Repeat.Value.IsTrue Then
                    query.Add(Constants.SetPlaybackStateParameters.ActionOnFinish, Constants.ActionOnFinishSettings.RestartPlayback)
                Else
                    query.Add(Constants.SetPlaybackStateParameters.ActionOnFinish, Constants.ActionOnFinishSettings.Exit)
                End If
            End If

            If VideoEnabled.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.VideoEnabled, VideoEnabled.Value.ToNumberString)
            End If

            If PlaybackVolume.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackVolume, PlaybackVolume.ToString)
            End If

            If PlaybackMute.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackMute, PlaybackMute.Value.ToNumberString)
            End If

            If AudioTrack.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.AudioTrack, AudioTrack.ToString)
            End If

            If VideoZoom.IsNotNullOrWhiteSpace Then
                query.Add(Constants.SetPlaybackStateParameters.VideoZoom, VideoZoom)
            End If

            If VideoOnTop.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.VideoOnTop, VideoOnTop.Value.ToNumberString)
            End If

            Return query
        End Function
    End Class

End Namespace
Imports System.Text
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to change various playback options.</summary>
    Public Class SetPlaybackStateCommand
        Inherits DuneCommand

        Private Const NotSupportedMessage As String = "This property requires a firmware update."

        Private _speed As Integer?
        Private _position As TimeSpan?
        Private _blackScreen As Boolean?
        Private _hideOnScreenDisplay As Boolean?
        Private _repeat As Boolean?
        Private _videoEnabled As Boolean?
        Private _volume As Byte?
        Private _mute As Boolean?
        Private _audioTrack As Byte?


        Public Sub New(ByRef dune As Dune)
            MyBase.New(dune)
        End Sub

#Region "Properties"

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        Public Property Speed As PlaybackSpeed?
            Get
                If _speed.HasValue Then
                    Return DirectCast(_speed.Value, PlaybackSpeed)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As PlaybackSpeed?)
                If value.HasValue Then
                    _speed = CInt(value)
                Else
                    _speed = Nothing
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        Public Property Position As TimeSpan?
            Get
                Return _position
            End Get
            Set(value As TimeSpan?)
                _position = value
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

        ''' <summary>
        ''' Gets or sets whether to show video output.
        ''' </summary>
        Public Property VideoEnabled As Boolean?
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoEnabled
            End Get
            Set(value As Boolean?)
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _videoEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the volume.
        ''' </summary>
        ''' <value>Must be between 0 and 100. Values above 100 are automatically reduced to 100.</value>
        Public Property Volume As Byte?
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _volume
            End Get
            Set(value As Byte?)
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _volume = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to mute the playback.
        ''' </summary>
        Public Property Mute As Boolean?
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _mute
            End Get
            Set(value As Boolean?)
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _mute = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the audio track number that is used in the current playback.
        ''' </summary>
        ''' <remarks>If a file contains multiple tracks (e.g. different languages, directors commentary), this property can be used to change the track.</remarks>
        Public Property AudioTrack As Byte?
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _audioTrack
            End Get
            Set(value As Byte?)
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _audioTrack = value
            End Set
        End Property

#End Region ' Properties

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.SetPlaybackState)

            If Speed.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackSpeed, CInt(Speed).ToString)
            End If

            If Position.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackPosition, Position.Value.TotalSeconds.ToString)
            End If

            If BlackScreen.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.BlackScreen, Math.Abs(CInt(BlackScreen)).ToString)
            End If

            If HideOnScreenDisplay.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.HideOnScreenDisplay, Math.Abs(CInt(HideOnScreenDisplay)).ToString)
            End If

            If Repeat.HasValue Then
                If Repeat = True Then
                    query.Add(Constants.SetPlaybackStateParameters.ActionOnFinish, Constants.ActionOnFinishSettings.RestartPlayback)
                Else
                    query.Add(Constants.SetPlaybackStateParameters.ActionOnFinish, Constants.ActionOnFinishSettings.Exit)
                End If
            End If

            If VideoEnabled.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.VideoEnabled, Math.Abs(CInt(VideoEnabled)).ToString)
            End If

            If Volume.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackVolume, Volume.ToString)
            End If

            If Mute.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.PlaybackMute, Math.Abs(CInt(Mute)).ToString)
            End If

            If AudioTrack.HasValue Then
                query.Add(Constants.SetPlaybackStateParameters.AudioTrack, AudioTrack.ToString)
            End If

            Return query
        End Function
    End Class

End Namespace
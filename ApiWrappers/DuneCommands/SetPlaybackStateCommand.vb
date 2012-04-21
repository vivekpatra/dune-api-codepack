Imports System.Text

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to change various playback options.</summary>
    Public Class SetPlaybackStateCommand
        Inherits DuneCommand

        Private Const NotSupportedMessage As String = "This property requires a firmware update."

        Private _speed As Nullable(Of Integer)
        Private _position As Nullable(Of TimeSpan)
        Private _blackScreen As Nullable(Of Boolean)
        Private _hideOnScreenDisplay As Nullable(Of Boolean)
        Private _repeat As Nullable(Of Boolean)
        Private _videoEnabled As Nullable(Of Boolean)
        Private _volume As Nullable(Of Byte)
        Private _mute As Nullable(Of Boolean)
        Private _audioTrack As Nullable(Of Byte)


        Public Sub New(ByRef dune As Dune)
            MyBase.New(dune)
            CommandType = Constants.Commands.SetPlaybackState
        End Sub

#Region "Properties"

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        Public Property Speed As Nullable(Of PlaybackSpeed)
            Get
                Return _speed
            End Get
            Set(value As Nullable(Of PlaybackSpeed))
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
        Public Property Position As Nullable(Of TimeSpan)
            Get
                Return _position
            End Get
            Set(value As Nullable(Of TimeSpan))
                _position = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show a black screen.
        ''' </summary>
        Public Property BlackScreen As Nullable(Of Boolean)
            Get
                Return _blackScreen
            End Get
            Set(value As Nullable(Of Boolean))
                _blackScreen = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to hide the OSD.
        ''' </summary>
        Public Property HideOnScreenDisplay As Nullable(Of Boolean)
            Get
                Return _hideOnScreenDisplay
            End Get
            Set(value As Nullable(Of Boolean))
                _hideOnScreenDisplay = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to repeat the playback.
        ''' </summary>
        Public Property Repeat As Nullable(Of Boolean)
            Get
                Return _repeat
            End Get
            Set(value As Nullable(Of Boolean))
                _repeat = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show video output.
        ''' </summary>
        Public Property VideoEnabled As Nullable(Of Boolean)
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoEnabled
            End Get
            Set(value As Nullable(Of Boolean))
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
        Public Property Volume As Nullable(Of Byte)
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _volume
            End Get
            Set(value As Nullable(Of Byte))
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _volume = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to mute the playback.
        ''' </summary>
        Public Property Mute As Nullable(Of Boolean)
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _mute
            End Get
            Set(value As Nullable(Of Boolean))
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
        Public Property AudioTrack As Nullable(Of Byte)
            Get
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _audioTrack
            End Get
            Set(value As Nullable(Of Byte))
                If Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _audioTrack = value
            End Set
        End Property

#End Region ' Properties

        Public Overrides Function GetQueryString() As String
            Dim query As New StringBuilder

            query.Append("cmd=")
            query.Append(CommandType)

            If Speed.HasValue Then
                query.Append("&speed=")
                query.Append(CInt(Speed).ToString)
            End If

            If Position.HasValue Then
                query.Append("&position=")
                query.Append(Position.Value.TotalSeconds.ToString)
            End If

            If BlackScreen.HasValue Then
                query.Append("&black_screen=")
                query.Append(Math.Abs(CInt(BlackScreen)).ToString)
            End If

            If HideOnScreenDisplay.HasValue Then
                query.Append("&hide_osd=")
                query.Append(Math.Abs(CInt(HideOnScreenDisplay)).ToString)
            End If

            If Repeat.HasValue Then
                query.Append("&action_on_finish=")
                If Repeat = True Then
                    query.Append(Constants.ActionOnFinishSettings.RestartPlayback)
                Else
                    query.Append(Constants.ActionOnFinishSettings.Exit)
                End If
            End If

            If VideoEnabled.HasValue Then
                query.Append("&video_enabled=")
                query.Append(Math.Abs(CInt(VideoEnabled)).ToString)
            End If

            If Volume.HasValue Then
                query.Append("&volume=")
                query.Append(Volume.ToString)
            End If

            If Mute.HasValue Then
                query.Append("&mute=")
                query.Append(Math.Abs(CInt(Mute)).ToString)
            End If

            If AudioTrack.HasValue Then
                query.Append("&audio_track=")
                query.Append(AudioTrack.ToString)
            End If

            If Timeout.HasValue AndAlso Timeout <> 20 Then
                query.Append("&timeout=")
                query.Append(Timeout.ToString)
            End If

            Return query.ToString
        End Function
    End Class

End Namespace
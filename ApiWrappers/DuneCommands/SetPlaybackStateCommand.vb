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
        End Sub

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
                If Dune.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoEnabled
            End Get
            Set(value As Nullable(Of Boolean))
                If Dune.ProtocolVersion < 2 Then
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
                If Dune.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _volume
            End Get
            Set(value As Nullable(Of Byte))
                If Dune.ProtocolVersion < 2 Then
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
                If Dune.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _mute
            End Get
            Set(value As Nullable(Of Boolean))
                If Dune.ProtocolVersion < 2 Then
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
                If Dune.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _audioTrack
            End Get
            Set(value As Nullable(Of Byte))
                If Dune.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                _audioTrack = value
            End Set
        End Property

        Public Overrides Function ToUri() As Uri
            Dim commandBuilder As New StringBuilder

            commandBuilder.Append("cmd=set_playback_state")

            If Speed.HasValue Then
                commandBuilder.Append("&speed=" + CInt(Speed).ToString)
            End If

            If Position.HasValue Then
                commandBuilder.Append("&position=" + Position.Value.TotalSeconds.ToString)
            End If

            If BlackScreen.HasValue Then
                commandBuilder.Append("&black_screen=" + Math.Abs(CInt(BlackScreen)).ToString)
            End If

            If HideOnScreenDisplay.HasValue Then
                commandBuilder.Append("&hide_osd=" + Math.Abs(CInt(HideOnScreenDisplay)).ToString)
            End If

            If Repeat.HasValue AndAlso Repeat = True Then
                commandBuilder.Append("&action_on_finish=restart_playback")
            ElseIf Repeat.HasValue AndAlso Repeat = False Then
                commandBuilder.Append("&action_on_finish=exit")
            End If

            If VideoEnabled.HasValue Then
                commandBuilder.Append("&video_enabled=" + Math.Abs(CInt(VideoEnabled)).ToString)
            End If

            If Volume.HasValue Then
                commandBuilder.Append("&volume=" + Volume.ToString)
            End If

            If Mute.HasValue Then
                commandBuilder.Append("&mute=" + Math.Abs(CInt(Mute)).ToString)
            End If

            If AudioTrack.HasValue Then
                commandBuilder.Append("&audio_track=" + AudioTrack.ToString)
            End If

            If Timeout.HasValue AndAlso Timeout <> 20 Then
                commandBuilder.AppendFormat("&timeout={0}", Timeout.ToString)
            End If

            Dim query As String = commandBuilder.ToString

            Return New Uri(BaseUri.ToString + query)
        End Function
    End Class

End Namespace
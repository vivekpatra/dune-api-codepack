Friend Class Constants
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Contains constants for each possible parameter name in the command results.
    ''' </summary>
    Friend Class CommandResults
        Private Sub New()
        End Sub

        Public Const CommandStatus As String = "command_status"
        Public Const ProtocolVersion As String = "protocol_version"
        Public Const PlayerState As String = "player_state"
        Public Const PlaybackSpeed As String = "playback_speed"
        Public Const PlaybackDuration As String = "playback_duration"
        Public Const PlaybackPosition As String = "playback_position"
        Public Const PlaybackIsBuffering As String = "playback_is_buffering"
        Public Const PlaybackVolume As String = "playback_volume"
        Public Const PlaybackMute As String = "playback_mute"
        Public Const AudioTrack As String = "audio_track"
        Public Const VideoFullscreen As String = "video_fullscreen"
        Public Const VideoX As String = "video_x"
        Public Const VideoY As String = "video_y"
        Public Const VideoWidth As String = "video_width"
        Public Const VideoHeight As String = "video_height"
        Public Const VideoTotalDisplayWidth As String = "video_total_display_width"
        Public Const VideoTotalDisplayHeight As String = "video_total_display_height"
        Public Const VideoEnabled As String = "video_enabled"
        Public Const VideoZoom As String = "video_zoom"
        Public Const ErrorKind As String = "error_kind"
        Public Const ErrorDescription As String = "error_description"
        Public Const PlaybackDvdMenu As String = "playback_dvd_menu"
    End Class

    ''' <summary>
    ''' Contains constants for each possible command status.
    ''' </summary>
    Friend Class Status
        Private Sub New()
        End Sub

        Public Const Ok As String = "ok"
        Public Const Timeout As String = "timeout"
        Public Const Failed As String = "failed"
    End Class


    ''' <summary>
    ''' Contains constants for each possible command (as defined by the API).
    ''' </summary>
    Friend Class Commands
        Private Sub New()
        End Sub

        Public Const Status As String = "status"
        Public Const StartFilePlayback As String = "start_file_playback"
        Public Const StartDvdPlayback As String = "start_dvd_playback"
        Public Const StartBlurayPlayback As String = "start_bluray_playback"
        Public Const SetPlaybackState As String = "set_playback_state"
        Public Const InfraredCode As String = "ir_code"
        Public Const MainScreen As String = "main_screen"
        Public Const BlackScreen As String = "black_screen"
        Public Const Standby As String = "standby"
        Public Const DvdNavigation As String = "dvd_navigation"

        ' Protocol version 3
        Public Const StartPlaylistPlayback As String = "start_playlist_playback"
        Public Const LaunchMediaUrl As String = "launch_media_url"

    End Class

    ''' <summary>
    ''' Contains constants for each possible player state.
    ''' </summary>
    Friend Class PlayerStateSettings
        Private Sub New()
        End Sub

        Public Const FilePlayback As String = "file_playback"
        Public Const DvdPlayback As String = "dvd_playback"
        Public Const BlurayPlayback As String = "bluray_playback"
        Public Const Standby As String = "standby"
        Public Const navigator As String = "navigator"
        Public Const Loading As String = "loading"

    End Class

    ''' <summary>
    ''' Contains constants for each possible product ID.
    ''' </summary>
    Friend Class ProductIDs
        Private Sub New()
        End Sub

        Public Const HDTV301 As String = "hdtv_301"
        Public Const HDTV101 As String = "hdtv_101"
        Public Const HDLite53D As String = "hdlite_53d"
        Public Const HDDuo As String = "hdduo"
        Public Const HDMax As String = "hdmax"
        Public Const HDSmartB1 As String = "hdsmart_b1"
        Public Const HDSmartD1 As String = "hdsmart_d1"
        Public Const HDSmartH1 As String = "hdsmart_h1"
        Public Const HDBase3 As String = "hdbase3"
        Public Const BDPrime3 As String = "bdprime3"

        ''' <summary>
        ''' Discontinued models don't have the API installed but are listed for consistency.
        ''' </summary>
        ''' <remarks>These models don't have update checkers.</remarks>
        Friend Class DiscontinuedModels
            Private Sub New()
            End Sub

            ' Discontinued models (no API support)
            Public Const HDBase2 As String = "hdbase2"
            Public Const HDBase As String = "hdbase"
            Public Const HDCenter As String = "hdcenter"
            Public Const BDPrime As String = "bdprime"
            Public Const HDMini As String = "hdmini"
            Public Const HDUltra As String = "hdultra"
        End Class

    End Class

    ''' <summary>
    ''' Contains constants for each possible playback speed setting.
    ''' </summary>
    ''' <remarks>
    ''' The "start_x_playback" commands only accept 0 and 256.
    ''' "set_playback_state" commands accept all values.
    ''' </remarks>
    Friend Class PlaybackSpeedSettings
        Private Sub New()
        End Sub

        Public Const Rewind16x As Short = -4096
        Public Const Rewind8x As Short = -2048
        Public Const Rewind4x As Short = -1024
        Public Const Rewind2x As Short = -512
        Public Const Rewind As Short = -256
        Public Const RewindSlow As Short = -64
        Public Const Pause As Short = 0
        Public Const Slow As Short = 64
        Public Const Normal As Short = 256
        Public Const Forward2x As Short = 512
        Public Const Forward4x As Short = 1024
        Public Const Forward8x As Short = 2048
        Public Const Forward16x As Short = 4096

        Public Shared _names As Dictionary(Of Short, String)
        Public Shared ReadOnly Property Names As Dictionary(Of Short, String)
            Get
                If _names Is Nothing Then
                    _names = New Dictionary(Of Short, String)
                    With _names
                        .Add(Rewind16x, "Rewind (16x)")
                        .Add(Rewind8x, "Rewind (8x)")
                        .Add(Rewind4x, "Rewind (4x)")
                        .Add(Rewind2x, "Rewind (2x)")
                        .Add(Rewind, "Rewind")
                        .Add(RewindSlow, "Rewind (slow)")
                        .Add(Pause, "Paused")
                        .Add(Normal, "Normal")
                        .Add(Forward2x, "Forward")
                        .Add(Forward4x, "Forward (4x)")
                        .Add(Forward8x, "Forward (8x)")
                        .Add(Forward16x, "Forward (16x)")
                    End With

                End If
                Return _names
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Contains constants for each possible zoom setting.
    ''' </summary>
    ''' <remarks>
    ''' Custom zoom settings cannot be set through the "video_zoom" parameter.
    ''' You must set video height and size parameters instead.
    ''' </remarks>
    Friend Class VideoZoomSettings
        Private Sub New()
        End Sub


        Public Const Normal As String = "normal"
        Public Const Enlarge As String = "enlarge"
        Public Const MakeWider As String = "make_wider"
        Public Const FillScreen As String = "fill_screen"
        Public Const FillFullScreen As String = "full_fill_screen"
        Public Const MakeTaller As String = "make_taller"
        Public Const CutEdges As String = "cut_edges"
    End Class

    ''' <summary>
    ''' Contains constants for each possible action on finish.
    ''' </summary>
    Friend Class ActionOnFinishSettings
        Private Sub New()
        End Sub

        Public Const RestartPlayback As String = "restart_playback"
        Public Const [Exit] As String = "exit"
    End Class

    Friend Class DvdNavigationActions
        Private Sub New()
        End Sub

        Public Const Left As String = "left"
        Public Const Right As String = "right"
        Public Const Up As String = "up"
        Public Const Down As String = "down"
        Public Const Enter As String = "enter"
    End Class

End Class

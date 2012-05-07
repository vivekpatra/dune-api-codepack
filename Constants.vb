Friend Class Constants
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Contains constants for each possible command (as defined by the API).
    ''' </summary>
    Friend Class Commands
        Private Sub New()
        End Sub

        ''' <summary>Get the player status without affecting the player state.</summary>
        Public Const Status As String = "status"
        ''' <summary>Start playback of any supported filetype that is not a DVD or Blu-ray EXCEPT playlists.</summary>
        Public Const StartFilePlayback As String = "start_file_playback"
        ''' <summary>Start a DVD iso or folder.</summary>
        Public Const StartDvdPlayback As String = "start_dvd_playback"
        ''' <summary>Start a Blu-ray iso or folder.</summary>
        Public Const StartBlurayPlayback As String = "start_bluray_playback"
        ''' <summary>Set various playback options. Possible parameters are enumerated in <see cref="Constants.SetPlaybackStateParameters" />.</summary>
        Public Const SetPlaybackState As String = "set_playback_state"
        ''' <summary>Emulate a button on the remote control.</summary>
        Public Const InfraredCode As String = "ir_code"
        ''' <summary>Sets the player to the main screen.</summary>
        Public Const MainScreen As String = "main_screen"
        ''' <summary>Sets the player to a black screen.</summary>
        Public Const BlackScreen As String = "black_screen"
        ''' <summary>Sets the player to standby.</summary>
        Public Const Standby As String = "standby"
        ''' <summary>Sends a dvd navigation command. It takes only one parameter: "action". Possible values are enumerated in <see cref="DvdNavigationActions"/>.</summary>
        Public Const DvdNavigation As String = "dvd_navigation"

        ' Protocol version 3
        ''' <summary>???</summary>
        Public Const StartPlaylistPlayback As String = "start_playlist_playback"
        ''' <summary>???</summary>
        Public Const LaunchMediaUrl As String = "launch_media_url"

    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter that you can supply with a start playback command.
    ''' </summary>
    Friend Class StartPlaybackParameters
        Private Sub New()
        End Sub

        ''' <summary>Path to a valid media location (mandatory).</summary>
        Public Const MediaLocation As String = "media_url"
        ''' <summary>Playback speed in decimal form. Possible values are enumerated in <see cref="Constants.PlaybackSpeedSettings"/>.</summary>
        Public Const PlaybackSpeed As String = "speed"
        ''' <summary>Playback position in seconds (defaults to 0).</summary>
        ''' <remarks>NOTE: restricted to values 0 and 256 (paused and normal).</remarks>
        Public Const PlaybackPosition As String = "position" ' NOTE: restricted to values 0 and 256 (paused or normal)
        ''' <summary>1 to show a black screen; otherwise 0 (defaults to 0)</summary>
        Public Const BlackScreen As String = "black_screen"
        ''' <summary>1 to hide the on-screen display; otherwise 0 (defaults to 0)</summary>
        Public Const HideOnScreenDisplay As String = "hide_osd"
        ''' <summary>Action to take on playback finish. Possible values are enumerated in <see cref="Constants.ActionOnFinishSettings" />.</summary>
        Public Const ActionOnFinish As String = "action_on_finish"
    End Class

    ''' <summary>
    ''' Containts constants for each possible parameter that you can supply with a set playback state command.
    ''' </summary>
    Friend Class SetPlaybackStateParameters
        Private Sub New()
        End Sub

        ''' <summary>Playback speed in decimal form. Possible values are enumerated in <see cref="Constants.PlaybackSpeedSettings"/>.</summary>
        Public Const PlaybackSpeed As String = "speed"
        ''' <summary>Playback position in seconds.</summary>
        Public Const PlaybackPosition As String = "position"
        ''' <summary>1 to show a black screen; otherwise 0</summary>
        Public Const BlackScreen As String = "black_screen"
        ''' <summary>1 to hide the on-screen display; otherwise 0</summary>
        Public Const HideOnScreenDisplay As String = "hide_osd"
        ''' <summary>Action to take on playback finish. Possible values are enumerated in <see cref="Constants.ActionOnFinishSettings" />.</summary>
        Public Const ActionOnFinish As String = "action_on_finish"

        Public Const SetKeyframe As String = "skip_frames"
        Public Const PlaybackVolume As String = "volume"
        Public Const PlaybackMute As String = "mute"
        Public Const AudioTrack As String = "audio_track"
        Public Const VideoEnabled As String = "video_enabled"
        Public Const VideoZoom As String = "video_zoom"
        Public Const VideoFullscreen As String = "video_fullscreen"
        Public Const VideoHorizontalPosition As String = "video_x"
        Public Const VideoVerticalPosition As String = "video_y"
        Public Const VideoWidth As String = "video_width"
        Public Const VideoHeight As String = "video_height"

    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter name in the command results.
    ''' </summary>
    Friend Class CommandResults
        Private Sub New()
        End Sub

        ''' <summary>Indicates the command status. Possible values are enumerated in <see cref="Constants.Status" />.</summary>
        Public Const CommandStatus As String = "command_status"
        ''' <summary>The protocol version used by the firmware.</summary>
        Public Const ProtocolVersion As String = "protocol_version"
        ''' <summary>Indicates the player state. Possible values are enumerated in <see cref=" Constants.PlayerStateSettings" />.</summary>
        ''' <remarks>The API contains a bug that displays this property as "file_playback" when the player is actually in "navigator" mode when playback has ended.</remarks>
        Public Const PlayerState As String = "player_state"
        ''' <summary>The playback speed in its decimal representation.</summary>
        Public Const PlaybackSpeed As String = "playback_speed"
        ''' <summary>The playback duration in seconds.</summary>
        Public Const PlaybackDuration As String = "playback_duration"
        ''' <summary>The playback position in seconds.</summary>
        Public Const PlaybackPosition As String = "playback_position"
        ''' <summary>1 if the player is buffering; otherwise 0.</summary>
        Public Const PlaybackIsBuffering As String = "playback_is_buffering"
        ''' <summary>The playback volume percentage (0 to 150).</summary>
        Public Const PlaybackVolume As String = "playback_volume"
        ''' <summary>1 if the playback is muted; otherwise 0.</summary>
        Public Const PlaybackMute As String = "playback_mute"
        ''' <summary>The audio track (read: language track) number that is playing.</summary>
        Public Const AudioTrack As String = "audio_track"
        ''' <summary>1 if the playback is fullscreen; otherwise 0.</summary>
        Public Const VideoFullscreen As String = "video_fullscreen"
        ''' <summary>The horizontal position of the video output (from left to right) in pixels.</summary>
        Public Const VideoX As String = "video_x"
        ''' <summary>The vertical position of the video output (from top to bottom) in pixels</summary>
        Public Const VideoY As String = "video_y"
        ''' <summary>The width of the video output.</summary>
        Public Const VideoWidth As String = "video_width"
        ''' <summary>The height of the video output.</summary>
        Public Const VideoHeight As String = "video_height"
        ''' <summary>The total display width of the video output.</summary>
        Public Const VideoTotalDisplayWidth As String = "video_total_display_width"
        ''' <summary>The total display height of the video output.</summary>
        Public Const VideoTotalDisplayHeight As String = "video_total_display_height"
        ''' <summary>1 if the video output is enabled; otherwise 0.</summary>
        Public Const VideoEnabled As String = "video_enabled"
        ''' <summary>Indicates the zoom mode of the video output. Possible values are enumerated in <see cref="Constants.VideoZoomSettings" />. Custom zoom settings are marked as "other".</summary>
        Public Const VideoZoom As String = "video_zoom"
        ''' <summary>Indicates the error kind. Possible values are enumerated in <see cref="Constants.ErrorKinds" />.</summary>
        Public Const ErrorKind As String = "error_kind"
        ''' <summary>Describes the command error.</summary>
        Public Const ErrorDescription As String = "error_description"
        ''' <summary>1 if a DVD menu is currently shown; otherwise 0.</summary>
        Public Const PlaybackDvdMenu As String = "playback_dvd_menu"
    End Class

    ''' <summary>
    ''' Contains constants for each possible command status.
    ''' </summary>
    Friend Class Status
        Private Sub New()
        End Sub
        ''' <summary>The command completed without error.</summary>
        Public Const Ok As String = "ok"
        ''' <summary>The command timed out, but the device continues processing the request.</summary>
        Public Const Timeout As String = "timeout"
        ''' <summary>The command returned an error. See the <see cref="Constants.CommandResults.ErrorKind"/> and <see cref="Constants.CommandResults.ErrorDescription" /> parameters in the command results for more information.</summary>
        Public Const Failed As String = "failed"
    End Class

    ''' <summary>
    ''' Contains constants for each possible error kind.
    ''' </summary>
    Friend Class ErrorKinds
        Private Sub New()
        End Sub

        ''' <summary>The command is not allowed in the current player state.
        ''' <example>For example: trying to set "volume" while the player state is "navigator" is not allowed.</example>
        ''' </summary>
        Public Const IllegalState As String = "illegal_state"
        ''' <summary>The player failed to process the request.</summary>
        Public Const OperationFailed As String = "operation_failed"
        ''' <summary>The requested command is unknown. Most likely occurs because the player firmware is outdated if you stick to using values enumerated in <see cref="Constants.Commands"/>.</summary>
        Public Const UnknownCommand As String = "unknown_command"
        ''' <summary>The requested command contains an invalid combination of parameters.</summary>
        Public Const InvalidParameters As String = "invalid_parameters"
        ''' <summary>The device experienced an internal error. There's not much you can do and it should be a one-time thing.</summary>
        Public Const InternalError As String = "internal_error"
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
        Public Const SafeMode As String = "safe_mode"
        Public Const PhotoViewer As String = "photo_viewer"
        Public Const Upgrade As String = "upgrade"
        Public Const BlackScreen As String = "black_screen"
        Public Const TorrentDownloads As String = "torrent_downloads"
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
        Public Const Normal As Short = &H100
        Public Const Forward2x As Short = 512
        Public Const Forward4x As Short = 1024
        Public Const Forward8x As Short = 2048
        Public Const Forward16x As Short = 4096
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

    ''' <summary>
    ''' Contains constants for each possible DVD menu action.
    ''' </summary>
    ''' <remarks>
    ''' These are pretty useless IMHO since you can emulate remote control buttons.
    ''' Checking for a dvd menu would be extra work.
    ''' </remarks>
    Friend Class DvdNavigationActions
        Private Sub New()
        End Sub

        Public Const Left As String = "left"
        Public Const Right As String = "right"
        Public Const Up As String = "up"
        Public Const Down As String = "down"
        Public Const Enter As String = "enter"
    End Class

    ''' <summary>
    ''' Contains constants for each possible interface language.
    ''' It might be a good idea to localize your app in each of these.
    ''' </summary>
    Friend Class InterfaceLanguages
        Private Sub New()
        End Sub

        Public Const English As String = "english"
        Public Const French As String = "french"
        Public Const German As String = "german"
        Public Const Dutch As String = "dutch"
        Public Const Spanish As String = "spanish"
        Public Const Italian As String = "italian"
        Public Const Russian As String = "russian"
        Public Const Ukrainian As String = "ukrainian"
        Public Const Romanian As String = "romanian"
        Public Const Hungarian As String = "hungarian"
        Public Const Polish As String = "polish"
        Public Const Greek As String = "greek"
        Public Const Danish As String = "danish"
        Public Const Czech As String = "czech"
        Public Const Swedish As String = "swedish"
        Public Const Estonian As String = "estonian"
        Public Const Turkish As String = "turkish"
        Public Const Hebrew As String = "hebrew"
        Public Const ChineseSimplified As String = "chinese_simplified"
        Public Const ChineseTraditional As String = "chinese_traditional"
        Public Const Japanese As String = "japanese"
    End Class

End Class

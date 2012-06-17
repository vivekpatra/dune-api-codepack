Public NotInheritable Class Constants
    Private Sub New()
    End Sub

    ''' <summary>For internal use when an IFormatProvider is required.</summary>
    Friend Shared ReadOnly FormatProvider As IFormatProvider = Globalization.CultureInfo.InvariantCulture

    ''' <summary>
    ''' Contains constants for each possible command parameter value.
    ''' </summary>
    Public NotInheritable Class CommandValues
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
        ''' <summary>Set various playback options. Possible parameters are enumerated in <see cref="Constants.SetPlaybackStateParameterNames" />.</summary>
        Public Const SetPlaybackState As String = "set_playback_state"
        ''' <summary>Emulate a button on the remote control.</summary>
        Public Const InfraredCode As String = "ir_code"
        ''' <summary>Sets the player to the main screen.</summary>
        Public Const MainScreen As String = "main_screen"
        ''' <summary>Sets the player to a black screen.</summary>
        Public Const BlackScreen As String = "black_screen"
        ''' <summary>Sets the player to standby.</summary>
        Public Const Standby As String = "standby"
        ''' <summary>Sends a dvd navigation command. It takes only one parameter: "action". Possible values are enumerated in <see cref="ActionValues"/>.</summary>
        Public Const DvdNavigation As String = "dvd_navigation"
        ''' <summary>Sends a bluray navigation command. It takes only one parameter: "action". Possible values are enumerated in <see cref="ActionValues"/>.</summary>
        Public Const BlurayNavigation As String = "bluray_navigation"
        ''' <summary>Start playback of a playlist file.</summary>
        ''' <remarks>Requires version 3.</remarks>
        Public Const StartPlaylistPlayback As String = "start_playlist_playback"
        ''' <summary>Detect media url type.</summary>
        ''' <remarks>Requires version 3.</remarks>
        Public Const LaunchMediaUrl As String = "launch_media_url"
        ''' <summary>Gets text from an input box (UTF-8).</summary>
        ''' <remarks>Requires version 3.</remarks>
        Public Const GetText As String = "get_text"
        ''' <summary>Sets text in an input box (UTF-8).</summary>
        ''' <remarks>Requires version 3.</remarks>
        Public Const SetText As String = "set_text"
    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter that you can specify in a 'start_x_playback' command.
    ''' </summary>
    Public NotInheritable Class StartPlaybackParameterNames
        Private Sub New()
        End Sub

        ''' <summary>Path to a valid media location (mandatory).</summary>
        Public Const MediaUrl As String = "media_url"
        ''' <summary>Playback speed in decimal form. Possible values are enumerated in <see cref="Constants.PlaybackSpeedValues"/>.</summary>
        Public Const PlaybackSpeed As String = "speed"
        ''' <summary>Playback position in seconds (defaults to 0).</summary>
        Public Const PlaybackPosition As String = "position"
        ''' <summary>1 to show a black screen; otherwise 0 (defaults to 0)</summary>
        Public Const BlackScreen As String = "black_screen"
        ''' <summary>1 to hide the on-screen display; otherwise 0 (defaults to 0)</summary>
        Public Const HideOnScreenDisplay As String = "hide_osd"
        ''' <summary>Action to take on playback finish. Possible values are enumerated in <see cref="Constants.ActionOnFinishValues" />.</summary>
        Public Const ActionOnFinish As String = "action_on_finish"
        ''' <summary>The zero-based index number of a media URL in a playlist file.</summary>
        Public Const StartIndex As String = "start_index"
    End Class

    ''' <summary>
    ''' Containts constants for each possible parameter that you can specify in a 'set_playback_state' command.
    ''' </summary>
    Public NotInheritable Class SetPlaybackStateParameterNames
        Private Sub New()
        End Sub

        ''' <summary>Playback speed in decimal form. Possible values are enumerated in <see cref="Constants.PlaybackSpeedValues"/>.</summary>
        Public Const PlaybackSpeed As String = "speed"
        ''' <summary>Playback position in seconds.</summary>
        Public Const PlaybackPosition As String = "position"
        ''' <summary>1 to show a black screen; otherwise 0</summary>
        Public Const BlackScreen As String = "black_screen"
        ''' <summary>1 to hide the on-screen display; otherwise 0</summary>
        Public Const HideOnScreenDisplay As String = "hide_osd"
        ''' <summary>Action to take on playback finish. Possible values are enumerated in <see cref="Constants.ActionOnFinishValues" />.</summary>
        Public Const ActionOnFinish As String = "action_on_finish"
        ''' <summary>1 to forward to the next keyframe; -1 to rewind to the previous keyframe. Only valid during DVD and MKV playback.</summary>
        Public Const SetKeyframe As String = "skip_frames"
        ''' <summary>Volume in percentage. Only integer values are allowed.</summary>
        Public Const PlaybackVolume As String = "volume"
        ''' <summary>1 to mute the volume; otherwise 0.</summary>
        Public Const PlaybackMute As String = "mute"
        ''' <summary>The 0-based index number of the current language track.</summary>
        Public Const AudioTrack As String = "audio_track"
        ''' <summary>0 to disable video output; otherwise 1.</summary>
        Public Const VideoEnabled As String = "video_enabled"
        ''' <summary>Name of the current zoom preset. Possible values are enumerated in <see cref="Constants.VideoZoomValues"/>.</summary>
        Public Const VideoZoom As String = "video_zoom"
        ''' <summary>0 to enable setting a custom playback window rectangle zoom; otherwise 1.</summary>
        Public Const VideoFullscreen As String = "video_fullscreen"
        ''' <summary>The playback window rectangle's horizontal offset.</summary>
        Public Const VideoHorizontalOffset As String = "video_x"
        ''' <summary>The playback window rectangle's vertical offset.</summary>
        Public Const VideoVerticalOffset As String = "video_y"
        ''' <summary>The playback window rectangle's width.</summary>
        Public Const VideoWidth As String = "video_width"
        ''' <summary>The playback window rectangle's height.</summary>
        Public Const VideoHeight As String = "video_height"
        ''' <summary>0 to enable setting a custom playback window rectangle zoom; otherwise 1.</summary>
        Public Const WindowFullscreen As String = "window_fullscreen"
        ''' <summary>The playback window rectangle's horizontal offset.</summary>
        Public Const WindowRectangleHorizontalOffset As String = "window_rect_x"
        ''' <summary>The playback window rectangle's vertical offset.</summary>
        Public Const WindowRectangleVerticalOffset As String = "window_rect_y"
        ''' <summary>The playback window rectangle's width.</summary>
        Public Const WindowRectangleWidth As String = "window_rect_width"
        ''' <summary>The playback window rectangle's height.</summary>
        Public Const WindowRectangleHeight As String = "window_rect_height"
        ''' <summary>The visible screen rectangle's horizontal offset.</summary>
        Public Const ClipRectangleHorizontalOffset As String = "clip_rect_x"
        ''' <summary>The visible screen rectangle's vertical offset.</summary>
        Public Const ClipRectangleVerticalOffset As String = "clip_rect_y"
        ''' <summary>The visible screen rectangle's width.</summary>
        Public Const ClipRectangleWidth As String = "clip_rect_width"
        ''' <summary>The visible screen rectangle's height.</summary>
        Public Const ClipRectangleHeight As String = "clip_rect_height"
        ''' <summary>1 to show video output on top of overlay graphics; otherwise 0.</summary>
        Public Const VideoOnTop As String = "video_on_top"
        ''' <summary>The 0-based index number of the current subtitles track.</summary>
        Public Const SubtitlesTrack As String = "subtitles_track"
    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter name in the command results.
    ''' </summary>
    Public NotInheritable Class CommandResultParameterNames
        Private Sub New()
        End Sub

        ''' <summary>Indicates the command status. Possible values are enumerated in <see cref="Constants.CommandStatusValues" />.</summary>
        Public Const CommandStatus As String = "command_status"
        ''' <summary>The protocol version used by the firmware.</summary>
        Public Const ProtocolVersion As String = "protocol_version"
        ''' <summary>Indicates the player state. Possible values are enumerated in <see cref=" Constants.PlayerStateValues" />.</summary>
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
        ''' <summary>Indicates the zoom mode of the video output. Possible values are enumerated in <see cref="Constants.VideoZoomValues" />. Custom zoom settings are marked as "other".</summary>
        Public Const VideoZoom As String = "video_zoom"
        ''' <summary>Indicates the error kind. Possible values are enumerated in <see cref="Constants.ErrorKindValues" />.</summary>
        Public Const ErrorKind As String = "error_kind"
        ''' <summary>Describes the command error.</summary>
        Public Const ErrorDescription As String = "error_description"
        ''' <summary>1 if a DVD menu is currently shown; otherwise 0.</summary>
        Public Const PlaybackDvdMenu As String = "playback_dvd_menu"
        ''' <summary>1 if a Blu-ray menu is currently shown; otherwise 0.</summary>
        Public Const PlaybackBlurayMenu As String = "playback_bluray_menu"
        ''' <summary>Regular expression that matches results that contain codec or language information about audio or subtitle tracks.</summary>
        Public Shared ReadOnly TrackRegex As Text.RegularExpressions.Regex = New Text.RegularExpressions.Regex("(audio|subtitles)_track\.[0-9]+\.(lang|codec)")

        ' New in protocol version 3
        ' TODO: add summaries
        Public Const PlaybackState As String = "playback_state"
        Public Const PreviousPlaybackState As String = "previous_playback_state"
        Public Const LastPlaybackEvent As String = "last_playback_event"
        Public Const PlaybackUrl As String = "playback_url"
        Public Const PlaybackVideoWidth As String = "playback_video_width"
        Public Const PlaybackVideoHeight As String = "playback_video_height"
        Public Const SubtitlesTrack As String = "subtitles_track"
        Public Const PlaybackWindowFullscreen As String = "playback_window_fullscreen"
        Public Const PlaybackWindowRectangleX As String = "playback_window_rect_x"
        Public Const PlaybackWindowRectangleY As String = "playback_window_rect_y"
        Public Const PlaybackWindowRectangleWidth As String = "playback_window_rect_width"
        Public Const PlaybackWindowRectangleHeight As String = "playback_window_rect_height"
        Public Const PlaybackClipRectangleX As String = "playback_clip_rect_x"
        Public Const PlaybackClipRectangleY As String = "playback_clip_rect_y"
        Public Const PlaybackClipRectangleWidth As String = "playback_clip_rect_width"
        Public Const PlaybackClipRectangleHeight As String = "playback_clip_rect_height"
        Public Const OnScreenDisplayWidth As String = "osd_width"
        Public Const OnScreenDisplayHeight As String = "osd_height"
        Public Const VideoOnTop As String = "video_on_top"
        Public Const Text As String = "text"
    End Class

    ''' <summary>
    ''' Contains constants for each possible 'command_status' value.
    ''' </summary>
    Public NotInheritable Class CommandStatusValues
        Private Sub New()
        End Sub
        ''' <summary>The command completed without error.</summary>
        Public Const Ok As String = "ok"
        ''' <summary>The command timed out, but the device continues processing the request.</summary>
        Public Const Timeout As String = "timeout"
        ''' <summary>The command returned an error. See the <see cref="Constants.CommandResultParameterNames.ErrorKind"/> and <see cref="Constants.CommandResultParameterNames.ErrorDescription" /> parameters in the command results for more information.</summary>
        Public Const Failed As String = "failed"
    End Class

    ''' <summary>
    ''' Contains constants for each possible 'error_kind'.
    ''' </summary>
    Public NotInheritable Class ErrorKindValues
        Private Sub New()
        End Sub

        ''' <summary>The command is not allowed in the current player state.
        ''' <example>For example: trying to set "volume" while the player state is "navigator" is not allowed.</example>
        ''' </summary>
        Public Const IllegalState As String = "illegal_state"
        ''' <summary>The player failed to process the request.</summary>
        Public Const OperationFailed As String = "operation_failed"
        ''' <summary>The requested command is unknown. Most likely occurs because the player firmware is outdated if you stick to using values enumerated in <see cref="Constants.CommandValues"/>.</summary>
        Public Const UnknownCommand As String = "unknown_command"
        ''' <summary>The requested command contains an invalid combination of parameters.</summary>
        Public Const InvalidParameters As String = "invalid_parameters"
        ''' <summary>The device experienced an internal error. There's not much you can do and it should be a one-time thing.</summary>
        Public Const InternalError As String = "internal_error"
    End Class

    ''' <summary>
    ''' Contains constants for each possible 'player_state' value.
    ''' </summary>
    Public NotInheritable Class PlayerStateValues
        Private Sub New()
        End Sub

        Public Const FilePlayback As String = "file_playback"
        Public Const DvdPlayback As String = "dvd_playback"
        Public Const BlurayPlayback As String = "bluray_playback"
        Public Const Standby As String = "standby"
        Public Const Navigator As String = "navigator"
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
    ''' <remarks>Does not include discontinued models which do not support the IP control API.</remarks>
    Public NotInheritable Class ProductIdentifierValues
        Private Sub New()
        End Sub

        ' Uncomment if and when the HD Pro is released.
        ' Public Const HDPro As String = "hdpro"

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
    End Class

    ''' <summary>
    ''' Contains constants for each possible 'playback_speed' value.
    ''' </summary>
    Public Enum PlaybackSpeedValues As Short
        ''' <summary>Rewind 32x</summary>
        Rewind8192 = -CShort(2 ^ 13)
        ''' <summary>Rewind 16x</summary>
        Rewind4096 = -CShort(2 ^ 12)
        ''' <summary>Rewind 8x</summary>
        Rewind2048 = -CShort(2 ^ 11)
        ''' <summary>Rewind 4x</summary>
        Rewind1024 = -CShort(2 ^ 10)
        ''' <summary>Rewind 2x</summary>
        Rewind512 = -CShort(2 ^ 9)
        ''' <summary>Rewind 1x</summary>
        Rewind256 = -CShort(2 ^ 8)
        ''' <summary>Rewind 1/2x</summary>
        Rewind128 = -CShort(2 ^ 7)
        ''' <summary>Rewind 1/4x</summary>
        Rewind64 = -CShort(2 ^ 6)
        ''' <summary>Rewind 1/8x</summary>
        Rewind32 = -CShort(2 ^ 5)
        ''' <summary>Rewind 1/16x</summary>
        Rewind16 = -CShort(2 ^ 4)
        ''' <summary>Rewind 1/32x</summary>
        Rewind8 = -CShort(2 ^ 3)
        ''' <summary>Pause</summary>
        Pause = 0
        ''' <summary>Slowdown 1/32x</summary>
        Play8 = CShort(2 ^ 3)
        ''' <summary>Slowdown 1/16x</summary>
        Play16 = CShort(2 ^ 4)
        ''' <summary>Slowdown 1/8x</summary>
        Play32 = CShort(2 ^ 5)
        ''' <summary>Slowdown 1/4x</summary>
        Play64 = CShort(2 ^ 6)
        ''' <summary>Slowdown 1/2x</summary>
        Play128 = CShort(2 ^ 7)
        ''' <summary>Normal</summary>
        Play256 = CShort(2 ^ 8)
        ''' <summary>Forward 2x</summary>
        Play512 = CShort(2 ^ 9)
        ''' <summary>Forward 4x</summary>
        Play1024 = CShort(2 ^ 10)
        ''' <summary>Forward 8x</summary>
        Play2048 = CShort(2 ^ 11)
        ''' <summary>Forward 16x</summary>
        Play4096 = CShort(2 ^ 12)
        ''' <summary>Forward 32x</summary>
        Play8192 = CShort(2 ^ 13)
    End Enum

    ''' <summary>
    ''' Contains constants for each possible 'video_zoom' value.
    ''' </summary>
    Public NotInheritable Class VideoZoomValues
        Private Sub New()
        End Sub

        ''' <summary>No zoom preset applied.</summary>
        Public Const Normal As String = "normal"
        ''' <summary>Full screen.</summary>
        ''' <remarks>Requires version 3.</remarks>
        Public Const FullEnlarge As String = "full_enlarge"
        ''' <summary>Stretch to full screen.</summary>
        ''' <remarks>Requires version 3.</remarks>
        Public Const FullStretch As String = "full_stretch"
        ''' <summary>Non-linear stretch.</summary>
        Public Const FillScreen As String = "fill_screen"
        ''' <summary>Non-linear stretch to full screen.</summary>
        Public Const FillFullScreen As String = "full_fill_screen"
        ''' <summary>Enlarge.</summary>
        Public Const Enlarge As String = "enlarge"
        ''' <summary>Make wider.</summary>
        Public Const MakeWider As String = "make_wider"
        ''' <summary>Make taller.</summary>
        Public Const MakeTaller As String = "make_taller"
        ''' <summary>Cut edges.</summary>
        Public Const CutEdges As String = "cut_edges"
        ''' <summary>Custom zoom settings applied.</summary>
        ''' <remarks>This value appears in the command results when a custom zoom mode is set using the ZOOM button on a remote control.</remarks>
        Public Const Other As String = "other"
    End Class

    ''' <summary>
    ''' Contains constants for each possible 'action_on_finish' value.
    ''' </summary>
    Public NotInheritable Class ActionOnFinishValues
        Private Sub New()
        End Sub
        ''' <summary>Repeat playback.</summary>
        Public Const RestartPlayback As String = "restart_playback"
        ''' <summary>Exit to navigator mode.</summary>
        Public Const [Exit] As String = "exit"
    End Class

    ''' <summary>
    ''' Contains constants for each possible DVD or Blu-ray navigation 'action' value.
    ''' </summary>
    Public NotInheritable Class ActionValues
        Private Sub New()
        End Sub

        Public Const Left As String = "left"
        Public Const Right As String = "right"
        Public Const Up As String = "up"
        Public Const Down As String = "down"
        Public Const Enter As String = "enter"
    End Class

    ''' <summary>
    ''' Contains constants for each pre-installed interface language.
    ''' It might be a good idea to localize your app in each of these.
    ''' </summary>
    ''' <remarks>
    ''' These languages are installed by default.
    ''' It is possible to install supplemental languages, in which case this enumeration becomes incomplete.
    ''' </remarks>
    Public NotInheritable Class InterfaceLanguageValues
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

    ''' <summary>Contains constants for each supported remote control and a helper function to retrieve the codes formatted as byte strings.</summary>
    ''' <remarks>All remote types have overlapping values, but it is in our best interest to keep redundant copies. Otherwise things become too complicated.</remarks>
    Public NotInheritable Class RemoteControls
        Private Sub New()
        End Sub

        ''' <summary>Control code for the new remotes.</summary>
        Public Const CustomerCode As Short = &HBF

        ''' <summary>
        ''' Returns the hexadecimal string representation of the 32-bit NEC code that belongs to a button.
        ''' Valid values are enumerated in <see cref="BigRemoteButtonValues"/>, <see cref="BigRemote2ButtonValues" /> and <see cref="SmallRemoteButtonValues"/>.
        ''' </summary>
        ''' <param name="button">The 16-bit value that represents a button.</param>
        ''' <returns>The 32-bit value that represents a NEC code.</returns>
        Public Shared Function GetButtonCode(button As Short) As String
            Dim control() As Byte = BitConverter.GetBytes(CustomerCode)
            Dim ir() As Byte = BitConverter.GetBytes(button)
            Dim necCode As Byte()

            If BitConverter.IsLittleEndian Then
                Array.Reverse(control)
                Array.Reverse(ir)
            End If

            necCode = control.Concat(ir).ToArray

            If BitConverter.IsLittleEndian Then
                Array.Reverse(necCode)
            End If

            Return BitConverterExtensions.ToString(necCode, Nothing)
        End Function

        ''' <summary>
        ''' Contains constants for each button on the big remote control (version 1).
        ''' </summary>
        Public Enum BigRemoteButtonValues As Short
            Eject = &H10EF
            Mute = &H46B9
            Mode = &H45BA
            Power = &H43BC
            Red = &H40BF
            Green = &H1FE0
            Yellow = &HFF
            Blue = &H41BE
            Num1 = &HBF4
            Num2 = &HCF3
            Num3 = &HDF2
            Num4 = &HEF1
            Num5 = &HFF0
            Num6 = &H1FE
            Num7 = &H11EE
            Num8 = &H12ED
            Num9 = &H13EC
            Num0 = &HAF5
            Clear = &H5FA
            [Select] = &H42BD
            VolumeUp = &H52AD
            VolumeDown = &H53AC
            PageUp = &H4BB4
            PageDown = &H4CB3
            Search = &H6F9
            Zoom = &H2FD
            Setup = &H4EB1
            Up = &H15EA
            Down = &H16E9
            Left = &H17E8
            Right = &H18E7
            Enter = &H14EB
            [Return] = &H4FB
            Info = &H50AF
            PopUpMenu = &H7F8
            TopMenu = &H51AE
            Play = &H48B7
            Pause = &H1EE1
            Previous = &H49B6
            [Next] = &H1DE2
            [Stop] = &H19E6
            Slow = &H1AE5
            Rewind = &H1CE3
            Forward = &H1BE4
            Subtitle = &H54AB
            AngleRotate = &H4DB2
            Audio = &H44BB
            Repeat = &H4FB0
            ShufflePip = &H47B8
            Url2ndAudio = &H3FC
        End Enum

        ''' <summary>
        ''' Contains constants for each button on the big remote control (version 2).
        ''' </summary>
        Public Enum BigRemote2ButtonValues As Short
            Eject = &H10EF
            Mute = &H46B9
            Mode = &H45BA
            Power = &H43BC
            Red = &H40BF
            Green = &H1FE0
            Yellow = &HFF
            Blue = &H41BE
            Num1 = &HBF4
            Num2 = &HCF3
            Num3 = &HDF2
            Num4 = &HEF1
            Num5 = &HFF0
            Num6 = &H1FE
            Num7 = &H11EE
            Num8 = &H12ED
            Num9 = &H13EC
            Num0 = &HAF5
            Clear = &H5FA
            [Select] = &H42BD
            VolumeUp = &H52AD
            VolumeDown = &H53AC
            PageUp = &H4BB4
            PageDown = &H4CB3
            Search = &H6F9
            Zoom = &H2FD
            Setup = &H4EB1
            Up = &H15EA
            Down = &H16E9
            Left = &H17E8
            Right = &H18E7
            Enter = &H14EB
            [Return] = &H4FB
            Info = &H50AF
            PopUpMenu = &H7F8
            TopMenu = &H51AE
            Play = &H48B7
            Pause = &H1EE1
            Previous = &H49B6
            [Next] = &H1DE2
            [Stop] = &H19E6
            Slow = &H1AE5
            Rewind = &H1CE3
            Forward = &H1BE4
            Subtitle = &H54AB
            AngleRotate = &H4DB2
            Audio = &H44BB
            Rec = &H609F
            Dune = &H619E
            Url = &H629D
        End Enum

        ''' <summary>
        ''' Contains constants for each button on the small remote control.
        ''' </summary>
        Public Enum SmallRemoteButtonValues As Short
            Mute = &H46B9
            Power = &H43BC
            Red = &H40BF
            Green = &H1FE0
            Yellow = &HFF
            Blue = &H41BE
            Num1 = &HBF4
            Num2 = &HCF3
            Num3 = &HDF2
            Num4 = &HEF1
            Num5 = &HFF0
            Num6 = &H1FE
            Num7 = &H11EE
            Num8 = &H12ED
            Num9 = &H13EC
            Num0 = &HAF5
            Clear = &H5FA
            [Select] = &H42BD
            Up = &H15EA
            Down = &H16E9
            Left = &H17E8
            Right = &H18E7
            Enter = &H14EB
            [Return] = &H4FB
            Info = &H50AF
            PopUpMenu = &H7F8
            TopMenu = &H51AE
            Previous = &H49B6
            [Next] = &H1DE2
            [Stop] = &H19E6
            Slow = &H1AE5
            Forward = &H1BE4
            Subtitle = &H54AB
            AngleRotate = &H4DB2
            Audio = &H44BB
            Rec = &H55AA
            PlayPause = &H56A9
            TopMenuDune = &H57A8
        End Enum

        ''' <summary>
        ''' Contains constants for button codes that are not on any Dune HD branded remote controls.
        ''' </summary>
        Public Enum SpecialButtonValues As Short
            DiscretePowerOn = &H5FA0
            DiscretePowerOff = &H5EA1
        End Enum
    End Class

    ''' <summary> Contains constants for each possible parameter that you can supply with 'dvd_navigation' or 'bluray_navigation' commands.</summary>
    Public NotInheritable Class NavigationParameterNames
        Private Sub New()
        End Sub

        Public Const Action As String = "action"
    End Class

    ''' <summary>Contains constants for each possible parameter that you can specify in an 'ir_code' command.</summary>
    Public NotInheritable Class InfraredCodeParameterNames
        Private Sub New()
        End Sub

        Public Const InfraredCode As String = "ir_code"
    End Class

    ''' <summary>Contains constants for each possible 'skip_keyframe' value.</summary>
    Public Enum SetKeyframeValues
        Previous = -1
        [Next] = 1
    End Enum

    ''' <summary>Contains constants for each possible 'playback_state' or 'last_playback_state' value.</summary>
    Public NotInheritable Class PlaybackStateValues
        Private Sub New()
        End Sub

        Public Const Playing As String = "playing"
        Public Const Stopped As String = "stopped"
        Public Const Buffering As String = "buffering"
        Public Const Initializing As String = "initializing"
        Public Const Deinitializing As String = "deinitializing"
        Public Const Seeking As String = "seeking"
    End Class

    ''' <summary>Contains constants for each possible 'last_playback_event' value.</summary>
    Public NotInheritable Class LastPlaybackEventValues
        Private Sub New()
        End Sub

        Public Const NoEvent As String = "no_event"
        Public Const ExternalAction As String = "external_action"
        Public Const MediaOpenFailed As String = "media_open_failed"
        Public Const MediaFormatNotSupported As String = "media_format_not_supported"
    End Class

    ''' <summary>Contains constants for each possible parameter that you can specify in a 'set_text' command.</summary>
    Public NotInheritable Class SetTextParameterNames
        Private Sub New()
        End Sub

        Public Const Text As String = "text"
    End Class

    ''' <summary>Contains constants for each possible parameter that you can specify in a 'launch_media_url' command when using www:// syntax.</summary>
    ''' <remarks>To use these, seperate the parameters from the 'media_url' value by using triple colons (":::") instead of an ampersand.</remarks>
    Public NotInheritable Class WebbrowserParameterNames
        Private Sub New()
        End Sub

        Public Const Fullscreen As String = "fullscreen"
        Public Const WebappKeys As String = "webapp_keys"
        Public Const ZoomLevel As String = "zoom_level"
        Public Const Overscan As String = "overscan"
        Public Const UserAgent As String = "user_agent"
        Public Const BackgroundColor As String = "background_color"
    End Class

End Class

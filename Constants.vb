Public NotInheritable Class Constants
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Contains constants for each possible command (as defined by the API).
    ''' </summary>
    Public NotInheritable Class Commands
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
        ''' <summary>Sends a dvd navigation command. It takes only one parameter: "action". Possible values are enumerated in <see cref="NavigationActions"/>.</summary>
        Public Const DvdNavigation As String = "dvd_navigation"
        ''' <summary>Sends a bluray navigation command. It takes only one parameter: "action". Possible values are enumerated in <see cref="NavigationActions"/>.</summary>
        Public Const BlurayNavigation As String = "bluray_navigation"

        ' Protocol version 3
        ''' <summary>Start playback of a playlist file.</summary>
        Public Const StartPlaylistPlayback As String = "start_playlist_playback"
        ''' <summary>Detect media url type.</summary>
        Public Const LaunchMediaUrl As String = "launch_media_url"

    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter that you can supply with a start playback command.
    ''' </summary>
    Public NotInheritable Class StartPlaybackParameters
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
    Public NotInheritable Class SetPlaybackStateParameters
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
        ''' <summary>Name of the zoom mode. Possible values are enumerated in <see cref="Constants.VideoZoomSettings"/>.</summary>
        Public Const VideoZoom As String = "video_zoom"
        ''' <summary>0 to enable custom video output dimensions; otherwise 1.</summary>
        Public Const VideoFullscreen As String = "video_fullscreen"
        ''' <summary>The horizontal video output position in pixels.</summary>
        Public Const VideoHorizontalPosition As String = "video_x"
        ''' <summary>The vertical video output position in pixels.</summary>
        Public Const VideoVerticalPosition As String = "video_y"
        ''' <summary>The horizontal video output width in pixels.</summary>
        Public Const VideoWidth As String = "video_width"
        ''' <summary>The vertical video output height in pixels.</summary>
        Public Const VideoHeight As String = "video_height"
    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter name in the command results.
    ''' </summary>
    Public NotInheritable Class CommandResults
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
    Public NotInheritable Class Status
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
    Public NotInheritable Class ErrorKinds
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
    Public NotInheritable Class PlayerStateSettings
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
    Public NotInheritable Class ProductIDs
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
    ''' Contains constants for each possible playback speed setting.
    ''' </summary>
    ''' <remarks>
    ''' The "start_x_playback" commands only accept values for normal speed and paused.
    ''' "set_playback_state" commands accept all values.
    ''' </remarks>
    Public Enum PlaybackSpeedSettings As Short
        Rewind32x = -CShort(2 ^ 13)
        Rewind16x = -CShort(2 ^ 12)
        Rewind8x = -CShort(2 ^ 11)
        Rewind4x = -CShort(2 ^ 10)
        Rewind2x = -CShort(2 ^ 9)
        Rewind1x = -CShort(2 ^ 8)
        Rewind1_2x = -CShort(2 ^ 7)
        Rewind1_4x = -CShort(2 ^ 6)
        Rewind1_8x = -CShort(2 ^ 5)
        Rewind1_16x = -CShort(2 ^ 4)
        ' Rewind1_32x = -CShort(2 ^ 3) uncomment if and when this becomes supported
        Pause = 0
        ' Slowdown1_32x = CShort(2 ^ 3) uncomment if and when this becomes supported
        Slowdown1_16x = CShort(2 ^ 4)
        Slowdown1_8x = CShort(2 ^ 5)
        Slowdown1_4x = CShort(2 ^ 6)
        Slowdown1_2x = CShort(2 ^ 7)
        Normal = CShort(2 ^ 8)
        Forward2x = CShort(2 ^ 9)
        Forward4x = CShort(2 ^ 10)
        Forward8x = CShort(2 ^ 11)
        Forward16x = CShort(2 ^ 12)
        Forward32x = CShort(2 ^ 13)
    End Enum

    ''' <summary>
    ''' Contains constants for each possible zoom setting.
    ''' </summary>
    ''' <remarks>
    ''' Custom zoom settings cannot be set through the "video_zoom" parameter.
    ''' You must set video height and size parameters instead.
    ''' </remarks>
    Public NotInheritable Class VideoZoomSettings
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
    Public NotInheritable Class ActionOnFinishSettings
        Private Sub New()
        End Sub

        Public Const RestartPlayback As String = "restart_playback"
        Public Const [Exit] As String = "exit"
    End Class

    ''' <summary>
    ''' Contains constants for each possible DVD menu action.
    ''' </summary>
    Public NotInheritable Class NavigationActions
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
    Public NotInheritable Class InterfaceLanguages
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

    ''' <summary>
    ''' Contains constants for each supported remote control and a helper function to retrieve the right codes.
    ''' </summary>
    ''' <remarks>Many of the buttons have overlapping values, but it is in our best interest to keep redundant copies or things get too complicated.</remarks>
    Public NotInheritable Class RemoteControls
        Private Sub New()
        End Sub

        ''' <summary>Code for the New remotes.</summary>
        Private Const CustomerCode As UShort = &HBF

        ''' <summary>
        ''' Returns the hexadecimal string representation of the 32-bit NEC code that belongs to a button.
        ''' Valid values are enumerated in <see cref="BigRemoteButtons"/>, <see cref="BigRemote2Buttons" /> and <see cref="SmallRemoteButtons"/>.
        ''' </summary>
        ''' <param name="button">The 16-bit value that represents a button.</param>
        ''' <returns>The 32-bit value that represents a NEC code.</returns>
        Public Shared Function GetButtonCode(ByVal button As UShort) As String
            Dim control() As Byte = BitConverter.GetBytes(CustomerCode)
            Dim suffix() As Byte = BitConverter.GetBytes(button)

            Return (String.Concat(BitConverter.ToString(suffix), BitConverter.ToString(control)).Replace("-"c, String.Empty))
        End Function

        ''' <summary>
        ''' Contains constants for each button on the big remote control (version 1).
        ''' </summary>
        Public Enum BigRemoteButtons As UShort
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
        Public Enum BigRemote2Buttons
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
        Public Enum SmallRemoteButtons
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
        Public Enum SpecialButtons
            DiscretePowerOn = &H5FA0
            DiscretePowerOff = &H5EA1
        End Enum
    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter that you can supply with a DVD or Blu-ray navigation command.
    ''' </summary>
    Public NotInheritable Class NavigationParameters
        Private Sub New()
        End Sub

        Public Const Action As String = "action"
    End Class

    ''' <summary>
    ''' Contains constants for each possible parameter that you can supply with an infrared code command.
    ''' </summary>
    Public NotInheritable Class InfraredCodeParameters
        Private Sub New()
        End Sub

        Public Const InfraredCode As String = "ir_code"
    End Class

    ''' <summary>
    ''' Contains constants for each possible 'skip_keyframe' setting.
    ''' </summary>
    Public Enum SetKeyframeSettings
        Previous = -1
        [Next] = 1
    End Enum

End Class

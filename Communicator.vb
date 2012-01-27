Imports System.Xml.Serialization
Imports System.Net
Imports System.IO
Imports System.Text

Namespace Dune.Communicator
    Public Class Communicator
        Private _dune As Dune
        Private _timeout As Integer = 20

        ''' <summary>
        ''' Constructor that attaches a Dune object to the communicator object.
        ''' </summary>
        ''' <param name="Dune"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dune As Dune)
            _dune = dune
        End Sub

        ''' <summary>
        ''' Private default constructor to prevent orphan objects.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Get or set the amount of seconds before a timeout is reached.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Timeout As Integer
            Get
                Return _timeout
            End Get
            Set(ByVal value As Integer)
                _timeout = value
            End Set
        End Property

#Region "Player Status"
        ''' <summary>
        ''' Gets the player status.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GetStatus()
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=status", _dune.IP, _dune.Port)
            DoCommand(commandURL)
        End Sub
#End Region

#Region "Start Playback"
        Public Enum PlaybackType
            File
            DVD
            BluRay
        End Enum

        Public Sub Start_Playback(ByVal type As PlaybackType, ByVal mediaURL As String)
            Call Start_Playback(type, mediaURL, New PlaybackOptions())
        End Sub

        Public Sub Start_Playback(ByVal type As PlaybackType, ByVal mediaURL As String, ByVal options As PlaybackOptions)
            Dim playbackType As String
            Dim actionOnFinish As String

            '===============
            '=check options=
            '===============
            Select Case Type
                Case Communicator.PlaybackType.File
                    playbackType = "start_file_playback"
                Case Communicator.PlaybackType.DVD
                    playbackType = "start_dvd_playback"
                Case Communicator.PlaybackType.BluRay
                    playbackType = "start_bluray_playback"
                Case Else
                    Throw New ArgumentException("Invalid playback type", "Type")
            End Select

            If String.IsNullOrWhiteSpace(MediaURL) Then
                Throw New ArgumentException("Media URL cannot be empty.", "MediaURL")
            End If

            If Options.Repeat = False Then
                actionOnFinish = "exit"
            Else
                actionOnFinish = "restart_playback"
            End If

            '========================
            '=build and send command=
            '========================

            Dim commandBuilder As New StringBuilder
            commandBuilder.AppendFormat("http://{0}:{1}/cgi-bin/do?cmd={2}", _dune.IP, _dune.Port, playbackType)
            commandBuilder.AppendFormat("&mediacommandURL={0}", mediaURL)
            commandBuilder.AppendFormat("&speed={0}", options.Speed)
            commandBuilder.AppendFormat("&position={0}", options.Position)
            commandBuilder.AppendFormat("&black_screen={0}", Math.Abs(CInt(options.BlackScreen)))
            commandBuilder.AppendFormat("&hide_osd={0}", Math.Abs(CInt(options.HideOSD)))
            commandBuilder.AppendFormat("&action_on_finish={0}", actionOnFinish)
            commandBuilder.AppendFormat("&timeout={0}", _timeout)

            DoCommand(commandBuilder.ToString)

        End Sub
#End Region

#Region "Set Playback State"
#Region "Volume"
        ''' <summary>
        ''' Toggles the muted status.
        ''' </summary>
        ''' <remarks>Mimics the remote control button, but you'll get more detailed player feedback.</remarks>
        Public Sub ToggleMute()
            If _dune.Muted Then
                UnmutePlayer()
            Else
                MutePlayer()
            End If
        End Sub

        ''' <summary>
        ''' Attempts to mute the player.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub MutePlayer()
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&mute=1&timeout=", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 _timeout)
            DoCommand(commandURL)
        End Sub

        ''' <summary>
        ''' Attempts to unmute the player.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UnmutePlayer()
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&mute=0&timeout={2}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 _timeout)
            DoCommand(commandURL)
        End Sub

        ''' <summary>
        ''' Sets the volume.
        ''' </summary>
        ''' <param name="Volume">value from 0 to 100.</param>
        ''' <remarks></remarks>
        Public Sub SetVolume(ByVal volume As Integer)
            If volume < 0 Or volume > 100 Then Throw New ArgumentOutOfRangeException("volume", volume, "Volume must be a positive value between 0 and 100.")
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&volume={2}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 volume)
            DoCommand(commandURL)
        End Sub

#End Region 'Volume

#Region "Position"
        ''' <summary>
        ''' Sets the playback position.
        ''' </summary>
        ''' <param name="Position">Position in seconds.</param>
        ''' <remarks></remarks>
        Public Sub SetPosition(ByVal position As Integer)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&position={2}&timeout={3}", _dune.IP, _dune.Port, position, _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Position

#Region "Black Screen"
        ''' <summary>
        ''' Enables or disables the black screen during playback.
        ''' </summary>
        ''' <param name="Enabled"></param>
        ''' <remarks></remarks>
        Public Sub SetBlackScreen(ByVal enabled As Boolean)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&black_screen={2}&timeout={3}", _dune.IP, _dune.Port, Math.Abs(CInt(enabled)), _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Black Screen

#Region "OSD"
        ''' <summary>
        ''' Hides or unhides the OSD during playback.
        ''' </summary>
        ''' <param name="Enabled"></param>
        ''' <remarks></remarks>
        Public Sub SetOSD(ByVal enabled As Boolean)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&hide_osd={2}&timeout={3}", _dune.IP, _dune.Port, Math.Abs(CInt(enabled)), _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'OSD

#Region "Speed"
        Public Enum Speeds
            Rewind_16x = -4096
            Rewind_8x = -2048
            Rewind_4x = -1024
            Rewind_2x = -512
            Rewind = -256
            Rewind_Slow = -64
            Pause = 0
            Slow = 64
            Normal = 256
            Forward_2x = 512
            Forward_4x = 1024
            Forward_8x = 2048
            Forward_16x = 4096
        End Enum

        ''' <summary>
        ''' Sets the playback speed.
        ''' </summary>
        ''' <param name="Speed"></param>
        ''' <remarks></remarks>
        Public Sub SetSpeed(ByVal speed As Speeds)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&speed={2}&timeout={3}", _dune.IP, _dune.Port, CInt(speed), _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Speed

#Region "Key frames"
        Public Enum Keyframe
            Previous = -1
            [Next] = 1
        End Enum

        ''' <summary>
        ''' Allows to navigate to the next/previous key frame.
        ''' </summary>
        ''' <param name="Direction"></param>
        ''' <remarks>Can only be used when the playback is paused; is supported for DVD and MKV only.</remarks>
        Public Sub SetKeyframe(ByVal direction As Keyframe)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&skip_frames={2}&timeout={3}", _dune.IP, _dune.Port, CInt(direction), _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Key frames

#Region "Action on finish"
        ''' <summary>
        ''' Sets the action on finish to exit or repeat playback.
        ''' </summary>
        ''' <param name="Repeat"></param>
        ''' <remarks></remarks>
        Public Sub SetRepeat(ByVal repeat As Boolean)
            Dim actionOnFinish As String
            If repeat Then
                actionOnFinish = "restart_playback"
            Else
                actionOnFinish = "exit"
            End If
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&action_on_finish={2}&timeout={3}", _dune.IP, _dune.Port, actionOnFinish, _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Action on finish

#Region "Video"
        ''' <summary>
        ''' Enables or disables video output during playback.
        ''' </summary>
        ''' <param name="Enabled"></param>
        ''' <remarks></remarks>
        Public Sub SetVideo(ByVal enabled As Boolean)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_enabled={2}&timeout={3}", _dune.IP, _dune.Port, Math.Abs(CInt(enabled)), _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Video

#Region "Zoom"
        Public Enum Zoom
            normal
            enlarge
            make_wider
            fill_screen
            full_fill_screen
            make_taller
            cut_edges
        End Enum

        ''' <summary>
        ''' Removes any and all zoom effects. Look at the overloads for more zoom options.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetZoom()
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_fullscreen={2}&timeout={3}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 1, _
                                                 _timeout)
            DoCommand(commandURL)
        End Sub

        ''' <summary>
        ''' Sets the zoom mode to one of the presets.
        ''' </summary>
        ''' <param name="Mode"></param>
        ''' <remarks></remarks>
        Public Sub SetZoom(ByVal mode As Zoom)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_zoom={2}&timeout={3}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 [Enum].GetName(GetType(Zoom), mode).ToLower, _
                                                 _timeout)

            DoCommand(commandURL)
        End Sub

        ''' <summary>
        ''' Use this to set a custom zoom mode.
        ''' </summary>
        ''' <param name="X">Padding between the left side of the screen and the video.</param>
        ''' <param name="Y">Padding between the top side of the screen and the video.</param>
        ''' <param name="Width">The video width.</param>
        ''' <param name="Height">The video height.</param>
        ''' <remarks>All values are in pixels.</remarks>
        Public Sub SetZoom(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_x={2}&video_y={3}&video_width={4}&video_height={5}&timeout={6}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 x, _
                                                 y, _
                                                 width, _
                                                 height, _
                                                 _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Zoom


#End Region 'Set Playback State

#Region "Player State"
        Public Enum PlayerStates
            Main_Screen
            Black_Screen
            Standby
        End Enum

        ''' <summary>
        ''' Sets the player state.
        ''' </summary>
        ''' <param name="State"></param>
        ''' <remarks></remarks>
        Public Sub SetPlayerState(ByVal state As PlayerStates)
            Dim command As String = state.ToString.ToLower
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd={2}&timeout={3}", _dune.IP, _dune.Port, command, _timeout)
            DoCommand(commandURL)
        End Sub
#End Region 'Player State


        ''' <summary>
        ''' Executes the supplied command and gets the command results.
        ''' </summary>
        ''' <param name="commandURL"></param>
        ''' <remarks></remarks>
        Public Sub DoCommand(ByVal commandURL As String)
            Dim results As New CommandResults(commandURL)

            If results.Parameters IsNot Nothing Then
                _dune._audioTracks.Clear()
                _dune._error = False
                For Each CommandParam In results.Parameters
                    Select Case CommandParam.Name.Split(".").GetValue(0).ToString
                        Case "protocol_version"
                            _dune._protocolVersion = CommandParam.Value
                        Case "command_status"
                            _dune._commandStatus = CommandParam.Value
                        Case "player_state"
                            _dune._playerState = CommandParam.Value
                        Case "playback_speed"
                            _dune._playbackSpeed = CInt(CommandParam.Value)
                        Case "playback_duration"
                            _dune._playbackDuration = CDbl(CommandParam.Value)
                        Case "playback_position"
                            _dune._playbackPosition = CDbl(CommandParam.Value)
                        Case "playback_is_buffering"
                            _dune._playbackIsBuffering = CBool(CommandParam.Value)
                        Case "playback_volume"
                            _dune._volume = CByte(CommandParam.Value)
                        Case "playback_mute"
                            _dune._playbackMute = CBool(CommandParam.Value)
                        Case "video_fullscreen"
                            _dune._fullscreen = CBool(CommandParam.Value)
                        Case "video_x"
                            _dune._videoX = CShort(CommandParam.Value)
                        Case "video_y"
                            _dune._videoY = CShort(CommandParam.Value)
                        Case "video_width"
                            _dune._videoWidth = CShort(CommandParam.Value)
                        Case "video_height"
                            _dune._videoHeight = CShort(CommandParam.Value)
                        Case "video_total_display_width"
                            _dune._totalDisplayWidth = CShort(CommandParam.Value)
                        Case "video_total_display_height"
                            _dune._totalDisplayHeight = CShort(CommandParam.Value)
                        Case "video_enabled"
                            _dune._videoEnabled = CBool(CommandParam.Value)
                        Case "video_zoom"
                            _dune._videoZoom = CommandParam.Value
                        Case "playback_dvd_menu"
                            _dune._DVDMenu = CBool(CommandParam.Value)
                        Case "error_kind"
                            _dune._error = True
                            _dune._errorKind = CommandParam.Value
                        Case "error_description"
                            _dune._errorDescription = CommandParam.Value
                        Case "audio_track"
                            If CommandParam.Name.Split(".").Length = 1 Then
                                _dune._audioTrack = CSByte(CommandParam.Value)
                            Else
                                _dune._audioTracks(CSByte(CommandParam.Name.Split(".").GetValue(1))) = CommandParam.Value
                            End If
                    End Select
                    _dune.RaisePropertyChanged(CommandParam.Name.Split(".").GetValue(0).ToString)
                Next
            Else
                _dune._error = True
                _dune._errorKind = "connection error"
                _dune._errorDescription = "command was not processed by the destination"
            End If
            Call _dune.RaiseUpdate()
        End Sub

    End Class

#Region "XMLSerializer structures"
    <XmlRoot("command_result")>
    Public Class CommandResults
        <XmlElement("param")>
        Public Property Parameters As List(Of CommandParam)


        Private Sub New()
        End Sub

        Public Sub New(ByVal commandURL As String)
            Try
                Dim request As HttpWebRequest = DirectCast(WebRequest.Create(commandURL), HttpWebRequest)
                request.Method = WebRequestMethods.Http.Get
                Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

                Dim cr As New CommandResults
                Using fs As Stream = response.GetResponseStream()
                    Dim xs As New XmlSerializer(GetType(CommandResults))
                    cr = DirectCast(xs.Deserialize(fs), CommandResults)
                End Using
                Parameters = cr.Parameters
            Catch ex As Exception
                Parameters = Nothing
            End Try
        End Sub
    End Class

    Public Class CommandParam
        <XmlAttribute("name")>
        Public Property Name As String
        <XmlAttribute("value")>
        Public Property Value As String
    End Class
#End Region
End Namespace
Imports System.Xml.Serialization
Imports System.Net
Imports System.IO
Imports System.Text

Namespace Dune.Communicator
    Public Class Communicator
        Private _dune As Dune
        Private intTimeoutSeconds As Integer = 20

        Public Sub New(ByVal Dune As Dune)
            _dune = Dune
        End Sub

        ''' <summary>
        ''' Get or set the amount of seconds before a timeout is reached.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Timeout As Integer
            Get
                Return intTimeoutSeconds
            End Get
            Set(ByVal value As Integer)
                intTimeoutSeconds = value
            End Set
        End Property

#Region "Player Status"
        ''' <summary>
        ''' Gets the player status.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GetStatus()
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=status", _dune.IP, _dune.Port)
            DoCommand(strURL)
        End Sub
#End Region

#Region "Start Playback"
        Public Enum PlaybackType
            File
            DVD
            BluRay
        End Enum

        Public Sub Start_Playback(ByVal Type As PlaybackType, ByVal MediaURL As String)
            Call Start_Playback(Type, MediaURL, New PlaybackOptions())
        End Sub

        Public Sub Start_Playback(ByVal Type As PlaybackType, ByVal MediaURL As String, ByVal Options As PlaybackOptions)
            Dim strPlaybackType As String
            Dim strActionOnFinish As String

            '===============
            '=check options=
            '===============
            Select Case Type
                Case PlaybackType.File
                    strPlaybackType = "start_file_playback"
                Case PlaybackType.DVD
                    strPlaybackType = "start_dvd_playback"
                Case PlaybackType.BluRay
                    strPlaybackType = "start_bluray_playback"
                Case Else
                    Throw New ArgumentException("Invalid playback type", "Type")
            End Select

            If String.IsNullOrWhiteSpace(MediaURL) Then
                Throw New ArgumentException("Media URL cannot be empty.", "MediaURL")
            End If

            If Options.Repeat = False Then
                strActionOnFinish = "exit"
            Else
                strActionOnFinish = "restart_playback"
            End If

            '========================
            '=build and send command=
            '========================

            Dim strCommandBuilder As New StringBuilder
            strCommandBuilder.AppendFormat("http://{0}:{1}/cgi-bin/do?cmd={2}", _dune.IP, _dune.Port, strPlaybackType)
            strCommandBuilder.AppendFormat("&media_url={0}", MediaURL)
            strCommandBuilder.AppendFormat("&speed={0}", Options.Speed)
            strCommandBuilder.AppendFormat("&position={0}", Options.Position)
            strCommandBuilder.AppendFormat("&black_screen={0}", Math.Abs(CInt(Options.BlackScreen)))
            strCommandBuilder.AppendFormat("&hide_osd={0}", Math.Abs(CInt(Options.HideOSD)))
            strCommandBuilder.AppendFormat("&action_on_finish={0}", strActionOnFinish)
            strCommandBuilder.AppendFormat("&timeout={0}", Me.intTimeoutSeconds)

            DoCommand(strCommandBuilder.ToString)

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
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&mute=1&timeout=", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 intTimeoutSeconds)
            DoCommand(strURL)
        End Sub

        ''' <summary>
        ''' Attempts to unmute the player.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UnmutePlayer()
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&mute=0&timeout={2}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 intTimeoutSeconds)
            DoCommand(strURL)
        End Sub

        Public Sub SetVolume(ByVal Volume As Integer)
            If Volume < 0 Or Volume > 100 Then Throw New ArgumentOutOfRangeException("Volume", Volume, "Volume must be a positive value between 0 and 100.")
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&volume={2}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 Volume)
            DoCommand(strURL)
        End Sub

#End Region 'Volume

#Region "Position"
        Public Sub SetPosition(ByVal Position As Integer)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&position={2}&timeout={3}", _dune.IP, _dune.Port, Position, intTimeoutSeconds)
            DoCommand(strURL)
        End Sub
#End Region 'Position

#Region "Black Screen"
        Public Sub SetBlackScreen(ByVal Enabled As Boolean)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&black_screen={2}&timeout={3}", _dune.IP, _dune.Port, Math.Abs(CInt(Enabled)), intTimeoutSeconds)
            DoCommand(strURL)
        End Sub
#End Region 'Black Screen

#Region "OSD"
        Public Sub SetOSD(ByVal Enabled As Boolean)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&hide_osd={2}&timeout={3}", _dune.IP, _dune.Port, Math.Abs(CInt(Enabled)), intTimeoutSeconds)
            DoCommand(strURL)
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

        Public Sub SetSpeed(ByVal Speed As Speeds)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&speed={2}&timeout={3}", _dune.IP, _dune.Port, CInt(Speed), intTimeoutSeconds)
            DoCommand(strURL)
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
        Public Sub SetKeyframe(ByVal Direction As Keyframe)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&skip_frames={2}&timeout={3}", _dune.IP, _dune.Port, CInt(Direction), intTimeoutSeconds)
            DoCommand(strURL)
        End Sub
#End Region 'Key frames

#Region "Action on finish"
        Public Sub SetRepeat(ByVal Repeat As Boolean)
            Dim strActionOnFinish As String
            If Repeat Then
                strActionOnFinish = "restart_playback"
            Else
                strActionOnFinish = "exit"
            End If
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&action_on_finish={2}&timeout={3}", _dune.IP, _dune.Port, strActionOnFinish, intTimeoutSeconds)
            DoCommand(strURL)
        End Sub
#End Region 'Action on finish

#Region "Video"
        Public Sub SetVideo(ByVal Enabled As Boolean)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_enabled={2}&timeout={3}", _dune.IP, _dune.Port, Math.Abs(CInt(Enabled)), intTimeoutSeconds)
            DoCommand(strURL)
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
        ''' Use this to set a preset zoom mode.
        ''' </summary>
        ''' <param name="Mode"></param>
        ''' <remarks></remarks>
        Public Sub SetZoom(ByVal Mode As Zoom)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_zoom={2}&timeout={3}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 [Enum].GetName(GetType(Zoom), Mode).ToLower, _
                                                 intTimeoutSeconds)

            DoCommand(strURL)
        End Sub

        ''' <summary>
        ''' Use this to return to fullscreen. 
        ''' </summary>
        ''' <param name="Fullscreen">Only use 'true' here, look at the overloads for other zoom options (where fullscreen=false is implied).</param>
        ''' <remarks>Setting this to false without supplying other parameters won't work. Look at the overloads for more zoom options.</remarks>
        Public Sub SetZoom(ByVal Fullscreen As Boolean)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_fullscreen={2}&timeout={3}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 Math.Abs(CInt(Fullscreen)), _
                                                 intTimeoutSeconds)
            DoCommand(strURL)
        End Sub

        ''' <summary>
        ''' Use this to set a custom zoom mode.
        ''' </summary>
        ''' <param name="Mode">Preset zoom mode.</param>
        ''' <param name="X">Padding between the left side of the screen and the video.</param>
        ''' <param name="Y">Padding between the top side of the screen and the video.</param>
        ''' <param name="Width">The video width.</param>
        ''' <param name="Height">The video height.</param>
        ''' <remarks>All values are in pixels.</remarks>
        Public Sub SetZoom(ByVal Mode As Zoom, ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer)
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=set_playback_state&video_x={2}&video_y={3}&video_width={4}&video_height={5}&timeout={6}", _
                                                 _dune.IP, _
                                                 _dune.Port, _
                                                 X, _
                                                 Y, _
                                                 Width, _
                                                 Height, _
                                                 intTimeoutSeconds)
            DoCommand(strURL)
        End Sub
#End Region 'Zoom


#End Region 'Set Playback State

#Region "Player State"
        Public Enum PlayerStates
            Main_Screen
            Black_Screen
            Standby
        End Enum

        Public Sub SetPlayerState(ByVal State As PlayerStates)
            Dim strCMD As String = State.ToString.ToLower
            Dim strURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd={2}&timeout={3}", _dune.IP, _dune.Port, strCMD, intTimeoutSeconds)
            DoCommand(strURL)
        End Sub
#End Region 'Player State


        ''' <summary>
        ''' Executes the supplied command and gets the command results.
        ''' </summary>
        ''' <param name="URL"></param>
        ''' <remarks></remarks>
        Public Sub DoCommand(ByVal URL As String)
            Dim results As New CommandResults(URL)

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

    <XmlRoot("command_result")>
    Public Class CommandResults
        <XmlElement("param")>
        Public Property Parameters As List(Of CommandParam)


        Public Sub New()
        End Sub

        Public Sub New(ByVal URL As String)
            ' URL = "http://192.168.1.142/cgi-bin/do?cmd=status" 'temporary
            Try
                Dim request As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
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
End Namespace
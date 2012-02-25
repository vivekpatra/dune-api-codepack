Namespace Dune
    ''' <summary>
    ''' Represents an object that is configured to communicate with a specified Dune.
    ''' </summary>
    ''' <remarks>Use the shared 'connect' function to create a new Dune object.</remarks>
    Public Class Dune
        Implements System.ComponentModel.INotifyPropertyChanged

        ' Network settings
        Private _IP As String
        Private _port As Integer
        Private _hostname As String


        ' Dune infomation
        Friend _commandStatus As String
        Friend _errorKind As String
        Friend _errorDescription As String
        Friend _protocolVersion As Byte
        Friend _playerState As String
        Friend _playbackSpeed As Integer
        Friend _playbackDuration As Integer
        Friend _playbackPosition As Integer
        Friend _playbackIsBuffering As Boolean
        Friend _volume As Byte
        Friend _playbackMute As Boolean
        Friend _audioTrack As SByte
        Friend _fullscreen As Boolean
        Friend _videoX As Short
        Friend _videoY As Short
        Friend _videoWidth As Short
        Friend _videoHeight As Short
        Friend _totalDisplayWidth As Short
        Friend _totalDisplayHeight As Short
        Friend _videoEnabled As Boolean
        Friend _videoZoom As String
        Friend _audioTracks As New SortedDictionary(Of SByte, String)
        Friend _DVDMenu As Boolean

        ' Program information
        Friend _connected As Boolean
        Friend _error As Boolean
        Friend _lastCommand As String
        Friend _lastRequest As String
        Friend _isContinious As Boolean
        Private WithEvents _communicator As Communicator.Communicator
        Private _remote As Communicator.StandardRemote
        Private _ping As Short
        Private _model As Models

        Private WithEvents _timer As New Timers.Timer

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        ''' <remarks>Should never be used</remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Address">The target Address (IP or hostname)</param>
        ''' <param name="Port">The port that should be used.</param>
        ''' <remarks></remarks>
        Private Sub New(ByVal model As Models, ByVal address As String, ByVal port As Integer)
            _model = model

            Dim host As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(address)

            _error = False
            _port = port
            If host.AddressList.Length > 0 Then
                _IP = host.AddressList.GetValue(0).ToString
            Else
                _IP = address
            End If
            _hostname = host.HostName

            _communicator = New Communicator.Communicator(Me)
            _remote = New Communicator.StandardRemote(Me)

            '====================
            '= get player status=
            '====================
            _communicator.GetStatus()
            If _error = True Then ' host is not a Dune or does not have firmware with IP control.
                Throw New System.Net.Sockets.SocketException(System.Net.Sockets.SocketError.ProtocolNotSupported)
            End If
            RaiseEvent StatusUpdated()
        End Sub

        ''' <summary>
        ''' Constructs a new Dune object, configured to connect to the supplied Address on port 80.
        ''' </summary>
        ''' <param name="Address">IP or hostname.</param>
        ''' <returns>Fully configured Dune object.</returns>
        ''' <remarks></remarks>
        Public Shared Function Connect(ByVal model As Models, ByVal address As String)
            Return Connect(model, address, 80)
        End Function

        ''' <summary>
        ''' Constructs a new Dune object, configured to connect to the supplied Address on the supplied port number.
        ''' </summary>
        ''' <param name="Address">IP or hostname.</param>
        ''' <param name="Port">Port number.</param>
        ''' <returns>Fully configured Dune object.</returns>
        ''' <remarks>The port number should only be changed if used in conjunction with port forwarding.</remarks>
        Public Shared Function Connect(ByVal model As Models, ByVal address As String, ByVal port As Integer)
            If String.IsNullOrWhiteSpace(address) Then
                Throw New ArgumentNullException("address", "Address cannot be an empty string.")
            End If

            Dim dune As New Dune(model, address, port)

            '================================
            '= start updating player status =
            '================================
            dune._timer.Interval = 1000
            dune._timer.Start()
            Return dune
        End Function

        ''' <summary>
        ''' If the connection was lost or suspended, use this to reconnect the Dune object.
        ''' </summary>
        ''' <remarks>Will throw an exception if a connection is still active.</remarks>
        Public Sub Reconnect()
            If Not _connected Then
                _commandStatus = "reconnecting"
                _timer.Start()
                RaiseEvent StatusUpdated()
            Else
                Throw New InvalidOperationException("Connection is still active.")
            End If
        End Sub

        ''' <summary>
        ''' Suspends the connection.
        ''' </summary>
        ''' <remarks>This method is the same one that is used for internal processing when connection is lost.</remarks>
        Public Sub Disconnect()
            If _timer.Enabled Then
                _timer.Enabled = False
                _connected = False
                _commandStatus = "disconnected"
                RaiseEvent StatusUpdated()
                RaiseEvent Disconnected()
            End If
        End Sub

        Public Event Disconnected()

#Region "Properties"
        ''' <summary>
        ''' Gets the Dune's IP.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IP As String
            Get
                Return _IP
            End Get
        End Property

        ''' <summary>
        ''' Gets the port number that is used.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Port As Integer
            Get
                Return _port
            End Get
        End Property

        ''' <summary>
        ''' Gets the Dune's hostname.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Hostname As String
            Get
                Return _hostname
            End Get
        End Property

        ''' <summary>
        ''' Gets the last command status.
        ''' </summary>
        ''' <value></value>
        ''' <returns>"ok" or "failed"</returns>
        ''' <remarks></remarks>
        ReadOnly Property Command_Status As String
            Get
                Return _commandStatus
            End Get
        End Property

        ''' <summary>
        ''' Gets the last error's error kind.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>This value remains until a new error occurs. Use the 'ErrorOccurred' property to check if an error occurred.</remarks>
        ReadOnly Property Error_Kind As String
            Get
                Return _errorKind
            End Get
        End Property


        ''' <summary>
        ''' Gets the last error's error description.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>This value remains until a new error occurs. Use the 'ErrorOccurred' property to check if an error occurred.</remarks>
        ReadOnly Property Error_Description As String
            Get
                Return _errorDescription
            End Get
        End Property

        ''' <summary>
        ''' Holds the Dune IP Protocol version.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Protocol_Version As Byte
            Get
                Return _protocolVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets the player state.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Player_State As String
            Get
                Return _playerState
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback speed (in human readable format).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Playback_Speed As String
            Get
                Select Case _playbackSpeed
                    Case "-4096"
                        Return "rewind (16x)"
                    Case "-2048"
                        Return "rewind (8x)"
                    Case "-1024"
                        Return "rewind (4x)"
                    Case "-512"
                        Return "rewind (2x)"
                    Case "-256"
                        Return "rewind"
                    Case "-64", "1073741760"
                        Return "slow rewind"
                    Case "0"
                        If _connected Then
                            If _playbackDuration = 0 Then
                                Return "N/A"
                            Else
                                Return "paused"
                            End If
                        Else
                            Return Nothing
                        End If
                    Case "64"
                        Return "slow"
                    Case "256"
                        Return "normal"
                    Case "512"
                        Return "forward"
                    Case "1024"
                        Return "forward (4x)"
                    Case "2048"
                        Return "forward (8x)"
                    Case "4096"
                        Return "forward (16x)"
                    Case Else
                        Return String.Format("unknown ({0})", _playbackSpeed)
                End Select
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback duration in seconds.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Playback_Duration As Integer
            Get
                Return _playbackDuration
            End Get
        End Property

        ''' <summary>
        ''' Use this to check for continuous streams.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the currently playing file is a continuous stream.</returns>
        ''' <remarks></remarks>
        ReadOnly Property IsContinuous As Boolean
            Get
                Return _isContinious
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback position in seconds.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Playback_Position As Integer
            Get
                Return _playbackPosition
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback time left in seconds.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Playback_Time_Left As Integer
            Get
                Return (_playbackDuration - _playbackPosition)
            End Get
        End Property

        ''' <summary>
        ''' Use this to see if the player is buffering.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the player is buffering, false if not.</returns>
        ''' <remarks></remarks>
        ReadOnly Property Playback_Is_Buffering As Boolean
            Get
                Return _playbackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Use this to find out if there is an active connection.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if there is a connection, false if not.</returns>
        ''' <remarks></remarks>
        ReadOnly Property Connected As Boolean
            Get
                Return _connected
            End Get
        End Property

        ''' <summary>
        ''' Use this to see if the last command returned an error.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if an error occurred, false if not.</returns>
        ''' <remarks></remarks>
        ReadOnly Property ErrorOccurred As Boolean
            Get
                Return _error
            End Get
        End Property

        ''' <summary>
        ''' Gets the last command.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property LastPlaybackCommand As String
            Get
                Return _lastCommand
            End Get
        End Property
        ''' <summary>
        ''' Gets the path/URL from the last playback request
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property LastPlaybackRequest As String
            Get
                Return _lastRequest
            End Get
        End Property

        ''' <summary>
        ''' Gets the player volume.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Value between 0 and 150.</returns>
        ''' <remarks>You can only set the volume above 100 by holding the volume up button on the remote control.</remarks>
        Public ReadOnly Property Playback_Volume As Byte
            Get
                Return _volume
            End Get
        End Property

        ''' <summary>
        ''' Gets the mute state.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Muted As Boolean
            Get
                Return _playbackMute
            End Get
        End Property

        ''' <summary>
        ''' Gets the current language track number.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Audio_Track As SByte
            Get
                Return _audioTrack
            End Get
        End Property

        ''' <summary>
        ''' Gets a dictionary type list of all current language tracks in tracknumber=language style.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Audio_Track_list As SortedDictionary(Of SByte, String)
            Get
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Gets the fullscreen state.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_Fullscreen As Boolean
            Get
                Return _fullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the horizontal position of the video.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_X As Short
            Get
                Return _videoX
            End Get
        End Property

        ''' <summary>
        ''' Gets the vertical position of the video.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_Y As Short
            Get
                Return _videoY
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of the video.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_Width As Short
            Get
                Return _videoWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the video.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_Height As Short
            Get
                Return _videoHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the available display width.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Total_Display_Width As Short
            Get
                Return _totalDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the available display height.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Total_Display_Height As Short
            Get
                Return _totalDisplayHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the video enabled state.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_Enabled As Boolean
            Get
                Return _videoEnabled
            End Get
        End Property

        ''' <summary>
        ''' Gets the zoom state.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Video_Zoom As String
            Get
                Return _videoZoom
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a DVD-menu is currently shown.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DVD_Menu As Boolean
            Get
                Return _DVDMenu
            End Get
        End Property

        Public ReadOnly Property Ping As Short
            Get
                Return _ping
            End Get
        End Property

        Public ReadOnly Property Model As Models
            Get
                Return _model
            End Get
        End Property
#End Region 'Properties

        ''' <summary>
        ''' Use this to interact with the current Dune.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>You can declare additional communicator objects if you somehow require multiple communicators for the same Dune.</remarks>
        Public ReadOnly Property Communicator As Communicator.Communicator
            Get
                Return _communicator
            End Get
        End Property

        ''' <summary>
        ''' Use this to emulate remote control button presses.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>You can declare additional standard remote objects if you somehow require multiple remote controls for the same Dune.</remarks>
        Public ReadOnly Property Remote As Communicator.StandardRemote
            Get
                Return _remote
            End Get
        End Property

        ''' <summary>
        ''' Runs every second to refresh the ping.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub _timer_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs) Handles _timer.Elapsed
            Dim ping As New Net.NetworkInformation.Ping
            AddHandler ping.PingCompleted, AddressOf ping_pingCompleted
            ping.SendAsync(Me._hostname, Me.Hostname)
        End Sub

        ''' <summary>
        ''' Runs when the ping has completed.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>If the ping was successfull, the status properties will be updated.</remarks>
        Private Sub ping_pingCompleted(ByVal sender As Object, ByVal e As Net.NetworkInformation.PingCompletedEventArgs)
            If _timer.Enabled Then
                Dim reply As System.Net.NetworkInformation.PingReply = e.Reply
                If reply Is Nothing Then
                    Call LostConnection()
                Else
                    If e.Reply.Status = System.Net.NetworkInformation.IPStatus.TimedOut Then
                        Call LostConnection()
                    Else
                        _ping = reply.RoundtripTime
                        RaisePropertyChanged("Ping")

                        If Not Me._connected Then
                            Me._commandStatus = "connected"
                            RaisePropertyChanged("Command_Status")

                            Me._connected = True
                            RaisePropertyChanged("Connected")
                        End If
                        Communicator.GetStatus()
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Used for internal processing of failed pings.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LostConnection()
            _ping = -1
            RaisePropertyChanged("Ping")

            Me._error = True
            Me._errorKind = "connection error"
            Me._errorDescription = "destination unreachable."
            Call Disconnect()
        End Sub

        ''' <summary>
        ''' Used for internal processing of updated properties.
        ''' </summary>
        ''' <remarks></remarks>
        Friend Sub RaiseUpdate()
            RaiseEvent StatusUpdated()
        End Sub

        ''' <summary>
        ''' Raised when a command has completed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Event StatusUpdated()

        Public Enum Output
            Parameters
        End Enum

        Public Overloads Function ToString() As String
            Return _hostname
        End Function

        Public Overloads Function ToString(ByVal Output As Output) As String
            Dim builder As New System.Text.StringBuilder

            If Not _connected Then
                builder.AppendFormat("Error: {0}{1}", Error_Kind, Environment.NewLine)
                builder.AppendFormat("Description: {0}{1}", Error_Description, Environment.NewLine)
                builder.AppendLine()
                builder.Append("Reconnecting")
                Reconnect()
            Else
                builder.AppendFormat("Host name: {0}{1}", Hostname, Environment.NewLine)
                builder.AppendFormat("IP address: {0}{1}", IP, Environment.NewLine)
                builder.AppendFormat("Protocol version: {0}{1}", Protocol_Version, Environment.NewLine)
                builder.AppendFormat("Ping: {0}{1}", _ping, Environment.NewLine)
                builder.AppendFormat("Player state: {0}{1}", Player_State, Environment.NewLine)
                builder.AppendFormat("Last command: {0}{1}", Command_Status, Environment.NewLine)
                If _error Then
                    builder.AppendFormat("Error: {0}{1}", Error_Kind, Environment.NewLine)
                    builder.AppendFormat("Description: {0}{1}", Error_Description, Environment.NewLine)
                End If
                builder.AppendFormat("Media URL: {0}{1}", LastPlaybackRequest, Environment.NewLine)
                If Playback_Speed <> "N/A" Then
                    builder.AppendFormat("Playback speed: {0}{1}", Playback_Speed, Environment.NewLine)
                    builder.AppendFormat("Playback position: {0}{1}", TimeSpan.FromSeconds(Playback_Position).ToString, Environment.NewLine)
                    builder.AppendFormat("Playback duration: {0}{1}", TimeSpan.FromSeconds(Playback_Duration).ToString, Environment.NewLine)
                    builder.AppendFormat("Playback time left: {0}{1}", TimeSpan.FromSeconds(Playback_Time_Left).ToString, Environment.NewLine)
                    builder.AppendFormat("Buffering: {0}{1}", Playback_Is_Buffering, Environment.NewLine)
                    If Protocol_Version >= 2 Then
                        builder.AppendFormat("Volume: {0}{1}", Playback_Volume, Environment.NewLine)
                        builder.AppendFormat("Mute: {0}{1}", Muted, Environment.NewLine)
                        If Audio_Track > -1 Then
                            builder.AppendFormat("Language track: {0}{1}", Audio_Track, Environment.NewLine)
                        End If
                        If Audio_Track_list.Count > 0 Then
                            builder.AppendFormat("Available language tracks: {0}{1}", Audio_Track_list.Count, Environment.NewLine)

                            For Each language As KeyValuePair(Of SByte, String) In Audio_Track_list
                                If String.IsNullOrWhiteSpace(language.Value) Then
                                    builder.AppendFormat("Language track #{0,5}: {1}{2}", language.Key, "undefined", Environment.NewLine)
                                Else
                                    builder.AppendFormat("Language track #{0,5}: {1}{2}", language.Key, language.Value, Environment.NewLine)

                                End If
                            Next
                        End If
                        If Video_X > -1 And Video_Y > -1 Then
                            builder.AppendFormat("Video X: {0}{1}", Video_X, Environment.NewLine)
                            builder.AppendFormat("Video Y: {0}{1}", Video_Y, Environment.NewLine)
                        End If
                        If Video_Width = -1 Or Video_Height = -1 Then
                            builder.AppendFormat("Video resolution: fullscreen{2}", Video_Width, Video_Height, Environment.NewLine)
                        Else
                            builder.AppendFormat("Video resolution: {0}x{1}{2}", Video_Width, Video_Height, Environment.NewLine)
                        End If
                        builder.AppendFormat("Display resolution: {0}x{1}{2}", Total_Display_Width, Total_Display_Height, Environment.NewLine)
                        builder.AppendFormat("Zoom: {0}{1}", Video_Zoom, Environment.NewLine)
                    End If
                End If
            End If
                Return builder.ToString
        End Function

        Public Sub RaisePropertyChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(info))
        End Sub
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Public Enum Models
            Generic
            TV_101
            Lite_53D
            Duo
            Max
            Smart_B1
            Smart_D1
            Smart_H1
            Base_3_0
            Dune_BD_Prime_3_0

            Base_2_0
            Base
            Center
            Dune_BD_Prime
            Mini
            Ultra
        End Enum
    End Class
End Namespace
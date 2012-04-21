Imports System.Net
Imports System.Net.Sockets
Imports System.ComponentModel
Imports DuneAPICodePack.Dune.ApiWrappers
Imports System.Globalization
Imports System.Timers
Imports System.Threading.Tasks

Namespace Dune

    ''' <summary>
    ''' This class represents a Dune player on the network and exposes several methods and properties to interact with it.
    ''' </summary>
    Public Class Dune
        Implements INotifyPropertyChanged
        Implements IProtocolVersion1
        Implements IProtocolVersion2

#Region "Private Fields"

        ' Custom fields
        Private _shares As List(Of NetworkDriveInfo)
        Private _model As String
        Private _playlist As Playlist
        Private _connected As Boolean

        ' Connection details
        Private _tcpClient As TcpClient

        Private _endpoint As IPEndPoint
        Private _hostname As String
        Private _macAddress As String
        Private _timeout As UInteger

        Private _updateTimer As Timer ' Responsible for status updates


        ' Protocol version 1 and up
        Private _remoteControl As RemoteControl

        Private _commandStatus As String
        Private _error As CommandException
        Private _protocolVersion As Byte
        Private _playerState As String
        Private _playbackSpeed As PlaybackSpeed
        Private _playbackDuration As TimeSpan
        Private _playbackPosition As TimeSpan
        Private _playbackTimeLeft As TimeSpan
        Private _playbackIsBuffering As Boolean
        Private _playbackDvdMenu As Boolean

        ' Protocol version 2 and up
        Private _playbackVolume As Byte?
        Private _playbackMute As Boolean?
        Private _audioTrack As SByte?
        Private _videoFullscreen As Boolean?
        Private _videoX As Short?
        Private _videoY As Short?
        Private _videoWidth As Short?
        Private _videoHeight As Short?
        Private _totalDisplayWidth As Short?
        Private _totalDisplayHeight As Short?
        Private _videoEnabled As Boolean?
        Private _videoZoom As String
        Private _audioTracks As SortedDictionary(Of Byte, CultureInfo)

#End Region ' Private Fields

#Region "Constructors"

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The IP address or hostname of the target device.</param>
        ''' <exception cref="System.Net.Sockets.SocketException">: An error is encountered when resolving address.</exception>
        Public Sub New(ByVal address As String)
            Me.New(address, 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The IP address of the target device.</param>
        ''' <exception cref="System.Net.Sockets.SocketException">: An error is encountered when resolving address.</exception>
        Public Sub New(ByVal address As IPAddress)
            Me.New(address, 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The host entry of the target device.</param>
        Public Sub New(ByVal address As IPHostEntry)
            Me.New(address, 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the specified port number.
        ''' </summary>
        ''' <param name="address">The IP address or hostname of the target device.</param>
        ''' <exception cref="ArgumentOutOfRangeException">: port is less than System.Net.IPEndPoint.MinPort.-or- port is greater than System.Net.IPEndPoint.MaxPort.-or- address is less than 0 or greater than 0x00000000FFFFFFFF.</exception>       
        ''' <exception cref="SocketException">: An error is encountered when resolving address.</exception>
        ''' <exception cref="WebException">: An error is encountered when trying to query the API.</exception>
        Public Sub New(ByVal address As String, ByVal port As Integer)
            Dim host As IPHostEntry = Dns.GetHostEntry(address)
            _endpoint = New IPEndPoint(host.AddressList.FirstOrDefault, port)
            _hostname = host.HostName.Split(".").FirstOrDefault
            Connect(_endpoint)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the specified port number.
        ''' </summary>
        ''' <param name="address">The IP address of the target device.</param>
        ''' <param name="port">The port number to connect on.</param>
        ''' <exception cref="ArgumentOutOfRangeException">: port is less than System.Net.IPEndPoint.MinPort.-or- port is greater than System.Net.IPEndPoint.MaxPort.-or- address is less than 0 or greater than 0x00000000FFFFFFFF.</exception>
        ''' <exception cref="SocketException">: An error is encountered when resolving address.</exception>
        ''' <exception cref="WebException">: An error is encountered when trying to query the API.</exception>
        Public Sub New(ByVal address As IPAddress, ByVal port As Integer)
            Dim host As IPHostEntry = Dns.GetHostEntry(address)
            _endpoint = New IPEndPoint(host.AddressList.FirstOrDefault, port)
            _hostname = host.HostName.Split(".").FirstOrDefault
            Connect(_endpoint)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the specified port number.
        ''' </summary>
        ''' <param name="address">The host entry of the target device.</param>
        ''' <param name="port">The port number to connect on.</param>
        ''' <exception cref="System.ArgumentOutOfRangeException">: port is less than System.Net.IPEndPoint.MinPort.-or- port is greater than System.Net.IPEndPoint.MaxPort.-or- address is less than 0 or greater than 0x00000000FFFFFFFF.</exception>
        ''' <exception cref="SocketException">: An error is encountered when resolving address.</exception>
        ''' <exception cref="WebException">: An error is encountered when trying to query the API.</exception>
        Public Sub New(ByVal address As IPHostEntry, ByVal port As Integer)
            _endpoint = New IPEndPoint(address.AddressList.FirstOrDefault, port)
            _hostname = address.HostName.Split(".").FirstOrDefault
            Connect(_endpoint)
        End Sub

#End Region ' Constructors

#Region "App Properties"

        ''' <summary>
        ''' Gets the IP address of the device.
        ''' </summary>
        Public ReadOnly Property Address As IPAddress
            Get
                Return _endpoint.Address
            End Get
        End Property

        ''' <summary>
        ''' Gets the port number used to connect to the service.
        ''' </summary>
        Public ReadOnly Property Port As Integer
            Get
                Return _endpoint.Port
            End Get
        End Property

        ''' <summary>
        ''' Gets the hostname of the device.
        ''' </summary>
        Public ReadOnly Property Hostname As String
            Get
                Return _hostname
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical (a.k.a. MAC) address.
        ''' </summary>
        Public ReadOnly Property PhysicalAdress As String
            Get
                If _macAddress = Nothing Then
                    _macAddress = NativeMethods.GetMacAddress(Address)
                End If
                Return _macAddress
            End Get
        End Property

        ''' <summary>
        ''' Gets an instance of the RemoteControl class.
        ''' </summary>
        Public ReadOnly Property RemoteControl As RemoteControl
            Get
                If _remoteControl Is Nothing Then
                    _remoteControl = New RemoteControl(Me)
                End If
                Return _remoteControl
            End Get
        End Property

        ''' <summary>
        ''' Gets a list of network shares made available by the player's SMB server.
        ''' </summary>
        Public ReadOnly Property NetworkShares As List(Of NetworkDriveInfo)
            Get
                If _shares Is Nothing Then
                    _shares = New NetworkShares(Me.Hostname).Shares
                End If
                Return _shares
            End Get
        End Property

        ''' <summary>
        ''' Gets the player's model. See remarks.
        ''' </summary>
        ''' <remarks>
        ''' This feature requires parsing a useragent string and is not yet implemented.
        ''' </remarks>
        Public ReadOnly Property Model As String
            Get
                'Throw New NotImplementedException("Retrieving player model is on my todo list.")
                If _model Is Nothing Then
                    ' TODO: implement product id stuff
                    _model = "Unknown"
                End If
                Return _model
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the connection is still active or sets it to on/off.
        ''' </summary>
        ''' <returns>True if there is a connection; otherwise false.</returns>
        Public Property Connected As Boolean
            Get
                Return _connected
            End Get
            Set(value As Boolean)
                If _connected <> value Then
                    If value = True Then
                        Reconnect()
                    Else
                        Interval = 0
                        ConnectedUpdate = False
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the connection status"
        ''' </summary>
        Friend WriteOnly Property ConnectedUpdate As Boolean
            Set(value As Boolean)
                If _connected <> value Then
                    _connected = value
                    RaisePropertyChanged("Connected")
                    If value = True Then
                        CommandStatusUpdate = "connected"
                    Else
                        CommandStatusUpdate = "disconnected"
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback time left.
        ''' </summary>
        ''' <remarks>Can be a negative value if the current playback is a livestream.</remarks>
        Public ReadOnly Property PlaybackTimeLeft As TimeSpan
            Get
                Return PlaybackDuration - PlaybackPosition
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of seconds before a command returns with a timeout.
        ''' </summary>
        ''' <value>A positive value bigger than 1</value>
        ''' <remarks>The default value is 20 seconds.</remarks>
        Public Property Timeout As UInteger
            Get
                If _timeout = Nothing Then
                    _timeout = 20
                End If
                Return _timeout
            End Get
            Set(value As UInteger)
                If value > 0 And Not _timeout.Equals(value) Then
                    _timeout = value
                    RaisePropertyChanged(Timeout)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the interval (in miliseconds) between status updates.
        ''' </summary>
        ''' <remarks>Setting this property to 0 will disable automatic status updates.</remarks>
        Public Property Interval As Double
            Get
                If _updateTimer Is Nothing Then
                    Return 0
                Else
                    Return _updateTimer.Interval
                End If
            End Get
            Set(value As Double)
                If value = 0 Then
                    If _updateTimer IsNot Nothing Then
                        _updateTimer.Stop()
                    End If
                    _updateTimer = Nothing
                Else
                    If _updateTimer Is Nothing Then
                        _updateTimer = New Timer()
                        AddHandler _updateTimer.Elapsed, AddressOf _updateTimer_elapsed
                        _updateTimer.Start()
                    End If
                    _updateTimer.Interval = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current playlist.
        ''' </summary>
        Public Property Playlist As Playlist
            Get
                If _playlist Is Nothing Then
                    _playlist = New Playlist(Me)
                    _playlist.RemoveLastFile = True
                End If
                Return _playlist
            End Get
            Set(value As Playlist)
                _playlist = value
            End Set
        End Property

#End Region ' App Properties

#Region "App methods"

        ''' <summary>
        ''' Attempts to initiate a connection with the target.
        ''' </summary>
        Private Sub Connect(ByVal target As IPEndPoint)
            Try
                If TargetIsValid(_endpoint) Then
                    CommandStatusUpdate = "connected"
                    Interval = 1000
                    ConnectedUpdate = True
                End If
            Catch connectionError As SocketException
                Throw connectionError
            Catch serviceError As WebException
                Throw serviceError
            End Try
        End Sub

        ''' <summary>
        ''' Verifies the target as a valid device.
        ''' </summary>
        Private Function TargetIsValid(ByVal target As IPEndPoint) As Boolean
            Try
                _tcpClient = New TcpClient()
                _tcpClient.Connect(target) ' can throw a SocketException

                UpdateValues(GetStatus()) ' can throw a WebException

            Catch tcpClientException As SocketException ' if the connection failed
                Throw tcpClientException
            Catch serviceException As WebException ' if the API didn't properly respond (errors 404, 401, 403 etc.)
                Throw serviceException
            End Try

            Return True
        End Function


        Private Sub _updateTimer_elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
            If Connected Then
                Try
                    UpdateValues(GetStatus)
                Catch ex As WebException
                    CommandStatusUpdate = "connection error"
                End Try
            Else
                Interval = 0
                CommandStatusUpdate = "lost connection"
            End If
        End Sub

        ''' <summary>
        ''' Updates the player status properties.
        ''' </summary>
        Private Sub UpdateValues(ByVal commandResult As CommandResult)
            With commandResult
                If Not String.IsNullOrWhiteSpace(commandResult.CommandStatus) Then
                    CommandStatusUpdate = .CommandStatus
                End If

                If .Error IsNot Nothing Then
                    ErrorUpdate = .Error
                Else
                    ErrorUpdate = Nothing
                End If

                ProtocolVersionUpdate = .ProtocolVersion

                PlayerStateUpdate = .PlayerState

                PlaybackSpeedUpdate = .PlaybackSpeed

                PlaybackDurationUpdate = .PlaybackDuration

                PlaybackPositionUpdate = .PlaybackPosition

                PlaybackIsBufferingUpdate = .PlaybackIsBuffering

                PlaybackDvdMenuUpdate = .PlaybackDvdMenu

                If .ProtocolVersion > 1 Then
                    VideoEnabledUpdate = .VideoEnabled

                    VideoZoomUpdate = .VideoZoom

                    VideoFullscreenUpdate = .VideoFullscreen

                    VideoXUpdate = .VideoX

                    VideoYUpdate = .VideoY

                    VideoWidthUpdate = .VideoWidth

                    VideoHeightUpdate = .VideoHeight

                    VideoTotalDisplayWidthUpdate = .VideoTotalDisplayWidth

                    VideoTotalDisplayHeightUpdate = .VideoTotalDisplayHeight

                    PlaybackVolumeUpdate = .PlaybackVolume

                    PlaybackMuteUpdate = .PlaybackMute

                    AudioTracksUpdate = .AudioTracks

                    AudioTrackUpdate = .AudioTrack

                End If

            End With
        End Sub

        ''' <summary>
        ''' Try to reconnect.
        ''' </summary>
        Public Sub Reconnect()
            If Not Connected Then
                CommandStatusUpdate = "reconnecting..."
                Try
                    If TargetIsValid(_endpoint) Then
                        Interval = 1000
                        ConnectedUpdate = True
                    End If
                Catch ex As SocketException
                    CommandStatusUpdate = "failed"
                End Try
            End If
        End Sub

        Protected Sub RaisePropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' Clears the command status.
        ''' </summary>
        Public Sub ClearStatus()
            CommandStatusUpdate = String.Empty
        End Sub

#End Region ' App methods

#Region "Properties v1"

        ''' <summary>
        ''' Gets the status of the last command.
        ''' </summary>
        Public ReadOnly Property CommandStatus As String Implements ApiWrappers.IProtocolVersion1.CommandStatus
            Get
                Return _commandStatus
            End Get
        End Property

        ''' <summary>
        ''' Sets the status of the last command.
        ''' </summary>
        Private WriteOnly Property CommandStatusUpdate As String Implements ApiWrappers.IProtocolVersion1.CommandStatusUpdate
            Set(value As String)
                If CommandStatus <> value Then
                    _commandStatus = value
                    RaisePropertyChanged("CommandStatus")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        Public ReadOnly Property PlaybackDuration As System.TimeSpan Implements ApiWrappers.IProtocolVersion1.PlaybackDuration
            Get
                Return _playbackDuration
            End Get
        End Property

        ''' <summary>
        ''' Sets the playback duration.
        ''' </summary>
        Private WriteOnly Property PlaybackDurationUpdate As System.TimeSpan Implements ApiWrappers.IProtocolVersion1.PlaybackDurationUpdate
            Set(value As System.TimeSpan)
                If Not PlaybackDuration.Equals(value) Then
                    _playbackDuration = value
                    RaisePropertyChanged("PlaybackDuration")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        ''' <exception cref="CommandException">
        ''' Sets the Error property to a new instance of CommandException if an error occurred.
        ''' </exception>
        Public Property PlaybackPosition As System.TimeSpan Implements ApiWrappers.IProtocolVersion1.PlaybackPosition
            Get
                Return _playbackPosition
            End Get
            Set(value As System.TimeSpan)
                Dim command As New SetPlaybackStateCommand(Me)
                command.Position = value
                command.Timeout = Timeout
                command.Method = DuneCommand.RequestMethod.Post
                Dim result As New CommandResult(command)
                UpdateValues(result)
            End Set
        End Property

        ''' <summary>
        ''' Sets the playback position.
        ''' </summary>
        Private WriteOnly Property PlaybackPositionUpdate As System.TimeSpan Implements ApiWrappers.IProtocolVersion1.PlaybackPositionUpdate
            Set(value As System.TimeSpan)
                If Not PlaybackPosition.Equals(value) Then
                    _playbackPosition = value
                    RaisePropertyChanged("PlaybackPosition")
                    RaisePropertyChanged("PlaybackTimeLeft")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        ''' <exception cref="CommandException">
        ''' Sets the Error property to a new instance of CommandException if an error occurred.
        ''' </exception>
        Public Property PlaybackSpeed As PlaybackSpeed Implements ApiWrappers.IProtocolVersion1.PlaybackSpeed
            Get
                Return _playbackSpeed
            End Get
            Set(value As PlaybackSpeed)
                Dim command As New SetPlaybackStateCommand(Me)
                command.Speed = CInt(value)
                command.Timeout = Timeout
                command.Method = DuneCommand.RequestMethod.Post
                Dim result As New CommandResult(command)
                UpdateValues(result)
            End Set
        End Property

        ''' <summary>
        ''' Sets the playback speed.
        ''' </summary>
        Private WriteOnly Property PlaybackSpeedUpdate As Integer Implements ApiWrappers.IProtocolVersion1.PlaybackSpeedUpdate
            Set(value As Integer)
                If PlaybackSpeed <> value Then
                    _playbackSpeed = value
                    RaisePropertyChanged("PlaybackSpeed")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the player state.
        ''' </summary>
        Public ReadOnly Property PlayerState As String Implements ApiWrappers.IProtocolVersion1.PlayerState
            Get
                Return _playerState
            End Get
        End Property

        ''' <summary>
        ''' Sets the player state.
        ''' </summary>
        Private WriteOnly Property PlayerStateUpdate As String Implements ApiWrappers.IProtocolVersion1.PlayerStateUpdate
            Set(value As String)
                Dim properCasedValue As String
                properCasedValue = StrConv(value.Replace("_", Space(1)), VbStrConv.ProperCase)

                If PlayerState Is Nothing OrElse Not PlayerState.Equals(properCasedValue) Then
                    _playerState = properCasedValue
                    RaisePropertyChanged("PlayerState")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the protocol version number.
        ''' </summary>
        Public ReadOnly Property ProtocolVersion As Byte Implements ApiWrappers.IProtocolVersion1.ProtocolVersion
            Get
                Return _protocolVersion
            End Get
        End Property

        ''' <summary>
        ''' Sets the protocol version number.
        ''' </summary>
        Private WriteOnly Property ProtocolVersionUpdate As Byte Implements ApiWrappers.IProtocolVersion1.ProtocolVersionUpdate
            Set(value As Byte)
                If Not ProtocolVersion.Equals(value) Then
                    _protocolVersion = value
                    RaisePropertyChanged("ProtocolVersion")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the error.
        ''' </summary>
        ''' <remarks>
        ''' Errors are a result of commands that:
        ''' <list type="bullet">
        ''' <item><description>are not allowed in the current player state</description></item>
        ''' <item><description>contain invalid values</description></item>
        ''' <item><description>are not recognised by the service</description></item>
        ''' </list>
        ''' </remarks>
        Public ReadOnly Property [Error] As ApiWrappers.CommandException Implements ApiWrappers.IProtocolVersion1.Error
            Get
                Return _error
            End Get
        End Property

        ''' <summary>
        ''' Sets the error.
        ''' </summary>
        Private WriteOnly Property ErrorUpdate As ApiWrappers.CommandException Implements ApiWrappers.IProtocolVersion1.ErrorUpdate
            Set(value As ApiWrappers.CommandException)
                If value IsNot Nothing Then
                    If _error Is Nothing OrElse Not _error.Equals(value) Then
                        _error = value
                        RaisePropertyChanged("Error")
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets whether a DVD menu is shown.
        ''' </summary>
        Public ReadOnly Property PlaybackDvdMenu As Boolean Implements ApiWrappers.IProtocolVersion1.PlaybackDvdMenu
            Get
                Return _playbackDvdMenu
            End Get
        End Property

        ''' <summary>
        ''' Sets whether a DVD menu is shown.
        ''' </summary>
        Private WriteOnly Property PlaybackDvdMenuUpdate As Boolean Implements ApiWrappers.IProtocolVersion1.PlaybackDvdMenuUpdate
            Set(value As Boolean)
                If Not PlaybackDvdMenu.Equals(value) Then
                    _playbackDvdMenu = value
                    RaisePropertyChanged("PlaybackDvdMenu")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        Public ReadOnly Property PlaybackIsBuffering As Boolean Implements ApiWrappers.IProtocolVersion1.PlaybackIsBuffering
            Get
                Return _playbackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Sets whether the playback is buffering.
        ''' </summary>
        Private WriteOnly Property PlaybackIsBufferingUpdate As Boolean Implements ApiWrappers.IProtocolVersion1.PlaybackIsBufferingUpdate
            Set(value As Boolean)
                If Not PlaybackIsBuffering.Equals(value) Then
                    _playbackIsBuffering = value
                    RaisePropertyChanged("PlaybackIsBuffering")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets whether to show a black screen during playback.
        ''' </summary>
        Public WriteOnly Property BlackScreen As Boolean Implements ApiWrappers.IProtocolVersion1.BlackScreen
            Set(value As Boolean)
                Dim command As New SetPlaybackStateCommand(Me)
                command.BlackScreen = value
                command.Timeout = Timeout
                Dim result As New CommandResult(command)
                UpdateValues(result)
            End Set
        End Property

        ''' <summary>
        ''' Sets whether to hide the OSD during playback.
        ''' </summary>
        Public WriteOnly Property HideOnScreenDisplay As Boolean Implements ApiWrappers.IProtocolVersion1.HideOnScreenDisplay
            Set(value As Boolean)
                Dim command As New SetPlaybackStateCommand(Me)
                command.HideOnScreenDisplay = value
                command.Timeout = Timeout
                Dim result As New CommandResult(command)
                UpdateValues(result)
            End Set
        End Property

#End Region ' Properties v1

#Region "Methods v1"

        ''' <summary>
        ''' Gets the player status and updates all relevant properties.
        ''' </summary>
        ''' <returns>New instance of the <seealso cref="CommandResult"/> class.</returns>
        ''' <exception cref="System.Net.WebException">: An error occurred when trying to query the API.</exception>
        Public Function GetStatus() As ApiWrappers.CommandResult Implements ApiWrappers.IProtocolVersion1.GetStatus
            Dim command As New GetStatusCommand(Me)
            command.Method = DuneCommand.RequestMethod.Post
            Dim result As New CommandResult(command)
            Return result
        End Function

        ''' <summary>
        ''' Changes playback options (such as volume) to get the desired playback state.
        ''' </summary>
        ''' <param name="command">An instance of a <see cref="SetPlaybackStateCommand"/> object that contains the desired parameter values.</param>
        ''' <returns>New instance of the <seealso cref="CommandResult"/> class.</returns>
        Public Function SetPlaybackState(ByVal command As ApiWrappers.SetPlaybackStateCommand) As ApiWrappers.CommandResult Implements ApiWrappers.IProtocolVersion1.SetPlaybackState
            If Not command.Timeout.HasValue Then
                command.Timeout = Timeout
            End If
            Dim result As New CommandResult(command)
            UpdateValues(result)
            Return result
        End Function

        ''' <summary>
        ''' Sets the player state to 'main screen', 'black screen' or 'standby'.
        ''' </summary>
        ''' <param name="command">An instance of a <see cref="SetPlayerStateCommand"/> object that contains the desired player state value.</param>
        ''' <returns>New instance of the <seealso cref="CommandResult"/> class.</returns>
        Public Function SetPlayerState(ByVal command As ApiWrappers.SetPlayerStateCommand) As ApiWrappers.CommandResult Implements ApiWrappers.IProtocolVersion1.SetPlayerState
            If Not command.Timeout.HasValue Then
                command.Timeout = Timeout
            End If
            Dim result As New CommandResult(command)
            UpdateValues(result)
            Return result
        End Function

        ''' <summary>
        ''' Starts playback.
        ''' </summary>
        ''' <param name="command">An instance of a <see cref="StartPlaybackCommand"/> object that contains the desired media URL and playback options.</param>
        ''' <returns>New instance of the <seealso cref="CommandResult"/> class.</returns>
        Public Function StartPlayback(ByVal command As ApiWrappers.StartPlaybackCommand) As ApiWrappers.CommandResult Implements ApiWrappers.IProtocolVersion1.StartPlayback
            If Not command.Timeout.HasValue Then
                command.Timeout = Timeout
            End If
            Dim result As New CommandResult(command)
            UpdateValues(result)
            Return result
        End Function

#End Region ' Methods v1

#Region "Properties v2"

        ''' <summary>
        ''' Gets or sets the current language track number.
        ''' </summary>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.</remarks>
        Public Property AudioTrack As Byte? Implements ApiWrappers.IProtocolVersion2.AudioTrack
            Get
                Return _audioTrack
            End Get
            Set(value As Byte?)
                If ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.AudioTrack = value
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the current language track number
        ''' </summary>
        Private WriteOnly Property AudioTrackUpdate As Byte? Implements ApiWrappers.IProtocolVersion2.AudioTrackUpdate
            Set(value As Byte?)
                If Not AudioTrack.Equals(value) Then
                    _audioTrack = value
                    RaisePropertyChanged("AudioTrack")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the video output is fullscreen.
        ''' </summary>
        Public ReadOnly Property VideoFullscreen As Boolean? Implements ApiWrappers.IProtocolVersion2.VideoFullscreen
            Get
                Return _videoFullscreen
            End Get
        End Property

        ''' <summary>
        ''' Sets whether the video output is fullscreen.
        ''' </summary>
        Private WriteOnly Property VideoFullscreenUpdate As Boolean? Implements ApiWrappers.IProtocolVersion2.VideoFullscreenUpdate
            Set(value As Boolean?)
                If Not VideoFullscreen.Equals(value) Then
                    _videoFullscreen = value
                    RaisePropertyChanged("VideoFullscreen")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the list of available audio tracks for the current playback.
        ''' </summary>
        ''' <returns>An instance of a <see cref="SortedDictionary(Of Byte, CultureInfo)" /> object that represents the list of available audio tracks.</returns>
        Public ReadOnly Property AudioTracks As SortedDictionary(Of Byte, CultureInfo) Implements ApiWrappers.IProtocolVersion2.AudioTracks
            Get
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Sets the list of available audio tracks for the current playback.
        ''' </summary>
        Private WriteOnly Property AudioTracksUpdate As SortedDictionary(Of Byte, CultureInfo) Implements ApiWrappers.IProtocolVersion2.AudioTracksUpdate
            Set(value As SortedDictionary(Of Byte, CultureInfo))
                If AudioTracks Is Nothing OrElse Not AudioTracks.Equals(value) Then
                    _audioTracks = value
                    RaisePropertyChanged("AudioTracks")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the mute status for the current playback.
        ''' </summary>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.</remarks>
        Public Property PlaybackMute As Boolean? Implements ApiWrappers.IProtocolVersion2.PlaybackMute
            Get
                Return _playbackMute
            End Get
            Set(value As Boolean?)
                If ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.Mute = value
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the mute status for the current playback.
        ''' </summary>
        Private WriteOnly Property PlaybackMuteUpdate As Boolean? Implements ApiWrappers.IProtocolVersion2.PlaybackMuteUpdate
            Set(value As Boolean?)
                If Not PlaybackMute.Equals(value) Then
                    _playbackMute = value
                    RaisePropertyChanged("PlaybackMute")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the video output is enabled.
        ''' </summary>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.</remarks>
        Public Property VideoEnabled As Boolean? Implements ApiWrappers.IProtocolVersion2.VideoEnabled
            Get
                Return _videoEnabled
            End Get
            Set(value As Boolean?)
                If ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.VideoEnabled = value
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets whether the video output is enabled.
        ''' </summary>
        Private WriteOnly Property VideoEnabledUpdate As Boolean? Implements ApiWrappers.IProtocolVersion2.VideoEnabledUpdate
            Set(value As Boolean?)
                If Not VideoEnabled.Equals(value) Then
                    _videoEnabled = value
                    RaisePropertyChanged("VideoEnabled")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video display height.
        ''' </summary>
        Public Property VideoHeight As UShort? Implements ApiWrappers.IProtocolVersion2.VideoHeight
            Get
                Return _videoHeight
            End Get
            Set(value As UShort?)
                If value.HasValue And ProtocolVersion >= 2 Then
                    Dim command As New SetVideoOutputCommand(Me, VideoX, VideoY, VideoWidth, value)
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video display height.
        ''' </summary>
        Private WriteOnly Property VideoHeightUpdate As UShort? Implements ApiWrappers.IProtocolVersion2.VideoHeightUpdate
            Set(value As UShort?)
                If Not VideoHeight.Equals(value) Then
                    _videoHeight = value
                    RaisePropertyChanged("VideoHeight")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total video display height.
        ''' </summary>
        Public ReadOnly Property VideoTotalDisplayHeight As UShort? Implements ApiWrappers.IProtocolVersion2.VideoTotalDisplayHeight
            Get
                Return _totalDisplayHeight
            End Get
        End Property

        ''' <summary>
        ''' Sets the total video display height.
        ''' </summary>
        Private WriteOnly Property VideoTotalDisplayHeightUpdate As UShort? Implements ApiWrappers.IProtocolVersion2.VideoTotalDisplayHeightUpdate
            Set(value As UShort?)
                If Not VideoTotalDisplayHeight.Equals(value) Then
                    _totalDisplayHeight = value
                    RaisePropertyChanged("VideoTotalDisplayHeight")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total video display width.
        ''' </summary>
        Public ReadOnly Property VideoTotalDisplayWidth As UShort? Implements ApiWrappers.IProtocolVersion2.VideoTotalDisplayWidth
            Get
                Return _totalDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Sets the total video display width.
        ''' </summary>
        Private WriteOnly Property VideoTotalDisplayWidthUpdate As UShort? Implements ApiWrappers.IProtocolVersion2.VideoTotalDisplayWidthUpdate
            Set(value As UShort?)
                If Not VideoTotalDisplayWidth.Equals(value) Then
                    _totalDisplayWidth = value
                    RaisePropertyChanged("VideoTotalDisplayWidth")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video display width.
        ''' </summary>
        Public Property VideoWidth As UShort? Implements ApiWrappers.IProtocolVersion2.VideoWidth
            Get
                Return _videoWidth
            End Get
            Set(value As UShort?)
                If value.HasValue And ProtocolVersion >= 2 Then
                    Dim command As New SetVideoOutputCommand(Me, VideoX, VideoY, value, VideoHeight)
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video display width.
        ''' </summary>
        Private WriteOnly Property VideoWidthUpdate As UShort? Implements ApiWrappers.IProtocolVersion2.VideoWidthUpdate
            Set(value As UShort?)
                If Not VideoWidth.Equals(value) Then
                    _videoWidth = value
                    RaisePropertyChanged("VideoWidth")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video's horizontal position.
        ''' </summary>
        Public Property VideoX As UShort? Implements ApiWrappers.IProtocolVersion2.VideoX
            Get
                Return _videoX
            End Get
            Set(value As UShort?)
                If value.HasValue And ProtocolVersion >= 2 Then
                    Dim command As New SetVideoOutputCommand(Me, value, VideoY, VideoWidth, VideoHeight)
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video's horizontal position.
        ''' </summary>
        Private WriteOnly Property VideoXUpdate As UShort? Implements ApiWrappers.IProtocolVersion2.VideoXUpdate
            Set(value As UShort?)
                If Not VideoX.Equals(value) Then
                    _videoX = value
                    RaisePropertyChanged("VideoX")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video's vertical position.
        ''' </summary>
        Public Property VideoY As UShort? Implements ApiWrappers.IProtocolVersion2.VideoY
            Get
                Return _videoY
            End Get
            Set(value As UShort?)
                If value.HasValue And ProtocolVersion >= 2 Then
                    Dim command As New SetVideoOutputCommand(Me, VideoX, value, VideoWidth, VideoHeight)
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video's vertical position.
        ''' </summary>
        Private WriteOnly Property VideoYUpdate As UShort? Implements ApiWrappers.IProtocolVersion2.VideoYUpdate
            Set(value As UShort?)
                If Not VideoY.Equals(value) Then
                    _videoY = value
                    RaisePropertyChanged("VideoY")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback volume
        ''' </summary>
        ''' <value>A positive value between 0 and 100.</value>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.
        ''' A maximum volume of 150 can be set using the remote control by holding 'volume up' for longer than usual.</remarks>
        Public Property PlaybackVolume As Byte? Implements ApiWrappers.IProtocolVersion2.PlaybackVolume
            Get
                Return _playbackVolume
            End Get
            Set(value As Byte?)
                If Not value.HasValue Then
                    value = 0
                ElseIf value > 150 Then
                    value = 100
                End If

                If ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.Volume = value
                    command.Timeout = Timeout
                    command.Method = DuneCommand.RequestMethod.Post
                    Dim result As CommandResult = New CommandResult(command)
                    UpdateValues(result)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the playback volume.
        ''' </summary>
        Private WriteOnly Property PlaybackVolumeUpdate As Byte? Implements ApiWrappers.IProtocolVersion2.PlaybackVolumeUpdate
            Set(value As Byte?)
                If Not PlaybackVolume.Equals(value) Then
                    _playbackVolume = value
                    RaisePropertyChanged("PlaybackVolume")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video zoom.
        ''' </summary>
        Public Property VideoZoom As String Implements ApiWrappers.IProtocolVersion2.VideoZoom
            Get
                Return _videoZoom
            End Get
            Set(value As String)
                Dim z As Zoom = (From zoom As Zoom In [Enum].GetValues(GetType(Zoom))
                                Where zoom.ToString = value
                                Select zoom).FirstOrDefault

                Dim command As New SetVideoOutputCommand(Me, z)
                command.Method = DuneCommand.RequestMethod.Post
                command.Timeout = Timeout
                Dim result As New CommandResult(command)
                UpdateValues(result)
            End Set
        End Property

        ''' <summary>
        ''' Sets the video zoom
        ''' </summary>
        Private WriteOnly Property VideoZoomUpdate As String Implements ApiWrappers.IProtocolVersion2.VideoZoomUpdate
            Set(value As String)
                If Not VideoZoom = value Then
                    _videoZoom = value
                    RaisePropertyChanged("VideoZoom")
                End If
            End Set
        End Property

#End Region 'Properties v2

#Region "Methods v2"

        ''' <summary>
        ''' Changes how the video output is displayed.
        ''' </summary>
        Public Function SetVideoOutput(ByVal command As SetVideoOutputCommand) As CommandResult Implements ApiWrappers.IProtocolVersion2.SetVideoOutput
            If Me.ProtocolVersion >= 2 Then
                If Not command.Timeout.HasValue Then
                    command.Timeout = Timeout
                End If
                Dim result As New CommandResult(command)
                UpdateValues(result)
                Return result
            Else
                Return Nothing
            End If
        End Function

#End Region ' Methods v2

        ''' <summary>
        ''' INotifyPropertyChanged implementation.
        ''' </summary>
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    End Class

End Namespace
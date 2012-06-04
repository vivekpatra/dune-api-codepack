Imports System.Net
Imports System.Net.Sockets
Imports System.ComponentModel
Imports SL.DuneApiCodePack.DuneUtilities.ApiWrappers
Imports System.Globalization
Imports System.Timers
Imports System.Threading.Tasks
Imports System.Net.NetworkInformation
Imports System.Collections.ObjectModel
Imports SL.DuneApiCodePack.Storage
Imports SL.DuneApiCodePack.Telnet
Imports SL.DuneApiCodePack.Extensions
Imports SL.DuneApiCodePack.ApiWrappers
Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities

    ''' <summary>
    ''' This class represents a Dune player on the network and exposes several methods and properties to interact with it.
    ''' </summary>
    Public Class Dune
        Implements INotifyPropertyChanged
        Implements IProtocolVersion1
        Implements IProtocolVersion2

#Region "Private Fields"

        ' Custom fields
        Private _shares As List(Of LocalStorage)
        Private _connected As Boolean
        Private _networkAdapterInfo As NetworkAdapterInfo
        Private _firmwares As List(Of FirmwareProperties)
        Private _telnetClient As TelnetClient
        Private _telnetEnabled As Boolean?
        Private _systemInfo As SystemInformation


        ' Connection details
        Private _hostentry As IPHostEntry
        Private _endpoint As IPEndPoint
        Private _timeout As UInteger
        Private _updateTimer As Timer ' Responsible for status updates


        ' Protocol version 1 and up
        Private _remoteControl As RemoteControl

        Private _commandStatus As String
        Private _commandError As CommandException
        Private _protocolVersion As Byte
        Private _playerState As String
        Private _playbackSpeed As Constants.PlaybackSpeedSettings
        Private _playbackDuration As TimeSpan?
        Private _playbackPosition As TimeSpan?
        Private _playbackTimeLeft As TimeSpan?
        Private _playbackIsBuffering As Boolean
        Private _playbackDvdMenu As Boolean

        ' Protocol version 2 and up
        Private _playbackVolume As Byte?
        Private _playbackMute As Boolean?
        Private _audioTrack As Byte?
        Private _videoFullscreen As Boolean?
        Private _videoX As UShort?
        Private _videoY As UShort?
        Private _videoWidth As UShort?
        Private _videoHeight As UShort?
        Private _videoTotalDisplayWidth As UShort?
        Private _videoTotalDisplayHeight As UShort?
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
            Me.New(Dns.GetHostEntry(address), 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The IP address of the target device.</param>
        ''' <exception cref="System.Net.Sockets.SocketException">: An error is encountered when resolving address.</exception>
        Public Sub New(ByVal address As IPAddress)
            Me.New(Dns.GetHostEntry(address), 80)
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
            Me.New(Dns.GetHostEntry(address), port)
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
            Me.New(Dns.GetHostEntry(address), port)
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
            _hostentry = address
            _networkAdapterInfo = New NetworkAdapterInfo(address.AddressList.First)
            _endpoint = New IPEndPoint(NetworkAdapterInfo.Address, port)

            Initialize()
        End Sub

        Private Sub Initialize()
            Connect(_endpoint)

            If Connected Then
                If TelnetClient IsNot Nothing Then
                    Task.Factory.StartNew(Sub() _firmwares = FirmwareProperties.GetAvailableFirmwaresAsync(ProductID).Result)
                End If
            End If
        End Sub

#End Region ' Constructors

#Region "App Properties"

        ''' <summary>
        ''' Gets the IP address of the device.
        ''' </summary>
        <DisplayName("IP address")>
        <Description("The IP address on which the connection is made.")>
        Public ReadOnly Property Address As IPAddress
            Get
                Return _endpoint.Address
            End Get
        End Property

        ''' <summary>
        ''' Gets the port number used to connect to the service.
        ''' </summary>
        <DisplayName("Port")>
        <Description("The port on which the app is connected to the service.")>
        Public ReadOnly Property Port As Integer
            Get
                Return _endpoint.Port
            End Get
        End Property

        ''' <summary>
        ''' Gets the host entry details returned from the DNS server.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HostEntry As IPHostEntry
            Get
                Return _hostentry
            End Get
        End Property

        ''' <summary>
        ''' Gets the hostname of the device.
        ''' </summary>
        <DisplayName("Hostname")>
        <Description("The device's hostname.")>
        Public ReadOnly Property Hostname As String
            Get
                If HostEntry.HostName.Contains(".") Then
                    Return HostEntry.HostName.Left(HostEntry.HostName.IndexOf("."c))
                Else
                    Return HostEntry.HostName
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets information about the device's network interface.
        ''' </summary>
        <DisplayName("Network adapter")>
        <Description("Displays information about the device's network interface and its vendor.")>
        Public ReadOnly Property NetworkAdapterInfo As NetworkAdapterInfo
            Get
                Return _networkAdapterInfo
            End Get
        End Property

        ''' <summary>
        ''' Gets an instance of the RemoteControl class.
        ''' </summary>
        <DisplayName("Remote Control")>
        <Description("Preconfigured instance of the RemoteControl class which is used for emulating button presses.")>
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
        <DisplayName("Network shares")>
        <Description("Collection of network shares exposed by the device's SMB server.")>
        Public ReadOnly Property NetworkShares As ReadOnlyCollection(Of LocalStorage)
            Get
                If _shares Is Nothing Then
                    _shares = LocalStorage.FromHost(Me)
                End If
                Return _shares.AsReadOnly
            End Get
        End Property

        ''' <summary>
        ''' Gets the player's product ID (using a Telnet connection).
        ''' </summary>
        <DisplayName("Product ID")>
        <Description("The device's product ID.")>
        Public ReadOnly Property ProductID As String
            Get
                If _systemInfo Is Nothing Then
                    _systemInfo = SystemInformation.FromHost(Me)
                End If
                Return SystemInfo.ProductId
            End Get
        End Property

        ''' <summary>
        ''' Gets the device's serial number (using a Telnet connection).
        ''' </summary>
        <DisplayName("Serial number")>
        <Description("The device's serial number.")>
        Public ReadOnly Property SerialNumber As String
            Get
                If _systemInfo Is Nothing Then
                    _systemInfo = SystemInformation.FromHost(Me)
                End If
                Return SystemInfo.Serial
            End Get
        End Property

        ''' <summary>
        ''' Gets the installed firmware version string (using a Telnet connection).
        ''' </summary>
        <DisplayName("Installed firmware")>
        <Description("The installed firmware version.")>
        Public ReadOnly Property FirmwareVersion As String
            Get
                If _systemInfo Is Nothing Then
                    _systemInfo = SystemInformation.FromHost(Me)
                End If
                Return SystemInfo.FirmareVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the connection is still active or sets it to on/off.
        ''' </summary>
        ''' <returns>True if there is a connection; otherwise false.</returns>
        <DisplayName("Connected")>
        <Description("Displays whether the application is still connected. You may change this if you wish to reconnect or disconnect.")>
        Public Property Connected As Boolean
            Get
                Return _connected
            End Get
            Set(value As Boolean)
                If _connected <> value Then
                    If value.IsTrue Then
                        Reconnect()
                    Else
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
                    If value.IsTrue Then
                        CommandStatusUpdate = "connected"
                    Else
                        Interval = 0
                        _telnetEnabled = Nothing
                        CommandStatusUpdate = "disconnected"
                    End If
                    RaisePropertyChanged("CommandStatus")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback time left. This value is calculated based on <see cref="PlaybackDuration"/> and <see cref="PlaybackPosition"/>.
        ''' </summary>
        <DisplayName("Playback time left")>
        <Description("The duration until the end of the current playback.")>
        Public ReadOnly Property PlaybackTimeLeft As TimeSpan?
            Get
                If PlaybackDuration.HasValue Then
                    Return PlaybackDuration - PlaybackPosition
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of seconds before a command returns with a timeout.
        ''' </summary>
        ''' <value>A positive value bigger than 1</value>
        ''' <remarks>The default value is 20 seconds.</remarks>
        <DisplayName("Command timeout")>
        <Description("The amount of seconds before a command returns with a timeout. Command execution is not aborted when a timeout is reached.")>
        Public Property Timeout As UInteger
            Get
                If _timeout = Nothing Then
                    _timeout = 20
                End If
                Return _timeout
            End Get
            Set(value As UInteger)
                If value > 0 And _timeout <> value Then
                    _timeout = value
                    RaisePropertyChanged("Timeout")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the interval (in miliseconds) between status updates.
        ''' </summary>
        ''' <remarks>Setting this property to 0 will disable automatic status updates.</remarks>
        <DisplayName("Update interval")>
        <Description("The amount of miliseconds between status updates.")>
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
        ''' Gets a list of available firmwares. The <see cref="ProductID" /> property must be set to populate the collection.
        ''' </summary>
        <DisplayName("Available firmwares")>
        <Description("The collection of available firmware versions for the specified product ID.")>
        Public ReadOnly Property AvailableFirmwares As ReadOnlyCollection(Of FirmwareProperties)
            Get
                If _firmwares Is Nothing Then
                    If ProductID.IsNotNullOrEmpty Then
                        Try
                            _firmwares = FirmwareProperties.GetAvailableFirmwaresAsync(ProductID).Result
                        Catch ex As Exception
                            _firmwares = New List(Of FirmwareProperties)
                        End Try
                    Else
                        _firmwares = New List(Of FirmwareProperties)
                    End If
                End If

                Return _firmwares.AsReadOnly
            End Get
        End Property

        ''' <summary>
        ''' Gets a preconfigured Telnet client.
        ''' </summary>
        Public ReadOnly Property TelnetClient As TelnetClient
            Get
                Try
                    If Not TelnetEnabled.HasValue Then
                        _telnetClient = New TelnetClient(Address)
                        _telnetClient.login("root")
                        _telnetEnabled = True
                    End If
                Catch ex As SocketException
                    _telnetEnabled = False
                End Try
                Return _telnetClient
            End Get
        End Property


        ''' <summary>
        ''' Gets the date and time when the device booted up.
        ''' </summary>
        <DisplayName("Up since")>
        <Description("The date and time since when the device is up")>
        Public ReadOnly Property BootTime As Date
            Get
                Return SystemInfo.BootTime
            End Get
        End Property

        ''' <summary>
        ''' Gets the device's uptime.
        ''' </summary>
        <DisplayName("Uptime")>
        <Description("The amount of time that the device has been running for.")>
        Public ReadOnly Property Uptime As TimeSpan
            Get
                If Connected AndAlso BootTime <> Nothing Then
                    Return Now.Subtract(BootTime)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets whether telnetd is turned on.
        ''' </summary>
        <DisplayName("Telnet access")>
        <Description("Displays whether telnetd is running and accessible.")>
        Public ReadOnly Property TelnetEnabled As Boolean?
            Get
                Return _telnetEnabled
            End Get
        End Property

        Public ReadOnly Property SystemInfo As SystemInformation
            Get
                If _systemInfo Is Nothing AndAlso TelnetEnabled Then
                    _systemInfo = SystemInformation.FromHost(Me)
                End If
                Return _systemInfo
            End Get
        End Property

#End Region ' App Properties

#Region "App methods"

        ''' <summary>
        ''' Scans the network and retrieves a collection of all Dune devices.
        ''' </summary>
        ''' <param name="ignoreNonSigmaNic">Specifies whether to ignore devices whose network card is not made by Sigma Designs.</param>
        Public Shared Function Scan(ByVal ignoreNonSigmaNic As Boolean) As Collection(Of Dune)
            Dim dunes As New Collection(Of Dune)

            For Each device As Networking.NetworkDevice In Networking.NetworkDevice.Scan
                If device.IsDune(ignoreNonSigmaNic) Then
                    dunes.Add(New Dune(device.Host))
                End If
            Next

            Return dunes
        End Function

        ''' <summary>
        ''' Attempts to initiate a connection with the target.
        ''' </summary>
        Private Sub Connect(ByVal target As IPEndPoint)
            Try
                If TargetIsValid(_endpoint) Then
                    Interval = 900
                    ConnectedUpdate = True
                Else
                    Throw New ArgumentException("The target host is not a Dune device!")
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
                Dim commandResult As CommandResult = GetStatus() ' can throw a WebException

                If commandResult.ProtocolVersion = Byte.MaxValue Then ' target is running a webserver but is not a Dune device
                    Return False
                End If
            Catch tcpClientException As SocketException ' if the connection failed
                Return False
            Catch serviceException As WebException ' if the API didn't properly respond (errors 404, 401, 403 etc.)
                Return False
            End Try

            Return True
        End Function

        ''' <summary>
        ''' Updates the status properties.
        ''' </summary>
        <DebuggerStepThrough()>
        Private Sub _updateTimer_elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
            If Connected Then
                Try
                    GetStatus()
                Catch ex As WebException
                    CommandStatusUpdate = "connection error"
                End Try
            Else
                CommandStatusUpdate = "lost connection"
            End If
        End Sub

        ''' <summary>
        ''' Executes commands and processes results.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <exception cref="CommandException">: The device could not successfully  complete the command.</exception>
        Public Function ProcessCommand(ByVal command As Command) As CommandResult
            If Not command.Timeout.HasValue Then
                command.Timeout = Me.Timeout
            End If

            Dim result As CommandResult = command.GetResult

            If result IsNot Nothing Then
                UpdateValues(result)

                If result.CommandError IsNot Nothing Then
                    Throw result.CommandError
                End If
            End If

            Return result
        End Function

        ''' <summary>
        ''' Executes commands and processes results asynchronously.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <exception cref="CommandException">: The device could not successfully  complete the command.</exception>
        Public Function ProcessCommandAsync(ByVal command As Command) As Task(Of CommandResult)
            Dim resultTask As Task(Of CommandResult)

            resultTask = Task(Of CommandResult).Factory.StartNew(Function()
                                                                     Dim result As CommandResult = command.GetResult
                                                                     UpdateValues(result)
                                                                     If result.CommandError IsNot Nothing Then
                                                                         Throw result.CommandError
                                                                     End If
                                                                     Return result
                                                                 End Function)

            Return resultTask
        End Function

        ''' <summary>
        ''' Updates the player status properties.
        ''' </summary>
        Private Sub UpdateValues(ByVal commandResult As CommandResult)
            If commandResult.CommandStatus.IsNotNullOrEmpty Then
                CommandStatusUpdate = commandResult.CommandStatus
            End If
            If commandResult.CommandError IsNot Nothing Then
                CommandErrorUpdate = commandResult.CommandError
            End If
            ProtocolVersionUpdate = commandResult.ProtocolVersion
            PlayerStateUpdate = commandResult.PlayerState
            PlaybackSpeedUpdate = commandResult.PlaybackSpeed
            PlaybackDurationUpdate = commandResult.PlaybackDuration
            PlaybackPositionUpdate = commandResult.PlaybackPosition
            PlaybackIsBufferingUpdate = commandResult.PlaybackIsBuffering
            PlaybackDvdMenuUpdate = commandResult.PlaybackDvdMenu
            If ProtocolVersion >= 2 Then
                VideoEnabledUpdate = commandResult.VideoEnabled
                VideoZoomUpdate = commandResult.VideoZoom
                VideoFullscreenUpdate = commandResult.VideoFullscreen
                VideoXUpdate = commandResult.VideoX
                VideoYUpdate = commandResult.VideoY
                VideoWidthUpdate = commandResult.VideoWidth
                VideoHeightUpdate = commandResult.VideoHeight
                VideoTotalDisplayWidthUpdate = commandResult.VideoTotalDisplayWidth
                VideoTotalDisplayHeightUpdate = commandResult.VideoTotalDisplayHeight
                PlaybackVolumeUpdate = commandResult.PlaybackVolume
                PlaybackMuteUpdate = commandResult.PlaybackMute
                AudioTracksUpdate = commandResult.AudioTracks
                AudioTrackUpdate = commandResult.AudioTrack
            End If
        End Sub

        ''' <summary>
        ''' Try to reconnect.
        ''' </summary>
        Public Sub Reconnect()
            If Not Connected Then
                CommandStatusUpdate = "reconnecting..."
                Try
                    If TargetIsValid(_endpoint) Then
                        SystemInfo.Refresh()

                        Interval = 990
                        ConnectedUpdate = True
                    End If
                Catch ex As SocketException
                    CommandStatusUpdate = "failed"
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Helper method helps raising PropertyChanged events.
        ''' </summary>
        ''' <param name="propertyName">The name of the property that changed.</param>
        Private Sub RaisePropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' Clears the command status.
        ''' </summary>
        Public Sub ClearStatus()
            CommandStatusUpdate = String.Empty
        End Sub

        ''' <summary>
        ''' Closes the disc tray (if any) using a telnet connection.
        ''' </summary>
        Public Sub CloseDiscTray()
            Try
                TelnetClient.ExecuteCommand("eject -t /dev/sr0")
            Catch ex As Exception
                Console.WriteLine("Telnet error: " + ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Toggles between open/close disc tray (if any) using a telnet connection.
        ''' </summary>
        Public Sub ToggleDiscTray()
            Try
                TelnetClient.ExecuteCommand("eject -T /dev/sr0")
            Catch ex As Exception
                Console.WriteLine("Telnet error: " + ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Sends a reboot command using telnet.
        ''' </summary>
        Public Sub Reboot()
            Try
                TelnetClient.ExecuteCommand("reboot")
            Catch ex As Exception
                Console.WriteLine("Telnet error: " + ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Sends a poweroff command using telnet.
        ''' </summary>
        Public Sub PowerOff()
            Try
                TelnetClient.ExecuteCommand("poweroff")
            Catch ex As Exception
                Console.WriteLine("Telnet error: " + ex.Message)
            End Try
        End Sub
#End Region ' App methods

#Region "Properties v1"

        ''' <summary>
        ''' Gets the status of the last command.
        ''' </summary>
        <DisplayName("Command status")>
        <Description("The status of the last command")>
        Public ReadOnly Property CommandStatus As String Implements IProtocolVersion1.CommandStatus
            Get
                Return _commandStatus
            End Get
        End Property

        ''' <summary>
        ''' Sets the status of the last command.
        ''' </summary>
        Private WriteOnly Property CommandStatusUpdate As String Implements IProtocolVersion1.CommandStatusUpdate
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
        <DisplayName("Playback duration")>
        <Description("The current playback duration.")>
        Public ReadOnly Property PlaybackDuration As System.TimeSpan? Implements IProtocolVersion1.PlaybackDuration
            Get
                Return _playbackDuration
            End Get
        End Property

        ''' <summary>
        ''' Sets the playback duration.
        ''' </summary>
        Private WriteOnly Property PlaybackDurationUpdate As System.TimeSpan? Implements IProtocolVersion1.PlaybackDurationUpdate
            Set(value As System.TimeSpan?)
                If Not Nullable.Equals(PlaybackDuration, value) Then
                    _playbackDuration = value
                    RaisePropertyChanged("PlaybackDuration")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        ''' <exception cref="CommandException">: Command execution failed.</exception>
        <DisplayName("Playback position")>
        <Description("The current playback position.")>
        Public Property PlaybackPosition As System.TimeSpan? Implements IProtocolVersion1.PlaybackPosition
            Get
                Return _playbackPosition
            End Get
            Set(value As System.TimeSpan?)
                If Not value.HasValue Then ' This setter must always have a value, so default to 0.
                    value = New TimeSpan(0)
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.Position = value
                command.Timeout = Timeout


                Dim processTask As Task = ProcessCommandAsync(command)

                Try
                    processTask.Wait()
                Catch ex As AggregateException
                    Throw ex.InnerException
                End Try
            End Set
        End Property

        ''' <summary>
        ''' Sets the playback position.
        ''' </summary>
        Private WriteOnly Property PlaybackPositionUpdate As System.TimeSpan? Implements IProtocolVersion1.PlaybackPositionUpdate
            Set(value As System.TimeSpan?)
                If Not Nullable.Equals(PlaybackPosition, value) Then
                    _playbackPosition = value
                    RaisePropertyChanged("PlaybackPosition")
                    RaisePropertyChanged("PlaybackTimeLeft")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        ''' <exception cref="CommandException">: Command execution failed.</exception>
        <DisplayName("Playback speed")>
        <Description("The playback speed.")>
        Public Property PlaybackSpeed As Constants.PlaybackSpeedSettings Implements IProtocolVersion1.PlaybackSpeed
            Get
                Return _playbackSpeed
            End Get
            Set(value As Constants.PlaybackSpeedSettings)
                Dim command As New SetPlaybackStateCommand(Me)
                command.Speed = value
                command.Timeout = Timeout


                Dim processTask As Task = ProcessCommandAsync(command)

                Try
                    processTask.Wait()
                Catch ex As AggregateException
                    Throw ex.InnerException
                End Try
            End Set
        End Property

        ''' <summary>
        ''' Sets the playback speed.
        ''' </summary>
        Private WriteOnly Property PlaybackSpeedUpdate As Short Implements IProtocolVersion1.PlaybackSpeedUpdate
            Set(value As Short)
                If PlaybackSpeed <> value Then
                    _playbackSpeed = DirectCast(value, Constants.PlaybackSpeedSettings)
                    RaisePropertyChanged("PlaybackSpeed")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the player state.
        ''' </summary>
        <DisplayName("Player state")>
        <Description("The player state. Accepted values are ""main_screen"", ""black_screen"" or ""standby"".")>
        Public Property PlayerState As String Implements IProtocolVersion1.PlayerState
            Get
                Return _playerState
            End Get
            Set(value As String)
                Dim command As New SetPlayerStateCommand(Me, value)
                command.Timeout = Timeout

                Dim processTask As Task = ProcessCommandAsync(command)

                Try
                    processTask.Wait()
                Catch ex As AggregateException
                    Throw ex.InnerException
                End Try
            End Set
        End Property

        ''' <summary>
        ''' Sets the player state.
        ''' </summary>
        Private WriteOnly Property PlayerStateUpdate As String Implements IProtocolVersion1.PlayerStateUpdate
            Set(value As String)
                If PlayerState <> value Then
                    _playerState = value
                    RaisePropertyChanged("PlayerState")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the protocol version number.
        ''' </summary>
        <DisplayName("Protocol version")>
        <Description("The API version. The latest supported version number is 2.")>
        Public ReadOnly Property ProtocolVersion As Byte Implements IProtocolVersion1.ProtocolVersion
            Get
                If _protocolVersion = 0 Then
                    _protocolVersion = Byte.MaxValue
                End If
                Return _protocolVersion
            End Get
        End Property

        ''' <summary>
        ''' Sets the protocol version number.
        ''' </summary>
        Private WriteOnly Property ProtocolVersionUpdate As Byte Implements IProtocolVersion1.ProtocolVersionUpdate
            Set(value As Byte)
                If ProtocolVersion <> value Then
                    _protocolVersion = value
                    RaisePropertyChanged("ProtocolVersion")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the last command error.
        ''' </summary>
        <DisplayName("Command error")>
        <Description("The last command error.")>
        Public ReadOnly Property CommandError As CommandException Implements IProtocolVersion1.CommandError
            Get
                Return _commandError
            End Get
        End Property

        ''' <summary>
        ''' Sets the last command error.
        ''' </summary>
        Private WriteOnly Property CommandErrorUpdate As CommandException Implements IProtocolVersion1.CommandErrorUpdate
            Set(value As CommandException)
                If value IsNot Nothing AndAlso _commandError IsNot value Then
                    _commandError = value
                    RaisePropertyChanged("Error")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets whether a DVD menu is shown.
        ''' </summary>
        <DisplayName("DVD menu")>
        <Description("Displays whether the player is showing a DVD menu.")>
        Public ReadOnly Property PlaybackDvdMenu As Boolean Implements IProtocolVersion1.PlaybackDvdMenu
            Get
                Return _playbackDvdMenu
            End Get
        End Property

        ''' <summary>
        ''' Sets whether a DVD menu is shown.
        ''' </summary>
        Private WriteOnly Property PlaybackDvdMenuUpdate As Boolean Implements IProtocolVersion1.PlaybackDvdMenuUpdate
            Set(value As Boolean)
                If Not Nullable.Equals(PlaybackDvdMenu, value) Then
                    _playbackDvdMenu = value
                    RaisePropertyChanged("PlaybackDvdMenu")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        <DisplayName("Playback buffering")>
        <Description("Displays whether the playback is buffering.")>
        Public ReadOnly Property PlaybackIsBuffering As Boolean Implements IProtocolVersion1.PlaybackIsBuffering
            Get
                Return _playbackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Sets whether the playback is buffering.
        ''' </summary>
        Private WriteOnly Property PlaybackIsBufferingUpdate As Boolean Implements IProtocolVersion1.PlaybackIsBufferingUpdate
            Set(value As Boolean)
                If Not Nullable.Equals(PlaybackIsBuffering, value) Then
                    _playbackIsBuffering = value
                    RaisePropertyChanged("PlaybackIsBuffering")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets whether to show a black screen during playback.
        ''' </summary>
        ''' <exception cref="CommandException">: Command execution failed.</exception>
        <DisplayName("Black screen")>
        <Description("Disables both the OSD and video output.")>
        Public Property BlackScreen As Boolean? Implements IProtocolVersion1.BlackScreen
            Get
                Return Nothing
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then value = False

                Dim command As New SetPlaybackStateCommand(Me)
                command.BlackScreen = value
                command.Timeout = Timeout

                Dim processTask As Task = ProcessCommandAsync(command)

                Try
                    processTask.Wait()
                Catch ex As AggregateException
                    Throw ex.InnerException
                End Try
            End Set

        End Property

        ''' <summary>
        ''' Sets whether to hide the OSD during playback.
        ''' </summary>
        ''' <exception cref="CommandException">: Command execution failed.</exception>
        <DisplayName("Hide on-screen display")>
        <Description("Disables all overlay menu items such as the pop-up menu.")>
        Public Property HideOnScreenDisplay As Boolean? Implements IProtocolVersion1.HideOnScreenDisplay
            Get
                Return Nothing
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then value = False

                Dim command As New SetPlaybackStateCommand(Me)
                command.HideOnScreenDisplay = value
                command.Timeout = Timeout

                Dim processTask As Task = ProcessCommandAsync(command)

                Try
                    processTask.Wait()
                Catch ex As AggregateException
                    Throw ex.InnerException
                End Try
            End Set
        End Property

#End Region ' Properties v1

#Region "Methods v1"

        ''' <summary>
        ''' Navigates to the previous keyframe during DVD or MKV playback.
        ''' </summary>
        Public Function GoToPreviousKeyframe() As CommandResult
            Dim command As New SetKeyframeCommand(Me, Constants.SetKeyframeSettings.Previous)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Navigates to the next keyframe during DVD or MKV playback.
        ''' </summary>
        Public Function GoToNextKeyframe() As CommandResult
            Dim command As New SetKeyframeCommand(Me, Constants.SetKeyframeSettings.Next)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Sets the device to standby mode.
        ''' </summary>
        Public Function SetToStandby() As CommandResult
            Dim command As New SetPlayerStateCommand(Me, Constants.Commands.Standby)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Sets the device to navigator mode.
        ''' </summary>
        Public Function SetToMainScreen() As CommandResult
            Dim command As New SetPlayerStateCommand(Me, Constants.Commands.MainScreen)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Sets the device to black screen mode.
        ''' </summary>
        Public Function SetToBlackScreen() As CommandResult
            Dim command As New SetPlayerStateCommand(Me, Constants.Commands.BlackScreen)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Navigates through menus.
        ''' </summary>
        ''' <param name="action">The navigation action. Possible values are enumerated in <see cref="Constants.NavigationActions"/>.</param>
        ''' <remarks>
        ''' The actual command is context sensitive.
        ''' If the device is in dvd playback, it will be a 'dvd_navigation' command.
        ''' Similarly, a 'bluray_navigation' command is sent when the device is in blu-ray playback mode.
        ''' In all other cases, a remote control button is emulated ('ir_code' command).
        ''' </remarks>
        Public Function NavigateMenu(ByVal action As String) As CommandResult
            Dim command As New NavigationCommand(Me, action)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Gets the player status.
        ''' </summary>
        Public Function GetStatus() As CommandResult
            Dim command As New GetStatusCommand(Me)
            Return ProcessCommand(command)
        End Function

#End Region ' Methods v1

#Region "Properties v2"

        ''' <summary>
        ''' Gets or sets the current language track number.
        ''' </summary>
        ''' <exception cref="CommandException">: Command execution failed.</exception>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.</remarks>
        <DisplayName("Audio track")>
        <Description("The language track number that is currently playing. Not to be confused with the track number in a playlist.")>
        Public Property AudioTrack As Byte? Implements IProtocolVersion2.AudioTrack
            Get
                Return _audioTrack
            End Get
            Set(value As Byte?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.AudioTrack = value
                    command.Timeout = Timeout


                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the current language track number
        ''' </summary>
        Private WriteOnly Property AudioTrackUpdate As Byte? Implements IProtocolVersion2.AudioTrackUpdate
            Set(value As Byte?)
                If Not Nullable.Equals(AudioTrack, value) Then
                    _audioTrack = value
                    RaisePropertyChanged("AudioTrack")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the video output is fullscreen.
        ''' </summary>
        <DisplayName("Fullscreen")>
        <Description("Displays whether the video is in full screen or if custom video output settings are applied. Note that custom settings can mimic fullscreen.")>
        Public Property VideoFullscreen As Boolean? Implements IProtocolVersion2.VideoFullscreen
            Get
                Return _videoFullscreen
            End Get
            Set(value As Boolean?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then
                    Dim command As New SetVideoOutputCommand(Me, value.Value)
                    command.Timeout = Timeout


                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets whether the video output is fullscreen.
        ''' </summary>
        Private WriteOnly Property VideoFullscreenUpdate As Boolean? Implements IProtocolVersion2.VideoFullscreenUpdate
            Set(value As Boolean?)
                If Not Nullable.Equals(VideoFullscreen, value) Then
                    _videoFullscreen = value
                    RaisePropertyChanged("VideoFullscreen")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the list of available audio tracks for the current playback.
        ''' </summary>
        ''' <returns>An instance of a <see cref="SortedDictionary(Of Byte, CultureInfo)" /> object that represents the list of available audio tracks.</returns>
        <DisplayName("Audio tracks")>
        <Description("The collection of language tracks in the current playback. Not to be confused with the amount of tracks in a playlist.")>
        Public ReadOnly Property AudioTracks As SortedDictionary(Of Byte, CultureInfo) Implements IProtocolVersion2.AudioTracks
            Get
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Sets the list of available audio tracks for the current playback.
        ''' </summary>
        Private WriteOnly Property AudioTracksUpdate As SortedDictionary(Of Byte, CultureInfo) Implements IProtocolVersion2.AudioTracksUpdate
            Set(value As SortedDictionary(Of Byte, CultureInfo))
                If AudioTracks Is Nothing OrElse Not Enumerable.SequenceEqual(AudioTracks, value) Then
                    _audioTracks = value
                    RaisePropertyChanged("AudioTracks")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the mute status for the current playback.
        ''' </summary>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.</remarks>
        <DisplayName("Playback mute")>
        <Description("Enables or disables audio output.")>
        Public Property PlaybackMute As Boolean? Implements IProtocolVersion2.PlaybackMute
            Get
                Return _playbackMute
            End Get
            Set(value As Boolean?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.Mute = value
                    command.Timeout = Timeout


                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the mute status for the current playback.
        ''' </summary>
        Private WriteOnly Property PlaybackMuteUpdate As Boolean? Implements IProtocolVersion2.PlaybackMuteUpdate
            Set(value As Boolean?)
                If Not Nullable.Equals(PlaybackMute, value) Then
                    _playbackMute = value
                    RaisePropertyChanged("PlaybackMute")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the video output is enabled.
        ''' </summary>
        ''' <remarks>Setting this property does nothing if the protocol version is 1.</remarks>
        <DisplayName("Video enabled")>
        <Description("Disabling the video does not hide the on-screen display.")>
        Public Property VideoEnabled As Boolean? Implements IProtocolVersion2.VideoEnabled
            Get
                Return _videoEnabled
            End Get
            Set(value As Boolean?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.VideoEnabled = value
                    command.Timeout = Timeout


                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets whether the video output is enabled.
        ''' </summary>
        Private WriteOnly Property VideoEnabledUpdate As Boolean? Implements IProtocolVersion2.VideoEnabledUpdate
            Set(value As Boolean?)
                If Not Nullable.Equals(VideoEnabled, value) Then
                    _videoEnabled = value
                    RaisePropertyChanged("VideoEnabled")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video display height.
        ''' </summary>
        <DisplayName("Video height")>
        <Description("The video output's height in pixels.")>
        Public Property VideoHeight As UShort? Implements IProtocolVersion2.VideoHeight
            Get
                If Not _videoHeight.HasValue Then
                    _videoHeight = VideoTotalDisplayHeight
                End If
                Return _videoHeight
            End Get
            Set(value As UShort?)
                If value.HasValue And ProtocolVersion >= 2 Then
                    Dim command As SetVideoOutputCommand

                    If value >= VideoTotalDisplayHeight Then
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, 0, VideoWidth, VideoTotalDisplayHeight)
                    ElseIf value + VideoY > VideoTotalDisplayHeight Then
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, (VideoTotalDisplayHeight - value), VideoWidth, value)
                    Else
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, VideoY, VideoWidth, value)
                    End If

                    command.Timeout = Timeout


                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video display height.
        ''' </summary>
        Private WriteOnly Property VideoHeightUpdate As UShort? Implements IProtocolVersion2.VideoHeightUpdate
            Set(value As UShort?)
                If Not Nullable.Equals(VideoHeight, value) Then
                    _videoHeight = value
                    RaisePropertyChanged("VideoHeight")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total video display height.
        ''' </summary>
        <DisplayName("Display height")>
        <Description("The total amount of pixels in the display height.")>
        Public ReadOnly Property VideoTotalDisplayHeight As UShort? Implements IProtocolVersion2.VideoTotalDisplayHeight
            Get
                Return _videoTotalDisplayHeight
            End Get
        End Property

        ''' <summary>
        ''' Sets the total video display height.
        ''' </summary>
        Private WriteOnly Property VideoTotalDisplayHeightUpdate As UShort? Implements IProtocolVersion2.VideoTotalDisplayHeightUpdate
            Set(value As UShort?)
                If Not Nullable.Equals(VideoTotalDisplayHeight, value) Then
                    _videoTotalDisplayHeight = value
                    RaisePropertyChanged("VideoTotalDisplayHeight")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total video display width.
        ''' </summary>
        <DisplayName("Display width")>
        <Description("The total amount of pixels in the display width.")>
        Public ReadOnly Property VideoTotalDisplayWidth As UShort? Implements IProtocolVersion2.VideoTotalDisplayWidth
            Get
                Return _videoTotalDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Sets the total video display width.
        ''' </summary>
        Private WriteOnly Property VideoTotalDisplayWidthUpdate As UShort? Implements IProtocolVersion2.VideoTotalDisplayWidthUpdate
            Set(value As UShort?)
                If Not Nullable.Equals(VideoTotalDisplayWidth, value) Then
                    _videoTotalDisplayWidth = value
                    RaisePropertyChanged("VideoTotalDisplayWidth")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video display width.
        ''' </summary>
        <DisplayName("Video width")>
        <Description("The video output's width in pixels.")>
        Public Property VideoWidth As UShort? Implements IProtocolVersion2.VideoWidth
            Get
                If Not _videoWidth.HasValue Then
                    _videoWidth = VideoTotalDisplayWidth
                End If
                Return _videoWidth
            End Get
            Set(value As UShort?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then

                    Dim command As SetVideoOutputCommand

                    If value >= VideoTotalDisplayWidth Then
                        command = New SetVideoOutputCommand(Me, Nothing, 0, VideoY, VideoTotalDisplayWidth, VideoHeight)
                    ElseIf value + VideoX > VideoTotalDisplayWidth Then
                        command = New SetVideoOutputCommand(Me, Nothing, (VideoTotalDisplayWidth - value), VideoY, value, VideoHeight)
                    Else
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, VideoY, value, VideoHeight)
                    End If

                    command.Timeout = Timeout

                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video display width.
        ''' </summary>
        Private WriteOnly Property VideoWidthUpdate As UShort? Implements IProtocolVersion2.VideoWidthUpdate
            Set(value As UShort?)
                If Not Nullable.Equals(VideoWidth, value) Then
                    _videoWidth = value
                    RaisePropertyChanged("VideoWidth")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video's horizontal position.
        ''' </summary>
        <DisplayName("Video horizontal position")>
        <Description("The amount of pixels between the left border of the display and the video output.")>
        Public Property VideoX As UShort? Implements IProtocolVersion2.VideoX
            Get
                If Not _videoX.HasValue Then
                    _videoX = 0
                End If
                Return _videoX
            End Get
            Set(value As UShort?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then
                    Dim command As SetVideoOutputCommand

                    If value >= VideoTotalDisplayWidth Then
                        command = New SetVideoOutputCommand(Me, Nothing, VideoTotalDisplayWidth, VideoY, 0, VideoHeight)
                    ElseIf value + VideoWidth > VideoTotalDisplayWidth Then
                        command = New SetVideoOutputCommand(Me, Nothing, value, VideoY, (VideoTotalDisplayWidth - value), VideoHeight)
                    Else
                        command = New SetVideoOutputCommand(Me, Nothing, value, VideoY, VideoWidth, VideoHeight)
                    End If

                    command.Timeout = Timeout

                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video's horizontal position.
        ''' </summary>
        Private WriteOnly Property VideoXUpdate As UShort? Implements IProtocolVersion2.VideoXUpdate
            Set(value As UShort?)
                If Not Nullable.Equals(VideoX, value) Then
                    _videoX = value
                    RaisePropertyChanged("VideoX")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the video's vertical position.
        ''' </summary>
        <DisplayName("Video vertical position")>
        <Description("The amount of pixels between the top border of the display and the video output.")>
        Public Property VideoY As UShort? Implements IProtocolVersion2.VideoY
            Get
                If Not _videoY.HasValue Then
                    _videoY = 0
                End If
                Return _videoY
            End Get
            Set(value As UShort?)
                If value.HasValue AndAlso ProtocolVersion >= 2 Then
                    Dim command As SetVideoOutputCommand

                    If value >= VideoTotalDisplayHeight Then
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, VideoTotalDisplayHeight, VideoWidth, 0)
                    ElseIf value + VideoHeight > VideoTotalDisplayHeight Then
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, value, VideoWidth, (VideoTotalDisplayHeight - value))
                    Else
                        command = New SetVideoOutputCommand(Me, Nothing, VideoX, value, VideoWidth, VideoHeight)
                    End If

                    command.Timeout = Timeout

                    Dim processTask As Task = ProcessCommandAsync(command)

                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the video's vertical position.
        ''' </summary>
        Private WriteOnly Property VideoYUpdate As UShort? Implements IProtocolVersion2.VideoYUpdate
            Set(value As UShort?)
                If Not Nullable.Equals(VideoY, value) Then
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
        <DisplayName("Playback volume")>
        <Description("The playback volume percentage.")>
        Public Property PlaybackVolume As Byte? Implements IProtocolVersion2.PlaybackVolume
            Get
                Return _playbackVolume
            End Get
            Set(value As Byte?)
                ' Let's do a little validation first.
                If Not value.HasValue Then
                    value = 0
                ElseIf value > 100 Then
                    value = 100
                End If
                ' Now we're ready for the real work.
                If ProtocolVersion >= 2 Then
                    Dim command As New SetPlaybackStateCommand(Me)
                    command.Volume = value
                    command.Timeout = Timeout

                    Dim processTask As Task = ProcessCommandAsync(command)
                    Try
                        processTask.Wait()
                    Catch ex As AggregateException
                        Throw ex.InnerException
                    End Try

                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the playback volume.
        ''' </summary>
        Private WriteOnly Property PlaybackVolumeUpdate As Byte? Implements IProtocolVersion2.PlaybackVolumeUpdate
            Set(value As Byte?)
                If Not Nullable.Equals(PlaybackVolume, value) Then
                    _playbackVolume = value
                    RaisePropertyChanged("PlaybackVolume")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the video zoom.
        ''' </summary>
        <DisplayName("Video zoom")>
        <Description("The video output's zoom mode. Accepted values are ""normal"", ""enlarge"", ""make_wider"", ""fill_screen"", ""full_fill_screen"", ""make_taller"" or ""cut_edges"".")>
        Public Property VideoZoom As String Implements IProtocolVersion2.VideoZoom
            Get
                Return _videoZoom
            End Get
            Set(value As String)
                Dim fullscreen As Boolean = CBool(IIf(VideoFullscreen.HasValue, VideoFullscreen, False))
                Dim command As New SetVideoOutputCommand(Me, fullscreen)
                command.Zoom = value
                command.Timeout = Timeout

                Dim processTask As Task = ProcessCommandAsync(command)

                Try
                    processTask.Wait()
                Catch ex As AggregateException
                    Throw ex.InnerException
                End Try
            End Set
        End Property

        ''' <summary>
        ''' Sets the video zoom
        ''' </summary>
        Private WriteOnly Property VideoZoomUpdate As String Implements IProtocolVersion2.VideoZoomUpdate
            Set(value As String)
                If Not VideoZoom = value Then
                    _videoZoom = value
                    RaisePropertyChanged("VideoZoom")
                End If
            End Set
        End Property

#End Region 'Properties v2

        ''' <summary>
        ''' INotifyPropertyChanged implementation.
        ''' </summary>
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    End Class

End Namespace
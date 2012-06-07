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
Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities

    ''' <summary>
    ''' This class represents a Dune player on the network and exposes several methods and properties to interact with it.
    ''' </summary>
    Public Class Dune
        Implements INotifyPropertyChanged

#Region "Private Fields"

        ' Connection details
        Private _hostentry As IPHostEntry
        Private _endpoint As IPEndPoint
        Private _timeout As UInteger
        Private _updateTimer As Timer ' Responsible for status updates


        ' Custom fields
        Private _remoteControl As RemoteControl
        Private _shares As List(Of LocalStorage)
        Private _connected As Boolean
        Private _networkAdapterInfo As NetworkAdapterInfo
        Private _firmwares As List(Of FirmwareProperties)
        Private _telnetClient As TelnetClient
        Private _telnetEnabled As Boolean?
        Private _systemInfo As SystemInformation
        Private _status As CommandResult

#End Region ' Private Fields

#Region "Constructors"

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The IP address or hostname of the target device.</param>
        ''' <exception cref="System.Net.Sockets.SocketException">: An error is encountered when resolving address.</exception>
        Public Sub New(address As String)
            Me.New(Dns.GetHostEntry(address), 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The IP address of the target device.</param>
        ''' <exception cref="System.Net.Sockets.SocketException">: An error is encountered when resolving address.</exception>
        Public Sub New(address As IPAddress)
            Me.New(Dns.GetHostEntry(address), 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the default port (80).
        ''' </summary>
        ''' <param name="address">The host entry of the target device.</param>
        Public Sub New(address As IPHostEntry)
            Me.New(address, 80)
        End Sub

        ''' <summary>
        ''' Gets a Dune object that represents a Dune device on the network at the specified address on the specified port number.
        ''' </summary>
        ''' <param name="address">The IP address or hostname of the target device.</param>
        ''' <exception cref="ArgumentOutOfRangeException">: port is less than System.Net.IPEndPoint.MinPort.-or- port is greater than System.Net.IPEndPoint.MaxPort.-or- address is less than 0 or greater than 0x00000000FFFFFFFF.</exception>       
        ''' <exception cref="SocketException">: An error is encountered when resolving address.</exception>
        ''' <exception cref="WebException">: An error is encountered when trying to query the API.</exception>
        Public Sub New(address As String, port As Integer)
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
        Public Sub New(address As IPAddress, port As Integer)
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
        Public Sub New(address As IPHostEntry, port As Integer)
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

                _updateTimer = New Timer
                _updateTimer.Interval = 990
                AddHandler _updateTimer.Elapsed, AddressOf _updateTimer_elapsed
                _updateTimer.Start()
            End If
        End Sub

#End Region ' Constructors

#Region "App Properties"

        ''' <summary>
        ''' Gets or sets a set of command results, representing the player status.
        ''' </summary>
        Private Property Status As CommandResult
            Get
                Return _status
            End Get
            Set(value As CommandResult)

                If Status Is Nothing Then
                    _status = value
                Else
                    Dim updates() As String = Status.GetDifferences(value)

                    _status = value
                    RaisePropertyChanged("Status")

                    For Each update As String In updates
                        RaisePropertyChanged(update)
                    Next

                End If
            End Set
        End Property

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
        ''' Gets or sets whether the connection is active.
        ''' </summary>
        <DisplayName("Connected")>
        <Description("Displays whether the application is still connected. You may change this if you wish to reconnect or disconnect.")>
        Public Property Connected As Boolean
            Get
                Return _connected
            End Get
            Set(value As Boolean)
                If _connected <> value Then
                    _connected = value
                    RaisePropertyChanged("Connected")

                    If SystemInfo IsNot Nothing Then
                        If value.IsTrue Then
                            SystemInfo.Refresh()
                        Else
                            SystemInfo.Clear()
                        End If
                    End If

                    If value.IsFalse Then
                        _telnetClient = Nothing
                        _telnetEnabled = Nothing
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the remaining playback time.
        ''' </summary>
        <DisplayName("Playback time remaining")>
        <Description("The remaining playback time.")>
        Public ReadOnly Property PlaybackTimeRemaining As TimeSpan?
            Get
                Return Status.PlaybackTimeRemaining
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
                If _updateTimer.Enabled Then
                    Return _updateTimer.Interval
                Else
                    Return 0
                End If
            End Get
            Set(value As Double)
                If value <= 0 Then
                    _updateTimer.Stop()
                Else
                    _updateTimer.Interval = value
                    If _updateTimer.Enabled.IsFalse Then
                        _updateTimer.Start()
                    End If
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
                    If Not TelnetEnabled.HasValue AndAlso Connected Then
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
        Public Shared Function Scan(ignoreNonSigmaNic As Boolean) As Collection(Of Dune)
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
        Private Sub Connect(target As IPEndPoint)
            If TargetIsValid(_endpoint) Then
                Connected = True
            Else
                Throw New ArgumentException("The target host is not a Dune device!")
            End If
        End Sub

        ''' <summary>
        ''' Verifies the target as a valid device.
        ''' </summary>
        Private Function TargetIsValid(target As IPEndPoint) As Boolean
            Dim commandResult As CommandResult = GetStatus() ' can throw a WebException
            Return commandResult.ProtocolVersion > 0
        End Function

        ''' <summary>
        ''' Updates the status properties.
        ''' </summary>
        <DebuggerStepThrough()>
        Private Sub _updateTimer_elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
            If Connected Then
                GetStatus()
            End If
        End Sub

        ''' <summary>
        ''' Executes commands and processes results.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <exception cref="CommandException">: command execution failed.</exception>
        Public Function ProcessCommand(command As Command) As CommandResult
            Dim result As CommandResult = Nothing

            Try ' to execute the command
                result = command.GetResult

                If result IsNot Nothing Then
                    Status = result

                    If result.CommandError IsNot Nothing Then
                        Throw result.CommandError
                    End If
                End If
            Catch ex As WebException ' handle faulty connection
                Connected = False
            End Try

            Return result
        End Function

        ''' <summary>
        ''' Executes commands and processes results asynchronously.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <exception cref="CommandException">: command execution failed.</exception>
        Public Function ProcessCommandAsync(command As Command) As Task(Of CommandResult)
            Dim resultTask As Task(Of CommandResult)

            resultTask = Task(Of CommandResult).Factory.StartNew(Function()
                                                                     Dim result As CommandResult = command.GetResult

                                                                     If result IsNot Nothing Then
                                                                         Status = result

                                                                         If result.CommandError IsNot Nothing Then
                                                                             Throw result.CommandError
                                                                         End If
                                                                     End If
                                                                     Return result
                                                                 End Function)

            Return resultTask
        End Function

        ''' <summary>
        ''' Helper method helps raising PropertyChanged events.
        ''' </summary>
        ''' <param name="propertyName">The name of the property that changed.</param>
        Private Sub RaisePropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
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
        ''' Gets the protocol version number.
        ''' </summary>
        <DisplayName("Protocol version")>
        <Description("The API version.")>
        Public ReadOnly Property ProtocolVersion As UShort
            Get
                Return Status.ProtocolVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets the status of the last command.
        ''' </summary>
        <DisplayName("Command status")>
        <Description("The status of the last command")>
        Public ReadOnly Property CommandStatus As String
            Get
                Return Status.CommandStatus
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        <DisplayName("Playback duration")>
        <Description("The current playback duration.")>
        Public ReadOnly Property PlaybackDuration As System.TimeSpan?
            Get
                Return Status.PlaybackDuration
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        <DisplayName("Playback position")>
        <Description("The current playback position.")>
        Public Property PlaybackPosition As System.TimeSpan?
            Get
                Return Status.PlaybackPosition
            End Get
            Set(value As System.TimeSpan?)
                If Not value.HasValue Then
                    value = TimeSpan.Zero
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.PlaybackPosition = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        <DisplayName("Playback speed")>
        <Description("The playback speed.")>
        Public Property PlaybackSpeed As Constants.PlaybackSpeedSettings?
            Get
                Return CType(Status.PlaybackSpeed, Constants.PlaybackSpeedSettings)
            End Get
            Set(value As Constants.PlaybackSpeedSettings?)
                If Not value.HasValue Then
                    value = Constants.PlaybackSpeedSettings.Normal
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.PlaybackSpeed = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the player state.
        ''' </summary>
        <DisplayName("Player state")>
        <Description("The player state. Accepted values are ""main_screen"", ""black_screen"" or ""standby"".")>
        Public Property PlayerState As String
            Get
                Return Status.PlayerState
            End Get
            Set(value As String)
                If value.IsNotNullOrWhiteSpace Then
                    value = Constants.Commands.MainScreen
                End If

                Dim command As New SetPlayerStateCommand(Me, value)

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the last command error.
        ''' </summary>
        <DisplayName("Command error")>
        <Description("The last command error.")>
        Public ReadOnly Property CommandError As CommandException
            Get
                Return Status.CommandError
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a DVD menu is shown.
        ''' </summary>
        <DisplayName("DVD menu")>
        <Description("Displays whether the player is showing a DVD menu.")>
        Public ReadOnly Property PlaybackDvdMenu As Boolean?
            Get
                Return Status.PlaybackDvdMenu
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        <DisplayName("Playback buffering")>
        <Description("Displays whether the playback is buffering.")>
        Public ReadOnly Property PlaybackIsBuffering As Boolean?
            Get
                Return Status.PlaybackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Sets whether to show a black screen during playback.
        ''' </summary>
        <DisplayName("Black screen")>
        <Description("Disables both the OSD and video output.")>
        Public Property BlackScreen As Boolean?
            Get
                Return Nothing
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then
                    value = False
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.BlackScreen = value

                ProcessCommand(command)
            End Set

        End Property

        ''' <summary>
        ''' Sets whether to hide the OSD during playback.
        ''' </summary>
        <DisplayName("Hide on-screen display")>
        <Description("Disables all overlay menu items such as the pop-up menu.")>
        Public Property HideOnScreenDisplay As Boolean?
            Get
                Return Nothing
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then
                    value = False
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.HideOnScreenDisplay = value

                ProcessCommand(command)
            End Set
        End Property

#End Region ' Properties v1

#Region "Properties v2"

        ''' <summary>
        ''' Gets or sets the playback volume
        ''' </summary>
        ''' <value>A positive integer value between 0 and 100.</value>
        ''' <remarks>A maximum volume of 150 can be set using the remote control by holding 'volume up' for longer than usual.</remarks>
        <DisplayName("Playback volume")>
        <Description("The playback volume percentage.")>
        Public Property PlaybackVolume As UShort?
            Get
                Return Status.PlaybackVolume
            End Get
            Set(value As UShort?)
                If Not value.HasValue Or value > 100 Then
                    value = 100
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.PlaybackVolume = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the mute status for the current playback.
        ''' </summary>
        <DisplayName("Playback mute")>
        <Description("Enables or disables audio output.")>
        Public Property PlaybackMute As Boolean?
            Get
                Return Status.PlaybackMute
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then
                    value = False
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.PlaybackMute = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current language track number.
        ''' </summary>
        <DisplayName("Audio track")>
        <Description("The language track number that is currently playing. Not to be confused with the track number in a playlist.")>
        Public Property AudioTrack As UShort?
            Get
                Return Status.AudioTrack
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.AudioTrack = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the list of available audio tracks for the current playback.
        ''' </summary>
        ''' <returns>An instance of a <see cref="SortedDictionary(Of UShort, CultureInfo)" /> object that represents the list of available audio tracks.</returns>
        <DisplayName("Audio tracks")>
        <Description("The collection of language tracks in the current playback. Not to be confused with the amount of tracks in a playlist.")>
        Public ReadOnly Property AudioTracks As SortedList(Of UShort, LanguageTrack)
            Get
                Return Status.AudioTracks
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether the video output is enabled.
        ''' </summary>
        <DisplayName("Video enabled")>
        <Description("Disabling the video does not hide the on-screen display.")>
        Public Property VideoEnabled As Boolean?
            Get
                Return Status.VideoEnabled
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then
                    value = True
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.VideoEnabled = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the total display height.
        ''' </summary>
        <DisplayName("Display height")>
        <Description("The total amount of pixels in the display height.")>
        Public ReadOnly Property OnScreenDisplayHeight As UShort?
            Get
                Return Status.OnScreenDisplayHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the total display width.
        ''' </summary>
        <DisplayName("Display width")>
        <Description("The total amount of pixels in the display width.")>
        Public ReadOnly Property OnScreenDisplayWidth As UShort?
            Get
                Return Status.OnScreenDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the video output is fullscreen.
        ''' </summary>
        <DisplayName("Fullscreen")>
        <Description("Displays whether the video is in full screen or if custom video output settings are applied. Note that custom settings can mimic fullscreen.")>
        Public Property PlaybackWindowFullscreen As Boolean?
            Get
                Return Status.PlaybackWindowFullscreen
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then
                    value = True
                End If

                Dim command As New SetPlaybackWindowZoomCommand(Me, value.Value)

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback window rectangle's horizontal offset.
        ''' </summary>
        <DisplayName("Playback window horizontal offset")>
        <Description("The offset between the left border of the display and the video output.")>
        Public Property PlaybackWindowRectangleHorizontalOffset As UShort?
            Get
                Return Status.PlaybackWindowRectangleHorizontalOffset
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As SetPlaybackWindowZoomCommand

                If value >= OnScreenDisplayWidth Then
                    command = New SetPlaybackWindowZoomCommand(Me, OnScreenDisplayWidth, PlaybackWindowRectangleVerticalOffset, 0, PlaybackWindowRectangleHeight)
                ElseIf value + PlaybackWindowRectangleWidth > OnScreenDisplayWidth Then
                    command = New SetPlaybackWindowZoomCommand(Me, value, PlaybackWindowRectangleVerticalOffset, (OnScreenDisplayWidth - value), PlaybackWindowRectangleHeight)
                Else
                    command = New SetPlaybackWindowZoomCommand(Me, value, PlaybackWindowRectangleVerticalOffset, PlaybackWindowRectangleWidth, PlaybackWindowRectangleHeight)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback window rectangle's vertical offset.
        ''' </summary>
        <DisplayName("Playback window vertical offset")>
        <Description("The offset between the top border of the display and the video output.")>
        Public Property PlaybackWindowRectangleVerticalOffset As UShort?
            Get
                Return Status.PlaybackWindowRectangleVerticalOffset
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As SetPlaybackWindowZoomCommand

                If value >= OnScreenDisplayHeight Then
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, OnScreenDisplayHeight, PlaybackWindowRectangleWidth, 0)
                ElseIf value + PlaybackWindowRectangleHeight > OnScreenDisplayHeight Then
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, value, PlaybackWindowRectangleWidth, (OnScreenDisplayHeight - value))
                Else
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, value, PlaybackWindowRectangleWidth, PlaybackWindowRectangleHeight)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback window rectangle width.
        ''' </summary>
        <DisplayName("Playback window width")>
        <Description("The playback window rectangle width.")>
        Public Property PlaybackWindowRectangleWidth As UShort?
            Get
                If Status.PlaybackWindowRectangleWidth.HasValue Then
                    Return Status.PlaybackWindowRectangleWidth
                Else
                    Return Status.OnScreenDisplayWidth
                End If
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = OnScreenDisplayWidth
                End If

                Dim command As SetPlaybackWindowZoomCommand

                If value >= OnScreenDisplayWidth Then
                    command = New SetPlaybackWindowZoomCommand(Me, 0, PlaybackWindowRectangleVerticalOffset, OnScreenDisplayWidth, PlaybackWindowRectangleHeight)
                ElseIf value + PlaybackWindowRectangleHorizontalOffset > OnScreenDisplayWidth Then
                    command = New SetPlaybackWindowZoomCommand(Me, (OnScreenDisplayWidth - value), PlaybackWindowRectangleVerticalOffset, value, PlaybackWindowRectangleHeight)
                Else
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, PlaybackWindowRectangleVerticalOffset, value, PlaybackWindowRectangleHeight)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback window rectangle height.
        ''' </summary>
        <DisplayName("Playback window height")>
        <Description("The video playback window rectangle height.")>
        Public Property PlaybackWindowRectangleHeight As UShort?
            Get
                If Status.PlaybackWindowRectangleHeight.HasValue Then
                    Return Status.PlaybackWindowRectangleHeight
                Else
                    Return Status.OnScreenDisplayHeight
                End If
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = OnScreenDisplayHeight
                End If

                Dim command As SetPlaybackWindowZoomCommand

                If value >= OnScreenDisplayHeight Then
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, 0, PlaybackWindowRectangleWidth, OnScreenDisplayHeight)
                ElseIf value + PlaybackWindowRectangleVerticalOffset > OnScreenDisplayHeight Then
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, (OnScreenDisplayHeight - value), PlaybackWindowRectangleWidth, value)
                Else
                    command = New SetPlaybackWindowZoomCommand(Me, PlaybackWindowRectangleHorizontalOffset, PlaybackWindowRectangleVerticalOffset, PlaybackWindowRectangleWidth, value)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the video zoom.
        ''' </summary>
        <DisplayName("Video zoom")>
        <Description("The video output's zoom mode. Accepted values are ""normal"", ""enlarge"", ""make_wider"", ""fill_screen"", ""full_fill_screen"", ""make_taller"" or ""cut_edges"".")>
        Public Property VideoZoom As String
            Get
                Return Status.VideoZoom
            End Get
            Set(value As String)
                If value.IsNotNullOrWhiteSpace Then
                    value = Constants.VideoZoomSettings.Normal
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.VideoZoom = value

                ProcessCommand(command)
            End Set
        End Property

#End Region 'Properties v2

#Region "Properties v3"

        ''' <summary>
        ''' Gets the current playback state.
        ''' </summary>
        <DisplayName("Playback state")>
        <Description("The current playback state.")>
        Public ReadOnly Property PlaybackState As String
            Get
                Return Status.PlaybackState
            End Get
        End Property

        ''' <summary>
        ''' Gets the previous playback state.
        ''' </summary>
        <DisplayName("Previous playback state")>
        <Description("The previous playback state.")>
        Public ReadOnly Property PreviousPlaybackState As String
            Get
                Return Status.PreviousPlaybackState
            End Get
        End Property

        ''' <summary>
        ''' Gets the last playback event.
        ''' </summary>
        <DisplayName("Last playback event")>
        <Description("The last playback event.")>
        Public ReadOnly Property LastPlaybackEvent As String
            Get
                Return Status.LastPlaybackEvent
            End Get
        End Property

        ''' <summary>
        ''' Gets the media URL for the current playback.
        ''' </summary>
        <DisplayName("Playback URL")>
        <Description("The media URL that is currently playing.")>
        Public ReadOnly Property PlaybackUrl As String
            Get
                Return Status.PlaybackUrl
            End Get
        End Property

        ''' <summary>
        ''' Gets the video width of the current playback.
        ''' </summary>
        <DisplayName("Video width")>
        <Description("The video width of the file that is currently playing.")>
        Public ReadOnly Property PlaybackVideoWidth As UShort?
            Get
                Return Status.PlaybackVideoWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the video height of the current playback.
        ''' </summary>
        <DisplayName("Video height")>
        <Description("The video height of the file that is currently playing.")>
        Public ReadOnly Property PlaybackVideoHeight As UShort?
            Get
                Return Status.PlaybackVideoHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the playback clip rectangle's horizontal offset.
        ''' </summary>
        <DisplayName("Playback clip horizontal offset")>
        <Description("The playback clip rectangle's horizontal offset.")>
        Public Property PlaybackClipRectangleHorizontalOffset As UShort?
            Get
                Return Status.PlaybackClipRectangleHorizontalOffset
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As SetPlaybackClipZoomCommand

                If value >= OnScreenDisplayWidth Then
                    command = New SetPlaybackClipZoomCommand(Me, OnScreenDisplayWidth, PlaybackClipRectangleVerticalOffset, 0, PlaybackClipRectangleHeight)
                ElseIf value + PlaybackClipRectangleWidth > OnScreenDisplayWidth Then
                    command = New SetPlaybackClipZoomCommand(Me, value, PlaybackClipRectangleVerticalOffset, (OnScreenDisplayWidth - value), PlaybackClipRectangleHeight)
                Else
                    command = New SetPlaybackClipZoomCommand(Me, value, PlaybackClipRectangleVerticalOffset, PlaybackClipRectangleWidth, PlaybackClipRectangleHeight)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback clip rectangle's vertical offset.
        ''' </summary>
        <DisplayName("Playback clip vertical offset")>
        <Description("The playback clip rectangle's vertical offset.")>
        Public Property PlaybackClipRectangleVerticalOffset As UShort?
            Get
                Return Status.PlaybackClipRectangleVerticalOffset
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = 0
                End If

                If value.HasValue Then
                    Dim command As SetPlaybackClipZoomCommand

                    If value >= OnScreenDisplayHeight Then
                        command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, OnScreenDisplayHeight, PlaybackClipRectangleWidth, 0)
                    ElseIf value + PlaybackClipRectangleHeight > OnScreenDisplayHeight Then
                        command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, value, PlaybackClipRectangleWidth, (OnScreenDisplayHeight - value))
                    Else
                        command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, value, PlaybackClipRectangleWidth, PlaybackClipRectangleHeight)
                    End If

                    ProcessCommand(command)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback clip rectangle's width.
        ''' </summary>
        <DisplayName("Playback clip width")>
        <Description("The playback clip rectangle's width.")>
        Public Property PlaybackClipRectangleWidth As UShort?
            Get
                Return Status.PlaybackClipRectangleWidth
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = PlaybackVideoWidth
                End If

                Dim command As SetPlaybackClipZoomCommand

                If value >= OnScreenDisplayWidth Then
                    command = New SetPlaybackClipZoomCommand(Me, 0, PlaybackClipRectangleVerticalOffset, OnScreenDisplayWidth, PlaybackClipRectangleHeight)
                ElseIf value + PlaybackClipRectangleHorizontalOffset > OnScreenDisplayWidth Then
                    command = New SetPlaybackClipZoomCommand(Me, (OnScreenDisplayWidth - value), PlaybackClipRectangleVerticalOffset, value, PlaybackClipRectangleHeight)
                Else
                    command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, PlaybackClipRectangleVerticalOffset, value, PlaybackClipRectangleHeight)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback clip rectangle's height.
        ''' </summary>
        <DisplayName("Playback clip height")>
        <Description("The playback clip rectangle's height.")>
        Public Property PlaybackClipRectangleHeight As UShort?
            Get
                Return Status.PlaybackClipRectangleHeight
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = PlaybackVideoHeight
                End If

                Dim command As SetPlaybackClipZoomCommand

                If value >= OnScreenDisplayHeight Then
                    command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, 0, PlaybackClipRectangleWidth, OnScreenDisplayHeight)
                ElseIf value + PlaybackClipRectangleVerticalOffset > OnScreenDisplayHeight Then
                    command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, (OnScreenDisplayHeight - value), PlaybackClipRectangleWidth, value)
                Else
                    command = New SetPlaybackClipZoomCommand(Me, PlaybackClipRectangleHorizontalOffset, PlaybackClipRectangleVerticalOffset, PlaybackClipRectangleWidth, value)
                End If

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show video on top.
        ''' </summary>
        Public Property VideoOnTop As Boolean?
            Get
                Return Status.VideoOnTop
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then
                    value = False
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.VideoOnTop = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the active subtitle track.
        ''' </summary>
        Public Property SubtitlesTrack As UShort?
            Get
                Return Status.SubtitlesTrack
            End Get
            Set(value As UShort?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.SubtitleTrack = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the list of available subtitles for the current playback.
        ''' </summary>
        <DisplayName("Subtitles")>
        <Description("The collection of subtitle tracks in the current playback.")>
        Public ReadOnly Property Subtitles As SortedList(Of UShort, LanguageTrack)
            Get
                Return Status.Subtitles
            End Get
        End Property


#End Region ' Properties v3

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
        Public Function NavigateMenu(action As String) As CommandResult
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

#Region "Methods v3"

        Public Function GetText() As CommandResult
            Dim command As New GetTextCommand(Me)
            Return ProcessCommand(command)
        End Function

        Public Function SetText(text As String) As CommandResult
            Dim command As New SetTextCommand(Me, text)
            Return ProcessCommand(command)
        End Function

#End Region ' Methods v3

#Region "App events"

#End Region ' App events

#Region "Interface implementations"

        ''' <summary>
        ''' INotifyPropertyChanged implementation.
        ''' </summary>
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region



    End Class

End Namespace
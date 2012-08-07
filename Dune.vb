#Region "License"
' Copyright 2012 Steven Liekens
' Contact: steven.liekens@gmail.com

' This file is part of DuneApiCodepack.

' DuneApiCodepack is free software: you can redistribute it and/or modify
' it under the terms of the GNU Lesser General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' DuneApiCodepack is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Lesser General Public License for more details.

' You should have received a copy of the GNU Lesser General Public License
' along with DuneApiCodepack.  If not, see <http://www.gnu.org/licenses/>.
#End Region ' License
Imports System.Net
Imports System.Net.Sockets
Imports System.ComponentModel
Imports SL.DuneApiCodePack.DuneUtilities.ApiWrappers
Imports System.Timers
Imports System.Threading.Tasks
Imports System.Collections.ObjectModel
Imports SL.DuneApiCodePack.Sources
Imports SL.DuneApiCodePack.Networking
Imports System.Runtime.Serialization

Namespace DuneUtilities

    ''' <summary>
    ''' Provides methods and properties to interact with an IP control-enabled Dune player.
    ''' </summary>
    ''' <remarks>
    ''' To connect, use one of the available connect methods. Or create a a Dune object using one of the constructor overloads, these constructors will automatically attempt a connection.
    ''' Note that you must have connected atleast once (to set initial connection details) before you can do anything meaningful. There are currently numerous bugs (mostly nullreferences) when trying to access properties without a connection.
    ''' </remarks>
    <Serializable()>
    Public Class Dune
        Implements INotifyPropertyChanging
        Implements INotifyPropertyChanged
        Implements ISerializable
        Implements IDisposable

#Region "Private Fields"

        ' Connection details
        Private _hostentry As IPHostEntry
        Private _endpoint As IPEndPoint
        Private _timeout As Integer


        ' Custom fields
        Private _statusUpdater As Timer
        Private _textUpdater As Timer
        Private _status As CommandResult
        Private _text As String
        Private _textAvailable As Boolean?
        Private _remoteControl As RemoteControl

        Private _firmwares As ReadOnlyCollection(Of FirmwareProperties)
        Private _shares As ReadOnlyCollection(Of LocalStorage)

        Private _connected As Boolean
        Private _networkAdapterInfo As NetworkAdapterInfo
        Private _telnetClient As TelnetClient
        Private _systemInfo As SystemInformation

#End Region ' Private Fields

#Region "Constructors"

        ''' <summary>
        ''' Default constructor. Use an overload of <see cref="Connect"/> to connect the object instance.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor overload that attempts to connect to the specified host name or IP address.
        ''' </summary>
        ''' <param name="hostNameOrAddress">The host name or IP address on which to connect.</param>
        Public Sub New(hostNameOrAddress As String)
            Connect(hostNameOrAddress)
        End Sub

        ''' <summary>
        ''' Constructor overload that attempts to connect to the specified IP address.
        ''' </summary>
        ''' <param name="address">The IP address on which to connect.</param>
        Public Sub New(address As IPAddress)
            Connect(address)
        End Sub

        ''' <summary>
        ''' Constructor overload that attempts to connect to the specified host name or IP address on the specified port number.
        ''' </summary>
        ''' <param name="hostNameOrAddress">The host name or IP address on which to connect.</param>
        ''' <param name="port">The port number on which to connect.</param>
        Public Sub New(hostNameOrAddress As String, port As Integer)
            Connect(hostNameOrAddress, port)
        End Sub

        ''' <summary>
        ''' Constructor overload that attempts to connect to the specified IP address on the specified port number.
        ''' </summary>
        ''' <param name="address">The IP address on which to connect.</param>
        ''' <param name="port">The port number on which to connect.</param>
        Public Sub New(address As IPAddress, port As Integer)
            Connect(address, port)
        End Sub

        ''' <summary>
        ''' Constructor overload that attempts to connect to the specified IP endpoint.
        ''' </summary>
        ''' <param name="endpoint">The IP endpoint on which to connect.</param>
        Public Sub New(endpoint As IPEndPoint)
            If endpoint.AddressFamily <> AddressFamily.InterNetwork Then
                Throw New ArgumentException("IP address must be in IPv4 format.", "endpoint")
            Else
                Connect(endpoint)
            End If
        End Sub
#End Region ' Constructors

#Region "Initialization"

        ''' <summary>
        ''' Connects the object instance to the specified host name or IP address.
        ''' </summary>
        ''' <param name="hostNameOrAddress">The host name or IP address on which to connect.</param>
        Public Sub Connect(hostNameOrAddress As String)
            Connect(hostNameOrAddress, 80)
        End Sub

        ''' <summary>
        ''' Connects the object instance to the specified IP address.
        ''' </summary>
        ''' <param name="address">The IP address on which to connect.</param>
        Public Sub Connect(address As IPAddress)
            Connect(address, 80)
        End Sub

        ''' <summary>
        ''' Connects the object instance to the specified IP endpoint.
        ''' </summary>
        ''' <param name="endpoint">The IP endpoint on which to connect.</param>
        Public Sub Connect(endpoint As IPEndPoint)
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry(endpoint.Address)
            Connect(hostEntry, Port)
        End Sub

        ''' <summary>
        ''' Connects the object instance to the specified host name or IP address on the specified port number.
        ''' </summary>
        ''' <param name="hostNameOrAddress">The host name or IP address on which to connect.</param>
        ''' <param name="port">The port number on which to connect.</param>
        Public Sub Connect(hostNameOrAddress As String, port As Integer)
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry(hostNameOrAddress)
            Connect(hostEntry, port)
        End Sub

        ''' <summary>
        ''' Connects the object instance to the specified IP address on the specified port number.
        ''' </summary>
        ''' <param name="address">The IP address on which to connect.</param>
        ''' <param name="port">The port number on which to connect.</param>
        Public Sub Connect(address As IPAddress, port As Integer)
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry(address)
            Connect(hostEntry, port)
        End Sub

        ''' <summary>
        ''' Sets connection details and attempts to initialize the object instance.
        ''' </summary>
        ''' <remarks>This method is for internal use only.</remarks>
        Private Sub Connect(address As IPHostEntry, port As Integer)
            _hostentry = address
            _endpoint = New IPEndPoint(address.AddressList.GetIPv4Addresses.First, port)

            Initialize()
        End Sub

        ''' <summary>
        ''' Initialization process.
        ''' </summary>
        ''' <remarks>This method is for internal use only.</remarks>
        Private Sub Initialize()
            Try
                GetStatus()
                _connected = True

                StatusUpdater.Start()
            Catch ex As WebException
                Disconnect()
                Throw
            End Try
        End Sub

#End Region ' Initialization

#Region "App Properties"

        ''' <summary>
        ''' Gets or sets a set of command results, representing the player status.
        ''' </summary>
        <Browsable(False)>
        Private Property Status As CommandResult
            Get
                Return _status
            End Get
            Set(value As CommandResult)
                RaisePropertyChanging("Status")
                If Status Is Nothing Then
                    _status = value
                Else
                    Dim updates() As String = Status.GetDifferences(value)

                    For Each update As String In updates
                        RaisePropertyChanging(update)
                    Next
                    _status = value
                    For Each update As String In updates
                        RaisePropertyChanged(update)
                    Next
                End If
                RaisePropertyChanged("Status")
            End Set
        End Property

        ''' <summary>
        ''' Gets the IP address of the device.
        ''' </summary>
        <DisplayName("IP address")>
        <Description("Indicates the IP address on which the app is connected to the HTTP service.")>
        <Category("Connection details")>
        Public ReadOnly Property Address As IPAddress
            Get
                If _endpoint IsNot Nothing Then
                    Return _endpoint.Address
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical address of the device.
        ''' </summary>
        <DisplayName("MAC address")>
        <Description("Indicates the hardware address of the device's network interface.")>
        <Category("Connection details")>
        <TypeConverter(GetType(PhysicalAddressConverter))>
        Public ReadOnly Property PhysicalAddress As NetworkInformation.PhysicalAddress
            Get
                Return NetworkAdapterInfo.PhysicalAddress
            End Get
        End Property

        ''' <summary>
        ''' Gets the port number used to connect to the service.
        ''' </summary>
        <DisplayName("Port")>
        <Description("Indicates the port on which the app is connected to the HTTP service.")>
        <Category("Connection details")>
        Public ReadOnly Property Port As Integer
            Get
                If _endpoint IsNot Nothing Then
                    Return _endpoint.Port
                Else
                    Return 80
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the host entry details returned from the DNS server.
        ''' </summary>
        <Browsable(False)>
        Public ReadOnly Property HostEntry As IPHostEntry
            Get
                Return _hostentry
            End Get
        End Property

        ''' <summary>
        ''' Gets the hostname of the device.
        ''' </summary>
        <DisplayName("Host name")>
        <Description("Indicates the device's host name.")>
        <Category("Connection details")>
        Public ReadOnly Property HostName As String
            Get
                If HostEntry.HostName.IsNotNullOrWhiteSpace Then
                    If HostEntry.HostName.Contains(".") Then
                        Return HostEntry.HostName.Left(HostEntry.HostName.IndexOf("."c))
                    Else
                        Return HostEntry.HostName
                    End If
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets information about the device's network interface.
        ''' </summary>
        <Browsable(False)>
        Public ReadOnly Property NetworkAdapterInfo As NetworkAdapterInfo
            Get
                If _networkAdapterInfo Is Nothing Then
                    _networkAdapterInfo = New NetworkAdapterInfo(Me.Address)
                End If
                Return _networkAdapterInfo
            End Get
        End Property

        ''' <summary>
        ''' Gets an instance of the RemoteControl class.
        ''' </summary>
        <Browsable(False)>
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
        <Description("Indicates the collection of network shares exposed by the device's SMB server.")>
        <Category("Connection details")>
        Public ReadOnly Property NetworkShares As ReadOnlyCollection(Of LocalStorage)
            Get
                If _shares Is Nothing Then
                    _shares = New ReadOnlyCollection(Of LocalStorage)(LocalStorage.FromHost(Me))
                End If

                Return _shares
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether the connection is active.
        ''' </summary>
        <DisplayName("Connected")>
        <Description("Indicates whether the app is still connected.")>
        <Category("Connection details")>
        Public ReadOnly Property IsConnected As Boolean
            Get
                Return _connected
            End Get
        End Property

        ''' <summary>
        ''' Gets the remaining playback time.
        ''' </summary>
        <DisplayName("Playback time remaining")>
        <Description("Indicates the remaining playback time.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackTimeRemaining As TimeSpan?
            Get
                If IsConnected Then
                    Return Status.PlaybackTimeRemaining
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
        <DisplayName("Timeout")>
        <Description("Indicates the amount of seconds before a command returns a timeout.")>
        <Category("Connection details")>
        Public Property Timeout As Integer
            Get
                If _timeout = 0 Then
                    _timeout = 20
                End If
                Return _timeout
            End Get
            Set(value As Integer)
                If _timeout <> value Then
                    RaisePropertyChanging("Timeout")
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
        <Description("Indicates the amount of miliseconds between successful status updates.")>
        <Category("Connection details")>
        Public Property Interval As Double
            Get
                Return StatusUpdater.Interval
            End Get
            Set(value As Double)
                StatusUpdater.Interval = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a list of available firmwares. The <see cref="ProductID" /> property must be set to populate the collection.
        ''' </summary>
        <DisplayName("Available firmwares")>
        <Description("Indicates the collection of available firmware versions for this device.")>
        <Category("Firmware information")>
        Public ReadOnly Property AvailableFirmwares As ReadOnlyCollection(Of FirmwareProperties)
            Get
                If _firmwares Is Nothing Then
                    Dim list() As FirmwareProperties

                    If ProductId.IsNotNullOrEmpty Then
                        Try
                            list = FirmwareProperties.GetAvailableFirmwares(ProductId)
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                            list = Nothing
                        End Try
                    Else
                        list = Nothing
                    End If

                    If list IsNot Nothing Then
                        _firmwares = New ReadOnlyCollection(Of FirmwareProperties)(list)
                    Else

                    End If
                End If

                Return _firmwares
            End Get
        End Property

        ''' <summary>
        ''' Gets whether there is a firmware update available.
        ''' </summary>
        <DisplayName("Firmware update available")>
        <Description("Indicates whether a firmware update is available.")>
        <Category("Firmware information")>
        Public ReadOnly Property UpdateAvailable As Boolean?
            Get
                If AvailableFirmwares Is Nothing OrElse AvailableFirmwares.Count = 0 Then
                    Return Nothing
                Else
                    If FirmwareVersion.IsNotNullOrEmpty Then
                        Return (AvailableFirmwares.Item(0).Version <> FirmwareVersion)
                    Else
                        Return Nothing
                    End If
                End If
                'If FirmwareVersion.IsNotNullOrEmpty AndAlso AvailableFirmwares.Count > 0 Then
                '    Return (AvailableFirmwares.Item(0).Version <> FirmwareVersion)
                'Else
                '    Return Nothing
                'End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the timer that's responsible for automatic status updates.
        ''' </summary>
        <Browsable(False)>
        Private ReadOnly Property StatusUpdater As Timer
            Get
                If _statusUpdater Is Nothing Then
                    _statusUpdater = New Timer
                    _statusUpdater.AutoReset = False
                    _statusUpdater.Interval = 100
                    AddHandler _statusUpdater.Elapsed, AddressOf StatusUpdater_elapsed
                End If

                Return _statusUpdater
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the text editor is currently open.
        ''' </summary>
        <DisplayName("Text editor available")>
        <Description("Indicates whether the text editor is currently open.")>
        <Category("Text editor")>
        Public ReadOnly Property TextAvailable As Boolean?
            Get
                If IsConnected Then
                    Return _textAvailable
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region ' App Properties

#Region "App methods"

        ''' <summary>
        ''' Scans the network and retrieves a collection of all Dune devices.
        ''' </summary>
        ''' <param name="ignoreNonSigmaNic">Specifies whether to ignore devices whose network card is not made by Sigma Designs.</param>
        Public Shared Function Scan(ignoreNonSigmaNic As Boolean) As Dune()
            Dim dunes As New ArrayList

            For Each device As Networking.NetworkDevice In Networking.NetworkDevice.Scan
                If device.IsDune(ignoreNonSigmaNic) Then
                    dunes.Add(New Dune(device.Host.HostName))
                End If
            Next

            Return DirectCast(dunes.ToArray(GetType(Dune)), Dune())
        End Function

        ''' <summary>
        ''' Updates the status properties.
        ''' </summary>
        <DebuggerStepThrough()>
        Private Sub StatusUpdater_elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
            If IsConnected Then
                Dim status As CommandResult = GetStatus()
                If status.ProtocolVersion IsNot Nothing AndAlso status.ProtocolVersion.Major >= 3 Then
                    Dim command As New GetTextCommand(Me)
                    Dim result As CommandResult

                    result = command.GetResult

                    If Not Nullable.Equals(_textAvailable, result.TextAvailable) Then
                        RaisePropertyChanging("TextAvailable")
                        _textAvailable = result.TextAvailable
                        RaisePropertyChanged("TextAvailable")
                    End If

                    If result.TextAvailable = True AndAlso _text <> result.Text Then
                        RaisePropertyChanging("Text")
                        _text = result.Text
                        RaisePropertyChanged("Text")
                    End If
                End If
                StatusUpdater.Start()
            End If
        End Sub

        ''' <summary>
        ''' Executes the specified command and processes its results.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <exception cref="CommandException">: command execution failed.</exception>
        Public Function ProcessCommand(command As Command) As CommandResult
            Return ProcessCommand(command, False)
        End Function

        ''' <summary>
        ''' Executes the specified command and processes its results asynchronously.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <exception cref="CommandException">: command execution failed.</exception>
        Public Function ProcessCommandAsync(command As Command) As Task(Of CommandResult)
            Return ProcessCommandAsync(command, False)
        End Function

        ''' <summary>
        ''' Executes the specified command and processes its results asynchronously.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <param name="suppressError">Indicates whether to throw an exception when a command returns an error.</param>
        ''' <exception cref="CommandException">: command execution failed.</exception>
        Public Function ProcessCommandAsync(command As Command, suppressError As Boolean) As Task(Of CommandResult)
            Return Task(Of CommandResult).Factory.StartNew(Function() ProcessCommand(command, suppressError))
        End Function

        ''' <summary>
        ''' Executes the specified command and processes its results.
        ''' </summary>
        ''' <param name="command">The command to execute.</param>
        ''' <param name="suppressError">Indicates whether to throw an exception when a command returns an error.</param>
        ''' <exception cref="CommandException">: command execution failed.</exception>
        Public Function ProcessCommand(command As Command, suppressError As Boolean) As CommandResult
            Try
                Dim result As CommandResult = command.GetResult
                Status = result

#If DEBUG Then
                _requestCount += 1
#End If

                If suppressError.IsFalse Then
                    Select Case result.ErrorKind
                        Case String.Empty
                            Exit Select
                        Case Constants.ErrorKindValues.UnknownCommand, Constants.ErrorKindValues.InvalidParameters
                            Throw New ArgumentException(result.ErrorDescription) With {.Source = result.Command.ToString}
                        Case Constants.ErrorKindValues.IllegalState
                            Throw New InvalidOperationException(result.ErrorDescription) With {.Source = result.Command.ToString}
                        Case Else
                            Throw New CommandException(result.ErrorKind, result.ErrorDescription) With {.Source = result.Command.ToString}
                    End Select
                End If

                Return result
            Catch ex As WebException
                Disconnect()
                If suppressError.IsFalse Then ' rethrow
                    Throw
                End If
            End Try

            Return Nothing
        End Function

        ''' <summary>
        ''' Disconnect the object instance from the target.
        ''' </summary>
        Private Sub Disconnect()
            If IsConnected Then
                RaisePropertyChanging("IsConnected")

                SystemInfo.Clear()
                TelnetClient.Dispose()
                _telnetClient = Nothing

                _connected = False
                RaisePropertyChanged("IsConnected")
            End If
        End Sub

        ''' <summary>
        ''' Reinitializes a connection.
        ''' </summary>
        Public Sub Reconnect()
            If IsConnected.IsFalse Then
                Try
                    GetStatus()

                    RaisePropertyChanging("IsConnected")
                    SystemInfo.Refresh()

                    _connected = True
                    RaisePropertyChanged("IsConnected")

                    StatusUpdater.Start()
                Catch ex As WebException
                    Throw New InvalidOperationException("A connection could not be established.", ex)
                End Try
            End If
        End Sub

#End Region ' App methods

#If DEBUG Then
#Region "Debugging help"
        Private _requestCount As Long
        Public ReadOnly Property RequestCount As Long
            Get
                Return _requestCount
            End Get
        End Property

#End Region
#End If

#Region "Telnet Properties"

        ''' <summary>
        ''' Gets a preconfigured Telnet client instance.
        ''' </summary>
        <Browsable(False)>
        Public ReadOnly Property TelnetClient As TelnetClient
            Get
                If _telnetClient Is Nothing Then
                    _telnetClient = New TelnetClient(Address)
                    _telnetClient.LogOn("root")
                End If
                Return _telnetClient
            End Get
        End Property

        <Browsable(False)>
        Private ReadOnly Property SystemInfo As SystemInformation
            Get
                If _systemInfo Is Nothing Then
                    _systemInfo = SystemInformation.FromHost(Me)
                End If
                Return _systemInfo
            End Get
        End Property

        ''' <summary>
        ''' Gets the player's product ID (using a Telnet connection).
        ''' </summary>
        <DisplayName("Product ID")>
        <Description("Indicates the device's product ID.")>
        <Category("Player information")>
        Public ReadOnly Property ProductId As String
            Get
                If SystemInfo IsNot Nothing Then
                    Return SystemInfo.ProductId
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the device's serial number (using a Telnet connection).
        ''' </summary>
        <DisplayName("Serial number")>
        <Description("Indicates the device's serial number.")>
        <Category("Player information")>
        Public ReadOnly Property SerialNumber As String
            Get
                If SystemInfo IsNot Nothing Then
                    Return SystemInfo.Serial
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the installed firmware version string (using a Telnet connection).
        ''' </summary>
        <DisplayName("Firmware version")>
        <Description("Indicates the installed firmware version.")>
        <Category("Firmware information")>
        Public ReadOnly Property FirmwareVersion As String
            Get
                If SystemInfo IsNot Nothing Then
                    Return SystemInfo.FirmareVersion
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the device booted up.
        ''' </summary>
        <DisplayName("Up since")>
        <Description("Indicates the date and time when the device powered on.")>
        <Category("Player information")>
        Public ReadOnly Property BootTime As Date
            Get
                If SystemInfo IsNot Nothing Then
                    Return SystemInfo.BootTime
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the device's uptime.
        ''' </summary>
        <DisplayName("Uptime")>
        <Description("Indicates the amount of elapsed time since the device powered on.")>
        <Category("Player information")>
        Public ReadOnly Property Uptime As TimeSpan?
            Get
                If SystemInfo IsNot Nothing AndAlso SystemInfo.BootTime <> Nothing Then
                    Dim start As New TimeSpan(BootTime.Ticks)
                    Dim current As New TimeSpan(Now.Ticks)
                    Return current.Subtract(start).RoundToSecond
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region ' Telnet Properties

#Region "Telnet Methods"

        ''' <summary>
        ''' Closes the disc tray (if any) using a telnet connection.
        ''' </summary>
        Public Sub CloseDiscTray()
            TelnetClient.ExecuteCommand("eject -t /dev/sr0")
        End Sub

        ''' <summary>
        ''' Toggles between open/close disc tray (if any) using a telnet connection.
        ''' </summary>
        Public Sub ToggleDiscTray()
            TelnetClient.ExecuteCommand("eject -T /dev/sr0")
        End Sub

        ''' <summary>
        ''' Sends a reboot command using telnet.
        ''' </summary>
        Public Sub Reboot()
            TelnetClient.ExecuteCommand("reboot")
        End Sub

#End Region ' Telnet Methods

#Region "Properties v1"

        ''' <summary>
        ''' Gets the protocol version number.
        ''' </summary>
        <DisplayName("Protocol version")>
        <Description("Indicates the API version.")>
        <Category("Firmware information")>
        Public ReadOnly Property ProtocolVersion As Version
            Get
                If IsConnected Then
                    Return Status.ProtocolVersion
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the status of the last command.
        ''' </summary>
        <DisplayName("Command status")>
        <Description("Indicates the status of the last command")>
        <Category("Player information")>
        Public ReadOnly Property CommandStatus As String
            Get
                If IsConnected Then
                    Return Status.CommandStatus
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        <DisplayName("Playback duration")>
        <Description("Indicates the playback duration.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackDuration As System.TimeSpan?
            Get
                If IsConnected Then
                    Return Status.PlaybackDuration
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        <DisplayName("Playback position")>
        <Description("Indicates the current playback position.")>
        <Category("Playback information")>
        Public Property PlaybackPosition As System.TimeSpan?
            Get
                If IsConnected Then
                    Return Status.PlaybackPosition
                Else
                    Return Nothing
                End If
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
        <Description("Indicates the playback speed.")>
        <Category("Playback information")>
        <TypeConverter(GetType(PlaybackSpeedConverter))>
        Public Property PlaybackSpeed As Constants.PlaybackSpeedValues?
            Get
                If IsConnected Then
                    If Status.PlaybackSpeed.HasValue Then
                        Return CType(Status.PlaybackSpeed, Constants.PlaybackSpeedValues)
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Constants.PlaybackSpeedValues?)
                If Not value.HasValue Then
                    value = Constants.PlaybackSpeedValues.Play256
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
        <Description("Indicates the player state.")>
        <Category("Player information")>
        <TypeConverter(GetType(PlayerStateConverter))>
        Public Property PlayerState As String
            Get
                If IsConnected Then
                    Return Status.PlayerState
                Else
                    Return String.Empty
                End If
            End Get
            Set(value As String)
                If value.IsNullOrWhiteSpace Then
                    value = Constants.CommandValues.MainScreen
                End If

                Dim command As New SetPlayerStateCommand(Me, value)

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets whether a DVD menu is shown.
        ''' </summary>
        <DisplayName("DVD menu")>
        <Description("Indicates whether the player is showing a DVD menu.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackDvdMenu As Boolean?
            Get
                If IsConnected Then
                    Return Status.PlaybackDvdMenu
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a Blu-ray menu is shown.
        ''' </summary>
        <DisplayName("Blu-ray menu")>
        <Description("Indicates whether the player is showing a Blu-ray menu.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackBlurayMenu As Boolean?
            Get
                If IsConnected Then
                    Return Status.PlaybackBlurayMenu
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        <DisplayName("Playback buffering")>
        <Description("Indicates whether the playback is buffering.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackIsBuffering As Boolean?
            Get
                If IsConnected Then
                    Return Status.PlaybackIsBuffering
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Sets whether to show a black screen during playback.
        ''' </summary>
        <DisplayName("Black screen")>
        <Description("Indicates whether to show a black screen.")>
        <Category("Playback information")>
        Public Property BlackScreen As Boolean?
            Get
                Return Nothing ' because this parameter is not supplied in the command results
            End Get
            Set(value As Boolean?)
                If Not value.HasValue Then ' default to false
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
        <Description("Indicates whether to hide overlay graphics.")>
        <Category("Playback information")>
        Public Property HideOnScreenDisplay As Boolean?
            Get
                Return Nothing ' because this parameter is not supplied in the command results
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
        <Description("Indicates the playback volume percentage.")>
        <Category("Playback information")>
        Public Property PlaybackVolume As Short?
            Get
                If IsConnected Then
                    Return Status.PlaybackVolume
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
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
        <Description("Indicates whether the volume is muted.")>
        <Category("Playback information")>
        Public Property PlaybackMute As Boolean?
            Get
                If IsConnected Then
                    Return Status.PlaybackMute
                Else
                    Return Nothing
                End If
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
        ''' Gets or sets the current audio track number.
        ''' </summary>
        <DisplayName("Audio track")>
        <Description("Indicates the active audio track. Not to be confused with the track number in a playlist.")>
        <Category("Playback information")>
        Public Property AudioTrack As Short?
            Get
                If IsConnected Then
                    Return Status.AudioTrack
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.AudioTrack = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the collection of audio tracks in the current playback.
        ''' </summary>
        ''' <returns>An instance of a <see cref="SortedDictionary(Of Short, CultureInfo)" /> object that represents the list of available audio tracks.</returns>
        <DisplayName("Audio tracks")>
        <Description("The collection of audio tracks in the current playback.")>
        <Category("Playback information")>
        Public ReadOnly Property AudioTracks As SortedList(Of Short, LanguageTrack)
            Get
                If IsConnected Then
                    Return Status.AudioTracks
                Else
                    Return New SortedList(Of Short, LanguageTrack)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether the video output is enabled.
        ''' </summary>
        <DisplayName("Video enabled")>
        <Description("Indicates whether video output is enabled.")>
        <Category("Playback information")>
        Public Property VideoEnabled As Boolean?
            Get
                If IsConnected Then
                    Return Status.VideoEnabled
                Else
                    Return Nothing
                End If
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
        <Description("Indicates the total available display height.")>
        <Category("Player information")>
        Public ReadOnly Property OnScreenDisplayHeight As Short?
            Get
                If IsConnected Then
                    Return Status.OnScreenDisplayHeight
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the total display width.
        ''' </summary>
        <DisplayName("Display width")>
        <Description("Indicates the total available display width.")>
        <Category("Player information")>
        Public ReadOnly Property OnScreenDisplayWidth As Short?
            Get
                If IsConnected Then
                    Return Status.OnScreenDisplayWidth
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the video output is fullscreen.
        ''' </summary>
        <DisplayName("Fullscreen")>
        <Description("Indicates whether custom playback window zoom settings are applied.")>
        <Category("Playback window zoom")>
        Public Property PlaybackWindowFullscreen As Boolean?
            Get
                If IsConnected Then
                    Return Status.PlaybackWindowFullscreen
                Else
                    Return Nothing
                End If
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
        <Description("Indicates the playback window rectangle's horizontal offset.")>
        <Category("Playback window zoom")>
        Public Property PlaybackWindowRectangleHorizontalOffset As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackWindowRectangleHorizontalOffset.HasValue Then ' return the actual value
                        Return Status.PlaybackWindowRectangleHorizontalOffset
                    ElseIf PlaybackWindowFullscreen = True Then ' return the default value
                        Return 0
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
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
        <Description("Indicates the playback window rectangle's vertical offset.")>
        <Category("Playback window zoom")>
        Public Property PlaybackWindowRectangleVerticalOffset As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackWindowRectangleVerticalOffset.HasValue Then ' return the actual value
                        Return Status.PlaybackWindowRectangleVerticalOffset
                    ElseIf Status.PlaybackWindowFullscreen = True Then ' return the default value
                        Return 0
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
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
        ''' Gets or sets the playback window rectangle's width.
        ''' </summary>
        <DisplayName("Playback window width")>
        <Description("Indicates the playback window rectangle's width.")>
        <Category("Playback window zoom")>
        Public Property PlaybackWindowRectangleWidth As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackWindowRectangleWidth.HasValue Then
                        Return Status.PlaybackWindowRectangleWidth
                    Else
                        Return Status.OnScreenDisplayWidth
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
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
        ''' Gets or sets the playback window rectangle's height.
        ''' </summary>
        <DisplayName("Playback window height")>
        <Description("Indicates the playback window rectangle's height.")>
        <Category("Playback window zoom")>
        Public Property PlaybackWindowRectangleHeight As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackWindowRectangleHeight.HasValue Then
                        Return Status.PlaybackWindowRectangleHeight
                    Else
                        Return Status.OnScreenDisplayHeight
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
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
        ''' Gets or sets the video zoom preset.
        ''' </summary>
        <DisplayName("Video zoom")>
        <Description("Indicates the video zoom mode.")>
        <Category("Playback information")>
        <TypeConverter(GetType(ZoomConverter))>
        Public Property VideoZoom As String
            Get
                If IsConnected Then
                    Return Status.VideoZoom
                Else
                    Return String.Empty
                End If
            End Get
            Set(value As String)
                If value.IsNullOrWhiteSpace Then
                    value = Constants.VideoZoomValues.Normal
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.VideoZoom = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the amount of units in the display's width in relation to the amount of units in its height.
        ''' </summary>
        <DisplayName("Screen aspect ratio (width)")>
        <Description("Indicates the amount of units in the display's width in relation to the amount of units in its height.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property OnScreenDisplayAspectRatioWidth As Short?
            Get
                If IsConnected Then
                    If OnScreenDisplayWidth.HasValue Then
                        Dim gcf As Integer = CInt(OnScreenDisplayWidth).GetGreatestCommonFactor(CInt(OnScreenDisplayHeight))
                        Dim width As Integer = CInt(OnScreenDisplayWidth / gcf)
                        Return CShort(width)
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of units in the display's height in relation to the amount of units in its width.
        ''' </summary>
        <DisplayName("Screen aspect ratio (height)")>
        <Description("Indicates the amount of units in the display's height in relation to the amount of units in its width.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property OnScreenDisplayAspectRatioHeight As Short?
            Get
                If IsConnected Then
                    If OnScreenDisplayWidth.HasValue Then
                        Dim gcf As Integer = CInt(OnScreenDisplayWidth).GetGreatestCommonFactor(CInt(OnScreenDisplayHeight))
                        Dim height As Integer = CInt(OnScreenDisplayHeight / gcf)
                        Return CShort(height)
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of units in the playback window rectangle's width in relation to the amount of units in its height.
        ''' </summary>
        <DisplayName("Playback window aspect ratio (width)")>
        <Description("Indicates the amount of units in the playback window rectangle's width in relation to the amount of units in its height.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property PlaybackWindowRectangleAspectRatioWidth As Short?
            Get
                If IsConnected Then
                    If PlaybackWindowRectangleWidth.HasValue Then
                        If PlaybackWindowRectangleWidth > 0 And PlaybackWindowRectangleHeight > 0 Then
                            Dim gcf As Integer = CInt(PlaybackWindowRectangleWidth).GetGreatestCommonFactor(CInt(PlaybackWindowRectangleHeight))
                            Dim width As Integer = CInt(PlaybackWindowRectangleWidth / gcf)
                            Return CShort(width)
                        Else
                            Return 0
                        End If
                    Else
                        Return OnScreenDisplayAspectRatioWidth
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of units in the playback window rectangle's height in relation to the amount of units in its width.
        ''' </summary>
        <DisplayName("Playback window aspect ratio (height)")>
        <Description("Indicates the amount of units in the playback window rectangle's height in relation to the amount of units in its width.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property PlaybackWindowRectangleAspectRatioHeight As Short?
            Get
                If IsConnected Then
                    If PlaybackWindowRectangleHeight.HasValue Then
                        If PlaybackWindowRectangleWidth > 0 And PlaybackWindowRectangleHeight > 0 Then
                            Dim gcf As Integer = CInt(PlaybackWindowRectangleWidth).GetGreatestCommonFactor(CInt(PlaybackWindowRectangleHeight))
                            Dim height As Integer = CInt(PlaybackWindowRectangleHeight / gcf)
                            Return CShort(height)
                        Else
                            Return 0
                        End If
                    Else
                        Return OnScreenDisplayAspectRatioHeight
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region 'Properties v2

#Region "Properties v3"

        ''' <summary>
        ''' Gets the current playback state.
        ''' </summary>
        <DisplayName("Playback state")>
        <Description("Indicates the current playback state.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackState As String
            Get
                If IsConnected Then
                    Return Status.PlaybackState
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the previous playback state.
        ''' </summary>
        <DisplayName("Previous playback state")>
        <Description("Indicates the previous playback state.")>
        <Category("Playback information")>
        Public ReadOnly Property PreviousPlaybackState As String
            Get
                If IsConnected Then
                    Return Status.PreviousPlaybackState
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the last playback event.
        ''' </summary>
        <DisplayName("Last playback event")>
        <Description("Indicates the last playback event.")>
        <Category("Player information")>
        Public ReadOnly Property LastPlaybackEvent As String
            Get
                If IsConnected Then
                    Return Status.LastPlaybackEvent
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the media URL for the current playback.
        ''' </summary>
        <DisplayName("Playback URL")>
        <Description("Indicates the media URL that is currently playing.")>
        <Category("Playback information")>
        Public Property PlaybackUrl As String
            Get
                If IsConnected Then
                    Return Status.PlaybackUrl
                Else
                    Return String.Empty
                End If
            End Get
            Set(value As String)
                Dim command As New StartPlaybackCommand(Me, value)
                ProcessCommand(command)
            End Set
        End Property

        <DisplayName("Playback name")>
        <Description("Indicates the file that is currently playing.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackName As String
            Get
                If IsConnected AndAlso PlaybackUrl.IsNotNullOrWhiteSpace Then
                    'Return IO.Path.GetFileName(PlaybackUrl)
                    Return PlaybackUrl.Substring(PlaybackUrl.LastIndexOf("/"c) + 1)
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the current video stream's horizontal resolution.
        ''' </summary>
        <DisplayName("Video width")>
        <Description("Indicates the current video stream's horizontal resolution.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackVideoWidth As Short?
            Get
                If IsConnected Then
                    Return Status.PlaybackVideoWidth
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the current video stream's vertical resolution.
        ''' </summary>
        <DisplayName("Video height")>
        <Description("Indicates the current video stream's vertical resolution.")>
        <Category("Playback information")>
        Public ReadOnly Property PlaybackVideoHeight As Short?
            Get
                If IsConnected Then
                    Return Status.PlaybackVideoHeight
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the visible screen rectangle's horizontal offset.
        ''' </summary>
        <DisplayName("Playback clip horizontal offset")>
        <Description("Indicates the visible screen rectangle's horizontal offset.")>
        <Category("Playback clip zoom")>
        Public Property PlaybackClipRectangleHorizontalOffset As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackClipRectangleHorizontalOffset.HasValue Then
                        Return Status.PlaybackClipRectangleHorizontalOffset
                    ElseIf Status.OnScreenDisplayWidth.HasValue Then
                        Return 0
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
                If Not value.HasValue Then ' default to 0
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
        ''' Gets or sets the visible screen rectangle's vertical offset.
        ''' </summary>
        <DisplayName("Playback clip vertical offset")>
        <Description("Indicates the visible screen rectangle's vertical offset.")>
        <Category("Playback clip zoom")>
        Public Property PlaybackClipRectangleVerticalOffset As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackClipRectangleVerticalOffset.HasValue Then
                        Return Status.PlaybackClipRectangleVerticalOffset
                    ElseIf Status.OnScreenDisplayHeight.HasValue Then
                        Return 0
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
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
        ''' Gets or sets the visible screen rectangle's width.
        ''' </summary>
        <DisplayName("Playback clip width")>
        <Description("Indicates the visible screen rectangle's width.")>
        <Category("Playback clip zoom")>
        Public Property PlaybackClipRectangleWidth As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackClipRectangleWidth.HasValue Then
                        Return Status.PlaybackClipRectangleWidth
                    Else
                        Return Status.OnScreenDisplayWidth
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
                If Not value.HasValue Then ' default to fullscreen width
                    value = Status.OnScreenDisplayWidth
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
        ''' Gets or sets the visible screen rectangle's height.
        ''' </summary>
        <DisplayName("Playback clip height")>
        <Description("Indicates the visible screen rectangle's height.")>
        <Category("Playback clip zoom")>
        Public Property PlaybackClipRectangleHeight As Short?
            Get
                If IsConnected Then
                    If Status.PlaybackClipRectangleHeight.HasValue Then
                        Return Status.PlaybackClipRectangleHeight
                    Else
                        Return Status.OnScreenDisplayHeight
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
                If Not value.HasValue Then ' default to fullscreen height
                    value = Status.OnScreenDisplayHeight
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
        ''' Gets or sets whether to show video output on top of overlay graphics.
        ''' </summary>
        <DisplayName("Video on top")>
        <Description("Indicates whether video is shown on top of overlay graphics.")>
        <Category("Playback information")>
        Public Property VideoOnTop As Boolean?
            Get
                If IsConnected Then
                    Return Status.VideoOnTop
                Else
                    Return Nothing
                End If
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
        <DisplayName("Subtitles track")>
        <Description("Indicates the active subtitles track.")>
        <Category("Playback information")>
        Public Property SubtitlesTrack As Short?
            Get
                If IsConnected Then
                    Return Status.SubtitlesTrack
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Short?)
                If Not value.HasValue Then
                    value = 0
                End If

                Dim command As New SetPlaybackStateCommand(Me)
                command.SubtitleTrack = value

                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the collection of subtitles in the current playback.
        ''' </summary>
        <DisplayName("Subtitles")>
        <Description("The collection of subtitle tracks in the current playback.")>
        <Category("Playback information")>
        Public ReadOnly Property Subtitles As SortedList(Of Short, LanguageTrack)
            Get
                If IsConnected Then
                    Return Status.Subtitles
                Else
                    Return New SortedList(Of Short, LanguageTrack)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the text in the current input field, if any.
        ''' </summary>
        <DisplayName("Text")>
        <Description("Indicates the text in the text editor.")>
        <Category("Text editor")>
        Public Property Text As String
            Get
                If IsConnected Then
                    Return _text
                Else
                    Return String.Empty
                End If
            End Get
            Set(value As String)
                Dim command As New SetTextCommand(Me, value)
                ProcessCommand(command)
            End Set
        End Property

        ''' <summary>
        ''' Gets the amount of units in the visible screen rectangle's width in relation to the amount of units in its height.
        ''' </summary>
        <DisplayName("Playback clip aspect ratio (width)")>
        <Description("Indicates the amount of units in the visible screen rectangle's width in relation to the amount of units in its height.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property PlaybackClipRectangleAspectRatioWidth As Short?
            Get
                If IsConnected Then
                    If PlaybackClipRectangleWidth.HasValue Then
                        If PlaybackClipRectangleWidth > 0 And PlaybackClipRectangleHeight > 0 Then
                            Dim gcf As Integer = CInt(PlaybackClipRectangleWidth).GetGreatestCommonFactor(CInt(PlaybackClipRectangleHeight))
                            Dim width As Integer = CInt(PlaybackClipRectangleWidth / gcf)
                            Return CShort(width)
                        Else
                            Return 0
                        End If
                    Else
                        Return OnScreenDisplayAspectRatioWidth
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of units in the visible screen rectangle's height in relation to the amount of units in its width.
        ''' </summary>
        <DisplayName("Playback clip aspect ratio (height)")>
        <Description("Indicates the amount of units in the visible screen rectangle's height in relation to the amount of units in its width.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property PlaybackClipRectangleAspectRatioHeight As Short?
            Get
                If IsConnected Then
                    If PlaybackClipRectangleHeight.HasValue Then
                        If PlaybackClipRectangleHeight > 0 And PlaybackClipRectangleHeight > 0 Then
                            Dim gcf As Integer = CInt(PlaybackClipRectangleWidth).GetGreatestCommonFactor(CInt(PlaybackClipRectangleHeight))
                            Dim height As Integer = CInt(PlaybackClipRectangleHeight / gcf)
                            Return CShort(height)
                        Else
                            Return 0
                        End If
                    Else
                        Return OnScreenDisplayAspectRatioHeight
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of units in the current video stream's width in relation to the amount of units in its height.
        ''' </summary>
        <DisplayName("Video aspect ratio (width)")>
        <Description("Indicates the amount of units in the current video stream's width in relation to the amount of units in its height.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property VideoAspectRatioWidth As Short?
            Get
                If IsConnected AndAlso PlaybackVideoWidth.HasValue Then
                    If PlaybackVideoWidth > 0 And PlaybackVideoHeight > 0 Then
                        Dim gcf As Integer = CInt(PlaybackVideoWidth).GetGreatestCommonFactor(CInt(PlaybackVideoHeight))
                        Dim width As Integer = CInt(PlaybackVideoWidth / gcf)
                        Return CShort(width)
                    Else
                        Return 0
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of units in the current video stream's height in relation to the amount of units in its width.
        ''' </summary>
        <DisplayName("Video aspect ratio (height)")>
        <Description("Indicates the amount of units in the current video stream's height in relation to the amount of units in its width.")>
        <Category("Aspect ratio")>
        Public ReadOnly Property VideoAspectRatioHeight As Short?
            Get
                If IsConnected AndAlso PlaybackVideoHeight.HasValue Then
                    If PlaybackVideoWidth > 0 And PlaybackVideoHeight > 0 Then
                        Dim gcf As Integer = CInt(PlaybackVideoWidth).GetGreatestCommonFactor(CInt(PlaybackVideoHeight))
                        Dim width As Integer = CInt(PlaybackVideoHeight / gcf)
                        Return CShort(width)
                    Else
                        Return 0
                    End If
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region ' Properties v3

#Region "Methods v1"

        ''' <summary>
        ''' Gets the player status.
        ''' </summary>
        Public Function GetStatus() As CommandResult
            Dim command As New GetStatusCommand(Me)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Navigates to the previous keyframe during DVD or MKV playback.
        ''' </summary>
        Public Function GoToPreviousKeyframe() As CommandResult
            Dim command As New SetKeyframeCommand(Me, Constants.SetKeyframeValues.Previous)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Navigates to the next keyframe during DVD or MKV playback.
        ''' </summary>
        Public Function GoToNextKeyframe() As CommandResult
            Dim command As New SetKeyframeCommand(Me, Constants.SetKeyframeValues.Next)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Sends a discrete power on command.
        ''' </summary>
        Public Function PowerOn() As CommandResult
            Return RemoteControl.PushSpecialButton(Constants.RemoteControls.SpecialButtonValues.DiscretePowerOn)
        End Function

        ''' <summary>
        ''' Sends a discrete power off command. Whether the device goes into standby mode or full power off mode depends on the user's settings.
        ''' </summary>
        Public Function PowerOff() As CommandResult
            Return RemoteControl.PushSpecialButton(Constants.RemoteControls.SpecialButtonValues.DiscretePowerOff)
        End Function

        ''' <summary>
        ''' Sets the device to standby mode.
        ''' </summary>
        Public Function SetToStandby() As CommandResult
            Dim command As New SetPlayerStateCommand(Me, Constants.CommandValues.Standby)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Sets the device to navigator mode.
        ''' </summary>
        Public Function SetToMainScreen() As CommandResult
            Dim command As New SetPlayerStateCommand(Me, Constants.CommandValues.MainScreen)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Sets the device to black screen mode.
        ''' </summary>
        Public Function SetToBlackScreen() As CommandResult
            Dim command As New SetPlayerStateCommand(Me, Constants.CommandValues.BlackScreen)
            Return ProcessCommand(command)
        End Function

        ''' <summary>
        ''' Navigates through menus.
        ''' </summary>
        ''' <param name="action">The navigation action. Possible values are enumerated in <see cref="Constants.ActionValues"/>.</param>
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

#End Region ' Methods v1

#Region "Methods v2"

        ''' <summary>
        ''' Scales the playback window rectangle to the specified aspect ratio (x:y), but keeps it centered on the display.
        ''' </summary>
        Public Function SetAspectRatio(width As Double, height As Double) As CommandResult
            Dim horizontalLines As Short
            Dim verticalLines As Short
            Dim ratio As Double = width / height
            Dim screenRatio As Double = CDbl(Status.OnScreenDisplayWidth / Status.OnScreenDisplayHeight)

            verticalLines = CShort(Status.OnScreenDisplayHeight)
            horizontalLines = CShort(CDbl(Status.OnScreenDisplayHeight / height) * width)

            If ratio > screenRatio Then
                Dim scaleFactor As Double = screenRatio / ratio
                verticalLines = CShort(Math.Ceiling(verticalLines * scaleFactor))
                horizontalLines = CShort(horizontalLines * scaleFactor)
            End If

            Return ScalePlaybackWindow(horizontalLines, verticalLines)
        End Function

        ''' <summary>
        ''' Scales the playback window to the specified amount of horizontal lines, but keeps it centered on the display while preserving the aspect ratio.
        ''' </summary>
        Public Function ScalePlaybackWindow(height As Short) As CommandResult
            Dim screenRatio As Double = CDbl(Status.OnScreenDisplayWidth / Status.OnScreenDisplayHeight)
            Dim width As Short = CShort(height * screenRatio)
            Return ScalePlaybackWindow(width, height)
        End Function

        ''' <summary>
        ''' Resizes the playback window rectangle to the specified width and height, but keeps it centered on the display.
        ''' </summary>
        Public Function ScalePlaybackWindow(width As Short, height As Short) As CommandResult
            Dim horizontalMargins As Short = CShort((Status.OnScreenDisplayWidth - width) / 2)
            Dim verticalMargins As Short = CShort((Status.OnScreenDisplayHeight - height) / 2)
            Dim command As New SetPlaybackWindowZoomCommand(Me, horizontalMargins, verticalMargins, width, height)
            Return ProcessCommand(command)
        End Function

#End Region ' Methods v2

#Region "Methods v3"

        ''' <summary>
        ''' Gets text from the selected text input field, if any.
        ''' </summary>
        Public Function GetText() As CommandResult
            Dim command As New GetTextCommand(Me)
            Return ProcessCommand(command)
        End Function

        ' ''' <summary>
        ' ''' Tries to get text from the selected text input field.
        ' ''' </summary>
        ' ''' <param name="text">The string variable that will be populated with text from the input field.</param>
        ' ''' <returns>True if text is returned; otherwise false.</returns>
        ' ''' <remarks>I felt that this function is necessary because cmd=get_text is basically the same as a cmd=status, yet can return command errors.</remarks>
        'Public Function TryGetText(ByRef text As String) As Boolean
        '    Dim command As New GetTextCommand(Me)
        '    Dim result As CommandResult = ProcessCommand(command, True)
        '    If result.CommandStatus <> Constants.Status.Failed Then
        '        text = result.Text
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function


        ''' <summary>
        ''' Sets text to the selected text input field, if any.
        ''' </summary>
        Public Function SetText(text As String) As CommandResult
            Dim command As New SetTextCommand(Me, text)
            Return ProcessCommand(command)
        End Function

#End Region ' Methods v3

#Region "IPropertyChanging Support"

        Private Sub RaisePropertyChanging(propertyName As String)
            RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs(propertyName))
        End Sub

        Public Event PropertyChanging(sender As Object, e As System.ComponentModel.PropertyChangingEventArgs) Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

#End Region

#Region "IPropertyChanged Support"

        ''' <summary>
        ''' Helper method helps raising PropertyChanged events.
        ''' </summary>
        ''' <param name="propertyName">The name of the property that changed.</param>
        Private Sub RaisePropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' INotifyPropertyChanged implementation.
        ''' </summary>
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _telnetClient.Dispose()
                    _statusUpdater.Dispose()
                    _textUpdater.Dispose()
                End If

                _telnetClient = Nothing
                _statusUpdater = Nothing
                _textUpdater = Nothing
                _status = Nothing
                _systemInfo = Nothing
                _firmwares = Nothing
                _hostentry = Nothing
                _endpoint = Nothing
                _shares = Nothing
                _remoteControl = Nothing
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

#Region "ISerializable Support"

        Public Overridable Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            info.AddValue("HostEntry", HostEntry)
            info.AddValue("Port", Port)
        End Sub

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            Dim hostEntry As IPHostEntry = DirectCast(info.GetValue("HostEntry", GetType(IPHostEntry)), IPHostEntry)
            Dim port As Integer = info.GetInt32("Port")
            Connect(hostEntry, port)
        End Sub

#End Region

    End Class

End Namespace
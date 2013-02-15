#Region "License"
' Copyright 2012-2013 Steven Liekens
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
Imports System.Net.NetworkInformation
Imports System.ComponentModel
Imports SL.DuneApiCodePack.DuneUtilities.ApiWrappers
Imports SL.DuneApiCodePack.Sources
Imports SL.DuneApiCodePack.Networking
Imports System.Runtime.Serialization
Imports System.Drawing

''' <summary>
''' Provides methods and properties to interact with an IP control-enabled Dune player.
''' </summary>
<Serializable()>
Public Class Dune
    Implements INotifyPropertyChanged
    Implements ISerializable
    Implements IDisposable

#Region "Private Fields"

    ' Connection details
    Private _hostname As String
    Private _endpoint As IPEndPoint
    Private _state As IPControlState

    ' Custom fields
    Private _firmwares As ReadOnlyObservableCollection(Of FirmwareProperties)
    Private _shares As ReadOnlyObservableCollection(Of LocalStorage)

    Private _networkAdapterInfo As NetworkInterfaceInfo
    Private _systemInfo As SystemInformation

#End Region ' Private Fields

#Region "Constructors"

    Private Sub New()
        ' memo: general initialization
        PlaybackStateChangedHandlers = New List(Of EventHandler(Of PlaybackStateChangedEventArgs))
        PlayerStateChangedHandlers = New List(Of EventHandler(Of PlayerStateChangedEventArgs))
        ClientStateChangedHandlers = New List(Of EventHandler(Of ClientStateChangedEventArgs))

        _state = New IPControlStateDisconnected(Me)
    End Sub

    Public Sub New(endPoint As IPEndPoint)
        Me.New()
        Me.EndPoint = endPoint
    End Sub

    Private Sub New(host As IPHostEntry, port As Integer)
        Me.New()
        If port < IPEndPoint.MinPort Or port > IPEndPoint.MaxPort Then
            Throw New ArgumentOutOfRangeException("port")
        End If
        Dim ip = host.AddressList.Single(Function(address) address.AddressFamily = Sockets.AddressFamily.InterNetwork)


        With Me
            .EndPoint = New IPEndPoint(ip, port)
            .HostName = host.HostName.Split("."c).First
        End With
    End Sub

#End Region ' Constructors

#Region "Initialization"

    Public Shared Function Connect(hostNameOrAddress As String) As Dune
        Return Dune.Connect(hostNameOrAddress, 80)
    End Function

    Public Shared Function Connect(hostNameOrAddress As String, port As Integer) As Dune
        If String.IsNullOrWhiteSpace(hostNameOrAddress) Then
            Throw New ArgumentNullException("hostNameOrAddress")
        End If

        Dim hostEntry = Dns.GetHostEntry(hostNameOrAddress)
        Return Dune.Connect(hostEntry, port)
    End Function

    Public Shared Function Connect(address As IPAddress) As Dune
        Return Dune.Connect(address, 80)
    End Function

    Public Shared Function Connect(address As IPAddress, port As Integer) As Dune
        If address Is Nothing Then
            Throw New ArgumentNullException("address")
        End If

        Dim hostEntry = Dns.GetHostEntry(address)
        Return Dune.Connect(hostEntry, port)
    End Function

    Private Shared Function Connect(hostEntry As IPHostEntry, port As Integer) As Dune
        Return New Dune(hostEntry, port).Initialize()
    End Function




    Public Shared Function ConnectAsync(hostNameOrAddress As String) As Task(Of Dune)
        Return Dune.ConnectAsync(hostNameOrAddress, 80)
    End Function

    Public Shared Async Function ConnectAsync(hostNameOrAddress As String, port As Integer) As Task(Of Dune)
        If String.IsNullOrWhiteSpace(hostNameOrAddress) Then
            Throw New ArgumentNullException("hostNameOrAddress")
        End If

        Dim hostEntry = Await Dns.GetHostEntryAsync(hostNameOrAddress).ConfigureAwait(False)
        Return Await Dune.ConnectAsync(hostEntry, port).ConfigureAwait(False)
    End Function

    Public Shared Function ConnectAsync(address As IPAddress) As Task(Of Dune)
        Return Dune.ConnectAsync(address, 80)
    End Function

    Public Shared Async Function ConnectAsync(address As IPAddress, port As Integer) As Task(Of Dune)
        If address Is Nothing Then
            Throw New ArgumentNullException("address")
        End If

        Dim hostEntry = Await Dns.GetHostEntryAsync(address).ConfigureAwait(False)
        Return Await Dune.ConnectAsync(hostEntry, port).ConfigureAwait(False)
    End Function

    Private Shared Async Function ConnectAsync(hostEntry As IPHostEntry, port As Integer) As Task(Of Dune)
        Return Await New Dune(hostEntry, port).InitializeAsync().ConfigureAwait(False)
    End Function

    Private Function Initialize() As Dune
        With Me
            .NetworkAdapterInfo = Networking.NetworkInterfaceInfo.FromHost(Me.EndPoint.Address)
            .State.Connect()
            Try
                .TelnetClient.Connect()
                If .TelnetClient.IsConnected Then
                    .SystemInfo = SystemInformation.FromHost(Me)
                End If
                .AvailableFirmwares = FirmwareProperties.FromHost(Me).ToObservableCollection.AsReadOnly
            Catch ex As Sockets.SocketException
                Me.SystemInfo = SystemInformation.FromHost(Me)


                Me.AvailableFirmwares = New Collection(Of FirmwareProperties)().ToObservableCollection.AsReadOnly
            End Try

        End With

        Return Me
    End Function

    Private Async Function InitializeAsync() As Task(Of Dune)
        With Me
            .NetworkAdapterInfo = Await Networking.NetworkInterfaceInfo.FromHostAsync(Me.EndPoint.Address).ConfigureAwait(False)
            Await .State.ConnectAsync
            Await TelnetClient.ConnectAsync
            If .TelnetClient.IsConnected Then
                .SystemInfo = SystemInformation.FromHost(Me)
            End If
            .AvailableFirmwares = (Await FirmwareProperties.GetFirmwareCollectionAsync(Me.ProductId)).ToObservableCollection.AsReadOnly
        End With
        Return Me
    End Function

#End Region ' Initialization

    Public Sub Connect()
        If Not Me.IsValidHost Then
            Throw New InvalidOperationException("Can't find an IP control-enabled device at the specified address")
        End If
        Me.State.Connect()
    End Sub

    Public Async Function ConnectAsync() As Task
        If Not Await Me.IsValidHostAsync.ConfigureAwait(False) Then
            Throw New InvalidOperationException("Can't find an IP control-enabled device at the specified address")
        End If
        Await Me.State.ConnectAsync().ConfigureAwait(False)
    End Function

    Public Sub Disconnect()
        Me.State.Disconnect()
    End Sub

#Region "App Properties"


    Friend Property State As IPControlState
        Get
            Return _state
        End Get
        Set(value As IPControlState)
            If value IsNot Nothing Then
                Dim previousState = New ClientState(Me.State)
                Me.State.Dispose()
                _state = value
                Dim currentState = New ClientState(Me.State)
                RaiseEvent ClientStateChanged(Me, New ClientStateChangedEventArgs(previousState, currentState))
            End If
        End Set
    End Property

    Private ReadOnly PlaybackStateChangedHandlers As List(Of EventHandler(Of PlaybackStateChangedEventArgs))

    Public Custom Event PlaybackStateChanged As EventHandler(Of PlaybackStateChangedEventArgs)
        AddHandler(value As EventHandler(Of PlaybackStateChangedEventArgs))
            If Not PlaybackStateChangedHandlers.Contains(value) Then
                Me.PlaybackStateChangedHandlers.Add(value)
            End If
        End AddHandler

        RemoveHandler(value As EventHandler(Of PlaybackStateChangedEventArgs))
            If PlaybackStateChangedHandlers.Contains(value) Then
                Me.PlaybackStateChangedHandlers.Remove(value)
            End If
        End RemoveHandler

        RaiseEvent(sender As Object, e As PlaybackStateChangedEventArgs)
#If DEBUG Then
            Debug.Print("Playback state changed (current: <{0}>, previous: <{1}>, last playback event: <{2}>)", e.PlaybackState, e.PreviousPlaybackState, e.LastPlaybackEvent)
#End If
            For Each handler In PlaybackStateChangedHandlers
                handler.Invoke(sender, e)
            Next
        End RaiseEvent
    End Event

    Friend Sub SignalPlaybackStateChanged(playbackState As PlaybackState, previousPlaybackState As PlaybackState, lastPlaybackEvent As PlaybackEvent)
        RaiseEvent PlaybackStateChanged(Me, New PlaybackStateChangedEventArgs(playbackState, previousPlaybackState, lastPlaybackEvent))
    End Sub

    Private ReadOnly PlayerStateChangedHandlers As List(Of EventHandler(Of PlayerStateChangedEventArgs))

    Public Custom Event PlayerStateChanged As EventHandler(Of PlayerStateChangedEventArgs)
        AddHandler(value As EventHandler(Of PlayerStateChangedEventArgs))
            If Not PlayerStateChangedHandlers.Contains(value) Then
                Me.PlayerStateChangedHandlers.Add(value)
            End If
        End AddHandler

        RemoveHandler(value As EventHandler(Of PlayerStateChangedEventArgs))
            If PlayerStateChangedHandlers.Contains(value) Then
                Me.PlayerStateChangedHandlers.Remove(value)
            End If
        End RemoveHandler

        RaiseEvent(sender As Object, e As PlayerStateChangedEventArgs)
            Dim updates = e.PlayerState.GetDifferences(e.PreviousPlayerState)
            If updates.Count > 0 Then RaisePropertyChanged(updates)

            For Each handler In PlayerStateChangedHandlers
                handler.Invoke(sender, e)
            Next
        End RaiseEvent
    End Event

    Friend Sub SignalPlayerStateChanged(playerState As StatusCommandResult, previousPlayerState As StatusCommandResult)
        RaiseEvent PlayerStateChanged(Me, New PlayerStateChangedEventArgs(playerState, previousPlayerState))
    End Sub

    Private ReadOnly ClientStateChangedHandlers As List(Of EventHandler(Of ClientStateChangedEventArgs))

    Public Custom Event ClientStateChanged As EventHandler(Of ClientStateChangedEventArgs)
        AddHandler(value As EventHandler(Of ClientStateChangedEventArgs))
            If Not ClientStateChangedHandlers.Contains(value) Then
                Me.ClientStateChangedHandlers.Add(value)
            End If
        End AddHandler

        RemoveHandler(value As EventHandler(Of ClientStateChangedEventArgs))
            If ClientStateChangedHandlers.Contains(value) Then
                Me.ClientStateChangedHandlers.Remove(value)
            End If
        End RemoveHandler

        RaiseEvent(sender As Object, e As ClientStateChangedEventArgs)
#If DEBUG Then
            If e.ClientState.IsConnected <> e.PreviousClientState.IsConnected Then
                If e.ClientState.IsConnected Then
                    Debug.Print("State change: connected")
                Else
                    Debug.Print("State change: disconnected")
                End If
            End If
#End If
            RaisePropertyChanged("IsConnected")
            For Each handler In ClientStateChangedHandlers
                handler.Invoke(sender, e)
            Next
        End RaiseEvent
    End Event

    ''' <summary>
    ''' Gets the IP address of the device.
    ''' </summary>
    <DisplayName("IP endpoint")>
    <Description("Indicates the IP address and port on which the app is connected to the HTTP service.")>
    <Category("Connection details")>
    Public Property EndPoint As IPEndPoint
        Get
            Return _endpoint
        End Get
        Private Set(value As IPEndPoint)
            _endpoint = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the physical address of the device.
    ''' </summary>
    <DisplayName("MAC address")>
    <Description("Indicates the hardware address of the device's network interface.")>
    <Category("Connection details")>
    <TypeConverter(GetType(PhysicalAddressConverter))>
    Public ReadOnly Property PhysicalAddress As PhysicalAddress
        Get
            Return Me.NetworkAdapterInfo.PhysicalAddress
        End Get
    End Property

    ''' <summary>
    ''' Gets the hostname of the device.
    ''' </summary>
    <DisplayName("Host name")>
    <Description("Indicates the device's host name.")>
    <Category("Connection details")>
    Public Property HostName As String
        Get
            Return _hostname
        End Get
        Private Set(value As String)
            _hostname = value
        End Set
    End Property

    ''' <summary>
    ''' Gets information about the device's network interface.
    ''' </summary>
    <Browsable(False)>
    Public Property NetworkAdapterInfo As NetworkInterfaceInfo
        Get
            Return _networkAdapterInfo
        End Get
        Private Set(value As NetworkInterfaceInfo)
            If Not value.Equals(_networkAdapterInfo) Then
                _networkAdapterInfo = value
                RaisePropertyChanged("NetworkAdapterInfo")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets a list of network shares made available by the player's SMB server.
    ''' </summary>
    <DisplayName("Network shares")>
    <Description("Indicates the collection of network shares exposed by the device's SMB server.")>
    <Category("Connection details")>
    Public ReadOnly Property NetworkShares As ReadOnlyObservableCollection(Of LocalStorage)
        Get
            If _shares Is Nothing Then
                _shares = New ObservableCollection(Of LocalStorage)(LocalStorage.FromHost(Me)).AsReadOnly
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
    Public Property IsConnected As Boolean
        Get
            Return Me.State.IsConnected
        End Get
        Set(value As Boolean)
            If value Then
                Me.State.Connect()
            Else
                Me.State.Disconnect()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the remaining playback time.
    ''' </summary>
    <DisplayName("Playback time remaining")>
    <Description("Indicates the remaining playback time.")>
    <Category("Playback information")>
    Public ReadOnly Property PlaybackTimeRemaining As TimeSpan?
        Get
            Return Me.State.PlaybackTimeRemaining
        End Get
    End Property

    ''' <summary>
    ''' Gets the amount of seconds before a command returns with a timeout.
    ''' </summary>
    ''' <value>A positive value bigger than 1</value>
    ''' <remarks>The default value is 20 seconds.</remarks>
    <DisplayName("Timeout")>
    <Description("Indicates the duration before a command returns a timeout.")>
    <Category("Connection details")>
    Public Property Timeout As TimeSpan
        Get
            Return Command.DefaultTimeout
        End Get
        Set(value As TimeSpan)
            Command.DefaultTimeout = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the interval between status updates.
    ''' </summary>
    <DisplayName("Update interval")>
    <Description("Indicates the timespan between status updates.")>
    <Category("Connection details")>
    Public Property Interval As Double
        Get
            Return Me.State.Interval
        End Get
        Set(value As Double)
            Me.State.Interval = value
        End Set
    End Property

    ''' <summary>
    ''' Gets a list of available firmwares. The <see cref="ProductID" /> property must be set to populate the collection.
    ''' </summary>
    <DisplayName("Available firmwares")>
    <Description("Indicates the collection of available firmware versions for this device.")>
    <Category("Firmware information")>
    Public Property AvailableFirmwares As ReadOnlyObservableCollection(Of FirmwareProperties)
        Get
            Return _firmwares
        End Get
        Private Set(value As ReadOnlyObservableCollection(Of FirmwareProperties))
            _firmwares = value
        End Set
    End Property

    ''' <summary>
    ''' Gets whether there is a firmware update available.
    ''' </summary>
    <DisplayName("Firmware update available")>
    <Description("Indicates whether a firmware update is available.")>
    <Category("Firmware information")>
    Public ReadOnly Property UpdateAvailable As Boolean?
        Get
            If AvailableFirmwares.Count > 0 Then
                Return Not Object.Equals(FirmwareVersion, AvailableFirmwares.First)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets whether the text editor is currently open.
    ''' </summary>
    <DisplayName("Text field active")>
    <Description("Indicates whether a text field is currently active.")>
    <Category("Text editor")>
    Public ReadOnly Property IsTextFieldActive As Boolean?
        Get
            Return Me.State.IsTextFieldActive
        End Get
    End Property

#End Region ' App Properties

#Region "App methods"

    Public Function IsValidHost() As Boolean
        Return IPControlClient.GetInstance().ServiceReturnsXmlAsync(Me.GetBaseAddress.Uri).Result
    End Function

    Public Shared Function IsValidHost(address As IPAddress) As Boolean
        Return Dune.IsValidHost(address, 80)
    End Function

    Public Shared Function IsValidHost(address As IPAddress, port As Integer) As Boolean
        If address Is Nothing Then
            Return False
        End If
        If address.AddressFamily <> Sockets.AddressFamily.InterNetwork Then
            Return False
        End If
        If port < IPEndPoint.MinPort Or port > IPEndPoint.MaxPort Then
            Return False
        End If
        Return Dune.IsValidHost(New IPEndPoint(address, port))
    End Function

    Public Shared Function IsValidHost(endpoint As IPEndPoint) As Boolean
        Try
            Using device As New Dune(endpoint)
                Return IPControlClient.GetInstance().ServiceReturnsXmlAsync(device.GetBaseAddress.Uri).Result
            End Using
        Catch
            Return False
        End Try
    End Function

    Public Function IsValidHostAsync() As Task(Of Boolean)
        Return IPControlClient.GetInstance().ServiceReturnsXmlAsync(Me.GetBaseAddress.Uri)
    End Function

    Public Shared Function IsValidHostAsync(address As IPAddress) As Task(Of Boolean)
        Return Dune.IsValidHostAsync(address, 80)
    End Function

    Public Shared Async Function IsValidHostAsync(address As IPAddress, port As Integer) As Task(Of Boolean)
        If address Is Nothing Then
            Return False
        End If
        If address.AddressFamily <> Sockets.AddressFamily.InterNetwork Then
            Return False
        End If
        If port < IPEndPoint.MinPort Or port > IPEndPoint.MaxPort Then
            Return False
        End If

        Return Await Dune.IsValidHostAsync(New IPEndPoint(address, port)).ConfigureAwait(False)
    End Function

    Public Shared Async Function IsValidHostAsync(endPoint As IPEndPoint) As Task(Of Boolean)
        Try
            Using device As New Dune(endPoint)
                Dim pingReply = Await IPv4Network.PingAsync(device.EndPoint.Address).ConfigureAwait(False)
                If pingReply.Status = IPStatus.Success Then
                    Return Await IPControlClient.GetInstance().ServiceReturnsXmlAsync(device.GetBaseAddress.Uri).ConfigureAwait(False)
                End If
                Return False
            End Using
        Catch
            Return False
        End Try
    End Function

    Public Shared Async Function AutoDiscoverAsync() As Task(Of IEnumerable(Of IPAddress))
        Dim checkValidity As New Dictionary(Of IPAddress, Task(Of Boolean))

        For Each network In IPv4Network.GetNetworks
            For Each host In network
                checkValidity.Add(host, Dune.IsValidHostAsync(host))
            Next
        Next
        Await Task.WhenAll(checkValidity.Values).ConfigureAwait(False)

        Dim players As New List(Of IPAddress)

        For Each address In checkValidity.Where(Function(kvp) kvp.Value.Result = True)
            players.Add(address.Key)
        Next

        Return players
    End Function

    'Public Shared Async Function AutoDiscoverAsync() As Task(Of IEnumerable(Of IPAddress))
    '    Dim pingTasks As New List(Of Task(Of IEnumerable(Of IPAddress)))
    '    For Each network In IPv4Network.GetNetworks
    '        pingTasks.Add(IPv4Network.GetReachableHostsAsync(network))
    '    Next
    '    Await Task.WhenAll(pingTasks).ConfigureAwait(False)

    '    Dim httpTasks As New Dictionary(Of IPAddress, Task(Of Boolean))
    '    For Each t In pingTasks
    '        For Each address In t.Result
    '            httpTasks.Add(address, Dune.IsValidHostAsync(address, 80))
    '        Next
    '    Next
    '    Await Task.WhenAll(httpTasks.Values).ConfigureAwait(False)

    '    Dim players As New List(Of IPAddress)

    '    For Each address In httpTasks.Where(Function(kvp) kvp.Value.Result = True)
    '        players.Add(address.Key)
    '    Next

    '    Return players
    'End Function

    Public Function GetBaseAddress() As UriBuilder
        Dim builder = GetBaseAddressBuilder()
        builder.Path &= "/do"

        Return builder
    End Function

    Public Function GetBaseAddress(plugin As String, executable As String) As UriBuilder
        Dim builder = GetBaseAddressBuilder()
        builder.Path &= "/plugins/" + plugin + "/" + executable
        Return builder
    End Function

    Private Function GetBaseAddressBuilder() As UriBuilder
        Dim builder As New UriBuilder
        builder.Scheme = Uri.UriSchemeHttp

        If Me.EndPoint IsNot Nothing Then
            builder.Host = Me.EndPoint.Address.ToString
        Else
            builder.Host = Me.HostName
        End If

        builder.Port = Me.EndPoint.Port
        builder.Path = "/cgi-bin"

        Return builder
    End Function

#End Region ' App methods

#Region "Telnet Properties"

    ''' <summary>
    ''' Gets a preconfigured Telnet client instance.
    ''' </summary>
    <Browsable(False)>
    Public ReadOnly Property TelnetClient As TelnetClient
        Get
            Return Networking.TelnetClient.GetInstance(Me)
        End Get
    End Property

    <Browsable(False)>
    Private Property SystemInfo As SystemInformation
        Get
            Return _systemInfo
        End Get
        Set(value As SystemInformation)
            _systemInfo = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the player's product ID (using a Telnet connection).
    ''' </summary>
    <DisplayName("Product ID")>
    <Description("Indicates the device's product ID.")>
    <Category("Player information")>
    <TypeConverter(GetType(ProductConverter))>
    Public Property ProductId As ProductID
        Get
            If SystemInfo IsNot Nothing Then
                Return SystemInfo.ProductID
            Else
                Return New ProductID(String.Empty)
            End If
        End Get
        Set(value As ProductID)
            Me.AvailableFirmwares = FirmwareProperties.GetFirmwareCollectionAsync(value).Result.ToObservableCollection.AsReadOnly
        End Set
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
                Return SystemInfo.SerialNumber
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
    Public ReadOnly Property FirmwareVersion As FirmwareProperties
        Get
            If SystemInfo IsNot Nothing AndAlso Not String.IsNullOrEmpty(SystemInfo.Firmware) Then
                Return AvailableFirmwares.Where(Function(fw) fw.Version.EqualsInvariantIgnoreCase(SystemInfo.Firmware)).Single
            Else
                Return Nothing
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
    <DisplayName("Operating time")>
    <Description("Indicates the amount of elapsed time since the device powered on.")>
    <Category("Player information")>
    Public ReadOnly Property OperatingTime As TimeSpan?
        Get
            If SystemInfo IsNot Nothing AndAlso SystemInfo.BootTime <> Nothing Then
                Dim start As New TimeSpan(BootTime.Ticks)
                Dim current As New TimeSpan(Date.Now.Ticks)
                Return current.Subtract(start).RoundToSecond
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region ' Telnet Properties

#Region "Properties v1"

    ''' <summary>
    ''' Gets the protocol version number.
    ''' </summary>
    <DisplayName("Protocol version")>
    <Description("Indicates the API version.")>
    <Category("Firmware information")>
    Public ReadOnly Property ProtocolVersion As Version
        Get
            Return Me.State.ProtocolVersion
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
            Return Me.State.PlaybackDuration
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
            Return Me.State.PlaybackPosition
        End Get
        Set(value As System.TimeSpan?)
            Me.State.PlaybackPosition = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the playback speed.
    ''' </summary>
    <DisplayName("Playback speed")>
    <Description("Indicates the playback speed.")>
    <Category("Playback information")>
    <TypeConverter(GetType(PlaybackSpeedConverter))>
    Public Property PlaybackSpeed As PlaybackSpeed
        Get
            Return Me.State.PlaybackSpeed
        End Get
        Set(value As PlaybackSpeed)
            Me.State.PlaybackSpeed = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the player state.
    ''' </summary>
    <DisplayName("Player state")>
    <Description("Indicates the player state.")>
    <Category("Player information")>
    <TypeConverter(GetType(PlayerStateConverter))>
    Public Property PlayerState As IPControl.PlayerState
        Get
            Return Me.State.PlayerState
        End Get
        Set(value As IPControl.PlayerState)
            Me.State.PlayerState = value
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
            Return Me.State.PlaybackDvdMenu
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
            Return Me.State.PlaybackBlurayMenu
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
            Return Me.State.PlaybackIsBuffering
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets whether to show a black screen during playback.
    ''' </summary>
    <DisplayName("Enable display")>
    <Description("Indicates whether to show a black screen.")>
    <Category("Playback information")>
    Public Property DisplayEnabled As Boolean?
        Get
            Return Me.State.DisplayEnabled
        End Get
        Set(value As Boolean?)
            Me.State.DisplayEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether to hide the OSD during playback.
    ''' </summary>
    <DisplayName("Enable on-screen display")>
    <Description("Indicates whether to hide overlay graphics.")>
    <Category("Playback information")>
    Public Property OnScreenDisplayEnabled As Boolean?
        Get
            Return Me.State.OnScreenDisplayEnabled
        End Get
        Set(value As Boolean?)
            Me.State.OnScreenDisplayEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether to repeat playback.
    ''' </summary>
    <DisplayName("Repeat playback")>
    <Description("Indicates whether to repeat playback.")>
    <Category("Playback information")>
    Public Property RepeatPlayback As Boolean?
        Get
            Return Me.State.RepeatPlayback
        End Get
        Set(value As Boolean?)
            Me.State.RepeatPlayback = value
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
    Public Property PlaybackVolume As Integer?
        Get
            Return Me.State.PlaybackVolume
        End Get
        Set(value As Integer?)
            Me.State.PlaybackVolume = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the mute status for the current playback.
    ''' </summary>
    <DisplayName("Audio enabled")>
    <Description("Indicates whether audio output is enabled.")>
    <Category("Playback information")>
    Public Property AudioEnabled As Boolean?
        Get
            Return Me.State.AudioEnabled
        End Get
        Set(value As Boolean?)
            Me.State.AudioEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the current audio track number.
    ''' </summary>
    <DisplayName("Audio track")>
    <Description("Indicates the active audio track. Not to be confused with the track number in a playlist.")>
    <Category("Playback information")>
    Public Property AudioTrack As Integer?
        Get
            Return Me.State.AudioTrack
        End Get
        Set(value As Integer?)
            Me.State.AudioTrack = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the collection of audio tracks in the current playback.
    ''' </summary>
    ''' <returns>An instance of a <see cref="SortedDictionary(Of Integer, CultureInfo)" /> object that represents the list of available audio tracks.</returns>
    <DisplayName("Audio tracks")>
    <Description("The collection of audio tracks in the current playback.")>
    <Category("Playback information")>
    Public ReadOnly Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream)
        Get
            Return Me.State.AudioTracks
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
            Return Me.State.VideoEnabled
        End Get
        Set(value As Boolean?)
            Me.State.VideoEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the total display size.
    ''' </summary>
    <DisplayName("Display size")>
    <Description("Indicates the total available display width and height.")>
    <Category("Player information")>
    Public ReadOnly Property DisplaySize As Size?
        Get
            Return Me.State.DisplaySize
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets whether custom playback window zoom settings are applied.
    ''' </summary>
    <DisplayName("Fullscreen")>
    <Description("Indicates whether custom playback window zoom settings are applied.")>
    <Category("Playback information")>
    Public Property PlaybackWindowFullscreen As Boolean?
        Get
            Return Me.State.PlaybackWindowFullscreen
        End Get
        Set(value As Boolean?)
            Me.State.PlaybackWindowFullscreen = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the playback window rectangle.
    ''' </summary>
    <DisplayName("Playback window rectangle")>
    <Description("Indicates the playback window rectangle's location and size.")>
    <Category("Playback information")>
    Public Property PlaybackWindowRectangle As Rectangle?
        Get
            Return Me.State.PlaybackWindowRectangle
        End Get
        Set(value As Rectangle?)
            Dim requested = GetCenteredRectangle(value, Me.PlaybackWindowRectangle)
            Me.State.PlaybackWindowRectangle = requested
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the video zoom preset.
    ''' </summary>
    <DisplayName("Video zoom")>
    <Description("Indicates the video zoom mode.")>
    <Category("Playback information")>
    <TypeConverter(GetType(ZoomConverter))>
    Public Property VideoZoom As VideoZoom
        Get
            Return Me.State.VideoZoom
        End Get
        Set(value As VideoZoom)
            Me.State.VideoZoom = value
        End Set
    End Property

#End Region 'Properties v2

#Region "Properties v3"

    ''' <summary>
    ''' Gets the current playback state.
    ''' </summary>
    <DisplayName("Playback state")>
    <Description("Indicates the current playback state.")>
    <Category("Playback information")>
    Public ReadOnly Property PlaybackState As PlaybackState
        Get
            Return Me.State.PlaybackState
        End Get
    End Property

    ''' <summary>
    ''' Gets the previous playback state.
    ''' </summary>
    <DisplayName("Previous playback state")>
    <Description("Indicates the previous playback state.")>
    <Category("Playback information")>
    Public ReadOnly Property PreviousPlaybackState As PlaybackState
        Get
            Return Me.State.PreviousPlaybackState
        End Get
    End Property

    ''' <summary>
    ''' Gets the last playback event.
    ''' </summary>
    <DisplayName("Last playback event")>
    <Description("Indicates the last playback event.")>
    <Category("Playback information")>
    <TypeConverter(GetType(PlaybackEventConverter))>
    Public ReadOnly Property LastPlaybackEvent As PlaybackEvent
        Get
            Return Me.State.LastPlaybackEvent
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
            Return Me.State.PlaybackUrl
        End Get
        Set(value As String)
            Me.State.PlaybackUrl = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the current video stream's horizontal resolution.
    ''' </summary>
    <DisplayName("Video size")>
    <Description("Indicates the current video stream's display resolution.")>
    <Category("Playback information")>
    Public ReadOnly Property PlaybackVideoSize As Size?
        Get
            Return Me.State.PlaybackVideoSize
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the playback window rectangle.
    ''' </summary>
    <DisplayName("Playback clip rectangle")>
    <Description("Indicates the playback clip rectangle's location and size.")>
    <Category("Playback information")>
    Public Property PlaybackClipRectangle As Rectangle?
        Get
            Return Me.State.PlaybackClipRectangle
        End Get
        Set(value As Rectangle?)
            Dim requested = GetCenteredRectangle(value, Me.PlaybackClipRectangle)
            Me.State.PlaybackClipRectangle = requested
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
            Return Me.State.VideoOnTop
        End Get
        Set(value As Boolean?)
            Me.State.VideoOnTop = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the active subtitle track.
    ''' </summary>
    <DisplayName("Subtitles enabled")>
    <Description("Indicates whether subtitles are enabled.")>
    <Category("Playback information")>
    Public Property SubtitlesEnabled As Boolean?
        Get
            Return Me.State.SubtitlesEnabled
        End Get
        Set(value As Boolean?)
            Me.State.SubtitlesEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the active subtitle track.
    ''' </summary>
    <DisplayName("Subtitles track")>
    <Description("Indicates the active subtitles track.")>
    <Category("Playback information")>
    Public Property SubtitlesTrack As Integer?
        Get
            Return Me.State.SubtitlesTrack
        End Get
        Set(value As Integer?)
            Me.State.SubtitlesTrack = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the collection of subtitles in the current playback.
    ''' </summary>
    <DisplayName("Subtitles")>
    <Description("The collection of subtitle tracks in the current playback.")>
    <Category("Playback information")>
    Public ReadOnly Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
        Get
            Return Me.State.SubtitlesTracks
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
            Return Me.State.Text
        End Get
        Set(value As String)
            Me.State.Text = value
        End Set
    End Property

#End Region ' Properties v3

#Region "Methods v1"

    Public Function GetStatus() As StatusCommandResult
        Return Me.State.GetStatus
    End Function

    Public Function GetStatusAsync() As Task(Of StatusCommandResult)
        Return Me.State.GetStatusAsync
    End Function

    Public Function SetPlayerState(state As PlayerState) As StatusCommandResult
        Return Me.State.SetPlayerState(state)
    End Function

    Public Function SetPlayerStateAsync(state As PlayerState) As Task(Of StatusCommandResult)
        Return Me.State.SetPlayerStateAsync(state)
    End Function

    Public Function SetPlaybackSpeed(speed As PlaybackSpeed) As StatusCommandResult
        Return Me.State.SetPlaybackSpeed(speed)
    End Function

    Public Function SetPlaybackSpeedAsync(speed As PlaybackSpeed) As Task(Of StatusCommandResult)
        Return Me.State.SetPlaybackSpeedAsync(speed)
    End Function

    Public Function SetPlaybackPosition(position As TimeSpan) As StatusCommandResult
        Return Me.State.SetPlaybackPosition(position)
    End Function

    Public Function SetPlaybackPositionAsync(position As TimeSpan) As Task(Of StatusCommandResult)
        Return Me.State.SetPlaybackPositionAsync(position)
    End Function

    Public Function SetOnScreenDisplayEnabled(enabled As Boolean) As StatusCommandResult
        Return Me.State.SetOnScreenDisplayEnabled(enabled)
    End Function

    Public Function SetOnScreenDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Return Me.State.SetOnScreenDisplayEnabledAsync(enabled)
    End Function

    Public Function SetDisplayEnabled(enabled As Boolean) As StatusCommandResult
        Return Me.State.SetDisplayEnabled(enabled)
    End Function

    Public Function SetDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Return Me.State.SetDisplayEnabledAsync(enabled)
    End Function

    Public Function SetActionOnFinish(action As ActionOnFinish) As StatusCommandResult
        Return Me.State.SetActionOnFinish(action)
    End Function

    Public Function SetActionOnFinishAsync(action As ActionOnFinish) As Task(Of StatusCommandResult)
        Return Me.State.SetActionOnFinishAsync(action)
    End Function

    Public Function SkipToKeyframe(direction As SkipFrames) As StatusCommandResult
        Return Me.State.SkipToKeyframe(direction)
    End Function

    Public Function SkipToKeyframeAsync(direction As SkipFrames) As Task(Of StatusCommandResult)
        Return Me.State.SkipToKeyframeAsync(direction)
    End Function

    ''' <summary>
    ''' Navigates through menus.
    ''' </summary>
    ''' <param name="action">Accepted values: left, right, up, down, enter.</param>
    ''' <remarks>
    ''' The actual HTTP query produced is context sensitive.
    ''' If the device is in DVD playback, it will be a 'dvd_navigation' command.
    ''' Similarly, a 'bluray_navigation' command is created when the device is in Blu-Ray playback mode.
    ''' In all other cases, a remote control button is emulated ('ir_code' command).
    ''' </remarks>
    Public Function Navigate(action As NavigationAction) As StatusCommandResult
        Return Me.State.Navigate(action)
    End Function

    Public Function NavigateAsync(action As NavigationAction) As Task(Of StatusCommandResult)
        Return Me.State.NavigateAsync(action)
    End Function

    Public Function SendKey(key As RemoteControlKey) As StatusCommandResult
        Return Me.State.SendKey(key)
    End Function

    Public Function SendKeyAsync(key As RemoteControlKey) As Task(Of StatusCommandResult)
        Return Me.State.SendKeyAsync(key)
    End Function

#End Region ' Methods v1

#Region "Methods v2"

    Public Function SetPlaybackVolume(volume As Integer) As StatusCommandResult
        Return Me.State.SetPlaybackVolume(volume)
    End Function

    Public Function SetPlaybackVolumeAsync(volume As Integer) As Task(Of StatusCommandResult)
        Return Me.State.SetPlaybackVolumeAsync(volume)
    End Function

    Public Function SetAudioEnabled(enabled As Boolean) As StatusCommandResult
        Return Me.State.SetAudioEnabled(enabled)
    End Function

    Public Function SetAudioEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Return Me.State.SetAudioEnabledAsync(enabled)
    End Function

    Public Function SetAudioTrack(index As Integer) As StatusCommandResult
        Return Me.State.SetAudioTrack(index)
    End Function

    Public Function SetAudioTrackAsync(index As Integer) As Task(Of StatusCommandResult)
        Return Me.State.SetAudioTrackAsync(index)
    End Function

    Public Function SetAudioTrack(track As MediaStream) As StatusCommandResult
        If track Is Nothing Then
            Throw New ArgumentNullException("track")
        ElseIf Not String.Equals(track.Type, Output.AudioTrack.Name, StringComparison.InvariantCultureIgnoreCase) Then
            Throw New ArgumentException
        End If
        Return Me.SetAudioTrack(track.Number)
    End Function

    Public Function SetAudioTrackAsync(track As MediaStream) As Task(Of StatusCommandResult)
        If track Is Nothing Then
            Throw New ArgumentNullException("track")
        ElseIf Not String.Equals(track.Type, Output.AudioTrack.Name, StringComparison.InvariantCultureIgnoreCase) Then
            Throw New ArgumentException
        End If
        Return Me.SetAudioTrackAsync(track.Number)
    End Function

    Public Function SetVideoEnabled(enabled As Boolean) As StatusCommandResult
        Return Me.State.SetVideoEnabled(enabled)
    End Function

    Public Function SetVideoEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Return Me.State.SetVideoEnabledAsync(enabled)
    End Function

    Public Function SetVideoZoom(zoom As VideoZoom) As StatusCommandResult
        Return Me.State.SetVideoZoom(zoom)
    End Function

    Public Function SetVideoZoomAsync(zoom As VideoZoom) As Task(Of StatusCommandResult)
        Return Me.State.SetVideoZoomAsync(zoom)
    End Function

#End Region ' Methods v2

#Region "Methods v3"

    Public Function GetText() As TextCommandResult
        Return Me.State.GetText
    End Function

    Public Function GetTextAsync() As Task(Of TextCommandResult)
        Return Me.State.GetTextAsync
    End Function

    Public Function SetText(text As String) As TextCommandResult
        Return Me.State.SetText(text)
    End Function

    Public Function SetTextAsync(text As String) As Task(Of TextCommandResult)
        Return Me.State.SetTextAsync(text)
    End Function

    Public Function OpenWebBrowser() As StatusCommandResult
        Return Me.OpenWebBrowser(New Uri("http://dune-hd.com"))
    End Function

    Public Function OpenWebBrowser(location As Uri) As StatusCommandResult
        Return Me.State.OpenWebBrowser(location)
    End Function

    Public Function OpenWebBrowserAsync() As Task(Of StatusCommandResult)
        Return Me.OpenWebBrowserAsync(New Uri("http://dune-hd.com"))
    End Function

    Public Function OpenWebBrowserAsync(location As Uri) As Task(Of StatusCommandResult)
        Return Me.State.OpenWebBrowserAsync(location)
    End Function

    Public Function SetSubtitlesTrack(index As Integer) As StatusCommandResult
        Return Me.State.SetSubtitlesTrack(index)
    End Function

    Public Function SetSubtitlesTrackAsync(index As Integer) As Task(Of StatusCommandResult)
        Return Me.State.SetSubtitlesTrackAsync(index)
    End Function

    Public Function SetSubtitlesTrack(track As MediaStream) As StatusCommandResult
        If track Is Nothing Then
            Throw New ArgumentNullException("track")
        ElseIf Not String.Equals(track.Type, Output.SubtitlesTrack.Name, StringComparison.InvariantCultureIgnoreCase) Then
            Throw New ArgumentException
        End If
        Return Me.SetSubtitlesTrack(track.Number)
    End Function

    Public Function SetSubtitlesTrackAsync(track As MediaStream) As Task(Of StatusCommandResult)
        If track Is Nothing Then
            Throw New ArgumentNullException("track")
        ElseIf Not String.Equals(track.Type, Output.SubtitlesTrack.Name, StringComparison.InvariantCultureIgnoreCase) Then
            Throw New ArgumentException
        End If
        Return Me.SetSubtitlesTrackAsync(track.Number)
    End Function

    Public Function SetSubtitlesEnabled(enabled As Boolean) As StatusCommandResult
        Return Me.SetSubtitlesEnabled(enabled)
    End Function

    Public Function SetSubtitlesEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Return Me.SetSubtitlesEnabledAsync(enabled)
    End Function

    Public Function SetVideoOnTop(onTop As Boolean) As StatusCommandResult
        Return Me.SetVideoOnTop(onTop)
    End Function

    Public Function SetVideoOnTopAsync(onTop As Boolean) As Task(Of StatusCommandResult)
        Return Me.SetVideoOnTopAsync(onTop)
    End Function

#End Region ' Methods v3

#Region "Methods remote-control"

    Public Function HasRemoteControlPlugin() As Boolean
        Return Me.State.HasRemoteControlPlugin()
    End Function

    Public Function CheckPluginAvailableAsync() As Task(Of Boolean)
        Return Me.State.HasRemoteControlPluginAsync
    End Function

    Public Function GetFileSystemEntries() As FileSystemCommandResult
        Return Me.State.GetFileSystemEntries
    End Function

    Public Function GetFileSystemEntries(path As String) As FileSystemCommandResult
        Return Me.State.GetFileSystemEntries(path)
    End Function

    Public Function GetFileSystemEntriesAsync() As Task(Of FileSystemCommandResult)
        Return Me.State.GetFileSystemEntriesAsync
    End Function

    Public Function GetFileSystemEntriesAsync(path As String) As Task(Of FileSystemCommandResult)
        Return Me.State.GetFileSystemEntriesAsync(path)
    End Function

#End Region

    Protected Function GetCenteredRectangle(value As Rectangle?, oldValue As Rectangle?) As Rectangle
        Dim display = Me.DisplaySize.GetValueOrDefault
        Dim current = oldValue.GetValueOrDefault
        Dim position As Point
        Dim size As Size

        If value.HasValue Then
            position = value.Value.Location
            size = value.Value.Size
        Else
            position = New Point
            size = display
        End If

        If position.X > (display.Width \ 2) Then position.X = display.Width \ 2
        If position.Y > (display.Height \ 2) Then position.Y = display.Height \ 2

        If size.Width > display.Width Then size.Width = display.Width
        If size.Height > display.Height Then size.Height = display.Height

        If (size.Width + (2 * position.X)) <> display.Width Then
            If position.X = current.Location.X Then
                position.X = (display.Width - size.Width) \ 2
            Else 'If size.Width = current.Size.Width Then
                size.Width = display.Width - (position.X * 2)
            End If
        End If

        If (size.Height + (2 * position.Y)) <> display.Height Then
            If position.Y = current.Location.Y Then
                position.Y = (display.Height - size.Height) \ 2
            Else 'If size.Height = current.Size.Height Then
                size.Height = display.Height - (position.Y * 2)
            End If
        End If

        Return New Rectangle(position, size)
    End Function

#Region "IPropertyChanged Support"

    ''' <summary>
    ''' Helper method helps raising PropertyChanged events.
    ''' </summary>
    ''' <param name="propertyName">The name of the property that changed.</param>
    Private Sub RaisePropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private Sub RaisePropertychanged(propertyNames As IEnumerable(Of String))
        For Each name As String In propertyNames
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        Next
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
                If Me.State IsNot Nothing Then
                    Me.State.Dispose()
                    Me.State = Nothing
                End If
                Me.TelnetClient.Dispose()
            End If

            _systemInfo = Nothing
            _firmwares = Nothing
            _endpoint = Nothing
            _shares = Nothing
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
        info.AddValue("EndPoint", Me.EndPoint)
        info.AddValue("HostName", Me.HostName)
        info.AddValue("Interval", Me.Interval)
        info.AddValue("Timeout", Me.Timeout)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        Me.EndPoint = DirectCast(info.GetValue("EndPoint", GetType(IPEndPoint)), IPEndPoint)
        Me.HostName = info.GetString("HostName")
        Me.Interval = info.GetDouble("Interval")
        Me.Timeout = DirectCast(info.GetValue("Timeout", GetType(TimeSpan)), TimeSpan)

        IsConnected = False
    End Sub

#End Region

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, Dune))
    End Function

    Public Overloads Function Equals(obj As Dune) As Boolean
        If obj IsNot Nothing Then
            If Object.ReferenceEquals(Me, obj) OrElse Object.Equals(Me.EndPoint, obj.EndPoint) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Me.EndPoint.GetHashCode
    End Function
End Class
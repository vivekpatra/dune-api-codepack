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
Imports System.Net.Sockets
Imports System.Net
Imports System.Threading.Tasks
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Threading

Namespace Networking

    ''' <summary>
    ''' Provides a simple implementation of a Telnet client which takes care of option negotiations (don't worry if you don't know what I'm talking about).
    ''' </summary>
    Public Class TelnetClient
        Implements IDisposable

        Private Shared Instances As Hashtable
        Friend Shared LogOnRegex As Regex
        Friend Shared PromptRegex As Regex
        Public Property EndPoint As IPEndPoint
        Friend Property State As TelnetState
        Private Property host As Dune

        Shared Sub New()
            Instances = New Hashtable
            TelnetClient.LogOnRegex = New Regex("\r\ntango3 login: ", RegexOptions.Singleline)
            TelnetClient.PromptRegex = New Regex("\r\ntango3\[.*\]# ", RegexOptions.Singleline)
        End Sub

        Private Sub New(host As Dune)
            Me.New(host, 23)
        End Sub

        Private Sub New(host As Dune, port As Integer)
            If host Is Nothing Then
                Throw New ArgumentNullException("host")
            End If
            If port < IPEndPoint.MinPort Or port > IPEndPoint.MaxPort Then
                Throw New ArgumentOutOfRangeException("port", "Port is outside the allowed range")
            End If
            Me.host = host
            Me.EndPoint = New IPEndPoint(host.EndPoint.Address, port)
            Me.State = New TelnetStateDisconnected(Me)
        End Sub

        Public Shared Function GetInstance(host As Dune) As TelnetClient
            If Not Instances.ContainsKey(host) Then
                Instances.Add(host, New TelnetClient(host))
            End If
            Return DirectCast(Instances.Item(host), TelnetClient)
        End Function

#Region "Properties"

        ''' <summary>
        ''' Gets whether the client is connected to a remote host.
        ''' </summary>
        Public ReadOnly Property IsConnected As Boolean
            Get
                Return Me.State.IsConnected
            End Get
        End Property

#End Region ' Properties

#Region "Methods"

        Public Sub Connect()
            Me.State.Connect()
        End Sub

        Public Function ConnectAsync() As Task
            Return Me.State.ConnectAsync
        End Function

        Public Sub Disconnect()
            Me.State.Disconnect()
        End Sub

        Public Function ExecuteCommand(command As String) As String
            Return Me.State.ExecuteCommandAsync(command).Result
        End Function

        Public Async Function ExecuteCommandAsync(command As String) As Task(Of String)
            Return Await Me.State.ExecuteCommandAsync(command).ConfigureAwait(False)
        End Function

        Public Function GetProductIdAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("cat /firmware/config/product_id.txt")
        End Function

        Public Function GetFirmwareVersionAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("cat /firmware/config/firmware_version.txt")
        End Function

        Public Function GetSerialNumberAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("grep serial_number /tmp/sysinfo.txt | awk '{print $2}'")
        End Function

        ''' <summary>
        ''' Get the current state of video and audio decoders.
        ''' </summary>
        Public Function GetDecoderStatusAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("/firmware/bin/dump_decoder_status.sh")
        End Function

        Public Function GetKernelMessagesAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("dmesg")
        End Function

        Public Function GetProcesses() As Task(Of String)
            Return Me.ExecuteCommandAsync("ps")
        End Function

        Public Function GetStartFirmwareLogsAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("cat /tmp/run/start_firmware.log")
        End Function

        Public Function GetPlaybackLogsAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("cat /tmp/run/play.log*")
        End Function

        Public Function GetRootLogsAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("cat /tmp/run/root.log*")
        End Function

        Public Function GetMemoryUsageStatisticsAsync() As Task(Of String)
            Return Me.ExecuteCommandAsync("cat /proc/meminfo")
        End Function

        Public Async Function RebootAsync() As Task
            Await Me.ExecuteCommandAsync("sync;reboot").ConfigureAwait(False)
            Me.Disconnect()
        End Function

        Public Async Function GetBootTimeAsync() As Task(Of DateTime)
            Dim separator As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

            Dim source = Await Me.ExecuteCommandAsync("cat /proc/uptime").ConfigureAwait(False)
            If IsNullOrWhiteSpace(source) Then
                Return DateTime.Now
            End If

            source = source.Split.First.Replace(".", separator)
            Dim uptime = TimeSpan.FromSeconds(Double.Parse(source))

            Return Date.Now.Subtract(uptime)
        End Function

        Public Function CloseDiscTrayAsync() As Task
            Return Me.ExecuteCommandAsync("eject -t /dev/sr0")
        End Function

        Public Function ToggleDiscTrayAsync() As Task
            Return Me.ExecuteCommandAsync("eject -T /dev/sr0")
        End Function

#End Region ' Methods

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If TelnetClient.Instances.ContainsValue(Me) Then
                        TelnetClient.Instances.Remove(Me.host)
                    End If
                    If Me.IsConnected Then
                        Me.State.Disconnect()
                    End If
                End If
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

    End Class

    Enum Commands As Byte ' as defined by RFC 854
        SE = 240                    ' End of subnegotiation parameters.
        NOP = 241                   ' No operation.
        DataMark = 242              ' The data stream portion of a Synch. This should always be accompanied by a TCP Urgent notification.
        Break = 243                 ' NVT character BRK.
        InterruptProcess = 244      ' The function IP.
        Abortoutput = 245           ' The function AO.
        AreYouThere = 246           ' The function AYT.
        EraseCharacter = 247        ' The function EC.
        EraseLine = 248             ' The function EL.
        GoAhead = 249               ' The GA signal.
        SB = 250                    ' Indicates that what follows is subnegotiation of the indicated option.
    End Enum

    Public Enum TelnetOptions As Byte
        BinaryTransmission = 0                  ' [RFC856]
        Echo = 1                                ' [RFC857]
        SuppressGoAhead = 3                     ' [RFC858]
        Status = 5                              ' [RFC859]
        TimingMark = 6                          ' [RFC860]
        RemoteControlledTransandEcho = 7        ' [RFC726]
        OutputCarriageReturnDisposition = 10    ' [RFC652]
        OutputHorizontalTabStops = 11           ' [RFC653]
        OutputHorizontalTabDisposition = 12     ' [RFC654]
        OutputFormfeedDisposition = 13          ' [RFC655]
        OutputVerticalTabstops = 14             ' [RFC656]
        OutputVerticalTabDisposition = 15       ' [RFC657]
        OutputLinefeedDisposition = 16          ' [RFC658]
        ExtendedASCII = 17                      ' [RFC698]
        Logout = 18                             ' [RFC727]
        ByteMacro = 19                          ' [RFC735]
        DataEntryTerminal = 20                  ' [RFC1043][RFC732]
        SUPDUP = 21                             ' [RFC736][RFC734]
        SUPDUPOutput = 22                       ' [RFC749]
        SendLocation = 23                       ' [RFC779]
        TerminalType = 24                       ' [RFC1091]
        EndofRecord = 25                        ' [RFC885]
        TACACSUserIdentification = 26           ' [RFC927]
        OutputMarking = 27                      ' [RFC933]
        TerminalLocationNumber = 28             ' [RFC946]
        Telnet3270Regime = 29                   ' [RFC1041]
        X3PAD = 30                              ' [RFC1053]
        NegotiateAboutWindowSize = 31           ' [RFC1073]
        TerminalSpeed = 32                      ' [RFC1079]
        RemoteFlowControl = 33                  ' [RFC1372]
        Linemode = 34                           ' [RFC1184]
        XDisplayLocation = 35                   ' [RFC1096]
        EnvironmentOption = 36                  ' [RFC1408]
        AuthenticationOption = 37               ' [RFC2941]
        EncryptionOption = 38                   ' [RFC2946]
        NewEnvironmentOption = 39               ' [RFC1572]
        TN3270E = 40                            ' [RFC2355]
        CHARSET = 42                            ' [RFC2066]
        ComPortControlOption = 44               ' [RFC2217]
        KERMIT = 47                             ' [RFC2840]
    End Enum

    Public Enum OptionCodes As Byte
        WILL = 251      ' Indicates the desire to begin performing, or confirmation that you are now performing, the indicated option.
        WONT = 252      ' Indicates the refusal to perform, or continue performing, the indicated option.
        [DO] = 253      ' Indicates the request that the other party perform, or confirmation that you are expecting the other party to perform, the indicated option.
        DONT = 254      ' Indicates the demand that the  other party stop performing, or confirmation that you are no longer expecting the other party to perform, the indicated option.
        IAC = 255       ' Data Byte 255.
    End Enum

End Namespace
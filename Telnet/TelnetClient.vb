Imports System.Net.Sockets
Imports System.Net
Imports System.Threading.Tasks
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO

Namespace Networking

    ''' <summary>
    ''' Provides a simple implementation of a Telnet client which takes care of option negotiations (don't worry if you don't know what I'm talking about).
    ''' </summary>
    Public Class TelnetClient
        Implements IDisposable

        Private _client As TcpClient
        Private _telnetLock As Object

        Private _logOnRegex As Regex
        Private _promptRegex As Regex


#Region "Constructors"

        Public Sub New(address As IPAddress)
            Me.New(address, 23)
        End Sub

        Public Sub New(address As IPAddress, port As Integer)
            _telnetLock = New Object
            _logOnRegex = New Regex("^tango3 login: $", RegexOptions.Multiline)
            _promptRegex = New Regex("^tango3.*# $", RegexOptions.Multiline)
            _client = New TcpClient

            Try
                Client.Connect(address, port)
            Catch ex As SocketException
                Console.WriteLine("Telnet is not accessible on port " + port.ToString)
            Catch ex As ArgumentException
                Console.WriteLine(ex.Message)
            End Try
        End Sub

#End Region ' Constructors

#Region "Properties"

        ''' <summary>
        ''' Gets the underlying TcpClient instance.
        ''' </summary>
        Private ReadOnly Property Client As TcpClient
            Get
                Return _client
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the client is connected to a remote host.
        ''' </summary>
        Public ReadOnly Property Connected As Boolean
            Get
                Return Client.Connected
            End Get
        End Property

        ''' <summary>
        ''' Convenience property that returns <see cref="Environment.NewLine"/>.
        ''' </summary>
        Private Shared ReadOnly Property CrLf As String
            Get
                Return Environment.NewLine
            End Get
        End Property

#End Region ' Properties

#Region "Methods"

        ''' <summary>
        ''' Blocks until the host asks for login and responds with the specified username.
        ''' </summary>
        Public Sub LogOn(userName As String)
            If Connected Then
                ReadUntilRegex(_logOnRegex)
                ExecuteCommand(userName)
            End If
        End Sub

        ''' <summary>
        ''' Reads bytes from the network stream until a match is found.
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReadUntilRegex(expression As Regex) As String
            Dim prompt As String = String.Empty

            For retries As Integer = 1 To 5
                prompt += ReadAndNegotiate()
                If expression.IsMatch(prompt) Then
                    Return expression.Replace(prompt, String.Empty)
                End If
                Threading.Thread.Sleep(100)
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Returns plain text from the stream and also takes care of option negotiations.
        ''' </summary>
        Public Function ReadAndNegotiate() As String
            Dim text As New StringBuilder

            Do
                If Client.Available > 0 Then
                    Dim read As Integer = Client.GetStream.ReadByte

                    If read > -1 Then
                        Select Case read
                            Case OptionCodes.IAC ' first 3 bytes represents a negotiation
                                Dim request As Short = CByte(Client.GetStream.ReadByte)
                                Dim action As Byte = CByte(Client.GetStream.ReadByte)

                                Dim response As Byte

                                If action = TelnetOptions.SuppressGoAhead Then ' WILL / DO
                                    If request = OptionCodes.DO Then
                                        response = OptionCodes.WILL
                                    Else
                                        response = OptionCodes.DO
                                    End If
                                Else ' WON'T / DON'T
                                    If request = OptionCodes.DO Then
                                        response = OptionCodes.WONT
                                    Else
                                        response = OptionCodes.DONT
                                    End If
                                End If

                                Dim sendData() As Byte = {OptionCodes.IAC, response, action}

                                Client.GetStream.Write(sendData, 0, sendData.Length)
                            Case Else ' plain text
                                text.Append(ChrW(read))
                        End Select
                    Else
                        Exit Do
                    End If
                Else
                    Exit Do
                End If
            Loop


            Return text.ToString
        End Function

        ''' <summary>
        ''' Sends the specified command.
        ''' </summary>
        Public Sub Write(command As String)
            Dim sendData() As Byte = Encoding.ASCII.GetBytes(command + CrLf)
            Client.GetStream.Write(sendData, 0, sendData.Length)
        End Sub

        ''' <summary>
        ''' Sends the specified command and returns the response.
        ''' </summary>
        Public Function ExecuteCommand(command As String) As String
            SyncLock _telnetLock
                Write(command)
                Dim response As String = ReadUntilRegex(_promptRegex)

                If response.Length > command.Length Then
                    Return response.Substring(command.Length).TrimStart
                Else
                    Return response
                End If
            End SyncLock
        End Function

#End Region ' Methods

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _client.Close()
                End If

                _telnetLock = Nothing
                _logOnRegex = Nothing
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
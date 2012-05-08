Imports System.Net.Sockets
Imports System.Net
Imports System.Threading.Tasks
Imports System.Text
Imports System.IO

Namespace Telnet


    Public Class TelnetClient
        Private _client As TcpClient

#Region "Constructors"

        Public Sub New(ByVal address As IPAddress)
            Me.New(address, 23)
        End Sub

        Public Sub New(ByVal address As IPAddress, ByVal port As Integer)
            Client.Connect(address, port)
            Do Until Client.Available >= 3
                Threading.Thread.Sleep(1)
            Loop
            Receive()
        End Sub

#End Region ' Constructors

#Region "Properties"

        Public ReadOnly Property Client As TcpClient
            Get
                If _client Is Nothing Then
                    _client = New TcpClient
                End If
                Return _client
            End Get
        End Property

#End Region ' Properties

        Public Property Reader As StreamReader

#Region "Methods"

        Public Sub login(ByVal username As String)
            Dim prompt As String = String.empty
            Do Until prompt.contains("login")
                prompt = receive
            Loop
            prompt = SendAndReceive(username)
        End Sub

        Public Function Receive() As String
            Dim text As New StringBuilder

            Do While Client.Available > 0
                Dim input As Byte = CByte(Client.GetStream.ReadByte)

                Select Case input
                    Case OptionCodes.IAC ' first 3 bytes represents a negotiation
                        Dim request As Byte = CByte(Client.GetStream.ReadByte)
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
                        text.Append(ChrW(input))

                End Select
            Loop

            Return text.ToString

            Return String.Empty
        End Function

        Public Sub Send(ByVal command As String)
            Dim sendData() As Byte = Encoding.ASCII.GetBytes(command + vbLf)
            Client.GetStream.Write(sendData, 0, sendData.Length)
        End Sub

        Public Function SendAndReceive(ByVal command As String) As String
            Send(command)

            Do Until Client.Available > Encoding.ASCII.GetBytes(command + vbCrLf).Length
                Threading.Thread.Sleep(100)
            Loop

            Dim response As String = Receive()

            Return response.Substring(command.Length, response.Length - command.Length).TrimStart
        End Function

        Public Function SendAndReceiveAsync(ByVal command As String) As Task(Of String)
            Return Nothing
        End Function

#End Region ' Methods
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
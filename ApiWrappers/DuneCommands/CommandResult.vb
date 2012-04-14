Imports System.Collections.Generic
Imports System.Globalization
Imports System.Net
Imports System.Collections.Specialized

Namespace Dune.ApiWrappers

    ''' <summary>
    ''' This class gets and holds the results for a requested command.
    ''' </summary>
    Public Class CommandResult

#Region "Parameter constants"

        Private Class Parameters
            Private Sub New()
            End Sub

            Public Const CommandStatus As String = "command_status"
            Public Const ProtocolVersion As String = "protocol_version"
            Public Const PlayerState As String = "player_state"
            Public Const PlaybackSpeed As String = "playback_speed"
            Public Const PlaybackDuration As String = "playback_duration"
            Public Const PlaybackPosition As String = "playback_position"
            Public Const PlaybackIsBuffering As String = "playback_is_buffering"
            Public Const PlaybackVolume As String = "playback_volume"
            Public Const PlaybackMute As String = "playback_mute"
            Public Const AudioTrack As String = "audio_track"
            Public Const VideoFullscreen As String = "video_fullscreen"
            Public Const VideoX As String = "video_x"
            Public Const VideoY As String = "video_y"
            Public Const VideoWidth As String = "video_width"
            Public Const VideoHeight As String = "video_height"
            Public Const VideoTotalDisplayWidth As String = "video_total_display_width"
            Public Const VideoTotalDisplayHeight As String = "video_total_display_height"
            Public Const VideoEnabled As String = "video_enabled"
            Public Const VideoZoom As String = "video_zoom"
            Public Const ErrorKind As String = "error_kind"
            Public Const ErrorDescription As String = "error_description"
            Public Const PlaybackDvdMenu As String = "playback_dvd_menu"

        End Class

#End Region

#Region "Private fields"

        Private Const NotSupportedMessage As String = "This property requires a firmware update."
        Private _command As Uri
        Private _rawData As NameValueCollection
        Private _roundTripTime As TimeSpan

        Private _commandStatus As String
        Private _protocolVersion As Byte
        Private _playerState As String
        Private _playbackSpeed As Integer
        Private _playbackDuration As TimeSpan
        Private _playbackPosition As TimeSpan
        Private _playbackIsBuffering As Boolean
        Private _playbackVolume As Nullable(Of SByte)
        Private _playbackMute As Nullable(Of Boolean)
        Private _audioTrack As Nullable(Of SByte)
        Private _videoFullscreen As Nullable(Of Boolean)
        Private _videoX As Nullable(Of Short)
        Private _videoY As Nullable(Of Short)
        Private _videoWidth As Nullable(Of Short)
        Private _videoHeight As Nullable(Of Short)
        Private _videoTotalDisplayWidth As Nullable(Of Short)
        Private _videoTotalDisplayHeight As Nullable(Of Short)
        Private _videoEnabled As Nullable(Of Boolean)
        Private _videoZoom As String
        Private _audioTracks As New SortedDictionary(Of Byte, CultureInfo)
        Private _errorKind As String
        Private _errorDescription As String
        Private _error As CommandException
        Private _playbackDvdMenu As Boolean

#End Region

        ''' <summary>Executes a command and processes the command results.</summary>
        ''' <param name="command">The command that needs to be executed.</param>
        ''' <exception cref="WebException">The exception that is thrown when an error occurs while querying the device.</exception>
        Public Sub New(ByVal command As DuneCommand)
            _command = command.ToUri()

            If Not _command.ToString.Contains("cmd=status") Then
                command.Dune.ClearStatus()
            End If

            Dim results As XDocument = GetResults(_command)
            ParseResults(results)
        End Sub

        ''' <summary>
        ''' Gets the command results in xml format and measures the latency.
        ''' </summary>
        <DebuggerStepThrough()>
        Private Function GetResults(ByVal command As Uri) As XDocument
            Dim stopwatch As Stopwatch = stopwatch.StartNew

            Dim request As WebRequest = DirectCast(HttpWebRequest.Create(command), WebRequest)
            Dim response As WebResponse = request.GetResponse

            stopwatch.Stop()

            _roundTripTime = stopwatch.Elapsed

            Return XDocument.Load(response.GetResponseStream)

        End Function

        ''' <summary>
        ''' Parses the xml document.
        ''' </summary>
        <DebuggerStepThrough()>
        Private Sub ParseResults(ByVal results As XDocument)
            Dim parameters As XElement = results.FirstNode

            For Each parameter As XElement In parameters.DescendantNodes
                ProcessParameter(parameter.FirstAttribute.Value, parameter.LastAttribute.Value)
            Next
        End Sub

        ''' <summary>
        ''' Processes the name=value results.
        ''' </summary>
        Private Sub ProcessParameter(ByVal name As String, ByVal value As String)
            RawData.Add(name, value)

            Select Case name
                Case Parameters.CommandStatus
                    _commandStatus = value
                Case Parameters.ProtocolVersion
                    _protocolVersion = CInt(value)
                Case Parameters.PlayerState
                    _playerState = value
                Case Parameters.PlaybackSpeed
                    _playbackSpeed = CInt(value)
                Case Parameters.PlaybackDuration
                    _playbackDuration = TimeSpan.FromSeconds(CInt(value))
                Case Parameters.PlaybackPosition
                    _playbackPosition = TimeSpan.FromSeconds(CInt(value))
                Case Parameters.PlaybackIsBuffering
                    _playbackIsBuffering = value.Equals("1")
                Case Parameters.PlaybackVolume
                    _playbackVolume = CByte(value)
                Case Parameters.PlaybackMute
                    _playbackMute = value.Equals("1")
                Case Parameters.AudioTrack
                    _audioTrack = CSByte(value)
                Case Parameters.VideoFullscreen
                    _videoFullscreen = value.Equals("1")
                Case Parameters.VideoX
                    _videoX = CShort(value)
                Case Parameters.VideoY
                    _videoY = CShort(value)
                Case Parameters.VideoWidth
                    _videoWidth = CShort(value)
                Case Parameters.VideoHeight
                    _videoHeight = CShort(value)
                Case Parameters.VideoTotalDisplayWidth
                    _videoTotalDisplayWidth = CShort(value)
                Case Parameters.VideoTotalDisplayHeight
                    _videoTotalDisplayHeight = CShort(value)
                Case Parameters.VideoEnabled
                    _videoEnabled = value.Equals("1")
                Case Parameters.VideoZoom
                    _videoZoom = value
                Case Parameters.ErrorKind
                    _errorKind = value
                Case Parameters.ErrorDescription
                    _errorDescription = value
                    _error = New CommandException(_errorKind, _errorDescription)
                Case Parameters.PlaybackDvdMenu
                    _playbackDvdMenu = value.Equals("1")
                Case Else
                    If name.Contains("audio") Then ' parse track.number.language information
                        AddAudioTrackInfo(name, value)
                    Else
                        Console.WriteLine("No parsing logic in place for {0} (value: {1})", name, value)
                    End If
            End Select
        End Sub

#Region "Public properties"

        ''' <summary>
        ''' Gets the command URI that was used for this command.
        ''' </summary>
        Public ReadOnly Property Command As Uri
            Get
                Return _command
            End Get
        End Property

        ''' <summary>
        ''' Gets the collection of unedited and uncut name=value pairs.
        ''' </summary>
        Public ReadOnly Property RawData As NameValueCollection
            Get
                If _rawData Is Nothing Then
                    _rawData = New NameValueCollection
                End If
                Return _rawData
            End Get
        End Property

        ''' <summary>
        ''' Gets the amount of miliseconds it took to complete the command.
        ''' </summary>
        ''' <remarks>This value is inaccurate when a timeout is reached.</remarks>
        Public ReadOnly Property RoundTripTime As TimeSpan
            Get
                Return _roundTripTime
            End Get
        End Property

        ''' <summary>
        ''' Gets the command status.
        ''' </summary>
        Public ReadOnly Property CommandStatus As String
            Get
                Return _commandStatus
            End Get
        End Property

        ''' <summary>
        ''' Gets the protocol version.
        ''' </summary>
        Public ReadOnly Property ProtocolVersion As Byte
            Get
                If _protocolVersion = Nothing Then
                    _protocolVersion = 0
                End If
                Return _protocolVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets the player state
        ''' </summary>
        Public ReadOnly Property PlayerState As String
            Get
                If _playerState = Nothing Then
                    _playerState = String.Empty
                End If
                Return _playerState
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback speed.
        ''' </summary>
        Public ReadOnly Property PlaybackSpeed As Integer
            Get
                If _playbackSpeed = Nothing Then
                    _playbackSpeed = 0
                End If
                Return _playbackSpeed
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        Public ReadOnly Property PlaybackDuration As TimeSpan
            Get
                If _playbackDuration = Nothing Then
                    _playbackDuration = TimeSpan.Zero
                End If
                Return _playbackDuration
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback position.
        ''' </summary>
        Public ReadOnly Property PlaybackPosition As TimeSpan
            Get
                If _playbackPosition = Nothing Then
                    _playbackPosition = TimeSpan.Zero
                End If
                Return _playbackPosition
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        Public ReadOnly Property PlaybackIsBuffering As Boolean
            Get
                If _playbackIsBuffering = Nothing Then
                    _playbackIsBuffering = False
                End If
                Return _playbackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback volume.
        ''' </summary>
        Public ReadOnly Property PlaybackVolume As Nullable(Of Byte)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _playbackVolume
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is muted.
        ''' </summary>
        Public ReadOnly Property PlaybackMute As Nullable(Of Boolean)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _playbackMute
            End Get
        End Property

        ''' <summary>
        ''' Gets the audio track number.
        ''' </summary>
        Public ReadOnly Property AudioTrack As Nullable(Of Byte)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _audioTrack = -1 Then
                    _audioTrack = Nothing
                End If
                Return _audioTrack
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the video output is in fullscreen.
        ''' </summary>
        Public ReadOnly Property VideoFullscreen As Nullable(Of Boolean)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoFullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the horizontal position of the video output.
        ''' </summary>
        Public ReadOnly Property VideoX As Nullable(Of Short)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _videoX = -1 Then
                    _videoX = Nothing
                End If
                Return _videoX
            End Get
        End Property

        ''' <summary>
        ''' Gets the vertical position of the video output.
        ''' </summary>
        Public ReadOnly Property VideoY As Nullable(Of Short)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _videoY = -1 Then
                    _videoY = Nothing
                End If
                Return _videoY
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of the video output.
        ''' </summary>
        Public ReadOnly Property VideoWidth As Nullable(Of Short)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _videoWidth = -1 Then
                    _videoWidth = Nothing
                End If
                Return _videoWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the video output.
        ''' </summary>
        Public ReadOnly Property VideoHeight As Nullable(Of Short)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _videoHeight = -1 Then
                    _videoHeight = Nothing
                End If
                Return _videoHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the total width of the display.
        ''' </summary>
        Public ReadOnly Property VideoTotalDisplayWidth As Nullable(Of Short)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _videoTotalDisplayWidth = -1 Then
                    _videoTotalDisplayWidth = Nothing
                End If
                Return _videoTotalDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the display.
        ''' </summary>
        Public ReadOnly Property VideoTotalDisplayHeight As Nullable(Of Short)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                ElseIf _videoTotalDisplayHeight = -1 Then
                    _videoTotalDisplayHeight = Nothing
                End If
                Return _videoTotalDisplayHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the video output is enabled.
        ''' </summary>
        Public ReadOnly Property VideoEnabled As Nullable(Of Boolean)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoEnabled
            End Get
        End Property

        ''' <summary>
        ''' Gets the video output zoom.
        ''' </summary>
        Public ReadOnly Property VideoZoom As String
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoZoom
            End Get
        End Property

        ''' <summary>
        ''' Gets a collection of audio tracks in the current playback in number=language format.
        ''' </summary>
        Public ReadOnly Property AudioTracks As SortedDictionary(Of Byte, CultureInfo)
            Get
                If ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Gets the command error, if any.
        ''' </summary>
        Public ReadOnly Property [Error] As CommandException
            Get
                Return _error
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a dvd menu is currently shown.
        ''' </summary>
        Public ReadOnly Property PlaybackDvdMenu As Boolean
            Get
                Return _playbackDvdMenu
            End Get
        End Property

#End Region


#Region "Methods"
        ''' <summary>
        ''' Adds audio track info to the collection.
        ''' </summary>
        Private Sub AddAudioTrackInfo(ByVal name As String, ByVal value As String)
            Dim delimiter As Char = "."

            ' get the track number (0...N)
            Dim key As Byte = CByte(name.Split(delimiter).GetValue(1))

            ' get the three-letter language code
            Dim languageCode As String = name.Split(delimiter).GetValue(2)

            ' get the corresponding CultureInfo object
            Dim language As CultureInfo = GetCultureInfo(languageCode)

            _audioTracks.Item(key) = language
        End Sub

        ''' <summary>
        ''' Converts a three-letter language code into a <see cref="CultureInfo"/> object.
        ''' </summary>
        Private Function GetCultureInfo(ByVal languageCode As String) As CultureInfo
            languageCode = GetTerminologyCode(languageCode)

            Dim allCultures As List(Of CultureInfo)
            allCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList

            For Each culture As CultureInfo In allCultures
                If culture.ThreeLetterISOLanguageName = languageCode Then
                    Return culture
                End If
            Next

            Return CultureInfo.InvariantCulture
        End Function

        ''' <summary>
        ''' Gets the terminology code for special cases where there are two codes per language.
        ''' </summary>
        Private Function GetTerminologyCode(ByVal languageCode As String) As String
            Select Case languageCode.ToLower
                Case "alb"
                    Return "sqi"
                Case "arm"
                    Return "hye"
                Case "baq"
                    Return "eus"
                Case "bur"
                    Return "mya"
                Case "chi"
                    Return "zho"
                Case "cze"
                    Return "ces"
                Case "dut"
                    Return "nld"
                Case "fre"
                    Return "fra"
                Case "geo"
                    Return "kat"
                Case "ger"
                    Return "deu"
                Case "gre"
                    Return "ell"
                Case "ice"
                    Return "isl"
                Case "mac"
                    Return "mkd"
                Case "mao"
                    Return "mri"
                Case "may"
                    Return "msa"
                Case "per"
                    Return "fas"
                Case "rum"
                    Return "ron"
                Case "slo"
                    Return "slk"
                Case "tib"
                    Return "bod"
                Case "wel"
                    Return "cym"
                Case Else
                    Return languageCode
            End Select
        End Function

#End Region ' Methods

    End Class

End Namespace
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Net
Imports System.Collections.Specialized
Imports System.Threading.Tasks
Imports SL.DuneApiCodePack.Extensions

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This class gets and holds the results for a requested command.
    ''' </summary>
    Public Class CommandResult

#Region "Private fields"

        Private Const NotSupportedMessage As String = "This property requires a firmware update."
        Private _command As Command
        Private _rawData As NameValueCollection
        Private _roundTripTime As TimeSpan

        Private _commandStatus As String
        Private _protocolVersion As Byte
        Private _playerState As String
        Private _playbackSpeed As Short
        Private _playbackDuration As TimeSpan?
        Private _playbackPosition As TimeSpan?
        Private _playbackIsBuffering As Boolean
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
        Private _errorKind As String
        Private _errorDescription As String
        Private _commandError As CommandException
        Private _playbackDvdMenu As Boolean

#End Region

        Friend Sub New(ByVal command As Command)
            _command = command
            If command.CommandType <> Constants.Commands.Status Then
                command.Target.ClearStatus()
            End If

            Dim stopwatch As Stopwatch = stopwatch.StartNew
            Using response As WebResponse = command.GetResponse
                stopwatch.Stop()
                _roundTripTime = stopwatch.Elapsed
                If response IsNot Nothing Then
                    Dim document As XDocument = XDocument.Load(response.GetResponseStream)
                    ParseResults(document)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Parses the xml document.
        ''' </summary>
        Private Sub ParseResults(ByVal results As XDocument)
            Dim parameters As IEnumerable(Of XElement) = results.Elements.First.Elements

            For Each XElement In parameters
                ProcessParameter(XElement.FirstAttribute.Value, XElement.LastAttribute.Value)
            Next
        End Sub

        ''' <summary>
        ''' Processes the name=value results.
        ''' </summary>
        Private Sub ProcessParameter(ByVal name As String, ByVal value As String)
            RawData.Add(name, value)

            If value = "-1" Then Exit Sub ' because nullables are much more awesome.

            Select Case name
                Case Constants.CommandResults.CommandStatus
                    _commandStatus = value
                Case Constants.CommandResults.ProtocolVersion
                    _protocolVersion = CByte(value)
                Case Constants.CommandResults.PlayerState
                    _playerState = value
                Case Constants.CommandResults.PlaybackSpeed
                    _playbackSpeed = GetSpeedFromBuggedValue(value)
                Case Constants.CommandResults.PlaybackDuration
                    If value = "0" Then Exit Select
                    _playbackDuration = TimeSpan.FromSeconds(CInt(value))
                Case Constants.CommandResults.PlaybackPosition
                    _playbackPosition = TimeSpan.FromSeconds(CInt(value))
                Case Constants.CommandResults.PlaybackIsBuffering
                    _playbackIsBuffering = value.Equals("1")
                Case Constants.CommandResults.PlaybackVolume
                    _playbackVolume = CByte(value)
                Case Constants.CommandResults.PlaybackMute
                    _playbackMute = value.Equals("1")
                Case Constants.CommandResults.AudioTrack
                    _audioTrack = CByte(value)
                Case Constants.CommandResults.VideoFullscreen
                    _videoFullscreen = value.Equals("1")
                Case Constants.CommandResults.VideoX
                    _videoX = CUShort(value)
                Case Constants.CommandResults.VideoY
                    _videoY = CUShort(value)
                Case Constants.CommandResults.VideoWidth
                    _videoWidth = CUShort(value)
                Case Constants.CommandResults.VideoHeight
                    _videoHeight = CUShort(value)
                Case Constants.CommandResults.VideoTotalDisplayWidth
                    _videoTotalDisplayWidth = CUShort(value)
                Case Constants.CommandResults.VideoTotalDisplayHeight
                    _videoTotalDisplayHeight = CUShort(value)
                Case Constants.CommandResults.VideoEnabled
                    _videoEnabled = value.Equals("1")
                Case Constants.CommandResults.VideoZoom
                    _videoZoom = value
                Case Constants.CommandResults.ErrorKind
                    _errorKind = value
                Case Constants.CommandResults.ErrorDescription
                    _errorDescription = value
                    _commandError = New CommandException(_errorKind, _errorDescription)
                    _commandError.Source = _command.ToString
                Case Constants.CommandResults.PlaybackDvdMenu
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
        ''' Gets the underlying command object that was used for this command.
        ''' </summary>
        Public ReadOnly Property Command As Command
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
                    _protocolVersion = Byte.MaxValue
                End If
                Return _protocolVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets the player state
        ''' </summary>
        Public ReadOnly Property PlayerState As String
            Get
                Return _playerState
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback speed.
        ''' </summary>
        Public ReadOnly Property PlaybackSpeed As Short
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
        Public ReadOnly Property PlaybackDuration As TimeSpan?
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
        Public ReadOnly Property PlaybackPosition As TimeSpan?
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
        Public ReadOnly Property PlaybackVolume As Byte?
            Get
                If Command.Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _playbackVolume
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is muted.
        ''' </summary>
        Public ReadOnly Property PlaybackMute As Boolean?
            Get
                If Command.Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _playbackMute
            End Get
        End Property

        ''' <summary>
        ''' Gets the audio track number.
        ''' </summary>
        Public ReadOnly Property AudioTrack As Byte?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoFullscreen As Boolean?
            Get
                If Command.Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                Return _videoFullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the horizontal position of the video output.
        ''' </summary>
        Public ReadOnly Property VideoX As UShort?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoY As UShort?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoWidth As UShort?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoHeight As UShort?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoTotalDisplayWidth As UShort?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoTotalDisplayHeight As UShort?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
        Public ReadOnly Property VideoEnabled As Boolean?
            Get
                If Command.Target.ProtocolVersion < 2 Then
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
                If Command.Target.ProtocolVersion < 2 Then
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
                If Command.Target.ProtocolVersion < 2 Then
                    Throw New NotSupportedException(NotSupportedMessage)
                End If
                If _audioTracks Is Nothing Then
                    _audioTracks = New SortedDictionary(Of Byte, CultureInfo)
                End If
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Gets the command error, if any.
        ''' </summary>
        Public ReadOnly Property CommandError As CommandException
            Get
                Return _commandError
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
            Dim delimiter As Char = Convert.ToChar(46) ' 46 = period

            ' get the track number (0...N)
            Dim key As Byte = CByte(name.Split(delimiter).GetValue(1))

            ' get the three-letter language code
            Dim languageCode As String = value

            ' get the corresponding CultureInfo object
            Dim language As CultureInfo = GetCultureInfo(languageCode)

            AudioTracks.Item(key) = language
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

        ''' <summary>
        ''' Workaround method for a bug in the API. This method gets the real playback speed for values where something server-side went wrong with bitwise operations.
        ''' The problem seems to be that the unit does not properly recognize the sign bit on values between -128 and -16.
        ''' </summary>
        ''' <param name="value">The actual value.</param>
        ''' <returns>The expected value.</returns>
        ''' <remarks>Version 1 and 2 have this bug. Hopefully it will be fixed in version 3. I've e-mailed support about this on 13 may 2012.</remarks>
        Private Function GetSpeedFromBuggedValue(ByVal value As String) As Short
            Dim buggedValue As Integer = Convert.ToInt32(value)
            Dim bytes() As Byte = BitConverter.GetBytes(buggedValue)
            Dim fixedValue As Short = BitConverter.ToInt16(bytes, 0)
            Return fixedValue
        End Function

#End Region ' Methods

    End Class

End Namespace
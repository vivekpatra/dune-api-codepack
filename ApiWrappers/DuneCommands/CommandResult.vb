Imports System.Collections.Generic
Imports System.Globalization
Imports System.Net
Imports System.Collections.Specialized
Imports System.Threading.Tasks
Imports SL.DuneApiCodePack.Extensions
Imports System.Text.RegularExpressions

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This class gets and holds the results for a requested command.
    ''' </summary>
    Public Class CommandResult

#Region "Private fields"

        Private _command As Command
        Private _rawData As NameValueCollection
        Private _roundTripTime As TimeSpan

        Private _commandStatus As String
        Private _protocolVersion As UShort
        Private _playerState As String
        Private _playbackSpeed As Short
        Private _playbackDuration As TimeSpan?
        Private _playbackPosition As TimeSpan?
        Private _playbackIsBuffering As Boolean
        Private _playbackVolume As UShort?
        Private _playbackMute As Boolean?
        Private _audioTrack As UShort?
        Private _playbackWindowFullscreen As Boolean?
        Private _playbackWindowRectangleX As UShort?
        Private _playbackWindowRectangleY As UShort?
        Private _playbackWindowRectangleWidth As UShort?
        Private _playbackWindowRectangleHeight As UShort?
        Private _onScreenDisplayWidth As UShort?
        Private _onScreenDisplayHeight As UShort?
        Private _videoEnabled As Boolean?
        Private _videoZoom As String
        Private _audioTracks As SortedList(Of UShort, LanguageTrack)
        Private _subtitles As SortedList(Of UShort, LanguageTrack)

        Private _errorKind As String
        Private _errorDescription As String
        Private _commandError As CommandException
        Private _playbackDvdMenu As Boolean

        Private _playbackClipRectangleX As UShort?
        Private _playbackClipRectangleY As UShort?
        Private _playbackClipRectangleWidth As UShort?
        Private _playbackClipRectangleHeight As UShort?

        Private _playbackVideoWidth As UShort?
        Private _playbackVideoHeight As UShort?

        Private _playbackState As String
        Private _previousPlaybackState As String

        Private _lastPlaybackEvent As String
        Private _playbackUrl As String
        Private _videoOnTop As Boolean?
        Private _subtitlesTrack As UShort?


#End Region

        Friend Sub New(command As Command)
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
        Private Sub ParseResults(results As XDocument)
            Dim parameters As IEnumerable(Of XElement) = results.Elements.First.Elements

            For Each XElement In parameters
                Dim name As String = XElement.FirstAttribute.Value
                Dim value As String = XElement.LastAttribute.Value
                RawData.Add(name, value)
            Next

            ProcessParameter()
        End Sub

        ''' <summary>
        ''' Processes the name=value results.
        ''' </summary>
        Private Sub ProcessParameter()
            For Each name As String In RawData.AllKeys
                Dim value As String = RawData.Get(name)

                If TrackRegex.IsMatch(name) Then
                    AddTrackInfo(name, value)
                Else
                    If value <> "-1" Then ' fill in the value, otherwise leave it null or default
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
                            Case Constants.CommandResults.SubtitlesTrack
                                _subtitlesTrack = CByte(value)
                            Case Constants.CommandResults.VideoFullscreen, Constants.CommandResults.PlaybackWindowFullscreen
                                _playbackWindowFullscreen = value.Equals("1")
                            Case Constants.CommandResults.VideoX, Constants.CommandResults.PlaybackWindowRectangleX
                                _playbackWindowRectangleX = CUShort(value)
                            Case Constants.CommandResults.VideoY, Constants.CommandResults.PlaybackWindowRectangleY
                                _playbackWindowRectangleY = CUShort(value)
                            Case Constants.CommandResults.VideoWidth, Constants.CommandResults.PlaybackWindowRectangleWidth
                                _playbackWindowRectangleWidth = CUShort(value)
                            Case Constants.CommandResults.VideoHeight, Constants.CommandResults.PlaybackWindowRectangleHeight
                                _playbackWindowRectangleHeight = CUShort(value)
                            Case Constants.CommandResults.VideoTotalDisplayWidth, Constants.CommandResults.OnScreenDisplayWidth
                                _onScreenDisplayWidth = CUShort(value)
                            Case Constants.CommandResults.VideoTotalDisplayHeight, Constants.CommandResults.OnScreenDisplayHeight
                                _onScreenDisplayHeight = CUShort(value)
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
                            Case Constants.CommandResults.PlaybackState
                                _playbackState = value
                            Case Constants.CommandResults.PreviousPlaybackState
                                _previousPlaybackState = value
                            Case Constants.CommandResults.LastPlaybackEvent
                                _lastPlaybackEvent = value
                            Case Constants.CommandResults.PlaybackUrl
                                _playbackUrl = value
                            Case Constants.CommandResults.PlaybackWindowFullscreen
                                _playbackWindowFullscreen = value.Equals("1")
                            Case Constants.CommandResults.OnScreenDisplayWidth
                                _onScreenDisplayWidth = CUShort(value)
                            Case Constants.CommandResults.OnScreenDisplayHeight
                                _onScreenDisplayHeight = CUShort(value)
                            Case Constants.CommandResults.VideoOnTop
                                _videoOnTop = value.Equals("1")
                            Case Constants.CommandResults.PlaybackClipRectangleX
                                _playbackClipRectangleX = CUShort(value)
                            Case Constants.CommandResults.PlaybackClipRectangleY
                                _playbackClipRectangleY = CUShort(value)
                            Case Constants.CommandResults.PlaybackWindowRectangleWidth
                                _playbackClipRectangleWidth = CUShort(value)
                            Case Constants.CommandResults.PlaybackClipRectangleHeight
                                _playbackClipRectangleHeight = CUShort(value)
                            Case Constants.CommandResults.PlaybackVideoWidth
                                _playbackVideoWidth = CUShort(value)
                            Case Constants.CommandResults.PlaybackVideoHeight
                                _playbackVideoHeight = CUShort(value)
                            Case Else
                                Console.WriteLine("No parsing logic in place for {0} (value: {1})", name, value)
                        End Select
                    End If
                End If
            Next
        End Sub

        Private _trackRegex As Regex
        Private ReadOnly Property TrackRegex As Regex
            Get
                If _trackRegex Is Nothing Then
                    _trackRegex = New Regex("(audio|subtitles)_track\.[0-9]+\.(lang|codec)")
                End If
                Return _trackRegex
            End Get
        End Property

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
        Public ReadOnly Property ProtocolVersion As UShort
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
                Return _playbackSpeed
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        Public ReadOnly Property PlaybackDuration As TimeSpan?
            Get
                If Not _playbackDuration.HasValue Then
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
                If Not _playbackPosition.HasValue Then
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
                Return _playbackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback volume.
        ''' </summary>
        Public ReadOnly Property PlaybackVolume As UShort?
            Get
                Return _playbackVolume
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is muted.
        ''' </summary>
        Public ReadOnly Property PlaybackMute As Boolean?
            Get
                Return _playbackMute
            End Get
        End Property

        ''' <summary>
        ''' Gets the audio track number.
        ''' </summary>
        Public ReadOnly Property AudioTrack As UShort?
            Get
                Return _audioTrack
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the video output is in fullscreen.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowFullscreen As Boolean?
            Get
                Return _playbackWindowFullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the horizontal offset of the playback window rectangle.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleHorizontalOffset As UShort?
            Get
                Return _playbackWindowRectangleX
            End Get
        End Property

        ''' <summary>
        ''' Gets the vertical offset of the playback window rectangle.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleVerticalOffset As UShort?
            Get
                Return _playbackWindowRectangleY
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of the video output.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleWidth As UShort?
            Get
                Return _playbackWindowRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the video output.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleHeight As UShort?
            Get
                Return _playbackWindowRectangleHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the total width of the display.
        ''' </summary>
        Public ReadOnly Property OnScreenDisplayWidth As UShort?
            Get
                Return _onScreenDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the display.
        ''' </summary>
        Public ReadOnly Property OnScreenDisplayHeight As UShort?
            Get
                Return _onScreenDisplayHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the video output is enabled.
        ''' </summary>
        Public ReadOnly Property VideoEnabled As Boolean?
            Get
                Return _videoEnabled
            End Get
        End Property

        ''' <summary>
        ''' Gets the video output zoom.
        ''' </summary>
        Public ReadOnly Property VideoZoom As String
            Get
                Return _videoZoom
            End Get
        End Property

        ''' <summary>
        ''' Gets a collection of audio tracks in the current playback.
        ''' </summary>
        Public ReadOnly Property AudioTracks As SortedList(Of UShort, LanguageTrack)
            Get
                If _audioTracks Is Nothing Then
                    _audioTracks = New SortedList(Of UShort, LanguageTrack)
                End If
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Gets a collection of subtitles in the current playback.
        ''' </summary>
        Public ReadOnly Property Subtitles As SortedList(Of UShort, LanguageTrack)
            Get
                If _subtitles Is Nothing Then
                    _subtitles = New SortedList(Of UShort, LanguageTrack)
                End If
                Return _subtitles
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

        Public ReadOnly Property PlaybackState As String
            Get
                Return _playbackState
            End Get
        End Property

        Public ReadOnly Property PreviousPlaybackState As String
            Get
                Return _previousPlaybackState
            End Get
        End Property

        Public ReadOnly Property LastPlaybackEvent As String
            Get
                Return _lastPlaybackEvent
            End Get
        End Property

        Public ReadOnly Property PlaybackUrl As String
            Get
                Return _playbackUrl
            End Get
        End Property

        Public ReadOnly Property VideoOnTop As Boolean?
            Get
                Return _videoOnTop
            End Get
        End Property

        Public ReadOnly Property SubtitlesTrack As UShort?
            Get
                Return _subtitlesTrack
            End Get
        End Property

        Public ReadOnly Property PlaybackVideoWidth As UShort?
            Get
                Return _playbackVideoWidth
            End Get
        End Property

        Public ReadOnly Property PlaybackVideoHeight As UShort?
            Get
                Return _playbackVideoHeight
            End Get
        End Property

        Public ReadOnly Property PlaybackClipRectangleHorizontalOffset As UShort?
            Get
                Return _playbackClipRectangleX
            End Get
        End Property

        Public ReadOnly Property PlaybackClipRectangleVerticalOffset As UShort?
            Get
                Return _playbackClipRectangleY
            End Get
        End Property

        Public ReadOnly Property PlaybackClipRectangleWidth As UShort?
            Get
                Return _playbackClipRectangleWidth
            End Get
        End Property

        Public ReadOnly Property PlaybackClipRectangleHeight As UShort?
            Get
                Return _playbackClipRectangleHeight
            End Get
        End Property

#End Region ' Properties


#Region "Methods"
        ''' <summary>
        ''' Adds audio track info to the collection.
        ''' </summary>
        Private Sub AddTrackInfo(name As String, value As String)
            If name.Contains("codec") Then
                Exit Sub
            Else
                Dim delimiter As Char = "."c

                ' get the track type (audio/subtitles)
                Dim type As String = name.Split(delimiter)(0)

                ' get the track number (0...N)
                Dim number As String = name.Split(delimiter)(1)

                ' get the three-letter language code
                Dim languageCode As String = value

                Dim codec As String = String.Empty
                If Command.Target.ProtocolVersion >= 3 Then ' also get codec information
                    codec = RawData.Get(name.Replace("lang", "codec"))
                End If

                Dim track As LanguageTrack = ApiWrappers.LanguageTrack.FromCommandResult(type, number, languageCode, codec)

                If type = "audio" Then
                    AudioTracks.Add(track.Number, track)
                ElseIf type = "subtitles" Then
                    Subtitles.Add(track.Number, track)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Workaround method for a bug in the API. This method gets the real playback speed for values where something server-side went wrong with bitwise operations.
        ''' The problem seems to be that the unit does not properly recognize the sign bit on values between -128 and -16.
        ''' </summary>
        ''' <param name="value">The actual value.</param>
        ''' <returns>The expected value.</returns>
        ''' <remarks>Version 1 and 2 have this bug. Hopefully it will be fixed in version 3. I've e-mailed support about this on 13 may 2012.</remarks>
        Private Function GetSpeedFromBuggedValue(value As String) As Short
            Dim buggedValue As Integer = Convert.ToInt32(value)
            Dim bytes() As Byte = BitConverter.GetBytes(buggedValue)
            Dim fixedValue As Short = BitConverter.ToInt16(bytes, 0)
            Return fixedValue
        End Function

#End Region ' Methods

    End Class

End Namespace
﻿#Region "License"
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
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Net
Imports System.Collections.Specialized
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' Represents the result of a command request.
    ''' </summary>
    Public Class CommandResult

#Region "Private fields"

        Private _command As Command
        Private _rawData As NameValueCollection
        Private _roundTripTime As TimeSpan
        Private _requestDateTime As Date

        ' command result parameters
        Private _commandStatus As String
        Private _errorKind As String
        Private _errorDescription As String
        Private _protocolVersion As Version
        Private _playerState As String
        Private _playbackSpeed As Short?
        Private _playbackDuration As TimeSpan?
        Private _playbackPosition As TimeSpan?
        Private _playbackTimeRemaining As TimeSpan?
        Private _playbackIsBuffering As Boolean?
        Private _playbackVolume As Short?
        Private _playbackMute As Boolean?
        Private _audioTrack As Short?
        Private _playbackWindowFullscreen As Boolean?
        Private _playbackWindowRectangleX As Short?
        Private _playbackWindowRectangleY As Short?
        Private _playbackWindowRectangleWidth As Short?
        Private _playbackWindowRectangleHeight As Short?
        Private _onScreenDisplayWidth As Short?
        Private _onScreenDisplayHeight As Short?
        Private _videoEnabled As Boolean?
        Private _videoZoom As String
        Private _audioTracks As SortedList(Of Short, LanguageTrack)
        Private _subtitles As SortedList(Of Short, LanguageTrack)
        Private _playbackDvdMenu As Boolean?
        Private _playbackBlurayMenu As Boolean?
        Private _playbackClipRectangleX As Short?
        Private _playbackClipRectangleY As Short?
        Private _playbackClipRectangleWidth As Short?
        Private _playbackClipRectangleHeight As Short?
        Private _playbackVideoWidth As Short?
        Private _playbackVideoHeight As Short?
        Private _playbackState As String
        Private _previousPlaybackState As String
        Private _lastPlaybackEvent As String
        Private _playbackUrl As String
        Private _videoOnTop As Boolean?
        Private _subtitlesTrack As Short?
        Private _text As String
        Private _textAvailable As Boolean?

#End Region

        Friend Sub New(command As Command)
            _command = command
            _requestDateTime = Date.Now

            Dim stopwatch As Stopwatch = stopwatch.StartNew
            Using response As WebResponse = command.GetResponse
                stopwatch.Stop()
                _roundTripTime = stopwatch.Elapsed

                Try
                    Dim document As XDocument = XDocument.Load(response.GetResponseStream)
                    ParseResults(document)
                Catch ex As IO.IOException
                    Console.WriteLine("I/O error occurred while loading xml document: " + ex.Message)
                End Try
            End Using
        End Sub

#Region "Properties"

        ''' <summary>
        ''' Gets the date and time indicating when the command was executed.
        ''' </summary>
        Public ReadOnly Property RequestDateTime As Date
            Get
                Return _requestDateTime
            End Get
        End Property

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
        Public ReadOnly Property ProtocolVersion As Version
            Get
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
        Public ReadOnly Property PlaybackSpeed As Short?
            Get
                Return _playbackSpeed
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        Public ReadOnly Property PlaybackDuration As TimeSpan?
            Get
                'If Not _playbackDuration.HasValue Then
                '    _playbackDuration = TimeSpan.Zero
                'End If
                Return _playbackDuration
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback position.
        ''' </summary>
        Public ReadOnly Property PlaybackPosition As TimeSpan?
            Get
                Return _playbackPosition
            End Get
        End Property

        ''' <summary>
        ''' Gets the remaining playback time.
        ''' </summary>
        ''' <remarks>This value is not part of the command results. Instead, it is calculated by substracting the playback position from the playback duration.</remarks>
        Public ReadOnly Property PlaybackTimeRemaining As TimeSpan?
            Get
                Return _playbackTimeRemaining
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        Public ReadOnly Property PlaybackIsBuffering As Boolean?
            Get
                Return _playbackIsBuffering
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback volume.
        ''' </summary>
        Public ReadOnly Property PlaybackVolume As Short?
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
        ''' Gets the zero-based index number of active audio track.
        ''' </summary>
        Public ReadOnly Property AudioTrack As Short?
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
        Public ReadOnly Property PlaybackWindowRectangleHorizontalOffset As Short?
            Get
                Return _playbackWindowRectangleX
            End Get
        End Property

        ''' <summary>
        ''' Gets the vertical offset of the playback window rectangle.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleVerticalOffset As Short?
            Get
                Return _playbackWindowRectangleY
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of the video output.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleWidth As Short?
            Get
                Return _playbackWindowRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the video output.
        ''' </summary>
        Public ReadOnly Property PlaybackWindowRectangleHeight As Short?
            Get
                Return _playbackWindowRectangleHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the total width of the display.
        ''' </summary>
        Public ReadOnly Property OnScreenDisplayWidth As Short?
            Get
                Return _onScreenDisplayWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the display.
        ''' </summary>
        Public ReadOnly Property OnScreenDisplayHeight As Short?
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
        ''' Gets the collection of audio tracks in the current playback.
        ''' </summary>
        Public ReadOnly Property AudioTracks As SortedList(Of Short, LanguageTrack)
            Get
                If _audioTracks Is Nothing Then
                    _audioTracks = New SortedList(Of Short, LanguageTrack)
                End If
                Return _audioTracks
            End Get
        End Property

        ''' <summary>
        ''' Gets the collection of subtitles in the current playback.
        ''' </summary>
        Public ReadOnly Property Subtitles As SortedList(Of Short, LanguageTrack)
            Get
                If _subtitles Is Nothing Then
                    _subtitles = New SortedList(Of Short, LanguageTrack)
                End If
                Return _subtitles
            End Get
        End Property

        Public ReadOnly Property ErrorKind As String
            Get
                Return _errorKind
            End Get
        End Property

        Public ReadOnly Property ErrorDescription As String
            Get
                Return _errorDescription
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a DVD menu is currently shown.
        ''' </summary>
        Public ReadOnly Property PlaybackDvdMenu As Boolean?
            Get
                Return _playbackDvdMenu
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a Blu-ray menu is currently shown.
        ''' </summary>
        Public ReadOnly Property PlaybackBlurayMenu As Boolean?
            Get
                Return _playbackBlurayMenu
            End Get
        End Property

        ''' <summary>
        ''' Gets the playback state.
        ''' </summary>
        Public ReadOnly Property PlaybackState As String
            Get
                Return _playbackState
            End Get
        End Property

        ''' <summary>
        ''' Gets the previous playback state.
        ''' </summary>
        Public ReadOnly Property PreviousPlaybackState As String
            Get
                Return _previousPlaybackState
            End Get
        End Property

        ''' <summary>
        ''' Gets the last playback event.
        ''' </summary>
        Public ReadOnly Property LastPlaybackEvent As String
            Get
                Return _lastPlaybackEvent
            End Get
        End Property

        ''' <summary>
        ''' Gets the media URL that is currently playing.
        ''' </summary>
        Public ReadOnly Property PlaybackUrl As String
            Get
                Return _playbackUrl
            End Get
        End Property

        ''' <summary>
        ''' Gets whether video output shows on top of overlay graphics.
        ''' </summary>
        Public ReadOnly Property VideoOnTop As Boolean?
            Get
                Return _videoOnTop
            End Get
        End Property

        ''' <summary>
        ''' Gets the zero-based index number of active subtitle track.
        ''' </summary>
        Public ReadOnly Property SubtitlesTrack As Short?
            Get
                Return _subtitlesTrack
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of the video.
        ''' </summary>
        Public ReadOnly Property PlaybackVideoWidth As Short?
            Get
                Return _playbackVideoWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of the video.
        ''' </summary>
        Public ReadOnly Property PlaybackVideoHeight As Short?
            Get
                Return _playbackVideoHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets the visible screen rectangle's horizontal offset.
        ''' </summary>
        Public ReadOnly Property PlaybackClipRectangleHorizontalOffset As Short?
            Get
                Return _playbackClipRectangleX
            End Get
        End Property

        ''' <summary>
        ''' Gets the visible screen rectangle's vertical offset.
        ''' </summary>
        Public ReadOnly Property PlaybackClipRectangleVerticalOffset As Short?
            Get
                Return _playbackClipRectangleY
            End Get
        End Property

        ''' <summary>
        ''' Gets the visible screen rectangle's width.
        ''' </summary>
        Public ReadOnly Property PlaybackClipRectangleWidth As Short?
            Get
                Return _playbackClipRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the visible screen rectangle's height.
        ''' </summary>
        Public ReadOnly Property PlaybackClipRectangleHeight As Short?
            Get
                Return _playbackClipRectangleHeight
            End Get
        End Property

        ''' <summary>
        ''' Gets whether a text editor is currently active.
        ''' </summary>
        Public ReadOnly Property TextAvailable As Boolean?
            Get
                Return _textAvailable
            End Get
        End Property

        ''' <summary>
        ''' Gets the text in the selected text input field, if any.
        ''' </summary>
        Public ReadOnly Property Text As String
            Get
                Return _text
            End Get
        End Property


#End Region ' Properties

#Region "Methods"

        ''' <summary>
        ''' Parses the command result XML document.
        ''' </summary>
        Private Sub ParseResults(results As XDocument)
            Dim commandResults = From xml In results.Descendants("param")
                          Select New With {
                              .Name = xml.Attribute("name"),
                              .Value = xml.Attribute("value")
                              }

            For Each result In commandResults
                Dim parameterName As String = result.Name.Value
                Dim parameterValue As String = result.Value.Value
                RawData.Add(parameterName, parameterValue)
            Next

            ' Set parameters that are always present
            _protocolVersion = _protocolVersion.Parse(RawData.Item(Constants.CommandResultParameterNames.ProtocolVersion), False)
            _playerState = RawData.Item(Constants.CommandResultParameterNames.PlayerState)

            ' Check if this is the result of a get_text command
            If Command.CommandType = Constants.CommandValues.GetText Then ' find out if the text editor is active
                _textAvailable = RawData.AllKeys.Contains(Constants.CommandResultParameterNames.Text)
            End If

            ' loop through the remaining parameters
            Parallel.ForEach(RawData.AllKeys, Sub(key As String)
                                                  Dim parameterName As String = key
                                                  Dim parameterValue As String = RawData.Get(key)
                                                  Try
                                                      If parameterValue <> "-1" Then
                                                          Select Case True
                                                              Case parameterName.Equals(Constants.CommandResultParameterNames.ProtocolVersion), parameterName.Equals(Constants.CommandResultParameterNames.PlayerState)
                                                                  Exit Select
                                                              Case parameterName.Equals(Constants.CommandResultParameterNames.CommandStatus)
                                                                  _commandStatus = parameterValue
                                                              Case parameterName.Equals(Constants.CommandResultParameterNames.Text)
                                                                  _text = parameterValue
                                                              Case parameterName.Contains("error")
                                                                  ProcessErrorParameter(parameterName, parameterValue)
                                                              Case parameterName.Contains("playback"), parameterName.Contains("video"), parameterName.Contains("osd")
                                                                  ProcessPlaybackParameter(parameterName, parameterValue)
                                                              Case parameterName.Contains("track")
                                                                  ProcessTrackParameter(parameterName, parameterValue)
                                                              Case Else
                                                                  Console.WriteLine("No parsing logic in place for unknown parameter {0} (value: {1})", parameterName, parameterValue)
                                                          End Select
                                                      End If
                                                  Catch ex As Exception
                                                      Console.WriteLine("parsing error: failed to parse " + parameterName)
                                                  End Try
                                              End Sub)
            

            If PlaybackDuration.HasValue AndAlso PlaybackPosition.HasValue Then
                _playbackTimeRemaining = PlaybackDuration - PlaybackPosition
            End If
        End Sub

        ''' <summary>
        ''' Sets fields that relate to error values.
        ''' </summary>
        Private Sub ProcessErrorParameter(parameterName As String, parameterValue As String)
            Select Case parameterName
                Case Constants.CommandResultParameterNames.ErrorKind : _errorKind = parameterValue
                Case Constants.CommandResultParameterNames.ErrorDescription : _errorDescription = parameterValue
                Case Else : Console.WriteLine("No parsing logic in place for error parameter {0} (value: {1})", parameterName, parameterValue)
            End Select
        End Sub

        ''' <summary>
        ''' Sets fields that relate to track settings.
        ''' </summary>
        Private Sub ProcessTrackParameter(parameterName As String, parameterValue As String)
            If Constants.CommandResultParameterNames.TrackRegex.IsMatch(parameterName) Then
                AddTrackInfo(parameterName, parameterValue)
            Else
                Select Case parameterName
                    Case Constants.CommandResultParameterNames.AudioTrack : _audioTrack = CShort(parameterValue)
                    Case Constants.CommandResultParameterNames.SubtitlesTrack : _subtitlesTrack = CShort(parameterValue)
                    Case Else : Console.WriteLine("No parsing logic in place for track parameter {0} (value: {1})", parameterName, parameterValue)
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Sets fields that relate to playback settings.
        ''' </summary>
        Private Sub ProcessPlaybackParameter(parameterName As String, parameterValue As String)
            Select Case parameterName
                Case Constants.CommandResultParameterNames.PlaybackSpeed : _playbackSpeed = GetSpeedFromBuggedValue(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackDuration : _playbackDuration = TimeSpan.FromSeconds(CInt(parameterValue))
                Case Constants.CommandResultParameterNames.PlaybackPosition : _playbackPosition = TimeSpan.FromSeconds(CInt(parameterValue))
                Case Constants.CommandResultParameterNames.PlaybackIsBuffering : _playbackIsBuffering = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.PlaybackVolume : _playbackVolume = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackMute : _playbackMute = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.VideoFullscreen, Constants.CommandResultParameterNames.PlaybackWindowFullscreen : _playbackWindowFullscreen = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.VideoX, Constants.CommandResultParameterNames.PlaybackWindowRectangleX : _playbackWindowRectangleX = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.VideoY, Constants.CommandResultParameterNames.PlaybackWindowRectangleY : _playbackWindowRectangleY = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.VideoWidth, Constants.CommandResultParameterNames.PlaybackWindowRectangleWidth : _playbackWindowRectangleWidth = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.VideoHeight, Constants.CommandResultParameterNames.PlaybackWindowRectangleHeight : _playbackWindowRectangleHeight = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.VideoTotalDisplayWidth, Constants.CommandResultParameterNames.OnScreenDisplayWidth : _onScreenDisplayWidth = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.VideoTotalDisplayHeight, Constants.CommandResultParameterNames.OnScreenDisplayHeight : _onScreenDisplayHeight = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.VideoEnabled : _videoEnabled = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.VideoZoom : _videoZoom = parameterValue
                Case Constants.CommandResultParameterNames.PlaybackDvdMenu : _playbackDvdMenu = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.PlaybackBlurayMenu : _playbackBlurayMenu = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.PlaybackState : _playbackState = parameterValue
                Case Constants.CommandResultParameterNames.PreviousPlaybackState : _previousPlaybackState = parameterValue
                Case Constants.CommandResultParameterNames.LastPlaybackEvent : _lastPlaybackEvent = parameterValue
                Case Constants.CommandResultParameterNames.PlaybackUrl : _playbackUrl = parameterValue
                Case Constants.CommandResultParameterNames.VideoOnTop : _videoOnTop = parameterValue.ToBoolean
                Case Constants.CommandResultParameterNames.PlaybackClipRectangleX : _playbackClipRectangleX = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackClipRectangleY : _playbackClipRectangleY = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackClipRectangleWidth : _playbackClipRectangleWidth = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackClipRectangleHeight : _playbackClipRectangleHeight = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackVideoWidth : _playbackVideoWidth = CShort(parameterValue)
                Case Constants.CommandResultParameterNames.PlaybackVideoHeight : _playbackVideoHeight = CShort(parameterValue)
                Case Else : Console.WriteLine("No parsing logic in place for playback parameter: {0} (value: {1})", parameterName, parameterValue)
            End Select
        End Sub

        ''' <summary>
        ''' Adds audio track info to the collection.
        ''' </summary>
        Private Sub AddTrackInfo(name As String, value As String)
            If name.Contains("lang") Then
                Dim delimiter As Char = "."c

                ' get the track type (audio/subtitles)
                Dim type As String = name.Split(delimiter)(0)

                ' get the track number (0...N)
                Dim number As String = name.Split(delimiter)(1)

                ' get the three-letter language code
                Dim languageCode As String = value

                Dim codec As String = String.Empty
                If ProtocolVersion.Major >= 3 Then ' also get codec information
                    codec = RawData.Get(name.Replace("lang", "codec"))
                End If

                Dim track As LanguageTrack = ApiWrappers.LanguageTrack.FromCommandResult(type, number, languageCode, codec)

                If type = Constants.CommandResultParameterNames.AudioTrack Then
                    AudioTracks.Add(track.Number, track)
                ElseIf type = Constants.CommandResultParameterNames.SubtitlesTrack Then
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
        ''' <remarks>Version 1 through 3 have this bug. Hopefully it will be fixed in version 3.</remarks>
        Private Function GetSpeedFromBuggedValue(value As String) As Short
            Dim buggedValue As Integer = Convert.ToInt32(value)
            Dim bytes() As Byte = BitConverter.GetBytes(buggedValue)
            Dim fixedValue As Short = BitConverter.ToInt16(bytes, 0)
            Return fixedValue
        End Function

        ''' <summary>
        ''' Gets a string array of the names of properties that are not equal.
        ''' </summary>
        ''' <param name="values">The values to compare.</param>
        ''' <param name="oldValues">The old values to compare with.</param>
        ''' <returns>String array containing the names of properties that are different.</returns>
        ''' <remarks>This is intended as a helper function for implementing INotifyPropertyUpdated.</remarks>
        Public Shared Function GetDifferences(values As CommandResult, oldValues As CommandResult) As IList(Of String)
            Dim differences As New List(Of String)

            If values.ProtocolVersion <> oldValues.ProtocolVersion Then differences.Add("ProtocolVersion")

            If oldValues.CommandStatus.IsNotNullOrEmpty Then ' command status is always unique (and so is the command error)
                differences.Add("CommandStatus")
                If oldValues.ErrorKind.IsNotNullOrEmpty Then
                    differences.Add("ErrorKind")
                    differences.Add("ErrorDescription")
                End If
            End If

            If values.PlayerState <> oldValues.PlayerState Then differences.Add("PlayerState")

            If values.PlaybackState <> oldValues.PlaybackState Then differences.Add("PlaybackState")

            If values.PreviousPlaybackState <> oldValues.PreviousPlaybackState Then differences.Add("PreviousPlaybackState")

            If values.LastPlaybackEvent <> oldValues.LastPlaybackEvent Then differences.Add("LastPlaybackEvent")

            If values.PlaybackUrl <> oldValues.PlaybackUrl Then differences.Add("PlaybackUrl")

            If Not Nullable.Equals(values.PlaybackSpeed, oldValues.PlaybackSpeed) Then differences.Add("PlaybackSpeed")

            If Not Nullable.Equals(values.PlaybackDuration, oldValues.PlaybackDuration) Then differences.Add("PlaybackDuration")

            If Not Nullable.Equals(values.PlaybackPosition, oldValues.PlaybackPosition) Then
                differences.Add("PlaybackPosition")
                differences.Add("PlaybackTimeRemaining")
            End If

            If Not Nullable.Equals(values.PlaybackIsBuffering, oldValues.PlaybackIsBuffering) Then differences.Add("PlaybackIsBuffering")

            If Not Nullable.Equals(values.PlaybackVolume, oldValues.PlaybackVolume) Then differences.Add("PlaybackVolume")

            If Not Nullable.Equals(values.PlaybackMute, oldValues.PlaybackMute) Then differences.Add("PlaybackMute")

            If Not Nullable.Equals(values.PlaybackVideoWidth, oldValues.PlaybackVideoWidth) Then differences.Add("PlaybackVideoWidth")

            If Not Nullable.Equals(values.PlaybackVideoHeight, oldValues.PlaybackVideoHeight) Then differences.Add("PlaybackVideoHeight")

            If Not Nullable.Equals(values.AudioTrack, oldValues.AudioTrack) Then differences.Add("AudioTrack")

            If Not Nullable.Equals(values.SubtitlesTrack, oldValues.SubtitlesTrack) Then differences.Add("SubtitlesTrack")

            If Not Nullable.Equals(values.PlaybackWindowFullscreen, oldValues.PlaybackWindowFullscreen) Then differences.Add("PlaybackWindowFullscreen")

            If Not Nullable.Equals(values.PlaybackWindowRectangleHorizontalOffset, oldValues.PlaybackWindowRectangleHorizontalOffset) Then differences.Add("PlaybackWindowRectangleHorizontalOffset")

            If Not Nullable.Equals(values.PlaybackWindowRectangleVerticalOffset, oldValues.PlaybackWindowRectangleVerticalOffset) Then differences.Add("PlaybackWindowRectangleVerticalOffset")

            If Not Nullable.Equals(values.PlaybackWindowRectangleWidth, oldValues.PlaybackWindowRectangleWidth) Then differences.Add("PlaybackWindowRectangleWidth")

            If Not Nullable.Equals(values.PlaybackWindowRectangleHeight, oldValues.PlaybackWindowRectangleHeight) Then differences.Add("PlaybackWindowRectangleHeight")

            If Not Nullable.Equals(values.PlaybackClipRectangleHorizontalOffset, oldValues.PlaybackClipRectangleHorizontalOffset) Then differences.Add("PlaybackClipRectangleHorizontalOffset")

            If Not Nullable.Equals(values.PlaybackClipRectangleVerticalOffset, oldValues.PlaybackClipRectangleVerticalOffset) Then differences.Add("PlaybackClipRectangleVerticalOffset")

            If Not Nullable.Equals(values.PlaybackClipRectangleWidth, oldValues.PlaybackClipRectangleWidth) Then differences.Add("PlaybackClipRectangleWidth")

            If Not Nullable.Equals(values.PlaybackClipRectangleHeight, oldValues.PlaybackClipRectangleHeight) Then differences.Add("PlaybackClipRectangleHeight")

            If Not Nullable.Equals(values.OnScreenDisplayWidth, oldValues.OnScreenDisplayWidth) Then differences.Add("OnScreenDisplayWidth")

            If Not Nullable.Equals(values.OnScreenDisplayHeight, oldValues.OnScreenDisplayHeight) Then differences.Add("OnScreenDisplayHeight")

            If Not Nullable.Equals(values.VideoEnabled, oldValues.VideoEnabled) Then differences.Add("VideoEnabled")

            If Not Nullable.Equals(values.VideoOnTop, oldValues.VideoOnTop) Then differences.Add("VideoOnTop")

            If Not String.Equals(values.VideoZoom, oldValues.VideoZoom, StringComparison.InvariantCultureIgnoreCase) Then differences.Add("VideoZoom")

            If Not Nullable.Equals(values.PlaybackDvdMenu, oldValues.PlaybackDvdMenu) Then differences.Add("PlaybackDvdMenu")

            If Not Nullable.Equals(values.PlaybackBlurayMenu, oldValues.PlaybackBlurayMenu) Then differences.Add("PlaybackBlurayMenu")

            If Not values.AudioTracks.SequenceEqual(oldValues.AudioTracks) Then differences.Add("AudioTracks")

            If Not values.Subtitles.SequenceEqual(oldValues.Subtitles) Then differences.Add("Subtitles")

            If values.TextAvailable.HasValue AndAlso oldValues.TextAvailable.HasValue Then
                If Not Nullable.Equals(values.TextAvailable, oldValues.TextAvailable) Then
                    differences.Add("TextAvailable")
                End If

                If Not String.Equals(values.Text, oldValues.Text, StringComparison.CurrentCulture) Then
                    differences.Add("Text")
                End If
            End If

            Return differences
        End Function

#End Region ' Methods

    End Class

End Namespace
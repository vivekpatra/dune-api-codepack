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
Imports System.Drawing
Imports System.Text.RegularExpressions

Namespace IPControl

    ''' <summary>
    ''' Represents the result of a command request.
    ''' </summary>
    Public Class StatusCommandResult : Inherits IPCommandResult
        Private Shared TrackMetadataPattern As Regex

        ' command result parameters
        Private _playbackSpeed As PlaybackSpeed
        Private _playbackDuration As TimeSpan? : Private _playbackPosition As TimeSpan?
        Private _playbackTimeRemaining As TimeSpan? : Private _playbackIsBuffering As Boolean?
        Private _playbackVolume As Integer? : Private _audioEnabled As Boolean?
        Private _audioTrack As Integer? : Private _playbackWindowFullscreen As Boolean?
        Private _playbackWindowRectangle As Rectangle? : Private _displaySize As Size?
        Private _videoEnabled As Boolean? : Private _videoZoom As VideoZoom
        Private _audioTracks As ReadOnlyObservableCollection(Of MediaStream)
        Private _subtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
        Private _playbackDvdMenu As Boolean? : Private _playbackBlurayMenu As Boolean?
        Private _playbackClipRectangle As Rectangle? : Private _playbackClipRectanglePosition As Point?
        Private _playbackClipRectangleSize As Size? : Private _playbackVideoSize As Size?
        Private _playbackState As PlaybackState : Private _previousPlaybackState As PlaybackState
        Private _lastPlaybackEvent As PlaybackEvent : Private _playbackUrl As String
        Private _videoOnTop As Boolean? : Private _subtitlesTrack As Integer?

        Shared Sub New()
            Dim options = DirectCast(RegexOptions.Compiled + RegexOptions.CultureInvariant + RegexOptions.IgnoreCase, RegexOptions)
            TrackMetadataPattern = New Regex("(?<type>(subtitles|audio)_track)\.(?<index>[0-9]+)\.(?<metadata>(lang|codec))", options)
        End Sub

        Public Sub New(request As IIPCommand, output As XDocument, requestDateTime As DateTimeOffset)
            MyBase.New(request, output, requestDateTime)
        End Sub

        Protected Overrides Sub MapActions()
            MyBase.MapActions()
            With Actions
                .Add(Output.PlaybackSpeed, AddressOf SetPlaybackSpeed)
                .Add(Output.PlaybackDuration, AddressOf SetPlaybackDuration)
                .Add(Output.PlaybackPosition, AddressOf SetPlaybackPosition)
                .Add(Output.PlaybackIsBuffering, AddressOf SetPlaybackIsBuffering)
                .Add(Output.PlaybackVolume, AddressOf SetPlaybackVolume)
                .Add(Output.PlaybackMute, AddressOf SetAudioEnabled)
                .Add(Output.PlaybackWindowFullscreen, AddressOf SetPlaybackWindowFullscreen)
                .Add(Output.VideoEnabled, AddressOf SetVideoEnabled)
                .Add(Output.VideoZoom, AddressOf SetVideoZoom)
                .Add(Output.VideoOnTop, AddressOf SetVideoOnTop)
                .Add(Output.PlaybackDvdMenu, AddressOf SetPlaybackDvdMenu)
                .Add(Output.PlaybackBlurayDmenu, AddressOf SetPlaybackBlurayMenu)
                .Add(Output.PlaybackState, AddressOf SetPlaybackState)
                .Add(Output.PreviousPlaybackState, AddressOf SetPreviousPlaybackState)
                .Add(Output.LastPlaybackEvent, AddressOf SetLastPlaybackEvent)
                .Add(Output.AudioTrack, AddressOf SetAudioTrack)
                .Add(Output.SubtitlesTrack, AddressOf SetSubtitlesTrack)
                .Add(Output.PlaybackUrl, AddressOf SetPlaybackUrl)
                .Add(Output.OnScreenDisplayWidth, AddressOf SetDisplayWidth)
                .Add(Output.OnScreenDisplayHeight, AddressOf SetDisplayHeight)
                .Add(Output.PlaybackVideoWidth, AddressOf SetPlaybackVideoWidth)
                .Add(Output.PlaybackVideoHeight, AddressOf SetPlaybackVideoHeight)
                .Add(Output.PlaybackWindowRectangleX, AddressOf SetPlaybackWindowRectangleLeft)
                .Add(Output.PlaybackWindowRectangleY, AddressOf SetPlaybackWindowRectangleTop)
                .Add(Output.PlaybackWindowRectangleWidth, AddressOf SetPlaybackWindowRectangleWidth)
                .Add(Output.PlaybackWindowRectangleHeight, AddressOf SetPlaybackWindowRectangleHeight)
                .Add(Output.PlaybackClipRectangleX, AddressOf SetPlaybackClipRectangleLeft)
                .Add(Output.PlaybackClipRectangleY, AddressOf SetPlaybackClipRectangleTop)
                .Add(Output.PlaybackClipRectangleWidth, AddressOf SetPlaybackClipRectangleWidth)
                .Add(Output.PlaybackClipRectangleHeight, AddressOf SetPlaybackClipRectangleHeight)
            End With
        End Sub

        Protected Overrides Sub Initialize()
            MyBase.Initialize()
            Dim tracks = (From items In Me.GetKeyValuePairs
                         Where TrackMetadataPattern.IsMatch(items.Key.Name)
                         Select items).ToDictionary(Function(parameter) parameter.Key, Function(parameter) parameter.Value)

            Me.AudioTracks = New ObservableCollection(Of MediaStream)(GetMediaTracks(Output.AudioTrack, tracks)).AsReadOnly
            Me.SubtitlesTracks = New ObservableCollection(Of MediaStream)(GetMediaTracks(Output.SubtitlesTrack, tracks)).AsReadOnly

            If PlaybackDuration.HasValue AndAlso PlaybackPosition.HasValue Then
                Me.PlaybackTimeRemaining = PlaybackDuration - PlaybackPosition
            End If
        End Sub

 

        Private Iterator Function GetMediaTracks(type As Parameter, source As IDictionary(Of Parameter, String)) As IEnumerable(Of MediaStream)
            If type IsNot Output.AudioTrack And type IsNot Output.SubtitlesTrack Then
                Throw New ArgumentException("invalid type")
            End If

            Dim languages As New Dictionary(Of Integer, String)
            Dim codecs As New Dictionary(Of Integer, String)

            For Each parameter In source
                Dim match = TrackMetadataPattern.Match(parameter.Key.Name)
                If match.Success Then
                    Dim stream = match.Groups("type").Value
                    Dim index = CInt(match.Groups("index").Value)
                    Dim metadata = match.Groups("metadata").Value
                    If New Output(stream) = type Then
                        Select Case metadata
                            Case "lang" : languages.Item(index) = parameter.Value
                            Case "codec" : codecs.Item(index) = parameter.Value
                        End Select
                    End If
                End If
            Next

            For Each item In languages
                Dim codec As String = String.Empty
                codecs.TryGetValue(item.Key, codec)
                Yield MediaStream.Parse(item.Key, type.Name, item.Value, codec)
            Next
        End Function


        Private Sub SetPlaybackSpeed(value As String)
            Me.PlaybackSpeed = IPControl.PlaybackSpeed.Parse(value)
        End Sub

        Private Sub SetPlaybackDuration(value As String)
            Me.PlaybackDuration = ParseTimeSpan(value)
        End Sub

        Private Sub SetPlaybackPosition(value As String)
            Me.PlaybackPosition = ParseTimeSpan(value)
        End Sub

        Private Sub SetPlaybackIsBuffering(value As String)
            Me.PlaybackIsBuffering = ParseBoolean(value)
        End Sub

        Private Sub SetPlaybackVolume(value As String)
            Me.PlaybackVolume = CInt(value)
        End Sub

        Private Sub SetAudioEnabled(value As String)
            Me.AudioEnabled = Not ParseBoolean(value)
        End Sub

        Private Sub SetPlaybackWindowFullscreen(value As String)
            Me.PlaybackWindowFullscreen = ParseBoolean(value)
        End Sub

        Private Sub SetVideoEnabled(value As String)
            Me.VideoEnabled = ParseBoolean(value)
        End Sub

        Private Sub SetVideoZoom(value As String)
            Me.VideoZoom = New VideoZoom(value)
        End Sub

        Private Sub SetVideoOnTop(value As String)
            Me.VideoOnTop = ParseBoolean(value)
        End Sub

        Private Sub SetPlaybackDvdMenu(value As String)
            Me.PlaybackDvdMenu = ParseBoolean(value)
        End Sub

        Private Sub SetPlaybackBlurayMenu(value As String)
            Me.PlaybackBlurayMenu = ParseBoolean(value)
        End Sub

        Private Sub SetPlaybackState(value As String)
            Me.PlaybackState = New PlaybackState(value)
        End Sub

        Private Sub SetPreviousPlaybackState(value As String)
            Me.PreviousPlaybackState = New PlaybackState(value)
        End Sub

        Private Sub SetLastPlaybackEvent(value As String)
            Me.LastPlaybackEvent = New PlaybackEvent(value)
        End Sub

        Private Sub SetAudioTrack(value As String)
            Me.AudioTrack = CInt(value)
        End Sub

        Private Sub SetSubtitlesTrack(value As String)
            Me.SubtitlesTrack = CInt(value)
        End Sub

        Private Sub SetPlaybackUrl(value As String)
            Me.PlaybackUrl = value
        End Sub

        Private Sub SetDisplayWidth(value As String)
            Dim height = Me.DisplaySize.GetValueOrDefault.Height
            Me.DisplaySize = New Size(CInt(value), height)
        End Sub

        Private Sub SetDisplayHeight(value As String)
            Dim width = Me.DisplaySize.GetValueOrDefault.Width
            Me.DisplaySize = New Size(width, CInt(value))
        End Sub

        Private Sub SetPlaybackVideoWidth(value As String)
            Dim height = Me.PlaybackVideoSize.GetValueOrDefault.Height
            Me.PlaybackVideoSize = New Size(CInt(value), height)
        End Sub

        Private Sub SetPlaybackVideoHeight(value As String)
            Dim width = Me.PlaybackVideoSize.GetValueOrDefault.Width
            Me.PlaybackVideoSize = New Size(width, CInt(value))
        End Sub

        Private Sub SetPlaybackWindowRectangleLeft(value As String)
            Dim rectangle = Me.PlaybackWindowRectangle.GetValueOrDefault
            Me.PlaybackWindowRectangle = New Rectangle(CInt(value), rectangle.Y, rectangle.Width, rectangle.Height)
        End Sub

        Private Sub SetPlaybackWindowRectangleTop(value As String)
            Dim rectangle = Me.PlaybackWindowRectangle.GetValueOrDefault
            Me.PlaybackWindowRectangle = New Rectangle(rectangle.X, CInt(value), rectangle.Width, rectangle.Height)
        End Sub

        Private Sub SetPlaybackWindowRectangleWidth(value As String)
            Dim rectangle = Me.PlaybackWindowRectangle.GetValueOrDefault
            Me.PlaybackWindowRectangle = New Rectangle(rectangle.X, rectangle.Y, CInt(value), rectangle.Height)
        End Sub

        Private Sub SetPlaybackWindowRectangleHeight(value As String)
            Dim rectangle = Me.PlaybackWindowRectangle.GetValueOrDefault
            Me.PlaybackWindowRectangle = New Rectangle(rectangle.X, rectangle.Y, rectangle.Width, CInt(value))
        End Sub

        Private Sub SetPlaybackClipRectangleLeft(value As String)
            Dim rectangle = Me.PlaybackClipRectangle.GetValueOrDefault
            Me.PlaybackClipRectangle = New Rectangle(CInt(value), rectangle.Y, rectangle.Width, rectangle.Height)
        End Sub

        Private Sub SetPlaybackClipRectangleTop(value As String)
            Dim rectangle = Me.PlaybackClipRectangle.GetValueOrDefault
            Me.PlaybackClipRectangle = New Rectangle(rectangle.X, CInt(value), rectangle.Width, rectangle.Height)
        End Sub

        Private Sub SetPlaybackClipRectangleWidth(value As String)
            Dim rectangle = Me.PlaybackClipRectangle.GetValueOrDefault
            Me.PlaybackClipRectangle = New Rectangle(rectangle.X, rectangle.Y, CInt(value), rectangle.Height)
        End Sub

        Private Sub SetPlaybackClipRectangleHeight(value As String)
            Dim rectangle = Me.PlaybackClipRectangle.GetValueOrDefault
            Me.PlaybackClipRectangle = New Rectangle(rectangle.X, rectangle.Y, rectangle.Width, CInt(value))
        End Sub

        ''' <summary>
        ''' Gets the playback speed.
        ''' </summary>
        Public Property PlaybackSpeed As PlaybackSpeed
            Get
                Return _playbackSpeed
            End Get
            Private Set(value As PlaybackSpeed)
                _playbackSpeed = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback duration.
        ''' </summary>
        Public Property PlaybackDuration As TimeSpan?
            Get
                Return _playbackDuration
            End Get
            Private Set(value As TimeSpan?)
                _playbackDuration = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback position.
        ''' </summary>
        Public Property PlaybackPosition As TimeSpan?
            Get
                Return _playbackPosition
            End Get
            Private Set(value As TimeSpan?)
                _playbackPosition = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the remaining playback time.
        ''' </summary>
        ''' <remarks>This value is not part of the command results. Instead, it is calculated by substracting the playback position from the playback duration.</remarks>
        Public Property PlaybackTimeRemaining As TimeSpan?
            Get
                Return _playbackTimeRemaining
            End Get
            Private Set(value As TimeSpan?)
                _playbackTimeRemaining = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the playback is buffering.
        ''' </summary>
        Public Property PlaybackIsBuffering As Boolean?
            Get
                Return _playbackIsBuffering
            End Get
            Private Set(value As Boolean?)
                _playbackIsBuffering = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback volume.
        ''' </summary>
        Public Property PlaybackVolume As Integer?
            Get
                Return _playbackVolume
            End Get
            Private Set(value As Integer?)
                _playbackVolume = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether audio is enabled.
        ''' </summary>
        Public Property AudioEnabled As Boolean?
            Get
                Return _audioEnabled
            End Get
            Private Set(value As Boolean?)
                _audioEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the zero-based index number of active audio track.
        ''' </summary>
        Public Property AudioTrack As Integer?
            Get
                Return _audioTrack
            End Get
            Private Set(value As Integer?)
                _audioTrack = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the video output is in fullscreen.
        ''' </summary>
        Public Property PlaybackWindowFullscreen As Boolean?
            Get
                Return _playbackWindowFullscreen
            End Get
            Private Set(value As Boolean?)
                _playbackWindowFullscreen = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback window rectangle.
        ''' </summary>
        Public Property PlaybackWindowRectangle As Rectangle?
            Get
                Return _playbackWindowRectangle
            End Get
            Private Set(value As Rectangle?)
                _playbackWindowRectangle = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the total display size.
        ''' </summary>
        Public Property DisplaySize As Size?
            Get
                Return _displaySize
            End Get
            Private Set(value As Size?)
                _displaySize = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the video output is enabled.
        ''' </summary>
        Public Property VideoEnabled As Boolean?
            Get
                Return _videoEnabled
            End Get
            Private Set(value As Boolean?)
                _videoEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the video output zoom.
        ''' </summary>
        Public Property VideoZoom As VideoZoom
            Get
                Return _videoZoom
            End Get
            Private Set(value As VideoZoom)
                _videoZoom = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the collection of audio tracks in the current playback.
        ''' </summary>
        Public Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Return _audioTracks
            End Get
            Private Set(value As ReadOnlyObservableCollection(Of MediaStream))
                _audioTracks = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the collection of subtitles in the current playback.
        ''' </summary>
        Public Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Return _subtitlesTracks
            End Get
            Private Set(value As ReadOnlyObservableCollection(Of MediaStream))
                _subtitlesTracks = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether a DVD menu is currently shown.
        ''' </summary>
        Public Property PlaybackDvdMenu As Boolean?
            Get
                Return _playbackDvdMenu
            End Get
            Private Set(value As Boolean?)
                _playbackDvdMenu = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether a Blu-ray menu is currently shown.
        ''' </summary>
        Public Property PlaybackBlurayMenu As Boolean?
            Get
                Return _playbackBlurayMenu
            End Get
            Private Set(value As Boolean?)
                _playbackBlurayMenu = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback state.
        ''' </summary>
        Public Property PlaybackState As PlaybackState
            Get
                Return _playbackState
            End Get
            Private Set(value As PlaybackState)
                _playbackState = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the previous playback state.
        ''' </summary>
        Public Property PreviousPlaybackState As PlaybackState
            Get
                Return _previousPlaybackState
            End Get
            Private Set(value As PlaybackState)
                _previousPlaybackState = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the last playback event.
        ''' </summary>
        Public Property LastPlaybackEvent As PlaybackEvent
            Get
                Return _lastPlaybackEvent
            End Get
            Private Set(value As PlaybackEvent)
                _lastPlaybackEvent = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the media URL that is currently playing.
        ''' </summary>
        Public Property PlaybackUrl As String
            Get
                Return _playbackUrl
            End Get
            Private Set(value As String)
                _playbackUrl = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether video output shows on top of overlay graphics.
        ''' </summary>
        Public Property VideoOnTop As Boolean?
            Get
                Return _videoOnTop
            End Get
            Private Set(value As Boolean?)
                _videoOnTop = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether subtitles are enabled.
        ''' </summary>
        Public ReadOnly Property SubtitlesEnabled As Boolean?
            Get
                If Me.PlayerState.IsPlaybackState Then
                    Return Me.SubtitlesTrack.HasValue
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the zero-based index number of active subtitle track.
        ''' </summary>
        Public Property SubtitlesTrack As Integer?
            Get
                Return _subtitlesTrack
            End Get
            Private Set(value As Integer?)
                _subtitlesTrack = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the video size.
        ''' </summary>
        Public Property PlaybackVideoSize As Size?
            Get
                Return _playbackVideoSize
            End Get
            Private Set(value As Size?)
                _playbackVideoSize = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the playback clip rectangle.
        ''' </summary>
        Public Property PlaybackClipRectangle As Rectangle?
            Get
                Return _playbackClipRectangle
            End Get
            Private Set(value As Rectangle?)
                _playbackClipRectangle = value
            End Set
        End Property

        Private Shared Function ParseTimeSpan(value As String) As TimeSpan
            Return TimeSpan.FromSeconds(CInt(value))
        End Function

        Private Shared Function ParseBoolean(value As String) As Boolean
            If String.IsNullOrEmpty(value) Then
                Return False
            End If
            Select Case value.ToLowerInvariant
                Case "0", Boolean.FalseString.ToLowerInvariant
                    Return False
                Case Else
                    Return True
            End Select
        End Function

        ''' <summary>
        ''' Compares the current instance to the specified instance. an <see cref="IEnumerable(Of String)" /> containing the names of properties where the values differ from the specified .
        ''' </summary>
        ''' <returns>Returns an <see cref="IEnumerable(Of String)" /> containing the names of properties where the property value differs from the specified instance.</returns>
        Public Overrides Function GetDifferences(values As StatusCommandResult) As IEnumerable(Of String)
            Dim differences As New List(Of String)(MyBase.GetDifferences(values))

            If Not Object.Equals(values.PlaybackState, Me.PlaybackState) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackState))
            End If

            If Not Object.Equals(values.PreviousPlaybackState, Me.PreviousPlaybackState) Then
                differences.Add(GetPropertyName(Function() Me.PreviousPlaybackState))
            End If

            If Not Object.Equals(values.LastPlaybackEvent, Me.LastPlaybackEvent) Then
                differences.Add(GetPropertyName(Function() Me.LastPlaybackEvent))
            End If

            If Not Object.Equals(values.PlaybackUrl, Me.PlaybackUrl) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackUrl))
            End If

            If Not Object.Equals(values.PlaybackSpeed, Me.PlaybackSpeed) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackSpeed))
            End If

            If Not Object.Equals(values.PlaybackDuration, Me.PlaybackDuration) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackDuration))
            End If

            If Not Object.Equals(values.PlaybackPosition, Me.PlaybackPosition) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackPosition))
                differences.Add(GetPropertyName(Function() Me.PlaybackTimeRemaining))
            End If

            If Not Object.Equals(values.PlaybackIsBuffering, Me.PlaybackIsBuffering) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackIsBuffering))
            End If

            If Not Object.Equals(values.PlaybackVolume, Me.PlaybackVolume) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackVolume))
            End If

            If Not Object.Equals(values.AudioEnabled, Me.AudioEnabled) Then
                differences.Add(GetPropertyName(Function() Me.AudioEnabled))
            End If

            If Not Object.Equals(values.PlaybackVideoSize, Me.PlaybackVideoSize) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackVideoSize))
            End If

            If Not Object.Equals(values.AudioTrack, Me.AudioTrack) Then
                differences.Add(GetPropertyName(Function() Me.AudioTrack))
            End If

            If Not Object.Equals(values.SubtitlesEnabled, Me.SubtitlesEnabled) Then
                differences.Add(GetPropertyName(Function() Me.SubtitlesEnabled))
            End If

            If Not Object.Equals(values.SubtitlesTrack, Me.SubtitlesTrack) Then
                differences.Add(GetPropertyName(Function() Me.SubtitlesTrack))
            End If

            If Not Object.Equals(values.PlaybackWindowFullscreen, Me.PlaybackWindowFullscreen) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackWindowFullscreen))
            End If

            If Not Object.Equals(values.PlaybackWindowRectangle, Me.PlaybackWindowRectangle) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackWindowRectangle))
            End If

            If Not Object.Equals(values.PlaybackClipRectangle, Me.PlaybackClipRectangle) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackClipRectangle))
            End If

            If Not Object.Equals(values.DisplaySize, Me.DisplaySize) Then
                differences.Add(GetPropertyName(Function() Me.DisplaySize))
            End If

            If Not Object.Equals(values.VideoEnabled, Me.VideoEnabled) Then
                differences.Add(GetPropertyName(Function() Me.VideoEnabled))
            End If

            If Not Object.Equals(values.VideoOnTop, Me.VideoOnTop) Then
                differences.Add(GetPropertyName(Function() Me.VideoOnTop))
            End If

            If Not Object.Equals(values.VideoZoom, Me.VideoZoom) Then
                differences.Add(GetPropertyName(Function() Me.VideoZoom))
            End If

            If Not Object.Equals(values.PlaybackDvdMenu, Me.PlaybackDvdMenu) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackDvdMenu))
            End If

            If Not Object.Equals(values.PlaybackBlurayMenu, Me.PlaybackBlurayMenu) Then
                differences.Add(GetPropertyName(Function() Me.PlaybackBlurayMenu))
            End If

            If Not values.AudioTracks.SequenceEqual(Me.AudioTracks) Then
                differences.Add(GetPropertyName(Function() Me.AudioTracks))
            End If

            If Not values.SubtitlesTracks.SequenceEqual(Me.SubtitlesTracks) Then
                differences.Add(GetPropertyName(Function() Me.SubtitlesTracks))
            End If

            Return differences
        End Function



    End Class

End Namespace
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
Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to change various playback options.</summary>
    Public Class SetPlaybackStateCommand
        Inherits Command

        Private _playbackSpeed As Short?
        Private _playbackPosition As TimeSpan?
        Private _blackScreen As Boolean?
        Private _hideOnScreenDisplay As Boolean?
        Private _repeat As Boolean?
        Private _videoEnabled As Boolean?
        Private _playbackVolume As Short?
        Private _playbackMute As Boolean?
        Private _audioTrack As Short?
        Private _subtitleTrack As Short?
        Private _videoZoom As String
        Private _videoOnTop As Boolean?


        Public Sub New(target As Dune)
            MyBase.New(target)
        End Sub

#Region "Properties"

#Region "Protocol version 2"

        ''' <summary>
        ''' Gets or sets the playback speed.
        ''' </summary>
        Public Property PlaybackSpeed As Constants.PlaybackSpeedValues?
            Get
                If _playbackSpeed.HasValue Then
                    Return DirectCast(_playbackSpeed.Value, Constants.PlaybackSpeedValues)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Constants.PlaybackSpeedValues?)
                If value.HasValue Then
                    _playbackSpeed = CShort(value)
                Else
                    _playbackSpeed = Nothing
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position.
        ''' </summary>
        Public Property PlaybackPosition As TimeSpan?
            Get
                Return _playbackPosition
            End Get
            Set(value As TimeSpan?)
                _playbackPosition = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show a black screen.
        ''' </summary>
        Public Property BlackScreen As Boolean?
            Get
                Return _blackScreen
            End Get
            Set(value As Boolean?)
                _blackScreen = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to hide the OSD.
        ''' </summary>
        Public Property HideOnScreenDisplay As Boolean?
            Get
                Return _hideOnScreenDisplay
            End Get
            Set(value As Boolean?)
                _hideOnScreenDisplay = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to repeat the playback.
        ''' </summary>
        Public Property Repeat As Boolean?
            Get
                Return _repeat
            End Get
            Set(value As Boolean?)
                _repeat = value
            End Set
        End Property

#End Region ' Protocol version 1

#Region "Protocol version 2"

        ''' <summary>
        ''' Gets or sets whether to show video output.
        ''' </summary>
        Public Property VideoEnabled As Boolean?
            Get
                Return _videoEnabled
            End Get
            Set(value As Boolean?)
                _videoEnabled = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the volume.
        ''' </summary>
        ''' <value>Must be between 0 and 100. Values above 100 are automatically reduced to 100.</value>
        Public Property PlaybackVolume As Short?
            Get
                Return _playbackVolume
            End Get
            Set(value As Short?)
                _playbackVolume = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to mute the playback.
        ''' </summary>
        Public Property PlaybackMute As Boolean?
            Get
                Return _playbackMute
            End Get
            Set(value As Boolean?)
                _playbackMute = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the audio track number that is used in the current playback.
        ''' </summary>
        ''' <remarks>If a file contains multiple tracks (e.g. different languages, directors commentary), this property can be used to change the track.</remarks>
        Public Property AudioTrack As Short?
            Get
                Return _audioTrack
            End Get
            Set(value As Short?)
                _audioTrack = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the video zoom mode.
        ''' </summary>
        Public Property VideoZoom As String
            Get
                Return _videoZoom
            End Get
            Set(value As String)
                _videoZoom = value
            End Set
        End Property

#End Region ' Protocol version 2

#Region "Protocol version 3"

        ''' <summary>
        ''' Gets or sets whether to show video on top.
        ''' </summary>
        Public Property VideoOnTop As Boolean?
            Get
                Return _videoOnTop
            End Get
            Set(value As Boolean?)
                _videoOnTop = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the active subtitle track.
        ''' </summary>
        Public Property SubtitleTrack As Short?
            Get
                Return _subtitleTrack
            End Get
            Set(value As Short?)
                _subtitleTrack = value
            End Set
        End Property

#End Region

#End Region ' Properties

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.SetPlaybackState)

            If PlaybackSpeed.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.PlaybackSpeed, CShort(PlaybackSpeed).ToString)
            End If

            If PlaybackPosition.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.PlaybackPosition, PlaybackPosition.Value.TotalSeconds.ToString)
            End If

            If BlackScreen.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.BlackScreen, BlackScreen.Value.ToNumberString)
            End If

            If HideOnScreenDisplay.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.HideOnScreenDisplay, HideOnScreenDisplay.Value.ToNumberString)
            End If

            If Repeat.HasValue Then
                If Repeat.Value.IsTrue Then
                    query.Add(Constants.SetPlaybackStateParameterNames.ActionOnFinish, Constants.ActionOnFinishValues.RestartPlayback)
                Else
                    query.Add(Constants.SetPlaybackStateParameterNames.ActionOnFinish, Constants.ActionOnFinishValues.Exit)
                End If
            End If

            If VideoEnabled.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.VideoEnabled, VideoEnabled.Value.ToNumberString)
            End If

            If PlaybackVolume.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.PlaybackVolume, PlaybackVolume.ToString)
            End If

            If PlaybackMute.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.PlaybackMute, PlaybackMute.Value.ToNumberString)
            End If

            If AudioTrack.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.AudioTrack, AudioTrack.ToString)
            End If

            If SubtitleTrack.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.SubtitlesTrack, SubtitleTrack.ToString)
            End If

            If VideoZoom.IsNotNullOrWhiteSpace Then
                query.Add(Constants.SetPlaybackStateParameterNames.VideoZoom, VideoZoom)
            End If

            If VideoOnTop.HasValue Then
                query.Add(Constants.SetPlaybackStateParameterNames.VideoOnTop, VideoOnTop.Value.ToNumberString)
            End If

            Return query
        End Function
    End Class

End Namespace
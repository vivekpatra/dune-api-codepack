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

Namespace IPControl

    Public Class Output : Inherits Parameter

        Private Shared ReadOnly _protocolVersion As Output = New Output("protocol_version")
        Private Shared ReadOnly _playerState As Output = New Output("player_state")
        Private Shared ReadOnly _commandStatus As Output = New Output("command_status")
        Private Shared ReadOnly _errorKind As Output = New Output("error_kind")
        Private Shared ReadOnly _errorDescription As Output = New Output("error_description")
        Private Shared ReadOnly _playbackSpeed As Output = New Output("playback_speed")
        Private Shared ReadOnly _playbackDuration As Output = New Output("playback_duration")
        Private Shared ReadOnly _playbackPosition As Output = New Output("playback_position")
        Private Shared ReadOnly _playbackIsBuffering As Output = New Output("playback_is_buffering")
        Private Shared ReadOnly _playbackDvdMenu As Output = New Output("playback_dvd_menu")
        Private Shared ReadOnly _playbackBlurayDmenu As Output = New Output("playback_bluray_dmenu")
        Private Shared ReadOnly _playbackVolume As Output = New Output("playback_volume")
        Private Shared ReadOnly _playbackMute As Output = New Output("playback_mute")
        Private Shared ReadOnly _audioTrack As Output = New Output("audio_track")
        Private Shared ReadOnly _subtitlesTrack As Output = New Output("subtitles_track")
        Private Shared ReadOnly _videoEnabled As Output = New Output("video_enabled")
        Private Shared ReadOnly _videoZoom As Output = New Output("video_zoom")
        Private Shared ReadOnly _videoOnTop As Output = New Output("video_on_top")
        Private Shared ReadOnly _playbackState As Output = New Output("playback_state")
        Private Shared ReadOnly _previousPlaybackState As Output = New Output("previous_playback_state")
        Private Shared ReadOnly _lastPlaybackEvent As Output = New Output("last_playback_event")
        Private Shared ReadOnly _playbackUrl As Output = New Output("playback_url")
        Private Shared ReadOnly _playbackVideoWidth As Output = New Output("playback_video_width")
        Private Shared ReadOnly _playbackVideoHeight As Output = New Output("playback_video_height")
        Private Shared ReadOnly _text As Output = New Output("text")
        Private Shared ReadOnly _videoTotalDisplayWidth As Output = New Output("video_total_display_width")
        Private Shared ReadOnly _videoTotalDisplayHeight As Output = New Output("video_total_display_height")
        Private Shared ReadOnly _onScreenDisplayWidth As Output = New Output("osd_width")
        Private Shared ReadOnly _onScreenDisplayHeight As Output = New Output("osd_height")
        Private Shared ReadOnly _videoFullscreen As Output = New Output("video_fullscreen")
        Private Shared ReadOnly _videoX As Output = New Output("video_x")
        Private Shared ReadOnly _videoY As Output = New Output("video_y")
        Private Shared ReadOnly _videoWidth As Output = New Output("video_width")
        Private Shared ReadOnly _videoHeight As Output = New Output("video_height")
        Private Shared ReadOnly _playbackWindowFullscreen As Output = New Output("playback_window_fullscreen")
        Private Shared ReadOnly _playbackWindowRectangleX As Output = New Output("playback_window_rect_x")
        Private Shared ReadOnly _playbackWindowRectangleY As Output = New Output("playback_window_rect_y")
        Private Shared ReadOnly _playbackWindowRectangleWidth As Output = New Output("playback_window_rect_width")
        Private Shared ReadOnly _playbackWindowRectangleHeight As Output = New Output("playback_window_rect_height")
        Private Shared ReadOnly _playbackClipRectangleX As Output = New Output("playback_clip_rect_x")
        Private Shared ReadOnly _playbackClipRectangleY As Output = New Output("playback_clip_rect_y")
        Private Shared ReadOnly _playbackClipRectangleWidth As Output = New Output("playback_clip_rect_width")
        Private Shared ReadOnly _playbackClipRectangleHeight As Output = New Output("playback_clip_rect_height")

        Private Shared ReadOnly _listName As Output = New Output("ls_name")
        Private Shared ReadOnly _listType As Output = New Output("ls_type")
        Private Shared ReadOnly _listIconUrl As Output = New Output("ls_icon_url")
        Private Shared ReadOnly _listMetadata As Output = New Output("ls_metadata")

        Private Shared ReadOnly _audioTrackMetadataPattern As Output = New Output("(?<type>audio_track)\.(?<index>[0-9]+)\.(?<metadata>(lang|codec))")
        Private Shared ReadOnly _subtitlesTrackMetadataPattern As Output = New Output("(?<type>subtitles_track)\.(?<index>[0-9]+)\.(?<metadata>(lang|codec))")

        Public Sub New(name As String)
            MyBase.New(name)
        End Sub

        Public Shared ReadOnly Property ProtocolVersion As Output
            Get
                Return Output._protocolVersion
            End Get
        End Property

        Public Shared ReadOnly Property PlayerState As Output
            Get
                Return Output._playerState
            End Get
        End Property

        Public Shared ReadOnly Property CommandStatus As Output
            Get
                Return Output._commandStatus
            End Get
        End Property

        Public Shared ReadOnly Property ErrorKind As Output
            Get
                Return Output._errorKind
            End Get
        End Property

        Public Shared ReadOnly Property ErrorDescription As Output
            Get
                Return Output._errorDescription
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackSpeed As Output
            Get
                Return Output._playbackSpeed
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackDuration As Output
            Get
                Return Output._playbackDuration
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackPosition As Output
            Get
                Return Output._playbackPosition
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackIsBuffering As Output
            Get
                Return Output._playbackIsBuffering
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackDvdMenu As Output
            Get
                Return Output._playbackDvdMenu
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackBlurayDmenu As Output
            Get
                Return Output._playbackBlurayDmenu
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackVolume As Output
            Get
                Return Output._playbackVolume
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackMute As Output
            Get
                Return Output._playbackMute
            End Get
        End Property

        Public Shared ReadOnly Property AudioTrack As Output
            Get
                Return Output._audioTrack
            End Get
        End Property

        Public Shared ReadOnly Property SubtitlesTrack As Output
            Get
                Return Output._subtitlesTrack
            End Get
        End Property

        Public Shared ReadOnly Property VideoEnabled As Output
            Get
                Return Output._videoEnabled
            End Get

        End Property

        Public Shared ReadOnly Property VideoZoom As Output
            Get
                Return Output._videoZoom
            End Get
        End Property

        Public Shared ReadOnly Property VideoOnTop As Output
            Get
                Return Output._videoOnTop
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackState As Output
            Get
                Return Output._playbackState
            End Get
        End Property

        Public Shared ReadOnly Property PreviousPlaybackState As Output
            Get
                Return Output._previousPlaybackState
            End Get
        End Property

        Public Shared ReadOnly Property LastPlaybackEvent As Output
            Get
                Return Output._lastPlaybackEvent
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackUrl As Output
            Get
                Return Output._playbackUrl
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackVideoWidth As Output
            Get
                Return Output._playbackVideoWidth
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackVideoHeight As Output
            Get
                Return Output._playbackVideoHeight
            End Get
        End Property

        Public Shared ReadOnly Property Text As Output
            Get
                Return Output._text
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoTotalDisplayWidth As Output
            Get
                Return Output._videoTotalDisplayWidth
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoTotalDisplayHeight As Output
            Get
                Return Output._videoTotalDisplayHeight
            End Get
        End Property

        Public Shared ReadOnly Property OnScreenDisplayWidth As Output
            Get
                Return Output._onScreenDisplayWidth
            End Get
        End Property

        Public Shared ReadOnly Property OnScreenDisplayHeight As Output
            Get
                Return Output._onScreenDisplayHeight
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoFullscreen As Output
            Get
                Return Output._videoFullscreen
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoX As Output
            Get
                Return Output._videoX
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoY As Output
            Get
                Return Output._videoY
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoWidth As Output
            Get
                Return Output._videoWidth
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoHeight As Output
            Get
                Return Output._videoHeight
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackWindowFullscreen As Output
            Get
                Return Output._playbackWindowFullscreen
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackWindowRectangleX As Output
            Get
                Return Output._playbackWindowRectangleX
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackWindowRectangleY As Output
            Get
                Return Output._playbackWindowRectangleY
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackWindowRectangleWidth As Output
            Get
                Return Output._playbackWindowRectangleWidth
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackWindowRectangleHeight As Output
            Get
                Return Output._playbackWindowRectangleHeight
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackClipRectangleX As Output
            Get
                Return Output._playbackClipRectangleX
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackClipRectangleY As Output
            Get
                Return Output._playbackClipRectangleY
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackClipRectangleWidth As Output
            Get
                Return Output._playbackClipRectangleWidth
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackClipRectangleHeight As Output
            Get
                Return Output._playbackClipRectangleHeight
            End Get
        End Property

        Public Shared ReadOnly Property AudioTrackMetadataPattern As Output
            Get
                Return Output._audioTrackMetadataPattern
            End Get
        End Property

        Public Shared ReadOnly Property SubtitlesTrackMetadataPattern As Output
            Get
                Return Output._subtitlesTrackMetadataPattern
            End Get
        End Property

        Public Shared ReadOnly Property ListName As Output
            Get
                Return Output._listName
            End Get
        End Property

        Public Shared ReadOnly Property ListType As Output
            Get
                Return Output._listType
            End Get
        End Property

        Public Shared ReadOnly Property ListIconUrl As Output
            Get
                Return Output._listIconUrl
            End Get
        End Property

        Public Shared ReadOnly Property ListMetadata As Output
            Get
                Return Output._listMetadata
            End Get
        End Property

    End Class

End Namespace
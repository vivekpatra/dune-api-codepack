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

    Public Class Input : Inherits Parameter

        Private Shared ReadOnly _command As Input = New Input("cmd")
        Private Shared ReadOnly _timeout As Input = New Input("timeout")

        Private Shared ReadOnly _irCode As Input = New Input("ir_code")

        Private Shared ReadOnly _action As Input = New Input("action")

        Private Shared ReadOnly _mediaUrl As Input = New Input("media_url")
        Private Shared ReadOnly _playbackSpeed As Input = New Input("speed")
        Private Shared ReadOnly _playbackPosition As Input = New Input("position")
        Private Shared ReadOnly _blackScreen As Input = New Input("black_screen")
        Private Shared ReadOnly _hideOnScreenDisplay As Input = New Input("hide_osd")
        Private Shared ReadOnly _actionOnFinish As Input = New Input("action_on_finish")
        Private Shared ReadOnly _actionOnExit As Input = New Input("action_on_exit")
        Private Shared ReadOnly _skipFrames As Input = New Input("skip_frames")
        Private Shared ReadOnly _playbackVolume As Input = New Input("volume")
        Private Shared ReadOnly _playbackMute As Input = New Input("mute")
        Private Shared ReadOnly _audioTrack As Input = New Input("audio_track")
        Private Shared ReadOnly _subtitlesTrack As Input = New Input("subtitles_track")
        Private Shared ReadOnly _videoEnabled As Input = New Input("video_enabled")
        Private Shared ReadOnly _videoZoom As Input = New Input("video_zoom")
        Private Shared ReadOnly _videoOnTop As Input = New Input("video_on_top")
        Private Shared ReadOnly _videoFullscreen As Input = New Input("video_fullscreen")
        Private Shared ReadOnly _videoX As Input = New Input("video_x")
        Private Shared ReadOnly _videoY As Input = New Input("video_y")
        Private Shared ReadOnly _videoWidth As Input = New Input("video_width")
        Private Shared ReadOnly _videoHeight As Input = New Input("video_height")
        Private Shared ReadOnly _windowFullscreen As Input = New Input("window_fullscreen")
        Private Shared ReadOnly _windowRectangleX As Input = New Input("window_rect_x")
        Private Shared ReadOnly _windowRectangleY As Input = New Input("window_rect_y")
        Private Shared ReadOnly _windowRectangleWidth As Input = New Input("window_rect_width")
        Private Shared ReadOnly _windowRectangleHeight As Input = New Input("window_rect_height")
        Private Shared ReadOnly _clipRectangleX As Input = New Input("clip_rect_x")
        Private Shared ReadOnly _clipRectangleY As Input = New Input("clip_rect_y")
        Private Shared ReadOnly _clipRectangleWidth As Input = New Input("clip_rect_width")
        Private Shared ReadOnly _clipRectangleHeight As Input = New Input("clip_rect_height")

        Private Shared ReadOnly _startIndex As Input = New Input("start_index")

        Private Shared ReadOnly _text As Input = New Input("text")

        Private Shared ReadOnly _fullscreen As Input = New Input("fullscreen")
        Private Shared ReadOnly _webappKeys As Input = New Input("webapp_keys")
        Private Shared ReadOnly _zoomLevel As Input = New Input("zoom_level")
        Private Shared ReadOnly _overscan As Input = New Input("overscan")
        Private Shared ReadOnly _userAgent As Input = New Input("user_agent")
        Private Shared ReadOnly _backgroundColor As Input = New Input("background_color")
        Private Shared ReadOnly _onScreenDisplaySize As Input = New Input("osd_size")

        Private Shared ReadOnly _folderUrl As Input = New Input("folder_url")


        Public Sub New(name As String)
            MyBase.New(name)
        End Sub

        Public Shared ReadOnly Property Command As Input
            Get
                Return Input._command
            End Get
        End Property

        Public Shared ReadOnly Property Timeout As Input
            Get
                Return Input._timeout
            End Get
        End Property

        Public Shared ReadOnly Property IRCode As Input
            Get
                Return Input._irCode
            End Get
        End Property

        Public Shared ReadOnly Property Action As Input
            Get
                Return Input._action
            End Get
        End Property

        Public Shared ReadOnly Property MediaUrl As Input
            Get
                Return Input._mediaUrl
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackSpeed As Input
            Get
                Return Input._playbackSpeed
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackPosition As Input
            Get
                Return Input._playbackPosition
            End Get
        End Property

        Public Shared ReadOnly Property BlackScreen As Input
            Get
                Return Input._blackScreen
            End Get
        End Property

        Public Shared ReadOnly Property HideOnScreenDisplay As Input
            Get
                Return Input._hideOnScreenDisplay
            End Get
        End Property

        Public Shared ReadOnly Property ActionOnFinish As Input
            Get
                Return Input._actionOnFinish
            End Get
        End Property

        Public Shared ReadOnly Property ActionOnExit As Input
            Get
                Return Input._actionOnExit
            End Get
        End Property

        Public Shared ReadOnly Property SkipFrames As Input
            Get
                Return Input._skipFrames
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackVolume As Input
            Get
                Return Input._playbackVolume
            End Get
        End Property

        Public Shared ReadOnly Property PlaybackMute As Input
            Get
                Return Input._playbackMute
            End Get
        End Property

        Public Shared ReadOnly Property AudioTrack As Input
            Get
                Return Input._audioTrack
            End Get
        End Property

        Public Shared ReadOnly Property SubtitlesTrack As Input
            Get
                Return Input._subtitlesTrack
            End Get
        End Property

        Public Shared ReadOnly Property VideoEnabled As Input
            Get
                Return Input._videoEnabled
            End Get
        End Property

        Public Shared ReadOnly Property VideoZoom As Input
            Get
                Return Input._videoZoom
            End Get
        End Property

        Public Shared ReadOnly Property VideoOnTop As Input
            Get
                Return Input._videoOnTop
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoFullScreen As Input
            Get
                Return Input._videoFullscreen
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoLeft As Input
            Get
                Return Input._videoX
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoTop As Input
            Get
                Return Input._videoY
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoWidth As Input
            Get
                Return Input._videoWidth
            End Get
        End Property

        <Obsolete("This parameter has been renamed in protocol version 3")>
        Public Shared ReadOnly Property VideoHeight As Input
            Get
                Return Input._videoHeight
            End Get
        End Property

        Public Shared ReadOnly Property WindowFullscreen As Input
            Get
                Return Input._windowFullscreen
            End Get
        End Property

        Public Shared ReadOnly Property WindowRectangleLeft As Input
            Get
                Return Input._windowRectangleX
            End Get
        End Property

        Public Shared ReadOnly Property WindowRectangleTop As Input
            Get
                Return Input._windowRectangleY
            End Get
        End Property

        Public Shared ReadOnly Property WindowRectangleWidth As Input
            Get
                Return Input._windowRectangleWidth
            End Get
        End Property

        Public Shared ReadOnly Property WindowRectangleHeight As Input
            Get
                Return Input._windowRectangleHeight
            End Get
        End Property

        Public Shared ReadOnly Property ClipRectangleLeft As Input
            Get
                Return Input._clipRectangleX
            End Get
        End Property

        Public Shared ReadOnly Property ClipRectangleTop As Input
            Get
                Return Input._clipRectangleY
            End Get
        End Property

        Public Shared ReadOnly Property ClipRectangleWidth As Input
            Get
                Return Input._clipRectangleWidth
            End Get
        End Property

        Public Shared ReadOnly Property ClipRectangleHeight As Input
            Get
                Return Input._clipRectangleHeight
            End Get
        End Property

        Public Shared ReadOnly Property StartIndex As Input
            Get
                Return Input._startIndex
            End Get
        End Property

        Public Shared ReadOnly Property Text As Input
            Get
                Return Input._text
            End Get
        End Property

        Public Shared ReadOnly Property Fullscreen As Input
            Get
                Return Input._fullscreen
            End Get
        End Property

        Public Shared ReadOnly Property WebappKeys As Input
            Get
                Return Input._webappKeys
            End Get
        End Property

        Public Shared ReadOnly Property ZoomLevel As Input
            Get
                Return Input._zoomLevel
            End Get
        End Property

        Public Shared ReadOnly Property Overscan As Input
            Get
                Return Input._overscan
            End Get
        End Property

        Public Shared ReadOnly Property UserAgent As Input
            Get
                Return Input._userAgent
            End Get
        End Property

        Public Shared ReadOnly Property BackgroundColor As Input
            Get
                Return Input._backgroundColor
            End Get
        End Property

        Public Shared ReadOnly Property OnScreenDisplaySize As Input
            Get
                Return Input._onScreenDisplaySize
            End Get
        End Property

        Public Shared ReadOnly Property FolderUrl As Input
            Get
                Return Input._folderUrl
            End Get
        End Property

    End Class

End Namespace
Imports System.Drawing

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

    ''' <summary>
    ''' Provides access to factory methods for creating IP Control command instances.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CommandFactory
        Private Shared Property Instance As CommandFactory

        Shared Sub New()
            Instance = New CommandFactory
        End Sub

        Private Sub New()
        End Sub

        Public Shared Function GetInstance() As CommandFactory
            Return Instance
        End Function

#Region "Remote control"

        Public Function SendKey(key As Key) As IIPCommand
            Return New RemoteControlCommand(New RemoteControlKey(key))
        End Function

        Public Function SendKey(key As RemoteControlKey) As IIPCommand
            Return New RemoteControlCommand(key)
        End Function

#End Region

#Region "Player"

        Public Function GetStatus() As IIPCommand
            Return New GetStatusCommand()
        End Function

        Public Function SetPlayerState(state As PlayerState) As IIPCommand
            Return New SetPlayerStateCommand(state)
        End Function

        Public Function GetText() As IIPCommand
            Return New GetTextCommand
        End Function

        Public Function SetText(text As String) As IIPCommand
            Return New SetTextCommand(text)
        End Function

        Public Function Navigate(action As NavigationAction) As IIPCommand
            Return New NavigationCommand(action)
        End Function

        Public Function OpenWebBrowser() As IIPCommand
            Return New LaunchWebsiteCommand(New Uri("http://dune-hd.com/"))
        End Function

        Public Function OpenWebBrowser(website As Uri) As IIPCommand
            Return New LaunchWebsiteCommand(website)
        End Function

        Public Function OpenFlashLiteApp(mediaUrl As String) As IIPCommand
            Return New LaunchFlashLiteCommand(mediaUrl)
        End Function

        Public Function OpenFlashLiteApp(mediaUrl As String, flashvars As IDictionary(Of String, String)) As IIPCommand
            Return New LaunchFlashLiteCommand(mediaUrl, CType(flashvars, Networking.HttpQuery))
        End Function

#End Region

#Region "Playback"

        Public Function StartPlayback(url As String) As IIPCommand
            Return New StartPlaybackCommand(url) With {.Type = StartPlaybackCommand.PlaybackType.Auto}
        End Function

        Public Function StartFilePlayback(url As String) As IIPCommand
            Return New StartPlaybackCommand(url) With {.Type = StartPlaybackCommand.PlaybackType.File}
        End Function

        Public Function StartDvdPlayback(url As String) As IIPCommand
            Return New StartPlaybackCommand(url) With {.Type = StartPlaybackCommand.PlaybackType.Dvd}
        End Function

        Public Function StartBlurayPlayback(url As String) As IIPCommand
            Return New StartPlaybackCommand(url) With {.Type = StartPlaybackCommand.PlaybackType.Bluray}
        End Function

        Public Function StartPlaylistPlayback(url As String) As IIPCommand
            Return New StartPlaybackCommand(url) With {.Type = StartPlaybackCommand.PlaybackType.Playlist}
        End Function

        Public Function SetSpeed(speed As PlaybackSpeed) As IIPCommand
            Return New SetPlaybackStateCommand With {.PlaybackSpeed = speed}
        End Function

        Public Function SetPosition(position As TimeSpan) As IIPCommand
            Return New SetPlaybackStateCommand With {.PlaybackPosition = position}
        End Function

        Public Function SetPosition(direction As SkipFrames) As IIPCommand
            Return New SetKeyframeCommand(direction)
        End Function

        Public Function SetDisplayEnabled(enabled As Boolean) As IIPCommand
            Return New SetPlaybackStateCommand With {.DisplayEnabled = enabled}
        End Function

        Public Function SetOnScreenDisplayEnabled(enabled As Boolean) As IIPCommand
            Return New SetPlaybackStateCommand With {.OnScreenDisplayEnabled = enabled}
        End Function

        Public Function SetVolume(volume As Integer) As IIPCommand
            Return New SetPlaybackStateCommand With {.PlaybackVolume = volume}
        End Function

        Public Function SetAudioEnabled(enabled As Boolean) As IIPCommand
            Return New SetPlaybackStateCommand With {.AudioEnabled = enabled}
        End Function

        Public Function SetAudioTrack(track As Integer) As IIPCommand
            Return New SetPlaybackStateCommand With {.AudioTrack = track}
        End Function

        Public Function SetVideoEnabled(enabled As Boolean) As IIPCommand
            Return New SetPlaybackStateCommand With {.VideoEnabled = enabled}
        End Function

        Public Function SetVideoZoom(zoom As VideoZoom) As IIPCommand
            Return New SetPlaybackStateCommand With {.VideoZoom = zoom}
        End Function

        Public Function SetVideoOnTop(onTop As Boolean) As IIPCommand
            Return New SetPlaybackStateCommand With {.VideoOnTop = onTop}
        End Function

        <Obsolete("For protocol version 3 and up, use SetWindowZoom instead.")>
        Public Function SetVideoZoom(display As Size, fullscreen As Boolean) As IIPCommand
            Return New SetPlaybackVideoZoomCommand(display) With {.VideoFullscreen = fullscreen}
        End Function

        <Obsolete("For protocol version 3 and up, use SetWindowZoom instead.")>
        Public Function SetVideoZoom(display As Size, zoom As Rectangle) As IIPCommand
            Return New SetPlaybackWindowZoomCommand(display) With {
                .WindowRectanglePosition = zoom.Location,
                .WindowRectangleSize = zoom.Size
            }
        End Function

        Public Function SetWindowZoom(display As Size, fullscreen As Boolean) As IIPCommand
            Return New SetPlaybackWindowZoomCommand(display) With {.WindowFullscreen = fullscreen}
        End Function

        Public Function SetWindowZoom(display As Size, zoom As Rectangle) As IIPCommand
            Return New SetPlaybackWindowZoomCommand(display) With {
                .WindowRectanglePosition = zoom.Location,
                .WindowRectangleSize = zoom.Size
            }
        End Function

        Public Function SetWindowZoom(display As Size, position As Point, size As Size) As IIPCommand
            Return New SetPlaybackWindowZoomCommand(display) With {
                .WindowRectanglePosition = position,
                .WindowRectangleSize = size
            }
        End Function

        Public Function SetWindowZoom(display As Size, left As Integer, top As Integer, width As Integer, height As Integer) As IIPCommand
            Return New SetPlaybackWindowZoomCommand(display) With {
                .WindowRectanglePosition = New Point(left, top),
                .WindowRectangleSize = New Size(width, height)
            }
        End Function

        Public Function SetClipZoom(display As Size, zoom As Rectangle) As IIPCommand
            Return New SetPlaybackClipZoomCommand(display) With {
                .ClipRectanglePosition = zoom.Location,
                .ClipRectangleSize = zoom.Size
            }
        End Function

        Public Function SetClipZoom(display As Size, position As Point, size As Size) As IIPCommand
            Return New SetPlaybackClipZoomCommand(display) With {
                .ClipRectanglePosition = position,
                .ClipRectangleSize = size
            }
        End Function

        Public Function SetClipZoom(display As Size, left As Integer, top As Integer, width As Integer, height As Integer) As IIPCommand
            Return New SetPlaybackClipZoomCommand(display) With {
                .ClipRectanglePosition = New Point(left, top),
                .ClipRectangleSize = New Size(width, height)
            }
        End Function

        Public Function SetSubtitlesEnabled(enabled As Boolean) As IIPCommand
            Return New SetPlaybackStateCommand() With {.SubtitlesTrack = -CInt(Not enabled) << 8}
        End Function

        Public Function SetSubtitlesTrack(track As Integer) As IIPCommand
            Return New SetPlaybackStateCommand() With {.SubtitlesTrack = track}
        End Function

        Public Function SetActionOnFinish(action As ActionOnFinish) As IIPCommand
            Return New SetPlaybackStateCommand With {.ActionOnFinish = action}
        End Function

#End Region

#Region "Storage"

        Public Function GetFileSystemEntries() As IIPCommand
            Return New ListCommand
        End Function

        Public Function GetFileSystemEntries(path As String) As IIPCommand
            Return New ListCommand With {.Path = path}
        End Function

#End Region

    End Class

End Namespace

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

Namespace IPControl

    Public Interface IIPControlState

        ReadOnly Property ProtocolVersion As Version

        Property IsConnected As Boolean

        Sub Connect()
        Function ConnectAsync() As task

        Sub Disconnect()

        Property PlayerState As PlayerState
        Property PlaybackSpeed As PlaybackSpeed
        Property PlaybackPosition As TimeSpan?
        ReadOnly Property PlaybackDuration As TimeSpan?
        ReadOnly Property PlaybackTimeRemaining As TimeSpan?
        ReadOnly Property PlaybackIsBuffering As Boolean?
        ReadOnly Property PlaybackDvdMenu As Boolean?
        ReadOnly Property PlaybackBlurayMenu As Boolean?
        ReadOnly Property PlaybackState As PlaybackState
        ReadOnly Property PreviousPlaybackState As PlaybackState
        ReadOnly Property LastPlaybackEvent As PlaybackEvent
        Property PlaybackVolume As Integer?
        Property AudioEnabled As Boolean?
        Property AudioTrack As Integer?
        Property SubtitlesEnabled As Boolean?
        Property SubtitlesTrack As Integer?
        Property VideoEnabled As Boolean?
        Property DisplayEnabled As Boolean?
        Property OnScreenDisplayEnabled As Boolean?
        Property RepeatPlayback As Boolean?
        Property VideoZoom As VideoZoom
        Property VideoOnTop As Boolean?
        ReadOnly Property DisplaySize As Size?
        Property PlaybackWindowFullscreen As Boolean?
        Property PlaybackWindowRectangle As Rectangle?
        Property PlaybackClipRectangle As Rectangle?
        ReadOnly Property PlaybackVideoSize As Size?
        Property PlaybackUrl As String
        ReadOnly Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream)
        ReadOnly Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
        ReadOnly Property IsTextFieldActive As Boolean?
        Property Text As String

        Function GetStatus() As StatusCommandResult
        Function GetStatusAsync() As Task(Of StatusCommandResult)

        Function GetText() As TextCommandResult
        Function GetTextAsync() As Task(Of TextCommandResult)
        Function SetText(text As String) As TextCommandResult
        Function SetTextAsync(text As String) As Task(Of TextCommandResult)

        Function SetPlayerState(state As PlayerState) As StatusCommandResult
        Function SetPlayerStateAsync(state As PlayerState) As Task(Of StatusCommandResult)
        Function SetPlaybackSpeed(speed As PlaybackSpeed) As StatusCommandResult
        Function SetPlaybackSpeedAsync(speed As PlaybackSpeed) As Task(Of StatusCommandResult)
        Function SetPlaybackPosition(position As TimeSpan) As StatusCommandResult
        Function SetPlaybackPositionAsync(position As TimeSpan) As Task(Of StatusCommandResult)
        Function SetOnScreenDisplayEnabled(enabled As Boolean) As StatusCommandResult
        Function SetOnScreenDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Function SetDisplayEnabled(enabled As Boolean) As StatusCommandResult
        Function SetDisplayEnabledAsync(async As Boolean) As Task(Of StatusCommandResult)
        Function SetActionOnFinish(action As ActionOnFinish) As StatusCommandResult
        Function SetActionOnFinishAsync(action As ActionOnFinish) As Task(Of StatusCommandResult)
        Function SkipToKeyframe(direction As SkipFrames) As StatusCommandResult
        Function SkipToKeyframeAsync(direction As SkipFrames) As Task(Of StatusCommandResult)
        Function Navigate(action As NavigationAction) As StatusCommandResult
        Function NavigateAsync(action As NavigationAction) As Task(Of StatusCommandResult)
        Function SendInput(key As RemoteControlKey) As StatusCommandResult
        Function SendInputAsync(key As RemoteControlKey) As Task(Of StatusCommandResult)

        Function SetPlaybackVolume(volume As Integer) As StatusCommandResult
        Function SetPlaybackVolumeAsync(volume As Integer) As Task(Of StatusCommandResult)
        Function SetAudioEnabled(enabled As Boolean) As StatusCommandResult
        Function SetAudioEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Function SetAudioTrack(index As Integer) As StatusCommandResult
        Function SetAudioTrackAsync(index As Integer) As Task(Of StatusCommandResult)
        Function SetVideoEnabled(enabled As Boolean) As StatusCommandResult
        Function SetVideoEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Function SetVideoZoom(zoom As VideoZoom) As StatusCommandResult
        Function SetVideoZoomAsync(zoom As VideoZoom) As Task(Of StatusCommandResult)
        Function OpenWebBrowser(location As Uri) As StatusCommandResult
        Function OpenWebBrowserAsync(location As Uri) As Task(Of StatusCommandResult)
        Function SetSubtitlesTrack(index As Integer) As StatusCommandResult
        Function SetSubtitlesTrackAsync(index As Integer) As Task(Of StatusCommandResult)
        Function SetSubtitlesEnabled(enabled As Boolean) As StatusCommandResult
        Function SetSubtitlesEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
        Function SetVideoOnTop(onTop As Boolean) As StatusCommandResult
        Function SetVideoOnTopAsync(onTop As Boolean) As Task(Of StatusCommandResult)

        Function HasRemoteControlPlugin() As Boolean
        Function HasRemoteControlPluginAsync() As Task(Of Boolean)

        Function GetFileSystemEntries() As FileSystemCommandResult
        Function GetFileSystemEntries(path As String) As FileSystemCommandResult
        Function GetFileSystemEntriesAsync() As Task(Of FileSystemCommandResult)
        Function GetFileSystemEntriesAsync(path As String) As Task(Of FileSystemCommandResult)


    End Interface

End Namespace


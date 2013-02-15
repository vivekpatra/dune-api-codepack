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

    Public Class IPControlStateDisconnected : Inherits IPControlState

        Private Shared Version As Version
        Private Shared ExceptionMessage As String = "No connection to host"

        Shared Sub New()
            IPControlStateDisconnected.Version = New Version(0, 0)
        End Sub

        Public Sub New(target As Dune)
            MyBase.New(target)
        End Sub

        Public Overrides Property AudioEnabled As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property AudioTrack As Integer?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Integer?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Sub Connect()
            Dim status = Me.GetStatus

            Select Case status.ProtocolVersion.Major
                Case 1
                    Me.Target.State = New IPControlStateVersion1(Me.Target, status)
                Case 2
                    Me.Target.State = New IPControlStateVersion2(Me.Target, status)
                Case 3
                    Me.Target.State = New IPControlStateVersion3(Me.Target, status)
                Case Else
                    Me.Target.State = New IPControlStateUnknown(Me.Target, status)
            End Select
        End Sub

        Public Overrides Async Function ConnectAsync() As Task
            Dim status = Await Me.GetStatusAsync

            Select Case status.ProtocolVersion.Major
                Case 1
                    Me.Target.State = New IPControlStateVersion1(Me.Target, status)
                Case 2
                    Me.Target.State = New IPControlStateVersion2(Me.Target, status)
                Case 3
                    Me.Target.State = New IPControlStateVersion3(Me.Target, status)
                Case Else
                    Me.Target.State = New IPControlStateUnknown(Me.Target, status)
            End Select
        End Function

        Protected Overrides Sub UpdateStatus(sender As Object, e As Timers.ElapsedEventArgs)
            Console.WriteLine("Disconnected")
        End Sub

        Public Overrides Sub Disconnect()
            Throw New InvalidOperationException("Already disconnected")
        End Sub

        Public Overrides Property DisplayEnabled As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property DisplaySize As Size?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property OnScreenDisplayEnabled As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property PlaybackClipRectangle As Rectangle?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Rectangle?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackDuration As TimeSpan?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides ReadOnly Property PlaybackIsBuffering As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property PlaybackPosition As TimeSpan?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As TimeSpan?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property PlaybackSpeed As PlaybackSpeed
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As PlaybackSpeed)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackTimeRemaining As TimeSpan?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property PlaybackUrl As String
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As String)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackVideoSize As Size?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property PlaybackVolume As Integer?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Integer?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property PlaybackWindowFullscreen As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property PlaybackWindowRectangle As Rectangle?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Rectangle?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property PlayerState As PlayerState
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As PlayerState)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property ProtocolVersion As Version
            Get
                Return IPControlStateDisconnected.Version
            End Get
        End Property

        Public Overrides Property SubtitlesEnabled As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property SubtitlesTrack As Integer?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Integer?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property Text As String
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As String)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property IsTextFieldActive As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property VideoEnabled As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property VideoOnTop As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackBlurayMenu As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides ReadOnly Property PlaybackDvdMenu As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Property RepeatPlayback As Boolean?
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As Boolean?)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides Property VideoZoom As VideoZoom
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
            Set(value As VideoZoom)
                Throw New InvalidOperationException(ExceptionMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property LastPlaybackEvent As PlaybackEvent
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides ReadOnly Property PlaybackState As PlaybackState
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides ReadOnly Property PreviousPlaybackState As PlaybackState
            Get
                Throw New InvalidOperationException(ExceptionMessage)
            End Get
        End Property

        Public Overrides Function Navigate(action As NavigationAction) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function NavigateAsync(action As NavigationAction) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function OpenWebBrowser(location As Uri) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function OpenWebBrowserAsync(location As Uri) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SendKey(key As RemoteControlKey) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SendKeyAsync(key As RemoteControlKey) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetActionOnFinish(action As ActionOnFinish) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetActionOnFinishAsync(action As ActionOnFinish) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetAudioEnabled(enabled As Boolean) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetAudioEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetAudioTrack(index As Integer) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetAudioTrackAsync(index As Integer) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetDisplayEnabled(enabled As Boolean) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetDisplayEnabledAsync(async As Boolean) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetOnScreenDisplayEnabled(enabled As Boolean) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetOnScreenDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlaybackPosition(position As TimeSpan) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlaybackPositionAsync(position As TimeSpan) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlaybackSpeed(speed As PlaybackSpeed) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlaybackSpeedAsync(speed As PlaybackSpeed) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlaybackVolume(volume As Integer) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlaybackVolumeAsync(volume As Integer) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlayerState(state As PlayerState) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetPlayerStateAsync(state As PlayerState) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetSubtitlesEnabled(enabled As Boolean) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetSubtitlesEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetSubtitlesTrack(index As Integer) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetSubtitlesTrackAsync(index As Integer) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetVideoEnabled(enabled As Boolean) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetVideoEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetVideoOnTop(onTop As Boolean) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetVideoOnTopAsync(onTop As Boolean) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetVideoZoom(zoom As VideoZoom) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetVideoZoomAsync(zoom As VideoZoom) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SkipToKeyframe(direction As SkipFrames) As StatusCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SkipToKeyframeAsync(direction As SkipFrames) As Task(Of StatusCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function GetText() As TextCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function GetTextAsync() As Task(Of TextCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetText(text As String) As TextCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function SetTextAsync(text As String) As Task(Of TextCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function HasRemoteControlPlugin() As Boolean
            Return False
        End Function

        Public Overrides Function HasRemoteControlPluginAsync() As Task(Of Boolean)
            Return Task.FromResult(Of Boolean)(False)
        End Function

        Public Overrides Function GetFileSystemEntries() As FileSystemCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function GetFileSystemEntries(path As String) As FileSystemCommandResult
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function GetFileSystemEntriesAsync() As Task(Of FileSystemCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

        Public Overrides Function GetFileSystemEntriesAsync(path As String) As Task(Of FileSystemCommandResult)
            Throw New InvalidOperationException(ExceptionMessage)
        End Function

    End Class

End Namespace


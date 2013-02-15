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
Imports System.Net

Namespace IPControl

    Public Class IPControlStateVersion1 : Inherits IPControlState

        Private NotSupportedMessage As String = String.Format("Action unavailable in protocol version {0}", Me.ProtocolVersion.Major)
        Private Shared Version As Version
        Private _displayEnabled As Boolean?
        Private _onScreenDisplayEnabled As Boolean?
        Private _repeatPlayback As Boolean?
        Private _playbackUrl As String
        Protected connectionLock As Object
        Private _hasRemoteControlPlugin As Boolean?

        Shared Sub New()
            IPControlStateVersion1.Version = New Version(1, 0)
        End Sub

        Public Sub New(target As Dune, status As StatusCommandResult)
            MyBase.New(target)
            Me.StatusSource = status
            Me.connectionLock = New Object
        End Sub

        Protected Sub ClearAssumptions()
            If Me.ProtocolVersion.Major < 3 Then
                _playbackUrl = Nothing
            End If
            _displayEnabled = Nothing
            _onScreenDisplayEnabled = Nothing
            _repeatPlayback = Nothing
        End Sub

        Public Overrides Property IsConnected As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
                MyBase.IsConnected = value
            End Set
        End Property

        Public Overrides Sub Connect()
            Throw New InvalidOperationException("Already connected")
        End Sub

        Public Overrides Function ConnectAsync() As Task
            Throw New InvalidOperationException("Already connected")
        End Function

        Public Overrides Sub Disconnect()
            SyncLock connectionLock
                Me.Target.State = New IPControlStateDisconnected(Me.Target)
            End SyncLock
        End Sub

        Protected Overrides Async Sub UpdateStatus(sender As Object, e As Timers.ElapsedEventArgs)
            Try
                Dim status = Await Me.GetStatusAsync
                Dim previousStatus = Me.StatusSource

                SyncLock connectionLock
                    If StatusUpdater Is Nothing Then
                        Exit Sub
                    End If
                    Me.StatusSource = status
                    Me.Target.SignalPlayerStateChanged(status, previousStatus)
                    If (status.PlaybackState <> previousStatus.PlaybackState) Or (status.LastPlaybackEvent <> previousStatus.LastPlaybackEvent) Then
                        Me.Target.SignalPlaybackStateChanged(status.PlaybackState, status.PreviousPlaybackState, status.LastPlaybackEvent)
                    End If
                    StatusUpdater.Start()
                End SyncLock
            Catch ex As Http.HttpRequestException
                Disconnect()
            End Try
        End Sub

        Public Overrides ReadOnly Property ProtocolVersion As Version
            Get
                Return IPControlStateVersion1.Version
            End Get
        End Property

        Public Overrides Property AudioEnabled As Boolean?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Boolean?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides Property AudioTrack As Integer?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Integer?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides Property DisplayEnabled As Boolean?
            Get
                If Me.StatusSource.PlayerState.IsPlaybackState Then
                    Return _displayEnabled
                Else
                    Return Me.StatusSource.PlayerState <> IPControl.PlayerState.Standby AndAlso Me.StatusSource.PlayerState <> IPControl.PlayerState.BlackScreen
                End If
            End Get
            Set(value As Boolean?)
                If Me.StatusSource.PlayerState.IsPlaybackState Then
                    If Me.Execute(Command.Factory.SetDisplayEnabled(value.GetValueOrDefault(True))).IsSuccessStatusCode Then
                        _displayEnabled = value.GetValueOrDefault(True)
                    End If
                Else
                    If value.GetValueOrDefault(True) Then
                        Me.Execute(Command.Factory.SetPlayerState(IPControl.PlayerState.Navigator))
                    Else
                        Me.Execute(Command.Factory.SetPlayerState(IPControl.PlayerState.BlackScreen))
                    End If
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property DisplaySize As Size?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides Property OnScreenDisplayEnabled As Boolean?
            Get
                Return _onScreenDisplayEnabled
            End Get
            Set(value As Boolean?)
                If Me.Execute(Command.Factory.SetOnScreenDisplayEnabled(value.GetValueOrDefault(True))).IsSuccessStatusCode Then
                    _onScreenDisplayEnabled = value.GetValueOrDefault(True)
                End If
            End Set
        End Property

        Public Overrides Property PlaybackClipRectangle As Rectangle?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Rectangle?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackDuration As TimeSpan?
            Get
                Return Me.StatusSource.PlaybackDuration
            End Get
        End Property

        Public Overrides ReadOnly Property PlaybackIsBuffering As Boolean?
            Get
                Return Me.StatusSource.PlaybackIsBuffering
            End Get
        End Property

        Public Overrides Property PlaybackPosition As TimeSpan?
            Get
                Return Me.StatusSource.PlaybackPosition
            End Get
            Set(value As TimeSpan?)
                Me.Execute(Command.Factory.SetPosition(value.GetValueOrDefault))
            End Set
        End Property

        Public Overrides Property PlaybackSpeed As PlaybackSpeed
            Get
                Return Me.StatusSource.PlaybackSpeed
            End Get
            Set(value As PlaybackSpeed)
                If value Is Nothing Then value = IPControl.PlaybackSpeed.Play256
                Me.Execute(Command.Factory.SetSpeed(value))
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackTimeRemaining As TimeSpan?
            Get
                Return Me.StatusSource.PlaybackTimeRemaining
            End Get
        End Property

        Public Overrides Property PlaybackUrl As String
            Get
                Return _playbackUrl
            End Get
            Set(value As String)
                If String.Equals(value, String.Empty, StringComparison.InvariantCultureIgnoreCase) Then
                    If Me.Execute(Command.Factory.SetPlayerState(IPControl.PlayerState.Navigator)).IsSuccessStatusCode Then
                        _playbackUrl = Nothing
                    End If
                Else
                    If Me.Execute(Command.Factory.StartFilePlayback(value)).IsSuccessStatusCode Then
                        _playbackUrl = value
                    End If
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackVideoSize As Size?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides Property PlaybackVolume As Integer?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Integer?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides Property PlaybackWindowFullscreen As Boolean?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Boolean?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides Property PlaybackWindowRectangle As Rectangle?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Rectangle?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides Property PlayerState As PlayerState
            Get
                Return Me.StatusSource.PlayerState
            End Get
            Set(value As PlayerState)
                Me.Execute(Command.Factory.SetPlayerState(value))
            End Set
        End Property

        Public Overrides Property SubtitlesEnabled As Boolean?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Boolean?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides Property SubtitlesTrack As Integer?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Integer?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides Property Text As String
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As String)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property IsTextFieldActive As Boolean?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides Property VideoEnabled As Boolean?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Boolean?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides Property VideoOnTop As Boolean?
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As Boolean?)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackBlurayMenu As Boolean?
            Get
                Return Me.StatusSource.PlaybackBlurayMenu
            End Get
        End Property

        Public Overrides ReadOnly Property PlaybackDvdMenu As Boolean?
            Get
                Return Me.StatusSource.PlaybackDvdMenu
            End Get
        End Property

        Public Overrides Property RepeatPlayback As Boolean?
            Get
                If Me.PlayerState.IsPlaybackState Then
                    Return _repeatPlayback
                End If
            End Get
            Set(value As Boolean?)
                Dim action As ActionOnFinish
                If value.GetValueOrDefault(False) Then
                    action = ActionOnFinish.RestartPlayback
                Else
                    action = ActionOnFinish.Exit
                End If
                If Me.Execute(Command.Factory.SetActionOnFinish(action)).IsSuccessStatusCode Then
                    _repeatPlayback = value.GetValueOrDefault(False)
                End If
            End Set
        End Property

        Public Overrides Property VideoZoom As VideoZoom
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
            Set(value As VideoZoom)
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Set
        End Property

        Public Overrides ReadOnly Property LastPlaybackEvent As PlaybackEvent
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides ReadOnly Property PlaybackState As PlaybackState
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides ReadOnly Property PreviousPlaybackState As PlaybackState
            Get
                Throw New NotSupportedException(Me.NotSupportedMessage)
            End Get
        End Property

        Public Overrides Function Navigate(action As NavigationAction) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.Navigate(action)), StatusCommandResult)
        End Function

        Public Overrides Async Function NavigateAsync(action As NavigationAction) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.Navigate(action)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function OpenWebBrowser(location As Uri) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function OpenWebBrowserAsync(location As Uri) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SendKey(key As RemoteControlKey) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SendKey(key)), StatusCommandResult)
        End Function

        Public Overrides Async Function SendKeyAsync(key As RemoteControlKey) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SendKey(key)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetActionOnFinish(action As ActionOnFinish) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetActionOnFinish(action)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetActionOnFinishAsync(action As ActionOnFinish) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetActionOnFinish(action)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetAudioEnabled(enabled As Boolean) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetAudioEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetAudioTrack(index As Integer) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetAudioTrackAsync(index As Integer) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetDisplayEnabled(enabled As Boolean) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetDisplayEnabled(enabled)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetDisplayEnabled(enabled)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetOnScreenDisplayEnabled(enabled As Boolean) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetOnScreenDisplayEnabled(enabled)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetOnScreenDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetOnScreenDisplayEnabled(enabled)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetPlaybackPosition(position As TimeSpan) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetPosition(position)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetPlaybackPositionAsync(position As TimeSpan) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetPosition(position)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetPlaybackSpeed(speed As PlaybackSpeed) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetSpeed(speed)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetPlaybackSpeedAsync(speed As PlaybackSpeed) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetSpeed(speed)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetPlaybackVolume(volume As Integer) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetPlaybackVolumeAsync(volume As Integer) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetPlayerState(state As PlayerState) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetPlayerState(state)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetPlayerStateAsync(state As PlayerState) As Task(Of StatusCommandResult)
            Return CType(Await Me.ExecuteAsync(Command.Factory.SetPlayerState(state)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetSubtitlesEnabled(enabled As Boolean) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetSubtitlesEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetSubtitlesTrack(index As Integer) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetSubtitlesTrackAsync(index As Integer) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetText(text As String) As TextCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetTextAsync(text As String) As Task(Of TextCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetVideoEnabled(enabled As Boolean) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetVideoEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetVideoOnTop(onTop As Boolean) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetVideoOnTopAsync(onTop As Boolean) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetVideoZoom(zoom As VideoZoom) As StatusCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SetVideoZoomAsync(zoom As VideoZoom) As Task(Of StatusCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function SkipToKeyframe(direction As SkipFrames) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetPosition(direction)), StatusCommandResult)
        End Function

        Public Overrides Async Function SkipToKeyframeAsync(direction As SkipFrames) As Task(Of StatusCommandResult)
            Return CType(Await Me.ExecuteAsync(Command.Factory.SetPosition(direction)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function GetText() As TextCommandResult
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function GetTextAsync() As Task(Of TextCommandResult)
            Throw New NotSupportedException(Me.NotSupportedMessage)
        End Function

        Public Overrides Function HasRemoteControlPlugin() As Boolean
            Try
                Return HasRemoteControlPluginAsync.Result
            Catch ex As AggregateException
                Throw ex.InnerException
            End Try
        End Function

        Public Overrides Async Function HasRemoteControlPluginAsync() As Task(Of Boolean)
            If Not _hasRemoteControlPlugin.HasValue Then
                Dim location = Me.Target.GetBaseAddress("remote-control", "do").Uri
                _hasRemoteControlPlugin = Await IPControlClient.GetInstance().ServiceReturnsXmlAsync(location).ConfigureAwait(False)
            End If
            Return _hasRemoteControlPlugin.Value
        End Function

        Public Overrides Function GetFileSystemEntries() As FileSystemCommandResult
            Return DirectCast(Me.Execute(Command.Factory.GetFileSystemEntries()), FileSystemCommandResult)
        End Function

        Public Overrides Function GetFileSystemEntries(path As String) As FileSystemCommandResult
            Return DirectCast(Me.Execute(Command.Factory.GetFileSystemEntries(path)), FileSystemCommandResult)
        End Function

        Public Overrides Async Function GetFileSystemEntriesAsync() As Task(Of FileSystemCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.GetFileSystemEntries()).ConfigureAwait(False), FileSystemCommandResult)
        End Function

        Public Overrides Async Function GetFileSystemEntriesAsync(path As String) As Task(Of FileSystemCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.GetFileSystemEntries(path)).ConfigureAwait(False), FileSystemCommandResult)
        End Function

    End Class

End Namespace


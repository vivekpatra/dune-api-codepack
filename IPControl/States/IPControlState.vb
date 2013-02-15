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
Imports System.Timers

Namespace IPControl

    Public MustInherit Class IPControlState : Implements IIPControlState : Implements IDisposable

        Public Sub New(target As Dune)
            Me.Target = target
            Me.StatusUpdater = New Timer(1000) With {.AutoReset = False, .Enabled = True}
            AddHandler StatusUpdater.Elapsed, AddressOf UpdateStatus
        End Sub

        Protected Property Target As Dune
        Protected Property StatusSource As StatusCommandResult
        Protected Property TextSource As TextCommandResult

        Public MustOverride Function HasRemoteControlPlugin() As Boolean Implements IIPControlState.HasRemoteControlPlugin
        Public MustOverride Function HasRemoteControlPluginAsync() As Task(Of Boolean) Implements IIPControlState.HasRemoteControlPluginAsync

        Public Property Interval As Double
            Get
                Return Me.StatusUpdater.Interval
            End Get
            Set(value As Double)
                Me.StatusUpdater.Interval = value
            End Set
        End Property

        Protected Function Execute(command As IIPCommand) As IIPCommandResult
            Return command.Execute(Me.Target)
        End Function

        Protected Function ExecuteAsync(command As IIPCommand) As Task(Of IIPCommandResult)
            Return command.ExecuteAsync(Me.Target)
        End Function

        Protected Function TryExecute(command As IIPCommand) As IIPCommandResult
            Return command.TryExecute(Me.Target)
        End Function

        Protected Function TryExecuteAsync(command As IIPCommand) As Task(Of IIPCommandResult)
            Return command.TryExecuteAsync(Me.Target)
        End Function

        Protected Property StatusUpdater As Timer
        Protected MustOverride Sub UpdateStatus(sender As Object, e As System.Timers.ElapsedEventArgs)

        Public Overridable Property IsConnected As Boolean Implements IIPControlState.IsConnected
            Get
                Return False
            End Get
            Set(value As Boolean)
                If value Then
                    Me.Connect()
                Else
                    Me.Disconnect()
                End If
            End Set
        End Property

        Public MustOverride Sub Connect() Implements IIPControlState.Connect

        Public MustOverride Function ConnectAsync() As Task Implements IIPControlState.ConnectAsync

        Public MustOverride Sub Disconnect() Implements IIPControlState.Disconnect

#Region "IP Control properties"

        Public MustOverride Property AudioEnabled As Boolean? Implements IIPControlState.AudioEnabled

        Public MustOverride Property AudioTrack As Integer? Implements IIPControlState.AudioTrack

        Public MustOverride ReadOnly Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream) Implements IIPControlState.AudioTracks

        Public MustOverride Property DisplayEnabled As Boolean? Implements IIPControlState.DisplayEnabled

        Public MustOverride ReadOnly Property DisplaySize As Size? Implements IIPControlState.DisplaySize

        Public MustOverride Property OnScreenDisplayEnabled As Boolean? Implements IIPControlState.OnScreenDisplayEnabled

        Public MustOverride Property PlaybackClipRectangle As Rectangle? Implements IIPControlState.PlaybackClipRectangle

        Public MustOverride ReadOnly Property PlaybackDuration As TimeSpan? Implements IIPControlState.PlaybackDuration

        Public MustOverride ReadOnly Property PlaybackIsBuffering As Boolean? Implements IIPControlState.PlaybackIsBuffering

        Public MustOverride Property PlaybackPosition As TimeSpan? Implements IIPControlState.PlaybackPosition

        Public MustOverride Property PlaybackSpeed As PlaybackSpeed Implements IIPControlState.PlaybackSpeed

        Public MustOverride ReadOnly Property PlaybackTimeRemaining As TimeSpan? Implements IIPControlState.PlaybackTimeRemaining

        Public MustOverride Property PlaybackUrl As String Implements IIPControlState.PlaybackUrl

        Public MustOverride ReadOnly Property PlaybackVideoSize As Size? Implements IIPControlState.PlaybackVideoSize

        Public MustOverride Property PlaybackVolume As Integer? Implements IIPControlState.PlaybackVolume

        Public MustOverride Property PlaybackWindowFullscreen As Boolean? Implements IIPControlState.PlaybackWindowFullscreen

        Public MustOverride Property PlaybackWindowRectangle As Rectangle? Implements IIPControlState.PlaybackWindowRectangle

        Public MustOverride ReadOnly Property PlaybackBlurayMenu As Boolean? Implements IIPControlState.PlaybackBlurayMenu

        Public MustOverride ReadOnly Property PlaybackDvdMenu As Boolean? Implements IIPControlState.PlaybackDvdMenu

        Public MustOverride Property PlayerState As PlayerState Implements IIPControlState.PlayerState

        Public MustOverride ReadOnly Property ProtocolVersion As Version Implements IIPControlState.ProtocolVersion

        Public MustOverride Property SubtitlesEnabled As Boolean? Implements IIPControlState.SubtitlesEnabled

        Public MustOverride Property SubtitlesTrack As Integer? Implements IIPControlState.SubtitlesTrack

        Public MustOverride ReadOnly Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream) Implements IIPControlState.SubtitlesTracks

        Public MustOverride Property Text As String Implements IIPControlState.Text

        Public MustOverride ReadOnly Property IsTextFieldActive As Boolean? Implements IIPControlState.IsTextFieldActive

        Public MustOverride Property VideoEnabled As Boolean? Implements IIPControlState.VideoEnabled

        Public MustOverride Property VideoOnTop As Boolean? Implements IIPControlState.VideoOnTop

        Public MustOverride Property VideoZoom As VideoZoom Implements IIPControlState.VideoZoom

        Public MustOverride Property RepeatPlayback As Boolean? Implements IIPControlState.RepeatPlayback

        Public MustOverride ReadOnly Property LastPlaybackEvent As PlaybackEvent Implements IIPControlState.LastPlaybackEvent

        Public MustOverride ReadOnly Property PlaybackState As PlaybackState Implements IIPControlState.PlaybackState

        Public MustOverride ReadOnly Property PreviousPlaybackState As PlaybackState Implements IIPControlState.PreviousPlaybackState

#End Region

#Region "IP Control commands"

        Public Function GetStatus() As StatusCommandResult Implements IIPControlState.GetStatus
            Return DirectCast(Me.Execute(Command.Factory.GetStatus), StatusCommandResult)
        End Function

        Public Async Function GetStatusAsync() As Task(Of StatusCommandResult) Implements IIPControlState.GetStatusAsync
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.GetStatus).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public MustOverride Function Navigate(action As NavigationAction) As StatusCommandResult Implements IIPControlState.Navigate

        Public MustOverride Function NavigateAsync(action As NavigationAction) As Task(Of StatusCommandResult) Implements IIPControlState.NavigateAsync

        Public MustOverride Function OpenWebBrowser(location As Uri) As StatusCommandResult Implements IIPControlState.OpenWebBrowser

        Public MustOverride Function OpenWebBrowserAsync(location As Uri) As Task(Of StatusCommandResult) Implements IIPControlState.OpenWebBrowserAsync

        Public MustOverride Function SendKey(key As RemoteControlKey) As StatusCommandResult Implements IIPControlState.SendInput

        Public MustOverride Function SendKeyAsync(key As RemoteControlKey) As Task(Of StatusCommandResult) Implements IIPControlState.SendInputAsync

        Public MustOverride Function SetActionOnFinish(action As ActionOnFinish) As StatusCommandResult Implements IIPControlState.SetActionOnFinish

        Public MustOverride Function SetActionOnFinishAsync(action As ActionOnFinish) As Task(Of StatusCommandResult) Implements IIPControlState.SetActionOnFinishAsync

        Public MustOverride Function SetAudioEnabled(enabled As Boolean) As StatusCommandResult Implements IIPControlState.SetAudioEnabled

        Public MustOverride Function SetAudioEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult) Implements IIPControlState.SetAudioEnabledAsync

        Public MustOverride Function SetAudioTrack(index As Integer) As StatusCommandResult Implements IIPControlState.SetAudioTrack

        Public MustOverride Function SetAudioTrackAsync(index As Integer) As Task(Of StatusCommandResult) Implements IIPControlState.SetAudioTrackAsync

        Public MustOverride Function SetDisplayEnabled(enabled As Boolean) As StatusCommandResult Implements IIPControlState.SetDisplayEnabled

        Public MustOverride Function SetDisplayEnabledAsync(async As Boolean) As Task(Of StatusCommandResult) Implements IIPControlState.SetDisplayEnabledAsync

        Public MustOverride Function SetOnScreenDisplayEnabled(enabled As Boolean) As StatusCommandResult Implements IIPControlState.SetOnScreenDisplayEnabled

        Public MustOverride Function SetOnScreenDisplayEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult) Implements IIPControlState.SetOnScreenDisplayEnabledAsync

        Public MustOverride Function SetPlaybackPosition(position As TimeSpan) As StatusCommandResult Implements IIPControlState.SetPlaybackPosition

        Public MustOverride Function SetPlaybackPositionAsync(position As TimeSpan) As Task(Of StatusCommandResult) Implements IIPControlState.SetPlaybackPositionAsync

        Public MustOverride Function SetPlaybackSpeed(speed As PlaybackSpeed) As StatusCommandResult Implements IIPControlState.SetPlaybackSpeed

        Public MustOverride Function SetPlaybackSpeedAsync(speed As PlaybackSpeed) As Task(Of StatusCommandResult) Implements IIPControlState.SetPlaybackSpeedAsync

        Public MustOverride Function SetPlaybackVolume(volume As Integer) As StatusCommandResult Implements IIPControlState.SetPlaybackVolume

        Public MustOverride Function SetPlaybackVolumeAsync(volume As Integer) As Task(Of StatusCommandResult) Implements IIPControlState.SetPlaybackVolumeAsync

        Public MustOverride Function SetPlayerState(state As PlayerState) As StatusCommandResult Implements IIPControlState.SetPlayerState

        Public MustOverride Function SetPlayerStateAsync(state As PlayerState) As Task(Of StatusCommandResult) Implements IIPControlState.SetPlayerStateAsync

        Public MustOverride Function SetSubtitlesEnabled(enabled As Boolean) As StatusCommandResult Implements IIPControlState.SetSubtitlesEnabled

        Public MustOverride Function SetSubtitlesEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult) Implements IIPControlState.SetSubtitlesEnabledAsync

        Public MustOverride Function SetSubtitlesTrack(index As Integer) As StatusCommandResult Implements IIPControlState.SetSubtitlesTrack

        Public MustOverride Function SetSubtitlesTrackAsync(index As Integer) As Task(Of StatusCommandResult) Implements IIPControlState.SetSubtitlesTrackAsync

        Public MustOverride Function SetText(text As String) As TextCommandResult Implements IIPControlState.SetText

        Public MustOverride Function SetTextAsync(text As String) As Task(Of TextCommandResult) Implements IIPControlState.SetTextAsync

        Public MustOverride Function SetVideoEnabled(enabled As Boolean) As StatusCommandResult Implements IIPControlState.SetVideoEnabled

        Public MustOverride Function SetVideoEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult) Implements IIPControlState.SetVideoEnabledAsync

        Public MustOverride Function SetVideoOnTop(onTop As Boolean) As StatusCommandResult Implements IIPControlState.SetVideoOnTop

        Public MustOverride Function SetVideoOnTopAsync(onTop As Boolean) As Task(Of StatusCommandResult) Implements IIPControlState.SetVideoOnTopAsync

        Public MustOverride Function SetVideoZoom(zoom As VideoZoom) As StatusCommandResult Implements IIPControlState.SetVideoZoom

        Public MustOverride Function SetVideoZoomAsync(zoom As VideoZoom) As Task(Of StatusCommandResult) Implements IIPControlState.SetVideoZoomAsync

        Public MustOverride Function SkipToKeyframe(direction As SkipFrames) As StatusCommandResult Implements IIPControlState.SkipToKeyframe

        Public MustOverride Function SkipToKeyframeAsync(direction As SkipFrames) As Task(Of StatusCommandResult) Implements IIPControlState.SkipToKeyframeAsync

        Public MustOverride Function GetText() As TextCommandResult Implements IIPControlState.GetText

        Public MustOverride Function GetTextAsync() As Task(Of TextCommandResult) Implements IIPControlState.GetTextAsync

#End Region

#Region "remote-control commands"

        Public MustOverride Function GetFileSystemEntries() As FileSystemCommandResult Implements IIPControlState.GetFileSystemEntries
        Public MustOverride Function GetFileSystemEntries(path As String) As FileSystemCommandResult Implements IIPControlState.GetFileSystemEntries

        Public MustOverride Function GetFileSystemEntriesAsync() As Task(Of FileSystemCommandResult) Implements IIPControlState.GetFileSystemEntriesAsync
        Public MustOverride Function GetFileSystemEntriesAsync(path As String) As Task(Of FileSystemCommandResult) Implements IIPControlState.GetFileSystemEntriesAsync

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                Console.WriteLine("disposing current state")
                If disposing Then
                    ' dispose status update timer
                    If Not Me.StatusUpdater Is Nothing Then
                        Me.StatusUpdater.Dispose()
                        Me.StatusUpdater = Nothing
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace


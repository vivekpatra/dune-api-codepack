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

    Public Class IPControlStateVersion3 : Inherits IPControlStateVersion2

        Private NotSupportedMessage As String = "Action unavailable in protocol version 3"
        Private Shared Version As Version

        Shared Sub New()
            IPControlStateVersion3.Version = New Version(3, 0)
        End Sub

        Public Sub New(target As Dune, status As StatusCommandResult)
            MyBase.New(target, status)
            AddHandler target.PlaybackStateChanged, AddressOf HandlePlaybackEvent
        End Sub

        Protected Sub HandlePlaybackEvent(sender As Object, e As PlaybackStateChangedEventArgs)
            If e.PlaybackState = PlaybackState.Stopped Then
                Me.ClearAssumptions()
            End If
        End Sub

        Public Overrides ReadOnly Property ProtocolVersion As Version
            Get
                Return IPControlStateVersion3.Version
            End Get
        End Property

        Protected Overrides Async Sub UpdateStatus(sender As Object, e As Timers.ElapsedEventArgs)
            Try
                Dim tasks As New Queue(Of Task(Of IIPCommandResult))

                Dim status = Command.Factory.GetStatus
                Dim text = Command.Factory.GetText
                Dim previousStatus = Me.StatusSource
                Dim previousText = Me.TextSource

                tasks.Enqueue(status.TryExecuteAsync(Me.Target))
                If text.CanExecute(Me.Target) Then
                    tasks.Enqueue(text.TryExecuteAsync(Me.Target))
                End If
                Await Task.WhenAll(tasks.ToArray).ConfigureAwait(False)

                SyncLock connectionLock
                    If StatusUpdater Is Nothing Then
                        Console.WriteLine("disconnecting")
                        Exit Sub
                    End If
                    Me.StatusSource = DirectCast(tasks.Dequeue.Result, StatusCommandResult)
                    If tasks.Count > 0 Then
                        Me.TextSource = DirectCast(tasks.Dequeue.Result, TextCommandResult)
                    Else
                        Me.TextSource = Nothing
                    End If

                    Me.Target.SignalPlayerStateChanged(Me.StatusSource, previousStatus)

                    If Me.StatusSource.PlaybackState IsNot Nothing Then
                        If (Me.StatusSource.PlaybackState <> previousStatus.PlaybackState) Or (Me.StatusSource.LastPlaybackEvent <> previousStatus.LastPlaybackEvent) Then
                            Me.Target.SignalPlaybackStateChanged(Me.StatusSource.PlaybackState, Me.StatusSource.PreviousPlaybackState, Me.StatusSource.LastPlaybackEvent)
                        End If
                    End If

                    StatusUpdater.Start()
                End SyncLock
            Catch ex As TaskCanceledException
                StatusUpdater.Start()
            Catch ex As Http.HttpRequestException
                Disconnect()
            End Try
        End Sub

        Public Overrides Property PlaybackUrl As String
            Get
                Return Me.StatusSource.PlaybackUrl
            End Get
            Set(value As String)
                If String.Equals(value, String.Empty, StringComparison.InvariantCultureIgnoreCase) Then
                    Me.Execute(Command.Factory.SetPlayerState(IPControl.PlayerState.Navigator))
                Else
                    Me.Execute(Command.Factory.StartPlayback(value))
                End If
            End Set
        End Property

        Public Overrides Property PlaybackClipRectangle As Rectangle?
            Get
                If Me.DisplaySize.HasValue Then
                    Return Me.StatusSource.PlaybackClipRectangle.GetValueOrDefault(New Rectangle(Nothing, Me.DisplaySize.Value))
                End If
            End Get
            Set(value As Rectangle?)
                Dim fullscreen As New Rectangle(Nothing, Me.DisplaySize.GetValueOrDefault)
                Me.Execute(Command.Factory.SetClipZoom(Me.DisplaySize.GetValueOrDefault, value.GetValueOrDefault(fullscreen)))
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackVideoSize As Size?
            Get
                Return Me.StatusSource.PlaybackVideoSize
            End Get
        End Property

        Public Overrides Property PlaybackWindowFullscreen As Boolean?
            Get
                Return MyBase.PlaybackWindowFullscreen
            End Get
            Set(value As Boolean?)
                Me.Execute(Command.Factory.SetWindowZoom(Me.DisplaySize.GetValueOrDefault, value.GetValueOrDefault(True)))
            End Set
        End Property

        Public Overrides Property PlaybackWindowRectangle As Rectangle?
            Get
                Return MyBase.PlaybackWindowRectangle
            End Get
            Set(value As Rectangle?)
                Dim fullscreen As New Rectangle(Nothing, Me.DisplaySize.GetValueOrDefault)
                Me.Execute(Command.Factory.SetWindowZoom(Me.DisplaySize.GetValueOrDefault, value.GetValueOrDefault(fullscreen)))
            End Set
        End Property

        Public Overrides Property SubtitlesEnabled As Boolean?
            Get
                Return Me.StatusSource.SubtitlesEnabled
            End Get
            Set(value As Boolean?)
                Me.Execute(Command.Factory.SetSubtitlesEnabled(value.GetValueOrDefault(False)))
            End Set
        End Property

        Public Overrides Property SubtitlesTrack As Integer?
            Get
                Return Me.StatusSource.SubtitlesTrack
            End Get
            Set(value As Integer?)
                If value >= &H40 Then value = &H3F
                Me.Execute(Command.Factory.SetSubtitlesTrack(value.GetValueOrDefault(&HFF)))
            End Set
        End Property

        Public Overrides ReadOnly Property SubtitlesTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Return Me.StatusSource.SubtitlesTracks
            End Get
        End Property

        Public Overrides Property Text As String
            Get
                If Me.TextSource Is Nothing Then
                    Return String.Empty
                End If
                Return Me.TextSource.Text
            End Get
            Set(value As String)
                Me.Execute(Command.Factory.SetText(value))
            End Set
        End Property

        Public Overrides ReadOnly Property IsTextFieldActive As Boolean?
            Get
                If Me.TextSource Is Nothing Then
                    Return False
                End If
                Return Me.TextSource.IsTextFieldActive
            End Get
        End Property

        Public Overrides Property VideoOnTop As Boolean?
            Get
                Return Me.StatusSource.VideoOnTop
            End Get
            Set(value As Boolean?)
                Me.Execute(Command.Factory.SetVideoOnTop(value.GetValueOrDefault(False)))
            End Set
        End Property

        Public Overrides ReadOnly Property PlaybackState As PlaybackState
            Get
                Return Me.StatusSource.PlaybackState
            End Get
        End Property

        Public Overrides ReadOnly Property PreviousPlaybackState As PlaybackState
            Get
                Return Me.StatusSource.PreviousPlaybackState
            End Get
        End Property

        Public Overrides ReadOnly Property LastPlaybackEvent As PlaybackEvent
            Get
                Return Me.StatusSource.LastPlaybackEvent
            End Get
        End Property

        Public Overrides Function SetSubtitlesEnabled(enabled As Boolean) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetSubtitlesEnabled(enabled)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetSubtitlesEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetSubtitlesEnabled(enabled)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetSubtitlesTrack(index As Integer) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetSubtitlesTrack(index)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetSubtitlesTrackAsync(index As Integer) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetSubtitlesTrack(index)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function GetText() As TextCommandResult
            Return DirectCast(Me.Execute(Command.Factory.GetText), TextCommandResult)
        End Function

        Public Overrides Async Function GetTextAsync() As Task(Of TextCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.GetText).ConfigureAwait(False), TextCommandResult)
        End Function

        Public Overrides Function SetText(text As String) As TextCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetText(text)), TextCommandResult)
        End Function

        Public Overrides Async Function SetTextAsync(text As String) As Task(Of TextCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetText(text)).ConfigureAwait(False), TextCommandResult)
        End Function

        Public Overrides Function SetVideoOnTop(onTop As Boolean) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetVideoOnTop(onTop)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetVideoOnTopAsync(onTop As Boolean) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetVideoOnTop(onTop)).ConfigureAwait(False), StatusCommandResult)
        End Function

    End Class

End Namespace


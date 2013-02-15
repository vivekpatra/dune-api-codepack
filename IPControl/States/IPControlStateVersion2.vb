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

    Public Class IPControlStateVersion2 : Inherits IPControlStateVersion1

        Private NotSupportedMessage As String = "Action unavailable in protocol version 2"
        Private Shared Version As Version

        Shared Sub New()
            IPControlStateVersion2.Version = New Version(2, 0)
        End Sub

        Public Sub New(target As Dune, status As StatusCommandResult)
            MyBase.New(target, status)
        End Sub

        Public Overrides ReadOnly Property ProtocolVersion As Version
            Get
                Return IPControlStateVersion2.Version
            End Get
        End Property

        Public Overrides Property AudioEnabled As Boolean?
            Get
                Return Me.StatusSource.AudioEnabled
            End Get
            Set(value As Boolean?)
                Me.Execute(Command.Factory.SetAudioEnabled(value.GetValueOrDefault(True)))
            End Set
        End Property

        Public Overrides Property AudioTrack As Integer?
            Get
                Return Me.StatusSource.AudioTrack
            End Get
            Set(value As Integer?)
                Me.Execute(Command.Factory.SetAudioTrack(value.GetValueOrDefault))
            End Set
        End Property

        Public Overrides ReadOnly Property AudioTracks As ReadOnlyObservableCollection(Of MediaStream)
            Get
                Return Me.StatusSource.AudioTracks
            End Get
        End Property

        Public Overrides ReadOnly Property DisplaySize As Size?
            Get
                Return Me.StatusSource.DisplaySize
            End Get
        End Property

        Public Overrides Property PlaybackVolume As Integer?
            Get
                Return Me.StatusSource.PlaybackVolume
            End Get
            Set(value As Integer?)
                Me.Execute(Command.Factory.SetVolume(value.GetValueOrDefault(100)))
            End Set
        End Property

        Public Overrides Property PlaybackWindowFullscreen As Boolean?
            Get
                Return Me.StatusSource.PlaybackWindowFullscreen
            End Get
            Set(value As Boolean?)
                Me.Execute(Command.Factory.SetVideoZoom(Me.DisplaySize.GetValueOrDefault, value.GetValueOrDefault(True)))
            End Set
        End Property

        Public Overrides Property PlaybackWindowRectangle As Rectangle?
            Get
                If Me.DisplaySize.HasValue Then
                    Return Me.StatusSource.PlaybackWindowRectangle.GetValueOrDefault(New Rectangle(Nothing, Me.DisplaySize.Value))
                End If
            End Get
            Set(value As Rectangle?)
                Dim fullscreen As New Rectangle(Nothing, Me.DisplaySize.GetValueOrDefault)
                Me.Execute(Command.Factory.SetVideoZoom(Me.DisplaySize.GetValueOrDefault, value.GetValueOrDefault(fullscreen)))
            End Set
        End Property

        Public Overrides Property VideoEnabled As Boolean?
            Get
                Return Me.StatusSource.VideoEnabled
            End Get
            Set(value As Boolean?)
                Me.Execute(Command.Factory.SetVideoEnabled(value.GetValueOrDefault(True)))
            End Set
        End Property

        Public Overrides Property VideoZoom As VideoZoom
            Get
                Return Me.StatusSource.VideoZoom
            End Get
            Set(value As VideoZoom)
                Me.Execute(Command.Factory.SetVideoZoom(value))
            End Set
        End Property

        Public Overrides Function SetAudioEnabled(enabled As Boolean) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetAudioEnabled(enabled)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetAudioEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetAudioEnabled(enabled)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetAudioTrack(index As Integer) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetAudioTrack(index)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetAudioTrackAsync(index As Integer) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetAudioTrack(index)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetPlaybackVolume(volume As Integer) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetVolume(volume)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetPlaybackVolumeAsync(volume As Integer) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetVolume(volume)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetVideoEnabled(enabled As Boolean) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetVideoEnabled(enabled)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetVideoEnabledAsync(enabled As Boolean) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetVideoEnabled(enabled)).ConfigureAwait(False), StatusCommandResult)
        End Function

        Public Overrides Function SetVideoZoom(zoom As VideoZoom) As StatusCommandResult
            Return DirectCast(Me.Execute(Command.Factory.SetVideoZoom(zoom)), StatusCommandResult)
        End Function

        Public Overrides Async Function SetVideoZoomAsync(zoom As VideoZoom) As Task(Of StatusCommandResult)
            Return DirectCast(Await Me.ExecuteAsync(Command.Factory.SetVideoZoom(zoom)).ConfigureAwait(False), StatusCommandResult)
        End Function

    End Class

End Namespace


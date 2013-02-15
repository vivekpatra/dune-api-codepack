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

''' <summary>
''' Provides data for the <see cref="Dune.PlaybackStateChanged"/> event.
''' </summary>
Public Class PlaybackStateChangedEventArgs
    Inherits EventArgs

    Private _playbackState As PlaybackState
    Private _previousPlaybackState As PlaybackState
    Private _lastPlaybackEvent As PlaybackEvent

    Public Sub New(playbackState As PlaybackState, previousPlaybackState As PlaybackState, lastPlaybackEvent As PlaybackEvent)
        Me.PlaybackState = playbackState
        Me.PreviousPlaybackState = previousPlaybackState
        Me.LastPlaybackEvent = lastPlaybackEvent
    End Sub

    ''' <summary>
    ''' Gets the current playback state.
    ''' </summary>
    Public Property PlaybackState As PlaybackState
        Get
            Return _playbackState
        End Get
        Private Set(value As PlaybackState)
            _playbackState = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the previous playback state.
    ''' </summary>
    Public Property PreviousPlaybackState As PlaybackState
        Get
            Return _previousPlaybackState
        End Get
        Private Set(value As PlaybackState)
            _previousPlaybackState = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the last playback event.
    ''' </summary>
    Public Property LastPlaybackEvent As PlaybackEvent
        Get
            Return _lastPlaybackEvent
        End Get
        Private Set(value As PlaybackEvent)
            _lastPlaybackEvent = value
        End Set
    End Property
End Class

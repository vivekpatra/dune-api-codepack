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
    ''' A helper class for retrieving and comparing playback states and for creating new playback states.
    ''' </summary>
    Public Class PlaybackState : Inherits NameValuePair

        Private Shared ReadOnly _playing As PlaybackState = New PlaybackState("playing")
        Private Shared ReadOnly _initializing As PlaybackState = New PlaybackState("initializing")
        Private Shared ReadOnly _paused As PlaybackState = New PlaybackState("paused")
        Private Shared ReadOnly _seeking As PlaybackState = New PlaybackState("seeking")
        Private Shared ReadOnly _stopped As PlaybackState = New PlaybackState("stopped")
        Private Shared ReadOnly _deinitializing As PlaybackState = New PlaybackState("deinitializing")
        Private Shared ReadOnly _buffering As PlaybackState = New PlaybackState("buffering")
        Private Shared ReadOnly _finished As PlaybackState = New PlaybackState("finished")

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlaybackState"/> class with a specific playback state.
        ''' </summary>
        Public Sub New(playbackState As String)
            MyBase.New(playbackState)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Output.PlaybackState.Name
            End Get
        End Property

        Public Shared ReadOnly Property Playing As PlaybackState
            Get
                Return PlaybackState._playing
            End Get
        End Property

        Public Shared ReadOnly Property Initializing As PlaybackState
            Get
                Return PlaybackState._initializing
            End Get
        End Property

        Public Shared ReadOnly Property Paused As PlaybackState
            Get
                Return PlaybackState._paused
            End Get
        End Property

        Public Shared ReadOnly Property Seeking As PlaybackState
            Get
                Return PlaybackState._seeking
            End Get
        End Property

        Public Shared ReadOnly Property Stopped As PlaybackState
            Get
                Return PlaybackState._stopped
            End Get
        End Property

        Public Shared ReadOnly Property Deinitializing As PlaybackState
            Get
                Return PlaybackState._deinitializing
            End Get
        End Property

        Public Shared ReadOnly Property Buffering As PlaybackState
            Get
                Return PlaybackState._buffering
            End Get
        End Property

        Public Shared ReadOnly Property Finished As PlaybackState
            Get
                Return PlaybackState._finished
            End Get
        End Property

    End Class

End Namespace
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
''' Provides data for the <see cref="Dune.PlayerStateChanged"/> event.
''' </summary>
Public Class PlayerStateChangedEventArgs
    Inherits EventArgs

    Private _playerState As StatusCommandResult
    Private _previousPlayerState As StatusCommandResult

    Public Sub New(playerState As StatusCommandResult, previousPlayerState As StatusCommandResult)
        Me.PlayerState = playerState
        Me.PreviousPlayerState = previousPlayerState
    End Sub

    ''' <summary>
    ''' Gets the current player state.
    ''' </summary>
    Public Property PlayerState As StatusCommandResult
        Get
            Return _playerState
        End Get
        Private Set(value As StatusCommandResult)
            _playerState = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the previous player state.
    ''' </summary>
    Public Property PreviousPlayerState As StatusCommandResult
        Get
            Return _previousPlayerState
        End Get
        Private Set(value As StatusCommandResult)
            _previousPlayerState = value
        End Set
    End Property

End Class

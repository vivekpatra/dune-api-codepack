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
''' Provides data for the <see cref="Dune.ClientStateChanged"/> event.
''' </summary>
Public Class ClientStateChangedEventArgs
    Inherits EventArgs

    Private _clientState As ClientState
    Private _previousClientState As ClientState

    Public Sub New(clientState As ClientState, previousClientState As ClientState)
        Me.ClientState = clientState
        Me.PreviousClientState = previousClientState
    End Sub

    ''' <summary>
    ''' Gets the current client state.
    ''' </summary>
    Public Property ClientState As ClientState
        Get
            Return _clientState
        End Get
        Private Set(value As ClientState)
            _clientState = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the previous client state.
    ''' </summary>
    Public Property PreviousClientState As ClientState
        Get
            Return _previousClientState
        End Get
        Private Set(value As ClientState)
            _previousClientState = value
        End Set
    End Property

End Class

Public Class ClientState

    Private _connected As Boolean
    Private _protocol As Version

    Public Sub New(state As IIPControlState)
        If state Is Nothing Then
            _connected = False
            _protocol = New Version(0, 0)
        Else
            _connected = state.IsConnected
            _protocol = state.ProtocolVersion
        End If
    End Sub

    Public ReadOnly Property IsConnected As Boolean
        Get
            Return _connected
        End Get
    End Property

    Public ReadOnly Property TargetProtocol As Version
        Get
            Return _protocol
        End Get
    End Property

End Class

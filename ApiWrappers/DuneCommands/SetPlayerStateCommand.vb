Imports System.Collections.Specialized

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to set the player state.</summary>
    Public Class SetPlayerStateCommand
        Inherits DuneCommand

        Private _state As PlayerState

        ''' <param name="dune">The target device.</param>
        ''' <param name="state">The requested player state.</param>
        Public Sub New(ByRef dune As Dune, ByVal state As PlayerState)
            MyBase.New(dune)
            _state = state
        End Sub

        ''' <summary>
        ''' Enumeration of supported player states.
        ''' </summary>
        Public Enum PlayerState
            MainScreen = 0
            BlackScreen = 1
            Standby = 2
        End Enum

        Public ReadOnly Property State As PlayerState
            Get
                Return _state
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            Select Case _state
                Case PlayerState.MainScreen
                    query.Add("cmd", Constants.Commands.MainScreen)
                Case PlayerState.BlackScreen
                    query.Add("cmd", Constants.Commands.BlackScreen)
                Case PlayerState.Standby
                    query.Add("cmd", Constants.Commands.Standby)
            End Select

            Return query
        End Function
    End Class

End Namespace
Namespace Dune.ApiWrappers

    ''' <summary>This command is used to set the player state.</summary>
    Public Class SetPlayerStateCommand
        Inherits DuneCommand

        ''' <param name="dune">The target device.</param>
        ''' <param name="state">The requested player state.</param>
        Public Sub New(ByRef dune As Dune, ByVal state As PlayerState)
            MyBase.New(dune)
            _playerState = state
        End Sub

        Private _playerState As PlayerState

        ''' <summary>
        ''' Gets the requested player state.
        ''' </summary>
        Public ReadOnly Property State As PlayerState
            Get
                Return _playerState
            End Get
        End Property

        ''' <summary>
        ''' Enumeration of supported player states.
        ''' </summary>
        Public Enum PlayerState
            MainScreen = 0
            BlackScreen = 1
            Standby = 2
        End Enum

        Public Overrides Function ToUri() As System.Uri
            Dim query As String = Nothing

            Select Case State
                Case PlayerState.MainScreen
                    query = "cmd=main_screen"
                Case PlayerState.BlackScreen
                    query = "cmd=black_screen"
                Case PlayerState.Standby
                    query = "cmd=standby"
            End Select

            Return New Uri(BaseUri.ToString + query)
        End Function
    End Class

End Namespace
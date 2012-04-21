Namespace Dune.ApiWrappers

    ''' <summary>This command is used to set the player state.</summary>
    Public Class SetPlayerStateCommand
        Inherits DuneCommand

        ''' <param name="dune">The target device.</param>
        ''' <param name="state">The requested player state.</param>
        Public Sub New(ByRef dune As Dune, ByVal state As PlayerState)
            MyBase.New(dune)
            Select Case state
                Case PlayerState.MainScreen
                    CommandType = Constants.Commands.MainScreen
                Case PlayerState.BlackScreen
                    CommandType = Constants.Commands.BlackScreen
                Case PlayerState.Standby
                    CommandType = Constants.Commands.Standby
            End Select
        End Sub

        ''' <summary>
        ''' Enumeration of supported player states.
        ''' </summary>
        Public Enum PlayerState
            MainScreen = 0
            BlackScreen = 1
            Standby = 2
        End Enum

        Public Overrides Function GetQueryString() As String
            Return New String("cmd=" + CommandType)
        End Function
    End Class

End Namespace
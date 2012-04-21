Namespace Dune.ApiWrappers

    ''' <summary>
    ''' This command gets the player status without changing the player state in any way.
    ''' </summary>
    Public Class GetStatusCommand
        Inherits DuneCommand

        Public Sub New(ByRef dune As Dune)
            MyBase.New(dune)
            CommandType = Constants.Commands.Status
        End Sub

        Public Overrides Function GetQueryString() As String
            Return New String("cmd=" + CommandType)
        End Function
    End Class

End Namespace
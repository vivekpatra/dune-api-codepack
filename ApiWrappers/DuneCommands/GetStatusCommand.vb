Namespace Dune.ApiWrappers

    ''' <summary>
    ''' This command gets the player status without changing the player state in any way.
    ''' </summary>
    Public Class GetStatusCommand
        Inherits DuneCommand

        Public Sub New(ByRef dune As Dune)
            MyBase.New(dune)
        End Sub

        Public Overrides Function ToUri() As System.Uri
            Dim query As String

            query = "cmd=status"

            Return New Uri(BaseUri.ToString + query)

        End Function
    End Class

End Namespace
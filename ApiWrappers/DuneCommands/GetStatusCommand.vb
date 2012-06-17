Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This command gets the player status without changing the player state in any way.
    ''' </summary>
    Public Class GetStatusCommand
        Inherits Command


        Public Sub New(target As Dune)
            MyBase.New(target)
        End Sub

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.Status)

            Return query
        End Function

    End Class

End Namespace
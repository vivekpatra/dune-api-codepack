Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This command gets the player status without changing the player state in any way.
    ''' </summary>
    Public Class GetStatusCommand
        Inherits Command


        Public Sub New(ByRef dune As Dune)
            MyBase.New(dune)
        End Sub

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.Status)

            Return query
        End Function

    End Class

End Namespace
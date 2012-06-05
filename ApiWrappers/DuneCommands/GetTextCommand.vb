Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This command is used to retrieve text from an input box.
    ''' </summary>
    Public Class GetTextCommand
        Inherits Command

        ''' <param name="target">The target device.</param>
        Public Sub New(target As Dune)
            MyBase.New(target)
        End Sub

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection
            query.Add("cmd", Constants.Commands.GetText)
            Return query
        End Function
    End Class

End Namespace
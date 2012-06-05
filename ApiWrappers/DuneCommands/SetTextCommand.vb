Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This command is used to set text to an input box.
    ''' </summary>
    Public Class SetTextCommand
        Inherits Command

        Private _text As String

        ''' <param name="target">The target device.</param>
        ''' <param name="text">The input text.</param>
        Public Sub New(target As Dune, text As String)
            MyBase.New(target)
            _text = text
        End Sub

        ''' <summary>
        ''' Gets the specified text.
        ''' </summary>
        Public ReadOnly Property Text As String
            Get
                Return _text
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection
            query.Add("cmd", Constants.Commands.GetText)
            query.Add(Constants.SetTextParameters.Text, Text)
            Return query
        End Function
    End Class

End Namespace
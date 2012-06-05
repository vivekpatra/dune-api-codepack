Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to set the player state.</summary>
    Public Class SetPlayerStateCommand
        Inherits Command

        Private _state As String

        ''' <param name="target">The target device.</param>
        ''' <param name="state">The requested player state.</param>
        Public Sub New(target As Dune, state As String)
            MyBase.New(target)
            _state = state
        End Sub

        Public ReadOnly Property State As String
            Get
                Return _state
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", State)

            Return query
        End Function
    End Class

End Namespace
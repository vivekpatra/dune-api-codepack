Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to set the player state.</summary>
    Public Class SetPlayerStateCommand
        Inherits DuneCommand

        Private _state As String

        ''' <param name="dune">The target device.</param>
        ''' <param name="state">The requested player state.</param>
        Public Sub New(ByRef dune As Dune, ByVal state As String)
            MyBase.New(dune)
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
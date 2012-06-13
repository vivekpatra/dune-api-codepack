Public Class DisconnectedEventArgs
    Inherits EventArgs

    Private _expected As Boolean

    Public Sub New(expected As Boolean)
        _expected = expected
    End Sub

    ''' <summary>
    ''' Gets whether the connection was aborted on purpose or not.
    ''' </summary>
    Public ReadOnly Property Expected As Boolean
        Get
            Return _expected
        End Get
    End Property

End Class

Public Delegate Sub DisconnectedEventHandler(sender As Object, e As DisconnectedEventArgs)
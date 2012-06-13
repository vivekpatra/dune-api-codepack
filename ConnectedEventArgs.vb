Public Class ConnectedEventArgs
    Inherits EventArgs

    Private _initialConnection As Boolean

    Public Sub New(initialConnection As Boolean)
        _initialConnection = initialConnection
    End Sub

    ''' <summary>
    ''' Gets whether this is the first time connecting to this host.
    ''' </summary>
    Public ReadOnly Property InitialConnection As Boolean
        Get
            Return _initialConnection
        End Get
    End Property

End Class

Public Delegate Sub ConnectedEventHandler(sender As Object, e As ConnectedEventArgs)

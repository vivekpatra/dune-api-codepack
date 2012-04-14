Namespace Dune.ApiWrappers

    ''' <summary>
    ''' This is the base class for all Dune commands.
    ''' </summary>
    Public MustInherit Class DuneCommand
        Private _dune As Dune
        Private _timeout As Nullable(Of Integer)

        Public Sub New(ByRef dune As Dune)
            _dune = dune
        End Sub

        ''' <summary>
        ''' Gets the target Dune device.
        ''' </summary>
        Public ReadOnly Property Dune As Dune
            Get
                Return _dune
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the command timeout.
        ''' </summary>
        Public Property Timeout As Nullable(Of Integer)
            Get
                Return _timeout
            End Get
            Set(value As Nullable(Of Integer))
                _timeout = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the base URI without command parameters.
        ''' </summary>
        Protected ReadOnly Property BaseUri As Uri
            Get
                Return New Uri(String.Format("http://{0}:{1}/cgi-bin/do?", Dune.Address, Dune.Port))
            End Get
        End Property

        ''' <summary>
        ''' Gets the command URI.
        ''' </summary>
        Public MustOverride Function ToUri() As Uri

    End Class

End Namespace
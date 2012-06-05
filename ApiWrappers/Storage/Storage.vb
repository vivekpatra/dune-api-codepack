Imports System.IO
Imports System.Net

Namespace Storage

    ''' <summary>
    ''' This is the base class for all types that represent a storage device, whether it be local or remote, accessible or inaccessible.
    ''' This class cannot be instantiated directly.
    ''' </summary>
    Public MustInherit Class Storage
        Protected _host As IPHostEntry
        Protected _root As DirectoryInfo

        Public Sub New(host As IPHostEntry)
            _host = host
        End Sub

        Public ReadOnly Property Host As IPHostEntry
            Get
                Return _host
            End Get
        End Property

        Public ReadOnly Property Root As DirectoryInfo
            Get
                Return _root
            End Get
        End Property

        Public Overridable Function GetMediaUrl() As String
            Return Root.FullName
        End Function

    End Class

End Namespace


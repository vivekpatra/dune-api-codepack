Imports System.IO
Imports System.Net

Namespace Sources

    ''' <summary>
    ''' This is the base class for all types that represent a storage device, whether it be local or remote, accessible or inaccessible.
    ''' This class cannot be instantiated directly.
    ''' </summary>
    Public MustInherit Class StorageDevice
        Friend _host As IPHostEntry
        Friend _root As DirectoryInfo

        Protected Sub New(host As IPHostEntry)
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


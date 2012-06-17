Imports System.Net

Namespace Sources

    Public Class NamedShare
        Inherits StorageDevice

        Private _name As String
        Private _share As IO.DirectoryInfo

        ''' <param name="host">The host where the network share is hosted.</param>
        ''' <param name="name">The name of the network folder as defined on the Dune device.</param>
        ''' <param name="path">The path to the named network folder.</param>
        ''' <remarks></remarks>
        Public Sub New(host As IPHostEntry, name As String, path As IO.FileSystemInfo)
            MyBase.New(host)
            _name = name
            _share = path.ToDirectoryInfo
            _root = path.GetRoot
        End Sub

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Public Property Share As IO.DirectoryInfo
            Get
                Return _share
            End Get
            Set(value As IO.DirectoryInfo)
                _share = value
            End Set
        End Property

        Public Overrides Function GetMediaUrl() As String
            Dim mediaUrl As New Text.StringBuilder

            mediaUrl.Append("network_folder://")
            mediaUrl.Append(Name)

            Return mediaUrl.ToString
        End Function

        Public Overloads Function GetMediaUrl(path As IO.FileSystemInfo) As String
            If Not path.FullName.Contains(Share.FullName) Then
                Throw New ArgumentException("The specified path is not a member of this network share.", "path")
            Else
                Dim mediaUrl As New Text.StringBuilder

                mediaUrl.Append(Me.GetMediaUrl)

                Dim suffix As String = path.FullName.Replace(Share.FullName, String.Empty).Replace("\"c, "/"c)

                mediaUrl.Append("/")
                mediaUrl.Append(suffix)

                Return mediaUrl.ToString
            End If
        End Function

        Public Function TryGetMediaUrl(path As IO.FileSystemInfo, ByRef mediaUrl As String) As Boolean
            If Not path.FullName.Contains(Share.FullName) Then
                Return False
            Else
                mediaUrl = GetMediaUrl(path)
                Return True
            End If
        End Function
    End Class

End Namespace
Imports System.Net
Imports SL.DuneApiCodePack.NativeMethods.Networking

Namespace Storage

    ''' <summary>
    ''' This type represents an SMB share.
    ''' </summary>
    Public Class SmbShare
        Inherits Storage
        Private _credentials As NetworkCredential
        Private _diskInfo As ShareInfo

        Public Sub New(ByVal uncPath As Uri)
            MyBase.New(Dns.GetHostEntry(uncPath.Host.ToUpper))
            _root = New IO.DirectoryInfo(uncPath.OriginalString).Root
            _credentials = New NetworkCredential
        End Sub

        ''' <summary>
        ''' The SMB username.
        ''' </summary>
        Public Property Username As String
            Get
                Return _credentials.UserName
            End Get
            Set(value As String)
                _credentials.UserName = value
            End Set
        End Property

        ''' <summary>
        ''' The SMB password.
        ''' </summary>
        Public Property Password As String
            Get
                Return _credentials.Password
            End Get
            Set(value As String)
                _credentials.Password = value
            End Set
        End Property

        Public ReadOnly Property DiskInfo As ShareInfo
            Get
                If _diskInfo.TotalNumberOfClusters = Nothing Then
                    _diskInfo = GetShareInfo(Me.Root.GetUri)
                End If
                Return _diskInfo
            End Get
        End Property

        Public Sub RefreshDiskInfo()
            _diskInfo = GetShareInfo(Me.Root.GetUri)
        End Sub

        ''' <summary>
        ''' Gets the media URL from the specified UNC path.
        ''' </summary>
        ''' <param name="path">The path in UNC notation.</param>
        ''' <returns>The media URL in smb://[username[:password]@]host/share/path[/file.extension] format</returns>
        ''' <remarks>Mounted shares are not supported, the path must be in UNC notation.</remarks>
        Public Function GetMediaUrl(ByVal path As IO.DirectoryInfo) As String
            If Not path.GetUri.IsUnc Then
                Throw New ArgumentException("The path must be a valid UNC notation, mounted shares are not supported.", "path")
            ElseIf path.Root.FullName <> Root.FullName Then
                Throw New ArgumentException(path.Name + " is not a child on this network share.", "path")
            Else
                Dim container As New IO.DirectoryInfo(path.FullName)

                Dim mediaUrl As New Text.StringBuilder
                mediaUrl.Append("smb://")
                If Not String.IsNullOrWhiteSpace(_credentials.UserName) Then
                    mediaUrl.Append(_credentials.UserName)
                    If Not String.IsNullOrWhiteSpace(_credentials.Password) Then
                        mediaUrl.Append(":")
                        mediaUrl.Append(_credentials.Password)
                    End If
                    mediaUrl.Append("@")
                End If
                mediaUrl.Append(Host.HostName)
                mediaUrl.Append("/")
                mediaUrl.Append(Root.Name)

                Dim suffix As String = path.FullName.Replace(container.Root.FullName, String.Empty).Replace("\"c, "/"c)
                mediaUrl.Append(suffix)
                Return mediaUrl.ToString
            End If
        End Function

        ''' <summary>
        ''' Tries to get the media URL for the specified UNC path.
        ''' </summary>
        ''' <param name="path">Path in valid UNC notation.</param>
        ''' <param name="mediaUrl">The string variable that will contain the media URL if the method call is successful.</param>
        ''' <returns>True if the call was successful; otherwise false.</returns>
        Public Function TryGetMediaUrl(ByVal path As IO.DirectoryInfo, ByRef mediaUrl As String) As Boolean
            If path.Root.FullName <> Root.FullName Or Not path.GetUri.IsUnc Then
                Return False
            Else
                mediaUrl = GetMediaUrl(path)
                Return True
            End If
        End Function

        Public Shared Function FromHost(ByVal host As IPHostEntry) As List(Of SmbShare)
            Dim shares As ShareCollection = ShareCollection.GetShares(host.HostName)
            Dim smbShares As New List(Of SmbShare)

            For Each share As Share In shares
                If share.ShareType = ShareType.Disk Then
                    Dim smbShare As New SmbShare(share.Root.GetUri)
                    smbShares.Add(smbShare)
                End If
            Next

            Return smbShares
        End Function

    End Class

End Namespace
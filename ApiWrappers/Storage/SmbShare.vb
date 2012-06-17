Imports System.Net
Imports SL.DuneApiCodePack.NativeMethods.Networking

Namespace Sources

    ''' <summary>
    ''' This type represents an SMB share.
    ''' </summary>
    Public Class SmbShare
        Inherits StorageDevice
        Private _credentials As NetworkCredential
        Private _diskInfo As ShareInfo

        Public Sub New(uncPath As Uri)
            MyBase.New(Dns.GetHostEntry(uncPath.Host.ToUpperInvariant))
            _root = New IO.DirectoryInfo(uncPath.OriginalString).Root
            _credentials = New NetworkCredential
        End Sub

        ''' <summary>
        ''' The SMB username.
        ''' </summary>
        Public Property UserName As String
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
        ''' Gets the media URL for the root of the share.
        ''' </summary>
        ''' <returns>The media URL in smb://[username[:password]@]host/root format</returns>
        Public Overrides Function GetMediaUrl() As String
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

            Return mediaUrl.ToString
        End Function

        ''' <summary>
        ''' Gets the media URL from the specified UNC path.
        ''' </summary>
        ''' <param name="path">The path in UNC notation.</param>
        ''' <returns>The media URL in smb://[username[:password]@]host/share/path[/file[.extension]] format</returns>
        ''' <remarks>Mounted shares are not supported, the path must be in UNC notation.</remarks>
        Public Overloads Function GetMediaUrl(path As IO.FileSystemInfo) As String
            If Not path.GetUri.IsUnc Then
                Throw New ArgumentException("The path must be a valid UNC notation, mounted shares are not supported.", "path")
            ElseIf New IO.DirectoryInfo(path.FullName).Root.FullName <> Root.FullName Then
                Throw New ArgumentException(path.Name + " is not a child on this network share.", "path")
            Else
                Dim container As New IO.DirectoryInfo(path.FullName)
                Dim mediaUrl As New Text.StringBuilder()

                mediaUrl.Append(Me.GetMediaUrl())

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
        Public Function TryGetMediaUrl(path As IO.FileSystemInfo, ByRef mediaUrl As String) As Boolean
            If New IO.DirectoryInfo(path.FullName).Root.FullName <> Root.FullName Or path.GetUri.IsUnc.IsFalse Then
                Return False
            Else
                mediaUrl = GetMediaUrl(path)
                Return True
            End If
        End Function

        ''' <summary>
        ''' Gets a list of SMB shares exposed by the specified host.
        ''' </summary>
        ''' <param name="host">The host to scan for network shares.</param>
        ''' <remarks>This method can only be used to return disk shares. Printer shares (or otherwise) are ignored.</remarks>
        Public Shared Function FromHost(host As IPHostEntry) As Collection(Of SmbShare)
            Dim shares As ShareCollection = ShareCollection.GetShares(host.HostName)
            Dim smbShares As New Collection(Of SmbShare)

            For Each share As Share In shares
                If share.ShareType = ShareType.Disk Then
                    Dim smbShare As New SmbShare(share.Root.GetUri)
                    smbShares.Add(smbShare)
                End If
            Next

            Return smbShares
        End Function

        Public Shared Function Parse(path As String) As SmbShare
            Dim share As SmbShare
            Dim credentials As String = String.Empty

            If path.Contains("@") Then 'get credentials
                credentials = path.Split("@"c).First.Substring(2)
                If credentials.Contains("/").IsFalse Then 'temporarily remove credentials so the URI can be parsed
                    path = path.Replace(String.Format(Globalization.CultureInfo.InvariantCulture, "{0}@", credentials), Nothing)
                End If
            End If

            share = New SmbShare(New Uri(path))

            If credentials.IsNotNullOrWhiteSpace Then
                If credentials.Contains(":") Then
                    share.UserName = credentials.Split(":"c).First
                    share.Password = credentials.Split(":"c).Last
                Else
                    share.UserName = credentials
                End If
            End If

            Return share
        End Function

    End Class

End Namespace
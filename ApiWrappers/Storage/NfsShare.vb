Imports System.Net
Imports System.IO

Namespace Sources

    ''' <summary>
    ''' This type represents an NFS share.
    ''' </summary>
    ''' <remarks>
    ''' NFS shares cannot be browsed directly, unless the user has 'services for NFS' installed (only on Windows 7 Ultimate and Enterprise or Windows Server editions).
    ''' When installed, it is favourable to mount the share to a drive letter, then correctly set the mount point property.
    ''' Doing so allows for input validation and ensures a correct playback media URL.
    ''' </remarks>
    Public Class NfsShare
        Inherits StorageDevice

        Private _mountPoint As DriveInfo
        Private _path As String
        Private _exportPath As String

        Public Sub New(host As IPHostEntry, path As String)
            Me.New(host, String.Empty, path, Nothing)
        End Sub

        Public Sub New(host As IPHostEntry, exportPath As String, path As String)
            Me.New(host, exportPath, path, Nothing)
        End Sub

        Public Sub New(host As IPHostEntry, path As String, mountPoint As DriveInfo)
            Me.New(host, String.Empty, path, mountPoint)
        End Sub

        Public Sub New(host As IPHostEntry, exportPath As String, path As String, mountPoint As DriveInfo)
            MyBase.New(host)

            _root = mountPoint.RootDirectory

            Me.MountPoint = mountPoint
            Me.ExportPath = exportPath
            Me.Path = path
        End Sub

        ''' <summary>
        ''' The NFS share path.
        ''' </summary>
        Public Property Path As String
            Get
                Return _path
            End Get
            Set(value As String)
                If String.IsNullOrWhiteSpace(value) Then
                    Throw New ArgumentException("The share path cannot be empty!")
                Else
                    _path = value.Replace("\"c, "/"c).Trim("/"c)
                End If
            End Set
        End Property

        ''' <summary>
        ''' The NFS export path.
        ''' </summary>
        Public Property ExportPath As String
            Get
                Return _exportPath
            End Get
            Set(value As String)
                If value = Nothing Then
                    _exportPath = Nothing
                Else
                    _exportPath = value.Replace("\"c, "/"c).Trim("/"c)
                End If
            End Set
        End Property

        ''' <summary>
        ''' The local drive letter on which the share is mounted.
        ''' </summary>
        Public Property MountPoint As DriveInfo
            Get
                Return _mountPoint
            End Get
            Set(value As DriveInfo)
                If value IsNot Nothing AndAlso value.DriveType <> DriveType.Network Then
                    Throw New ArgumentException("The specified mount point does not point to a network location!")
                Else
                    _mountPoint = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Tries to get the media URL for the specified absolute path.
        ''' </summary>
        ''' <param name="absolutePath">The full path, including the full share <see cref="Path"/>.</param>
        ''' <param name="mediaUrl">The string variable that will contain the media URL if the method call is successful.</param>
        ''' <returns>True if the call was successful; otherwise false.</returns>
        Public Function TryGetMediaUrl(absolutePath As DirectoryInfo, ByRef mediaUrl As String) As Boolean
            If MountPoint IsNot Nothing Then
                If MountPoint.RootDirectory.FullName <> absolutePath.Root.FullName Then
                    Return False
                Else
                    mediaUrl = GetMediaUrl(absolutePath)
                    Return True
                End If
            Else
                mediaUrl = GetMediaUrl(absolutePath)
                Return True
            End If
        End Function

        ''' <summary>
        ''' Gets the media URL for the specified absolute path where absolute path is a file or directory on this NFS share.
        ''' </summary>
        ''' <param name="absolutePath">The full path, including the full share <see cref="Path"/>.</param>
        ''' <returns>The media URL in nfs://host[:/export_path]:/share_path:/relative_path[/file[.extension]] format.</returns>
        Public Overloads Function GetMediaUrl(absolutePath As FileSystemInfo) As String
            Dim container As New DirectoryInfo(absolutePath.FullName)

            Dim relativePath As String

            If MountPoint IsNot Nothing Then
                If MountPoint.RootDirectory.FullName <> container.Root.FullName Then ' TODO: verify validation against more use cases
                    Throw New ArgumentException("The specified path is not a member of this NFS share!")
                Else
                    relativePath = absolutePath.FullName.Replace(MountPoint.RootDirectory.FullName, String.Empty).Replace("\"c, "/"c)
                End If
            Else
                Dim builder As New Text.StringBuilder

                builder.Append("/")
                Dim suffix As String = absolutePath.FullName.Replace(container.Root.FullName, String.Empty).Replace("\"c, "/"c).Trim("/"c).Replace(Path, String.Empty)
                builder.Append(suffix)
                relativePath = builder.ToString
            End If

            Return GetMediaUrl(relativePath)
        End Function

        ''' <summary>
        ''' Gets the media URL for the specified relative path where relative path is a file or directory on this NFS share.
        ''' </summary>
        ''' <param name="relativePath">The relative path, relative to the share <see cref="Path"/>.</param>
        ''' <returns>The media URL in nfs://host[:/export_path]:/share_path/relative_path[/file[.extension]] format.</returns>
        Public Overloads Function GetMediaUrl(relativePath As String) As String
            Dim mediaUrl As New Text.StringBuilder

            mediaUrl.Append(Me.GetMediaUrl())

            mediaUrl.Append("/")
            mediaUrl.Append(relativePath.TrimStart("/"c))

            Return mediaUrl.ToString
        End Function

        ''' <summary>
        ''' Gets the media URL for the root of this NFS share.
        ''' </summary>
        ''' <returns>The media URL in nfs://host[:/export_path]:/share_path format.</returns>
        Public Overrides Function GetMediaUrl() As String
            Dim mediaUrl As New Text.StringBuilder

            mediaUrl.Append("nfs://")
            Dim host As IPAddress = Me.Host.AddressList.First(Function(address) address.AddressFamily = Sockets.AddressFamily.InterNetwork And Not IPAddress.IsLoopback(address))
            If host IsNot Nothing Then ' use IPv4 address
                mediaUrl.Append(host.ToString)
            Else ' use hostname
                mediaUrl.Append(Me.Host.HostName)
            End If
            If Not String.IsNullOrWhiteSpace(ExportPath) Then
                mediaUrl.Append(":/")
                mediaUrl.Append(ExportPath)
            End If
            mediaUrl.Append(":/")
            mediaUrl.Append(Path)

            Return mediaUrl.ToString
        End Function

    End Class

End Namespace
Namespace Dune.Communicator
    Public Class MediaURL
        Public Enum Protocols
            HTTP
            SMB
            NFS
            UDP
            storage_name
            storage_label
            storage_uuid
        End Enum
        Public Shared Function MakeURL_SMB(ByVal Path As String)
            Return MakeURL_SMB(Path, Nothing)
        End Function

        Public Shared Function MakeURL_SMB(ByVal Path As String, ByVal SMBUsername As String)
            Return MakeURL_SMB(Path, SMBUsername, Nothing)
        End Function

        Public Shared Function MakeURL_SMB(ByVal Path As String, ByVal SMBUsername As String, ByVal SMBPassword As String)
            Dim strPath As String

            strPath = "smb://"
            If Not String.IsNullOrWhiteSpace(SMBUsername) Then
                strPath = String.Format("{0}{1}", strPath, SMBUsername)
                If Not String.IsNullOrWhiteSpace(SMBPassword) Then
                    strPath = String.Format("{0}:{1}", strPath, SMBPassword)
                End If
                strPath = String.Format("{0}@", strPath)
            End If
            strPath = String.Format("{0}", Replace(Path.Replace("\\", Nothing), "\", "/"))

            Return strPath
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Host">Can be IP-address or DNS-name of NFS server.</param>
        ''' <param name="Path">File path.</param>
        ''' <returns></returns>
        ''' <remarks>Path syntax: /folder/name[/file.ext]</remarks>
        Public Shared Function MakeURL_NFS(ByVal Host As String, ByVal Path As String)
            Return MakeURL_NFS(Host, Nothing, Path)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Host">Can be IP-address or DNS-name of NFS server.</param>
        ''' <param name="ExportPath">Optional. When omitted, the player tries to 
        ''' automatically deduce it from the specified "path" (by analyzing all NFS-exports of the NFS-server).
        ''' For better performance and correct working, it is recommended to always explicitly specify "export-path".
        ''' </param>
        ''' <param name="Path">File path.</param>
        ''' <returns></returns>
        ''' <remarks>(export) path syntax: /folder/name[/file.ext]</remarks>
        Public Shared Function MakeURL_NFS(ByVal Host As String, ByVal ExportPath As String, ByVal Path As String)
            Dim strPath As String

            strPath = "nfs://"

            If Not String.IsNullOrWhiteSpace(ExportPath) Then
                strPath = String.Format(":{0}", ExportPath)
            End If

            strPath = String.Format(":{0}", Path)

            Return strPath
        End Function



    End Class
End Namespace
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

        'Public Shared Function MakeURL_SMB(ByVal Path As String)
        '    Return MakeURL_SMB(Path, Nothing)
        'End Function

        'Public Shared Function MakeURL_SMB(ByVal Path As String, ByVal SMBUsername As String)
        '    Return MakeURL_SMB(Path, SMBUsername, Nothing)
        'End Function

        'Public Shared Function MakeURL_SMB(ByVal Path As String, ByVal SMBUsername As String, ByVal SMBPassword As String)
        '    Dim strPath As String

        '    strPath = "smb://"
        '    If Not String.IsNullOrWhiteSpace(SMBUsername) Then
        '        strPath = String.Format("{0}{1}", strPath, SMBUsername)
        '        If Not String.IsNullOrWhiteSpace(SMBPassword) Then
        '            strPath = String.Format("{0}:{1}", strPath, SMBPassword)
        '        End If
        '        strPath = String.Format("{0}@", strPath)
        '    End If
        '    strPath = String.Format("{0}", Replace(Path.Replace("\\", Nothing), "\", "/"))

        '    Return strPath
        'End Function

        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="Host">Can be IP-address or DNS-name of NFS server.</param>
        ' ''' <param name="Path">File path.</param>
        ' ''' <returns></returns>
        ' ''' <remarks>Path syntax: /folder/name[/file.ext]</remarks>
        'Public Shared Function MakeURL_NFS(ByVal Host As String, ByVal Path As String)
        '    Return MakeURL_NFS(Host, Nothing, Path)
        'End Function

        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="Host">Can be IP-address or DNS-name of NFS server.</param>
        ' ''' <param name="ExportPath">Optional. When omitted, the player tries to 
        ' ''' automatically deduce it from the specified "path" (by analyzing all NFS-exports of the NFS-server).
        ' ''' For better performance and correct working, it is recommended to always explicitly specify "export-path".
        ' ''' </param>
        ' ''' <param name="Path">File path.</param>
        ' ''' <returns></returns>
        ' ''' <remarks>(export) path syntax: /folder/name[/file.ext]</remarks>
        'Public Shared Function MakeURL_NFS(ByVal Host As String, ByVal ExportPath As String, ByVal Path As String)
        '    Dim strPath As String

        '    strPath = "nfs://"

        '    If Not String.IsNullOrWhiteSpace(ExportPath) Then
        '        strPath = String.Format(":{0}", ExportPath)
        '    End If

        '    strPath = String.Format(":{0}", Path)

        '    Return strPath
        'End Function


        Public Shared Function FormatURL(ByVal Dune As Dune, ByVal MediaURL As String) As String
            ' TODO: add support for nfs and udp protocols

            Dim protocol As Protocols = Nothing

            MediaURL = MediaURL.Replace("\", "/")
            Dim credentials As String = String.Empty

            If MediaURL.IndexOf("@") > -1 Then 'get credentials
                credentials = MediaURL.Split("@").GetValue(0).ToString.Replace("//", Nothing)
                If credentials.Contains("/") Then 'filename has one or more @ signs. What kind of person does that?!
                    credentials = String.Empty
                Else 'temporary remove credentials so the URI can be parsed
                    MediaURL = MediaURL.Replace(String.Format("{0}@", credentials), Nothing)
                End If
            End If


            Dim mediaURI As New Uri(MediaURL)
            Dim result As String = Nothing

            If mediaURI.IsUnc Then 'file is on an SMB share

                If mediaURI.Host = Dune.IP Or mediaURI.Host.ToLower = Dune.Hostname.ToLower Then ' use 'storage_name' approach
                    protocol = Protocols.storage_name
                    result = String.Format("storage_name:/{0}", mediaURI.AbsolutePath.Replace("&", "%26"))
                Else 'use 'smb' approach
                    protocol = Protocols.SMB
                    Dim delimiter As String() = New String() {Uri.SchemeDelimiter}
                    result = String.Format("smb://{0}", mediaURI.AbsoluteUri.Split(delimiter, StringSplitOptions.None).GetValue(1).ToString.Replace("&", "%26"))
                End If
            Else
                result = Uri.EscapeUriString(mediaURI.OriginalString).Replace("&", "%26")
                If result.ToLower.Contains("http:") Then
                    protocol = Protocols.HTTP
                End If
            End If

            If Not String.IsNullOrWhiteSpace(credentials) And protocol = Protocols.SMB Or protocol = Protocols.HTTP Then
                result = result.Replace(mediaURI.Host, String.Format("{0}@{1}", credentials, mediaURI.Host))
            End If

            Return result

        End Function



    End Class
End Namespace
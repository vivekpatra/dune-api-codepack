Namespace Dune.ApiWrappers

    ''' <summary>
    ''' This class is used to format media URLs.
    ''' </summary>
    Public NotInheritable Class UrlConverter

        ''' <summary>
        ''' Enumeration of supported protocols.
        ''' </summary>
        Public Enum Protocol
            HTTP
            SMB
            NFS
            UDP
            storage_name
            storage_label
            storage_uuid
        End Enum

        ' TODO: add support for other protocols
        ''' <summary>
        ''' Formats the media URL to a string that the device can use.
        ''' </summary>
        Public Shared Function FormatUrl(ByRef dune As Dune, ByVal mediaUrl As String) As String

            Dim protocol As Protocol = Nothing

            mediaUrl = mediaUrl.Replace("\", "/")
            Dim credentials As String = String.Empty

            If mediaUrl.IndexOf("@") > -1 Then 'get credentials
                credentials = mediaUrl.Split("@").GetValue(0).ToString.Replace("//", Nothing)
                If credentials.Contains("/") Then 'filename has one or more @ signs. What kind of person does that?!
                    credentials = String.Empty
                Else 'temporary remove credentials so the URI can be parsed
                    mediaUrl = mediaUrl.Replace(String.Format("{0}@", credentials), Nothing)
                End If
            End If


            Dim mediaUri As New Uri(mediaUrl)
            Dim result As String = Nothing

            If mediaUri.IsUnc Then 'file is on an SMB share

                If mediaUri.Host = dune.Address.ToString Or mediaUri.Host.ToLower = dune.Hostname.ToLower Then ' use 'storage_name' approach
                    protocol = protocol.storage_name
                    result = String.Format("storage_name:/{0}", mediaUri.AbsolutePath.Replace("&", "%26"))
                Else 'use 'smb' approach
                    protocol = protocol.SMB
                    Dim delimiter As String() = New String() {Uri.SchemeDelimiter}
                    result = String.Format("smb://{0}", mediaUri.AbsoluteUri.Split(delimiter, StringSplitOptions.None).GetValue(1).ToString.Replace("&", "%26"))
                End If
            Else
                result = Uri.EscapeUriString(mediaUri.OriginalString).Replace("&", "%26")
                If result.ToLower.Contains("http:") Then
                    protocol = protocol.HTTP
                End If
            End If

            If Not String.IsNullOrWhiteSpace(credentials) And protocol = protocol.SMB Or protocol = protocol.HTTP Then
                result = result.Replace(mediaUri.Host, String.Format("{0}@{1}", credentials, mediaUri.Host))
            End If

            Return result

        End Function



    End Class
End Namespace
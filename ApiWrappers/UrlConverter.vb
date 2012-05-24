Option Compare Text

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This class is used to format media URLs.
    ''' </summary>
    Public NotInheritable Class UrlConverter

        ' TODO: add support for other protocols
        ''' <summary>
        ''' Formats the media URL to a string that the device can use.
        ''' </summary>
        Public Shared Function FormatUrl(ByRef target As Dune, ByVal mediaUrl As String) As String

            mediaUrl = mediaUrl.Replace("\", "/")
            Dim credentials As String = String.Empty

            If mediaUrl.Contains("@") Then 'get credentials
                credentials = mediaUrl.Split(CChar("@")).GetValue(0).ToString.Replace("//", Nothing)
                If credentials.Contains("/") Then 'filename has one or more @ signs. What kind of person does that?!
                    credentials = String.Empty
                Else 'temporarily remove credentials so the URI can be parsed
                    mediaUrl = mediaUrl.Replace(String.Format("{0}@", credentials), Nothing)
                End If
            End If


            Dim mediaUri As New Uri(mediaUrl)
            Dim result As String = Nothing

            If mediaUri.IsUnc Then 'file is on an SMB share

                If mediaUri.Host = target.Address.ToString Or mediaUri.Host = target.Hostname Then ' use 'storage_name' approach
                    result = String.Format("storage_name:{0}", mediaUri.OriginalString.Replace("/" + target.Hostname, String.Empty))
                Else 'use 'smb' approach
                    result = "smb:" + mediaUri.OriginalString
                End If
            Else
                result = mediaUri.OriginalString
            End If

            If Not String.IsNullOrWhiteSpace(credentials) Then
                result = result.Replace(mediaUri.Host, String.Format("{0}@{1}", credentials, mediaUri.Host))
            End If

            Return result

        End Function



    End Class
End Namespace
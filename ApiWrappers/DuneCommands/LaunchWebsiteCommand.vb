Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    Public Class LaunchWebsiteCommand
        Inherits Command

        Private _website As Uri

        Public Sub New(target As Dune, website As Uri)
            MyBase.New(target)
            _website = website
        End Sub

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.LaunchMediaUrl)
            query.Add(Constants.StartPlaybackParameters.MediaUrl, "www://" + _website.AbsoluteUri)

            Return query
        End Function
    End Class

End Namespace
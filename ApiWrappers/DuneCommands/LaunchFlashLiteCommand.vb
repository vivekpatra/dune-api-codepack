Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    Public Class LaunchFlashLiteCommand
        Inherits Command

        Private _mediaUrl As String
        Private _flashvars As NameValueCollection

        Public Sub New(target As Dune, mediaUrl As String)
            MyBase.New(target)
            _mediaUrl = mediaUrl
            _flashvars = Web.HttpUtility.ParseQueryString(String.Empty)
        End Sub

        Public Sub New(target As Dune, mediaUrl As String, flashvars As NameValueCollection)
            Me.New(target, mediaUrl)
            _flashvars.Add(flashvars)
        End Sub

        ''' <summary>
        ''' Gets the media URL for the requested Flash Lite application.
        ''' </summary>
        Public ReadOnly Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
        End Property

        ''' <summary>
        ''' Gets the collection of Flash Lite startup parameters.
        ''' </summary>
        Public ReadOnly Property Flashvars As NameValueCollection
            Get
                Return _flashvars
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.LaunchMediaUrl)
            query.Add(Constants.StartPlaybackParameters.MediaUrl, "swf://" + MediaUrl)

            Return query
        End Function

        Protected Overrides Function GetQueryString() As String
            If Flashvars.Count > 0 Then
                Dim query As New Text.StringBuilder
                query.Append(MyBase.GetQueryString())
                query.Append(":::")
                query.Append(Flashvars.ToString)
                Return query.ToString
            Else
                Return MyBase.GetQueryString
            End If

        End Function
    End Class

End Namespace
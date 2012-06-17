Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    Public Class LaunchFlashLiteCommand
        Inherits Command

        Private _mediaUrl As String
        Private _flashvars As HttpQuery

        Public Sub New(target As Dune, mediaUrl As String)
            MyBase.New(target)
            _mediaUrl = mediaUrl
            _flashvars = New HttpQuery
        End Sub

        Public Sub New(target As Dune, mediaUrl As String, flashvars As HttpQuery)
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
        Public ReadOnly Property Flashvars As HttpQuery
            Get
                Return _flashvars
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.LaunchMediaUrl)
            query.Add(Constants.StartPlaybackParameterNames.MediaUrl, "swf://" + MediaUrl)

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
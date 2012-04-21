Imports System.Net
Imports System.IO

Namespace Dune.ApiWrappers

    ''' <summary>
    ''' This is the base class for all Dune commands.
    ''' </summary>
    Public MustInherit Class DuneCommand
        Private _target As Dune
        Private _timeout As Nullable(Of Integer)
        Private _requestMethod As RequestMethod
        Private _commandType As String

        Public Sub New(ByRef dune As Dune)
            _target = dune
            _requestMethod = RequestMethod.Get
        End Sub

        ''' <summary>
        ''' Gets the target Dune device.
        ''' </summary>
        Public ReadOnly Property Target As Dune
            Get
                Return _target
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the requested command type as defined by the API.
        ''' </summary>
        ''' <remarks>Do use the constants defined in the <see cref="Constants.Commands"/> class.</remarks>
        Protected Friend Property CommandType As String
            Get
                Return _commandType
            End Get
            Set(value As String)
                _commandType = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the command timeout.
        ''' </summary>
        Public Property Timeout As Nullable(Of Integer)
            Get
                Return _timeout
            End Get
            Set(value As Nullable(Of Integer))
                _timeout = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the base command URL without command parameters.
        ''' </summary>
        Protected ReadOnly Property CommandBase As String
            Get
                Return String.Format("http://{0}:{1}/cgi-bin/do", Target.Address, Target.Port)
            End Get
        End Property

        ''' <summary>
        ''' Gets the command query string.
        ''' </summary>
        Public MustOverride Function GetQueryString() As String

        ''' <summary>
        ''' Gets or sets the request method that should be used.
        ''' </summary>
        Public Property Method As RequestMethod
            Get
                Return _requestMethod
            End Get
            Set(value As RequestMethod)
                _requestMethod = value
            End Set
        End Property

        Public Function GetRequest() As WebRequest
            Dim request As HttpWebRequest
            Dim query As String = GetQueryString()

            If Method = RequestMethod.Get Then
                Dim uri As Uri = New Uri(CommandBase + "?" + query)
                request = HttpWebRequest.Create(uri)
            Else
                query = query.Replace("&", Environment.NewLine + "&")
                Dim postData() As Byte = Text.Encoding.ASCII.GetBytes(query)

                request = HttpWebRequest.Create(CommandBase)
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.ContentLength = postData.Length
                Dim requestStream As Stream = request.GetRequestStream()
                requestStream.Write(postData, 0, postData.Length)
                requestStream.Flush()
                requestStream.Close()
            End If

            request.UserAgent = My.Application.Info.ProductName + " " + My.Application.Info.Version.ToString

            Return DirectCast(request, WebRequest)
        End Function

        Public Enum RequestMethod
            [Get]
            Post
        End Enum

    End Class

End Namespace
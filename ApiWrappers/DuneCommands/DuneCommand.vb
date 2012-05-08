Imports System.Net
Imports System.IO
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This is the base class for all Dune commands.
    ''' </summary>
    Public MustInherit Class DuneCommand
        Private _target As Dune
        Private _timeout As UInteger?
        Private _requestMethod As RequestMethod
        Private _commandType As String
        Private _query As NameValueCollection


        Public Sub New(ByRef dune As Dune)
            _target = dune
            _requestMethod = RequestMethod.Post
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
        Protected Friend ReadOnly Property CommandType As String
            Get
                Return GetQuery.Item("cmd")
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the command timeout.
        ''' </summary>
        Public Property Timeout As UInteger?
            Get
                Return _timeout
            End Get
            Set(value As UInteger?)
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

        Protected MustOverride Function GetQuery() As NameValueCollection

        ''' <summary>
        ''' Gets the command query string.
        ''' </summary>
        Public Function GetQueryString() As String
            Dim request As NameValueCollection = Web.HttpUtility.ParseQueryString(String.Empty)
            request.Add(GetQuery)

            If Timeout > 0 AndAlso Timeout <> 20 Then
                request.Add("timeout", Timeout.Value.ToString)
            End If

            Return request.ToString
        End Function

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
            Dim request As WebRequest
            Dim query As String = GetQueryString()

            If Method = RequestMethod.Get Then
                Dim uri As Uri = New Uri(CommandBase + "?" + query)
                request = WebRequest.Create(uri)
            Else
                query = query.Replace("&", Environment.NewLine + "&")
                Dim postData() As Byte = Text.Encoding.ASCII.GetBytes(query)

                request = WebRequest.Create(CommandBase)
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.ContentLength = postData.Length
                Dim requestStream As Stream = request.GetRequestStream()
                requestStream.Write(postData, 0, postData.Length)
                requestStream.Flush()
                requestStream.Close()
            End If

            DirectCast(request, HttpWebRequest).UserAgent = My.Application.Info.ProductName + Convert.ToChar(47) + My.Application.Info.Version.ToString
            Return DirectCast(request, WebRequest)
        End Function

        Public Enum RequestMethod
            [Get]
            Post
        End Enum

        Public Overrides Function ToString() As String
            Return Me.GetType.Name + " (query string: """ + GetQueryString() + """)."
        End Function

    End Class

End Namespace
Imports System.Net
Imports System.IO
Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This is the base class for all Dune command types.
    ''' </summary>
    Public MustInherit Class Command
        Private _target As Dune
        Private _timeout As Integer?
        Private _httpMethod As String

        Protected Sub New(target As Dune)
            _target = target
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
        ''' Gets the requested command type as defined by the API.
        ''' </summary>
        ''' <remarks>Do use the constants defined in the <see cref="Constants.CommandValues"/> class.</remarks>
        Protected Friend ReadOnly Property CommandType As String
            Get
                Return GetQuery.Item("cmd")
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the command timeout.
        ''' </summary>
        Public Property Timeout As Integer?
            Get
                If Not _timeout.HasValue Then
                    _timeout = Target.Timeout
                End If
                Return _timeout
            End Get
            Set(value As Integer?)
                If value <= 0 Then
                    value = Nothing
                End If
                _timeout = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the base command URL, without command parameters, in the form of: http://address[:port]/cgi-bin/do.
        ''' </summary>
        Protected Function GetCommandBase() As String
            Dim address As New Text.StringBuilder

            If Target.Address IsNot Nothing Then
                address.Append(Target.Address.ToString)
            ElseIf Target.HostName.IsNotNullOrWhiteSpace Then
                address.Append(Target.HostName)
            Else ' default to "Dune".
                address.Append("Dune")
            End If

            If Target.Port <> 80 Then
                address.Append(Target.Port)
            End If

            Return String.Format("http://{0}/cgi-bin/do", address.ToString)
        End Function

        Protected MustOverride Function GetQuery() As HttpQuery

        ''' <summary>
        ''' Gets the command query string.
        ''' </summary>
        Protected Overridable Function GetQueryString() As String
            Dim request As New Networking.HttpQuery

            request.Add(GetQuery)
            request.Add("timeout", Timeout.Value.ToString(Constants.FormatProvider))

            Return request.ToString
        End Function

        ''' <summary>
        ''' Gets or sets the HTTP method that should be used.
        ''' </summary>
        Public Property Method As String
            Get
                If _httpMethod.IsNullOrEmpty Then
                    _httpMethod = WebRequestMethods.Http.Post
                End If
                Return _httpMethod
            End Get
            Set(value As String)
                Select Case value
                    Case WebRequestMethods.Http.Get, WebRequestMethods.Http.Post
                        _httpMethod = value
                    Case Else
                        Throw New ArgumentException(value + " is not a supported HTTP method.")
                End Select
            End Set
        End Property

        ''' <summary>
        ''' Gets a <see cref="WebRequest"/> instance for the current instance of the command.
        ''' </summary>
        Private Function GetRequest() As WebRequest
            Dim request As WebRequest

            Dim requestUri As Uri

            If Method = WebRequestMethods.Http.Get Then
                requestUri = New Uri(GetCommandBase() + "?" + GetQueryString())
            Else ' If Method = WebRequestMethods.Http.Post Then
                requestUri = New Uri(GetCommandBase)
            End If

            request = WebRequest.Create(requestUri)
            request.Method = Method
            request.Timeout = Integer.MaxValue
            SetUserAgent(request)

            If request.Method = WebRequestMethods.Http.Post Then ' write command query to the request stream
                WritePostData(request, GetQueryString)
            End If

            Return request
        End Function

        Private Sub SetUserAgent(request As WebRequest)
            DirectCast(request, HttpWebRequest).UserAgent = String.Concat(My.Application.Info.ProductName, "/", My.Application.Info.Version.ToString)
        End Sub

        ''' <summary>
        ''' Writes the specified query string to the request stream.
        ''' </summary>
        Private Sub WritePostData(request As WebRequest, query As String)
            request.ContentType = "application/x-www-form-urlencoded"

            query = query.Replace("&", Environment.NewLine + "&")
            Dim postData() As Byte = Text.Encoding.ASCII.GetBytes(query)

            request.ContentLength = postData.Length

            Dim requestStream As Stream = request.GetRequestStream()
            requestStream.Write(postData, 0, postData.Length)
            requestStream.Close()
        End Sub

        ''' <summary>Executes a command and processes the command results.</summary>
        ''' <exception cref="WebException">: An error occurred when trying to query the API.</exception>
        Friend Function GetResponse() As WebResponse
            Dim request As WebRequest = GetRequest()
            Return request.GetResponse
        End Function

        Public Function GetResult() As CommandResult
            Return New CommandResult(Me)
        End Function

        Public Overrides Function ToString() As String
            Return MyBase.ToString + " (query string: """ + GetQueryString() + """)."
        End Function

    End Class

End Namespace
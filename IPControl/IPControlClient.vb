#Region "License"
' Copyright 2012-2013 Steven Liekens
' Contact: steven.liekens@gmail.com

' This file is part of DuneApiCodepack.

' DuneApiCodepack is free software: you can redistribute it and/or modify
' it under the terms of the GNU Lesser General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' DuneApiCodepack is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Lesser General Public License for more details.

' You should have received a copy of the GNU Lesser General Public License
' along with DuneApiCodepack.  If not, see <http://www.gnu.org/licenses/>.
#End Region ' License
Imports System.Net.Http

Namespace IPControl

    Public Class IPControlClient
        Inherits HttpClient

        Private Shared Property Instance As IPControlClient

        Public Shared Function GetInstance() As IPControlClient
            If Instance Is Nothing Then
                Instance = New IPControlClient
            End If
            Return Instance
        End Function

        Private Function IsSupportedUri(address As Uri) As Boolean
            Dim isHttp = String.Equals(address.Scheme, Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase)
            Dim isHttps = String.Equals(address.Scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase)

            Return isHttp Or isHttps
        End Function

        Public Async Function IsResourceAvailableAsync(location As Uri) As Task(Of Boolean)
            If Not Me.IsSupportedUri(location) Then
                Throw New ArgumentException("Invalid scheme")
            End If

            Try
                Using request = New HttpRequestMessage(HttpMethod.Head, location)
                    Using response = Await Me.SendAsync(request).ConfigureAwait(False)
                        Return response.IsSuccessStatusCode
                    End Using
                End Using
            Catch ex As HttpRequestException
                Return False
            End Try
        End Function

        Public Async Function ServiceReturnsXmlAsync(address As Uri) As Task(Of Boolean)
            If Not Me.IsSupportedUri(address) Then
                Throw New ArgumentException("Invalid scheme")
            End If
            Dim timeout As New Threading.CancellationTokenSource(1000)

            Try
                Using request As New HttpRequestMessage(HttpMethod.Head, address)
                    Using response = Await Me.SendAsync(request, timeout.Token).ConfigureAwait(False)
                        Return String.Equals(response.Content.Headers.ContentType.MediaType, "text/xml", StringComparison.InvariantCultureIgnoreCase)
                    End Using
                End Using
            Catch
                Return False
            End Try
        End Function

        Public Async Function GetContentHeadersAsync(location As Uri) As Task(Of Headers.HttpContentHeaders)
            If Not Me.IsSupportedUri(location) Then
                Throw New ArgumentException("Invalid scheme")
            End If

            Try
                Using request As New HttpRequestMessage(HttpMethod.Head, location)
                    Using response = Await Me.SendAsync(request).ConfigureAwait(False)
                        Return response.Content.Headers
                    End Using
                End Using
            Catch ex As HttpRequestException
                Return Nothing
            End Try
        End Function

        Public Async Function GetXmlAsync(request As HttpRequestMessage) As Task(Of XDocument)
            Using request
                Using response = Await Me.SendAsync(request).ConfigureAwait(False)
                    response.EnsureSuccessStatusCode()
                    Return XDocument.Load(Await response.Content.ReadAsStreamAsync.ConfigureAwait(False))
                End Using
            End Using
        End Function

    End Class

End Namespace
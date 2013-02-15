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
Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Networking

    Public Class TelnetStateConnected : Inherits TelnetState

        Private Property Client As TcpClient

        Public Sub New(context As TelnetClient, client As TcpClient)
            MyBase.New(context)
            If client Is Nothing OrElse (Not client.Connected) Then
                Throw New ArgumentException("no connection to host", "client")
            End If
            Me.Client = client
        End Sub

        Public Overrides Sub Connect()
            Throw New InvalidOperationException("already connected")
        End Sub

        Public Overrides Function ConnectAsync() As Task
            Throw New InvalidOperationException("already connected")
        End Function

        Public Overrides Sub Disconnect()
            Me.Context.State = New TelnetStateDisconnected(Me.Context)
            Me.Client.Close()
        End Sub

        Public Overrides Property IsConnected As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
                MyBase.IsConnected = value
            End Set
        End Property

        ''' <summary>
        ''' Reads until, but not including, the specified value, or until timeout is reached.
        ''' </summary>
        Public Overrides Async Function ReadUntilAsync(value As String) As Task(Of String)
            Dim text As String = String.Empty
            If Client.Available > 0 Then
                text = Await Task(Of String).Run(AddressOf ReadAsync).ConfigureAwait(False)
                Dim ismatch = text.Contains(value)

                If ismatch Then
                    Return text.Substring(0, text.LastIndexOf(value))
                End If
            End If
            Await Task.Delay(300).ConfigureAwait(False)
            If Client.Available > 0 Then
                Return String.Concat(text, Await Me.ReadUntilAsync(value).ConfigureAwait(False))
            Else
                Return text
            End If
        End Function

        Public Overrides Async Function ReadUntilPatternAsync(pattern As Regex) As Task(Of String)
            Dim text As String = String.Empty
            If Client.Available > 0 Then
                text = Await Task(Of String).Run(AddressOf ReadAsync).ConfigureAwait(False)
                Dim ismatch = pattern.IsMatch(text)

                If ismatch Then
                    Return pattern.Replace(text, String.Empty).Trim
                End If
            End If
            Await Task.Delay(300).ConfigureAwait(False)
            If Client.Available > 0 Then
                Return String.Concat(text, Await Me.ReadUntilPatternAsync(pattern).ConfigureAwait(False))
            Else
                Return text
            End If
        End Function

        Public Async Function ReadAsync() As Task(Of String)
            Dim memory As New MemoryStream()

            Dim text As New StringBuilder
            Dim stream = Client.GetStream

            Do
                If Not stream.DataAvailable Then
                    Exit Do
                End If
                Try
                    Dim buffer(1023) As Byte
                    Dim count = Await stream.ReadAsync(buffer, 0, Math.Min(buffer.Length, Client.Available)).ConfigureAwait(False)

                    For i = 0 To count - 1
                        If (buffer(i) And &H80) <> 0 Then
                            i += 2
                        Else
                            text.Append(Encoding.ASCII.GetString(buffer, i, 1))
                        End If
                    Next
                Catch ex As SocketException
                    Disconnect()
                    Throw
                End Try
            Loop

            Return text.ToString
        End Function

        ''' <summary>
        ''' Sends the specified command.
        ''' </summary>
        Public Sub Write(command As String)
            Dim sendData() As Byte = Encoding.ASCII.GetBytes(command + Environment.NewLine)
            Client.GetStream.Write(sendData, 0, sendData.Length)
        End Sub

        Public Async Function WriteAsync(value As String) As Task
            Dim sendData() = Encoding.ASCII.GetBytes(value + Environment.NewLine)
            Await Client.GetStream.WriteAsync(sendData, 0, sendData.Length).ConfigureAwait(False)
        End Function

        Public Overrides Async Function ExecuteCommandAsync(command As String) As Task(Of String)
            Await Me.WriteAsync(command).ConfigureAwait(False)
            Dim response As String = Await Me.ReadUntilPatternAsync(TelnetClient.PromptRegex).ConfigureAwait(False)

            If response.Length > command.Length Then
                Return response.Substring(command.Length).Trim
            Else
                Return response
            End If
        End Function

    End Class

End Namespace


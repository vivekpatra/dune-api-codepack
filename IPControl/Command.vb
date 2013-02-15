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
Imports SL.DuneApiCodePack.Networking
Imports SL.DuneApiCodePack.DuneUtilities
Imports System.Net.Http

Namespace IPControl

    ''' <summary>
    ''' This is the base class for all Dune command types.
    ''' </summary>
    Public MustInherit Class Command : Implements IIPCommand

        Protected ReadOnly parameters As HttpQuery

        Private _command As CommandValue
        Private _timeout As TimeSpan
        Private Shared _defaultTimeout As TimeSpan

        Shared Sub New()
            IPControl.Command.DefaultTimeout = TimeSpan.FromSeconds(20)
        End Sub

        Protected Sub New(command As CommandValue)
            Me.parameters = New HttpQuery

            Me.Command = command
            Me.Timeout = IPControl.Command.DefaultTimeout
        End Sub

        Public MustOverride ReadOnly Property RequiredVersion As Version Implements IIPCommand.RequiredVersion

        ''' <summary>
        ''' Gets or sets the command value.
        ''' </summary>
        Protected Property Command As CommandValue Implements IIPCommand.Command
            Get
                Return _command
            End Get
            Set(value As CommandValue)
                _command = value
                If value Is Nothing Then
                    Me.parameters.Remove(Input.Command)
                Else
                    Me.parameters.Item(Input.Command) = value.Value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the command timeout.
        ''' </summary>
        Public Property Timeout As TimeSpan Implements IIPCommand.Timeout
            Get
                Return _timeout
            End Get
            Set(value As TimeSpan)
                If (value = Nothing) OrElse (value.TotalSeconds = 20) Then
                    _timeout = TimeSpan.FromSeconds(20)
                    Me.parameters.Remove(Input.Timeout)
                ElseIf value < TimeSpan.FromSeconds(1) Then
                    Throw New ArgumentOutOfRangeException("value", "Minimum timeout is 1 second.")
                Else
                    Me.parameters.Item(Input.Timeout) = value.RoundToSecond.TotalSeconds.ToString
                End If
            End Set
        End Property

        Public Shared Property DefaultTimeout As TimeSpan
            Get
                Return IPControl.Command._defaultTimeout
            End Get
            Set(value As TimeSpan)
                If (value = Nothing) OrElse (value.TotalSeconds = 20) Then
                    IPControl.Command._defaultTimeout = TimeSpan.FromSeconds(20)
                ElseIf value < TimeSpan.FromSeconds(1) Then
                    Throw New ArgumentOutOfRangeException("value", "Minimum timeout is 1 second.")
                Else
                    IPControl.Command._defaultTimeout = value.RoundToSecond
                End If
            End Set
        End Property

        Private Function CreateGetRequestMessage(target As Dune, baseAddress As Uri) As HttpRequestMessage
            Dim builder As New UriBuilder(baseAddress)
            builder.Query = Me.GetQuery(target).ToString
            Return New HttpRequestMessage(HttpMethod.Get, builder.Uri)
        End Function

        Private Function CreatePostRequestMessage(target As Dune, baseAddress As Uri) As HttpRequestMessage
            Return New HttpRequestMessage(HttpMethod.Post, baseAddress) With {.Content = New FormUrlEncodedContent(Me.GetQuery(target))}
        End Function

        Protected Function GetRequestMessage(target As Dune, method As HttpMethod, baseAddress As Uri) As HttpRequestMessage Implements IIPCommand.GetRequestMessage
            Select Case method
                Case HttpMethod.Get
                    Return CreateGetRequestMessage(target, baseAddress)
                Case HttpMethod.Post
                    Return CreatePostRequestMessage(target, baseAddress)
                Case Else
                    Throw New ArgumentException("Invalid HTTP method", "method")
            End Select
        End Function

        Public Overridable Function GetRequestMessage(target As Dune) As HttpRequestMessage Implements IIPCommand.GetRequestMessage
            Return Me.GetRequestMessage(target, HttpMethod.Get, target.GetBaseAddress.Uri)
        End Function

        ''' <summary>
        ''' Gets the name=value pairs needed to carry out the command instructions.
        ''' </summary>
        Protected Overridable Function GetQuery(target As Dune) As IDictionary(Of String, String) Implements IIPCommand.GetQuery
            Return New HttpQuery(parameters)
        End Function

        Public Overridable Function CanExecute(target As Dune) As Boolean Implements IIPCommand.CanExecute
            Return target.IsConnected AndAlso (target.ProtocolVersion >= Me.RequiredVersion)
        End Function

        Overridable Function GetIllegalStateReason(target As Dune) As String
            Return "command is not expected in current player state"
        End Function

        Public Function Execute(target As Dune) As IIPCommandResult Implements IIPCommand.Execute
            Try
                Return Me.ExecuteAsync(target).Result
            Catch ex As AggregateException
                Throw ex.InnerException
            End Try
        End Function

        Public Async Function ExecuteAsync(target As Dune) As Task(Of IIPCommandResult) Implements IIPCommand.ExecuteAsync
            If Not Me.CanExecute(target) Then
                Dim msg As String
                Select Case False
                    Case target.IsConnected
                        msg = "No connection to host"
                    Case target.ProtocolVersion >= Me.RequiredVersion
                        msg = "Host requires a firmware update"
                    Case Else
                        msg = GetIllegalStateReason(target)
                End Select
                Throw New CommandException(ErrorKind.IllegalState, msg)
            End If
            Return (Await Me.GetCommandResultAsync(target).ConfigureAwait(False)).EnsureSuccessStatusCode
        End Function

        Public Function TryExecute(target As Dune) As IIPCommandResult Implements IIPCommand.TryExecute
            Try
                Return Me.GetCommandResultAsync(target).Result
            Catch ex As AggregateException
                Throw ex.InnerException
            End Try
        End Function

        Public Async Function TryExecuteAsync(target As Dune) As Task(Of IIPCommandResult) Implements IIPCommand.TryExecuteAsync
            Return Await Me.GetCommandResultAsync(target).ConfigureAwait(False)
        End Function

        Public Shared ReadOnly Property Factory As CommandFactory
            Get
                Return CommandFactory.GetInstance
            End Get
        End Property

        Private Async Function GetResponseMessageAsync(target As Dune) As Task(Of XDocument)
            Try
                Using request = Me.GetRequestMessage(target)
                    Return Await IPControlClient.GetInstance().GetXmlAsync(request).ConfigureAwait(False)
                End Using
            Catch ex As Xml.XmlException
                Throw New CommandException(ErrorKind.InternalError, String.Empty, ex)
            End Try
        End Function

        Protected MustOverride Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult

        Public Overridable Async Function GetCommandResultAsync(target As Dune) As Task(Of IIPCommandResult)
            Dim output = Await Me.GetResponseMessageAsync(target).ConfigureAwait(False)
            Return InitializeCommandResult(output, DateTimeOffset.UtcNow)
        End Function

    End Class

End Namespace
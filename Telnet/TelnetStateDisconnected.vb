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
Imports System.Text.RegularExpressions

Namespace Networking

    Public Class TelnetStateDisconnected : Inherits TelnetState

        Public Sub New(context As TelnetClient)
            MyBase.New(context)
        End Sub

        Public Overrides Sub Connect()
            Dim client As New TcpClient
            client.Connect(Me.Context.EndPoint)
            Me.Context.State = New TelnetStateConnected(Me.Context, client)
            Dim consumePrompt = Me.Context.State.ReadUntilPatternAsync(TelnetClient.LogOnRegex).Result
            Dim logOn = Me.Context.State.ExecuteCommandAsync("root").Result
        End Sub

        Public Overrides Async Function ConnectAsync() As Task
            Dim client As New TcpClient
            Await client.ConnectAsync(Me.Context.EndPoint.Address, Me.Context.EndPoint.Port).ConfigureAwait(False)
            Me.Context.State = New TelnetStateConnected(Me.Context, client)
            Dim consumePrompt = Await Me.Context.State.ReadUntilPatternAsync(TelnetClient.LogOnRegex).ConfigureAwait(False)
            Dim logOn = Await Me.Context.State.ExecuteCommandAsync("root").ConfigureAwait(False)
        End Function

        Public Overrides Sub Disconnect()
            Throw New InvalidOperationException("already disconnected")
        End Sub

        Public Overrides Function ReadUntilAsync(value As String) As Task(Of String)
            Throw New InvalidOperationException("no connection to host")
        End Function

        Public Overrides Function ReadUntilPatternAsync(pattern As Regex) As Task(Of String)
            Throw New InvalidOperationException("no connection to host")
        End Function

        Public Overrides Function ExecuteCommandAsync(command As String) As Task(Of String)
            Throw New InvalidOperationException("no connection to host")
        End Function

    End Class

End Namespace


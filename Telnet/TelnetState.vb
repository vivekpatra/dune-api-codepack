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
Imports System.Net
Imports System.Text.RegularExpressions

Namespace Networking

    Public MustInherit Class TelnetState

        Private _context As TelnetClient

        Protected Property Context As TelnetClient
            Get
                Return _context
            End Get
            Private Set(value As TelnetClient)
                If value IsNot Nothing Then
                    _context = value
                End If
            End Set
        End Property

        Public Sub New(context As TelnetClient)
            If context Is Nothing Then
                Throw New ArgumentNullException("context")
            End If
            Me.Context = context
        End Sub

        Public Overridable Property IsConnected As Boolean
            Get
                Return False
            End Get
            Set(value As Boolean)
                If value Then
                    Connect()
                Else
                    Disconnect()
                End If
            End Set
        End Property

        Public MustOverride Sub Connect()
        Public MustOverride Function ConnectAsync() As task

        Public MustOverride Sub Disconnect()


        Public MustOverride Function ReadUntilAsync(value As String) As Task(Of String)
        Public MustOverride Function ReadUntilPatternAsync(pattern As Regex) As Task(Of String)
        Public MustOverride Function ExecuteCommandAsync(command As String) As Task(Of String)



    End Class

End Namespace


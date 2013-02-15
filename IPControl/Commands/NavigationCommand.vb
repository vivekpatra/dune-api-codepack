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

Namespace IPControl

    ''' <summary>This command can be used to navigate menus.</summary>
    ''' <remarks>This command is context sensitive, meaning:
    ''' If the target device is playing a DVD, it will produce a dvd_navigation command.
    ''' Likewise, it will produce a bluray_navigation command if a Blu-ray is playing.
    ''' In all other cases, a remote control button will be emulated.</remarks>
    Public Class NavigationCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(1, 0)
        Private _action As NavigationAction

        Public Sub New(action As NavigationAction)
            MyBase.New(Nothing)
            _action = action
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the action that needs to be performed.
        ''' </summary>
        Public Property Action As NavigationAction
            Get
                Return _action
            End Get
            Set(value As NavigationAction)
                _action = value
            End Set
        End Property

        Protected Overrides Function GetQuery(target As Dune) As IDictionary(Of String, String)
            Dim query As HttpQuery = DirectCast(MyBase.GetQuery(target), HttpQuery)

            Dim cmd As CommandValue

            Select Case target.PlayerState
                Case PlayerState.DvdPlayback : cmd = CommandValue.DvdNavigation
                Case PlayerState.BlurayPlayback : cmd = CommandValue.BlurayNavigation
                Case Else : cmd = CommandValue.IrCode
            End Select

            query.Item(Input.Command) = cmd.Value

            If cmd = CommandValue.IrCode Then
                query.Item(Input.IRCode) = New RemoteControlKey(Action).Value
            Else
                query.Item(Input.Action) = Me.Action.Value
            End If

            Return query
        End Function

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New StatusCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
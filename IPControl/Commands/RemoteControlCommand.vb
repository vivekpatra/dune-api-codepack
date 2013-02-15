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

    ''' <summary>This command is used to emulate a button press on the remote control.</summary>
    Public Class RemoteControlCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(1, 0)
        Private _button As RemoteControlKey

        ''' <param name="button">The button to send.</param>
        Public Sub New(button As RemoteControlKey)
            MyBase.New(CommandValue.IrCode)
            Me.Button = button
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        Public Property Button As RemoteControlKey
            Get
                Return _button
            End Get
            Set(value As RemoteControlKey)
                _button = value

                If value IsNot Nothing Then
                    Me.parameters.Item(Input.IRCode) = value.Value
                Else
                    Me.parameters.Remove(Input.IRCode)
                End If
            End Set
        End Property

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New StatusCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
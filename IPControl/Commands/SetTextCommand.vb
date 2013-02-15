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

    ''' <summary>
    ''' This command is used to set text to an input box.
    ''' </summary>
    Public Class SetTextCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(3, 0)
        Private _text As String

        Public Sub New()
            MyBase.New(CommandValue.SetText)
            Me.Text = SetTextCommand.DefaultText
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        ''' <param name="text">The input text.</param>
        Public Sub New(text As String)
            Me.New()
            Me.Text = text
        End Sub

        ''' <summary>
        ''' Gets or sets the specified text.
        ''' </summary>
        Public Property Text As String
            Get
                Return _text
            End Get
            Set(value As String)
                _text = Text
                Me.parameters.Item(Input.Text) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the default text.
        ''' </summary>
        Public Shared Property DefaultText As String = String.Empty

        Public Overrides Function CanExecute(target As Dune) As Boolean
            If MyBase.CanExecute(target) Then
                Return target.IsTextFieldActive.GetValueOrDefault(False)
            End If
            Return False
        End Function

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New TextCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
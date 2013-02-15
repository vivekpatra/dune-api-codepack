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
Imports System.Drawing

Namespace IPControl

    Public Class SetPlaybackClipZoomCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(3, 0)
        Private Property Display As Display

        Public Sub New(display As Size)
            MyBase.New(CommandValue.SetPlaybackState)
            Me.Display = New Display(display)
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the clip rectangle's position.
        ''' </summary>
        Public Property ClipRectanglePosition As Point
            Get
                Return Display.Position
            End Get
            Set(value As Point)
                Display.Position = value
                Me.parameters.Item(Input.ClipRectangleLeft) = Me.ClipRectanglePosition.X.ToString
                Me.parameters.Item(Input.ClipRectangleTop) = Me.ClipRectanglePosition.Y.ToString
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the clip rectangle's size.
        ''' </summary>
        Public Property ClipRectangleSize As Size
            Get
                Return Display.Size
            End Get
            Set(value As Size)
                Display.Size = value
                Me.parameters.Item(Input.ClipRectangleWidth) = Me.ClipRectangleSize.Width.ToString
                Me.parameters.Item(Input.ClipRectangleHeight) = Me.ClipRectangleSize.Height.ToString
            End Set
        End Property

        Public Overrides Function CanExecute(target As Dune) As Boolean
            If MyBase.CanExecute(target) Then
                Return target.PlayerState.IsPlaybackState
            End If
            Return False
        End Function

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New StatusCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
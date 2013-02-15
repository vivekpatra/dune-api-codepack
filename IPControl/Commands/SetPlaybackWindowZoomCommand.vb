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

    ''' <summary>This command is used to change the playback window zoom (size; position).</summary>
    Public Class SetPlaybackWindowZoomCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(3, 0)

        Public Sub New(display As Size)
            MyBase.New(CommandValue.SetPlaybackState)
            Me.display = New Display(display)
            Me.WindowFullscreen = True
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        Private display As Display

        ''' <summary>
        ''' Gets whether to set the video output to fullscreen.
        ''' </summary>
        Public Property WindowFullscreen As Boolean
            Get
                Select Case False
                    Case Object.Equals(display.Position, display.Min)
                        Return False
                    Case Object.Equals(display.Size, New Size(display.Max.X, display.Max.Y))
                        Return False
                End Select
                Return True
            End Get
            Set(value As Boolean)
                If value = True Then
                    If Not WindowFullscreen Then
                        Me.WindowRectanglePosition = display.Min
                        Me.WindowRectangleSize = New Size(display.Max.X, display.Max.Y)
                    End If
                Else
                    If WindowFullscreen Then
                        Me.WindowRectanglePosition = New Point(display.Max.X \ 2, display.Max.Y \ 2)
                        Me.WindowRectangleSize = New Size(0, 0)
                    End If
                End If
                SetParameters()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the window's offset.
        ''' </summary>
        Public Property WindowRectanglePosition As Point
            Get
                Return display.Position
            End Get
            Set(value As Point)
                display.Position = value
                SetParameters()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the window's size.
        ''' </summary>
        Public Property WindowRectangleSize As Size
            Get
                Return display.Size
            End Get
            Set(value As Size)
                display.Size = value
                SetParameters()
            End Set
        End Property

        Private Sub SetParameters()
            Me.parameters.Item(Input.WindowFullscreen) = Me.WindowFullscreen.ToNumberString
            If Me.WindowFullscreen Then
                With Me.parameters
                    .Remove(Input.WindowRectangleLeft)
                    .Remove(Input.WindowRectangleTop)
                    .Remove(Input.WindowRectangleWidth)
                    .Remove(Input.WindowRectangleHeight)
                End With
            Else
                With Me.parameters
                    .Item(Input.WindowRectangleLeft) = Me.WindowRectanglePosition.X.ToString
                    .Item(Input.WindowRectangleTop) = Me.WindowRectanglePosition.Y.ToString
                    .Item(Input.WindowRectangleWidth) = Me.WindowRectangleSize.Width.ToString
                    .Item(Input.WindowRectangleHeight) = Me.WindowRectangleSize.Height.ToString
                End With
            End If
        End Sub

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
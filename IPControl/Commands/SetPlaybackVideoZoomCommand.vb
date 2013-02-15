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
    <Obsolete("For protocol version 3 and up, use SetPlaybackWindowZoomCommand instead.")>
    Public Class SetPlaybackVideoZoomCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(2, 0)

        Public Sub New(display As Size)
            MyBase.New(CommandValue.SetPlaybackState)
            Me.display = New Display(display)
            Me.VideoFullscreen = True
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
        Public Property VideoFullscreen As Boolean
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
                    If Not VideoFullscreen Then
                        Me.VideoRectanglePosition = display.Min
                        Me.VideoRectangleSize = New Size(display.Max.X, display.Max.Y)
                    End If
                Else
                    If VideoFullscreen Then
                        Me.VideoRectanglePosition = New Point(display.Max.X \ 2, display.Max.Y \ 2)
                        Me.VideoRectangleSize = New Size(0, 0)
                    End If
                End If
                SetParameters()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the window's offset.
        ''' </summary>
        Public Property VideoRectanglePosition As Point
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
        Public Property VideoRectangleSize As Size
            Get
                Return display.Size
            End Get
            Set(value As Size)
                display.Size = value
                SetParameters()
            End Set
        End Property

        Private Sub SetParameters()
            Me.parameters.Item(Input.VideoFullScreen) = Me.VideoFullscreen.ToNumberString
            If Me.VideoFullscreen Then
                With Me.parameters
                    .Remove(Input.VideoLeft)
                    .Remove(Input.VideoTop)
                    .Remove(Input.VideoWidth)
                    .Remove(Input.VideoHeight)
                End With
            Else
                With Me.parameters
                    .Item(Input.VideoLeft) = Me.VideoRectanglePosition.X.ToString
                    .Item(Input.VideoTop) = Me.VideoRectanglePosition.Y.ToString
                    .Item(Input.VideoWidth) = Me.VideoRectangleSize.Width.ToString
                    .Item(Input.VideoHeight) = Me.VideoRectangleSize.Height.ToString
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
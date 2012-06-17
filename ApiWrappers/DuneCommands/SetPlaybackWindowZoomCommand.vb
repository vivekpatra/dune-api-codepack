#Region "License"
' Copyright 2012 Steven Liekens
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

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to change the playback window zoom (width; height; position).</summary>
    Public Class SetPlaybackWindowZoomCommand
        Inherits Command

        Private _windowFullscreen As Boolean
        Private _windowRectangleHorizontalOffset As Short
        Private _windowRectangleVerticalOffset As Short
        Private _windowRectangleWidth As Short
        Private _windowRectangleHeight As Short

        ''' <param name="target">The target device.</param>
        ''' <param name="windowRectangleHorizontalOffset">The requested horizontal offset.</param>
        ''' <param name="windowRectangleVerticalOffset">The requested vertical offset.</param>
        ''' <param name="windowRectangleWidth">The requested width.</param>
        ''' <param name="windowRectangleHeight">The requested height.</param>
        ''' <remarks></remarks>
        Public Sub New(target As Dune, windowRectangleHorizontalOffset As Short?, windowRectangleVerticalOffset As Short?, windowRectangleWidth As Short?, windowRectangleHeight As Short?)
            MyBase.New(target)

            If windowRectangleHorizontalOffset.HasValue Then ' use the specified value
                _windowRectangleHorizontalOffset = windowRectangleHorizontalOffset.Value
            ElseIf target.PlaybackWindowRectangleHorizontalOffset.HasValue Then ' use the current value
                _windowRectangleHorizontalOffset = target.PlaybackWindowRectangleHorizontalOffset.Value
            Else ' default to 0
                _windowRectangleHorizontalOffset = 0
            End If

            If windowRectangleVerticalOffset.HasValue Then ' use the specified value
                _windowRectangleVerticalOffset = windowRectangleVerticalOffset.Value
            ElseIf target.PlaybackWindowRectangleVerticalOffset.HasValue Then ' use the current value
                _windowRectangleVerticalOffset = target.PlaybackWindowRectangleVerticalOffset.Value
            Else ' default to 0
                _windowRectangleVerticalOffset = 0
            End If

            If windowRectangleWidth.HasValue Then ' use the specified value
                _windowRectangleWidth = windowRectangleWidth.Value
            ElseIf target.PlaybackWindowRectangleWidth.HasValue Then ' use the current value
                _windowRectangleWidth = target.PlaybackWindowRectangleWidth.Value
            ElseIf target.OnScreenDisplayWidth.HasValue Then ' use the maximum value
                _windowRectangleWidth = target.OnScreenDisplayWidth.Value
            Else ' default to 0
                _windowRectangleWidth = 0
            End If

            If windowRectangleHeight.HasValue Then ' use the specified value
                _windowRectangleHeight = windowRectangleHeight.Value
            ElseIf target.PlaybackWindowRectangleHeight.HasValue Then ' use the current value
                _windowRectangleHeight = target.PlaybackWindowRectangleHeight.Value
            ElseIf target.OnScreenDisplayHeight.HasValue Then ' use the maximum value
                _windowRectangleHeight = target.OnScreenDisplayHeight.Value
            Else ' default to 0
                _windowRectangleHeight = 0
            End If
        End Sub

        Public Sub New(dune As Dune, fullscreen As Boolean)
            Me.New(dune, dune.PlaybackWindowRectangleHorizontalOffset, dune.PlaybackWindowRectangleVerticalOffset, dune.PlaybackWindowRectangleWidth, dune.PlaybackWindowRectangleHeight)
            _windowFullscreen = fullscreen
            If fullscreen.IsFalse Then
                _windowRectangleHeight = 0
                _windowRectangleWidth = 0
            End If
        End Sub

        ''' <summary>
        ''' Gets whether to set the video output to fullscreen.
        ''' </summary>
        Public ReadOnly Property VideoFullscreen As Boolean
            Get
                Return _windowFullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested horizontal offset.
        ''' </summary>
        Public ReadOnly Property WindowRectangleHorizontalOffset As Short
            Get
                Return _windowRectangleHorizontalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested vertical offset.
        ''' </summary>
        Public ReadOnly Property WindowRectangleVerticalOffset As Short
            Get
                Return _windowRectangleVerticalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested width.
        ''' </summary>
        Public ReadOnly Property WindowRectangleWidth As Short
            Get
                Return _windowRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested height.
        ''' </summary>
        Public ReadOnly Property WindowRectangleHeight As Short
            Get
                Return _windowRectangleHeight
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.SetPlaybackState)

            If _windowFullscreen.IsTrue Then
                If Target.ProtocolVersion.Major = 2 Then ' use the parameter name for version 2
                    query.Add(Constants.SetPlaybackStateParameterNames.VideoFullscreen, True.GetNumberString)
                ElseIf Target.ProtocolVersion.Major >= 3 Then ' use the new parameter name
                    query.Add(Constants.SetPlaybackStateParameterNames.WindowFullscreen, True.GetNumberString)
                End If
            Else
                If Target.ProtocolVersion.Major = 2 Then ' use the parameter names for version 2
                    query.Add(Constants.SetPlaybackStateParameterNames.VideoFullscreen, False.GetNumberString)
                    query.Add(Constants.SetPlaybackStateParameterNames.VideoHorizontalOffset, WindowRectangleHorizontalOffset.ToString)
                    query.Add(Constants.SetPlaybackStateParameterNames.VideoVerticalOffset, WindowRectangleVerticalOffset.ToString)
                    query.Add(Constants.SetPlaybackStateParameterNames.VideoWidth, WindowRectangleWidth.ToString)
                    query.Add(Constants.SetPlaybackStateParameterNames.VideoHeight, WindowRectangleHeight.ToString)
                ElseIf Target.ProtocolVersion.Major >= 3 Then ' use the new parameter names
                    query.Add(Constants.SetPlaybackStateParameterNames.WindowFullscreen, False.GetNumberString)
                    query.Add(Constants.SetPlaybackStateParameterNames.WindowRectangleHorizontalOffset, WindowRectangleHorizontalOffset.ToString)
                    query.Add(Constants.SetPlaybackStateParameterNames.WindowRectangleVerticalOffset, WindowRectangleVerticalOffset.ToString)
                    query.Add(Constants.SetPlaybackStateParameterNames.WindowRectangleWidth, WindowRectangleWidth.ToString)
                    query.Add(Constants.SetPlaybackStateParameterNames.WindowRectangleHeight, WindowRectangleHeight.ToString)
                End If
            End If

            Return query
        End Function
    End Class

End Namespace
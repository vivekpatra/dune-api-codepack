Imports System.Text
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to change the playback window zoom (width; height; position).</summary>
    Public Class SetPlaybackWindowZoomCommand
        Inherits Command

        Private _windowFullscreen As Boolean?
        Private _windowRectangleHorizontalOffset As UShort?
        Private _windowRectangleVerticalOffset As UShort?
        Private _windowRectangleWidth As UShort?
        Private _windowRectangleHeight As UShort?

        ''' <param name="target">The target device.</param>
        ''' <param name="windowRectangleHorizontalOffset">The requested horizontal offset.</param>
        ''' <param name="windowRectangleVerticalOffset">The requested vertical offset.</param>
        ''' <param name="windowRectangleWidth">The requested width.</param>
        ''' <param name="windowRectangleHeight">The requested height.</param>
        ''' <remarks></remarks>
        Public Sub New(target As Dune, windowRectangleHorizontalOffset As UShort?, windowRectangleVerticalOffset As UShort?, windowRectangleWidth As UShort?, windowRectangleHeight As UShort?)
            MyBase.New(target)

            _windowRectangleHorizontalOffset = CUShort(IIf(windowRectangleHorizontalOffset.HasValue, windowRectangleHorizontalOffset, 0))
            _windowRectangleVerticalOffset = CUShort(IIf(windowRectangleVerticalOffset.HasValue, windowRectangleVerticalOffset, 0))
            _windowRectangleWidth = CUShort(IIf(windowRectangleWidth.HasValue, windowRectangleWidth, target.OnScreenDisplayWidth))
            _windowRectangleHeight = CUShort(IIf(windowRectangleHeight.HasValue, windowRectangleHeight, target.OnScreenDisplayHeight))
        End Sub

        Public Sub New(dune As Dune, fullscreen As Boolean)
            Me.New(dune, dune.PlaybackWindowRectangleHorizontalOffset, dune.PlaybackWindowRectangleVerticalOffset, dune.PlaybackWindowRectangleWidth, dune.PlaybackWindowRectangleHeight)
            _windowFullscreen = fullscreen
        End Sub

        ''' <summary>
        ''' Gets whether to set the video output to fullscreen.
        ''' </summary>
        Public ReadOnly Property VideoFullscreen As Boolean?
            Get
                Return _windowFullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested horizontal offset.
        ''' </summary>
        Public ReadOnly Property WindowRectangleHorizontalOffset As UShort?
            Get
                Return _windowRectangleHorizontalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested vertical offset.
        ''' </summary>
        Public ReadOnly Property WindowRectangleVerticalOffset As UShort?
            Get
                Return _windowRectangleVerticalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested width.
        ''' </summary>
        Public ReadOnly Property WindowRectangleWidth As UShort?
            Get
                Return _windowRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested height.
        ''' </summary>
        Public ReadOnly Property WindowRectangleHeight As UShort?
            Get
                Return _windowRectangleHeight
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection

            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.SetPlaybackState)

            If _windowFullscreen.HasValue AndAlso _windowFullscreen.Value.IsTrue Then
                If Target.ProtocolVersion.Major = 2 Then
                    query.Add(Constants.SetPlaybackStateParameters.VideoFullscreen, "1")
                ElseIf Target.ProtocolVersion.Major >= 3 Then
                    query.Add(Constants.SetPlaybackStateParameters.WindowFullscreen, "1")
                End If
            Else
                If Target.ProtocolVersion.Major = 2 Then
                    query.Add(Constants.SetPlaybackStateParameters.VideoFullscreen, "0")
                    query.Add(Constants.SetPlaybackStateParameters.VideoHorizontalOffset, WindowRectangleHorizontalOffset.Value.ToString)
                    query.Add(Constants.SetPlaybackStateParameters.VideoVerticalOffset, WindowRectangleVerticalOffset.Value.ToString)
                    query.Add(Constants.SetPlaybackStateParameters.VideoWidth, WindowRectangleWidth.Value.ToString)
                    query.Add(Constants.SetPlaybackStateParameters.VideoHeight, WindowRectangleHeight.Value.ToString)
                ElseIf Target.ProtocolVersion.Major >= 3 Then
                    query.Add(Constants.SetPlaybackStateParameters.WindowFullscreen, "0")
                    query.Add(Constants.SetPlaybackStateParameters.WindowRectangleHorizontalOffset, WindowRectangleHorizontalOffset.Value.ToString)
                    query.Add(Constants.SetPlaybackStateParameters.WindowRectangleVerticalOffset, WindowRectangleVerticalOffset.Value.ToString)
                    query.Add(Constants.SetPlaybackStateParameters.WindowRectangleWidth, WindowRectangleWidth.Value.ToString)
                    query.Add(Constants.SetPlaybackStateParameters.WindowRectangleHeight, WindowRectangleHeight.Value.ToString)
                End If
            End If

            Return query
        End Function
    End Class

End Namespace
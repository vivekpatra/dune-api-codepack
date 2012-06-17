Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    Public Class SetPlaybackClipZoomCommand
        Inherits Command

        Private _clipRectangleHorizontalOffset As Short
        Private _clipRectangleVerticalOffset As Short
        Private _clipRectangleWidth As Short
        Private _clipRectangleHeight As Short


        Public Sub New(target As Dune, clipRectangleHorizontalOffset As Short?, clipRectangleVerticalOffset As Short?, clipRectangleWidth As Short?, clipRectangleHeight As Short?)
            MyBase.New(target)

            If clipRectangleHorizontalOffset.HasValue Then ' use the specified value
                _clipRectangleHorizontalOffset = clipRectangleHorizontalOffset.Value
            ElseIf target.PlaybackClipRectangleHorizontalOffset.HasValue Then ' use the current value
                _clipRectangleHorizontalOffset = target.PlaybackClipRectangleHorizontalOffset.Value
            Else ' default to 0
                _clipRectangleHorizontalOffset = 0
            End If

            If clipRectangleVerticalOffset.HasValue Then ' use the specified value
                _clipRectangleVerticalOffset = clipRectangleVerticalOffset.Value
            ElseIf target.PlaybackClipRectangleVerticalOffset.HasValue Then ' use the current value
                _clipRectangleVerticalOffset = target.PlaybackClipRectangleVerticalOffset.Value
            Else ' default to 0
                _clipRectangleVerticalOffset = 0
            End If

            If clipRectangleWidth.HasValue Then ' use the specified value
                _clipRectangleWidth = clipRectangleWidth.Value
            ElseIf target.PlaybackClipRectangleWidth.HasValue Then ' use the current value
                _clipRectangleWidth = target.PlaybackClipRectangleWidth.Value
            ElseIf target.OnScreenDisplayWidth.HasValue Then ' use the maximum value
                _clipRectangleWidth = target.OnScreenDisplayWidth.Value
            Else ' default to 0
                _clipRectangleWidth = 0
            End If

            If clipRectangleHeight.HasValue Then ' use the specified value
                _clipRectangleHeight = clipRectangleHeight.Value
            ElseIf target.PlaybackClipRectangleHeight.HasValue Then ' use the current value
                _clipRectangleHeight = target.PlaybackClipRectangleHeight.Value
            ElseIf target.OnScreenDisplayHeight.HasValue Then ' use the maximum value
                _clipRectangleHeight = target.OnScreenDisplayHeight.Value
            Else ' default to 0
                _clipRectangleHeight = 0
            End If
        End Sub

        ''' <summary>
        ''' Gets the requested horizontal offset.
        ''' </summary>
        Public ReadOnly Property ClipRectangleHorizontalOffset As Short
            Get
                Return _clipRectangleHorizontalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested vertical offset.
        ''' </summary>
        Public ReadOnly Property ClipRectangleVerticalOffset As Short
            Get
                Return _clipRectangleVerticalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested width.
        ''' </summary>
        Public ReadOnly Property ClipRectangleWidth As Short
            Get
                Return _clipRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested height.
        ''' </summary>
        Public ReadOnly Property ClipRectangleHeight As Short
            Get
                Return _clipRectangleHeight
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.SetPlaybackState)

            query.Add(Constants.SetPlaybackStateParameterNames.ClipRectangleHorizontalOffset, ClipRectangleHorizontalOffset.ToString)
            query.Add(Constants.SetPlaybackStateParameterNames.ClipRectangleVerticalOffset, ClipRectangleVerticalOffset.ToString)
            query.Add(Constants.SetPlaybackStateParameterNames.ClipRectangleWidth, ClipRectangleWidth.ToString)
            query.Add(Constants.SetPlaybackStateParameterNames.ClipRectangleHeight, ClipRectangleHeight.ToString)

            Return query
        End Function
    End Class

End Namespace
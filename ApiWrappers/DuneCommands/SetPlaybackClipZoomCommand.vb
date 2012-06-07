Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    Public Class SetPlaybackClipZoomCommand
        Inherits Command

        Private _clipRectangleHorizontalOffset As UShort?
        Private _clipRectangleVerticalOffset As UShort?
        Private _clipRectangleWidth As UShort?
        Private _clipRectangleHeight As UShort?


        Public Sub New(target As Dune, clipRectangleHorizontalOffset As UShort?, clipRectangleVerticalOffset As UShort?, clipRectangleWidth As UShort?, clipRectangleHeight As UShort?)
            MyBase.New(target)

            _clipRectangleHorizontalOffset = CUShort(IIf(clipRectangleHorizontalOffset.HasValue, clipRectangleHorizontalOffset, 0))
            _clipRectangleVerticalOffset = CUShort(IIf(clipRectangleVerticalOffset.HasValue, clipRectangleVerticalOffset, 0))
            _clipRectangleWidth = CUShort(IIf(clipRectangleWidth.HasValue, clipRectangleWidth, target.OnScreenDisplayWidth))
            _clipRectangleHeight = CUShort(IIf(clipRectangleHeight.HasValue, clipRectangleHeight, target.OnScreenDisplayHeight))
        End Sub

        ''' <summary>
        ''' Gets the requested horizontal offset.
        ''' </summary>
        Public ReadOnly Property ClipRectangleHorizontalOffset As UShort?
            Get
                Return _clipRectangleHorizontalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested vertical offset.
        ''' </summary>
        Public ReadOnly Property ClipRectangleVerticalOffset As UShort?
            Get
                Return _clipRectangleVerticalOffset
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested width.
        ''' </summary>
        Public ReadOnly Property ClipRectangleWidth As UShort?
            Get
                Return _clipRectangleWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested height.
        ''' </summary>
        Public ReadOnly Property ClipRectangleHeight As UShort?
            Get
                Return _clipRectangleHeight
            End Get
        End Property

        Protected Overrides Function GetQuery() As System.Collections.Specialized.NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.SetPlaybackState)

            query.Add(Constants.SetPlaybackStateParameters.ClipRectangleHorizontalOffset, ClipRectangleHorizontalOffset.Value.ToString)
            query.Add(Constants.SetPlaybackStateParameters.ClipRectangleVerticalOffset, ClipRectangleVerticalOffset.Value.ToString)
            query.Add(Constants.SetPlaybackStateParameters.ClipRectangleWidth, ClipRectangleWidth.Value.ToString)
            query.Add(Constants.SetPlaybackStateParameters.ClipRectangleHeight, ClipRectangleHeight.Value.ToString)

            Return query
        End Function
    End Class

End Namespace
Imports System.Text
Imports System.Collections.Specialized
Imports SL.DuneApiCodePack.Extensions

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to change the video output (zoom; width; height; position).</summary>
    Public Class SetVideoOutputCommand
        Inherits Command

        Private Const NotSupportedMessage As String = "This command requires a firmware update."

        Private _zoom As String
        Private _fullscreen As Boolean?
        Private _videoX As UShort?
        Private _videoY As UShort?
        Private _videoWidth As UShort?
        Private _videoHeight As UShort?

        ''' <param name="dune">The target device.</param>
        ''' <param name="zoom">The requested zoom setting.</param>
        ''' <param name="videoX">The requested horizontal position.</param>
        ''' <param name="videoY">The requested vertical position.</param>
        ''' <param name="videoWidth">The requested width.</param>
        ''' <param name="videoHeight">The requested height.</param>
        ''' <remarks></remarks>
        Public Sub New(ByRef dune As Dune, ByVal zoom As String, ByVal videoX As UShort?, ByVal videoY As UShort?, ByVal videoWidth As UShort?, ByVal videoHeight As UShort?)
            MyBase.New(dune)
            If dune.ProtocolVersion < 2 Then
                Throw New NotSupportedException(NotSupportedMessage)
            End If

            _zoom = zoom

            _videoX = CUShort(IIf(videoX.HasValue, videoX, 0))
            _videoY = CUShort(IIf(videoY.HasValue, videoY, 0))
            _videoWidth = CUShort(IIf(videoWidth.HasValue, videoWidth, dune.VideoTotalDisplayWidth))
            _videoHeight = CUShort(IIf(videoHeight.HasValue, videoHeight, dune.VideoTotalDisplayHeight))
        End Sub

        Public Sub New(ByVal dune As Dune, ByVal fullscreen As Boolean)
            Me.New(dune, Nothing, dune.VideoX, dune.VideoY, dune.VideoWidth, dune.VideoHeight)
            _fullscreen = fullscreen
        End Sub

        ''' <summary>
        ''' Gets or sets the requested zoom.
        ''' </summary>
        Public Property Zoom As String
            Get
                Return _zoom
            End Get
            Set(value As String)
                _zoom = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether to set the video output to fullscreen.
        ''' </summary>
        Public ReadOnly Property VideoFullscreen As Boolean?
            Get
                Return _fullscreen
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested horizontal position.
        ''' </summary>
        Public ReadOnly Property VideoX As UShort?
            Get
                Return _videoX
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested vertical position.
        ''' </summary>
        Public ReadOnly Property VideoY As UShort?
            Get
                Return _videoY
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested width.
        ''' </summary>
        Public ReadOnly Property VideoWidth As UShort?
            Get
                Return _videoWidth
            End Get
        End Property

        ''' <summary>
        ''' Gets the requested height.
        ''' </summary>
        Public ReadOnly Property VideoHeight As UShort?
            Get
                Return _videoHeight
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection

            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.SetPlaybackState)

            If _fullscreen.HasValue AndAlso _fullscreen.Value.IsTrue Then
                query.Add(Constants.SetPlaybackStateParameters.VideoFullscreen, "1")
            Else
                query.Add(Constants.SetPlaybackStateParameters.VideoFullscreen, "0")
                query.Add(Constants.SetPlaybackStateParameters.VideoHorizontalPosition, VideoX.Value.ToString)
                query.Add(Constants.SetPlaybackStateParameters.VideoVerticalPosition, VideoY.Value.ToString)
                query.Add(Constants.SetPlaybackStateParameters.VideoWidth, VideoWidth.Value.ToString)
                query.Add(Constants.SetPlaybackStateParameters.VideoHeight, VideoHeight.Value.ToString)
            End If

            If Zoom.IsNotNullOrEmpty Then
                query.Add(Constants.SetPlaybackStateParameters.VideoZoom, Zoom.ToString)
            End If

            Return query
        End Function
    End Class

End Namespace
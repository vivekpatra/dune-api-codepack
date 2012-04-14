Imports System.Text

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to change the video output (zoom; width; height; position).</summary>
    Public Class SetVideoOutputCommand
        Inherits DuneCommand

        Private Const NotSupportedMessage As String = "This command requires a firmware update."

        Private _zoom As Zoom?

        Private _videoX As UShort?
        Private _videoY As UShort?
        Private _videoWidth As UShort?
        Private _videoHeight As UShort?

        ''' <param name="dune">The target device.</param>
        ''' <param name="zoom">The requested zoom setting.</param>
        Public Sub New(ByRef dune As Dune, ByVal zoom As Zoom)
            MyBase.New(dune)
            If dune.ProtocolVersion < 2 Then
                Throw New NotSupportedException(NotSupportedMessage)
            End If
            _zoom = zoom
        End Sub

        ''' <param name="dune">The target device.</param>
        ''' <param name="videoX">The requested horizontal position.</param>
        ''' <param name="videoY">The requested vertical position.</param>
        ''' <param name="videoWidth">The requested width.</param>
        ''' <param name="videoHeight">The requested height.</param>
        ''' <remarks></remarks>
        Public Sub New(ByRef dune As Dune, ByVal videoX As UShort?, ByVal videoY As UShort?, ByVal videoWidth As UShort?, ByVal videoHeight As UShort?)
            MyBase.New(dune)
            If dune.ProtocolVersion < 2 Then
                Throw New NotSupportedException(NotSupportedMessage)
            End If
            _videoX = videoX
            _videoY = videoY
            _videoWidth = videoWidth
            _videoHeight = videoHeight
        End Sub

        ''' <summary>
        ''' Gets the requested zoom.
        ''' </summary>
        Public ReadOnly Property Zoom As Nullable(Of Zoom)
            Get
                Return _zoom
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

        Public Overrides Function ToUri() As System.Uri
            Dim query As New StringBuilder

            query.Append("cmd=set_playback_state")

            If Zoom.HasValue Then
                query.Append("&video_zoom=" + Zoom.Value.ToString())
            Else
                query.Append("&fullscreen=0")

                If VideoX.HasValue Then
                    query.Append("&video_x=" + VideoX.Value.ToString)
                End If

                If VideoY.HasValue Then
                    query.Append("&video_y=" + VideoY.Value.ToString)
                End If

                If VideoWidth.HasValue Then
                    query.Append("&video_width=" + VideoWidth.Value.ToString)
                End If

                If VideoHeight.HasValue Then
                    query.Append("&video_height=" + VideoHeight.Value.ToString)
                End If
            End If

            Return New Uri(BaseUri.ToString + query.ToString)
        End Function
    End Class

End Namespace
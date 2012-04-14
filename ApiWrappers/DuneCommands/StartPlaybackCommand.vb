Imports System.Text

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to send a new playback request.</summary>
    Public Class StartPlaybackCommand
        Inherits DuneCommand

        Private _type As PlaybackType
        Private _mediaUrl As String
        Private _paused As Boolean
        Private _position As TimeSpan
        Private _blackScreen As Boolean
        Private _hideOnScreenDisplay As Boolean
        Private _repeat As Boolean

        ''' <param name="dune">The target device.</param>
        ''' <param name="mediaUrl">The media URL.</param>
        ''' <remarks>Does not support playlists.</remarks>
        Public Sub New(ByRef dune As Dune, ByVal mediaUrl As String)
            MyBase.New(dune)
            _mediaUrl = UrlConverter.FormatUrl(dune, mediaUrl)
        End Sub

        ''' <summary>
        ''' Gets or sets whether to start the playback paused.
        ''' </summary>
        Public Property Paused As Boolean
            Get
                Return _paused
            End Get
            Set(value As Boolean)
                _paused = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback position to begin from.
        ''' </summary>
        Public Property Position As TimeSpan
            Get
                Return _position
            End Get
            Set(value As TimeSpan)
                _position = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playback type.
        ''' </summary>
        Public Property Type As PlaybackType
            Get
                If _type = Nothing Then
                    _type = PlaybackType.File
                End If
                Return _type
            End Get
            Set(value As PlaybackType)
                _type = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the requested media URL.
        ''' </summary>
        Public ReadOnly Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether to show a black screen.
        ''' </summary>
        Public Property BlackScreen As Boolean
            Get
                Return _blackScreen
            End Get
            Set(value As Boolean)
                _blackScreen = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to hide the OSD.
        ''' </summary>
        Public Property HideOnScreenDisplay As Boolean
            Get
                Return _hideOnScreenDisplay
            End Get
            Set(value As Boolean)
                _hideOnScreenDisplay = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to repeat the playback.
        ''' </summary>
        Public Property Repeat As Boolean
            Get
                Return _repeat
            End Get
            Set(value As Boolean)
                _repeat = value
            End Set
        End Property

        ''' <summary>
        ''' Enumeration of supported playback types.
        ''' </summary>
        ''' <remarks>"File" includes everything that is not a DVD or blu-ray iso or folder, although playlist files are not supported.</remarks>
        Public Enum PlaybackType
            File = 0
            Dvd = 1
            Bluray = 2
        End Enum

        Public Overrides Function ToUri() As Uri
            Dim commandBuilder As New StringBuilder

            Select Case Type
                Case PlaybackType.File
                    commandBuilder.Append("cmd=start_file_playback")
                Case PlaybackType.Dvd
                    commandBuilder.Append("cmd=start_dvd_playback")
                Case PlaybackType.Bluray
                    commandBuilder.Append("cmd=start_bluray_playback")
            End Select

            commandBuilder.AppendFormat("&media_url={0}", MediaUrl)

            If Paused Then
                commandBuilder.Append("&speed=0")
            End If

            If Position <> Nothing Then
                commandBuilder.AppendFormat("&position={0}", Position.TotalSeconds)
            End If

            If BlackScreen Then
                commandBuilder.Append("&black_screen=1")
            End If

            If HideOnScreenDisplay Then
                commandBuilder.Append("&hide_osd=1")
            End If

            If Repeat Then
                commandBuilder.Append("&action_on_finish=restart_playback")
            End If

            If Timeout > 0 AndAlso Timeout <> 20 Then
                commandBuilder.AppendFormat("&timeout={0}", Timeout)
            End If

            Dim query As String = commandBuilder.ToString

            Return New Uri(BaseUri.ToString + query)

        End Function
    End Class

End Namespace
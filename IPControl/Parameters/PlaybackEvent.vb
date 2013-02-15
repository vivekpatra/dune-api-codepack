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

Namespace IPControl

    ''' <summary>
    ''' A helper class for retrieving and comparing playback events and for creating new playback events.
    ''' </summary>
    Public Class PlaybackEvent : Inherits NameValuePair

        Private Shared ReadOnly _noEvent As PlaybackEvent = New PlaybackEvent("no_event")
        Private Shared ReadOnly _mediaDescriptionChanged As PlaybackEvent = New PlaybackEvent("media_description_changed")
        Private Shared ReadOnly _mediaReadStalled As PlaybackEvent = New PlaybackEvent("media_read_stalled")
        Private Shared ReadOnly _endOfMedia As PlaybackEvent = New PlaybackEvent("end_of_media")
        Private Shared ReadOnly _externalAction As PlaybackEvent = New PlaybackEvent("external_action")
        Private Shared ReadOnly _mediaFormatNotSupported As PlaybackEvent = New PlaybackEvent("media_format_not_supported")
        Private Shared ReadOnly _mediaOpenFailed As PlaybackEvent = New PlaybackEvent("media_open_failed")
        Private Shared ReadOnly _mediaReadFailed As PlaybackEvent = New PlaybackEvent("media_read_failed")
        Private Shared ReadOnly _mediaProtocolNotSupported As PlaybackEvent = New PlaybackEvent("media_protocol_not_supported")
        Private Shared ReadOnly _mediaPermissionDenied As PlaybackEvent = New PlaybackEvent("media_permission_denied")
        Private Shared ReadOnly _internalError As PlaybackEvent = New PlaybackEvent("internal_error")
        Private Shared ReadOnly _playlistChanged As PlaybackEvent = New PlaybackEvent("playlist_changed")
        Private Shared ReadOnly _mediaChanged As PlaybackEvent = New PlaybackEvent("media_changed")
        Private Shared ReadOnly _audioStreamChanged As PlaybackEvent = New PlaybackEvent("audio_stream_changed")
        Private Shared ReadOnly _subtitleStreamChanged As PlaybackEvent = New PlaybackEvent("subtitle_stream_changed")
        Private Shared ReadOnly _pcrDiscontinuity As PlaybackEvent = New PlaybackEvent("pcr_discontinuity")

        Public Sub New(playbackEvent As String)
            MyBase.New(playbackEvent)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Output.LastPlaybackEvent.Name
            End Get
        End Property

        Public Shared ReadOnly Property NoEvent As PlaybackEvent
            Get
                Return PlaybackEvent._noEvent
            End Get
        End Property

        Public Shared ReadOnly Property MediaDescriptionChanged As PlaybackEvent
            Get
                Return PlaybackEvent._mediaDescriptionChanged
            End Get
        End Property

        Public Shared ReadOnly Property MediaReadStalled As PlaybackEvent
            Get
                Return PlaybackEvent._mediaReadStalled
            End Get
        End Property

        Public Shared ReadOnly Property EndOfMedia As PlaybackEvent
            Get
                Return PlaybackEvent._endOfMedia
            End Get
        End Property

        Public Shared ReadOnly Property ExternalAction As PlaybackEvent
            Get
                Return PlaybackEvent._externalAction
            End Get
        End Property

        Public Shared ReadOnly Property MediaFormatNotSupported As PlaybackEvent
            Get
                Return PlaybackEvent._mediaFormatNotSupported
            End Get
        End Property

        Public Shared ReadOnly Property MediaOpenFailed As PlaybackEvent
            Get
                Return PlaybackEvent._mediaOpenFailed
            End Get
        End Property

        Public Shared ReadOnly Property MediaReadFailed As PlaybackEvent
            Get
                Return PlaybackEvent._mediaReadFailed
            End Get
        End Property

        Public Shared ReadOnly Property MediaProtocolNotSupported As PlaybackEvent
            Get
                Return PlaybackEvent._mediaProtocolNotSupported
            End Get
        End Property

        Public Shared ReadOnly Property MediaPermissionDenied As PlaybackEvent
            Get
                Return PlaybackEvent._mediaPermissionDenied
            End Get
        End Property

        Public Shared ReadOnly Property InternalError As PlaybackEvent
            Get
                Return PlaybackEvent._internalError
            End Get
        End Property

        Public Shared ReadOnly Property PlaylistChanged As PlaybackEvent
            Get
                Return PlaybackEvent._playlistChanged
            End Get
        End Property

        Public Shared ReadOnly Property MediaChanged As PlaybackEvent
            Get
                Return PlaybackEvent._mediaChanged
            End Get
        End Property

        Public Shared ReadOnly Property AudioStreamChanged As PlaybackEvent
            Get
                Return PlaybackEvent._audioStreamChanged
            End Get
        End Property

        Public Shared ReadOnly Property SubtitleStreamChanged As PlaybackEvent
            Get
                Return PlaybackEvent._subtitleStreamChanged
            End Get
        End Property

        ''' <summary>If the difference between two consecutive PCR (Primary Clock Reference) values is outside a certain range, this error can occur.</summary>
        Public Shared ReadOnly Property PcrDiscontinuity As PlaybackEvent
            Get
                Return PlaybackEvent._pcrDiscontinuity
            End Get
        End Property

    End Class

End Namespace
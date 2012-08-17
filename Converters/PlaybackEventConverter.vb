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
Imports System.ComponentModel

Public Class PlaybackEventConverter
    Inherits StringConverter

    Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object
        Select Case DirectCast(value, String)
            Case Constants.LastPlaybackEventValues.AudioStreamChanged
                Return CObj("Audio stream changed")
            Case Constants.LastPlaybackEventValues.EndOfMedia
                Return CObj("End of media")
            Case Constants.LastPlaybackEventValues.ExternalAction
                Return CObj("External action")
            Case Constants.LastPlaybackEventValues.InternalError
                Return CObj("Internal error")
            Case Constants.LastPlaybackEventValues.MediaChanged
                Return CObj("Media changed")
            Case Constants.LastPlaybackEventValues.MediaDescriptionChanged
                Return CObj("Media description changed")
            Case Constants.LastPlaybackEventValues.MediaFormatNotSupported
                Return CObj("Media format not supported")
            Case Constants.LastPlaybackEventValues.MediaOpenFailed
                Return CObj("Media open failed")
            Case Constants.LastPlaybackEventValues.MediaPermissionDenied
                Return CObj("Media permission denied")
            Case Constants.LastPlaybackEventValues.MediaProtocolNotSupported
                Return CObj("Media protocol not supported")
            Case Constants.LastPlaybackEventValues.MediaReadFailed
                Return CObj("Media read failed")
            Case Constants.LastPlaybackEventValues.MediaReadStalled
                Return CObj("Media read stalled")
            Case Constants.LastPlaybackEventValues.NoEvent
                Return CObj("No event")
            Case Constants.LastPlaybackEventValues.PcrDiscontinuity
                Return CObj("PCR discontinuity")
            Case Constants.LastPlaybackEventValues.PlaylistChanged
                Return CObj("Playlist changed")
            Case Constants.LastPlaybackEventValues.SubtitleStreamChanged
                Return CObj("Subtitle stream changed")
            Case Else
                Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Select
    End Function

    Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
        Select Case DirectCast(value, String)
            Case "Main screen"
                Return Constants.CommandValues.MainScreen
            Case "Black screen"
                Return Constants.CommandValues.BlackScreen
            Case "Standby"
                Return Constants.CommandValues.Standby
            Case Else
                Return MyBase.ConvertFrom(context, culture, value)
        End Select
    End Function

End Class

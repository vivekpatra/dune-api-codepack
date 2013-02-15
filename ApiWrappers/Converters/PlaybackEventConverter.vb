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
Imports System.ComponentModel

Public Class PlaybackEventConverter
    Inherits TypeConverter

    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return False
    End Function

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType Is GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is PlaybackEvent Then
            Select Case DirectCast(value, PlaybackEvent)
                Case PlaybackEvent.AudioStreamChanged
                    Return "Audio stream changed"
                Case PlaybackEvent.EndOfMedia
                    Return "End of media"
                Case PlaybackEvent.ExternalAction
                    Return "External action"
                Case PlaybackEvent.InternalError
                    Return "Internal error"
                Case PlaybackEvent.MediaChanged
                    Return "Media changed"
                Case PlaybackEvent.MediaDescriptionChanged
                    Return "Media description changed"
                Case PlaybackEvent.MediaFormatNotSupported
                    Return "Media format not supported"
                Case PlaybackEvent.MediaOpenFailed
                    Return "Media open failed"
                Case PlaybackEvent.MediaPermissionDenied
                    Return "Media permission denied"
                Case PlaybackEvent.MediaProtocolNotSupported
                    Return "Media protocol not supported"
                Case PlaybackEvent.MediaReadFailed
                    Return "Media read failed"
                Case PlaybackEvent.MediaReadStalled
                    Return "Media read stalled"
                Case PlaybackEvent.NoEvent
                    Return "No event"
                Case PlaybackEvent.PcrDiscontinuity
                    Return "PCR discontinuity"
                Case PlaybackEvent.PlaylistChanged
                    Return "Playlist changed"
                Case PlaybackEvent.SubtitleStreamChanged
                    Return "Subtitle stream changed"
                Case Else
                    Return DirectCast(value, PlaybackEvent).Value
            End Select
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class

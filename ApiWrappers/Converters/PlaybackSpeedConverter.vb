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

Public Class PlaybackSpeedConverter
    Inherits TypeConverter

    Private Shared standardValues As StandardValuesCollection

    Shared Sub New()
        Dim values As New System.Collections.ObjectModel.Collection(Of PlaybackSpeed)
        With values
            For i = 13 To 3 Step -1
                .Add(New PlaybackSpeed(-(1 << i)))
            Next
            .Add(PlaybackSpeed.Pause)
            For i = 3 To 13
                .Add(New PlaybackSpeed(1 << i))
            Next
        End With
        standardValues = New StandardValuesCollection(values)
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return standardValues
    End Function

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType = GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Dim input As String = DirectCast(value, String).Trim

            Select Case True
                Case String.Equals(input, "Rewind (32x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind8192
                Case String.Equals(input, "Rewind (16x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind4096
                Case String.Equals(input, "Rewind (8x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind2048
                Case String.Equals(input, "Rewind (4x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind1024
                Case String.Equals(input, "Rewind (2x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind512
                Case String.Equals(input, "Rewind", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind256
                Case String.Equals(input, "Rewind (1/2x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind128
                Case String.Equals(input, "Rewind (1/4x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind64
                Case String.Equals(input, "Rewind (1/8x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind32
                Case String.Equals(input, "Rewind (1/16x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind16
                Case String.Equals(input, "Rewind (1/32x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Rewind8
                Case String.Equals(input, "Pause", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Pause
                Case String.Equals(input, "Slowdown (1/32x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play8
                Case String.Equals(input, "Slowdown (1/16x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play16
                Case String.Equals(input, "Slowdown (1/8x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play32
                Case String.Equals(input, "Slowdown (1/4x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play64
                Case String.Equals(input, "Slowdown (1/2x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play128
                Case String.Equals(input, "Normal", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play256
                Case String.Equals(input, "Forward (2x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play512
                Case String.Equals(input, "Forward (4x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play1024
                Case String.Equals(input, "Forward (8x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play2048
                Case String.Equals(input, "Forward (16x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play4096
                Case String.Equals(input, "Forward (32x)", StringComparison.InvariantCultureIgnoreCase)
                    Return PlaybackSpeed.Play8192
                Case Else
                    Return PlaybackSpeed.Play256
            End Select
        End If

        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is PlaybackSpeed Then
            Select Case DirectCast(value, PlaybackSpeed)
                Case PlaybackSpeed.Rewind8192 : Return "Rewind (32x)"
                Case PlaybackSpeed.Rewind4096 : Return "Rewind (16x)"
                Case PlaybackSpeed.Rewind2048 : Return "Rewind (8x)"
                Case PlaybackSpeed.Rewind1024 : Return "Rewind (4x)"
                Case PlaybackSpeed.Rewind512 : Return "Rewind (2x)"
                Case PlaybackSpeed.Rewind256 : Return "Rewind"
                Case PlaybackSpeed.Rewind128 : Return "Rewind (1/2x)"
                Case PlaybackSpeed.Rewind64 : Return "Rewind (1/4x)"
                Case PlaybackSpeed.Rewind32 : Return "Rewind (1/8x)"
                Case PlaybackSpeed.Rewind16 : Return "Rewind (1/16x)"
                Case PlaybackSpeed.Rewind8 : Return "Rewind (1/32x)"
                Case PlaybackSpeed.Pause : Return "Pause"
                Case PlaybackSpeed.Play8 : Return "Slowdown (1/32x)"
                Case PlaybackSpeed.Play16 : Return "Slowdown (1/16x)"
                Case PlaybackSpeed.Play32 : Return "Slowdown (1/8x)"
                Case PlaybackSpeed.Play64 : Return "Slowdown (1/4x)"
                Case PlaybackSpeed.Play128 : Return "Slowdown (1/2x)"
                Case PlaybackSpeed.Play256 : Return "Normal"
                Case PlaybackSpeed.Play512 : Return "Forward (2x)"
                Case PlaybackSpeed.Play1024 : Return "Forward (4x)"
                Case PlaybackSpeed.Play2048 : Return "Forward (8x)"
                Case PlaybackSpeed.Play4096 : Return "Forward (16x)"
                Case PlaybackSpeed.Play8192 : Return "Forward (32x)"
                Case Else : Return DirectCast(value, PlaybackSpeed).Value
            End Select
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class

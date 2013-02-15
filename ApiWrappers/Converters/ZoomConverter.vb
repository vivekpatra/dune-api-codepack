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

Public Class ZoomConverter
    Inherits TypeConverter

    Private Shared standardValues As StandardValuesCollection

    Shared Sub New()
        Dim values As New System.Collections.ObjectModel.Collection(Of VideoZoom)
        With values
            .Add(VideoZoom.Normal)
            .Add(VideoZoom.Enlarge)
            .Add(VideoZoom.MakeWider)
            .Add(VideoZoom.FillScreen)
            .Add(VideoZoom.FullFillScreen)
            .Add(VideoZoom.MakeTaller)
            .Add(VideoZoom.CutEdges)
            .Add(VideoZoom.FullEnlarge)
            .Add(VideoZoom.FullStretch)
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
                Case String.IsNullOrEmpty(input)
                    Return VideoZoom.Normal
                Case String.Equals(input, "Normal", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.Normal
                Case String.Equals(input, "Full screen", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.FullEnlarge
                Case String.Equals(input, "Stretch to full screen", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.FullStretch
                Case String.Equals(input, "Non-linear stretch", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.FillScreen
                Case String.Equals(input, "Non-linear stretch to full screen", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.FullFillScreen
                Case String.Equals(input, "Enlarge", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.Enlarge
                Case String.Equals(input, "Make wider", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.MakeWider
                Case String.Equals(input, "Make taller", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.MakeTaller
                Case String.Equals(input, "Cut edges", StringComparison.InvariantCultureIgnoreCase)
                    Return VideoZoom.CutEdges
                Case Else
                    Return New VideoZoom(DirectCast(value, String))
            End Select
        End If

        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is VideoZoom Then
            Select Case DirectCast(value, VideoZoom)
                Case VideoZoom.Normal
                    Return "Normal"
                Case VideoZoom.FullEnlarge
                    Return "Full screen"
                Case VideoZoom.FullStretch
                    Return "Stretch to full screen"
                Case VideoZoom.FillScreen
                    Return "Non-linear stretch"
                Case VideoZoom.FullFillScreen
                    Return "Non-linear stretch to full screen"
                Case VideoZoom.Enlarge
                    Return "Enlarge"
                Case VideoZoom.MakeWider
                    Return "Make wider"
                Case VideoZoom.MakeTaller
                    Return "Make taller"
                Case VideoZoom.CutEdges
                    Return "Cut edges"
                Case VideoZoom.Other
                    Return "Other"
                Case Else
                    Return DirectCast(value, VideoZoom).Value
            End Select
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class
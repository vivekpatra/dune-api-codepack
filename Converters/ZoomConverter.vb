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

Public Class ZoomConverter
    Inherits StringConverter

    Private _standardValues As ArrayList

    Public Sub New()
        Dim zooms As New ArrayList
        zooms.Add(Constants.VideoZoomValues.Normal)
        zooms.Add(Constants.VideoZoomValues.Enlarge)
        zooms.Add(Constants.VideoZoomValues.MakeWider)
        zooms.Add(Constants.VideoZoomValues.FillScreen)
        zooms.Add(Constants.VideoZoomValues.FillFullScreen)
        zooms.Add(Constants.VideoZoomValues.MakeTaller)
        zooms.Add(Constants.VideoZoomValues.CutEdges)
        zooms.Add(Constants.VideoZoomValues.FullEnlarge)
        zooms.Add(Constants.VideoZoomValues.FullStretch)
        _standardValues = zooms
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(_standardValues)
    End Function

    Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object
        Select Case DirectCast(value, String)
            Case Constants.VideoZoomValues.Normal
                Return "Normal"
            Case Constants.VideoZoomValues.FullEnlarge
                Return "Full screen"
            Case Constants.VideoZoomValues.FullStretch
                Return "Stretch to full screen"
            Case Constants.VideoZoomValues.FillScreen
                Return "Non-linear stretch"
            Case Constants.VideoZoomValues.FillFullScreen
                Return "Non-linear stretch to full screen"
            Case Constants.VideoZoomValues.Enlarge
                Return "Enlarge"
            Case Constants.VideoZoomValues.MakeWider
                Return "Make wider"
            Case Constants.VideoZoomValues.MakeTaller
                Return "Make taller"
            Case Constants.VideoZoomValues.CutEdges
                Return "Cut edges"
            Case Constants.VideoZoomValues.Other
                Return "Other"
            Case Else
                Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Select
    End Function

    Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
        Select Case DirectCast(value, String)
            Case "Normal"
                Return Constants.VideoZoomValues.Normal
            Case "Full screen"
                Return Constants.VideoZoomValues.FullEnlarge
            Case "Stretch to full screen"
                Return Constants.VideoZoomValues.FullStretch
            Case "Non-linear stretch"
                Return Constants.VideoZoomValues.FillScreen
            Case "Non-linear stretch to full screen"
                Return Constants.VideoZoomValues.FillFullScreen
            Case "Enlarge"
                Return Constants.VideoZoomValues.Enlarge
            Case "Make wider"
                Return Constants.VideoZoomValues.MakeWider
            Case "Make taller"
                Return Constants.VideoZoomValues.MakeTaller
            Case "Cut edges"
                Return Constants.VideoZoomValues.CutEdges
            Case Else
                Return MyBase.ConvertFrom(context, culture, value)
        End Select
    End Function
End Class
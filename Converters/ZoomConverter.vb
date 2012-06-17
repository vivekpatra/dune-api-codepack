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
End Class

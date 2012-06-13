Imports System.ComponentModel

Public Class ZoomConverter
    Inherits StringConverter

    Private _standardValues As ArrayList

    Public Sub New()
        Dim zooms As New ArrayList
        zooms.Add(Constants.VideoZoomSettings.Normal)
        zooms.Add(Constants.VideoZoomSettings.Enlarge)
        zooms.Add(Constants.VideoZoomSettings.MakeWider)
        zooms.Add(Constants.VideoZoomSettings.FillScreen)
        zooms.Add(Constants.VideoZoomSettings.FillFullScreen)
        zooms.Add(Constants.VideoZoomSettings.MakeTaller)
        zooms.Add(Constants.VideoZoomSettings.CutEdges)
        zooms.Add(Constants.VideoZoomSettings.FullEnlarge)
        zooms.Add(Constants.VideoZoomSettings.FullStretch)
        _standardValues = zooms
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(_standardValues)
    End Function
End Class

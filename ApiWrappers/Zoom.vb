Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' Enumeration of supported zoom settings.
    ''' </summary>
    Public Enum Zoom
        normal = 0
        enlarge = 1
        make_wider = 2
        fill_screen = 3
        full_fill_screen = 4
        make_taller = 5
        cut_edges = 6
    End Enum

    Public Class ZoomConverter
        ''' <summary>
        ''' Get the proper spelling for a zoom setting.
        ''' </summary>
        Public Shared Function GetName(ByVal setting As Zoom) As String
            Select Case setting
                Case Zoom.normal
                    Return "Normal"
                Case Zoom.enlarge
                    Return "Enlarged"
                Case Zoom.make_wider
                    Return "Wider"
                Case Zoom.fill_screen
                    Return "Filling screen"
                Case Zoom.full_fill_screen
                    Return "Full filling screen"
                Case Zoom.make_taller
                    Return "Taller"
                Case Zoom.cut_edges
                    Return "Cut edges"
                Case Else
                    Return "Custom"
            End Select
        End Function
    End Class

End Namespace
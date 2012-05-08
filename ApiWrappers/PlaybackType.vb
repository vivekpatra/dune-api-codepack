Namespace DuneUtilities.ApiWrappers

    Public Enum PlaybackType
        File = 0
        Dvd = 1
        Bluray = 2
    End Enum

    Public Class PlaybackTypeConverter
        Public Shared Function GetName(ByVal type As PlaybackType) As String
            Select Case type
                Case PlaybackType.File
                    Return "File"
                Case PlaybackType.Dvd
                    Return "DVD"
                Case PlaybackType.Bluray
                    Return "Blu-ray"
                Case Else
                    Throw New NotImplementedException
            End Select
        End Function
    End Class

End Namespace
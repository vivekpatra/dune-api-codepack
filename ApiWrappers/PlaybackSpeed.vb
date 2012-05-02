Namespace Dune.ApiWrappers

    Public Enum PlaybackSpeed
        Rewind_16x = -4096
        Rewind_8x = -2048
        Rewind_4x = -1024
        Rewind_2x = -512
        Rewind = -256
        Rewind_Slow = -64
        Pause = 0
        Slow = 64
        Normal = 256
        Forward_2x = 512
        Forward_4x = 1024
        Forward_8x = 2048
        Forward_16x = 4096
    End Enum

    Public Class PlaybackSpeedConverter
        Public Shared Function GetName(ByRef dune As Dune, ByVal speed As PlaybackSpeed) As String
            Select Case speed
                Case PlaybackSpeed.Rewind_16x
                    Return "Rewinding (16x)"
                Case PlaybackSpeed.Rewind_8x
                    Return "Rewinding (8x)"
                Case PlaybackSpeed.Rewind_4x
                    Return "Rewinding (4x)"
                Case PlaybackSpeed.Rewind_2x
                    Return "Rewinding (2x)"
                Case PlaybackSpeed.Rewind
                    Return "Rewinding"
                Case PlaybackSpeed.Rewind_Slow
                    Return "Rewinding (Slow)"
                Case PlaybackSpeed.Pause
                    If dune.PlaybackDuration.Value.TotalSeconds > 0 Then
                        Return "Paused"
                    Else
                        Return "N/A"
                    End If
                Case PlaybackSpeed.Slow
                    Return "Slow"
                Case PlaybackSpeed.Normal
                    Return "Normal"
                Case PlaybackSpeed.Forward_2x
                    Return "Forwarding (2x)"
                Case PlaybackSpeed.Forward_4x
                    Return "Forwarding (4x)"
                Case PlaybackSpeed.Forward_8x
                    Return "Forwarding (8x)"
                Case PlaybackSpeed.Forward_16x
                    Return "Forwarding (16x)"
                Case Else
                    Return "Undetermined"
            End Select
        End Function
    End Class

End Namespace
Imports System.Runtime.CompilerServices

Namespace Extensions

    ''' <summary>
    ''' Extensions for the Integer type.
    ''' </summary>
    Public Module IntegerExtensions

        <Extension()>
        Public Function GetGreatestCommonFactor(int As Integer, value As Integer) As Integer
            Dim temp As Integer

            Do
                temp = int Mod value
                int = value
                value = temp
            Loop Until value = 0

            Return int
        End Function

    End Module

End Namespace
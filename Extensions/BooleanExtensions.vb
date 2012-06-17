Imports System.Runtime.CompilerServices

Namespace Extensions

    ''' <summary>
    ''' Extensions for the Boolean type.
    ''' </summary>
    Public Module BooleanExtensions

        ''' <summary>
        ''' Toggles the value when called.
        ''' <example>Example use case: <c>duneInstance.PlaybackMute.Toggle()</c></example>
        ''' </summary>
        <Extension()>
        Public Sub Toggle(ByRef value As Boolean)
            value = Not value
        End Sub

        ''' <summary>
        ''' Returns "1" if true; otherwise "0".
        ''' </summary>
        <Extension()>
        Public Function ToNumberString(value As Boolean) As String
            Return Math.Abs(CInt(value)).ToString(Constants.FormatProvider)
        End Function

        <Extension()>
        Public Function GetNumberString(value As Boolean) As String
            If value.IsTrue Then
                Return "1"
            Else
                Return "0"
            End If
        End Function

        ''' <summary>
        ''' Gets whether the value is True.
        ''' </summary>
        <Extension()>
        Public Function IsTrue(value As Boolean) As Boolean
            Return value = True
        End Function

        ''' <summary>
        ''' Gets whether the value is False.
        ''' </summary>
        <Extension()>
        Public Function IsFalse(value As Boolean) As Boolean
            Return value = False
        End Function

    End Module

End Namespace
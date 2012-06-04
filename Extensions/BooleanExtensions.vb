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
        ''' Gets whether the value is True.
        ''' </summary>
        <Extension()>
        Public Function IsTrue(ByVal value As Boolean) As Boolean
            Return value = True
        End Function

        ''' <summary>
        ''' Gets whether the value is False.
        ''' </summary>
        <Extension()>
        Public Function IsFalse(ByVal value As Boolean) As Boolean
            Return value = False
        End Function

    End Module

End Namespace
Imports System.Runtime.CompilerServices

Namespace Extensions

    ''' <summary>
    ''' Extensions for the Boolean type.
    ''' </summary>
    Module BooleanExtensions

        ''' <summary>
        ''' Toggles the value when called.
        ''' <example>Example use case: <c>duneInstance.PlaybackMute.Toggle()</c></example>
        ''' </summary>
        <Extension()>
        Public Sub Toggle(ByRef value As Boolean)
            value = Not value
        End Sub

    End Module

End Namespace
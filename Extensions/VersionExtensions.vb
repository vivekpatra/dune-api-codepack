Imports System.Runtime.CompilerServices
Imports System.Net.NetworkInformation

Namespace Extensions
    ''' <summary>
    ''' Extensions for the <see cref="PhysicalAddress"/> type.
    ''' </summary>
    Public Module VersionExtensions

        <Extension()>
        Public Function Parse(version As Version, input As String, strict As Boolean) As Version
            If strict Then
                Return System.Version.Parse(input)
            Else
                If input.Contains("."c).IsFalse Then
                    input += ".0"
                End If
                Return System.Version.Parse(input)
            End If
        End Function

        

    End Module
End Namespace

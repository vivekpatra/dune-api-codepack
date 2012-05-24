Imports System.Runtime.CompilerServices
Imports System.Net.NetworkInformation

Namespace Extensions
    ''' <summary>
    ''' Extensions for the <see cref="PhysicalAddress"/> type.
    ''' </summary>
    Public Module PhysicalAddressExtensions

        <Extension()>
        Public Function ToDelimitedString(ByVal value As PhysicalAddress) As String
            Return ToDelimitedString(value, ":"c)
        End Function

        <Extension()>
        Public Function ToDelimitedString(ByVal value As PhysicalAddress, ByVal delimiter As Char) As String
            Dim address() As Byte = value.GetAddressBytes
            Return BitConverterExtensions.ToString(address, delimiter)
        End Function

    End Module
End Namespace

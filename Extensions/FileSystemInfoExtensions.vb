Imports System.Runtime.CompilerServices
Imports System.IO

Module FileSystemInfoExtensions

    <Extension()>
    Public Function GetUri(entry As FileSystemInfo) As Uri
        Return New Uri(entry.FullName)
    End Function

End Module

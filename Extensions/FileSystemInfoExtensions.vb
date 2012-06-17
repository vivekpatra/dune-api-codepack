Imports System.Runtime.CompilerServices
Imports System.IO

Namespace Extensions

    Public Module FileSystemInfoExtensions

        ''' <summary>
        ''' Gets a <see cref="System.Uri"/> instance for the specified path.
        ''' </summary>
        <Extension()>
        Public Function GetUri(entry As FileSystemInfo) As Uri
            Return New Uri(entry.FullName)
        End Function

        ''' <summary>
        ''' Gets the root directory of this instance.
        ''' </summary>
        <Extension()>
        Public Function GetRoot(entry As FileSystemInfo) As DirectoryInfo
            Return New DirectoryInfo(entry.FullName).Root
        End Function

        ''' <summary>
        ''' Creates a new instance of <see cref="DirectoryInfo"/>.
        ''' </summary>
        <Extension()>
        Public Function ToDirectoryInfo(entry As FileSystemInfo) As DirectoryInfo
            Return New DirectoryInfo(entry.FullName)
        End Function

        ''' <summary>
        ''' Creates a new instance of <see cref="FileInfo"/>.
        ''' </summary>
        <Extension()>
        Public Function ToFileInfo(entry As FileSystemInfo) As FileInfo
            Return New FileInfo(entry.FullName)
        End Function

    End Module

End Namespace

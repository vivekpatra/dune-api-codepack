Imports System.Text

Public MustInherit Class UnixPath

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Gets whether the specified path is an absolute path.
    ''' </summary>
    Public Shared Function IsAbsolute(path As String) As Boolean
        If String.IsNullOrEmpty(path) Then
            Return False
        End If
        Return path.StartsWith(UnixPath.Separator, StringComparison.InvariantCulture)
    End Function

    ''' <summary>
    ''' Normalizes a string path, taking care of '..' and '.' parts.
    ''' </summary>
    Public Shared Function Normalize(path As String) As String
        If String.IsNullOrEmpty(path) Then
            Return String.Empty
        End If
        Dim oldPath = path.Split(New Char() {UnixPath.Separator}, StringSplitOptions.RemoveEmptyEntries)
        Dim newPath As New Stack(Of String)
        Dim skipCount As Integer = 0

        For i = oldPath.GetUpperBound(0) To oldPath.GetLowerBound(0) Step -1
            If String.Equals(oldPath(i), UnixPath.CurrentDirectory, StringComparison.InvariantCulture) Then
                Continue For
            ElseIf String.Equals(oldPath(i), UnixPath.ParentDirectory, StringComparison.InvariantCulture) Then
                skipCount += 1
            ElseIf skipCount > 0 Then
                skipCount -= 1
            Else
                newPath.Push(oldPath(i))
            End If
        Next

        If UnixPath.IsAbsolute(path) Then
            Return UnixPath.Join(UnixPath.Separator, UnixPath.Join(newPath.ToArray))
        Else
            For i = 1 To skipCount
                newPath.Push(UnixPath.ParentDirectory)
            Next
            Return UnixPath.Join(newPath.ToArray)
        End If
    End Function

    ''' <summary>
    ''' Combines an array of string paths.
    ''' </summary>
    Public Shared Function Join(ParamArray paths As String()) As String
        If paths.Length = 0 Then
            Return String.Empty
        End If

        Dim builder As New StringBuilder

        ' add leading slash if first path is an absolute path
        If UnixPath.IsAbsolute(paths.First) Then
            builder.Append(UnixPath.Separator)
        End If

        For Each path In paths
            If String.IsNullOrEmpty(path) Then
                Continue For
            End If
            For Each component In path.Split(New Char() {UnixPath.Separator}, StringSplitOptions.RemoveEmptyEntries)
                If String.IsNullOrEmpty(component) Then
                    Continue For
                End If
                builder.Append(component)
                builder.Append(UnixPath.Separator)
            Next
        Next

        ' discard trailing slash if path is not the root directory
        If builder.Length > 1 Then
            builder.Remove(builder.Length - 1, 1)
        End If

        Return builder.ToString
    End Function

    Public Shared Function GetBaseName(path As String) As String
        Return UnixPath.GetBaseName(path, String.Empty, StringComparison.InvariantCultureIgnoreCase)
    End Function

    Public Shared Function GetBaseName(path As String, suffix As String) As String
        Return GetBaseName(path, suffix, StringComparison.InvariantCultureIgnoreCase)
    End Function

    Public Shared Function GetBaseName(path As String, suffix As String, comparisonType As System.StringComparison) As String
        If String.IsNullOrEmpty(path) Then
            Return String.Empty
        End If

        Dim name = path.Split(New Char() {"/"c}, StringSplitOptions.RemoveEmptyEntries).LastOrDefault
        If name.EndsWith(suffix, comparisonType) Then
            Return name.Left(name.Length - suffix.Length)
        End If

        Return name
    End Function

    Public Shared ReadOnly Property CurrentDirectory As String
        Get
            Return "."
        End Get
    End Property

    Public Shared ReadOnly Property ParentDirectory As String
        Get
            Return ".."
        End Get
    End Property

    Public Shared ReadOnly Property Separator As Char
        Get
            Return "/"c
        End Get
    End Property

    Public Shared Function HasExtension(path As String) As Boolean
        Dim baseName = UnixPath.GetBaseName(path)
        Return baseName.Contains("."c) AndAlso Not baseName.EndsWith(".", StringComparison.InvariantCulture)
    End Function

    Public Shared Function GetExtension(path As String) As String
        If Not UnixPath.HasExtension(path) Then
            Return String.Empty
        End If
        Return path.Substring(path.LastIndexOf("."c))
    End Function

End Class

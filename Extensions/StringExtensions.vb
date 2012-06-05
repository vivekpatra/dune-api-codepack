Imports System.Runtime.CompilerServices

Namespace Extensions

    Public Module StringExtensions

        ''' <summary>
        ''' Indicates whether the instance is null or a System.String.Empty string.
        ''' </summary>
        <Extension()>
        Public Function IsNullOrEmpty(text As String) As Boolean
            Return String.IsNullOrEmpty(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is not null or not a System.String.Empty string.
        ''' </summary>
        <Extension()>
        Public Function IsNotNullOrEmpty(text As String) As Boolean
            Return Not String.IsNullOrEmpty(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is null, empty or consists only of white-space characters.
        ''' </summary>
        <Extension()>
        Public Function IsNullOrWhiteSpace(text As String) As Boolean
            Return String.IsNullOrWhiteSpace(text)
        End Function

        ''' <summary>
        ''' Indicates whether the instance is not null, not empty or does not consist only of white-space characters.
        ''' </summary>
        <Extension()>
        Public Function IsNotNullOrWhiteSpace(text As String) As Boolean
            Return Not String.IsNullOrWhiteSpace(text)
        End Function

        ''' <summary>
        ''' Sets the instance to a System.String.Empty string.
        ''' </summary>
        <Extension()>
        Public Sub Clear(ByRef text As String)
            text = String.Empty
        End Sub

        ''' <summary>
        ''' Returns a substring, starting from the left.
        ''' </summary>
        <Extension()>
        Public Function Left(text As String, length As Integer) As String
            Return text.Substring(0, length)
        End Function

        ''' <summary>
        ''' Returns a substring, starting from the right.
        ''' </summary>
        <Extension()>
        Public Function Right(text As String, length As Integer) As String
            Return text.Substring(text.Length - length)
        End Function

    End Module

End Namespace
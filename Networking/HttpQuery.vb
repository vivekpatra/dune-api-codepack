Imports System.Runtime.Serialization

Namespace Networking

    ''' <summary>
    ''' Type that inherits from NameValueCollection and returns a properly encoded query string when you call ToString().
    ''' </summary>
    <Serializable()>
    Public Class HttpQuery
        Inherits System.Collections.Specialized.NameValueCollection

        Public Sub New()
            ' default constructor
        End Sub

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub


        Public Overrides Function ToString() As String
            Dim queryBuilder As New Text.StringBuilder

            For Each key As String In Me.AllKeys
                queryBuilder.Append(key)
                queryBuilder.Append("="c)
                Dim encodedValue As String = System.Uri.EscapeDataString(Me.Item(key))
                queryBuilder.Append(encodedValue)
                If key <> AllKeys.Last Then
                    queryBuilder.Append("&"c)
                End If
            Next

            Return queryBuilder.ToString
        End Function
    End Class

End Namespace
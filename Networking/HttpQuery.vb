#Region "License"
' Copyright 2012 Steven Liekens
' Contact: steven.liekens@gmail.com

' This file is part of DuneApiCodepack.

' DuneApiCodepack is free software: you can redistribute it and/or modify
' it under the terms of the GNU Lesser General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' DuneApiCodepack is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Lesser General Public License for more details.

' You should have received a copy of the GNU Lesser General Public License
' along with DuneApiCodepack.  If not, see <http://www.gnu.org/licenses/>.
#End Region ' License
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
#Region "License"
' Copyright 2012-2013 Steven Liekens
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
Imports System.Collections.Specialized
Imports System.Net.Http

Namespace Networking

    ''' <summary>
    ''' Type that inherits from NameValueCollection and returns a properly encoded query string when you call ToString().
    ''' </summary>
    <Serializable()>
    Public Class HttpQuery
        Inherits Dictionary(Of String, String)

        Public Sub New()
            ' default constructor
        End Sub

        Public Sub New(pairs As IDictionary(Of String, String))
            If pairs IsNot Nothing Then
                For Each pair In pairs
                    Me.Item(pair.Key) = pair.Value
                Next
            End If
        End Sub

        Public Sub New(pairs As IDictionary(Of Parameter, String))
            If pairs IsNot Nothing Then
                For Each pair In pairs
                    Me.Item(pair.Key) = pair.Value
                Next
            End If
        End Sub

        Public Overloads Property Item(key As Parameter) As String
            Get
                Return MyBase.Item(key.Name)
            End Get
            Set(value As String)
                MyBase.Item(key.Name) = value
            End Set
        End Property

        Public Overloads Sub Add(key As Parameter, value As String)
            MyBase.Add(key.Name, value)
        End Sub

        Public Overloads Sub Remove(key As Parameter)
            MyBase.Remove(key.Name)
        End Sub

        Public Overloads Function ContainsKey(key As Parameter) As Boolean
            Return MyBase.ContainsKey(key.Name)
        End Function

        Public Overloads Function TryGetValue(key As Parameter, ByRef value As String) As Boolean
            Return MyBase.TryGetValue(key.Name, value)
        End Function

        Public Function GetValueOrDefault(key As String) As String
            Return Me.GetValueOrDefault(key, String.Empty)
        End Function

        Public Function GetValueOrDefault(key As String, defaultValue As String) As String
            If MyBase.ContainsKey(key) Then
                Return MyBase.Item(key)
            End If
            Return defaultValue
        End Function

        Public Function GetValueOrDefault(key As Parameter) As String
            Return Me.GetValueOrDefault(key, String.Empty)
        End Function

        Public Function GetValueOrDefault(key As Parameter, defaultValue As String) As String
            If MyBase.ContainsKey(key.Name) Then
                Return MyBase.Item(key.Name)
            End If
            Return defaultValue
        End Function

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Shared Function ParseQueryString(query As String) As IDictionary(Of String, String)
            Dim collection As New HttpQuery

            Dim parts = query.Split("="c)

            For index As Integer = 0 To parts.Length - 1
                If index Mod 2 = 0 Then
                    collection.Add(parts(index), parts(index + 1))
                End If
            Next

            Return collection
        End Function

        Public Overrides Function ToString() As String
            Dim queryBuilder As New Text.StringBuilder

            For Each key As String In Me.Keys
                queryBuilder.Append(key)
                queryBuilder.Append("="c)
                Dim encodedValue As String = System.Uri.EscapeDataString(Me.Item(key))
                queryBuilder.Append(encodedValue)
                If key <> Me.Keys.Last Then
                    queryBuilder.Append("&"c)
                End If
            Next

            Return queryBuilder.ToString
        End Function
    End Class

End Namespace
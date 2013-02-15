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
Imports System.Globalization

Namespace IPControl

    Public Class MediaStream

        Private _type As String
        Private _number As Integer
        Private _language As CultureInfo
        Private _codec As String

        Shared Sub New()
            For Each culture In CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                CulturesByTwoLetterISOLanguageName.Item(culture.TwoLetterISOLanguageName) = culture
                CulturesByThreeLetterISOLanguageName.Item(culture.ThreeLetterISOLanguageName) = culture
            Next
        End Sub

        Private Shared Property CulturesByTwoLetterISOLanguageName As New Dictionary(Of String, CultureInfo)
        Private Shared Property CulturesByThreeLetterISOLanguageName As New Dictionary(Of String, CultureInfo)

        Public Property Type As String
            Get
                Return _type
            End Get
            Private Set(value As String)
                _type = value
            End Set
        End Property

        Public Property Number As Integer
            Get
                Return _number
            End Get
            Private Set(value As Integer)
                _number = value
            End Set
        End Property

        Public Property Language As CultureInfo
            Get
                Return _language
            End Get
            Private Set(value As CultureInfo)
                _language = value
            End Set
        End Property

        Public Property Codec As String
            Get
                Return _codec
            End Get
            Private Set(value As String)
                _codec = value
            End Set
        End Property

        Public Shared Function Parse(language As KeyValuePair(Of String, String)) As MediaStream
            Return Parse(language, Nothing)
        End Function

        Public Shared Function Parse(language As KeyValuePair(Of String, String), codec As KeyValuePair(Of String, String)) As MediaStream
            Return Parse(
                CInt(language.Key.Split("."c)(1)),
                language.Key.Split("."c)(0),
                language.Value,
                codec.Value
                )
        End Function

        Public Shared Function Parse(index As Integer, type As String, language As String, codec As String) As MediaStream
            Return New MediaStream With {
                .Number = index,
                .Type = type,
                .Language = GetCultureInfo(language),
                .Codec = codec
            }
        End Function


        ''' <summary>
        ''' Converts a three-letter language code into a <see cref="CultureInfo"/> object.
        ''' </summary>
        Private Shared Function GetCultureInfo(languageCode As String) As CultureInfo
            If languageCode.Length = 3 Then
                languageCode = GetTerminologyCode(languageCode)
            End If

            If CulturesByTwoLetterISOLanguageName.ContainsKey(languageCode) Then
                Return CulturesByTwoLetterISOLanguageName.Item(languageCode)
            ElseIf CulturesByThreeLetterISOLanguageName.ContainsKey(languageCode) Then
                Return CulturesByThreeLetterISOLanguageName.Item(languageCode)
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the terminology code for special cases where there are two codes per language.
        ''' </summary>
        Private Shared Function GetTerminologyCode(languageCode As String) As String
            Select Case languageCode.ToLower
                Case "alb" : Return "sqi"
                Case "arm" : Return "hye"
                Case "baq" : Return "eus"
                Case "bur" : Return "mya"
                Case "chi" : Return "zho"
                Case "cze" : Return "ces"
                Case "dut" : Return "nld"
                Case "fre" : Return "fra"
                Case "geo" : Return "kat"
                Case "ger" : Return "deu"
                Case "gre" : Return "ell"
                Case "ice" : Return "isl"
                Case "mac" : Return "mkd"
                Case "mao" : Return "mri"
                Case "may" : Return "msa"
                Case "per" : Return "fas"
                Case "rum" : Return "ron"
                Case "slo" : Return "slk"
                Case "tib" : Return "bod"
                Case "wel" : Return "cym"
                Case Else : Return languageCode
            End Select
        End Function

        Public Shared Operator =(a As MediaStream, b As MediaStream) As Boolean
            If Object.ReferenceEquals(a, b) Then
                Return True
            End If

            If (DirectCast(a, Object) Is Nothing) OrElse (DirectCast(b, Object) Is Nothing) Then
                Return False
            End If

            Select Case False
                Case String.Equals(a.Type, b.Type, StringComparison.InvariantCultureIgnoreCase)
                    Return False
                Case a.Number = b.Number
                    Return False
                Case String.Equals(a.Codec, b.Codec, StringComparison.InvariantCultureIgnoreCase)
                    Return False
                Case Object.Equals(a.Language, b.Language)
                    Return False
                Case Else
                    Return True
            End Select
        End Operator

        Public Shared Operator <>(a As MediaStream, b As MediaStream) As Boolean
            Return Not (a = b)
        End Operator

        Public Overrides Function Equals(obj As Object) As Boolean
            Return Me.Equals(TryCast(obj, MediaStream))
        End Function

        Public Overloads Function Equals(obj As MediaStream) As Boolean
            Return obj IsNot Nothing AndAlso (Object.ReferenceEquals(Me, obj) OrElse Me = obj)
        End Function

        Public Overrides Function ToString() As String
            Dim text As New Text.StringBuilder
            If Language IsNot Nothing Then
                text.Append(Language.DisplayName)
            Else
                text.Append("Undefined")
            End If
            If Codec.IsNotNullOrWhiteSpace Then
                text.Append("(".PadLeft(1)) : text.Append(Codec) : text.Append(")")
            End If
            Return text.ToString
        End Function
    End Class

End Namespace

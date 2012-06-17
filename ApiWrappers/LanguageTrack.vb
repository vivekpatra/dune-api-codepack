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
Imports System.Globalization

Namespace DuneUtilities.ApiWrappers

    Public Class LanguageTrack

        Private _type As String
        Private _trackNumber As Short
        Private _language As CultureInfo
        Private _codec As String

        Public Sub New(type As String, number As Short, language As CultureInfo, codec As String)
            _type = type
            _trackNumber = number
            _language = language
            _codec = codec
        End Sub

        Public ReadOnly Property Type As String
            Get
                Return _type
            End Get
        End Property

        Public ReadOnly Property Number As Short
            Get
                Return _trackNumber
            End Get
        End Property

        Public ReadOnly Property Language As CultureInfo
            Get
                Return _language
            End Get
        End Property

        Public ReadOnly Property Codec As String
            Get
                Return _codec
            End Get
        End Property

        Public Shared Function FromCommandResult(type As String, number As String, languageCode As String, codec As String) As LanguageTrack
            Dim key As Short = CShort(number)
            Dim language As CultureInfo = GetCultureInfo(languageCode)

            Return New LanguageTrack(type, key, language, codec)
        End Function

        ''' <summary>
        ''' Converts a three-letter language code into a <see cref="CultureInfo"/> object.
        ''' </summary>
        Private Shared Function GetCultureInfo(languageCode As String) As CultureInfo
            If languageCode.IsNullOrEmpty Then
                languageCode = "und"
            ElseIf languageCode.Length > 2 Then
                languageCode = GetTerminologyCode(languageCode)
            End If

            Dim length As Integer = languageCode.Length
            
            For Each culture As CultureInfo In CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                Select Case length
                    Case 2
                        If culture.TwoLetterISOLanguageName = languageCode Then
                            Return culture
                        End If
                    Case 3
                        If culture.ThreeLetterISOLanguageName = languageCode Then
                            Return culture
                        End If
                End Select
            Next

            Return CultureInfo.InvariantCulture
        End Function

        ''' <summary>
        ''' Gets the terminology code for special cases where there are two codes per language.
        ''' </summary>
        Private Shared Function GetTerminologyCode(languageCode As String) As String
            Select Case languageCode.ToLower
                Case "alb"
                    Return "sqi"
                Case "arm"
                    Return "hye"
                Case "baq"
                    Return "eus"
                Case "bur"
                    Return "mya"
                Case "chi"
                    Return "zho"
                Case "cze"
                    Return "ces"
                Case "dut"
                    Return "nld"
                Case "fre"
                    Return "fra"
                Case "geo"
                    Return "kat"
                Case "ger"
                    Return "deu"
                Case "gre"
                    Return "ell"
                Case "ice"
                    Return "isl"
                Case "mac"
                    Return "mkd"
                Case "mao"
                    Return "mri"
                Case "may"
                    Return "msa"
                Case "per"
                    Return "fas"
                Case "rum"
                    Return "ron"
                Case "slo"
                    Return "slk"
                Case "tib"
                    Return "bod"
                Case "wel"
                    Return "cym"
                Case Else
                    Return languageCode
            End Select
        End Function


        Public Overrides Function ToString() As String
            Dim text As New Text.StringBuilder
            If Language IsNot CultureInfo.InvariantCulture Then
                text.Append(Language.DisplayName)
            Else
                text.Append("Undefined")
            End If
            If Codec.IsNotNullOrWhiteSpace Then
                text.Append(Space(1))
                text.Append("(")
                text.Append(Codec)
                text.Append(")")
            End If
            Return text.ToString
        End Function
    End Class

End Namespace

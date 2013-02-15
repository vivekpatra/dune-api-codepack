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
Imports System.IO

Namespace DuneUtilities

    Public Class Playlist

        Private _media() As String

        Public Sub New(media() As String)
            _media = media
        End Sub

        Public ReadOnly Property Media As String()
            Get
                Return _media
            End Get
        End Property

        Public Sub Save(location As FileSystemInfo)
            Using stream As New FileStream(location.FullName, FileMode.Create)
                Using writer As New StreamWriter(stream, Text.Encoding.UTF8)
                    writer.Write(ToString(location.Extension))
                End Using
            End Using
        End Sub

        Public Overrides Function ToString() As String
            Return ToPlainString()
        End Function

        Private Overloads Function ToString(extension As String) As String
            If extension.EqualsInvariantIgnoreCaseAny(".m3u", ".m3u8") Then
                Return ToExtendedM3uString()
            ElseIf extension.EqualsInvariantIgnoreCase(".pls") Then
                Return ToPlsString()
            Else
                Return ToPlainString()
            End If
        End Function

        Public Function ToPlainString() As String
            Dim playlist As New Text.StringBuilder
            For Each location In _media
                playlist.AppendLine(location)
            Next
            Return playlist.ToString
        End Function

        Public Function ToExtendedM3uString() As String
            Dim playlist As New Text.StringBuilder
            playlist.AppendLine("#EXTM3U")
            For Each location In _media
                playlist.AppendFormat("#EXTINF:{0},{1}", -1, location)
                playlist.AppendLine()
                playlist.AppendLine(location)
            Next
            Return playlist.ToString
        End Function

        Public Function ToPlsString() As String
            Dim playlist As New Text.StringBuilder
            playlist.AppendLine("[playlist]")
            For index As Integer = 0 To _media.Count - 1
                playlist.AppendFormat("File{0}={1}", index + 1, _media(index)) : playlist.AppendLine()
                playlist.AppendFormat("Title{0}={1}", index + 1, _media(index)) : playlist.AppendLine()
                playlist.AppendFormat("Length{0}=-1", index + 1) : playlist.AppendLine()
            Next
            playlist.AppendFormat("NumberOfEntries={0}", _media.Count) : playlist.AppendLine()
            playlist.Append("Version=2")
            Return playlist.ToString
        End Function

    End Class

End Namespace



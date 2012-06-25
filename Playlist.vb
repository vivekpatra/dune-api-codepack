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
                    If location.Extension.EqualsInvariantIgnoreCaseAny(".m3u", ".m3u8") Then
                        WriteAsExtendedM3u(writer)
                    ElseIf location.Extension.EqualsInvariantIgnoreCase(".pls") Then
                        WriteAsPls(writer)
                    Else
                        WriteAsPlainText(writer)
                    End If
                End Using
            End Using
        End Sub

        Private Sub WriteAsPlainText(writer As StreamWriter)
            For Each location In _media
                writer.WriteLine(location)
            Next
        End Sub

        Private Sub WriteAsExtendedM3u(writer As StreamWriter)
            writer.WriteLine("#EXTM3U")

            For Each location In _media
                writer.WriteLine("#EXTINF:{0},{1}", -1, location)
                writer.WriteLine(location)
            Next
        End Sub

        Private Sub WriteAsPls(writer As StreamWriter)
            writer.WriteLine("[playlist]")

            For index As Integer = 0 To _media.Count - 1
                writer.WriteLine("File{0}={1}", index + 1, _media(index))
                writer.WriteLine("Title{0}={1}", index + 1, _media(index))
                writer.WriteLine("Length{0}=-1", index + 1)
            Next
            writer.WriteLine("NumberOfEntries={0}", _media.Count)
            writer.Write("Version=2")
        End Sub

    End Class

End Namespace



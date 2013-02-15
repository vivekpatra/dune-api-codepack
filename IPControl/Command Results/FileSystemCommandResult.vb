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
Imports System.Drawing
Imports SL.DuneApiCodePack.InOut

Namespace IPControl

    ''' <summary>
    ''' Represents the result of a list command request.
    ''' </summary>
    Public Class FileSystemCommandResult : Inherits IPCommandResult

        Private _originalPath As String
        Private _fileSystemEntries As List(Of MediaSource)

        Public Sub New(request As IIPCommand, output As XDocument, requestDateTime As DateTimeOffset)
            MyBase.New(request, output, requestDateTime)
        End Sub

        Private Shared Sub PrepareInput(source As XDocument)
            For Each element In source.Descendants(Output.ListMetadata.Name)
                If element.Value IsNot Nothing Then
                    element.Add(New XElement("size", element.Value))
                End If
                If element.Attribute("mime") IsNot Nothing Then
                    element.Add(New XElement("mime", element.Attribute("mime").Value))
                    element.Attribute("mime").Remove()
                End If
            Next
        End Sub

        Protected Overrides Sub Initialize()
            MyBase.Initialize()
            FileSystemCommandResult.PrepareInput(Me.Input)

            _fileSystemEntries = New List(Of MediaSource)
            Dim items = Me.Input.Descendants("items").Elements
            Dim parent = DirectCast(Me.Request, ListCommand).Path

            For Each item In items
                Dim name = item.Element(Output.ListName.Name).Value
                Dim type = New ListType(item.Element(Output.ListType.Name).Value)
                If type = ListType.Folder Then
                    If String.Equals(name, UnixPath.CurrentDirectory, StringComparison.InvariantCulture) Or String.Equals(name, UnixPath.ParentDirectory, StringComparison.InvariantCulture) Then
                        Continue For
                    End If
                End If
                Dim size = item.Element(Output.ListMetadata.Name).Element("size").Value
                Dim contentType = GetContentType(item.Element(Output.ListMetadata.Name).Element("mime").Value)
                _fileSystemEntries.Add(New MediaSource(parent, name, type, size, contentType))
            Next
        End Sub

        Public ReadOnly Property FileSystemEntries() As IEnumerable(Of IMediaSource)
            Get
                Return _fileSystemEntries
            End Get
        End Property

        Public Iterator Function GetDirectories() As IEnumerable(Of IMediaSource)
            For Each entry In Me.FileSystemEntries
                If entry.Type = ListType.Folder Then
                    Yield entry
                End If
            Next
        End Function

        Public Iterator Function GetFiles() As IEnumerable(Of IMediaSource)
            For Each entry In Me.FileSystemEntries
                If entry.Type <> ListType.Folder Then
                    Yield entry
                End If
            Next
        End Function

        Private Shared Function GetContentType(mime As String) As Mime.ContentType
            If String.Equals(mime, "directory", StringComparison.InvariantCultureIgnoreCase) Or String.IsNullOrWhiteSpace(mime) Then
                Return Nothing
            End If
            Return New Mime.ContentType(mime)
        End Function

    End Class

End Namespace
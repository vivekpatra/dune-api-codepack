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
Imports System.Net
Imports System.IO

Namespace Sources

    ''' <summary>
    ''' This type represents an NFS share.
    ''' </summary>
    Public Class NfsShare
        Inherits StorageDevice

        Private _exportPath As String

        ''' <param name="host">The NFS server.</param>
        ''' <param name="exportPath">The export path as defined in the NFS exports file on the NFS server.</param>
        Public Sub New(host As IPAddress, exportPath As String)
            MyBase.New(host)
            exportPath = exportPath.Trim("/"c)
        End Sub

        ''' <summary>
        ''' Gets the export path.
        ''' </summary>
        Public ReadOnly Property ExportPath As String
            Get
                Return _exportPath
            End Get
        End Property

        ''' <summary>
        ''' Gets the media url for the specified path.
        ''' </summary>
        ''' <param name="path">A relative path (relative to the export path if set, otherwise relative to the host address).</param>
        ''' <returns>The media URL in nfs://host[:/export_path]:/share_path format.</returns>
        Public Overloads Function GetMediaUrl(path As String) As String
            Dim mediaUrl As New Text.StringBuilder

            mediaUrl.Append(Me.GetRoot())

            If path.IsNotNullOrWhiteSpace Then
                mediaUrl.Append(":/")
                mediaUrl.Append(path.TrimStart("/"c))
            End If

            Return mediaUrl.ToString
        End Function

        ''' <summary>
        ''' Gets the media URL for the root of this NFS share.
        ''' </summary>
        ''' <returns>The media URL in nfs://host[:/export_path] format.</returns>
        Public Function GetRoot() As String
            Dim mediaUrl As New Text.StringBuilder

            mediaUrl.Append("nfs://")

            mediaUrl.Append(Me.Host.ToString)

            If Not String.IsNullOrWhiteSpace(ExportPath) Then
                mediaUrl.Append(":/")
                mediaUrl.Append(ExportPath)
            End If

            Return mediaUrl.ToString
        End Function

    End Class

End Namespace
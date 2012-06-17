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
Imports System.Net

Namespace Sources

    Public Class NamedShare
        Inherits StorageDevice

        Private _name As String
        Private _share As IO.DirectoryInfo

        ''' <param name="host">The host where the network share is hosted.</param>
        ''' <param name="name">The name of the network folder as defined on the Dune device.</param>
        ''' <param name="path">The path to the named network folder.</param>
        ''' <remarks></remarks>
        Public Sub New(host As IPHostEntry, name As String, path As IO.FileSystemInfo)
            MyBase.New(host)
            _name = name
            _share = path.ToDirectoryInfo
            _root = path.GetRoot
        End Sub

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Public Property Share As IO.DirectoryInfo
            Get
                Return _share
            End Get
            Set(value As IO.DirectoryInfo)
                _share = value
            End Set
        End Property

        Public Overrides Function GetMediaUrl() As String
            Dim mediaUrl As New Text.StringBuilder

            mediaUrl.Append("network_folder://")
            mediaUrl.Append(Name)

            Return mediaUrl.ToString
        End Function

        Public Overloads Function GetMediaUrl(path As IO.FileSystemInfo) As String
            If Not path.FullName.Contains(Share.FullName) Then
                Throw New ArgumentException("The specified path is not a member of this network share.", "path")
            Else
                Dim mediaUrl As New Text.StringBuilder

                mediaUrl.Append(Me.GetMediaUrl)

                Dim suffix As String = path.FullName.Replace(Share.FullName, String.Empty).Replace("\"c, "/"c)

                mediaUrl.Append("/")
                mediaUrl.Append(suffix)

                Return mediaUrl.ToString
            End If
        End Function

        Public Function TryGetMediaUrl(path As IO.FileSystemInfo, ByRef mediaUrl As String) As Boolean
            If Not path.FullName.Contains(Share.FullName) Then
                Return False
            Else
                mediaUrl = GetMediaUrl(path)
                Return True
            End If
        End Function
    End Class

End Namespace
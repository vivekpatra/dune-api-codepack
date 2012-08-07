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
Imports System.IO
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' This class is a stub. This class is a wraps the dune_folder concept.
    ''' </summary>
    <Serializable()>
    Public Class Folder
        Inherits IO.FileSystemInfo

        ' TODO: implement dune_folder.txt functionallity
        ' http://dune-hd.com/firmware/misc/dune_folder_howto.txt

        Private _location As Uri
        Private _directoryInfo As DirectoryInfo
        Private _mediaUrl As String

        Public Sub New()
            ' default constructor
        End Sub

        Public Sub New(location As Uri)
            _location = location
            If location.IsUnc Then
                _directoryInfo = New DirectoryInfo(location.AbsolutePath)
            End If
        End Sub

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Overrides Sub Delete()
            If Location.IsUnc Then
                Directory.Delete(Me.FullName)
            Else
                Throw New NotImplementedException("Can't delete non-UNC location.")
            End If
        End Sub

        Public Overrides ReadOnly Property Exists As Boolean
            Get
                If Location.IsUnc Then
                    Return Directory.Exists(Me.FullName)
                Else
                    Throw New NotImplementedException("Can't check if non-UNC location exists.")
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return Path.GetDirectoryName(Location.LocalPath)
            End Get
        End Property

        Public ReadOnly Property Location As Uri
            Get
                Return _location
            End Get
        End Property

        Public ReadOnly Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
        End Property


        Public Shared Function GetDuneFolderSettings(path As Uri) As NameValueCollection
            Dim settings As New NameValueCollection

            Try
                Dim duneFolderTxt As New MemoryStream

                If path.Scheme.EqualsInvariantIgnoreCaseAny(Uri.UriSchemeFile, Uri.UriSchemeHttp, Uri.UriSchemeFtp) Then
                    Dim request = Net.WebRequest.Create(path)
                    Using response = request.GetResponse
                        response.GetResponseStream.CopyTo(duneFolderTxt)
                    End Using
                Else
                    File.OpenRead(path.LocalPath + "\dune_folder.txt").CopyTo(duneFolderTxt)
                End If
                duneFolderTxt.Position = 0

                'Dim duneFolderTxt = File.OpenRead(path.LocalPath + "\dune_folder.txt")

                Using reader As New StreamReader(duneFolderTxt)
                    Do Until reader.EndOfStream
                        If ChrW(reader.Peek) = "#"c Then ' ignore comment
                            reader.ReadLine()
                        Else ' get name=value pair
                            Dim nameValuePair As String = reader.ReadLine
                            If nameValuePair.Contains("=") AndAlso nameValuePair.IndexOf("="c) = nameValuePair.LastIndexOf("="c) Then
                                Dim name As String = nameValuePair.Split("="c)(0).Trim.ToLowerInvariant
                                Dim value As String = nameValuePair.Split("="c)(1).Trim
                                settings.Add(name, value)
                            End If
                        End If
                    Loop
                End Using

                duneFolderTxt.Dispose()
            Catch ex As IOException
                MsgBox(ex.Message)
            End Try

            Return settings
        End Function

    End Class

End Namespace
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
Imports SL.DuneApiCodePack.DuneUtilities
Imports System.Net
Imports System.Threading.Tasks
Imports System.IO

Public Class FtpClient
    Private _target As Dune
    Private _root As Uri
    Private _workingDirectory As Uri

    Public Sub New(target As Dune)
        _target = target
        If target.Address IsNot Nothing Then
            _root = New Uri("ftp://" + target.Address.ToString + "/")
        Else
            _root = New Uri("ftp://" + target.HostName + "/")
        End If
        _workingDirectory = _root
    End Sub

    ''' <summary>
    ''' Gets the root URI of the FTP server.
    ''' </summary>
    Public ReadOnly Property Root As Uri
        Get
            Return _root
        End Get
    End Property

    ''' <summary>
    ''' Gets a directory listing.
    ''' </summary>
    Public Function ListDirectory(directory As Uri) As String()
        Dim request = FtpWebRequest.Create(directory)
        request.Method = WebRequestMethods.Ftp.ListDirectory
        Dim contents As New ArrayList
        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Using reader As New IO.StreamReader(response.GetResponseStream)
                Do Until reader.EndOfStream
                    contents.Add(reader.ReadLine)
                Loop
            End Using
        End Using

        Return DirectCast(contents.ToArray(GetType(String)), String())
    End Function

    ''' <summary>
    ''' Gets a directory listing as well as filesystem details.
    ''' </summary>
    Public Function ListDirectoryDetails(directory As Uri) As String()
        Dim request = FtpWebRequest.Create(directory)
        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails
        Dim contents As New ArrayList
        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Using reader As New IO.StreamReader(response.GetResponseStream)
                Do Until reader.EndOfStream
                    contents.Add(reader.ReadLine)
                Loop
            End Using
        End Using
        Return DirectCast(contents.ToArray(GetType(String)), String())
    End Function

    ''' <summary>
    ''' Gets the filesize of the specified URI.
    ''' </summary>
    Public Function GetFileSize(file As Uri) As Long
        Dim request = FtpWebRequest.Create(file)
        request.Method = WebRequestMethods.Ftp.GetFileSize
        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Return response.ContentLength
        End Using
    End Function

    ''' <summary>
    ''' Downloads the specified file to the specified target location.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="output"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DownloadFile(source As Uri, output As Uri) As FtpStatusCode
        Dim request = FtpWebRequest.Create(source)
        request.Method = WebRequestMethods.Ftp.DownloadFile

        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Using inputStream = response.GetResponseStream
                Using outputStream = New FileStream(output.AbsolutePath, FileMode.Create)
                    Do
                        Dim buffer(8192) As Byte
                        Dim buffered = inputStream.Read(buffer, 0, buffer.Length)
                        outputStream.Write(buffer, 0, buffered)
                    Loop While outputStream.Position < response.ContentLength
                End Using
            End Using
            Return response.StatusCode
        End Using
    End Function

    ''' <summary>
    ''' Uploads the specified file to the specified target location.
    ''' </summary>
    Public Function UploadFile(target As Uri, file As IO.FileInfo) As FtpStatusCode
        Dim request = FtpWebRequest.Create(target)
        request.Method = WebRequestMethods.Ftp.UploadFile

        Using writer As New IO.BinaryWriter(request.GetRequestStream, Text.Encoding.UTF8)
            Using stream = file.OpenRead
                Do
                    Dim buffer(16384) As Byte
                    Dim buffered = stream.Read(buffer, 0, buffer.Length)
                    writer.Write(buffer, 0, buffered)
                Loop Until stream.Position = stream.Length
            End Using
        End Using

        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Return response.StatusCode
        End Using
    End Function

    ''' <summary>
    ''' Permanently deletes the specified file.
    ''' </summary>
    Public Function DeleteFile(file As Uri) As FtpStatusCode
        Dim request As FtpWebRequest = CType(FtpWebRequest.Create(file), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.DeleteFile

        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Return response.StatusCode
        End Using
    End Function

    Public Function MakeDirectory(directory As Uri) As FtpStatusCode
        Dim request As FtpWebRequest = CType(FtpWebRequest.Create(directory), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.MakeDirectory

        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Return response.StatusCode
        End Using
    End Function

    Public Function GetDateTimeStamp(file As Uri) As Date
        Dim request As FtpWebRequest = CType(FtpWebRequest.Create(file), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.GetDateTimestamp

        Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            Return response.LastModified
        End Using
    End Function

End Class

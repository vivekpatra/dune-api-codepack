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
Imports System.ComponentModel
Imports System.IO

''' <summary>
''' Provides information about firmware versions.
''' </summary>
Public Class FirmwareProperties
    Private Const api As String = "http://dune-hd.com/firmware/online_upgrade/" ' + productid.txt

    Private _public As Boolean
    Private _version As String
    Private _uncompressedLocation As Uri
    Private _uncompressedLength As Long
    Private _gzipLocation As Uri
    Private _gzipLength As Long
    Private _zipLocation As Uri
    Private _zipLength As Long
    Private _beta As Boolean
    Private _approximateLength As Integer
    Private _buildDate As Date

    Private Sub New(version As String, location As String, stability As String, size As String)
        _version = version
        If Not String.Equals(location, "-") Then
            _public = True

            _gzipLocation = New Uri(location)
            _zipLocation = New Uri(location.Replace(".gz", ".zip"))
            _uncompressedLocation = New Uri(location.Remove(location.IndexOf(".gz")))
        End If
        _beta = String.Equals(stability, "non-stable", StringComparison.InvariantCultureIgnoreCase)
        _approximateLength = Integer.Parse(size) \ 2
    End Sub

    Private Shared Function GetApi(product As ProductID) As Uri
        If product Is Nothing Then
            Throw New ArgumentNullException("product")
        End If
        Dim builder As New UriBuilder("http://dune-hd.com/")
        builder.Path = String.Concat("firmware/online_upgrade/", product.Value, ".txt")
        Return builder.Uri
    End Function

    Public Shared Function FromHost(host As Dune) As IEnumerable(Of FirmwareProperties)
        Try
            Return GetFirmwareCollectionAsync(host.ProductId).Result
        Catch ex As AggregateException
            Throw ex.InnerException
        End Try
    End Function

    Public Shared Async Function GetFirmwareCollectionAsync(product As ProductID) As Task(Of IEnumerable(Of FirmwareProperties))
        If product Is Nothing Then
            Throw New ArgumentNullException("product")
        End If

        Dim values As New List(Of FirmwareProperties)

        Try
            Dim client = IPControlClient.GetInstance()
            Dim api = FirmwareProperties.GetApi(product)

            Dim source = Await client.GetStringAsync(api).ConfigureAwait(False)

            If String.IsNullOrEmpty(source) Then
                Return values
            End If

            Using reader As New StringReader(source)
                Do
                    Dim entry = (Await reader.ReadLineAsync.ConfigureAwait(False)).Split

                    Dim version = entry(Firmware.Version)
                    Dim location = entry(Firmware.Location)
                    Dim stability = entry(Firmware.Stability)
                    Dim size = entry(Firmware.Size)

                    values.Add(New FirmwareProperties(version, location, stability, size))
                Loop Until reader.Peek = -1
            End Using

        Catch ex As Http.HttpRequestException
            Debug.Print(ex.Message)
        End Try

        Return values
    End Function

    Private Property Host As Dune

    ''' <summary>
    ''' Gets the firmware version string.
    ''' </summary>
    <Category("Firmware information")>
    Public Property Version As String
        Get
            Return _version
        End Get
        Set(value As String)
            Throw New ArgumentException("Please don't try to change this value!")
        End Set
    End Property

    ''' <summary>
    ''' Gets whether this is a non-stable release.
    ''' </summary>
    <Category("Firmware information")>
    <DisplayName("Beta release")>
    Public ReadOnly Property Beta As Boolean
        Get
            Return _beta
        End Get
    End Property

    ''' <summary>
    ''' Gets the approximate filesize in megabytes.
    ''' </summary>
    <DisplayName("Approximate filesize (MiB)")>
    <Category("Firmware information")>
    Public ReadOnly Property ApproximateFileSize As Integer
        Get
            Return _approximateLength
        End Get
    End Property

    ''' <summary>
    ''' Gets the build date of the firmware file.
    ''' </summary>
    <Category("Firmware information")>
    <DisplayName("Date")>
    Public ReadOnly Property BuildDate As Date
        Get
            If _buildDate = Nothing Then
                _buildDate = GetBuildDate(Me.Version)
            End If
            Return _buildDate
        End Get
    End Property

    ''' <summary>
    ''' Gets the download link of the uncompressed DFF file.
    ''' </summary>
    <Category("Uncompressed")>
    <DisplayName("URL")>
    Public ReadOnly Property UncompressedLocation As Uri
        Get
            Return _uncompressedLocation
        End Get
    End Property

    ''' <summary>
    ''' Gets the filesize of the uncompressed DFF file in bytes.
    ''' </summary>
    <Category("Uncompressed")>
    <DisplayName("Size (bytes)")>
    Public ReadOnly Property UncompressedLength As Long
        Get
            If _uncompressedLength = 0 Then
                Dim length = GetLength(Me.UncompressedLocation)
                If length > 0 Then
                    _uncompressedLength = length
                Else
                    _uncompressedLength = CLng(Me.ApproximateFileSize) << 20
                End If
            End If
            Return _uncompressedLength
        End Get
    End Property

    ''' <summary>
    ''' Gets the download link of the zipped DFF file.
    ''' </summary>
    <Category("Zip")>
    <DisplayName("URL")>
    Public ReadOnly Property ZipLocation As Uri
        Get
            Return _zipLocation
        End Get
    End Property

    ''' <summary>
    ''' Gets the filesize of the zipped DFF file in bytes.
    ''' </summary>
    <Category("Zip")>
    <DisplayName("Size (bytes)")>
    Public ReadOnly Property ZipLength As Long
        Get
            If _zipLength = 0 Then
                _zipLength = GetLength(_zipLocation)
            End If
            Return _zipLength
        End Get
    End Property

    ''' <summary>
    ''' Gets the download link of the gzipped DFF file.
    ''' </summary>
    <Category("Gzip")>
    <DisplayName("URL")>
    Public ReadOnly Property GzipLocation As Uri
        Get
            Return _gzipLocation
        End Get
    End Property

    ''' <summary>
    ''' Gets the filesize of the gzipped DFF file in bytes.
    ''' </summary>
    <Category("Gzip")>
    <DisplayName("Size (bytes)")>
    Public ReadOnly Property GzipLength As Long
        Get
            If _gzipLength = 0 Then
                _gzipLength = GetLength(_gzipLocation)
            End If
            Return _gzipLength
        End Get
    End Property

    ''' <summary>
    ''' Gets the size of a file on a remote location.
    ''' </summary>
    ''' <param name="file">The link to the file.</param>
    ''' <returns>The filesize in bytes.</returns>
    Private Function GetLength(file As Uri) As Long
        If file Is Nothing Then
            Return 0
        End If
        Dim client = IPControlClient.GetInstance()
        Using response = client.SendAsync(New HttpRequestMessage(HttpMethod.Head, file), HttpCompletionOption.ResponseHeadersRead).Result
            Return response.Content.Headers.ContentLength.GetValueOrDefault()
        End Using
    End Function

    Private Shared Function GetBuildDate(version As String) As Date
        Dim dt As Date
        Dim format As String = "yyMMdd_HHmm"

        If DateTime.TryParseExact(version.Substring(0, format.Length), format, Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dt) Then
            Return dt
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function ToString() As String
        Return Version
    End Function

    Private Enum Firmware
        Version = 0
        Location = 1
        Stability = 2
        Size = 3
    End Enum

End Class
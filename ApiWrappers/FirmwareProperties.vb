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
Imports System.IO
Imports System.ComponentModel
Imports System.Threading.Tasks

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' Provides information about firmware versions.
    ''' </summary>
    Public Class FirmwareProperties
        Private Const BaseUri As String = "http://dune-hd.com/firmware/online_upgrade/" ' + productid.txt

        Private _firmware As String
        Private _uncompressedLocation As Uri
        Private _uncompressedLength As Long
        Private _gzipLocation As Uri
        Private _gzipLength As Long
        Private _zipLocation As Uri
        Private _zipLength As Long
        Private _beta As Boolean
        Private _approximateLength As Integer

        Private Sub New(firmware As String)
            Dim pieces() As String = firmware.Split(Convert.ToChar(32))

            _firmware = pieces(0)

            _gzipLocation = New Uri(pieces(1))
            _zipLocation = New Uri(pieces(1).Replace(".gz", ".zip"))
            _uncompressedLocation = New Uri(pieces(1).Replace(".gz", String.Empty))

            Dim t1 As Action = Sub() _gzipLength = GetSize(_gzipLocation)
            Dim t2 As Action = Sub() _zipLength = GetSize(_zipLocation)
            Dim t3 As Action = (Sub() _uncompressedLength = GetSize(_uncompressedLocation))

            Parallel.Invoke(t1, t2, t3)

            _beta = String.Equals(pieces(2), "non-stable")
            _approximateLength = CInt(Integer.Parse(pieces(3)) / 2)
        End Sub

        ''' <summary>
        ''' Gets the firmware version string.
        ''' </summary>
        <Category("Firmware information")>
        Public Property Version As String
            Get
                Return _firmware
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
                Dim dt As Date
                Dim format As String = "yyMMdd_HHmm"

                If DateTime.TryParseExact(Version.Substring(0, format.Length), format, Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dt) Then
                    Return dt
                Else
                    Return Nothing
                End If
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
                Return _uncompressedLength
            End Get
        End Property

        <Category("Uncompressed")>
        <DisplayName("Size (kibibytes)")>
        Public ReadOnly Property UncompressedLengthKibibyte As Double
            Get
                Return UncompressedLength / 1024
            End Get
        End Property

        <Category("Uncompressed")>
        <DisplayName("Size (mebibytes)")>
        Public ReadOnly Property UncompressedLengthMebibyte As Double
            Get
                Return UncompressedLengthKibibyte / 1024
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
                Return _zipLength
            End Get
        End Property

        <Category("Zip")>
        <DisplayName("Size (kibibytes)")>
        Public ReadOnly Property ZipLengthKibibyte As Double
            Get
                Return ZipLength / 1024
            End Get
        End Property

        <Category("Zip")>
        <DisplayName("Size (mebibytes)")>
        Public ReadOnly Property ZipLengthMebibyte As Double
            Get
                Return ZipLengthKibibyte / 1024
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
                Return _gzipLength
            End Get
        End Property

        <Category("Gzip")>
        <DisplayName("Size (kibibytes)")>
        Public ReadOnly Property GzipLengthKibibyte As Double
            Get
                Return GzipLength / 1024
            End Get
        End Property

        <Category("Gzip")>
        <DisplayName("Size (mebibytes)")>
        Public ReadOnly Property GzipLengthMebibyte As Double
            Get
                Return GzipLengthKibibyte / 1024
            End Get
        End Property

        ''' <summary>
        ''' Gets the size of a file on a remote location.
        ''' </summary>
        ''' <param name="file">The link to the file.</param>
        ''' <returns>The filesize in bytes.</returns>
        Private Function GetSize(file As Uri) As Long
            Dim request As WebRequest = DirectCast(HttpWebRequest.Create(file), WebRequest)
            request.Method = "HEAD"

            Try
                Using response As WebResponse = DirectCast(request.GetResponse, WebResponse)
                    Return response.ContentLength
                End Using
            Catch ex As WebException
                Return 0
            End Try
        End Function

        Public Shared Function GetAvailableFirmwares(product As String) As FirmwareProperties()
            Dim results() As String

            Dim firmwares As New ArrayList()

            Using client As New WebClient
                Using reader As StringReader = New StringReader(client.DownloadString(New Uri(BaseUri + product + ".txt")))
                    Dim delimiters() As String = {vbCr, vbLf}
                    results = reader.ReadToEnd.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                End Using
            End Using

            ' get the firmware properties and add to the list
            Parallel.ForEach(results, Sub(firmware)
                                          Dim properties As New FirmwareProperties(firmware)
                                          firmwares.Add(properties)
                                      End Sub)

            Dim unsorted As FirmwareProperties() = CType(firmwares.ToArray(GetType(FirmwareProperties)), FirmwareProperties())
            Dim sorted = From firmware In unsorted
                         Select firmware
                         Order By firmware.Version Descending.ToArray

            Return sorted
        End Function

        Public Shared Function GetAvailableFirmwaresAsync(product As String) As Task(Of FirmwareProperties())
            Return Task(Of FirmwareProperties()).Factory.StartNew(Function() GetAvailableFirmwares(product))
        End Function

        Public Overrides Function ToString() As String
            Return Version
        End Function
    End Class

End Namespace

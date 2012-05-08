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
        Private _number As Integer

        Private Sub New(ByVal firmware As String)
            Dim pieces() As String = firmware.Split(Convert.ToChar(32))
            Dim client As New WebClient

            _firmware = pieces(0)

            _gzipLocation = New Uri(pieces(1))
            _gzipLength = GetSize(_gzipLocation)

            _zipLocation = New Uri(pieces(1).Replace(".gz", ".zip"))
            _zipLength = GetSize(_zipLocation)

            _uncompressedLocation = New Uri(pieces(1).Replace(".gz", String.Empty))
            _uncompressedLength = GetSize(_uncompressedLocation)

            _beta = String.Equals(pieces(2), "non-stable")
            _number = Integer.Parse(pieces(3))
        End Sub

        ''' <summary>
        ''' Gets the firmware version string.
        ''' </summary>
        Public ReadOnly Property Version As String
            Get
                Return _firmware
            End Get
        End Property

        ''' <summary>
        ''' Gets whether this is a non-stable release.
        ''' </summary>
        Public ReadOnly Property Beta As Boolean
            Get
                Return _beta
            End Get
        End Property

        ''' <summary>
        ''' ???
        ''' </summary>
        Public ReadOnly Property SomeWeirdNumberThatNobodyKnowsTheMeaningOf As Integer
            Get
                Return _number
            End Get
        End Property

        ''' <summary>
        ''' Gets the download link of the uncompressed DFF file.
        ''' </summary>
        Public ReadOnly Property UncompressedLocation As Uri
            Get
                Return _uncompressedLocation
            End Get
        End Property

        ''' <summary>
        ''' Gets the filesize of the uncompressed DFF file in bytes.
        ''' </summary>
        Public ReadOnly Property UncompressedLEngth As Long
            Get
                Return _uncompressedLength
            End Get
        End Property

        ''' <summary>
        ''' Gets the download link of the zipped DFF file.
        ''' </summary>
        Public ReadOnly Property ZipLocation As Uri
            Get
                Return _zipLocation
            End Get
        End Property

        ''' <summary>
        ''' Gets the filesize of the zipped DFF file in bytes.
        ''' </summary>
        Public ReadOnly Property ZipLength As Long
            Get
                Return _zipLength
            End Get
        End Property

        ''' <summary>
        ''' Gets the download link of the gzipped DFF file.
        ''' </summary>
        Public ReadOnly Property GzipLocation As Uri
            Get
                Return _gzipLocation
            End Get
        End Property

        ''' <summary>
        ''' Gets the filesize of the gzipped DFF file in bytes.
        ''' </summary>
        Public ReadOnly Property GzipLength As Long
            Get
                Return _gzipLength
            End Get
        End Property

        ''' <summary>
        ''' Gets the build date of the firmware file.
        ''' </summary>
        Public ReadOnly Property BuildDate As Date
            Get
                Dim dt As Date
                Dim format As String = "yyMMdd_HHmm"

                DateTime.TryParseExact(Version.Substring(0, format.Length), format, Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dt)

                Return dt
            End Get
        End Property

        ''' <summary>
        ''' Gets the size of a file on a remote location.
        ''' </summary>
        ''' <param name="file">The link to the file.</param>
        ''' <returns>The filesize in bytes.</returns>
        Private Function GetSize(ByVal file As Uri) As Long
            Dim request As WebRequest = DirectCast(HttpWebRequest.Create(file), WebRequest)
            request.Method = "HEAD"

            Try
                Using response As WebResponse = DirectCast(request.GetResponse, WebResponse)
                    Return response.ContentLength
                End Using
            Catch ex As Exception

            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Downloads a list of available firmware versions for a specific device.
        ''' </summary>
        ''' <param name="product">The device's product ID (see <see cref="Constants.ProductIDs"/> for a list of constants that you can use).</param>
        ''' <returns>A list of FirmwareInfo objects.</returns>
        Public Shared Function GetAvailableFirmwares(ByVal product As String) As List(Of FirmwareProperties)
            Dim client As New WebClient
            Dim firmwares As New List(Of FirmwareProperties)

            Dim reader As New StringReader(client.DownloadString(New Uri(BaseUri + product + ".txt")))

            Do
                Dim firmware As String = reader.ReadLine

                If firmware Is Nothing Then
                    Exit Do
                End If

                firmwares.Add(New FirmwareProperties(firmware))
            Loop

            Return firmwares
        End Function


        ''' <summary>
        ''' Downloads a list of available firmware versions for a specific device.
        ''' </summary>
        ''' <param name="product">The device's product ID (see <see cref="Constants.ProductIDs"/> for a list of constants that you can use).</param>
        ''' <returns>A list of FirmwareInfo objects.</returns>
        Public Shared Function GetAvailableFirmwaresAsync(ByVal product As String) As Task(Of List(Of FirmwareProperties))
            Dim client As New WebClient
            Dim firmwares As New List(Of FirmwareProperties)
            Dim request As WebRequest = WebRequest.Create(New Uri(BaseUri + product + ".txt"))
            Dim response As WebResponse

            Dim downloadTask As Task(Of List(Of FirmwareProperties)) =
                Task.Factory.StartNew(Sub() response = Task.Factory.FromAsync(AddressOf request.BeginGetResponse, AddressOf request.EndGetResponse, Nothing, Nothing).Result) _
            .ContinueWith(Of List(Of FirmwareProperties))(Function(antecedent)
                                                              Dim streamReader As New StreamReader(response.GetResponseStream)
                                                              Dim stringReader As New StringReader(streamReader.ReadToEnd)
                                                              Do
                                                                  Dim firmware As String = stringReader.ReadLine
                                                                  If firmware Is Nothing Then
                                                                      Exit Do
                                                                  End If
                                                                  firmwares.Add(New FirmwareProperties(firmware))
                                                              Loop
                                                              Return firmwares
                                                          End Function)

            Return downloadTask
        End Function

        Public Overrides Function ToString() As String
            Return _firmware + " (" + Math.Round((_uncompressedLength / 1024 ^ 2), 1).ToString + " MiB)"
        End Function
    End Class

End Namespace

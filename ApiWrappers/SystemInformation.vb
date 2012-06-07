Imports SL.DuneApiCodePack.DuneUtilities

Namespace DuneUtilities.ApiWrappers

    Public Class SystemInformation

        Private _host As Dune
        Private _firmwareVersion As String
        Private _serialNumber As String
        Private _productId As String
        Private _bootTime As Date


        Private Sub New(host As Dune)
            _host = host
        End Sub

        Public ReadOnly Property FirmareVersion As String
            Get
                Return _firmwareVersion
            End Get
        End Property

        Public ReadOnly Property Serial As String
            Get
                Return _serialNumber
            End Get
        End Property

        Public ReadOnly Property ProductId As String
            Get
                Return _productId
            End Get
        End Property

        Public ReadOnly Property BootTime As Date
            Get
                If Host.Connected Then
                    Return _bootTime
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property Host As Dune
            Get
                Return _host
            End Get
        End Property

        Public Shared Function FromHost(host As Dune) As SystemInformation
            If host.TelnetEnabled Then
                Dim info As New SystemInformation(host)
                info.GetSysinfo()
                info.GetBootTime()
                Return info
            Else
                Throw New ArgumentException("The specified host does not have telnet enabled.")
            End If
        End Function

        Public Sub Clear()
            _productId.Clear()
            _serialNumber.Clear()
            _firmwareVersion.Clear()
            _bootTime = Nothing
        End Sub

        ''' <summary>
        ''' Gets system information (using a Telnet connection).
        ''' </summary>
        Private Sub GetSysinfo()
            Try
                Dim file As String = Host.TelnetClient.ExecuteCommand("cat /tmp/sysinfo.txt")
                Dim split() As String = {"product_id: ", "serial_number: ", "firmware_version: "}

                Dim info() As String = file.Split(split, StringSplitOptions.RemoveEmptyEntries)

                _productId = info(0).TrimEnd
                _serialNumber = info(1).TrimEnd
                _firmwareVersion = info(2).TrimEnd
            Catch ex As Exception
                Console.WriteLine("GetSysinfo error: " + ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the uptime of the device (using a Telnet connection).
        ''' </summary>
        Private Sub GetBootTime()
            Try
                Dim separator As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
                Dim uptimeSeconds As String = Host.TelnetClient.ExecuteCommand("cat /proc/uptime").Split(" "c).GetValue(0).ToString.Replace(".", separator)
                Dim uptime As TimeSpan = TimeSpan.FromSeconds(Double.Parse(uptimeSeconds))

                _bootTime = Now.Subtract(uptime)
            Catch ex As Exception
                Console.WriteLine("GetBootTime error: " + ex.Message)
            End Try
        End Sub

        Public Sub Refresh()
            Clear()
            GetSysinfo()
            GetBootTime()
        End Sub

    End Class

End Namespace

﻿#Region "License"
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

Namespace DuneUtilities.ApiWrappers

    Public Class SystemInformation

        Private _host As Dune
        Private _firmware As FirmwareProperties
        Private _latestFirmware As FirmwareProperties
        Private _availableFirmwares As List(Of FirmwareProperties)
        Private _serialNumber As String
        Private _productId As String
        Private _bootTime As Date

        Private Sub New(host As Dune)
            _host = host
        End Sub

        Public ReadOnly Property Firmware As FirmwareProperties
            Get
                Return _firmware
            End Get
        End Property

        Public ReadOnly Property AvailableFirmwares As ReadOnlyCollection(Of FirmwareProperties)
            Get
                If _availableFirmwares Is Nothing Then
                    If ProductId.IsNotNullOrEmpty Then
                        _availableFirmwares = FirmwareProperties.GetAvailableFirmwares(ProductId).ToList
                    Else
                        _availableFirmwares = New List(Of FirmwareProperties)
                    End If
                End If

                Return _availableFirmwares.AsReadOnly
            End Get
        End Property

        Public ReadOnly Property LastestFirmware As FirmwareProperties
            Get
                If _latestFirmware Is Nothing And AvailableFirmwares.Count > 0 Then
                    _latestFirmware = AvailableFirmwares.OrderByDescending(Function(firmware) firmware.BuildDate).First
                End If
                Return _latestFirmware
            End Get
        End Property

        Public ReadOnly Property UpdateAvailable As Boolean?
            Get
                If Firmware IsNot Nothing And AvailableFirmwares.Count > 0 Then
                    Return Firmware.Version <> LastestFirmware.Version
                Else
                    Return Nothing
                End If
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
                If Host.IsConnected Then
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
            Dim info As SystemInformation = Nothing

            If host.TelnetClient.Connected Then
                info = New SystemInformation(host)
                info.GetSysinfo()
                info.GetBootTime()
            Else
                Console.WriteLine("The specified host does not have telnet enabled.")
            End If

            Return info
        End Function

        Public Sub Clear()
            _productId.Clear()
            _serialNumber.Clear()
            _firmware = Nothing
            _bootTime = Nothing
        End Sub

        ''' <summary>
        ''' Gets system information (using a Telnet connection).
        ''' </summary>
        Private Sub GetSysinfo()
            Try
                Dim file As String = Host.TelnetClient.ExecuteCommand("cat /tmp/sysinfo.txt")

                If Not file.Contains("No such file or directory") Then
                    Dim split() As String = {"product_id: ", "serial_number: ", "firmware_version: "}

                    Dim info() As String = file.Split(split, StringSplitOptions.RemoveEmptyEntries)

                    _productId = info(0).TrimEnd
                    _serialNumber = info(1).TrimEnd

                    Dim firmware = info(2).TrimEnd
                    _firmware = FirmwareProperties.Create(_productId, firmware)
                End If
            Catch ex As Net.Sockets.SocketException
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
            Catch ex As Net.Sockets.SocketException
                Console.WriteLine("GetBootTime error: " + ex.Message)
            End Try
        End Sub

        Public Sub Refresh()
            Clear()
            GetSysinfo()
            GetBootTime()
            _availableFirmwares = FirmwareProperties.GetAvailableFirmwares(ProductId).ToList
        End Sub

    End Class

End Namespace

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
Imports SL.DuneApiCodePack.DuneUtilities

Namespace DuneUtilities.ApiWrappers

    Public Class SystemInformation

        Private _host As Dune
        Private _firmware As String
        Private _serialNumber As String
        Private _productID As ProductID
        Private _bootTime As Date

        Private Sub New(host As Dune)
            _host = host
        End Sub

        Public Property Firmware As String
            Get
                Return _firmware
            End Get
            Private Set(value As String)
                _firmware = value
            End Set
        End Property

        Public Property SerialNumber As String
            Get
                Return _serialNumber
            End Get
            Private Set(value As String)
                _serialNumber = value
            End Set
        End Property

        Public Property ProductID As ProductID
            Get
                Return _productID
            End Get
            Private Set(value As ProductID)
                _productID = value
            End Set
        End Property

        Public Property BootTime As Date
            Get
                If Host.IsConnected Then
                    Return _bootTime
                Else
                    Return Nothing
                End If
            End Get
            Private Set(value As Date)
                _bootTime = value
            End Set
        End Property

        Public ReadOnly Property Host As Dune
            Get
                Return _host
            End Get
        End Property

        Public Shared Function FromHost(host As Dune) As SystemInformation
            Dim info As SystemInformation = Nothing

            If host.TelnetClient.IsConnected Then
                info = New SystemInformation(host)
                info.SetSysInfo()
                info.BootTime = host.TelnetClient.GetBootTimeAsync.Result
            Else
                Console.WriteLine("The specified host does not have telnet enabled.")
            End If

            Return info
        End Function

        ''' <summary>
        ''' Gets system information (using a Telnet connection).
        ''' </summary>
        Private Sub SetSysInfo()
            Try
                Dim file As String = Host.TelnetClient.ExecuteCommand("cat /tmp/sysinfo.txt")

                If file.IsNullOrWhiteSpace OrElse file.Contains("No such file or directory") Then
                    Exit Sub
                End If

                Dim properties = file.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

                Me.ProductID = New ProductID(properties(0).Split.LastOrDefault)
                Me.SerialNumber = properties(1).Split.LastOrDefault
                Me.Firmware = properties(2).Split.LastOrDefault

            Catch ex As Net.Sockets.SocketException
                Console.WriteLine("SetSysinfo error: " + ex.Message)
            End Try
        End Sub

    End Class

End Namespace

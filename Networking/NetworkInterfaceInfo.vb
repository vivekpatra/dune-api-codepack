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
Imports System.Net.Sockets
Imports System.Net.NetworkInformation
Imports System.Threading.Tasks

Namespace Networking

    ''' <summary>
    ''' Reveals information about a network interface on a remote machine.
    ''' </summary>
    Public Class NetworkInterfaceInformation

        Private _ipAddress As IPAddress
        Private _physicalAddress As PhysicalAddress
        Private _vendor As NetworkInterfaceVendorInfo

        Private Sub New()
        End Sub

        Public Shared Function FromHost(host As IPAddress) As NetworkInterfaceInformation
            Return New NetworkInterfaceInformation With {
                .Address = host,
                .PhysicalAddress = ArpRequest.GetResponse(host).Value
            }
        End Function

        Public Shared Function FromHostAsync(host As IPAddress) As Task(Of NetworkInterfaceInformation)
            Return FromHostAsyncInternal(host)
        End Function

        Private Shared Async Function FromHostAsyncInternal(host As IPAddress) As Task(Of NetworkInterfaceInformation)
            Return New NetworkInterfaceInformation With {
                .Address = host,
                .PhysicalAddress = (Await ArpRequest.GetResponseAsync(host)).Value
            }
        End Function

        ''' <summary>
        ''' Gets the configured IP address.
        ''' </summary>
        Public Property Address As IPAddress
            Get
                Return _ipAddress
            End Get
            Private Set(value As IPAddress)
                _ipAddress = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the MAC address.
        ''' </summary>
        Public Property PhysicalAddress As PhysicalAddress
            Get
                Return _physicalAddress
            End Get
            Set(value As PhysicalAddress)
                _physicalAddress = value
            End Set
        End Property

        ''' <summary>
        ''' Gets information about the network card's vendor.
        ''' </summary>
        Public ReadOnly Property Vendor As NetworkInterfaceVendorInfo
            Get
                If _vendor Is Nothing Then
                    _vendor = NetworkInterfaceVendorInfo.GetVendorInformationAsync(Me.PhysicalAddress).Result
                End If
                Return _vendor
            End Get
        End Property

        ''' <summary>
        ''' Pretty prints the adapter and vendor info.
        ''' </summary>
        Public Overrides Function ToString() As String
            Dim text As New Text.StringBuilder

            text.AppendLine("IP address: " + Address.ToString)
            text.Append("MAC address: " + PhysicalAddress.ToString)

            Return text.ToString
        End Function

    End Class

End Namespace

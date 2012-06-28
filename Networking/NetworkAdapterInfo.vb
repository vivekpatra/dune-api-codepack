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
Imports System.Net.Sockets
Imports System.Net.NetworkInformation
Imports System.Threading.Tasks

Namespace Networking

    ''' <summary>
    ''' Reveals information about a network interface on a remote machine.
    ''' </summary>
    ''' <remarks>None of this has been tested with a local interface. Actually that's a good idea for a new TODO.</remarks>
    Public Class NetworkAdapterInfo

        Private _ipAddress As IPAddress
        Private _physicalAddress As PhysicalAddress
        Private _vendor As NetworkCardVendor


        Public Sub New(address As IPAddress)
            _ipAddress = address

            For Each nic In NetworkInterface.GetAllNetworkInterfaces ' check if this is a local IP address
                If nic.GetIPProperties.UnicastAddresses.Where(Function(unicastAddress) unicastAddress.Address.Equals(address)).Count > 0 Then
                    _physicalAddress = nic.GetPhysicalAddress
                End If
            Next

            If _physicalAddress Is Nothing Then ' get address from remote target
                _physicalAddress = NativeMethods.Networking.GetMacAddress(address)
            End If
        End Sub

        ''' <summary>
        ''' Gets the configured IP address.
        ''' </summary>
        Public ReadOnly Property Address As IPAddress
            Get
                Return _ipAddress
            End Get
        End Property

        ''' <summary>
        ''' Gets the MAC address.
        ''' </summary>
        Public ReadOnly Property PhysicalAddress As PhysicalAddress
            Get
                Return _physicalAddress
            End Get
        End Property

        ''' <summary>
        ''' Gets information about the network card's vendor.
        ''' </summary>
        Public ReadOnly Property Vendor As NetworkCardVendor
            Get
                If _vendor Is Nothing Then
                    _vendor = New NetworkCardVendor(_physicalAddress)
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

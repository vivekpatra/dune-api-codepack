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
Imports System.Runtime.CompilerServices
Imports System.Net

Namespace Extensions

    ''' <summary>
    ''' Extensions for the <see cref="IPAddress"/> type.
    ''' </summary>
    Public Module IPAddressExtensions

        <Extension()>
        Public Function GetBroadcastAddress(address As IPAddress, subnetMask As IPAddress) As IPAddress
            Dim ipAdressBytes As Byte() = address.GetAddressBytes()
            Dim subnetMaskBytes As Byte() = subnetMask.GetAddressBytes()

            If ipAdressBytes.Length <> subnetMaskBytes.Length Then
                Throw New ArgumentException("Lengths of IP address and subnet mask do not match.")
            End If

            Dim broadcastAddress As Byte() = New Byte(ipAdressBytes.Length - 1) {}
            For i As Integer = 0 To broadcastAddress.Length - 1
                broadcastAddress(i) = CByte(ipAdressBytes(i) Or (subnetMaskBytes(i) Xor 255))
            Next
            Return New IPAddress(broadcastAddress)
        End Function

        <Extension()>
        Public Function GetNetworkAddress(address As IPAddress, subnetMask As IPAddress) As IPAddress
            Dim ipAdressBytes As Byte() = address.GetAddressBytes()
            Dim subnetMaskBytes As Byte() = subnetMask.GetAddressBytes()

            If ipAdressBytes.Length <> subnetMaskBytes.Length Then
                Throw New ArgumentException("Lengths of IP address and subnet mask do not match.")
            End If

            Dim broadcastAddress As Byte() = New Byte(ipAdressBytes.Length - 1) {}
            For i As Integer = 0 To broadcastAddress.Length - 1
                broadcastAddress(i) = CByte(ipAdressBytes(i) And (subnetMaskBytes(i)))
            Next
            Return New IPAddress(broadcastAddress)
        End Function

        <Extension()>
        Public Function IsInSameSubnet(address2 As IPAddress, address As IPAddress, subnetMask As IPAddress) As Boolean
            Dim networkAddress1 As IPAddress = address.GetNetworkAddress(subnetMask)
            Dim networkAddress2 As IPAddress = address2.GetNetworkAddress(subnetMask)

            Return networkAddress1.Equals(networkAddress2)
        End Function

        ''' <summary>
        ''' Gets all IPv4 addresses from an IPAddress array.
        ''' </summary>
        <Extension()>
        Public Iterator Function GetIPv4Addresses(addressList As IPAddress()) As IEnumerable(Of IPAddress)
            For Each address In addressList
                If address.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                    Yield address
                End If
            Next
        End Function

    End Module

End Namespace
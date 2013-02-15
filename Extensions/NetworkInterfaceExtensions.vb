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
Imports System.Runtime.CompilerServices
Imports System.Net.NetworkInformation
Imports System.Net.Sockets

Namespace Extensions

    ''' <summary>
    ''' Extensions for the <see cref="NetworkInterface"/> type.
    ''' </summary>
    Public Module NetworkInterfaceExtensions

        <DebuggerStepThrough()>
        <Extension()>
        Public Function WithActiveConnection(value() As NetworkInterface) As IEnumerable(Of UnicastIPAddressInformation)
            Dim list As New List(Of UnicastIPAddressInformation)

            Dim interfaces = From allInterfaces In value
                             Where allInterfaces.OperationalStatus = OperationalStatus.Up
                             Where allInterfaces.Supports(NetworkInterfaceComponent.IPv4)
                             Where allInterfaces.NetworkInterfaceType <> NetworkInterfaceType.Loopback
                             Select allInterfaces

            For Each nic In interfaces
                Dim connectedNics = From addresses In nic.GetIPProperties.UnicastAddresses
                                    Where addresses.Address.AddressFamily = AddressFamily.InterNetwork

                list.AddRange(connectedNics)
            Next

            Return list
        End Function

    End Module

End Namespace
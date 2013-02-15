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

Namespace Extensions

    ''' <summary>
    ''' Extensions for the <see cref="PhysicalAddress"/> type.
    ''' </summary>
    Public Module PhysicalAddressExtensions

        <Extension()>
        Public Function ToString(value As PhysicalAddress, delimiter As Char) As String
            Dim address() As Byte = value.GetAddressBytes
            Return BitConverter.ToString(address).Replace("-"c, delimiter)
        End Function

        <Extension()>
        Public Function GetOrganizationallyUniqueIdentifier(value As PhysicalAddress) As Byte()
            Dim address = value.GetAddressBytes
            ReDim Preserve address(2)
            Return address
        End Function

        <Extension()>
        Public Function GetVendorInfo(value As PhysicalAddress) As DuneApiCodePack.Networking.NetworkInterfaceVendorInfo
            Try
                Return Networking.NetworkInterfaceVendorInfo.GetVendorInformationAsync(value).Result
            Catch ex As AggregateException
                Throw ex.InnerException
            End Try
        End Function

    End Module
End Namespace

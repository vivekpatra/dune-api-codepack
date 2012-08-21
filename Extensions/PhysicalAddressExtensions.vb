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
Imports System.Net.NetworkInformation

Namespace Extensions
    ''' <summary>
    ''' Extensions for the <see cref="PhysicalAddress"/> type.
    ''' </summary>
    Public Module PhysicalAddressExtensions

        <Extension()>
        Public Function ToString(value As PhysicalAddress, delimiter As Char) As String
            Dim address() As Byte = value.GetAddressBytes
            Return BitConverterExtensions.ToString(address, delimiter)
        End Function


        <Extension()>
        Public Function GetVendorInfo(value As PhysicalAddress) As DuneApiCodePack.Networking.NetworkCardVendorInfo
            Return New DuneApiCodePack.Networking.NetworkCardVendorInfo(value)
        End Function

    End Module
End Namespace

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
Imports System.Net.NetworkInformation
Imports System.Net.Http
Imports System.IO

Namespace Networking

    Public Class NetworkInterfaceVendorInfo

        Private _company As String
        Private _department As String
        Private _address1 As String
        Private _address2 As String
        Private _country As String

        Private Sub New()
        End Sub

        Private Shared Function GetApi() As UriBuilder
            Return New UriBuilder("http://www.macvendorlookup.com/api/BOWGGVM/")
        End Function

        ''' <summary>
        ''' Gets vendor information from the OUI database based on the specified hardware address.
        ''' </summary>
        ''' <param name="physicalAddress">The MAC address to look up.</param>
        Public Shared Async Function GetVendorInformationAsync(physicalAddress As PhysicalAddress) As Task(Of NetworkInterfaceVendorInfo)
            If physicalAddress Is Nothing Then
                Throw New ArgumentNullException("physicalAddress")
            End If

            Dim builder = NetworkInterfaceVendorInfo.GetApi
            builder.Path &= BitConverter.ToString(physicalAddress.GetOrganizationallyUniqueIdentifier)

            Dim info As New NetworkInterfaceVendorInfo
            Try
                Using client As New HttpClient
                    Dim results = (Await client.GetStringAsync(builder.Uri)).Split(NetworkInterfaceVendorInfo.Delimiter)

                    With info
                        .Company = results(Vendor.Company)
                        .Department = results(Vendor.Department)
                        .Address1 = results(Vendor.Address1)
                        .Address2 = results(Vendor.Address2)
                        .Country = results(Vendor.Country)
                    End With
                End Using
            Catch ex As WebException
                Console.WriteLine("macvendorlookup: " + ex.Message)
            End Try

            Return info
        End Function

        ''' <summary>
        ''' Gets the name of the company.
        ''' </summary>
        Public Property Company As String
            Get
                Return _company
            End Get
            Private Set(value As String)
                _company = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the company's department.
        ''' </summary>
        Public Property Department As String
            Get
                Return _department
            End Get
            Private Set(value As String)
                _department = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the company's first address line.
        ''' </summary>
        Public Property Address1 As String
            Get
                Return _address1
            End Get
            Private Set(value As String)
                _address1 = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the company's second address line.
        ''' </summary>
        Public Property Address2 As String
            Get
                Return _address2
            End Get
            Private Set(value As String)
                _address2 = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the company's country.
        ''' </summary>
        Public Property Country As String
            Get
                Return _country
            End Get
            Private Set(value As String)
                _country = value
            End Set
        End Property

        Private Shared ReadOnly Property Delimiter As Char
            Get
                Return "|"c
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim text As New Text.StringBuilder

            text.AppendLine("Vendor: " + Company)
            text.AppendLine("Department: " + Department)
            text.AppendLine("Address line 1: " + Address1)
            text.AppendLine("Address line 2: " + Address2)
            text.Append("Country: " + Country)

            Return text.ToString
        End Function

        Private Enum Vendor
            Company = 0
            Department = 1
            Address1 = 2
            Address2 = 3
            Country = 4
        End Enum

    End Class

End Namespace
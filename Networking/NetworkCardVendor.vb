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
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO

Namespace Networking

    Public Class NetworkCardVendor

        Private Const ApiBaseUri As String = "http://www.macvendorlookup.com/api/"
        Private Const ApiKey As String = "BOWGGVM" ' get your own damn key if you want to use this API outside this class!

        Private _company As String
        Private _department As String
        Private _address1 As String
        Private _address2 As String
        Private _country As String

        Public Sub New(physicalAddress As PhysicalAddress)
            GetResults(physicalAddress)
        End Sub

        ''' <summary>
        ''' Gets vendor information from the OUI database based on the supplied hardware address.
        ''' </summary>
        ''' <param name="physicalAddress">The MAC address to look up.</param>
        Private Sub GetResults(physicalAddress As PhysicalAddress)
            Dim delimiter As Char = "|"c

            Try
                Using client As New WebClient()
                    Dim request As New Uri(ApiBaseUri + ApiKey + "/" + physicalAddress.ToString)

                    Dim results As String = client.DownloadString(request)

                    _company = results.Split(delimiter).GetValue(0).ToString
                    _department = results.Split(delimiter).GetValue(1).ToString
                    _address1 = results.Split(delimiter).GetValue(2).ToString
                    _address2 = results.Split(delimiter).GetValue(3).ToString
                    _country = results.Split(delimiter).GetValue(4).ToString
                End Using
            Catch ex As Exception
                Console.WriteLine("Couldn't get information from the OUI database: " + ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the name of the company.
        ''' </summary>
        Public ReadOnly Property Company As String
            Get
                Return _company
            End Get
        End Property

        ''' <summary>
        ''' Gets the company's department.
        ''' </summary>
        Public ReadOnly Property Department As String
            Get
                Return _department
            End Get
        End Property

        ''' <summary>
        ''' Gets the company's first address line.
        ''' </summary>
        Public ReadOnly Property Address1 As String
            Get
                Return _address1
            End Get
        End Property

        ''' <summary>
        ''' Gets the company's second address line.
        ''' </summary>
        Public ReadOnly Property Address2 As String
            Get
                Return _address2
            End Get
        End Property

        ''' <summary>
        ''' Gets the company's country.
        ''' </summary>
        Public ReadOnly Property Country As String
            Get
                Return _country
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
    End Class

End Namespace
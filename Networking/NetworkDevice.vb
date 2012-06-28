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
Imports SL.DuneApiCodePack.Sources
Imports System.DirectoryServices

Namespace Networking

    Public Class NetworkDevice
        Private _host As IPHostEntry
        Private _networkAdapter As NetworkAdapterInfo

        Public Sub New(host As IPHostEntry)
            _host = host
        End Sub

        Public ReadOnly Property Host As IPHostEntry
            Get
                Return _host
            End Get
        End Property

        Public Function GetShares() As Collection(Of SmbShare)
            Return SmbShare.FromHost(Host)
        End Function

        Public ReadOnly Property NetworkAdapter As NetworkAdapterInfo
            Get
                If _networkAdapter Is Nothing Then
                    _networkAdapter = New NetworkAdapterInfo(Host.AddressList.Where(Function(address) address.AddressFamily = Sockets.AddressFamily.InterNetwork).First)
                End If
                Return _networkAdapter
            End Get
        End Property

        Public Function IsDune() As Boolean
            Return IsDune(True)
        End Function

        Public Function IsDune(ignoreNonSigmaNic As Boolean) As Boolean
            If ignoreNonSigmaNic Then
                Return NetworkAdapter.PhysicalAddress.ToString.Contains("0016E8")
            Else
                Try
                    Using dune As DuneUtilities.Dune = New DuneUtilities.Dune(Host.HostName)
                        Return dune.IsConnected
                    End Using
                Catch ex As Exception
                    Return False
                End Try
            End If
        End Function

        ''' <summary>
        ''' Scans all workgroups and retrieves a list of network devices.
        ''' </summary>
        Public Shared Function Scan() As List(Of NetworkDevice)
            Dim devices As New List(Of NetworkDevice)

            Using network As New DirectoryEntry()
                network.Path = "WinNT:"

                For Each workgroup As DirectoryEntry In network.Children()
                    devices.AddRange(Scan(workgroup))
                Next
            End Using

            Return devices
        End Function

        ''' <summary>
        ''' Scans the specified workgroup and retrieves a list of network devices.
        ''' </summary>
        Public Shared Function Scan(workgroup As DirectoryEntry) As List(Of NetworkDevice)
            Dim devices As New List(Of NetworkDevice)

            Try
                For Each computer As DirectoryEntry In workgroup.Children
                    If computer.SchemaClassName = "Computer" Then
                        Dim device As New NetworkDevice(Dns.GetHostEntry(computer.Name))
                        devices.Add(device)
                    End If
                Next
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            
            Return devices
        End Function

    End Class

End Namespace
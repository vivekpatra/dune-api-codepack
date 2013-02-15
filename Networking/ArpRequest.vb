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
Imports System.ComponentModel
Imports System.Threading.Tasks

Public NotInheritable Class ArpRequest

    Private Sub New()
    End Sub

    Private Shared Function SendArp(host As IPAddress) As KeyValuePair(Of IPAddress, PhysicalAddress)
        Dim kvp = TrySendArp(host)

        If kvp.Value IsNot Nothing Then
            Return kvp
        Else
            ' TODO: handle various SendARP errors
            ' http://msdn.microsoft.com/en-us/library/windows/desktop/aa366358(v=vs.85).aspx
            Throw New Win32Exception(Runtime.InteropServices.Marshal.GetLastWin32Error)
        End If
    End Function

    Private Shared Function TrySendArp(host As IPAddress) As KeyValuePair(Of IPAddress, PhysicalAddress)
        Dim mac() As Byte = New Byte(5) {}

        Dim returnValue = SendArpInternal(host, mac)

        If returnValue = NativeMethods.IPHelper.ReturnValue.NO_ERROR Then
            Return New KeyValuePair(Of IPAddress, PhysicalAddress)(host, New PhysicalAddress(mac))
        Else
            Return New KeyValuePair(Of IPAddress, PhysicalAddress)(host, Nothing)
        End If
    End Function

    Private Shared Function SendArpInternal(host As IPAddress, ByRef mac() As Byte) As Integer
        Dim ip As UInteger = BitConverter.ToUInt32(host.GetAddressBytes(), 0)
        Return CInt(NativeMethods.IPHelper.SendARP(CUInt(ip), 0, mac, mac.Length))
    End Function

    Public Shared Async Function SendArpAsync(address As IPAddress) As Task(Of KeyValuePair(Of IPAddress, PhysicalAddress))
        Dim ip As UInteger = BitConverter.ToUInt32(address.GetAddressBytes(), 0)
        Dim mac() As Byte = New Byte(5) {}

        Dim returnValue = Await Task.Factory.StartNew(Of Integer)(Function() CInt(NativeMethods.IPHelper.SendARP(CUInt(ip), 0, mac, mac.Length))).ConfigureAwait(False)

        If returnValue = NativeMethods.IPHelper.ReturnValue.NO_ERROR Then
            Return New KeyValuePair(Of IPAddress, PhysicalAddress)(address, New PhysicalAddress(mac))
        Else
            ' TODO: handle various SendARP errors
            ' http://msdn.microsoft.com/en-us/library/windows/desktop/aa366358(v=vs.85).aspx
            Throw New Win32Exception(returnValue)
        End If
    End Function

    Public Shared Async Function TrySendArpAsync(host As IPAddress) As Task(Of KeyValuePair(Of IPAddress, PhysicalAddress))
        Dim mac() As Byte = New Byte(5) {}

        Dim returnValue = Await Task.Run(Of Integer)(Function() SendArpInternal(host, mac)).ConfigureAwait(False)

        If returnValue = NativeMethods.IPHelper.ReturnValue.NO_ERROR Then
            Return New KeyValuePair(Of IPAddress, PhysicalAddress)(host, New PhysicalAddress(mac))
        Else
            Return New KeyValuePair(Of IPAddress, PhysicalAddress)(host, Nothing)
        End If
    End Function

    Public Shared Function GetResponse(host As IPAddress) As KeyValuePair(Of IPAddress, PhysicalAddress)
        Return SendArp(host)
    End Function

    Public Shared Iterator Function TryGetResponse(hosts As IEnumerable(Of IPAddress)) As IEnumerable(Of KeyValuePair(Of IPAddress, PhysicalAddress))
        For Each host In hosts
            Dim pair = TrySendArp(host)
            If pair.Key Is host Then
                Yield pair
            End If
        Next
    End Function

    Public Shared Function GetResponseAsync(host As IPAddress) As Task(Of KeyValuePair(Of IPAddress, PhysicalAddress))
        Return GetResponseAsync(host, New Threading.CancellationTokenSource().Token)
    End Function

    Public Shared Function GetResponseAsync(host As IPAddress, cancellationToken As Threading.CancellationToken) As Task(Of KeyValuePair(Of IPAddress, PhysicalAddress))
        Return GetResponseAsyncInternal(host, cancellationToken)
    End Function

    Private Shared Async Function GetResponseAsyncInternal(host As IPAddress, cancellationToken As Threading.CancellationToken) As Task(Of KeyValuePair(Of IPAddress, PhysicalAddress))
        Return Await SendArpAsync(host).WithCancellation(cancellationToken).ConfigureAwait(False)
    End Function

End Class

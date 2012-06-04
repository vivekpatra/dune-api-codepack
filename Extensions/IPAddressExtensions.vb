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

    End Module

End Namespace
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

            For Each nic In NetworkInterface.GetAllNetworkInterfaces
                If nic.GetIPProperties.UnicastAddresses.Where(Function(unicastAddress) unicastAddress.Address.Equals(address)).Count > 0 Then
                    _physicalAddress = nic.GetPhysicalAddress
                End If
            Next

            If _physicalAddress Is Nothing Then
                _physicalAddress = NativeMethods.Networking.GetMacAddress(address)
            End If

            _vendor = New NetworkCardVendor(_physicalAddress)
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
                Return _vendor
            End Get
        End Property

        ''' <summary>
        ''' Pretty prints the adapter and vendor info.
        ''' </summary>
        Public Overrides Function ToString() As String
            Dim text As New Text.StringBuilder

            text.AppendLine("IP address: " + Address.ToString)
            text.Append("MAC address: " + PhysicalAddress.ToDelimitedString)

            Return text.ToString
        End Function

    End Class

End Namespace

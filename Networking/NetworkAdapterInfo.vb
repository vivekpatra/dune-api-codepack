Imports System.Net
Imports System.Net.Sockets
Imports System.Net.NetworkInformation
Imports System.Threading.Tasks
Imports SL.DuneApiCodePack.Extensions

Namespace Networking

    ''' <summary>
    ''' Reveals information about a network interface on a remote machine.
    ''' </summary>
    ''' <remarks>None of this has been tested with a local interface. Actually that's a good idea for a new TODO.</remarks>
    Public Class NetworkAdapterInfo ' TODO: test with local interfaces.

        Private _ipAddress As IPAddress
        Private _physicalAddress As PhysicalAddress
        Private _vendor As NetworkCardVendor


        Public Sub New(ByVal address As IPAddress)
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
        ''' Gets the amount of hops between the client and the server.
        ''' </summary>
        Public ReadOnly Property Hops As Integer
            Get
                Return GetTraceRoute("dune").Count
            End Get
        End Property

        ''' <summary>
        ''' Calculates the amount of hops between the client and the server.
        ''' </summary>
        Public Shared Function GetTraceRoute(ByVal hostNameOrAddress As String) As IEnumerable(Of IPAddress)
            Return GetTraceRoute(hostNameOrAddress, 1)
        End Function

        Private Shared Function GetTraceRoute(ByVal hostNameOrAddress As String, ByVal ttl As Integer) As IEnumerable(Of IPAddress)
            Dim pinger As Ping = New Ping
            Dim pingerOptions As PingOptions = New PingOptions(ttl, True)
            Dim timeout As Integer = 10000
            Dim reply As PingReply

            reply = pinger.Send(hostNameOrAddress, timeout, New Byte() {}, pingerOptions)

            Dim result As List(Of IPAddress) = New List(Of IPAddress)
            If reply.Status = IPStatus.Success Then
                result.Add(reply.Address)
            ElseIf reply.Status = IPStatus.TtlExpired Then
                'add the currently returned address
                result.Add(reply.Address)
                'recurse to get the next address...
                Dim tempResult As IEnumerable(Of IPAddress)
                tempResult = GetTraceRoute(hostNameOrAddress, ttl + 1)
                result.AddRange(tempResult)
            Else
                'failure 
            End If

            Return result
        End Function

        ''' <summary>
        ''' Gets the outgoing network interface that connects to the specified endpoint.
        ''' </summary>
        Private Function GetBestInterfaceManaged(ByVal address As IPAddress, ByVal port As Integer) As NetworkInterface

            Dim socket As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
            Dim outgoingInterface As NetworkInterface = Nothing

            Try
                Dim t As Task = task.Factory.FromAsync(AddressOf socket.BeginConnect, AddressOf socket.EndConnect, New IPEndPoint(address, port), Nothing)

                Return t.ContinueWith(Of NetworkInterface)(Function()
                                                               If socket.Connected Then
                                                                   Dim interfaces As List(Of NetworkInterface) = NetworkInterface.GetAllNetworkInterfaces.ToList()

                                                                   For Each nic As NetworkInterface In interfaces
                                                                       Dim properties As IPInterfaceProperties = nic.GetIPProperties

                                                                       For Each unicastAddress In properties.UnicastAddresses
                                                                           If unicastAddress.Address.Equals(DirectCast(socket.LocalEndPoint, IPEndPoint).Address) Then
                                                                               outgoingInterface = nic
                                                                               Exit For
                                                                           End If
                                                                       Next
                                                                   Next
                                                               End If
                                                               Return outgoingInterface
                                                           End Function).Result
            Catch ex As SocketException
                Console.WriteLine(ex.Message)
            End Try

            Return outgoingInterface

        End Function

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

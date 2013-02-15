Imports System.Net
Imports System.Net.NetworkInformation

Namespace Networking

    Public Class IPv4Network
        Implements IEnumerable(Of IPAddress)

        Private _networkAddress As IPAddress
        Private _broadcastAddress As IPAddress
        Private _subnetMask As IPAddress

        Private Sub New(address As IPAddress, subnetMask As IPAddress)
            Me.SubnetMask = subnetMask
            Me.NetworkAddress = address.GetNetworkAddress(subnetMask)
            Me.BroadcastAddress = address.GetBroadcastAddress(subnetMask)
        End Sub

        Public Property SubnetMask As IPAddress
            Get
                Return _subnetMask
            End Get
            Private Set(value As IPAddress)
                _subnetMask = value
            End Set
        End Property

        Public Property NetworkAddress As IPAddress
            Get
                Return _networkAddress
            End Get
            Private Set(value As IPAddress)
                _networkAddress = value
            End Set
        End Property

        Public Property BroadcastAddress As IPAddress
            Get
                Return _broadcastAddress
            End Get
            Private Set(value As IPAddress)
                _broadcastAddress = value
            End Set
        End Property

        Public Shared Iterator Function GetNetworks() As IEnumerable(Of IPv4Network)
            Dim networkInterfaces = NetworkInterface.GetAllNetworkInterfaces.WithActiveConnection

            For Each IPConfig In networkInterfaces
                Yield New IPv4Network(IPConfig.Address, IPConfig.IPv4Mask)
            Next
        End Function

        Public Shared Function PingAsync(address As IPAddress) As Task(Of PingReply)
            Dim p As New Ping
            Dim options As New PingOptions
            options.Ttl = 1
            Return p.SendPingAsync(address, 1, New Byte() {}, options)
        End Function

        Public Shared Async Function GetReachableHostsAsync(network As IPv4Network) As Task(Of IEnumerable(Of IPAddress))
            Dim tasks As New List(Of Task(Of PingReply))
            For Each address In network
                tasks.Add(IPv4Network.PingAsync(address))
            Next

            Await Task.WhenAll(tasks)

            Return From replies In tasks Select replies.Result Where Result.Status = IPStatus.Success
                   Select Result.Address
        End Function

        Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New IPv4NetworkEnumerator(Me)
        End Function

        Private Function GetEnumerator() As IEnumerator(Of IPAddress) Implements IEnumerable(Of IPAddress).GetEnumerator
            Return New IPv4NetworkEnumerator(Me)
        End Function

    End Class

    Public Class IPv4NetworkEnumerator
        Implements IEnumerator(Of IPAddress)

        Private _network As IPv4Network

        Private _networkAddress As UInt32
        Private _broadcastAddress As UInt32
        Private _firstUsable As UInt32
        Private _lastUsable As UInt32
        Private _current As UInt32

        Public Sub New(network As IPv4Network)
            _network = network

            _networkAddress = GetUnsignedInteger(network.NetworkAddress)
            _broadcastAddress = GetUnsignedInteger(network.BroadcastAddress)

            _firstUsable = GetUnsignedInteger(network.NetworkAddress.GetFirstUsableAddress(network.SubnetMask))
            _lastUsable = GetUnsignedInteger(network.NetworkAddress.GetLastUsableAddress(network.SubnetMask))

            _current = _networkAddress
        End Sub

        Public ReadOnly Property Current As IPAddress Implements IEnumerator(Of IPAddress).Current
            Get
                Dim bytes = BitConverter.GetBytes(_current)
                If BitConverter.IsLittleEndian Then
                    Array.Reverse(bytes)
                End If
                Return New IPAddress(bytes)
            End Get
        End Property

        Private ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Return Me.Current
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            _current = CUInt(_current + 1)

            If _current >= _broadcastAddress Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            _current = _networkAddress
        End Sub

        Private Shared Function GetUnsignedInteger(value As IPAddress) As UInt32
            Dim bytes = value.GetAddressBytes
            If BitConverter.IsLittleEndian Then
                Array.Reverse(bytes)
            End If
            Return BitConverter.ToUInt32(bytes, 0)
        End Function

        Private Shared Function GetIPAddress(value As UInt32) As IPAddress
            Dim bytes = BitConverter.GetBytes(value)
            If BitConverter.IsLittleEndian Then
                Array.Reverse(bytes)
            End If
            Return New IPAddress(bytes)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                ' nothing to dispose
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace

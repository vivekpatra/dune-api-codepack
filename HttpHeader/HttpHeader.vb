Namespace Dune.HttpHeader

    ''' <summary>
    ''' Information retrieved from the device's HTTP header.
    ''' </summary>
    ''' <remarks>
    ''' Instantiating this class requires sending a custom playback command to the device.
    ''' The information can only be pulled by a tcplistener on the local host.
    ''' </remarks>
    Public Class HttpHeader

        Dim _productId As String
        Dim _serialNumber As String
        Dim _interfaceLanguage As String
        ' Dim _firmware As FirmwareInfo

        Public Sub New(ByRef dune As Dune)
            ' TODO: implement http header stuff
            _productId = String.Empty
            _serialNumber = String.Empty
            _interfaceLanguage = String.Empty
            '_firmware = New FirmwareInfo(String.Empty)
        End Sub

#Region "Properties"

        ''' <summary>
        ''' Gets the product ID.
        ''' </summary>
        ReadOnly Property ProductId As String
            Get
                Return _productId
            End Get
        End Property

        ''' <summary>
        ''' Gets the device's serial number.
        ''' </summary>
        ReadOnly Property SerialNumber As String
            Get
                Return _serialNumber
            End Get
        End Property

        ''' <summary>
        ''' Gets the device's interface language.
        ''' </summary>
        ReadOnly Property InterfaceLanguage As String
            Get
                Return _interfaceLanguage
            End Get
        End Property

        ' ''' <summary>
        ' ''' Gets information about the device's firmware.
        ' ''' </summary>
        'ReadOnly Property Firmware As FirmwareInfo
        '    Get
        '        Return _firmware
        '    End Get
        'End Property

#End Region

    End Class

End Namespace

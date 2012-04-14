Namespace Dune.HttpHeader

    ''' <summary>
    ''' Provides information about the installed firmware version,
    ''' </summary>
    Public Class FirmwareInfo
        Private _firmware As String
        Private _beta As Boolean
        Private _provider As IFormatProvider

        ''' <param name="firmware">The firmware string as defined in the Dune's useragent.</param>
        Public Sub New(ByVal firmware As String)
            _firmware = firmware

            _beta = (_firmware.LastIndexOf("_")) > (_firmware.IndexOf("_"))

            _provider = New System.Globalization.CultureInfo("nl-BE")
        End Sub

        ''' <summary>
        ''' Gets the text representation of the firmware.
        ''' </summary>
        Public ReadOnly Property Firmware As String
            Get
                Return _firmware
            End Get
        End Property

        ''' <summary>
        ''' Gets whether this is a stable release or beta release.
        ''' </summary>
        Public ReadOnly Property Beta As Boolean
            Get
                Return _beta
            End Get
        End Property

        ''' <summary>
        ''' Gets the build date (usually on or near the release date) of the firmware.
        ''' </summary>
        Public ReadOnly Property BuildDate As Date
            Get
                If _beta Then
                    Return Date.Parse(_firmware.Substring(0, _firmware.LastIndexOf("_")), _provider)
                Else
                    Return Date.Parse(_firmware, _provider)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Returns the text representation of the firmware.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Firmware
        End Function
    End Class

End Namespace

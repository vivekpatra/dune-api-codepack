Imports System.Globalization

Namespace Dune.ApiWrappers

    ''' <summary>
    ''' Interface that defines properties and methods available in protocol version 2 and up.
    ''' </summary>
    ''' <remarks>
    ''' if [Propertyname] has a setter, use it only to execute a command.
    ''' Only [PropertynameUpdate] should be used to internally update the field value.
    ''' </remarks>
    Public Interface IProtocolVersion2

#Region "Properties"

        Property PlaybackVolume As Byte?

        WriteOnly Property PlaybackVolumeUpdate As Byte?

        Property PlaybackMute As Boolean?

        WriteOnly Property PlaybackMuteUpdate As Boolean?

        Property AudioTrack As Byte?

        WriteOnly Property AudioTrackUpdate As Byte?

        Property VideoFullscreen As Boolean?

        WriteOnly Property VideoFullscreenUpdate As Boolean?

        Property VideoX As UShort?

        WriteOnly Property VideoXUpdate As UShort?

        Property VideoY As UShort?

        WriteOnly Property VideoYUpdate As UShort?

        Property VideoWidth As UShort?

        WriteOnly Property VideoWidthUpdate As UShort?

        Property VideoHeight As UShort?

        WriteOnly Property VideoHeightUpdate As UShort?

        ReadOnly Property VideoTotalDisplayWidth As UShort?

        WriteOnly Property VideoTotalDisplayWidthUpdate As UShort?

        ReadOnly Property VideoTotalDisplayHeight As UShort?

        WriteOnly Property VideoTotalDisplayHeightUpdate As UShort?

        Property VideoEnabled As Boolean?

        WriteOnly Property VideoEnabledUpdate As Boolean?

        Property VideoZoom As String

        WriteOnly Property VideoZoomUpdate As String

        ReadOnly Property AudioTracks As SortedDictionary(Of Byte, CultureInfo)

        WriteOnly Property AudioTracksUpdate As SortedDictionary(Of Byte, CultureInfo)

#End Region ' Properties

#Region "Methods"

        Function SetVideoOutput(ByVal command As SetVideoOutputCommand) As CommandResult

#End Region

    End Interface

End Namespace
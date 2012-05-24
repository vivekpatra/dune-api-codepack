Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' Interface that defines properties available in protocol version 1 and up.
    ''' </summary>
    ''' <remarks>
    ''' if [Propertyname] has a setter, it is used to send a command.
    ''' Only [PropertynameUpdate] is to be used to update the field value.
    ''' </remarks>
    Public Interface IProtocolVersion1

#Region "Properties"

        ReadOnly Property CommandStatus As String

        WriteOnly Property CommandStatusUpdate As String

        ReadOnly Property CommandError As CommandException

        WriteOnly Property CommandErrorUpdate As CommandException

        ReadOnly Property ProtocolVersion As Byte

        WriteOnly Property ProtocolVersionUpdate As Byte

        Property PlayerState As String

        WriteOnly Property PlayerStateUpdate As String

        Property PlaybackSpeed As PlaybackSpeed

        WriteOnly Property PlaybackSpeedUpdate As Integer

        ReadOnly Property PlaybackDuration As TimeSpan?

        WriteOnly Property PlaybackDurationUpdate As TimeSpan?

        Property PlaybackPosition As TimeSpan?

        WriteOnly Property PlaybackPositionUpdate As TimeSpan?

        ReadOnly Property PlaybackIsBuffering As Boolean

        WriteOnly Property PlaybackIsBufferingUpdate As Boolean

        ReadOnly Property PlaybackDvdMenu As Boolean

        WriteOnly Property PlaybackDvdMenuUpdate As Boolean

        Property BlackScreen As Boolean?

        Property HideOnScreenDisplay As Boolean?

#End Region ' Properties

    End Interface

End Namespace
Namespace Dune.ApiWrappers

    ''' <summary>
    ''' Interface that defines properties and methods available in protocol version 1 and up.
    ''' </summary>
    ''' <remarks>
    ''' if [Propertyname] has a setter, use it only to execute a command.
    ''' Only [PropertynameUpdate] should be used to internally update the field value.
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

#Region "Methods"

        Function GetStatus() As CommandResult

        Function StartPlayback(ByVal command As StartPlaybackCommand) As CommandResult

        Function SetPlaybackState(ByVal command As SetPlaybackStateCommand) As CommandResult

        Function SetPlayerState(ByVal command As SetPlayerStateCommand) As CommandResult

#End Region ' Methods

    End Interface

End Namespace
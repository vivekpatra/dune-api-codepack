Imports System.Text

Namespace Dune.ApiWrappers
    ''' <summary>
    ''' This command is used to go to the previous or next keyframe.
    ''' </summary>
    ''' <remarks>This command only works for DVDs and MKV.
    ''' Sadly you can't know if the current file playback is an MKV file
    ''' so there is little point in checking whether the player state is DVD or file playback.
    ''' Also, playback needs to be paused for this to work at all, but this is done automatically by the code pack.</remarks>
    Public Class SetKeyframeCommand
        Inherits DuneCommand

        ''' <summary>
        ''' Enumeration of supported actions.
        ''' </summary>
        Public Enum Keyframe
            Previous = -1
            [Next] = 1
        End Enum

        Private _keyframe As Keyframe

        ''' <param name="dune">The target device.</param>
        ''' <param name="keyframe">The action that needs to be performed.</param>
        Public Sub New(ByVal dune As Dune, ByVal keyframe As Keyframe)
            MyBase.New(dune)
            CommandType = Constants.Commands.SetPlaybackState
            _keyframe = keyframe
        End Sub

        ''' <summary>
        ''' Gets the action that needs to be performed.
        ''' </summary>
        Public ReadOnly Property Action As Keyframe
            Get
                Return _keyframe
            End Get
        End Property

        Public Overrides Function GetQueryString() As String
            Dim query As New StringBuilder

            query.Append("cmd=")
            query.Append(CommandType)

            query.Append("&skip_frames=")
            query.Append(CInt(Action).ToString)

            Return query.ToString
        End Function
    End Class

End Namespace
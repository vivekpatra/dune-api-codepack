Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers
    ''' <summary>
    ''' This command is used to go to the previous or next keyframe.
    ''' </summary>
    ''' <remarks>This command only works for DVDs and MKV.
    ''' Sadly you can't know if the current file playback is an MKV file
    ''' so there is little point in checking whether the player state is DVD or file playback.
    ''' Also, playback needs to be paused for this to work at all, but this is done automatically by the code pack.</remarks>
    Public Class SetKeyframeCommand
        Inherits Command

        Private _keyframe As Constants.SetKeyframeValues

        ''' <param name="dune">The target device.</param>
        ''' <param name="keyframe">The action that needs to be performed.</param>
        Public Sub New(dune As Dune, keyframe As Constants.SetKeyframeValues)
            MyBase.New(dune)
            _keyframe = keyframe
        End Sub

        ''' <summary>
        ''' Gets the action that needs to be performed.
        ''' </summary>
        Public ReadOnly Property Action As Constants.SetKeyframeValues
            Get
                Return _keyframe
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.SetPlaybackState)
            query.Add(Constants.SetPlaybackStateParameterNames.PlaybackSpeed, Constants.PlaybackSpeedValues.Pause.ToString)
            query.Add(Constants.SetPlaybackStateParameterNames.SetKeyframe, CInt(Action).ToString)

            Return query
        End Function
    End Class

End Namespace
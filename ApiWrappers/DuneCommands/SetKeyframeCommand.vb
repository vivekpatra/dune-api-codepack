﻿Imports System.Text

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

        Public Overrides Function ToUri() As System.Uri
            Dim query As New StringBuilder

            ' Playback must be paused for this to work
            If Dune.PlaybackSpeed <> 0 Then
                Dim pauseCommand As New SetPlaybackStateCommand(Dune)
                pauseCommand.Speed = PlaybackSpeed.Pause
                Dim result As New CommandResult(pauseCommand)

                If result.Error IsNot Nothing Then ' pause playback by emulating a remote control button press
                    Dune.RemoteControl.Push(IRemoteControl.Button.Pause)
                End If
            End If

            query.Append("cmd=set_playback_state")
            query.Append("&skip_frames=" + CInt(Action).ToString)

            Return New Uri(BaseUri.ToString + query.ToString)

        End Function
    End Class

End Namespace
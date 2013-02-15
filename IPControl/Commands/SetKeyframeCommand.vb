#Region "License"
' Copyright 2012-2013 Steven Liekens
' Contact: steven.liekens@gmail.com

' This file is part of DuneApiCodepack.

' DuneApiCodepack is free software: you can redistribute it and/or modify
' it under the terms of the GNU Lesser General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' DuneApiCodepack is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Lesser General Public License for more details.

' You should have received a copy of the GNU Lesser General Public License
' along with DuneApiCodepack.  If not, see <http://www.gnu.org/licenses/>.
#End Region ' License
Imports SL.DuneApiCodePack.Networking

Namespace IPControl
    ''' <summary>
    ''' This command is used to go to the previous or next keyframe.
    ''' </summary>
    ''' <remarks>This command only works for DVDs and MKV.
    ''' Sadly you can't know if the current file playback is an MKV file
    ''' so there is little point in checking whether the player state is DVD or file playback.
    ''' Also, playback needs to be paused for this to work at all, but this is done automatically by the code pack.</remarks>
    Public Class SetKeyframeCommand : Inherits Command

        Private _requiredVersion As Version = New Version(1, 0)
        Private _direction As SkipFrames

        ''' <param name="direction">The direction that needs to be performed.</param>
        Public Sub New(direction As SkipFrames)
            MyBase.New(CommandValue.SetPlaybackState)
            Me.parameters.Item(Input.PlaybackSpeed) = CStr(0)
            Me.Direction = direction
        End Sub

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        ''' <summary>
        ''' Gets the action that needs to be performed.
        ''' </summary>
        Public Property Direction As SkipFrames
            Get
                Return _direction
            End Get
            Set(value As SkipFrames)
                _direction = value
                If value IsNot Nothing Then
                    Me.parameters.Item(Input.SkipFrames) = value.Value
                Else
                    Me.parameters.Remove(Input.SkipFrames)
                End If
            End Set
        End Property

        Public Overrides Function CanExecute(target As Dune) As Boolean
            If MyBase.CanExecute(target) Then
                Return target.PlayerState.IsPlaybackState
            End If
            Return False
        End Function

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New StatusCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
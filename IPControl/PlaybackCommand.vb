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
    ''' This is the base class for commands which affect playback.
    ''' </summary>
    Public MustInherit Class PlaybackCommand : Inherits Command

        Private _playbackSpeed As PlaybackSpeed
        Private _playbackPosition As TimeSpan?
        Private _displayEnabled As Boolean?
        Private _onScreenDisplayEnabled As Boolean?
        Private _actionOnFinish As ActionOnFinish
        Private _actionOnExit As ActionOnExit
        Private _videoEnabled As Boolean?
        Private _playbackVolume As Integer?
        Private _playbackMute As Boolean?
        Private _audioTrack As Integer?
        Private _subtitleTrack As Integer?
        Private _videoZoom As VideoZoom
        Private _videoOnTop As Boolean?

        Public Sub New(command As CommandValue)
            MyBase.New(command)
        End Sub

        ''' <summary>
        ''' Gets or sets a playback speed.
        ''' </summary>
        Public Property PlaybackSpeed As PlaybackSpeed
            Get
                Return _playbackSpeed
            End Get
            Set(value As PlaybackSpeed)
                _playbackSpeed = value

                If value IsNot Nothing Then
                    Me.parameters.Item(Input.PlaybackSpeed) = value.Value
                Else
                    Me.parameters.Remove(Input.PlaybackSpeed)
                End If
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets a playback position.
        ''' </summary>
        Public Property PlaybackPosition As TimeSpan?
            Get
                Return _playbackPosition
            End Get
            Set(value As TimeSpan?)
                _playbackPosition = value

                If value.HasValue Then
                    Me.parameters.Item(Input.PlaybackPosition) = CInt(value.Value.TotalSeconds).ToString
                Else
                    Me.parameters.Remove(Input.PlaybackPosition)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to show a black screen.
        ''' </summary>
        Public Property DisplayEnabled As Boolean?
            Get
                Return _displayEnabled
            End Get
            Set(value As Boolean?)
                _displayEnabled = value

                If value.HasValue Then
                    Me.parameters.Item(Input.BlackScreen) = (Not value.Value).ToNumberString
                Else
                    Me.parameters.Remove(Input.BlackScreen)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to enable the OSD.
        ''' </summary>
        Public Property OnScreenDisplayEnabled As Boolean?
            Get
                Return _onScreenDisplayEnabled
            End Get
            Set(value As Boolean?)
                _onScreenDisplayEnabled = value

                If value.HasValue Then
                    Me.parameters.Item(Input.HideOnScreenDisplay) = (Not value.Value).ToNumberString
                Else
                    Me.parameters.Remove(Input.HideOnScreenDisplay)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to repeat playback.
        ''' </summary>
        Public Property ActionOnFinish As ActionOnFinish
            Get
                Return _actionOnFinish
            End Get
            Set(value As ActionOnFinish)
                _actionOnFinish = value

                If value IsNot Nothing Then
                    Me.parameters.Item(Input.ActionOnFinish) = value.Value
                Else
                    Me.parameters.Remove(Input.ActionOnFinish)
                End If
            End Set
        End Property

        Public Property ActionOnExit As ActionOnExit
            Get
                Return _actionOnExit
            End Get
            Set(value As ActionOnExit)
                _actionOnExit = value

                If value IsNot Nothing Then
                    Me.parameters.Item(Input.ActionOnExit) = value.Value
                Else
                    Me.parameters.Remove(Input.ActionOnExit)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether video output is enabled.
        ''' </summary>
        Public Property VideoEnabled As Boolean?
            Get
                Return _videoEnabled
            End Get
            Set(value As Boolean?)
                _videoEnabled = value

                If value.HasValue Then
                    Me.parameters.Item(Input.VideoEnabled) = value.Value.ToNumberString
                Else
                    Me.parameters.Remove(Input.VideoEnabled)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a video zoom preset.
        ''' </summary>
        Public Property VideoZoom As VideoZoom
            Get
                Return _videoZoom
            End Get
            Set(value As VideoZoom)
                _videoZoom = value

                If value IsNot Nothing Then
                    Me.parameters.Item(Input.VideoZoom) = value.Value
                Else
                    Me.parameters.Remove(Input.VideoZoom)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a volume percentage.
        ''' </summary>
        Public Property PlaybackVolume As Integer?
            Get
                Return _playbackVolume
            End Get
            Set(value As Integer?)
                _playbackVolume = value

                If value.HasValue Then
                    Me.parameters.Item(Input.PlaybackVolume) = value.Value.ToString
                Else
                    Me.parameters.Remove(Input.PlaybackVolume)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether audio output is enabled.
        ''' </summary>
        Public Property AudioEnabled As Boolean?
            Get
                Return _playbackMute
            End Get
            Set(value As Boolean?)
                If value.HasValue Then
                    _playbackMute = Not value
                    Me.parameters.Item(Input.PlaybackMute) = (Not value.Value).ToNumberString
                Else
                    _playbackMute = Nothing
                    Me.parameters.Remove(Input.PlaybackMute)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an audio track number.
        ''' </summary>
        Public Property AudioTrack As Integer?
            Get
                Return _audioTrack
            End Get
            Set(value As Integer?)
                _audioTrack = value

                If value.HasValue Then
                    Me.parameters.Item(Input.AudioTrack) = value.Value.ToString
                Else
                    Me.parameters.Remove(Input.AudioTrack)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a subtitles track number.
        ''' </summary>
        Public Property SubtitlesTrack As Integer?
            Get
                Return _subtitleTrack
            End Get
            Set(value As Integer?)
                _subtitleTrack = value

                If value.HasValue Then
                    Me.parameters.Item(Input.SubtitlesTrack) = value.Value.ToString
                Else
                    Me.parameters.Remove(Input.SubtitlesTrack)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether video output is shown on top of OSD graphics.
        ''' </summary>
        Public Property VideoOnTop As Boolean?
            Get
                Return _videoOnTop
            End Get
            Set(value As Boolean?)
                _videoOnTop = value

                If value.HasValue Then
                    Me.parameters.Item(Input.VideoOnTop) = value.Value.ToNumberString
                Else
                    Me.parameters.Remove(Input.VideoOnTop)
                End If
            End Set
        End Property

        Public MustOverride Overrides ReadOnly Property RequiredVersion As Version

    End Class

End Namespace
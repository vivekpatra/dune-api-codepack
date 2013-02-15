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

    ''' <summary>This command is used to send a new playback request.</summary>
    Public Class StartPlaybackCommand : Inherits PlaybackCommand

        Private Shared _requiredVersion1 As Version = New Version(1, 0)
        Private Shared _requiredVersion3 As Version = New Version(3, 0)

        Private _type As PlaybackType
        Private _mediaUrl As String
        Private _startIndex As Integer

        ''' <param name="mediaUrl">The media URL.</param>
        ''' <remarks>Does not support playlists.</remarks>
        Public Sub New(mediaUrl As String)
            MyBase.New(New CommandValue(String.Empty))
            Me.MediaUrl = mediaUrl
        End Sub

        ''' <summary>
        ''' Gets or sets the playback type.
        ''' </summary>
        Public Property Type As PlaybackType
            Get
                Return _type
            End Get
            Set(value As PlaybackType)
                _type = value
                Select Case value
                    Case PlaybackType.Auto
                        Me.Command = CommandValue.LaunchMediaUrl
                    Case PlaybackType.File
                        Me.Command = CommandValue.StartFilePlayback
                    Case PlaybackType.Dvd
                        Me.Command = CommandValue.StartDvdPlayback
                    Case PlaybackType.Bluray
                        Me.Command = CommandValue.StartBlurayPlayback
                    Case PlaybackType.Playlist
                        Me.Command = CommandValue.StartPlaylistPlayback
                End Select
            End Set
        End Property

        ''' <summary>
        ''' Gets the requested media URL.
        ''' </summary>
        Public Property MediaUrl As String
            Get
                Return _mediaUrl
            End Get
            Set(value As String)
                _mediaUrl = value
                If value IsNot Nothing Then
                    Me.parameters.Item(Input.MediaUrl) = value
                Else
                    Me.parameters.Remove(Input.MediaUrl)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the playlist start index.
        ''' </summary>
        Public Property StartIndex As Integer
            Get
                Return _startIndex
            End Get
            Set(value As Integer)
                _startIndex = value
            End Set
        End Property

        ''' <summary>
        ''' Enumeration of supported playback types.
        ''' </summary>
        Public Enum PlaybackType
            Auto = 0
            File = 1
            Dvd = 2
            Bluray = 3
            Playlist = 4
        End Enum

        Public Overrides Function CanExecute(target As Dune) As Boolean
            If MyBase.CanExecute(target) Then
                If Type = PlaybackType.Auto Or Type = PlaybackType.Playlist Then
                    Return target.ProtocolVersion.Major >= 3
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Select Case Me.Command
                    Case CommandValue.StartFilePlayback, CommandValue.StartDvdPlayback, CommandValue.StartBlurayPlayback
                        Return _requiredVersion1
                    Case CommandValue.StartPlaylistPlayback, CommandValue.LaunchMediaUrl
                        Return _requiredVersion3
                    Case Else
                        Throw New NotImplementedException
                End Select
            End Get
        End Property

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Return MyBase.GetRequestMessage(target, HttpMethod.Post, target.GetBaseAddress.Uri)
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New StatusCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
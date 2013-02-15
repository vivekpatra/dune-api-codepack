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

Namespace IPControl

    ''' <summary>
    ''' A helper class for retrieving and comparing command values and for creating new command values.
    ''' </summary>
    Public Class CommandValue : Inherits NameValuePair

        Private Shared ReadOnly _status As CommandValue = New CommandValue("status")
        Private Shared ReadOnly _mainScreen As CommandValue = New CommandValue("main_screen")
        Private Shared ReadOnly _blackScreen As CommandValue = New CommandValue("black_screen")
        Private Shared ReadOnly _standby As CommandValue = New CommandValue("standby")
        Private Shared ReadOnly _irCode As CommandValue = New CommandValue("ir_code")
        Private Shared ReadOnly _dvdNavigation As CommandValue = New CommandValue("dvd_navigation")
        Private Shared ReadOnly _blurayNavigation As CommandValue = New CommandValue("bluray_navigation")
        Private Shared ReadOnly _setPlaybackState As CommandValue = New CommandValue("set_playback_state")
        Private Shared ReadOnly _startDvdPlayback As CommandValue = New CommandValue("start_dvd_playback")
        Private Shared ReadOnly _startBlurayPlayback As CommandValue = New CommandValue("start_bluray_playback")
        Private Shared ReadOnly _startFilePlayback As CommandValue = New CommandValue("start_file_playback")
        Private Shared ReadOnly _startPlaylistPlayback As CommandValue = New CommandValue("start_playlist_playback")
        Private Shared ReadOnly _launchMediaUrl As CommandValue = New CommandValue("launch_media_url")
        Private Shared ReadOnly _getText As CommandValue = New CommandValue("get_text")
        Private Shared ReadOnly _setText As CommandValue = New CommandValue("set_text")
        Private Shared ReadOnly _list As CommandValue = New CommandValue("ls")

        Public Sub New(command As String)
            MyBase.New(command)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.Command.Name
            End Get
        End Property

        ''' <summary>
        ''' Represents a status command.
        ''' </summary>
        Public Shared ReadOnly Property Status As CommandValue
            Get
                Return CommandValue._status
            End Get
        End Property

        ''' <summary>
        ''' Represents a main_screen command.
        ''' </summary>
        Public Shared ReadOnly Property MainScreen As CommandValue
            Get
                Return CommandValue._mainScreen
            End Get
        End Property

        ''' <summary>
        ''' Represents a black_screen command.
        ''' </summary>
        Public Shared ReadOnly Property BlackScreen As CommandValue
            Get
                Return CommandValue._blackScreen
            End Get
        End Property

        ''' <summary>
        ''' Represents a standby command.
        ''' </summary>
        Public Shared ReadOnly Property Standby As CommandValue
            Get
                Return CommandValue._standby
            End Get
        End Property

        ''' <summary>
        ''' Represents an ir_code command.
        ''' </summary>
        Public Shared ReadOnly Property IrCode As CommandValue
            Get
                Return CommandValue._irCode
            End Get
        End Property

        ''' <summary>
        ''' Represents a dvd_navigation command.
        ''' </summary>
        Public Shared ReadOnly Property DvdNavigation As CommandValue
            Get
                Return CommandValue._dvdNavigation
            End Get
        End Property

        ''' <summary>
        ''' Represents a bluray_navigation command.
        ''' </summary>
        Public Shared ReadOnly Property BlurayNavigation As CommandValue
            Get
                Return CommandValue._blurayNavigation
            End Get
        End Property

        ''' <summary>
        ''' Represents a set_playback_state command.
        ''' </summary>
        Public Shared ReadOnly Property SetPlaybackState As CommandValue
            Get
                Return CommandValue._setPlaybackState
            End Get
        End Property

        ''' <summary>
        ''' Represents a start_dvd_playback command.
        ''' </summary>
        Public Shared ReadOnly Property StartDvdPlayback As CommandValue
            Get
                Return CommandValue._startDvdPlayback
            End Get
        End Property

        ''' <summary>
        ''' Represents a start_bluray_playback command.
        ''' </summary>
        Public Shared ReadOnly Property StartBlurayPlayback As CommandValue
            Get
                Return CommandValue._startBlurayPlayback
            End Get
        End Property

        ''' <summary>
        ''' Represents a start_file_playback command.
        ''' </summary>
        Public Shared ReadOnly Property StartFilePlayback As CommandValue
            Get
                Return CommandValue._startFilePlayback
            End Get
        End Property

        ''' <summary>
        ''' Represents a start_playlist_playback command.
        ''' </summary>
        Public Shared ReadOnly Property StartPlaylistPlayback As CommandValue
            Get
                Return CommandValue._startPlaylistPlayback
            End Get
        End Property

        ''' <summary>
        ''' Represents a launch_media_url command.
        ''' </summary>
        Public Shared ReadOnly Property LaunchMediaUrl As CommandValue
            Get
                Return CommandValue._launchMediaUrl
            End Get
        End Property

        ''' <summary>
        ''' Represents a get_text command.
        ''' </summary>
        Public Shared ReadOnly Property GetText As CommandValue
            Get
                Return CommandValue._getText
            End Get
        End Property

        ''' <summary>
        ''' Represents a set_text command.
        ''' </summary>
        Public Shared ReadOnly Property SetText As CommandValue
            Get
                Return CommandValue._setText
            End Get
        End Property

        ''' <summary>
        ''' Represents a list command.
        ''' </summary>
        Public Shared ReadOnly Property List As CommandValue
            Get
                Return CommandValue._list
            End Get
        End Property

    End Class

End Namespace
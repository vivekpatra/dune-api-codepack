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
    ''' A helper class for retrieving and comparing player states and for creating new player states.
    ''' </summary>
    Public Class PlayerState : Inherits NameValuePair

        Private Shared ReadOnly _standby As PlayerState = New PlayerState("standby")
        Private Shared ReadOnly _loading As PlayerState = New PlayerState("loading")
        Private Shared ReadOnly _navigator As PlayerState = New PlayerState("navigator")
        Private Shared ReadOnly _torrentDownloads As PlayerState = New PlayerState("torrent_downloads")
        Private Shared ReadOnly _photoViewer As PlayerState = New PlayerState("photo_viewer")
        Private Shared ReadOnly _blackScreen As PlayerState = New PlayerState("black_screen")
        Private Shared ReadOnly _dvdPlayback As PlayerState = New PlayerState("dvd_playback")
        Private Shared ReadOnly _blurayPlayback As PlayerState = New PlayerState("bluray_playback")
        Private Shared ReadOnly _filePlayback As PlayerState = New PlayerState("file_playback")
        Private Shared ReadOnly _safeMode As PlayerState = New PlayerState("safe_mode")

        Private Shared ReadOnly _indexSetup As PlayerState = New PlayerState("index_setup")
        Private Shared ReadOnly _systemStorageSetup As PlayerState = New PlayerState("sysstor_setup")
        Private Shared ReadOnly _firmwareUpgradeSetup As PlayerState = New PlayerState("fw_upgrade_setup")
        Private Shared ReadOnly _genericSetup As PlayerState = New PlayerState("generic_setup")
        Private Shared ReadOnly _systemInformation As PlayerState = New PlayerState("sysinfo")
        Private Shared ReadOnly _networkSetup As PlayerState = New PlayerState("network_setup")
        Private Shared ReadOnly _torrentSetup As PlayerState = New PlayerState("torrent_setup")
        Private Shared ReadOnly _videoSetup As PlayerState = New PlayerState("video_setup")

        Private Const ObsoleteInfo As String = "This player state is no longer used, starting with protocol version 2."

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PlayerState"/> class with a specific player state.
        ''' </summary>
        Public Sub New(playerState As String)
            MyBase.New(playerState)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Output.PlayerState.Name
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the current player state is a playback state.
        ''' </summary>
        Public ReadOnly Property IsPlaybackState As Boolean
            Get
                Return Me.Equals(PlayerState.FilePlayback) Or Me.Equals(PlayerState.DvdPlayback) Or Me.Equals(PlayerState.BlurayPlayback)
            End Get
        End Property

        Public Shared ReadOnly Property Standby As PlayerState
            Get
                Return PlayerState._standby
            End Get
        End Property

        Public Shared ReadOnly Property Loading As PlayerState
            Get
                Return PlayerState._loading
            End Get
        End Property

        Public Shared ReadOnly Property Navigator As PlayerState
            Get
                Return PlayerState._navigator
            End Get

        End Property

        Public Shared ReadOnly Property TorrentDownloads As PlayerState
            Get
                Return PlayerState._torrentDownloads
            End Get
        End Property

        Public Shared ReadOnly Property PhotoViewer As PlayerState
            Get
                Return PlayerState._photoViewer
            End Get
        End Property

        Public Shared ReadOnly Property BlackScreen As PlayerState
            Get
                Return PlayerState._blackScreen
            End Get
        End Property

        Public Shared ReadOnly Property DvdPlayback As PlayerState
            Get
                Return PlayerState._dvdPlayback
            End Get
        End Property

        Public Shared ReadOnly Property BlurayPlayback As PlayerState
            Get
                Return PlayerState._blurayPlayback
            End Get
        End Property

        Public Shared ReadOnly Property FilePlayback As PlayerState
            Get
                Return PlayerState._filePlayback
            End Get
        End Property

        Public Shared ReadOnly Property SafeMode As PlayerState
            Get
                Return PlayerState._safeMode
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property IndexSetup As PlayerState
            Get
                Return PlayerState._indexSetup
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property SystemStorageSetup As PlayerState
            Get
                Return PlayerState._systemStorageSetup
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property FirmwareUpgradeSetup As PlayerState
            Get
                Return PlayerState._firmwareUpgradeSetup
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property GenericSetup As PlayerState
            Get
                Return PlayerState._genericSetup
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property SystemInformation As PlayerState
            Get
                Return PlayerState._systemInformation
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property NetworkSetup As PlayerState
            Get
                Return PlayerState._networkSetup
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property TorrentSetup As PlayerState
            Get
                Return PlayerState._torrentSetup
            End Get
        End Property

        <Obsolete(ObsoleteInfo)>
        Public Shared ReadOnly Property VideoSetup As PlayerState
            Get
                Return PlayerState._videoSetup
            End Get
        End Property

    End Class

End Namespace
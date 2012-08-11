#Region "License"
' Copyright 2012 Steven Liekens
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
Imports System.Net
Imports System.IO
Imports SL.DuneApiCodePack.NativeMethods.Networking
Imports System.ComponentModel
Imports SL.DuneApiCodePack.DuneUtilities

Namespace Sources

    ''' <summary>
    ''' This type represents a storage device which is installed in or directly attached to the player.
    ''' This includes internal hard drives, hot swappable drives, CD/DVD/Blu-Ray discs, external USB drives, USB flash drives, SD cards and just about any other form of data storage not mentioned.
    ''' </summary>
    Public Class LocalStorage
        Inherits StorageDevice

        Private _name As String
        Private _label As String
        Private _uuid As String
        Private _caption As String

        ''' <param name="host">The host (i.e. the Dune device) where the storage is attached/installed.</param>
        ''' <param name="path">The UNC path to the storage device.</param>
        ''' <remarks>
        ''' This constructor tries to derive the storage name, storage caption and storage UUID from the specified path.
        ''' The path does not have to be existent so it can be spoofed.
        ''' </remarks>
        Public Sub New(host As IPHostEntry, path As FileSystemInfo)
            MyBase.New(host)
            Dim root As DirectoryInfo = path.GetRoot
            _name = root.Name

            ParseStorageDetails(root)
        End Sub

        Public Sub New(host As Dune, storageName As String)
            MyBase.New(host.HostEntry)

            _name = storageName

            Dim hostNameOrAddress As String
            If host.Address IsNot Nothing Then
                hostNameOrAddress = host.Address.ToString
            Else
                hostNameOrAddress = host.HostName
            End If

            Dim root As New DirectoryInfo(String.Concat("\\", hostNameOrAddress, "\", storageName))

            ParseStorageDetails(Root)
        End Sub

        Private Sub ParseStorageDetails(root As FileSystemInfo)
            _root = root.ToDirectoryInfo
            If _root.Exists Then
                Dim settings As Dictionary(Of String, String) = GetDuneFolderSettings()

                If settings.Keys.Contains("storage_caption") Then
                    _caption = settings("storage_caption")
                End If

                If settings.Keys.Contains("storage_name") Then
                    _name = settings("storage_name")
                    Exit Sub ' because additional parsing wouldn't make sense
                End If
            End If

            If _name.Contains("usb_storage") Or _name.Contains("optical_drive") Then
                Dim uuidIndex As Integer
                Dim pieces() As String = _name.Split("_"c)

                If IsNumeric(pieces(2)) Then ' this is not the first usb_storage or optical_drive without a label
                    uuidIndex = pieces(0).Length + pieces(1).Length + pieces(2).Length + 3
                Else ' everything after the second underscore is a UUID
                    uuidIndex = pieces(0).Length + pieces(1).Length + 2
                End If

                If uuidIndex > _name.Length Then ' there isn't actually a UUID
                    uuidIndex = _name.Length
                End If

                _label = _name.Left(uuidIndex - 1)
                _uuid = _name.Substring(uuidIndex)
            ElseIf _name.Contains("_") Then
                _label = _name.Left(_name.IndexOf("_"c))
                _uuid = _name.Substring(_label.Length + 1)
            End If
        End Sub


        ''' <summary>
        ''' The storage name.
        ''' </summary>
        <DisplayName("storage_name://")>
        Public ReadOnly Property StorageName As String
            Get
                Return _name
            End Get
        End Property

        ''' <summary>
        ''' The storage label. This can be found by looking at the information screen on the device.
        ''' </summary>
        <DisplayName("storage_label://")>
        Public Property StorageLabel As String
            Get
                Return _label
            End Get
            Set(value As String)
                _label = value
            End Set
        End Property

        ''' <summary>
        ''' The storage UUID. This can be found by looking at the information screen on the device.
        ''' </summary>
        <DisplayName("storage_uuid://")>
        Public Property StorageUuid As String
            Get
                Return _uuid
            End Get
            Set(value As String)
                _uuid = value
            End Set
        End Property

        ''' <summary>
        ''' The storage caption. This can be found in the dune_folder.txt file. It is only used to alter the storage display name.
        ''' </summary>
        <DisplayName("Caption")>
        Public Property StorageCaption As String
            Get
                Return _caption
            End Get
            Set(value As String)
                _caption = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the media URL for the specified path.
        ''' </summary>
        ''' <param name="path">The UNC path which needs to be converted.</param>
        ''' <returns>The media URL in 'storage_name://actual_value/path[/file[.extension]]' format.</returns>
        Public Function GetMediaUrlFromStorageName(path As FileSystemInfo) As String
            If String.IsNullOrWhiteSpace(StorageName) Then
                Throw New System.Configuration.ConfigurationErrorsException("Storage name is not specified!")
            Else
                Dim container As New DirectoryInfo(path.FullName)

                Dim prefix As String = "storage_name://" + StorageName
                Dim suffix As String = path.FullName.Replace(container.Root.FullName, String.Empty).Replace("\"c, "/"c)
                Return prefix + suffix
            End If
        End Function

        ''' <summary>
        ''' Tries to get the media URL for the specified path.
        ''' </summary>
        ''' <param name="path">The UNC path which needs to be converted.</param>
        ''' <param name="mediaUrl">The string variable that will contain the media URL if the method call is successful.</param>
        ''' <returns>True if the call was successful; otherwise false.</returns>
        Public Function TryGetMediaUrlFromStorageName(path As FileSystemInfo, ByRef mediaUrl As String) As Boolean
            If String.IsNullOrWhiteSpace(StorageName) Then
                Return False
            Else
                mediaUrl = GetMediaUrlFromStorageName(path)
                Return True
            End If
        End Function

        ''' <summary>
        ''' Gets the media URL for the specified path.
        ''' </summary>
        ''' <param name="path">The UNC path which needs to be converted.</param>
        ''' <returns>The media URL in 'storage_label://actual_value/path[/file[.extension]]' format.</returns>
        Public Function GetMediaUrlFromStorageLabel(path As FileSystemInfo) As String
            If String.IsNullOrWhiteSpace(StorageLabel) Then
                Throw New System.Configuration.ConfigurationErrorsException("Storage label is not specified!")
            Else
                Dim container As New DirectoryInfo(path.FullName)

                Dim prefix As String = "storage_label://" + StorageLabel
                Dim suffix As String = path.FullName.Replace(container.Root.FullName, String.Empty).Replace("\"c, "/"c)
                Return prefix + suffix
            End If
        End Function

        ''' <summary>
        ''' Tries to get the media URL for the specified path.
        ''' </summary>
        ''' <param name="path">The UNC path which needs to be converted.</param>
        ''' <param name="mediaUrl">The string variable that will contain the media URL if the method call is successful.</param>
        ''' <returns>True if the call was successful; otherwise false.</returns>
        Public Function TryGetMediaUrlFromStorageLabel(path As FileSystemInfo, ByRef mediaUrl As String) As Boolean
            If String.IsNullOrWhiteSpace(StorageLabel) Then
                Return False
            Else
                mediaUrl = GetMediaUrlFromStorageLabel(path)
                Return True
            End If
        End Function

        ''' <summary>
        ''' Gets the media URL for the specified path.
        ''' </summary>
        ''' <param name="path">The UNC path which needs to be converted.</param>
        ''' <returns>The media URL in 'storage_uuid://actual_value/path[/file[.extension]]' format.</returns>
        Public Function GetMediaUrlFromStorageUuid(path As FileSystemInfo) As String
            If String.IsNullOrWhiteSpace(StorageUuid) Then
                Throw New System.Configuration.ConfigurationErrorsException("Storage UUID is not specified!")
            Else
                Dim container As New DirectoryInfo(path.FullName)

                Dim prefix As String = "storage_uuid://" + StorageUuid
                Dim suffix As String = path.FullName.Replace(container.Root.FullName, String.Empty).Replace("\"c, "/"c)
                Return prefix + suffix
            End If
        End Function

        ''' <summary>
        ''' Tries to get the media URL for the specified path.
        ''' </summary>
        ''' <param name="path">The UNC path which needs to be converted.</param>
        ''' <param name="mediaUrl">The string variable that will contain the media URL if the method call is successful.</param>
        ''' <returns>True if the call was successful; otherwise false.</returns>
        Public Function TryGetMediaUrlFromStorageUuid(path As FileSystemInfo, ByRef mediaUrl As String) As Boolean
            If String.IsNullOrWhiteSpace(StorageUuid) Then
                Return False
            Else
                mediaUrl = GetMediaUrlFromStorageUuid(path)
                Return True
            End If
        End Function

        ''' <summary>
        ''' Parses settings from a dune_folder.txt file in the root of the storage.
        ''' </summary>
        ''' <returns>A dictionary containing the settings if a dune_folder.txt was found, otherwise nothing.</returns>
        Private Function GetDuneFolderSettings() As Dictionary(Of String, String)
            Dim duneFolderTxt As New FileInfo(Root.FullName + "\dune_folder.txt")
            Dim settings As Dictionary(Of String, String)

            If duneFolderTxt.Exists Then
                Dim reader As New StreamReader(duneFolderTxt.OpenRead)
                settings = New Dictionary(Of String, String)

                Do Until reader.EndOfStream
                    If ChrW(reader.Peek) = "#"c Then ' ignore comment
                        reader.ReadLine()
                    Else ' get name=value pair
                        Dim nameValuePair As String = reader.ReadLine
                        If nameValuePair.Contains("=") AndAlso nameValuePair.IndexOf("="c) = nameValuePair.LastIndexOf("="c) Then
                            Dim name As String = nameValuePair.Split("="c)(0).Trim.ToLowerInvariant
                            Dim value As String = nameValuePair.Split("="c)(1).Trim
                            settings.Add(name, value)
                        End If
                    End If
                Loop
            Else
                settings = New Dictionary(Of String, String)
            End If

            Return settings
        End Function

        Public Shared Iterator Function FromHost(host As DuneApiCodePack.DuneUtilities.Dune) As IEnumerable(Of LocalStorage)
            Dim client As New FtpClient(host)
            Dim shares() As String = client.ListDirectory(client.Root)

            For Each directory As String In shares
                Yield New LocalStorage(host, directory)
            Next
        End Function

        ''' <summary>
        ''' Gets the best possible media URL for the root of this storage device.
        ''' </summary>
        ''' <remarks>
        ''' This method tries to generate a media url until it succeeds in the following order:
        ''' storage_uuid:// -> storage_name:// -> storage_label:// -> null.</remarks>
        Public Overrides Function GetMediaUrl() As String
            Return Me.GetMediaUrl(Root)
        End Function

        ''' <summary>
        ''' Gets the best possible media URL for the specified path.
        ''' </summary>
        ''' <remarks>
        ''' This method tries to generate a media url until it succeeds in the following order:
        ''' storage_uuid:// -> storage_name:// -> storage_label:// -> null.</remarks>
        Public Overloads Function GetMediaUrl(path As FileSystemInfo) As String
            Dim mediaUrl As String = Nothing

            If TryGetMediaUrlFromStorageUuid(path, mediaUrl) Then
                Return mediaUrl
            ElseIf TryGetMediaUrlFromStorageName(path, mediaUrl) Then
                Return mediaUrl
            ElseIf TryGetMediaUrlFromStorageLabel(path, mediaUrl) Then
                Return mediaUrl
            Else
                Return String.Empty
            End If
        End Function

        Public Overrides Function ToString() As String
            If StorageCaption.IsNotNullOrEmpty Then
                Return StorageCaption
            Else
                Return StorageName
            End If
        End Function
    End Class

End Namespace
Imports System.Net
Imports System.IO

Namespace Storage

    ''' <summary>
    ''' This type represents a storage device which is installed in or directly attached to the player.
    ''' This includes internal hard drives, hot swappable drives, CD/DVD/Blu-Ray discs, external USB drives, USB flash drives, SD cards and just about any other form of data storage not mentioned.
    ''' </summary>
    Public Class LocalStorage
        Inherits Storage

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
        Public Sub New(ByVal host As IPHostEntry, ByVal path As DirectoryInfo)
            MyBase.New(host)
            _root = path.Root

            If _root.Exists Then
                Dim settings As Dictionary(Of String, String) = GetDuneFolderSettings()

                If settings.Keys.Contains("storage_name") Then
                    settings.TryGetValue("storage_name", _name)
                Else
                    _name = path.Root.Name
                End If

                If settings.Keys.Contains("storage_caption") Then
                    settings.TryGetValue("storage_caption", _caption)
                End If
            Else
                _name = path.Root.Name
            End If

            If _name.Contains("usb_storage") Or _name.Contains("optical_disk") Then
                Dim uuidIndex As Integer
                Dim pieces() As String = _name.Split("_"c)

                If IsNumeric(pieces(2)) Then
                    uuidIndex = pieces(0).Length + pieces(1).Length + pieces(2).Length + 3
                Else
                    uuidIndex = pieces(0).Length + pieces(1).Length + 2
                End If

                If uuidIndex > _name.Length Then
                    uuidIndex = _name.Length
                End If

                _label = _name.Substring(0, uuidIndex - 1)
                _uuid = _name.Substring(uuidIndex)
            ElseIf _name.Contains("_") Then
                _label = Left(_name, _name.IndexOf("_"c))
                _uuid = _name.Substring(_label.Length + 1)
            End If
        End Sub

        ''' <summary>
        ''' The storage name.
        ''' </summary>
        Public Property StorageName As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ''' <summary>
        ''' The storage label. This can be found by looking at the information screen on the device.
        ''' </summary>
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
        Public Property StorageUuid As String
            Get
                Return _uuid
            End Get
            Set(value As String)
                _uuid = value
            End Set
        End Property

        ''' <summary>
        ''' The storage caption. This can be found in the dune_folder.txt file. It is only used for the display caption.
        ''' </summary>
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
        ''' <returns>The media URL in 'storage_name://actual_value/path[/file.extension]' format.</returns>
        Public Function GetMediaUrlFromStorageName(ByVal path As FileSystemInfo) As String
            If String.IsNullOrWhiteSpace(StorageName) Then
                Throw New NullReferenceException("Storage name is not specified!")
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
        Public Function TryGetMediaUrlFromStorageName(ByVal path As FileSystemInfo, ByRef mediaUrl As String) As Boolean
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
        ''' <returns>The media URL in 'storage_label://actual_value/path[/file.extension]' format.</returns>
        Public Function GetMediaUrlFromStorageLabel(ByVal path As FileSystemInfo) As String
            If String.IsNullOrWhiteSpace(StorageLabel) Then
                Throw New NullReferenceException("Storage label is not specified!")
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
        Public Function TryGetMediaUrlFromStorageLabel(ByVal path As FileSystemInfo, ByRef mediaUrl As String) As Boolean
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
        ''' <returns>The media URL in 'storage_uuid://actual_value/path[/file.extension]' format.</returns>
        Public Function GetMediaUrlFromStorageUuid(ByVal path As FileSystemInfo) As String
            If String.IsNullOrWhiteSpace(StorageUuid) Then
                Throw New NullReferenceException("Storage UUID is not specified!")
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
        Public Function TryGetMediaUrlFromStorageUuid(ByVal path As FileSystemInfo, ByRef mediaUrl As String) As Boolean
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
                            Dim name As String = nameValuePair.Split("="c)(0).Trim.ToLower
                            Dim value As String = nameValuePair.Split("="c)(1).Trim
                            settings.Add(name, value)
                        End If
                    End If
                Loop
            Else
                settings = Nothing
            End If

            Return settings
        End Function

    End Class

End Namespace
Imports System.IO
Imports SL.DuneApiCodePack.DuneUtilities.ApiWrappers
Imports System.Timers
Imports System.ComponentModel
Imports System.Collections.ObjectModel


Namespace DuneUtilities

    ''' <summary>
    ''' A collection of files that represent a music playlist.
    ''' </summary>
    ''' <remarks>
    ''' This class uses a client-side playlist implementation and should be updated when a server-side API becomes available.
    ''' </remarks>
    Public Class Playlist
        Implements INotifyPropertyChanged

        Private _dune As Dune
        Private _files As ObservableCollection(Of FileInfo)
        Private _position As Integer
        Private _shuffle As Boolean
        Private _repeat As Boolean
        Private _removeLastFile As Boolean
        Private _random As Random
        Private WithEvents _statusWatchTimer As Timer

#Region "Constructors"

        ''' <summary>
        ''' Creates an empty playlist.
        ''' Use the methods and properties on the <see cref="Files"/> list to populate it.
        ''' </summary>
        ''' <param name="dune">The target device.</param>
        Public Sub New(dune As Dune)
            _dune = dune
            _files = New ObservableCollection(Of FileInfo)
            _random = New Random
            _statusWatchTimer = New Timer()
        End Sub

        ''' <summary>
        ''' Creates a playlist from a list of files.
        ''' </summary>
        ''' <param name="dune">The target device.</param>
        ''' <param name="files">The collection of files to add to the playlist.</param>
        Public Sub New(dune As Dune, files As Collection(Of FileInfo))
            Me.New(dune)

            For Each file As FileInfo In files
                _files.Add(file)
            Next
        End Sub

        ''' <summary>
        ''' Load a playlist from a playlist file.
        ''' </summary>
        ''' <param name="dune">The target device.</param>
        ''' <param name="path">Path to a playlist file.</param>
        Public Sub New(dune As Dune, path As Uri)
            Me.New(dune)

            ' TODO: implement playlist parsers
            Throw New NotImplementedException("Playlist parsers are not yet implemented.")
        End Sub

#End Region

        ''' <summary>
        ''' Gets the playlist.
        ''' </summary>
        Public ReadOnly Property Files As ObservableCollection(Of FileInfo)
            Get
                Return _files
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the 0-based index representing the position in the playlist.
        ''' </summary>
        Public Property Position As Integer
            Get
                Return _position
            End Get
            Set(value As Integer)
                If value >= Files.Count Then
                    If Repeat.IsTrue Then
                        value = 0
                    Else
                        value = -1
                    End If
                ElseIf value < 0 Then
                    value = 0
                End If

                If value <> _position Then
                    _position = value
                    RaisePropertyChanged("Position")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to shuffle playback without changing the playlist order.
        ''' </summary>
        Public Property Shuffle As Boolean
            Get
                Return _shuffle
            End Get
            Set(value As Boolean)
                If value <> _shuffle Then
                    _shuffle = value
                    RaisePropertyChanged("Shuffle")
                    If value.IsTrue Then
                        Position = _random.Next(0, Files.Count - 1)
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the playlist should repeat from the beginning if the end is reached.
        ''' </summary>
        ''' <remarks>If you want to repeat a single file, set the repeat property on the individual playback command instead.</remarks>
        Public Property Repeat As Boolean
            Get
                Return _repeat
            End Get
            Set(value As Boolean)
                If value <> _repeat Then
                    _repeat = value
                    RaisePropertyChanged("Repeat")
                End If
            End Set
        End Property

        ''' <summary>
        ''' If enabled, files are removed from the playlist after playback started.
        ''' This is useful if you want to schedule playback commands for single playback.
        ''' </summary>
        Public Property RemoveLastFile As Boolean
            Get
                Return _removeLastFile
            End Get
            Set(value As Boolean)
                If value <> _removeLastFile Then
                    _removeLastFile = value
                    RaisePropertyChanged("RemoveLastFile")
                End If
            End Set
        End Property


        ''' <summary>
        ''' Starts the playlist at the current <see cref="Position"/>.
        ''' </summary>
        Public Sub Start()
            _statusWatchTimer.Stop()

            Dim file As FileInfo = Files(Position)

            Dim command As New StartPlaybackCommand(_dune, file.FullName)
            command.Timeout = _dune.Timeout


            Select Case file.Extension.ToLower
                Case ".iso"
                    If file.Length > (8.5 * 1024 ^ 3) Then ' assume it is a bluray disc image
                        command.Type = StartPlaybackCommand.PlaybackType.Bluray
                    Else ' assume it is a dvd disc image
                        command.Type = StartPlaybackCommand.PlaybackType.Dvd
                    End If
                Case Else
                    command.Type = StartPlaybackCommand.PlaybackType.File
            End Select

            Console.WriteLine(file)

            Dim result As CommandResult = command.GetResult

            If result.CommandStatus = "ok" Or result.CommandStatus = "timeout" Then
                _statusWatchTimer.Start()
            ElseIf result.CommandStatus = "failed" Then
                Start()
            End If

        End Sub

        ''' <summary>
        ''' Stops the playlist.
        ''' </summary>
        Public Sub [Stop]()
            _statusWatchTimer.Stop()
            Position = 0
        End Sub

        ''' <summary>
        ''' Starts the next file in the playlist.
        ''' </summary>
        Public Sub [Next]()

            Position += 1

            If Position > -1 Then Start()
        End Sub

        ''' <summary>
        ''' Starts the previous file in the playlist.
        ''' </summary>
        Public Sub Previous()
            _statusWatchTimer.Stop()

            Position -= 1

            If Position > -1 Then Start()
        End Sub

        Private Sub _statusWatchTimer_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs) Handles _statusWatchTimer.Elapsed
            If _dune.PlaybackPosition = _dune.PlaybackDuration Then

                Console.WriteLine(_dune.PlaybackPosition.ToString + "/" + _dune.PlaybackDuration.ToString)



                If Shuffle = False Then
                    If RemoveLastFile.IsTrue Then
                        Files.RemoveAt(Position)
                    Else
                        Position += 1
                    End If
                Else
                    If RemoveLastFile.IsTrue Then
                        Files.RemoveAt(Position)
                    End If
                    Position = _random.Next(0, Files.Count - 1)
                End If


                If Position > -1 Then
                    Start()
                Else
                    _statusWatchTimer.Stop()
                End If

            End If
        End Sub

        ''' <summary>
        ''' Helper method for the INotifyPropertyChanged implementation.
        ''' </summary>
        Private Sub RaisePropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

End Namespace

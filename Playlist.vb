Imports System.IO
Imports DuneAPICodePack.Dune.ApiWrappers
Imports System.Timers

Namespace Dune

    ''' <summary>
    ''' A collection of files that represent a music playlist.
    ''' </summary>
    ''' <remarks>
    ''' This class uses a client-side playlist implementation and should be updated when a server-side API becomes available.
    ''' </remarks>
    Public Class Playlist
        Private _dune As Dune
        Private _files As List(Of FileInfo)
        Private _position As Integer
        Private _shuffle As Boolean
        Private _repeat As Boolean
        Private _removeLastFile As Boolean
        Private WithEvents _statusWatchTimer As Timer

#Region "Constructors"

        ''' <summary>
        ''' Creates an empty playlist.
        ''' Use the methods and properties on the <see cref="Files"/> list to populate it.
        ''' </summary>
        ''' <param name="dune">The target device.</param>
        Public Sub New(ByVal dune As Dune)
            _dune = dune
            _files = New List(Of FileInfo)
            _statusWatchTimer = New Timer()
        End Sub

        ''' <summary>
        ''' Creates a playlist from a list of files.
        ''' </summary>
        ''' <param name="dune">The target device.</param>
        ''' <param name="files">The list of files to add to the playlist.</param>
        Public Sub New(ByVal dune As Dune, ByVal files As List(Of FileInfo))
            _dune = dune
            _files = files
            _statusWatchTimer = New Timer(1000)
        End Sub

        ''' <summary>
        ''' Load a playlist from a playlist file.
        ''' </summary>
        ''' <param name="dune">The target device.</param>
        ''' <param name="path">Path to a playlist file.</param>
        Public Sub New(ByVal dune As Dune, ByVal path As Uri)
            _dune = dune
            ' TODO: implement playlist parsers
            Throw New NotImplementedException("Playlist parsers are not yet implemented.")
            _statusWatchTimer = New Timer(1000)
        End Sub

#End Region

        ''' <summary>
        ''' Gets the playlist.
        ''' </summary>
        Public ReadOnly Property Files As List(Of FileInfo)
            Get
                Return _files
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets 0-based index representing the position in the playlist.
        ''' </summary>
        Public Property Position As Integer
            Get
                Return _position
            End Get
            Set(value As Integer)
                If value >= Files.Count Then
                    If Repeat = True Then
                        value = 0
                    Else
                        value = -1
                    End If
                ElseIf value < 0 Then
                    value = 0
                End If
                _position = value
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
                _shuffle = value
                If value = True Then
                    Position = _random.Next(0, Files.Count - 1)
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
                _repeat = value
            End Set
        End Property

        ''' <summary>
        ''' If enabled, files are removed from the playlist after playback started.
        ''' This is useful for scheduling playback commands.
        ''' </summary>
        Public Property RemoveLastFile As Boolean
            Get
                Return _removeLastFile
            End Get
            Set(value As Boolean)
                _removeLastFile = True
            End Set
        End Property


        Public Sub Start()
            _statusWatchTimer.Stop()

            Dim file As FileInfo = Files(Position)

            Dim command As New StartPlaybackCommand(_dune, file.FullName)
            command.Timeout = _dune.Timeout


            Select file.Extension.ToLower
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

            Dim result As New CommandResult(command)

            If result.CommandStatus = "ok" Or result.CommandStatus = "timeout" Then
                _statusWatchTimer.Start()
            ElseIf result.CommandStatus = "failed" Then
                Start()
            End If

        End Sub

        Public Sub [Stop]()
            _statusWatchTimer.Stop()
            Position = 0
        End Sub

        Public Sub [Next]()

            Position += 1

            If Position > -1 Then Start()
        End Sub

        Public Sub Previous()
            _statusWatchTimer.Stop()

            Position -= 1

            If Position > -1 Then Start()
        End Sub

        Private _random As New Random
        Private Sub _statusWatchTimer_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs) Handles _statusWatchTimer.Elapsed
            If _dune.PlaybackPosition = _dune.PlaybackDuration Then

                Console.WriteLine(_dune.PlaybackPosition.ToString + "/" + _dune.PlaybackDuration.ToString)



                If Shuffle = False Then
                    If RemoveLastFile = True Then
                        Files.RemoveAt(Position)
                    Else
                        Position += 1
                    End If
                Else
                    If RemoveLastFile = True Then
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
    End Class

End Namespace

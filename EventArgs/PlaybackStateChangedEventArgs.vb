Namespace DuneUtilities

    ''' <summary>
    ''' Provides data for the <see cref="Dune.PlaybackStateChanged"/> event.
    ''' </summary>
    Public Class PlaybackStateChangedEventArgs
        Inherits EventArgs

        Private _playbackState As String
        Private _previousPlaybackState As String

        Public Sub New(playbackState As String, previousPlaybackState As String)
            _playbackState = playbackState
            _previousPlaybackState = previousPlaybackState
        End Sub

        ''' <summary>
        ''' Gets the current playback state.
        ''' </summary>
        Public ReadOnly Property PlaybackState As String
            Get
                Return _playbackState
            End Get
        End Property

        ''' <summary>
        ''' Gets the previous playback state.
        ''' </summary>
        Public ReadOnly Property PreviousPlaybackState As String
            Get
                Return _previousPlaybackState
            End Get
        End Property
    End Class

End Namespace

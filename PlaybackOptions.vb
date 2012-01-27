Namespace Dune.Communicator
    Public Class PlaybackOptions
        Private _playbackSpeed As Integer
        Private _position As Integer
        Private _blackScreen As Boolean
        Private _hideOSD As Boolean
        Private _repeat As Boolean

        Public Sub New()
            Me.New(False, 0, False, False, False)
        End Sub

        Public Sub New(ByVal paused As Boolean)
            Me.New(Paused, 0, False, False, False)
        End Sub

        Public Sub New(ByVal paused As Boolean, ByVal position As Integer)
            Me.New(paused, position, False, False, False)
        End Sub

        Public Sub New(ByVal paused As Boolean, ByVal position As Integer, ByVal blackScreen As Boolean)
            Me.New(paused, position, blackScreen, False, False)
        End Sub

        Public Sub New(ByVal paused As Boolean, ByVal position As Integer, ByVal blackScreen As Boolean, ByVal hideOSD As Boolean)
            Me.New(paused, position, blackScreen, hideOSD, False)
        End Sub

        Public Sub New(ByVal paused As Boolean, ByVal position As Integer, ByVal BlacblackScreenkScreen As Boolean, ByVal hideOSD As Boolean, ByVal repeat As Boolean)
            If paused = True Then
                _playbackSpeed = 0
            Else
                _playbackSpeed = 256
            End If

            _position = position

            _blackScreen = BlackScreen

            _hideOSD = hideOSD

            _repeat = repeat
        End Sub

        Public Property Paused As Boolean
            Get
                Return Not CBool(_playbackSpeed)
            End Get
            Set(value As Boolean)
                If value = True Then
                    _playbackSpeed = 0
                Else
                    _playbackSpeed = 256
                End If
            End Set
        End Property

        Public Property Position As Integer
            Get
                Return _position
            End Get
            Set(value As Integer)
                _position = value
            End Set
        End Property

        Public Property BlackScreen As Boolean
            Get
                Return _blackScreen
            End Get
            Set(value As Boolean)
                _blackScreen = value
            End Set
        End Property

        Public Property HideOSD As Boolean
            Get
                Return _hideOSD
            End Get
            Set(value As Boolean)
                _hideOSD = value
            End Set
        End Property

        Public Property Repeat As Boolean
            Get
                Return _repeat
            End Get
            Set(value As Boolean)
                _repeat = value
            End Set
        End Property

        ''' <summary>
        ''' Used for internal command processing.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Playback speed (0 = paused, 256 = normal)</returns>
        ''' <remarks> Only use the 'Paused' property to change this.</remarks>
        Friend ReadOnly Property Speed As Integer
            Get
                Return _playbackSpeed
            End Get
        End Property

    End Class
End Namespace

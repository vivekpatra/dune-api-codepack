Namespace Dune.Communicator
    Public Class PlaybackOptions
        Private _playbackSpeed As Integer
        Private intPosition As Integer
        Private blnBlackScreen As Boolean
        Private blnHideOSD As Boolean
        Private blnRepeat As Boolean

        Public Sub New()
            Me.New(False, 0, False, False, False)
        End Sub

        Public Sub New(ByVal Paused As Boolean)
            Me.New(Paused, 0, False, False, False)
        End Sub

        Public Sub New(ByVal Paused As Boolean, ByVal Position As Integer)
            Me.New(Paused, Position, False, False, False)
        End Sub

        Public Sub New(ByVal Paused As Boolean, ByVal Position As Integer, ByVal BlackScreen As Boolean)
            Me.New(Paused, Position, BlackScreen, False, False)
        End Sub

        Public Sub New(ByVal Paused As Boolean, ByVal Position As Integer, ByVal BlackScreen As Boolean, ByVal HideOSD As Boolean)
            Me.New(Paused, Position, BlackScreen, HideOSD, False)
        End Sub

        Public Sub New(ByVal Paused As Boolean, ByVal Position As Integer, ByVal BlackScreen As Boolean, ByVal HideOSD As Boolean, ByVal Repeat As Boolean)
            If Paused = True Then
                _playbackSpeed = 0
            Else
                _playbackSpeed = 256
            End If

            intPosition = Position

            blnBlackScreen = BlackScreen

            blnHideOSD = HideOSD

            blnRepeat = Repeat
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
                Return intPosition
            End Get
            Set(value As Integer)
                intPosition = value
            End Set
        End Property

        Public Property BlackScreen As Boolean
            Get
                Return blnBlackScreen
            End Get
            Set(value As Boolean)
                blnBlackScreen = value
            End Set
        End Property

        Public Property HideOSD As Boolean
            Get
                Return blnHideOSD
            End Get
            Set(value As Boolean)
                blnHideOSD = value
            End Set
        End Property

        Public Property Repeat As Boolean
            Get
                Return blnRepeat
            End Get
            Set(value As Boolean)
                blnRepeat = value
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

Namespace Dune.Communicator
    Public Class StandardRemote
        Private _dune As Dune
        Private _remoteCode As String = "BF00" 'defaults to "new" remotes.

        ''' <summary>
        ''' Constructor that attaches a Dune object to the standardremote object.
        ''' </summary>
        ''' <param name="Dune"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dune As Dune)
            _dune = dune
        End Sub

        ''' <summary>
        ''' Private default constructor to prevent orphan objects.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub

        Public Enum Remote
            Old
            [New]
        End Enum

        ''' <summary>
        ''' Gets or sets the remote type. I don't know if the "Old remote" setting actually works with the IP control API.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RemoteType As Remote
            Get
                Select Case _remoteCode
                    Case "BE01"
                        Return Remote.Old
                    Case Else
                        Return Remote.New
                End Select
            End Get
            Set(ByVal value As Remote)
                Select Case value.ToString.ToLower
                    Case "old"
                        _remoteCode = "BE01"
                    Case Else
                        _remoteCode = "BF00"
                End Select
            End Set
        End Property

#Region "Remote Control Buttons"
        ''' <summary>
        ''' Sends the button command.
        ''' </summary>
        ''' <param name="Code">IR code in hex format.</param>
        ''' <remarks></remarks>
        Private Sub Push(ByVal code As String)
            Dim commandURL As String = String.Format("http://{0}:{1}/cgi-bin/do?cmd=ir_code&ir_code={2}{3}&timeout={4}", _dune.IP, _dune.Port, code, _remoteCode, _dune.Communicator.Timeout)
            _dune.Communicator.DoCommand(commandURL)
        End Sub

        Public Sub Power()
            Push("BC43")
        End Sub

        Public Sub Eject()
            Push("EF10")
        End Sub

        Public Sub Mute()
            Push("B946")
        End Sub

        Public Sub Mode()
            Push("BA45")
        End Sub

        Public Sub A()
            Push("BF40")
        End Sub

        Public Sub B()
            Push("E01F")
        End Sub

        Public Sub C()
            Push("FF00")
        End Sub

        Public Sub D()
            Push("BE41")
        End Sub

        Public Sub Num1()
            Push("F40B")
        End Sub

        Public Sub Num2()
            Push("F30C")
        End Sub

        Public Sub Num3()
            Push("F20D")
        End Sub

        Public Sub Num4()
            Push("F10E")
        End Sub

        Public Sub Num5()
            Push("F00F")
        End Sub

        Public Sub Num6()
            Push("FE01")
        End Sub

        Public Sub Num7()
            Push("EE11")
        End Sub

        Public Sub Num8()
            Push("ED12")
        End Sub

        Public Sub Num9()
            Push("EC13")
        End Sub

        Public Sub Num0()
            Push("F50A")
        End Sub

        Public Sub Clear()
            Push("FA05")
        End Sub

        Public Sub Select_CapNum()
            Push("BD42")
        End Sub

        Public Sub VolumeUp()
            Push("AD52")
        End Sub

        Public Sub VolumeDown()
            Push("AC53")
        End Sub

        Public Sub PageUp()
            Push("B44B")
        End Sub

        Public Sub PageDown()
            Push("B34C")
        End Sub

        Public Sub Search()
            Push("F906")
        End Sub

        Public Sub Zoom()
            Push("FD02")
        End Sub

        Public Sub Setup()
            Push("BE41")
        End Sub

        Public Sub Up()
            Push("EA15")
        End Sub

        Public Sub Down()
            Push("E916")
        End Sub

        Public Sub Left()
            Push("E817")
        End Sub

        Public Sub Right()
            Push("E718")
        End Sub

        Public Sub Enter()
            Push("EB14")
        End Sub

        Public Sub Return_Back()
            Push("FB04")
        End Sub

        Public Sub Info()
            Push("AF50")
        End Sub

        Public Sub PopUpMenu()
            Push("F807")
        End Sub

        Public Sub TopMenu()
            Push("AE51")
        End Sub

        Public Sub Play()
            Push("B748")
        End Sub

        Public Sub Pause()
            Push("E11E")
        End Sub

        Public Sub Previous()
            Push("B649")
        End Sub

        Public Sub Next_Skip()
            Push("E21D")
        End Sub

        Public Sub Stop_Exit()
            Push("E619")
        End Sub

        Public Sub Slow()
            Push("E51A")
        End Sub

        Public Sub Rewind()
            Push("E31C")
        End Sub

        Public Sub Forward()
            Push("E41B")
        End Sub

        Public Sub Subtitle()
            Push("AB54")
        End Sub

        Public Sub Angle_Rotate()
            Push("B24D")
        End Sub

        Public Sub Audio()
            Push("BB44")
        End Sub

        Public Sub Record()
            Push("B04F")
        End Sub

        Public Sub Shuffle_PIP()
            Push("B847")
        End Sub

        Public Sub URL_2ndAudio()
            Push("FC03")
        End Sub
#End Region
    End Class
End Namespace
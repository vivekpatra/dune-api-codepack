Imports System.ComponentModel

Namespace Dune.ApiWrappers

    ''' <summary>
    ''' Interface that defines buttons on the remote control.
    ''' </summary>
    Public Interface IRemoteControl

        Function Push(ByVal button As Button) As CommandResult

        ''' <summary>
        ''' Enumeration of remote types.
        ''' </summary>
        ''' <remarks></remarks>
        Enum RemoteType As Integer
            Old = &HBE01
            [New] = &HBF
        End Enum

        ''' <summary>
        ''' Enumeration of supported buttons.
        ''' </summary>
        Enum Button

            None = 0

#Region "First row"

            <Description("Eject")>
                Eject = &H10EF

            <Description("Mute")>
                Mute = &H46B9

            <Description("Mode")>
                Mode = &H45BA

            <Description("Power")>
                Power = &H43BC

#End Region ' First row

#Region "Second row"

            <Description("A (red)")>
                Red = &H40BF

            <Description("B (green)")>
                Green = &H1FE0

            <Description("C (yellow)")>
                Yellow = &HFF

            <Description("D (blue)")>
                Blue = &H41BE

#End Region ' Second row

#Region "Third row"

            <Description("1 (.:/)")>
                Num1 = &HBF4

            <Description("2 (abc)")>
                Num2 = &HCF3

            <Description("3 (def)")>
                Num3 = &HDF2

#End Region ' Third row

#Region "Fourth row"

            <Description("4 (ghi)")>
                Num4 = &HEF1

            <Description("5 (jkl)")>
                Num5 = &HFF0

            <Description("6 (mno)")>
                Num6 = &H1FE

#End Region ' Fourth row

#Region "Fifth row"

            <Description("7 (pqrs)")>
                Num7 = &H11EE

            <Description("8 (tuv)")>
                Num8 = &H12ED

            <Description("9 (wxyz)")>
                Num9 = &H13EC

#End Region ' Fifth row

#Region "Sixth row"

            <Description("Clear")>
                Clear = &H5FA

            <Description("0 (␣)")>
                Num0 = &HAF5

            <Description("Select (cap/num)")>
                [Select] = &H42BD

#End Region ' Sixth row

#Region "Seventh row"

            <Description("Volume up")>
                VolumeUp = &H52AD

            <Description("Search (?)")>
                Search = &H6F9

            <Description("Zoom")>
                Zoom = &H2FD

            <Description("Page up")>
                PageUp = &H4BB4

#End Region ' Seventh row

#Region "Eighth row"

            <Description("Volume down")>
               VolumeDown = &H53AC

            <Description("Setup")>
                Setup = &H4EB1

            <Description("Page down")>
                PageDown = &H4CB3

#End Region ' Eighth row

#Region "Ninth row"

            <Description("Info")>
               Info = &H50AF

            <Description("Up arrow")>
                UpArrow = &H15EA

            <Description("Pop up menu")>
                PopUpMenu = &H7F8

#End Region ' Ninth row

#Region "Tenth row"

            <Description("Left arrow")>
               LeftArrow = &H17E8

            <Description("Enter")>
                Enter = &H14EB

            <Description("Right arrow")>
                RightArrow = &H18E7

#End Region ' Tenth row

#Region "Eleventh row"

            <Description("Return")>
               [Return] = &H4FB

            <Description("Down arrow")>
                DownArrow = &H16E9

            <Description("Top menu")>
                TopMenu = &H51AE

#End Region ' Eleventh row

#Region "Twelfth row"

            <Description("Play")>
                Play = &H48B7

            <Description("Pause")>
                Pause = &H1EE1

            <Description("Previous")>
                Previous = &H49B6

            <Description("Next")>
                [Next] = &H1DE2

#End Region ' Twelfth row

#Region "Thirteenth row"

            <Description("Stop")>
                [Stop] = &H19E6

            <Description("Slow")>
                Slow = &H1AE5

            <Description("Rewind")>
                Rewind = &H1CE3

            <Description("Forward")>
                Forward = &H1BE4

#End Region ' Thirteenth row

#Region "Fourteenth row"

            <Description("Subtitle")>
                Subtitle = &H54AB

            <Description("Angle/Rotate")>
                AngleRotate = &H4DB2

            <Description("Audio")>
                Audio = &H44BB

#End Region ' Fourteenth row

#Region "Fifteenth row"

            <Description("Repeat/Record")>
                RepeatRecord = &H4FB0

            <Description("Shuffle/PIP")>
                ShufflePip = &H47B8

            <Description("URL/2nd audio")>
                UrlSecondAudio = &H3FC

#End Region ' Fifteenth row

            DiscretePowerOn = &H5FA0

            DiscretePowerOff = &H5EA1

            ' Big V2 Only
            REC = &H609F ' Acts like Rec/Repeat and Mode buttons

            DUNE = &H619E

            URL = &H629D

        End Enum

    End Interface

End Namespace
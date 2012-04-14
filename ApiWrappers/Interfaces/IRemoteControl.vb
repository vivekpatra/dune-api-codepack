Imports System.ComponentModel

Namespace Dune.ApiWrappers

    ''' <summary>
    ''' Interface that defines buttons on the remote control.
    ''' </summary>
    Public Interface IRemoteControl

        Function Push(ByVal button As Button) As CommandResult

        ''' <summary>
        ''' Enumeration of supported buttons.
        ''' </summary>
        Enum Button

#Region "First row"

            <Description("Eject")>
            <InfraredCode("EF10")>
                Eject

            <Description("Mute")>
            <InfraredCode("B946")>
                Mute

            <Description("Mode")>
            <InfraredCode("BA45")>
                Mode

            <Description("Power")>
            <InfraredCode("BC43")>
                Power

#End Region ' First row

#Region "Second row"

            <Description("A (red)")>
            <InfraredCode("BF40")>
                Red

            <Description("B (green)")>
            <InfraredCode("E01F")>
                Green

            <Description("C (yellow)")>
            <InfraredCode("FF00")>
                Yellow

            <Description("D (blue)")>
            <InfraredCode("BE41")>
                Blue

#End Region ' Second row

#Region "Third row"

            <Description("1 (.:/)")>
            <InfraredCode("F40B")>
                Num1

            <Description("2 (abc)")>
            <InfraredCode("F30C")>
                Num2

            <Description("3 (def)")>
            <InfraredCode("F20D")>
                Num3

#End Region ' Third row

#Region "Fourth row"

            <Description("4 (ghi)")>
            <InfraredCode("F10E")>
                Num4

            <Description("5 (jkl)")>
            <InfraredCode("F00F")>
                Num5

            <Description("6 (mno)")>
            <InfraredCode("FE01")>
                Num6

#End Region ' Fourth row

#Region "Fifth row"

            <Description("7 (pqrs)")>
            <InfraredCode("EE11")>
                Num7

            <Description("8 (tuv)")>
            <InfraredCode("ED12")>
                Num8

            <Description("9 (wxyz)")>
            <InfraredCode("EC13")>
                Num9

#End Region ' Fifth row

#Region "Sixth row"

            <Description("Clear")>
            <InfraredCode("FA05")>
                Clear

            <Description("0 (␣)")>
            <InfraredCode("F50A")>
                Num0

            <Description("Select (cap/num)")>
            <InfraredCode("BD42")>
                [Select]

#End Region ' Sixth row

#Region "Seventh row"

            <Description("Volume up")>
            <InfraredCode("AD52")>
                VolumeUp

            <Description("Search (?)")>
            <InfraredCode("F906")>
                Search

            <Description("Zoom")>
            <InfraredCode("FD02")>
                Zoom

            <Description("Page up")>
            <InfraredCode("B44B")>
                PageUp

#End Region ' Seventh row

#Region "Eighth row"

            <Description("Volume down")>
            <InfraredCode("AC53")>
               VolumeDown

            <Description("Setup")>
            <InfraredCode("BE41")>
                Setup

            <Description("Page down")>
            <InfraredCode("B34C")>
                PageDown

#End Region ' Eighth row

#Region "Ninth row"

            <Description("Info")>
            <InfraredCode("AF50")>
               Info

            <Description("Up arrow")>
            <InfraredCode("EA15")>
                UpArrow

            <Description("Pop up menu")>
            <InfraredCode("F807")>
                PopUpMenu

#End Region ' Ninth row

#Region "Tenth row"

            <Description("Left arrow")>
            <InfraredCode("E817")>
               LeftArrow

            <Description("Enter")>
            <InfraredCode("EB14")>
                Enter

            <Description("Right arrow")>
            <InfraredCode("E718")>
                RightArrow

#End Region ' Tenth row

#Region "Eleventh row"

            <Description("Return")>
            <InfraredCode("FB04")>
               [Return]

            <Description("Down arrow")>
            <InfraredCode("E916")>
                DownArrow

            <Description("Top menu")>
            <InfraredCode("AE51")>
                TopMenu

#End Region ' Eleventh row

#Region "Twelfth row"

            <Description("Play")>
            <InfraredCode("B748")>
                Play

            <Description("Pause")>
            <InfraredCode("E11E")>
                Pause

            <Description("Previous")>
            <InfraredCode("B649")>
                Previous

            <Description("Next")>
            <InfraredCode("E21D")>
                [Next]

#End Region ' Twelfth row

#Region "Thirteenth row"

            <Description("Stop")>
            <InfraredCode("E619")>
                [Stop]

            <Description("Slow")>
            <InfraredCode("E51A")>
                Slow

            <Description("Rewind")>
            <InfraredCode("E31C")>
                Rewind

            <Description("Forward")>
            <InfraredCode("E41B")>
                Forward

#End Region ' Thirteenth row

#Region "Fourteenth row"

            <Description("Subtitle")>
            <InfraredCode("AB54")>
                Subtitle

            <Description("Angle/Rotate")>
            <InfraredCode("B24D")>
                AngleRotate

            <Description("Audio")>
            <InfraredCode("BB44")>
                Audio

#End Region ' Fourteenth row

#Region "Fifteenth row"

            <Description("Repeat/Record")>
            <InfraredCode("B04F")>
                RepeatRecord

            <Description("Shuffle/PIP")>
            <InfraredCode("B847")>
                ShufflePip

            <Description("URL/2nd audio")>
            <InfraredCode("FC03")>
                UrlSecondAudio

#End Region ' Fifteenth row

        End Enum

    End Interface

End Namespace
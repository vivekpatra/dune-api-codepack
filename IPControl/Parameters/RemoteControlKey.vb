#Region "License"
' Copyright 2012-2013 Steven Liekens
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

Namespace IPControl

    ''' <summary>
    ''' A helper class for retrieving and comparing remote control keys and for creating new remote control keys.
    ''' </summary>
    Public Class RemoteControlKey : Inherits NameValuePair

        Private Const CustomerCode As Byte = &HBF

        Public Sub New(button As key)
            MyBase.New(ToNec(button))
        End Sub

        Public Sub New(button As Byte)
            MyBase.New(ToNec(button))
        End Sub

        Public Sub New(action As NavigationAction)
            MyBase.New(ToNec(GetNavigationKey(action)))
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.IRCode.Name
            End Get
        End Property

        ''' <summary>
        ''' Returns the hexadecimal string representation of the 32-bit NEC code for a button.
        ''' </summary>
        Private Shared Function ToNec(button As Byte) As String
            Dim key() As Byte = {&H0, &HBF, button, Not button}
            If BitConverter.IsLittleEndian Then Array.Reverse(key)
            Return BitConverter.ToString(key).Replace("-", String.Empty)
        End Function

        ''' <summary>
        ''' Returns the key that corresponds to the specified navigation action.
        ''' </summary>
        Private Shared Function GetNavigationKey(action As NavigationAction) As Key
            Select Case action
                Case NavigationAction.Left : Return Key.Left
                Case NavigationAction.Right : Return Key.Right
                Case NavigationAction.Up : Return Key.Up
                Case NavigationAction.Down : Return Key.Down
                Case NavigationAction.Enter : Return Key.Enter
                Case Else : Throw New ArgumentException("invalid action")
            End Select
        End Function

    End Class

    ''' <summary>
    ''' Enumerates the available key values.
    ''' </summary>
    Public Enum Key As Byte
        Eject = &H10
        Mute = &H46
        Mode = &H45
        Power = &H43
        A = &H40
        B = &H1F
        C = &H0
        D = &H41
        Num0 = &HA
        Num1 = &HB
        Num2 = &HC
        Num3 = &HD
        Num4 = &HE
        Num5 = &HF
        Num6 = &H1
        Num7 = &H11
        Num8 = &H12
        Num9 = &H13
        Clear = &H5
        [Select] = &H42
        VolumeUp = &H52
        VolumeDown = &H53
        PageUp = &H4B
        PageDown = &H4C
        Search = &H6
        Zoom = &H2
        Setup = &H4E
        Up = &H15
        Down = &H16
        Left = &H17
        Right = &H18
        Enter = &H14
        [Return] = &H4
        Info = &H50
        PopUpMenu = &H7
        TopMenu = &H51
        TopMenu2 = &H57
        Play = &H48
        Pause = &H1E
        PlayPause = &H56
        Previous = &H49
        [Next] = &H1D
        [Stop] = &H19
        Slow = &H1A
        Rewind = &H1C
        Forward = &H1B
        Subtitle = &H54
        Angle = &H4D
        Rotate = &H4D
        Audio = &H44
        Repeat = &H4F
        Record = &H60
        Record2 = &H55
        Dune = &H61
        Dune2 = &H57
        Shuffle = &H47
        PictureInPicture = &H47
        Url = &H3
        Url2 = &H62
        SecondAudio = &H3
        DiscretePowerOn = &H5F
        DiscretePowerOff = &H5E
        PowerToggle = &H70
        PowerOff = &H71
    End Enum

End Namespace
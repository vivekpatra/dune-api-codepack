#Region "License"
' Copyright 2012 Steven Liekens
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
Imports SL.DuneApiCodePack.DuneUtilities.ApiWrappers

Namespace DuneUtilities

    ''' <summary>This class represents a standard remote control, much like the physical remote that comes with the box.</summary>
    Public Class RemoteControl
        Private _target As Dune

        ''' <param name="target">The target device.</param>
        Public Sub New(target As Dune)
            _target = target
        End Sub

        ''' <summary>
        ''' Emulates a button press.
        ''' </summary>
        ''' <param name="button">The button to emulate. Possible values are enumerated in <see cref="Constants.RemoteControls"/>.</param>
        Public Function PushButton(button As Short) As CommandResult
            Dim command As New RemoteControlCommand(_target, button)
            Dim result As CommandResult = Target.ProcessCommand(command)
            Return result
        End Function

        ''' <summary>
        ''' Emulates a button on the small remote control.
        ''' </summary>
        ''' <param name="button">The button to emulate.</param>
        Public Function PushSmallRemoteButton(button As Constants.RemoteControls.SmallRemoteButtonValues) As CommandResult
            Return PushButton(CShort(button))
        End Function

        ''' <summary>
        ''' Emulates a button on the big remote control (version 1).
        ''' </summary>
        ''' <param name="button">The button to emulate.</param>
        Public Function PushBigRemoteButton(button As Constants.RemoteControls.BigRemoteButtonValues) As CommandResult
            Return PushButton(CShort(button))
        End Function

        ''' <summary>
        ''' Emulates a button on the big remote control (version 2).
        ''' </summary>
        ''' <param name="button">The button to emulate.</param>
        Public Function PushBigRemote2Button(button As Constants.RemoteControls.BigRemote2ButtonValues) As CommandResult
            Return PushButton(CShort(button))
        End Function

        ''' <summary>
        ''' Emulates a special button.
        ''' </summary>
        ''' <param name="button">The button to emulate.</param>
        Public Function PushSpecialButton(button As Constants.RemoteControls.SpecialButtonValues) As CommandResult
            Return PushButton(CShort(button))
        End Function


        ''' <summary>
        ''' Gets the target Dune device.
        ''' </summary>
        Public ReadOnly Property Target As Dune
            Get
                Return _target
            End Get
        End Property
    End Class

End Namespace
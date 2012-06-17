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
Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to emulate a button press on the remote control.</summary>
    Public Class RemoteControlCommand
        Inherits Command

        Private _button As Short
        Private _code As String

        ''' <param name="target">The target device.</param>
        ''' <param name="button">The button to send.</param>
        Public Sub New(target As Dune, button As Short)
            MyBase.New(target)
            _button = button
        End Sub

        ''' <summary>
        ''' Gets the string representation of a hexadecimal code which represents the specified button.
        ''' </summary>
        Public ReadOnly Property HexCode As String
            Get
                If _code.IsNullOrEmpty Then
                    _code = Constants.RemoteControls.GetButtonCode(_button)
                End If
                Return _code
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", Constants.CommandValues.InfraredCode)
            query.Add(Constants.InfraredCodeParameterNames.InfraredCode, HexCode)

            Return query
        End Function
    End Class

End Namespace
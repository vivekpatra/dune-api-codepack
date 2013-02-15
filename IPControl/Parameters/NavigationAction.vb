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
    ''' A helper class for retrieving and comparing navigation actions and for creating new navigation actions.
    ''' </summary>
    Public Class NavigationAction : Inherits NameValuePair

        Private Shared ReadOnly _left As NavigationAction = New NavigationAction("left")
        Private Shared ReadOnly _right As NavigationAction = New NavigationAction("right")
        Private Shared ReadOnly _up As NavigationAction = New NavigationAction("up")
        Private Shared ReadOnly _down As NavigationAction = New NavigationAction("down")
        Private Shared ReadOnly _enter As NavigationAction = New NavigationAction("enter")

        Public Sub New(action As String)
            MyBase.New(action)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.Action.Name
            End Get
        End Property

        Public Shared ReadOnly Property Left As NavigationAction
            Get
                Return NavigationAction._left
            End Get
        End Property

        Public Shared ReadOnly Property Right As NavigationAction
            Get
                Return NavigationAction._right
            End Get
        End Property

        Public Shared ReadOnly Property Up As NavigationAction
            Get
                Return NavigationAction._up
            End Get
        End Property

        Public Shared ReadOnly Property Down As NavigationAction
            Get
                Return NavigationAction._down
            End Get
        End Property

        Public Shared ReadOnly Property Enter As NavigationAction
            Get
                Return NavigationAction._enter
            End Get
        End Property

    End Class

End Namespace
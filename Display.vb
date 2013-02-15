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
Imports System.Drawing

Public Class Display

    Private _size As Size
    Private _window As Rectangle

    Public Sub New(size As Size)
        With Me
            ._size = size
            ._window = New Rectangle(New Point(0, 0), size)
        End With
    End Sub

    Private ReadOnly Property Rectangle As Rectangle
        Get
            Return _window
        End Get
    End Property

    Public Property Position As Point
        Get
            Return Rectangle.Location
        End Get
        Set(value As Point)
            _window.Location = value
        End Set
    End Property

    Public Property Size As Size
        Get
            Return Rectangle.Size
        End Get
        Set(value As Size)
            _window.Size = value
        End Set
    End Property

    Public ReadOnly Property Min As Point
        Get
            Return New Point(0, 0)
        End Get
    End Property

    Public ReadOnly Property Max As Point
        Get
            Return New Point(_size.Width, _size.Height)
        End Get
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not TypeOf obj Is Display Then
            Return False
        End If

        With DirectCast(obj, Display)
            Select Case False
                Case .Position = Me.Position
                    Return False
                Case .Size = Me.Size
                    Return False
                Case .Max = Me.Max
                    Return False
                Case Else
                    Return True
            End Select
        End With
    End Function

End Class
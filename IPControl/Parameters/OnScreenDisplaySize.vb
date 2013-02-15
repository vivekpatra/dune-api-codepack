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
    ''' A helper class for retrieving and comparing video modes and for creating new video modes.
    ''' </summary>
    Public Class OnScreenDisplaySize : Inherits NameValuePair

        Private Shared ReadOnly _720x480 As OnScreenDisplaySize = New OnScreenDisplaySize("720x480")
        Private Shared ReadOnly _720x576 As OnScreenDisplaySize = New OnScreenDisplaySize("720x576")
        Private Shared ReadOnly _1280x720 As OnScreenDisplaySize = New OnScreenDisplaySize("1280x720")
        Private Shared ReadOnly _1920x1080 As OnScreenDisplaySize = New OnScreenDisplaySize("1920x1080")

        ''' <summary>
        ''' Initializes a new instance of the <see cref="OnScreenDisplaySize"/> class with a specific video mode.
        ''' </summary>
        Public Sub New(size As String)
            MyBase.New(size)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.OnScreenDisplaySize.Name
            End Get
        End Property

        Public Shared ReadOnly Property NTSC As OnScreenDisplaySize
            Get
                Return OnScreenDisplaySize._720x480
            End Get
        End Property

        Public Shared ReadOnly Property PAL As OnScreenDisplaySize
            Get
                Return OnScreenDisplaySize._720x576
            End Get
        End Property

        Public Shared ReadOnly Property HD As OnScreenDisplaySize
            Get
                Return OnScreenDisplaySize._1280x720
            End Get
        End Property

        Public Shared ReadOnly Property FullHD As OnScreenDisplaySize
            Get
                Return OnScreenDisplaySize._1920x1080
            End Get
        End Property

    End Class

End Namespace
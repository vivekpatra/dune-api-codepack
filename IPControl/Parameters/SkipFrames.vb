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
    ''' A helper class for retrieving and comparing keyframe directions and for creating new keyframe directions.
    ''' </summary>
    Public Class SkipFrames : Inherits NameValuePair

        Private Shared ReadOnly _backward As SkipFrames = New SkipFrames("-1")
        Private Shared ReadOnly _forward As SkipFrames = New SkipFrames("1")

        Public Sub New(direction As String)
            MyBase.New(direction)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.SkipFrames.Name
            End Get
        End Property

        Public Shared ReadOnly Property Backward As SkipFrames
            Get
                Return SkipFrames._backward
            End Get
        End Property

        Public Shared ReadOnly Property Forward As SkipFrames
            Get
                Return SkipFrames._forward
            End Get
        End Property

    End Class

End Namespace
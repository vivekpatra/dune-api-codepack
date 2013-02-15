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
    ''' A helper class for retrieving and comparing video zoom presets and for creating new video zoom presets.
    ''' </summary>
    Public Class VideoZoom : Inherits NameValuePair

        Private Shared ReadOnly _normal As VideoZoom = New VideoZoom("normal")
        Private Shared ReadOnly _fullEnlarge As VideoZoom = New VideoZoom("full_enlarge")
        Private Shared ReadOnly _fullStretch As VideoZoom = New VideoZoom("full_stretch")
        Private Shared ReadOnly _fillScreen As VideoZoom = New VideoZoom("fill_screen")
        Private Shared ReadOnly _fullFillScreen As VideoZoom = New VideoZoom("full_fill_screen")
        Private Shared ReadOnly _enlarge As VideoZoom = New VideoZoom("enlarge")
        Private Shared ReadOnly _makeWider As VideoZoom = New VideoZoom("make_wider")
        Private Shared ReadOnly _makeTaller As VideoZoom = New VideoZoom("make_taller")
        Private Shared ReadOnly _cutEdges As VideoZoom = New VideoZoom("cut_edges")
        Private Shared ReadOnly _other As VideoZoom = New VideoZoom("other")

        Public Sub New(zoom As String)
            MyBase.New(zoom)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.VideoZoom.Name
            End Get
        End Property

        ''' <summary>
        ''' Represents no video zoom preset.
        ''' </summary>
        Public Shared ReadOnly Property Normal As VideoZoom
            Get
                Return VideoZoom._normal
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Full screen.
        ''' </summary>
        Public Shared ReadOnly Property FullEnlarge As VideoZoom
            Get
                Return VideoZoom._fullEnlarge
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Stretch to full screen.
        ''' </summary>
        Public Shared ReadOnly Property FullStretch As VideoZoom
            Get
                Return VideoZoom._fullStretch
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Non-linear stretch.
        ''' </summary>
        Public Shared ReadOnly Property FillScreen As VideoZoom
            Get
                Return VideoZoom._fillScreen
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Non-linear stretch to full screen.
        ''' </summary>
        Public Shared ReadOnly Property FullFillScreen As VideoZoom
            Get
                Return VideoZoom._fullFillScreen
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Enlarge.
        ''' </summary>
        Public Shared ReadOnly Property Enlarge As VideoZoom
            Get
                Return VideoZoom._enlarge
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Make wider.
        ''' </summary>
        Public Shared ReadOnly Property MakeWider As VideoZoom
            Get
                Return VideoZoom._makeWider
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Make taller.
        ''' </summary>
        Public Shared ReadOnly Property MakeTaller As VideoZoom
            Get
                Return VideoZoom._makeTaller
            End Get
        End Property

        ''' <summary>
        ''' Represets a video zoom preset: Cut edges.
        ''' </summary>
        Public Shared ReadOnly Property CutEdges As VideoZoom
            Get
                Return VideoZoom._cutEdges
            End Get
        End Property

        Public Shared ReadOnly Property Other As VideoZoom
            Get
                Return VideoZoom._other
            End Get
        End Property

    End Class

End Namespace
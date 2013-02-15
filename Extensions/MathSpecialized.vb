Imports System.Drawing

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

Namespace Extensions

    ''' <summary>
    ''' Provides static methods for mathematical algorithms not already available on the <see cref="Math"/> type.
    ''' </summary>
    Public MustInherit Class MathSpecialized

        Public Shared Function GetGreatestCommonFactor(m As Integer, n As Integer) As Integer
            Return If(CBool(n), GetGreatestCommonFactor(n, m Mod n), m)
        End Function

        Public Shared Function GetCenteredRectangle(bounds As Rectangle, size As Size) As Rectangle
            Dim width = Math.Min(Math.Abs(size.Width), Math.Abs(bounds.Size.Width))
            Dim height = Math.Min(Math.Abs(size.Height), Math.Abs(bounds.Size.Height))

            Dim x = (Math.Abs(bounds.Center.X) - (width \ 2)) * Math.Sign(bounds.Center.X)
            Dim y = (Math.Abs(bounds.Center.Y) - (height \ 2)) * Math.Sign(bounds.Center.Y)

            bounds.Offset(x, y)

            Return New Rectangle(bounds.Location, New Size(width * Math.Sign(bounds.Center.X), height * Math.Sign(bounds.Center.Y)))
        End Function

    End Class

End Namespace


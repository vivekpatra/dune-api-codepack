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

Namespace IPControl

    Public Class IPControlStateUnknown : Inherits IPControlStateVersion3

        Public Sub New(target As Dune, status As StatusCommandResult)
            MyBase.New(target, status)
        End Sub

        Public Overrides ReadOnly Property ProtocolVersion As Version
            Get
                Return Target.ProtocolVersion
            End Get
        End Property

    End Class

End Namespace


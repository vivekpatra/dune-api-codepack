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

    ''' <summary>This command is used to set the player state.</summary>
    Public Class SetPlayerStateCommand
        Inherits Command

        Private _state As String

        ''' <param name="target">The target device.</param>
        ''' <param name="state">The requested player state.</param>
        Public Sub New(target As Dune, state As String)
            MyBase.New(target)
            _state = state
        End Sub

        Public ReadOnly Property State As String
            Get
                Return _state
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery

            query.Add("cmd", State)

            Return query
        End Function
    End Class

End Namespace
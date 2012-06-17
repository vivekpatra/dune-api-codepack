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

    ''' <summary>
    ''' This command is used to set text to an input box.
    ''' </summary>
    Public Class SetTextCommand
        Inherits Command

        Private _text As String

        ''' <param name="target">The target device.</param>
        ''' <param name="text">The input text.</param>
        Public Sub New(target As Dune, text As String)
            MyBase.New(target)
            _text = text
        End Sub

        ''' <summary>
        ''' Gets the specified text.
        ''' </summary>
        Public ReadOnly Property Text As String
            Get
                Return _text
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery
            query.Add("cmd", Constants.CommandValues.SetText)
            query.Add(Constants.SetTextParameterNames.Text, Text)
            Return query
        End Function
    End Class

End Namespace
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

    ''' <summary>
    ''' Represents the result of a text command request.
    ''' </summary>
    Public Class TextCommandResult : Inherits StatusCommandResult
        Private _text As String

        Public Sub New(request As IIPCommand, output As XDocument, requestDateTime As DateTimeOffset)
            MyBase.New(request, output, requestDateTime)
        End Sub

        Protected Overrides Sub MapActions()
            MyBase.MapActions()
            With Actions
                .Add(Output.Text, AddressOf SetText)
            End With
        End Sub

        Private Sub SetText(value As String)
            Me.Text = value
        End Sub

        ''' <summary>
        ''' Gets whether a text editor is currently active.
        ''' </summary>
        Public ReadOnly Property IsTextFieldActive As Boolean?
            Get
                Return Me.IsSuccessStatusCode
            End Get
        End Property

        ''' <summary>
        ''' Gets the text in the selected text input field, if any.
        ''' </summary>
        Public Property Text As String
            Get
                Return _text
            End Get
            Private Set(value As String)
                _text = value
            End Set
        End Property

        Public Overloads Function GetDifferences(values As TextCommandResult) As IEnumerable(Of String)
            Dim differences As New List(Of String)(MyBase.GetDifferences(values))

            If Not Object.Equals(values.IsTextFieldActive, Me.IsTextFieldActive) Then
                differences.Add(GetPropertyName(Function() Me.IsTextFieldActive))
            End If

            If values.IsTextFieldActive Or Me.IsTextFieldActive Then
                If Not Object.Equals(values.Text, Me.Text) Then
                    differences.Add(GetPropertyName(Function() Me.Text))
                End If
            End If

            Return differences
        End Function

    End Class

End Namespace
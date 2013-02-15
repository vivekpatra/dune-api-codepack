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

    Public MustInherit Class NameValuePair

        Private _value As String

        Public Sub New(value As String)
            _value = value
        End Sub

        Public MustOverride ReadOnly Property Name As String

        Public ReadOnly Property Value As String
            Get
                Return _value
            End Get
        End Property

        Public Overrides Function Equals(obj As Object) As Boolean
            Return Me.Equals(TryCast(obj, NameValuePair))
        End Function

        Public Overloads Function Equals(obj As NameValuePair) As Boolean
            If obj IsNot Nothing Then
                If Object.ReferenceEquals(Me, obj) Then
                    Return True
                ElseIf String.Equals(Me.Name, obj.Name, StringComparison.InvariantCultureIgnoreCase) Then
                    Return String.Equals(Me.Value, obj.Value, StringComparison.InvariantCultureIgnoreCase)
                End If
            End If
            Return False
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return String.Concat(Me.Name, Me.Value).ToUpperInvariant.GetHashCode
        End Function

        Public Overrides Function ToString() As String
            Return Me.Value.ToString
        End Function


        Shared Operator =(left As NameValuePair, right As NameValuePair) As Boolean
            If left Is Nothing Then Return right Is Nothing
            If right Is Nothing Then Return left Is Nothing

            If Object.ReferenceEquals(left, right) Then
                Return True
            ElseIf String.Equals(left.Name, right.Name, StringComparison.InvariantCultureIgnoreCase) Then
                Return String.Equals(left.Value, right.Value, StringComparison.InvariantCultureIgnoreCase)
            End If
            Return False
        End Operator

        Shared Operator <>(left As NameValuePair, right As NameValuePair) As Boolean
            Return Not (left = right)
        End Operator

    End Class

End Namespace
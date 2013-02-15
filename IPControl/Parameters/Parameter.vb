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

    Public MustInherit Class Parameter : Implements IParameter

        Private Property _name As String

        Public Sub New(name As String)
            _name = name
        End Sub

        Public ReadOnly Property Name As String Implements IParameter.Name
            Get
                Return _name
            End Get
        End Property

        Public Overrides Function Equals(obj As Object) As Boolean
            Return Me.Equals(TryCast(obj, Parameter))
        End Function

        Public Overloads Function Equals(obj As Parameter) As Boolean
            Return obj IsNot Nothing AndAlso (Object.ReferenceEquals(Me, obj) OrElse Me.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.Name.ToUpperInvariant.GetHashCode
        End Function

        Public Overrides Function ToString() As String
            Return Me.Name.ToString
        End Function

        Shared Operator =(left As Parameter, right As Parameter) As Boolean
            If left Is Nothing Then Return right Is Nothing
            If right Is Nothing Then Return left Is Nothing

            Return left.Name.Equals(right.Name, StringComparison.InvariantCultureIgnoreCase)
        End Operator

        Shared Operator <>(left As Parameter, right As Parameter) As Boolean
            Return Not (left Is right)
        End Operator

    End Class

End Namespace
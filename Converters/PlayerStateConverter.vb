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
Imports System.ComponentModel

Public Class PlayerStateConverter
    Inherits StringConverter

    Private _standardValues As ArrayList

    Public Sub New()
        Dim states As New ArrayList
        states.Add(Constants.CommandValues.MainScreen)
        states.Add(Constants.CommandValues.BlackScreen)
        states.Add(Constants.CommandValues.Standby)
        _standardValues = states
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(_standardValues)
    End Function
End Class

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

    Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object
        Select Case DirectCast(value, String)
            Case Constants.CommandValues.MainScreen
                Return CObj("Main screen")
            Case Constants.PlayerStateValues.Navigator
                Return CObj("Navigator")
            Case Constants.PlayerStateValues.BlackScreen
                Return CObj("Black screen")
            Case Constants.PlayerStateValues.Standby
                Return CObj("Standby")
            Case Constants.PlayerStateValues.BlurayPlayback
                Return CObj("Blu-ray playback")
            Case Constants.PlayerStateValues.DvdPlayback
                Return CObj("DVD playback")
            Case Constants.PlayerStateValues.FilePlayback
                Return CObj("File playback")
            Case Constants.PlayerStateValues.Loading
                Return CObj("Loading")
            Case Constants.PlayerStateValues.SafeMode
                Return CObj("Safe mode")
            Case Else
                Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Select
    End Function

    Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
        Select Case DirectCast(value, String)
            Case "Main screen"
                Return Constants.CommandValues.MainScreen
            Case "Black screen"
                Return Constants.CommandValues.BlackScreen
            Case "Standby"
                Return Constants.CommandValues.Standby
            Case Else
                Return MyBase.ConvertFrom(context, culture, value)
        End Select
    End Function

End Class

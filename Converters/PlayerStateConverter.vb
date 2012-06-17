﻿Imports System.ComponentModel

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
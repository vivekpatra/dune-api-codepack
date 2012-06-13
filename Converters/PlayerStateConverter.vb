Imports System.ComponentModel

Public Class PlayerStateConverter
    Inherits StringConverter

    Private _standardValues As ArrayList

    Public Sub New()
        Dim states As New ArrayList
        states.Add(Constants.Commands.MainScreen)
        states.Add(Constants.Commands.BlackScreen)
        states.Add(Constants.Commands.Standby)
        _standardValues = states
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(_standardValues)
    End Function
End Class

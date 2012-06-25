Imports System.ComponentModel

Public Class PlaybackSpeedConverter
    Inherits EnumConverter

    Private _values As Dictionary(Of Constants.PlaybackSpeedValues, String)

    Public Sub New()
        MyBase.New(GetType(Constants.PlaybackSpeedValues))

        _values = New Dictionary(Of Constants.PlaybackSpeedValues, String)

        _values.Add(Constants.PlaybackSpeedValues.Rewind8192, "Rewind (32x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind4096, "Rewind (16x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind2048, "Rewind (8x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind1024, "Rewind (4x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind512, "Rewind (2x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind256, "Rewind")
        _values.Add(Constants.PlaybackSpeedValues.Rewind128, "Rewind (1/2x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind64, "Rewind (1/4x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind32, "Rewind (1/8x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind16, "Rewind (1/16x)")
        _values.Add(Constants.PlaybackSpeedValues.Rewind8, "Rewind (1/32x)")
        _values.Add(Constants.PlaybackSpeedValues.Pause, "Pause")
        _values.Add(Constants.PlaybackSpeedValues.Play8, "Slowdown (1/32x)")
        _values.Add(Constants.PlaybackSpeedValues.Play16, "Slowdown (1/16x)")
        _values.Add(Constants.PlaybackSpeedValues.Play32, "Slowdown (1/8x)")
        _values.Add(Constants.PlaybackSpeedValues.Play64, "Slowdown (1/4x)")
        _values.Add(Constants.PlaybackSpeedValues.Play128, "Slowdown (1/2x)")
        _values.Add(Constants.PlaybackSpeedValues.Play256, "Normal")
        _values.Add(Constants.PlaybackSpeedValues.Play512, "Forward")
        _values.Add(Constants.PlaybackSpeedValues.Play1024, "Forward (4x)")
        _values.Add(Constants.PlaybackSpeedValues.Play2048, "Forward (8x)")
        _values.Add(Constants.PlaybackSpeedValues.Play4096, "Forward (16x)")
        _values.Add(Constants.PlaybackSpeedValues.Play8192, "Forward (32x)")
    End Sub

    Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object
        If TypeOf value Is [Enum] AndAlso destinationType = GetType(String) Then
            Return _values(CType(value, Constants.PlaybackSpeedValues))
        ElseIf TypeOf value Is String AndAlso destinationType = GetType(String) Then
            Return ConvertTo([Enum].Parse(GetType(Constants.PlaybackSpeedValues), CStr(value)), GetType(String))
        Else
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End If
    End Function

    Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Return _values.Where(Function(pair) pair.Value = CStr(value)).First.Key
        Else
            Return MyBase.ConvertFrom(context, culture, value)
        End If
    End Function


End Class

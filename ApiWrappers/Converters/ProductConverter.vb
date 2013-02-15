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
Imports System.ComponentModel

Public Class ProductConverter
    Inherits TypeConverter

    Private Shared standardValues As StandardValuesCollection

    Shared Sub New()
        Dim values As New System.Collections.ObjectModel.Collection(Of ProductID)
        With values
            .Add(ProductID.HDPro)
            .Add(ProductID.HDConnect)
            .Add(ProductID.HDBase3D)
            .Add(ProductID.HDTV303D)
            .Add(ProductID.HDTV301)
            .Add(ProductID.HDTV102)
            .Add(ProductID.HDTV101)
            .Add(ProductID.HDLite53D)
            .Add(ProductID.HDDuo)
            .Add(ProductID.HDMax)
            .Add(ProductID.HDSmartB1)
            .Add(ProductID.HDSmartD1)
            .Add(ProductID.HDSmartH1)
            .Add(ProductID.HDBase3)
            .Add(ProductID.BDPrime3)
            .Add(ProductID.HDBase2)
            .Add(ProductID.HDBase)
            .Add(ProductID.HDCenter)
            .Add(ProductID.BDPrime)
            .Add(ProductID.HDMini)
            .Add(ProductID.HDUltra)
        End With
        standardValues = New StandardValuesCollection(values)
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return standardValues
    End Function

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType = GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Dim input As String = DirectCast(value, String).Trim

            Select Case True
                Case String.IsNullOrEmpty(input)
                    Return New ProductID(String.Empty)
                Case String.Equals(input, "Dune HD Pro", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDPro
                Case String.Equals(input, "Dune HD Connect", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDConnect
                Case String.Equals(input, "Dune HD Max", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDMax
                Case String.Equals(input, "Dune HD Duo", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDDuo
                Case String.Equals(input, "Dune HD Base 3D", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDBase3D
                Case String.Equals(input, "Dune HD Smart B1", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDSmartB1
                Case String.Equals(input, "Dune HD Smart H1", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDSmartH1
                Case String.Equals(input, "Dune HD Smart D1", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDSmartD1
                Case String.Equals(input, "Dune HD TV-101", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDTV101
                Case String.Equals(input, "Dune HD TV-102", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDTV102
                Case String.Equals(input, "Dune HD TV-301", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDTV301
                Case String.Equals(input, "Dune HD TV-303D", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDTV303D
                Case String.Equals(input, "Dune HD Lite 53D", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDLite53D
                Case String.Equals(input, "Dune BD Prime 3.0", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.BDPrime3
                Case String.Equals(input, "Dune BD Prime", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.BDPrime
                Case String.Equals(input, "Dune HD Base 3.0", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDBase3
                Case String.Equals(input, "Dune HD Base 2.0", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDBase2
                Case String.Equals(input, "Dune HD Base", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDBase
                Case String.Equals(input, "Dune HD Center", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDCenter
                Case String.Equals(input, "Dune HD Mini", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDMini
                Case String.Equals(input, "Dune HD Ultra", StringComparison.InvariantCultureIgnoreCase)
                    Return ProductID.HDUltra
                Case Else
                    Return New ProductID(DirectCast(value, String))
            End Select
        End If

        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is ProductID Then
            Select Case DirectCast(value, ProductID)
                Case ProductID.HDPro : Return "Dune HD Pro"
                Case ProductID.HDConnect : Return "Dune HD Connect"
                Case ProductID.HDMax : Return "Dune HD Max"
                Case ProductID.HDDuo : Return "Dune HD Duo"
                Case ProductID.HDBase3D : Return "Dune HD Base 3D"
                Case ProductID.HDSmartB1 : Return "Dune HD Smart B1"
                Case ProductID.HDSmartH1 : Return "Dune HD Smart H1"
                Case ProductID.HDSmartD1 : Return "Dune HD Smart D1"
                Case ProductID.HDTV101 : Return "Dune HD TV-101"
                Case ProductID.HDTV102 : Return "Dune HD TV-102"
                Case ProductID.HDTV301 : Return "Dune HD TV-301"
                Case ProductID.HDTV303D : Return "Dune HD TV-303D"
                Case ProductID.HDLite53D : Return "Dune HD Lite 53D"
                Case ProductID.BDPrime3 : Return "Dune BD Prime 3.0"
                Case ProductID.BDPrime : Return "Dune BD Prime"
                Case ProductID.HDBase3 : Return "Dune HD Base 3.0"
                Case ProductID.HDBase2 : Return "Dune HD Base 2.0"
                Case ProductID.HDBase : Return "Dune HD Base"
                Case ProductID.HDCenter : Return "Dune HD Center"
                Case ProductID.HDMini : Return "Dune HD Mini"
                Case ProductID.HDUltra : Return "Dune HD Ultra"
                Case Else : Return DirectCast(value, ProductID).Value
            End Select
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class
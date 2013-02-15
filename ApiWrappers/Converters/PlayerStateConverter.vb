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

Public Class PlayerStateConverter
    Inherits TypeConverter

    Private Shared standardValues As StandardValuesCollection

    Shared Sub New()
        Dim values As New Collection(Of PlayerState)
        With values
            .Add(PlayerState.Navigator)
            .Add(PlayerState.BlackScreen)
            .Add(PlayerState.Standby)
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
            Dim input = DirectCast(value, String).Trim
            Select Case True
                Case String.Equals(input, "Navigator", StringComparison.InvariantCultureIgnoreCase)
                    Return PlayerState.Navigator
                Case String.Equals(input, "Black screen", StringComparison.InvariantCultureIgnoreCase)
                    Return PlayerState.BlackScreen
                Case String.Equals(input, "Standby", StringComparison.InvariantCultureIgnoreCase)
                    Return PlayerState.Standby
                Case Else
                    Return New PlayerState(DirectCast(value, String))
            End Select
        End If

        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is PlayerState Then
            Select Case DirectCast(value, PlayerState)
                Case PlayerState.Navigator : Return "Navigator"
                Case PlayerState.BlackScreen : Return "Black screen"
                Case PlayerState.Standby : Return "Standby"
                Case PlayerState.BlurayPlayback : Return "Blu-ray playback"
                Case PlayerState.DvdPlayback : Return "DVD playback"
                Case PlayerState.FilePlayback : Return "File playback"
                Case PlayerState.PhotoViewer : Return "Photo viewer"
                Case PlayerState.Loading : Return "Loading"
                Case PlayerState.SafeMode : Return "Safe mode"
                Case PlayerState.IndexSetup : Return "Index setup"
                Case PlayerState.GenericSetup : Return "Generic setup"
                Case PlayerState.VideoSetup : Return "Video setup"
                Case PlayerState.NetworkSetup : Return "Network setup"
                Case PlayerState.TorrentSetup : Return "Torrent setup"
                Case PlayerState.TorrentDownloads : Return "Torrent downloads"
                Case PlayerState.SystemStorageSetup : Return "System storage setup"
                Case PlayerState.FirmwareUpgradeSetup : Return "Firmware upgrade setup"
                Case PlayerState.SystemInformation : Return "System information"
                Case Else
                    Return DirectCast(value, PlayerState).Value
            End Select
        End If
        Return MyBase.ConvertTo(value, destinationType)
    End Function



End Class

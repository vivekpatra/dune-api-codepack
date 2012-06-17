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
Imports System.Runtime.Serialization

Namespace DuneUtilities.ApiWrappers

    <Serializable()>
    Public Class Folder
        Inherits IO.FileSystemInfo

        ' TODO: implement dune_folder.txt functionallity
        ' http://dune-hd.com/firmware/misc/dune_folder_howto.txt

        Public Sub New()
            ' default constructor
        End Sub

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Overrides Sub Delete()
            Throw New NotImplementedException
        End Sub

        Public Overrides ReadOnly Property Exists As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Throw New NotImplementedException
            End Get
        End Property
    End Class

End Namespace
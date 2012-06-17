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

    ''' <summary>
    ''' The exception that is created when a command fails.
    ''' </summary>
    <Serializable()>
    Public Class CommandException
        Inherits Exception
        Implements ISerializable

        Private _errorKind As String

        ''' <summary>
        ''' Gets the error kind.
        ''' </summary>
        Public ReadOnly Property ErrorKind As String
            Get
                Return _errorKind
            End Get
        End Property

        Private Sub New()
            MyBase.New()
        End Sub

        Public Sub New(errorKind As String, errorDescription As String)
            MyBase.New(errorDescription)
            _errorKind = errorKind
        End Sub

        Public Sub New(errorKind As String, errorDescription As String, inner As Exception)
            MyBase.New(errorDescription, inner)
            _errorKind = errorKind
        End Sub

        ''' <remarks>This constructor is needed for serialization.</remarks>
        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)

            If info IsNot Nothing Then
                _errorKind = info.GetString("_errorKind")
            End If
        End Sub

        Public Overrides Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
            MyBase.GetObjectData(info, context)

            If info IsNot Nothing Then
                info.AddValue("_errorKind", _errorKind)
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return ErrorKind + ": " + Message
        End Function
    End Class

End Namespace
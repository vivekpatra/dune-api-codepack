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

    ''' <summary>
    ''' A helper class for retrieving and comparing command status values and for creating new command status values.
    ''' </summary>
    Public Class CommandStatus : Inherits NameValuePair

        Private Shared ReadOnly _ok As CommandStatus = New CommandStatus("ok")
        Private Shared ReadOnly _failed As CommandStatus = New CommandStatus("failed")
        Private Shared ReadOnly _timeout As CommandStatus = New CommandStatus("timeout")

        Public Sub New(status As String)
            MyBase.New(status)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Output.CommandStatus.Name
            End Get
        End Property

        Public Shared ReadOnly Property OK As CommandStatus
            Get
                Return CommandStatus._ok
            End Get
        End Property

        Public Shared ReadOnly Property Failed As CommandStatus
            Get
                Return CommandStatus._failed
            End Get
        End Property

        Public Shared ReadOnly Property Timeout As CommandStatus
            Get
                Return CommandStatus._timeout
            End Get
        End Property

    End Class

End Namespace
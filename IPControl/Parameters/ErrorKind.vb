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
    ''' A helper class for retrieving and comparing error kinds and for creating new error kinds.
    ''' </summary>
    Public Class ErrorKind : Inherits NameValuePair

        Private Shared ReadOnly _illegalState As ErrorKind = New ErrorKind("illegal_state")
        Private Shared ReadOnly _operationFailed As ErrorKind = New ErrorKind("operation_failed")
        Private Shared ReadOnly _unknownCommand As ErrorKind = New ErrorKind("unknown_command")
        Private Shared ReadOnly _invalidParameters As ErrorKind = New ErrorKind("invalid_parameters")
        Private Shared ReadOnly _internalError As ErrorKind = New ErrorKind("internal_error")

        Public Sub New(kind As String)
            MyBase.New(kind)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Output.ErrorKind.Name
            End Get
        End Property

        Public Shared ReadOnly Property IllegalState As ErrorKind
            Get
                Return ErrorKind._illegalState
            End Get
        End Property

        Public Shared ReadOnly Property OperationFailed As ErrorKind
            Get
                Return ErrorKind._operationFailed
            End Get
        End Property

        Public Shared ReadOnly Property UnknownCommand As ErrorKind
            Get
                Return ErrorKind._unknownCommand
            End Get
        End Property

        Public Shared ReadOnly Property InvalidParameters As ErrorKind
            Get
                Return ErrorKind._invalidParameters
            End Get
        End Property

        Public Shared ReadOnly Property InternalError As ErrorKind
            Get
                Return ErrorKind._internalError
            End Get
        End Property

    End Class

End Namespace
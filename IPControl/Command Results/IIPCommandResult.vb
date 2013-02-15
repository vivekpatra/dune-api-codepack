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

    Public Interface IIPCommandResult
        Property ProtocolVersion As Version
        Property CommandStatus As CommandStatus
        Property ErrorKind As ErrorKind
        Property ErrorDescription As String
        Property PlayerState As PlayerState
        ReadOnly Property IsSuccessStatusCode As Boolean
        Function EnsureSuccessStatusCode() As IIPCommandResult
        Function GetKeyValuePairs() As IDictionary(Of Parameter, String)

        ReadOnly Property Request As IIPCommand
        ReadOnly Property RequestDateTime As DateTimeOffset
        ReadOnly Property ResponsedateTime As DateTimeOffset
    End Interface

End Namespace


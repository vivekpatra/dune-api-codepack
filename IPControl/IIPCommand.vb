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

    Public Interface IIPCommand
        ReadOnly Property RequiredVersion As Version
        Property Command As CommandValue
        Function CanExecute(target As Dune) As Boolean
        Function Execute(target As Dune) As IIPCommandResult
        Function ExecuteAsync(target As Dune) As Task(Of IIPCommandResult)
        Function TryExecute(target As Dune) As IIPCommandResult
        Function TryExecuteAsync(target As Dune) As Task(Of IIPCommandResult)
        Function GetRequestMessage(target As Dune) As HttpRequestMessage
        Function GetRequestMessage(target As Dune, method As HttpMethod, requestUri As Uri) As HttpRequestMessage
        Function GetQuery(target As Dune) As IDictionary(Of String, String)
        Property Timeout As TimeSpan
    End Interface

End Namespace


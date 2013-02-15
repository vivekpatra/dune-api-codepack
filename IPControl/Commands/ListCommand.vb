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

    Public Class ListCommand : Inherits Command

        Private Shared _requiredVersion As Version = New Version(1, 0)

        Public Sub New(path As String)
            MyBase.New(CommandValue.List)
            Me.Path = path
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Property Path As String
            Get
                Return Me.parameters.Item(Input.FolderUrl)
            End Get
            Set(value As String)
                Me.parameters.Item(Input.FolderUrl) = UnixPath.Normalize(value)
            End Set
        End Property

        Public Overrides ReadOnly Property RequiredVersion As Version
            Get
                Return _requiredVersion
            End Get
        End Property

        Public Overrides Function GetRequestMessage(target As Dune) As Net.Http.HttpRequestMessage
            Dim builder = target.GetBaseAddress("remote-control", "do")
            builder.Query = Me.GetQuery(target).ToString
            Return MyBase.GetRequestMessage(target, HttpMethod.Get, builder.Uri)
        End Function

        Public Overrides Function CanExecute(target As Dune) As Boolean
            If MyBase.CanExecute(target) Then
                Return target.HasRemoteControlPlugin
            End If
            Return False
        End Function

        Public Overrides Function GetIllegalStateReason(target As Dune) As String
            Return "remote-control plugin is unavailable"
        End Function

        Protected Overrides Function InitializeCommandResult(input As XDocument, requestDateTime As DateTimeOffset) As IIPCommandResult
            Return New FileSystemCommandResult(Me, input, requestDateTime)
        End Function

    End Class

End Namespace
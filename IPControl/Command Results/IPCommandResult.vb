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

    Public MustInherit Class IPCommandResult : Implements IIPCommandResult

        Private _request As IIPCommand
        Private _requestDateTime As DateTimeOffset
        Private _responseDateTime As DateTimeOffset

        Private _protocolVersion As Version
        Private _commandStatus As CommandStatus
        Private _errorKind As ErrorKind
        Private _errorDescription As String
        Private _playerState As PlayerState
        Private _actions As Dictionary(Of Parameter, Action(Of String))
        Private _input As XDocument

        Protected Sub New(query As IIPCommand, output As XDocument, requestDateTime As DateTimeOffset)
            _responseDateTime = DateTimeOffset.UtcNow
            _requestDateTime = requestDateTime
            _request = query
            IPCommandResult.PrepareInput(output)
            _input = output
            Me.Initialize()
        End Sub

        Protected ReadOnly Property Input As XDocument
            Get
                Return _input
            End Get
        End Property

        Protected Overridable Sub Initialize()
            Me.MapActions()
            Dim parameters As New Dictionary(Of Parameter, String)(Me.GetKeyValuePairs)
            For Each parameter In parameters
                If Not Actions.ContainsKey(parameter.Key) Then
                    Continue For
                End If
                Actions.Item(parameter.Key).Invoke(parameter.Value)
            Next
        End Sub

        Protected Overridable Sub MapActions()
            _actions = New Dictionary(Of Parameter, Action(Of String))
            With Me.Actions
                .Add(Output.ProtocolVersion, AddressOf SetProtocolVersion)
                .Add(Output.CommandStatus, AddressOf SetCommandStatus)
                .Add(Output.ErrorKind, AddressOf SetErrorKind)
                .Add(Output.ErrorDescription, AddressOf SetErrorDescription)
                .Add(Output.PlayerState, AddressOf SetPlayerState)
            End With
        End Sub

        Private Shared Sub PrepareInput(source As XDocument)
            For Each descendant As XElement In source.Descendants.Elements.ToList
                If descendant.HasAttributes Then
                    If descendant.Attribute("value").Value = "-1" Then
                        descendant.Remove()
                        Continue For
                    End If

                    Dim parameter = New Output(descendant.Attribute("name").Value)
                    descendant.Attribute("name").Remove()
                    Select Case parameter
                        Case Output.VideoFullscreen : descendant.Name = Output.PlaybackWindowFullscreen.Name
                        Case Output.VideoX : descendant.Name = Output.PlaybackWindowRectangleX.Name
                        Case Output.VideoY : descendant.Name = Output.PlaybackWindowRectangleY.Name
                        Case Output.VideoWidth : descendant.Name = Output.PlaybackWindowRectangleWidth.Name
                        Case Output.VideoHeight : descendant.Name = Output.PlaybackWindowRectangleHeight.Name
                        Case Output.VideoTotalDisplayWidth : descendant.Name = Output.OnScreenDisplayWidth.Name
                        Case Output.VideoTotalDisplayHeight : descendant.Name = Output.OnScreenDisplayHeight.Name
                        Case Else : descendant.Name = parameter.Name
                    End Select

                    descendant.Value = descendant.Attribute("value").Value
                    descendant.Attribute("value").Remove()
                End If
            Next
        End Sub


        Protected ReadOnly Property Actions As Dictionary(Of Parameter, Action(Of String))
            Get
                Return _actions
            End Get
        End Property

        ''' <summary>
        ''' Gets the protocol version.
        ''' </summary>
        Public Property ProtocolVersion As Version Implements IIPCommandResult.ProtocolVersion
            Get
                Return _protocolVersion
            End Get
            Protected Set(value As Version)
                _protocolVersion = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the command status.
        ''' </summary>
        Public Property CommandStatus As CommandStatus Implements IIPCommandResult.CommandStatus
            Get
                Return _commandStatus
            End Get
            Protected Set(value As CommandStatus)
                _commandStatus = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the error kind.
        ''' </summary>
        Public Property ErrorKind As ErrorKind Implements IIPCommandResult.ErrorKind
            Get
                Return _errorKind
            End Get
            Protected Set(value As ErrorKind)
                _errorKind = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the error description.
        ''' </summary>
        Public Property ErrorDescription As String Implements IIPCommandResult.ErrorDescription
            Get
                Return _errorDescription
            End Get
            Protected Set(value As String)
                _errorDescription = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the player state.
        ''' </summary>
        Public Property PlayerState As PlayerState Implements IIPCommandResult.PlayerState
            Get
                Return _playerState
            End Get
            Protected Set(value As PlayerState)
                _playerState = value
            End Set
        End Property

        Private Sub SetProtocolVersion(value As String)
            Me.ProtocolVersion = ParseVersion(value)
        End Sub

        Private Shared Function ParseVersion(value As String) As Version
            If Not value.Contains("."c) Then
                value += ".0"
            End If
            Return Version.Parse(value)
        End Function

        Private Sub SetCommandStatus(value As String)
            Me.CommandStatus = New CommandStatus(value)
        End Sub

        Private Sub SetErrorKind(value As String)
            Me.ErrorKind = New ErrorKind(value)
        End Sub

        Private Sub SetErrorDescription(value As String)
            Me.ErrorDescription = value
        End Sub

        Private Sub SetPlayerState(value As String)
            Me.PlayerState = New PlayerState(value)
        End Sub

        ''' <summary>
        ''' Gets whether the command status (if any) indicates an error.
        ''' </summary>
        Public ReadOnly Property IsSuccessStatusCode As Boolean Implements IIPCommandResult.IsSuccessStatusCode
            Get
                Return Not (Me.CommandStatus = IPControl.CommandStatus.Failed Or Me.CommandStatus = IPControl.CommandStatus.Timeout)
            End Get
        End Property

        ''' <summary>
        ''' Throws an exception if the <see cref="IsSuccessStatusCode" /> property indicates that an error occurred; otherwise returns the current instance.
        ''' </summary>
        Public Function EnsureSuccessStatusCode() As IIPCommandResult Implements IIPCommandResult.EnsureSuccessStatusCode
            Select Case Me.CommandStatus
                Case IPControl.CommandStatus.Failed
                    Throw New CommandException(Me.ErrorKind, Me.ErrorDescription)
                Case IPControl.CommandStatus.Timeout
                    Throw New TimeoutException("Timeout was reached and the server closed the connection.")
                Case Else
                    Return Me
            End Select
        End Function

        ''' <summary>
        ''' Compares the current instance to the specified instance.
        ''' </summary>
        ''' <returns>Returns an <see cref="IEnumerable(Of String)" /> containing the names of properties where the property value differs from the specified instance.</returns>
        Public Overridable Function GetDifferences(values As StatusCommandResult) As IEnumerable(Of String)
            Dim differences As New List(Of String)

            If Object.ReferenceEquals(Me, values) Then
                Return differences
            End If

            If Not Object.Equals(values.ProtocolVersion, Me.ProtocolVersion) Then
                differences.Add(GetPropertyName(Function() Me.ProtocolVersion))
            End If

            If Not Object.Equals(values.PlayerState, Me.PlayerState) Then
                differences.Add(GetPropertyName(Function() Me.PlayerState))
            End If

            Return differences
        End Function

        Protected Function GetPropertyName(Of TValue)(propertyId As Expressions.Expression(Of Func(Of TValue))) As String
            Return DirectCast(propertyId.Body, Expressions.MemberExpression).Member.Name
        End Function

        Public Function GetKeyValuePairs() As IDictionary(Of Parameter, String) Implements IIPCommandResult.GetKeyValuePairs
            Dim pairs As New Dictionary(Of Parameter, String)
            For Each parameter In Me.Input.Descendants("command_result").Elements
                If parameter.HasElements Then
                    Continue For
                End If
                pairs.Add(New Output(parameter.Name.ToString), parameter.Value)
            Next
            Return pairs
        End Function

        Protected ReadOnly Property Request As IIPCommand Implements IIPCommandResult.Request
            Get
                Return _request
            End Get
        End Property

        Protected ReadOnly Property RequestDateTime As DateTimeOffset Implements IIPCommandResult.RequestDateTime
            Get
                Return _requestDateTime
            End Get
        End Property

        Protected ReadOnly Property ResponsedateTime As DateTimeOffset Implements IIPCommandResult.ResponsedateTime
            Get
                Return _responseDateTime
            End Get
        End Property
    End Class

End Namespace


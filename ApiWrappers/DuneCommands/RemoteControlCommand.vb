Imports System.Text
Imports System.Reflection
Imports System.Collections.Specialized

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to emulate a button press on the remote control.</summary>
    Public Class RemoteControlCommand
        Inherits DuneCommand

        Private _remoteType As IRemoteControl.RemoteType
        Private _button As IRemoteControl.Button
        Private _code As String

        ''' <param name="dune">The target device.</param>
        ''' <param name="button">The button to send.</param>
        Public Sub New(ByRef dune As Dune, ByVal button As IRemoteControl.Button)
            MyBase.New(dune)
            _button = button
        End Sub

        ''' <summary>
        ''' Gets the hexadecimal code for the current button.
        ''' </summary>
        Public ReadOnly Property HexCode As String
            Get
                If String.IsNullOrEmpty(_code) Then
                    Dim remoteBytes() As Byte = BitConverter.GetBytes(RemoteType)
                    Dim buttonBytes() As Byte = BitConverter.GetBytes(Button)

                    _code = (BitConverter.ToString(buttonBytes, 0, 2) + BitConverter.ToString(remoteBytes, 0, 2)).Replace("-", String.Empty)
                End If
                Return _code
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the remote type. The default of "New" should be fine in all cases.
        ''' </summary>
        Public Property RemoteType As IRemoteControl.RemoteType
            Get
                If _remoteType = Nothing Then
                    _remoteType = IRemoteControl.RemoteType.New
                End If
                Return _remoteType
            End Get
            Set(value As IRemoteControl.RemoteType)
                _remoteType = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the current button.
        ''' </summary>
        Public ReadOnly Property Button As IRemoteControl.Button
            Get
                Return _button
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.InfraredCode)
            query.Add(Constants.Commands.InfraredCode, HexCode)

            Return query
        End Function
    End Class

End Namespace
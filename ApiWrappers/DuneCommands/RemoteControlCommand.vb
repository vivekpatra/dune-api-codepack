Imports System.Text
Imports System.Reflection

Namespace Dune.ApiWrappers

    ''' <summary>This command is used to emulate a button press on the remote control.</summary>
    Public Class RemoteControlCommand
        Inherits DuneCommand

        Private _button As IRemoteControl.Button
        Private _code As String

        ''' <param name="dune">The target device.</param>
        ''' <param name="button">The button to send.</param>
        Public Sub New(ByRef dune As Dune, ByVal button As IRemoteControl.Button)
            MyBase.New(dune)
            CommandType = Constants.Commands.InfraredCode
            _button = button
        End Sub

        ''' <summary>
        ''' Gets the hexadecimal code for the current button.
        ''' </summary>
        Public ReadOnly Property HexCode As String
            Get
                If String.IsNullOrEmpty(_code) Then
                    _code = String.Concat(GetCodeFromButton(Button), "BF00")
                End If
                Return _code
            End Get
        End Property

        ''' <summary>
        ''' Gets the current button.
        ''' </summary>
        Public ReadOnly Property Button As IRemoteControl.Button
            Get
                Return _button
            End Get
        End Property

        ''' <summary>
        ''' Method that uses reflection to get the <see cref="InfraredCodeAttribute"/> value for the supplied button.
        ''' </summary>
        ''' <param name="button">The button to get the code for.</param>
        ''' <returns>A hexadecimal code.</returns>
        Public Shared Function GetCodeFromButton(ByVal button As IRemoteControl.Button) As String
            Dim memberInfo As MemberInfo() = button.GetType().GetMember(button.ToString())
            If memberInfo IsNot Nothing AndAlso memberInfo.Length > 0 Then
                Dim attribute As InfraredCodeAttribute = TryCast(System.Attribute.GetCustomAttribute(memberInfo(0), GetType(InfraredCodeAttribute)), InfraredCodeAttribute)
                If attribute IsNot Nothing Then
                    Return attribute.Code
                End If
            End If
            Return Nothing
        End Function

        Public Overrides Function GetQueryString() As String
            Dim query As New StringBuilder

            query.Append("cmd=")
            query.Append(CommandType)

            query.Append("&ir_code=")
            query.Append(HexCode)

            Return query.ToString
        End Function
    End Class

End Namespace
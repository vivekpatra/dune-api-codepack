Imports System.Text
Imports System.Reflection
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command is used to emulate a button press on the remote control.</summary>
    Public Class RemoteControlCommand
        Inherits Command

        Private _button As UShort
        Private _code As String

        ''' <param name="dune">The target device.</param>
        ''' <param name="button">The button to send.</param>
        Public Sub New(ByRef dune As Dune, ByVal button As UShort)
            MyBase.New(dune)
            _button = button
        End Sub

        ''' <summary>
        ''' Gets the hexadecimal code for the specified button.
        ''' </summary>
        Public ReadOnly Property HexCode As String
            Get
                If String.IsNullOrEmpty(_code) Then
                    _code = Constants.RemoteControls.GetButtonCode(_button)
                End If
                Return _code
            End Get
        End Property

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.InfraredCode)
            query.Add(Constants.InfraredCodeParameters.InfraredCode, HexCode)

            Return query
        End Function
    End Class

End Namespace
Namespace Dune.ApiWrappers

    ''' <summary>This class represents a standard remote control, much like the physical remote that comes with the box.</summary>
    Public Class RemoteControl
        Implements IRemoteControl

        Private _dune As Dune
        Private _lastButton As IRemoteControl.Button

        ''' <param name="dune">The target device.</param>
        Public Sub New(ByRef dune As Dune)
            _dune = dune
        End Sub

        ''' <summary>
        ''' Emulates a button on the remote control.
        ''' </summary>
        ''' <param name="button">The button to emulate</param>
        ''' <returns>New instance of the <seealso cref="CommandResult"/> class.</returns>
        Public Function Push(button As IRemoteControl.Button) As CommandResult Implements IRemoteControl.Push
            If button = IRemoteControl.Button.None Then
                Return Nothing
            End If
            _lastButton = button
            Dim command As New RemoteControlCommand(_dune, button)
            Dim result As CommandResult = CommandResult.FromCommand(command)
            _dune.UpdateValues(result)
            Return result
        End Function

        Public Overrides Function ToString() As String
            Dim text As String = "Last button push: " + _lastButton.ToString
            Return text
        End Function
    End Class

End Namespace
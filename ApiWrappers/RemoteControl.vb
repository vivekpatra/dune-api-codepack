Namespace Dune.ApiWrappers

    ''' <summary>This class represents a standard remote control, much like the physical remote that comes with the box.</summary>
    Public Class RemoteControl
        Implements IRemoteControl

        Private _dune As Dune

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
            Dim command As New RemoteControlCommand(_dune, button)
            Return New CommandResult(command)
        End Function
    End Class

End Namespace
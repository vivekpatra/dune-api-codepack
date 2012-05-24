Imports System.ComponentModel

Namespace DuneUtilities.ApiWrappers

    ''' <summary>
    ''' Interface that defines buttons on the remote control.
    ''' </summary>
    Public Interface IRemoteControl

        Function Push(ByVal button As UShort) As CommandResult

    End Interface

End Namespace
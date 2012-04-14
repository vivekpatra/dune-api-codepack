Namespace Dune.ApiWrappers

    ''' <summary>Provides a text attribute to set hex codes on buttons in the <see cref="IRemoteControl.Button"/> enumeration.</summary>
    Public Class InfraredCodeAttribute
        Inherits Attribute

        Public Property Code As String

        Public Sub New(ByVal code As String)
            Me.Code = code
        End Sub

    End Class

End Namespace
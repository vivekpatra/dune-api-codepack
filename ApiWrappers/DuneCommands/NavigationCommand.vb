Imports SL.DuneApiCodePack.Networking

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command can be used to navigate menus.</summary>
    ''' <remarks>This command is context sensitive, meaning:
    ''' If the target device is playing a DVD, it will produce a dvd_navigation command.
    ''' Likewise, it will produce a bluray_navigation command if a Blu-ray is playing.
    ''' In all other cases, a remote control button will be emulated.</remarks>
    Public Class NavigationCommand
        Inherits Command

        Private _action As String

        Public Sub New(target As Dune, action As String)
            MyBase.New(target)
            _action = action
        End Sub

        ''' <summary>
        ''' Gets the action that needs to be performed.
        ''' </summary>
        Public ReadOnly Property Action As String
            Get
                Return _action
            End Get
        End Property

        Protected Overrides Function GetQuery() As HttpQuery
            Dim query As New HttpQuery
            Dim command As String = Nothing
            Dim parameter As String = Nothing
            Dim value As String = Nothing

            ' determine which command to send and set the appropriate parameter name.
            Select Case Target.PlayerState
                Case Constants.PlayerStateValues.DvdPlayback
                    command = Constants.CommandValues.DvdNavigation
                    parameter = Constants.NavigationParameterNames.Action
                Case Constants.PlayerStateValues.BlurayPlayback
                    command = Constants.CommandValues.BlurayNavigation
                    parameter = Constants.NavigationParameterNames.Action
                Case Else
                    command = Constants.CommandValues.InfraredCode
                    parameter = Constants.InfraredCodeParameterNames.InfraredCode
            End Select

            ' set the parameter value
            Select Case Action.ToLower
                Case Constants.ActionValues.Left
                    If command = Constants.CommandValues.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Constants.RemoteControls.BigRemote2ButtonValues.Left)
                    Else
                        value = Constants.ActionValues.Left
                    End If
                Case Constants.ActionValues.Right
                    If command = Constants.CommandValues.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Constants.RemoteControls.BigRemote2ButtonValues.Right)
                    Else
                        value = Constants.ActionValues.Right
                    End If
                Case Constants.ActionValues.Up
                    If command = Constants.CommandValues.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Constants.RemoteControls.BigRemote2ButtonValues.Up)
                    Else
                        value = Constants.ActionValues.Up
                    End If
                Case Constants.ActionValues.Down
                    If command = Constants.CommandValues.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Constants.RemoteControls.BigRemote2ButtonValues.Down)
                    Else
                        value = Constants.ActionValues.Down
                    End If
                Case Constants.ActionValues.Enter
                    If command = Constants.CommandValues.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Constants.RemoteControls.BigRemote2ButtonValues.Down)
                    Else
                        value = Constants.ActionValues.Down
                    End If
            End Select

            query.Add("cmd", command)
            query.Add(parameter, value)


            Return query
        End Function
    End Class

End Namespace
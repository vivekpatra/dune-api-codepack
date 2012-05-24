Imports System.Text
Imports System.Collections.Specialized

Namespace DuneUtilities.ApiWrappers

    ''' <summary>This command can be used to navigate menus.</summary>
    ''' <remarks>This command is context sensitive, meaning:
    ''' If the target device is playing a DVD, it will produce a dvd_navigation command.
    ''' Likewise, it will produce a bluray_navigation command if a Blu-ray is playing.
    ''' In all other cases, a remote control button will be emulated.</remarks>
    Public Class NavigationCommand
        Inherits Command

        Private _action As String

        Public Sub New(ByRef dune As Dune, ByVal action As String)
            MyBase.New(dune)
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

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection
            Dim command As String = Nothing
            Dim parameter As String = Nothing
            Dim value As String = Nothing

            ' determine which command to send and set the appropriate parameter name.
            Select Case Target.PlayerState
                Case Constants.PlayerStateSettings.DvdPlayback
                    command = Constants.Commands.DvdNavigation
                    parameter = Constants.NavigationParameters.Action
                Case Constants.PlayerStateSettings.BlurayPlayback
                    command = Constants.Commands.BlurayNavigation
                    parameter = Constants.NavigationParameters.Action
                Case Else
                    command = Constants.Commands.InfraredCode
                    parameter = Constants.InfraredCodeParameters.InfraredCode
            End Select

            ' set the parameter value
            Select Case Action.ToLower
                Case Constants.NavigationActions.Left
                    If command = Constants.Commands.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Convert.ToUInt16(Constants.RemoteControls.BigRemote2Buttons.Left))
                    Else
                        value = Constants.NavigationActions.Left
                    End If
                Case Constants.NavigationActions.Right
                    If command = Constants.Commands.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Convert.ToUInt16(Constants.RemoteControls.BigRemote2Buttons.Right))
                    Else
                        value = Constants.NavigationActions.Right
                    End If
                Case Constants.NavigationActions.Up
                    If command = Constants.Commands.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Convert.ToUInt16(Constants.RemoteControls.BigRemote2Buttons.Up))
                    Else
                        value = Constants.NavigationActions.Up
                    End If
                Case Constants.NavigationActions.Down
                    If command = Constants.Commands.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Convert.ToUInt16(Constants.RemoteControls.BigRemote2Buttons.Down))
                    Else
                        value = Constants.NavigationActions.Down
                    End If
                Case Constants.NavigationActions.Enter
                    If command = Constants.Commands.InfraredCode Then
                        value = Constants.RemoteControls.GetButtonCode(Convert.ToUInt16(Constants.RemoteControls.BigRemote2Buttons.Down))
                    Else
                        value = Constants.NavigationActions.Down
                    End If
            End Select

            query.Add("cmd", command)
            query.Add(parameter, value)


            Return query
        End Function
    End Class

End Namespace
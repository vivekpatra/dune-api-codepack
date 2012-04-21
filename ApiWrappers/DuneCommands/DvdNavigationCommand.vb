Imports System.Text

Namespace Dune.ApiWrappers

    ''' <summary>This command can be used to navigate dvd menus.</summary>
    ''' <remarks>Test the <see cref="Dune.PlaybackDvdMenu"/> property for true or false before using.</remarks>
    Public Class DvdNavigationCommand
        Inherits DuneCommand

        Private _action As MenuAction

        Public Sub New(ByRef dune As Dune, ByVal action As MenuAction)
            MyBase.New(dune)
            CommandType = Constants.Commands.DvdNavigation
            _action = action
        End Sub

        ''' <summary>
        ''' Gets the action that needs to be performed.
        ''' </summary>
        Public ReadOnly Property Action As MenuAction
            Get
                Return _action
            End Get
        End Property

        Public Overrides Function GetQueryString() As String
            Dim query As New StringBuilder

            query.Append("cmd=")
            query.Append(CommandType)

            query.Append("&action=")

            Select Case Action
                Case MenuAction.Left
                    query.Append(Constants.DvdNavigationActions.Left)
                Case MenuAction.Right
                    query.Append(Constants.DvdNavigationActions.Right)
                Case MenuAction.Up
                    query.Append(Constants.DvdNavigationActions.Up)
                Case MenuAction.Down
                    query.Append(Constants.DvdNavigationActions.Down)
                Case MenuAction.Enter
                    query.Append(Constants.DvdNavigationActions.Enter)
            End Select

            Return query.ToString

        End Function

        ''' <summary>
        ''' Enumeration of supported actions.
        ''' </summary>
        Public Enum MenuAction
            Left = 0
            Right = 1
            Up = 2
            Down = 3
            Enter = 4
        End Enum
    End Class

End Namespace
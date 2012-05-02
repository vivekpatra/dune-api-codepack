Imports System.Text
Imports System.Collections.Specialized

Namespace Dune.ApiWrappers

    ''' <summary>This command can be used to navigate dvd menus.</summary>
    ''' <remarks>Test the <see cref="Dune.PlaybackDvdMenu"/> property for true or false before using.</remarks>
    Public Class DvdNavigationCommand
        Inherits DuneCommand

        Private _action As MenuAction

        Public Sub New(ByRef dune As Dune, ByVal action As MenuAction)
            MyBase.New(dune)
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

        Protected Overrides Function GetQuery() As NameValueCollection
            Dim query As New NameValueCollection

            query.Add("cmd", Constants.Commands.DvdNavigation)

            Select Case Action
                Case MenuAction.Left
                    query.Add("action", Constants.DvdNavigationActions.Left)
                Case MenuAction.Right
                    query.Add("action", Constants.DvdNavigationActions.Right)
                Case MenuAction.Up
                    query.Add("action", Constants.DvdNavigationActions.Up)
                Case MenuAction.Down
                    query.Add("action", Constants.DvdNavigationActions.Down)
                Case MenuAction.Enter
                    query.Add("action", Constants.DvdNavigationActions.Enter)
            End Select

            Return query
        End Function
    End Class

End Namespace
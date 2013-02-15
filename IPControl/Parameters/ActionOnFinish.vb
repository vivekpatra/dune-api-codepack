﻿#Region "License"
' Copyright 2012-2013 Steven Liekens
' Contact: steven.liekens@gmail.com

' This file is part of DuneApiCodepack.

' DuneApiCodepack is free software: you can redistribute it and/or modify
' it under the terms of the GNU Lesser General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' DuneApiCodepack is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Lesser General Public License for more details.

' You should have received a copy of the GNU Lesser General Public License
' along with DuneApiCodepack.  If not, see <http://www.gnu.org/licenses/>.
#End Region ' License

Namespace IPControl

    ''' <summary>
    ''' A helper class for retrieving and comparing actions on finish and for creating new actions on finish.
    ''' </summary>
    Public Class ActionOnFinish : Inherits NameValuePair

        Private Shared ReadOnly _exit As ActionOnFinish = New ActionOnFinish("exit")
        Private Shared ReadOnly _restartPlayback As ActionOnFinish = New ActionOnFinish("restart_playback")

        Public Sub New(action As String)
            MyBase.New(action)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.ActionOnFinish.Name
            End Get
        End Property

        ''' <summary>
        ''' Represents an 'exit' action.
        ''' </summary>
        Public Shared ReadOnly Property [Exit] As ActionOnFinish
            Get
                Return ActionOnFinish._exit
            End Get
        End Property

        ''' <summary>
        ''' Represets a 'restart playback' action.
        ''' </summary>
        Public Shared ReadOnly Property RestartPlayback As ActionOnFinish
            Get
                Return ActionOnFinish._restartPlayback
            End Get
        End Property

    End Class

End Namespace
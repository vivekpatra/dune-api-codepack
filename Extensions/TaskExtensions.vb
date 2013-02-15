#Region "License"
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
Imports System.Runtime.CompilerServices

Namespace Extensions

    ''' <summary>
    ''' Extensions for the <see cref="System.Threading.Tasks.Task"/> type.
    ''' </summary>
    Public Module TaskExtensions

        <Extension()>
        Public Async Function WithCancellation(value As Task, cancellationToken As Threading.CancellationToken) As Task
            Dim tcs = New TaskCompletionSource(Of Boolean)
            Using cancellationToken.Register(Sub() tcs.TrySetResult(True))
                If value IsNot Await Task.WhenAny(value, tcs.Task).ConfigureAwait(False) Then
                    cancellationToken.ThrowIfCancellationRequested()
                End If
            End Using
        End Function

        <Extension()>
        Public Async Function WithCancellation(Of T)(value As Task(Of T), cancellationToken As Threading.CancellationToken) As Task(Of T)
            Dim tcs = New TaskCompletionSource(Of Boolean)
            Using cancellationToken.Register(Sub() tcs.TrySetResult(True))
                If value IsNot Await Task.WhenAny(value, tcs.Task).ConfigureAwait(False) Then
                    cancellationToken.ThrowIfCancellationRequested()
                End If
            End Using
            Return Await value.ConfigureAwait(False)
        End Function

    End Module

End Namespace

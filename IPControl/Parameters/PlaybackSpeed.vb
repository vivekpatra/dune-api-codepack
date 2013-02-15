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

Namespace IPControl

    ''' <summary>
    ''' A helper class for retrieving and comparing speed values and for creating new speed values.
    ''' </summary>
    Public Class PlaybackSpeed : Inherits NameValuePair

        Private Shared ReadOnly _rewind8192 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind8192)
        Private Shared ReadOnly _rewind4096 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind4096)
        Private Shared ReadOnly _rewind2048 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind2048)
        Private Shared ReadOnly _rewind1024 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind1024)
        Private Shared ReadOnly _rewind512 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind512)
        Private Shared ReadOnly _rewind256 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind256)
        Private Shared ReadOnly _rewind128 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind128)
        Private Shared ReadOnly _rewind64 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind64)
        Private Shared ReadOnly _rewind32 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind32)
        Private Shared ReadOnly _rewind16 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind16)
        Private Shared ReadOnly _rewind8 As PlaybackSpeed = New PlaybackSpeed(Speed.Rewind8)
        Private Shared ReadOnly _pause As PlaybackSpeed = New PlaybackSpeed(Speed.Pause)
        Private Shared ReadOnly _play8 As PlaybackSpeed = New PlaybackSpeed(Speed.Play8)
        Private Shared ReadOnly _play16 As PlaybackSpeed = New PlaybackSpeed(Speed.Play16)
        Private Shared ReadOnly _play32 As PlaybackSpeed = New PlaybackSpeed(Speed.Play32)
        Private Shared ReadOnly _play64 As PlaybackSpeed = New PlaybackSpeed(Speed.Play64)
        Private Shared ReadOnly _play128 As PlaybackSpeed = New PlaybackSpeed(Speed.Play128)
        Private Shared ReadOnly _play256 As PlaybackSpeed = New PlaybackSpeed(Speed.Play256)
        Private Shared ReadOnly _play512 As PlaybackSpeed = New PlaybackSpeed(Speed.Play512)
        Private Shared ReadOnly _play1024 As PlaybackSpeed = New PlaybackSpeed(Speed.Play1024)
        Private Shared ReadOnly _play2048 As PlaybackSpeed = New PlaybackSpeed(Speed.Play2048)
        Private Shared ReadOnly _play4096 As PlaybackSpeed = New PlaybackSpeed(Speed.Play4096)
        Private Shared ReadOnly _play8192 As PlaybackSpeed = New PlaybackSpeed(Speed.Play8192)

        Public Sub New(speed As Speed)
            MyBase.New(CStr(speed))
        End Sub

        Public Sub New(speed As Integer)
            MyBase.New(CStr(speed))
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return Input.PlaybackSpeed.Name
            End Get
        End Property

        Public Shared ReadOnly Property Rewind8192 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind8192
            End Get
        End Property

        Public Shared ReadOnly Property Rewind4096 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind4096
            End Get
        End Property

        Public Shared ReadOnly Property Rewind2048 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind2048
            End Get
        End Property

        Public Shared ReadOnly Property Rewind1024 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind1024
            End Get
        End Property

        Public Shared ReadOnly Property Rewind512 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind512
            End Get
        End Property

        Public Shared ReadOnly Property Rewind256 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind256
            End Get
        End Property

        Public Shared ReadOnly Property Rewind128 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind128
            End Get
        End Property

        Public Shared ReadOnly Property Rewind64 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind64
            End Get
        End Property

        Public Shared ReadOnly Property Rewind32 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind32
            End Get
        End Property

        Public Shared ReadOnly Property Rewind16 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind16
            End Get
        End Property

        Public Shared ReadOnly Property Rewind8 As PlaybackSpeed
            Get
                Return PlaybackSpeed._rewind8
            End Get
        End Property

        Public Shared ReadOnly Property Pause As PlaybackSpeed
            Get
                Return PlaybackSpeed._pause
            End Get
        End Property

        Public Shared ReadOnly Property Play8 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play8
            End Get
        End Property

        Public Shared ReadOnly Property Play16 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play16
            End Get
        End Property

        Public Shared ReadOnly Property Play32 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play32
            End Get
        End Property

        Public Shared ReadOnly Property Play64 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play64
            End Get
        End Property

        Public Shared ReadOnly Property Play128 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play128
            End Get
        End Property

        Public Shared ReadOnly Property Play256 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play256
            End Get
        End Property

        Public Shared ReadOnly Property Play512 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play512
            End Get
        End Property

        Public Shared ReadOnly Property Play1024 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play1024
            End Get
        End Property

        Public Shared ReadOnly Property Play2048 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play2048
            End Get
        End Property

        Public Shared ReadOnly Property Play4096 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play4096
            End Get
        End Property

        Public Shared ReadOnly Property Play8192 As PlaybackSpeed
            Get
                Return PlaybackSpeed._play8192
            End Get
        End Property

        Public Shared Operator +(left As PlaybackSpeed, right As PlaybackSpeed) As PlaybackSpeed
            If left Is Nothing Then Return +right
            If right Is Nothing Then Return +left
            Return New PlaybackSpeed(Integer.Parse(left.Value) + Integer.Parse(right.Value))
        End Operator

        Public Shared Operator +(value As PlaybackSpeed) As PlaybackSpeed
            Return New PlaybackSpeed(+Integer.Parse(value.Value))
        End Operator

        Public Shared Operator -(left As PlaybackSpeed, right As PlaybackSpeed) As PlaybackSpeed
            If left Is Nothing Then Return -right
            If right Is Nothing Then Return -left
            Return New PlaybackSpeed(Integer.Parse(left.Value) - Integer.Parse(right.Value))
        End Operator

        Public Shared Operator -(value As PlaybackSpeed) As PlaybackSpeed
            Return New PlaybackSpeed(-Integer.Parse(value.Value))
        End Operator

        Public Shared Operator >(left As PlaybackSpeed, right As PlaybackSpeed) As Boolean
            If left Is Nothing Then Return 0 > Integer.Parse(right.Value)
            If right Is Nothing Then Return Integer.Parse(left.Value) > 0
            Return Integer.Parse(left.Value) > Integer.Parse(right.Value)
        End Operator

        Public Shared Operator <(left As PlaybackSpeed, right As PlaybackSpeed) As Boolean
            If left Is Nothing Then Return 0 < Integer.Parse(right.Value)
            If right Is Nothing Then Return Integer.Parse(left.Value) < 0
            Return Integer.Parse(left.Value) < Integer.Parse(right.Value)
        End Operator

        Public Shared Operator *(left As PlaybackSpeed, right As PlaybackSpeed) As PlaybackSpeed
            If left Is Nothing OrElse right Is Nothing Then Return New PlaybackSpeed(0)
            Return New PlaybackSpeed(Integer.Parse(left.Value) * Integer.Parse(right.Value))
        End Operator

        Public Shared Operator /(left As PlaybackSpeed, right As PlaybackSpeed) As PlaybackSpeed
            Return left \ right
        End Operator

        Public Shared Operator \(left As PlaybackSpeed, right As PlaybackSpeed) As PlaybackSpeed
            If left Is Nothing Then Return New PlaybackSpeed(0 \ Integer.Parse(right.Value))
            If right Is Nothing Then Return New PlaybackSpeed(Integer.Parse(left.Value) \ 0)
            Return New PlaybackSpeed(Integer.Parse(left.Value) \ Integer.Parse(right.Value))
        End Operator

        Public Shared Operator <<(left As PlaybackSpeed, amount As Integer) As PlaybackSpeed
            If left Is Nothing Then Return New PlaybackSpeed(0)
            Return New PlaybackSpeed(Integer.Parse(left.Value) << amount)
        End Operator

        Public Shared Operator >>(left As PlaybackSpeed, amount As Integer) As PlaybackSpeed
            If left Is Nothing Then Return New PlaybackSpeed(0)
            Return New PlaybackSpeed(Integer.Parse(left.Value) >> amount)
        End Operator

        Public Shared Widening Operator CType(value As Speed) As PlaybackSpeed
            Return New PlaybackSpeed(value)
        End Operator

        Public Shared Narrowing Operator CType(value As PlaybackSpeed) As Speed
            Return DirectCast([Enum].Parse(GetType(Speed), value.Value), Speed)
        End Operator

        Public Shared Function Parse(value As String) As PlaybackSpeed
            Dim input = Integer.Parse(value)
            Dim bytes = BitConverter.GetBytes(input)

            If BitConverter.IsLittleEndian Then
                If bytes(3) > Byte.MinValue And bytes(3) < Byte.MaxValue Then
                    input = input Or (input << 5)
                End If
            Else
                If bytes(0) > Byte.MinValue And bytes(0) < Byte.MaxValue Then
                    input = input Or (input >> 5)
                End If
            End If

            Return New PlaybackSpeed(input)
        End Function
    End Class


    ''' <summary>
    ''' Specifies the possible playback speed values.
    ''' </summary>
    Public Enum Speed As Integer
        ''' <summary>Rewind 32x</summary>
        Rewind8192 = -1 << 13
        ''' <summary>Rewind 16x</summary>
        Rewind4096 = -1 << 12
        ''' <summary>Rewind 8x</summary>
        Rewind2048 = -1 << 11
        ''' <summary>Rewind 4x</summary>
        Rewind1024 = -1 << 10
        ''' <summary>Rewind 2x</summary>
        Rewind512 = -1 << 9
        ''' <summary>Rewind 1x</summary>
        Rewind256 = -1 << 8
        ''' <summary>Rewind 1/2x</summary>
        Rewind128 = -1 << 7
        ''' <summary>Rewind 1/4x</summary>
        Rewind64 = -1 << 6
        ''' <summary>Rewind 1/8x</summary>
        Rewind32 = -1 << 5
        ''' <summary>Rewind 1/16x</summary>
        Rewind16 = -1 << 4
        ''' <summary>Rewind 1/32x</summary>
        Rewind8 = -1 << 3
        ''' <summary>Pause</summary>
        Pause = 0
        ''' <summary>Slowdown 1/32x</summary>
        Play8 = 1 << 3
        ''' <summary>Slowdown 1/16x</summary>
        Play16 = 1 << 4
        ''' <summary>Slowdown 1/8x</summary>
        Play32 = 1 << 5
        ''' <summary>Slowdown 1/4x</summary>
        Play64 = 1 << 6
        ''' <summary>Slowdown 1/2x</summary>
        Play128 = 1 << 7
        ''' <summary>Normal</summary>
        Play256 = 1 << 8
        ''' <summary>Forward 2x</summary>
        Play512 = 1 << 9
        ''' <summary>Forward 4x</summary>
        Play1024 = 1 << 10
        ''' <summary>Forward 8x</summary>
        Play2048 = 1 << 11
        ''' <summary>Forward 16x</summary>
        Play4096 = 1 << 12
        ''' <summary>Forward 32x</summary>
        Play8192 = 1 << 13
    End Enum

End Namespace
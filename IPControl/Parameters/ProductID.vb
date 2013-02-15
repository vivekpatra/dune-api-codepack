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
    ''' A helper class for retrieving and comparing product IDs and for creating new product IDs.
    ''' </summary>
    Public Class ProductID : Inherits NameValuePair

        Private Shared ReadOnly _pro As ProductID = New ProductID("pro")
        Private Shared ReadOnly _connect As ProductID = New ProductID("connect")
        Private Shared ReadOnly _hdBase3D As ProductID = New ProductID("base3d")
        Private Shared ReadOnly _hdTV303D As ProductID = New ProductID("tv303d")
        Private Shared ReadOnly _hdTV301 As ProductID = New ProductID("hdtv_301")
        Private Shared ReadOnly _hdTV102 As ProductID = New ProductID("tv102")
        Private Shared ReadOnly _hdTV101 As ProductID = New ProductID("hdtv_101")
        Private Shared ReadOnly _hdLite53D As ProductID = New ProductID("hdlite_53d")
        Private Shared ReadOnly _hdDuo As ProductID = New ProductID("hdduo")
        Private Shared ReadOnly _hdMax As ProductID = New ProductID("hdmax")
        Private Shared ReadOnly _hdSmartB1 As ProductID = New ProductID("hdsmart_b1")
        Private Shared ReadOnly _hdSmartD1 As ProductID = New ProductID("hdsmart_d1")
        Private Shared ReadOnly _hdSmartH1 As ProductID = New ProductID("hdsmart_h1")
        Private Shared ReadOnly _hdBase3 As ProductID = New ProductID("hdbase3")
        Private Shared ReadOnly _bdPrime3 As ProductID = New ProductID("bdprime3")
        Private Shared ReadOnly _hdBase2 As ProductID = New ProductID("hdbase2")
        Private Shared ReadOnly _hdBase As ProductID = New ProductID("hdbase_sony")
        Private Shared ReadOnly _hdCenter As ProductID = New ProductID("hdcenter_sony")
        Private Shared ReadOnly _bdPrime As ProductID = New ProductID("bdprime_sony")
        Private Shared ReadOnly _hdMini As ProductID = New ProductID("hdmini")
        Private Shared ReadOnly _hdUltra As ProductID = New ProductID("hdultra")

        Public Sub New(product As String)
            MyBase.New(product)
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return "product_id"
            End Get
        End Property

        Public Shared ReadOnly Property HDPro As ProductID
            Get
                Return ProductID._pro
            End Get
        End Property

        Public Shared ReadOnly Property HDConnect As ProductID
            Get
                Return ProductID._connect
            End Get
        End Property

        Public Shared ReadOnly Property HDBase3D As ProductID
            Get
                Return ProductID._hdBase3D
            End Get
        End Property

        Public Shared ReadOnly Property HDTV303D As ProductID
            Get
                Return ProductID._hdTV303D
            End Get
        End Property

        Public Shared ReadOnly Property HDTV102 As ProductID
            Get
                Return ProductID._hdTV102
            End Get
        End Property

        Public Shared ReadOnly Property HDTV301 As ProductID
            Get
                Return ProductID._hdTV301
            End Get
        End Property

        Public Shared ReadOnly Property HDTV101 As ProductID
            Get
                Return ProductID._hdTV101
            End Get
        End Property

        Public Shared ReadOnly Property HDLite53D As ProductID
            Get
                Return ProductID._hdLite53D
            End Get
        End Property

        Public Shared ReadOnly Property HDDuo As ProductID
            Get
                Return ProductID._hdDuo
            End Get
        End Property

        Public Shared ReadOnly Property HDMax As ProductID
            Get
                Return ProductID._hdMax
            End Get
        End Property

        Public Shared ReadOnly Property HDSmartB1 As ProductID
            Get
                Return ProductID._hdSmartB1
            End Get
        End Property

        Public Shared ReadOnly Property HDSmartD1 As ProductID
            Get
                Return ProductID._hdSmartD1
            End Get
        End Property

        Public Shared ReadOnly Property HDSmartH1 As ProductID
            Get
                Return ProductID._hdSmartH1
            End Get
        End Property

        Public Shared ReadOnly Property HDBase3 As ProductID
            Get
                Return ProductID._hdBase3
            End Get
        End Property

        Public Shared ReadOnly Property BDPrime3 As ProductID
            Get
                Return ProductID._bdPrime3
            End Get
        End Property

        Public Shared ReadOnly Property HDBase2 As ProductID
            Get
                Return ProductID._hdBase2
            End Get
        End Property

        Public Shared ReadOnly Property HDBase As ProductID
            Get
                Return ProductID._hdBase
            End Get
        End Property

        Public Shared ReadOnly Property HDCenter As ProductID
            Get
                Return ProductID._hdCenter
            End Get
        End Property

        Public Shared ReadOnly Property BDPrime As ProductID
            Get
                Return ProductID._bdPrime
            End Get
        End Property

        Public Shared ReadOnly Property HDMini As ProductID
            Get
                Return ProductID._hdMini
            End Get
        End Property

        Public Shared ReadOnly Property HDUltra As ProductID
            Get
                Return ProductID._hdUltra
            End Get
        End Property

    End Class

End Namespace
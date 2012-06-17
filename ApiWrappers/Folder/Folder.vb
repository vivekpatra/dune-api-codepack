Imports System.Runtime.Serialization

Namespace DuneUtilities.ApiWrappers

    <Serializable()>
    Public Class Folder
        Inherits IO.FileSystemInfo

        ' TODO: implement dune_folder.txt functionallity
        ' http://dune-hd.com/firmware/misc/dune_folder_howto.txt

        Public Sub New()
            ' default constructor
        End Sub

        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Overrides Sub Delete()
            Throw New NotImplementedException
        End Sub

        Public Overrides ReadOnly Property Exists As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Throw New NotImplementedException
            End Get
        End Property
    End Class

End Namespace
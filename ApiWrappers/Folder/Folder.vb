Namespace DuneUtilities.ApiWrappers

    Public Class Folder
        Inherits IO.FileSystemInfo

        ' TODO: implement dune_folder.txt functionallity
        ' http://dune-hd.com/firmware/misc/dune_folder_howto.txt


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
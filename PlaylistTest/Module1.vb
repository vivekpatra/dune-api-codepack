Imports DuneAPICodePack.Dune
Imports System.IO

Module Module1

    Sub Main()
        Dim dune As New Dune("DUNE")

        For Each filepath As String In Directory.EnumerateFiles("\\DUNE\DuneHDD_227c0ea3_d586_4186_a1d1_d81432256bc2\Muziek\Various Artists\Life is Music_ 100 onsterfelijke Studio Brussel songs\Disc 1")
            If filepath.ToLower.EndsWith("mp3") Then dune.Playlist.Files.Add(New FileInfo(filepath))
        Next

        dune.Playlist.Repeat = True

        dune.Playlist.Position = 1

        dune.Playlist.Start()

        Do


        Loop

    End Sub

End Module

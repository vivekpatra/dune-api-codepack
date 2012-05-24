Imports SL.DuneApiCodePack.NativeMethods.Networking

Namespace Storage

    ''' <summary>
    ''' A collection of network shares and information about their disk space.
    ''' </summary>
    ''' <remarks>This type depends on a native method and should be replaced when a managed alternative becomes available.</remarks> 
    Public Class NetworkSharess

        Private _shares As List(Of NetworkDriveInfo)

        ''' <param name="host">The target host.</param>
        Public Sub New(ByVal host As String)
            Dim shares As ShareCollection = ShareCollection.GetShares(host)
            _shares = New List(Of NetworkDriveInfo)

            For Each share As Share In shares
                If share.ShareType = ShareType.Disk Then
                    _shares.Add(New NetworkDriveInfo(share.Root.FullName))
                End If
            Next
        End Sub

        ''' <summary>
        ''' Gets the list of available shares.
        ''' </summary>
        Public ReadOnly Property Shares As List(Of NetworkDriveInfo)
            Get
                Return _shares
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim text As New Text.StringBuilder

            For Each drive As NetworkDriveInfo In Shares
                text.AppendLine(drive.Root.Name)
            Next

            Return text.ToString
        End Function
    End Class

End Namespace
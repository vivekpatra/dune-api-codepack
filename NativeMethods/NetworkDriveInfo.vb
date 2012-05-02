Imports System.IO

''' <summary>
''' Wrapper class that exposes information about a network share.
''' </summary>
Public Class NetworkDriveInfo

    Private _root As DirectoryInfo
    Private _shareInfo As Dictionary(Of NativeMethods.ShareInfo, UInteger)

    Public Sub New(ByVal share As String)
        _root = New DirectoryInfo(share)
        _shareInfo = NativeMethods.GetShareInfo(share)
    End Sub


    ''' <summary>
    ''' Gets the root directory of the network share.
    ''' </summary>
    Public ReadOnly Property Root As DirectoryInfo
        Get
            Return _root
        End Get
    End Property

    ''' <summary>
    ''' Gets the bytes per sector.
    ''' </summary>
    Public ReadOnly Property BytesPerSector As UInteger
        Get
            Return _shareInfo(NativeMethods.ShareInfo.BytesPerSector)
        End Get
    End Property

    ''' <summary>
    ''' Gets the number of free clusters.
    ''' </summary>
    Public ReadOnly Property NumberOfFreeClusters As UInteger
        Get
            Return _shareInfo(NativeMethods.ShareInfo.NumberOfFreeClusters)
        End Get
    End Property

    ''' <summary>
    ''' Gets the sectors per cluster.
    ''' </summary>
    Public ReadOnly Property SectorsPerCluster As UInteger
        Get
            Return _shareInfo(NativeMethods.ShareInfo.SectorsPerCluster)
        End Get
    End Property

    ''' <summary>
    ''' Gets the total number of clusters.
    ''' </summary>
    Public ReadOnly Property TotalNumberOfClusters As UInteger
        Get
            Return _shareInfo(NativeMethods.ShareInfo.TotalNumberOfClusters)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return _root.ToString
    End Function

End Class

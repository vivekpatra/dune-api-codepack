Imports System.Net
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.IO
Imports System.Net.NetworkInformation

''' <summary>
''' Exposes wrapper methods for native methods in the Windows API.
''' </summary>
Public NotInheritable Class NativeMethods
    Private Sub New()
    End Sub

    Public NotInheritable Class Networking
        Private Sub New()
        End Sub

#Region "MAC Addresses"

        ''' <summary>
        ''' API function declaration.
        ''' </summary>
        <DllImport("iphlpapi.dll", SetLastError:=True)>
        Private Shared Function SendARP(
         DestIP As UInt32,
         SrcIP As UInt32,
         pMacAddr() As Byte,
         ByRef PhyAddrLen As Int32) As UInt32
        End Function

        ' Return values
        Private Const NO_ERROR As Integer = 0
        Private Const ERROR_BAD_NET_NAME As Integer = 67
        Private Const ERROR_BUFFER_OVERFLOW As Integer = 111
        Private Const ERROR_GEN_FAILURE As Integer = 31
        Private Const ERROR_INVALID_PARAMETER As Integer = 87
        Private Const ERROR_INVALID_USER_BUFFER As Integer = 1784
        Private Const ERROR_NOT_FOUND As Integer = 1168
        Private Const ERROR_NOT_SUPPORTED As Integer = 50

        ''' <summary>
        ''' Gets the MAC address that belongs to the specified IP address.
        ''' </summary>
        ''' <remarks>This uses a native method and should be replaced when a managed alternative becomes available.</remarks>
        Public Shared Function GetMacAddress(address As IPAddress) As PhysicalAddress
            Dim IP As UInteger = BitConverter.ToUInt32(address.GetAddressBytes(), 0)
            Dim mac() As Byte = New Byte(5) {}

            Dim ReturnValue As UInteger = SendARP(IP, 0, mac, mac.Length)

            If ReturnValue = NO_ERROR Then
                Return New PhysicalAddress(mac)
            Else
                ' TODO: handle various SendARP errors
                ' http://msdn.microsoft.com/en-us/library/windows/desktop/aa366358(v=vs.85).aspx
                Throw New Win32Exception(CInt(ReturnValue))
            End If
        End Function

#End Region ' MAC Addresses

#Region "Network disk space"
        ''' <summary>
        ''' API function declaration.
        ''' </summary>
        <DllImport("kernel32.dll", SetLastError:=True)> _
        Private Shared Function GetDiskFreeSpace( _
        lpRootPathName As String,
        ByRef lpSectorsPerCluster As UInt32,
        ByRef lpBytesPerSector As UInt32,
        ByRef lpNumberOfFreeClusters As UInt32,
        ByRef lpTotalNumberOfClusters As UInt32) As Boolean
        End Function

        ''' <summary>
        ''' Gets disk information about a network share.
        ''' </summary>
        ''' <remarks>This uses a native method and should be replaced when a managed alternative becomes available.</remarks>
        Public Shared Function GetShareInfo(uncPath As Uri) As ShareInfo
            Dim info As ShareInfo

            ' Variables to store seperate parameters
            Dim sectorsPerCluster As UInt32
            Dim bytesPerSector As UInt32
            Dim numberOfFreeClusters As UInt32
            Dim totalNumberOfClusters As UInt32

            ' Make the API call
            ' If the function succeeds, the return value is nonzero.
            ' If the function fails, the return value is zero.
            Dim ReturnValue As Boolean = GetDiskFreeSpace(uncPath.LocalPath, sectorsPerCluster, bytesPerSector, numberOfFreeClusters, totalNumberOfClusters)

            If ReturnValue.IsTrue Then ' create a dictionary of the returned values
                info = New ShareInfo(sectorsPerCluster, bytesPerSector, numberOfFreeClusters, totalNumberOfClusters)
            Else
                Throw New Win32Exception(Marshal.GetLastWin32Error)
            End If

            Return info
        End Function

        Public Structure ShareInfo
            Private _sectorsPerCluster As UInteger
            Private _bytesPerSector As UInteger
            Private _numberOfFreeClusters As UInteger
            Private _totalNumberOfClusters As UInteger

            Public Sub New(sectorsPerCluster As UInteger, bytesPerSector As UInteger, numberOfFreeClusters As UInteger, totalNumberOfClusters As UInteger)
                _sectorsPerCluster = sectorsPerCluster
                _bytesPerSector = bytesPerSector
                _numberOfFreeClusters = numberOfFreeClusters
                _totalNumberOfClusters = totalNumberOfClusters
            End Sub

            Public ReadOnly Property SectorsPerCluster As UInteger
                Get
                    Return _sectorsPerCluster
                End Get
            End Property

            Public ReadOnly Property BytesPerSector As UInteger
                Get
                    Return _bytesPerSector
                End Get
            End Property

            Public ReadOnly Property NumberOfFreeClusters As UInteger
                Get
                    Return _numberOfFreeClusters
                End Get
            End Property

            Public ReadOnly Property TotalNumberOfClusters As UInteger
                Get
                    Return _totalNumberOfClusters
                End Get
            End Property

            Public Overrides Function ToString() As String
                Dim total As ULong = CULng(TotalNumberOfClusters) * CULng(SectorsPerCluster) * CULng(BytesPerSector)
                Dim free As ULong = CULng(NumberOfFreeClusters) * CULng(SectorsPerCluster) * CULng(BytesPerSector)
                Dim used As ULong = total - free

                Return Math.Round(total / 1024 ^ 3, 2).ToString + " GiB (" + Math.Round(total / 1000 ^ 3, 2).ToString + " GB)"
            End Function

        End Structure

#End Region ' Network disk space

#Region "Network shares"

#Region "Share Type"

        ''' <summary>
        ''' Enumeration of share types.
        ''' </summary>
        <Flags()> _
        Public Enum ShareType
            ''' <summary>Disk share</summary>
            Disk = 0
            ''' <summary>Printer share</summary>
            Printer = 1
            ''' <summary>Device share</summary>
            Device = 2
            ''' <summary>IPC share</summary>
            IPC = 3
            ''' <summary>Special share</summary>
            Special = -2147483648
            ' 0x80000000,
        End Enum

#End Region

#Region "Share"

        ''' <summary>
        ''' Information about a local share
        ''' </summary>
        ''' <remarks>This type depends on a native method and should be replaced when a managed alternative becomes available.</remarks>
        Public Class Share

            Private _server As String
            Private _netName As String
            Private _path As String
            Private _shareType As ShareType
            Private _remark As String

            Public Sub New(server As String, netName As String, path As String, shareType As ShareType, remark As String)
                If shareType.Special = shareType AndAlso "IPC$" = netName Then
                    shareType = shareType Or shareType.IPC
                End If

                _server = server
                _netName = netName
                _path = path
                _shareType = shareType
                _remark = remark
            End Sub

#Region "Properties"

            ''' <summary>
            ''' Gets the name of the computer that this share belongs to.
            ''' </summary>
            Public ReadOnly Property Server() As String
                Get
                    Return _server
                End Get
            End Property

            ''' <summary>
            ''' Gets the share name.
            ''' </summary>
            Public ReadOnly Property NetName() As String
                Get
                    Return _netName
                End Get
            End Property

            ''' <summary>
            ''' Gets the local path.
            ''' </summary>
            Public ReadOnly Property Path() As String
                Get
                    Return _path
                End Get
            End Property

            ''' <summary>
            ''' Gets the share type.
            ''' </summary>
            Public ReadOnly Property ShareType() As ShareType
                Get
                    Return _shareType
                End Get
            End Property

            ''' <summary>
            ''' Comment
            ''' </summary>
            Public ReadOnly Property Remark() As String
                Get
                    Return _remark
                End Get
            End Property

            ''' <summary>
            ''' Gets whether the share is a file system share.
            ''' </summary>
            Public ReadOnly Property IsFileSystem() As Boolean
                Get
                    ' Shared device
                    If 0 <> (_shareType And ShareType.Device) Then
                        Return False
                    End If
                    ' IPC share
                    If 0 <> (_shareType And ShareType.IPC) Then
                        Return False
                    End If
                    ' Shared printer
                    If 0 <> (_shareType And ShareType.Printer) Then
                        Return False
                    End If

                    ' Standard disk share
                    If 0 = (_shareType And ShareType.Special) Then
                        Return True
                    End If

                    ' Special disk share (e.g. C$)
                    If ShareType.Special = _shareType AndAlso _netName IsNot Nothing AndAlso 0 <> _netName.Length Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
            End Property

            ''' <summary>
            ''' Get the root of a disk-based share.
            ''' </summary>
            Public ReadOnly Property Root() As DirectoryInfo
                Get
                    If IsFileSystem Then
                        If _server Is Nothing OrElse 0 = _server.Length Then
                            If _path Is Nothing OrElse 0 = _path.Length Then
                                Return New DirectoryInfo(ToString())
                            Else
                                Return New DirectoryInfo(_path)
                            End If
                        Else
                            Return New DirectoryInfo(ToString())
                        End If
                    Else
                        Return Nothing
                    End If
                End Get
            End Property

#End Region

            ''' <summary>
            ''' Returns the path to this share
            ''' </summary>
            Public Overrides Function ToString() As String
                If _server Is Nothing OrElse 0 = _server.Length Then
                    Return String.Format("\\{0}\{1}", Environment.MachineName, _netName)
                Else
                    Return String.Format("\\{0}\{1}", _server, _netName)
                End If
            End Function

            ''' <summary>
            ''' Returns true if this share matches the local path
            ''' </summary>
            ''' <param name="path">The path to match.</param>
            ''' <returns></returns>
            Public Function MatchesPath(path As String) As Boolean
                If Not IsFileSystem Then
                    Return False
                End If
                If path Is Nothing OrElse 0 = path.Length Then
                    Return True
                End If

                Return path.ToLower().StartsWith(_path.ToLower())
            End Function
        End Class

#End Region

#Region "Share Collection"

        ''' <summary>
        ''' A collection of shares
        ''' </summary>
        Public Class ShareCollection
            Inherits ReadOnlyCollectionBase
#Region "Platform"

            ''' <summary>
            ''' Is this an NT platform?
            ''' </summary>
            Protected Shared ReadOnly Property IsNT() As Boolean
                Get
                    Return (PlatformID.Win32NT = Environment.OSVersion.Platform)
                End Get
            End Property

            ''' <summary>
            ''' Returns true if this is Windows 2000 or higher
            ''' </summary>
            Protected Shared ReadOnly Property IsW2KUp() As Boolean
                Get
                    Dim os As OperatingSystem = Environment.OSVersion
                    If PlatformID.Win32NT = os.Platform AndAlso os.Version.Major >= 5 Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
            End Property

#End Region

#Region "Interop"

#Region "Constants"

            ''' <summary>Maximum path length</summary>
            Protected Const MAX_PATH As Integer = 260
            ''' <summary>No error</summary>
            Protected Const NO_ERROR As Integer = 0
            ''' <summary>Access denied</summary>
            Protected Const ERROR_ACCESS_DENIED As Integer = 5
            ''' <summary>Access denied</summary>
            Protected Const ERROR_WRONG_LEVEL As Integer = 124
            ''' <summary>More data available</summary>
            Protected Const ERROR_MORE_DATA As Integer = 234
            ''' <summary>Not connected</summary>
            Protected Const ERROR_NOT_CONNECTED As Integer = 2250
            ''' <summary>Level 1</summary>
            Protected Const UNIVERSAL_NAME_INFO_LEVEL As Integer = 1
            ''' <summary>Max extries (9x)</summary>
            Protected Const MAX_SI50_ENTRIES As Integer = 20

#End Region

#Region "Structures"

            ''' <summary>Unc name</summary>
            <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
            Protected Structure UNIVERSAL_NAME_INFO
                <MarshalAs(UnmanagedType.LPTStr)> _
                Public lpUniversalName As String
            End Structure

            ''' <summary>Share information, NT, level 2</summary>
            ''' <remarks>
            ''' Requires admin rights to work. 
            ''' </remarks>
            <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
            Protected Structure SHARE_INFO_2
                <MarshalAs(UnmanagedType.LPWStr)> _
                Public NetName As String
                Public ShareType As ShareType
                <MarshalAs(UnmanagedType.LPWStr)> _
                Public Remark As String
                Public Permissions As Integer
                Public MaxUsers As Integer
                Public CurrentUsers As Integer
                <MarshalAs(UnmanagedType.LPWStr)> _
                Public Path As String
                <MarshalAs(UnmanagedType.LPWStr)> _
                Public Password As String
            End Structure

            ''' <summary>Share information, NT, level 1</summary>
            ''' <remarks>
            ''' Fallback when no admin rights.
            ''' </remarks>
            <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
            Protected Structure SHARE_INFO_1
                <MarshalAs(UnmanagedType.LPWStr)> _
                Public NetName As String
                Public ShareType As ShareType
                <MarshalAs(UnmanagedType.LPWStr)> _
                Public Remark As String
            End Structure

            ''' <summary>Share information, Win9x</summary>
            <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi, Pack:=1)> _
            Protected Structure SHARE_INFO_50
                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=13)> _
                Public NetName As String

                Public bShareType As UShort
                Public Flags As UShort

                <MarshalAs(UnmanagedType.LPTStr)> _
                Public Remark As String
                <MarshalAs(UnmanagedType.LPTStr)> _
                Public Path As String

                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=9)> _
                Public PasswordRW As String
                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=9)> _
                Public PasswordRO As String

                Public ReadOnly Property ShareType() As ShareType
                    Get
                        Return CType(CInt(bShareType) And &H7F, ShareType)
                    End Get
                End Property
            End Structure

            ''' <summary>Share information level 1, Win9x</summary>
            <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi, Pack:=1)> _
            Protected Structure SHARE_INFO_1_9x
                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=13)> _
                Public NetName As String
                Public Padding As UShort

                Public bShareType As UShort

                <MarshalAs(UnmanagedType.LPTStr)> _
                Public Remark As String

                Public ReadOnly Property ShareType() As ShareType
                    Get
                        Return CType(CInt(bShareType) And &H7FFF, ShareType)
                    End Get
                End Property
            End Structure

#End Region

#Region "Functions"

            ''' <summary>Get a UNC name</summary>
            <DllImport("mpr", CharSet:=CharSet.Auto)> _
            Protected Shared Function WNetGetUniversalName(lpLocalPath As String, dwInfoLevel As Integer, ByRef lpBuffer As UNIVERSAL_NAME_INFO, ByRef lpBufferSize As Integer) As Integer
            End Function

            ''' <summary>Get a UNC name</summary>
            <DllImport("mpr", CharSet:=CharSet.Auto)> _
            Protected Shared Function WNetGetUniversalName(lpLocalPath As String, dwInfoLevel As Integer, lpBuffer As IntPtr, ByRef lpBufferSize As Integer) As Integer
            End Function

            ''' <summary>Enumerate shares (NT)</summary>
            <DllImport("netapi32", CharSet:=CharSet.Unicode)> _
            Protected Shared Function NetShareEnum(lpServerName As String, dwLevel As Integer, ByRef lpBuffer As IntPtr, dwPrefMaxLen As Integer, ByRef entriesRead As Integer, ByRef totalEntries As Integer, _
   ByRef hResume As Integer) As Integer
            End Function

            ''' <summary>Enumerate shares (9x)</summary>
            <DllImport("svrapi", CharSet:=CharSet.Ansi)> _
            Protected Shared Function NetShareEnum(<MarshalAs(UnmanagedType.LPTStr)> lpServerName As String, dwLevel As Integer, lpBuffer As IntPtr, cbBuffer As UShort, ByRef entriesRead As UShort, ByRef totalEntries As UShort) As Integer
            End Function

            ''' <summary>Free the buffer (NT)</summary>
            <DllImport("netapi32")> _
            Protected Shared Function NetApiBufferFree(lpBuffer As IntPtr) As Integer
            End Function

#End Region

#Region "Enumerate shares"

            ''' <summary>
            ''' Enumerates the shares on Windows NT
            ''' </summary>
            ''' <param name="server">The server name</param>
            ''' <param name="shares">The ShareCollection</param>
            Protected Shared Sub EnumerateSharesNT(server As String, shares As ShareCollection)
                Dim level As Integer = 2
                Dim entriesRead As Integer, totalEntries As Integer, nRet As Integer, hResume As Integer = 0
                Dim pBuffer As IntPtr = IntPtr.Zero

                Try
                    nRet = NetShareEnum(server, level, pBuffer, -1, entriesRead, totalEntries, _
                     hResume)

                    If ERROR_ACCESS_DENIED = nRet Then
                        'Need admin for level 2, drop to level 1
                        level = 1
                        nRet = NetShareEnum(server, level, pBuffer, -1, entriesRead, totalEntries, _
                         hResume)
                    End If

                    If NO_ERROR = nRet AndAlso entriesRead > 0 Then
                        Dim t As Type = If((2 = level), GetType(SHARE_INFO_2), GetType(SHARE_INFO_1))
                        Dim offset As Integer = Marshal.SizeOf(t)

                        Dim i As Integer = 0, lpItem As Integer = pBuffer.ToInt32()
                        While i < entriesRead
                            Dim pItem As New IntPtr(lpItem)
                            If 1 = level Then
                                Dim si As SHARE_INFO_1 = CType(Marshal.PtrToStructure(pItem, t), SHARE_INFO_1)
                                shares.Add(si.NetName, String.Empty, si.ShareType, si.Remark)
                            Else
                                Dim si As SHARE_INFO_2 = CType(Marshal.PtrToStructure(pItem, t), SHARE_INFO_2)
                                shares.Add(si.NetName, si.Path, si.ShareType, si.Remark)
                            End If
                            i += 1
                            lpItem += offset
                        End While

                    End If
                Finally
                    ' Clean up buffer allocated by system
                    If IntPtr.Zero <> pBuffer Then
                        NetApiBufferFree(pBuffer)
                    End If
                End Try
            End Sub

            ''' <summary>
            ''' Enumerates the shares on Windows 9x
            ''' </summary>
            ''' <param name="server">The server name</param>
            ''' <param name="shares">The ShareCollection</param>
            Protected Shared Sub EnumerateShares9x(server As String, shares As ShareCollection)
                Dim level As Integer = 50
                Dim nRet As Integer = 0
                Dim entriesRead As UShort, totalEntries As UShort

                Dim t As Type = GetType(SHARE_INFO_50)
                Dim size As Integer = Marshal.SizeOf(t)
                Dim cbBuffer As UShort = CUShort(MAX_SI50_ENTRIES * size)
                'On Win9x, must allocate buffer before calling API
                Dim pBuffer As IntPtr = Marshal.AllocHGlobal(cbBuffer)

                Try
                    nRet = NetShareEnum(server, level, pBuffer, cbBuffer, entriesRead, totalEntries)

                    If ERROR_WRONG_LEVEL = nRet Then
                        level = 1
                        t = GetType(SHARE_INFO_1_9x)
                        size = Marshal.SizeOf(t)

                        nRet = NetShareEnum(server, level, pBuffer, cbBuffer, entriesRead, totalEntries)
                    End If

                    If NO_ERROR = nRet OrElse ERROR_MORE_DATA = nRet Then
                        Dim i As Integer = 0, lpItem As Integer = pBuffer.ToInt32()
                        While i < entriesRead
                            Dim pItem As New IntPtr(lpItem)

                            If 1 = level Then
                                Dim si As SHARE_INFO_1_9x = CType(Marshal.PtrToStructure(pItem, t), SHARE_INFO_1_9x)
                                shares.Add(si.NetName, String.Empty, si.ShareType, si.Remark)
                            Else
                                Dim si As SHARE_INFO_50 = CType(Marshal.PtrToStructure(pItem, t), SHARE_INFO_50)
                                shares.Add(si.NetName, si.Path, si.ShareType, si.Remark)
                            End If
                            i += 1
                            lpItem += size
                        End While
                    Else
                        Console.WriteLine(nRet)

                    End If
                Finally
                    'Clean up buffer
                    Marshal.FreeHGlobal(pBuffer)
                End Try
            End Sub

            ''' <summary>
            ''' Enumerates the shares
            ''' </summary>
            ''' <param name="server">The server name</param>
            ''' <param name="shares">The ShareCollection</param>
            Protected Shared Sub EnumerateShares(server As String, shares As ShareCollection)
                If server IsNot Nothing AndAlso 0 <> server.Length AndAlso Not IsW2KUp Then
                    server = server.ToUpper()

                    ' On NT4, 9x and Me, server has to start with "\\"
                    If Not ("\"c = server(0) AndAlso "\"c = server(1)) Then
                        server = "\\" & server
                    End If
                End If

                If IsNT Then
                    EnumerateSharesNT(server, shares)
                Else
                    EnumerateShares9x(server, shares)
                End If
            End Sub

#End Region

#End Region

#Region "Static methods"

            ''' <summary>
            ''' Returns true if fileName is a valid local file-name of the form:
            ''' X:\, where X is a drive letter from A-Z
            ''' </summary>
            ''' <param name="fileName">The filename to check</param>
            ''' <returns></returns>
            Public Shared Function IsValidFilePath(fileName As String) As Boolean
                If fileName Is Nothing OrElse 0 = fileName.Length Then
                    Return False
                End If

                Dim drive As Char = Char.ToUpper(fileName(0))
                If "A"c > drive OrElse drive > "Z"c Then
                    Return False

                ElseIf Path.VolumeSeparatorChar <> fileName(1) Then
                    Return False
                ElseIf Path.DirectorySeparatorChar <> fileName(2) Then
                    Return False
                Else
                    Return True
                End If
            End Function

            ''' <summary>
            ''' Returns the UNC path for a mapped drive or local share.
            ''' </summary>
            ''' <param name="fileName">The path to map</param>
            ''' <returns>The UNC path (if available)</returns>
            Public Shared Function PathToUnc(fileName As String) As String
                If fileName Is Nothing OrElse 0 = fileName.Length Then
                    Return String.Empty
                End If

                fileName = Path.GetFullPath(fileName)
                If Not IsValidFilePath(fileName) Then
                    Return fileName
                End If

                Dim nRet As Integer = 0
                Dim rni As New UNIVERSAL_NAME_INFO()
                Dim bufferSize As Integer = Marshal.SizeOf(rni)

                nRet = WNetGetUniversalName(fileName, UNIVERSAL_NAME_INFO_LEVEL, rni, bufferSize)

                If ERROR_MORE_DATA = nRet Then
                    Dim pBuffer As IntPtr = Marshal.AllocHGlobal(bufferSize)


                    Try
                        nRet = WNetGetUniversalName(fileName, UNIVERSAL_NAME_INFO_LEVEL, pBuffer, bufferSize)

                        If NO_ERROR = nRet Then
                            rni = CType(Marshal.PtrToStructure(pBuffer, GetType(UNIVERSAL_NAME_INFO)), UNIVERSAL_NAME_INFO)
                        End If
                    Finally
                        Marshal.FreeHGlobal(pBuffer)
                    End Try
                End If

                Select Case nRet
                    Case NO_ERROR
                        Return rni.lpUniversalName

                    Case ERROR_NOT_CONNECTED
                        'Local file-name
                        Dim shi As ShareCollection = LocalShares
                        If shi IsNot Nothing Then
                            Dim share As Share = shi(fileName)
                            If share IsNot Nothing Then
                                Dim path__1 As String = share.Path
                                If path__1 IsNot Nothing AndAlso 0 <> path__1.Length Then
                                    Dim index As Integer = path__1.Length
                                    If Path.DirectorySeparatorChar <> path__1(path__1.Length - 1) Then
                                        index += 1
                                    End If

                                    If index < fileName.Length Then
                                        fileName = fileName.Substring(index)
                                    Else
                                        fileName = String.Empty
                                    End If

                                    fileName = Path.Combine(share.ToString(), fileName)
                                End If
                            End If
                        End If

                        Return fileName
                    Case Else

                        Console.WriteLine("Unknown return value: {0}", nRet)
                        Return String.Empty
                End Select
            End Function

            ''' <summary>
            ''' Returns the local <see cref="Share"/> object with the best match
            ''' to the specified path.
            ''' </summary>
            ''' <param name="fileName"></param>
            ''' <returns></returns>
            Public Shared Function PathToShare(fileName As String) As Share
                If fileName Is Nothing OrElse 0 = fileName.Length Then
                    Return Nothing
                End If

                fileName = Path.GetFullPath(fileName)
                If Not IsValidFilePath(fileName) Then
                    Return Nothing
                End If

                Dim shi As ShareCollection = LocalShares
                If shi Is Nothing Then
                    Return Nothing
                Else
                    Return shi(fileName)
                End If
            End Function

#End Region

#Region "Local shares"

            ''' <summary>The local shares</summary>
            Private Shared _local As ShareCollection = Nothing

            ''' <summary>
            ''' Return the local shares
            ''' </summary>
            Public Shared ReadOnly Property LocalShares() As ShareCollection
                Get
                    If _local Is Nothing Then
                        _local = New ShareCollection()
                    End If

                    Return _local
                End Get
            End Property

            ''' <summary>
            ''' Return the shares for a specified machine
            ''' </summary>
            ''' <param name="server"></param>
            ''' <returns></returns>
            Public Shared Function GetShares(server As String) As ShareCollection
                Return New ShareCollection(server)
            End Function

#End Region

#Region "Private Data"

            ''' <summary>The name of the server this collection represents</summary>
            Private _server As String

#End Region

#Region "Constructor"

            ''' <summary>
            ''' Default constructor - local machine
            ''' </summary>
            Public Sub New()
                _server = String.Empty
                EnumerateShares(_server, Me)
            End Sub

            ''' <summary>
            ''' Constructor
            ''' </summary>
            ''' <param name="Server"></param>
            Public Sub New(server As String)
                _server = server
                EnumerateShares(_server, Me)
            End Sub

#End Region

#Region "Add"

            Protected Sub Add(share As Share)
                InnerList.Add(share)
            End Sub

            Protected Sub Add(netName As String, path As String, shareType As ShareType, remark As String)
                InnerList.Add(New Share(_server, netName, path, shareType, remark))
            End Sub

#End Region

#Region "Properties"

            ''' <summary>
            ''' Returns the name of the server this collection represents
            ''' </summary>
            Public ReadOnly Property Server() As String
                Get
                    Return _server
                End Get
            End Property

            ''' <summary>
            ''' Returns the <see cref="Share"/> at the specified index.
            ''' </summary>
            Default Public ReadOnly Property Item(index As Integer) As Share
                Get
                    Return DirectCast(InnerList(index), Share)
                End Get
            End Property

            ''' <summary>
            ''' Returns the <see cref="Share"/> which matches a given local path
            ''' </summary>
            ''' <param name="path">The path to match</param>
            Default Public ReadOnly Property Item(path As String) As Share
                Get
                    If path Is Nothing OrElse 0 = path.Length Then
                        Return Nothing
                    End If

                    path = System.IO.Path.GetFullPath(path)
                    If Not IsValidFilePath(path) Then
                        Return Nothing
                    End If

                    Dim match As Share = Nothing

                    For i As Integer = 0 To InnerList.Count - 1
                        Dim s As Share = DirectCast(InnerList(i), Share)

                        If s.IsFileSystem AndAlso s.MatchesPath(path) Then
                            'Store first match
                            If match Is Nothing Then
                                match = s

                                ' If this has a longer path,
                                ' and this is a disk share or match is a special share, 
                                ' then this is a better match
                            ElseIf match.Path.Length < s.Path.Length Then
                                If ShareType.Disk = s.ShareType OrElse ShareType.Disk <> match.ShareType Then
                                    match = s
                                End If
                            End If
                        End If
                    Next

                    Return match
                End Get
            End Property

#End Region

#Region "Implementation of ICollection"

            ''' <summary>
            ''' Copy this collection to an array
            ''' </summary>
            Public Sub CopyTo(array As Share(), index As Integer)
                InnerList.CopyTo(array, index)
            End Sub

#End Region
        End Class

#End Region

#End Region ' Network shares

    End Class



End Class

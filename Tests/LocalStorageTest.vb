Imports System.Net

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SL.DuneApiCodePack.Storage
Imports System.IO



'''<summary>
'''This is a test class for LocalStorageTest and is intended
'''to contain all LocalStorageTest Unit Tests
'''</summary>
<TestClass()> _
Public Class LocalStorageTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for LocalStorage Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub LocalStorageConstructorTest()
        Dim host As IPHostEntry = Dns.GetHostEntry("DUNE")

        Dim optical As String = "optical_disk_3"
        Dim tar As New LocalStorage(host, New IO.DirectoryInfo("\\" + host.HostName + "\" + optical))

        Dim nameAndUuid As String = "DuneHDD_227c0ea3_d586_4186_a1d1_d81432256bc2"
        Dim name As String = "DuneHDD"
        Dim uuid As String = "227c0ea3_d586_4186_a1d1_d81432256bc2"
        Dim label As String = "Dune HDD"

        Dim usb As String = "usb_storage_227c0ea3_d586_4186_a1d1_d81432256bc2"
        Dim usb2 As String = "usb_storage_2_227c0ea3_d586_4186_a1d1_d81432256bc2"

        Dim usblocal As LocalStorage = New LocalStorage(host, New IO.DirectoryInfo("\\" + host.HostName + "\" + usb))

        Dim usblocal2 As LocalStorage = New LocalStorage(host, New IO.DirectoryInfo("\\" + host.HostName + "\" + usb2))




        Dim targetlo As LocalStorage = New LocalStorage(host, New IO.DirectoryInfo("\\" + host.HostName + "\" + nameAndUuid))

        Dim testFile As New FileInfo("\\DUNE\DuneHDD\play\Bangarang\02 Bangarang.mp3")
        Dim url As String
        Dim success As Boolean = targetlo.TryGetMediaUrlFromStorageName(testFile, url)


        Dim fromName As String = targetlo.GetMediaUrlFromStorageName(testFile)
        Dim fromLabel As String = targetlo.GetMediaUrlFromStorageLabel(testFile)
        Dim fromUuid As String = targetlo.GetMediaUrlFromStorageUuid(testFile)



        Dim target As LocalStorage = New LocalStorage(host, New IO.DirectoryInfo("\\" + host.HostName + "\" + name))
        Dim target3 As LocalStorage = New LocalStorage(host, New IO.DirectoryInfo("\\" + host.HostName + "\" + label))
    End Sub
End Class

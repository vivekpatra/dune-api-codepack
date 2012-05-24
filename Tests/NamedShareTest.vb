Imports System.IO

Imports System.Net

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SL.DuneApiCodePack.Storage



'''<summary>
'''This is a test class for NamedShareTest and is intended
'''to contain all NamedShareTest Unit Tests
'''</summary>
<TestClass()> _
Public Class NamedShareTest


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
            testContextInstance = Value
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
    '''A test for GetMediaUrl
    '''</summary>
    <TestMethod()> _
    Public Sub GetMediaUrlTest()
        Dim host As IPHostEntry = Dns.GetHostEntry("DUNE")
        Dim name As String = "TheNamedFolder"
        Dim path As New IO.DirectoryInfo("\\WRT610N\Public\Multimedia\")
        Dim target As NamedShare = New NamedShare(host, name, path)

        Dim mediaurl As DirectoryInfo = New DirectoryInfo("\\WRT610N\Public\Multimedia\TV\2 Broke Girls\Season1\2 Broke Girls.S01E01.Pilot.avi")

        Dim expected As String = "network_folder://TheNamedFolder/TV/2 Broke Girls/Season1/2 Broke Girls.S01E01.Pilot.avi"

        Dim actual As String

        actual = target.GetMediaUrl(mediaurl)
        Assert.AreEqual(expected, actual)
    End Sub
End Class

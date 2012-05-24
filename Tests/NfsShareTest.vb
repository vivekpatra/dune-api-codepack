Imports System.IO

Imports System.Net

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SL.DuneApiCodePack.Storage



'''<summary>
'''This is a test class for NfsShareTest and is intended
'''to contain all NfsShareTest Unit Tests
'''</summary>
<TestClass()> _
Public Class NfsShareTest


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
        Dim host As IPHostEntry = Dns.GetHostEntry("192.168.1.1")

        Dim path As String = "/Public/Multimedia"
        Dim exportPath As String = "Dune"


        Dim target As NfsShare = New NfsShare(host, exportPath, path)

        Dim absolutePath As FileInfo = New FileInfo("P:\TV\2 Broke Girls\Season1\2 Broke Girls.S01E01.Pilot.avi")


        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.GetMediaUrl(absolutePath)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetMediaUrl
    '''</summary>
    <TestMethod()> _
    Public Sub GetMediaUrlTest1()
        Dim host As IPHostEntry = Nothing ' TODO: Initialize to an appropriate value
        Dim path As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim target As NfsShare = New NfsShare(host, path) ' TODO: Initialize to an appropriate value
        Dim relativePath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.GetMediaUrl(relativePath)
        Assert.AreEqual(expected, actual)
    End Sub
End Class

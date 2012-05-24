Imports System.IO

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SL.DuneApiCodePack.Storage



'''<summary>
'''This is a test class for SmbShareTest and is intended
'''to contain all SmbShareTest Unit Tests
'''</summary>
<TestClass()> _
Public Class SmbShareTest


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
    '''A test for SmbShare Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub SmbShareConstructorTest()
        Dim path As String = "\\WRT610N\Public"
        Dim uncPath As Uri = New Uri(path)
        Dim target As SmbShare = New SmbShare(uncPath)

        Assert.IsTrue(target.Root.FullName = "\\WRT610N\Public")
    End Sub

    '''<summary>
    '''A test for GetMediaUrl
    '''</summary>
    <TestMethod()> _
    Public Sub GetMediaUrlTest()
        Dim path As String = "\\WRT610N\Public"
        Dim uncPath As Uri = New Uri(path)
        Dim target As SmbShare = New SmbShare(uncPath)

        Dim mediaurl As DirectoryInfo = New DirectoryInfo("\\WRT610N\Public\Multimedia\TV\2 Broke Girls\Season1\2 Broke Girls.S01E01.Pilot.avi")

        Dim expected As String = "smb://WRT610N/Public/Multimedia/TV/2 Broke Girls/Season1/2 Broke Girls.S01E01.Pilot.avi"
        Dim actual As String
        actual = target.GetMediaUrl(mediaurl)
        Assert.AreEqual(expected, actual)

        target.Username = "User"
        Dim withUser As String = target.GetMediaUrl(mediaurl)

        target.Password = "Password"
        Dim withUserAndPassword As String = target.GetMediaUrl(mediaurl)


    End Sub
End Class

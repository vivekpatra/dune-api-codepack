Imports System.Collections.Generic

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SL.DuneApiCodePack.Networking



'''<summary>
'''This is a test class for NetworkDeviceTest and is intended
'''to contain all NetworkDeviceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class NetworkDeviceTest


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
    '''A test for Scan
    '''</summary>
    <TestMethod()> _
    Public Sub ScanTest()

        Dim actual As List(Of NetworkDevice)
        actual = NetworkDevice.Scan

        For Each NetworkDevice In actual
            Debug.WriteLine(NetworkDevice.IsDune)
            Dim shares = NetworkDevice.GetShares
        Next
    End Sub
End Class

Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO

Public Class NetworkCardVendor

    Private Const ApiBaseUri As String = "http://www.macvendorlookup.com/api/"
    Private Const ApiKey As String = "BOWGGVM" ' get your own damn key if you want to use this API outside this class!

    Private _company As String
    Private _department As String
    Private _address1 As String
    Private _address2 As String
    Private _country As String

    Public Sub New(ByVal physicalAddress As PhysicalAddress)
        GetResults(physicalAddress)
    End Sub

    ''' <summary>
    ''' Gets vendor information from the OUI database based on the supplied hardware address.
    ''' </summary>
    ''' <param name="physicalAddress">The MAC address to look up.</param>
    Private Sub GetResults(ByVal physicalAddress As PhysicalAddress)
        Dim delimiter As Char = "|"c

        Dim client As New WebClient()
        Dim request As New Uri(ApiBaseUri + ApiKey + "/" + physicalAddress.ToString)

        Dim results As String = client.DownloadString(request)

        _company = results.Split(delimiter).GetValue(0).ToString
        _department = results.Split(delimiter).GetValue(1).ToString
        _address1 = results.Split(delimiter).GetValue(2).ToString
        _address2 = results.Split(delimiter).GetValue(3).ToString
        _country = results.Split(delimiter).GetValue(4).ToString
    End Sub

    ''' <summary>
    ''' Gets the name of the company.
    ''' </summary>
    Public ReadOnly Property Company As String
        Get
            Return _company
        End Get
    End Property

    ''' <summary>
    ''' Gets the company's department.
    ''' </summary>
    Public ReadOnly Property Department As String
        Get
            Return _department
        End Get
    End Property

    ''' <summary>
    ''' Gets the company's first address line.
    ''' </summary>
    Public ReadOnly Property Address1 As String
        Get
            Return _address1
        End Get
    End Property

    ''' <summary>
    ''' Gets the company's second address line.
    ''' </summary>
    Public ReadOnly Property Address2 As String
        Get
            Return _address2
        End Get
    End Property

    ''' <summary>
    ''' Gets the company's country.
    ''' </summary>
    Public ReadOnly Property Country As String
        Get
            Return _country
        End Get
    End Property
End Class

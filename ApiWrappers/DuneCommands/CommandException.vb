Imports System.Runtime.Serialization

Namespace Dune.ApiWrappers

    ''' <summary>
    ''' The exception that is created when a command fails.
    ''' </summary>
    <Serializable()>
    Public Class CommandException
        Inherits Exception
        Implements ISerializable

        Private _errorKind As String

        ''' <summary>
        ''' Gets the error kind.
        ''' </summary>
        Public ReadOnly Property ErrorKind As String
            Get
                Return _errorKind
            End Get
        End Property

        Private Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal errorKind As String, ByVal errorDescription As String)
            MyBase.New(errorDescription)
            _errorKind = errorKind
        End Sub

        Public Sub New(ByVal errorKind As String, ByVal errorDescription As String, ByVal inner As Exception)
            MyBase.New(errorDescription, inner)
            _errorKind = errorKind
        End Sub

        ''' <remarks>This constructor is needed for serialization.</remarks>
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)

            If info IsNot Nothing Then
                _errorKind = info.GetString("_errorKind")
            End If
        End Sub

        Public Overrides Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
            MyBase.GetObjectData(info, context)

            If info IsNot Nothing Then
                info.AddValue("_errorKind", _errorKind)
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return ErrorKind + ": " + Message
        End Function
    End Class

End Namespace
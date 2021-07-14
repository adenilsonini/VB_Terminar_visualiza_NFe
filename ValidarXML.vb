Imports System.IO
Imports System.Xml
Imports System.Xml.Schema

Public Module ValidaXML
    Private _Erro As String
    Public Property Erro() As String
        Get
            Return _Erro
        End Get
        Set(ByVal value As String)
            _Erro = value
        End Set
    End Property

  
End Module
Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text

Public Class ArquivoIni

    Private EXE As String = Assembly.GetExecutingAssembly().Location
    Private Path As String

    <DllImport("kernel32.dll")> Private Shared Function WritePrivateProfileString(ByVal section As String, ByVal key As String, ByVal val As String, ByVal filePath As String) As Long
    End Function
    <DllImport("kernel32")> Private Shared Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
    End Function

    Public Sub New(ByVal IniPath As String)
        Me.Path = IniPath
    End Sub

    Public Function Read(ByVal Key As String, Optional ByVal Section As String = Nothing, Optional ByVal [Default] As String = "") As String
        Try
            Dim retVal As StringBuilder = New StringBuilder(CInt(Byte.MaxValue))
            ArquivoIni.GetPrivateProfileString(Section, Key, [Default], retVal, CInt(Byte.MaxValue), Me.Path)
            Return retVal.ToString()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub Write(ByVal Key As String, ByVal Value As String, Optional ByVal Section As String = Nothing)
        ArquivoIni.WritePrivateProfileString(If(Section, Me.EXE), Key, Value, Me.Path)
    End Sub

    Public Sub DeleteKey(ByVal Key As String, Optional ByVal Section As String = Nothing)
        Me.Write(Key, CStr(Nothing), If(Section, Me.EXE))
    End Sub

    Public Sub DeleteSection(Optional ByVal Section As String = Nothing)
        Me.Write(CStr(Nothing), CStr(Nothing), If(Section, Me.EXE))
    End Sub

    Public Function KeyExists(ByVal Key As String, Optional ByVal Section As String = Nothing) As Boolean
        Return Me.Read(Key, Section, "").Length > 0
    End Function
End Class



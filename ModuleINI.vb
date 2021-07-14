Imports System.Text
Imports System.Runtime.InteropServices
Module ModuleINI
    <DllImport("kernel32.dll", SetLastError:=True)> Public Function WritePrivateProfileString _
      (ByVal lpApplicationName As String, _
      ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> Public Function GetPrivateProfileString( _
      ByVal lpAppName As String, _
      ByVal lpKeyName As String, _
      ByVal lpDefault As String, _
      ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    End Function

    Public Function lerINI(ByVal strIniFile As String, ByVal strKey As String, ByVal strItem As String) As String
        Dim strValue As StringBuilder = New StringBuilder(255)
        Dim intSize As Integer
        intSize = GetPrivateProfileString(strKey, strItem, "", strValue, 255, strIniFile)
        Return strValue.ToString
    End Function

    Public Function escreveINI(ByVal strIniFile As String, ByVal strKey As String, _
                            ByVal strItem As String, ByVal strValue As String) _
                                As Boolean
        Return WritePrivateProfileString(strKey, strItem, strValue, strIniFile)
    End Function

    'Para ler um arquivo .ini: lerINI(Diretório do arquivo + arquivo.ini, "valor entre cochetes", "valor antes do igual")
    'Para escrever arquivo .ini: escreveINI(Diretório do arquivo + arquivo.ini, "Valor entre cochetes", "valor antes do igual", "valor depois do igual")
End Module

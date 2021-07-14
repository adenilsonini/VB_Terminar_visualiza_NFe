Imports System.Security
Imports System.Security.Principal.WindowsIdentity
Imports System.Data.OleDb
Imports System.IO
Imports System.Data.SQLite
Imports MySql.Data.MySqlClient
Module conexao
    Dim caminho_banco As String
    Public cnado As New OleDb.OleDbConnection
    Public cnsql As New ADODB.Connection
    Public cn_p As New ADODB.Connection
    Public cn As New ADODB.Connection
    Public cn_nfe As New ADODB.Connection
    Public cn_ONE As New ADODB.Connection
    Public cn_DFe As New ADODB.Connection
    Public cnemail As New ADODB.Connection
    Public cnpj As String, ip As String
    Public dr As MySqlDataReader
    Public oConnsg As New MySqlConnection
    Public oConn As New MySqlConnection
    Public sConnectionString As String
    Public data_licenca As Date
    Public Sub conectar_mysqlDFe()

        Dim arquivoIni As ArquivoIni = New ArquivoIni(Application.StartupPath & "\config.ini")
        If arquivoIni.KeyExists("ip_mysql", "DFe") = False Then
            arquivoIni.Write("ip_mysql", "192.168.254.2", "DFe")
        End If

      oConn = New MySqlConnection("server=" & arquivoIni.Read("ip_mysql", "DFe") & ";User Id=adminlinear;database=dfe; password=@2013linear")

        If oConn.State = ConnectionState.Closed Then
            oConn.Open()
        End If
    End Sub
    Public Sub conectarsqlinx()
        Try
            ip = lerINI(Application.StartupPath & "\sglinear.ini", "SGLINX", "IP")


            If cnsql.State = ConnectionState.Closed Then
                cnsql.Open("DRIVER={MySQL ODBC 3.51 Driver};Database=sglinx;Server=" & ip & ";Port=3306;uid=adminlinear;pwd=@2013linear;STMT=;Opt")
            End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso, Erro Conexão Sistema SGLinear.")
        End Try

    End Sub

    Public Sub conectar_mysql_sglinx()
       
        oConnsg = New MySqlConnection("server=192.168.254.2;User Id=adminlinear;database=sglinx; password=@2013linear")
        '  oConn = New MySqlConnection("host=" & arquivoIni.Read("ip_mysql", "DFe") & "; port=5040; userid=spedfiscal; password=spedfiscal; database=dfe")


        If oConnsg.State = ConnectionState.Closed Then
            oConnsg.Open()
        End If

    End Sub
    Public Function executa_query_SGlinx(ByVal sql As String) As Boolean
        Dim ret As Boolean

        Try
            conectar_mysql_sglinx()
            Dim insertSQL2 As MySqlCommand = New MySqlCommand(sql, oConnsg)
            insertSQL2.ExecuteNonQuery()
            oConnsg.Close()
            ret = True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "erro execute query")
        End Try


        Return ret
    End Function
    Public Sub conectar_ado()

        If cnado.State = ConnectionState.Closed Then
            cnado.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;data source= " & lerINI(Application.StartupPath & "\config.ini", "DFe", "caminho_banco") & "\sefaz.mdb"
            cnado.Open()
        End If

    End Sub

    Public Sub conectar_DFe()

        caminho_banco = lerINI(Application.StartupPath & "\config.ini", "DFe", "caminho_banco")

        Try

            If cn_DFe.State = ConnectionState.Closed Then
                cn_DFe.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & caminho_banco & "\sefaz.mdb")
            End If

        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso, Erro conexão Sistema DFe.")

        End Try

    End Sub


    Public Sub conectar_preco()

        Try

            If cn_p.State = ConnectionState.Closed Then
                cn_p.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lerINI(Application.StartupPath & "\sglinear.ini", "FORMA_PRECO", "caminho_base_temp") & "")
            End If

        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso, Erro conexão Sistema Forma Preço.")

        End Try

    End Sub


    Public Sub conectar_onedrive(ByVal caminho_one As String)

        Try

            If cn_ONE.State = ConnectionState.Closed Then
                cn_ONE.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & caminho_one & "")
            End If

        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso, Erro conexão sistema OneDrive.")

        End Try

    End Sub


    Public Sub conectar_teste()

        '  Try

        If cn_p.State = ConnectionState.Closed Then
            cn_p.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Application.StartupPath & "\banco_duplicata.mdb")
        End If

        '  Catch ex As Exception

        '     MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso, Erro conexão Sistema Forma Preço.")

        '  End Try

    End Sub

    Public Sub conectar_boleto()


        Try

            If cn.State = ConnectionState.Closed Then
                cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lerINI(Application.StartupPath & "\config.ini", "BANCO", "caminho_dup") & "")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Module

Imports System.Threading
Imports System.Xml
Imports System.Text
Imports System.Data.OleDb
Imports MySql.Data.MySqlClient

Public Class frmpesquisarnfe
    Public bytePicData() As Byte
    Dim datai As Date, dataf As Date
    Dim xmldoc As New XmlDocument()
    Dim xmlnsManager As New XmlNamespaceManager(xmldoc.NameTable)
    Private Sub cbotipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbotipo.SelectedIndexChanged
        If cbotipo.Text = "Nome do Fornecedor" Then
            txtpar.Text = "Nome_emi"
            lblinfo.Visible = True
        End If

        If cbotipo.Text = "CNPJ Fornecedor" Then
            txtpar.Text = "Cnpj_emi"
            lblinfo.Visible = True
        End If

        If cbotipo.Text = "Valor NFe" Then
            txtpar.Text = "Valor_nfe"
            lblinfo.Visible = True
        End If

        If cbotipo.Text = "Numero NFe" Then
            txtpar.Text = "Numero_nfe"
            lblinfo.Visible = True
        End If

        If cbotipo.Text = "Chave de Acesso NFe" Then
            txtpar.Text = "Chave_nfe"
            lblinfo.Visible = False
        End If

        If cbotipo.Text = "Entre Datas" Then
            Paneldata.Enabled = True
            txtpesquisa.Enabled = False
        Else
            Paneldata.Enabled = False
            txtpesquisa.Enabled = True
        End If
    End Sub

    Private Sub txtpesquisa_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtpesquisa.KeyPress
        If (e.KeyChar = Chr(13)) Then
            lblinfo.Visible = True
            lblinfo.Text = "Aguarde, o sistema esta pesquisando a NF-e Informada..."
            lblinfo.Refresh()
            pesquisar_sql()
        End If
    End Sub

    Private Sub pesquisar_sql()
        Dim loc As Boolean = False
        Dim sqlconsulta As String
        Dim xmldisponivel As String, visualizar As String

        sqlconsulta = "select * from NFe where " & txtpar.Text & " like '%" & txtpesquisa.Text & "%' and Tipo_nfe = '1-NFe Saida'"


        If chkstatus_nfe.Checked = True Then
            sqlconsulta = "select * from NFe where " & txtpar.Text & " like '%" & txtpesquisa.Text & "%' and Tipo_nfe = '1-NFe Saida' and Status_nfe = '" & cbostatusnfe.Text & "'"
        End If

        If cbotipo.Text = "Entre Datas" Then
            datai = mskdatainicial.Text
            dataf = mskdatafinal.Text
            If chkstatus_nfe.Checked = True Then
                sqlconsulta = "select * from NFe where Data_emi BETWEEN '" & datai.ToString("yyyy-MM-dd") & "' AND '" & dataf.ToString("yyyy-MM-dd") & "' and Tipo_nfe = '1-NFe Saida' and Status_nfe = '" & cbostatusnfe.Text & "'"
            Else
                sqlconsulta = "select * from NFe where Data_emi BETWEEN '" & datai.ToString("yyyy-MM-dd") & "' AND '" & dataf.ToString("yyyy-MM-dd") & "' and Tipo_nfe = '1-NFe Saida'"
            End If
        End If

        conectar_mysqlDFe()

        Dim SQLcommand As MySqlCommand
        SQLcommand = oConn.CreateCommand
        SQLcommand.CommandText = sqlconsulta
        dr = SQLcommand.ExecuteReader()

        While (dr.Read())
            If dr("xml_nfe").ToString = "" Then
                xmldisponivel = "Importar"
                visualizar = ""
            Else
                xmldisponivel = "XML SIM"
                visualizar = "Visualizar"
            End If

            If chkvlrboleto.Checked = True Then
                dgvpesquisa.Rows.Add(dr("Nome_emi"), dr("Numero_nfe"), dr("Valor_nfe"), dr("Data_emi"), _
                                    xmldisponivel, visualizar, dr("chave_nfe"), consulta_valor_boleto(dr("chave_nfe")), dr("Status_nfe"))
            Else
                dgvpesquisa.Rows.Add(dr("Nome_emi"), dr("Numero_nfe"), dr("Valor_nfe"), dr("data_emi"), _
                                   xmldisponivel, visualizar, dr("chave_nfe"), "", dr("Status_nfe"))
            End If
            loc = True
        End While

        If loc = False Then
            MsgBox("Nenhum Registro encontrado !", MsgBoxStyle.Exclamation, "Atenção")
        End If

        oConn.Close()
    End Sub

  
    Private Sub consultar()
        Dim loc As Boolean = False
        Dim rssefaz As New ADODB.Recordset
        Dim xmldisponivel As String, visualizar As String
        Dim sqlconsulta As String

        dgvpesquisa.Rows.Clear()


        ''''''''''''''''''''''''''''Tira os espacos vazios que separam a chave de acesso da NFe'''''''''''''''''''''''''''''''

        sqlconsulta = "select * from nfe where " & txtpar.Text & " like '%" & txtpesquisa.Text & "%'"



        conectar_mysqlDFe()

        Dim SQLcommand As MySqlCommand
        SQLcommand = oConn.CreateCommand
        SQLcommand.CommandText = sqlconsulta
        dr = SQLcommand.ExecuteReader()

        While (dr.Read())
            If dr("xml_nfe").ToString = "" Then
                xmldisponivel = "Importar"
                visualizar = ""
            Else
                xmldisponivel = "XML SIM"
                visualizar = "Visualizar"
            End If

            If rssefaz.Fields("xml_nfe").Value.ToString <> "" Then
                xmldisponivel = "XML SIM"
                visualizar = "Visualizar"
            Else
                xmldisponivel = ""
                visualizar = ""
            End If

            If chkvlrboleto.Checked = True Then
                dgvpesquisa.Rows.Add(dr("Nome_emi"), dr("Numero_nfe"), dr("Valor_nfe"), dr("Data_emi"), _
                                    xmldisponivel, visualizar, dr("chave_nfe"), consulta_valor_boleto(dr("chave_nfe")), dr("Status_nfe"))
            Else
                dgvpesquisa.Rows.Add(dr("Nome_emi"), dr("Numero_nfe"), dr("Valor_nfe"), dr("Data_emi"), _
                                   xmldisponivel, visualizar, dr("chave_nfe"), "", dr("Status_nfe"))
            End If
            loc = True
        End While

        If loc = False Then
            MsgBox("Nenhum Registro encontrado !", MsgBoxStyle.Exclamation, "Atenção")
        End If


        oConn.Close()

        lblinfo.Text = "Total de Notas Fiscais Encontradas: " & dgvpesquisa.Rows.Count
    End Sub


    Private Function consulta_valor_boleto(ByVal chave_n As String) As String

        Dim rssalvam As New ADODB.Recordset
        Dim ret As String = "0,00"

        conectar_boleto()

        rssalvam.Open("select * from fatura_codbarra where chave_nfe = '" & chave_n & "'", conexao.cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic)

        Do While rssalvam.EOF = False

            ret = CDec(ret) + CDec(rssalvam.Fields("valor_dup").Value)

            rssalvam.MoveNext()
        Loop

        conexao.cn.Close()


        Return Format(CDec(ret), "###0.00")

    End Function

    Private Sub frmpesquisarnfe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '  Psenha.Visible = False
        '   txtsenha.Focus()
        '   Panel1.Enabled = False


        If cbotipo.Text = "" Then
            cbotipo.SelectedIndex = 0
        End If
        cbostatusnfe.SelectedIndex = 0

        txtpesquisa.Focus()
    End Sub

    Private Sub dgvpesquisa_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvpesquisa.CellContentClick


        Try

            If dgvpesquisa.CurrentCell.Value = "XML SIM" Or dgvpesquisa.CurrentCell.Value = "Visualizar" Then

                Dim rs As New ADODB.Recordset
                Dim saveFileDialog1 As New SaveFileDialog()


                conectar_mysqlDFe()

                Dim SQLcommand As MySqlCommand
                SQLcommand = oConn.CreateCommand
                SQLcommand.CommandText = "select * from NFe where Chave_nfe = '" & dgvpesquisa.CurrentRow.Cells(6).Value & "'"
                dr = SQLcommand.ExecuteReader()

                While (dr.Read())

                    If dr("xml_nfe").ToString <> "" Then
                        bytePicData = dr("xml_nfe")

                        Try
                            xmldoc.LoadXml(Encoding.UTF8.GetString(bytePicData))
                        Catch ex As Exception
                            MsgBox(Encoding.UTF8.GetString(bytePicData))
                            xmldoc.LoadXml(Encoding.UTF8.GetString(bytePicData).Remove(0, 38))
                        End Try

                        If dgvpesquisa.CurrentCell.Value = "XML SIM" Then

                            saveFileDialog1.FileName = dgvpesquisa.CurrentRow.Cells(6).Value + ".xml"

                            If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                                xmldoc.Save(saveFileDialog1.FileName)
                                MsgBox("Arquivo XML NF-e salvo na pasta Informada !", MsgBoxStyle.Information, "Aviso !")
                            End If
                        End If

                        If dgvpesquisa.CurrentCell.Value = "Visualizar" Then
                            frmxml.carregaxml_pesquisa()
                            Me.Close()
                        End If
                    End If

                End While

              oConn.Close()

            End If



            If dgvpesquisa.CurrentCell.Value = "Importar" Then

                Dim chavevalida As String
                Dim dataemi As Date
                Dim nome As String, vnf As String, ie As String, tpnf As String, cnpj_emi As String, data_auto As String, protocolo As String
                Dim dialogo As New OpenFileDialog

                dialogo.Title = "Selecione o Arquivo XML para salvar no Banco"
                dialogo.Filter = "Arquivos (.xml)|*.xml"

                If dialogo.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    ''''''''''''''''Abre o arquivo xml para validacao deste''''''''''''''''''''
                    xmldoc.Load(dialogo.FileName)
                    Dim xmllist As System.Xml.XmlNodeList
                    Dim xmlnode As System.Xml.XmlNode
                    xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
                    xmllist = xmldoc.SelectNodes("//bk:NFe", xmlnsManager)

                    For Each xmlnode In xmllist
                        chavevalida = xmlnode("infNFe").Attributes.ItemOf("Id").InnerText.Substring(3, 44)
                    Next





                    '''''''''''''''''''''''''''''''''''''''Rotina para captura os dados resumo da NFe"""""""""""""""""""""""""""'
                    Try

                        xmllist = xmldoc.GetElementsByTagName("infNFe")

                        For Each xmlnode In xmllist

                            If Not xmlnode("ide")("dhEmi") Is Nothing Then
                                dataemi = xmlnode("ide")("dhEmi").InnerText
                            End If

                            If Not xmlnode("emit")("xNome") Is Nothing Then
                                nome = xmlnode("emit")("xNome").InnerText
                            End If

                            If Not xmlnode("emit")("CNPJ") Is Nothing Then
                                nome = xmlnode("emit")("CNPJ").InnerText
                            End If

                            If Not xmlnode("dest")("CNPJ") Is Nothing Then
                                cnpj_emi = xmlnode("dest")("CNPJ").InnerText
                            End If
                            If Not xmlnode("dest")("CPF") Is Nothing Then
                                cnpj_emi = xmlnode("dest")("CPF").InnerText
                            End If

                            If Not xmlnode("total")("ICMSTot")("vNF") Is Nothing Then
                                vnf = xmlnode("total")("ICMSTot")("vNF").InnerText
                            End If

                            If Not xmlnode("emit")("IE") Is Nothing Then
                                ie = xmlnode("emit")("IE").InnerText
                            End If

                            If Not xmlnode("ide")("tpNF") Is Nothing Then

                                If xmlnode("ide")("tpNF").InnerText = "0" Then
                                    tpnf = "0-NFe Entrada"
                                End If
                                If xmlnode("ide")("tpNF").InnerText = "1" Then
                                    tpnf = "1-NFe Saida"
                                End If

                            End If

                        Next


                        xmllist = xmldoc.GetElementsByTagName("protNFe")

                        For Each xmlnode In xmllist

                            If Not xmlnode("infProt")("dhRecbto") Is Nothing Then
                                data_auto = xmlnode("infProt")("dhRecbto").InnerText
                            End If
                            If Not xmlnode("infProt")("nProt") Is Nothing Then
                                protocolo = xmlnode("infProt")("nProt").InnerText
                            End If

                        Next
                    Catch ex As Exception

                    End Try
                    ''''''''''''''''''''''''''#######################"""""""""""""""""""""""""""""''

                   

                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvpesquisa_SelectionChanged(sender As Object, e As EventArgs) Handles dgvpesquisa.SelectionChanged
        TextBox1.Text = dgvpesquisa.CurrentRow.Cells(6).Value
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim rs As New ADODB.Recordset
        conectar_DFe()
        '''''''''''''''''nova rotina de arquivo xml'''''''''''''''''''''''''''
        rs.Open("update NFe_consultadaDFe set status_nfe = 'NF-e autorizada' where status_nfe = 'NF-e autorizado'", cn_DFe, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic)

        cn_DFe.Close()

        MsgBox("executado")
    End Sub

    Private Sub mskdatafinal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles mskdatafinal.KeyPress
        If (e.KeyChar = Chr(13)) Then
            lblinfo.Visible = True
            lblinfo.Text = "Aguarde, o sistema esta pesquisando a NF-e Informada..."
            lblinfo.Refresh()
            pesquisar_sql()
        End If
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        '''''''''''''''''''''''''''''''''''''''Rotina para captura os dados resumo da NFe"""""""""""""""""""""""""""'
        Try

            Dim dialogo As New OpenFileDialog

            dialogo.Title = "Selecione o Arquivo XML para salvar no Banco"
            dialogo.Filter = "Arquivos (.xml)|*.xml"

            If dialogo.ShowDialog() = Windows.Forms.DialogResult.OK Then

                ''''''''''''''''Abre o arquivo xml para validacao deste''''''''''''''''''''
                xmldoc.Load(dialogo.FileName)
                Dim xmllist As System.Xml.XmlNodeList
                Dim xmlnode As System.Xml.XmlNode

                xmllist = xmldoc.GetElementsByTagName("infNFe")

                For Each xmlnode In xmllist

                    If Not xmlnode("ide")("dhEmi") Is Nothing Then
                        MsgBox(xmlnode("ide")("dhEmi").InnerText)
                        '  dgvnotas.CurrentRow.Cells(3).Value = dataemi.ToString("dd/MM/yyyy")
                    End If

                    If Not xmlnode("emit")("xNome") Is Nothing Then
                        MsgBox(xmlnode("emit")("xNome").InnerText)
                    End If

                    If Not xmlnode("dest")("CNPJ") Is Nothing Then
                        MsgBox(xmlnode("dest")("CNPJ").InnerText)
                    End If

                    If Not xmlnode("total")("ICMSTot")("vNF") Is Nothing Then
                        MsgBox(xmlnode("total")("ICMSTot")("vNF").InnerText)
                    End If

                    If Not xmlnode("emit")("IE") Is Nothing Then
                        MsgBox(xmlnode("emit")("IE").InnerText)
                    End If

                    If Not xmlnode("ide")("tpNF") Is Nothing Then

                        If xmlnode("ide")("tpNF").InnerText = "0" Then
                            '  tpnf = "0-NFe Entrada"
                        End If
                        If xmlnode("ide")("tpNF").InnerText = "1" Then
                            '   tpnf = "1-NFe Saida"
                        End If

                    End If

                Next


                xmllist = xmldoc.GetElementsByTagName("protNFe")

                For Each xmlnode In xmllist

                    If Not xmlnode("infProt")("dhRecbto") Is Nothing Then
                        MsgBox(xmlnode("infProt")("dhRecbto").InnerText)
                    End If

                Next

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub txtpesquisa_TextChanged(sender As Object, e As EventArgs) Handles txtpesquisa.TextChanged

    End Sub
End Class
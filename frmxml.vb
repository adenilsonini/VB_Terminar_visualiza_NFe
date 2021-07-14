Imports System.Security.Cryptography.X509Certificates
Imports System.Xml
Imports System.Text
Imports System.Threading
Imports System.Drawing.Text
Imports System.IO

Imports System.Xml.Linq
Imports System.Data.OleDb
Imports System.Globalization
Imports MySql.Data.MySqlClient
Imports System.Reflection

Public Class frmxml


    Public certificado As X509Certificate2

    Dim erro_validacao As Boolean = False
    Dim inclusao As Boolean = False

    Dim ind As Integer
    Dim rotina1 As Thread, rotina2 As Thread, rotina3 As Thread


    Dim data_emissao As Date, data_saida As Date, data_prot As Date

    Public valortotaltrib As String, hora As String, uf As String, tiponota As String, Recibonfe As String, xmlnfe As String
    Public versao As String = "Visualizador Nota Fiscal Eletronica"

    Dim verifica As Boolean, reparaxml_auto As Boolean, contigencia As Boolean

    Dim rs As New ADODB.Recordset

    Public xmldoc As New XmlDocument()
    Dim xmlnsManager As New XmlNamespaceManager(xmldoc.NameTable)






#Region "''''Rotina Cria o Código de barra da nfe"



    Private Function carregalogo(ByVal fileName As String) As Byte()

        'Método para carregar uma imagem do disco e retorná-la como um byteStream
        Dim fs As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim br As BinaryReader = New BinaryReader(fs)
        Return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)))

    End Function

    Private Function carregaImagem(ByVal fileName As String) As Byte()

        'Método para carregar uma imagem do disco e retorná-la como um byteStream
        Dim fs As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim br As BinaryReader = New BinaryReader(fs)
        Return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)))

    End Function
#End Region


#Region "''''Rotina carrega os dados da NFe"

    Private Sub chave()
        Dim xmllist As System.Xml.XmlNodeList
        Dim xmlnode As System.Xml.XmlNode
        xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
        limpa_nfe()


        xmllist = xmldoc.SelectNodes("//bk:infProt", xmlnsManager)

        For Each xmlnode In xmllist
            txtcoduf.Text = xmlnode("chNFe").InnerText.Substring(0, 2)
            mskchave.Text = xmlnode("chNFe").InnerText
            data_prot = xmlnode("dhRecbto").InnerText
            txtprotocolo.Text = xmlnode("nProt").InnerText & "  " & data_prot
            msknprot.Text = xmlnode("nProt").InnerText
            txtxmotivo.Text = xmlnode("xMotivo").InnerText

            '----------gera o codigo de barra para chave de acesso
            ' codBarImagem.Image.Save(Application.StartupPath & "\codbarranfe.jpg")

            '  geracodbarra()
            '  salvacodbarra()
            mskchave.Mask = "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000"
            lblnfe.Visible = True
            lblnfe.Text = "NF-e Autorizada"
            lblnfe.BackColor = Color.Blue
        Next


        lblcanc.Visible = False
        lblmotcanc.Visible = False
        txtprotocolocanc.Visible = False
        txtxmotivocanc.Visible = False
    End Sub

    Private Sub dadosnfe()

        Dim xmllist As System.Xml.XmlNodeList
        Dim xmlnode As System.Xml.XmlNode, xmlnode2 As System.Xml.XmlNode
        xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
        xmllist = xmldoc.SelectNodes("//bk:NFe", xmlnsManager)

        '---------------------------Abre dados do fornecedor-------------------------------------------------
        limpar_abas()

        For Each xmlnode In xmllist
            System.Diagnostics.Debug.WriteLine(xmlnode.InnerText)
            txtuffor.Text = xmlnode("infNFe")("emit")("enderEmit")("UF").InnerText

            txtpainel.Text = xmlnode("infNFe")("emit")("xNome").InnerText

            txtnomefor.Text = xmlnode("infNFe")("emit")("xNome").InnerText

            If Not xmlnode("infNFe")("emit")("CNPJ") Is Nothing Then

                mskcnpjfor.Mask = "00,000,000/0000-00"
                mskcnpjfor.Text = xmlnode("infNFe")("emit")("CNPJ").InnerText

            End If

            If Not xmlnode("infNFe")("emit")("CPF") Is Nothing Then

                mskcnpjfor.Mask = "000,000,000-00"
                mskcnpjfor.Text = xmlnode("infNFe")("emit")("CPF").InnerText

            End If

            txtendfor.Text = xmlnode("infNFe")("emit")("enderEmit")("xLgr").InnerText
            txtnumerofor.Text = xmlnode("infNFe")("emit")("enderEmit")("nro").InnerText
            txtiefor.Text = xmlnode("infNFe")("emit")("IE").InnerText
            txtbairrofor.Text = xmlnode("infNFe")("emit")("enderEmit")("xBairro").InnerText
            txtcidadefor.Text = xmlnode("infNFe")("emit")("enderEmit")("xMun").InnerText
            txtregime.Text = xmlnode("infNFe")("emit")("CRT").InnerText



            mskchave.Text = xmlnode("infNFe").Attributes.ItemOf("Id").InnerText.Substring(3, 44)
            '  lblversao.Text = "A Versão XML: " & xmlnode("infNFe").Attributes.ItemOf("versao").InnerText


            If txtcoduf.Text = "" Then
                txtcoduf.Text = mskchave.Text.Substring(0, 2)
            End If

            If Not xmlnode("infNFe")("emit")("IEST") Is Nothing Then
                txtinscst.Text = xmlnode("infNFe")("emit")("IEST").InnerText
            End If
            If Not xmlnode("infNFe")("emit")("IM") Is Nothing Then
                txtim.Text = xmlnode("infNFe")("emit")("IM").InnerText
            End If
            If Not xmlnode("infNFe")("emit")("CNAE") Is Nothing Then
                txtcnae.Text = xmlnode("infNFe")("emit")("CNAE").InnerText
            End If
            If Not xmlnode("infNFe")("emit")("xFant") Is Nothing Then
                txtnomefanfor.Text = xmlnode("infNFe")("emit")("xFant").InnerText
            End If
            If Not xmlnode("infNFe")("emit")("enderEmit")("xPais") Is Nothing Then
                txtpaisfor.Text = xmlnode("infNFe")("emit")("enderEmit")("xPais").InnerText
            End If
            If Not xmlnode("infNFe")("ide")("idDest") Is Nothing Then
                txtidDest.Text = xmlnode("infNFe")("ide")("idDest").InnerText
            End If
            If Not xmlnode("infNFe")("emit")("enderEmit")("cMun") Is Nothing Then
                txtcmunfor.Text = xmlnode("infNFe")("emit")("enderEmit")("cMun").InnerText
            End If
            If Not xmlnode("infNFe")("emit")("enderEmit")("cPais") Is Nothing Then
                txtcodpaisfor.Text = xmlnode("infNFe")("emit")("enderEmit")("cPais").InnerText
            End If
            mskcepfor.Text = xmlnode("infNFe")("emit")("enderEmit")("CEP").InnerText

            If Not xmlnode("infNFe")("emit")("enderEmit")("fone") Is Nothing Then
                mskfonefor.Text = Format(Convert.ToDouble(xmlnode("infNFe")("emit")("enderEmit")("fone").InnerText), "##########")
                mskfonefor.Mask = "(00) 0000-0000"
            End If


            If mskcnpjfor.Text = "10.962.965/0001-19" Then
                txtemailfor.Text = "Supermercadodoka@gmail.com"
            End If
            '---------------------------------------fecha dados do fornecedor-----------------------------------------------


            '---------------------------------------Abre dados do Destinatario---------------------------------------------

            If Not xmlnode("infNFe")("dest")("xFant") Is Nothing Then
                txtnomefandest.Text = xmlnode("infNFe")("dest")("xFant").InnerText
            End If
            If Not xmlnode("infNFe")("dest")("CNPJ") Is Nothing Then
                mskcnpjdest.Mask = "00,000,000/0000-00"
                mskcnpjdest.Text = xmlnode("infNFe")("dest")("CNPJ").InnerText
            End If

            If Not xmlnode("infNFe")("dest")("CPF") Is Nothing Then
                mskcnpjdest.Mask = "000,000,000-00"
                mskcnpjdest.Text = xmlnode("infNFe")("dest")("CPF").InnerText
            End If
            If Not xmlnode("infNFe")("dest")("enderDest")("xPais") Is Nothing Then
                txtpaisdest.Text = xmlnode("infNFe")("dest")("enderDest")("xPais").InnerText
            End If
            If Not xmlnode("infNFe")("dest")("email") Is Nothing Then
                txtemaildest.Text = xmlnode("infNFe")("dest")("email").InnerText
            End If
            If Not xmlnode("infNFe")("dest")("enderDest")("cMun") Is Nothing Then
                txtcmundest.Text = xmlnode("infNFe")("dest")("enderDest")("cMun").InnerText
            End If
            If Not xmlnode("infNFe")("dest")("enderDest")("cPais") Is Nothing Then
                txtcodpaisdest.Text = xmlnode("infNFe")("dest")("enderDest")("cPais").InnerText
            End If

            If Not xmlnode("infNFe")("dest")("enderDest")("fone") Is Nothing Then
                mskfonedest.Text = Format(Convert.ToDouble(xmlnode("infNFe")("dest")("enderDest")("fone").InnerText), "##########")
                mskfonedest.Mask = "(00) 0000-0000"
            End If

            txtnomedest.Text = xmlnode("infNFe")("dest")("xNome").InnerText

            txtenddest.Text = xmlnode("infNFe")("dest")("enderDest")("xLgr").InnerText
            txtnumerodest.Text = xmlnode("infNFe")("dest")("enderDest")("nro").InnerText

            If Not xmlnode("infNFe")("dest")("IE") Is Nothing Then
                txtiedest.Text = xmlnode("infNFe")("dest")("IE").InnerText
            End If

            If Not xmlnode("infNFe")("dest")("indIEDest") Is Nothing Then
                txtindIEDest.Text = xmlnode("infNFe")("dest")("indIEDest").InnerText
            End If

            txtbairrodest.Text = xmlnode("infNFe")("dest")("enderDest")("xBairro").InnerText
            txtcidadedest.Text = xmlnode("infNFe")("dest")("enderDest")("xMun").InnerText

            If Not xmlnode("infNFe")("dest")("enderDest")("CEP") Is Nothing Then
                mskcepdest.Text = xmlnode("infNFe")("dest")("enderDest")("CEP").InnerText
                mskcepdest.Mask = "00,000-000"
            End If
            txtufdest.Text = xmlnode("infNFe")("dest")("enderDest")("UF").InnerText

            ' mskcnpjdest.Mask = "00,000,000/0000-00"
            '--------------------------------fecha dados do destinatario---------------------------------------------


            '--------------------------------Abre dados do Cabeçalho da Nfe--------------------------------------------

            If Not xmlnode("infNFe")("ide")("dEmi") Is Nothing Then
                '  data_emissao = xmlnode("infNFe")("ide")("dhEmi").InnerText
                txtdataemissao.Text = Convert.ToDateTime(xmlnode("infNFe")("ide")("dEmi").InnerText).ToString("dd/MM/yyyy HH:mm:ss")
            End If

            If Not xmlnode("infNFe")("ide")("dhEmi") Is Nothing Then
                '  data_emissao = xmlnode("infNFe")("ide")("dhEmi").InnerText
                txtdataemissao.Text = Convert.ToDateTime(xmlnode("infNFe")("ide")("dhEmi").InnerText).ToString("dd/MM/yyyy HH:mm:ss")
            End If


            txtnumeronfe.Text = xmlnode("infNFe")("ide")("nNF").InnerText
            txtserienfe.Text = xmlnode("infNFe")("ide")("serie").InnerText
            txtmodelonfe.Text = xmlnode("infNFe")("ide")("mod").InnerText
            cbonat.Text = xmlnode("infNFe")("ide")("natOp").InnerText

            txttpNF.Text = xmlnode("infNFe")("ide")("tpNF").InnerText
            txttpEmis.Text = xmlnode("infNFe")("ide")("tpEmis").InnerText


            If xmlnode("infNFe")("ide")("finNFe").InnerText.Substring(0, 1) = "1" Then
                cbofinNFe.Text = "1-NFe normal"
            End If
            If xmlnode("infNFe")("ide")("finNFe").InnerText.Substring(0, 1) = "2" Then
                cbofinNFe.Text = "2-NFe complementar"
            End If
            If xmlnode("infNFe")("ide")("finNFe").InnerText.Substring(0, 1) = "3" Then
                cbofinNFe.Text = "3-NFe de ajuste"
            End If
            If xmlnode("infNFe")("ide")("finNFe").InnerText.Substring(0, 1) = "4" Then
                cbofinNFe.Text = "4-Devolução/Retorno"
            End If

            If Not xmlnode("infNFe")("ide")("indPag") Is Nothing Then
                If xmlnode("infNFe")("ide")("indPag").InnerText.Substring(0, 1) = "0" Then
                    cboindPag.Text = "0-Pagamento à vista"
                End If
                If xmlnode("infNFe")("ide")("indPag").InnerText.Substring(0, 1) = "1" Then
                    cboindPag.Text = "1-Pagamento à prazo"
                End If
                If xmlnode("infNFe")("ide")("indPag").InnerText.Substring(0, 1) = "2" Then
                    cboindPag.Text = "2-Outros"
                End If
            End If



            txttpAmb.Text = xmlnode("infNFe")("ide")("tpAmb").InnerText
            txtprocEmi.Text = xmlnode("infNFe")("ide")("procEmi").InnerText
            txtverProc.Text = xmlnode("infNFe")("ide")("verProc").InnerText

            '''''''''''''''''''Ler tag NFref'''''''''''''''''''''''''''''''''''''''''
            If Not xmlnode("infNFe")("ide")("NFref") Is Nothing Then

                If Not xmlnode("infNFe")("ide")("NFref")("refNFe") Is Nothing Then
                    mskchvdevolucao.Text = xmlnode("infNFe")("ide")("NFref")("refNFe").InnerText
                End If

                If Not xmlnode("infNFe")("ide")("NFref")("refECF") Is Nothing Then
                    TabControl_nfe.Controls.Add(aba_Cobertura_ECF)

                    For Each InnerNode2 As XmlNode In xmlnode("infNFe")("ide")
                        If InnerNode2.Name = "NFref" Then
                            For Each InnerNode3 As XmlNode In InnerNode2

                                dgvecf.Rows.Add(InnerNode3.ChildNodes(0).ChildNodes(0).InnerText, _
                                               InnerNode3.ChildNodes(1).ChildNodes(0).InnerText, _
                                               InnerNode3.ChildNodes(2).ChildNodes(0).InnerText)

                            Next
                        End If
                    Next

                End If
            End If


            If Not xmlnode("infNFe")("ide")("indFinal") Is Nothing Then
                cboindFinal.SelectedIndex = xmlnode("infNFe")("ide")("indFinal").InnerText
            End If

            If Not xmlnode("infNFe")("ide")("indPres") Is Nothing Then
                If xmlnode("infNFe")("ide")("indPres").InnerText = "9" Then
                    cboindPres.Text = "9-Não presencial, outros"
                ElseIf xmlnode("infNFe")("ide")("indPres").InnerText = "0" Then
                    cboindPres.Text = "0-Não se aplica (ex.: Nota Fiscal complementar ou de ajuste)"
                ElseIf xmlnode("infNFe")("ide")("indPres").InnerText = "1" Then
                    cboindPres.Text = "1-Operação presencial"
                ElseIf xmlnode("infNFe")("ide")("indPres").InnerText = "2" Then
                    cboindPres.Text = "2-Não presencial, internet"
                ElseIf xmlnode("infNFe")("ide")("indPres").InnerText = "3" Then
                    cboindPres.Text = "3-Não presencial, teleatendimento"
                ElseIf xmlnode("infNFe")("ide")("indPres").InnerText = "4" Then
                    cboindPres.Text = "4-NFC-e entrega em domicílio"
                    ' Else
                    ' cboindPres.SelectedIndex = xmlnode("infNFe")("ide")("indPres").InnerText
                End If
            End If

            If Not xmlnode("infNFe")("ide")("dhSaiEnt") Is Nothing Then

                txtdsaida.Text = Convert.ToDateTime(xmlnode("infNFe")("ide")("dhSaiEnt").InnerText).ToString("dd/MM/yyyy HH:mm:ss")

            End If

            If Not xmlnode("infNFe")("ide")("dSaiEnt") Is Nothing Then

                txtdsaida.Text = Convert.ToDateTime(xmlnode("infNFe")("ide")("dSaiEnt").InnerText).ToString("dd/MM/yyyy HH:mm:ss")

            End If



            If Not xmlnode("infNFe")("infAdic") Is Nothing Then

                If Not xmlnode("infNFe")("infAdic")("infAdFisco") Is Nothing Then
                    '  txtinfcompl.Width = 536
                    ' lbladifisco.Visible = True
                    '  txtadifisco.Visible = True
                    txtadifisco.Text = xmlnode("infNFe")("infAdic")("infAdFisco").InnerText
                End If

                If Not xmlnode("infNFe")("infAdic")("infCpl") Is Nothing Then
                    '  If txtadifisco.Text = "" Then
                    'lbladifisco.Visible = False
                    '   txtadifisco.Visible = False
                    '     txtinfcompl.Width = 967
                    ' End If
                    txtinfcompl.Text = xmlnode("infNFe")("infAdic")("infCpl").InnerText
                End If

            End If

            '----------------------------------Fecha dados do cabeçalho da nfe-------------------------------------------


            '----------------------------------Abre aba de Valores da Nfe------------------------------------------------
            uf = xmlnode("infNFe")("dest")("enderDest")("UF").InnerText
            txtvlrtotalbaseicms_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vBC").InnerText.Replace(".", ",")
            txtvlrtotalicms_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vICMS").InnerText.Replace(".", ",")
            txtvlrtotalbasest_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vBCST").InnerText.Replace(".", ",")
            txtvlrtotalst_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vST").InnerText.Replace(".", ",")
            txtvlrtotaldesc_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vDesc").InnerText.Replace(".", ",")
            txtvlrtotalfrete_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vFrete").InnerText.Replace(".", ",")
            txtvlrtotalpis_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vPIS").InnerText.Replace(".", ",")
            txtvlrtotalcofins_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vCOFINS").InnerText.Replace(".", ",")
            txtvrltotalipi_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vIPI").InnerText.Replace(".", ",")
            txtvlrtotalprod_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vProd").InnerText.Replace(".", ",")
            txtvlrtotalseg_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vSeg").InnerText.Replace(".", ",")
            txtvlrtotaloutras_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vOutro").InnerText.Replace(".", ",")
            txtvlrtotal_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vNF").InnerText.Replace(".", ",")
            txtvlrtotal_nfe.Text = xmlnode("infNFe")("total")("ICMSTot")("vNF").InnerText.Replace(".", ",")
            If Not xmlnode("infNFe")("total")("ICMSTot")("vTotTrib") Is Nothing Then
                txtvlrtotalaproxtrib.Text = xmlnode("infNFe")("total")("ICMSTot")("vTotTrib").InnerText.Replace(".", ",")
            End If
            If Not xmlnode("infNFe")("total")("ICMSTot")("vFCPUFDest") Is Nothing Then
                txtvlrtotalvFCPUFDest.Text = xmlnode("infNFe")("total")("ICMSTot")("vFCPUFDest").InnerText.Replace(".", ",")
            End If
            If Not xmlnode("infNFe")("total")("ICMSTot")("vICMSUFDest") Is Nothing Then
                txtvlrtotalvICMSUFDest.Text = xmlnode("infNFe")("total")("ICMSTot")("vICMSUFDest").InnerText.Replace(".", ",")
            End If
            If Not xmlnode("infNFe")("total")("ICMSTot")("vICMSUFRemet") Is Nothing Then
                txtvlrtotalvICMSUFRemet.Text = xmlnode("infNFe")("total")("ICMSTot")("vICMSUFRemet").InnerText.Replace(".", ",")
            End If

            '----------------------------------fecha aba Valores da NFe------------------------------------------------------
        Next
        ' Me.Width = 1010
        ' Me.Height = 630
    End Sub

    Private Sub fatura()
        Dim parcela As Integer
        Dim datavenc As Date
        Dim nduplicata As Integer = 0


        Dim xmllist5 As System.Xml.XmlNodeList
        Dim xmlnode5 As System.Xml.XmlNode
        xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
        xmllist5 = xmldoc.SelectNodes("//bk:NFe", xmlnsManager)
        Try



            For Each xmlnode5 In xmllist5

                If Not xmlnode5("infNFe")("cobr") Is Nothing Then
                    If Not xmlnode5("infNFe")("cobr")("dup") Is Nothing Then
                        Dim xmllist As System.Xml.XmlNodeList
                        Dim xmlnode As System.Xml.XmlNode
                        xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
                        xmllist = xmldoc.SelectNodes("//bk:dup", xmlnsManager)

                        parcela = "0"
                        For Each xmlnode In xmllist

                            parcela = parcela + 1
                            datavenc = xmlnode("dVenc").InnerText

                            If Not xmlnode("nDup") Is Nothing Then
                                dgvfatura.Rows.Add(parcela, xmlnode("nDup").InnerText.Replace(".", ","), xmlnode("vDup").InnerText.Replace(".", ","), datavenc.ToString("dd/MM/yyyy"))


                            Else
                                dgvfatura.Rows.Add(parcela, parcela, xmlnode("vDup").InnerText.Replace(".", ","), datavenc.ToString("dd/MM/yyyy"))

                            End If

                        Next
                    End If
                End If
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub dadostrasp()
        Dim xmllist As System.Xml.XmlNodeList
        Dim xmlnode As System.Xml.XmlNode
        xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
        xmllist = xmldoc.SelectNodes("//bk:NFe", xmlnsManager)

        For Each xmlnode In xmllist
            System.Diagnostics.Debug.WriteLine(xmlnode.InnerText)

            If Not xmlnode("infNFe")("transp")("modFrete") Is Nothing Then
                txtfretecont.Text = xmlnode("infNFe")("transp")("modFrete").InnerText
            End If

            If Not xmlnode("infNFe")("transp")("vol") Is Nothing Then
                If Not xmlnode("infNFe")("transp")("vol")("qVol") Is Nothing Then
                    txtQvol.Text = xmlnode("infNFe")("transp")("vol")("qVol").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("vol")("esp") Is Nothing Then
                    txtesp.Text = xmlnode("infNFe")("transp")("vol")("esp").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("vol")("marca") Is Nothing Then
                    txtmarca.Text = xmlnode("infNFe")("transp")("vol")("marca").InnerText
                End If

                If Not xmlnode("infNFe")("transp")("vol")("nVol") Is Nothing Then
                    txtnvol.Text = xmlnode("infNFe")("transp")("vol")("nVol").InnerText
                End If

                If Not xmlnode("infNFe")("transp")("vol")("pesoB") Is Nothing Then
                    txtpesob.Text = xmlnode("infNFe")("transp")("vol")("pesoB").InnerText.Replace(".", ",")
                End If
                If Not xmlnode("infNFe")("transp")("vol")("pesoL") Is Nothing Then
                    txtpesol.Text = xmlnode("infNFe")("transp")("vol")("pesoL").InnerText.Replace(".", ",")
                End If
            End If

            If Not xmlnode("infNFe")("transp")("transporta") Is Nothing Then

                If Not xmlnode("infNFe")("transp")("transporta")("CNPJ") Is Nothing Then
                    mskcnpjtrp.Mask = "00,000,000/0000-00"
                    mskcnpjtrp.Text = xmlnode("infNFe")("transp")("transporta")("CNPJ").InnerText

                End If
                If Not xmlnode("infNFe")("transp")("transporta")("CPF") Is Nothing Then
                    mskcnpjtrp.Mask = "000,000,000-00"
                    mskcnpjtrp.Text = xmlnode("infNFe")("transp")("transporta")("CPF").InnerText

                End If
                If Not xmlnode("infNFe")("transp")("transporta")("xNome") Is Nothing Then
                    txtnometrp.Text = xmlnode("infNFe")("transp")("transporta")("xNome").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("transporta")("Bairro") Is Nothing Then
                    txtbairrotrp.Text = xmlnode("infNFe")("transp")("transporta")("Bairro").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("transporta")("xEnder") Is Nothing Then
                    txtendtrp.Text = xmlnode("infNFe")("transp")("transporta")("xEnder").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("transporta")("IE") Is Nothing Then
                    txtietrp.Text = xmlnode("infNFe")("transp")("transporta")("IE").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("transporta")("xMun") Is Nothing Then
                    txtcidadetrp.Text = xmlnode("infNFe")("transp")("transporta")("xMun").InnerText
                End If
                If Not xmlnode("infNFe")("transp")("transporta")("UF") Is Nothing Then
                    txtuftrp.Text = xmlnode("infNFe")("transp")("transporta")("UF").InnerText
                End If


                If Not xmlnode("infNFe")("transp")("veicTransp") Is Nothing Then
                    If Not xmlnode("infNFe")("transp")("veicTransp")("placa") Is Nothing Then
                        txtplacatrp.Text = xmlnode("infNFe")("transp")("veicTransp")("placa").InnerText
                    End If

                    If Not xmlnode("infNFe")("transp")("veicTransp")("UF") Is Nothing Then
                        txtufplacatrp.Text = xmlnode("infNFe")("transp")("veicTransp")("UF").InnerText
                    End If

                    If Not xmlnode("infNFe")("transp")("veicTransp")("RNTC") Is Nothing Then
                        txtantt.Text = xmlnode("infNFe")("transp")("veicTransp")("RNTC").InnerText
                    End If
                End If

            End If
        Next
    End Sub

    Private Sub itens_nfe()
        Dim i As Integer
        Dim datavmed As Date
        xmlnsManager.AddNamespace("bk", "http://www.portalfiscal.inf.br/nfe")
        Dim xmlList As System.Xml.XmlNodeList
        Dim xmlnode As System.Xml.XmlNode
        xmlList = xmldoc.SelectNodes("//bk:det", xmlnsManager)

        dgvgrid_xml.Rows.Clear()



        chklote.Checked = False

        For Each xmlnode In xmlList
            limpar_itens()

            Try
                txtcodfor_item.Text = xmlnode("prod")("cProd").InnerText.Replace("-", "")

                txtcodfor_item.Text = xmlnode("prod")("cProd").InnerText.TrimStart("0")

            Catch ex As Exception
                txtcodfor_item.Text = xmlnode("prod")("cProd").InnerText
            End Try

            txtnr.Text = xmlnode.Attributes.ItemOf("nItem").InnerText

            txtdesc_item.Text = xmlnode("prod")("xProd").InnerText
            txtvlrunit_item.Text = xmlnode("prod")("vUnCom").InnerText
            txtvlrunit_item.Text = Format(CDec(txtvlrunit_item.Text.Replace(".", ",")), "#,##0.0000")
            txtqtde_item.Text = xmlnode("prod")("qCom").InnerText
            txtqtdecx_item.Text = "1"
            txtvlrtotal_item.Text = xmlnode("prod")("vProd").InnerText.Replace(".", ",")
            txtncm_item.Text = xmlnode("prod")("NCM").InnerText

            If Not xmlnode("prod")("CEST") Is Nothing Then
                txtcest_item.Text = xmlnode("prod")("CEST").InnerText
            End If

            If Not xmlnode("prod")("EXTIPI") Is Nothing Then
                txtncmex_item.Text = xmlnode("prod")("EXTIPI").InnerText
            End If

            txtun_item.Text = xmlnode("prod")("uCom").InnerText

            If optean.Checked = True Then
                txtcodbarra_item.Text = xmlnode("prod")("cEAN").InnerText
            Else
                txtcodbarra_item.Text = xmlnode("prod")("cEANTrib").InnerText
            End If
            txtcfop_item.Text = xmlnode("prod")("CFOP").InnerText

            If xmlnode("imposto")("ICMS").ChildNodes.Item(0).ChildNodes.Item(1).Name = "CSOSN" Then
                lbltagcst.Text = "CSOSN"

                lbltagcst.Location = New Point(177, 31)
            Else
                lbltagcst.Text = "CST:"
                lbltagcst.Location = New Point(193, 31)
            End If

            txtcst_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("orig").InnerText + xmlnode("imposto")("ICMS").ChildNodes.Item(0).ChildNodes.Item(1).InnerText

            If Not xmlnode("prod")("vDesc") Is Nothing Then
                txtvlrdesc_item.Text = xmlnode("prod")("vDesc").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("prod")("vFrete") Is Nothing Then
                txtvlrfrete_item.Text = xmlnode("prod")("vFrete").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("prod")("vOutro") Is Nothing Then
                txtvlroutros_item.Text = xmlnode("prod")("vOutro").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("prod")("med") Is Nothing Then
                chklote.Checked = True
                txtlote_item.Text = xmlnode("prod")("med")("nLote").InnerText
            End If

            If Not xmlnode("prod")("med") Is Nothing Then
                txtpmc_item.Text = xmlnode("prod")("med")("vPMC").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("prod")("med") Is Nothing Then
                datavmed = xmlnode("prod")("med")("dVal").InnerText
                mskdvenc_item.Text = datavmed.ToString("dd/MM/yyyy")
            End If

            If Not xmlnode("imposto")("vTotTrib") Is Nothing Then
                txtvlrtrib_item.Text = xmlnode("imposto")("vTotTrib").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMSST") Is Nothing Then
                txtvlrst_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMSST").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vFCPST") Is Nothing Then
                txtvlrfcpst_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vFCPST").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMSSTRet") Is Nothing Then
                txtvlrst_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMSSTRet").InnerText.Replace(".", ",")
                ' txtvlrstprod.Text = "0,00"
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBCSTRet") Is Nothing Then
                txtbasest_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBCSTRet").InnerText.Replace(".", ",")
                '  txtbasestprod.Text = "0,00"
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBCST") Is Nothing Then
                txtbasest_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBCST").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMSST") Is Nothing Then
                txtvlrst_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMSST").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBCST") Is Nothing Then
                txtbst.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBCST").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pICMSST") Is Nothing Then
                ' txtaliqst.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pICMSST").InnerText
                txtaliqst_item.Text = Format(CDec(xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pICMSST").InnerText.Replace(".", ",")), "#,##0.00")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pMVAST") Is Nothing Then
                txtmva_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pMVAST").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pICMS") Is Nothing Then
                '  txtaliqicmsprod.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pICMS").InnerText
                txtaliqicms_item.Text = Format(CDec(xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pICMS").InnerText.Replace(".", ",")), "#,##0.00")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMS") Is Nothing Then
                txtvlricms_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vICMS").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pCredSN") Is Nothing Then
                txtaliqicms_item.Text = Format(CDec(xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pCredSN").InnerText.Replace(".", ",")), "#,##0.00")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vCredICMSSN") Is Nothing Then
                txtvlricms_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vCredICMSSN").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBC") Is Nothing Then
                txtbaseicms_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("vBC").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pRedBC") Is Nothing Then
                txtbaseredicms_item.Text = xmlnode("imposto")("ICMS").ChildNodes.Item(0)("pRedBC").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("ICMS")("ICMS20") Is Nothing Then
                txtbaseredicms_item.Text = xmlnode("imposto")("ICMS")("ICMS20")("pRedBC").InnerText.Replace(".", ",")
            End If

            txtcstpis_item.Text = xmlnode("imposto")("PIS").ChildNodes.Item(0).ChildNodes.Item(0).InnerText
            txtcstcofins_item.Text = xmlnode("imposto")("COFINS").ChildNodes.Item(0).ChildNodes.Item(0).InnerText

            If Not xmlnode("imposto")("PIS").ChildNodes.Item(0).ChildNodes.Item(2) Is Nothing Then
                txtaliqpis_item.Text = xmlnode("imposto")("PIS").ChildNodes.Item(0).ChildNodes.Item(2).InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("PIS").ChildNodes.Item(0)("vPIS") Is Nothing Then
                txtvlrpis_item.Text = xmlnode("imposto")("PIS").ChildNodes.Item(0)("vPIS").InnerText.Replace(".", ",")
            End If


            If Not xmlnode("imposto")("PISST") Is Nothing Then
                txtaliqpis_item.Text = xmlnode("imposto")("PISST")("pPIS").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("PISST") Is Nothing Then
                txtvlrpis_item.Text = xmlnode("imposto")("PISST")("vPIS").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("COFINSST") Is Nothing Then
                txtaliqcofins_item.Text = xmlnode("imposto")("COFINSST")("pCOFINS").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("COFINSST") Is Nothing Then
                txtvlrcofins_item.Text = xmlnode("imposto")("COFINSST")("vCOFINS").InnerText.Replace(".", ",")
            End If



            If Not xmlnode("imposto")("COFINS").ChildNodes.Item(0).ChildNodes.Item(2) Is Nothing Then
                txtaliqcofins_item.Text = xmlnode("imposto")("COFINS").ChildNodes.Item(0).ChildNodes.Item(2).InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("COFINS").ChildNodes.Item(0)("vCOFINS") Is Nothing Then
                txtvlrcofins_item.Text = xmlnode("imposto")("COFINS").ChildNodes.Item(0)("vCOFINS").InnerText.Replace(".", ",")
            End If

            If Not xmlnode("imposto")("IPI") Is Nothing Then
                If Not xmlnode("imposto")("IPI").ChildNodes.Item(1)("vIPI") Is Nothing Then
                    txtvlripi_item.Text = xmlnode("imposto")("IPI").ChildNodes.Item(1)("vIPI").InnerText.Replace(".", ",")
                End If
                If Not xmlnode("imposto")("IPI").ChildNodes.Item(1)("pIPI") Is Nothing Then
                    txtaliqIPI_item.Text = xmlnode("imposto")("IPI").ChildNodes.Item(1)("pIPI").InnerText.Replace(".", ",")
                End If
                If Not xmlnode("imposto")("IPI").ChildNodes.Item(1)("CST") Is Nothing Then
                    txtcstipi_item.Text = xmlnode("imposto")("IPI").ChildNodes.Item(1)("CST").InnerText
                End If
            End If

            If Not xmlnode("imposto")("IPI") Is Nothing Then
                If Not xmlnode("imposto")("IPI")("IPITrib") Is Nothing Then
                    If Not xmlnode("imposto")("IPI")("IPITrib")("vIPI") Is Nothing Then
                        txtvlripi_item.Text = xmlnode("imposto")("IPI")("IPITrib")("vIPI").InnerText.Replace(".", ",")
                    End If
                    If Not xmlnode("imposto")("IPI")("IPITrib")("pIPI") Is Nothing Then
                        txtaliqIPI_item.Text = xmlnode("imposto")("IPI")("IPITrib")("pIPI").InnerText.Replace(".", ",")
                    End If
                    If Not xmlnode("imposto")("IPI")("IPITrib")("CST") Is Nothing Then
                        txtcstipi_item.Text = xmlnode("imposto")("IPI")("IPITrib")("CST").InnerText
                    End If
                End If
            End If

            If Not xmlnode("infAdProd") Is Nothing Then
                txtinfoadic_item.Text = xmlnode("infAdProd").InnerText
            End If

            ''''''''''Calcular valor final produto
            txtvlrfinal_prod_item.Text = (CDec(txtvlrtotal_item.Text) - CDec(txtvlrdesc_item.Text)) + CDec(txtvlrst_item.Text) + CDec(txtvlroutros_item.Text) + CDec(txtvlrfrete_item.Text) + CDec(txtvlripi_item.Text)
            txtvlrfinal_prod_item.Text = formata_valor_campo(txtvlrfinal_prod_item.Text)

            adiciona_iten_grid()

        Next


        limpar_itens()


    End Sub


#End Region


#Region "''''Rotina limpa controles"

    Public Sub limpar_itens()



        txtnr.Clear()
        txtqtdecx_item.ReadOnly = True

        txtvBCUFDest_item.Text = valor_campo_vazio()
        txtpFCPUFDest_item.Text = valor_campo_vazio()
        txtpICMSUFDest_item.Text = valor_campo_vazio()
        txtpICMSInter_item.Text = valor_campo_vazio()
        txtvlrdificms_item.Text = valor_campo_vazio()
        txtvlrfinal_prod_item.Text = valor_campo_vazio()
        txtvlrfcpst_item.Text = valor_campo_vazio()

        txtbaseredicmsst_item.Text = valor_campo_vazio()

        txtpICMSInterPart_item.Text = valor_campo_vazio()
        txtvFCPUFDest_item.Text = valor_campo_vazio()
        txtvICMSUFRemet_item.Text = valor_campo_vazio()
        txtvICMSUFDest_item.Text = valor_campo_vazio()

        txtvlrtrib_item.Text = valor_campo_vazio()

        txtbst.Text = valor_campo_vazio()

        txtbaseicms_item.Text = valor_campo_vazio()

        txtdesc_item.Clear()

        txtvlrfrete_item.Text = valor_campo_vazio()

        txtcodbarra_item.Clear()

        txtcodfor_item.Clear()

        txtvlrunit_item.Text = valor_campo_vazio()

        txtvlroutros_item.Text = valor_campo_vazio()

        txtqtde_item.Clear()

        If inclusao = False Then
            txtcfop_item.Text = ""
        End If

        txtncmex_item.Clear()

        txtcst_item.Clear()

        txtbaseredicms_item.Text = valor_campo_vazio()

        txtvlrdesc_item.Text = valor_campo_vazio()

        txtcstpis_item.Clear()

        txtaliqpis_item.Text = valor_campo_vazio()

        txtvlrpis_item.Text = valor_campo_vazio()

        txtcstcofins_item.Clear()

        txtaliqcofins_item.Text = valor_campo_vazio()

        txtvlrcofins_item.Text = valor_campo_vazio()

        txtncm_item.Clear()

        txtcest_item.Clear()

        txtun_item.Clear()

        txtmva_item.Text = valor_campo_vazio()

        txtaliqIPI_item.Text = valor_campo_vazio()

        txtvlrst_item.Text = valor_campo_vazio()

        txtaliqst_item.Text = valor_campo_vazio()

        txtlote_item.Clear()

        txtaliqicms_item.Text = valor_campo_vazio()

        txtvlricms_item.Text = valor_campo_vazio()

        mskdvenc_item.Clear()

        txtcstipi_item.Clear()

        txtvlripi_item.Text = valor_campo_vazio()

        txtvlrtotal_item.Text = valor_campo_vazio()

        txtpmc_item.Text = valor_campo_vazio()

        txtbasest_item.Text = valor_campo_vazio()

        txtinfoadic_item.Clear()

    End Sub

    Private Sub limpar_abas()
        txtprotocolocanc.Clear()


        txtxmotivocanc.Clear()
        txtemaildest.Clear()
        txtantt.Clear()
        txtemailfor.Clear()
        txtnomefor.Clear()
        txtnomefanfor.Clear()
        txtiefor.Clear()
        txtendfor.Clear()
        txtbairrofor.Clear()

        txtcidadefor.Clear()
        mskcnpjfor.Clear()
        txtuffor.Clear()
        mskcepfor.Clear()
        txtpaisfor.Clear()
        txtcodpaisfor.Clear()
        txtcmunfor.Clear()
        mskfonefor.Clear()
        txtcnae.Clear()
        txtnumerodest.Clear()
        txtnumerofor.Clear()
        txtim.Clear()
        txtinscst.Clear()
        txtregime.Clear()

        txtnomedest.Clear()
        txtnomefandest.Clear()
        mskcnpjdest.Clear()
        txtiedest.Clear()
        txtenddest.Clear()
        txtbairrodest.Clear()
        txtcidadedest.Clear()
        txtufdest.Clear()
        mskcepdest.Clear()
        txtcodpaisdest.Clear()
        txtidDest.Clear()
        txtpaisdest.Clear()
        txtcmundest.Clear()
        mskfonedest.Clear()
        txtemaildest.Clear()
        txtadifisco.Clear()

        txtnometrp.Clear()
        mskcnpjtrp.Clear()
        txtietrp.Clear()
        txtplacatrp.Clear()
        txtufplacatrp.Clear()
        txtendtrp.Clear()
        txtbairrotrp.Clear()
        txtcidadetrp.Clear()
        txtuftrp.Clear()
    End Sub

    Private Sub limpa_nfe()
        msknprot.Clear()

        txtvlrtotalvFCPUFDest.Text = valor_campo_vazio()
        txtvlrtotalvICMSUFDest.Text = valor_campo_vazio()
        txtvlrtotalvICMSUFRemet.Text = valor_campo_vazio()

        txttpEmis.Clear()
        txtcoduf.Clear()

        txttpNF.Clear()

        txttpAmb.Clear()
        txtprocEmi.Clear()
        txtverProc.Clear()
        txtdepc.Clear()
        txtinfcompl.Clear()
        dgvfatura.Rows.Clear()
        txtnumeronfe.Clear()
        txtidDest.Clear()
        txtserienfe.Clear()
        txtmodelonfe.Clear()
        txtdataemissao.Clear()
        txtdsaida.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
        cbonat.Text = ""
        mskchave.Clear()

        txtcfop_item.Text = ""

        mskchave_cont.Clear()

        txtprotocolo.Clear()
        txtxmotivo.Clear()
        txtQvol.Clear()
        txtesp.Clear()
        txtmarca.Clear()
        txtpesob.Clear()
        txtnvol.Clear()
        txtpesol.Clear()
        txtfretecont.Text = valor_campo_vazio()
        txtvlrtotalprod_nfe.Text = valor_campo_vazio()
        txtvlrtotal_nfe.Text = valor_campo_vazio()
        txtvlrtotalbaseicms_nfe.Text = valor_campo_vazio()
        txtvlrtotalicms_nfe.Text = valor_campo_vazio()
        txtvlrtotalbasest_nfe.Text = valor_campo_vazio()
        txtvlrtotalst_nfe.Text = valor_campo_vazio()
        txtvrltotalipi_nfe.Text = valor_campo_vazio()
        txtvlrtotalaproxtrib.Text = valor_campo_vazio()
        txtvlrtotalpis_nfe.Text = valor_campo_vazio()
        txtvlrtotalcofins_nfe.Text = valor_campo_vazio()
        txtvlrtotaldesc_nfe.Text = valor_campo_vazio()
        txtvlrtotalfrete_nfe.Text = valor_campo_vazio()
        txtvlrtotalseg_nfe.Text = valor_campo_vazio()
        txtvlrtotaloutras_nfe.Text = valor_campo_vazio()
        dgvecf.Rows.Clear()
        TabControl_nfe.Controls.Remove(aba_Cobertura_ECF)
    End Sub

    Private Sub limpardup()
        txtndup1.Clear()
        txtndup2.Clear()
        txtndup3.Clear()
        txtndup4.Clear()
        txtndup5.Clear()
        txtndup6.Clear()

        txtddup1.Clear()
        txtddup2.Clear()
        txtddup3.Clear()
        txtddup4.Clear()
        txtddup5.Clear()
        txtddup6.Clear()

        txtvdup1.Clear()
        txtvdup2.Clear()
        txtvdup3.Clear()
        txtvdup4.Clear()
        txtvdup5.Clear()
        txtvdup6.Clear()
        dgvfatura.Rows.Clear()
    End Sub

#End Region







#Region "''''Regras de validacao do CFOP_item e ICMS_item"
    Private Sub regua_validacao_icms()


        If txtcst_item.Text.Length = 3 Then

            If txtcst_item.Text.Substring(1, 2) = "60" Or txtcst_item.Text.Substring(1, 2) = "40" Then
                txtbaseicms_item.Text = valor_campo_vazio()
                txtvlricms_item.Text = valor_campo_vazio()
                txtaliqicms_item.Text = valor_campo_vazio()
                txtbaseredicms_item.Text = valor_campo_vazio()

                txtbasest_item.Text = valor_campo_vazio()
                txtaliqst_item.Text = valor_campo_vazio()
                txtvlrst_item.Text = valor_campo_vazio()
                txtmva_item.Text = valor_campo_vazio()
            End If

            If txtcst_item.Text.Substring(1, 2) = "00" Or txtcst_item.Text.Substring(1, 2) = "20" Then
                txtbasest_item.Text = valor_campo_vazio()
                txtaliqst_item.Text = valor_campo_vazio()
                txtvlrst_item.Text = valor_campo_vazio()
                txtmva_item.Text = valor_campo_vazio()
            End If

            If txtcst_item.Text.Substring(1, 2) = "30" Then
                txtbaseicms_item.Text = valor_campo_vazio()
                txtvlricms_item.Text = valor_campo_vazio()
                txtaliqicms_item.Text = valor_campo_vazio()
                txtbaseredicms_item.Text = valor_campo_vazio()
            End If

            If txtcst_item.Text.Substring(1, 2) = "10" Then
                txtbaseredicms_item.Text = valor_campo_vazio()
            End If

        End If

        If txtcst_item.Text.Length = 4 Then

            If txtcst_item.Text.Substring(1, 3) = "500" Or txtcst_item.Text.Substring(1, 3) = "102" Or txtcst_item.Text.Substring(1, 3) = "103" _
                Or txtcst_item.Text.Substring(1, 3) = "300" Or txtcst_item.Text.Substring(1, 3) = "400" Then

                txtbaseicms_item.Text = valor_campo_vazio()
                txtvlricms_item.Text = valor_campo_vazio()
                txtaliqicms_item.Text = valor_campo_vazio()
                txtbaseredicms_item.Text = valor_campo_vazio()

                txtbasest_item.Text = valor_campo_vazio()
                txtaliqst_item.Text = valor_campo_vazio()
                txtvlrst_item.Text = valor_campo_vazio()
                txtmva_item.Text = valor_campo_vazio()

            End If

            If txtcst_item.Text.Substring(1, 3) = "101" Then
                txtbasest_item.Text = valor_campo_vazio()
                txtaliqst_item.Text = valor_campo_vazio()
                txtvlrst_item.Text = valor_campo_vazio()
                txtmva_item.Text = valor_campo_vazio()
            End If

            If txtcst_item.Text.Substring(1, 3) = "202" Then
                txtbaseicms_item.Text = valor_campo_vazio()
                txtvlricms_item.Text = valor_campo_vazio()
                txtaliqicms_item.Text = valor_campo_vazio()
                txtbaseredicms_item.Text = valor_campo_vazio()
            End If

        End If


        ''''''''''''''''''''''''Valida campos obrigatorios
        If txtcst_item.Text.Length = 3 Then

            If txtcst_item.Text.Substring(1, 2) = "00" Then

                If txtaliqicms_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota de ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqicms_item.Focus()
                End If

            End If


            If txtcst_item.Text.Substring(1, 2) = "20" Then

                If txtaliqicms_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota de ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqicms_item.Focus()
                End If

                If txtbaseredicms_item = valor_campo_vazio() Then
                    MsgBox("Informe o Percentual da redução da Base do ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtbaseredicms_item.Focus()
                End If

            End If

            If txtcst_item.Text.Substring(1, 2) = "10" Then

                If txtaliqicms_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota de ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqicms_item.Focus()
                End If

                If txtaliqst_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota interna do ICMS/ST (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqst_item.Focus()
                End If

                If txtmva_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe o percentual da MVA (Obrigatorio ?)", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtmva_item.Focus()
                End If

                If txtbaseredicms_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe o Percentual da redução da Base do ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtbaseredicms_item.Focus()
                End If

            End If


            If txtcst_item.Text.Substring(1, 2) = "70" Then

                If txtaliqicms_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota de ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqicms_item.Focus()
                End If

                If txtaliqst_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota interna do ICMS/ST (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqst_item.Focus()
                End If

                If txtmva_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe o percentual da MVA (Obrigatorio ?)", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtmva_item.Focus()
                End If

                If txtbaseredicms_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe o Percentual da redução da Base do ICMS (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtbaseredicms_item.Focus()
                End If

                If txtbaseredicmsst_item.Text = valor_campo_vazio() Then
                    MsgBox("Informou o Percentual de Redução da Base do ICMS/ST (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtbaseredicmsst_item.Focus()
                End If

            End If


        End If

        '''''''''''''''''''''''''''Validacao simpre nacional
        If txtcst_item.Text.Length = 4 Then

            If txtcst_item.Text.Substring(1, 3) = "201" Or txtcst_item.Text.Substring(1, 3) = "202" Then


                If txtbaseredicmsst_item.Text = valor_campo_vazio() Then
                    MsgBox("Informou o Percentual de Redução da Base do ICMS/ST (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtbaseredicmsst_item.Focus()
                End If

                If txtmva_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe o percentual da MVA (Obrigatorio ?)", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtmva_item.Focus()
                End If

                If txtaliqst_item.Text = valor_campo_vazio() Then
                    MsgBox("Informe a Aliquota interna do ICMS/ST (Obrigatorio) ?", MsgBoxStyle.Exclamation, "Regra de Validação ICMS !")
                    erro_validacao = True
                    txtaliqst_item.Focus()
                End If

            End If




        End If

    End Sub

    Private Sub validar_item_cfop()



        erro_validacao = False

        Dim cst_calc As String

        If txtcst_item.Text <> "" Then
            If regimetrib = 3 Then
                cst_calc = txtcst_item.Text.Substring(1, 2)
            Else
                cst_calc = txtcst_item.Text.Substring(1, 3)
            End If
        Else
            cst_calc = "0"
        End If



        If cst_calc = "00" Or cst_calc = "20" Or cst_calc = "40" Or cst_calc = "51" Or cst_calc = "101" Or cst_calc = "102" Or cst_calc = "103" Or cst_calc = "300" Or cst_calc = "400" Then

            ' If txttipo.Text = "v" Then

            If txtcfop_item.Text.Substring(1, 1) <> "1" Then
                MsgBox("O CFOP de venda está incorreto, pois deve iniciar com (52) dentro do estado" & vbCrLf & "ou (62) fora do estado para produtos tributados pelo ICMS", MsgBoxStyle.Exclamation, "Regra de Validação CFOP!")
                erro_validacao = True
            End If

        End If

        '  If txttipo.Text = "d" Then

        If txtcfop_item.Text.Substring(1, 1) <> "2" Then
            MsgBox("O CFOP de Devolução está incorreto, pois deve iniciar com (52) dentro do estado" & vbCrLf & "ou (62) fora do estado para produtos tributados pelo ICMS", MsgBoxStyle.Exclamation, "Regra de Validação CFOP !")
            erro_validacao = True
        End If



        '  End If

        ' If cst_calc = "10" Or cst_calc = "30" Or cst_calc = "60" Or cst_calc = "70" Or cst_calc = "90" Or cst_calc = "500" Or cst_calc = "201" Or cst_calc = "202" Or cst_calc = "900" Then

        'If txttipo.Text = "v" Then

        If txtcfop_item.Text.Substring(1, 1) <> "4" Then
            MsgBox("O CFOP de venda ou Devolução está incorreto, pois deve iniciar com (54) dentro estado" & vbCrLf & "ou (64) fora do estado para produtos tributados pelo ICMS/ST !", MsgBoxStyle.Exclamation, "Regra de Validação CFOP !")
            erro_validacao = True
        End If

        '   End If

        ' End If

        ' End If
    End Sub

#End Region

    Private Sub imprimirDanfe()


        Dim dados As XmlDocument = New XmlDocument

        '  Try

        ''''''gera o danfe
        dados.LoadXml(xmldoc.OuterXml)

        '    danfe(dados)
        '  Catch ex As Exception

        ' End Try

    End Sub


    Public Sub carregaxml_pesquisa()

        Try
            xmldoc.LoadXml(Encoding.UTF8.GetString(frmpesquisarnfe.bytePicData))
        Catch ex As Exception
            xmldoc.LoadXml(Encoding.UTF8.GetString(frmpesquisarnfe.bytePicData).Remove(0, 39))
        End Try

        dgvgrid_xml.Rows.Clear()
        txtprotocolocanc.Visible = False
        chave()
        dadosnfe()
        fatura()
        dadostrasp()
        itens_nfe()

        TabControl_nfe.SelectTab(TabPrincipal)


        ativa_tab()


        MsgBox("XML carregado Com sucesso !!", MsgBoxStyle.Information, "Aviso !")
    End Sub



#Region "''''Rotina para capturar os dados do certificado digital"

    Public Sub busca_certificado()

        If nfecer = "" Then
            certificado = SelecionarCertificado("")
        Else
            certificado = SelecionarCertificado(nfecer)
        End If

    End Sub

    Public Sub carrega_dados_certificado()
        If nfecer <> "" Then

            If CDate(certificado.NotAfter.ToString) < Now.Date Then

                Dim wD As Long = DateDiff(DateInterval.Day, CDate(certificado.NotAfter.ToString), Now.Date)

                MsgBox("Faltam " & wD.ToString & " dias para seu certificado digital vencer !", MsgBoxStyle.Exclamation, "Aviso !")
            End If

            ' frminfocertificado.txtdatavenc.Text = certificado.NotAfter.ToString
            '  frminfocertificado.txtdataemi.Text = certificado.NotBefore.ToString
            '  frminfocertificado.txtrazao.Text = certificado.SubjectName.Name

            '  frminfocertificado.removecarrazao()
            For i As Integer = 0 To certificado.Extensions.Count - 1
                If certificado.Extensions.Item(i).Oid.Value = "2.5.29.17" Then

                    '    frminfocertificado.txtdadosbruto.Text = certificado.Extensions.Item(i).Format(True)

                End If
            Next

            ' frminfocertificado.pegacnpj()




        End If
    End Sub

#End Region


#Region "''''Função da tabpages"

    Public Sub desativa_tab()
        TabControl_nfe.TabPages(0).Enabled = False
        TabControl_nfe.TabPages(1).Enabled = False
        TabControl_nfe.TabPages(2).Enabled = False
        TabControl_nfe.TabPages(3).Enabled = False
    End Sub

    Public Sub ativa_tab()

        TabControl_nfe.TabPages(0).Enabled = True
        TabControl_nfe.TabPages(1).Enabled = True
        TabControl_nfe.TabPages(2).Enabled = True
        TabControl_nfe.TabPages(3).Enabled = True


    End Sub



#End Region




#Region "''''Rotinas do datagridview adicionar itens,excluir calcular valores da aba total"

    Private Sub dgvgrid_xml_Click(sender As Object, e As EventArgs) Handles dgvgrid_xml.Click

        If txtprotocolo.Text <> "" Then
            itenscod()
        End If

    End Sub

    Public Sub adiciona_iten_grid()
        Dim i As Integer
        dgvgrid_xml.Rows.Add()

        i = dgvgrid_xml.Rows.Count - 1

        If txtnr.Text = "" Then
            dgvgrid_xml.Rows.Item(i).Cells("Nr").Value = dgvgrid_xml.Rows.Count
        Else
            dgvgrid_xml.Rows.Item(i).Cells("Nr").Value = txtnr.Text
        End If


        dgvgrid_xml.Rows.Item(i).Cells("cod_sglinx").Value = "0"


        dgvgrid_xml.Rows.Item(i).Cells("codfor").Value = txtcodfor_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("descricao").Value = txtdesc_item.Text.TrimEnd()
        dgvgrid_xml.Rows.Item(i).Cells("qtde").Value = txtqtde_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlrunit").Value = txtvlrunit_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlrtotal").Value = txtvlrtotal_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("codbarra").Value = txtcodbarra_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("ncm").Value = txtncm_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("cfop").Value = txtcfop_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("cst").Value = txtcst_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("aliqicms").Value = txtaliqicms_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlricms").Value = txtvlricms_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlrst").Value = txtvlrst_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vFCP_st").Value = txtvlrfcpst_item.Text

        dgvgrid_xml.Rows.Item(i).Cells("cstpis").Value = txtcstpis_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("aliqpis").Value = txtaliqpis_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlrpis").Value = txtvlrpis_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("cstcofins").Value = txtcstcofins_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("aliqcofins").Value = txtaliqcofins_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlrcofins").Value = txtvlrcofins_item.Text

        dgvgrid_xml.Rows.Item(i).Cells("un").Value = txtun_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("mva").Value = txtmva_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("aliqst").Value = txtaliqst_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("cstipi").Value = txtcstipi_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlripi").Value = txtvlripi_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vlrdesc").Value = txtvlrdesc_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("lote").Value = txtlote_item.Text

        dgvgrid_xml.Rows.Item(i).Cells("pmc").Value = txtpmc_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("datavenc").Value = mskdvenc_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("baseicms").Value = txtbaseicms_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("basest").Value = txtbasest_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("predbc").Value = txtbaseredicms_item.Text

        dgvgrid_xml.Rows.Item(i).Cells("aliqipi").Value = txtaliqIPI_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("valor_aprox_trib").Value = txtvlrtrib_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("inf_adi_prod").Value = txtinfoadic_item.Text

        dgvgrid_xml.Rows.Item(i).Cells("vlroutros").Value = txtvlroutros_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("qtdecx").Value = "1"
        dgvgrid_xml.Rows.Item(i).Cells("vlrfrete").Value = txtvlrfrete_item.Text


        dgvgrid_xml.Rows.Item(i).Cells("cest").Value = txtcest_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("ex_tipe").Value = txtncmex_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("predbcst").Value = txtbaseredicmsst_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vicmsdeson").Value = "0.00"
        dgvgrid_xml.Rows.Item(i).Cells("motdesicms").Value = ""

        dgvgrid_xml.Rows.Item(i).Cells("vlrtotal_cdesc").Value = txtvlrfinal_prod_item.Text


        dgvgrid_xml.Rows.Item(i).Cells("vBCUFDest").Value = txtvBCUFDest_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("pFCPUFDest").Value = txtpFCPUFDest_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("pICMSUFDest").Value = txtpICMSUFDest_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("pICMSInter").Value = txtpICMSInter_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("pICMSInterPart").Value = txtpICMSInterPart_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vFCPUFDest").Value = txtvFCPUFDest_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vICMSUFRemet").Value = txtvICMSUFRemet_item.Text
        dgvgrid_xml.Rows.Item(i).Cells("vICMSUFDest").Value = txtvICMSUFDest_item.Text

    End Sub


    Private Sub itenscod()
        If dgvgrid_xml.Rows.Count <> "0" Then
            limpar_itens()
            txtdesc_item.Text = dgvgrid_xml.CurrentRow.Cells("descricao").Value
            txtcodbarra_item.Text = dgvgrid_xml.CurrentRow.Cells("codbarra").Value
            txtcodfor_item.Text = dgvgrid_xml.CurrentRow.Cells("codfor").Value
            txtvlrunit_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrunit").Value
            txtqtde_item.Text = dgvgrid_xml.CurrentRow.Cells("qtde").Value
            txtqtdecx_item.Text = dgvgrid_xml.CurrentRow.Cells("Qtdecx").Value



            txtcfop_item.Text = dgvgrid_xml.CurrentRow.Cells("cfop").Value
            txtcst_item.Text = dgvgrid_xml.CurrentRow.Cells("cst").Value
            txtvlrdesc_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrdesc").Value
            txtcstpis_item.Text = dgvgrid_xml.CurrentRow.Cells("cstpis").Value
            txtaliqpis_item.Text = dgvgrid_xml.CurrentRow.Cells("aliqpis").Value
            txtvlrpis_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrpis").Value
            txtcstcofins_item.Text = dgvgrid_xml.CurrentRow.Cells("cstcofins").Value
            txtaliqcofins_item.Text = dgvgrid_xml.CurrentRow.Cells("aliqcofins").Value
            txtvlrcofins_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrcofins").Value
            txtncm_item.Text = dgvgrid_xml.CurrentRow.Cells("ncm").Value

            txtncmex_item.Text = dgvgrid_xml.CurrentRow.Cells("ex_tipe").Value
            txtcest_item.Text = dgvgrid_xml.CurrentRow.Cells("cest").Value
            txtun_item.Text = dgvgrid_xml.CurrentRow.Cells("un").Value
            txtvlrfrete_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrfrete").Value
            txtmva_item.Text = dgvgrid_xml.CurrentRow.Cells("mva").Value
            txtvlrst_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrst").Value

            txtvlrfcpst_item.Text = dgvgrid_xml.CurrentRow.Cells("vFCP_st").Value
            txtaliqst_item.Text = dgvgrid_xml.CurrentRow.Cells("aliqst").Value
            txtlote_item.Text = dgvgrid_xml.CurrentRow.Cells("lote").Value
            txtaliqicms_item.Text = dgvgrid_xml.CurrentRow.Cells("aliqicms").Value
            txtvlricms_item.Text = dgvgrid_xml.CurrentRow.Cells("vlricms").Value
            mskdvenc_item.Text = dgvgrid_xml.CurrentRow.Cells("datavenc").Value
            txtcstipi_item.Text = dgvgrid_xml.CurrentRow.Cells("cstipi").Value
            txtvlripi_item.Text = dgvgrid_xml.CurrentRow.Cells("vlripi").Value
            txtvlrtotal_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrtotal").Value

            If dgvgrid_xml.CurrentRow.Cells("pmc").Value <> "" Then
                txtpmc_item.Text = dgvgrid_xml.CurrentRow.Cells("pmc").Value
            End If



            txtbaseicms_item.Text = dgvgrid_xml.CurrentRow.Cells("baseicms").Value
            txtbasest_item.Text = dgvgrid_xml.CurrentRow.Cells("basest").Value
            txtbaseredicms_item.Text = dgvgrid_xml.CurrentRow.Cells("predbc").Value
            txtaliqIPI_item.Text = dgvgrid_xml.CurrentRow.Cells("aliqipi").Value
            txtvlrtrib_item.Text = dgvgrid_xml.CurrentRow.Cells("Valor_aprox_trib").Value
            txtvlroutros_item.Text = dgvgrid_xml.CurrentRow.Cells("vlroutros").Value

            txtvlrfinal_prod_item.Text = dgvgrid_xml.CurrentRow.Cells("vlrtotal_cdesc").Value


            If dgvgrid_xml.CurrentRow.Cells("inf_adi_prod").Value <> "" Then
                inf_adici_prod.Visible = True
                txtinfoadic_item.Text = dgvgrid_xml.CurrentRow.Cells("inf_adi_prod").Value
            Else
                inf_adici_prod.Visible = False
            End If


            If dgvgrid_xml.CurrentRow.Cells("Qtdecx").Value <> "0" Then

                txtvlrunit_item.Text = CDec(txtvlrunit_item.Text) / CDec(dgvgrid_xml.CurrentRow.Cells("Qtdecx").Value)
                txtvlrunit_item.Text = formata_valor_campo(txtvlrunit_item.Text)

            End If



        End If
    End Sub


    Private Sub formata_grid()

        For i As Integer = 0 To dgvgrid_xml.Rows.Count - 1

            dgvgrid_xml.Rows.Item(i).Cells("qtde").Value = dgvgrid_xml.Rows.Item(i).Cells("qtde").Value.Replace(".", ",")
            dgvgrid_xml.Rows.Item(i).Cells("qtde").Value = Format(CDec(dgvgrid_xml.Rows.Item(i).Cells("qtde").Value), "#####0.000")
            dgvgrid_xml.Rows.Item(i).Cells("qtde").Value = dgvgrid_xml.Rows.Item(i).Cells("qtde").Value.Replace(",", ".")

            dgvgrid_xml.Rows.Item(i).Cells("aliqicms").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("aliqicms").Value)
            dgvgrid_xml.Rows.Item(i).Cells("vlricms").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlricms").Value)
            dgvgrid_xml.Rows.Item(i).Cells("vlrst").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlrst").Value)

            dgvgrid_xml.Rows.Item(i).Cells("aliqpis").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("aliqpis").Value)
            dgvgrid_xml.Rows.Item(i).Cells("vlrpis").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlrpis").Value)

            dgvgrid_xml.Rows.Item(i).Cells("aliqcofins").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("aliqcofins").Value)
            dgvgrid_xml.Rows.Item(i).Cells("vlrcofins").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlrcofins").Value)

            dgvgrid_xml.Rows.Item(i).Cells("mva").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("mva").Value)
            dgvgrid_xml.Rows.Item(i).Cells("aliqst").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("aliqst").Value)

            dgvgrid_xml.Rows.Item(i).Cells("vlripi").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlripi").Value)
            dgvgrid_xml.Rows.Item(i).Cells("vlrdesc").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlrdesc").Value)

            dgvgrid_xml.Rows.Item(i).Cells("aliqipi").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("aliqipi").Value)
            dgvgrid_xml.Rows.Item(i).Cells("valor_aprox_trib").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("valor_aprox_trib").Value)

            dgvgrid_xml.Rows.Item(i).Cells("vlroutros").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlroutros").Value)

            dgvgrid_xml.Rows.Item(i).Cells("vlrfrete").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vlrfrete").Value)
            dgvgrid_xml.Rows.Item(i).Cells("predbcst").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("predbcst").Value)

            dgvgrid_xml.Rows.Item(i).Cells("vicmsdeson").Value = formata_valor_campo(dgvgrid_xml.Rows.Item(i).Cells("vicmsdeson").Value)
        Next
    End Sub




#End Region






    Private Sub txttpEmis_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txttpEmis.TextChanged
        If txttpEmis.Text = "1" Then
            txttpEmis.Text = "1-Emissão Normal"
        End If
        If txttpEmis.Text = "2" Then
            txttpEmis.Text = "2-Contingência FS"
        End If
        If txttpEmis.Text = "3" Then
            txttpEmis.Text = "3-Contingência SCAN"
        End If
        If txttpEmis.Text = "4" Then
            txttpEmis.Text = "4-Contingência DPEC"
        End If
        If txttpEmis.Text = "5" Then
            txttpEmis.Text = "5-Contingência FSDA"
        End If
        If txttpEmis.Text = "6" Then
            txttpEmis.Text = "6-Contingência SVC - AN"

        End If
        If txttpEmis.Text = "7" Then
            txttpEmis.Text = "7-Contingência SVC - RS"
        End If
    End Sub

    Private Sub tpAmb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txttpAmb.TextChanged
        If txttpAmb.Text = "1" Then
            txttpAmb.Text = "1-Ambiente Produção"
        End If
        If txttpAmb.Text = "2" Then
            txttpAmb.Text = "2-Ambiente Homologação"
        End If
    End Sub

    Private Sub txtprocEmi_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtprocEmi.TextChanged
        If txtprocEmi.Text = "0" Then
            txtprocEmi.Text = "0-Aplicativo do contribuinte"
        End If
        If txtprocEmi.Text = "1" Then
            txtprocEmi.Text = "1-Emissão de NF-e avulsa pelo Fisco"
        End If
        If txtprocEmi.Text = "2" Then
            txtprocEmi.Text = "2-emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco"
        End If
        If txtprocEmi.Text = "3" Then
            txtprocEmi.Text = "3-Aplicativo fornecido pelo Fisco"
        End If
    End Sub

    Private Sub txttpNF_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txttpNF.TextChanged
        If txttpNF.Text = "0" Then
            txttpNF.Text = "0-NFe de Entrada"
            tiponota = "0"
            optentrada.Checked = True
        End If
        If txttpNF.Text = "1" Then
            txttpNF.Text = "1-NFe de Saída"
            tiponota = "1"
            optsaida.Checked = True
        End If
    End Sub

    Private Sub txtregime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtregime.TextChanged
        If txtregime.Text = "1" Then
            txtregime.Text = "1-Simples Nacional"
        End If
        If txtregime.Text = "2" Then
            txtregime.Text = "2-Simples Nacional – excesso de sublimite de receita bruta"
        End If
        If txtregime.Text = "3" Then
            txtregime.Text = "3-Regime Normal"
        End If
    End Sub

    Private Sub txtfretecont_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtfretecont.TextChanged
        If txtfretecont.Text = "0" Then
            txtfretecont.Text = "(0) Emitente"
            optemi.Checked = True
        End If
        If txtfretecont.Text = "1" Then
            txtfretecont.Text = "(1) Dest/Rem"
            optdest.Checked = True
        End If
        If txtfretecont.Text = "2" Then
            txtfretecont.Text = "(2) Terseiros"
            optter.Checked = True
        End If
        If txtfretecont.Text = "9" Then
            txtfretecont.Text = "(9) Sem frete"
            optsem.Checked = True
        End If
    End Sub

    Private Sub txtinddest_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtidDest.TextChanged
        If txtidDest.TextLength = "1" Then
            If txtidDest.Text = "1" Then
                txtidDest.Text = "1-Interna"
            End If
            If txtidDest.Text = "2" Then
                txtidDest.Text = "2-Interestadual"
            End If
            If txtidDest.Text = "3" Then
                txtidDest.Text = "3-Exterior"
            End If
        End If
    End Sub

    Private Sub txtcodbarra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcodbarra_item.Click
        txtcodbarra_item.SelectAll()
    End Sub

    Private Sub txtcodbarra_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcodbarra_item.GotFocus
        txtcodbarra_item.BackColor = Color.White
        txtcodbarra_item.BorderStyle = BorderStyle.Fixed3D
    End Sub

    Private Sub txtcodbarra_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtcodbarra_item.KeyPress
        If (e.KeyChar = Chr(13)) Then

            If txtcodbarra_item.Text <> "" Then

                txtcodfor_item.Clear()


            End If
        End If
    End Sub

    Private Sub txtcodbarra_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcodbarra_item.LostFocus
        txtcodbarra_item.BackColor = Color.LemonChiffon
        'txtcodbarra.BorderStyle = BorderStyle.FixedSingle
    End Sub


    Private Sub mskchave_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles mskchave.DoubleClick

        If nfecer = "" Then
            MsgBox("Não existe certificado Digital Selecionado !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If
        If mskchave.Text = "" Then
            MsgBox("Informe a Chave de acesso da NF-e", MsgBoxStyle.Exclamation, "Aviso !")
            mskchave.Focus()
            Exit Sub
        End If



        lbltimer.Text = "0"

    End Sub

    Private Sub mskchave_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles mskchave.GotFocus
        mskchave.Mask = "00000000000000000000000000000000000000000000"
    End Sub

    Private Sub mskchave_KeyPress(sender As Object, e As KeyPressEventArgs) Handles mskchave.KeyPress
        If (e.KeyChar = Chr(13)) Then
            If mskchave.Text = "" Then
                MsgBox("Informe a Chave de acesso da NF-e", MsgBoxStyle.Exclamation, "Aviso !")
                mskchave.Focus()
                Exit Sub
            End If

            busca_xml_consultadest_sql()
        End If

    End Sub

    Private Sub mskchave_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles mskchave.LostFocus
        If mskchave.Text <> "" Then
            mskchave.Mask = "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000"
        End If
    End Sub

    Private Sub frmxml_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        End
    End Sub

    Private Sub frmxml_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{Tab}")
        End If

        Select Case e.KeyCode

            Case Keys.F8
                imprimirDanfe()

            Case Keys.Escape
                Me.Dispose()
        End Select
    End Sub


    Private Sub frmxml_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pitens.Enabled = False

        '   desativa_tab()

        '  TabControl_nfe.Controls.Remove(aba_Cobertura_ECF)

        Me.Text = versao
        cbosefaz.Text = "Selecione"

        pitens.Enabled = True


        '''''''''''''''''''''''''''''''''''''''Rotina busca certificado digital'''''''''''''''''''''''''''''''''
        '  If lerINI(Application.StartupPath & "\config.ini", "FORMULARIO", "pedircert") = True Then

        If nfecer = "" Then
            '   busca_certificado()
            '   carrega_dados_certificado()
        Else
            '    certificado = SelecionarCertificado(nfecer)
            '   carrega_dados_certificado()
        End If

        Me.Text = "RFB XML Online  |  Versao: " & Assembly.GetExecutingAssembly.GetName.Version.ToString & "  13-02-2020   |   .Net 4.5"
    End Sub


    Private Sub cbosefaz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbosefaz.SelectedIndexChanged
        If cbosefaz.Text = "Acre" Then
            Process.Start("http://www.sefaz.ac.gov.br/sefaz2010/")
        End If
        If cbosefaz.Text = "Alagoas" Then
            Process.Start("http://www.sefaz.al.gov.br/nfe/")
        End If
        If cbosefaz.Text = "Amazonas" Then
            Process.Start("http://sistemas.sefaz.am.gov.br/nfeweb/portal/index.do")
        End If
        If cbosefaz.Text = "Bahia" Then
            Process.Start("http://www.sefaz.ba.gov.br/nfen/portal/home.htm")
        End If
        If cbosefaz.Text = "Ceará" Then
            Process.Start("http://nfe.sefaz.ce.gov.br/")
        End If
        If cbosefaz.Text = "Distrito Federal" Then
            Process.Start("http://dec.fazenda.df.gov.br/")
        End If
        If cbosefaz.Text = "Espirito Santo" Then
            Process.Start("http://internet.sefaz.es.gov.br/informacoes/nfe/")
        End If
        If cbosefaz.Text = "Goiás" Then
            Process.Start("http://nfe.sefaz.go.gov.br/")
        End If
        If cbosefaz.Text = "Maranhão" Then
            Process.Start("http://www.sefaz.ma.gov.br/NFE/")
        End If
        If cbosefaz.Text = "Mato Grosso" Then
            Process.Start("http://www.sefaz.mt.gov.br/portal/nfe/")
        End If
        If cbosefaz.Text = "Mato Grosso do Sul" Then
            Process.Start("http://www1.nfe.ms.gov.br/")
        End If
        If cbosefaz.Text = "Minas Gerais" Then
            Process.Start("http://portalnfe.fazenda.mg.gov.br/")
        End If
        If cbosefaz.Text = "Pará" Then
            Process.Start("http://www.sefa.pa.gov.br/site/")
        End If
        If cbosefaz.Text = "Paraíba" Then
            Process.Start("http://www.receita.pb.gov.br/portalnf-e.php")
        End If
        If cbosefaz.Text = "Paraná" Then
            Process.Start("http://www.sped.fazenda.pr.gov.br/")
        End If
        If cbosefaz.Text = "Pernambuco" Then
            Process.Start("http://www.sefaz.pe.gov.br/sefaz2/asp2/mostra.asp?pai=1040")
        End If
        If cbosefaz.Text = "Piauí" Then
            Process.Start("http://www.sefaz.pi.gov.br/conteudo_internet.php?p=nfe_home")
        End If
        If cbosefaz.Text = "Rio de Janeiro" Then
            Process.Start("http://www.fazenda.rj.gov.br/portal/")
        End If
        If cbosefaz.Text = "Rio Grande do Norte" Then
            Process.Start("http://www.set.rn.gov.br/contentProducao/aplicacao/set_v2/nfe/gerados/inicio.asp")
        End If
        If cbosefaz.Text = "Rio Grande do Sul" Then
            Process.Start("http://www.sefaz.rs.gov.br/SEF_ROOT/inf/SEF-NFE.htm")
        End If
        If cbosefaz.Text = "Rondônia" Then
            Process.Start("http://www.portal.sefin.ro.gov.br/site/conteudo.action?conteudo=234")
        End If
        If cbosefaz.Text = "Santa Catarina" Then
            Process.Start("http://nfe.sef.sc.gov.br/")
        End If
        If cbosefaz.Text = "São Paulo" Then
            Process.Start("http://www.fazenda.sp.gov.br/nfe/")
        End If
        If cbosefaz.Text = "Sergipe" Then
            Process.Start("http://nfe.sefaz.se.gov.br/")
        End If
        If cbosefaz.Text = "Tocantins" Then
            Process.Start("http://www.sefaz.to.gov.br/nfe/consultanfe.php")
        End If

    End Sub

    Private Sub txtqtdecx_item_DoubleClick(sender As Object, e As EventArgs) Handles txtqtdecx_item.DoubleClick
        txtqtdecx_item.ReadOnly = False
    End Sub

    Private Sub txtqtdecx_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtqtdecx_item.LostFocus
        If txtvlrunit_item.Text <> "" Then
            If txtqtdecx_item.Text = "" Then
                txtqtdecx_item.Text = "1"
            End If
            txtvlrunit_item.Text = CDec(txtvlrunit_item.Text) / CDec(txtqtdecx_item.Text)
            If Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator = "." Then
                txtvlrunit_item.Text = Format(CDec(txtvlrunit_item.Text), "#,##0.0000")
            End If
        End If
    End Sub

    Private Sub txtdesc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtdesc_item.GotFocus
        txtdesc_item.Text = txtdesc_item.Text.ToUpper
    End Sub


    Private Sub Label129_Click(sender As Object, e As EventArgs)
        txtdataemissao.Text = Now
        txtdsaida.Text = Now
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start(Application.StartupPath & "\projeto_atualizacao.exe")
        End
    End Sub

    Private Sub Label59_Click(sender As Object, e As EventArgs) Handles Label59.Click

        If dgvgrid_xml.Rows.Count <> "0" Then
            MsgBox(dgvgrid_xml.CurrentRow.Cells("custofinal").Value)
        End If

    End Sub

    Private Sub dgvgrid_xml_DoubleClick(sender As Object, e As EventArgs) Handles dgvgrid_xml.DoubleClick
        frmcalcpreco.txtcod_sg.Text = ""

        If dgvgrid_xml.CurrentRow.Cells("cod_sglinx").Value = "0" Then
            verifica_cadastro_sglinx()
        End If

        frmcalcpreco.txtcod_sg.Text = dgvgrid_xml.CurrentRow.Cells("cod_sglinx").Value


        If txtuffor.Text <> "MG" Then
            MsgBox("Nota Fiscal de outro estado, O calculo pode ser feito errado !", MsgBoxStyle.Exclamation, "Aviso !")
        End If


        Try
            If lerINI(Application.StartupPath & "\sglinear.ini", "FORMA_PRECO", "calc_preco") = True Then
                frmcalcpreco.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso, chama forme Preço")
        End Try

    End Sub


    Private Sub CapturarDadosNFePortalNacionalToolStripMenuItem_Click(sender As Object, e As EventArgs)
        limpa_nfe()
        limpar_abas()
        limpar_itens()
        limpardup()
        dgvgrid_xml.Rows.Clear()

    End Sub

    Private Sub CapturarDadosNFeSEFAZMGToolStripMenuItem_Click(sender As Object, e As EventArgs)
        limpa_nfe()
        limpar_abas()
        limpar_itens()
        limpardup()
        dgvgrid_xml.Rows.Clear()

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)

        If cnpj <> cnpj_emp Then
            MsgBox("Certificado Digital seleciona está incorreto, " & vbCrLf & "O CNPJ do Certificado e Diferente da Empresa Logada no Sistema !" & vbCrLf & "Operação cancelada !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If

        If nfecer = "" Then
            MsgBox("Informe um Certificado Digital !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If




    End Sub



    Private Sub bntconsregime_Click(sender As Object, e As EventArgs) Handles bntconsregime.Click
        If txtuffor.Text = "MG" Then
            coduf = "31"

        End If

        If txtuffor.Text <> "MG" Then
            MsgBox("Esta operação só é Permitida para Forneccedores de Minas Gerais", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If


        If txtuffor.Text = "SP" Then
            coduf = "35"

        End If

        If txtuffor.Text = "PR" Then
            coduf = "41"

        End If

        If txtuffor.Text = "RS" Then
            coduf = "43"

        End If

        If txtuffor.Text = "AM" Then
            coduf = "13"

        End If

        If txtuffor.Text = "BA" Then
            coduf = "29"

        End If

        If txtuffor.Text = "CE" Then
            coduf = "23"

        End If

        If txtuffor.Text = "ES" Then
            coduf = "32"

        End If

        If txtuffor.Text = "GO" Then
            coduf = "52"

        End If

        If txtuffor.Text = "MT" Then
            coduf = "51"

        End If

        If txtuffor.Text = "MS" Then
            coduf = "50"

        End If

        If txtuffor.Text = "PE" Then
            coduf = "26"

        End If

        If txtuffor.Text = "MA" Then
            coduf = "21"

        End If

        If txtuffor.Text = "AC" Or txtuffor.Text = "RN" Or txtuffor.Text = "PB" Or txtuffor.Text = "SC" Then



            If txtuffor.Text = "AC" Then
                coduf = "12"
            ElseIf txtuffor.Text = "RN" Then
                coduf = "24"
            ElseIf txtuffor.Text = "PB" Then
                coduf = "25"
            ElseIf txtuffor.Text = "SC" Then
                coduf = "42"
            End If
        End If

        Label91.Text = "Validando a Regime tributario do Fornecedor..."

        lbltimer.Text = "0"
        tcad.Enabled = True
    End Sub


    Private Sub txtpainel_Click(sender As Object, e As EventArgs) Handles txtpainel.Click
        If txtpainel.Text.Substring(0, 19) = "Ultima consulta DFe" Then
            Me.TabControl_nfe.SelectTab("DFe")
        End If
    End Sub


    Private Sub EnviaEmailNFeToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If txtcaminhoxml.Text = "" Then
            MsgBox("Abra primeiro o xml no Visualizador, para depois enviá-lo por e-mail !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If

        If File.Exists(txtcaminhoxml.Text) = False Then
            MsgBox("Arquivo xml que você esta tentando enviar não existe !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If

        email = True

        Dim dados As XmlDocument = New XmlDocument

        dados.Load(txtcaminhoxml.Text)

        ' danfe(dados)


    End Sub



    Private Sub VisualizarNFeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisualizarNFeToolStripMenuItem.Click

        Try

            dgvgrid_xml.Rows.Clear()
            txtprotocolocanc.Visible = False
            chave()
            dadosnfe()
            fatura()
            dadostrasp()
            itens_nfe()

            ' frminfo.Close()
            TabControl_nfe.SelectTab(TabPrincipal)

            Label78.Text = "Protocolo de Autorização"



            MsgBox("Arquivo Carregado !", MsgBoxStyle.Information, "Aviso !")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso !")
        End Try
    End Sub

    Private Sub txtindIEDest_TextChanged(sender As Object, e As EventArgs) Handles txtindIEDest.TextChanged
        If txtindIEDest.Text = "1" Then
            txtindIEDest.Text = "1 – Contribuinte ICMS"
        End If
        If txtindIEDest.Text = "2" Then
            txtindIEDest.Text = "2 – Contribuinte isento de inscrição"
        End If
        If txtindIEDest.Text = "9" Then
            txtindIEDest.Text = "9 – Não Contribuinte"
        End If

    End Sub

    Private Function pes_Gridibpt(ByVal id As String) As String

        For Each row As DataGridViewRow In Me.dgvibpt.Rows

            If Not row.IsNewRow Then
                If row.Cells(0).Value + row.Cells(1).Value = id Then
                    Return row.Index
                End If
            End If

        Next

        Return 0

    End Function

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs)

        txttpNF.Text = "0-NFe de Entrada"
        tiponota = "0"

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs)

        txttpNF.Text = "1-NFe de Saída"
        tiponota = "1"

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If cbomod_ecf.Text = "" Then
            MsgBox("Informe 0 Código do modelo do Documento Fiscal Preencher com(2B), quando se tratar de Cupom Fiscal emitido por máquina registradora (não ECF), com (2C), quando se tratar de Cupom Fiscal PDV, ou (2D), quando se tratar de Cupom Fiscal (emitido por ECF)", MsgBoxStyle.Exclamation, "Aviso !")
            cbomod_ecf.Focus()
            Exit Sub
        End If
        If txtnecf.Text = "" Or txtnecf.Text = "000" Then
            MsgBox("Informar o número de ordem seqüencial do ECF que emitiu o Cupom Fiscal vinculado à NF-e", MsgBoxStyle.Exclamation, "Aviso !")
            txtnecf.Focus()
            Exit Sub
        End If

        If txtncoo.Text = "" Or txtncoo.Text = "000000" Then
            MsgBox("Informar o Número do Contador de Ordem de Operação - COO vinculado à NF-e", MsgBoxStyle.Exclamation, "Aviso !")
            txtncoo.Focus()
            Exit Sub
        End If

        dgvecf.Rows.Add(cbomod_ecf.Text, txtnecf.Text, txtncoo.Text)

        cbomod_ecf.Text = ""
        txtnecf.Text = ""
        txtncoo.Text = ""
    End Sub

    Private Sub txtnecf_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnecf.KeyPress
        If Char.IsLetter(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtncoo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtncoo.KeyPress
        If Char.IsLetter(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Pic_pis_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub txtvlrunit_item_TextChanged(sender As Object, e As EventArgs) Handles txtvlrunit_item.TextChanged

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If dgvecf.RowCount = "0" Then
            Exit Sub
        End If

        If MessageBox.Show("Deseja Realmente excluir o item selecionado ?", "Aviso !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

            dgvecf.Rows.Remove(dgvecf.CurrentRow)

            MsgBox("Item excluido com sucesso !", MsgBoxStyle.Information, "Aviso !")
        End If
    End Sub

    Private Sub btndanfe_Click(sender As Object, e As EventArgs)
        imprimirDanfe()
    End Sub

    Private Sub txtcalc_prod_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtnecf_LostFocus(sender As Object, e As EventArgs) Handles txtnecf.LostFocus
        txtnecf.Text = txtnecf.Text.PadLeft(3, "0")
    End Sub

    Private Sub txtncoo_LostFocus(sender As Object, e As EventArgs) Handles txtncoo.LostFocus
        txtncoo.Text = txtncoo.Text.PadLeft(6, "0")
    End Sub


    Private Sub txtufdest_TextChanged(sender As Object, e As EventArgs) Handles txtufdest.TextChanged

        If txtufdest.Text = txtuffor.Text Then
            txtidDest.Text = "1-Interna"
        End If
        If txtufdest.Text <> txtuffor.Text Then
            txtidDest.Text = "2-Interestadual"
        End If


    End Sub

    Private Sub LinkLabel1_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        carregar_precovenda()
    End Sub

    Private Sub bntcancelar_Click(sender As Object, e As EventArgs)

        If cnpj <> cnpj_emp Then
            MsgBox("Certificado Digital seleciona está incorreto, " & vbCrLf & "O CNPJ do Certificado e Diferente da Empresa Logada no SIstema !", MsgBoxStyle.Information, "Aviso !")
            Exit Sub
        End If

        If nfecer = "" Then
            MsgBox("Informe um Certificado Digital !", MsgBoxStyle.Information, "Aviso !")
            Exit Sub
        End If



    End Sub

    Private Sub txtadifisco_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtadifisco.KeyPress
        If e.KeyChar = Chr(13) Then


            TabControl_nfe.SelectTab(Tabitens)
            txtcodfor_item.Focus()

        End If
    End Sub

    Public Sub abrir_xml(ByVal xml As String)


        xmldoc.LoadXml(xml)
        dgvgrid_xml.Rows.Clear()

        txtdsaida.Format = DateTimePickerFormat.Custom
        txtdsaida.CustomFormat = "dd/MM/yyyy HH:mm:ss"

        '''''''''''''''''''''''Rotina para carregar os dados da NFe abaixo:''''''''''''''''''''''''''''''''''''
        ' LerNFE(txtcaminhoxml.Text)
        chave()
        dadosnfe()
        fatura()
        dadostrasp()
        itens_nfe()


        lblcanc.Visible = True
        lblmotcanc.Visible = True
        txtprotocolocanc.Visible = True
        txtxmotivocanc.Visible = True


        TabControl_nfe.SelectTab(TabPrincipal)
        '    verificar_ean()

        '  valida_icms()

        ativa_tab()
    End Sub
    Private Sub bntopen_Click(sender As Object, e As EventArgs) Handles bntopen.Click
        '   Try

        txtcaminhoxml.BackColor = Color.White
        frminfo.Show()
        frminfo.txtinfo.Text = vbCrLf + "Aguarde, Carregando o arquivo XML ..."


        dialogo.Filter = "Arquivos (.xml .txt)|*.xml;*.txt"

        If dialogo.ShowDialog() = Windows.Forms.DialogResult.OK Then

            txtcaminhoxml.Text = dialogo.FileName

            If Path.GetExtension(txtcaminhoxml.Text) = ".xml" Or Path.GetExtension(txtcaminhoxml.Text) = ".XML" Then

                xmldoc.Load(txtcaminhoxml.Text)
                dgvgrid_xml.Rows.Clear()

                txtdsaida.Format = DateTimePickerFormat.Custom
                txtdsaida.CustomFormat = "dd/MM/yyyy HH:mm:ss"
                '''''''''''''''''''''''Rotina para carregar os dados da NFe abaixo:''''''''''''''''''''''''''''''''''''
                ' LerNFE(txtcaminhoxml.Text)
                chave()
                dadosnfe()
                fatura()
                dadostrasp()
                itens_nfe()


                lblcanc.Visible = True
                lblmotcanc.Visible = True
                txtprotocolocanc.Visible = True
                txtxmotivocanc.Visible = True


                frminfo.Close()
                TabControl_nfe.SelectTab(TabPrincipal)
                '    verificar_ean()

                '  valida_icms()

                ativa_tab()

            End If

        End If

        frminfo.Close()

        '''''''''''''''''''''Evento abaixo executado quando ocorrer um erro'''''''''''''''''''''''''''''''''''
        '   Catch ex As Exception
        'MsgBox(ex.Message + vbCrLf + "Contate o Administrador do Sistema." + vbCrLf + "Operação cancelada !", MsgBoxStyle.Critical, "Aviso !")
        '   frminfo.Close()
        '  End Try
    End Sub

    Private Sub cbonat_TextChanged(sender As Object, e As EventArgs) Handles cbonat.TextChanged
        Dim boxIndex As Integer, lExst As Boolean


        Dim box As ComboBox = sender

        Dim txt As String = box.Text

        Dim posCursor As Integer = box.SelectionStart

        ' Se o cursor não estiver no inicio do textbox inicia a busca

        If posCursor <> 0 Then

            lExst = False

            ' Procura na combo pela entrada na lista

            For boxIndex = 0 To box.Items.Count - 1

                If UCase(Mid(box.Items(boxIndex), 1, posCursor)) = UCase(Mid(txt, 1, posCursor)) Then

                    box.Text = box.Items(boxIndex)

                    box.SelectionStart = posCursor

                    lExst = True

                    Exit For

                End If

            Next

            ' Se não encontrar retorna o valor anterior

            If Not lExst Then

                box.Text = Mid(txt, 1, posCursor - 1) + Mid(txt, posCursor + 1)

                box.SelectionStart = posCursor - 1

            End If

        End If
    End Sub


    Private Sub btncarta_Click(sender As Object, e As EventArgs)
        If txtprotocolo.Text = "" Then
            MsgBox("Carregue abaixo a Nota Fiscal para gerar a carta de correção !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If

        '''''''''''''''''''''''''Regra de validacao de cnpj certificado''''''''''''''''''''
        If removeFormatacao(mskcnpjfor.Text) <> cnpj Then
            MsgBox("CNPJ do Certificado Digital informado diferente do CNPJ Emitente da NF-e !" + vbCrLf + "Operação cancelada !", MsgBoxStyle.Exclamation, "Aviso !")
            Exit Sub
        End If


    End Sub
    Private Sub inf_adici_prod_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles inf_adici_prod.LinkClicked
        MsgBox(txtinfoadic_item.Text, MsgBoxStyle.Information, "Informação adicional Produto !")
    End Sub

    Private Sub txtcalc_prod_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsLetter(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub


    Public Sub carregar_precovenda()
        Try

            conectar_preco()

            For i As Integer = 0 To dgvgrid_xml.Rows.Count - 1


                rs.Open("select * from preco_nfe where chave_pr like '" & mskchave.Text.Replace(" ", "") + dgvgrid_xml.Rows.Item(i).Cells("codfor").Value & "'", conexao.cn_p, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                If rs.EOF = False Then

                    dgvgrid_xml.Rows.Item(i).Cells("pmc").Value = rs.Fields("preco_vd").Value
                    '   dgvgrid_xml.Rows.Item(i).Cells("custofinal").Value = rs.Fields("precocusto_vd").Value

                    If rs.Fields("Qtdecx").Value.ToString.Trim = "" Then
                        dgvgrid_xml.Rows.Item(i).Cells("Qtdecx").Value = "1"
                    Else
                        dgvgrid_xml.Rows.Item(i).Cells("Qtdecx").Value = rs.Fields("Qtdecx").Value
                    End If

                End If

                rs.Close()

            Next

            conexao.cn_p.Close()

            MsgBox("Os preços de venda foram carregado com Sucesso !", MsgBoxStyle.Information, "Aviso !")

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso !")
        End Try

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        frmpesquisarnfe.ShowDialog()
    End Sub



    Private Sub Button1_MouseUp(sender As Object, e As MouseEventArgs)
        Me.ContextMenuStrip3.Show(TryCast(sender, Control), e.X, e.Y)
    End Sub



#Region "''''Rotina DO sistema SGlinx"


    Private Sub verifica_cadastro_sglinx()
        Dim cod_cad_for As String

        Dim sql As String


        Try


            conectarsqlinx()


            '''''''''''''''''verifica se o fornecedor esta cadastrado'''''''''''''''''''
            sql = "SELECT cg2_cgc, cg2_cod FROM cg2  where cg2_cgc = '" & mskcnpjfor.Text & "'"

            rs.Open(sql, conexao.cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)


            If rs.EOF = True Then
                MsgBox("CNPJ Não Cadastrado no Banco", MsgBoxStyle.Exclamation, "Aviso !")
                cnsql.Close()
                Exit Sub
            Else
                cod_cad_for = rs.Fields("cg2_cod").Value
                rs.Close()
            End If



            For Each row As DataGridViewRow In dgvgrid_xml.Rows

                row.Cells("cod_sglinx").Value = "0"



                'Rotina Busca pelo codigo de barras cadastrado'''''''''''''''''''''''''
                sql = "SELECT es1_cod, es1_codbarra FROM es1a  where es1_codbarra = '" & row.Cells("codbarra").Value & "'"

                rs.Open(sql, conexao.cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                If rs.EOF = False Then

                    row.Cells("cod_sglinx").Value = rs.Fields("es1_cod").Value

                End If

                rs.Close()



                'Rotina Busca pelo codigo fornecedor'''''''''''''''''''''''''

                If row.Cells("cod_sglinx").Value = "0" Then

                    sql = "SELECT cg2_cod , es1_cod, es1_codforn FROM es1i  where cg2_cod = '" & cod_cad_for & "' and es1_codforn = '" & row.Cells("codfor").Value & "' "

                    rs.Open(sql, cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                    If rs.EOF = True Then

                        With Me.dgvgrid_xml.Rows(row.Index).DefaultCellStyle
                            .BackColor = Color.Red
                            .ForeColor = Color.White
                        End With

                    Else

                        row.Cells("cod_sglinx").Value = rs.Fields("es1_cod").Value

                    End If

                    rs.Close()

                End If




                'Rotina Busca pelo verificar se o produto esta ativo'''''''''''''''''''''''''

                If row.Cells("cod_sglinx").Value <> "0" Then

                    sql = "SELECT es1_cod, Es1_Ativo FROM es1 where es1_cod = '" & row.Cells("cod_sglinx").Value & "'"

                    rs.Open(sql, cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                    If rs.Fields("Es1_Ativo").Value = "0" Then

                        With Me.dgvgrid_xml.Rows(row.Index).DefaultCellStyle
                            .BackColor = Color.Coral
                            .ForeColor = Color.White
                        End With


                    End If

                    rs.Close()

                End If



            Next


            cnsql.Close()


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso verifica cod SGlinx")
            cnsql.Close()
        End Try


    End Sub

    Private Sub VerificaCadastroSGLinearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerificaCadastroSGLinearToolStripMenuItem.Click
        verifica_cadastro_sglinx()
    End Sub

    Private Sub ContextMenuStrip_grid_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_grid.Opening

        Try
            If lerINI(Application.StartupPath & "\sglinear.ini", "SGLINX", "verifica_cad") = True Then
                VerificaCadastroSGLinearToolStripMenuItem.Visible = True
            Else
                VerificaCadastroSGLinearToolStripMenuItem.Visible = False
            End If
        Catch ex As Exception

        End Try

    End Sub
#End Region



    Private Sub busca_xml_consultadest_sql()
        Dim loc As Boolean = False

        Try


            conectar_mysqlDFe()

            Dim SQLcommand As MySqlCommand
            SQLcommand = oConn.CreateCommand
            SQLcommand.CommandText = "select * from NFe where Chave_nfe = '" & mskchave.Text & "'"
            dr = SQLcommand.ExecuteReader()

            While (dr.Read())
                loc = True
                Dim bytePicData() As Byte = dr("xml_nfe")

                txtcaminhoxml.Text = "Arquivo XML carregado do Banco de dados !"
                txtcaminhoxml.BackColor = Color.Khaki

                Try
                    xmldoc.LoadXml(Encoding.UTF8.GetString(bytePicData))
                Catch ex As Exception
                    xmldoc.LoadXml(Encoding.UTF8.GetString(bytePicData).Remove(0, 39))
                End Try

                dgvgrid_xml.Rows.Clear()
                txtprotocolocanc.Visible = False
                chave()
                dadosnfe()
                fatura()
                dadostrasp()
                itens_nfe()

                frminfo.Close()
                '  TabControl1.SelectTab(TabPage3)
                '  verificar_ean()
                MsgBox("Nota Fiscal carregado com Sucesso !", MsgBoxStyle.Information, "Aviso !")

            End While
            oConn.Close()

            If loc = False Then
                MsgBox("Nota Fiscal Não encontrada na Base de dados !", MsgBoxStyle.Exclamation, "Aviso !")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso !")
            oConn.Close()
        End Try

    End Sub

    Private Sub busca_xml_consultadest()
        Try


            conectar_DFe()
            '''''''''''''''''nova rotina de arquivo xml'''''''''''''''''''''''''''
            rs.Open("select * from NFe_consultadaDFe where chave_nfe = '" & mskchave.Text & "'", cn_DFe, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic)

            If rs.EOF = True Then
                MsgBox("Chave de Acessso da NFe não encontrada no Banco de dados !", MsgBoxStyle.Exclamation, "Aviso !")
                cn_DFe.Close()
                Exit Sub
            End If

            Dim bytePicData() As Byte = rs.Fields("XML_NFe").Value

            txtcaminhoxml.Text = "Arquivo XML carregado do Banco de dados !"
            txtcaminhoxml.BackColor = Color.Khaki

            Try
                xmldoc.LoadXml(Encoding.UTF8.GetString(bytePicData))
            Catch ex As Exception
                xmldoc.LoadXml(Encoding.UTF8.GetString(bytePicData).Remove(0, 39))
            End Try

            dgvgrid_xml.Rows.Clear()
            txtprotocolocanc.Visible = False
            chave()
            dadosnfe()
            fatura()
            dadostrasp()
            itens_nfe()

            frminfo.Close()
            '  TabControl1.SelectTab(TabPage3)
            '  verificar_ean()

            cn_DFe.Close()
            MsgBox("Nota Fiscal carregado com Sucesso !", MsgBoxStyle.Information, "Aviso !")

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Aviso !")
            cn_DFe.Close()
        End Try

    End Sub


    Private Sub mskchave_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskchave.MaskInputRejected

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        frmpesquisarnfe.ShowDialog()
    End Sub

    Private Sub dgvgrid_xml_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvgrid_xml.CellContentClick

    End Sub
End Class
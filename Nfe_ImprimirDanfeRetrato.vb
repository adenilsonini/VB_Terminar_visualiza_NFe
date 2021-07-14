
''CLASSE PARA IMPRESSÃO DO DANFE SEM O USO DE GERADORES
'DE RELATÓRIOS E BIBLIOTECAS DE CÓDIGO DE BARRAS DE TERCEIROS

' NOME: DANIEL COUTINHO DE MELO
'EMAIL: DANIELCPAETE@GMAIL.COM
' DATA: 24-02-2010 PORTO ALEGRE - RS

'ATUALIZAÇÃO DELWOKO VBMANIA 06/01/2010
Imports System.Drawing.Printing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic.Devices
Imports MessagingToolkit.QRCode.Codec

Public Class Nfe_ImprimirDanfeRetrato
    Dim computerInfo As New ComputerInfo
    Private ControlaImpressao As Integer = 0
    Private ContaProdutos As Integer = 0
    Private TotalFolha As Integer = 1
    Private ContaProdutos1 As Integer = 0
    Private qrencod As New QRCodeEncoder()
    Private qrcode As Bitmap
    'FONTES DA DANFE
    Private Font12_B As New Font("Times New Roman", 12, FontStyle.Bold)
    Private Font12 As New Font("Times New Roman", 12, FontStyle.Regular)
    Private Font6 As New Font("Times New Roman", 6, FontStyle.Regular)
    Private Font6_Courier As New Font("Courier New", 6, FontStyle.Regular)
    Private Font6_B As New Font("Times New Roman", 6, FontStyle.Bold)
    Private Font5 As New Font("Times New Roman", 6, FontStyle.Regular)
    Private Font5_B As New Font("Courier New", 6, FontStyle.Bold)
    Private Font12_S As New Font("Times New Roman", 12, FontStyle.Underline)
    Private Font8 As New Font("Times New Roman", 8, FontStyle.Regular)
    Private Font7 As New Font("Times New Roman", 7, FontStyle.Regular)
    Private Font10 As New Font("Times New Roman", 10, FontStyle.Regular)
    Private Font10_B As New Font("Times New Roman", 10, FontStyle.Bold)
    Private Font10_S As New Font("Times New Roman", 10, FontStyle.Underline)
    Private Font8_B As New Font("Times New Roman", 8, FontStyle.Bold)

    Private FontArial7 As New Font("Arial", 7, FontStyle.Bold)
    Private FontArial6 As New Font("Arial", 6, FontStyle.Regular)
    Private FontArial8 As New Font("Arial", 8, FontStyle.Bold)


    Public Structure Doutros
        Dim emailfor As String
        Dim numero_epec As String
        Dim protocolocanc As String
        Dim caminho_logo As String
        Dim caminho_QRCode As String
    End Structure

    'DADOS DO EMITENTE
    Public Structure DEmitente_Retrato
        Dim NOME As String
        Dim xfantNOME As String
        Dim ENDERECO_COMPLETO As String
        Dim BAIRRO As String
        Dim TELEFONE As String
        Dim CEP As String
        Dim MUNICIPIO As String
        Dim UF As String
        Dim IE As String
        Dim IESUBS As String
        Dim CNPJ As String
    End Structure
    Public Structure DFaturamento_Retrato
        Dim numero As String()
        Dim valor As String()
        Dim vencimento As String()
    End Structure
    'DADOS DO DESTINATARIO
    Public Structure DDestinatario_Retrato
        Dim NOME As String
        Dim CEP As String
        Dim ENDERECO As String
        Dim BAIRRO As String
        Dim MUNICIPIO As String
        Dim UF As String
        Dim TELEFONE As String
        Dim IE As String
        Dim CNPJ As String
    End Structure

    'DADOS DAS DATAS DA NOTA EMISSAO DATA SAIDA E HORA SAIDA
    Public Structure DDataeHora_Retrato
        Dim DATA_EMISSAO As Date
        Dim DATA_ENTRADA_SAIDA As Date
        Dim HORA_ENTRADA_SAIDA As String
    End Structure
    ' Nr da Nota Fiscal Chave de Acesso e Protocolo
    Public Structure DDadosNfe_Retrato
        Dim IMP_DV As String
        Dim NUMERO_COPIAS As String
        Dim NUMERO_NFE As String
        Dim SERIE_NFE As String
        Dim xJust As String
        Dim DHCONT As Date
        Dim CHAVEACESSO_NFE As String
        Dim PROTOCOLO_NFE As String
        Dim DHRECBTO_NFE As String
        Dim TIPONOTA_NFE As String
        Dim NATUREZA_NFE As String
    End Structure

    'DADOS DOS VALORES TOTAIS 
    Public Structure DValores_Retrato
        Dim BASE_CALCULO_ICMS As String
        Dim VALOR_ICMS As String
        Dim VALOR_TOTALTRIB As String
        Dim BASE_CALCULO_ICMS_SUBS As String
        Dim VALOR_ICMS_SUBS As String
        Dim VALOR_TOTAL_PRODUTOS As String
        Dim VALOR_FRETE As String
        Dim VALOR_SEGURO As String
        Dim VALOR_PIS As String
        Dim VALOR_COFINS As String
        Dim DESCONTO As String
        Dim OUTRAS_DESPESAS As String
        Dim VALOR_IPI As String
        Dim VALOR_TOTAL_NOTA As String
    End Structure

    Public Enum TipoNota_Retrato
        ENTRADA = 0
        SAIDA = 1
    End Enum

    'DADOS DA TRANSPORTADORA E VOLUME
    Public Structure DTransportadora_Retrato
        Dim NOME As String
        Dim CEP As String
        Dim modfrete As String
        Dim ENDERECO As String
        Dim MUNICIPIO As String
        Dim PLACA_VEICULO As String
        Dim UF_PLACA As String
        Dim CODIGO_ANTT As String
        Dim UF As String
        Dim IE As String
        Dim CNPJ As String
        Dim QUANTIDADE As String
        Dim ESPECIE As String
        Dim MARCA As String
        Dim NUMERO As String
        Dim PESOBRUTO As String
        Dim PESOLIQUIDO As String
    End Structure

    'DADOS DO ISSQN
    Public Structure DISSQN_Retrato
        Dim IM As String
        Dim VALOR_TOTAL_SERVICOS As String
        Dim BASE_CALCULO_ISSQN As String
        Dim VALOR_ISSQN As String
    End Structure

    'DADOS DO ISSQN
    Public Structure DINFOCOMPLEMENTAR_Retrato
        Dim DADOSADIC As String
    End Structure

    'VARIAVEIS QUE VÃO ARMAZENAR OS DADOS DA NOTA
    Private V_DEmitente_Retrato As DEmitente_Retrato
    Private V_DDestinatario_Retrato As DDestinatario_Retrato
    Private V_DDataeHora_Retrato As DDataeHora_Retrato
    Private V_DValores_Retrato As DValores_Retrato
    Private V_DFaturamento_Retrato As DFaturamento_Retrato
    Private V_DTransportadora_Retrato As DTransportadora_Retrato
    Private V_DISSQN_Retrato As DISSQN_Retrato
    Private I_DDadosNfe_Retrato As DDadosNfe_Retrato
    Private I_Doutros_Retrato As Doutros
    Private INFOCOMP_Retrato As DINFOCOMPLEMENTAR_Retrato

    'MATRIZ DE CLASSES DE PRODUTOS DA DANFE
    'CADA PRODUTO DA DANFE É UMA CLASSE
    '----------------------------------------------------------
    Private V_PRODUTOS_Retrato As IList(Of ProdutoDanfe_Retrato)
    Public Property AddProdutosDanfe() As IList(Of ProdutoDanfe_Retrato)
        Get
            Return V_PRODUTOS_Retrato
        End Get
        Set(ByVal value As IList(Of ProdutoDanfe_Retrato))
            V_PRODUTOS_Retrato = value
        End Set
    End Property


    Private V_FATURA_Retrato As IList(Of FaturaDanfe_Retrato)
    Public Property AddfaturaDanfe() As IList(Of FaturaDanfe_Retrato)
        Get
            Return V_FATURA_Retrato
        End Get
        Set(ByVal value As IList(Of FaturaDanfe_Retrato))
            V_FATURA_Retrato = value
        End Set
    End Property

    Public WriteOnly Property Dados_outros() As Doutros
        Set(ByVal outros As Doutros)
            I_Doutros_Retrato = outros
        End Set
    End Property

    Public WriteOnly Property Dados_Nfe() As DDadosNfe_Retrato
        Set(ByVal Valor As DDadosNfe_Retrato)
            I_DDadosNfe_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property Identificacao_Emitente() As DEmitente_Retrato
        Set(ByVal Valor As DEmitente_Retrato)
            V_DEmitente_Retrato = Valor
        End Set
    End Property

    Public WriteOnly Property Identificacao_Destinatario() As DDestinatario_Retrato
        Set(ByVal Valor As DDestinatario_Retrato)
            V_DDestinatario_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property Data_Hora() As DDataeHora_Retrato
        Set(ByVal Valor As DDataeHora_Retrato)
            V_DDataeHora_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property Valores_Nota() As DValores_Retrato
        Set(ByVal Valor As DValores_Retrato)
            V_DValores_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property Faturamento() As DFaturamento_Retrato
        Set(ByVal Valor As DFaturamento_Retrato)
            V_DFaturamento_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property Identificacao_Transportadora() As DTransportadora_Retrato
        Set(ByVal Valor As DTransportadora_Retrato)
            V_DTransportadora_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property Valores_ISSQN() As DISSQN_Retrato
        Set(ByVal Valor As DISSQN_Retrato)
            V_DISSQN_Retrato = Valor
        End Set
    End Property
    Public WriteOnly Property InformacoesComplementares() As DINFOCOMPLEMENTAR_Retrato
        Set(ByVal Valor As DINFOCOMPLEMENTAR_Retrato)
            INFOCOMP_Retrato = Valor
        End Set
    End Property

    'SEMPRE QUE UMA CHAMADA A DANFE É FEITA A CHAVE DE ACESSO DEVE SER INFORMADA
    Public Sub New()
        'CRIA UMA NOVA LISTA GENERICA DE CLASSES DE PRODUTOS
        V_PRODUTOS_Retrato = New List(Of ProdutoDanfe_Retrato)()
        V_FATURA_Retrato = New List(Of FaturaDanfe_Retrato)()
    End Sub


    Public Sub VisualizarImpressao()
        Dim Prv_Visualizador As New PrintPreviewDialog
        Dim Doc_VisualizarDanfe As New PrintDocument
        'Dim Prn_Dialogo As New PrintDialog

        'ASSOCIA OS EVENTOS DE IMPRESSÃO COM MINHAS FUNÇÕES
        AddHandler Doc_VisualizarDanfe.BeginPrint, AddressOf InicioImpressao
        AddHandler Doc_VisualizarDanfe.PrintPage, AddressOf ImprimirDanfe

        Doc_VisualizarDanfe.DefaultPageSettings.Landscape = False
        Doc_VisualizarDanfe.DefaultPageSettings.PrinterSettings.Copies = I_DDadosNfe_Retrato.NUMERO_COPIAS

        'Doc_VisualizarDanfe.OriginAtMargins = False

        'AS MARGENS PRECISAM SER AJUSTADAS PARA QUE A IMPRESSÃO INICIAL TENHA 5 MILIMETROS
        'Doc_VisualizarDanfe.DefaultPageSettings.Margins = New Printing.Margins(11, 11, 11, 11)

        If I_DDadosNfe_Retrato.IMP_DV = "SIM" Then


            Prv_Visualizador.Document = Doc_VisualizarDanfe
            frmdanfe.PrintPreviewControl1.Document = Doc_VisualizarDanfe


            frmdanfe.ShowDialog()


        End If

    End Sub
    Public Sub InicioImpressao()
        'ZERA VARIAVEIS DE CONTROLE DE IMPRESSÃO
        ControlaImpressao = 0
        ContaProdutos = 0
        TotalFolha = 1
    End Sub

    Public Sub ImprimirDanfe(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        'VERIFICA SE É PRIMEIRA FOLHA OU DEMAIS PARA FORMATAR A IMPRESSÃO
        If ControlaImpressao = 0 Then
            InserirCabecalho(e)
            Preencher_Dados_Danfe(e)
            Imprimir_Produtos(e)
        Else
            InserirCabecalho(e)
            Imprimir_Produtos(e)
        End If
    End Sub

    'GERA CABEÇALHO E GUIA DO CLIENTE
    Private Sub InserirCabecalho(ByVal Gra_Saida As System.Drawing.Printing.PrintPageEventArgs)

        Dim pen As New Pen(Brushes.Black, 0.5)

        Dim CodigoBarra As CodigodeBarra
        CodigoBarra = New CodigodeBarra(CodigodeBarra.BCEncoding.Code128C)
        Gra_Saida.Graphics.DrawImage(CodigoBarra.DrawBarCode(I_DDadosNfe_Retrato.CHAVEACESSO_NFE), 467, 20, 355, 39)
        '  Gra_Saida.Graphics.DrawString("CONTROLE DO FISCO", Font5_B, Brushes.Black, 570, 20, New StringFormat)
        Gra_Saida.Graphics.DrawString("IDENTIFICAÇÃO DO EMITENTE", Font5_B, Brushes.Black, 110, 20, New StringFormat)
        'quadro de informações sobre o Emitente
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 20, 335, 129)

        If V_DEmitente_Retrato.NOME.Length > 30 Then
            Dim Alinhamento As New StringFormat()
            Alinhamento.Alignment = StringAlignment.Center
            Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.NOME, Font12_B, Brushes.Black, New RectangleF(12, 28, 330, 50), Alinhamento)
        Else
            Dim Alinhamento As New StringFormat()
            Alinhamento.Alignment = StringAlignment.Center
            Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.NOME, Font12_B, Brushes.Black, New RectangleF(12, 35, 330, 50), Alinhamento)
        End If

        If V_DEmitente_Retrato.ENDERECO_COMPLETO.Length > 40 Then
            Dim Alinhamento1 As New StringFormat()
            Alinhamento1.Alignment = StringAlignment.Near
            Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.ENDERECO_COMPLETO, FontArial7, Brushes.Black, New RectangleF(94, 65, 250, 50), Alinhamento1)
        Else
            Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.ENDERECO_COMPLETO, FontArial7, Brushes.Black, 94, 76, New StringFormat)
        End If

        Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.BAIRRO & " - " & V_DEmitente_Retrato.CEP, FontArial7, Brushes.Black, 94, 89, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.MUNICIPIO, FontArial7, Brushes.Black, 94, 102, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.TELEFONE, FontArial7, Brushes.Black, 94, 115, New StringFormat)

        Gra_Saida.Graphics.DrawString(I_Doutros_Retrato.emailfor, FontArial7, Brushes.Black, 94, 128, New StringFormat)

        '''''''''''''''''Carrega a imagem do logo da NF-e para o DANFE'''''''''''''''''''   
        Try
            Gra_Saida.Graphics.DrawImage(Image.FromFile(I_Doutros_Retrato.caminho_logo), 10, 60, 85, 85)
        Catch ex As Exception

        End Try


        '''''''''''''''''Carrega a imagem do QRCode da NF-e para o DANFE'''''''''''''''''''   
        Try


            qrcode = qrencod.Encode(I_DDadosNfe_Retrato.CHAVEACESSO_NFE)


            Gra_Saida.Graphics.DrawImage(TryCast(qrcode, Image), 708, 64, 85, 85)

        Catch ex As Exception

        End Try

        ' Gra_Saida.Graphics.DrawImage(Image.FromFile("logo.gif"), 5, 54, 90, 100)

        Gra_Saida.Graphics.DrawString("DANFE", Font12_B, Brushes.Black, 370, 23, New StringFormat)
        Gra_Saida.Graphics.DrawString(" Documento Auxiliar da Nota", FontArial6, Brushes.Black, 350, 43, New StringFormat)
        Gra_Saida.Graphics.DrawString("         Fiscal Eletrônica", FontArial6, Brushes.Black, 350, 53, New StringFormat)
        Gra_Saida.Graphics.DrawString("0 - ENTRADA", FontArial8, Brushes.Black, 350, 68, New StringFormat)
        Gra_Saida.Graphics.DrawString("1 - SAÍDA", FontArial8, Brushes.Black, 350, 83, New StringFormat)
        Gra_Saida.Graphics.DrawString("N.º   " & Int32.Parse(I_DDadosNfe_Retrato.NUMERO_NFE).ToString("000,000,000"), FontArial8, Brushes.Black, 350, 98, New StringFormat)
        Gra_Saida.Graphics.DrawString("SÉRIE  " & Int32.Parse(I_DDadosNfe_Retrato.SERIE_NFE).ToString("000"), FontArial8, Brushes.Black, 350, 115, New StringFormat)
        '   Gra_Saida.Graphics.DrawString("FOLHA  " & ControlaImpressao + 1 & " / " & Conte, FontArial7, Brushes.Black, 350, 133, New StringFormat)
        Gra_Saida.Graphics.DrawRectangle(pen, 438, 71, 20, 22)
        Gra_Saida.Graphics.DrawString(I_DDadosNfe_Retrato.TIPONOTA_NFE, Font10, Brushes.Black, 443, 75, New StringFormat)

        ' Dados Nfe
        ''''''''''''''''''''''''''''''''''''''dir''bai''l''''sima
        Gra_Saida.Graphics.DrawRectangle(pen, 465, 20, 330, 129)
        Gra_Saida.Graphics.DrawLine(pen, 465, 68, 705, 68)
        Gra_Saida.Graphics.DrawLine(pen, 465, 94, 705, 94)
        Gra_Saida.Graphics.DrawLine(pen, 465, 178, 795, 178)
        '''''''''''''''''''''''

        'CALCULA TOTAL DE FOLHAS
        Dim Resto As Integer
        If V_PRODUTOS_Retrato.Count > 3 Then
            Resto = (V_PRODUTOS_Retrato.Count - 3) Mod 8
            If Resto > 0 Then
                TotalFolha = 2 + ((V_PRODUTOS_Retrato.Count - 3) - Resto) / 8
            Else
                TotalFolha = 1 + ((V_PRODUTOS_Retrato.Count - 3) - Resto) / 8
            End If
        End If


        Dim LimitePagina As Integer
        Dim AlturaLinha As Integer

        LimitePagina = 150
        AlturaLinha = 115

        Dim Conte As Integer = 1
        Dim ContaProd As Integer = 0
        ContaProdutos1 = 0
        For Each PDanfe As ProdutoDanfe_Retrato In V_PRODUTOS_Retrato
            ContaProd += 1
            If ContaProdutos1 < Conte Then
                AlturaLinha += 2.8
                Dim ContaPreen As Integer = 0
                Dim SLinhaLote As String = ""
                For Each LinhaAdi In PDanfe.LinhaProd
                    ContaPreen += 1
                    SLinhaLote &= " " & LinhaAdi.ToString
                    If ContaPreen = 3 Then
                        ContaPreen = 0
                        SLinhaLote = ""
                        AlturaLinha += 2.8
                    End If
                Next
                If SLinhaLote <> "" Then
                    AlturaLinha += 2.8
                End If

                If AlturaLinha > LimitePagina And ContaProd < V_PRODUTOS_Retrato.Count Then
                    Conte = Conte + 1
                    AlturaLinha = 52
                    LimitePagina = 150
                End If
            End If
        Next



        Gra_Saida.Graphics.DrawString("FOLHA  " & Int32.Parse(ControlaImpressao + 1).ToString("000") & " / " & Int32.Parse(Conte).ToString("000"), FontArial7, Brushes.Black, 350, 133, New StringFormat)
        '''''''''''''linha acama protocolo'''''''''''
        ' Gra_Saida.Graphics.DrawLine(pen, 465, 203, 795, 203)


        Dim NChave As String = ""
        Dim ContarS As Integer = 0
        For x As Int16 = 0 To I_DDadosNfe_Retrato.CHAVEACESSO_NFE.Length - 1
            ContarS = ContarS + 1
            NChave = NChave & I_DDadosNfe_Retrato.CHAVEACESSO_NFE.Substring(x, 1)
            If ContarS = 4 Then
                ContarS = 0
                NChave = NChave & " "
            End If
        Next


        Gra_Saida.Graphics.DrawString("CHAVE DE ACESSO", FontArial6, Brushes.Black, 468, 69, New StringFormat)
        Gra_Saida.Graphics.DrawString(NChave, FontArial6, Brushes.Black, 468, 78, New StringFormat)
        frmdanfe.txtchavenfe.Text = NChave.Replace(" ", "")

        Dim Alinhamentode As New StringFormat()
        Alinhamentode.Alignment = StringAlignment.Center
        Gra_Saida.Graphics.DrawString("Consulta de autenticidade no portal nacional da NF-e www.nfe.fazenda.gov.br/portal ou no site da Sefaz Autorizadora", New Font("Times New Roman", 9, FontStyle.Regular), Brushes.Black, New RectangleF(460, 100, 245, 50), Alinhamentode)
        If frmxml.txtdepc.Text = "" Then
            Gra_Saida.Graphics.DrawString("PROTOCOLO DE AUTORIZAÇÃO DE USO", FontArial6, Brushes.Black, 470, 152, New StringFormat)
            Gra_Saida.Graphics.DrawString(I_DDadosNfe_Retrato.PROTOCOLO_NFE & "  " & I_DDadosNfe_Retrato.DHRECBTO_NFE, FontArial8, Brushes.Black, 530, 161, New StringFormat)
        Else
            Gra_Saida.Graphics.DrawString("NÚMERO DE REGISTRO EPEC", FontArial6, Brushes.Black, 470, 152, New StringFormat)
            Gra_Saida.Graphics.DrawString(I_Doutros_Retrato.numero_epec, FontArial8, Brushes.Black, 530, 161, New StringFormat)
        End If

        Gra_Saida.Graphics.DrawRectangle(pen, 465, 151, 330, 54)
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 151, 453, 54)
        Gra_Saida.Graphics.DrawLine(pen, 10, 178, 463, 178)
        Gra_Saida.Graphics.DrawLine(pen, 230, 178, 230, 205)

        Gra_Saida.Graphics.DrawString("NATUREZA DE OPERACAO", Font5_B, Brushes.Black, 12, 152, New StringFormat)
        Gra_Saida.Graphics.DrawString(I_DDadosNfe_Retrato.NATUREZA_NFE, FontArial7, Brushes.Black, 12, 163, New StringFormat)
        Gra_Saida.Graphics.DrawString("INSCRIÇÃO ESTADUAL", Font5_B, Brushes.Black, 12, 178, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.IE, FontArial7, Brushes.Black, 12, 189, New StringFormat)
        Gra_Saida.Graphics.DrawString("INSCR EST DO SUBST TRIBUTARIO", Font5_B, Brushes.Black, 232, 178, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.IESUBS, FontArial7, Brushes.Black, 260, 189, New StringFormat)
        Gra_Saida.Graphics.DrawString("CNPJ", Font5_B, Brushes.Black, 470, 178, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DEmitente_Retrato.CNPJ, New Font("Times New Roman", 10, FontStyle.Regular), Brushes.Black, 530, 189, New StringFormat)


    End Sub

    'PREENCHE DADOS QUANDO É PRIMEIRA PAGINA
    Private Sub Preencher_Dados_Danfe(ByVal Gra_Saida As System.Drawing.Printing.PrintPageEventArgs)
        Dim cont_fat As Integer
        Dim pen As New Pen(Brushes.Black, 0.5)
        ' REMETENTE
        ' Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 208, 785, 12)
        Gra_Saida.Graphics.DrawString("DESTINATARIO/REMETENTE", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, 207, New StringFormat)

        'quadrado abaixo aredor da informacoes do dest
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 218, 785, 81)

        'quadrado abaixo entre data de emissao e saida  do dest
        Gra_Saida.Graphics.DrawRectangle(pen, 670, 218, 125, 54)

        'linha horizontal abaixo entre Razasocial eendereco  do dest
        Gra_Saida.Graphics.DrawLine(pen, 10, 245, 795, 245)
        'linha horizontal abaixo entre endereco e municipio do dest
        Gra_Saida.Graphics.DrawLine(pen, 10, 272, 795, 272)
        'linha vertical abaixo na frente da nome dest
        Gra_Saida.Graphics.DrawLine(pen, 470, 218, 470, 245)
        'linha vertical abaixo na frente da endereco dest
        Gra_Saida.Graphics.DrawLine(pen, 390, 245, 390, 272)
        'linha vertical abaixo na frente da bairro dest
        Gra_Saida.Graphics.DrawLine(pen, 590, 245, 590, 272)
        'linha vertical abaixo na frente da municipio dest
        Gra_Saida.Graphics.DrawLine(pen, 260, 272, 260, 299)
        'linha vertical abaixo na frente da fone dest
        Gra_Saida.Graphics.DrawLine(pen, 440, 272, 440, 299)
        'linha vertical abaixo na frente da uf dest
        Gra_Saida.Graphics.DrawLine(pen, 480, 272, 480, 299)


        Gra_Saida.Graphics.DrawString("NOME/RAZÃO SOCIAL", Font5_B, Brushes.Black, 12, 218, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.NOME, FontArial7, Brushes.Black, 12, 232, New StringFormat)
        Gra_Saida.Graphics.DrawString("CNPJ/CPF", Font5_B, Brushes.Black, 472, 218, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.CNPJ, FontArial7, Brushes.Black, 472, 232, New StringFormat)
        Gra_Saida.Graphics.DrawString("DATA E HORA EMISSÃO", Font5_B, Brushes.Black, 678, 218, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDataeHora_Retrato.DATA_EMISSAO.ToString("dd/MM/yyyy HH:mm:ss"), FontArial7, Brushes.Black, 678, 232)
        ''''''''''''''''''''''''''############################'''''''''''''''''''''''''''''''''''''''''''''''''
        Gra_Saida.Graphics.DrawString("ENDEREÇO", Font5_B, Brushes.Black, 12, 245, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.ENDERECO, FontArial7, Brushes.Black, 12, 259, New StringFormat)
        Gra_Saida.Graphics.DrawString("BAIRRO/DISTRITO", Font5_B, Brushes.Black, 392, 245, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.BAIRRO, FontArial7, Brushes.Black, 392, 259, New StringFormat)
        Gra_Saida.Graphics.DrawString("CEP", Font5_B, Brushes.Black, 592, 245, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.CEP, FontArial7, Brushes.Black, 592, 259, New StringFormat)
        Gra_Saida.Graphics.DrawString("DATA E HORA SAÍDA", Font5_B, Brushes.Black, 678, 245, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDataeHora_Retrato.DATA_ENTRADA_SAIDA.ToString("dd/MM/yyyy HH:mm:ss"), FontArial7, Brushes.Black, 678, 259)

        Gra_Saida.Graphics.DrawString("MUNICIPIO", Font5_B, Brushes.Black, 12, 272, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.MUNICIPIO, FontArial7, Brushes.Black, 12, 286, New StringFormat)
        Gra_Saida.Graphics.DrawString("FONE/FAX", Font5_B, Brushes.Black, 262, 272, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.TELEFONE, FontArial7, Brushes.Black, 262, 286, New StringFormat)
        Gra_Saida.Graphics.DrawString("UF", Font5_B, Brushes.Black, 442, 272, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.UF, FontArial7, Brushes.Black, 442, 286, New StringFormat)
        Gra_Saida.Graphics.DrawString("INSCRIÇÃO ESTADUAL", Font5_B, Brushes.Black, 482, 272, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DDestinatario_Retrato.IE, FontArial7, Brushes.Black, 482, 286, New StringFormat)
        '   Gra_Saida.Graphics.DrawString("HORA SAIDA/ENTRADA", Font5_B, Brushes.Black, 678, 272, New StringFormat)
        ' Gra_Saida.Graphics.DrawString(V_DDataeHora_Retrato.HORA_ENTRADA_SAIDA, FontArial7, Brushes.Black, 678, 286, New StringFormat)

        'FATURA
        ''''''''''''''''Bordas''''''''''''''''''''''''''''
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 314, 785, 40)
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 314, 785, 14)

        Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 315, 785, 12)
        Gra_Saida.Graphics.DrawString("FATURA/DUPLICATA", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, 302, New StringFormat)

        Gra_Saida.Graphics.DrawLine(pen, 90, 315, 90, 354)
        Gra_Saida.Graphics.DrawString("N° DUPLICATA", Font5_B, Brushes.Black, 12, 316, New StringFormat)
        Gra_Saida.Graphics.DrawLine(pen, 180, 315, 180, 354)
        Gra_Saida.Graphics.DrawString("VENCIMENTO", Font5_B, Brushes.Black, 93, 316, New StringFormat)
        Gra_Saida.Graphics.DrawLine(pen, 270, 315, 270, 354)
        Gra_Saida.Graphics.DrawString("VALOR R$", Font5_B, Brushes.Black, 184, 316, New StringFormat)

        Gra_Saida.Graphics.DrawLine(pen, 360, 315, 360, 354)
        Gra_Saida.Graphics.DrawString("N° DUPLICATA", Font5_B, Brushes.Black, 275, 316, New StringFormat)
        Gra_Saida.Graphics.DrawLine(pen, 450, 315, 450, 354)
        Gra_Saida.Graphics.DrawString("VENCIMENTO", Font5_B, Brushes.Black, 366, 316, New StringFormat)
        Gra_Saida.Graphics.DrawLine(pen, 540, 315, 540, 354)
        Gra_Saida.Graphics.DrawString("VALOR R$", Font5_B, Brushes.Black, 457, 316, New StringFormat)

        Gra_Saida.Graphics.DrawLine(pen, 630, 315, 630, 354)
        Gra_Saida.Graphics.DrawString("N° DUPLICATA", Font5_B, Brushes.Black, 548, 316, New StringFormat)
        Gra_Saida.Graphics.DrawLine(pen, 720, 315, 720, 354)
        Gra_Saida.Graphics.DrawString("VENCIMENTO", Font5_B, Brushes.Black, 639, 316, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR R$", Font5_B, Brushes.Black, 730, 316, New StringFormat)



        For Each PDanfe_fat As FaturaDanfe_Retrato In V_FATURA_Retrato

            '  MsgBox(PDanfe_fat.Dvenc)
            If cont_fat = "0" Then
                Gra_Saida.Graphics.DrawString(PDanfe_fat.DNumero, Font5_B, Brushes.Black, 12, 330, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvenc, Font5_B, Brushes.Black, 93, 330, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvalor, Font5_B, Brushes.Black, 184, 330, New StringFormat)
            End If

            If cont_fat = "1" Then
                Gra_Saida.Graphics.DrawString(PDanfe_fat.DNumero, Font5_B, Brushes.Black, 275, 330, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvenc, Font5_B, Brushes.Black, 366, 330, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvalor, Font5_B, Brushes.Black, 457, 330, New StringFormat)
            End If

            If cont_fat = "2" Then
                Gra_Saida.Graphics.DrawString(PDanfe_fat.DNumero, Font5_B, Brushes.Black, 548, 330, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvenc, Font5_B, Brushes.Black, 639, 330, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvalor, Font5_B, Brushes.Black, 730, 330, New StringFormat)
            End If

            If cont_fat = "3" Then
                Gra_Saida.Graphics.DrawString(PDanfe_fat.DNumero, Font5_B, Brushes.Black, 12, 340, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvenc, Font5_B, Brushes.Black, 93, 340, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvalor, Font5_B, Brushes.Black, 184, 340, New StringFormat)
            End If

            If cont_fat = "4" Then
                Gra_Saida.Graphics.DrawString(PDanfe_fat.DNumero, Font5_B, Brushes.Black, 275, 340, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvenc, Font5_B, Brushes.Black, 366, 340, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvalor, Font5_B, Brushes.Black, 457, 340, New StringFormat)
            End If

            If cont_fat = "5" Then
                Gra_Saida.Graphics.DrawString(PDanfe_fat.DNumero, Font5_B, Brushes.Black, 548, 340, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvenc, Font5_B, Brushes.Black, 639, 340, New StringFormat)
                Gra_Saida.Graphics.DrawString(PDanfe_fat.Dvalor, Font5_B, Brushes.Black, 730, 340, New StringFormat)
            End If

            cont_fat += 1

        Next




        '  Gra_Saida.Graphics.DrawString(V_DFaturamento_Retrato.Faturamento, FontArial7, Brushes.Black, 12, 350, New StringFormat)

        ' VALORES
        Dim Alinhamento As New StringFormat()
        Alinhamento.Alignment = StringAlignment.Far
        'Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 357, 785, 12)
        Gra_Saida.Graphics.DrawString("CÁLCULO DO IMPOSTO", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, 357, New StringFormat)
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 370, 785, 56)
        Gra_Saida.Graphics.DrawLine(pen, 10, 398, 795, 398)
        Gra_Saida.Graphics.DrawLine(pen, 140, 370, 140, 398)
        Gra_Saida.Graphics.DrawLine(pen, 238, 370, 238, 398)
        Gra_Saida.Graphics.DrawLine(pen, 342, 370, 342, 398)
        Gra_Saida.Graphics.DrawLine(pen, 454, 370, 454, 426)

        Gra_Saida.Graphics.DrawLine(pen, 560, 370, 560, 426)
        Gra_Saida.Graphics.DrawLine(pen, 648, 370, 648, 426)

        Gra_Saida.Graphics.DrawString("BASE DE CÁLCULO DE ICMS", Font5_B, Brushes.Black, 12, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR ICMS", Font5_B, Brushes.Black, 142, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString("VLR.APROX.TRIBUTO", Font5_B, Brushes.Black, 240, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString("BASE CÁLCULO ICMS ST", Font5_B, Brushes.Black, 345, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR ICMS ST", Font5_B, Brushes.Black, 458, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR DO PIS", Font5_B, Brushes.Black, 563, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR TOTAL DOS PRODUTOS", Font5_B, Brushes.Black, 652, 370, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.BASE_CALCULO_ICMS, FontArial8, Brushes.Black, 135, 382, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_ICMS, FontArial8, Brushes.Black, 236, 382, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_TOTALTRIB, FontArial8, Brushes.Black, 340, 382, Alinhamento)

        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.BASE_CALCULO_ICMS_SUBS, FontArial8, Brushes.Black, 452, 382, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_ICMS_SUBS, FontArial8, Brushes.Black, 558, 382, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_PIS, FontArial8, Brushes.Black, 646, 382, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_TOTAL_PRODUTOS, FontArial8, Brushes.Black, 790, 382, Alinhamento)

        Gra_Saida.Graphics.DrawLine(pen, 114, 398, 114, 426)
        Gra_Saida.Graphics.DrawLine(pen, 238, 398, 238, 426)
        Gra_Saida.Graphics.DrawLine(pen, 342, 398, 342, 426)


        Gra_Saida.Graphics.DrawString("VALOR DO FRETE", Font5_B, Brushes.Black, 12, 398, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR DO SEGURO", Font5_B, Brushes.Black, 116, 398, New StringFormat)
        Gra_Saida.Graphics.DrawString("DESCONTO", Font5_B, Brushes.Black, 240, 398, New StringFormat)
        Gra_Saida.Graphics.DrawString("OUTRAS DESPESAS", Font5_B, Brushes.Black, 348, 398, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR DO IPI", Font5_B, Brushes.Black, 458, 398, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR DA COFINS", Font5_B, Brushes.Black, 563, 398, New StringFormat)
        Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 648, 399, 146, 27)
        Gra_Saida.Graphics.DrawString("VALOR TOTAL DA NOTA", Font5_B, Brushes.Black, 652, 398, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_FRETE, FontArial8, Brushes.Black, 112, 410, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_SEGURO, FontArial8, Brushes.Black, 236, 410, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.DESCONTO, FontArial8, Brushes.Black, 340, 410, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.OUTRAS_DESPESAS, FontArial8, Brushes.Black, 452, 410, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_IPI, FontArial8, Brushes.Black, 558, 410, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_COFINS, FontArial8, Brushes.Black, 646, 410, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DValores_Retrato.VALOR_TOTAL_NOTA, FontArial8, Brushes.Black, 790, 410, Alinhamento)

        'TRANSPORTADORA
        ' Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 429, 785, 12)
        Gra_Saida.Graphics.DrawString("TRANSPORTADOR/VOLUMES TRANSPORTADOS", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, 427, New StringFormat)
        ' 1º linha 
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 439, 785, 78)
        Gra_Saida.Graphics.DrawLine(pen, 10, 465, 795, 465)
        Gra_Saida.Graphics.DrawLine(pen, 533, 439, 533, 465)
        Gra_Saida.Graphics.DrawLine(pen, 450, 439, 450, 465)
        Gra_Saida.Graphics.DrawLine(pen, 353, 439, 353, 465)
        Gra_Saida.Graphics.DrawLine(pen, 620, 439, 620, 484)
        Gra_Saida.Graphics.DrawLine(pen, 650, 439, 650, 484)

        Gra_Saida.Graphics.DrawString("RAZÃO SOCIAL", Font5_B, Brushes.Black, 12, 439, New StringFormat)
        Gra_Saida.Graphics.DrawString("FRETE POR CONTA", Font5_B, Brushes.Black, 355, 439, New StringFormat)
        Gra_Saida.Graphics.DrawString("CODIGO ANTT", Font5_B, Brushes.Black, 452, 439, New StringFormat)
        Gra_Saida.Graphics.DrawString("PLACA VEICULO", Font5_B, Brushes.Black, 533, 439, New StringFormat)
        Gra_Saida.Graphics.DrawString("UF", Font5_B, Brushes.Black, 622, 439, New StringFormat)
        Gra_Saida.Graphics.DrawString("CNPJ/CPF", Font5_B, Brushes.Black, 652, 439, New StringFormat)

        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.NOME, FontArial7, Brushes.Black, 12, 450, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.modfrete, Font5_B, Brushes.Black, 355, 450, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.CODIGO_ANTT, FontArial7, Brushes.Black, 452, 450, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.PLACA_VEICULO, FontArial7, Brushes.Black, 533, 450, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.UF_PLACA, FontArial7, Brushes.Black, 622, 450, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.CNPJ, FontArial7, Brushes.Black, 652, 450, New StringFormat)

        ' 2º linha 
        Gra_Saida.Graphics.DrawLine(pen, 10, 492, 795, 492)
        Gra_Saida.Graphics.DrawLine(pen, 370, 465, 370, 492)
        Gra_Saida.Graphics.DrawLine(pen, 620, 484, 620, 492)
        Gra_Saida.Graphics.DrawLine(pen, 650, 484, 650, 504)

        Gra_Saida.Graphics.DrawString("ENDEREÇO", Font5_B, Brushes.Black, 12, 465, New StringFormat)
        Gra_Saida.Graphics.DrawString("MUNICIPIO", Font5_B, Brushes.Black, 372, 465, New StringFormat)
        Gra_Saida.Graphics.DrawString("UF", Font5_B, Brushes.Black, 622, 465, New StringFormat)
        Gra_Saida.Graphics.DrawString("INSCRIÇÃO ESTADUAL", Font5_B, Brushes.Black, 652, 465, New StringFormat)

        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.ENDERECO, FontArial7, Brushes.Black, 12, 476, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.MUNICIPIO, FontArial7, Brushes.Black, 372, 476, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.UF, FontArial7, Brushes.Black, 622, 476, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.IE, FontArial7, Brushes.Black, 652, 476, New StringFormat)

        ' 3º linha 
        Gra_Saida.Graphics.DrawLine(pen, 100, 492, 100, 517)
        Gra_Saida.Graphics.DrawLine(pen, 210, 492, 210, 517)
        Gra_Saida.Graphics.DrawLine(pen, 320, 492, 320, 517)
        Gra_Saida.Graphics.DrawLine(pen, 500, 492, 500, 517)
        Gra_Saida.Graphics.DrawLine(pen, 650, 492, 650, 517)

        Gra_Saida.Graphics.DrawString("QUANTIDADE", Font5_B, Brushes.Black, 12, 492, New StringFormat)
        Gra_Saida.Graphics.DrawString("ESPÉCIE", Font5_B, Brushes.Black, 102, 492, New StringFormat)
        Gra_Saida.Graphics.DrawString("MARCA", Font5_B, Brushes.Black, 212, 492, New StringFormat)
        Gra_Saida.Graphics.DrawString("NUMERAÇÃO", Font5_B, Brushes.Black, 322, 492, New StringFormat)
        Gra_Saida.Graphics.DrawString("PESO BRUTO", Font5_B, Brushes.Black, 502, 492, New StringFormat)
        Gra_Saida.Graphics.DrawString("PESO LÍQUIDO", Font5_B, Brushes.Black, 652, 492, New StringFormat)

        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.QUANTIDADE, FontArial7, Brushes.Black, 90, 507, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.ESPECIE, FontArial7, Brushes.Black, 102, 507, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.MARCA, FontArial7, Brushes.Black, 212, 507, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.NUMERO, FontArial7, Brushes.Black, 322, 507, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.PESOBRUTO, FontArial7, Brushes.Black, 640, 507, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DTransportadora_Retrato.PESOLIQUIDO, FontArial7, Brushes.Black, 780, 507, Alinhamento)

        'CÁLCULO DO ISSQN
        '  Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 885, 785, 12)
        Gra_Saida.Graphics.DrawString("CÁLCULO DO ISSQN", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, 878, New StringFormat)
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 888, 785, 27)
        Gra_Saida.Graphics.DrawLine(pen, 194, 888, 194, 915)
        Gra_Saida.Graphics.DrawLine(pen, 388, 888, 388, 915)
        Gra_Saida.Graphics.DrawLine(pen, 582, 888, 582, 915)
        Gra_Saida.Graphics.DrawString("INSCRIÇÃO MUNICIPAL", Font5_B, Brushes.Black, 12, 888, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR TOTAL DOS SERVIÇOS", Font5_B, Brushes.Black, 196, 888, New StringFormat)
        Gra_Saida.Graphics.DrawString("BASE DE CALCULO DO ISSQN", Font5_B, Brushes.Black, 390, 888, New StringFormat)
        Gra_Saida.Graphics.DrawString("VALOR DO ISSQN", Font5_B, Brushes.Black, 584, 888, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DISSQN_Retrato.IM, FontArial7, Brushes.Black, 12, 902, New StringFormat)
        Gra_Saida.Graphics.DrawString(V_DISSQN_Retrato.VALOR_TOTAL_SERVICOS, FontArial7, Brushes.Black, 383, 902, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DISSQN_Retrato.BASE_CALCULO_ISSQN, FontArial7, Brushes.Black, 577, 902, Alinhamento)
        Gra_Saida.Graphics.DrawString(V_DISSQN_Retrato.VALOR_ISSQN, FontArial7, Brushes.Black, 780, 902, Alinhamento)

        ''''''''''''''''''''''''''''''''''''DADOS ADICIONAIS''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '  Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 916, 785, 12)
        Gra_Saida.Graphics.DrawString("DADOS ADICIONAIS", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, 916, New StringFormat)
        Gra_Saida.Graphics.DrawRectangle(pen, 10, 926, 500, 120)
        Gra_Saida.Graphics.DrawRectangle(pen, 510, 926, 285, 120)
        Gra_Saida.Graphics.DrawString("INFORMAÇÕES COMPLEMENTARES", Font5_B, Brushes.Black, 12, 926, New StringFormat)
        Gra_Saida.Graphics.DrawString("RESERVADO AO FISCO", Font5_B, Brushes.Black, 512, 926, New StringFormat)
        Gra_Saida.Graphics.DrawString(vbCrLf + INFOCOMP_Retrato.DADOSADIC, Font6, Brushes.Black, New RectangleF(12, 934, 508, 117), New StringFormat) '''''''iNFORMAÇÃO COMPLEMENTAR



        Dim AlinhamentoCenter As New StringFormat()
        AlinhamentoCenter.Alignment = StringAlignment.Center



        If I_DDadosNfe_Retrato.xJust <> "" Then
            Gra_Saida.Graphics.DrawString("DANFE em Contingência - impresso em decorrência de problemas técnicos" & " - " & I_DDadosNfe_Retrato.DHCONT.ToString("dd/MM/yyyy HH:mm:ss"), New Font("Times New Roman", 9, FontStyle.Bold), Brushes.Black, New RectangleF(5, 1015, 520, 50), AlinhamentoCenter)
        Else
            If I_Doutros_Retrato.numero_epec <> "" Then
                Gra_Saida.Graphics.DrawString("DANFE impresso em contingência - EPEC regularmente recebida pela Receita Federal do Brasil", New Font("Times New Roman", 9, FontStyle.Bold), Brushes.Black, New RectangleF(15, 1015, 320, 50), AlinhamentoCenter)
            End If
        End If


        '   End If
        '''''''''''''''CANHOTO DA NFE''''''''''''''''''''''''''''''

        Gra_Saida.Graphics.DrawLine(pen, 10, 1095, 680, 1095) '''''''''''LINHA HORIZONTAR
        Gra_Saida.Graphics.DrawLine(pen, 190, 1130, 190, 1095) ''''''''''''''LINHA VERTICAR
        Gra_Saida.Graphics.DrawLine(pen, 680, 1130, 680, 1060) ''''''''''''''LINHA VERTICAR

        Gra_Saida.Graphics.DrawRectangle(pen, 10, 1060, 785, 70)
        Dim AlinhamentoR As New StringFormat()
        AlinhamentoR.Alignment = StringAlignment.Near
        Gra_Saida.Graphics.DrawString("RECEBEMOS DE " & V_DEmitente_Retrato.NOME & " OS PRODUTOS CONSTANTES DA NOTA FISCAL ELETRÔNICA INDICADA AO LADO", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, New RectangleF(12, 1063, 650, 50), AlinhamentoR)

        Gra_Saida.Graphics.DrawString("DATA DE RECEBIMENTO", Font5_B, Brushes.Black, 12, 1095, New StringFormat)
        Gra_Saida.Graphics.DrawString("IDENTIFICAÇÃO E ASSINATURA DO RECEBEDOR", Font5_B, Brushes.Black, 200, 1095, New StringFormat)
        Gra_Saida.Graphics.DrawString("NF-e", New Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, 725, 1067, New StringFormat)

        Gra_Saida.Graphics.DrawString("N.º   " & Int32.Parse(I_DDadosNfe_Retrato.NUMERO_NFE).ToString("000,000,000"), FontArial8, Brushes.Black, 690, 1085, New StringFormat)
        Gra_Saida.Graphics.DrawString("SÉRIE: 001 ", FontArial8, Brushes.Black, 690, 1105, New StringFormat)

        pen.DashStyle = Drawing2D.DashStyle.Dash
        Gra_Saida.Graphics.DrawLine(pen, 10, 1055, 795, 1055) ''''''''''LINHA HORIZONTAL

        ''''''Rotina para Danfe cancelado'''''''''''''''''''''''''''''''
        If I_Doutros_Retrato.protocolocanc <> "" Then
            Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 315, 785, 110)
            Gra_Saida.Graphics.DrawString("NF-e CANCELADA PELO EMITENTE", New Font("Times New Roman", 30, FontStyle.Bold), Brushes.Black, 32, 325, New StringFormat)
            Gra_Saida.Graphics.DrawString("Protocolo de Cancelamento: " + I_Doutros_Retrato.protocolocanc, New Font("Times New Roman", 18, FontStyle.Bold), Brushes.Black, 32, 380, New StringFormat)
        End If
    End Sub

    'IMPRIME PRODUTOS NA DANFE
    Private Sub Imprimir_Produtos(ByVal Gra_Saida As System.Drawing.Printing.PrintPageEventArgs)

        Dim AlturaLinha As Single
        Dim LinhaSepara As Boolean = False
        Dim pen As New Pen(Brushes.Black, 0.5)

        'VALORES DA NOTA
        Dim AlinhamentoFar As New StringFormat()
        AlinhamentoFar.Alignment = StringAlignment.Far
        Dim AlinhamentoCenter As New StringFormat()
        AlinhamentoCenter.Alignment = StringAlignment.Center

        Dim LimitePagina As Integer = 0
        Dim RetHeight As Integer = 0
        Dim Y2Line As Integer = 0

        'VERIFICA SE É PRIMEIRA PAGINA E LIMITA NUMERO DE PRODUTOS
        If ControlaImpressao = 0 Then
            LimitePagina = 30
            AlturaLinha = 520
            RetHeight = 439
            Y2Line = 974 - 117

        Else
            LimitePagina = 50
            AlturaLinha = 260 - 50
            RetHeight = 845
            Y2Line = 1115 - 50
        End If

        Gra_Saida.Graphics.DrawString("DADOS DOS PRODUTOS / SERVIÇOS", New Font("Times New Roman", 6, FontStyle.Regular), Brushes.Black, 8, AlturaLinha - 2, New StringFormat)
        AlturaLinha += 10

        If ControlaImpressao = 0 Then
            Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 530, 784, 18)
            Gra_Saida.Graphics.DrawRectangle(pen, 10, AlturaLinha, 785, RetHeight - 95)
        Else
            Gra_Saida.Graphics.FillRectangle(Brushes.LightGray, 10, 221, 784, 18)
            Gra_Saida.Graphics.DrawRectangle(pen, 10, AlturaLinha, 785, RetHeight + 18)
        End If

        'Verticais
        Gra_Saida.Graphics.DrawLine(pen, 70, AlturaLinha, 70, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 278, AlturaLinha, 278, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 322, AlturaLinha, 322, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 350, AlturaLinha, 350, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 380, AlturaLinha, 380, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 420, AlturaLinha, 420, Y2Line + 17) 'VLR UNIT
        Gra_Saida.Graphics.DrawLine(pen, 468, AlturaLinha, 468, Y2Line + 17) 'TOTAL PRO
        Gra_Saida.Graphics.DrawLine(pen, 516, AlturaLinha, 516, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 564, AlturaLinha, 564, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 612, AlturaLinha, 612, Y2Line + 17) 'VLR BASE ST
        Gra_Saida.Graphics.DrawLine(pen, 660, AlturaLinha, 660, Y2Line + 17) 'VLR ICMS ST
        Gra_Saida.Graphics.DrawLine(pen, 708, AlturaLinha, 708, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 745, AlturaLinha, 745, Y2Line + 17)
        Gra_Saida.Graphics.DrawLine(pen, 770, AlturaLinha + 10, 770, Y2Line + 17)
        AlturaLinha += 5

        Gra_Saida.Graphics.DrawString("CÓD.PRODUTO" + vbCrLf + "    GTIN", Font5_B, Brushes.Black, 11, AlturaLinha - 4, New StringFormat)
        Gra_Saida.Graphics.DrawString("DESCRIÇÃO DOS PRODUTOS/SERVIÇOS", Font5_B, Brushes.Black, 72, AlturaLinha, New StringFormat)
        If frmxml.chklote.Checked = True Then
            Gra_Saida.Graphics.DrawString("NCM/SH" + vbCrLf + "LOTE", Font5_B, Brushes.Black, 301, AlturaLinha - 4, AlinhamentoCenter)
        Else
            Gra_Saida.Graphics.DrawString("NCM/SH", Font5_B, Brushes.Black, 301, AlturaLinha, AlinhamentoCenter)
        End If

        ' Gra_Saida.Graphics.DrawString("CST", Font5_B, Brushes.Black, 316, AlturaLinha, AlinhamentoCenter)

        Gra_Saida.Graphics.DrawString("CST" + vbCrLf + "CFOP", Font5_B, Brushes.Black, 336, AlturaLinha - 4, AlinhamentoCenter)
        Gra_Saida.Graphics.DrawString("UN", Font5_B, Brushes.Black, 365, AlturaLinha, AlinhamentoCenter)
        Gra_Saida.Graphics.DrawString("QTDE", Font5_B, Brushes.Black, 400, AlturaLinha, AlinhamentoCenter)
        Gra_Saida.Graphics.DrawString("VL.UNIT.", Font5_B, Brushes.Black, 468, AlturaLinha, AlinhamentoFar)
        Gra_Saida.Graphics.DrawString("VL.TOTAL", Font5_B, Brushes.Black, 514, AlturaLinha, AlinhamentoFar)
        Gra_Saida.Graphics.DrawString("BC ICMS", Font5_B, Brushes.Black, 560, AlturaLinha, AlinhamentoFar)
        Gra_Saida.Graphics.DrawString("VL.ICMS", Font5_B, Brushes.Black, 608, AlturaLinha, AlinhamentoFar)
        Gra_Saida.Graphics.DrawString("BC.ICMS" & vbCrLf & "ST", Font5_B, Brushes.Black, 635, AlturaLinha - 4, AlinhamentoCenter)
        Gra_Saida.Graphics.DrawString("VL.ICMS" & vbCrLf & "ST", Font5_B, Brushes.Black, 684, AlturaLinha - 4, AlinhamentoCenter)
        Gra_Saida.Graphics.DrawString("V.IPI", Font5_B, Brushes.Black, 742, AlturaLinha, AlinhamentoFar)
        Gra_Saida.Graphics.DrawString("ALÍQUOTAS", New Font("Times New Roman", 5, FontStyle.Regular), Brushes.Black, 790, AlturaLinha - 3, AlinhamentoFar)
        Gra_Saida.Graphics.DrawLine(pen, 794, AlturaLinha + 5, 745, AlturaLinha + 5)
        Gra_Saida.Graphics.DrawString("ICMS", Font5_B, Brushes.Black, 769, AlturaLinha + 5, AlinhamentoFar)
        Gra_Saida.Graphics.DrawString("IPI", Font5_B, Brushes.Black, 793, AlturaLinha + 5, AlinhamentoFar)
        AlturaLinha += 14
        Gra_Saida.Graphics.DrawLine(pen, 10, AlturaLinha, 795, AlturaLinha)

        '  DFDF()

        AlturaLinha += 5

        ' Dim Conte As Integer = 1
        Dim ConteImp As Integer = 0
        Dim Conte As Integer = 1
        LinhaSepara = False
        Dim Alinhamento As New StringFormat()
        Alinhamento.Alignment = StringAlignment.Far

        'PERCORRE TODA A MATRIZ DE PRODUTOS E IMPRIME OS MESMOS
        For Each PDanfe As ProdutoDanfe_Retrato In V_PRODUTOS_Retrato
            'VERIFICA SE PRODUTO NÃO FOI IMPRESSO

            If ContaProdutos < Conte Then

                'SE POSSUI MAIS PAGINAS CHAMA A MESMA E SAI DA MATRIZ
                If AlturaLinha > Y2Line Then
                    Gra_Saida.HasMorePages = True
                    Exit For
                Else
                    Gra_Saida.HasMorePages = False
                End If

                ContaProdutos += 1
                ConteImp += 1

                'INSERE LINHA PARA DELIMITAR PRODUTOS
                If LinhaSepara = False Then
                    LinhaSepara = True
                Else
                    'PARA USAR LINHAS PONTILHADAS HABILITAR FUNÇÃO ABAIXO
                    'pen.DashStyle = Drawing2D.DashStyle.Dot
                    If ControlaImpressao = 0 Then
                        ' Gra_Saida.Graphics.DrawLine(pen, 31, AlturaLinha, 289, AlturaLinha)
                    Else
                        ' Gra_Saida.Graphics.DrawLine(pen, 23, AlturaLinha, 289, AlturaLinha)
                    End If
                End If


                Gra_Saida.Graphics.DrawString(PDanfe.DCodigoProd, Font6, Brushes.Black, 40, AlturaLinha, AlinhamentoCenter)
                Gra_Saida.Graphics.DrawString(PDanfe.DCodigoBARRA, Font6, Brushes.Black, 40, AlturaLinha + 10, AlinhamentoCenter)
                Gra_Saida.Graphics.DrawString(PDanfe.DNCM, Font6, Brushes.Black, 300, AlturaLinha, AlinhamentoCenter)
                Gra_Saida.Graphics.DrawString(PDanfe.DLOTE, Font6, Brushes.Black, 300, AlturaLinha + 10, AlinhamentoCenter)

                Gra_Saida.Graphics.DrawString(PDanfe.DCST, Font6, Brushes.Black, 336, AlturaLinha, AlinhamentoCenter) '''''''''cst
                Gra_Saida.Graphics.DrawString(PDanfe.DCFOP, Font6, Brushes.Black, 336, AlturaLinha + 10, AlinhamentoCenter) '''''''''cfop
                Gra_Saida.Graphics.DrawString(PDanfe.DUNID, Font6, Brushes.Black, 365, AlturaLinha, AlinhamentoCenter) '''''''''unidade
                Gra_Saida.Graphics.DrawString(PDanfe.DQT, Font6, Brushes.Black, 400, AlturaLinha, AlinhamentoCenter) ''''''''''Quantidade

                Gra_Saida.Graphics.DrawString(PDanfe.DVALORUNI, Font6, Brushes.Black, 466, AlturaLinha, AlinhamentoFar) ''''''''''Valor unitario
                Gra_Saida.Graphics.DrawString(PDanfe.DVALORTOTAL, Font6, Brushes.Black, 514, AlturaLinha, AlinhamentoFar) '''''''valor Total
                Gra_Saida.Graphics.DrawString(PDanfe.DBCALC_ICMS, Font6, Brushes.Black, 562, AlturaLinha, AlinhamentoFar) ''''''Base ICMS
                Gra_Saida.Graphics.DrawString(PDanfe.DVALORICMS, Font6, Brushes.Black, 610, AlturaLinha, AlinhamentoFar) ''''''''Valor ICMS
                Gra_Saida.Graphics.DrawString(PDanfe.DBCALC_ICMS_ST, Font6, Brushes.Black, 658, AlturaLinha, AlinhamentoFar) '''''''''Base de ST
                Gra_Saida.Graphics.DrawString(PDanfe.DVALORICMS_ST, Font6, Brushes.Black, 706, AlturaLinha, AlinhamentoFar) ''''''''''Valor ST

                Gra_Saida.Graphics.DrawString(PDanfe.DVALORIPI, Font6, Brushes.Black, 744, AlturaLinha, AlinhamentoFar) '''''''''''Valor IPI
                Gra_Saida.Graphics.DrawString(PDanfe.DALIQUOTAICMS, Font6, Brushes.Black, 769, AlturaLinha, AlinhamentoFar) '''''''''Aliq IPI
                Gra_Saida.Graphics.DrawString(PDanfe.DALIQUOTAIPI, Font6, Brushes.Black, 792, AlturaLinha, AlinhamentoFar)

                If PDanfe.DDescricao.Length > 40 Then
                    Alinhamento.Alignment = StringAlignment.Near

                    If PDanfe.DDescricao.Length > 84 Then
                        Gra_Saida.Graphics.DrawString(PDanfe.DDescricao, Font6, Brushes.Black, New RectangleF(71, AlturaLinha, 190, 30), Alinhamento)
                        AlturaLinha += 20

                        If PDanfe.DVALOR_TRIB_ITEM <> "R$ 0,00" Then
                            Gra_Saida.Graphics.DrawString("Valor aproximado do Tributo: " + PDanfe.DVALOR_TRIB_ITEM, Font6, Brushes.Black, 71, AlturaLinha + 20, New StringFormat)
                            AlturaLinha += 10
                        End If

                    Else

                        Gra_Saida.Graphics.DrawString(PDanfe.DDescricao, Font6, Brushes.Black, New RectangleF(71, AlturaLinha, 190, 30), Alinhamento)

                        If PDanfe.DVALOR_TRIB_ITEM <> "R$ 0,00" Then
                            Gra_Saida.Graphics.DrawString("Valor aproximado do Tributo: " + PDanfe.DVALOR_TRIB_ITEM, Font6, Brushes.Black, 71, AlturaLinha + 20, New StringFormat)
                            AlturaLinha += 10
                        End If
                        AlturaLinha += 10
                    End If

                Else

                    Gra_Saida.Graphics.DrawString(PDanfe.DDescricao, Font6, Brushes.Black, 71, AlturaLinha, New StringFormat)

                    If PDanfe.DVALOR_TRIB_ITEM <> "R$ 0,00" Then
                        Gra_Saida.Graphics.DrawString("Valor aproximado do Tributo: " + PDanfe.DVALOR_TRIB_ITEM, Font6, Brushes.Black, 71, AlturaLinha + 10, New StringFormat)
                    End If
                    AlturaLinha += 10
                End If
                AlturaLinha += 10


                pen.DashStyle = Drawing2D.DashStyle.Dash

                If AlturaLinha < Y2Line Then
                    Gra_Saida.Graphics.DrawLine(pen, 10, AlturaLinha, 795, AlturaLinha)
                End If

                AlturaLinha += 4

            End If
            Conte = Conte + 1

        Next
        ControlaImpressao += 1
        frmdanfe.cbopage.Items.Add(ControlaImpressao)
        frmdanfe.cbopage.SelectedIndex = 0
        frmdanfe.lblpag.Text = ControlaImpressao
    End Sub

End Class
'CLASSE DE PRODUTOS DA DANFE
Public Class ProdutoDanfe_Retrato
    Private S_VALOR_TRIB_ITEM As String
    Private S_CODPROD As String
    Private S_CODBARRA As String
    Private S_DESCRICAO As String
    Private S_NCM As String
    Private S_LOTE As String
    Private S_CST As String
    Private S_CFOP As String
    Private S_UNID As String
    Private S_QUANT As String
    Private S_VALORUN As String
    Private S_VALORTOTAL As String
    Private S_VALORDESC As String
    Private S_BASE_ICMS As String
    Private S_VALOR_ICMS As String
    Private S_BASE_ICMS_ST As String
    Private S_VALOR_ICMS_ST As String
    Private S_VALOR_IPI As String
    Private S_ALIQUOTA_IPI As String
    Private S_ALIQUOTA_ICMS As String
    Public Sub New()
        _LinhaProd = New List(Of String)
    End Sub

    Public Property DCodigoProd() As String
        Get
            Return S_CODPROD
        End Get
        Set(ByVal value As String)
            S_CODPROD = value
        End Set
    End Property

    Public Property DCodigoBARRA() As String
        Get
            Return S_CODBARRA
        End Get
        Set(ByVal value As String)
            S_CODBARRA = value
        End Set
    End Property

    Public Property DDescricao() As String
        Get
            Return S_DESCRICAO
        End Get
        Set(ByVal value As String)
            S_DESCRICAO = value
        End Set
    End Property

    Public Property DNCM() As String
        Get
            Return S_NCM
        End Get
        Set(ByVal value As String)
            S_NCM = value
        End Set
    End Property

    Public Property DLOTE() As String
        Get
            Return S_LOTE
        End Get
        Set(ByVal value As String)
            S_LOTE = value
        End Set
    End Property

    Public Property DCST() As String
        Get
            Return S_CST
        End Get
        Set(ByVal value As String)
            S_CST = value
        End Set
    End Property

    Public Property DCFOP() As String
        Get
            Return S_CFOP
        End Get
        Set(ByVal value As String)
            S_CFOP = value
        End Set
    End Property

    Public Property DUNID() As String
        Get
            Return S_UNID
        End Get
        Set(ByVal value As String)
            S_UNID = value
        End Set
    End Property

    Public Property DQT() As String
        Get
            Return S_QUANT
        End Get
        Set(ByVal value As String)
            S_QUANT = value
        End Set
    End Property

    Public Property DVALORUNI() As String
        Get
            Return S_VALORUN
        End Get
        Set(ByVal value As String)
            S_VALORUN = value
        End Set
    End Property

    Public Property DVALORTOTAL() As String
        Get
            Return S_VALORTOTAL
        End Get
        Set(ByVal value As String)
            S_VALORTOTAL = value
        End Set
    End Property

    Public Property DVALORDESC() As String
        Get
            Return S_VALORDESC
        End Get
        Set(ByVal value As String)
            S_VALORDESC = value
        End Set
    End Property

    Public Property DBCALC_ICMS() As String
        Get
            Return S_BASE_ICMS
        End Get
        Set(ByVal value As String)
            S_BASE_ICMS = value
        End Set
    End Property

    Public Property DVALORICMS() As String
        Get
            Return S_VALOR_ICMS
        End Get
        Set(ByVal value As String)
            S_VALOR_ICMS = value
        End Set
    End Property

    Public Property DBCALC_ICMS_ST() As String
        Get
            Return S_BASE_ICMS_ST
        End Get
        Set(ByVal value As String)
            S_BASE_ICMS_ST = value
        End Set
    End Property

    Public Property DVALORICMS_ST() As String
        Get
            Return S_VALOR_ICMS_ST
        End Get
        Set(ByVal value As String)
            S_VALOR_ICMS_ST = value
        End Set
    End Property

    Public Property DVALORIPI() As String
        Get
            Return S_VALOR_IPI
        End Get
        Set(ByVal value As String)
            S_VALOR_IPI = value
        End Set
    End Property

    Public Property DALIQUOTAICMS() As String
        Get
            Return S_ALIQUOTA_ICMS
        End Get
        Set(ByVal value As String)
            S_ALIQUOTA_ICMS = value
        End Set
    End Property

    Public Property DALIQUOTAIPI() As String
        Get
            Return S_ALIQUOTA_IPI
        End Get
        Set(ByVal value As String)
            S_ALIQUOTA_IPI = value
        End Set
    End Property

    Private _LinhaProd As List(Of String)

    Public Property LinhaProd() As List(Of String)
        Get
            Return _LinhaProd
        End Get
        Set(ByVal value As List(Of String))
            _LinhaProd = value
        End Set
    End Property

    Public Property DVALOR_TRIB_ITEM() As String
        Get
            Return S_VALOR_TRIB_ITEM
        End Get
        Set(ByVal value As String)
            S_VALOR_TRIB_ITEM = value
        End Set
    End Property

End Class

Public Class FaturaDanfe_Retrato
    Private Numero_dup As String
    Private valor_dup As String
    Private datavenc_dup As String


    Public Property DNumero() As String
        Get
            Return Numero_dup
        End Get
        Set(ByVal value As String)
            Numero_dup = value
        End Set
    End Property
    Public Property Dvalor() As String
        Get
            Return valor_dup
        End Get
        Set(ByVal value As String)
            valor_dup = value
        End Set
    End Property
    Public Property Dvenc() As String
        Get
            Return datavenc_dup
        End Get
        Set(ByVal value As String)
            datavenc_dup = value
        End Set
    End Property



End Class

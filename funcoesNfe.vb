Imports System.Xml
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports DFe_aplicativo.DefaultPrinter
Imports System.Drawing.Printing
Imports System.Net


Public Module funcoesNfe
    Public ativa_webservice As Boolean
    Public ds As DataSet
    Public nsufalta As Boolean, email As Boolean
    Public reterro As String, cnpj_emp As String, coduf As String
    Public nfecancelada As Boolean
    Public svc_status As String, hora_verao As String, versao_sist As String
    Public vempresa As String, vnome_usu As String
    Public webservice_download As Boolean
    Public ct As Integer, regimetrib As Integer

    Public Function formatar_valor(ByVal texto As String) As String
        If texto <> "" Then

            Dim txt As String = ""

            Try
                txt = Format(Convert.ToDouble(texto.Replace(".", ",")), "###0.00")
            Catch ex As Exception

                Try
                    txt = Format(Convert.ToDouble(texto), "###0.00")

                Catch ex1 As Exception

                End Try

            End Try

            Return txt.Replace(",", ".")
        End If
    End Function

    Public Function formatar_valor_4c(ByVal texto As String) As String
        If texto <> "" Then

            Dim txt As String = ""

            Try
                txt = Format(Convert.ToDouble(texto.Replace(".", ",")), "###0.0000")
            Catch ex As Exception

                Try
                    txt = Format(Convert.ToDouble(texto), "###0.0000")

                Catch ex1 As Exception

                End Try

            End Try

            Return txt.Replace(",", ".")
        End If
    End Function

    Public Function removeFormatacao(ByVal texto As String) As String

        Dim txt As String = ""

        txt = texto.Replace(".", "")
        txt = txt.Replace("-", "")
        txt = txt.Replace("/", "")
        txt = txt.Replace("(", "")
        txt = txt.Replace(")", "")
        txt = txt.Replace(" ", "")

        Return txt
    End Function

    Public Function removezerodireita(ByVal txt As String) As String
        If txt.Substring(txt.Length - 1, 1) = "0" Then
            txt = txt.Remove(txt.Length - 1, 1)
        End If
        If txt.Substring(txt.Length - 1, 1) = "0" Then
            txt = txt.Remove(txt.Length - 2, 2)
        End If
        Return txt
    End Function

    Public Sub retornaAtributos(ByVal obj As Object(), ByRef cultura As CultureInfo, ByRef formato As String, ByRef obrigatorio As Boolean)
        cultura = CultureInfo.CreateSpecificCulture("en-US")
        formato = "###0.00"
        obrigatorio = False
        For Each objeto As Object In obj
            ' If TypeOf objeto Is Formato Then
            '    Dim culturaStr As String = DirectCast(objeto, Formato).cultura
            ''    formato = DirectCast(objeto, Formato).formato
            '     cultura = CultureInfo.CreateSpecificCulture(culturaStr)
            '  ElseIf TypeOf objeto Is Obrigatorio Then
            ' '      obrigatorio = DirectCast(objeto, Obrigatorio).propriedadeObrigatoria

            '  End If

            '   cultura.NumberFormat.NumberDecimalSeparator = ",";
            ' cultura.NumberFormat.NumberGroupSeparator = ".";
        Next
    End Sub

    Public Function modulo11(ByVal chaveNFE As String) As Integer
        If chaveNFE.Length <> 43 Then
            Throw New Exception("Chave inválida, não é possível calcular o digito verificador")
        End If


        Dim baseCalculo As String = "4329876543298765432987654329876543298765432"
        Dim somaResultados As Integer = 0

        For i As Integer = 0 To chaveNFE.Length - 1
            Dim numNF As Integer = Convert.ToInt32(chaveNFE(i).ToString())
            Dim numBaseCalculo As Integer = Convert.ToInt32(baseCalculo(i).ToString())

            somaResultados += (numBaseCalculo * numNF)
        Next

        Dim restoDivisao As Integer = (somaResultados Mod 11)
        Dim dv As Integer = 11 - restoDivisao
        If (dv = 0) OrElse (dv > 9) Then
            Return 0
        Else
            Return dv
        End If
    End Function

    Public Function TirarAcento(ByVal palavra As String) As String
        Dim palavraSemAcento As String = ""
        Dim caracterComAcento As String = "áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ"
        Dim caracterSemAcento As String = "aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC"

        For i As Integer = 0 To palavra.Length - 1
            If caracterComAcento.IndexOf(Convert.ToChar(palavra.Substring(i, 1))) >= 0 Then
                Dim car As Integer = caracterComAcento.IndexOf(Convert.ToChar(palavra.Substring(i, 1)))
                palavraSemAcento += caracterSemAcento.Substring(car, 1)
            Else
                palavraSemAcento += palavra.Substring(i, 1)
            End If
        Next

        Return palavraSemAcento
    End Function

    ''' <summary>
    ''' Função que utilizo para saber se é uma propriedade simples (String, Int) ou uma nova classe, que deve gerar
    ''' uma nova tag xml
    ''' </summary>
    ''' <param name="propriedade"></param>
    ''' <returns></returns>
    Public Function novaTag(ByVal propriedade As PropertyInfo) As Boolean
        'TODO: Buscar uma forma melhor de identificar as subclasses.

        Select Case propriedade.PropertyType.ToString()
            'Propriedades que podem ser nulas (com ?)...
            Case "System.DateTime", "System.Int32", "System.String", "System.Double", "System.Nullable", "System.Decimal", _
            "System.Nullable`1[System.Int32]", "System.Nullable`1[System.DateTime]", "System.Nullable`1[System.Decimal]", "System.Nullable`1[System.Double]"
                Return False
            Case Else
                Return True
        End Select
    End Function

    Public Sub gravarElemento(ByVal writer As XmlWriter, ByVal nomeTag As String, ByVal valorTag As Object, ByVal atributos As Object())
        'CultureInfo cultBR = new CultureInfo("pt-BR");
        ' CultureInfo cultUS = new CultureInfo("en-US");

        Dim cult As CultureInfo
        Dim formato As String
        Dim obrigatorio As Boolean
        retornaAtributos(atributos, cult, formato, obrigatorio)

        If valorTag IsNot Nothing Then
            Dim valor As String = ""
            Select Case valorTag.[GetType]().ToString()
                Case "System.DateTime"
                    valor = DirectCast((valorTag), DateTime).ToString("yyyy-MM-dd")
                    'formata no padrão necessário para a NFe
                    Exit Select
                Case "System.Int32"
                    valor = valorTag.ToString()
                    If valor.Trim() = "0" Then
                        'campos do tipo inteiro com valor 0 serão ignorados
                        valor = ""
                    End If
                    Exit Select
                Case "System.String"
                    valor = TirarAcento(valorTag.ToString().Replace(vbCr & vbLf, " - ")).Trim()
                    'remove linhas... e tira acentos
                    Exit Select
                Case "System.Double"
                    valor = CDbl(valorTag).ToString(formato, cult.NumberFormat)
                    'formata de acordo com o atributo
                    Exit Select
                Case "System.Decimal"
                    valor = CDec(valorTag).ToString(formato, cult.NumberFormat)
                    'formata de acordo com o atributo
                    Exit Select

            End Select
            If (valor.Trim().Length > 0) OrElse (obrigatorio) Then
                writer.WriteElementString(nomeTag, valor)
            End If
        End If
    End Sub

    Public Function tamanhoXML(ByVal documento As XmlDocument) As Long
        Dim nomeArquivo As String = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "")

        Try
            documento.Save(nomeArquivo)
            Dim info As New FileInfo(nomeArquivo)
            Dim tamanhoArquivo As Long = info.Length

            info.Delete()

            Return tamanhoArquivo
        Catch
            Return 0

        End Try
    End Function

    Public Function RandomNumber(ByVal MaxNumber As Integer, _
   Optional ByVal MinNumber As Integer = 0) As Integer

        'initialize random number generator
        Dim r As New Random(System.DateTime.Now.Millisecond)

        'if passed incorrect arguments, swap them
        'can also throw exception or return 0

        If MinNumber > MaxNumber Then
            Dim t As Integer = MinNumber
            MinNumber = MaxNumber
            MaxNumber = t
        End If

        Return r.Next(MinNumber, MaxNumber)

    End Function

    Public Function SepararNota(ByVal vNfeDadosMsg As String) As String

        'Separar somente o conteúdo a partir da tag <NFe> até </NFe>
        Dim nPosI As Int32 = vNfeDadosMsg.IndexOf("<NFe")
        Dim nPosF As Int32 = vNfeDadosMsg.Length - nPosI
        Dim vStringNfe As String = vNfeDadosMsg.Substring(nPosI, nPosF - 10)

        Return vStringNfe

    End Function

    Public Function GerarNotaProcessada(ByVal vStringNfe As String, ByVal VStringRecibo As String, ByVal EmissaoNota As Date, ByVal chavenfe As String) As String
        Dim cVersaoDados As String = "3.10"

        'Montar a parte do XML referente ao Lote e acrescentar a Nota Fiscal
        Dim vStringLoteNfe As String = String.Empty
        vStringLoteNfe += "<?xml version=""1.0"" encoding=""utf-8""?>"
        vStringLoteNfe += "<nfeProc versao=""" & cVersaoDados & """ xmlns=""http://www.portalfiscal.inf.br/nfe"">"
        vStringLoteNfe += vStringNfe
        '  vStringLoteNfe += "<protNFe versao=""" & cVersaoDados & """>"
        vStringLoteNfe += VStringRecibo
        ' vStringLoteNfe += "</protNFe>"
        vStringLoteNfe += "</nfeProc>"

        Dim PastaNota As String = ""
        PastaNota = Application.StartupPath & "\NFE_XML\" & Date.Parse(EmissaoNota).ToString("dd-MM-yyyy") & "\"
        If Directory.Exists(PastaNota) = False Then
            Directory.CreateDirectory(PastaNota)
        End If


        Dim NDoc = New XmlDocument()

        NDoc.LoadXml(vStringLoteNfe)

        Using xmltw As New XmlTextWriter(PastaNota & chavenfe.Replace(" ", "") + ".xml", New UTF8Encoding(False))
            NDoc.WriteTo(xmltw)
            xmltw.Close()
        End Using

        Return PastaNota & chavenfe + ".xml"
    End Function

    Public Function SepararNotaxml(ByVal vNfeDadosMsg As String) As String

        'Separar somente o conteúdo a partir da tag <NFe> até </NFe>
        Dim nPosI As Int32 = vNfeDadosMsg.IndexOf("<NFe")
        Dim n5t As Int32 = vNfeDadosMsg.IndexOf("<protNFe")
        Dim total As Int32 = vNfeDadosMsg.Length - n5t
        Dim nPosF As Int32 = vNfeDadosMsg.Length - nPosI
        Dim vStringNfe As String = vNfeDadosMsg.Substring(nPosI, nPosF - total)
        Return vStringNfe

    End Function


    Public Function validacnpj(ByVal cnpj As String)

        Dim Numero(13) As Integer

        Dim soma As Integer

        Dim i As Integer

        Dim valida As Boolean

        Dim resultado1 As Integer

        Dim resultado2 As Integer

        For i = 0 To Numero.Length - 1

            Numero(i) = CInt(cnpj.Substring(i, 1))

        Next

        soma = Numero(0) * 5 + Numero(1) * 4 + Numero(2) * 3 + Numero(3) * 2 + Numero(4) * 9 + Numero(5) * 8 + Numero(6) * 7 + _
                   Numero(7) * 6 + Numero(8) * 5 + Numero(9) * 4 + Numero(10) * 3 + Numero(11) * 2

        soma = soma - (11 * (Int(soma / 11)))

        If soma = 0 Or soma = 1 Then

            resultado1 = 0

        Else

            resultado1 = 11 - soma

        End If

        If resultado1 = Numero(12) Then

            soma = Numero(0) * 6 + Numero(1) * 5 + Numero(2) * 4 + Numero(3) * 3 + Numero(4) * 2 + Numero(5) * 9 + Numero(6) * 8 + _
                         Numero(7) * 7 + Numero(8) * 6 + Numero(9) * 5 + Numero(10) * 4 + Numero(11) * 3 + Numero(12) * 2

            soma = soma - (11 * (Int(soma / 11)))

            If soma = 0 Or soma = 1 Then

                resultado2 = 0

            Else

                resultado2 = 11 - soma

            End If

            If resultado2 = Numero(13) Then

                Return True

            Else

                Return False

            End If

        Else

            Return False

        End If

    End Function

    Function validaCPF(ByVal strCPFCliente As String) As Boolean

        '--Declaração das Variáveis
        Dim strCPFOriginal As String = strCPFCliente.Replace(".", "").Replace("-", "").Replace(",", "")
        Dim strCPF As String = Mid(strCPFOriginal, 1, 9)
        Dim strCPFTemp As String
        Dim intSoma As Integer
        Dim intResto As Integer
        Dim strDigito As String
        Dim intMultiplicador As Integer = 10
        Const constIntMultiplicador As Integer = 11
        Dim i As Integer
        '--------------------------

        For i = 0 To strCPF.ToString.Length - 1
            intSoma += CInt(strCPF.ToString.Chars(i).ToString) * intMultiplicador
            intMultiplicador -= 1
        Next

        If (intSoma Mod constIntMultiplicador) < 2 Then
            intResto = 0
        Else
            intResto = constIntMultiplicador - (intSoma Mod constIntMultiplicador)
        End If

        strDigito = intResto
        intSoma = 0

        strCPFTemp = strCPF & strDigito
        intMultiplicador = 11

        For i = 0 To strCPFTemp.Length - 1
            intSoma += CInt(strCPFTemp.Chars(i).ToString) * intMultiplicador
            intMultiplicador -= 1
        Next

        If (intSoma Mod constIntMultiplicador) < 2 Then
            intResto = 0
        Else
            intResto = constIntMultiplicador - (intSoma Mod constIntMultiplicador)
        End If

        strDigito &= intResto

        If strDigito = Mid(strCPFOriginal, 10, strCPFOriginal.Length) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function casasdecimal2(ByRef txt As TextBox, ByVal e As System.Windows.Forms.KeyPressEventArgs) As Integer
        'so aceita dois casas a pos a virgula'
        Dim texto As String
        Dim posição As Integer
        Dim cursorpos As Integer
        Dim depois As String

        If e.Handled <> 8 Then
            texto = txt.Text
            cursorpos = txt.SelectionStart
            posição = InStr(1, texto, ",")

            If e.KeyChar = "." Then
                e.Handled = True
            End If

            If cursorpos >= posição Then
                If posição > 0 Then
                    depois = Microsoft.VisualBasic.Right(texto, Len(texto) - posição)
                    If e.KeyChar = "," Then
                        e.Handled = True
                    End If
                    If Len(depois) > 1 Then
                        If Not e.KeyChar = vbBack Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End If
        casasdecimal2 = e.Handled
    End Function

    Public Function casasdecimal4(ByRef txt As TextBox, ByVal e As System.Windows.Forms.KeyPressEventArgs) As Integer
        'so aceita dois casas a pos a virgula'
        Dim texto As String
        Dim posição As Integer
        Dim cursorpos As Integer
        Dim depois As String

        If e.Handled <> 8 Then
            texto = txt.Text
            cursorpos = txt.SelectionStart
            posição = InStr(1, texto, ",")

            If e.KeyChar = "." Then
                e.Handled = True
            End If

            If cursorpos >= posição Then
                If posição > 0 Then
                    depois = Microsoft.VisualBasic.Right(texto, Len(texto) - posição)
                    If e.KeyChar = "," Then
                        e.Handled = True
                    End If
                    If Len(depois) > 3 Then
                        If Not e.KeyChar = vbBack Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End If
        casasdecimal4 = e.Handled
    End Function

    Public Function GetDataTable(path As String, seperator As Char) As DataTable
        Dim dt As New DataTable()
        Dim aFile As New FileStream(path, FileMode.Open)
        Using sr As New StreamReader(aFile, System.Text.Encoding.[Default])
            Dim strLine As String = sr.ReadLine()
            Dim strArray As String() = strLine.Split(seperator)

            For Each value As String In strArray

                dt.Columns.Add(value.Trim())
            Next

            Dim dr As DataRow = dt.NewRow()

            While sr.Peek() > -1
                strLine = sr.ReadLine()
                strArray = strLine.Split(seperator)
                dt.Rows.Add(strArray)
            End While
        End Using
        Return dt
    End Function

    Public Function FormatarCpfCnpj(strCpfCnpj As String) As String

        If strCpfCnpj.Length <= 8 Then

            Dim mtpCpf As New MaskedTextProvider("00,000-000")

            mtpCpf.[Set](ZerosEsquerda(strCpfCnpj, 8))


            Return mtpCpf.ToString()
        End If

        If strCpfCnpj.Length <= 10 Then

            Dim mtpCpf As New MaskedTextProvider("(00) 0000-0000")

            mtpCpf.[Set](ZerosEsquerda(strCpfCnpj, 10))


            Return mtpCpf.ToString()
        End If

        If strCpfCnpj.Length <= 11 Then


            Dim mtpCpf As New MaskedTextProvider("000\.000\.000-00")

            mtpCpf.[Set](ZerosEsquerda(strCpfCnpj, 11))


            Return mtpCpf.ToString()
        Else



            Dim mtpCnpj As New MaskedTextProvider("00\.000\.000/0000-00")

            mtpCnpj.[Set](ZerosEsquerda(strCpfCnpj, 11))


            Return mtpCnpj.ToString()
        End If

    End Function


    Public Function ZerosEsquerda(strString As String, intTamanho As Integer) As String


        Dim strResult As String = ""

        For intCont As Integer = 1 To (intTamanho - strString.Length)



            strResult += "0"
        Next

        Return strResult & strString

    End Function

    Public Function convert_valor(ByVal doublestring As String) As Double
        Dim retval As Double
        Dim sep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

        If Double.TryParse(Replace(Replace(doublestring, ".", sep), ",", sep), retval) Then
            Return retval
        Else
            Return Nothing
        End If
    End Function

    Public Function valor_campo_vazio()
        Dim sep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
        Dim valor_ret As String

        valor_ret = "0" & sep & "00"

        Return valor_ret

    End Function

    Public Function formata_valor_campo(ByVal valor As String) As String
        Dim valor_ret As String

        valor_ret = Format(Convert.ToDouble(valor), "#,##0.00")

        Return valor_ret

    End Function

    Public Function formata_valor_campo_4c(ByVal valor As String) As String
        Dim valor_ret As String

        valor_ret = Format(Convert.ToDouble(valor), "#,##0.0000")

        Return valor_ret

    End Function

    Function BuscaCep(ByVal cep As String) As Hashtable

        Dim _resultado As String
        Dim ht As System.Collections.Hashtable

        Try
            ds = New DataSet()
            ds.ReadXml("http://cep.republicavirtual.com.br/web_cep.php?cep=" + cep.Replace("-", "").Trim() + "&formato=xml")

            If Not IsNothing(ds) Then

                If (ds.Tables(0).Rows.Count > 0) Then

                    _resultado = ds.Tables(0).Rows(0).Item("resultado").ToString()

                    ht = New Hashtable

                    Select Case _resultado

                        Case "1"
                            ht.Add("UF", ds.Tables(0).Rows(0).Item("uf").ToString().Trim())
                            ht.Add("cidade", ds.Tables(0).Rows(0).Item("cidade").ToString().Trim())
                            ht.Add("bairro", ds.Tables(0).Rows(0).Item("bairro").ToString().Trim())
                            ht.Add("tipologradouro", ds.Tables(0).Rows(0).Item("tipo_logradouro").ToString().Trim())
                            ht.Add("logradouro", ds.Tables(0).Rows(0).Item("logradouro").ToString().Trim())
                            ht.Add("tipo", 1)

                        Case "2"
                            ht.Add("UF", ds.Tables(0).Rows(0).Item("uf").ToString().Trim())
                            ht.Add("cidade", ds.Tables(0).Rows(0).Item("cidade").ToString().Trim())
                            ht.Add("tipo", 2)

                        Case Else
                            ht.Add("tipo", 0)

                    End Select

                End If

            End If

            Return ht

        Catch ex As Exception
            Throw New Exception("Falha ao Buscar o Cep" & vbCrLf & ex.ToString)
            Return Nothing
        End Try
    End Function

    Function SoLETRAS(ByVal KeyAscii As Integer) As Integer

        'Transformar letras minusculas em Maiúsculas

        KeyAscii = Asc(UCase(Chr(KeyAscii)))

        ' Intercepta um código ASCII recebido e admite somente letras

        If InStr("AÃÁBCÇDEÉÊFGHIÍJKLMNOPQRSTUÚVWXYZ", Chr(KeyAscii)) = 0 Then

            SoLETRAS = 0

        Else

            SoLETRAS = KeyAscii

        End If



        ' teclas adicionais permitidas

        If KeyAscii = 8 Then SoLETRAS = KeyAscii ' Backspace

        If KeyAscii = 13 Then SoLETRAS = KeyAscii ' Enter

        If KeyAscii = 32 Then SoLETRAS = KeyAscii ' Espace
    End Function

    Function SoNumeros(ByVal Keyascii As Short) As Short

        If InStr("1234567890", Chr(Keyascii)) = 0 Then

            SoNumeros = 0

        Else

            SoNumeros = Keyascii

        End If



        Select Case Keyascii

            Case 8

                SoNumeros = Keyascii

            Case 13

                SoNumeros = Keyascii

            Case 32

                SoNumeros = Keyascii

        End Select

    End Function

    Public Function validaGTIN(code As String) As Boolean
        If code <> (New Regex("[^0-9]")).Replace(code, "") Then
            ' is not numeric
            Return False
        End If
        ' pad with zeros to lengthen to 14 digits
        Select Case code.Length
            Case 8
                code = Convert.ToString("000000") & code
                Exit Select
            Case 12
                code = Convert.ToString("00") & code
                Exit Select
            Case 13
                code = Convert.ToString("0") & code
                Exit Select
            Case 14
                Exit Select
            Case Else
                ' wrong number of digits
                Return False
        End Select
        ' calculate check digit
        Dim a As Integer() = New Integer(12) {}
        a(0) = Integer.Parse(code(0).ToString()) * 3
        a(1) = Integer.Parse(code(1).ToString())
        a(2) = Integer.Parse(code(2).ToString()) * 3
        a(3) = Integer.Parse(code(3).ToString())
        a(4) = Integer.Parse(code(4).ToString()) * 3
        a(5) = Integer.Parse(code(5).ToString())
        a(6) = Integer.Parse(code(6).ToString()) * 3
        a(7) = Integer.Parse(code(7).ToString())
        a(8) = Integer.Parse(code(8).ToString()) * 3
        a(9) = Integer.Parse(code(9).ToString())
        a(10) = Integer.Parse(code(10).ToString()) * 3
        a(11) = Integer.Parse(code(11).ToString())
        a(12) = Integer.Parse(code(12).ToString()) * 3
        Dim sum As Integer = a(0) + a(1) + a(2) + a(3) + a(4) + a(5) + a(6) + a(7) + a(8) + a(9) + a(10) + a(11) + a(12)
        Dim check As Integer = (10 - (sum Mod 10)) Mod 10
        ' evaluate check digit
        Dim last As Integer = Integer.Parse(code(13).ToString())
        Return check = last
    End Function

    Public Function VerificaConectividade_net() As Boolean
        Dim vRetorno As Boolean = False

        Try
            If My.Computer.Network.Ping("www.google.com", 10000) = True Then
                vRetorno = True
            End If
        Catch ex As Exception

        End Try


        Return vRetorno
    End Function


    Public Function GerarLote_manifestacao()

        'Pegar o último número de lote de NFe utilizado e acrescentar + 1 para novo envio
        Dim vArqXmlLote As String = "TryLote_manifestacao.xml"
        Dim nNumeroLote As Int32 = 1

        If File.Exists(vArqXmlLote) Then
            Dim oLerXml As New XmlTextReader(vArqXmlLote)

            While oLerXml.Read()
                If oLerXml.NodeType = XmlNodeType.Element Then
                    If oLerXml.Name = "UltimoLoteEnviado" Then
                        oLerXml.Read()
                        nNumeroLote = Convert.ToInt32(oLerXml.Value) + 1
                        Exit While
                    End If
                End If
            End While
            oLerXml.Close()
        End If

        'Salvar o número de lote de NFe utilizado
        Dim oSettings As New XmlWriterSettings()
        oSettings.Indent = True
        oSettings.IndentChars = ""
        oSettings.NewLineOnAttributes = False
        oSettings.OmitXmlDeclaration = False

        Dim oXmlGravar As XmlWriter = XmlWriter.Create("TryLote_manifestacao.xml", oSettings)

        oXmlGravar.WriteStartDocument()
        oXmlGravar.WriteStartElement("DadosLoteNfe")
        oXmlGravar.WriteElementString("UltimoLoteEnviado", nNumeroLote.ToString())
        oXmlGravar.WriteEndElement()
        'DadosLoteNfe
        oXmlGravar.WriteEndDocument()
        oXmlGravar.Flush()
        oXmlGravar.Close()

        Return nNumeroLote.ToString("000000000000000")

    End Function


    Public Function gerar_pdf_sistema(Documento As PrintDocument, saida_doc_pdf As String)

        Dim Nome_impressora As String
        Nome_impressora = Documento.PrinterSettings.PrinterName

        Documento.PrinterSettings.PrinterName = "Microsoft Print To PDF"

        Documento.DefaultPageSettings.PrinterSettings.PrintToFile = True

        Documento.DefaultPageSettings.PrinterSettings.PrintFileName = saida_doc_pdf
        Documento.PrintController = New StandardPrintController()
        Documento.Print()


        Documento.DefaultPageSettings.PrinterSettings.PrintToFile = False

        '  DefaultPrinterName = Nome_impressora

    End Function

    Public Function gerar_xps_PDF_sistema(Documento As PrintDocument, nome_doc As String, caminho_saida_pdf As String)

        Dim Nome_impressora As String
        Nome_impressora = Documento.PrinterSettings.PrinterName

        Documento.PrinterSettings.PrinterName = "Microsoft XPS Document Writer"

        Documento.DefaultPageSettings.PrinterSettings.PrintToFile = True

        Documento.DefaultPageSettings.PrinterSettings.PrintFileName = Application.StartupPath & "\Temp_pdf\" & nome_doc & ".xps"
        Documento.PrintController = New StandardPrintController()
        Documento.Print()


        Documento.DefaultPageSettings.PrinterSettings.PrintToFile = False

        '   DefaultPrinterName = Nome_impressora


        Dim objArguments As String
        objArguments = "-sDEVICE=pdfwrite -sOutputFile=" & Application.StartupPath & "\Temp_pdf\" & nome_doc & ".pdf" & " -dNOPAUSE " & Application.StartupPath & "\Temp_pdf\" & nome_doc & ".xps" & ""

        Dim objStartInfo As New ProcessStartInfo()

        objStartInfo.UseShellExecute = False
        objStartInfo.RedirectStandardError = True
        objStartInfo.FileName = Application.StartupPath & "\pdf.exe"
        objStartInfo.Arguments = objArguments
        objStartInfo.WindowStyle = ProcessWindowStyle.Hidden
        objStartInfo.ErrorDialog = True


        Using objProcess As Process = New Process
            objProcess.StartInfo = objStartInfo
            objProcess.Start()

            objProcess.WaitForExit()
            objProcess.Close()
        End Using


        File.Delete(Application.StartupPath & "\Temp_pdf\" & nome_doc & ".xps")

        If caminho_saida_pdf <> "" Then
            File.Move(Application.StartupPath & "\Temp_pdf\" & nome_doc & ".pdf", caminho_saida_pdf)
        End If

    End Function

    Public Function converte_xmlbytes(ByVal fileName As String) As Byte()

        If fileName <> "" Then
            Dim encText As New System.Text.UTF8Encoding()
            Dim btText() As Byte
            btText = encText.GetBytes(fileName)
            Return btText
        End If
    End Function


    Public Function IsConectedToHost(ByVal uri As Uri) As Boolean
        Try
            Dns.GetHostEntry(uri.Host)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
End Module
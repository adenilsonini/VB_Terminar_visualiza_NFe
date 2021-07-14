Imports System.IO
Imports System.Drawing.Printing
Imports DFe_aplicativo.DefaultPrinter
Imports Microsoft.VisualBasic.Devices
Imports System.Text
Imports System.Xml

Public Class frmdanfe
    Dim Doc_Danfe As New PrintDocument
    Dim nome_impressora As String
    Dim controle As Boolean

    Private Sub frmdanfe_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cboimpressora.Items.Clear()


        Dim aNomeImpressora As String

        For Each aNomeImpressora In PrinterSettings.InstalledPrinters

            cboimpressora.Items.Add(aNomeImpressora)

        Next

        cboimpressora.SelectedIndex = 0



    End Sub

  Private Sub cbopage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbopage.SelectedIndexChanged
        PrintPreviewControl1.StartPage = cbopage.Text - 1
    End Sub


    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If nome_impressora <> "" Then
            ' DefaultPrinterName = nome_impressora
        End If

        Doc_Danfe = PrintPreviewControl1.Document

        Dim prn As New Printing.PageSettings()


        Doc_Danfe.PrinterSettings.PrinterName = prn.PrinterSettings.PrinterName()

        If chkselecao.Checked = True Then
            Doc_Danfe.PrinterSettings.PrinterName = cboimpressora.Text
        End If

        Doc_Danfe.Print()


    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Dim save As Boolean = False
        Dim computerInfo As New ComputerInfo
        Dim SaveFiled As New SaveFileDialog()




        SaveFiled.Title = "Local para salvar o DANFe PDF"

        SaveFiled.Filter = "Arquivos (*.pdf)|*.pdf|All files (*.*)|*.*"

        SaveFiled.FileName = frmxml.mskchave.Text.Replace(" ", "")


        If SaveFiled.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pinfo.Visible = True

            If computerInfo.OSFullName.Substring(0, 20) = "Microsoft Windows 10" Then

                nome_impressora = PrintPreviewControl1.Document.PrinterSettings.PrinterName

                PrintPreviewControl1.Document.PrinterSettings.PrinterName = "Microsoft Print To PDF"

                PrintPreviewControl1.Document.DefaultPageSettings.PrinterSettings.PrintToFile = True

                PrintPreviewControl1.Document.DefaultPageSettings.PrinterSettings.PrintFileName = SaveFiled.FileName
                PrintPreviewControl1.Document.PrintController = New StandardPrintController()
                PrintPreviewControl1.Document.Print()

                save = True

                PrintPreviewControl1.Document.DefaultPageSettings.PrinterSettings.PrintToFile = False
            

            Else

                nome_impressora = PrintPreviewControl1.Document.PrinterSettings.PrinterName

                PrintPreviewControl1.Document.PrinterSettings.PrinterName = "Microsoft XPS Document Writer"

                PrintPreviewControl1.Document.DefaultPageSettings.PrinterSettings.PrintToFile = True

                PrintPreviewControl1.Document.DefaultPageSettings.PrinterSettings.PrintFileName = SaveFiled.FileName & ".xps"
                PrintPreviewControl1.Document.PrintController = New StandardPrintController()
                PrintPreviewControl1.Document.Print()
                PrintPreviewControl1.Document.DefaultPageSettings.PrinterSettings.PrintToFile = False

                '''''''''''''''''Rotina gerar pdf abaixo'''''''''''''''''
                Dim proc As New System.Diagnostics.ProcessStartInfo()
                proc.FileName = Application.StartupPath & "\pdf.exe"
                proc.UseShellExecute = True
                '  proc.Verb = "runas" 'esta linha executa o aplicativo como administrador!
                Dim strArguments As String = ""
                strArguments += "-sDEVICE=pdfwrite -sOutputFile=" & SaveFiled.FileName & " -dNOPAUSE " & SaveFiled.FileName & ".xps" & ""

                Console.WriteLine(strArguments)
                proc.Arguments = strArguments
                System.Diagnostics.Process.Start(proc)
                save = True

            End If

            pinfo.Visible = False
        End If

        If save = True Then
            MsgBox("DANFE em PDF, gerado com sucesso gerado no local informado !", MsgBoxStyle.Information, "Aviso !")
            save = False
            File.Delete(SaveFiled.FileName & ".xps")
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If PrintPreviewControl1.StartPage <> "0" Then
            PrintPreviewControl1.StartPage = PrintPreviewControl1.StartPage - 1
            cbopage.SelectedIndex = cbopage.FindString(PrintPreviewControl1.StartPage + 1)
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If PrintPreviewControl1.StartPage <= lblpag.Text Then
            PrintPreviewControl1.StartPage = PrintPreviewControl1.StartPage + 1
            cbopage.SelectedIndex = cbopage.FindString(PrintPreviewControl1.StartPage + 1)
        End If
    End Sub

    Public Sub criar_xml_email()

        Dim xmldoc_mail As New XmlDocument()

        If Directory.Exists(Application.StartupPath & "\Temp_pdf") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Temp_pdf")
        End If


        xmldoc_mail.LoadXml(frmxml.xmldoc.OuterXml)
        xmldoc_mail.Save(Application.StartupPath & "\Temp_pdf\" & frmxml.mskchave.Text & ".xml")
        '  frmenvia_email.caminhoxml = Application.StartupPath & "\Temp_pdf\" & frmxml.mskchave.Text & ".xml"

    End Sub
    Private Sub geraPDF_microsoft(ByVal chave As String)

        Dim computerInfo As New ComputerInfo

        If Directory.Exists(Application.StartupPath & "\Temp_pdf") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Temp_pdf")
        End If


        If computerInfo.OSFullName.Substring(0, 20) = "Microsoft Windows 10" Then

            gerar_pdf_sistema(PrintPreviewControl1.Document, Application.StartupPath & "\Temp_pdf\" & chave & ".pdf")

        Else

            gerar_xps_PDF_sistema(PrintPreviewControl1.Document, chave, "")

        End If


        '  frmenvia_email.caminhodanfe = Application.StartupPath & "\Temp_pdf\" & chave & ".pdf"



    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        criar_xml_email()

        geraPDF_microsoft(txtchavenfe.Text)

        ' frmenvia_email.Text = "Envio de Nota Fiscal para empresa destinataria"
        ' frmenvia_email.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try

            '  DefaultPrinterName = cboimpressora.Text

            MsgBox("Impressora Definido com padrão, " + vbCrLf + cboimpressora.Text, MsgBoxStyle.Information, "Aviso !")

        Catch ex As Exception
            MessageBox.Show("Você deve selecionar uma impressora!", "Selecione uma impressora", _
                         MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

  
End Class
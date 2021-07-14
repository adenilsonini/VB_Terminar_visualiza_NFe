Public Class frmcalcpreco
    Private Sub item()
        txtdesc.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("descricao").Value

        txtvlrunit.Text = txtvlrunit.Text.Replace(".", ",")
        txtvlrunit.Text = Format(CDec(frmxml.dgvgrid_xml.CurrentRow.Cells("vlrunit").Value), "#,##0.00")

        txtqtde.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("Qtde").Value
        txtqtdecx.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("Qtdecx").Value
        txtun.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("un").Value
        txtvlrfrte.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vlrfrete").Value.ToString.Replace(".", ",")
        txtfcpst.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vFCP_st").Value.ToString.Replace(".", ",")

        txtcodbarra.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("codbarra").Value

        If txtvlrfrte.Text = "" Then
            txtvlrfrte.Text = "0,00"
        End If

        Try

            txtvlrvenda.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("pmc").Value

            txtvlrcusto.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("custofinal").Value

        Catch ex As Exception

        End Try

        txtqtde.Text = txtqtde.Text.Replace(".", ",")

        Try

            If txtvlrvenda.Text <> "0,00" And txtvlrcusto.Text <> "0,00" Then
                txtmva.Text = (CDec(txtvlrvenda.Text) - CDec(txtvlrcusto.Text)) * 100 / CDec(txtvlrcusto.Text)
                txtmva.Text = Format(CDec(txtmva.Text), "#,##0.00")
            End If

        Catch ex As Exception

        End Try




        If frmxml.dgvgrid_xml.CurrentRow.Cells("vlrst").Value <> "" Then

            If frmxml.dgvgrid_xml.CurrentRow.Cells("cst").Value.ToString.Substring(1, 2) = "60" Then
                txtst.Text = "0,00"
            Else
                txtst.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vlrst").Value
                txtst.Text = txtst.Text.Replace(".", ",")
            End If

        End If

        If frmxml.dgvgrid_xml.CurrentRow.Cells("vlripi").Value <> "" Then

            txtipi.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vlripi").Value
            txtipi.Text = txtipi.Text.Replace(".", ",")
        End If

        If frmxml.dgvgrid_xml.CurrentRow.Cells("vlrtotal").Value <> "" Then

            txtvlrtotal.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vlrtotal").Value
            txtvlrtotal.Text = txtvlrtotal.Text.Replace(".", ",")
        End If

        If frmxml.dgvgrid_xml.CurrentRow.Cells("vlrdesc").Value <> "" Then

            txtvlrdesc.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vlrdesc").Value
            txtvlrdesc.Text = txtvlrdesc.Text.Replace(".", ",")
        End If
            If frmxml.dgvgrid_xml.CurrentRow.Cells("vlroutros").Value <> "" Then

            txtvlrout.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("vlroutros").Value
            txtvlrout.Text = txtvlrout.Text.Replace(".", ",")
        End If

    End Sub

    Private Sub txtqtdecx_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtqtdecx.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If

        If (e.KeyChar = Chr(13)) Then

            '  calcula_custo()

            '  txtqtdenfe.Text = CDec(txtqtdecx.Text) * txtqtde.Text
            '  txtmargem.Text = Format(CDec(txtmargem.Text), "###0.00")

            '  forma_prvenda()

           
            ' txtvlrvenda.Focus()
        End If
    End Sub

    Private Sub calcula_custo()
        If txtqtdecx.Text = "" Then
            txtqtdecx.Text = "1"
        End If
        If CDec(txtqtdecx.Text) < 1 Then
            MsgBox("A quantidade na caixa deve = 1 ou + que 1 ")
            txtqtdecx.Focus()
            Exit Sub
        End If

        txtvlrcusto.Text = "0,00"

        txtvlrcusto.Text = CDec(txtvlrtotal.Text) + (CDec(txtst.Text) + CDec(txtipi.Text) + CDec(txtvlrout.Text) + CDec(txtvlrfrte.Text) + CDec(txtfcpst.Text))
        txtvlrcusto.Text = Format(CDec(txtvlrcusto.Text), "#,##0.00")

        txtvlrcusto.Text = CDec(txtvlrcusto.Text) - CDec(txtvlrdesc.Text)
        txtvlrcusto.Text = Format(CDec(txtvlrcusto.Text), "#,##0.00")

        txtvlrcusto.Text = CDec(txtvlrcusto.Text) / CDec(txtqtde.Text)
        txtvlrcusto.Text = Format(CDec(txtvlrcusto.Text), "#,##0.00")

        txtvlrcusto.Text = CDec(txtvlrcusto.Text) / CDec(txtqtdecx.Text)
        txtvlrcusto.Text = Format(CDec(txtvlrcusto.Text), "#,##0.00")



        txtmargem.Text = (CDec(txtvenda_sg.Text) - CDec(txtvlrcusto.Text)) * 100 / CDec(txtvlrcusto.Text)
        txtmargem.Text = Format(CDec(txtmargem.Text), "#,##0.00")

    End Sub

    Private Sub frmcalcpreco_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If txtqtdecx.Text = "" Then
            txtqtdecx.Text = "1"
        End If

        ' Try
        If lerINI(Application.StartupPath & "\sglinear.ini", "FORMA_PRECO", "salvapreco") = True Then
            Try
                salvapreco()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End If
        frmxml.dgvgrid_xml.CurrentRow.Cells("pmc").Value = txtvlrvenda.Text
        frmxml.dgvgrid_xml.CurrentRow.Cells("custofinal").Value = txtvlrcusto.Text
        frmxml.dgvgrid_xml.CurrentRow.Cells("Qtdecx").Value = txtqtdecx.Text
        '    Catch ex As Exception
        'MsgBox(ex.Message)
        '  End Try

    End Sub

    Private Sub frmcalcpreco_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub txtmva_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmva.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If

        If (e.KeyChar = Chr(13)) Then



        End If
    End Sub

    Private Sub forma_prvenda()
        If txtqtdecx.Text = "" Then
            MsgBox("Informe a Quantidade da caixa ?", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtdecx.Focus()
            Exit Sub
        End If
        If CDec(txtqtdecx.Text) < 1 Then
            MsgBox("A Quantidade não pode ser menor que (1) !", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtdecx.Focus()
            Exit Sub
        End If
        If txtqtde.Text = "" Then
            MsgBox("Informe a Quantidade da NFe ?", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtde.Focus()
            Exit Sub
        End If
        If CDec(txtqtde.Text) < 1 Then
            MsgBox("A Quantidade não pode ser menor que (1) !", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtde.Focus()
            Exit Sub
        End If

        txtvlrvenda.Text = ((CDec(txtvlrcusto.Text) * CDec(txtmva.Text)) / 100) + CDec(txtvlrcusto.Text)
        txtvlrvenda.Text = Format(CDec(txtvlrvenda.Text), "#,##0.00")

    End Sub
    Private Sub txtmva_LostFocus(sender As Object, e As EventArgs) Handles txtmva.LostFocus
        Try
            If txtmva.Text = "" Then
                MsgBox("Informe a Margem de Venda do Produto ? ")
                txtmva.Focus()
                Exit Sub
            End If
            txtmva.Text = Format(CDec(txtmva.Text), "#,##0.00")

            forma_prvenda()

        Catch ex As Exception
            txtmva.Text = "0,00"
        End Try

    End Sub

   Private Sub bntcalc_Click(sender As Object, e As EventArgs) Handles bntcalc.Click
       

        If txtqtdecx.Text = "" Then
            MsgBox("Informe a Quantidade da caixa ?", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtdecx.Focus()
            Exit Sub
        End If
        If CDec(txtqtdecx.Text) < 1 Then
            MsgBox("A Quantidade não pode ser menor que (1) !", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtdecx.Focus()
            Exit Sub
        End If
        If txtqtde.Text = "" Then
            MsgBox("Informe a Quantidade da NFe ?", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtde.Focus()
            Exit Sub
        End If
        If CDec(txtqtde.Text) < 1 Then
            MsgBox("A Quantidade não pode ser menor que (1) !", MsgBoxStyle.Exclamation, "Aviso !")
            txtqtde.Focus()
            Exit Sub
        End If

        txtvlrvenda.Text = ((CDec(txtvlrcusto.Text) * CDec(txtmva.Text)) / 100) + CDec(txtvlrcusto.Text)
        txtvlrvenda.Text = Format(CDec(txtvlrvenda.Text), "#,##0.00")

    End Sub

    Private Sub limpa()
        txtst.Text = "0,00"
        txtipi.Text = "0,00"
        txtvlrdesc.Text = "0,00"
        txtvlrunit.Text = "0,00"
        txtqtdecx.Text = "1"
        txtmva.Text = "0,00"
        txtvlrcusto.Text = "0,00"
        txtvlrout.Text = "0,00"
        txtvlrvenda.Text = "0,00"
        txtvlrtotal.Text = "0,00"
        txtqtde.Text = "0,00"
    End Sub

    Private Sub frmcalcpreco_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        limpa()
        item()
        txtcod.Text = frmxml.dgvgrid_xml.CurrentRow.Cells("codfor").Value
        txtqtdecx.Focus()

        If txtcod_sg.Text <> "" Then
            alterados_sglinx()
            cod_for()
        End If

        If txtcod_sg.Text = "0" Then
            Label22.Visible = True
            txtcusto_sg.Text = "0,00"
            txtvenda_sg.Text = "0,00"
        Else
            Label22.Visible = False
        End If
    End Sub
    Private Sub txtvlroutros_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlrout.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If

        If (e.KeyChar = Chr(13)) Then

            calcula_custo()

        End If
    End Sub

   
    Private Sub salvapreco()

        If txtvlrvenda.Text <> "" Then

            Dim rs As New ADODB.Recordset
            conectar_preco()

            rs.Open("select * from preco_nfe where chave_pr like '" & frmxml.mskchave.Text.Replace(" ", "") + frmxml.dgvgrid_xml.CurrentRow.Cells(0).Value & "'", cn_p, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
            If rs.EOF = True Then
                rs.AddNew()
            End If
            rs.Fields("chave_pr").Value = frmxml.mskchave.Text.Replace(" ", "") + frmxml.dgvgrid_xml.CurrentRow.Cells(0).Value
            rs.Fields("Qtdecx").Value = txtqtdecx.Text
            rs.Fields("preco_vd").Value = txtvlrvenda.Text
            rs.Fields("precocusto_vd").Value = txtvlrcusto.Text
            rs.Update()

            rs.Close()
            cn_p.Close()
        End If
    End Sub

    Private Sub txtvlrvenda_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlrvenda.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If
    End Sub

    Private Sub txtvlrvenda_LostFocus(sender As Object, e As EventArgs) Handles txtvlrvenda.LostFocus

        Try
            If txtvlrvenda.Text = "" Or txtvlrvenda.Text = "0" Then
                txtvlrvenda.Text = "0,00"
            Else
                txtvlrvenda.Text = Format(CDec(txtvlrvenda.Text), "#,##0.00")
                txtmva.Text = (CDec(txtvlrvenda.Text) - CDec(txtvlrcusto.Text)) * 100 / CDec(txtvlrcusto.Text)
                txtmva.Text = Format(CDec(txtmva.Text), "#,##0.00")
            End If
        Catch ex As Exception
            txtvlrvenda.Text = "0,00"
            txtvlrvenda.Focus()
        End Try
       
    End Sub

    Private Sub txtqtdecx_LostFocus(sender As Object, e As EventArgs) Handles txtqtdecx.LostFocus
        Try
            If txtqtdecx.Text = "" Then
                MsgBox("Informe a Quantidade da caixa ?")
                txtqtdecx.Focus()
                Exit Sub
            End If

            calcula_custo()


            If txtcod_sg.Text <> "" Then
                executa_query_SGlinx("update es1 set es1_qembc = '" & txtqtdecx.Text.Replace(".", "").Replace(",", ".") & "', es1_prcusto = '" & txtvlrcusto.Text.Replace(".", "").Replace(",", ".") & "', es1_prcompra =  '" & txtvlrcusto.Text.Replace(".", "").Replace(",", ".") & "' where es1_cod = '" & txtcod_sg.Text & "'")
            End If

            forma_prvenda()

           

        Catch ex As Exception
            txtqtdecx.Text = "1"
            calcula_custo()
        End Try

    End Sub

    Private Sub txtvlrcusto_LostFocus(sender As Object, e As EventArgs) Handles txtvlrcusto.LostFocus
        Try
            If txtvlrcusto.Text = "" Or txtvlrcusto.Text = "0" Then
                txtvlrcusto.Text = "0,00"
            End If
        Catch ex As Exception
            txtvlrcusto.Text = "0,00"
        End Try

    End Sub

    Private Sub txtqtdecx_TextChanged(sender As Object, e As EventArgs) Handles txtqtdecx.TextChanged

    End Sub

    Private Sub chkeditar_CheckedChanged(sender As Object, e As EventArgs) Handles chkeditar.CheckedChanged
        If chkeditar.Checked = True Then
            Label14.Visible = True
            txtst.ReadOnly = False

            txtqtde.BackColor = Color.White
            txtqtde.ReadOnly = False

            txtst.BackColor = Color.White
            txtipi.ReadOnly = False
            txtvlrfrte.BackColor = Color.White
            txtvlrfrte.ReadOnly = False
            txtipi.BackColor = Color.White
            txtvlrout.ReadOnly = False
            txtvlrout.BackColor = Color.White
            txtvlrdesc.ReadOnly = False
            txtvlrdesc.BackColor = Color.White

        Else

            Label14.Visible = False

            txtst.ReadOnly = True

            txtqtde.ReadOnly = True

            txtipi.ReadOnly = True

            txtvlrout.ReadOnly = True

            txtvlrdesc.ReadOnly = True

        End If


    End Sub

    Function SoNumeros(ByVal Keyascii As Short) As Short
        If InStr("1234567890,", Chr(Keyascii)) = 0 Then
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

    Private Sub txtst_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtst.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If
    End Sub

    Private Sub txtst_LostFocus(sender As Object, e As EventArgs) Handles txtst.LostFocus
        If txtst.Text = "" Then
            txtst.Text = "0,00"
            else
            txtst.Text = Format(CDec(txtst.Text), "#,##0.00")
        End If
    End Sub

    Private Sub txtipi_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtipi.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If
    End Sub

    Private Sub txtipi_LostFocus(sender As Object, e As EventArgs) Handles txtipi.LostFocus
        If txtipi.Text = "" Then
            txtipi.Text = "0,00"
        Else
            txtipi.Text = Format(CDec(txtipi.Text), "#,##0.00")
        End If
    End Sub

   Private Sub txtvlrout_LostFocus(sender As Object, e As EventArgs) Handles txtvlrout.LostFocus
        If txtvlrout.Text = "" Then
            txtvlrout.Text = "0,00"
        Else
            txtvlrout.Text = Format(CDec(txtvlrout.Text), "#,##0.00")
        End If
    End Sub

    Private Sub txtvlrdesc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvlrdesc.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If
    End Sub

    Private Sub txtvlrdesc_LostFocus(sender As Object, e As EventArgs) Handles txtvlrdesc.LostFocus
        If txtvlrdesc.Text = "" Then
            txtvlrdesc.Text = "0,00"
        Else
            txtvlrdesc.Text = Format(CDec(txtvlrdesc.Text), "#,##0.00")
        End If
    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click
        calcula_custo()
    End Sub

    Private Sub txtqtde_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtqtde.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If
    End Sub

  Private Sub txtvlrfrte_LostFocus(sender As Object, e As EventArgs) Handles txtvlrfrte.LostFocus
        If txtvlrfrte.Text = "" Then
            txtvlrfrte.Text = "0,00"
        Else
            txtvlrfrte.Text = Format(CDec(txtvlrfrte.Text), "#,##0.00")
        End If
    End Sub

    Private Sub txtvenda_sg_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtvenda_sg.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        KeyAscii = CShort(SoNumeros(KeyAscii))

        If KeyAscii = 0 Then

            e.Handled = True

        End If
    End Sub

    Private Sub txtvenda_sg_LostFocus(sender As Object, e As EventArgs) Handles txtvenda_sg.LostFocus
        If txtvenda_sg.Text = "" Then
            txtvenda_sg.Text = "0,00"
        Else
            txtvenda_sg.Text = Format(CDec(txtvenda_sg.Text), "#,##0.00")
        End If
    End Sub


#Region "''''Rotinas para Integração do Sistema SGLinear"

    Private Sub alterados_sglinx()
        Dim sql As String
        Dim rs As New ADODB.Recordset

        conectarsqlinx()

        sql = "SELECT * FROM es1  where es1_cod = '" & txtcod_sg.Text & "'"

        rs.Open(sql, cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)


        If rs.EOF = False Then
         
            txtvenda_sg.Text = rs.Fields("es1_prvarejo").Value
            txtcusto_sg.Text = rs.Fields("es1_prcusto").Value
            txtnfe_num.Text = rs.Fields("CMD_NUM").Value
            txtdata_nfe.Text = rs.Fields("Es1_ULTCOMPRA").Value.ToString
            txtqtde_ult.Text = rs.Fields("ES1_QUANT").Value.ToString
            txtmargem.Text = rs.Fields("es1_ultmargem").Value.ToString
            txtmva.Text = rs.Fields("es1_ultmargem").Value.ToString

        End If

      
        cnsql.Close()
    End Sub

    Private Sub cod_for()
        Dim sql As String
        Dim rs As New ADODB.Recordset

        conectarsqlinx()

        sql = "SELECT * from cg2 where cg2_cgc = '" & frmxml.mskcnpjfor.Text & "'"

        rs.Open(sql, cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)


        If rs.EOF = False Then

            txtcod_for.Text = rs.Fields("cg2_cod").Value


        End If

        cnsql.Close()
    End Sub

#End Region

    Private Sub txtcod_sg_LostFocus(sender As Object, e As EventArgs) Handles txtcod_sg.LostFocus
        Exit Sub
        If txtcod_for.Text <> "" Then
            Dim rsclisg As New ADODB.Recordset

            conectarsqlinx()
            rsclisg.Open("select * from es1i where cg2_cod = '" & txtcod_for.Text & "' and es1_codforn = '" & txtcod.Text & "'", cnsql, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

            If rsclisg.EOF = True Then
                rsclisg.AddNew()
                rsclisg.Fields("es1_cod").Value = txtcod_sg.Text
                rsclisg.Fields("cg2_cod").Value = txtcod_for.Text
                rsclisg.Fields("es1_codforn").Value = txtcod.Text
                rsclisg.Fields("cg1_cod").Value = "0"
                rsclisg.Update()
            Else

                rsclisg.Fields("es1_cod").Value = txtcod_sg.Text
                rsclisg.Update()

            End If




            cnsql.Close()

        Else

            MsgBox("Fornecedor não cadastrado !!!", MsgBoxStyle.Exclamation, "Aviso !")
        End If
    End Sub

   
    Private Sub txtmva_TextChanged(sender As Object, e As EventArgs) Handles txtmva.TextChanged

    End Sub

    Private Sub txtvlrvenda_TextChanged(sender As Object, e As EventArgs) Handles txtvlrvenda.TextChanged

    End Sub
End Class
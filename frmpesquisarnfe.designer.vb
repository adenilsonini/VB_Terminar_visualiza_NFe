<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmpesquisarnfe
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkstatus_nfe = New System.Windows.Forms.CheckBox()
        Me.chkvlrboleto = New System.Windows.Forms.CheckBox()
        Me.cbostatusnfe = New System.Windows.Forms.ComboBox()
        Me.Paneldata = New System.Windows.Forms.Panel()
        Me.lbldataini = New System.Windows.Forms.Label()
        Me.lbldatafim = New System.Windows.Forms.Label()
        Me.mskdatainicial = New System.Windows.Forms.DateTimePicker()
        Me.mskdatafinal = New System.Windows.Forms.DateTimePicker()
        Me.txtpar = New System.Windows.Forms.TextBox()
        Me.txtpesquisa = New System.Windows.Forms.TextBox()
        Me.cbotipo = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblinfo = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dgvpesquisa = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.vnf = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.chave_nfe = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.vlr_boleto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.status_nfe = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ManifestacaoDestinatarioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.Paneldata.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvpesquisa, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chkstatus_nfe)
        Me.Panel1.Controls.Add(Me.chkvlrboleto)
        Me.Panel1.Controls.Add(Me.cbostatusnfe)
        Me.Panel1.Controls.Add(Me.Paneldata)
        Me.Panel1.Controls.Add(Me.txtpar)
        Me.Panel1.Controls.Add(Me.txtpesquisa)
        Me.Panel1.Controls.Add(Me.cbotipo)
        Me.Panel1.Location = New System.Drawing.Point(5, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(936, 54)
        Me.Panel1.TabIndex = 0
        '
        'chkstatus_nfe
        '
        Me.chkstatus_nfe.AutoSize = True
        Me.chkstatus_nfe.Checked = True
        Me.chkstatus_nfe.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkstatus_nfe.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkstatus_nfe.ForeColor = System.Drawing.Color.Blue
        Me.chkstatus_nfe.Location = New System.Drawing.Point(7, 2)
        Me.chkstatus_nfe.Name = "chkstatus_nfe"
        Me.chkstatus_nfe.Size = New System.Drawing.Size(103, 20)
        Me.chkstatus_nfe.TabIndex = 46
        Me.chkstatus_nfe.Text = "NFe Status"
        Me.chkstatus_nfe.UseVisualStyleBackColor = True
        '
        'chkvlrboleto
        '
        Me.chkvlrboleto.AutoSize = True
        Me.chkvlrboleto.Location = New System.Drawing.Point(494, 26)
        Me.chkvlrboleto.Name = "chkvlrboleto"
        Me.chkvlrboleto.Size = New System.Drawing.Size(83, 17)
        Me.chkvlrboleto.TabIndex = 40
        Me.chkvlrboleto.Text = "Valor Boleto"
        Me.chkvlrboleto.UseVisualStyleBackColor = True
        '
        'cbostatusnfe
        '
        Me.cbostatusnfe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbostatusnfe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbostatusnfe.FormattingEnabled = True
        Me.cbostatusnfe.Items.AddRange(New Object() {"NF-e autorizada", "NF-e cancelada"})
        Me.cbostatusnfe.Location = New System.Drawing.Point(6, 24)
        Me.cbostatusnfe.Name = "cbostatusnfe"
        Me.cbostatusnfe.Size = New System.Drawing.Size(118, 21)
        Me.cbostatusnfe.TabIndex = 45
        '
        'Paneldata
        '
        Me.Paneldata.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Paneldata.Controls.Add(Me.lbldataini)
        Me.Paneldata.Controls.Add(Me.lbldatafim)
        Me.Paneldata.Controls.Add(Me.mskdatainicial)
        Me.Paneldata.Controls.Add(Me.mskdatafinal)
        Me.Paneldata.Enabled = False
        Me.Paneldata.Location = New System.Drawing.Point(715, 0)
        Me.Paneldata.Name = "Paneldata"
        Me.Paneldata.Size = New System.Drawing.Size(216, 49)
        Me.Paneldata.TabIndex = 39
        '
        'lbldataini
        '
        Me.lbldataini.AutoSize = True
        Me.lbldataini.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldataini.Location = New System.Drawing.Point(3, 3)
        Me.lbldataini.Name = "lbldataini"
        Me.lbldataini.Size = New System.Drawing.Size(74, 16)
        Me.lbldataini.TabIndex = 37
        Me.lbldataini.Text = "Data Inicial"
        '
        'lbldatafim
        '
        Me.lbldatafim.AutoSize = True
        Me.lbldatafim.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldatafim.Location = New System.Drawing.Point(108, 3)
        Me.lbldatafim.Name = "lbldatafim"
        Me.lbldatafim.Size = New System.Drawing.Size(69, 16)
        Me.lbldatafim.TabIndex = 38
        Me.lbldatafim.Text = "Data Final"
        '
        'mskdatainicial
        '
        Me.mskdatainicial.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.mskdatainicial.Location = New System.Drawing.Point(6, 22)
        Me.mskdatainicial.Name = "mskdatainicial"
        Me.mskdatainicial.Size = New System.Drawing.Size(99, 20)
        Me.mskdatainicial.TabIndex = 36
        '
        'mskdatafinal
        '
        Me.mskdatafinal.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.mskdatafinal.Location = New System.Drawing.Point(111, 22)
        Me.mskdatafinal.Name = "mskdatafinal"
        Me.mskdatafinal.Size = New System.Drawing.Size(99, 20)
        Me.mskdatafinal.TabIndex = 35
        '
        'txtpar
        '
        Me.txtpar.Location = New System.Drawing.Point(290, 3)
        Me.txtpar.Name = "txtpar"
        Me.txtpar.Size = New System.Drawing.Size(65, 20)
        Me.txtpar.TabIndex = 32
        Me.txtpar.Text = "Nome_emi"
        Me.txtpar.Visible = False
        '
        'txtpesquisa
        '
        Me.txtpesquisa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtpesquisa.Location = New System.Drawing.Point(140, 26)
        Me.txtpesquisa.Name = "txtpesquisa"
        Me.txtpesquisa.Size = New System.Drawing.Size(337, 20)
        Me.txtpesquisa.TabIndex = 14
        '
        'cbotipo
        '
        Me.cbotipo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cbotipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbotipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbotipo.ForeColor = System.Drawing.Color.Blue
        Me.cbotipo.FormattingEnabled = True
        Me.cbotipo.Items.AddRange(New Object() {"Nome do Fornecedor", "CNPJ Fornecedor", "Valor NFe", "Chave de Acesso NFe", "Numero NFe", "Entre Datas"})
        Me.cbotipo.Location = New System.Drawing.Point(140, 3)
        Me.cbotipo.Name = "cbotipo"
        Me.cbotipo.Size = New System.Drawing.Size(144, 21)
        Me.cbotipo.TabIndex = 13
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(102, 384)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(337, 22)
        Me.TextBox1.TabIndex = 34
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(644, 105)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 34
        Me.Button1.Text = "sql"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'lblinfo
        '
        Me.lblinfo.AutoSize = True
        Me.lblinfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblinfo.ForeColor = System.Drawing.Color.Blue
        Me.lblinfo.Location = New System.Drawing.Point(445, 384)
        Me.lblinfo.Name = "lblinfo"
        Me.lblinfo.Size = New System.Drawing.Size(39, 20)
        Me.lblinfo.TabIndex = 33
        Me.lblinfo.Text = "info"
        Me.lblinfo.Visible = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(563, 105)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 39
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.lblinfo)
        Me.Panel2.Controls.Add(Me.dgvpesquisa)
        Me.Panel2.Controls.Add(Me.TextBox1)
        Me.Panel2.Location = New System.Drawing.Point(5, 72)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(936, 413)
        Me.Panel2.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(6, 387)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 16)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "Chave NF-e"
        '
        'dgvpesquisa
        '
        Me.dgvpesquisa.AllowUserToAddRows = False
        Me.dgvpesquisa.AllowUserToDeleteRows = False
        Me.dgvpesquisa.AllowUserToResizeColumns = False
        Me.dgvpesquisa.AllowUserToResizeRows = False
        Me.dgvpesquisa.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvpesquisa.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvpesquisa.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column7, Me.vnf, Me.Column3, Me.Column4, Me.Column5, Me.chave_nfe, Me.vlr_boleto, Me.status_nfe})
        Me.dgvpesquisa.ContextMenuStrip = Me.ContextMenuStrip1
        Me.dgvpesquisa.EnableHeadersVisualStyles = False
        Me.dgvpesquisa.GridColor = System.Drawing.Color.Red
        Me.dgvpesquisa.Location = New System.Drawing.Point(3, 3)
        Me.dgvpesquisa.Name = "dgvpesquisa"
        Me.dgvpesquisa.ReadOnly = True
        Me.dgvpesquisa.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Gray
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvpesquisa.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvpesquisa.RowHeadersWidth = 22
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        Me.dgvpesquisa.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvpesquisa.Size = New System.Drawing.Size(928, 375)
        Me.dgvpesquisa.TabIndex = 2
        '
        'Column1
        '
        Me.Column1.HeaderText = "Nome do Fornecedor"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 300
        '
        'Column7
        '
        Me.Column7.HeaderText = "Número NFe"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Width = 75
        '
        'vnf
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.vnf.DefaultCellStyle = DataGridViewCellStyle2
        Me.vnf.HeaderText = "Valor NFe"
        Me.vnf.Name = "vnf"
        Me.vnf.ReadOnly = True
        Me.vnf.Width = 80
        '
        'Column3
        '
        DataGridViewCellStyle3.Format = "d"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column3.HeaderText = "Data Emissão"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 85
        '
        'Column4
        '
        Me.Column4.HeaderText = "XML NFe"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column4.Width = 60
        '
        'Column5
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column5.HeaderText = "Visualizar NFe"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column5.Width = 80
        '
        'chave_nfe
        '
        Me.chave_nfe.HeaderText = "chave nfe"
        Me.chave_nfe.Name = "chave_nfe"
        Me.chave_nfe.ReadOnly = True
        Me.chave_nfe.Visible = False
        '
        'vlr_boleto
        '
        Me.vlr_boleto.HeaderText = "Valor Boleto"
        Me.vlr_boleto.Name = "vlr_boleto"
        Me.vlr_boleto.ReadOnly = True
        Me.vlr_boleto.Width = 80
        '
        'status_nfe
        '
        Me.status_nfe.HeaderText = "Status NF-e"
        Me.status_nfe.Name = "status_nfe"
        Me.status_nfe.ReadOnly = True
        Me.status_nfe.Width = 130
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ManifestacaoDestinatarioToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(212, 26)
        '
        'ManifestacaoDestinatarioToolStripMenuItem
        '
        Me.ManifestacaoDestinatarioToolStripMenuItem.Name = "ManifestacaoDestinatarioToolStripMenuItem"
        Me.ManifestacaoDestinatarioToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.ManifestacaoDestinatarioToolStripMenuItem.Text = "Manifestacao Destinatario"
        '
        'frmpesquisarnfe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(942, 485)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmpesquisarnfe"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tela de Pesquisa de Notas Fiscais"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Paneldata.ResumeLayout(False)
        Me.Paneldata.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvpesquisa, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtpesquisa As System.Windows.Forms.TextBox
    Friend WithEvents cbotipo As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dgvpesquisa As System.Windows.Forms.DataGridView
    Friend WithEvents txtpar As System.Windows.Forms.TextBox
    Friend WithEvents lblinfo As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents mskdatainicial As System.Windows.Forms.DateTimePicker
    Friend WithEvents mskdatafinal As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbldatafim As System.Windows.Forms.Label
    Friend WithEvents lbldataini As System.Windows.Forms.Label
    Friend WithEvents Paneldata As System.Windows.Forms.Panel
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents chkvlrboleto As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ManifestacaoDestinatarioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents vnf As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents chave_nfe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents vlr_boleto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents status_nfe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbostatusnfe As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkstatus_nfe As System.Windows.Forms.CheckBox
End Class

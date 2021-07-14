<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmdanfe
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmdanfe))
        Me.PrintPreviewControl1 = New System.Windows.Forms.PrintPreviewControl()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.cbopage = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblpag = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.cboimpressora = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkselecao = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.pdlImpressoras = New System.Windows.Forms.PrintDialog()
        Me.pinfo = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtchavenfe = New System.Windows.Forms.TextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.pinfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintPreviewControl1
        '
        Me.PrintPreviewControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrintPreviewControl1.AutoZoom = False
        Me.PrintPreviewControl1.Location = New System.Drawing.Point(2, 47)
        Me.PrintPreviewControl1.Name = "PrintPreviewControl1"
        Me.PrintPreviewControl1.Size = New System.Drawing.Size(1197, 372)
        Me.PrintPreviewControl1.TabIndex = 0
        Me.PrintPreviewControl1.Zoom = 1.5R
        '
        'cbopage
        '
        Me.cbopage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbopage.FormattingEnabled = True
        Me.cbopage.Location = New System.Drawing.Point(170, 11)
        Me.cbopage.Name = "cbopage"
        Me.cbopage.Size = New System.Drawing.Size(64, 21)
        Me.cbopage.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Total Páginas"
        '
        'lblpag
        '
        Me.lblpag.AutoSize = True
        Me.lblpag.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblpag.Location = New System.Drawing.Point(101, 13)
        Me.lblpag.Name = "lblpag"
        Me.lblpag.Size = New System.Drawing.Size(16, 16)
        Me.lblpag.TabIndex = 5
        Me.lblpag.Text = "0"
        '
        'PictureBox1
        '
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(310, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(34, 32)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "Clique aqui para imprimir o DANFE")
        '
        'PictureBox2
        '
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(362, 4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(27, 32)
        Me.PictureBox2.TabIndex = 7
        Me.PictureBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox2, "Clique aqui para gerar o DANFE em PDF")
        '
        'cboimpressora
        '
        Me.cboimpressora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboimpressora.FormattingEnabled = True
        Me.cboimpressora.Location = New System.Drawing.Point(659, 17)
        Me.cboimpressora.Name = "cboimpressora"
        Me.cboimpressora.Size = New System.Drawing.Size(318, 21)
        Me.cboimpressora.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(656, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(148, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Impressoras instaladas"
        '
        'chkselecao
        '
        Me.chkselecao.AutoSize = True
        Me.chkselecao.Location = New System.Drawing.Point(563, 20)
        Me.chkselecao.Name = "chkselecao"
        Me.chkselecao.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkselecao.Size = New System.Drawing.Size(90, 17)
        Me.chkselecao.TabIndex = 10
        Me.chkselecao.Text = "Usar Seleção"
        Me.chkselecao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkselecao.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.chkselecao)
        Me.Panel1.Controls.Add(Me.cbopage)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.lblpag)
        Me.Panel1.Controls.Add(Me.cboimpressora)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Location = New System.Drawing.Point(2, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1197, 44)
        Me.Panel1.TabIndex = 11
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Button4.Location = New System.Drawing.Point(983, 9)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(196, 29)
        Me.Button4.TabIndex = 13
        Me.Button4.Text = "Definir impressora Padao"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(424, 6)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(126, 33)
        Me.Button3.TabIndex = 15
        Me.Button3.Text = "Envia NF-e E-mail"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(238, 10)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(29, 23)
        Me.Button2.TabIndex = 12
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(137, 10)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(29, 23)
        Me.Button1.TabIndex = 11
        Me.Button1.UseVisualStyleBackColor = True
        '
        'pdlImpressoras
        '
        Me.pdlImpressoras.UseEXDialog = True
        '
        'pinfo
        '
        Me.pinfo.BackColor = System.Drawing.Color.Blue
        Me.pinfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pinfo.Controls.Add(Me.Label3)
        Me.pinfo.Location = New System.Drawing.Point(370, 54)
        Me.pinfo.Name = "pinfo"
        Me.pinfo.Size = New System.Drawing.Size(349, 52)
        Me.pinfo.TabIndex = 12
        Me.pinfo.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(16, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(322, 24)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Gerando arquivo PDF, Aguarde..."
        '
        'txtchavenfe
        '
        Me.txtchavenfe.Location = New System.Drawing.Point(370, 123)
        Me.txtchavenfe.Name = "txtchavenfe"
        Me.txtchavenfe.Size = New System.Drawing.Size(349, 20)
        Me.txtchavenfe.TabIndex = 13
        Me.txtchavenfe.Visible = False
        '
        'frmdanfe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 417)
        Me.Controls.Add(Me.txtchavenfe)
        Me.Controls.Add(Me.pinfo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PrintPreviewControl1)
        Me.Name = "frmdanfe"
        Me.Text = "DANFE DA NF-e"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pinfo.ResumeLayout(False)
        Me.pinfo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PrintPreviewControl1 As System.Windows.Forms.PrintPreviewControl
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents cbopage As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblpag As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents cboimpressora As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkselecao As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents pdlImpressoras As System.Windows.Forms.PrintDialog
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents pinfo As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents txtchavenfe As System.Windows.Forms.TextBox
End Class

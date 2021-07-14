<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frminfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frminfo))
        Me.pinfo = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.txtinfo = New System.Windows.Forms.TextBox()
        Me.pic_Ico = New System.Windows.Forms.PictureBox()
        Me.pinfo.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pic_Ico, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pinfo
        '
        Me.pinfo.BackColor = System.Drawing.Color.Red
        Me.pinfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pinfo.Controls.Add(Me.pic_Ico)
        Me.pinfo.Controls.Add(Me.PictureBox2)
        Me.pinfo.Controls.Add(Me.txtinfo)
        Me.pinfo.Location = New System.Drawing.Point(1, 0)
        Me.pinfo.Name = "pinfo"
        Me.pinfo.Size = New System.Drawing.Size(353, 124)
        Me.pinfo.TabIndex = 39
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))

        Me.PictureBox2.Location = New System.Drawing.Point(118, 65)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(112, 38)
        Me.PictureBox2.TabIndex = 40
        Me.PictureBox2.TabStop = False
        '
        'txtinfo
        '
        Me.txtinfo.AccessibleDescription = resources.GetString("txtinfo.AccessibleDescription")
        Me.txtinfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtinfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtinfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtinfo.Location = New System.Drawing.Point(3, 3)
        Me.txtinfo.Multiline = True
        Me.txtinfo.Name = "txtinfo"
        Me.txtinfo.Size = New System.Drawing.Size(345, 116)
        Me.txtinfo.TabIndex = 38
        Me.txtinfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'pic_Ico
        '
        Me.pic_Ico.Image = CType(resources.GetObject("pic_Ico.Image"), System.Drawing.Image)
        Me.pic_Ico.Location = New System.Drawing.Point(5, 104)
        Me.pic_Ico.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pic_Ico.Name = "pic_Ico"
        Me.pic_Ico.Size = New System.Drawing.Size(340, 11)
        Me.pic_Ico.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pic_Ico.TabIndex = 42
        Me.pic_Ico.TabStop = False
        Me.pic_Ico.WaitOnLoad = True
        '
        'frminfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(355, 123)
        Me.Controls.Add(Me.pinfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frminfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frminfo"
        Me.pinfo.ResumeLayout(False)
        Me.pinfo.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pic_Ico, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pinfo As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents txtinfo As System.Windows.Forms.TextBox
    Friend WithEvents pic_Ico As System.Windows.Forms.PictureBox
End Class

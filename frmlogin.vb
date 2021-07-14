Imports System.Reflection

Public Class frmlogin

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress

        If (e.KeyChar = Chr(13)) Then
            If TextBox1.Text = "1019" Then


                Dim arquivoIni As ArquivoIni = New ArquivoIni(Application.StartupPath & "\config.ini")
                If arquivoIni.KeyExists("bloqueio", "DFe") = False Then
                    arquivoIni.Write("bloqueio", "false", "DFe")
                End If

                If arquivoIni.Read("bloqueio", "DFe") = True Then
                    MsgBox("Sistema Bloqueado, licensa vencida !", MsgBoxStyle.Exclamation, "Aviso !")
                    End
                End If


                If IsConectedToHost(New Uri("http://www.google.com")) = False Then

                    MsgBox("Você esta sem conexao com a internet !" & vbCrLf & "Operação cancelada !", MsgBoxStyle.Exclamation, "Aviso !")
                    End

                End If

                frmxml.Show()
            Else
                MsgBox("Senha informada é Invalida !!!", MsgBoxStyle.Exclamation, "Aviso !")
            End If
        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub frmlogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "RFB XML Online  |  Versao: " & Assembly.GetExecutingAssembly.GetName.Version.ToString & "  09-04-2020   |   .Net 4.5"

       
    End Sub


End Class
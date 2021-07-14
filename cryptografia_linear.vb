Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text


Module cryptografia_linear
    Function Criptografar(ByVal texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return ""

        Using rfc2898DeriveBytes As Rfc2898DeriveBytes = New Rfc2898DeriveBytes("l1n3@r", New Byte(12) {CByte(73), CByte(118), CByte(97), CByte(110), CByte(32), CByte(77), CByte(101), CByte(100), CByte(118), CByte(101), CByte(100), CByte(101), CByte(118)})
            Dim bytes As Byte() = Encoding.Unicode.GetBytes(texto)

            Using aes As Aes = aes.Create()
                aes.Key = rfc2898DeriveBytes.GetBytes(32)
                aes.IV = rfc2898DeriveBytes.GetBytes(16)

                Using memoryStream As MemoryStream = New MemoryStream()

                    Using cryptoStream As CryptoStream = New CryptoStream(CType(memoryStream, Stream), aes.CreateEncryptor(), CryptoStreamMode.Write)
                        cryptoStream.Write(bytes, 0, bytes.Length)
                        cryptoStream.Close()
                    End Using

                    texto = Convert.ToBase64String(memoryStream.ToArray())
                End Using
            End Using
        End Using

        Return texto
    End Function

    Function Descriptografar(ByVal texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return ""

        Using rfc2898DeriveBytes As Rfc2898DeriveBytes = New Rfc2898DeriveBytes("l1n3@r", New Byte(12) {CByte(73), CByte(118), CByte(97), CByte(110), CByte(32), CByte(77), CByte(101), CByte(100), CByte(118), CByte(101), CByte(100), CByte(101), CByte(118)})
            texto = texto.Replace(" ", "+")
            Dim buffer As Byte() = Convert.FromBase64String(texto)

            Using aes As Aes = aes.Create()
                aes.Key = rfc2898DeriveBytes.GetBytes(32)
                aes.IV = rfc2898DeriveBytes.GetBytes(16)

                Using memoryStream As MemoryStream = New MemoryStream()

                    Using cryptoStream As CryptoStream = New CryptoStream(CType(memoryStream, Stream), aes.CreateDecryptor(), CryptoStreamMode.Write)
                        cryptoStream.Write(buffer, 0, buffer.Length)
                        cryptoStream.Close()
                    End Using

                    texto = Encoding.Unicode.GetString(memoryStream.ToArray())
                End Using
            End Using
        End Using

        Return texto
    End Function
End Module


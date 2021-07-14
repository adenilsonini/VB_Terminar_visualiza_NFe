Public Class CodigodeBarra

    Private Const BARCODE_HEIGHT As Integer = 80
    Private Const BARCODE_TOP As Integer = 10
    Private Const BARCODE_LEFT As Integer = 10

    Private mShowString As Boolean = False

    Private mUseCompression As Boolean = False
    Private mBarWidth As Single = 1
    Private mCheckSum As Integer = 0
    Private mBinaryString As String = ""
    Private mEncodedString As String = ""


    Private Structure Part
        Dim sValue As String
        Dim sType As String
    End Structure

    Public Enum BCEncoding
        Code128A = 103
        Code128B = 104
        Code128C = 105
    End Enum

    Dim mEncoding As BCEncoding

    Private arCodes128(106) As String
    Private arSymbols(31) As String

    Private Const BC_STOP = 106
    Private Const BC_STARTA = 103
    Private Const BC_STARTB = 104
    Private Const BC_STARTC = 105
    Private Const BC_CODEC = 99
    Private Const BC_CODEB = 100
    Private Const BC_CODEA = 101

    Private Const SPECIAL_CODE_START As String = "["
    Private Const SPECIAL_CODE_END As String = "]"

    '
    Public Sub New(ByVal CodeType As BCEncoding, Optional ByVal UseCompression As Boolean = False)
        mEncoding = CodeType
        mUseCompression = UseCompression
        LoadCodes()
    End Sub

    Public Overloads Function DrawBarCode(ByVal sBarcodeString As String) As Image
        If CheckBarcodeString(sBarcodeString) Then
            Return CreateBarcodeImage(sBarcodeString)
        Else
            Return Nothing
        End If
    End Function

    Public Overloads Function DrawBarCode(ByVal sBarcodeString As String, _
                                          ByVal g As Graphics, _
                                          ByVal snXPos As Single, _
                                          ByVal snYPos As Single, _
                                          ByVal snBarcodeWidth As Single, _
                                          ByVal snBarcodeHeight As Single)

        If CheckBarcodeString(sBarcodeString) Then
            Dim Img As Image = CreateBarcodeImage(sBarcodeString)

            With g
                .PageUnit = GraphicsUnit.Millimeter
                .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                .SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                .CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                .CompositingMode = Drawing2D.CompositingMode.SourceCopy
                .DrawImage(Img, snXPos, snYPos, snBarcodeWidth, snBarcodeHeight)
            End With
        End If
        Return ""
    End Function

    Private Function CreateBarcodeImage(ByVal sBarcodeString As String) As Image
        Dim pb As PictureBox
        Dim gr As Graphics = Nothing
        Dim iStringLength
        Dim k As Integer

        Dim iCalcBarcodeW As Integer

        mEncodedString = sBarcodeString

        If sBarcodeString = "" Then
            Return Nothing
        End If

        If mUseCompression = True And mEncoding <> BCEncoding.Code128C Then
            sBarcodeString = CompressString(sBarcodeString)
        End If

        mBinaryString = MakeBinaryString(sBarcodeString)


        iStringLength = mBinaryString.Length - 1
        'calculate the number of pixels needed to draw the barcode
        iCalcBarcodeW = iStringLength * mBarWidth + 50 'ADDITIONAL 50 PIXELS for the CLIENT AREA

        pb = New PictureBox


        'Creating the Bar Code
        With pb
            .Width = iCalcBarcodeW
            .Height = BARCODE_HEIGHT
            pb.Image = New Bitmap(.Width, .Height)
        End With

        Try

            gr = Graphics.FromImage(pb.Image)
            gr.Clear(Color.White)

            Dim pen As New System.Drawing.Pen(System.Drawing.Color.Black, mBarWidth / 2)
            Dim PenW As New System.Drawing.Pen(System.Drawing.Color.White, mBarWidth / 2)

            For k = 0 To iStringLength
                If mBinaryString.Substring(k, 1) = "1" Then
                    gr.DrawRectangle(pen, BARCODE_LEFT + k * mBarWidth, _
                                      BARCODE_TOP, 1, BARCODE_HEIGHT)
                Else
                    gr.DrawRectangle(PenW, BARCODE_LEFT + k * mBarWidth, _
                                      BARCODE_TOP, 1, BARCODE_HEIGHT)
                End If
            Next

            gr.DrawRectangle(PenW, BARCODE_LEFT + k * mBarWidth, _
                              BARCODE_TOP, 1, BARCODE_HEIGHT)


            If mShowString = True Then
                Dim f As New Font("Arial", 9)
                Dim r As RectangleF
                Dim Sz As SizeF
                Sz = gr.MeasureString(sBarcodeString, f)
                With r
                    .X = (BARCODE_LEFT + k * mBarWidth - Sz.Width) / 2
                    .Y = BARCODE_HEIGHT - Sz.Height
                    .Width = Sz.Width
                    .Height = Sz.Height
                End With
                gr.FillRectangle(Brushes.White, r)

                gr.DrawString(sBarcodeString, f, Brushes.Black, r)

            End If

            Return pb.Image

        Catch se As Exception
            MessageBox.Show(se.Message)
            Return pb.Image
        Finally
            pb.Dispose()
            gr.Dispose()
        End Try
    End Function

    Private Function CheckBarcodeString(ByVal sIn As String) As Boolean
        Dim iCount As Integer
        Dim k As Integer

        Dim s As String

        If mEncoding = BCEncoding.Code128A Then
            '1 check that for each SPECIAL_CODE_START symbol there is 
            '  a SPECIAL_CODE_END symbol
            iCount = sIn.Length - 1


            For k = 0 To sIn.Length - 1
                If sIn.Chars(k) = SPECIAL_CODE_START Then
                    iCount += 1
                ElseIf sIn.Chars(k) = SPECIAL_CODE_END Then
                    iCount -= 1
                End If
            Next
            If iCount <> 0 Then Return False
            'end 1

            '2. check that the symbols enclosed in the string
            '   are only the ones in the arSymbols array
            k = 0
            Do
                k = sIn.IndexOf(SPECIAL_CODE_START, k + 1)
                If k > 0 Then
                    s = GetSymbol(sIn, k)
                    If GetSymbolIndex(s) = 0 Then
                        Return False
                    End If
                End If
            Loop
            'end 2
        ElseIf mEncoding = BCEncoding.Code128C Then
            'check that the string has only an EVEN number of DIGITS
            If sIn.Length Mod 2 <> 0 Then
                Return False
            End If
            'end
            'check that the string has only digits
            For k = 0 To sIn.Length - 1
                If Not IsNumeric(sIn.Chars(k)) Then
                    Return False
                End If
            Next

        End If

        Return True

    End Function

    Private Function MakeBinaryString(ByVal sIn As String) As String
        Dim BinaryS As String
        Dim k As Integer
        Dim sLen As Integer
        Dim j As Integer
        Dim sTemp As String

        'appending the Start_X character
        BinaryS = arCodes128(mEncoding)
        sLen = sIn.Length
        If sLen > 0 Then
            'looping through the string...
            Do
                If mEncoding = BCEncoding.Code128B OrElse mEncoding = BCEncoding.Code128A Then
                    sTemp = sIn.Chars(j)
                    Select Case Asc(sTemp)
                        'start character of a compressed section
                        Case BC_CODEC
                            BinaryS &= arCodes128(BC_CODEC)
                            j += 1

                            Do

                                Select Case sIn.Chars(j)
                                    Case "0" To "9"
                                        BinaryS &= arCodes128(Val(sIn.Substring(j, 2)))

                                        j += 2
                                        If j >= sLen Then Exit Do
                                    Case Else
                                        'end of the compressed section
                                        If mEncoding = BCEncoding.Code128A Then
                                            BinaryS &= arCodes128(BC_CODEA)
                                        ElseIf mEncoding = BCEncoding.Code128B Then
                                            BinaryS &= arCodes128(BC_CODEB)
                                        End If

                                        j += 1
                                        Exit Do
                                End Select
                            Loop
                        Case Else
                            'append the binary format of the character to the binary string
                            BinaryS &= arCodes128(Asc(sIn.Chars(j)) - 32)
                    End Select

                    j += 1
                    If j >= sLen Then Exit Do
                ElseIf mEncoding = BCEncoding.Code128C Then
                    BinaryS &= arCodes128(Val(sIn.Substring(j, 2)))
                    j += 2
                    If j >= sLen Then Exit Do
                End If

            Loop
        End If

        Select Case mEncoding
            Case BCEncoding.Code128A
                k = CalculateCheckSumA(sIn)
            Case BCEncoding.Code128B
                k = CalculateCheckSumB(sIn)
            Case BCEncoding.Code128C
                k = CalculateCheckSumC(sIn)
        End Select

        BinaryS &= arCodes128(k) & arCodes128(BC_STOP) & "11"

        Return BinaryS
    End Function

    'used only for codea and codeb barcodes.
    Private Function CompressString(ByVal sIn As String) As String
        'if sIn has more than 2 numbers a START_C char will be inserted
        'before the numbers

        Dim k As Integer
        Dim iLen As Integer
        Dim sTemp As String
        Dim arParts() As Part = Nothing
        Dim j As Integer = -1
        Dim sPrevType As String = ""
        Dim sOut As String = ""

        If sIn Like "*######*" Then    'if there are at least 6 consecutive numeric chars

            'the following block of code creates an array
            'by breaking the sIn string in chars and numbers:
            'AB4594584FR --> AB
            '            4594584
            '            FR

            iLen = sIn.Length - 1

            For k = 0 To iLen
                sTemp = sIn.Chars(k)
                Select Case sTemp
                    Case "0" To "9"

                        If sPrevType = "S" OrElse sPrevType = "" Then
                            sPrevType = "N"
                            j += 1
                            ReDim Preserve arParts(j)
                            arParts(j).sValue = sTemp
                            arParts(j).sType = "N"
                        Else
                            arParts(j).sValue &= sTemp
                        End If
                    Case Else
                        If sPrevType = "N" OrElse sPrevType = "" Then
                            sPrevType = "S"
                            j += 1
                            ReDim Preserve arParts(j)
                            arParts(j).sValue = sTemp
                            arParts(j).sType = "S"
                        Else
                            arParts(j).sValue &= sTemp
                        End If
                End Select
            Next
        Else 'if there are NOT at least 2 consecutive numeric chars
            Return sIn
        End If

        'the following block of code takes the array and insert 
        'BC_CODEA (BC_CODEB) or BC_CODEC chars where is necessary or convenient
        ' AB
        ' 4594584
        ' FR
        '------------> AB[CODEC]459458[CODEA]4FR
        Dim bInsertStartA As Boolean = False

        For k = 0 To j
            If arParts(k).sType = "N" Then
                If arParts(k).sValue.Length > 1 Then
                    If arParts(k).sValue.Length Mod 2 = 0 Then
                        sOut &= Chr(BC_CODEC) & arParts(k).sValue
                    Else

                        If mEncoding = BCEncoding.Code128A Then
                            sOut &= Chr(BC_CODEC) & _
                                    arParts(k).sValue.Substring(0, arParts(k).sValue.Length - 1) & _
                                    Chr(BC_CODEA) & _
                                    arParts(k).sValue.Substring(arParts(k).sValue.Length - 1, 1)
                        ElseIf mEncoding = BCEncoding.Code128B Then
                            sOut &= Chr(BC_CODEC) & _
                                    arParts(k).sValue.Substring(0, arParts(k).sValue.Length - 1) & _
                                    Chr(BC_CODEB) & _
                                    arParts(k).sValue.Substring(arParts(k).sValue.Length - 1, 1)
                        End If
                        bInsertStartA = True
                    End If
                Else
                    sOut &= arParts(k).sValue
                    bInsertStartA = True
                End If
            ElseIf arParts(k).sType = "S" Then
                If bInsertStartA = True Then
                    sOut &= arParts(k).sValue
                    bInsertStartA = False
                Else
                    If k = 0 Then
                        sOut = arParts(k).sValue
                    Else
                        If mEncoding = BCEncoding.Code128A Then
                            sOut &= Chr(BC_CODEA) & arParts(k).sValue
                        ElseIf mEncoding = BCEncoding.Code128B Then
                            sOut &= Chr(BC_CODEB) & arParts(k).sValue
                        ElseIf mEncoding = BCEncoding.Code128C Then
                            sOut &= Chr(BC_CODEC) & arParts(k).sValue
                        End If
                    End If

                End If
            End If
        Next

        Return sOut

    End Function

#Region "CheckSum Procedures"
    'The Checksum is calculated by summing the start code value plus 
    'the product of each character position (most significant character
    'position equals 1) and the character value of the character at that
    'position. This sum is divided by 103. The remainder of the answer 
    'is the CheckSum. Every encoded character is included except the 
    'Stop and Check Character. 
    '
    'Example: BarCode 1
    'Message : Start B   B   a   r   C   o   d   e      1
    'Value      104      34  65  82  35  79  68  69  0  17
    'Position:   -       1   2   3   4   5   6   7   8  9
    'Calculate Total: 104 + (34x1) + (65x2) + (82x3) + (35x4) + (79x5) +
    '                 (68x6) + (69x7) + (0x8) + (17x9) = 2093
    '2093/103 = 20 remainder 33
    '33 = A
    Private Function CalculateCheckSumB(ByVal sIn As String) As Integer
        Dim Sum As Integer
        Dim k As Integer
        Dim sLen As Integer
        Dim j As Integer

        Sum = mEncoding
        sLen = sIn.Length - 1

        Do
            If Asc(sIn.Chars(k)) = BC_CODEC Then
                'foud a compressed section
                Sum += BC_CODEC * (j + 1)
                j += 1
                k += 1
                Do


                    If Asc(sIn.Chars(k)) = BC_CODEB Then
                        Sum += BC_CODEB * (j + 1)
                        j += 1
                        Exit Do
                    ElseIf Val(sIn.Chars(k + 1)) <> 0 Then
                        Sum += Val(sIn.Substring(k, 2)) * (j + 1)
                        j += 1
                        k += 2
                        If k > sLen Then Exit Do

                    End If
                Loop


            Else
                '
                Sum += (Asc(sIn.Chars(k)) - 32) * (j + 1)
                j += 1
            End If

            k += 1
            If k > sLen Then Exit Do

        Loop
        mCheckSum = Sum Mod 103
        Return mCheckSum
    End Function

    'for barcodes encoded as 128C, the string is assumed to be composed
    'by pairs of numbers.
    'The Checksum is calculated by summing the start code value plus 
    'the product of each couple of digits (most significant couple
    'position equals 1) and the character value of the character at that
    'position. This sum is divided by 103. The remainder of the answer 
    'is the CheckSum. Every encoded character is included except the 
    'Stop and Check Character. 
    '
    'Example: 15027854
    'Message : Start C   15  02  78  54
    'Value      105      15  02  78  54
    'Position:   -       1   2   3   4 
    'Calculate Total: 105 + (15x1) + (02x2) + (78x3) + (54x4) = 574
    '574/103 = 5 remainder 59
    Private Function CalculateCheckSumC(ByVal sIn As String) As Integer
        Dim Sum As Integer
        Dim k As Integer
        Dim sLen As Integer
        Dim j As Integer

        Sum = mEncoding
        sLen = sIn.Length - 1

        Do
            Try
                Sum += CInt(sIn.Substring(k, 2)) * (j + 1)
            Catch ex As Exception

            End Try

            j += 1
            k += 2
            If k > sLen Then Exit Do

        Loop
        mCheckSum = Sum Mod 103
        Return mCheckSum
    End Function

    'in barcodes encoded as 128A, the string may have special symbols like
    'NAK, CR etc.
    'whenever the string contains this characters, the are assumed to be
    'enclosed in squared brackets ([NAK], [CR] etc)
    'The Checksum is calculated by summing the start code value plus 
    'the product of symbol and the character value of the character at that
    'position. This sum is divided by 103. The remainder of the answer 
    'is the CheckSum. Every encoded character is included except the 
    'Stop and Check Character. 
    '
    'Example: D41[CR]B3
    'Message : Start A   D   4   1   [CR]   B   3
    'Value      103      36  20  17  77     34  19
    'Position:   -       1   2   3   4      5   6 
    'Calculate Total: 103 + (36x1) + (20x2) + (17x3) + (77x4) _
    '                 + (34x5) + (19x6) = 822
    '822/103 = 7 remainder 101
    Private Function CalculateCheckSumA(ByVal sIn As String) As Integer
        Dim Sum As Integer
        Dim k As Integer
        Dim sLen As Integer
        Dim j As Integer
        Dim s As String

        Sum = mEncoding
        sLen = sIn.Length - 1

        Do
            If Asc(sIn.Chars(k)) = BC_CODEC Then
                'foud a compressed section
                Sum += BC_CODEC * (j + 1)
                j += 1
                k += 1
                Do

                    If Asc(sIn.Chars(k)) = BC_CODEA Then
                        'end of the compressed section
                        Sum += BC_CODEA * (j + 1)
                        j += 1
                        k += 1

                    ElseIf Val(sIn.Chars(k + 1)) <> 0 Then
                        Sum += Val(sIn.Substring(k, 2)) * (j + 1)
                        j += 1
                        k += 2
                        If k > sLen Then Exit Do
                    End If

                Loop

            ElseIf sIn.Chars(k) = "[" Then
                s = GetSymbol(sIn, k)
                If s <> "" Then
                    Sum += GetSymbolIndex(s) * (j + 1)

                    j += 1
                    k += s.Length
                End If
            Else
                '
                Sum += (Asc(sIn.Chars(k)) - 32) * (j + 1)
                j += 1
                k += 1
            End If

            If k > sLen Then Exit Do
        Loop
        mCheckSum = Sum Mod 103
        Return mCheckSum

    End Function
#End Region

    'called only by CalculateCheckSumA and checkstring
    Private Function GetSymbol(ByVal sIn As String, ByVal iStartPos As Integer) As String
        Dim i As Integer
        i = iStartPos
        Do
            i += 1
            If i > sIn.Length - 1 Then
                If sIn.Chars(i) = "]" Then
                    Return sIn.Substring(iStartPos, i - iStartPos)
                End If
            Else
                'the char ] is not found
                'there is an error in the string
                Return ""
            End If
        Loop
    End Function

    Private Function GetSymbolIndex(ByVal sSymbol As String) As Integer
        Dim k As Integer

        For k = 0 To 31
            If sSymbol = arSymbols(k) Then
                Return k + 64
            End If
        Next
    End Function

    Private Sub LoadCodes()
        arCodes128(0) = "11011001100"
        arCodes128(1) = "11001101100"
        arCodes128(2) = "11001100110"
        arCodes128(3) = "10010011000"
        arCodes128(4) = "10010001100"
        arCodes128(5) = "10001001100"
        arCodes128(6) = "10011001000"
        arCodes128(7) = "10011000100"
        arCodes128(8) = "10001100100"
        arCodes128(9) = "11001001000"
        arCodes128(10) = "11001000100"
        arCodes128(11) = "11000100100"
        arCodes128(12) = "10110011100"
        arCodes128(13) = "10011011100"
        arCodes128(14) = "10011001110"
        arCodes128(15) = "10111001100"
        arCodes128(16) = "10011101100"
        arCodes128(17) = "10011100110"
        arCodes128(18) = "11001110010"
        arCodes128(19) = "11001011100"
        arCodes128(20) = "11001001110"
        arCodes128(21) = "11011100100"
        arCodes128(22) = "11001110100"
        arCodes128(23) = "11101101110"
        arCodes128(24) = "11101001100"
        arCodes128(25) = "11100101100"
        arCodes128(26) = "11100100110"
        arCodes128(27) = "11101100100"
        arCodes128(28) = "11100110100"
        arCodes128(29) = "11100110010"
        arCodes128(30) = "11011011000"
        arCodes128(31) = "11011000110"
        arCodes128(32) = "11000110110"
        arCodes128(33) = "10100011000"
        arCodes128(34) = "10001011000"
        arCodes128(35) = "10001000110"
        arCodes128(36) = "10110001000"
        arCodes128(37) = "10001101000"
        arCodes128(38) = "10001100010"
        arCodes128(39) = "11010001000"
        arCodes128(40) = "11000101000"
        arCodes128(41) = "11000100010"
        arCodes128(42) = "10110111000"
        arCodes128(43) = "10110001110"
        arCodes128(44) = "10001101110"
        arCodes128(45) = "10111011000"
        arCodes128(46) = "10111000110"
        arCodes128(47) = "10001110110"
        arCodes128(48) = "11101110110"
        arCodes128(49) = "11010001110"
        arCodes128(50) = "11000101110"
        arCodes128(51) = "11011101000"
        arCodes128(52) = "11011100010"
        arCodes128(53) = "11011101110"
        arCodes128(54) = "11101011000"
        arCodes128(55) = "11101000110"
        arCodes128(56) = "11100010110"
        arCodes128(57) = "11101101000"
        arCodes128(58) = "11101100010"
        arCodes128(59) = "11100011010"
        arCodes128(60) = "11101111010"
        arCodes128(61) = "11001000010"
        arCodes128(62) = "11110001010"
        arCodes128(63) = "10100110000"
        arCodes128(64) = "10100001100"
        arCodes128(65) = "10010110000"
        arCodes128(66) = "10010000110"
        arCodes128(67) = "10000101100"
        arCodes128(68) = "10000100110"
        arCodes128(69) = "10110010000"
        arCodes128(70) = "10110000100"
        arCodes128(71) = "10011010000"
        arCodes128(72) = "10011000010"
        arCodes128(73) = "10000110100"
        arCodes128(74) = "10000110010"
        arCodes128(75) = "11000010010"
        arCodes128(76) = "11001010000"
        arCodes128(77) = "11110111010"
        arCodes128(78) = "11000010100"
        arCodes128(79) = "10001111010"
        arCodes128(80) = "10100111100"
        arCodes128(81) = "10010111100"
        arCodes128(82) = "10010011110"
        arCodes128(83) = "10111100100"
        arCodes128(84) = "10011110100"
        arCodes128(85) = "10011110010"
        arCodes128(86) = "11110100100"
        arCodes128(87) = "11110010100"
        arCodes128(88) = "11110010010"
        arCodes128(89) = "11011011110"
        arCodes128(90) = "11011110110"
        arCodes128(91) = "11110110110"
        arCodes128(92) = "10101111000"
        arCodes128(93) = "10100011110"
        arCodes128(94) = "10001011110"
        arCodes128(95) = "10111101000"
        arCodes128(96) = "10111100010"
        arCodes128(97) = "11110101000"
        arCodes128(98) = "11110100010"
        arCodes128(99) = "10111011110"
        arCodes128(100) = "10111101110"
        arCodes128(101) = "11101011110"
        arCodes128(102) = "11110101110"
        arCodes128(103) = "11010000100"
        arCodes128(104) = "11010010000"
        arCodes128(105) = "11010011100"
        arCodes128(106) = "11000111010"

        arSymbols(0) = SPECIAL_CODE_START & "NUL" & SPECIAL_CODE_END
        arSymbols(1) = SPECIAL_CODE_START & "SOH" & SPECIAL_CODE_END
        arSymbols(2) = SPECIAL_CODE_START & "STX" & SPECIAL_CODE_END
        arSymbols(3) = SPECIAL_CODE_START & "ETX" & SPECIAL_CODE_END
        arSymbols(4) = SPECIAL_CODE_START & "EOT" & SPECIAL_CODE_END
        arSymbols(5) = SPECIAL_CODE_START & "ENQ" & SPECIAL_CODE_END
        arSymbols(6) = SPECIAL_CODE_START & "ACK" & SPECIAL_CODE_END
        arSymbols(7) = SPECIAL_CODE_START & "BEL" & SPECIAL_CODE_END
        arSymbols(8) = SPECIAL_CODE_START & "BS" & SPECIAL_CODE_END
        arSymbols(9) = SPECIAL_CODE_START & "HT" & SPECIAL_CODE_END
        arSymbols(10) = SPECIAL_CODE_START & "LF" & SPECIAL_CODE_END
        arSymbols(11) = SPECIAL_CODE_START & "VT" & SPECIAL_CODE_END
        arSymbols(12) = SPECIAL_CODE_START & "FF" & SPECIAL_CODE_END
        arSymbols(13) = SPECIAL_CODE_START & "CR" & SPECIAL_CODE_END
        arSymbols(14) = SPECIAL_CODE_START & "SO" & SPECIAL_CODE_END
        arSymbols(15) = SPECIAL_CODE_START & "SI" & SPECIAL_CODE_END
        arSymbols(16) = SPECIAL_CODE_START & "DLE" & SPECIAL_CODE_END
        arSymbols(17) = SPECIAL_CODE_START & "DC1" & SPECIAL_CODE_END
        arSymbols(18) = SPECIAL_CODE_START & "DC2" & SPECIAL_CODE_END
        arSymbols(19) = SPECIAL_CODE_START & "DC3" & SPECIAL_CODE_END
        arSymbols(20) = SPECIAL_CODE_START & "DC4" & SPECIAL_CODE_END
        arSymbols(21) = SPECIAL_CODE_START & "NAK" & SPECIAL_CODE_END
        arSymbols(22) = SPECIAL_CODE_START & "SYN" & SPECIAL_CODE_END
        arSymbols(23) = SPECIAL_CODE_START & "ETB" & SPECIAL_CODE_END
        arSymbols(24) = SPECIAL_CODE_START & "CAN" & SPECIAL_CODE_END
        arSymbols(25) = SPECIAL_CODE_START & "EM" & SPECIAL_CODE_END
        arSymbols(26) = SPECIAL_CODE_START & "SUB" & SPECIAL_CODE_END
        arSymbols(27) = SPECIAL_CODE_START & "ESC" & SPECIAL_CODE_END
        arSymbols(28) = SPECIAL_CODE_START & "FS" & SPECIAL_CODE_END
        arSymbols(29) = SPECIAL_CODE_START & "GS" & SPECIAL_CODE_END
        arSymbols(30) = SPECIAL_CODE_START & "RS" & SPECIAL_CODE_END
        arSymbols(31) = SPECIAL_CODE_START & "US" & SPECIAL_CODE_END
    End Sub

#Region "Properties"

    Public Property ShowString() As Boolean
        Get
            Return mShowString
        End Get
        Set(ByVal Value As Boolean)
            mShowString = Value
        End Set
    End Property

    Public ReadOnly Property UseCompression() As Boolean
        Get
            Return mUseCompression
        End Get
    End Property

    Public Property BarWidth() As Integer
        Get
            Return mBarWidth
        End Get
        Set(ByVal Value As Integer)
            If Value > 0 Then
                mBarWidth = Value
            Else
                Throw New Exception("The Bar width must be greater than zero")
            End If

        End Set
    End Property

    Public ReadOnly Property CheckSum() As Integer
        Get
            Return mCheckSum
        End Get
    End Property

    Public ReadOnly Property BinaryString() As String
        Get
            Return mBinaryString
        End Get
    End Property

    Public ReadOnly Property EncodedString() As String
        Get
            Return mEncodedString
        End Get
    End Property

    Public ReadOnly Property EncodingType() As BCEncoding
        Get
            Return mEncoding
        End Get
    End Property

#End Region

End Class


Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography

Public Class EncryptDecryptData

    '*************************
    '** Global Variables
    '*************************

    Private blnSuccess As Boolean
    Private strErrorMessage As String
    Private strOutputFile As String

    Private salt As String ' = "Ium(81*Qtr" 'SharedFunction.GetFileEncryptionKey.Split("|")(1) '"Ium(81*Qtr" 'default key

    Public ReadOnly Property IsSuccess() As Boolean
        Get
            Return blnSuccess
        End Get
    End Property

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return strErrorMessage
        End Get
    End Property

    Public ReadOnly Property OutputFile() As String
        Get
            Return strOutputFile
        End Get
    End Property

    Dim strFileToEncrypt As String
    Dim strFileToDecrypt As String
    Dim strOutputEncrypt As String
    Dim strOutputDecrypt As String
    Dim fsInput As System.IO.FileStream
    Dim fsOutput As System.IO.FileStream

    'Private Const FileExtension As String = ".phic"
    Public ReadOnly FileExtension As String = ".sss"
    Private ExtensionDataLength As Integer = FileExtension.Length

    '*************************
    '** Create A Key
    '*************************

    Private Function CreateKey_SHA(ByVal strPassword As String) As Byte()
        'Convert strPassword to an array and store in chrData.
        Dim chrData() As Char = strPassword.ToCharArray
        'Use intLength to get strPassword size.
        Dim intLength As Integer = chrData.GetUpperBound(0)
        'Declare bytDataToHash and make it the same size as chrData.
        Dim bytDataToHash(intLength) As Byte

        'Use For Next to convert and store chrData into bytDataToHash.
        For i As Integer = 0 To chrData.GetUpperBound(0)
            bytDataToHash(i) = CByte(Asc(chrData(i)))
        Next

        'Declare what hash to use.
        Dim SHA512 As New System.Security.Cryptography.SHA512Managed
        'Declare bytResult, Hash bytDataToHash and store it in bytResult.
        Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
        'Declare bytKey(31).  It will hold 256 bits.
        Dim bytKey(31) As Byte

        'Use For Next to put a specific size (256 bits) of 
        'bytResult into bytKey. The 0 To 31 will put the first 256 bits
        'of 512 bits into bytKey.
        For i As Integer = 0 To 31
            bytKey(i) = bytResult(i)
        Next

        Return bytKey 'Return the key.
    End Function

    Private Function CreateKey(ByVal strPassword As String) As Byte()
        Dim bytKey As Byte()
        Dim bytSalt As Byte() = System.Text.Encoding.ASCII.GetBytes(salt)
        Dim pdb As New PasswordDeriveBytes(strPassword, bytSalt)

        bytKey = pdb.GetBytes(32)

        Return bytKey 'Return the key.
    End Function

    '*************************
    '** Create An IV
    '*************************

    Private Function CreateIV_SHA(ByVal strPassword As String) As Byte()
        'Convert strPassword to an array and store in chrData.
        Dim chrData() As Char = strPassword.ToCharArray
        'Use intLength to get strPassword size.
        Dim intLength As Integer = chrData.GetUpperBound(0)
        'Declare bytDataToHash and make it the same size as chrData.
        Dim bytDataToHash(intLength) As Byte

        'Use For Next to convert and store chrData into bytDataToHash.
        For i As Integer = 0 To chrData.GetUpperBound(0)
            bytDataToHash(i) = CByte(Asc(chrData(i)))
        Next

        'Declare what hash to use.
        Dim SHA512 As New System.Security.Cryptography.SHA512Managed
        'Declare bytResult, Hash bytDataToHash and store it in bytResult.
        Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
        'Declare bytIV(15).  It will hold 128 bits.
        Dim bytIV(15) As Byte

        'Use For Next to put a specific size (128 bits) of bytResult into bytIV.
        'The 0 To 30 for bytKey used the first 256 bits of the hashed password.
        'The 32 To 47 will put the next 128 bits into bytIV.
        For i As Integer = 32 To 47
            bytIV(i - 32) = bytResult(i)
        Next

        Return bytIV 'Return the IV.
    End Function

    Private Function CreateIV(ByVal strPassword As String) As Byte()
        Dim bytIV As Byte()
        Dim bytSalt As Byte() = System.Text.Encoding.ASCII.GetBytes(salt)
        Dim pdb As New PasswordDeriveBytes(strPassword, bytSalt)

        bytIV = pdb.GetBytes(16)

        Return bytIV 'Return the IV.
    End Function

    '****************************
    '** Encrypt/Decrypt File
    '****************************

    Private Enum CryptoAction
        'Define the enumeration for CryptoAction.
        ActionEncrypt = 1
        ActionDecrypt = 2
    End Enum

    Private Sub EncryptOrDecryptFile(ByVal strInputFile As String, _
                                     ByVal strOutputFile As String, _
                                     ByVal bytKey() As Byte, _
                                     ByVal bytIV() As Byte, _
                                     ByVal Direction As CryptoAction)
        Try 'In case of errors.

            'Setup file streams to handle input and output.
            fsInput = New System.IO.FileStream(strInputFile, FileMode.Open, _
                                                  FileAccess.Read)
            fsOutput = New System.IO.FileStream(strOutputFile, _
                                                   FileMode.OpenOrCreate, _
                                                   FileAccess.Write)
            fsOutput.SetLength(0) 'make sure fsOutput is empty

            'Declare variables for encrypt/decrypt process.
            Dim bytBuffer(4096) As Byte 'holds a block of bytes for processing
            Dim lngBytesProcessed As Long = 0 'running count of bytes processed
            Dim lngFileLength As Long = fsInput.Length 'the input file's length
            Dim intBytesInCurrentBlock As Integer 'current bytes being processed
            Dim csCryptoStream As CryptoStream
            'Declare your CryptoServiceProvider.
            Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged
            'Setup Progress Bar

            'Determine if ecryption or decryption and setup CryptoStream.
            Select Case Direction
                Case CryptoAction.ActionEncrypt
                    csCryptoStream = New CryptoStream(fsOutput, _
                    cspRijndael.CreateEncryptor(bytKey, bytIV), _
                    CryptoStreamMode.Write)

                Case CryptoAction.ActionDecrypt
                    csCryptoStream = New CryptoStream(fsOutput, _
                    cspRijndael.CreateDecryptor(bytKey, bytIV), _
                    CryptoStreamMode.Write)
            End Select

            'Use While to loop until all of the file is processed.
            While lngBytesProcessed < lngFileLength
                'Read file with the input filestream.
                intBytesInCurrentBlock = fsInput.Read(bytBuffer, 0, 4096)
                'Write output file with the cryptostream.
                csCryptoStream.Write(bytBuffer, 0, intBytesInCurrentBlock)
                'Update lngBytesProcessed
                lngBytesProcessed = lngBytesProcessed + _
                                        CLng(intBytesInCurrentBlock)
            End While

            'Close FileStreams and CryptoStream.
            csCryptoStream.Close()
            fsInput.Close()
            fsOutput.Close()

            'fsInput = Nothing
            'fsOutput = Nothing

            blnSuccess = True
        Catch ex As Exception
            strErrorMessage = ex.Message
            blnSuccess = False
        End Try
    End Sub

    Public Sub EncryptFile(ByVal strFile As String, ByVal strPassword As String)
        Try
            salt = strPassword

            Dim strFileExtension As String = Path.GetExtension(strFile)

            strFileToEncrypt = strFile

            Dim iPosition As Integer = 0
            Dim i As Integer = 0

            'Get the position of the last "\" in the OpenFileDialog.FileName path.
            '-1 is when the character your searching for is not there.
            'IndexOf searches from left to right.
            While strFileToEncrypt.IndexOf("\"c, i) <> -1
                iPosition = strFileToEncrypt.IndexOf("\"c, i)
                i = iPosition + 1
            End While

            'Assign strOutputFile to the position after the last "\" in the path.
            'This position is the beginning of the file name.
            strOutputEncrypt = strFileToEncrypt.Substring(iPosition + 1)
            'Assign S the entire path, ending at the last "\".
            Dim S As String = strFileToEncrypt.Substring(0, iPosition + 1)
            'Replace the "." in the file extension with "_".
            'strOutputEncrypt = strOutputEncrypt.Replace("."c, "_"c)
            strOutputEncrypt = strOutputEncrypt.Replace(strFileExtension, strFileExtension.Replace("."c, "_"c))

            'The final file name.  XXXXX.encrypt
            'txtDestinationEncrypt.Text = S + strOutputEncrypt + FileExtension
            strOutputEncrypt = S + strOutputEncrypt + FileExtension

            'Declare variables for the key and iv.
            'The key needs to hold 256 bits and the iv 128 bits.
            Dim bytKey As Byte()
            Dim bytIV As Byte()
            'Send the password to the CreateKey function.
            bytKey = CreateKey(strPassword)
            'Send the password to the CreateIV function.
            bytIV = CreateIV(strPassword)
            'Start the encryption.
            EncryptOrDecryptFile(strFileToEncrypt, strOutputEncrypt, _
                                 bytKey, bytIV, CryptoAction.ActionEncrypt)

            strOutputFile = strOutputEncrypt

            'If New FileInfo(strOutputDecrypt).Length > 0 Then
            '    blnSuccess = True
            'Else
            '    blnSuccess = False
            'End If
            blnSuccess = True
        Catch ex As Exception
            strErrorMessage = ex.Message
            blnSuccess = False
        End Try
    End Sub

    Public Sub DecryptFile(ByVal strFile As String, ByVal strPassword As String)
        Try
            salt = strPassword

            strFileToDecrypt = strFile

            Dim iPosition As Integer = 0
            Dim i As Integer = 0
            'Get the position of the last "\" in the OpenFileDialog.FileName path.
            '-1 is when the character your searching for is not there.
            'IndexOf searches from left to right.

            While strFileToDecrypt.IndexOf("\"c, i) <> -1
                iPosition = strFileToDecrypt.IndexOf("\"c, i)
                i = iPosition + 1
            End While

            'strOutputFile = the file path minus the last 8 characters (.encrypt)
            strOutputDecrypt = strFileToDecrypt.Substring(0, _
                                                    strFileToDecrypt.Length - ExtensionDataLength)
            'Assign S the entire path, ending at the last "\".
            Dim S As String = strFileToDecrypt.Substring(0, iPosition + 1)
            'Assign strOutputFile to the position after the last "\" in the path.
            strOutputDecrypt = strOutputDecrypt.Substring((iPosition + 1))
            'Replace "_" with "."
            'txtDestinationDecrypt.Text = S + strOutputDecrypt.Replace("_"c, "."c)
            'strOutputDecrypt = S + strOutputDecrypt.Replace("_"c, "."c)
            Dim intLastIndex As Integer = strOutputDecrypt.LastIndexOf("_")
            strOutputDecrypt = S + strOutputDecrypt.Remove(intLastIndex, 1).Insert(intLastIndex, ".")

            'Declare variables for the key and iv.
            'The key needs to hold 256 bits and the iv 128 bits.
            Dim bytKey As Byte()
            Dim bytIV As Byte()
            'Send the password to the CreateKey function.
            bytKey = CreateKey(strPassword)
            'Send the password to the CreateIV function.
            bytIV = CreateIV(strPassword)
            'Start the decryption.
            EncryptOrDecryptFile(strFileToDecrypt, strOutputDecrypt, _
                                 bytKey, bytIV, CryptoAction.ActionDecrypt)

            strOutputFile = strOutputDecrypt

            'If New FileInfo(strOutputDecrypt).Length > 0 Then
            '    blnSuccess = True
            'Else
            '    blnSuccess = False
            'End If
            blnSuccess = True
        Catch ex As Exception
            strErrorMessage = ex.Message
            blnSuccess = False
        End Try
    End Sub

    'Public Sub DecryptFile(ByVal strFile As String, ByVal strPassword As String)
    '    Try
    '        strFileToDecrypt = strFile

    '        Dim iPosition As Integer = 0
    '        Dim i As Integer = 0
    '        'Get the position of the last "\" in the OpenFileDialog.FileName path.
    '        '-1 is when the character your searching for is not there.
    '        'IndexOf searches from left to right.

    '        While strFileToDecrypt.IndexOf("\"c, i) <> -1
    '            iPosition = strFileToDecrypt.IndexOf("\"c, i)
    '            i = iPosition + 1
    '        End While

    '        'strOutputFile = the file path minus the last 8 characters (.encrypt)
    '        strOutputDecrypt = strFileToDecrypt.Substring(0, _
    '                                                strFileToDecrypt.Length - ExtensionDataLength)
    '        'Assign S the entire path, ending at the last "\".
    '        Dim S As String = strFileToDecrypt.Substring(0, iPosition + 1)
    '        'Assign strOutputFile to the position after the last "\" in the path.
    '        strOutputDecrypt = strOutputDecrypt.Substring((iPosition + 1))
    '        'Replace "_" with "."
    '        'txtDestinationDecrypt.Text = S + strOutputDecrypt.Replace("_"c, "."c)
    '        'strOutputDecrypt = S + strOutputDecrypt.Replace("_"c, "."c)
    '        Dim intLastIndex As Integer = strOutputDecrypt.LastIndexOf("_")
    '        strOutputDecrypt = S + strOutputDecrypt.Remove(intLastIndex, 1).Insert(intLastIndex, ".")

    '        'Declare variables for the key and iv.
    '        'The key needs to hold 256 bits and the iv 128 bits.
    '        Dim bytKey As Byte()
    '        Dim bytIV As Byte()
    '        'Send the password to the CreateKey function.
    '        bytKey = CreateKey(strPassword)
    '        'Send the password to the CreateIV function.
    '        bytIV = CreateIV(strPassword)
    '        'Start the decryption.
    '        EncryptOrDecryptFile(strFileToDecrypt, strOutputDecrypt, _
    '                             bytKey, bytIV, CryptoAction.ActionDecrypt)

    '        strOutputFile = strOutputDecrypt

    '        'If New FileInfo(strOutputDecrypt).Length > 0 Then
    '        '    blnSuccess = True
    '        'Else
    '        '    blnSuccess = False
    '        'End If
    '        blnSuccess = True
    '    Catch ex As Exception
    '        strErrorMessage = ex.Message
    '        blnSuccess = False
    '    End Try
    'End Sub



End Class


Imports System.IO

Public Structure SharedFunction

    Public Shared ReadOnly ErrorColor As System.Drawing.Color = Drawing.Color.OrangeRed
    Public Shared ReadOnly SuccessColor As System.Drawing.Color = Drawing.Color.ForestGreen

    Public Shared ReadOnly MainRepository As String = My.Settings.MainRepository
    'Public Shared ReadOnly MainRepository As String = "D:\SSS_CPS"

    Public Shared ReadOnly ForUploadingRepository As String = String.Format("{0}\{1}", MainRepository, "ForUploading")
    Public Shared ReadOnly CMS_ReprintRepository As String = String.Format("{0}\{1}", MainRepository, "ForUploading\CMS_Reprint")
    Public Shared ReadOnly ForUploadingCDFRRepository As String = String.Format("{0}\{1}", ForUploadingRepository, "CDFR")
    Public Shared ReadOnly ForUploadingCMSRepository As String = String.Format("{0}\{1}", ForUploadingRepository, "CMS")
    'Public Shared ReadOnly ForUploadingRepository As String = "\\192.168.200.10\ForUploading"

    Public Shared ReadOnly IndigoExtractRepository As String = String.Format("{0}\{1}", MainRepository, "Indigo\IndigoExtract")
    Public Shared ReadOnly ReportsRepository As String = String.Format("{0}\{1}", MainRepository, "Reports")

    Public Shared ReadOnly ReportsDeliveryReceiptRepository As String = String.Format("{0}\{1}", MainRepository, "Reports\DeliveryReceipt")
    Public Shared ReadOnly ReportsUBPRFRepository As String = String.Format("{0}\{1}", MainRepository, "Reports\UBP RF")


    'Public Shared ReadOnly LogRepository As String = String.Format("{0}\{1}", MainRepository, "Logs")
    Public Shared ReadOnly ProcessLogRepository As String = String.Format("{0}\{1}", MainRepository, "ProcessLog")
    Public Shared ReadOnly BarcodeRepository As String = String.Format("{0}\{1}", MainRepository, "Barcodes")

    Public Shared ReadOnly PersoRepository As String = String.Format("{0}\{1}", MainRepository, "Perso")
    Public Shared ReadOnly IndigoRepository As String = String.Format("{0}\{1}", MainRepository, "Indigo")
    'Public Shared ReadOnly AssemblyRepository As String = String.Format("{0}\{1}", MainRepository, "Assembly")
    Public Shared ReadOnly QCRepository As String = String.Format("{0}\{1}", MainRepository, "QC")
    Public Shared ReadOnly SystemRepository As String = String.Format("{0}\{1}", MainRepository, "System")

    Public Shared ReadOnly PSBImagesRepository As String = String.Format("{0}\{1}", MainRepository, "PSBImages")
    'Public Shared ReadOnly PSBImagesRepository As String = "Z:\"

    Public Shared ReadOnly UMIDCoding_PO_Job As String = String.Format("{0}\{1}", SystemRepository, "UMIDCoding_PO_Job.xml")
    Public Shared ReadOnly UMIDCoding_PO_Job_Seg As String = String.Format("{0}\{1}", SystemRepository, "UMIDCoding_PO_Job_Seg.xml")
    Public Shared ReadOnly UMIDCoding_PO_Job2 As String = String.Format("{0}\{1}", SystemRepository, "UMIDCoding_PO_Job2.xml")

    Public Shared ReadOnly NoImageFound As String = String.Format("{0}\{1}", SystemRepository, "no_image_found.jpg")

    Public Shared ReadOnly templateXML_File As String = String.Format("{0}\{1}", SystemRepository, "templateXML.xml")
    Public Shared ReadOnly templateANSI_File As String = String.Format("{0}\{1}", SystemRepository, "templateANSI.ansi-fmr")
    Public Shared ReadOnly templateSignature_File As String = String.Format("{0}\{1}", SystemRepository, "templateSignature.tiff")
    Public Shared ReadOnly BackCardImage_File As String = String.Format("{0}\{1}", SystemRepository, "BackCardImage")

    Public Shared ReadOnly DR_PDF_DOCCNTR As String = String.Format("{0}\{1}", SystemRepository, "DR_PDF_DOCCNTR")
    Public Shared ReadOnly ForUploadingFileType As String = String.Format("{0}\{1}", SystemRepository, "ForUploadingFileType")

    Public Shared ReadOnly CardsPerBox As Integer = 210
    Public Shared ReadOnly CardsPerSheet As Integer = 21

    Public Shared ReadOnly key As String = "u78*)qWP1"

    Public Shared Hasher As New System.Security.Cryptography.MD5CryptoServiceProvider()

    Public Shared Sub ReComputePageAndSeries(ByRef dt As DataTable, Optional ByVal intPrintable As Integer = 0)
        Try
            Dim intPage As Integer = 1
            Dim intSeries As Integer = 1
            Dim intPageCntr As Integer = 1

            For Each rw As DataRow In dt.Rows
                rw("CurrentPage") = intPage
                rw("CurrentSeries") = intSeries

                If intPrintable = 0 And dt.DefaultView.Count < SharedFunction.CardsPerSheet Then
                    rw("Type") = rw("Type").ToString.Trim & " (drop)"
                ElseIf intPrintable > 0 And dt.Columns.Contains("Type") Then
                    If rw("CurrentSeries") > intPrintable Then
                        rw("Type") = rw("Type").ToString.Trim & " (drop)"
                    End If
                End If

                rw.AcceptChanges()

                intSeries += 1

                If intPageCntr <> SharedFunction.CardsPerSheet Then
                    intPageCntr += 1
                Else
                    intPage += 1
                    intPageCntr = 1
                End If
            Next
        Catch ex As Exception
            'IO.File.WriteAllText(String.Format("{0}\status2.txt", SharedFunction.MainRepository), ex.Message)
        End Try
    End Sub

    Public Shared Sub GenerateSheetBarcode(ByRef dt As DataTable)
        dt.Columns.Add("CRN1", Type.GetType("System.String"))
        dt.Columns.Add("CRN2", Type.GetType("System.String"))
        dt.Columns.Add("FrontSheetBarcode", Type.GetType("System.String"))
        dt.Columns.Add("BackSheetBarcode", Type.GetType("System.String"))

        Dim countPerSheet As Short = 21
        Dim intCntr As Integer = 1

        Dim intEnd As Integer = countPerSheet * intCntr
        Dim intStart As Integer = intEnd - (countPerSheet - 1)
        Dim rn As New Random()
        Dim intRandomNumber As Integer = rn.Next(1, 999800)
        Dim strTimestamp As String = Now.ToString("hhmmss")
        Dim strFrontBarcode As String = String.Format("{0}{1}", strTimestamp, intRandomNumber.ToString.PadRight(6, "0"))
        Dim strBackBarcode As String = String.Format("{0}{1}", strTimestamp, (intRandomNumber + 171).ToString.PadRight(6, "0"))

        For i As Integer = 1 To (dt.DefaultView.Count / countPerSheet)
            'Dim dt21 As DataTable = dt.Select(String.Format("CurrentSeries >= {0} AND CurrentSeries <= {1}", intStart, intEnd)).CopyToDataTable
            Dim IsFirst As Boolean = True
            For Each rw As DataRow In dt.Select(String.Format("CurrentSeries >= {0} AND CurrentSeries <= {1}", intStart, intEnd))
                If IsFirst Then
                    rw("CRN1") = dt.Select(String.Format("CurrentSeries >= {0} AND CurrentSeries <= {1}", intStart, intEnd))(0)("CRN").ToString.Trim
                    rw("CRN2") = dt.Select(String.Format("CurrentSeries >= {0} AND CurrentSeries <= {1}", intStart, intEnd))(20)("CRN").ToString.Trim
                    IsFirst = False
                End If
                rw("FrontSheetBarcode") = strFrontBarcode
                rw("BackSheetBarcode") = strBackBarcode
            Next

            'recompute
            intRandomNumber = rn.Next(1, 999800)
            strTimestamp = Now.ToString("hhmmss")
            strFrontBarcode = String.Format("{0}{1}", strTimestamp, intRandomNumber.ToString.PadRight(6, "0"))
            strBackBarcode = String.Format("{0}{1}", strTimestamp, (intRandomNumber + 171).ToString.PadRight(6, "0"))

            intCntr += 1
            intEnd = countPerSheet * intCntr
            intStart = intEnd - (countPerSheet - 1)
        Next
    End Sub

    Public Shared Function GetNextActivity(ByVal ActivityID As DataKeysEnum.ActivityID) As DataKeysEnum.ActivityID
        Select Case ActivityID
            Case DataKeysEnum.ActivityID.Done
                Return DataKeysEnum.ActivityID.Done
            Case Else
                Return ActivityID + 1
        End Select
    End Function

    Public Shared Function GetActivityDesc(ByVal ActivityID As DataKeysEnum.ActivityID) As String
        Return ActivityID.ToString
    End Function

    Public Shared Function GetTotalBoxDivisibleBy210(ByVal inQty As Integer) As Integer
        Dim strParam As String = (inQty / 210).ToString
        Return IIf(strParam.Contains("."), strParam.Split(".")(0), strParam)
    End Function

    Public Shared Sub CPS_Folder_PurchaseOrder_Housekeeping(ByVal strPurchaseOrder As String)
        DeletePO_ForUploadingRepository(strPurchaseOrder)
        DeletePO_IndigoExtractRepository(strPurchaseOrder)
        DeletePO_PersoRepository(strPurchaseOrder)
        DeletePO_ReportsRepository(strPurchaseOrder)
    End Sub

    Public Shared Function DeletePO_PersoRepository(ByVal strPO As String) As Boolean
        Try
            Dim strPath As String = String.Format("{0}\{1}", SharedFunction.PersoRepository, strPO)
            IO.Directory.Delete(strPath, True)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function DeletePO_ForUploadingRepository(ByVal strPO As String) As Boolean
        Try
            Dim strPath As String = String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strPO)
            If Directory.Exists(strPath) Then IO.Directory.Delete(strPath, True)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function DeletePO_IndigoExtractRepository(ByVal strPO As String) As Boolean
        Try
            Dim strPath As String = String.Format("{0}\{1}", SharedFunction.IndigoExtractRepository, strPO)
            IO.Directory.Delete(strPath, True)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function DeletePO_ReportsRepository(ByVal strPO As String) As Boolean
        Try
            Dim strPath As String = String.Format("{0}\{1}", SharedFunction.ReportsRepository, strPO)
            IO.Directory.Delete(strPath, True)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#Region " Key "

    Public Shared Function EncryptData(ByVal strData As String) As String
        Dim encdec As New AllcardEncryptDecrypt.EncryptDecrypt(key)
        Return encdec.TripleDesEncryptText(strData)
    End Function

    Public Shared Function DecryptData(ByVal strData As String) As String
        Dim encdec As New AllcardEncryptDecrypt.EncryptDecrypt(key)
        Return encdec.TripleDesDecryptText(strData)
    End Function

#End Region

    'Public Shared Function GetServer() As String
    '    Return DecryptData(My.Settings.Server)
    'End Function

    'Public Shared Function GetDatabase() As String
    '    Return DecryptData(My.Settings.Database)
    'End Function

    'Public Shared Function GetDBUser() As String
    '    Return DecryptData(My.Settings.User)
    'End Function

    'Public Shared Function GetDBPassword() As String
    '    Return DecryptData(My.Settings.Password)
    'End Function

    Public Shared Function UserID(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(0)
    End Function

    Public Shared Function RoleID(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(1)
    End Function

    Public Shared Function RoleDesc(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(2)
    End Function

    Public Shared Function UserCompleteName(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(3)
    End Function

    Public Shared Function LogID(ByVal credential As String) As Integer
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(4)
    End Function

    Public Shared Function LoginTime(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(5).ToString.Replace("_", ":")
    End Function

    Public Shared Function RoleModules(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(6)
    End Function

    Public Shared Function UserActivity(ByVal credential As String) As String
        Dim arr() As String = DecryptData(credential).Split(":")

        Return arr(7)
    End Function

    Public Shared Function IsHaveAccessToPage(ByVal LoggedUserRoleModule As String, ByVal ModuleID As DataKeysEnum.ModuleID) As Boolean
        If LoggedUserRoleModule = "" Then Return False

        If LoggedUserRoleModule.Contains(",") Then
            For Each intModuleID As Integer In LoggedUserRoleModule.Split(",")
                If intModuleID = ModuleID Then Return True
            Next
        Else
            If CInt(LoggedUserRoleModule) = ModuleID Then Return True
        End If

        'Return False
    End Function

    Public Shared Function IsHaveAccessToActivity(ByVal LoggedUserRoleActivity As String, ByVal ActivityID As DataKeysEnum.ActivityID) As Boolean
        If LoggedUserRoleActivity = "" Then Return False

        If LoggedUserRoleActivity.Contains(",") Then
            For Each intActivityID As Integer In LoggedUserRoleActivity.Split(",")
                If intActivityID = ActivityID Then Return True
            Next
        Else
            If CType(LoggedUserRoleActivity, DataKeysEnum.ActivityID) = ActivityID Then Return True
        End If

        'Return False
    End Function

    Public Shared Function IsHaveAccessToRole(ByVal UserRoles As String, ByVal ParamArray RoleID() As DataKeysEnum.RoleID) As Boolean
        If UserRoles = "" Then Return False

        If UserRoles.Contains(",") Then
            For Each intRoleID As Integer In UserRoles.Split(",")
                For Each intRoleID2 As Integer In RoleID
                    If intRoleID = intRoleID2 Then Return True
                Next
            Next
        Else
            'If CType(UserRoles, DataKeysEnum.RoleID) = RoleID(0) Then Return True
            For Each intRoleID As Integer In RoleID
                If intRoleID = CType(UserRoles, DataKeysEnum.RoleID) Then Return True
            Next
        End If

        'Return False
    End Function

    Public Shared Function GetCurrentPage(ByVal AbsolutePath As String) As String
        Return IO.Path.GetFileName(AbsolutePath).Replace(".aspx", "")
    End Function

    Public Shared Function SaveToTextfile(ByVal strFile As String, ByVal strData As String, ByRef sb As StringBuilder) As Boolean
        Try
            Using sr = New IO.StreamWriter(strFile, True)
                sr.WriteLine(strData)
            End Using

            Return True
        Catch ex As Exception
            sb.AppendLine(String.Format("SaveToTextfile(): Runtime error encountered {0}", ex.Message))

            Return False
        End Try
    End Function

    Public Shared Function GetFileEncryptionKey() As String
        Dim DAL As New DAL
        Try
            Dim key As String = ""
            If DAL.ExecuteScalar("SELECT ISNULL(FileEncryptionKey,'') As FileEncryptionKey FROM tblSystemParameter") Then
                key = DAL.ObjectResult.ToString

                Dim wrapper As New Simple3Des()
                Dim cipherText As String = wrapper.DecryptData(key.Trim)
                wrapper = Nothing

                If key <> "" Then
                    Return String.Format("0|{0}", cipherText)
                Else
                    Return String.Format("1|{0}", "Key is empty")
                End If
            Else
                Return String.Format("1|DAL error {0}", DAL.ErrorMessage)
            End If
        Catch ex As Exception
            Return String.Format("1|Runtime error {0}", IIf(ex.Message.Trim = "Bad Data.", "Check file key if empty or invalid length", ex.Message.Trim))
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Function EncryptDecryptFile(ByVal strFile As String, ByVal IsEncryptFile As Boolean, Optional ByRef outputFile As String = "") As String
        Dim salt As String = SharedFunction.GetFileEncryptionKey '.Split("|||")(1)

        If salt.Split("|")(0) = "0" Then
            Dim encryptdecryptdata As New EncryptDecryptData()
            Dim strProcess As String
            Dim origFile As String = strFile
            If IsEncryptFile Then
                'Ium(81*Qtr
                encryptdecryptdata.EncryptFile(strFile, salt.Split("|")(1)) 'encryptdecryptdata.salt)
                outputFile = encryptdecryptdata.OutputFile
                strProcess = "Encryption "
            Else
                'encryptdecryptdata.DecryptFile(strFile, salt.Split("|")(1)) 'encryptdecryptdata.salt)
                encryptdecryptdata.DecryptFile(strFile, salt.Split("|")(1))
                outputFile = encryptdecryptdata.OutputFile
                strProcess = "Decryption "
            End If

            Dim errmsg As String = encryptdecryptdata.ErrorMessage

            If encryptdecryptdata.IsSuccess Then
                encryptdecryptdata = Nothing
                'IO.File.Delete(strFile)
                Return ""
            Else
                encryptdecryptdata = Nothing
                Return strProcess & " process failed. Returned error " & errmsg
                'SharedFunction.SaveToErrorLog(SharedFunction.TimeStamp & "EncryptDecryptFile(): Failed " & strProcess & "process " & Path.GetFileName(strFile))
                'Return False
            End If
        Else
            Return salt.Split("|")(1)
        End If
    End Function

    Public Shared Function TIFFtoJPG(ByVal img() As Byte, ByVal strFilename As String) As Boolean
        Try
            Using image As New System.Drawing.Bitmap(New IO.MemoryStream(img))
                image.Save(strFilename, System.Drawing.Imaging.ImageFormat.Jpeg)
            End Using

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function GetReportNameByReportTypeID(ByVal Report As DataKeysEnum.Report) As String
        Select Case Report
            Case DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport
                Return "ReportA_SummaryOfCreatedPrintOrderReport_" & Now.ToString("hhmmss")
            Case DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest
                Return "ReportC_SummaryOfInitialPrintAndReprintRequest_" & Now.ToString("hhmmss")
            Case DataKeysEnum.Report.ElectronicReportOfDeliveredCardsPerPrintOrder
                Return "ReportE_ElectronicReportOfDeliveredCardsPerPrintOrder"
            Case DataKeysEnum.Report.ElectronicReportOfGoodCards
                Return "ReportG_ElectronicReportOfGoodCards"
            Case DataKeysEnum.Report.InspectionQualityControlAndYieldReport
                Return "InspectionQualityControlAndYieldReport"
            Case DataKeysEnum.Report.CertificateOfDeletion
                Return "CertificateOfDeletion"
            Case DataKeysEnum.Report.MaterialInventory
                Return "MaterialInventory" & "_" & Now.ToString("MMddyy_hhmmtt")
            Case DataKeysEnum.Report.RejectReportDaily
                Return "RejectReportDaily" & "_" & Now.ToString("MMddyy_hhmmtt")
            Case DataKeysEnum.Report.RejectReportByPO
                Return "RejectReportPO" & "_" & Now.ToString("MMddyy_hhmmtt")
            Case DataKeysEnum.Report.DeliveryReceipt
                Return "DR_" & Now.ToString("MMddyy_hhmmtt")
            Case DataKeysEnum.Report.DeliveryReceipt2
                Return "DR2_" & Now.ToString("MMddyy_hhmmtt")
            Case DataKeysEnum.Report.DeliveryReceipt_PDF
                Return "DR-PDF_" & Now.ToString("MMddyy_hhmmtt")
            Case Else
                Return "CPSReport"
        End Select
    End Function

    Public Shared Function GetMiscReportNameByReportTypeID(ByVal Report As DataKeysEnum.Report) As String
        Select Case Report
            Case DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport
                Return "Inspection_QC_YieldReport"
            Case Else
                Return "CPSReport"
        End Select
    End Function

    Public Shared Function CRNFormat(ByVal CRN As String) As String
        Return String.Format("{0}-{1}-{2}", CRN.Substring(0, 4), CRN.Substring(4, 7), CRN.Substring(11, 1))
    End Function

    Private Shared Function GetFileByte(ByVal strFile As String, ByVal intType As Short) As Byte()
        If File.Exists(strFile) Then
            Return File.ReadAllBytes(strFile)
        Else
            Select Case intType
                Case 1 'xml
                    Return File.ReadAllBytes(SharedFunction.templateXML_File)
                Case 2 'signature
                    Return File.ReadAllBytes(SharedFunction.templateSignature_File)
                Case 3 'ansi
                    Return File.ReadAllBytes(SharedFunction.templateANSI_File)
            End Select
        End If
    End Function

    Private Shared Sub CheckIfFileExist(ByVal strFile As String, ByVal strDesc As String, ByVal strBarcode As String, ByRef intError As Integer, ByRef sb As StringBuilder, ByRef intZeroByteCntr As Integer)
        If Not File.Exists(strFile) Then
            intError += 1
            sb.AppendLine(String.Format("MemberXML(): No " & strDesc.ToLower & " found for Barcode {0}.<br>", strBarcode))
        Else
            If New FileInfo(strFile).Length = 0 Then
                intZeroByteCntr += 1
                intError += 1
                sb.AppendLine(String.Format("MemberXML(): " & strDesc & " is zero byte for Barcode {0}.<br>", strBarcode))
            End If
        End If
    End Sub

    Public Shared Function UploadData_v2(ByVal strPurchaseOrder As String, ByVal strBatch As String, ByVal strRawFile As String,
                           ByVal AbsolutePath As String, ByVal UserID As Integer,
                           ByRef sb As StringBuilder) As Boolean
        Dim DAL As New DAL

        Try
            Dim intError As Integer = 0

            sb.AppendLine(String.Format("Start of UploadData() process {0}<br>", Now.ToString))
            sb.AppendLine(String.Format("Purchase Order {0}<br>", strPurchaseOrder))

            Dim dtData As New DataTable
            'Dim dtDataGood As New DataTable
            'Dim dtDataReject As New DataTable
            Dim dtDataCubao As New DataTable
            Dim dtDataUBP As New DataTable

            dtData.Columns.Add("Barcode", Type.GetType("System.String"))
            dtData.Columns.Add("CRN", Type.GetType("System.String"))
            dtData.Columns.Add("FirstName", Type.GetType("System.String"))
            dtData.Columns.Add("MiddleName", Type.GetType("System.String"))
            dtData.Columns.Add("LastName", Type.GetType("System.String"))
            dtData.Columns.Add("Suffix", Type.GetType("System.String"))
            dtData.Columns.Add("Sex", Type.GetType("System.String"))
            dtData.Columns.Add("DateOfBirth", Type.GetType("System.String"))
            dtData.Columns.Add("Address", Type.GetType("System.String"))
            dtData.Columns.Add("Path", Type.GetType("System.String"))
            dtData.Columns.Add("AddressDelimited", Type.GetType("System.String"))

            'dtDataReject = dtData.Clone
            'dtDataGood = dtData.Clone
            dtDataCubao = dtData.Clone
            dtDataUBP = dtData.Clone

            'Dim dtSSSReject As DataTable
            'If DAL.SelectSSSRejectPending Then
            '    dtSSSReject = DAL.TableResult
            'End If

            sb.AppendLine(String.Format("Start of XML reading {0}<br>", Now.ToString))

            Dim intSignatureWithZeroByte As Integer = 0
            Dim intLprimaryWithZeroByte As Integer = 0
            Dim intLbackupWithZeroByte As Integer = 0
            Dim intRprimaryWithZeroByte As Integer = 0
            Dim intRbackupWithZeroByte As Integer = 0

            Dim MemberXML As MemberXML
            For Each strSubDir As String In Directory.GetDirectories(SharedFunction.ForUploadingRepository & "\" & strPurchaseOrder)
                For Each strSubDir2 As String In Directory.GetDirectories(strSubDir)
                    Dim _barcode As String = strSubDir2.Substring(strSubDir2.LastIndexOf("\") + 1)

                    Try
                        'critical
                        Dim xmlFile As String = String.Format("{0}\{1}.xml", strSubDir2, _barcode)
                        Dim photoFile As String = String.Format("{0}\{1}_Photo.jpg", strSubDir2, _barcode)

                        'non critical
                        Dim signatureFile As String = String.Format("{0}\{1}_Signature.tiff", strSubDir2, _barcode)
                        Dim lprimaryFile As String = String.Format("{0}\{1}_Lprimary.ansi-fmr", strSubDir2, _barcode)
                        Dim lbackupFile As String = String.Format("{0}\{1}_Lbackup.ansi-fmr", strSubDir2, _barcode)
                        Dim rprimaryFile As String = String.Format("{0}\{1}_Rprimary.ansi-fmr", strSubDir2, _barcode)
                        Dim rbackupFile As String = String.Format("{0}\{1}_Rbackup.ansi-fmr", strSubDir2, _barcode)

                        If File.Exists(xmlFile) And File.Exists(photoFile) Then
                            MemberXML = New MemberXML(xmlFile)
                            If MemberXML.GetDataAndPopulate() Then
                                'If dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Reject'", MemberXML.Barcode)).Length > 0 Then
                                '    AddToRow(MemberXML, strSubDir2, dtDataReject)
                                'ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Reject'", MemberXML.CRN)).Length > 0 Then
                                '    AddToRow(MemberXML, strSubDir2, dtDataReject)
                                'ElseIf dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Good'", MemberXML.Barcode)).Length > 0 Then
                                '    AddToRow(MemberXML, strSubDir2, dtDataGood)
                                'ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Good'", MemberXML.CRN)).Length > 0 Then
                                '    AddToRow(MemberXML, strSubDir2, dtDataGood)
                                'Else
                                '    AddToRow(MemberXML, strSubDir2, dtData)
                                'End If

                                'If Not MemberXML.Barcode.ToUpper.Contains("C01") Then

                                ''clarify first cubao and ubp data
                                'If Not SharedFunction.IsCubaoData(MemberXML.Barcode) Then
                                '    AddToRow(MemberXML, strSubDir2, dtData)
                                'Else
                                '    AddToRow(MemberXML, strSubDir2, dtDataCubao)
                                'End If

                                If DAL.UpdateRelCDFRDataByBarcode(MemberXML.Barcode, strPurchaseOrder) Then
                                    If DAL.ObjectResult.ToString.Split("|")(0) = "0" Then
                                        AddToRow(MemberXML, strSubDir2, dtDataUBP)
                                    ElseIf DAL.ObjectResult.ToString.Split("|")(0) = "1" Then
                                        AddToRow(MemberXML, strSubDir2, dtData)
                                        sb.AppendLine(DAL.ObjectResult.ToString.Split("|")(1))
                                    End If
                                End If
                            Else
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML.GetDataAndPopulate(): Barcode {0}. Returned error {1}<br>", _barcode, MemberXML.ErrorMessage))
                            End If
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("MemberXML(): No xml/photo found for Barcode {0}.<br>", _barcode))
                        End If

                        CheckIfFileExist(signatureFile, "Signature", _barcode, 0, sb, intSignatureWithZeroByte)
                        CheckIfFileExist(lprimaryFile, "Lprimary", _barcode, 0, sb, intLprimaryWithZeroByte)
                        CheckIfFileExist(lbackupFile, "Lbackup", _barcode, 0, sb, intLbackupWithZeroByte)
                        CheckIfFileExist(rprimaryFile, "Rprimary", _barcode, 0, sb, intRprimaryWithZeroByte)
                        CheckIfFileExist(rbackupFile, "Rbackup", _barcode, 0, sb, intRbackupWithZeroByte)
                    Catch ex As Exception
                        intError += 1
                        sb.AppendLine(String.Format("Runtime error on xml reading of {0}\{1}.xml. Returned error {2}<br>", strSubDir2, _barcode, ex.Message))
                    End Try

                    MemberXML = Nothing
                Next
            Next

            If intSignatureWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total signature(s) with zero byte: {0}.<br>", intSignatureWithZeroByte.ToString))

            If intLprimaryWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total lprimary with zero byte: {0}.<br>", intLprimaryWithZeroByte.ToString))

            If intLbackupWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total lbackup with zero byte: {0}.<br>", intLbackupWithZeroByte.ToString))

            If intRprimaryWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total rprimary with zero byte: {0}.<br>", intRprimaryWithZeroByte.ToString))

            If intRbackupWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total rbackup with zero byte: {0}.<br>", intRbackupWithZeroByte.ToString))

            sb.AppendLine(String.Format("End of XML reading {0}<br>", Now.ToString))

            Try
                Dim intPageCntr As Short = 1
                Dim intPageSeriesCntr As Short = 1

                Dim intBoxCntr As Integer = 1
                Dim intBoxSeriesCntr As Integer = 1

                Dim IsSuccess As Boolean = True

                Dim intDtlCntr As Integer = 0

                sb.AppendLine(String.Format("Start of PO adding {0}<br>", Now.ToString))

                If DAL.GetLastBackOCR_ByPO(strPurchaseOrder) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If DAL.ObjectResult.ToString <> "" Then
                            '134.32.182
                            intBoxCntr = DAL.ObjectResult.ToString.Split(".")(1)
                            intBoxSeriesCntr = DAL.ObjectResult.ToString.Split(".")(2)

                            RecomputeBackOCR(intBoxCntr, intBoxSeriesCntr)
                        End If
                    End If
                End If

                Dim intPOID As Integer = 0
                If DAL.AddPO(strPurchaseOrder, 0, strBatch) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If Not IsDBNull(DAL.ObjectResult) Then
                            intPOID = DAL.ObjectResult
                            If intPOID > 0 Then

                                AddPODetails(DAL, dtDataUBP, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID,, True)
                                AddPODetails(DAL, dtData, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)

                                ''clarify first cubao and ubp data
                                AddPODetails(DAL, dtDataCubao, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)

                                'AddPODetails(DAL, dtDataReject, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)
                                'AddPODetails(DAL, dtDataGood, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID, DataKeysEnum.ActivityID.Done)

                                If strRawFile <> "" Then
                                    'If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder)) Then
                                    If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", Path.GetFileName(strRawFile))) Then
                                        sb.AppendLine("Failed to delete PO " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
                                    End If
                                End If

                                If Not DAL.ExecuteQuery(String.Format("UPDATE tblPO SET Quantity={0} WHERE POID={1}", intDtlCntr.ToString, intPOID)) Then
                                    sb.AppendLine("Failed to update PO qty of " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
                                End If
                            Else
                                IsSuccess = False
                                sb.AppendLine("POID is zero<br>")
                            End If
                        Else
                            IsSuccess = False
                            sb.AppendLine("POID is null<br>")
                        End If
                    Else
                        IsSuccess = False
                        sb.AppendLine("POID is nothing<br>")
                    End If
                Else
                    IsSuccess = False
                    sb.AppendLine(String.Format("AddPO(): PurchaseOrder {0}. Returned error {2}<br>", strPurchaseOrder, DAL.ErrorMessage))
                End If

                'For Each rwSSSReject As DataRow In dtSSSReject.Rows
                '    If Not DAL.UpdateSSSRejectByQAGRID(rwSSSReject("QAGRID"), strPurchaseOrder) Then
                '        intError += 1
                '        IsSuccess = False
                '        sb.AppendLine(String.Format("UpdateSSSRejectByQAGRID(): Failed to update QAGRID {0} for PO {1}. Returned error {2}<br>", rwSSSReject("QAGRID"), strPurchaseOrder, DAL.ErrorMessage))
                '    End If
                'Next

                'added on 07/31/2017
                If DAL.SelectQuery(String.Format("SELECT backocr, count(backocr) FROM dbo.tblRelPOData GROUP BY poid, backocr having (POID = {0}) and count(backocr)> 1", intPOID)) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        sb.AppendLine(String.Format("Duplicate BackOCR {0}<br>", _rw("backocr").ToString.Trim))
                        intError += 1
                        IsSuccess = False
                    Next
                End If

                If DAL.SelectQuery(String.Format("SELECT crn, count(crn) FROM dbo.tblRelPOData GROUP BY poid, crn having (POID = {0}) and count(crn)> 1", intPOID)) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        sb.AppendLine(String.Format("Duplicate CRN {0}<br>", _rw("backocr").ToString.Trim))
                        intError += 1
                        IsSuccess = False
                    Next
                End If

                If DAL.SelectQuery(String.Format("SELECT Barcode, count(Barcode) FROM dbo.tblRelPOData GROUP BY poid, Barcode having (POID = {0}) and count(Barcode)> 1", intPOID)) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        sb.AppendLine(String.Format("Duplicate Barcode {0}<br>", _rw("backocr").ToString.Trim))
                        intError += 1
                        IsSuccess = False
                    Next
                End If

                Dim intBoxDivisibleBy210 As Integer = SharedFunction.GetTotalBoxDivisibleBy210(intDtlCntr)

                If DAL.SelectPO_PerBoxCntr(intPOID) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        If CInt(_rw("Box")) <> (intBoxDivisibleBy210 + 1) Then
                            If CInt(_rw("Cntr")) <> 210 Then
                                sb.AppendLine(String.Format("Box {0} have {1} count<br>", _rw("Box"), _rw("Cntr")))
                                intError += 1
                                IsSuccess = False
                            End If
                        End If
                    Next
                End If
                ''07/31/2017

                If dtDataCubao.DefaultView.Count > 0 Then
                    sb.AppendLine("")
                    sb.AppendLine(String.Format("SSS Cubao Branch data: {0}<br>", dtDataCubao.DefaultView.Count.ToString("N0")))
                    sb.AppendLine("")
                    For Each _rw As DataRow In dtDataCubao.Rows
                        sb.AppendLine(_rw("Barcode").ToString.Trim)
                    Next

                    sb.AppendLine("")
                End If

                sb.AppendLine(String.Format("End of PO adding {0}<br>", Now.ToString) & vbNewLine)
                sb.AppendLine(String.Format("Error {0}<br>", intError.ToString) & vbNewLine)
                sb.AppendLine(String.Format("End process {0}<br>", Now.ToString) & vbNewLine)

                If IsSuccess Then
                    DAL.AddSystemLog(sb.ToString, AbsolutePath, UserID)
                Else
                    DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)
                End If

                Return True
            Catch ex2 As Exception
                sb.AppendLine(String.Format("UploadData(ex2): {0}<br>", ex2.Message) & vbNewLine)
                DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)

                Return False
            End Try
        Catch ex1 As Exception
            sb.AppendLine(String.Format("UploadData(ex1): {0}<br>", ex1.Message) & vbNewLine)
            DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)

            Return False
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    'changed due to sss cubao branch data
    Public Shared Function UploadData_old_20180726(ByVal strPurchaseOrder As String, ByVal strBatch As String, ByVal strRawFile As String,
                           ByVal AbsolutePath As String, ByVal UserID As Integer,
                           ByRef sb As StringBuilder) As Boolean
        Dim DAL As New DAL

        Try
            Dim intError As Integer = 0

            sb.AppendLine(String.Format("Start of UploadData() process {0}<br>", Now.ToString))
            sb.AppendLine(String.Format("Purchase Order {0}<br>", strPurchaseOrder))

            Dim dtData As New DataTable
            Dim dtDataGood As New DataTable
            Dim dtDataReject As New DataTable
            dtData.Columns.Add("Barcode", Type.GetType("System.String"))
            dtData.Columns.Add("CRN", Type.GetType("System.String"))
            dtData.Columns.Add("FirstName", Type.GetType("System.String"))
            dtData.Columns.Add("MiddleName", Type.GetType("System.String"))
            dtData.Columns.Add("LastName", Type.GetType("System.String"))
            dtData.Columns.Add("Suffix", Type.GetType("System.String"))
            dtData.Columns.Add("Sex", Type.GetType("System.String"))
            dtData.Columns.Add("DateOfBirth", Type.GetType("System.String"))
            dtData.Columns.Add("Address", Type.GetType("System.String"))
            dtData.Columns.Add("Path", Type.GetType("System.String"))
            dtData.Columns.Add("AddressDelimited", Type.GetType("System.String"))

            dtDataReject = dtData.Clone
            dtDataGood = dtData.Clone

            Dim dtSSSReject As DataTable
            If DAL.SelectSSSRejectPending Then
                dtSSSReject = DAL.TableResult
            End If

            sb.AppendLine(String.Format("Start of XML reading {0}<br>", Now.ToString))

            Dim intSignatureWithZeroByte As Integer = 0
            Dim intLprimaryWithZeroByte As Integer = 0
            Dim intLbackupWithZeroByte As Integer = 0
            Dim intRprimaryWithZeroByte As Integer = 0
            Dim intRbackupWithZeroByte As Integer = 0

            Dim MemberXML As MemberXML
            For Each strSubDir As String In Directory.GetDirectories(SharedFunction.ForUploadingRepository & "\" & strPurchaseOrder)
                For Each strSubDir2 As String In Directory.GetDirectories(strSubDir)
                    Dim _barcode As String = strSubDir2.Substring(strSubDir2.LastIndexOf("\") + 1)

                    Try
                        'critical
                        Dim xmlFile As String = String.Format("{0}\{1}.xml", strSubDir2, _barcode)
                        Dim photoFile As String = String.Format("{0}\{1}_Photo.jpg", strSubDir2, _barcode)

                        'non critical
                        Dim signatureFile As String = String.Format("{0}\{1}_Signature.tiff", strSubDir2, _barcode)
                        Dim lprimaryFile As String = String.Format("{0}\{1}_Lprimary.ansi-fmr", strSubDir2, _barcode)
                        Dim lbackupFile As String = String.Format("{0}\{1}_Lbackup.ansi-fmr", strSubDir2, _barcode)
                        Dim rprimaryFile As String = String.Format("{0}\{1}_Rprimary.ansi-fmr", strSubDir2, _barcode)
                        Dim rbackupFile As String = String.Format("{0}\{1}_Rbackup.ansi-fmr", strSubDir2, _barcode)

                        If File.Exists(xmlFile) And File.Exists(photoFile) Then
                            MemberXML = New MemberXML(xmlFile)
                            If MemberXML.GetDataAndPopulate() Then
                                If dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Reject'", MemberXML.Barcode)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataReject)
                                ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Reject'", MemberXML.CRN)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataReject)
                                ElseIf dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Good'", MemberXML.Barcode)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataGood)
                                ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Good'", MemberXML.CRN)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataGood)
                                Else
                                    AddToRow(MemberXML, strSubDir2, dtData)
                                End If
                            Else
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML.GetDataAndPopulate(): Barcode {0}. Returned error {1}<br>", _barcode, MemberXML.ErrorMessage))
                            End If
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("MemberXML(): No xml/photo found for Barcode {0}.<br>", _barcode))
                        End If

                        CheckIfFileExist(signatureFile, "Signature", _barcode, 0, sb, intSignatureWithZeroByte)
                        CheckIfFileExist(lprimaryFile, "Lprimary", _barcode, 0, sb, intLprimaryWithZeroByte)
                        CheckIfFileExist(lbackupFile, "Lbackup", _barcode, 0, sb, intLbackupWithZeroByte)
                        CheckIfFileExist(rprimaryFile, "Rprimary", _barcode, 0, sb, intRprimaryWithZeroByte)
                        CheckIfFileExist(rbackupFile, "Rbackup", _barcode, 0, sb, intRbackupWithZeroByte)
                    Catch ex As Exception
                        intError += 1
                        sb.AppendLine(String.Format("Runtime error on xml reading of {0}\{1}.xml. Returned error {2}<br>", strSubDir2, _barcode, ex.Message))
                    End Try

                    MemberXML = Nothing
                Next
            Next

            If intSignatureWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total signature(s) with zero byte: {0}.<br>", intSignatureWithZeroByte.ToString))

            If intLprimaryWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total lprimary with zero byte: {0}.<br>", intLprimaryWithZeroByte.ToString))

            If intLbackupWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total lbackup with zero byte: {0}.<br>", intLbackupWithZeroByte.ToString))

            If intRprimaryWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total rprimary with zero byte: {0}.<br>", intRprimaryWithZeroByte.ToString))

            If intRbackupWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total rbackup with zero byte: {0}.<br>", intRbackupWithZeroByte.ToString))

            sb.AppendLine(String.Format("End of XML reading {0}<br>", Now.ToString))

            Try
                Dim intPageCntr As Short = 1
                Dim intPageSeriesCntr As Short = 1

                Dim intBoxCntr As Integer = 1
                Dim intBoxSeriesCntr As Integer = 1

                Dim IsSuccess As Boolean = True

                Dim intDtlCntr As Integer = 0

                sb.AppendLine(String.Format("Start of PO adding {0}<br>", Now.ToString))

                If DAL.GetLastBackOCR_ByPO(strPurchaseOrder) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If DAL.ObjectResult.ToString <> "" Then
                            '134.32.182
                            intBoxCntr = DAL.ObjectResult.ToString.Split(".")(1)
                            intBoxSeriesCntr = DAL.ObjectResult.ToString.Split(".")(2)

                            RecomputeBackOCR(intBoxCntr, intBoxSeriesCntr)
                        End If
                    End If
                End If

                Dim intPOID As Integer = 0
                If DAL.AddPO(strPurchaseOrder, 0, strBatch) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If Not IsDBNull(DAL.ObjectResult) Then
                            intPOID = DAL.ObjectResult
                            If intPOID > 0 Then
                                AddPODetails(DAL, dtData, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)
                                AddPODetails(DAL, dtDataReject, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)
                                AddPODetails(DAL, dtDataGood, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID, DataKeysEnum.ActivityID.Done)

                                If strRawFile <> "" Then
                                    'If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder)) Then
                                    If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", Path.GetFileName(strRawFile))) Then
                                        sb.AppendLine("Failed to delete PO " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
                                    End If
                                End If

                                If Not DAL.ExecuteQuery(String.Format("UPDATE tblPO SET Quantity={0} WHERE POID={1}", intDtlCntr.ToString, intPOID)) Then
                                    sb.AppendLine("Failed to update PO qty of " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
                                End If
                            Else
                                IsSuccess = False
                                sb.AppendLine("POID is zero<br>")
                            End If
                        Else
                            IsSuccess = False
                            sb.AppendLine("POID is null<br>")
                        End If
                    Else
                        IsSuccess = False
                        sb.AppendLine("POID is nothing<br>")
                    End If
                Else
                    IsSuccess = False
                    sb.AppendLine(String.Format("AddPO(): PurchaseOrder {0}. Returned error {2}<br>", strPurchaseOrder, DAL.ErrorMessage))
                End If

                For Each rwSSSReject As DataRow In dtSSSReject.Rows
                    If Not DAL.UpdateSSSRejectByQAGRID(rwSSSReject("QAGRID"), strPurchaseOrder) Then
                        intError += 1
                        IsSuccess = False
                        sb.AppendLine(String.Format("UpdateSSSRejectByQAGRID(): Failed to update QAGRID {0} for PO {1}. Returned error {2}<br>", rwSSSReject("QAGRID"), strPurchaseOrder, DAL.ErrorMessage))
                    End If
                Next

                'added on 07/31/2017
                If DAL.SelectQuery(String.Format("SELECT backocr, count(backocr) FROM dbo.tblRelPOData GROUP BY poid, backocr having (POID = {0}) and count(backocr)> 1", intPOID)) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        sb.AppendLine(String.Format("Duplicate BackOCR {0}<br>", _rw("backocr").ToString.Trim))
                        intError += 1
                        IsSuccess = False
                    Next
                End If

                If DAL.SelectQuery(String.Format("SELECT crn, count(crn) FROM dbo.tblRelPOData GROUP BY poid, crn having (POID = {0}) and count(crn)> 1", intPOID)) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        sb.AppendLine(String.Format("Duplicate CRN {0}<br>", _rw("backocr").ToString.Trim))
                        intError += 1
                        IsSuccess = False
                    Next
                End If

                If DAL.SelectQuery(String.Format("SELECT Barcode, count(Barcode) FROM dbo.tblRelPOData GROUP BY poid, Barcode having (POID = {0}) and count(Barcode)> 1", intPOID)) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        sb.AppendLine(String.Format("Duplicate Barcode {0}<br>", _rw("backocr").ToString.Trim))
                        intError += 1
                        IsSuccess = False
                    Next
                End If

                Dim intBoxDivisibleBy210 As Integer = SharedFunction.GetTotalBoxDivisibleBy210(intDtlCntr)

                If DAL.SelectPO_PerBoxCntr(intPOID) Then
                    For Each _rw As DataRow In DAL.TableResult.Rows
                        If CInt(_rw("Box")) <> (intBoxDivisibleBy210 + 1) Then
                            If CInt(_rw("Cntr")) <> 210 Then
                                sb.AppendLine(String.Format("Box {0} have {1} count<br>", _rw("Box"), _rw("Cntr")))
                                intError += 1
                                IsSuccess = False
                            End If
                        End If
                    Next
                End If
                ''07/31/2017

                sb.AppendLine(String.Format("End of PO adding {0}<br>", Now.ToString) & vbNewLine)
                sb.AppendLine(String.Format("Error {0}<br>", intError.ToString) & vbNewLine)
                sb.AppendLine(String.Format("End process {0}<br>", Now.ToString) & vbNewLine)

                If IsSuccess Then
                    DAL.AddSystemLog(sb.ToString, AbsolutePath, UserID)
                Else
                    DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)
                End If

                Return True
            Catch ex2 As Exception
                sb.AppendLine(String.Format("UploadData(ex2): {0}<br>", ex2.Message) & vbNewLine)
                DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)

                Return False
            End Try
        Catch ex1 As Exception
            sb.AppendLine(String.Format("UploadData(ex1): {0}<br>", ex1.Message) & vbNewLine)
            DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)

            Return False
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Function UploadData2(ByVal strPurchaseOrder As String, ByVal strBatch As String, ByVal strRawFile As String,
                                      ByVal AbsolutePath As String, ByVal UserID As Integer,
                                      ByRef sb As StringBuilder) As Boolean
        Try
            Dim intError As Integer = 0

            sb.AppendLine(String.Format("Start of UploadData() process {0}<br>", Now.ToString))
            sb.AppendLine(String.Format("Purchase Order {0}<br>", strPurchaseOrder))

            Dim dtData As New DataTable
            Dim dtDataGood As New DataTable
            Dim dtDataReject As New DataTable
            dtData.Columns.Add("Barcode", Type.GetType("System.String"))
            dtData.Columns.Add("CRN", Type.GetType("System.String"))
            dtData.Columns.Add("FirstName", Type.GetType("System.String"))
            dtData.Columns.Add("MiddleName", Type.GetType("System.String"))
            dtData.Columns.Add("LastName", Type.GetType("System.String"))
            dtData.Columns.Add("Suffix", Type.GetType("System.String"))
            dtData.Columns.Add("Sex", Type.GetType("System.String"))
            dtData.Columns.Add("DateOfBirth", Type.GetType("System.String"))
            dtData.Columns.Add("Address", Type.GetType("System.String"))
            dtData.Columns.Add("Path", Type.GetType("System.String"))
            dtData.Columns.Add("AddressDelimited", Type.GetType("System.String"))

            dtDataReject = dtData.Clone
            dtDataGood = dtData.Clone

            Dim DAL As New DAL
            Dim dtSSSReject As DataTable
            If DAL.SelectSSSRejectPending Then
                dtSSSReject = DAL.TableResult
            End If

            sb.AppendLine(String.Format("Start of XML reading {0}<br>", Now.ToString))

            Dim intSignatureWithZeroByte As Integer = 0
            Dim intLprimaryWithZeroByte As Integer = 0
            Dim intLbackupWithZeroByte As Integer = 0
            Dim intRprimaryWithZeroByte As Integer = 0
            Dim intRbackupWithZeroByte As Integer = 0

            Dim MemberXML As MemberXML
            For Each strSubDir As String In Directory.GetDirectories(SharedFunction.ForUploadingRepository & "\" & strPurchaseOrder)
                For Each strSubDir2 As String In Directory.GetDirectories(strSubDir)
                    Dim _barcode As String = strSubDir2.Substring(strSubDir2.LastIndexOf("\") + 1)

                    Try
                        'critical
                        Dim xmlFile As String = String.Format("{0}\{1}.xml", strSubDir2, _barcode)
                        Dim photoFile As String = String.Format("{0}\{1}_Photo.jpg", strSubDir2, _barcode)

                        'non critical
                        Dim signatureFile As String = String.Format("{0}\{1}_Signature.tiff", strSubDir2, _barcode)
                        'Dim xmlPersoFile As String = String.Format("{0}\{1}_perso.xml", strSubDir2, _barcode)
                        Dim lprimaryFile As String = String.Format("{0}\{1}_Lprimary.ansi-fmr", strSubDir2, _barcode)
                        Dim lbackupFile As String = String.Format("{0}\{1}_Lbackup.ansi-fmr", strSubDir2, _barcode)
                        Dim rprimaryFile As String = String.Format("{0}\{1}_Rprimary.ansi-fmr", strSubDir2, _barcode)
                        Dim rbackupFile As String = String.Format("{0}\{1}_Rbackup.ansi-fmr", strSubDir2, _barcode)

                        If File.Exists(xmlFile) And File.Exists(photoFile) Then
                            MemberXML = New MemberXML(xmlFile)
                            If MemberXML.GetDataAndPopulate() Then
                                If dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Reject'", MemberXML.Barcode)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataReject)
                                ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Reject'", MemberXML.CRN)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataReject)
                                ElseIf dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Good'", MemberXML.Barcode)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataGood)
                                ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Good'", MemberXML.CRN)).Length > 0 Then
                                    AddToRow(MemberXML, strSubDir2, dtDataGood)
                                Else
                                    AddToRow(MemberXML, strSubDir2, dtData)
                                End If
                            Else
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML.GetDataAndPopulate(): Barcode {0}. Returned error {1}<br>", _barcode, MemberXML.ErrorMessage))
                            End If
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("MemberXML(): No xml/photo found for Barcode {0}.<br>", _barcode))
                        End If

                        CheckIfFileExist(signatureFile, "Signature", _barcode, 0, sb, intSignatureWithZeroByte)
                        CheckIfFileExist(lprimaryFile, "Lprimary", _barcode, 0, sb, intLprimaryWithZeroByte)
                        CheckIfFileExist(lbackupFile, "Lbackup", _barcode, 0, sb, intLbackupWithZeroByte)
                        CheckIfFileExist(rprimaryFile, "Rprimary", _barcode, 0, sb, intRprimaryWithZeroByte)
                        CheckIfFileExist(rbackupFile, "Rbackup", _barcode, 0, sb, intRbackupWithZeroByte)
                    Catch ex As Exception
                        intError += 1
                        sb.AppendLine(String.Format("Runtime error on xml reading of {0}\{1}.xml. Returned error {2}<br>", strSubDir2, _barcode, ex.Message))
                    End Try

                    MemberXML = Nothing
                Next
            Next

            If intSignatureWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total signature(s) with zero byte: {0}.<br>", intSignatureWithZeroByte.ToString))

            If intLprimaryWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total lprimary with zero byte: {0}.<br>", intLprimaryWithZeroByte.ToString))

            If intLbackupWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total lbackup with zero byte: {0}.<br>", intLbackupWithZeroByte.ToString))

            If intRprimaryWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total rprimary with zero byte: {0}.<br>", intRprimaryWithZeroByte.ToString))

            If intRbackupWithZeroByte > 0 Then _
                sb.AppendLine(String.Format("Total rbackup with zero byte: {0}.<br>", intRbackupWithZeroByte.ToString))

            sb.AppendLine(String.Format("End of XML reading {0}<br>", Now.ToString))

            Try
                Dim intPageCntr As Short = 1
                Dim intPageSeriesCntr As Short = 1

                Dim intBoxCntr As Integer = 1
                Dim intBoxSeriesCntr As Integer = 1

                Dim IsSuccess As Boolean = True

                Dim intDtlCntr As Integer = 0

                sb.AppendLine(String.Format("Start of PO adding {0}<br>", Now.ToString))

                If DAL.GetLastBackOCR_ByPO(strPurchaseOrder) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If DAL.ObjectResult.ToString <> "" Then
                            '134.32.182
                            intBoxCntr = DAL.ObjectResult.ToString.Split(".")(1)
                            intBoxSeriesCntr = DAL.ObjectResult.ToString.Split(".")(2)

                            RecomputeBackOCR(intBoxCntr, intBoxSeriesCntr)
                        End If
                    End If
                End If

                Dim intPOID As Integer = 0
                If DAL.AddPO(strPurchaseOrder, 0, strBatch) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If Not IsDBNull(DAL.ObjectResult) Then
                            intPOID = DAL.ObjectResult
                            If intPOID > 0 Then
                                AddPODetails(DAL, dtData, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)
                                AddPODetails(DAL, dtDataReject, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID)
                                AddPODetails(DAL, dtDataGood, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, UserID, DataKeysEnum.ActivityID.Done)

                                If strRawFile <> "" Then
                                    'If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder)) Then
                                    If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strRawFile)) Then
                                        sb.AppendLine("Failed to delete PO " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
                                    End If
                                End If

                                If Not DAL.ExecuteQuery(String.Format("UPDATE tblPO SET Quantity={0} WHERE POID={1}", intDtlCntr.ToString, intPOID)) Then
                                    sb.AppendLine("Failed to update PO qty of " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
                                End If
                            Else
                                IsSuccess = False
                                sb.AppendLine("POID is zero<br>")
                            End If
                        Else
                            IsSuccess = False
                            sb.AppendLine("POID is null<br>")
                        End If
                    Else
                        IsSuccess = False
                        sb.AppendLine("POID is nothing<br>")
                    End If
                Else
                    IsSuccess = False
                    sb.AppendLine(String.Format("AddPO(): PurchaseOrder {0}. Returned error {2}<br>", strPurchaseOrder, DAL.ErrorMessage))
                End If

                For Each rwSSSReject As DataRow In dtSSSReject.Rows
                    If Not DAL.UpdateSSSRejectByQAGRID(rwSSSReject("QAGRID"), strPurchaseOrder) Then
                        intError += 1
                        IsSuccess = False
                        sb.AppendLine(String.Format("UpdateSSSRejectByQAGRID(): Failed to update QAGRID {0} for PO {1}. Returned error {2}<br>", rwSSSReject("QAGRID"), strPurchaseOrder, DAL.ErrorMessage))
                    End If
                Next

                sb.AppendLine(String.Format("End of PO adding {0}<br>", Now.ToString) & vbNewLine)
                sb.AppendLine(String.Format("Error {0}<br>", intError.ToString) & vbNewLine)
                sb.AppendLine(String.Format("End process {0}<br>", Now.ToString) & vbNewLine)

                If IsSuccess Then
                    DAL.AddSystemLog(sb.ToString, AbsolutePath, UserID)
                Else
                    DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)
                End If

                DAL.Dispose()
                DAL = Nothing
            Catch ex As Exception
            End Try

            Return True
        Catch ex3 As Exception
            Return False
        End Try
    End Function

    Private Shared Sub AddPODetails(ByVal DAL As DAL, ByVal dt As DataTable, ByVal intPOID As Integer, ByVal strBatch As String,
                                ByRef intBoxCntr As Integer, ByRef intBoxSeriesCntr As Integer, ByRef intDtlCntr As Integer,
                                ByRef intPageCntr As Short, ByRef intPageSeriesCntr As Short, ByRef IsSuccess As Boolean,
                                ByRef intError As Integer, ByRef sb As StringBuilder,
                                ByVal UserID As Integer,
                                Optional ActivityID As DataKeysEnum.ActivityID = DataKeysEnum.ActivityID.IndigoDownload,
                                Optional IsUBP As Boolean = False)
        For Each rw As DataRow In dt.Rows
            Dim strBackOCR As String = String.Format("{0}.{1}.{2}", strBatch, intBoxCntr.ToString, intBoxSeriesCntr.ToString)
            Dim strSubDir As String = rw("Path").ToString.Trim.Split("\")(rw("Path").ToString.Trim.Split("\").Length - 2)

            If DAL.AddRelPOData(intPOID, rw("CRN"), rw("Barcode"), rw("FirstName"), rw("MiddleName"), rw("LastName"), rw("Suffix"), rw("Sex"), rw("DateOfBirth"), rw("Address"), ActivityID, strBackOCR, 0, 0, strSubDir, rw("AddressDelimited"), IsUBP) Then
                intDtlCntr += 1

                DAL.AddCardActivity(intPOID, rw("CRN"), rw("Barcode"), "Uploaded", UserID, "Uploaded")

                If Not SharedFunction.CopyTo_PSBImages(String.Format("{0}\{1}_Photo.jpg", rw("Path"), rw("Barcode"))) Then
                    IsSuccess = False
                    intError += 1
                    sb.AppendLine(String.Format("AddRelPOImage(): Barcode {0}, CRN {1}. {2}<br>", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Unable to copy photo to pbs images"))
                End If

                Dim strSignatureFile As String = GetFile(String.Format("{0}\{1}_Signature.tiff", rw("Path"), rw("Barcode")), 2)
                If SharedFunction.TIFFtoJPG(File.ReadAllBytes(strSignatureFile), strSignatureFile.Replace(".tiff", ".jpg")) Then
                    If Not SharedFunction.CopyTo_PSBImages(strSignatureFile.Replace(".tiff", ".jpg")) Then
                        IsSuccess = False
                        intError += 1
                        sb.AppendLine(String.Format("AddRelPOImage(): Barcode {0}, CRN {1}. Returned error {2}<br>", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Unable to copy signature to pbs images"))
                    End If
                Else
                    IsSuccess = False
                    intError += 1
                    sb.AppendLine(String.Format("AddRelPOImage(): Barcode {0}, CRN {1}. {2}<br>", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Unable to convert signature from tiff to jpg"))
                End If
            Else
                IsSuccess = False
                intError += 1
                sb.AppendLine(String.Format("AddRelPOData(): Barcode {0}, CRN {1}. Returned error {2}", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, DAL.ErrorMessage))
            End If

            If intPageSeriesCntr = SharedFunction.CardsPerSheet Then
                intPageSeriesCntr = 1
                intPageCntr += 1
            Else
                intPageSeriesCntr += 1
            End If

            If intBoxSeriesCntr = SharedFunction.CardsPerBox Then
                intBoxSeriesCntr = 1
                intBoxCntr += 1
            Else
                intBoxSeriesCntr += 1
            End If
        Next
    End Sub

    Private Shared Sub AddPODetails2(ByVal DAL As DAL, ByVal dt As DataTable, ByVal intPOID As Integer, ByVal strBatch As String,
                                ByRef intBoxCntr As Integer, ByRef intBoxSeriesCntr As Integer, ByRef intDtlCntr As Integer,
                                ByRef intPageCntr As Short, ByRef intPageSeriesCntr As Short, ByRef IsSuccess As Boolean,
                                ByRef intError As Integer, ByRef sb As StringBuilder,
                                ByVal UserID As Integer,
                                Optional ActivityID As DataKeysEnum.ActivityID = DataKeysEnum.ActivityID.IndigoDownload)
        For Each rw As DataRow In dt.Rows
            Dim strBackOCR As String = String.Format("{0}.{1}.{2}", strBatch, intBoxCntr.ToString, intBoxSeriesCntr.ToString)
            Dim strSubDir As String = rw("Path").ToString.Substring(rw("Path").ToString.LastIndexOf("\") + 1)

            If DAL.AddRelPOData(intPOID, rw("CRN"), rw("Barcode"), rw("FirstName"), rw("MiddleName"), rw("LastName"), rw("Suffix"), rw("Sex"), rw("DateOfBirth"), rw("Address"), ActivityID, strBackOCR, 0, 0, strSubDir, rw("AddressDelimited")) Then
                intDtlCntr += 1

                DAL.AddCardActivity(intPOID, rw("CRN"), rw("Barcode"), "Uploaded", UserID, "Uploaded")

                If Not DAL.AddRelPOImage(rw("Barcode"),
                                            File.ReadAllBytes(String.Format("{0}\{1}.xml", rw("Path"), rw("Barcode"))),
                                            GetFileByte(String.Format("{0}\{1}_perso.xml", rw("Path"), rw("Barcode")), 1),
                                            File.ReadAllBytes(String.Format("{0}\{1}_Photo.jpg", rw("Path"), rw("Barcode"))),
                                            GetFileByte(String.Format("{0}\{1}_Signature.tiff", rw("Path"), rw("Barcode")), 2),
                                            GetFileByte(String.Format("{0}\{1}_Rprimary.ansi-fmr", rw("Path"), rw("Barcode")), 3),
                                            GetFileByte(String.Format("{0}\{1}_Rbackup.ansi-fmr", rw("Path"), rw("Barcode")), 3),
                                            GetFileByte(String.Format("{0}\{1}_Lprimary.ansi-fmr", rw("Path"), rw("Barcode")), 3),
                                            GetFileByte(String.Format("{0}\{1}_Lbackup.ansi-fmr", rw("Path"), rw("Barcode")), 3)) Then
                    IsSuccess = False
                    intError += 1
                    sb.AppendLine(String.Format("AddRelPOImage(): Barcode {0}, CRN {1}. Returned error {2}<br>", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, DAL.ErrorMessage))
                End If
            Else
                IsSuccess = False
                intError += 1
                sb.AppendLine(String.Format("AddRelPOData(): Barcode {0}, CRN {1}. Returned error {2}", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, DAL.ErrorMessage))
            End If

            If intPageSeriesCntr = SharedFunction.CardsPerSheet Then
                intPageSeriesCntr = 1
                intPageCntr += 1
            Else
                intPageSeriesCntr += 1
            End If

            If intBoxSeriesCntr = SharedFunction.CardsPerBox Then
                intBoxSeriesCntr = 1
                intBoxCntr += 1
            Else
                intBoxSeriesCntr += 1
            End If
        Next
    End Sub

    Private Shared Sub RecomputeBackOCR(ByRef intBoxCntr As Integer, ByRef intBoxSeriesCntr As Integer)
        If intBoxSeriesCntr = SharedFunction.CardsPerBox Then
            intBoxSeriesCntr = 1
            intBoxCntr += 1
        Else
            intBoxSeriesCntr += 1
        End If
    End Sub

    Private Shared Sub AddToRow(ByVal MemberXML As MemberXML, ByVal strSubDir2 As String, ByRef dt As DataTable)
        Dim rw As DataRow = dt.NewRow
        rw("Barcode") = MemberXML.Barcode
        rw("CRN") = MemberXML.CRN
        rw("FirstName") = MemberXML.FirstName
        rw("MiddleName") = MemberXML.MiddleName
        rw("LastName") = MemberXML.LastName
        rw("Suffix") = MemberXML.Suffix
        rw("Sex") = MemberXML.Sex
        rw("DateOfBirth") = MemberXML.DateOfBirth
        rw("Address") = MemberXML.Address
        rw("Path") = strSubDir2
        rw("AddressDelimited") = MemberXML.AddressDelimited
        dt.Rows.Add(rw)
    End Sub

    Public Shared Function GetPath(ByVal strPurchaseOrder As String, ByVal strSubDir As String, ByVal strBarcode As String) As String
        Try
            Dim strPath As String = String.Format("{0}\{1}\{2}\{3}", ForUploadingRepository, strPurchaseOrder, strSubDir, strBarcode)
            If Directory.Exists(strPath) Then
                Return strPath
            Else
                Dim dirPath As String = String.Format("{0}\{1}", ForUploadingRepository, strPurchaseOrder)

                If Directory.Exists(dirPath) Then
                    For Each strSubDir2 As String In Directory.GetDirectories(String.Format("{0}\{1}", ForUploadingRepository, strPurchaseOrder))
                        If Directory.Exists(String.Format("{0}\{1}", strSubDir2, strBarcode)) Then
                            Return String.Format("{0}\{1}", strSubDir2, strBarcode)
                            Exit For
                        End If
                    Next
                Else
                    Return "Error"
                End If
            End If

            Return "Error"
        Catch ex As Exception
            Return "Error"
        End Try
        
    End Function

    Public Shared Function CheckAndCreateDirectory(ByVal strDirectory As String) As String
        If Not Directory.Exists(strDirectory) Then Directory.CreateDirectory(strDirectory)

        Return strDirectory
    End Function

    Public Shared Sub FileCopyImage(ByVal strSourceFile As String, ByVal strDestination As String)
        If File.Exists(strSourceFile) Then
            File.Copy(strSourceFile, String.Format("{0}\{1}", SharedFunction.CheckAndCreateDirectory(strDestination), Path.GetFileName(strSourceFile)), True)
        Else
            If Path.GetFileName(strSourceFile).Contains(".xml") Then
                File.Copy(SharedFunction.templateXML_File, String.Format("{0}\{1}", SharedFunction.CheckAndCreateDirectory(strDestination), Path.GetFileName(strSourceFile)))
            ElseIf Path.GetFileName(strSourceFile).Contains("_Signature.tiff") Then
                File.Copy(SharedFunction.templateSignature_File, String.Format("{0}\{1}", SharedFunction.CheckAndCreateDirectory(strDestination), Path.GetFileName(strSourceFile)))
            ElseIf Path.GetFileName(strSourceFile).Contains(".ansi-fmr") Then
                File.Copy(SharedFunction.templateANSI_File, String.Format("{0}\{1}", SharedFunction.CheckAndCreateDirectory(strDestination), Path.GetFileName(strSourceFile)))
            End If
        End If
    End Sub

    Public Shared Sub FileCopyImagev2(ByVal strSourceFile As String, ByVal strDestination As String)
        If File.Exists(strSourceFile) Then
            File.Copy(strSourceFile, String.Format("{0}\{1}", strDestination, Path.GetFileName(strSourceFile)), True)
        Else
            If Path.GetFileName(strSourceFile).Contains(".xml") Then
                File.Copy(SharedFunction.templateXML_File, String.Format("{0}\{1}", strDestination, Path.GetFileName(strSourceFile)))
            ElseIf Path.GetFileName(strSourceFile).Contains("_Signature.tiff") Then
                File.Copy(SharedFunction.templateSignature_File, String.Format("{0}\{1}", strDestination, Path.GetFileName(strSourceFile)))
            ElseIf Path.GetFileName(strSourceFile).Contains(".ansi-fmr") Then
                File.Copy(SharedFunction.templateANSI_File, String.Format("{0}\{1}", strDestination, Path.GetFileName(strSourceFile)))
            End If
        End If
    End Sub

    Public Shared Function CopyTo_PSBImages(ByVal strFile As String) As Boolean
        Try
            File.Copy(strFile, String.Format("{0}\{1}", PSBImagesRepository, Path.GetFileName(strFile)), True)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function GetFrom_PSBImages(ByVal strFile As String) As String
        Try
            If File.Exists(String.Format("{0}\{1}", PSBImagesRepository, strFile)) Then
                Return String.Format("{0}\{1}", PSBImagesRepository, strFile)
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function GetPhotoImage(ByVal strBarcode As String) As Byte()
        Dim result As String = GetFrom_PSBImages(String.Format("{0}_Photo.jpg", strBarcode))
        If result <> "" Then
            Return File.ReadAllBytes(result)
        Else
            Return File.ReadAllBytes(NoImageFound)
        End If
    End Function

    Public Shared Function GetSignatureImage(ByVal strBarcode As String) As Byte()
        Dim result As String = GetFrom_PSBImages(String.Format("{0}_Signature.jpg", strBarcode))
        If result <> "" Then
            Return File.ReadAllBytes(result)
        Else
            Return File.ReadAllBytes(NoImageFound)
        End If
    End Function

    Public Shared Function GetDR_PDF_DOCCNTR() As Integer
        If Not File.Exists(DR_PDF_DOCCNTR) Then
            Return 0
        Else
            Return CInt(IO.File.ReadAllText(DR_PDF_DOCCNTR))
        End If
    End Function

    Public Shared Function GetForUploadingFileType() As String
        Dim str As String = File.ReadAllText(ForUploadingFileType)
        Return str
    End Function

    Public Shared Function GetFile(ByVal strFile As String, ByVal intType As Short) As String
        If File.Exists(strFile) Then
            Return strFile
        Else
            Select Case intType
                Case 1 'xml
                    Return templateXML_File
                Case 2 'signature
                    Return templateSignature_File
                Case 3 'ansi
                    Return templateANSI_File
            End Select
        End If
    End Function

    Public Shared Function UploadDataCDFR(ByVal strReference As String, ByVal dtData As DataTable, ByRef IsSuccess As Boolean,
                           ByVal AbsolutePath As String, ByVal UserID As Integer,
                           ByRef sb As StringBuilder) As Boolean
        Dim DAL As New DAL

        Try
            Dim intError As Integer = 0

            sb.AppendLine(String.Format("Start of UploadDataCDFR() process {0}<br>", Now.ToString))
            sb.AppendLine(String.Format("CDFR {0}<br>", strReference))

            Try
                sb.AppendLine(String.Format("Start of cdfr adding {0}<br>", Now.ToString))

                Dim intCntr As Integer = 0
                Dim intGoodCntr As Integer = 0
                Dim intBadCntr As Integer = 0

                Dim intCDFRID As Integer = 0
                If DAL.AddCDFR(strReference) Then
                    If Not DAL.ObjectResult Is Nothing Then
                        If Not IsDBNull(DAL.ObjectResult) Then
                            intCDFRID = DAL.ObjectResult
                            If intCDFRID > 0 Then
                                For Each rw As DataRow In dtData.Rows
                                    If DAL.AddRelCDFRData(intCDFRID, rw("CRN"), rw("Barcode"), rw("GSISNo"), rw("BPNo"), rw("SSSNo"), rw("ACNo"), rw("CNo"), rw("ExpDate"), rw("OrigData")) Then
                                        If DAL.ObjectResult.ToString.Split("|")(0) = "1" Then
                                            intError += 1
                                            intBadCntr += 1
                                            sb.AppendLine(String.Format("AddRelCDFRData(): CRN {0}. {1}<br>", rw("CRN"), DAL.ObjectResult.ToString.Split("|")(1)))
                                        Else
                                            intGoodCntr += 1
                                        End If
                                    Else
                                        intError += 1
                                        intBadCntr += 1
                                        sb.AppendLine(String.Format("AddRelCDFRData(): Failed to add CRN {0}. Error {1}<br>", rw("CRN"), DAL.ErrorMessage))
                                    End If
                                    intCntr += 1
                                Next

                                If Not DAL.ExecuteQuery(String.Format("UPDATE tblCDFR SET Quantity={0} WHERE CDFRID={1}", intCntr.ToString, intCDFRID)) Then
                                    sb.AppendLine("Failed to update cdfr qty of " & strReference & ". Returned error " & DAL.ErrorMessage & "<br>")
                                End If
                            Else
                                IsSuccess = False
                                sb.AppendLine("CDFR is zero<br>")
                            End If
                        Else
                            IsSuccess = False
                            sb.AppendLine("CDFR is null<br>")
                        End If
                    Else
                        IsSuccess = False
                        sb.AppendLine("CDFR is nothing<br>")
                    End If
                Else
                    IsSuccess = False
                    sb.AppendLine(String.Format("AddCDFR(): CDFR {0}. Returned error {1}<br>", strReference, DAL.ErrorMessage))
                End If

                sb.AppendLine(String.Format("Success: {0}, Failed {1}, Total: {2}<br>", intGoodCntr.ToString("N0"), intBadCntr.ToString("N0"), intCntr.ToString("N0")))

                ''If DAL.SelectQuery(String.Format("SELECT crn, count(crn) FROM dbo.tblRelBatchData GROUP BY BatchID, crn having (BatchID = {0}) and count(backocr)> 1", intBatchID)) Then
                'If DAL.SelectQuery(String.Format("SELECT crn, count(crn) FROM dbo.tblRelBatchData GROUP BY Batch, crn having (Batch = '{0}') and count(backocr)> 1", strBatch)) Then
                '    For Each _rw As DataRow In DAL.TableResult.Rows
                '        sb.AppendLine(String.Format("Duplicate CRN {0}<br>", _rw("crn").ToString.Trim))
                '        intError += 1
                '        IsSuccess = False
                '    Next
                'End If

                sb.AppendLine(String.Format("End of cdfr adding {0}<br>", Now.ToString) & vbNewLine)
                sb.AppendLine(String.Format("Error {0}<br>", intError.ToString) & vbNewLine)
                sb.AppendLine(String.Format("End process {0}<br>", Now.ToString) & vbNewLine)

                If IsSuccess Then
                    DAL.AddSystemLog(sb.ToString, AbsolutePath, UserID)
                Else
                    DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)
                End If

                Return True
            Catch ex2 As Exception
                sb.AppendLine(String.Format("UploadDataCDFR(ex2): {0}<br>", ex2.Message) & vbNewLine)
                DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)

                Return False
            End Try
        Catch ex1 As Exception
            sb.AppendLine(String.Format("UploadDataCDFR(ex1): {0}<br>", ex1.Message) & vbNewLine)
            DAL.AddErrorLog(sb.ToString, AbsolutePath, UserID)

            Return False
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Sub Generate_PO_Reports(ByVal POID As Integer, ByVal PurchaseOrder As String, _
                                          ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String, ByRef lblStatus As DevExpress.Web.ASPxEditors.ASPxLabel)
        'PDF
        Dim RequestedBy As String, PODate As String, RecordsReceived As String, InvalidRecords As String, ValidRecords As String, Reprints As String, CardForPrinting As String, Description As String, JVConfirmedDate As String, JVConfirmedBy As String

        Description = ""

        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT SSSRepresentative, AllcardSalesRepresentative FROM tblSystemParameter") Then
            RequestedBy = DAL.TableResult.Rows(0)("SSSRepresentative")
            JVConfirmedBy = DAL.TableResult.Rows(0)("AllcardSalesRepresentative")
        End If

        If DAL.SelectQuery(String.Format("SELECT POID, PurchaseOrder, Quantity, DateTimePosted FROM dbo.tblPO WHERE POID={0}", POID)) Then
            For Each rw As DataRow In DAL.TableResult.Rows
                PurchaseOrder = rw("PurchaseOrder")
                PODate = rw("DateTimePosted")
                ValidRecords = rw("Quantity")
                RecordsReceived = rw("Quantity")
                CardForPrinting = rw("Quantity")

                JVConfirmedDate = PODate

                CreateSummaryOfCreatedPrintOrderReport_and_CreateSummaryOfInitialPrintAndReprintRequest(PurchaseOrder, RequestedBy, PODate, RecordsReceived, "0", ValidRecords, "0", CardForPrinting, Description, JVConfirmedDate, JVConfirmedBy, AbsolutePath, UserID, UserCompleteName, lblStatus)
            Next
        End If

        DAL.Dispose()
        DAL = Nothing

        'textfile
        GenerateReportE(PurchaseOrder, AbsolutePath, UserID, UserCompleteName, lblStatus)
        GenerateReportG(PurchaseOrder, AbsolutePath, UserID, UserCompleteName, lblStatus)

        Dim _PO As New PurchaseOrder
        _PO.GenerateDR2(POID, SharedFunction.ReportsRepository & "\" & PurchaseOrder, UserID, UserCompleteName, GetCurrentPage(AbsolutePath), New System.Text.StringBuilder, New Integer)
        _PO = Nothing
    End Sub

    Public Shared Sub CreateSummaryOfCreatedPrintOrderReport_and_CreateSummaryOfInitialPrintAndReprintRequest(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Description As String, ByVal JVConfirmedDate As String, ByVal JVConfirmedBy As String, _
                                                                                                                ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String, ByRef lblStatus As DevExpress.Web.ASPxEditors.ASPxLabel)
        CreateSummaryOfCreatedPrintOrderReport(PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, "PO Created", Description, AbsolutePath, UserID, UserCompleteName, lblStatus)
        CreateSummaryOfInitialPrintAndReprintRequest(PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, "Card Production", Description, JVConfirmedDate, JVConfirmedBy, AbsolutePath, UserID, UserCompleteName, lblStatus)
    End Sub

    Public Shared Sub CreateSummaryOfCreatedPrintOrderReport(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Status As String, ByVal Description As String, _
                                                       ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String, _
                                                       ByRef lblStatus As DevExpress.Web.ASPxEditors.ASPxLabel)
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        If rg.GenerateReport(DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, 1, outputFile, PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, Status, Description) Then
            If IO.File.Exists(outputFile) Then
                DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate SummaryOfCreatedPrintOrderReport report for PurchaseOrder {1}", UserCompleteName, PurchaseOrder), GetCurrentPage(AbsolutePath), UserID)
                File.Delete(outputFile)
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find SummaryOfCreatedPrintOrderReport report", GetCurrentPage(AbsolutePath), UserID)
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        Else
            lblStatus.Text = "Unable to generate report"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            WriteToErrorLog(rg.ErrorMessage, AbsolutePath, UserID)

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Public Shared Sub CreateSummaryOfInitialPrintAndReprintRequest(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Status As String, ByVal Description As String, ByVal JVConfirmedDate As String, ByVal JVConfirmedBy As String, _
                                                             ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String, _
                                                             ByRef lblStatus As DevExpress.Web.ASPxEditors.ASPxLabel)
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        If rg.GenerateReport(DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest, 1, outputFile, PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, Status, Description, JVConfirmedDate, JVConfirmedBy) Then
            If IO.File.Exists(outputFile) Then
                DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate SummaryOfInitialPrintAndReprintRequest report for PurchaseOrder {1}", UserCompleteName, PurchaseOrder), GetCurrentPage(AbsolutePath), UserID)
                File.Delete(outputFile)
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find SummaryOfInitialPrintAndReprintRequest report", GetCurrentPage(AbsolutePath), UserID)
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        Else
            lblStatus.Text = "Unable to generate report"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            WriteToErrorLog(rg.ErrorMessage, AbsolutePath, UserID)

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Public Shared Sub GenerateReportE(ByVal PurchaseOrder As String, ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String, _
                                ByRef lblStatus As DevExpress.Web.ASPxEditors.ASPxLabel)
        Dim outputFile As String = ""
        Dim cr As New CMS_Report
        Dim DAL As New DAL
        Try
            If cr.GenerateReportEorReportG(PurchaseOrder, "ElectronicReportOfDeliveredCardsPerPrintOrder", outputFile) Then
                If IO.File.Exists(outputFile) Then
                    DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.ElectronicReportOfDeliveredCardsPerPrintOrder, IO.File.ReadAllBytes(outputFile))
                    DAL.AddSystemLog(String.Format("{0} generate ElectronicReportOfDeliveredCardsPerPrintOrder report for PurchaseOrder {1}", UserCompleteName, PurchaseOrder), GetCurrentPage(AbsolutePath), UserID)
                Else
                    lblStatus.Text = "Unable to find ElectronicReportOfDeliveredCardsPerPrintOrder report"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    DAL.AddErrorLog("Unable to find ElectronicReportOfDeliveredCardsPerPrintOrder report", GetCurrentPage(AbsolutePath), UserID)
                End If
            Else
                lblStatus.Text = "Unable to ElectronicReportOfDeliveredCardsPerPrintOrder generate report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                WriteToErrorLog(cr.ErrorMessage, AbsolutePath, UserID)
            End If
        Catch ex As Exception
        Finally
            cr = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Sub

    Public Shared Sub GenerateReportG(ByVal PurchaseOrder As String, ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String, _
                                ByRef lblStatus As DevExpress.Web.ASPxEditors.ASPxLabel)

        Dim outputFile As String = ""
        Dim cr As New CMS_Report
        Dim DAL As New DAL
        Try
            If cr.GenerateReportEorReportG(PurchaseOrder, "ElectronicReportOfGoodCards", outputFile) Then
                If IO.File.Exists(outputFile) Then
                    DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.ElectronicReportOfGoodCards, IO.File.ReadAllBytes(outputFile))
                    DAL.AddSystemLog(String.Format("{0} generate ElectronicReportOfGoodCards report for PurchaseOrder {1}", UserCompleteName, PurchaseOrder), SharedFunction.GetCurrentPage(AbsolutePath), UserID)
                Else
                    lblStatus.Text = "Unable to find ElectronicReportOfGoodCards report"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    DAL.AddErrorLog("Unable to find ElectronicReportOfGoodCards report", GetCurrentPage(AbsolutePath), UserID)
                End If
            Else
                lblStatus.Text = "Unable to ElectronicReportOfGoodCards generate report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                WriteToErrorLog(cr.ErrorMessage, AbsolutePath, UserID)
            End If
        Catch ex As Exception
        Finally
            cr = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Sub

    Public Shared Function GenerateCertificateOfDeletion_PurchaseOrder(ByVal intPOID As Integer, ByVal strPurchaseOrder As String, ByRef intPOReportID As Integer, _
                                                                 ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String) As Boolean
        Dim DAL As New DAL
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If rg.GenerateReport(DataKeysEnum.Report.CertificateOfDeletion, 1, outputFile, intPOID, strPurchaseOrder) Then
                If DAL.AddRelPOReport(intPOID, strPurchaseOrder, DataKeysEnum.Report.CertificateOfDeletion, IO.File.ReadAllBytes(outputFile)) Then
                    If DAL.TableResult.DefaultView.Count > 0 Then
                        intPOReportID = DAL.TableResult.Rows(0)("POReportID")

                        DAL.AddSystemLog(String.Format("{0} generated certification of deletion report for Purchase Order {1}", UserCompleteName, strPurchaseOrder, rg.ErrorMessage), GetCurrentPage(AbsolutePath), UserID)
                    Else
                        DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}", UserCompleteName, strPurchaseOrder, rg.ErrorMessage), GetCurrentPage(AbsolutePath), UserID)
                        Return False
                    End If
                End If
            Else
                DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}. Returned error {2}", UserCompleteName, strPurchaseOrder, rg.ErrorMessage), GetCurrentPage(AbsolutePath), UserID)
                Return False
            End If

            Return True
        Catch ex As Exception
            DAL.AddErrorLog(String.Format("GenerateCertificateOfDeletion(): Runtime error " & ex.Message, UserCompleteName, strPurchaseOrder, rg.ErrorMessage), GetCurrentPage(AbsolutePath), UserID)
            Return False
        Finally
            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Function GenerateCertificateOfDeletion_Manual(ByVal param As String) As Boolean
        Dim DAL As New DAL
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If Not rg.GenerateReport(DataKeysEnum.Report.CertificateOfDeletion_v3_manual, 1, outputFile, param.Split("|")(0).Trim, param.Split("|")(1).Trim, param.Split("|")(2).Trim, param.Split("|")(3).Trim) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            Return False
        Finally
            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Function GenerateCertificateOfDeletion_PurchaseOrder2(ByVal intPOID As Integer, ByVal strPurchaseOrder As String, ByRef intPOReportID As Integer, _
                                                                 ByVal AbsolutePath As String, ByVal UserID As Integer, ByVal UserCompleteName As String) As Boolean
        Dim DAL As New DAL
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If rg.GenerateReport(DataKeysEnum.Report.CertificateOfDeletion, 1, outputFile, intPOID, strPurchaseOrder) Then
                DAL.UpdateRelPOReportByPOAndReportTypeID(strPurchaseOrder, DataKeysEnum.Report.CertificateOfDeletion, IO.File.ReadAllBytes(outputFile))
            Else
                DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}. Returned error {2}", UserCompleteName, strPurchaseOrder, rg.ErrorMessage), GetCurrentPage(AbsolutePath), UserID)
                Return False
            End If

            Return True
        Catch ex As Exception
            DAL.AddErrorLog(String.Format("GenerateCertificateOfDeletion(): Runtime error " & ex.Message, UserCompleteName, strPurchaseOrder, rg.ErrorMessage), GetCurrentPage(AbsolutePath), UserID)
            Return False
        Finally
            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Sub WriteToErrorLog(ByVal strError As String, ByVal AbsolutePath As String, ByVal UserID As Integer)
        Dim DAL As New DAL
        DAL.AddErrorLog(strError, GetCurrentPage(AbsolutePath), UserID)
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Public Shared Function ExportToExcel(ByVal dt As DataTable, ByVal excelFile As String, ByVal sheetName As String) As Integer
        Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelFile & ";Extended Properties=Excel 12.0 Xml;"
        Dim rNumb As Integer = 0
        Try
            Using con As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection(connString)
                con.Open()
                Dim strField As StringBuilder = New StringBuilder()
                For i As Integer = 0 To dt.Columns.Count - 1
                    strField.Append("[" & dt.Columns(i).ColumnName & "],")
                Next

                strField = strField.Remove(strField.Length - 1, 1)
                Dim sqlCmd = "CREATE TABLE [" & sheetName & "] (" + strField.ToString().Replace("]", "] text") & ")"
                Dim cmd As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand(sqlCmd, con)
                cmd.ExecuteNonQuery()

                'details
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim strValue As StringBuilder = New StringBuilder()
                    For j As Integer = 0 To dt.Columns.Count - 1
                        strValue.Append("'" & dt.Rows(i)(j).ToString() & "',")
                    Next

                    strValue = strValue.Remove(strValue.Length - 1, 1)
                    cmd.CommandText = "INSERT INTO [" & sheetName & "] (" + strField.ToString() & ") VALUES (" + strValue.ToString() & ")"
                    cmd.ExecuteNonQuery()
                    rNumb = i + 1
                Next

                con.Close()
            End Using

            Return rNumb
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Shared Sub ViewDownloadFile(ByVal strFile As String)
        HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
        Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
        HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
        Dim Dfile As New System.IO.FileInfo(strFile)
        HttpContext.Current.Response.WriteFile(Dfile.FullName)
        HttpContext.Current.Response.[End]()
    End Sub

    Public Shared Function IsCubaoData(ByVal Barcode As String) As Boolean
        'If Barcode.Trim = "0120180912C1ID012033" Then
        '    Console.Write("TEST")
        'End If

        If Barcode.Substring(10, 2) = "C1" Then
            Return True
        Else
            Return False
        End If
    End Function

    'Public Shared Function ResponseFile_FileNaming(ByVal BatchID As Integer, ByVal GSUFileName As String, Optional ByVal FilterByID As Boolean = True) As String
    Public Shared Function ResponseFile_FileNaming(ByVal PurchaseOrder As String, ByVal GSUFileName As String, Optional ByVal IsBatchComplete As Boolean = True) As String
        Dim destiFolder As String = String.Format("{0}\{1}", SharedFunction.ReportsUBPRFRepository, PurchaseOrder)
        If Not Directory.Exists(destiFolder) Then Directory.CreateDirectory(destiFolder)

        Dim destiFolder_FilesCntr As Integer = Directory.GetFiles(destiFolder).Length
        Dim filePadder As String = "000"

        If Not IsBatchComplete Then filePadder = filePadder = (destiFolder_FilesCntr + 1).ToString.PadLeft(3, "0")

        'Dim DAL As New DAL
        'DAL.CheckIfBatchIsComplete(BatchID, FilterByID)
        'If CInt(DAL.ObjectResult) > 0 Then filePadder = (destiFolder_FilesCntr + 1).ToString.PadLeft(3, "0")
        'DAL.Dispose()
        'DAL = Nothing

        'Return String.Format("{0}\{1}_{2}", destiFolder, Path.GetFileNameWithoutExtension(GSUFileName), filePadder)
        Dim arrGSUFile() As String = Path.GetFileNameWithoutExtension(GSUFileName).Split("_")
        Return String.Format("{0}\CDFS{1}_{2}_{3}_{4}.txt", destiFolder, Microsoft.VisualBasic.Right(arrGSUFile(0), 3), arrGSUFile(1), arrGSUFile(2), filePadder)
    End Function


End Structure


Imports System.IO

Public Class PurchaseOrder

    Private strPurchaseOrder As String
    Private strDirectory As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal strPurchaseOrder As String, ByVal strDirectory As String)
        Me.strPurchaseOrder = strPurchaseOrder
        Me.strDirectory = strDirectory
    End Sub

    Private Sub WriteStatus2(ByVal status As String)
        IO.File.WriteAllText(String.Format("{0}\status.txt", SharedFunction.MainRepository), status)
    End Sub

    'Public Function SaveToDownloadableFiles_Mailer(ByVal strIDs As String, _
    '                                         ByVal strPurchaseOrders As String, _
    '                                         ByVal strBatchAndQtys As String, _
    '                                         ByVal strType As String, _
    '                                         ByVal intUserID As Integer, _
    '                                         ByVal OutputDirectory As String, _
    '                                         ByRef sb As StringBuilder, ByRef intError As Integer) As Boolean
    '    Dim DAL As New DAL
    '    Try
    '        Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|LastName|FirstName|MiddleName|Sex|Birthday|Address|BackOCR|Photo_Image|Signature_Image|Barcode_Image"

    '        CheckAndCreateDirectory(OutputDirectory)

    '        Dim strDescription As String = strBatchAndQtys

    '        Dim strTextFileMailer As String = String.Format("{0}\{1}_Mailer.txt", OutputDirectory, strPurchaseOrders)
    '        Dim strTextFileLaserShort As String = String.Format("{0}\{1}_LaserShort.txt", OutputDirectory, strPurchaseOrders)
    '        Dim strTextFileLaserLong As String = String.Format("{0}\{1}_LaserLong.txt", OutputDirectory, strPurchaseOrders)

    '        SaveToTextfile(strTextFileMailer, strHeader, sb)

    '        Dim dt As DataTable = Nothing
    '        If DAL.SelectDataForMailer(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
    '            dt = DAL.TableResult
    '        End If

    '        SharedFunction.ReComputePageAndSeries(dt, 0)

    '        For Each rw As DataRow In dt.Rows
    '            Try
    '                Dim strLine As String = String.Format("{0}-{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", _
    '                                     rw("Batch").ToString.Trim, rw("CurrentPage").ToString.Trim, rw("CurrentSeries").ToString.Trim, rw("Barcode").ToString.Trim, _
    '                                     rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, _
    '                                     rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, rw("BackOCR").ToString.Trim, _
    '                                     rw("Barcode").ToString.Trim & "_Photo.jpg", _
    '                                     rw("Barcode").ToString.Trim & "_Signature.tiff", _
    '                                     rw("Barcode").ToString.Trim & ".jpg")

    '                SaveToTextfile(strTextFileMailer, strLine, sb)

    '                If rw("FName").ToString.Trim.Length > 25 Then
    '                    SaveToTextfile(strTextFileLaserLong, LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, rw("CurrentSeries").ToString), sb)
    '                Else
    '                    SaveToTextfile(strTextFileLaserShort, LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, rw("CurrentSeries").ToString), sb)
    '                End If
    '            Catch ex As Exception
    '                intError += 1
    '                sb.AppendLine(String.Format("Runtime error encountered in textfile saving for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
    '            End Try
    '        Next

    '        'add to downloadable the purchaseorder - mailer.txt
    '        If DAL.AddDownloadableFiles(strDescription, strTextFileMailer, intUserID, String.Format("{0} - Mailer", strType), strIDs) Then
    '        End If

    '        'add to downloadable the purchaseorder - laser.txt
    '        If DAL.AddDownloadableFiles(strDescription, strTextFileLaserShort, intUserID, String.Format("{0} - Laser", strType), strIDs) Then
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        intError += 1
    '        sb.AppendLine(String.Format("SaveToDownloadableFiles_Mailer(): Runtime error {0}", ex.Message))
    '        Return False
    '    Finally
    '        DAL.Dispose()
    '        DAL = Nothing
    '    End Try
    'End Function

    Public Function SaveToDownloadableFiles_ForIndigo(ByVal strIDs As String, _
                                             ByVal strPurchaseOrders As String, _
                                             ByVal strBatchAndQtys As String, _
                                             ByVal strType As String, _
                                             ByVal intUserID As Integer, _
                                             ByVal intPrintable As Integer, _
                                             ByVal OutputDirectory As String, _
                                             ByVal CardIDs As String, _
                                             ByRef sb As StringBuilder, ByRef intError As Integer) As Boolean
        Dim DAL As New DAL
        Dim fc As New FileCompression
        Try
            Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|BackOCR|Photo_Image|Barcode_Image|Signature_Image"
            Dim strFolder As String = "Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strDescription As String = strBatchAndQtys

            'Dim strTextFile As String = String.Format("{0}\{1}.txt", strDirectory, strDescription)
            Dim strTextFile As String = String.Format("{0}\{1}.txt", strDirectory, strFolder.Replace("Temp", "DFID"))

            SaveToTextfile(strTextFile, strHeader, sb)

            Dim dt As DataTable
            If DAL.SelectDataForIndigoPrinting(String.Format(" WHERE dbo.tblRelPOData.CardID IN ({0})", CardIDs)) Then
                dt = DAL.TableResult
            Else
                sb.AppendLine("SelectDataForIndigoPrinting(): Returned error " & DAL.ErrorMessage)
            End If

            SharedFunction.ReComputePageAndSeries(dt, 0)

            'WriteStatus(dt.DefaultView.Count)
            For Each rw As DataRow In dt.Rows
                Try
                    If rw("CurrentSeries") > intPrintable Then
                        'drop cards are not included if for indigo
                    Else
                        Dim strLine As String = String.Format(IIf(IsDBNull(rw("DateTimeExtracted")), "", "R") & _
                                             "{0}-{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", _
                                             rw("Batch").ToString.Trim, rw("CurrentPage").ToString.Trim, rw("CurrentSeries").ToString.Trim, rw("Barcode").ToString.Trim, _
                                             rw("CRN").ToString.Trim, rw("BackOCR").ToString.Trim, _
                                             rw("Barcode").ToString.Trim & "_Photo.jpg", _
                                             rw("Barcode").ToString.Trim & ".jpg", _
                                             rw("Barcode").ToString.Trim & "_Signature.jpg")

                        SaveToTextfile(strTextFile, strLine, sb)
                        'If intPrintable = 100000 Then SaveToTextfile(strTextFile.Replace(".txt", "_laserprinter.txt"), LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim), sb)

                        CheckAndCreateDirectory(strImagesDirectory)

                        If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                        If Not IsDBNull(rw("Signature")) Then SharedFunction.TIFFtoJPG(rw("Signature"), String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & "_Signature.jpg"))

                        ''-------
                        'Dim bg As New BarcodeWeb
                        'Dim bln As Boolean
                        'For i As Short = 1 To 3
                        '    bln = bg.GenerateBarcode(rw("Barcode").ToString.Trim, String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                        '    If bln Then Exit For
                        'Next
                        'If Not bln Then DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                        'bg = Nothing
                        ''-------

                        ''-------
                        If File.Exists(String.Format("{0}\{1}", SharedFunction.BarcodeRepository, rw("Barcode").ToString.Trim & ".jpg")) Then
                            File.Copy(String.Format("{0}\{1}", SharedFunction.BarcodeRepository, rw("Barcode").ToString.Trim & ".jpg"), String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("Barcode generation. Unable to find Barcode {0} in barcode repository", rw("Barcode").ToString.Trim))
                        End If
                        ''-------
                    End If
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            ''add to downloadable the purchaseorder.zip
            'If DAL.AddDownloadableFiles(strDescription, "", intUserID, strType, strIDs) Then
            '    Dim strZipFile As String
            '    If fc.Compress(strDirectory, strZipFile) Then
            '        Using _zip = New ICSharpCode.SharpZipLib.Zip.ZipFile(strZipFile)
            '            _zip.UseZip64 = ICSharpCode.SharpZipLib.Zip.UseZip64.On
            '            _zip.BeginUpdate()

            '            fc.addFolderToZip(_zip, strDirectory, strImagesDirectory)

            '            _zip.CommitUpdate()
            '            _zip.Close()
            '        End Using

            '        Dim strNewFile As String = ""

            '        strNewFile = String.Format("{0}\{1}", strZipFile.Substring(0, strZipFile.LastIndexOf("\")), "DFID" & DAL.ObjectResult.ToString.Trim & ".zip")

            '        If File.Exists(strNewFile) Then File.Delete(strNewFile)

            '        FileSystem.Rename(strZipFile, strNewFile)

            '        If Not DAL.ExecuteQuery(String.Format("UPDATE tblDownloadableFiles SET FilePath='{0}' WHERE DownloadFileID={1}", strNewFile, DAL.ObjectResult)) Then
            '            intError += 1
            '            sb.AppendLine(String.Format("Failed to update FilePath. Returned error {0}", DAL.ErrorMessage))
            '        End If

            '        'delete physical PO folder in repository
            '        For Each _PO As String In strPurchaseOrders.Split(",")
            '            SharedFunction.DeletePO_ForUploadingRepository(_PO)
            '        Next

            '        Directory.Delete(strDirectory, True)
            '    Else
            '        intError += 1
            '        sb.AppendLine(String.Format("Compress(): Returned error {0}", fc.ErrorMessage))
            '    End If
            'Else
            '    intError += 1
            '    sb.AppendLine(String.Format("AddDownloadableFiles(): Returned error {0}", DAL.ErrorMessage))
            'End If

            ''commented by edel 2016-06-23. delete source on delivery date
            ''delete physical PO folder in repository
            'For Each _PO As String In strPurchaseOrders.Split(",")
            '    SharedFunction.DeletePO_ForUploadingRepository(_PO)
            'Next

            'new 04/07/2016
            Dim strNewFolder As String = strDirectory.Replace(strFolder, "DFID") & "_" & Now.ToString("yyyyMMdd_hhmm")

            If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

            FileSystem.Rename(strDirectory, strNewFolder)

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigo(): Runtime error {0}", ex.Message))
            Return False
        Finally
            fc = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Function SaveToDownloadableFiles_ForIndigov2(ByVal strIDs As String,
                                             ByVal strPurchaseOrders As String,
                                             ByVal strBatchAndQtys As String,
                                             ByVal strType As String,
                                             ByVal intUserID As Integer,
                                             ByVal intPrintable As Integer,
                                             ByVal OutputDirectory As String,
                                             ByVal CardIDs As String,
                                             ByRef sb As StringBuilder, ByRef intError As Integer, ByRef intSignatureError As Integer,
                                             ByRef intBarcodeError As Integer, ByRef intSheetBarcodeError As Integer) As Boolean
        Dim DAL As New DAL
        Dim fc As New FileCompression
        Try
            Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|BackOCR|Photo_Image|Barcode_Image|Signature_Image|SheetBarcode"
            'Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|BackOCR|Photo_Image|Barcode_Image|Signature_Image"
            Dim strFolder As String = "Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strDescription As String = strBatchAndQtys

            Dim strTextFile As String = String.Format("{0}\{1}.txt", strDirectory, strFolder.Replace("Temp", "DFID"))

            SaveToTextfile(strTextFile, strHeader, sb)

            Dim dt As DataTable
            If DAL.SelectDataForIndigoPrinting(String.Format(" WHERE dbo.tblRelPOData.CardID IN ({0})", CardIDs)) Then
                dt = DAL.TableResult
            Else
                sb.AppendLine("SelectDataForIndigoPrinting(): Returned error " & DAL.ErrorMessage)
            End If

            SharedFunction.ReComputePageAndSeries(dt, 0)

            dt.Columns.Add("SheetBarcode", Type.GetType("System.String"))

            GenerateSheetBarcode(intPrintable, dt)

            For Each rw As DataRow In dt.Rows
                Try
                    If rw("CurrentSeries") > intPrintable Then
                        'drop cards are not included if for indigo
                    Else
                        Dim strSheetBarcode As String = rw("SheetBarcode").ToString.Trim

                        Dim strLine As String = String.Format(IIf(IsDBNull(rw("DateTimeExtracted")), "", "R") &
                                             "{0}-{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                                             rw("Batch").ToString.Trim, rw("CurrentPage").ToString.Trim, rw("CurrentSeries").ToString.Trim, rw("Barcode").ToString.Trim,
                                             rw("CRN").ToString.Trim, rw("BackOCR").ToString.Trim,
                                             rw("Barcode").ToString.Trim & "_Photo.jpg",
                                             rw("Barcode").ToString.Trim & ".jpg",
                                             rw("Barcode").ToString.Trim & "_Signature.jpg",
                                             IIf(strSheetBarcode = "", "", strSheetBarcode & ".jpg"))

                        SaveToTextfile(strTextFile, strLine, sb)

                        CheckAndCreateDirectory(strImagesDirectory)

                        Dim strBarcode As String = rw("Barcode").ToString.Trim
                        'Dim strSourcePath As String = SharedFunction.GetPath(rw("PurchaseOrder").ToString.Trim, IIf(IsDBNull(rw("POSubFolder")), "", rw("POSubFolder").ToString.Trim), strBarcode)
                        Dim strSourcePath As String = SharedFunction.PSBImagesRepository
                        Dim strSourceFilePhoto As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Photo.jpg").Replace("\\", "\")
                        'Dim strSourceFileSignatureTIFF As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Signature.tiff")
                        Dim strSourceFileSignatureJPG As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Signature.jpg").Replace("\\", "\")

                        If strSourcePath <> "Error" Then
                            If File.Exists(strSourceFilePhoto) Then
                                SharedFunction.FileCopyImage(strSourceFilePhoto, strImagesDirectory)
                            Else
                                intError += 1
                                sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov2(): Unable to find photo for {0} in pbs image", rw("Barcode").ToString.Trim))
                            End If

                            If File.Exists(strSourceFileSignatureJPG) Then
                                SharedFunction.FileCopyImage(strSourceFileSignatureJPG, strImagesDirectory)
                            Else
                                intError += 1
                                intSignatureError += 1
                                sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov2(): Unable to find signature for {0} in pbs image", rw("Barcode").ToString.Trim))
                            End If
                        End If

                        '-------
                        Dim bg As New BarcodeWeb
                        Dim bln As Boolean

                        'barcode
                        For i As Short = 1 To 3
                            bln = bg.GenerateBarcode(rw("Barcode").ToString.Trim, String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                            If bln Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                        If Not bln Then
                            intError += 1
                            intBarcodeError += 1
                            DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                        End If

                        'sheet barcode
                        Dim bg2 As New BarcodeWeb2
                        If strSheetBarcode <> "" Then
                            For i As Short = 1 To 3
                                bln = bg2.GenerateBarcode(strSheetBarcode, String.Format("{0}\{1}", strImagesDirectory, strSheetBarcode & ".jpg"))
                                If bln Then Exit For
                                System.Threading.Thread.Sleep(3000)
                            Next
                            If Not bln Then
                                intError += 1
                                intSheetBarcodeError += 1
                                DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                            End If
                        End If

                        bg = Nothing
                        bg2 = Nothing
                        '-------

                        ' ''-------
                        'If File.Exists(String.Format("{0}\{1}", SharedFunction.BarcodeRepository, rw("Barcode").ToString.Trim & ".jpg")) Then
                        '    File.Copy(String.Format("{0}\{1}", SharedFunction.BarcodeRepository, rw("Barcode").ToString.Trim & ".jpg"), String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                        'Else
                        '    intError += 1
                        '    sb.AppendLine(String.Format("Barcode generation. Unable to find Barcode {0} in barcode repository", rw("Barcode").ToString.Trim))
                        'End If
                        ' ''-------
                    End If
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            'new 04/07/2016
            Dim strNewFolder As String = strDirectory.Replace(strFolder, "DFID") & "_" & Now.ToString("yyyyMMdd_hhmm")

            If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

            FileSystem.Rename(strDirectory, strNewFolder)

            Dim dtDistinctPO As DataTable = dt.DefaultView.ToTable(True, "PurchaseOrder")
            For Each rw As DataRow In dtDistinctPO.Rows
                Try
                    Dim encryptedFile As String = String.Format("{0}\{1}_zip.sss", SharedFunction.ForUploadingRepository, rw("PurchaseOrder").ToString.Trim)
                    Dim ZipFile As String = String.Format("{0}\{1}.zip", SharedFunction.ForUploadingRepository, rw("PurchaseOrder").ToString.Trim)

                    If File.Exists(encryptedFile) Then File.Delete(encryptedFile)
                    If File.Exists(ZipFile) Then File.Delete(ZipFile)
                Catch ex2 As Exception
                End Try
            Next

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigo(): Runtime error {0}", ex.Message))
            Return False
        Finally
            fc = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Function SaveToDownloadableFiles_ForIndigov3(ByVal dt As DataTable,
                                                        ByVal intUserID As Integer, ByVal OutputDirectory As String,
                                                        ByRef sb As StringBuilder, ByRef intError As Integer, ByRef intSignatureError As Integer,
                                                        ByRef intBarcodeError As Integer, ByRef intSheetBarcodeError As Integer,
                                                        ByRef strTempOutputDir As String,
                                                        ByVal IsSSSUBP As Boolean) As Boolean
        Dim DAL As New DAL
        Dim fc As New FileCompression
        Try
            Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|BackOCR|Photo_Image|Barcode_Image|Signature_Image|FrontSheetBarcode|BackSheetBarcode"

            Dim strFolder As String = "Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strTextFile As String = String.Format("{0}\{1}.txt", strDirectory, strFolder.Replace("Temp", "DFID"))

            SaveToTextfile(strTextFile, strHeader, sb)

            For Each rw As DataRow In dt.Rows
                Try
                    Dim strFrontSheetBarcode As String = rw("FrontSheetBarcode").ToString.Trim
                    Dim strBackSheetBarcode As String = rw("BackSheetBarcode").ToString.Trim

                    Dim strLine As String = String.Format(IIf(IsDBNull(rw("DateTimeExtracted")), "", "R") &
                                         "{0}-{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                                         rw("Batch").ToString.Trim, rw("CurrentPage").ToString.Trim, rw("CurrentSeries").ToString.Trim, rw("Barcode").ToString.Trim,
                                         rw("CRN").ToString.Trim, rw("BackOCR").ToString.Trim,
                                         rw("Barcode").ToString.Trim & "_Photo.jpg",
                                         rw("Barcode").ToString.Trim & ".jpg",
                                         rw("Barcode").ToString.Trim & "_Signature.jpg",
                                         IIf(rw("CRN1").ToString.Trim = "", "", strFrontSheetBarcode & ".jpg"),
                                         IIf(rw("CRN1").ToString.Trim = "", "", strBackSheetBarcode & ".jpg"))

                    SaveToTextfile(strTextFile, strLine, sb)

                    CheckAndCreateDirectory(strImagesDirectory)

                    Dim strBarcode As String = rw("Barcode").ToString.Trim
                    Dim strSourcePath As String = SharedFunction.PSBImagesRepository
                    Dim strSourceFilePhoto As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Photo.jpg").Replace("\\", "\")
                    Dim strSourceFileSignatureJPG As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Signature.jpg").Replace("\\", "\")

                    If strSourcePath <> "Error" Then
                        If File.Exists(strSourceFilePhoto) Then
                            SharedFunction.FileCopyImage(strSourceFilePhoto, strImagesDirectory)
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov3(): Unable to find photo for {0} in pbs image", rw("Barcode").ToString.Trim))
                        End If

                        If File.Exists(strSourceFileSignatureJPG) Then
                            SharedFunction.FileCopyImage(strSourceFileSignatureJPG, strImagesDirectory)
                        Else
                            intError += 1
                            intSignatureError += 1
                            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov3(): Unable to find signature for {0} in pbs image", rw("Barcode").ToString.Trim))
                        End If
                    End If

                    '-------
                    Dim bg As New BarcodeWeb
                    Dim bln As Boolean

                    'barcode
                    For i As Short = 1 To 3
                        bln = bg.GenerateBarcode(rw("Barcode").ToString.Trim, String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                        If bln Then Exit For
                        System.Threading.Thread.Sleep(3000)
                    Next
                    If Not bln Then
                        intError += 1
                        intBarcodeError += 1
                        DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                    End If

                    'sheet barcode
                    Dim bg2 As New BarcodeWeb2
                    If strFrontSheetBarcode <> "" Then
                        For i As Short = 1 To 3
                            bln = bg2.GenerateBarcode(strFrontSheetBarcode, String.Format("{0}\{1}", strImagesDirectory, strFrontSheetBarcode & ".jpg"))
                            If bln Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                        If Not bln Then
                            intError += 1
                            intSheetBarcodeError += 1
                            DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                        End If
                    End If
                    If strBackSheetBarcode <> "" Then
                        For i As Short = 1 To 3
                            bln = bg2.GenerateBarcode(strBackSheetBarcode, String.Format("{0}\{1}", strImagesDirectory, strBackSheetBarcode & ".jpg"))
                            If bln Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                        If Not bln Then
                            intError += 1
                            intSheetBarcodeError += 1
                            DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                        End If
                    End If

                    bg = Nothing
                    bg2 = Nothing

                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {3}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            'new 04/07/2016
            Dim strNewFolder As String = strDirectory.Replace(strFolder, "DFID") & "_" & IIf(IsSSSUBP, "SSSUBP_", "") & Now.ToString("yyyyMMdd_hhmm")

            If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

            FileSystem.Rename(strDirectory, strNewFolder)

            strTempOutputDir = strNewFolder

            ''uncomment
            Dim dtDistinctPO As DataTable = dt.DefaultView.ToTable(True, "PurchaseOrder")
            For Each rw As DataRow In dtDistinctPO.Rows
                Try
                    Dim encryptedFile As String = String.Format("{0}\{1}_zip.sss", SharedFunction.ForUploadingRepository, rw("PurchaseOrder").ToString.Trim)
                    Dim ZipFile As String = String.Format("{0}\{1}.zip", SharedFunction.ForUploadingRepository, rw("PurchaseOrder").ToString.Trim)

                    If File.Exists(encryptedFile) Then File.Delete(encryptedFile)
                    If File.Exists(ZipFile) Then File.Delete(ZipFile)
                Catch ex2 As Exception
                End Try
            Next

            'added on 11/25/2019. save copy to network folder
            'Try
            '    If My.Settings.IndigoExtractRepository <> "" Then FileSystem.Rename(strNewFolder, My.Settings.IndigoExtractRepository)
            'Catch ex As Exception
            '    intError += 1
            '    sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigo3(): Failed to copy to " & My.Settings.IndigoExtractRepository & ". Runtime error {0}", ex.Message))
            '    Return False
            'End Try
            If Directory.Exists(strNewFolder) Then
                File.WriteAllText(String.Format("{0}\status", strNewFolder), "1")
            End If

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigo3(): Runtime error {0}", ex.Message))
            Return False
        Finally
            fc = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Function SaveToDownloadableFiles_ForIndigov4(ByVal dt As DataTable,
                                                        ByVal intUserID As Integer, ByVal OutputDirectory As String,
                                                        ByRef sb As StringBuilder, ByRef intError As Integer, ByRef intSignatureError As Integer,
                                                        ByRef intBarcodeError As Integer, ByRef intSheetBarcodeError As Integer,
                                                        ByRef strTempOutputDir As String,
                                                        ByVal IsSSSUBP As Boolean) As Boolean
        Dim DAL As New DAL
        Dim fc As New FileCompression

        Dim dtBackCardImageProfile As DataTable = Nothing
        dtBackCardImageProfile.Columns.Add("Type", GetType(Int16))
        dtBackCardImageProfile.Columns.Add("Path", GetType(String))

        Dim BackCardImageType As Short = 1
        If IsSSSUBP Then BackCardImageType = 2

        For Each strLine As String In File.ReadAllLines(SharedFunction.BackCardImage_File)
            If strLine.Trim <> "" Then
                Dim rw As DataRow = dtBackCardImageProfile.NewRow
                rw(0) = strLine.Split("|")(0).Trim
                rw(1) = strLine.Split("|")(1).Trim
                dtBackCardImageProfile.Rows.Add(rw)
            End If
        Next

        Try
            Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|BackOCR|Photo_Image|Barcode_Image|Signature_Image|FrontSheetBarcode|BackSheetBarcode|BackCardImage"

            Dim strFolder As String = "Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strTextFile As String = String.Format("{0}\{1}.txt", strDirectory, strFolder.Replace("Temp", "DFID"))

            SaveToTextfile(strTextFile, strHeader, sb)

            For Each rw As DataRow In dt.Rows
                Try
                    Dim strFrontSheetBarcode As String = rw("FrontSheetBarcode").ToString.Trim
                    Dim strBackSheetBarcode As String = rw("BackSheetBarcode").ToString.Trim

                    Dim strLine As String = String.Format(IIf(IsDBNull(rw("DateTimeExtracted")), "", "R") &
                                         "{0}-{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                                         rw("Batch").ToString.Trim, rw("CurrentPage").ToString.Trim, rw("CurrentSeries").ToString.Trim, rw("Barcode").ToString.Trim,
                                         rw("CRN").ToString.Trim, rw("BackOCR").ToString.Trim,
                                         rw("Barcode").ToString.Trim & "_Photo.jpg",
                                         rw("Barcode").ToString.Trim & ".jpg",
                                         rw("Barcode").ToString.Trim & "_Signature.jpg",
                                         IIf(rw("CRN1").ToString.Trim = "", "", strFrontSheetBarcode & ".jpg"),
                                         IIf(rw("CRN1").ToString.Trim = "", "", strBackSheetBarcode & ".jpg"),
                                         rw("Barcode").ToString.Trim & "_BackCardImage.jpg")

                    SaveToTextfile(strTextFile, strLine, sb)

                    CheckAndCreateDirectory(strImagesDirectory)

                    Dim strBarcode As String = rw("Barcode").ToString.Trim
                    Dim strSourcePath As String = SharedFunction.PSBImagesRepository
                    Dim strSourceFilePhoto As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Photo.jpg").Replace("\\", "\")
                    Dim strSourceFileSignatureJPG As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Signature.jpg").Replace("\\", "\")

                    Dim strBackCardImage As String = ""
                    If Not dtBackCardImageProfile Is Nothing Then
                        If dtBackCardImageProfile.Select("Type=" & BackCardImageType).Length > 0 Then
                            strBackCardImage = String.Format("{0}\{1}", SharedFunction.SystemRepository, dtBackCardImageProfile.Select("Type=" & BackCardImageType)(0)("Path").ToString.Trim).Replace("\\", "\")
                        End If
                    End If

                    If strSourcePath <> "Error" Then
                        If File.Exists(strSourceFilePhoto) Then
                            SharedFunction.FileCopyImage(strSourceFilePhoto, strImagesDirectory)
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov4(): Unable to find photo for {0} in pbs image", rw("Barcode").ToString.Trim))
                        End If

                        If File.Exists(strSourceFileSignatureJPG) Then
                            SharedFunction.FileCopyImage(strSourceFileSignatureJPG, strImagesDirectory)
                        Else
                            intError += 1
                            intSignatureError += 1
                            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov4(): Unable to find signature for {0} in pbs image", rw("Barcode").ToString.Trim))
                        End If
                    End If

                    If strBackCardImage <> "" Then
                        If File.Exists(strBackCardImage) Then
                            File.Copy(strBackCardImage, String.Format("{0}\{1}_BackCardImage.jpg", strImagesDirectory, strBarcode.Trim), True)
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov4(): Unable to find {0} back card image for {1}", strBackCardImage, rw("Barcode").ToString.Trim))
                        End If
                    End If

                    '-------
                    Dim bg As New BarcodeWeb
                    Dim bln As Boolean

                    'barcode
                    For i As Short = 1 To 3
                        bln = bg.GenerateBarcode(rw("Barcode").ToString.Trim, String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                        If bln Then Exit For
                        System.Threading.Thread.Sleep(3000)
                    Next
                    If Not bln Then
                        intError += 1
                        intBarcodeError += 1
                        DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                    End If

                    'sheet barcode
                    Dim bg2 As New BarcodeWeb2
                    If strFrontSheetBarcode <> "" Then
                        For i As Short = 1 To 3
                            bln = bg2.GenerateBarcode(strFrontSheetBarcode, String.Format("{0}\{1}", strImagesDirectory, strFrontSheetBarcode & ".jpg"))
                            If bln Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                        If Not bln Then
                            intError += 1
                            intSheetBarcodeError += 1
                            DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                        End If
                    End If
                    If strBackSheetBarcode <> "" Then
                        For i As Short = 1 To 3
                            bln = bg2.GenerateBarcode(strBackSheetBarcode, String.Format("{0}\{1}", strImagesDirectory, strBackSheetBarcode & ".jpg"))
                            If bln Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                        If Not bln Then
                            intError += 1
                            intSheetBarcodeError += 1
                            DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", intUserID)
                        End If
                    End If

                    bg = Nothing
                    bg2 = Nothing

                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {3}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            'new 04/07/2016
            Dim strNewFolder As String = strDirectory.Replace(strFolder, "DFID") & "_" & IIf(IsSSSUBP, "SSSUBP_", "") & Now.ToString("yyyyMMdd_hhmm")

            If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

            FileSystem.Rename(strDirectory, strNewFolder)

            strTempOutputDir = strNewFolder

            ''uncomment
            Dim dtDistinctPO As DataTable = dt.DefaultView.ToTable(True, "PurchaseOrder")
            For Each rw As DataRow In dtDistinctPO.Rows
                Try
                    Dim encryptedFile As String = String.Format("{0}\{1}_zip.sss", SharedFunction.ForUploadingRepository, rw("PurchaseOrder").ToString.Trim)
                    Dim ZipFile As String = String.Format("{0}\{1}.zip", SharedFunction.ForUploadingRepository, rw("PurchaseOrder").ToString.Trim)

                    If File.Exists(encryptedFile) Then File.Delete(encryptedFile)
                    If File.Exists(ZipFile) Then File.Delete(ZipFile)
                Catch ex2 As Exception
                End Try
            Next

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForIndigov4(): Runtime error {0}", ex.Message))
            Return False
        Finally
            fc = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Sub GenerateSheetBarcode(ByVal intPrintable As Integer, ByRef dt As DataTable)
        Dim IsGetCRN1 As Boolean = True
        Dim IsGetCRN2 As Boolean = True

        Dim intSeriesCntr As Integer = 1

        Dim strCRN1 As String = ""
        Dim strCRN2 As String = ""

        For Each rw As DataRow In dt.Rows
            If intSeriesCntr = 1 Then
                strCRN1 = rw("CRN").ToString.Trim
                intSeriesCntr += 1
            ElseIf intSeriesCntr = 21 And rw("CurrentSeries") <> intPrintable Then
                strCRN2 = rw("CRN").ToString.Trim
                intSeriesCntr = 1
            ElseIf rw("CurrentSeries") = intPrintable Then
                strCRN2 = rw("CRN").ToString.Trim
                intSeriesCntr = 1
            Else
                intSeriesCntr += 1
            End If

            If strCRN1 <> "" And strCRN2 <> "" Then
                Dim strBarcode As String = strCRN1 & strCRN2
                dt.Select("CRN='" & strCRN1 & "'")(0)("SheetBarcode") = strBarcode.Replace("-", "")
                strCRN1 = ""
                strCRN2 = ""
            End If
        Next
    End Sub

    '12/08/2017
    Private Sub GenerateSheetBarcode_bak(ByVal intPrintable As Integer, ByRef dt As DataTable)
        Dim IsGetCRN1 As Boolean = True
        Dim IsGetCRN2 As Boolean = True

        Dim intSeriesCntr As Integer = 1

        Dim strCRN1 As String = ""
        Dim strCRN2 As String = ""

        For Each rw As DataRow In dt.Rows
            If rw("CurrentSeries") > intPrintable Then
                strCRN2 = rw("CRN").ToString.Trim

                If strCRN1 <> "" And strCRN2 <> "" Then
                    Dim strBarcode As String = strCRN1 & strCRN2
                    dt.Select("CRN='" & strCRN1 & "'")(0)("SheetBarcode") = strBarcode.Replace("-", "")
                    strCRN1 = ""
                    strCRN2 = ""
                End If

                Exit For
            Else
                If intSeriesCntr = 1 Then
                    strCRN1 = rw("CRN").ToString.Trim
                    intSeriesCntr += 1
                ElseIf intSeriesCntr = 21 And rw("CurrentSeries") <> intPrintable Then
                    strCRN2 = rw("CRN").ToString.Trim
                    intSeriesCntr = 1
                ElseIf rw("CurrentSeries") = intPrintable Then
                    strCRN2 = rw("CRN").ToString.Trim
                    intSeriesCntr = 1
                Else
                    intSeriesCntr += 1
                End If

                If strCRN1 <> "" And strCRN2 <> "" Then
                    Dim strBarcode As String = strCRN1 & strCRN2
                    dt.Select("CRN='" & strCRN1 & "'")(0)("SheetBarcode") = strBarcode.Replace("-", "")
                    strCRN1 = ""
                    strCRN2 = ""
                End If
            End If
        Next
    End Sub


    'Private Sub GenerateSheetBarcode(ByVal strFile As String, ByVal ImagesFolder As String)
    'Dim CRN As String = ""

    'Dim intSeries As Short = 1
    'Dim intCntr As Short = 1

    'Dim intSheetBarcodeCntr As Integer = 0

    'Dim strFirstCRN As String = ""
    'Dim strTwentyFirstCRN As String = ""

    'Dim intError As Integer = 0

    'Dim strLines() As String = File.ReadAllLines(strFile)

    'Dim bg As New BarcodeWeb




    '    For Each strLine As String In strLines
    '        Try
    'Dim arr() As String = strLine.Split("|")

    '            If arr(3).ToUpper <> "CRN" Then
    '                CRN = arr(3)

    '                If intSeries = 1 Then
    '                    strFirstCRN = arr(3)
    '                    intSeries += 1
    '                ElseIf intSeries = 21 Then
    '                    strTwentyFirstCRN = arr(3)
    '                    intSeries = 1
    '                Else
    '                    intSeries += 1
    '                End If

    '                If strFirstCRN <> "" And strTwentyFirstCRN <> "" Then
    ''generate barcode
    'Dim strBarcode As String = strFirstCRN.Replace("-", "") & strTwentyFirstCRN.Replace("-", "")
    ''sbGeneratedBarcodes.AppendLine(strBarcode)
    '                    rtbBarcodes.AppendText(strBarcode & ",")

    ''Dim bg As New BarcodeGenerator
    '                    Try
    ''Dim result As String = GenerateBarcode(strBarcode, SHEET_BARCODES_FOLDER)
    'Dim bln As Boolean
    '                        For i As Short = 1 To 3
    '                            bln = bg.GenerateBarcode(strBarcode, String.Format("{0}\{1}", ImagesFolder, strBarcode & ".jpg"))
    '                            If bln Then Exit For
    '                            System.Threading.Thread.Sleep(3000)
    '                        Next
    ''If Not bln Then DAL.AddErrorLog(bg.ErrorMessage, "IndigoExtract", 0)
    '                        If bln Then
    '                            intSheetBarcodeCntr += 1
    '                        Else
    '                            intError += 1
    '                        End If

    ''If result <> "" Then
    ''    intError += 1
    ''    SaveToErrorLog(String.Format("{0}{1}", TimeStamp(), "BarcodeGenerationAndFileRevision(): Returned error " & result))
    ''Else
    ''    intSheetBarcodeCntr += 1
    ''End If
    '                    Catch ex As Exception
    '                        intError += 1
    '                    End Try

    '                    strFirstCRN = ""
    '                    strTwentyFirstCRN = ""
    '                End If

    '                intCntr += 1
    '            End If
    '        Catch ex As Exception
    '            intError += 1
    '        End Try
    '    Next
    '    bg = Nothing

    'Dim intSeriesCntr As Short = 1
    '    For Each strLine As String In File.ReadAllLines(TextBox1.Text)
    '        Try
    'Dim arr() As String = strLine.Split("|")

    '            If rtbConso.Text = "" Then rtbConso.AppendText(strLine & "|SheetBarcode" & vbNewLine)

    '            If arr(0).ToUpper <> "Batch-Page".ToUpper Then
    '                If Not File.Exists(rtbBarcodes.Text.Split(",")(CInt(arr(0).Split("-")(1)) - 1) & ".jpg") Then
    '                    rtb.AppendText(rtbBarcodes.Text.Split(",")(CInt(arr(0).Split("-")(1)) - 1) & vbNewLine)
    '                End If

    '                If intSeriesCntr = 1 Then
    '                    rtbConso.AppendText(strLine & "|" & rtbBarcodes.Text.Split(",")(CInt(arr(0).Split("-")(1)) - 1) & ".jpg" & vbNewLine)
    '                    intSeriesCntr += 1
    '                ElseIf intSeriesCntr = 21 Then
    '                    rtbConso.AppendText(strLine & "|" & vbNewLine)
    '                    intSeriesCntr = 1
    '                Else
    '                    rtbConso.AppendText(strLine & "|" & vbNewLine)
    '                    intSeriesCntr += 1
    '                End If
    '            End If
    '        Catch ex As Exception
    '            intError += 1
    '            SaveToErrorLog(String.Format("{0}{1}", TimeStamp(), "BarcodeGenerationAndFileRevision(2): Runtime error " & ex.Message))
    '        End Try
    '    Next

    '    rtbConso.SaveFile(TextBox1.Text.Replace(".txt", "_revised.txt"), RichTextBoxStreamType.PlainText)
    'End Sub

    Public Function SaveToDownloadableFiles_ForMuhlbauer(ByVal strIDs As String, _
                                             ByVal strPurchaseOrders As String, _
                                             ByVal intUserID As Integer, _
                                             ByVal OutputDirectory As String, _
                                             ByRef sb As StringBuilder, ByRef intError As Integer) As Boolean
        Try
            Dim strFolder As String = strPurchaseOrders '"Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            CheckAndCreateDirectory(strDirectory)

            Dim strDescription As String = strPurchaseOrders 'strBatchAndQtys

            'Dim strTextFile As String = String.Format("{0}\{1}.txt", OutputDirectory, strPurchaseOrders)

            Dim DAL As New DAL
            Dim dt As DataTable = Nothing

            Try
                If DAL.SelectDataForMuhlbauer(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
                    dt = DAL.TableResult
                End If
            Catch ex As Exception
            End Try

            Dim sbXML_CardData As New StringBuilder

            For Each rw As DataRow In dt.Rows
                Try
                    sbXML_CardData.AppendLine(XML_CardData_Format4(rw("Barcode").ToString.Trim))

                    Dim strFile As String = String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & ".xml")
                    If Not IsDBNull(rw("Xml")) Then
                        If ByteToFile(strFile, rw("Xml"), sb) Then
                            Dim strLine As String = ""
                            Dim MemberXML As New MemberXML(strFile)
                            Dim SSSNo As String = ""
                            If Not IsDBNull(rw("SSSNo")) Then SSSNo = rw("SSSNo")
                            If Not MemberXML.ExtractDataForMuhlbauer(strLine, SSSNo) Then
                                'File.WriteAllText(strFile.Replace(".xml", ".txt"), strLine)
                                'Else
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML.ExtractDataForMuhlbauer(): Unable to extract data of Xml field of Barcode {0}", rw("Barcode")))
                            End If
                            MemberXML = Nothing

                            'File.Delete(strFile)
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("ByteToFile(): Unable to convert to file from byte Xml field of Barcode {0}", rw("Barcode")))
                        End If
                    End If

                    If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                    If Not IsDBNull(rw("Signature")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Signature.tiff"), rw("Signature"), sb)
                    'If Not IsDBNull(rw("PersoXml")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_perso.xml"), rw("PersoXml"), sb)
                    If Not IsDBNull(rw("LprimaryANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Lprimary.ansi-fmr"), rw("LprimaryANSI378"), sb)
                    If Not IsDBNull(rw("LbackupANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Lbackup.ansi-fmr"), rw("LbackupANSI378"), sb)
                    If Not IsDBNull(rw("RprimaryANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Rprimary.ansi-fmr"), rw("RprimaryANSI378"), sb)
                    If Not IsDBNull(rw("RbackupANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Rbackup.ansi-fmr"), rw("RbackupANSI378"), sb)
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            Dim sbConsolidatedXML As New StringBuilder
            Dim UMIDCoding_PO_Job As String = File.ReadAllText(SharedFunction.UMIDCoding_PO_Job)
            sbConsolidatedXML.Append(UMIDCoding_PO_Job.Replace("@@PO_NAME@@", strPurchaseOrders).Replace("@@DATA@@", sbXML_CardData.ToString))

            IO.File.WriteAllText(String.Format("{0}\{1}", strDirectory, Path.GetFileName(SharedFunction.UMIDCoding_PO_Job).Replace("UMIDCoding_PO_Job", strPurchaseOrders)), sbConsolidatedXML.ToString)

            'add to downloadable the purchaseorder.zip
            'If DAL.AddDownloadableFiles(strDescription, "", intUserID, String.Format("{0} - Muhlbauer", strType), strIDs) Then
            '    Dim fc As New FileCompression
            '    Dim strZipFile As String
            '    If fc.Compress(strDirectory, strZipFile) Then
            '        Using _zip = New ICSharpCode.SharpZipLib.Zip.ZipFile(strZipFile)
            '            _zip.UseZip64 = ICSharpCode.SharpZipLib.Zip.UseZip64.On
            '            _zip.BeginUpdate()

            '            For Each rw As DataRow In dt.Rows
            '                Try
            '                    fc.addFolderToZip(_zip, strDirectory, strDirectory & "\" & rw("Barcode").ToString.Trim)
            '                Catch ex As Exception
            '                End Try
            '            Next

            '            _zip.CommitUpdate()
            '            _zip.Close()
            '        End Using

            '        Dim strNewFile As String = String.Format("{0}\{1}", strZipFile.Substring(0, strZipFile.LastIndexOf("\")), strPurchaseOrders & "_Muhlbauer.zip")

            '        FileSystem.Rename(strZipFile, strNewFile)

            '        If Not DAL.ExecuteQuery(String.Format("UPDATE tblDownloadableFiles SET FilePath='{0}' WHERE DownloadFileID={1}", strNewFile, DAL.ObjectResult)) Then
            '            intError += 1
            '            sb.AppendLine(String.Format("Failed to update FilePath. Returned error {0}", DAL.ErrorMessage))
            '        End If
            '    Else
            '        intError += 1
            '        sb.AppendLine(String.Format("Compress(): Returned error {0}", fc.ErrorMessage))
            '    End If
            '    fc = Nothing
            'Else
            '    intError += 1
            '    sb.AppendLine(String.Format("AddDownloadableFiles(): Returned error {0}", DAL.ErrorMessage))
            'End If

            DAL.Dispose()
            DAL = Nothing

            'new 04/07/2016
            'Dim strNewFolder As String = strDirectory.Replace(strFolder, strPurchaseOrders) & "_Muhlbauer"
            'If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

            'FileSystem.Rename(strDirectory, strNewFolder)

            'Directory.Delete(strDirectory, True)

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForMuhlbauer(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Public Function SaveToDownloadableFiles_ForMuhlbauerv2(ByVal strIDs As String,
                                             ByVal strPurchaseOrders As String,
                                             ByVal intUserID As Integer,
                                             ByVal OutputDirectory As String,
                                             ByRef sb As StringBuilder, ByRef intError As Integer,
                                             ByVal IsByPO As Boolean) As Boolean
        Try
            Dim strFolder As String = strPurchaseOrders
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            CheckAndCreateDirectory(strDirectory)

            Dim strDescription As String = strPurchaseOrders

            Dim DAL As New DAL
            Dim dt As DataTable = Nothing

            Dim dt_CubaoBranch As DataTable = Nothing

            Try
                Dim strQuery1 As String = String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)
                If Not IsByPO Then strQuery1 = strIDs
                If DAL.SelectDataForMuhlbauer(strQuery1) Then
                    dt = DAL.TableResult
                End If

                Dim strQuery2 As String = String.Format(" AND dbo.tblPO.POID IN ({0})", strIDs)
                If Not IsByPO Then strQuery2 = strIDs.Replace("WHERE ", "AND ")
                If DAL.SelectCubaoBranchList(strQuery2) Then
                    dt_CubaoBranch = DAL.TableResult
                End If
            Catch ex As Exception
            End Try

            Dim sbXML_CardData As New StringBuilder
            Dim sbXML_CardData_CubaoBranch As New StringBuilder

            Dim IsWithUBPData As Boolean = False
            If Not dt_CubaoBranch Is Nothing Then If dt_CubaoBranch.DefaultView.Count > 0 Then IsWithUBPData = True

            For Each rw As DataRow In dt.Rows
                Try
                    Dim _crn As String = rw("CRN").ToString.Trim
                    Dim _sssNo As String = rw("SSSNo").ToString.Trim
                    If _sssNo = "" Then _crn = ""

                    If IsWithUBPData Then
                        If dt_CubaoBranch.Select("Barcode='" & rw("Barcode").ToString.Trim & "'").Length > 0 Then _
                            sbXML_CardData_CubaoBranch.AppendLine(XML_CardData_Format4(rw("Barcode").ToString.Trim, _crn))
                    End If

                    sbXML_CardData.AppendLine(XML_CardData_Format4(rw("Barcode").ToString.Trim, _crn))

                    'If dt_CubaoBranch Is Nothing Then
                    '    sbXML_CardData_CubaoBranch.AppendLine(XML_CardData_Format4(rw("Barcode").ToString.Trim))
                    'Else
                    '    If dt_CubaoBranch.DefaultView.Count = 0 Then
                    '        sbXML_CardData_CubaoBranch.AppendLine(XML_CardData_Format4(rw("Barcode").ToString.Trim))
                    '    Else
                    '        If dt_CubaoBranch.Select("Barcode='" & rw("Barcode").ToString.Trim & "'").Length = 0 Then
                    '            sbXML_CardData_CubaoBranch.AppendLine(XML_CardData_Format4(rw("Barcode").ToString.Trim))
                    '        End If
                    '    End If
                    'End If

                    Dim strFile As String = String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & ".xml")
                    If Not IsDBNull(rw("Xml")) Then
                        If ByteToFile(strFile, rw("Xml"), sb) Then
                            Dim strLine As String = ""
                            Dim MemberXML As New MemberXML(strFile)
                            Dim SSSNo As String = ""
                            If Not IsDBNull(rw("SSSNo")) Then SSSNo = rw("SSSNo")
                            If Not MemberXML.ExtractDataForMuhlbauer(strLine, SSSNo) Then
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML.ExtractDataForMuhlbauer(): Unable to extract data of Xml field of Barcode {0}", rw("Barcode")))
                            End If

                            'Dim sbRevisedXML As New StringBuilder
                            'sbRevisedXML.Append("<?xml version=""1.0"" encoding=""UTF-8""?>")
                            'sbRevisedXML.Append(File.ReadAllText(strFile))
                            'File.WriteAllText(strFile, sbRevisedXML.ToString)
                            'sbRevisedXML = Nothing

                            MemberXML = Nothing
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("ByteToFile(): Unable to convert to file from byte Xml field of Barcode {0}", rw("Barcode")))
                        End If

                        If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                        If Not IsDBNull(rw("Signature")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Signature.tiff"), rw("Signature"), sb)
                        If Not IsDBNull(rw("LprimaryANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Lprimary.ansi-fmr"), rw("LprimaryANSI378"), sb)
                        If Not IsDBNull(rw("LbackupANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Lbackup.ansi-fmr"), rw("LbackupANSI378"), sb)
                        If Not IsDBNull(rw("RprimaryANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Rprimary.ansi-fmr"), rw("RprimaryANSI378"), sb)
                        If Not IsDBNull(rw("RbackupANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Rbackup.ansi-fmr"), rw("RbackupANSI378"), sb)
                    Else
                        Dim strPO As String = rw("PurchaseOrder").ToString.Trim
                        Dim strBarcode As String = rw("Barcode").ToString.Trim
                        Dim strPOFolder As String = String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strPO)

                        If Directory.Exists(strPOFolder) Then
                            Dim strSourcePath As String = SharedFunction.GetPath(strPO, IIf(IsDBNull(rw("POSubFolder")), "", rw("POSubFolder").ToString.Trim), strBarcode)
                            If strSourcePath <> "Error" Then
                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & ".xml"), strDirectory & "\" & strBarcode.Trim)

                                Dim strLine As String = ""
                                Dim MemberXML As New MemberXML(String.Format("{0}\{1}", SharedFunction.CheckAndCreateDirectory(strDirectory & "\" & strBarcode.Trim), Path.GetFileName(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & ".xml"))))
                                Dim SSSNo As String = ""
                                If Not IsDBNull(rw("SSSNo")) Then SSSNo = rw("SSSNo")
                                If Not MemberXML.ExtractDataForMuhlbauer(strLine, SSSNo) Then
                                    intError += 1
                                    sb.AppendLine(String.Format("MemberXML.ExtractDataForMuhlbauer(): Unable to extract data of Xml field of Barcode {0}", strBarcode.Trim))
                                End If
                                MemberXML = Nothing

                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Photo.jpg"), strDirectory & "\" & strBarcode.Trim)
                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Signature.tiff"), strDirectory & "\" & strBarcode.Trim)
                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Lprimary.ansi-fmr"), strDirectory & "\" & strBarcode.Trim)
                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Lbackup.ansi-fmr"), strDirectory & "\" & strBarcode.Trim)
                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Rprimary.ansi-fmr"), strDirectory & "\" & strBarcode.Trim)
                                SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Rbackup.ansi-fmr"), strDirectory & "\" & strBarcode.Trim)
                            Else
                                intError += 1
                                sb.AppendLine(String.Format("ByteToFile(): Xml field of Barcode {0} is null", rw("Barcode")))
                            End If
                        Else
                            intError += 1
                            sb.AppendLine(String.Format("POFolder(): Unable to find PO {0} in {1} repository", strPO, SharedFunction.ForUploadingRepository))
                        End If
                    End If
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            Dim sbConsolidatedXML As New StringBuilder
            Dim sb_CubaoBranch_XML As New StringBuilder

            Dim UMIDCoding_PO_Job As String = File.ReadAllText(SharedFunction.UMIDCoding_PO_Job)
            Dim UMIDCoding_PO_Job_Seg As String = File.ReadAllText(SharedFunction.UMIDCoding_PO_Job_Seg)
            sbConsolidatedXML.Append(UMIDCoding_PO_Job.Replace("@@PO_NAME@@", strPurchaseOrders).Replace("@@DATA@@", sbXML_CardData.ToString))

            If IsWithUBPData Then sb_CubaoBranch_XML.Append(UMIDCoding_PO_Job_Seg.Replace("@@PO_NAME@@", strPurchaseOrders).Replace("@@DATA@@", sbXML_CardData_CubaoBranch.ToString))

            IO.File.WriteAllText(String.Format("{0}\{1}", strDirectory, Path.GetFileName(SharedFunction.UMIDCoding_PO_Job).Replace("UMIDCoding_PO_Job", strPurchaseOrders)), sbConsolidatedXML.ToString)

            'IO.File.WriteAllText(String.Format("{0}\{1}", strDirectory, Path.GetFileName(SharedFunction.UMIDCoding_PO_Job_Seg).Replace("UMIDCoding_PO_Job_Seg", strPurchaseOrders & "_MuhlSegregation")), sb_CubaoBranch_XML.ToString)
            If IsWithUBPData Then IO.File.WriteAllText(String.Format("{0}\{1}", strDirectory, Path.GetFileName(SharedFunction.UMIDCoding_PO_Job_Seg).Replace("UMIDCoding_PO_Job_Seg", strPurchaseOrders & "_Cubao")), sb_CubaoBranch_XML.ToString)

            DAL.Dispose()
            DAL = Nothing

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForMuhlbauerv2(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Public Function SaveToDownloadableFiles_ForRF(ByVal strIDs As String,
                                                  ByVal Batches As String,
                                                  ByVal intUserID As Integer,
                                                  ByRef sb As StringBuilder, ByRef intError As Integer,
                                                  Optional ByVal IsByPO As Boolean = True,
                                                  Optional ByVal ReportDate As Date = Nothing,
                                                  Optional ByRef OutputFile As String = "") As Boolean
        Try
            Dim DAL As New DAL
            Dim dt As DataTable = Nothing


            Dim strQuery As String = String.Format("SELECT OrigData,RF_ReasonCode FROM dbo.tblRelCDFRData WHERE PurchaseOrder IN ('{0}')", strIDs)
            If Not IsByPO Then strQuery = "SELECT OrigData,RF_ReasonCode FROM dbo.tblRelCDFRData"

            Try
                If DAL.SelectDataForRF(strQuery) Then
                    dt = DAL.TableResult
                End If
            Catch ex As Exception
            End Try

            Dim sbDataMain As New StringBuilder

            If dt.DefaultView.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    If IsByPO Then
                        sbDataMain.Append(String.Format("{0}{1}{2}", rw("OrigData").ToString.Trim.Substring(0, 451), Now.ToString("yyyyMMdd").PadRight(50, " "), rw("RF_ReasonCode").ToString.Trim) & vbNewLine)
                    Else
                        sbDataMain.Append(String.Format("{0}{1}{2}", rw("OrigData").ToString.Trim.Substring(0, 451), ReportDate.ToString("yyyyMMdd").PadRight(50, " "), rw("RF_ReasonCode").ToString.Trim) & vbNewLine)
                    End If
                Next

                If IsByPO Then
                    File.WriteAllText(String.Format("{0}\RF_B{1}.txt", SharedFunction.PersoRepository, Batches), sbDataMain.ToString)
                Else
                    OutputFile = String.Format("{0}\RF_B{1}.txt", CheckAndCreateDirectory(String.Format("{0}\BATCH{1}-MAILER_RF_KITTING", SharedFunction.PersoRepository, Batches)), Batches)
                    File.WriteAllText(OutputFile, sbDataMain.ToString)
                End If
            End If

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles_ForRF(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Private Function ReplaceWithEnye(ByVal obj As String) As String
        Return obj.Replace("?", "Ñ").Replace("�", "Ñ")
    End Function

    'Public Function SaveToDownloadableFiles_ForLaser(ByVal strIDs As String, _
    '                                         ByVal strPurchaseOrders As String, _
    '                                         ByVal intUserID As Integer, _
    '                                         ByVal OutputDirectory As String, _
    '                                         ByRef sb As StringBuilder, ByRef intError As Integer) As Boolean
    '    Try
    '        Dim strFolder As String = strPurchaseOrders '"Temp_" & Now.ToString("yyyMMdd_hhmmss")
    '        Dim strDirectory As String = OutputDirectory 'String.Format("{0}\{1}", OutputDirectory, strFolder)

    '        Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

    '        CheckAndCreateDirectory(strDirectory)

    '        'Dim strDescription As String = strBatchAndQtys

    '        Dim strTextFileLaserEngraving_PurchaseOrder As String = String.Format("{0}\{1}_LaserEngraving.txt", strDirectory, strPurchaseOrders)
    '        Dim strTextFileLaserEngraving_PurchaseOrder_5linesAddress As String = String.Format("{0}\{1}_LaserEngraving_5linesAddress.txt", strDirectory, strPurchaseOrders)

    '        Dim DAL As New DAL
    '        Dim dt As DataTable = Nothing

    '        Try
    '            If DAL.SelectDataForPrinting(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
    '                dt = DAL.TableResult
    '            End If
    '        Catch ex As Exception
    '        Finally
    '            DAL.Dispose()
    '            DAL = Nothing
    '        End Try

    '        Dim intSeries As Integer = 1

    '        For Each rw As DataRow In dt.Rows
    '            Try
    '                Dim FileType As Short = 1
    '                Dim line As String = ReplaceWithEnye(LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, intSeries, FileType))

    '                If line <> "" Then
    '                    If FileType = 1 Then
    '                        SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder, line, sb)
    '                    Else
    '                        SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_5linesAddress, line, sb)
    '                    End If

    '                    intSeries += 1

    '                    If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strImagesDirectory), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
    '                    'If Not IsDBNull(rw("Signature")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strImagesDirectory), rw("Barcode").ToString.Trim & "_Signature.tif"), rw("Signature"), sb)
    '                End If

    '            Catch ex As Exception
    '                intError += 1
    '                sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
    '            End Try
    '        Next

    '        ''new 04/07/2016
    '        'Dim strNewFolder As String = strDirectory.Replace(strFolder, strPurchaseOrders) & "_LaserEngraving"
    '        'If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

    '        'FileSystem.Rename(strDirectory, strNewFolder)

    '        'Directory.Delete(strDirectory, True)

    '        Return True
    '    Catch ex As Exception
    '        intError += 1
    '        sb.AppendLine(String.Format("SaveToDownloadableFiles(): Runtime error {0}", ex.Message))
    '        Return False
    '    End Try
    'End Function

    Public Function SaveToDownloadableFiles_ForLaserv2(ByVal strIDs As String,
                                             ByVal strPurchaseOrders As String,
                                             ByVal intUserID As Integer,
                                             ByVal OutputDirectory As String,
                                             ByRef sb As StringBuilder, ByRef intError As Integer,
                                             ByVal IsByPO As Boolean) As Boolean
        Try
            Dim strFolder As String = strPurchaseOrders
            Dim strDirectory As String = OutputDirectory

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)
            'Dim strImagesDirectory2 As String = String.Format("{0}\Images2", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strTextFileLaserEngraving_PurchaseOrder As String = String.Format("{0}\{1}_LaserEngraving.txt", strDirectory, strPurchaseOrders)
            Dim strTextFileLaserEngraving_PurchaseOrder_5linesAddress As String = String.Format("{0}\{1}_LaserEngraving_5linesAddress.txt", strDirectory, strPurchaseOrders)
            Dim strTextFileLaserEngraving_PurchaseOrder_4linesAddress2linesLastName As String = String.Format("{0}\{1}_LaserEngraving_4linesAddress2linesLastName.txt", strDirectory, strPurchaseOrders)
            Dim strTextFileLaserEngraving_PurchaseOrder_5linesAddress2linesLastName As String = String.Format("{0}\{1}_LaserEngraving_5linesAddress2linesLastName.txt", strDirectory, strPurchaseOrders)

            Dim DAL As New DAL
            Dim dt As DataTable = Nothing

            Try
                Dim strQuery As String = String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)
                If Not IsByPO Then strQuery = strIDs
                If DAL.SelectDataForPrintingForLaser(strQuery) Then
                    dt = DAL.TableResult
                End If
            Catch ex As Exception
            Finally
                DAL.Dispose()
                DAL = Nothing
            End Try

            Dim intSeries As Integer = 1

            For Each rw As DataRow In dt.Rows
                Try
                    Dim FileType As Short = 1
                    Dim line As String = ReplaceWithEnye(LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("Suffix").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, rw("CNo").ToString.Trim, rw("ExpDate").ToString.Trim, rw("ACNo").ToString.Trim, rw("OrigData").ToString.Trim, intSeries, FileType, sb, intError))

                    If line <> "" Then
                        Dim IsUBP As Boolean = line.Contains("<cardno>")

                        Select Case FileType
                            Case 1
                                If Not IsUBP Then
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder, line.Replace("<lname2>", ""), sb)
                                Else
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder.Replace(".txt", "_UBP.txt"), line.Replace("<lname2>", ""), sb)
                                End If
                            Case 2
                                If Not IsUBP Then
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_5linesAddress, line.Replace("<lname2>", ""), sb)
                                Else
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_5linesAddress.Replace(".txt", "_UBP.txt"), line.Replace("<lname2>", ""), sb)
                                End If
                            Case 3
                                If Not IsUBP Then
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_4linesAddress2linesLastName, line, sb)
                                Else
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_4linesAddress2linesLastName.Replace(".txt", "_UBP.txt"), line, sb)
                                End If
                            Case 4
                                If Not IsUBP Then
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_5linesAddress2linesLastName, line, sb)
                                Else
                                    SaveToTextfileUTF8(strTextFileLaserEngraving_PurchaseOrder_5linesAddress2linesLastName.Replace(".txt", "_UBP.txt"), line, sb)
                                End If
                        End Select

                        intSeries += 1

                        Dim strBarcode As String = rw("Barcode").ToString.Trim
                        'Dim strSourcePath As String = SharedFunction.GetPath(rw("PurchaseOrder").ToString.Trim, IIf(IsDBNull(rw("POSubFolder")), "", rw("POSubFolder").ToString.Trim), strBarcode)
                        Dim strSourcePath As String = SharedFunction.PSBImagesRepository
                        Dim strSourceFile As String = String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Photo.jpg")

                        If strSourcePath <> "Error" Then
                            If File.Exists(strSourceFile) Then
                                SharedFunction.FileCopyImage(strSourceFile, strImagesDirectory)

                                'added by edel on 06/27/2017 03:21pm. to create grayscale copy of photo
                                'SharedFunction.FileCopyImage(strSourceFile, strImagesDirectory)
                            Else
                                'If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strImagesDirectory), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                                sb.AppendLine(String.Format("SaveToDownloadableFiles_ForLaserv2(): Error in Purchase Order {0} Barcode {0}. Unable to find photo in pbs images.", rw("PurchaseOrder"), rw("Barcode")))
                            End If
                            'Else
                            '    If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strImagesDirectory), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                            'intError += 1
                            'sb.AppendLine(String.Format("SaveToDownloadableFiles_ForLaserv2(): Error in Purchase Order {0} Barcode {0}", rw("PurchaseOrder"), rw("Barcode")))                            
                        End If
                    End If
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Public Function GenerateDR2(ByVal strIDs As String, _
                                ByVal OutputDirectory As String, _
                                ByVal UserID As Integer,
                                ByVal UserCompleteName As String,
                                ByVal AbsolutePath As String,
                                ByRef sb As StringBuilder, ByRef intError As Integer) As Boolean
        Try
            Dim strDirectory As String = OutputDirectory

            CheckAndCreateDirectory(strDirectory)

            Dim DAL As New DAL
            Dim dt As DataTable = Nothing

            Try
                If DAL.SelectDataForDeliveryReceipt2(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
                    dt = DAL.TableResult
                End If
            Catch ex As Exception
            Finally
                'DAL.Dispose()
                'DAL = Nothing
            End Try

            Dim intPOID As Integer = 0
            Dim sbData As New StringBuilder
            Dim _PO As String = ""

            For Each rw As DataRow In dt.Rows
                Try
                    If intPOID = 0 Then
                        intPOID = rw("POID")
                        _PO = rw("PurchaseOrder")
                        sbData.AppendLine(String.Format("{0}{1}{2}{3}{4}{5}", "CRN" & vbTab, "last_name" & vbTab, "first_name" & vbTab, "middle_name" & vbTab, "back_ocr" & vbTab, "Barcode"))
                    ElseIf intPOID = rw("POID") Then
                        intPOID = rw("POID")
                    Else
                        IO.File.WriteAllText(String.Format("{0}\{1}.csv", strDirectory, _PO), sbData.ToString)
                        sbData.Clear()

                        DAL.UpdateRelPOReportByPOAndReportTypeID(_PO, DataKeysEnum.Report.DeliveryReceipt2, IO.File.ReadAllBytes(String.Format("{0}\{1}.csv", strDirectory, _PO)))
                        DAL.AddSystemLog(String.Format("{0} generated PO {1} dr", UserCompleteName, _PO), AbsolutePath, UserID)

                        intPOID = rw("POID")
                        _PO = rw("PurchaseOrder")
                        sbData.AppendLine(String.Format("{0}{1}{2}{3}{4}{5}", "CRN" & vbTab, "last_name" & vbTab, "first_name" & vbTab, "middle_name" & vbTab, "back_ocr" & vbTab, "Barcode"))
                    End If

                    sbData.AppendLine(String.Format("{0}{1}{2}{3}{4}{5}", rw("CRN").ToString.Trim & vbTab, rw("LName").ToString.Trim & vbTab, rw("FName").ToString.Trim & vbTab, rw("MName").ToString.Trim & vbTab, rw("BackOCR").ToString.Trim & vbTab, rw("Barcode").ToString.Trim))
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error Purchase Order {0 }Barcode {1} CRN {2}. Error {4}", rw("PurchaseOrder"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            IO.File.WriteAllText(String.Format("{0}\{1}.csv", strDirectory, _PO), sbData.ToString)
            DAL.UpdateRelPOReportByPOAndReportTypeID(_PO, DataKeysEnum.Report.DeliveryReceipt2, IO.File.ReadAllBytes(String.Format("{0}\{1}.csv", strDirectory, _PO)))
            DAL.AddSystemLog(String.Format("{0} generated PO {1} dr", UserCompleteName, _PO), AbsolutePath, UserID)

            DAL.Dispose()
            DAL = Nothing

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("GenerateDR2(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Public Function SaveToDownloadableFiles_ForLaser_bak(ByVal strIDs As String, _
                                             ByVal strPurchaseOrders As String, _
                                             ByVal strBatchAndQtys As String, _
                                             ByVal strType As String, _
                                             ByVal intUserID As Integer, _
                                             ByVal OutputDirectory As String, _
                                             ByRef sb As StringBuilder, ByRef intError As Integer) As Boolean
        Try
            Dim strFolder As String = "Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strDescription As String = strBatchAndQtys

            Dim strTextFileLaserFName1LineAddress4lines As String = String.Format("{0}\{1}_LaserEngravingFName1LineAddress4lines.txt", strDirectory, strPurchaseOrders)
            Dim strTextFileLaserFName2LinesAddress4lines As String = String.Format("{0}\{1}_LaserEngravingFName2LinesAddress4lines.txt", strDirectory, strPurchaseOrders)
            Dim strTextFileLaserFName1LineAddress5lines As String = String.Format("{0}\{1}_LaserEngravingFName1LineAddress5lines.txt", strDirectory, strPurchaseOrders)
            Dim strTextFileLaserFName2LinesAddress5lines As String = String.Format("{0}\{1}_LaserEngravingFName2LinesAddress5lines.txt", strDirectory, strPurchaseOrders)

            Dim strTextFileLaser_Conso As String = String.Format("{0}\{1}_LaserEngravingConso.txt", strDirectory, strPurchaseOrders)

            'Dim strTextFileLaserLong As String = String.Format("{0}\{1}_LaserEngravingLong.txt", strDirectory, strPurchaseOrders)

            Dim DAL As New DAL
            Dim dt As DataTable = Nothing

            Try
                If DAL.SelectDataForPrinting(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
                    dt = DAL.TableResult
                End If
            Catch ex As Exception
            End Try

            Dim intfileType As Short

            For Each rw As DataRow In dt.Rows
                Try
                    Dim line As String = "" 'LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, rw("CurrentSeries").ToString) ', intfileType)

                    Select Case intfileType
                        Case 1
                            SaveToTextfile(strTextFileLaserFName1LineAddress4lines, line, sb)
                        Case 2
                            SaveToTextfile(strTextFileLaserFName2LinesAddress4lines, line, sb)
                        Case 3
                            SaveToTextfile(strTextFileLaserFName1LineAddress5lines, line, sb)
                        Case 4
                            SaveToTextfile(strTextFileLaserFName2LinesAddress5lines, line, sb)
                    End Select

                    SaveToTextfile(strTextFileLaser_Conso, line, sb)

                    If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strImagesDirectory), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                    If Not IsDBNull(rw("Signature")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strImagesDirectory), rw("Barcode").ToString.Trim & "_Signature.tif"), rw("Signature"), sb)
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            ''add to downloadable the purchaseorder.txt
            'If DAL.AddDownloadableFiles(strDescription, strTextFile, intUserID, String.Format("{0} - Mailer", strType), strIDs) Then
            'End If

            'add to downloadable the purchaseorder.zip
            'If DAL.AddDownloadableFiles(strDescription, "", intUserID, String.Format("{0} - LaserEngraving", strType), strIDs) Then
            '    Dim fc As New FileCompression
            '    Dim strZipFile As String
            '    If fc.Compress(strDirectory, strZipFile) Then
            '        Using _zip = New ICSharpCode.SharpZipLib.Zip.ZipFile(strZipFile)
            '            _zip.UseZip64 = ICSharpCode.SharpZipLib.Zip.UseZip64.On
            '            _zip.BeginUpdate()

            '            For Each rw As DataRow In dt.Rows
            '                Try
            '                    fc.addFolderToZip(_zip, strDirectory, strImagesDirectory)
            '                Catch ex As Exception
            '                End Try
            '            Next

            '            _zip.CommitUpdate()
            '            _zip.Close()
            '        End Using

            '        Dim strNewFile As String = String.Format("{0}\{1}", strZipFile.Substring(0, strZipFile.LastIndexOf("\")), strPurchaseOrders & "_LaserEngraving.zip")

            '        FileSystem.Rename(strZipFile, strNewFile)

            '        If Not DAL.ExecuteQuery(String.Format("UPDATE tblDownloadableFiles SET FilePath='{0}' WHERE DownloadFileID={1}", strNewFile, DAL.ObjectResult)) Then
            '            intError += 1
            '            sb.AppendLine(String.Format("Failed to update FilePath. Returned error {0}", DAL.ErrorMessage))
            '        End If
            '    Else
            '        intError += 1
            '        sb.AppendLine(String.Format("Compress(): Returned error {0}", fc.ErrorMessage))
            '    End If
            '    fc = Nothing
            'Else
            '    intError += 1
            '    sb.AppendLine(String.Format("AddDownloadableFiles(): Returned error {0}", DAL.ErrorMessage))
            'End If

            DAL.Dispose()
            DAL = Nothing

            'new 04/07/2016
            Dim strNewFolder As String = strDirectory.Replace(strFolder, strPurchaseOrders) & "_LaserEngraving"
            If Directory.Exists(strNewFolder) Then Directory.Delete(strNewFolder, True)

            FileSystem.Rename(strDirectory, strNewFolder)

            'Directory.Delete(strDirectory, True)

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Public Function SaveToDownloadableFiles_bak(ByVal strIDs As String, _
                                             ByVal strPurchaseOrders As String, _
                                             ByVal strBatchAndQtys As String, _
                                             ByVal strType As String, _
                                             ByVal intUserID As Integer, _
                                             ByVal intPrintable As Integer, _
                                             ByVal IsDeleteSource As Boolean, _
                                             ByVal OutputDirectory As String, _
                                             ByRef sb As StringBuilder, ByRef intError As Integer, _
                                             Optional IsMuhlbauerExtraction As Boolean = False) As Boolean
        Try
            Dim strHeader As String = "Batch-Page|Series|Barcode|CRN|LastName|FirstName|MiddleName|Sex|Birthday|Address|BackOCR|Photo_Image|Signature_Image|Barcode_Image"
            Dim strFolder As String = "Temp_" & Now.ToString("yyyMMdd_hhmmss")
            Dim strDirectory As String = String.Format("{0}\{1}", OutputDirectory, strFolder)

            Dim strImagesDirectory As String = String.Format("{0}\Images", strDirectory)

            CheckAndCreateDirectory(strDirectory)

            Dim strDescription As String = strBatchAndQtys

            Dim strTextFile As String ' = String.Format("{0}\{1}.txt", strDirectory, strDescription)

            If intPrintable = 100000 Then
                strTextFile = String.Format("{0}\{1}.txt", OutputDirectory, strPurchaseOrders)
            Else
                strTextFile = String.Format("{0}\{1}.txt", strDirectory, strDescription)
            End If

            SaveToTextfile(strTextFile, strHeader, sb)

            Dim DAL As New DAL
            Dim dt As DataTable = Nothing
            'Dim barcodeGenerator As New BarcodeGenerator

            'WriteStatus("If Not IsMuhlbauerExtraction Then")
            Try
                If Not IsMuhlbauerExtraction Then
                    'WriteStatus(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs))
                    If DAL.SelectDataForPrinting(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
                        dt = DAL.TableResult
                    End If
                Else
                    If DAL.SelectDataForMuhlbauer(String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)) Then
                        dt = DAL.TableResult
                    End If
                End If

                'WriteStatus("Success")
            Catch ex As Exception
                'WriteStatus("If Not IsMuhlbauerExtraction Then..." & ex.Message)
            End Try

            SharedFunction.ReComputePageAndSeries(dt, 0)

            'WriteStatus(dt.DefaultView.Count)
            For Each rw As DataRow In dt.Rows
                Try
                    If rw("CurrentSeries") > intPrintable Then
                        'drop cards are not included if for indigo
                    Else
                        Dim strLine As String = String.Format("{0}-{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", _
                                             rw("Batch").ToString.Trim, rw("CurrentPage").ToString.Trim, rw("CurrentSeries").ToString.Trim, rw("Barcode").ToString.Trim, _
                                             rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, _
                                             rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim, rw("BackOCR").ToString.Trim, _
                                             rw("Barcode").ToString.Trim & "_Photo.jpg", _
                                             rw("Barcode").ToString.Trim & "_Signature.tiff", _
                                             rw("Barcode").ToString.Trim & ".jpg")

                        SaveToTextfile(strTextFile, strLine, sb)
                        'If intPrintable = 100000 Then SaveToTextfile(strTextFile.Replace(".txt", "_laserprinter.txt"), LaserPrintingLineStructure(rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, rw("LName").ToString.Trim, rw("FName").ToString.Trim, rw("MName").ToString.Trim, rw("Sex").ToString.Trim, rw("DateOfBirth").ToString.Trim, rw("Address").ToString.Trim), sb)

                        If IsMuhlbauerExtraction Then
                            'WriteStatus("If IsMuhlbauerExtraction Then...Photo")
                            If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                            If Not IsDBNull(rw("Signature")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Signature.tiff"), rw("Signature"), sb)
                            If Not IsDBNull(rw("Xml")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & ".xml"), rw("Xml"), sb)
                            If Not IsDBNull(rw("PersoXml")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_perso.xml"), rw("PersoXml"), sb)
                            If Not IsDBNull(rw("LprimaryANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Lprimary.ansi-fmr"), rw("LprimaryANSI378"), sb)
                            If Not IsDBNull(rw("LbackupANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Lbackup.ansi-fmr"), rw("LbackupANSI378"), sb)
                            If Not IsDBNull(rw("RprimaryANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Rprimary.ansi-fmr"), rw("RprimaryANSI378"), sb)
                            If Not IsDBNull(rw("RbackupANSI378")) Then ByteToFile(String.Format("{0}\{1}", CheckAndCreateDirectory(strDirectory & "\" & rw("Barcode").ToString.Trim), rw("Barcode").ToString.Trim & "_Rbackup.ansi-fmr"), rw("RbackupANSI378"), sb)
                        Else
                            CheckAndCreateDirectory(strImagesDirectory)

                            If Not IsDBNull(rw("Photo")) Then ByteToFile(String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & "_Photo.jpg"), rw("Photo"), sb)
                            'If Not IsDBNull(rw("Signature")) Then ByteToFile(String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & "_Signature.tiff"), rw("Signature"), sb)

                            If File.Exists(String.Format("{0}\{1}", SharedFunction.BarcodeRepository, rw("Barcode").ToString.Trim & ".jpg")) Then
                                File.Copy(String.Format("{0}\{1}", SharedFunction.BarcodeRepository, rw("Barcode").ToString.Trim & ".jpg"), String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg"))
                            Else
                                intError += 1
                                sb.AppendLine(String.Format("Barcode generation. Unable to find Barcode {0} in barcode repository", rw("Barcode").ToString.Trim))
                            End If

                            'If Not barcodeGenerator.Generate(rw("Barcode").ToString.Trim, String.Format("{0}\{1}", strImagesDirectory, rw("Barcode").ToString.Trim & ".jpg")) Then
                            '    intError += 1
                            '    sb.AppendLine(String.Format("barcodeGenerator.Generate(): Failed to generate barcode for {0}. Returned error {1}", rw("Barcode").ToString.Trim, barcodeGenerator.ErrorMessage))
                            'End If
                        End If
                    End If
                Catch ex As Exception
                    intError += 1
                    sb.AppendLine(String.Format("Runtime error encountered in textfile saving and image extraction for CardID {0} Barcode {1} CRN {2}. Error {4}", rw("CardID"), rw("Barcode"), rw("CRN"), ex.Message))
                End Try
            Next

            'If Not barcodeGenerator Is Nothing Then barcodeGenerator = Nothing

            'WriteStatus("If IsMuhlbauerExtraction Then''''add to downloadable the purchaseorder.txt")
            If IsMuhlbauerExtraction Then
                'add to downloadable the purchaseorder.txt
                If DAL.AddDownloadableFiles(strDescription, strTextFile, intUserID, String.Format("{0} - Mailer", strType), strIDs) Then
                End If
            End If

            'add to downloadable the purchaseorder.zip
            If DAL.AddDownloadableFiles(strDescription, "", intUserID, IIf(IsMuhlbauerExtraction = False, strType, String.Format("{0} - Muhlbauer", strType)), strIDs) Then
                'FileSystem.Rename(strTextFile, String.Format("{0}\{1}", strTextFile.Substring(0, strTextFile.LastIndexOf("\")), "DFID" & DAL.ObjectResult.ToString.Trim & ".txt"))

                '("Dim fc As New FileCompression")
                Dim fc As New FileCompression
                Dim strZipFile As String
                If fc.Compress(strDirectory, strZipFile) Then
                    Using _zip = New ICSharpCode.SharpZipLib.Zip.ZipFile(strZipFile)
                        _zip.UseZip64 = ICSharpCode.SharpZipLib.Zip.UseZip64.On
                        _zip.BeginUpdate()

                        If Not IsMuhlbauerExtraction Then
                            fc.addFolderToZip(_zip, strDirectory, strImagesDirectory)
                        Else
                            For Each rw As DataRow In dt.Rows
                                Try
                                    If rw("CurrentSeries") > intPrintable Then
                                        'drop cards are not included if for indigo
                                    Else
                                        fc.addFolderToZip(_zip, strDirectory, strDirectory & "\" & rw("Barcode").ToString.Trim)
                                    End If
                                Catch ex As Exception
                                End Try
                            Next
                        End If

                        _zip.CommitUpdate()
                        _zip.Close()
                    End Using

                    Dim strNewFile As String = ""

                    'WriteStatus("If intPrintable = 100000 Then")
                    If intPrintable = 100000 Then
                        strNewFile = String.Format("{0}\{1}", strZipFile.Substring(0, strZipFile.LastIndexOf("\")), strPurchaseOrders & ".zip")
                    Else
                        strNewFile = String.Format("{0}\{1}", strZipFile.Substring(0, strZipFile.LastIndexOf("\")), "DFID" & DAL.ObjectResult.ToString.Trim & ".zip")
                    End If

                    'WriteStatus("FileSystem.Rename(strZipFile, strNewFile)")
                    FileSystem.Rename(strZipFile, strNewFile)

                    If Not DAL.ExecuteQuery(String.Format("UPDATE tblDownloadableFiles SET FilePath='{0}' WHERE DownloadFileID={1}", strNewFile, DAL.ObjectResult)) Then
                        intError += 1
                        sb.AppendLine(String.Format("Failed to update FilePath. Returned error {0}", DAL.ErrorMessage))
                    End If

                    'WriteStatus("If IsDeleteSource Then")
                    If IsDeleteSource Then
                        'delete physical PO folder in repository
                        For Each _PO As String In strPurchaseOrders.Split(",")
                            SharedFunction.DeletePO_ForUploadingRepository(_PO)
                        Next
                    End If

                    Directory.Delete(strDirectory, True)
                Else
                    intError += 1
                    sb.AppendLine(String.Format("Compress(): Returned error {0}", fc.ErrorMessage))
                End If
                fc = Nothing
            Else
                intError += 1
                sb.AppendLine(String.Format("AddDownloadableFiles(): Returned error {0}", DAL.ErrorMessage))
            End If

            DAL.Dispose()
            DAL = Nothing

            Return True
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("SaveToDownloadableFiles(): Runtime error {0}", ex.Message))
            Return False
        End Try
    End Function

    Private Function CheckAndCreateDirectory(ByVal strDirectory As String) As String
        If Not Directory.Exists(strDirectory) Then Directory.CreateDirectory(strDirectory)

        Return strDirectory
    End Function

    Private Function SaveToTextfile(ByVal strFile As String, ByVal strData As String, ByRef sb As StringBuilder) As Boolean
        Try
            Using sr = New StreamWriter(strFile, True)
                sr.WriteLine(strData)
            End Using

            Return True

        Catch ex As Exception
            sb.AppendLine(String.Format("SaveToTextfile(): Runtime error encountered {0}", ex.Message))

            Return False
        End Try
    End Function

    Private Function SaveToTextfileUTF8(ByVal strFile As String, ByVal strData As String, ByRef sb As StringBuilder) As Boolean
        Try
            Using sr = New StreamWriter(strFile, True, System.Text.Encoding.UTF8)
                sr.WriteLine(strData)
            End Using

            Return True

        Catch ex As Exception
            sb.AppendLine(String.Format("SaveToTextfile(): Runtime error encountered {0}", ex.Message))

            Return False
        End Try
    End Function

    Private Function ByteToFile(ByVal strFile As String, ByVal blob As Byte(), ByRef sb As StringBuilder) As Boolean
        Try
            Dim FS1 As New IO.FileStream(strFile, IO.FileMode.Create)
            FS1.Write(blob, 0, blob.Length)
            FS1.Close()
            FS1 = Nothing

            Return True
        Catch ex As Exception
            sb.AppendLine(String.Format("ByteToFile(): Runtime error encountered {0}", ex.Message))

            Return False
        End Try
    End Function

    'Public Function LaserPrintingLineStructure_orig(ByVal CSN As String, ByVal CRN As String, ByVal LastNameSuffix As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal Sex As String, ByVal BirthDate As String, ByVal Address As String, ByVal Series As String, ByRef FileType As Short) As String
    '    Try
    '        Dim _FirstName() As String = FormatDataWithCharLength(FirstName, 27).Split(vbNewLine)
    '        Dim FirstName1 As String = ""
    '        Dim FirstName2 As String = ""

    '        For i As Short = 0 To _FirstName.Length - 1
    '            Select Case Trim(_FirstName(i))
    '                Case "", " ", vbNewLine
    '                Case Else
    '                    Select Case i
    '                        Case 0
    '                            FirstName1 = _FirstName(i)
    '                        Case 1
    '                            FirstName2 = _FirstName(i)
    '                    End Select
    '            End Select
    '        Next

    '        Dim _Address() As String = FormatData(Address.Trim).Split(vbNewLine)
    '        Dim Address1 As String = ""
    '        Dim Address2 As String = ""
    '        Dim Address3 As String = ""
    '        Dim Address4 As String = ""
    '        Dim Address5 As String = ""

    '        For i As Short = 0 To _Address.Length - 1
    '            Select Case Trim(_Address(i))
    '                Case "", " ", vbNewLine
    '                Case Else
    '                    Select Case i
    '                        Case 0
    '                            Address1 = _Address(i)
    '                        Case 1
    '                            Address2 = _Address(i)
    '                        Case 2
    '                            Address3 = _Address(i)
    '                        Case 3
    '                            Address4 = _Address(i)
    '                        Case 4
    '                            Address5 = _Address(i)
    '                    End Select
    '            End Select
    '        Next

    '        If Address5 <> "" Then
    '            FileType = 2
    '        End If

    '        Return String.Format("{0}" & _
    '                     "<csn>{1}" & _
    '                     "<crn>{2}" & _
    '                     "<lname>{3}" & _
    '                     "<fname>{4}" & _
    '                     "<fname2>{5}" & _
    '                     "<mname>{6}" & _
    '                     "<sex>{7}" & _
    '                     "<dob>{8}" & _
    '                     "<address1>{9}" & _
    '                     "<address2>{10}" & _
    '                     "<address3>{11}" & _
    '                     "<address4>{12}" & _
    '                     "<address5>{13}" & _
    '                     "<photo>{1}_Photo.jpg", _
    '                      Series, CSN, CRN, LastNameSuffix, FirstName1, FirstName2, MiddleName, Sex.Substring(0, 1).ToUpper, BirthDate, Address1, Address2, Address3, Address4, Address5)

    '        'Series, CSN, CRN, LastNameSuffix, FirstName, MiddleName, Sex.Substring(0, 1).ToUpper, CDate(BirthDate).ToString("MMMM dd, yyyy"), Address1, Address2, Address3, Address4, Address5)
    '    Catch ex As Exception
    '        Return ""
    '    End Try
    'End Function

    Private Function FirstNameSuffix(ByVal Suffix As String) As String
        If Suffix.Trim = "" Then
            Return ""
        Else
            Select Case Suffix.Trim.ToUpper
                Case "JR", "SR", "JR.", "SR."
                    Return ", " & Suffix
                    'Case "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI", "XXII", "XXIII", "XXIV", "XXV", "XXVI", "XXVII", "XXVIII", "XXIX", "XXX", "XXXI", "XXXII", "XXXIII", "XXXIV", "XXXV", "XXXVI", "XXXVII", "XXXVIII", "XXXIX", "XL", "XLI", "XLII", "XLIII", "XLIV", "XLV", "XLVI", "XLVII", "XLVIII", "XLIX", "L"
                Case Else
                    Return " " & Suffix
            End Select
        End If
    End Function

    Public Function LaserPrintingLineStructure(ByVal CSN As String, ByVal CRN As String, ByVal LastName As String, ByVal Suffix As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal Sex As String, ByVal BirthDate As String, ByVal Address As String,
                                               ByVal CNo As String, ByVal ExpDate As String, ByVal ACNo As String, ByVal OrigData As String,
                                               ByVal Series As String, ByRef FileType As Short,
                                               ByRef sb As StringBuilder, ByRef intError As Integer) As String
        Try

            Dim IsErrorFlag As Boolean

            Dim _FirstName() As String = FormatDataWithCharLength(FirstName & FirstNameSuffix(Suffix), My.Settings.FName_Limit_Laser, IsErrorFlag).Split(vbNewLine)

            If IsErrorFlag Then
                intError += 1
                sb.AppendLine(String.Format("LaserPrintingLineStructure(): Failed in CSN {0}, CRN {1}. Exceed FName loop limit. First name need editing.", CSN, CRN))

                Return ""
            End If

            Dim FirstName1 As String = ""
            Dim FirstName2 As String = ""

            Dim _LastName() As String = FormatDataWithCharLength(LastName, My.Settings.LName_Limit_Laser, IsErrorFlag).Split(vbNewLine)

            If IsErrorFlag Then
                intError += 1
                sb.AppendLine(String.Format("LaserPrintingLineStructure(): Failed in CSN {0}, CRN {1}. Exceed LName loop limit. Last name need editing.", CSN, CRN))

                Return ""
            End If

            Dim LastName1 As String = ""
            Dim LastName2 As String = ""

            For i As Short = 0 To _FirstName.Length - 1
                Select Case Trim(_FirstName(i))
                    Case "", " ", vbNewLine
                    Case Else
                        Select Case i
                            Case 0
                                FirstName1 = _FirstName(i)
                            Case 1
                                FirstName2 = _FirstName(i)
                        End Select
                End Select
            Next

            For i As Short = 0 To _LastName.Length - 1
                Select Case Trim(_LastName(i))
                    Case "", " ", vbNewLine
                    Case Else
                        Select Case i
                            Case 0
                                LastName1 = _LastName(i)
                            Case 1
                                LastName2 = _LastName(i)
                        End Select
                End Select
            Next

            'Dim _Address() As String = FormatData(Address.Trim).Split(vbNewLine)
            Dim _Address() As String = FormatDataWithCharLength(Address.Trim, My.Settings.Address_Limit_Laser, IsErrorFlag).Split(vbNewLine)

            If IsErrorFlag Then
                intError += 1
                sb.AppendLine(String.Format("LaserPrintingLineStructure(): Failed in CSN {0}, CRN {1}. Exceed Address loop limit. Address need editing.", CSN, CRN))

                Return ""
            End If

            Dim Address1 As String = ""
            Dim Address2 As String = ""
            Dim Address3 As String = ""
            Dim Address4 As String = ""
            Dim Address5 As String = ""

            For i As Short = 0 To _Address.Length - 1
                Select Case Trim(_Address(i))
                    Case "", " ", vbNewLine
                    Case Else
                        Select Case i
                            Case 0
                                Address1 = _Address(i)
                            Case 1
                                Address2 = _Address(i)
                            Case 2
                                Address3 = _Address(i)
                            Case 3
                                Address4 = _Address(i)
                            Case 4
                                Address5 = _Address(i)
                        End Select
                End Select
            Next

            If Address5 <> "" And LastName2 = "" Then '4 lines address, 1 line lastname
                FileType = 2
            ElseIf Address5 = "" And LastName2 <> "" Then '4 lines address, 2 lines lastname
                FileType = 3
            ElseIf Address5 <> "" And LastName2 <> "" Then '5 lines address, 2 lines lastname
                FileType = 4
            End If

            If CNo.Trim = "" Then
                Return String.Format("{0}" &
                         "<csn>{1}" &
                         "<crn>{2}" &
                         "<lname>{3}" &
                         "<lname2>{4}" &
                         "<fname>{5}" &
                         "<fname2>{6}" &
                         "<mname>{7}" &
                         "<sex>{8}" &
                         "<dob>{9}" &
                         "<address1>{10}" &
                         "<address2>{11}" &
                         "<address3>{12}" &
                         "<address4>{13}" &
                         "<address5>{14}" &
                         "<photo>{1}_Photo.jpg" &
                         "<track3>{2}",
                          Series, CSN, CRN, LastName1.Trim, LastName2.Trim, FirstName1.Trim, FirstName2.Trim, MiddleName.Trim, Sex.Substring(0, 1).ToUpper, BirthDate, Address1.Trim, Address2.Trim, Address3.Trim, Address4.Trim, Address5.Trim, CRN.Replace("-", ""))
            Else
                Return String.Format("{0}" &
                         "<csn>{1}" &
                         "<crn>{2}" &
                         "<lname>{3}" &
                         "<lname2>{4}" &
                         "<fname>{5}" &
                         "<fname2>{6}" &
                         "<mname>{7}" &
                         "<sex>{8}" &
                         "<dob>{9}" &
                         "<address1>{10}" &
                         "<address2>{11}" &
                         "<address3>{12}" &
                         "<address4>{13}" &
                         "<address5>{14}" &
                         "<photo>{1}_Photo.jpg" &
                         "<cardno>{15}" &
                         "<expdate>{16}" &
                         "<acctno>{17}" &
                         "<origdata>{18}",
                          Series, CSN, CRN, LastName1.Trim, LastName2.Trim, FirstName1.Trim, FirstName2.Trim, MiddleName.Trim, Sex.Substring(0, 1).ToUpper, BirthDate, Address1.Trim, Address2.Trim, Address3.Trim, Address4.Trim, Address5.Trim, CNo.Trim, ExpDate.Trim, ACNo.Trim, OrigData.Trim)
            End If



            'Series, CSN, CRN, LastNameSuffix, FirstName, MiddleName, Sex.Substring(0, 1).ToUpper, CDate(BirthDate).ToString("MMMM dd, yyyy"), Address1, Address2, Address3, Address4, Address5)
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("LaserPrintingLineStructure(): Failed in CSN {0}, CRN {1}. Error catched {2}", CSN, CRN, ex.Message))
            Return ""
        End Try
    End Function

    Public Shared Function FormatDataWithCharLength(ByVal value As String, ByVal intCharLength As Short, ByRef IsErrorFlag As Boolean) As String
        Dim space As String = " "

        Dim sbOld As New System.Text.StringBuilder
        sbOld.Append(value)

        Dim sbNew As New System.Text.StringBuilder

        'check loop to see if it exceeds normal number
        Dim intLoop As Short = 0
        Dim intNormalLoop As Short = 5

        Do While sbOld.ToString <> ""
            Dim intSpaceLastIndex As Short
            If sbOld.ToString.Length > intCharLength Then
                If sbOld.ToString.Substring(intCharLength - 1, 1) = space Then
                    sbNew.AppendLine(Trim(sbOld.ToString.Substring(0, intCharLength)))
                    sbOld.Remove(0, intCharLength)
                ElseIf sbOld.ToString.Substring(intCharLength - 1, 1) <> space And sbOld.ToString.Substring(intCharLength, 1) <> space Then
                    intSpaceLastIndex = sbOld.ToString.Substring(0, intCharLength).LastIndexOf(space)
                    sbNew.AppendLine(Trim(sbOld.ToString.Substring(0, intSpaceLastIndex)))
                    sbOld.Remove(0, intSpaceLastIndex)
                Else
                    sbNew.AppendLine(Trim(sbOld.ToString.Substring(0, intCharLength)))
                    sbOld.Remove(0, intCharLength)
                End If

                intLoop += 1

                If intLoop > intNormalLoop Then
                    IsErrorFlag = True
                    Return ""
                End If
            Else
                sbNew.Append(Trim(sbOld.ToString.Substring(0)))
                sbOld.Clear()
            End If
        Loop

        IsErrorFlag = False
        Return sbNew.ToString
    End Function

    Private Function XML_CardData_Format(ByVal CSN As String) As String
        Dim sb As New StringBuilder
        sb.Append("<Card>" & vbNewLine)
        sb.Append("<CardName Format=""Text"">" & CSN & "</CardName>" & vbNewLine)
        sb.Append("<WSData WSName=""MKW_Rinas"" Nr=""1"">" & vbNewLine)
        sb.Append("<Variable Name=""Track2"" Side=""1"" Format=""Text""></Variable>" & vbNewLine)
        sb.Append("</WSData>" & vbNewLine)
        sb.Append("<WSData WSName=""Coding_Contactless"" Nr=""1"">" & vbNewLine)
        sb.Append("<RawData Format=""Text"">" & vbNewLine)
        sb.Append("<![CDATA[<?xml version=""1.0"" ?>" & vbNewLine)
        sb.Append("<ChipCodingData Lcid=""String"" >" & vbNewLine)
        sb.Append("<LDS>" & vbNewLine)
        sb.Append("<CSN>" & CSN & "</CSN>" & vbNewLine)
        sb.Append("</LDS>" & vbNewLine)
        sb.Append("</ChipCodingData>]]>" & vbNewLine)
        sb.Append("</RawData>" & vbNewLine)
        sb.Append("</WSData>" & vbNewLine)
        sb.Append("</Card>")

        Return sb.ToString
    End Function

    Private Function XML_CardData_Format2(ByVal CSN As String) As String
        Dim sb As New StringBuilder
        sb.Append("	  <Card>" & vbNewLine)
        sb.Append("	  <CardName Format=""Text"">" & CSN & "</CardName>" & vbNewLine)
        sb.Append("	  <WSData WSName=""MKW_Rinas"" Nr=""1"">" & vbNewLine)
        sb.Append("	  <Variable Name=""Track2"" Side=""1"" Format=""Text""></Variable>" & vbNewLine)
        sb.Append("	  </WSData>" & vbNewLine)
        sb.Append("			<WSData WSName=""Coding_Contactless"" Nr=""1"">" & vbNewLine)
        sb.Append("				<RawData Format=""Text"">" & vbNewLine)
        sb.Append("				<![CDATA[<?xml version=""1.0"" ?>" & vbNewLine)
        sb.Append("				<ChipCodingData Lcid=""String"" >" & vbNewLine)
        sb.Append("				<LDS>" & vbNewLine)
        sb.Append("					<CSN>" & CSN & "</CSN>" & vbNewLine)
        sb.Append("				</LDS>" & vbNewLine)
        sb.Append("				</ChipCodingData>]]>" & vbNewLine)
        sb.Append("				</RawData>" & vbNewLine)
        sb.Append("			</WSData>" & vbNewLine)
        sb.Append("	  </Card>")

        Return sb.ToString
    End Function

    Private Function XML_CardData_Format3(ByVal CSN As String) As String
        Dim sb As New StringBuilder
        sb.Append("<Card>")
        sb.Append("<!-- card name is for the matching process -->")
        sb.Append("<CardName Format=""Text"">" & CSN & "</CardName>")
        sb.Append("<!-- MKW data for this card -->")
        sb.Append("<WSData WSName=""MKW_Rinas"" Nr=""1"">")
        sb.Append("<Variable Name=""Track3"" Side=""1"" Format=""Text""></Variable>")
        sb.Append("</WSData>")
        sb.Append("<!-- chip data -->")
        sb.Append("<WSData WSName=""Coding_Contactless"" Nr=""1"">")
        sb.Append("<RawData Format=""Text"">" & CSN & "</RawData> <!-- data set number -->")
        sb.Append("</WSData>")
        sb.Append("</Card>")
        sb.Append("<Card>")

        Return sb.ToString
    End Function

    Private Function XML_CardData_Format4(ByVal CSN As String, Optional ByVal CRN As String = "") As String
        Dim sb As New StringBuilder
        sb.Append("<Card>")
        sb.Append("<CardName Format=""Text"">" & CSN & "</CardName>")
        sb.Append("<WSData WSName=""MKW_Rinas"" Nr=""1"">")

        'revised on 08/11/2019. added track3
        'sb.Append("<Variable Name=""Track2"" Side=""1"" Format=""Text""></Variable>")
        If CRN.Trim = "" Then
            sb.Append("<Variable Name=""Track2"" Side=""1"" Format=""Text""></Variable>")
        Else
            sb.Append("<Variable Name=""Track3"" Side=""1"" Format=""Text"">" & CRN.Trim.Replace("-", "") & "</Variable>")
        End If

        sb.Append("</WSData>")
        sb.Append("<WSData WSName=""Coding_Contactless"" Nr=""1"">")
        sb.Append("<RawData Format=""Text"">")
        sb.Append("<![CDATA[<?xml version=""1.0"" ?>")
        sb.Append("<ChipCodingData Lcid=""String"" >")
        sb.Append("<LDS>")
        sb.Append("<CSN>" & CSN & "</CSN>")
        sb.Append("</LDS>")
        sb.Append("</ChipCodingData>]]>")
        sb.Append("</RawData>")
        sb.Append("</WSData>")
        sb.Append("</Card>")

        Return sb.ToString
    End Function


End Class

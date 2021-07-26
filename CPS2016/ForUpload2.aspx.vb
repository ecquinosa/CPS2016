
Imports System.IO

Public Class ForUpload2
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            Dim strPurchaseOrder As String = IO.File.ReadAllText(SharedFunction.ForUploadingRepository & "\ManualUpload.txt")
            Button1.Text = strPurchaseOrder

            GridDataBind()
        End If
    End Sub

    Private Function GetTotalPhoto(ByVal strSubDir As String) As Integer
        Dim dirInfo As DirectoryInfo
        Dim intCntr As Integer = 0

        For Each strSubDir2 As String In Directory.GetDirectories(strSubDir)
            dirInfo = New DirectoryInfo(strSubDir)
            intCntr += dirInfo.GetFiles("*.jpg").Length
            dirInfo = Nothing
        Next

        Return intCntr
    End Function

    Private Function GetTotalFolder(ByVal strSubDir As String) As Integer
        Dim intCntr As Integer = 0

        For Each strSubDir1 As String In Directory.GetDirectories(strSubDir)
            intCntr += Directory.GetDirectories(strSubDir1).Length
        Next

        Return intCntr
    End Function

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
        Dim strPurchaseOrder As String = xGrid.GetRowValues(index, "PurchaseOrder")
        Dim strBatch As String = xGrid.GetRowValues(index, "Batch")

        UploadData(strPurchaseOrder.Trim.Substring(0, 33), strBatch.Trim, strPurchaseOrder.Trim) ', ID)
    End Sub

    Private Sub AddToRow(ByVal MemberXML As MemberXML, ByVal strSubDir2 As String, ByRef dt As DataTable)
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

    Private Sub AddPODetails(ByVal DAL As DAL, ByVal dt As DataTable, ByVal intPOID As Integer, ByVal strBatch As String, _                            ByRef intBoxCntr As Integer, ByRef intBoxSeriesCntr As Integer, ByRef intDtlCntr As Integer, _                            ByRef intPageCntr As Short, ByRef intPageSeriesCntr As Short, ByRef IsSuccess As Boolean, _                            ByRef intError As Integer, ByRef sb As StringBuilder, _                            Optional ActivityID As DataKeysEnum.ActivityID = DataKeysEnum.ActivityID.IndigoDownload)        For Each rw As DataRow In dt.Rows            Dim strBackOCR As String = String.Format("{0}.{1}.{2}", strBatch, intBoxCntr.ToString, intBoxSeriesCntr.ToString)            Dim strSubDir As String = rw("Path").ToString.Substring(rw("Path").ToString.LastIndexOf("\") + 1)            If DAL.AddRelPOData(intPOID, rw("CRN"), rw("Barcode"), rw("FirstName"), rw("MiddleName"), rw("LastName"), rw("Suffix"), rw("Sex"), rw("DateOfBirth"), rw("Address"), ActivityID, strBackOCR, 0, 0, strSubDir, rw("AddressDelimited")) Then                intDtlCntr += 1                DAL.AddCardActivity(intPOID, rw("CRN"), rw("Barcode"), "Uploaded", SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))                If Not DAL.AddRelPOImage(rw("Barcode"), _                                            File.ReadAllBytes(String.Format("{0}\{1}.xml", rw("Path"), rw("Barcode"))), _                                            GetFileByte(String.Format("{0}\{1}_perso.xml", rw("Path"), rw("Barcode")), 1), _                                            File.ReadAllBytes(String.Format("{0}\{1}_Photo.jpg", rw("Path"), rw("Barcode"))), _                                            GetFileByte(String.Format("{0}\{1}_Signature.tiff", rw("Path"), rw("Barcode")), 2), _                                            GetFileByte(String.Format("{0}\{1}_Rprimary.ansi-fmr", rw("Path"), rw("Barcode")), 3), _                                            GetFileByte(String.Format("{0}\{1}_Rbackup.ansi-fmr", rw("Path"), rw("Barcode")), 3), _                                            GetFileByte(String.Format("{0}\{1}_Lprimary.ansi-fmr", rw("Path"), rw("Barcode")), 3), _                                            GetFileByte(String.Format("{0}\{1}_Lbackup.ansi-fmr", rw("Path"), rw("Barcode")), 3)) Then                    IsSuccess = False                    intError += 1                    sb.AppendLine(String.Format("AddRelPOImage(): Barcode {0}, CRN {1}. Returned error {2}<br>", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, DAL.ErrorMessage))                End If            Else                IsSuccess = False                intError += 1                sb.AppendLine(String.Format("AddRelPOData(): Barcode {0}, CRN {1}. Returned error {2}", rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, DAL.ErrorMessage))            End If            If intPageSeriesCntr = SharedFunction.CardsPerSheet Then                intPageSeriesCntr = 1                intPageCntr += 1            Else                intPageSeriesCntr += 1            End If            If intBoxSeriesCntr = SharedFunction.CardsPerBox Then                intBoxSeriesCntr = 1                intBoxCntr += 1            Else                intBoxSeriesCntr += 1            End If        Next    End Sub

    Private Function DecryptData(ByVal strFile As String, ByRef outputFile As String) As String
        Dim response As String = SharedFunction.EncryptDecryptFile(strFile, False, outputFile)

        If response = "" Then
            Return ""
        Else
            Dim DAL As New DAL
            DAL.AddErrorLog("DecryptData(): " & response, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            DAL.Dispose()
            DAL = Nothing
            Return response
        End If
    End Function

    Private Function UnzipFile(ByVal strFile As String, ByVal outputDir As String) As Boolean
        Dim fc As New FileCompression
        Try
            'If Not fc.ExtractZipFile(strFile, String.Format("{0}\{1}", outputDir, IO.Path.GetFileNameWithoutExtension(strFile))) Then
            If Not fc.ExtractZipFile(strFile, outputDir) Then
                Dim DAL As New DAL
                DAL.AddErrorLog("UnzipFile(): fc.ExtractZipFile. Returned error" & fc.ErrorMessage, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                DAL.Dispose()
                DAL = Nothing

                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Dim DAL As New DAL
            DAL.AddErrorLog("UnzipFile(): Runtime error " & ex.Message, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            DAL.Dispose()
            DAL = Nothing

            Return False
        Finally
            fc = Nothing
        End Try
    End Function

    Private Sub UploadData(ByVal strPurchaseOrder As String, ByVal strBatch As String, ByVal strFile As String) ', ByVal ID2 As Integer)
        Dim outputFile As String = ""
        Dim response As String = DecryptData(String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strFile), outputFile)
        If response = "" Then
            If UnzipFile(outputFile, String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strPurchaseOrder)) Then
                ASPxMemo1.Visible = False

                Dim sb As New StringBuilder

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

                Dim MemberXML As MemberXML
                For Each strSubDir As String In Directory.GetDirectories(SharedFunction.ForUploadingRepository & "\" & strPurchaseOrder)
                    For Each strSubDir2 As String In Directory.GetDirectories(strSubDir)
                        Dim _barcode As String = strSubDir2.Substring(strSubDir2.LastIndexOf("\") + 1)

                        Try
                            Dim xmlFile As String = String.Format("{0}\{1}.xml", strSubDir2, _barcode)
                            Dim signatureFile As String = String.Format("{0}\{1}_Signature.tiff", strSubDir2, _barcode)
                            Dim photoFile As String = String.Format("{0}\{1}_Photo.jpg", strSubDir2, _barcode)

                            If File.Exists(xmlFile) Then
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
                                sb.AppendLine(String.Format("MemberXML(): No xml found for Barcode {0}.<br>", _barcode))
                            End If

                            If Not File.Exists(photoFile) Then
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML(): No photo found for Barcode {0}.<br>", _barcode))
                            End If

                            If Not File.Exists(signatureFile) Then
                                intError += 1
                                sb.AppendLine(String.Format("MemberXML(): No signature found for Barcode {0}.<br>", _barcode))
                            Else
                                If New FileInfo(signatureFile).Length = 0 Then
                                    intSignatureWithZeroByte += 1
                                    intError += 1
                                    sb.AppendLine(String.Format("MemberXML(): Signature is zero byte for Barcode {0}.<br>", _barcode))
                                End If
                            End If
                        Catch ex As Exception
                            intError += 1
                            sb.AppendLine(String.Format("Runtime error on xml reading of {0}\{1}.xml. Returned error {2}<br>", strSubDir2, _barcode, ex.Message))
                        End Try

                        MemberXML = Nothing
                    Next
                Next

                If intSignatureWithZeroByte > 0 Then
                    sb.AppendLine(String.Format("Total signature with zero byte: {0}.<br>", intSignatureWithZeroByte.ToString))
                End If

                sb.AppendLine(String.Format("End of XML reading {0}<br>", Now.ToString))

                Try
                    Dim intPageCntr As Short = 1
                    Dim intPageSeriesCntr As Short = 1

                    Dim intBoxCntr As Integer = 1
                    Dim intBoxSeriesCntr As Integer = 1

                    Dim IsSuccess As Boolean = True

                    Dim intDtlCntr As Integer = 0

                    sb.AppendLine(String.Format("Start of PO adding {0}<br>", Now.ToString))

                    Dim intPOID As Integer = 0
                    If DAL.AddPO(strPurchaseOrder, 0, strBatch) Then
                        If Not DAL.ObjectResult Is Nothing Then
                            If Not IsDBNull(DAL.ObjectResult) Then
                                intPOID = DAL.ObjectResult
                                If intPOID > 0 Then
                                    AddPODetails(DAL, dtData, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb)
                                    AddPODetails(DAL, dtDataReject, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb)
                                    AddPODetails(DAL, dtDataGood, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, DataKeysEnum.ActivityID.Done)

                                    'If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder)) Then
                                    If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strFile)) Then
                                        sb.AppendLine("Failed to delete PO " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")
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

                    'Dim _PO As New PurchaseOrder
                    '_PO.SaveToDownloadableFiles_Mailer(intPOID, strPurchaseOrder, strPurchaseOrder, "PurchaseOrder", SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & strPurchaseOrder, sb, intError)
                    '_PO = Nothing

                    If IsSuccess Then
                        'delete encrypted file
                        File.Delete(String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strFile))

                        'delete zip file
                        IO.File.Delete(outputFile)

                        DAL.AddSystemLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    Else
                        DAL.AddErrorLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    End If

                    DAL.Dispose()
                    DAL = Nothing

                    ASPxMemo1.Visible = True
                    ASPxMemo1.Text = sb.ToString.Replace("<br>", "")

                    GridDataBind()

                    'xGrid.Columns(5).Visible = True
                Catch ex As Exception
                End Try
            Else 'unzip failed
                ASPxMemo1.Visible = True
                ASPxMemo1.Text = "Unzipping process failed"
            End If
        Else 'decrypt failed
            ASPxMemo1.Visible = True
            ASPxMemo1.Text = "Decryption process failed. " & response
        End If
    End Sub

    Private Sub UploadExtractedData() ', ByVal ID2 As Integer)       
        Dim strPurchaseOrder As String = IO.File.ReadAllText(SharedFunction.ForUploadingRepository & "\ManualUpload.txt")
        Dim strBatch As String = strPurchaseOrder.Split("-")(3)

        ASPxMemo1.Visible = False

        Dim sb As New StringBuilder

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

        sb.AppendLine(String.Format("Start of XML reading {0}<br>", Now.ToString))        Dim intSignatureWithZeroByte As Integer = 0        Dim intLprimaryWithZeroByte As Integer = 0        Dim intLbackupWithZeroByte As Integer = 0        Dim intRprimaryWithZeroByte As Integer = 0        Dim intRbackupWithZeroByte As Integer = 0        Dim MemberXML As MemberXML        For Each strSubDir As String In Directory.GetDirectories(SharedFunction.ForUploadingRepository & "\" & strPurchaseOrder)            For Each strSubDir2 As String In Directory.GetDirectories(strSubDir)                Dim _barcode As String = strSubDir2.Substring(strSubDir2.LastIndexOf("\") + 1)                Try                    'critical                    Dim xmlFile As String = String.Format("{0}\{1}.xml", strSubDir2, _barcode)                    Dim photoFile As String = String.Format("{0}\{1}_Photo.jpg", strSubDir2, _barcode)                    'non critical                    Dim signatureFile As String = String.Format("{0}\{1}_Signature.tiff", strSubDir2, _barcode)                    'Dim xmlPersoFile As String = String.Format("{0}\{1}_perso.xml", strSubDir2, _barcode)                    Dim lprimaryFile As String = String.Format("{0}\{1}_Lprimary.ansi-fmr", strSubDir2, _barcode)                    Dim lbackupFile As String = String.Format("{0}\{1}_Lbackup.ansi-fmr", strSubDir2, _barcode)                    Dim rprimaryFile As String = String.Format("{0}\{1}_Rprimary.ansi-fmr", strSubDir2, _barcode)                    Dim rbackupFile As String = String.Format("{0}\{1}_Rbackup.ansi-fmr", strSubDir2, _barcode)                    If File.Exists(xmlFile) And File.Exists(photoFile) Then                        MemberXML = New MemberXML(xmlFile)                        If MemberXML.GetDataAndPopulate() Then                            If dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Reject'", MemberXML.Barcode)).Length > 0 Then                                AddToRow(MemberXML, strSubDir2, dtDataReject)                            ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Reject'", MemberXML.CRN)).Length > 0 Then                                AddToRow(MemberXML, strSubDir2, dtDataReject)                            ElseIf dtSSSReject.Select(String.Format("Barcode='{0}' AND Tag='Good'", MemberXML.Barcode)).Length > 0 Then                                AddToRow(MemberXML, strSubDir2, dtDataGood)                            ElseIf dtSSSReject.Select(String.Format("CRN='{0}' AND Tag='Good'", MemberXML.CRN)).Length > 0 Then                                AddToRow(MemberXML, strSubDir2, dtDataGood)                            Else                                AddToRow(MemberXML, strSubDir2, dtData)                            End If                        Else                            intError += 1                            sb.AppendLine(String.Format("MemberXML.GetDataAndPopulate(): Barcode {0}. Returned error {1}<br>", _barcode, MemberXML.ErrorMessage))                        End If                    Else                        intError += 1                        sb.AppendLine(String.Format("MemberXML(): No xml/photo found for Barcode {0}.<br>", _barcode))                    End If                    CheckIfFileExist(signatureFile, "Signature", _barcode, 0, sb, intSignatureWithZeroByte)                    CheckIfFileExist(lprimaryFile, "Lprimary", _barcode, 0, sb, intLprimaryWithZeroByte)                    CheckIfFileExist(lbackupFile, "Lbackup", _barcode, 0, sb, intLbackupWithZeroByte)                    CheckIfFileExist(rprimaryFile, "Rprimary", _barcode, 0, sb, intRprimaryWithZeroByte)                    CheckIfFileExist(rbackupFile, "Rbackup", _barcode, 0, sb, intRbackupWithZeroByte)                Catch ex As Exception                    intError += 1                    sb.AppendLine(String.Format("Runtime error on xml reading of {0}\{1}.xml. Returned error {2}<br>", strSubDir2, _barcode, ex.Message))                End Try                MemberXML = Nothing            Next        Next        If intSignatureWithZeroByte > 0 Then _            sb.AppendLine(String.Format("Total signature(s) with zero byte: {0}.<br>", intSignatureWithZeroByte.ToString))        If intLprimaryWithZeroByte > 0 Then _            sb.AppendLine(String.Format("Total lprimary with zero byte: {0}.<br>", intLprimaryWithZeroByte.ToString))        If intLbackupWithZeroByte > 0 Then _            sb.AppendLine(String.Format("Total lbackup with zero byte: {0}.<br>", intLbackupWithZeroByte.ToString))        If intRprimaryWithZeroByte > 0 Then _            sb.AppendLine(String.Format("Total rprimary with zero byte: {0}.<br>", intRprimaryWithZeroByte.ToString))        If intRbackupWithZeroByte > 0 Then _            sb.AppendLine(String.Format("Total rbackup with zero byte: {0}.<br>", intRbackupWithZeroByte.ToString))        sb.AppendLine(String.Format("End of XML reading {0}<br>", Now.ToString))        Try            Dim intPageCntr As Short = 1            Dim intPageSeriesCntr As Short = 1            Dim intBoxCntr As Integer = 1            Dim intBoxSeriesCntr As Integer = 1            Dim IsSuccess As Boolean = True            Dim intDtlCntr As Integer = 0            sb.AppendLine(String.Format("Start of PO adding {0}<br>", Now.ToString))            Dim intPOID As Integer = 0            If DAL.AddPO(strPurchaseOrder, 0, strBatch) Then                If Not DAL.ObjectResult Is Nothing Then                    If Not IsDBNull(DAL.ObjectResult) Then                        intPOID = DAL.ObjectResult                        If intPOID > 0 Then                            AddPODetails(DAL, dtData, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb)                            AddPODetails(DAL, dtDataReject, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb)                            AddPODetails(DAL, dtDataGood, intPOID, strBatch, intBoxCntr, intBoxSeriesCntr, intDtlCntr, intPageCntr, intPageSeriesCntr, IsSuccess, intError, sb, DataKeysEnum.ActivityID.Done)                            ''If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder)) Then                            'If Not DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strFile)) Then                            '    sb.AppendLine("Failed to delete PO " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")                            'End If                            If Not DAL.ExecuteQuery(String.Format("UPDATE tblPO SET Quantity={0} WHERE POID={1}", intDtlCntr.ToString, intPOID)) Then                                sb.AppendLine("Failed to update PO qty of " & strPurchaseOrder & ". Returned error " & DAL.ErrorMessage & "<br>")                            End If                        Else                            IsSuccess = False                            sb.AppendLine("POID is zero<br>")                        End If                    Else                        IsSuccess = False                        sb.AppendLine("POID is null<br>")                    End If                Else                    IsSuccess = False                    sb.AppendLine("POID is nothing<br>")                End If            Else                IsSuccess = False                sb.AppendLine(String.Format("AddPO(): PurchaseOrder {0}. Returned error {2}<br>", strPurchaseOrder, DAL.ErrorMessage))            End If            For Each rwSSSReject As DataRow In dtSSSReject.Rows                If Not DAL.UpdateSSSRejectByQAGRID(rwSSSReject("QAGRID"), strPurchaseOrder) Then                    intError += 1                    IsSuccess = False                    sb.AppendLine(String.Format("UpdateSSSRejectByQAGRID(): Failed to update QAGRID {0} for PO {1}. Returned error {2}<br>", rwSSSReject("QAGRID"), strPurchaseOrder, DAL.ErrorMessage))                End If            Next            sb.AppendLine(String.Format("End of PO adding {0}<br>", Now.ToString) & vbNewLine)            sb.AppendLine(String.Format("Error {0}<br>", intError.ToString) & vbNewLine)            sb.AppendLine(String.Format("End process {0}<br>", Now.ToString) & vbNewLine)            'Dim _PO As New PurchaseOrder            '_PO.SaveToDownloadableFiles_Mailer(intPOID, strPurchaseOrder, strPurchaseOrder, "PurchaseOrder", SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & strPurchaseOrder, sb, intError)            '_PO = Nothing            If IsSuccess Then                ''delete encrypted file                'File.Delete(String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strFile))                ''delete zip file                'IO.File.Delete(outputFile)                DAL.AddSystemLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))            Else                DAL.AddErrorLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))            End If            DAL.Dispose()            DAL = Nothing            ASPxMemo1.Visible = True            ASPxMemo1.Text = sb.ToString.Replace("<br>", "")            GridDataBind()            'xGrid.Columns(5).Visible = True        Catch ex As Exception        End Try

    End Sub

    Private Function GetFileByte(ByVal strFile As String, ByVal intType As Short) As Byte()        If File.Exists(strFile) Then            Return File.ReadAllBytes(strFile)        Else            Select Case intType                Case 1 'xml                    Return File.ReadAllBytes(SharedFunction.templateXML_File)                Case 2 'signature                    Return File.ReadAllBytes(SharedFunction.templateSignature_File)                Case 3 'ansi                    Return File.ReadAllBytes(SharedFunction.templateANSI_File)            End Select        End If    End Function    Private Sub CheckIfFileExist(ByVal strFile As String, ByVal strDesc As String, ByVal strBarcode As String, ByRef intError As Integer, ByRef sb As StringBuilder, ByRef intZeroByteCntr As Integer)        If Not File.Exists(strFile) Then            intError += 1            sb.AppendLine(String.Format("MemberXML(): No " & strDesc.ToLower & " found for Barcode {0}.<br>", strBarcode))        Else            If New FileInfo(strFile).Length = 0 Then                intZeroByteCntr += 1                intError += 1                sb.AppendLine(String.Format("MemberXML(): " & strDesc & " is zero byte for Barcode {0}.<br>", strBarcode))            End If        End If    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim dtForUpload As DataTable
        Dim dtUploaded As DataTable

        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblForUpload") Then
            dtForUpload = DAL.TableResult.Copy
        Else
            DAL.Dispose()
            DAL = Nothing
            Exit Sub
        End If

        If DAL.SelectQuery("SELECT PurchaseOrder FROM tblPO") Then
            dtUploaded = DAL.TableResult
        Else
            DAL.Dispose()
            DAL = Nothing
            Exit Sub
        End If

        'For Each strSubDir In Directory.GetDirectories(SharedFunction.ForUploadingRepository)
        For Each strFile In Directory.GetFiles(SharedFunction.ForUploadingRepository)
            Try
                'Dim PO As String = strSubDir.Substring(strSubDir.LastIndexOf("\") + 1)
                Dim orig_file As String = Path.GetFileName(strFile)
                Dim PO As String = Path.GetFileNameWithoutExtension(strFile).Substring(0, 33)

                If orig_file.Contains("_zip.sss") Then
                    If Not IsPOExist(PO, dtUploaded) Then
                        If Not IsPOExist(orig_file, dtForUpload) Then
                            Dim rw As DataRow = dtForUpload.NewRow
                            rw("PurchaseOrder") = orig_file 'PO
                            rw("Batch") = PO.Split("-")(3)
                            rw("DateTimePosted") = File.GetCreationTime(strFile)
                            rw("Quantity") = 0 'GetTotalFolder(strSubDir)
                            dtForUpload.Rows.Add(rw)

                            If DAL.AddForUpload(rw("PurchaseOrder"), rw("Batch"), rw("DateTimePosted"), rw("Quantity")) Then
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                DAL.AddErrorLog("PopulatingFolder: Error encountered in " & Path.GetFileName(strFile), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End Try
        Next

        xGrid.DataSource = dtForUpload

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Function IsPOExist(ByVal strPO As String, ByVal dt As DataTable) As Boolean
        If dt.Select("PurchaseOrder='" & strPO & "'").Length = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_ForUpload"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("FOR UPLOAD")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'UploadExtractedData()

        Dim strPurchaseOrder As String = IO.File.ReadAllText(SharedFunction.ForUploadingRepository & "\ManualUpload.txt")
        Dim strBatch As String = strPurchaseOrder.Split("-")(3)

        ASPxMemo1.Visible = False

        Dim sb As New StringBuilder
        If SharedFunction.UploadData_v2(strPurchaseOrder, strBatch, "", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name), sb) Then
            ''delete encrypted file
            'File.Delete(String.Format("{0}\{1}", SharedFunction.ForUploadingRepository, strFile))

            ''delete zip file
            'IO.File.Delete(outputFile)
        End If

        ASPxMemo1.Visible = True        ASPxMemo1.Text = sb.ToString.Replace("<br>", "")        GridDataBind()
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs)
        Try
            Dim currentButton As LinkButton = TryCast(sender, LinkButton)
            Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
            Dim strPurchaseOrder As String = xGrid.GetRowValues(index, "PurchaseOrder")
            Dim DAL As New DAL
            DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder))
            DAL.Dispose()
            DAL = Nothing
        Catch ex As Exception
        End Try

        BindGrid()
    End Sub

End Class
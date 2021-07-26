
Imports System.IO

Public Class IndigoExtract
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.IndigoExtract) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()
            GridDataBind()

            btnSubmit.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Indigo)
        End If

        If Not Session("selTotal") Is Nothing Then Label1.Text = Session("selTotal").ToString
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectReprocessAndNewUploadPO(cboSelect.SelectedIndex) Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim strIDs As String = GetSelectedItems_POID()

        If strIDs = "" Then
            lblStatus.Text = "Please select item(s) to process"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            If GetPrintable() = 0 Then
                lblStatus.Text = "Printable is zero no data to process"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                Label1.Text = Session("selTotal").ToString
            Else
                ProcessSubmitted(strIDs)
            End If
        End If

        GridDataBind()
    End Sub

    Private Function GetSelectedItems_POID() As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGrid.GetSelectedFieldValues("POID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            End If
        Next

        Return sb.ToString
    End Function

    Private Function GetSelectedItems_PurchaseOrder() As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGrid.GetSelectedFieldValues("PurchaseOrder").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("PurchaseOrder")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("PurchaseOrder")(i)))
            End If
        Next

        Return sb.ToString
    End Function

    Private Function GetSelectedItems_BatchAndQty() As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGrid.GetSelectedFieldValues("POID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("New-Batch{0} Qty{1}", xGrid.GetSelectedFieldValues("Batch")(i).ToString.Trim, xGrid.GetSelectedFieldValues("Quantity")(i).ToString.Trim))
            Else
                sb.Append(String.Format(",New-Batch{0} Qty{1}", xGrid.GetSelectedFieldValues("Batch")(i).ToString.Trim, xGrid.GetSelectedFieldValues("Quantity")(i).ToString.Trim))
            End If
        Next

        Return sb.ToString
    End Function

    Private Function GetPrintable() As Integer
        Dim intTotal As Integer = 0
        Dim intDivisibleBy21 As Integer = 0
        Dim strQuotient As String
        Dim intDrop As Integer = 0

        For i As Short = 0 To xGrid.GetSelectedFieldValues("POID").Count - 1
            intTotal += CInt(xGrid.GetSelectedFieldValues("Quantity")(i))
        Next

        'intTotal = 5

        If intTotal > 0 Then
            If intTotal < 21 Then
                If chkPrintAll.Checked Then
                    intDivisibleBy21 = intTotal
                Else
                    intDrop = intTotal
                End If
            Else
                If chkPrintAll.Checked Then
                    intDivisibleBy21 = intTotal
                Else
                    strQuotient = (intTotal / 21).ToString
                    If strQuotient.Contains(".") Then
                        Dim intDivisor As Integer = strQuotient.Split(".")(0)
                        intDivisibleBy21 = 21 * intDivisor
                        intDrop = intTotal - intDivisibleBy21
                    Else
                        intDivisibleBy21 = intTotal
                    End If
                End If
            End If
        End If

        Session("selTotal") = String.Format("Total: {0}<br><br> Printable: {1} For Drop: {2}", intTotal.ToString, intDivisibleBy21, intDrop)

        Return intDivisibleBy21
    End Function

    Private Sub ProcessSubmitted(ByVal strIDs As String)
        Dim DAL As New DAL
        Dim sb As New StringBuilder

        Dim intError As Integer = 0
        Dim intSignatureError As Integer = 0
        Dim intBarcodeError As Integer = 0
        Dim intSheetBarcodeError As Integer = 0

        Dim strTempOutputDir As String = ""

        Try
            Dim sbSubmitted As New StringBuilder
            sb.AppendLine(String.Format("Start of ProcessSubmitted() {0}", Now.ToString))
            sb.AppendLine(String.Format("{0}", GetSelectedItems_PurchaseOrder))

            If DAL.SelectDataForStatusUpdatev2(strIDs, cboSelect.SelectedIndex) Then
                Dim dt As DataTable = DAL.TableResult

                Dim intPrintable As Integer = GetPrintable()
                SharedFunction.ReComputePageAndSeries(dt, intPrintable)

                Dim dtForPrinting As DataTable = Nothing
                Dim dtDropped As DataTable = Nothing

                If dt.Select("CurrentSeries <= " & intPrintable).Length > 0 Then
                    dtForPrinting = dt.Select("CurrentSeries <= " & intPrintable).CopyToDataTable
                    'for printing
                    For Each rw As DataRow In dtForPrinting.Rows
                        'If sbSubmitted.ToString = "" Then
                        '    sbSubmitted.Append(rw("CardID"))
                        'Else
                        '    sbSubmitted.Append("," & rw("CardID"))
                        'End If

                        'uncomment
                        If Not DAL.IndigoExtract(rw("CardID"), DataKeysEnum.ActivityID.Indigo, rw("CurrentPage"), rw("CurrentSeries")) Then
                            intError += 1
                            sb.AppendLine(String.Format("Failed to update CardID {0}. Error encountered: {1}", rw("CardID"), DAL.ErrorMessage))
                        Else
                            DAL.AddCardActivity(rw("POID"), rw("CRN"), rw("Barcode"), String.Format("{0} changed the status of record from {1} to {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(rw("ActivityID")), SharedFunction.GetActivityDesc(SharedFunction.GetNextActivity(rw("ActivityID")))), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))
                        End If
                    Next
                End If

                If dt.Select("CurrentSeries > " & intPrintable).Length > 0 Then
                    dtDropped = dt.Select("CurrentSeries > " & intPrintable).CopyToDataTable                    '

                    For Each rw As DataRow In dtDropped.Rows
                        sb.AppendLine(String.Format("CardID {0} Barcode {1} CRN {2} is dropped", rw("CardID"), rw("Barcode"), rw("CRN")))
                        DAL.ExecuteQuery(String.Format("UPDATE dbo.tblRelPOData SET ActivityID = 1 WHERE CardID = {0}", rw("CardID")))
                    Next
                End If

                If Not dtForPrinting Is Nothing Then
                    SharedFunction.GenerateSheetBarcode(dtForPrinting)

                    'uncomment
                    For Each rw As DataRow In dtForPrinting.Select("CRN1<>''")
                        DAL.AddRelPODataSheetBarcode(rw("CRN1"), rw("CRN2"), rw("FrontSheetBarcode"), rw("BackSheetBarcode"))
                    Next

                    Dim _PO As New PurchaseOrder
                    '_PO.SaveToDownloadableFiles_ForIndigov2(strIDs, GetSelectedItems_PurchaseOrder, GetSelectedItems_BatchAndQty(), "IndigoExtract", SharedFunction.UserID(Page.User.Identity.Name), intPrintable, SharedFunction.IndigoExtractRepository, sbSubmitted.ToString, sb, intError, intSignatureError, intBarcodeError, intSheetBarcodeError)
                    Dim IsSSSUBP As Boolean = False
                    If cboSelect.SelectedIndex = 2 Then IsSSSUBP = True
                    _PO.SaveToDownloadableFiles_ForIndigov3(dtForPrinting, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.IndigoExtractRepository, sb, intError, intSignatureError, intBarcodeError, intSheetBarcodeError, strTempOutputDir, IsSSSUBP)
                    _PO = Nothing
                End If

                Try
                    'uncomment
                    If Not DAL.TagPOAsExtracted(strIDs) Then
                        intError += 1
                        sb.AppendLine(String.Format("TagPOAsExtracted(): Returned error {0}", DAL.ErrorMessage))
                    End If
                Catch ex As Exception
                End Try
            Else
                intError += 1
                sb.AppendLine(String.Format("SelectDataForStatusUpdate(): Returned error {0}", DAL.ErrorMessage))
            End If
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("ProcessSubmitted(): Runtime error encountered {0}", ex.Message))

            lblStatus.Text = "Error processing data"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Finally
            sb.AppendLine(String.Format("End of ProcessSubmitted() process {0}", Now.ToString))

            If Directory.Exists(strTempOutputDir) Then _
                File.WriteAllText(String.Format("{0}\ProcessLog.txt", strTempOutputDir), sb.ToString)

            'Using sr = New StreamWriter(String.Format("{0}\{1}.txt", SharedFunction.LogRepository, "Log"), True)
            '    sr.WriteLine(sb.ToString)
            'End Using

            If intError = 0 Then
                DAL.AddSystemLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                lblStatus.Text = "Process is done"
                lblStatus.ForeColor = SharedFunction.SuccessColor
            Else
                DAL.AddErrorLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                lblStatus.Text = String.Format("Process is done with {0} total error(s) encountered, Signature Error {1}, Barcode error {2}, Sheet barcode error {3}", intError.ToString, intSignatureError.ToString, intBarcodeError.ToString, intSheetbarcodeError.ToString)
                lblStatus.ForeColor = SharedFunction.ErrorColor
            End If

            DAL.Dispose()
            DAL = Nothing
            sb = Nothing
        End Try
    End Sub

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

    Protected Sub btnViewData_Click(sender As Object, e As EventArgs) Handles btnViewData.Click
        Dim strIDs As String = GetSelectedItems_POID()

        If strIDs = "" Then
            lblStatus.Text = "Please select item(s) to process"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            Session("DataForPrinting_POID") = String.Format(" AND dbo.tblPO.POID IN ({0})", GetSelectedItems_POID())

            ASPxPopupControl1.Height = 600
            ASPxPopupControl1.Width = 1400
            ASPxPopupControl1.ShowOnPageLoad = True

            'GridDataBind()

            Session("Printable") = GetPrintable()

            Label1.Text = Session("selTotal").ToString
        End If

        GridDataBind()
    End Sub

    Private Sub xGrid_HtmlDataCellPrepared(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs) Handles xGrid.HtmlDataCellPrepared
        If e.DataColumn.FieldName = "Type" Then
            If e.CellValue.ToString.Trim = "New upload!" Then
                e.Cell.ForeColor = Drawing.Color.OrangeRed
            End If
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_IndigoForExtraction"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("INDIGO FOR EXTRACTION")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub cboSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSelect.SelectedIndexChanged
        Try
            GridDataBind()

            Select Case cboSelect.SelectedIndex
                Case 0
                    'xGrid.Columns(6).Visible = False
                    btnSubmit.Enabled = False
                Case Else
                    'xGrid.Columns(6).Visible = True
                    btnSubmit.Enabled = True
            End Select
        Catch ex As Exception
        End Try
    End Sub

    '    orig data
    '9011-1954887-0 	0120181211C6ID008009	01-20181227-P-262-0042-C-001-0040	2018-12-28	000	2019-01-08


    'test data
    '9011-1954887-0 	0120190103D1ID155071	01-20190104-P-006-7816-C-002-0379	2018-12-28	909	2019-01-08

End Class
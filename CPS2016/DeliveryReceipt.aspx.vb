
Public Class DeliveryReceipt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.DeliveryReceipt) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            PopulatePurchaseOrder()

            If Not Session("DR_Param") Is Nothing Then
                Dim param As String = Session("DR_Param")
                cboPO.Text = param.Split("|")(0)
                dateEdit.Text = param.Split("|")(1)
                cboDisplay.SelectedIndex = param.Split("|")(2)
                Search()
            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Search
    End Sub

    Private Sub Search()
        lblStatus.ForeColor = Drawing.Color.Black
        lblStatus.Text = ""

        If cboPO.Text = "" Then
            lblStatus.Text = "Please enter Purchase Order"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            If Not cboPO.Text.Contains("-") Then
                lblStatus.Text = "Invalid Purchase Order format"
                lblStatus.ForeColor = SharedFunction.ErrorColor
            Else
                GridDataBind()
            End If
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        Dim dt As DataTable

        Try
            If dateEdit.Text = "" Then
                'If DAL.SelectDRListByActivePO(cboPO.Text.Trim) Then
                If DAL.SelectDRListByActivePOByDisplay(cboPO.Text.Trim, cboDisplay.SelectedValue) Then
                    dt = DAL.TableResult
                End If
            Else
                'If DAL.SelectDRListByActivePOAndPersoDate(cboPO.Text.Trim, CDate(dateEdit.Value)) Then
                If DAL.SelectDRListByActivePOAndPersoDateByDisplay(cboPO.Text.Trim, CDate(dateEdit.Value), cboDisplay.SelectedValue) Then
                    dt = DAL.TableResult
                End If
            End If
        Catch ex As Exception
            lblStatus.Text = "An error has occurred"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            DAL.AddErrorLog(String.Format("{0} - failed to extract DR List ", cboPO.Text.Trim), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Return
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try

        If dt Is Nothing Then
            lblStatus.Text = "Table is empty"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Return
        End If

        Dim dt2 As DataTable
        If dt.DefaultView.Count > 0 Then
            lbExcluded.Enabled = True
        Else
            lbExcluded.Enabled = False
        End If

        Dim intBarcodeDuplicate As Integer = 0

        If dt.DefaultView.Count = 0 Then
            dt2 = dt
        ElseIf dt.DefaultView.Count = 1 Then
            dt2 = dt
        Else
            Dim dups = From row In dt.AsEnumerable()
                       Let Barcode = row.Field(Of String)("Barcode")
                       Group row By Barcode Into DupBarcode = Group
                       Where DupBarcode.Count() > 1
                       Select DupBarcode

            'Dim intCntr As Short = 0

            Dim dtBarcodeDuplicate As New DataTable
            dtBarcodeDuplicate.Columns.Add("Barcode", Type.GetType("System.String"))
            dtBarcodeDuplicate.Columns.Add("Count", Type.GetType("System.Int16"))

            For Each dupBarcodeRows In dups
                Dim intCntr As Short = 1
                For Each row In dupBarcodeRows
                    If dtBarcodeDuplicate.Select("Barcode='" & row.Field(Of String)("Barcode") & "'").Length = 0 Then
                        Dim rw As DataRow = dtBarcodeDuplicate.NewRow
                        rw("Barcode") = row.Field(Of String)("Barcode")
                        dtBarcodeDuplicate.Rows.Add(rw)
                        intCntr += 1
                    Else
                        dtBarcodeDuplicate.Select("Barcode='" & row.Field(Of String)("Barcode") & "'")(0)(1) = intCntr
                        intCntr += 1
                    End If
                Next
            Next

            For Each dupBarcodeRows In dups
                Dim intCntr As Short = 1
                For Each row In dupBarcodeRows
                    If dtBarcodeDuplicate.Select("Barcode='" & row.Field(Of String)("Barcode") & "'")(0)(1) <> intCntr Then
                        row("Barcode") = row.Field(Of String)("Barcode") & "^"
                        'intBarcodeDuplicate += 1
                        intCntr += 1
                    Else
                        row("Barcode") = row.Field(Of String)("Barcode") & "**"
                        intBarcodeDuplicate += 1
                        intCntr += 1
                    End If
                Next
            Next

            dt2 = dt.Select("Barcode NOT LIKE '%^'").CopyToDataTable
        End If

        xGrid.DataSource = dt2

        lbViewDownloadTXT.Visible = False
        lbViewDownloadPDF.Visible = False
        If dt.DefaultView.Count = 0 Then
            LinkButton1.Visible = False
        Else
            LinkButton1.Visible = True
        End If

        Session("dtGrid") = dt2

        Dim intPOQty As Integer = 0
        If dt2.DefaultView.Count > 0 Then intPOQty = dt2.Rows(0)("Quantity").ToString
        Dim intBarcodeDistinct As Integer = IIf(dt.DefaultView.Count = 0, 0, dt.DefaultView.Count - intBarcodeDuplicate)
        Dim intBarcodeWithSerial As Integer = IIf(dt.DefaultView.Count = 0, 0, dt.Select("ChipSerialNo NOT IS NULL").Length - intBarcodeDuplicate)

        'lblTotal.Text = String.Format("PO Quantity: {0},        With Chip Serial: {1},        Missing: {2},        Duplicate: {3}", intBarcodeDistinct.ToString, intBarcodeWithSerial.ToString, (intBarcodeDistinct - intBarcodeWithSerial).ToString, intBarcodeDuplicate.ToString)
        lblTotal.Text = String.Format("PO Quantity: {0},        With Chip Serial: {1},        Missing: {2},        Duplicate: {3}", intPOQty, intBarcodeWithSerial.ToString, (intBarcodeDistinct - intBarcodeWithSerial).ToString, 0)
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Sub PopulatePurchaseOrder()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT PurchaseOrder FROM tblPO ORDER BY CAST(SUBSTRING(PurchaseOrder,15,3) as int)") Then
            cboPO.Items.Add("-Select-", 0)
            cboPO.SelectedIndex = 0

            For Each rw As DataRow In DAL.TableResult.Rows
                cboPO.Items.Add(rw("PurchaseOrder"), rw("PurchaseOrder"))
            Next
        Else
            cboPO.Text = "Unable to populate data"
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub ExtractAndDownload()
        Dim _rn1 As New Random
        Dim _rn2 As New Random

        Dim dtDRExcludedBarcodes As DataTable
        If Not Session("dtDRExcludedBarcodes") Is Nothing Then dtDRExcludedBarcodes = CType(Session("dtDRExcludedBarcodes"), DataTable)

        Dim intCntr As Integer = 0
        Dim sb As New StringBuilder
        Dim sbDR_Summary As New StringBuilder
        Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

        Dim sbInvalidDate As New StringBuilder

        Dim DAL As New DAL

        For Each rw As DataRow In dt.Rows
            If rw("Barcode").ToString.Trim <> rw("Barcode").ToString.Trim.Replace("*", "") & "*" Then
                Dim bln As Boolean = False

                If Not dtDRExcludedBarcodes Is Nothing Then _
                    If dtDRExcludedBarcodes.Select("Barcode='" & rw("Barcode").ToString.Trim & "'").Length > 0 Then bln = True

                If Not bln Then
                    If Not IsDBNull(rw("UID")) Then
                        'sb.AppendLine(String.Format("{0},{1},{2}", rw("Barcode").ToString.Trim, rw("ChipSerialNo").ToString.Trim, rw("CardDate").ToString.Trim))
                        If Not IsDate(rw("CardDate")) Then
                            sbInvalidDate.Append(String.Format("Invalid date {0},{1}", rw("Barcode").ToString.Trim, rw("CardDate").ToString))
                        Else
                            sb.AppendLine(String.Format("{0},{1},{2}", rw("Barcode").ToString.Trim.Replace("*", ""), rw("UID").ToString.Trim.PadLeft(32, "0"), CDate(rw("CardDate")).ToString("ddMMyyyy")))

                            If sbDR_Summary.ToString = "" Then
                                sbDR_Summary.Append(String.Format("'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                            Else
                                sbDR_Summary.Append(String.Format(",'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                            End If
                        End If

                        DAL.UpdateRelCDFRDataByPOAndBarcode(rw("Barcode").ToString.Trim.Replace("*", ""), cboPO.Text.Trim)

                        intCntr += 1
                    Else
                        If chkCompleteDR.Checked Then
                            Dim chipserial As String = "0000000000000000" & _rn1.Next(12345678, 99999999).ToString.PadLeft(8, "0") & _rn2.Next(12345678, 99999999).ToString.PadLeft(8, "0")
                            sb.AppendLine(String.Format("{0},{1},{2}", rw("Barcode").ToString.Trim.Replace("*", ""), chipserial, Now.ToString("ddMMyyyy")))
                            intCntr += 1

                            If sbDR_Summary.ToString = "" Then
                                sbDR_Summary.Append(String.Format("'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                            Else
                                sbDR_Summary.Append(String.Format(",'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                            End If
                        End If
                    End If
                End If
            End If
        Next

        If Not IO.Directory.Exists(SharedFunction.ReportsRepository & "\" & cboPO.Text.Trim) Then IO.Directory.CreateDirectory(SharedFunction.ReportsRepository & "\" & cboPO.Text.Trim)

        Dim sbError As New StringBuilder
        Dim strFile As String = String.Format("{0}\{1}\{2}.txt", SharedFunction.ReportsRepository, cboPO.Text.Trim, cboPO.Text.Trim)
        IO.File.WriteAllText(strFile, sb.ToString)

        If sbInvalidDate.ToString <> "" Then sbError.Append(sbInvalidDate.ToString)


        If sbError.ToString = "" Then
            If IO.File.Exists(strFile) Then
                Session("drTxtFile") = strFile
                DAL.UpdateRelPOReportByPOAndReportTypeID(cboPO.Text, DataKeysEnum.Report.DeliveryReceipt, IO.File.ReadAllBytes(strFile))
                DAL.AddSystemLog(String.Format("{0} generated PO {1} dr", SharedFunction.UserCompleteName(Page.User.Identity.Name), cboPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                'Dim result As String = GenerateDR_PDF(cboPO.Text, intCntr, sb.ToString)
                'If result = "Error" Then
                '    DAL.AddErrorLog(String.Format("ExtractAndDownload(): GenerateDR_PDF - Failed to generate dr pdf report of " & cboPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                'ElseIf result = "" Then
                '    DAL.AddErrorLog(String.Format("ExtractAndDownload(): GenerateDR_PDF - Failed to generate dr pdf report of " & cboPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                'Else
                '    Session("drPdfFile") = result
                '    DAL.AddSystemLog(String.Format("{0} generated PO {1} dr", SharedFunction.UserCompleteName(Page.User.Identity.Name), cboPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                'End If

                'DAL.Dispose()
                'DAL = Nothing

                If Not Session("dtDRExcludedBarcodes") Is Nothing Then _
                    SharedFunction.SaveToTextfile(String.Format("{0}\BarcodeExcluded_Qty{1}_{2}_{3}_{4}.txt", SharedFunction.ProcessLogRepository, CType(Session("dtDRExcludedBarcodes"), DataTable).DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt"), SharedFunction.UserCompleteName(Page.User.Identity.Name), cboPO.Text), Session("txtDRExcludedBarcodes").ToString, New StringBuilder)

                'for dr summary report sessions
                Session("drParameters") = String.Format("{0}|{1}|{2}", cboPO.Text, intCntr.ToString, sbDR_Summary.ToString)

                Session("txtDRExcludedBarcodes") = Nothing
                Session("dtDRExcludedBarcodes") = Nothing

                'ViewDownloadFile(strFile)
                'ViewDownloadFile(result)
                'HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
                'Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
                'HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
                'Dim Dfile As New System.IO.FileInfo(strFile)
                'HttpContext.Current.Response.WriteFile(Dfile.FullName)
                'HttpContext.Current.Response.[End]()
            End If
        Else
            lblStatus.Text = sbError.ToString
            lblStatus.ForeColor = SharedFunction.ErrorColor
            DAL.AddErrorLog(String.Format("{0} failed to generate PO {1} dr due to " & sbError.ToString, SharedFunction.UserCompleteName(Page.User.Identity.Name), cboPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        End If

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub ViewDownloadFile(ByVal strFile As String)
        HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
        Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
        HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
        Dim Dfile As New System.IO.FileInfo(strFile)
        HttpContext.Current.Response.WriteFile(Dfile.FullName)
        HttpContext.Current.Response.[End]()
    End Sub

    Private Function GenerateDR_PDF(ByVal strPurchaseOrder As String, ByVal intQty As Integer, ByVal strData As String) As String
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If rg.GenerateReport(DataKeysEnum.Report.DeliveryReceipt_PDF, 1, outputFile, strPurchaseOrder, intQty, strData) Then
                Return outputFile
            Else
                Return "Error"
            End If
        Catch ex As Exception
            Return "Error"
        Finally
            rg = Nothing
        End Try
    End Function

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        ExtractAndDownload()
        lbViewDownloadTXT.Visible = True
        lbViewGenerateSummaryPDF.Visible = True
        'lbViewDownloadPDF.Visible = True
    End Sub

    Protected Sub lbViewDownloadTXT_Click(sender As Object, e As EventArgs) Handles lbViewDownloadTXT.Click
        If Not Session("drTxtFile") Is Nothing Then ViewDownloadFile(Session("drTxtFile").ToString)
    End Sub

    Protected Sub lbViewDownloadPDF_Click(sender As Object, e As EventArgs) Handles lbViewDownloadPDF.Click
        If Not Session("drPdfFile") Is Nothing Then ViewDownloadFile(Session("drPdfFile").ToString)
    End Sub

    Private Function ReportName() As String
        Return "CPS_DeliveryReceipt"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine(cboPO.Text & " Delivery Receipt")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles lbExcluded.Click
        Session("DR_Param") = String.Format("{0}|{1}|{2}", cboPO.Text, dateEdit.Text, cboDisplay.SelectedIndex)
        'Session("OriginatingPage") = Request.Url.AbsolutePath
        'Response.Redirect("DeliveryReceiptExcludedList.aspx")

        ASPxPopupControl1.ContentUrl = "~/DeliveryReceiptExcludedList.aspx"

        ASPxPopupControl1.Height = 650
        ASPxPopupControl1.Width = 700
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub lbViewGenerateSummaryPDF_Click(sender As Object, e As EventArgs) Handles lbViewGenerateSummaryPDF.Click
        ASPxPopupControl1.ContentUrl = "~/ReportD_DeliveryReceipt.aspx"
        ASPxPopupControl1.Height = 650
        ASPxPopupControl1.Width = 700
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

End Class
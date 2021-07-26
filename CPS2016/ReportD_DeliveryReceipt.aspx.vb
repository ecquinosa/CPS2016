
Public Class ReportD_DeliveryReceipt
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

            txtDRNumber.Text = (SharedFunction.GetDR_PDF_DOCCNTR + 1).ToString.PadLeft(6, "0")

            If Not Session("drParameters") Is Nothing Then
                Button1.Enabled = True
                'Session("drParameters") = String.Format("{0}|{1}|{2}", cboPO.Text, sb.Length, sbDR_Summary.ToString)
                Dim parameters As String = Session("drParameters").ToString
                cboPO.Text = parameters.Split("|")(0)
                BindPODetails()

                txtTotalCardsDelivered.Text = parameters.Split("|")(1)

                Dim DAL As New DAL
                If DAL.GetPurchaseOrderCMSReprint(cboPO.Text, parameters.Split("|")(2)) Then
                    txtQtyReprinted.Text = CInt(DAL.ObjectResult)
                End If
                DAL.Dispose()
                DAL = Nothing

                Compute()
            End If
        End If
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

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        Dim strDocCntr As String = String.Format("DR{0}{1}", Now.ToString("yyyyMM"), (SharedFunction.GetDR_PDF_DOCCNTR + 1).ToString.PadLeft(4, "0"))
        If rg.GenerateReport(DataKeysEnum.Report.DeliveryReceipt_PDF, 1, outputFile, cboPO.Text.Trim, txtPODate.Text, txtDescription.Text, txtQtyOrdered.Text, txtQtyReprinted.Text, txtTotalCardsDelivered.Text, txtBoxes.Text, strDocCntr) Then
            If IO.File.Exists(outputFile) Then
                'DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate DeliveryReceipt_PDF report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), cboPO.Text.Trim), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find DeliveryReceipt_PDF report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing

            IO.File.WriteAllText(SharedFunction.DR_PDF_DOCCNTR, SharedFunction.GetDR_PDF_DOCCNTR + 1)

            Session("pdfFile") = outputFile
            Response.Redirect("~/Rpt/PDFViewer.aspx")
        Else
            lblStatus.Text = "Unable to generate report"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            WriteToErrorLog(rg.ErrorMessage)

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    'Private Sub CreateSummaryOfInitialPrintAndReprintRequest(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Status As String, ByVal Description As String, ByVal JVConfirmedDate As String, ByVal JVConfirmedBy As String)
    'Dim rg As New RptGenerator
    'Dim outputFile As String = ""
    'Dim DAL As New DAL
    '    If rg.GenerateReport(DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest, 1, outputFile, PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, Status, Description, JVConfirmedDate, JVConfirmedBy) Then
    '        If IO.File.Exists(outputFile) Then
    '            DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
    '            DAL.AddSystemLog(String.Format("{0} generate SummaryOfInitialPrintAndReprintRequest report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
    '        Else
    '            lblStatus.Text = "Unable to find report"
    '            lblStatus.ForeColor = SharedFunction.ErrorColor

    '            DAL.AddErrorLog("Unable to find SummaryOfInitialPrintAndReprintRequest report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
    '        End If

    '        rg = Nothing
    '        DAL.Dispose()
    '        DAL = Nothing

    '        Session("pdfFile") = outputFile
    '        Response.Redirect("~/Rpt/PDFViewer.aspx")
    '    Else
    '        lblStatus.Text = "Unable to generate report"
    '        lblStatus.ForeColor = SharedFunction.ErrorColor

    '        WriteToErrorLog(rg.ErrorMessage)

    '        rg = Nothing
    '        DAL.Dispose()
    '        DAL = Nothing
    '    End If
    'End Sub

    Private Sub WriteToErrorLog(ByVal strError As String)
        Dim DAL As New DAL
        DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub btnCompute_Click(sender As Object, e As EventArgs) Handles btnCompute.Click
        Compute()
    End Sub

    Private Sub Compute()
        lblStatus.Text = ""

        Dim intTotalCardsDelivered As Integer = txtTotalCardsDelivered.Text
        Dim strDivisibleBy210 As String = intTotalCardsDelivered / 210

        If Not strDivisibleBy210.Contains(".") Then
            txtBoxes.Text = strDivisibleBy210
        Else
            txtBoxes.Text = (CInt(strDivisibleBy210.Split(".")(0)) + 1)
        End If
    End Sub

    Protected Sub cboPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPO.SelectedIndexChanged
        If cboPO.Text = "" Then
            Button1.Enabled = False
            Exit Sub
        Else
            Button1.Enabled = True
        End If

        BindPODetails
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtTotalCardsDelivered.Text = 0 Then
            lblStatus.Text = "Please enter card/s for delivery"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf txtTotalCardsDelivered.Text = "" Then
            lblStatus.Text = "Please enter card/s for delivery"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf txtBoxes.Text = 0 Then
            lblStatus.Text = "Please enter # of box"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf txtBoxes.Text = "" Then
            lblStatus.Text = "Please enter # of box"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf txtQtyOrdered.Text = 0 Then
            lblStatus.Text = "Please enter card/s for delivery"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf txtQtyOrdered.Text = "" Then
            lblStatus.Text = "Please enter card/s for delivery"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf CInt(txtTotalCardsDelivered.Text) > CInt(txtQtyOrdered.Text) Then
            lblStatus.Text = "Card(s) delivered cannot be greater than quantity ordered"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf txtDRNumber.Text = "" Then
            lblStatus.Text = "Please enter DR Number"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        ElseIf Not IsNumeric(txtDRNumber.Text) Then
            lblStatus.Text = "Please enter numeric DR Number"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Exit Sub
        End If

        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        'Dim strDocCntr As String = String.Format("DR{0}{1}", Now.ToString("yyyyMM"), (SharedFunction.GetDR_PDF_DOCCNTR + 1).ToString.PadLeft(4, "0"))
        'Dim strDocCntr As String = String.Format("DR{0}{1}", Now.ToString("yyyy"), txtDRNumber.Text.PadLeft(6, "0"))
        Dim strDocCntr As String = String.Format("DR{0}{1}", Now.ToString("yyyy"), (SharedFunction.GetDR_PDF_DOCCNTR + 1).ToString.PadLeft(6, "0"))
        'Dim strDocCntr As String = txtDRNumber.Text.PadLeft(12, "0")
        If rg.GenerateReport(DataKeysEnum.Report.DeliveryReceipt_PDF, 1, outputFile, cboPO.Text.Trim, txtPODate.Text, txtDescription.Text, txtQtyOrdered.Text, txtQtyReprinted.Text, txtTotalCardsDelivered.Text, txtBoxes.Text, strDocCntr) Then
            If IO.File.Exists(outputFile) Then
                'DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate DeliveryReceipt_PDF {1} report for PurchaseOrder {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), strDocCntr, cboPO.Text.Trim), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find DeliveryReceipt_PDF report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing

            IO.File.WriteAllText(SharedFunction.DR_PDF_DOCCNTR, SharedFunction.GetDR_PDF_DOCCNTR + 1)

            Session("pdfFile") = outputFile
            Response.Redirect("~/Rpt/PDFViewer.aspx")
        Else
            lblStatus.Text = "Unable to generate report"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            WriteToErrorLog(rg.ErrorMessage)

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Protected Sub cboPO_TextChanged(sender As Object, e As EventArgs) Handles cboPO.TextChanged
        If cboPO.Text = "" Then
            Button1.Enabled = False
            Exit Sub
        Else
            Button1.Enabled = True
        End If

        BindPODetails
    End Sub

    Private Sub BindPODetails()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT Quantity, DateTimePosted FROM tblPO WHERE PurchaseOrder='" & cboPO.Text & "'") Then
            If DAL.TableResult.DefaultView.Count > 0 Then
                txtPODate.Text = DAL.TableResult.Rows(0)("DateTimePosted")
                txtQtyOrdered.Text = DAL.TableResult.Rows(0)("Quantity")
                txtDescription.Text = "Delivery for PO#" & cboPO.Text
                txtTotalCardsDelivered.Text = txtQtyOrdered.Text
            Else
                txtPODate.Text = ""
                txtQtyOrdered.Text = 0
                txtDescription.Text = "Delivery for PO#"
                txtTotalCardsDelivered.Text = 0
            End If
        Else
            txtPODate.Text = ""
            txtQtyOrdered.Text = 0
            txtDescription.Text = "Delivery for PO#"
            txtTotalCardsDelivered.Text = 0
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

End Class
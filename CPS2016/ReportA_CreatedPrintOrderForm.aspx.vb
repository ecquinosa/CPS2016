
Public Class ReportA_CreatedPrintOrderForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.POReports) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            If Not Session("PurchaseOrder") Is Nothing Then
                If Not Session("ReportTypeID") Is Nothing Then
                    txtPurchaseOrder.Text = Session("PurchaseOrder").ToString

                    Dim DAL As New DAL
                    If DAL.SelectQuery(String.Format("SELECT Quantity, DateTimePosted FROM tblPO WHERE PurchaseOrder='{0}'", txtPurchaseOrder.Text)) Then
                        txtPODate.Text = DAL.TableResult.Rows(0)("DateTimePosted")
                        txtValidRecords.Text = DAL.TableResult.Rows(0)("Quantity")
                        txtRecordsReceived.Text = DAL.TableResult.Rows(0)("Quantity")
                        txtCardForPrinting.Text = DAL.TableResult.Rows(0)("Quantity")
                    Else
                        txtValidRecords.Text = 0
                        txtRecordsReceived.Text = 0
                        txtCardForPrinting.Text = 0
                    End If

                    If DAL.SelectQuery("SELECT SSSRepresentative, AllcardSalesRepresentative FROM tblSystemParameter") Then
                        txtRequestedBy.Text = DAL.TableResult.Rows(0)("SSSRepresentative")
                        txtJVConfirmedDate.Text = txtPODate.Text 'Now.ToString
                        txtJVConfirmedBy.Text = DAL.TableResult.Rows(0)("AllcardSalesRepresentative")
                    End If

                    DAL.Dispose()
                    DAL = Nothing

                    If CType(Session("ReportTypeID"), DataKeysEnum.Report) = DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport Then
                        Label1.Visible = False
                        Label2.Visible = False
                        txtJVConfirmedDate.Visible = False
                        txtJVConfirmedBy.Visible = False
                    Else
                        txtStatus.Text = "Card Production"
                    End If
                End If
            End If

        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        Select Case CType(Session("ReportTypeID"), DataKeysEnum.Report)
            Case DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport
                CreateSummaryOfCreatedPrintOrderReport(txtPurchaseOrder.Text, txtRequestedBy.Text, txtPODate.Text, txtRecordsReceived.Text, txtInvalidRecords.Text, txtValidRecords.Text, txtReprints.Text, txtCardForPrinting.Text, txtStatus.Text, txtDescription.Text)

                'If rg.GenerateReport(DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, 1, outputFile, txtPurchaseOrder.Text, txtRequestedBy.Text, txtPODate.Text, txtRecordsReceived.Text, txtInvalidRecords.Text, txtValidRecords.Text, txtReprints.Text, txtCardForPrinting.Text, txtStatus.Text, txtDescription.Text) Then
                '    If IO.File.Exists(outputFile) Then
                '        DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                '        DAL.AddSystemLog(String.Format("{0} generate SummaryOfCreatedPrintOrderReport report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPurchaseOrder.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                '    Else
                '        lblStatus.Text = "Unable to find report"
                '        lblStatus.ForeColor = SharedFunction.ErrorColor

                '        DAL.AddErrorLog("Unable to find SummaryOfCreatedPrintOrderReport report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                '    End If

                '    Session("pdfFile") = outputFile
                '    Response.Redirect("~/Rpt/PDFViewer.aspx")
                'Else
                '    lblStatus.Text = "Unable to generate report"
                '    lblStatus.ForeColor = SharedFunction.ErrorColor

                '    WriteToErrorLog(rg.ErrorMessage)
                'End If
            Case DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest
                CreateSummaryOfInitialPrintAndReprintRequest(txtPurchaseOrder.Text, txtRequestedBy.Text, txtPODate.Text, txtRecordsReceived.Text, txtInvalidRecords.Text, txtValidRecords.Text, txtReprints.Text, txtCardForPrinting.Text, txtStatus.Text, txtDescription.Text, txtJVConfirmedDate.Text, txtJVConfirmedBy.Text)

                'If rg.GenerateReport(DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest, 1, outputFile, txtPurchaseOrder.Text, txtRequestedBy.Text, txtPODate.Text, txtRecordsReceived.Text, txtInvalidRecords.Text, txtValidRecords.Text, txtReprints.Text, txtCardForPrinting.Text, txtStatus.Text, txtDescription.Text, txtJVConfirmedDate.Text, txtJVConfirmedBy.Text) Then
                '    If IO.File.Exists(outputFile) Then
                '        DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                '        DAL.AddSystemLog(String.Format("{0} generate SummaryOfInitialPrintAndReprintRequest report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPurchaseOrder.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                '    Else
                '        lblStatus.Text = "Unable to find report"
                '        lblStatus.ForeColor = SharedFunction.ErrorColor

                '        DAL.AddErrorLog("Unable to find SummaryOfInitialPrintAndReprintRequest report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                '    End If

                '    Session("pdfFile") = outputFile
                '    Response.Redirect("~/Rpt/PDFViewer.aspx")
                'Else
                '    lblStatus.Text = "Unable to generate report"
                '    lblStatus.ForeColor = SharedFunction.ErrorColor

                '    WriteToErrorLog(rg.ErrorMessage)
                'End If
        End Select
        DAL.Dispose()
        DAL = Nothing

        If rg.GenerateReport(CShort(Session("ReportTypeID")), 1, outputFile, txtPurchaseOrder.Text, txtRequestedBy.Text, txtPODate.Text, txtRecordsReceived.Text, txtInvalidRecords.Text, txtValidRecords.Text, txtReprints.Text, txtCardForPrinting.Text, txtStatus.Text, txtDescription.Text) Then
            Session("pdfFile") = outputFile
            Response.Redirect("~/Rpt/PDFViewer.aspx")
        Else
            lblStatus.Text = "Unable to generate report"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            WriteToErrorLog(rg.ErrorMessage)
        End If
        rg = Nothing
    End Sub

    Private Sub CreateSummaryOfCreatedPrintOrderReport(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Status As String, ByVal Description As String)
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        If rg.GenerateReport(DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, 1, outputFile, PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, Status, Description) Then
            If IO.File.Exists(outputFile) Then
                DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate SummaryOfCreatedPrintOrderReport report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find SummaryOfCreatedPrintOrderReport report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing

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

    Private Sub CreateSummaryOfInitialPrintAndReprintRequest(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Status As String, ByVal Description As String, ByVal JVConfirmedDate As String, ByVal JVConfirmedBy As String)
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        If rg.GenerateReport(DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest, 1, outputFile, PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, Status, Description, JVConfirmedDate, JVConfirmedBy) Then
            If IO.File.Exists(outputFile) Then
                DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate SummaryOfInitialPrintAndReprintRequest report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find SummaryOfInitialPrintAndReprintRequest report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing

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

        Dim intRecordsReceived As Integer = 0
        Dim intInvalid As Integer = 0
        Dim intValid As Integer = 0
        Dim intReprint As Integer = 0
        Dim intCardsForPrinting As Integer = 0

        If Not IsNumeric(txtRecordsReceived.Text) Then
            lblStatus.Text = "Please enter valid numeric value to records received"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            intRecordsReceived = txtRecordsReceived.Text
        End If

        If Not IsNumeric(txtInvalidRecords.Text) Then
            lblStatus.Text = "Please enter valid numeric value to invalid records"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            intInvalid = txtInvalidRecords.Text
        End If

        If Not IsNumeric(txtValidRecords.Text) Then
            lblStatus.Text = "Please enter valid numeric value to valid records"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            intValid = txtValidRecords.Text
        End If

        If Not IsNumeric(txtReprints.Text) Then
            lblStatus.Text = "Please enter valid numeric value to reprints"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            intReprint = txtReprints.Text
        End If

        If Not IsNumeric(txtCardForPrinting.Text) Then
            lblStatus.Text = "Please enter valid numeric value to card for printing"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            intCardsForPrinting = txtCardForPrinting.Text
        End If

        txtValidRecords.Text = (intRecordsReceived - intInvalid).ToString("N0")
        txtCardForPrinting.Text = (intValid + intReprint).ToString("N0")
    End Sub



End Class
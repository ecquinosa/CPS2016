
Imports DevExpress.Web.ASPxGridView.Rendering

Public Class POReports
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

        Try
            If SharedFunction.RoleID(Page.User.Identity.Name) <> DataKeysEnum.RoleID.SystemAdmin Then
                xGrid.Columns(3).Visible = False
            End If
        Catch ex As Exception
        End Try

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim sbWhereCriteria As New StringBuilder
        Dim sbReportTypeIDs As New StringBuilder
        sbWhereCriteria.Append("")

        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Personalization, DataKeysEnum.RoleID.QualityControl) Then
            If sbReportTypeIDs.ToString = "" Then
                sbReportTypeIDs.Append("1,3,5,7")
            Else
                sbReportTypeIDs.Append(",1,3,5,7")
            End If
        End If

        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Personalization) Then
            If sbReportTypeIDs.ToString = "" Then
                sbReportTypeIDs.Append("4,16,20")
            Else
                sbReportTypeIDs.Append(",4,16,20")
            End If
        End If

        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.InventoryAdmin) Then
            If sbReportTypeIDs.ToString = "" Then
                sbReportTypeIDs.Append("16")
            Else
                sbReportTypeIDs.Append(",16")
            End If
        End If

        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.QualityControl) Then
            If sbReportTypeIDs.ToString = "" Then
                sbReportTypeIDs.Append("13,14,15,18,19")
            Else
                sbReportTypeIDs.Append(",13,14,15,18,19")
            End If
        End If

        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.InventoryAdmin) Then
            If sbReportTypeIDs.ToString = "" Then
                sbReportTypeIDs.Append("17")
            Else
                sbReportTypeIDs.Append(",17")
            End If
        End If

        ''tempo start
        'sbReportTypeIDs.Clear()
        'sbReportTypeIDs.Append("16")
        ''temp end

        If sbReportTypeIDs.ToString <> "" Then
            sbWhereCriteria.Append(String.Format(" {0} dbo.tblRelPOReport.ReportTypeID IN ({1})", IIf(sbWhereCriteria.ToString = "", "WHERE", "AND"), sbReportTypeIDs.ToString))

            'sbWhereCriteria.Append(" AND dbo.tblRelPOReport.POID IN (" & TextBox1.Text & ")")
        End If

        Dim DAL As New DAL
        If DAL.SelectPurchaseOrderReports(sbWhereCriteria.ToString) Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF PURCHASE ORDER DOCUMENTS/ REPORTS ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_ProcessOnClickRowFilter(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewOnClickRowFilterEventArgs) Handles xGrid.ProcessOnClickRowFilter
        BindGrid()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    'Private Function ReportName() As String
    '    Return "CPS_Roles"
    'End Function

    'Private Function ReportHeader() As String
    '    Dim sb As New StringBuilder
    '    sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
    '    sb.AppendLine("SYSTEM ROLES")
    '    sb.AppendLine(String.Format("As of {0}", Now.ToString))

    '    Return sb.ToString
    'End Function

    'Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
    '    xGridExporter.FileName = ReportName()
    '    xGridExporter.PageHeader.Left = ReportHeader()

    '    If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    'End Sub

    Protected Sub lbGenerate_Click(sender As Object, e As EventArgs)
        lblStatus.Text = ""

        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Dim rg As New RptGenerator
        Select Case CType(xGrid.GetRowValues(index, "ReportTypeID"), DataKeysEnum.Report)
            Case DataKeysEnum.Report.DeliveryReceipt
            Case DataKeysEnum.Report.CertificateOfDeletion
                GenerateCertificateOfDeletion(index)
            Case Else
                Dim outputFile As String = ""
                If rg.GenerateReport(xGrid.GetRowValues(index, "ReportTypeID"), 1, outputFile, xGrid.GetRowValues(index, "PurchaseOrder")) Then
                    Dim DAL As New DAL
                    If Not DAL.UpdateRelPOReportByPOReportID(xGrid.GetRowValues(index, "POReportID"), IO.File.ReadAllBytes(outputFile)) Then
                        WriteToErrorLog(DAL.ErrorMessage)
                    End If
                Else
                    lblStatus.Text = "Unable to generate report"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    WriteToErrorLog(rg.ErrorMessage)
                End If
        End Select
        rg = Nothing

        GridDataBind()
    End Sub

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

    Private Sub WriteToErrorLog(ByVal strError As String)
        Dim DAL As New DAL
        DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub lbView_Click(sender As Object, e As EventArgs)
        lblStatus.Text = ""

        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Dim DAL As New DAL
        Dim byteReportImage As Object
        If DAL.ExecuteScalar("SELECT ReportImage FROM tblRelPOReport WHERE POReportID=" & xGrid.GetRowValues(index, "POReportID")) Then
            If Not DAL.ObjectResult Is Nothing Then
                byteReportImage = DAL.ObjectResult
            Else
                lblStatus.Text = "Report image is nothing"
                lblStatus.ForeColor = SharedFunction.ErrorColor
            End If
        Else
            DAL.AddErrorLog(DAL.ErrorMessage, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            lblStatus.Text = "Unable to get report image"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        End If
        DAL.Dispose()
        DAL = Nothing

        Select Case CType(xGrid.GetRowValues(index, "ReportTypeID"), DataKeysEnum.Report)
            Case DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest
                If IsDBNull(byteReportImage) Then
                    Session("POReportID") = xGrid.GetRowValues(index, "POReportID")
                    Session("PurchaseOrder") = xGrid.GetRowValues(index, "PurchaseOrder")
                    Session("ReportTypeID") = xGrid.GetRowValues(index, "ReportTypeID")
                    ASPxPopupControl1.ContentUrl = "~/ReportA_CreatedPrintOrderForm.aspx"
                    ASPxPopupControl1.Height = 550
                    ASPxPopupControl1.Width = 600
                    ASPxPopupControl1.ShowOnPageLoad = True
                Else
                    ExtractToPDF(xGrid.GetRowValues(index, "ReportTypeID"), byteReportImage)

                    ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
                    ASPxPopupControl1.ShowOnPageLoad = True
                End If
            Case DataKeysEnum.Report.DeliveryReceipt, DataKeysEnum.Report.DeliveryReceipt2
                ExtractToTXT(xGrid.GetRowValues(index, "ReportTypeID"), xGrid.GetRowValues(index, "PurchaseOrder"), byteReportImage)

                'ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
                'ASPxPopupControl1.ShowOnPageLoad = True                
            Case Else
                'Dim DAL As New DAL
                'If DAL.ExecuteScalar("SELECT ReportImage FROM tblRelPOReport WHERE POReportID=" & xGrid.GetRowValues(index, "POReportID")) Then
                '    ExtractToPDF(xGrid.GetRowValues(index, "ReportTypeID"), DAL.ObjectResult)

                '    ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
                '    ASPxPopupControl1.ShowOnPageLoad = True
                'End If
                'DAL.Dispose()
                'DAL = Nothing

                ExtractToPDF(xGrid.GetRowValues(index, "ReportTypeID"), byteReportImage)

                ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
                ASPxPopupControl1.ShowOnPageLoad = True
        End Select

    End Sub

    Private Sub ExtractToPDF(ByVal Report As DataKeysEnum.Report, ByVal blob() As Byte)
        Dim strFile As String = String.Format("{0}\{1}.pdf", SharedFunction.ReportsRepository, SharedFunction.GetReportNameByReportTypeID(Report) & "_" & SharedFunction.UserID(Page.User.Identity.Name))
        ByteToFile(strFile, blob, New StringBuilder)

        If IO.File.Exists(strFile) Then Session("pdfFile") = strFile
    End Sub

    Private Sub ExtractToTXT(ByVal Report As DataKeysEnum.Report, ByVal strPurchaseOrder As String, ByVal blob() As Byte)
        Dim strFile As String = String.Format("{0}\{1}.txt", SharedFunction.ReportsRepository, strPurchaseOrder & "_" & SharedFunction.GetReportNameByReportTypeID(Report) & "_" & SharedFunction.UserID(Page.User.Identity.Name))
        ByteToFile(strFile, blob, New StringBuilder)

        'If IO.File.Exists(strFile) Then Session("pdfFile") = strFile
        HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
        Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
        HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
        Dim Dfile As New System.IO.FileInfo(strFile)
        HttpContext.Current.Response.WriteFile(Dfile.FullName)
        HttpContext.Current.Response.[End]()
    End Sub

    Private Function GenerateCertificateOfDeletion(ByVal index As Integer) As Boolean
        Dim DAL As New DAL
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If rg.GenerateReport(DataKeysEnum.Report.CertificateOfDeletion, 1, outputFile, xGrid.GetRowValues(index, "PurchaseOrder")) Then
                'ExtractToPDF(xGrid.GetRowValues(index, "ReportTypeID"), IO.File.ReadAllBytes(outputFile))

                'ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
                'ASPxPopupControl1.ShowOnPageLoad = True

                If Not DAL.UpdateRelPOReportByPOReportID(xGrid.GetRowValues(index, "POReportID"), IO.File.ReadAllBytes(outputFile)) Then
                    DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), xGrid.GetRowValues(index, "PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    Return False
                End If
            Else
                DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}. Returned error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), xGrid.GetRowValues(index, "PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                Return False
            End If


            Return True
        Catch ex As Exception
            DAL.AddErrorLog(String.Format("GenerateCertificateOfDeletion(): Runtime error " & ex.Message, SharedFunction.UserCompleteName(Page.User.Identity.Name), xGrid.GetRowValues(index, "PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Return False
        Finally
            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function
   
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim PurchaseOrder As String, RequestedBy As String, PODate As String, RecordsReceived As String, InvalidRecords As String, ValidRecords As String, Reprints As String, CardForPrinting As String, Description As String, JVConfirmedDate As String, JVConfirmedBy As String

        'Description = ""

        'Dim DAL As New DAL
        'If DAL.SelectQuery("SELECT SSSRepresentative, AllcardSalesRepresentative FROM tblSystemParameter") Then
        '    RequestedBy = DAL.TableResult.Rows(0)("SSSRepresentative")
        '    JVConfirmedBy = DAL.TableResult.Rows(0)("AllcardSalesRepresentative")
        'End If

        'If DAL.SelectQuery(String.Format("SELECT POID, PurchaseOrder, Quantity, DateTimePosted FROM dbo.tblPO WHERE (POID IN ({0}))", TextBox1.Text)) Then
        '    For Each rw As DataRow In DAL.TableResult.Rows
        '        PurchaseOrder = rw("PurchaseOrder")
        '        PODate = rw("DateTimePosted")
        '        ValidRecords = rw("Quantity")
        '        RecordsReceived = rw("Quantity")
        '        CardForPrinting = rw("Quantity")

        '        JVConfirmedDate = PODate

        '        CreateSummaryOfCreatedPrintOrderReport_and_CreateSummaryOfInitialPrintAndReprintRequest(PurchaseOrder, RequestedBy, PODate, RecordsReceived, "0", ValidRecords, "0", CardForPrinting, Description, JVConfirmedDate, JVConfirmedBy)
        '        GenerateReportE(PurchaseOrder)
        '        GenerateReportG(PurchaseOrder)
        '    Next
        'End If

        'DAL.Dispose()
        'DAL = Nothing        

        Dim DAL As New DAL
        If DAL.SelectQuery(String.Format("SELECT POID, PurchaseOrder FROM dbo.tblPO WHERE (POID IN ({0}))", TextBox1.Text)) Then
            For Each rw As DataRow In DAL.TableResult.Rows
                SharedFunction.Generate_PO_Reports(rw("POID"), rw("PurchaseOrder").ToString.Trim, Request.Url.AbsolutePath, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name), lblStatus)
            Next
        End If

        DAL.Dispose()
        DAL = Nothing

        Button1.Enabled = False
    End Sub

    Private Sub Generate_PO_Reports(ByVal POID As Integer, ByVal PurchaseOrder As String)
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

                CreateSummaryOfCreatedPrintOrderReport_and_CreateSummaryOfInitialPrintAndReprintRequest(PurchaseOrder, RequestedBy, PODate, RecordsReceived, "0", ValidRecords, "0", CardForPrinting, Description, JVConfirmedDate, JVConfirmedBy)
            Next
        End If

        DAL.Dispose()
        DAL = Nothing

        'textfile
        GenerateReportE(PurchaseOrder)
        GenerateReportG(PurchaseOrder)

        Dim _PO As New PurchaseOrder
        _PO.GenerateDR2(POID, SharedFunction.ReportsRepository & "\" & PurchaseOrder, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), New System.Text.StringBuilder, New Integer)
        _PO = Nothing
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Then Exit Sub

        'Dim dt As DataTable
        'Dim DAL As New DAL
        'If DAL.SelectPurchaseOrderReports(" WHERE dbo.tblRelPOReport.ReportTypeID = 16" & " AND dbo.tblRelPOReport.POID IN (" & TextBox1.Text & ")") Then
        '    dt = DAL.TableResult
        'End If
        'DAL.Dispose()
        'DAL = Nothing

        'For Each rw As DataRow In dt.Rows
        '    GenerateCertificateOfDeletion2(rw)
        'Next

        Dim dt As DataTable
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT POID, PurchaseOrder FROM dbo.tblPOArchive WHERE POID IN (" & TextBox1.Text & ")") Then
            dt = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing

        For Each rw As DataRow In dt.Rows
            Dim intPOReportID As Integer = 0
            SharedFunction.GenerateCertificateOfDeletion_PurchaseOrder2(rw("POID"), rw("PurchaseOrder"), intPOReportID, Request.Url.AbsolutePath, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name))
        Next

        Button2.Enabled = False
    End Sub

    Private Function GenerateCertificateOfDeletion2(ByVal rw As DataRow) As Boolean
        Dim DAL As New DAL
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If rg.GenerateReport(DataKeysEnum.Report.CertificateOfDeletion, 1, outputFile, rw("PurchaseOrder")) Then
                'ExtractToPDF(xGrid.GetRowValues(index, "ReportTypeID"), IO.File.ReadAllBytes(outputFile))

                'ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
                'ASPxPopupControl1.ShowOnPageLoad = True

                If Not DAL.UpdateRelPOReportByPOReportID(rw("POReportID"), IO.File.ReadAllBytes(outputFile)) Then
                    DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), rw("PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    Return False
                End If
            Else
                DAL.AddErrorLog(String.Format("{0} failed to generate certification of deletion of Purchase Order {1}. Returned error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), rw("PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                Return False
            End If


            Return True
        Catch ex As Exception
            DAL.AddErrorLog(String.Format("GenerateCertificateOfDeletion(): Runtime error " & ex.Message, SharedFunction.UserCompleteName(Page.User.Identity.Name), rw("PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Return False
        Finally
            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Sub CreateSummaryOfCreatedPrintOrderReport_and_CreateSummaryOfInitialPrintAndReprintRequest(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Description As String, ByVal JVConfirmedDate As String, ByVal JVConfirmedBy As String)
        CreateSummaryOfCreatedPrintOrderReport(PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, "PO Created", Description)
        CreateSummaryOfInitialPrintAndReprintRequest(PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, "Card Production", Description, JVConfirmedDate, JVConfirmedBy)
    End Sub

    Private Sub CreateSummaryOfCreatedPrintOrderReport(ByVal PurchaseOrder As String, ByVal RequestedBy As String, ByVal PODate As String, ByVal RecordsReceived As String, ByVal InvalidRecords As String, ByVal ValidRecords As String, ByVal Reprints As String, ByVal CardForPrinting As String, ByVal Status As String, ByVal Description As String)
        Dim rg As New RptGenerator
        Dim outputFile As String = ""
        Dim DAL As New DAL
        If rg.GenerateReport(DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, 1, outputFile, PurchaseOrder, RequestedBy, PODate, RecordsReceived, InvalidRecords, ValidRecords, Reprints, CardForPrinting, Status, Description) Then
            If IO.File.Exists(outputFile) Then
                'DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.SummaryOfCreatedPrintOrderReport, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate SummaryOfCreatedPrintOrderReport report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find SummaryOfCreatedPrintOrderReport report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing

            'Session("pdfFile") = outputFile
            'Response.Redirect("~/Rpt/PDFViewer.aspx")
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
                'DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.SummaryOfInitialPrintAndReprintRequest, IO.File.ReadAllBytes(outputFile))
                DAL.AddSystemLog(String.Format("{0} generate SummaryOfInitialPrintAndReprintRequest report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                lblStatus.Text = "Unable to find report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                DAL.AddErrorLog("Unable to find SummaryOfInitialPrintAndReprintRequest report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing

            'Session("pdfFile") = outputFile
            'Response.Redirect("~/Rpt/PDFViewer.aspx")
        Else
            lblStatus.Text = "Unable to generate report"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            WriteToErrorLog(rg.ErrorMessage)

            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Private Sub GenerateReportE(ByVal PurchaseOrder As String)
        Dim outputFile As String = ""
        Dim cr As New CMS_Report
        Dim DAL As New DAL
        Try
            If cr.GenerateReportEorReportG(PurchaseOrder, "ElectronicReportOfDeliveredCardsPerPrintOrder", outputFile) Then
                If IO.File.Exists(outputFile) Then
                    'DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                    DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.ElectronicReportOfDeliveredCardsPerPrintOrder, IO.File.ReadAllBytes(outputFile))
                    DAL.AddSystemLog(String.Format("{0} generate ElectronicReportOfDeliveredCardsPerPrintOrder report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                Else
                    lblStatus.Text = "Unable to find ElectronicReportOfDeliveredCardsPerPrintOrder report"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    DAL.AddErrorLog("Unable to find ElectronicReportOfDeliveredCardsPerPrintOrder report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                End If
            Else
                lblStatus.Text = "Unable to ElectronicReportOfDeliveredCardsPerPrintOrder generate report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                WriteToErrorLog(cr.ErrorMessage)
            End If
        Catch ex As Exception
        Finally
            cr = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Sub

    Private Sub GenerateReportG(ByVal PurchaseOrder As String)
        Dim outputFile As String = ""
        Dim cr As New CMS_Report
        Dim DAL As New DAL
        Try
            If cr.GenerateReportEorReportG(PurchaseOrder, "ElectronicReportOfGoodCards", outputFile) Then
                If IO.File.Exists(outputFile) Then
                    'DAL.UpdateRelPOReportByPOReportID(Session("POReportID").ToString, IO.File.ReadAllBytes(outputFile))
                    DAL.UpdateRelPOReportByPOAndReportTypeID(PurchaseOrder, DataKeysEnum.Report.ElectronicReportOfGoodCards, IO.File.ReadAllBytes(outputFile))
                    DAL.AddSystemLog(String.Format("{0} generate ElectronicReportOfGoodCards report for PurchaseOrder {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), PurchaseOrder), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                Else
                    lblStatus.Text = "Unable to find ElectronicReportOfGoodCards report"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    DAL.AddErrorLog("Unable to find ElectronicReportOfGoodCards report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                End If
            Else
                lblStatus.Text = "Unable to ElectronicReportOfGoodCards generate report"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                WriteToErrorLog(cr.ErrorMessage)
            End If
        Catch ex As Exception
        Finally
            cr = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Sub


    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox1.Text = "" Then Exit Sub

        Dim dt As DataTable
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT ReportImage, ReportTypeID, PurchaseOrder FROM dbo.tblRelPOReport WHERE (ReportTypeID IN (1, 3)) AND (ReportImage IS NOT NULL) AND POID IN (" & TextBox1.Text & ")") Then
            dt = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing

        For Each rw As DataRow In dt.Rows
            Dim path As String = SharedFunction.ReportsRepository & "\" & rw("PurchaseOrder").ToString.Trim
            If Not System.IO.Directory.Exists(path) Then System.IO.Directory.CreateDirectory(path)
            Dim filename As String = rw("PurchaseOrder").ToString.Trim

            If rw("ReportTypeID").ToString.Trim = "1" Then
                filename += "_SummaryOfCreatedPrintOrderReport"
            Else
                filename += "_SummaryOfInitialPrintAndReprintRequest"
            End If

            Dim strFile As String = String.Format("{0}\{1}.pdf", path, filename)
            ByteToFile(strFile, rw("ReportImage"), New StringBuilder)
        Next

        Button3.Enabled = False
    End Sub

End Class
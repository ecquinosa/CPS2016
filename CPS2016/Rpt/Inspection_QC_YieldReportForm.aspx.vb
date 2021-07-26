
Public Class Inspection_QC_YieldReportForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.MiscReports) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            dateEdit.Text = Now.ToString("MM/dd/yyyy")

            Select Case CType(Session("Report"), DataKeysEnum.Report)
                Case DataKeysEnum.Report.InspectionQualityControlAndYieldReport
                    GridDataBind()
                Case DataKeysEnum.Report.RejectReportDaily
                    xGrid.Visible = False
                    ASPxButton1.Visible = False
            End Select
        End If
    End Sub

    Private Function GetSelectedItems_POID(ByRef intTotal As Integer, ByRef sbPurchaseOrders As StringBuilder, ByRef sbBatches As StringBuilder) As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGrid.GetSelectedFieldValues("POID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            End If
            sbPurchaseOrders.AppendLine(xGrid.GetSelectedFieldValues("PurchaseOrder")(i).ToString.trim)
            If sbBatches.ToString = "" Then
                sbBatches.Append(xGrid.GetSelectedFieldValues("Batch")(i).ToString.Trim)
            Else
                sbBatches.Append("," & xGrid.GetSelectedFieldValues("Batch")(i).ToString.Trim)
            End If

            intTotal += xGrid.GetSelectedFieldValues("Quantity")(i)
        Next

        Return sb.ToString
    End Function

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim sb As New StringBuilder
        'sb.Append(" WHERE POID IN (SELECT DISTINCT POID FROM dbo.tblCardReject) ")
        sb.Append(" WHERE POID NOT IN (SELECT dbo.tblRelPORprtRefPO.POID ")
        sb.Append("FROM dbo.tblRelPOReport INNER JOIN ")
        sb.Append("dbo.tblRelPORprtRefPO ON dbo.tblRelPOReport.POReportID = dbo.tblRelPORprtRefPO.POReportID ")
        sb.Append(String.Format("WHERE dbo.tblRelPOReport.ReportTypeID = {0}) ", CShort(DataKeysEnum.Report.InspectionQualityControlAndYieldReport)))
        'sb.Append("ORDER BY dbo.tblRelPORprtRefPO.POID DESC) ")

        'SELECT DISTINCT POID FROM            dbo.tblCardReject

        Dim strWhereCriteria As String = ""
        If dateEdit.Text <> "" Then sb.Append(String.Format(" AND CONVERT(char(10), DateTimePosted, 101)='{0}'", CDate(dateEdit.Text).ToString("MM/dd/yyyy")))

        Dim DAL As New DAL
        If DAL.SelectPOArchive(sb.ToString) Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs) Handles ASPxButton1.Click
        BindGrid()
        xGrid.DataBind()
    End Sub

    Protected Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Select Case CType(Session("Report"), DataKeysEnum.Report)
            Case DataKeysEnum.Report.InspectionQualityControlAndYieldReport
                If txtExecutedBy.Text = "" Then
                    lblStatus.Text = "Please enter execute by (certificate portion below)"
                    lblStatus.ForeColor = SharedFunction.ErrorColor
                    Exit Sub
                End If

                If txtWitnessedBy.Text = "" Then
                    lblStatus.Text = "Please enter witness by (certificate portion below)"
                    lblStatus.ForeColor = SharedFunction.ErrorColor
                    Exit Sub
                End If

                Dim intTotal As Integer = 0
                Dim sbPurchaseOrders As New StringBuilder
                Dim sbBatches As New StringBuilder
                Dim POIDs As String = GetSelectedItems_POID(intTotal, sbPurchaseOrders, sbBatches)
                Dim POIDs2 As String = String.Format(" WHERE POID IN ({0})", POIDs)

                Dim POIDs3 As String = String.Format(" WHERE dbo.tblCardReject.POID IN ({0})", POIDs)
                Dim outputFile As String = ""

                Dim intIQCYR_POID As String = 0
                Dim intRejectReportByPO_POID As String = 0

                Dim strIQCYR_DocCntr As String = ""
                Dim strRejectReportByPO_DocCntr As String = ""
                If AddDocumentWithDocCntr(DataKeysEnum.Report.InspectionQualityControlAndYieldReport, POIDs, POIDs2, intTotal, sbPurchaseOrders.ToString.Trim, outputFile, strIQCYR_DocCntr, intIQCYR_POID) Then
                    strRejectReportByPO_DocCntr = strIQCYR_DocCntr
                    If AddDocumentWithDocCntr(DataKeysEnum.Report.RejectReportByPO, POIDs, POIDs3, intTotal, sbBatches.ToString.Trim, , strRejectReportByPO_DocCntr, intRejectReportByPO_POID) Then
                        Dim DAL As New DAL
                        DAL.ExecuteQuery("UPDATE dbo.tblRelPOReport SET PurchaseOrder = ' Reference DocCntr " & strRejectReportByPO_DocCntr & "' WHERE POReportID=" & intIQCYR_POID.ToString & "; UPDATE dbo.tblRelPOReport SET PurchaseOrder = ' Reference DocCntr " & strIQCYR_DocCntr & "' WHERE POReportID=" & intRejectReportByPO_POID.ToString)
                        DAL.Dispose()
                        DAL = Nothing

                        Session("pdfFile") = outputFile
                        Response.Redirect("PDFViewer.aspx")
                    End If
                End If
            Case DataKeysEnum.Report.RejectReportDaily
                Dim rg As New RptGenerator
                Dim outputFile As String = ""
                If rg.GenerateReport(DataKeysEnum.Report.RejectReportDaily, 1, outputFile, String.Format(" WHERE CONVERT(char(10),dbo.tblCardReject.DateTimePosted, 101)='{0}'", CDate(dateEdit.Value).ToString("MM/dd/yyyy")), CDate(dateEdit.Value).ToString("MM/dd/yyyy")) Then
                    If IO.File.Exists(outputFile) Then
                        Session("pdfFile") = outputFile
                        Response.Redirect("PDFViewer.aspx")
                    Else
                        lblStatus.Text = "Unable to find report"
                        lblStatus.ForeColor = SharedFunction.ErrorColor
                    End If
                End If

                rg = Nothing

                divCertificateHeader.Visible = False
                divExecutedBy.Visible = False
                divWitnessedBy.Visible = False
        End Select
    End Sub

    Private Function AddDocumentWithDocCntr(ByVal Report As DataKeysEnum.Report, ByVal POIDs As String, ByVal POIDs2 As String, ByVal intTotal As Integer, ByVal sbPurchaseOrders As String, _
                                            Optional ByRef outputFile As String = "", Optional ByRef strDocCntr As String = "", _
                                            Optional ByRef intPOReportID As Integer = 0) As Boolean
        Dim DAL As New DAL
        Dim rg As New RptGenerator
        Try
            'Dim intPOReportID As Integer
            Dim _DocCntr As String

            Dim _byte() As Byte
            If DAL.AddRelPOReport(0, "", Report, _byte) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    intPOReportID = DAL.TableResult.Rows(0)("POReportID")
                    For Each POID As String In POIDs.Split(",")
                        DAL.ExecuteQuery(String.Format("INSERT INTO tblRelPORprtRefPO (POReportID, POID) VALUES ({0},{1})", intPOReportID, POID))
                    Next
                End If

                _DocCntr = String.Format("{0}-{1}", DAL.TableResult.Rows(0)("DocCntr").ToString.Trim.PadLeft(4, "0"), Now.ToString("yy"))
            Else
                lblStatus.Text = "Failed to add report to RelPOReport"
                lblStatus.ForeColor = SharedFunction.ErrorColor
                DAL.AddErrorLog(String.Format("AddRelPOReport(): Failed add " & SharedFunction.GetReportNameByReportTypeID(Report) & " report to RelPOReport. Returned error {0}", DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                Return False
            End If

            'Dim outputFile As String = ""
            Dim bln As Boolean

            If Report = DataKeysEnum.Report.InspectionQualityControlAndYieldReport Then
                bln = rg.GenerateReport(Report, 1, outputFile, POIDs2, intTotal, sbPurchaseOrders.ToString, _DocCntr)
            Else
                If DAL.ExecuteScalar("SELECT COUNT(POID) FROM dbo.tblCardReject WHERE POID IN (" & POIDs & ")") Then intTotal = CInt(DAL.ObjectResult)

                bln = rg.GenerateReport(Report, 1, outputFile, txtExecutedBy.Text, txtWitnessedBy.Text, intTotal, sbPurchaseOrders.ToString.Replace(" ", ""), _DocCntr, POIDs2, strDocCntr)
            End If

            strDocCntr = _DocCntr

            If bln Then
                If IO.File.Exists(outputFile) Then
                    DAL.UpdateRelPOReportByPOReportID(intPOReportID, IO.File.ReadAllBytes(outputFile))
                    DAL.AddSystemLog(String.Format("{0} generate InspectionQualityControlAndYieldReport report with DocCntr {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), _DocCntr), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                    'Session("pdfFile") = outputFile
                    'Response.Redirect("PDFViewer.aspx")
                Else
                    lblStatus.Text = "Unable to find report"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    DAL.AddErrorLog("Unable to find " & SharedFunction.GetReportNameByReportTypeID(Report) & " report", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                    Return False
                End If
            Else
                DAL.ExecuteQuery("DELETE FROM tblRelPOReport WHERE POReportID=" & intPOReportID.ToString)
                DAL.ExecuteQuery("DELETE FROM tblRelPORprtRefPO WHERE POReportID=" & intPOReportID.ToString)
                lblStatus.Text = "Unable to generate report"
                lblStatus.ForeColor = SharedFunction.ErrorColor
                DAL.AddErrorLog(rg.ErrorMessage, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            Return True
        Catch ex As Exception
            DAL.AddErrorLog("AddDocumentWithDocCntr(): Runtime error " & ex.Message, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Return False
        Finally
            rg = Nothing
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Sub WriteToErrorLog(ByVal strError As String)
        Dim DAL As New DAL
        DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        DAL.Dispose()
        DAL = Nothing
    End Sub

End Class
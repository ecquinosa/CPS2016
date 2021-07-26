
Imports System.IO

Public Class MiscTxn
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
            
        End If
    End Sub

    'Private Sub ShowPopup(ByVal Report As DataKeysEnum.Report)
    '    Session("Report") = Report

    '    Select Case Report
    '        Case DataKeysEnum.Report.InspectionQualityControlAndYieldReport
    '            ASPxPopupControl1.HeaderText = "QC Yield Report"
    '            ASPxPopupControl1.ContentUrl = "~/Rpt/Inspection_QC_YieldReportForm.aspx"
    '        Case DataKeysEnum.Report.MaterialInventory
    '            ASPxPopupControl1.HeaderText = "Material Inventory"

    '            Dim rg As New RptGenerator
    '            Dim outputFile As String = ""
    '            If rg.GenerateReport(DataKeysEnum.Report.MaterialInventory, 1, outputFile, "") Then
    '                Session("pdfFile") = outputFile
    '            End If
    '            rg = Nothing

    '            ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
    '        Case DataKeysEnum.Report.RejectReportDaily
    '            ASPxPopupControl1.HeaderText = "Reject Report"
    '            ASPxPopupControl1.ContentUrl = "~/Rpt/Inspection_QC_YieldReportForm.aspx"
    '        Case Else
    '            ASPxPopupControl1.HeaderText = "Data Extraction"
    '            ASPxPopupControl1.ContentUrl = "~/Rpt/SelectPurchaseOrder.aspx"
    '    End Select

    '    ASPxPopupControl1.Height = 600
    '    ASPxPopupControl1.Width = 800
    '    ASPxPopupControl1.ShowOnPageLoad = True
    'End Sub   

    Protected Sub btnSubmitUploadPO_Click(sender As Object, e As EventArgs) Handles btnSubmitUploadPO.Click
        Dim strPurchaseOrder As String = txtUploadPO.Text
        Dim strBatch As String = strPurchaseOrder.Split("-")(3)

        ASPxMemo1.Visible = False

        Dim sb As New StringBuilder

        Dim DAL As New DAL
        If SharedFunction.UploadData_v2(strPurchaseOrder, strBatch, "", SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name), sb) Then
            DAL.AddSystemLog(String.Format("{0} Uploaded PO {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPOStatusReturn.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        Else
            DAL.AddErrorLog(String.Format("{0} failed to upload PO {1}. Error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPOStatusReturn.Text, DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        End If
        DAL.Dispose()
        DAL = Nothing

        ASPxMemo1.Visible = True
        ASPxMemo1.Text = sb.ToString.Replace("<br>", "")
        ASPxMemo1.ForeColor = Drawing.Color.Black
    End Sub

    Protected Sub btnPOStatusReturn_Click(sender As Object, e As EventArgs) Handles btnPOStatusReturn.Click
        ASPxMemo1.ForeColor = Drawing.Color.Black

        Dim DAL As New DAL
        If DAL.ReturnPOStatusToNewUpload(txtPOStatusReturn.Text) Then
            ASPxMemo1.Visible = True
            ASPxMemo1.Text = "SUCCESS"
            ASPxMemo1.ForeColor = SharedFunction.SuccessColor

            DAL.AddSystemLog(String.Format("{0} changed status of PO {1} to Indigo Extract", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPOStatusReturn.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        Else
            ASPxMemo1.Visible = True
            ASPxMemo1.Text = "FAILED. " & vbNewLine & DAL.ErrorMessage
            ASPxMemo1.ForeColor = SharedFunction.ErrorColor
            DAL.AddErrorLog(String.Format("{0} failed to changed status of PO {1} to Indigo Extract. Error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPOStatusReturn.Text, DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

End Class
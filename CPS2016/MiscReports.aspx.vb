
Public Class MiscReports
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

            lbQCYieldReport.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.QualityControl)
            lbInventoryReport.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.InventoryAdmin)
            lbRejectReport.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.QualityControl)
            lbExtractLaserData.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Personalization)
            lbExtractMuhlbauerData.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Personalization)
            lbExtractRejectCard.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.QualityControl)
            lbDeliveryReceipt2.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Personalization)
        End If
    End Sub

    Private Sub ShowPopup(ByVal Report As DataKeysEnum.Report)
        Session("Report") = Report

        Select Case Report
            Case DataKeysEnum.Report.InspectionQualityControlAndYieldReport
                ASPxPopupControl1.HeaderText = "QC Yield Report"
                ASPxPopupControl1.ContentUrl = "~/Rpt/Inspection_QC_YieldReportForm.aspx"
            Case DataKeysEnum.Report.MaterialInventory
                ASPxPopupControl1.HeaderText = "Material Inventory"

                Dim rg As New RptGenerator
                Dim outputFile As String = ""
                If rg.GenerateReport(DataKeysEnum.Report.MaterialInventory, 1, outputFile, "") Then
                    Session("pdfFile") = outputFile
                End If
                rg = Nothing

                ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
            Case DataKeysEnum.Report.RejectReportDaily
                ASPxPopupControl1.HeaderText = "Reject Report"
                ASPxPopupControl1.ContentUrl = "~/Rpt/Inspection_QC_YieldReportForm.aspx"
            Case DataKeysEnum.Report.DeliveryReceipt_PDF
                ASPxPopupControl1.HeaderText = "Delivery Receipt PDF"
                ASPxPopupControl1.ContentUrl = "ReportD_DeliveryReceipt.aspx"
            Case DataKeysEnum.Report.EditNamesAddress
                ASPxPopupControl1.HeaderText = "MODIFY FIRSTNAME/ LASTNAME/ ADDRESS"
                ASPxPopupControl1.ContentUrl = "EditNamesAddress.aspx"
            Case DataKeysEnum.Report.AcknowledgeFile
                Session("ReportType") = DataKeysEnum.Report.AcknowledgeFile
                ASPxPopupControl1.HeaderText = "Generate Acknowledge File"
                ASPxPopupControl1.ContentUrl = "AcknowledgeFile.aspx"
                ASPxPopupControl1.Height = 300
                ASPxPopupControl1.Width = 600
                ASPxPopupControl1.ShowOnPageLoad = True
                Return
            Case DataKeysEnum.Report.ResponseFile
                Session("ReportType") = DataKeysEnum.Report.ResponseFile
                ASPxPopupControl1.HeaderText = "Generate Response File"
                ASPxPopupControl1.ContentUrl = "AcknowledgeFile.aspx"
                ASPxPopupControl1.Height = 300
                ASPxPopupControl1.Width = 600
                ASPxPopupControl1.ShowOnPageLoad = True
                Return
            Case Else
                ASPxPopupControl1.HeaderText = "Data Extraction"
                ASPxPopupControl1.ContentUrl = "~/Rpt/SelectPurchaseOrder.aspx"
        End Select

        ASPxPopupControl1.Height = 600
        ASPxPopupControl1.Width = 800
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Protected Sub lbQCYieldReport_Click(sender As Object, e As EventArgs) Handles lbQCYieldReport.Click
        ShowPopup(DataKeysEnum.Report.InspectionQualityControlAndYieldReport)
    End Sub

    Protected Sub lbInventoryReport_Click(sender As Object, e As EventArgs) Handles lbInventoryReport.Click
        ShowPopup(DataKeysEnum.Report.MaterialInventory)
    End Sub

    Protected Sub lbRejectReport_Click(sender As Object, e As EventArgs) Handles lbRejectReport.Click
        ShowPopup(DataKeysEnum.Report.RejectReportDaily)
    End Sub

    Protected Sub lbExtractLaserData_Click(sender As Object, e As EventArgs) Handles lbExtractLaserData.Click
        Session("DataExtraction") = "LaserEngraving"
        ShowPopup(0)
    End Sub

    Protected Sub lbExtractMuhlbauerData_Click(sender As Object, e As EventArgs) Handles lbExtractMuhlbauerData.Click
        Session("DataExtraction") = "Muhlbauer"
        ShowPopup(0)
    End Sub

    Protected Sub lbExtractRejectCard_Click(sender As Object, e As EventArgs) Handles lbExtractRejectCard.Click
        Session("DataExtraction") = "RejectList"
        ShowPopup(0)
    End Sub

    Protected Sub lbDeliveryReceipt2_Click(sender As Object, e As EventArgs) Handles lbDeliveryReceipt2.Click
        Session("DataExtraction") = "DeliveryReceipt2"
        ShowPopup(0)
    End Sub

    Protected Sub lbDeliveryReceiptManualExtraction_Click(sender As Object, e As EventArgs) Handles lbDeliveryReceiptManualExtraction.Click
        Session("ListType") = "DR"
        Server.Transfer("DeliveryReceiptByScan.aspx")
    End Sub

    Protected Sub lbDR_PDF_Click(sender As Object, e As EventArgs) Handles lbDR_PDF.Click
        'Session("DataExtraction") = "RejectList"
        ShowPopup(DataKeysEnum.Report.DeliveryReceipt_PDF)
    End Sub

    Protected Sub lbExtractCubaoBranchData_Click(sender As Object, e As EventArgs) Handles lbExtractCubaoBranchData.Click
        Session("DataExtraction") = "CubaoBranchData"
        ShowPopup(0)
    End Sub

    Protected Sub lbModifyLaserData_Click(sender As Object, e As EventArgs) Handles lbModifyLaserData.Click
        ShowPopup(DataKeysEnum.Report.EditNamesAddress)
    End Sub

    Protected Sub lbAcknowledgeFile_Click(sender As Object, e As EventArgs) Handles lbAcknowledgeFile.Click
        ShowPopup(DataKeysEnum.Report.AcknowledgeFile)
    End Sub

    Protected Sub lbResponseFile_Click(sender As Object, e As EventArgs) Handles lbResponseFile.Click
        'Session("DataExtraction") = "UBP_RF"
        'ShowPopup(0)
        ShowPopup(DataKeysEnum.Report.ResponseFile)
    End Sub

    Protected Sub lbExtractMuhlbauerData0_Click(sender As Object, e As EventArgs) Handles lbExtractMuhlbauerData0.Click
        Session("ListType") = "Muhlbauer"
        Server.Transfer("DeliveryReceiptByScan.aspx")
    End Sub

    Protected Sub lbExtractLaserData0_Click(sender As Object, e As EventArgs) Handles lbExtractLaserData0.Click
        Session("ListType") = "Laser"
        Server.Transfer("DeliveryReceiptByScan.aspx")
    End Sub


    Protected Sub lbACMLFile_Click(sender As Object, e As EventArgs) Handles lbACMLFile.Click
        Session("DataExtraction") = "ACML"
        ShowPopup(0)
    End Sub

End Class
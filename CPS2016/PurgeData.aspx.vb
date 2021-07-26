
Imports System.IO

Public Class PurgeData
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Process_DataPurge) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()
            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Function GenerateCertificateOfDeletion(ByVal index As Integer, ByRef intPOReportID As Integer) As Boolean
        Try
            If Not chkReupload.Checked Then _
                Return SharedFunction.GenerateCertificateOfDeletion_PurchaseOrder(xGrid.GetRowValues(index, "POID"), xGrid.GetRowValues(index, "PurchaseOrder"), intPOReportID, Request.Url.AbsolutePath, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name))
        Catch ex As Exception
            'DAL.AddErrorLog(String.Format("GenerateCertificateOfDeletion(): Runtime error " & ex.Message, SharedFunction.UserCompleteName(Page.User.Identity.Name), xGrid.GetRowValues(index, "PurchaseOrder"), rg.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Return False
        Finally
        End Try
    End Function

    Private Function GenerateCertificateOfDeletion_PurchaseOrder(ByVal intPOID As Integer, ByVal strPurchaseOrder As String, ByRef intPOReportID As Integer) As Boolean
        Try
            If Not chkReupload.Checked Then _
                Return SharedFunction.GenerateCertificateOfDeletion_PurchaseOrder(intPOID, strPurchaseOrder, intPOReportID, Request.Url.AbsolutePath, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name))
        Catch ex As Exception
            Return False
        Finally
        End Try
    End Function

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ASPxMemo1.Visible = False

        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Dim DAL As New DAL

        Dim intPOReportID As Integer

        'If Not DAL.PurgePOData(xGrid.GetRowValues(index, "POID"), chkReupload.Checked) Then
        If Not PurgeData(xGrid.GetRowValues(index, "POID")) Then
            DAL.ExecuteQuery("DELETE FROM tblRelPOReport WHERE POReportID=" & intPOReportID.ToString)
            DAL.ExecuteQuery("DELETE FROM tblRelPORprtRefPO WHERE POReportID=" & intPOReportID.ToString)

            DAL.AddErrorLog(String.Format("{0} failed to purge data of Purchase Order {1}. Returned error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), xGrid.GetRowValues(index, "PurchaseOrder"), DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            ASPxMemo1.Visible = True
            ASPxMemo1.Text = DAL.ErrorMessage
        Else
            DAL.AddSystemLog(String.Format("{0} purged data of Purchase Order {1}. IsForReupload {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), xGrid.GetRowValues(index, "PurchaseOrder"), chkReupload.Checked.ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

            Try
                'housekeeping----
                'SharedFunction.CPS_Folder_PurchaseOrder_Housekeeping(xGrid.GetRowValues(index, "PurchaseOrder"))
                Dim strPO As String = xGrid.GetRowValues(index, "PurchaseOrder")
                'SharedFunction.DeletePO_ForUploadingRepository(strPO)

                Dim encryptedFile As String = String.Format("{0}\{1}_zip.sss", SharedFunction.ForUploadingRepository, strPO)
                Dim ZipFile As String = String.Format("{0}\{1}.zip", SharedFunction.ForUploadingRepository, strPO)

                'If File.Exists(encryptedFile) Then File.Delete(encryptedFile)
                'If File.Exists(ZipFile) Then File.Delete(ZipFile)
                '-----
            Catch ex As Exception
            End Try

            If GenerateCertificateOfDeletion(index, intPOReportID) Then
                'housekeeping
                ASPxMemo1.Visible = True
                ASPxMemo1.Text = "Process is done"
            Else
                DAL.ExecuteQuery("DELETE FROM tblRelPOReport WHERE POReportID=" & intPOReportID.ToString)
                DAL.ExecuteQuery("DELETE FROM tblRelPORprtRefPO WHERE POReportID=" & intPOReportID.ToString)
                lblStatus.Text = "Unable to generate certification report"
                lblStatus.ForeColor = SharedFunction.ErrorColor
            End If

            GridDataBind()
        End If

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Function PurgeData(ByVal POID As Integer) As Boolean
        Dim DAL As New DAL
        For i As Short = 1 To 8
            If Not DAL.PurgePODatav2(POID, chkReupload.Checked, i) Then
                DAL.Dispose()
                DAL = Nothing
                Return False
            End If
        Next
        DAL.Dispose()
        DAL = Nothing

        Return True
    End Function

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If Not chkReupload.Checked Then
            If DAL.SelectPOForPurging Then
                xGrid.DataSource = DAL.TableResult
            End If
        Else
            If DAL.SelectPOForReupload Then
                xGrid.DataSource = DAL.TableResult
            End If
        End If

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GenerateDR(ByVal POID As Integer, ByVal strPurchaseOrder As String)
        Dim strCurrentFolder As String = String.Format("{0}\{1}", SharedFunction.ReportsRepository, Now.ToString("MM-dd-yyyy"))

        If Not IO.Directory.Exists(strCurrentFolder) Then IO.Directory.CreateDirectory(strCurrentFolder)

        Dim sb As New StringBuilder

        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT Barcode FROM tblRelPOData WHERE POID=" & POID) Then
            For Each rw As DataRow In DAL.TableResult.Rows
                sb.AppendLine(String.Format("{0},{1},{2}", rw("Barcode").ToString.Trim, "ChipSerial", Now.ToString("ddMMyyyy")))
            Next
        End If
        DAL.Dispose()
        DAL = Nothing

        Dim sw As New StreamWriter(String.Format("{0}\{1}.txt", strCurrentFolder, strPurchaseOrder))
        sw.Write(sb.ToString)
        sw.Dispose()
        sw.Close()
        sw = Nothing
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_DataForPurging"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("DATA FOR PURGING")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub chkReupload_CheckedChanged(sender As Object, e As EventArgs) Handles chkReupload.CheckedChanged
        xGrid.DataBind()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim DAL As New DAL
        If DAL.SelectPOForPurging Then
            For Each rw As DataRow In DAL.TableResult.Rows
                Dim intPOReportID As Integer

                If Not DAL.PurgePOData(rw("POID"), chkReupload.Checked) Then
                    DAL.ExecuteQuery("DELETE FROM tblRelPOReport WHERE POReportID=" & intPOReportID.ToString)
                    DAL.ExecuteQuery("DELETE FROM tblRelPORprtRefPO WHERE POReportID=" & intPOReportID.ToString)

                    DAL.AddErrorLog(String.Format("{0} failed to purge data of Purchase Order {1}. Returned error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), rw("PurchaseOrder"), DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    ASPxMemo1.Visible = True
                    ASPxMemo1.Text = DAL.ErrorMessage
                Else
                    DAL.AddSystemLog(String.Format("{0} purged data of Purchase Order {1}. IsForReupload {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), rw("PurchaseOrder"), chkReupload.Checked.ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                    SharedFunction.Generate_PO_Reports(rw("POID"), rw("PurchaseOrder").ToString.Trim, Request.Url.AbsolutePath, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name), New DevExpress.Web.ASPxEditors.ASPxLabel)

                    'If Not GenerateCertificateOfDeletion_PurchaseOrder(rw("POID"), rw("PurchaseOrder"), intPOReportID) Then
                    '    DAL.ExecuteQuery("DELETE FROM tblRelPOReport WHERE POReportID=" & intPOReportID.ToString)
                    '    DAL.ExecuteQuery("DELETE FROM tblRelPORprtRefPO WHERE POReportID=" & intPOReportID.ToString)
                    '    lblStatus.Text = "Unable to generate certification report"
                    '    lblStatus.ForeColor = SharedFunction.ErrorColor
                    'End If
                End If
            Next
        End If
        DAL.Dispose()
        DAL = Nothing

        Button1.Enabled = False
    End Sub

End Class
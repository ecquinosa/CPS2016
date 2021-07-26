
Imports System.IO

Public Class DownloadFile
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Download) Then
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

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Dim intDFID As String = xGrid.GetRowValues(index, "DownloadFileID")
        Dim strFile As String = xGrid.GetRowValues(index, "FilePath").ToString.Trim
        Dim strDesc As String = xGrid.GetRowValues(index, "Description").ToString.Trim
        Dim intDownloadCntr As Integer = xGrid.GetRowValues(index, "DownloadCntr")

        Dim DAL As New DAL
        Try
            DAL.AddSystemLog(String.Format("{0} downloaded {1} file with DFID {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), strDesc, intDFID.ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            DAL.ExecuteQuery(String.Format("UPDATE tblDownloadableFiles SET DateTimeExtracted=GETDATE(), DownloadCntr={0} WHERE DownloadFileID={1}", intDownloadCntr + 1, intDFID.ToString))

            If Not SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin) Then
                If intDownloadCntr = 1 Then IO.File.Delete(strFile)
            End If
        Catch ex As Exception
            DAL.AddErrorLog(String.Format("LinkButton1_Click(): Failed to update DFID {0}. Runtime error {1}", intDFID.ToString, ex.Message), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try

        GridDataBind()

        If File.Exists(strFile) Then
            HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
            Dim Header As [String] = "Attachment; Filename=" & Path.GetFileName(strFile)
            HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
            Dim Dfile As New System.IO.FileInfo(strFile)
            HttpContext.Current.Response.WriteFile(Dfile.FullName)
            HttpContext.Current.Response.[End]()

            'Dim DAL As New DAL
            'Try
            '    DAL.AddSystemLog(String.Format("{0} downloaded {1} file with DFID {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), strDesc, intDFID.ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            '    DAL.ExecuteQuery(String.Format("UPDATE tblDownloadableFiles SET DateTimeExtracted=GETDATE(), DownloadCntr={0} WHERE DownloadFileID={1}", intDownloadCntr + 1, intDFID.ToString))
            'Catch ex As Exception
            '    DAL.AddErrorLog(String.Format("LinkButton1_Click(): Failed to update DFID {0}. Runtime error {1}", intDFID.ToString, ex.Message), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            'Finally
            '    DAL.Dispose()
            '    DAL = Nothing
            'End Try
        End If
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim sb As New StringBuilder
        sb.Append("")

        'Dim strWhereCriteria As String = " WHERE DownloadCntr < 2"
        'If chkShowAll.Checked Then strWhereCriteria = ""

        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin) Then
        Else
            sb.Append(" WHERE DownloadCntr < 2")

            If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Indigo) Then
                sb.Append(IIf(sb.ToString = "", " WHERE ", " AND ") & "Type LIKE 'IndigoExtract%'")
            ElseIf SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Personalization) Then
                sb.Append(IIf(sb.ToString = "", " WHERE ", " AND ") & "Type LIKE 'PurchaseOrder%'")
            End If
        End If

        Dim DAL As New DAL
        If DAL.SelectDownloadableFiles(sb.ToString) Then
            xGrid.DataSource = DAL.TableResult
        End If

        DAL.Dispose()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_DownloadFile"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("DOWNLOAD FILE")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub


End Class
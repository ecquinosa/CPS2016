
Imports DevExpress.Web.ASPxGridView.Rendering

Public Class ProcessedPOMat
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Processed_Inv) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.InventoryAdmin) Then
            btnNew.Visible = False
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            Dim bln As Boolean = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.InventoryAdmin)
            btnNew.Visible = bln
            xGrid.Columns(5).Visible = bln

            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectPOMaterial() Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF PROCESSED ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_ProcessOnClickRowFilter(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewOnClickRowFilterEventArgs) Handles xGrid.ProcessOnClickRowFilter
        BindGrid()
    End Sub

    Private Sub xGrid_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles xGrid.RowUpdating
        e.Cancel = True
        xGrid.CancelEdit()
        BindGrid()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_ProcessedPOMaterial"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("PROCESSED PURCHASE ORDER/S FOR MATERIAL ALLOTMENT")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Session("IsNewProcess") = 1
        ASPxPopupControl1.Height = 500
        ASPxPopupControl1.Width = 800
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Dim intPOMaterialID As String = xGrid.GetRowValues(index, "POMaterialID")

        Dim DAL As New DAL
        Try
            If DAL.ExecuteQuery(String.Format("UPDATE tblPOMaterial SET IsCancel = 1 WHERE POMaterialID={0}", intPOMaterialID)) Then
                DAL.AddSystemLog(String.Format("{0} cancelled POMaterialID {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), intPOMaterialID.ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                DAL.AddErrorLog(String.Format("LinkButton1_Click(): Failed to cancel POMaterialID {0}. Returned error {1}", intPOMaterialID.ToString, DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            GridDataBind()
        Catch ex As Exception
            DAL.AddErrorLog(String.Format("LinkButton1_Click(): Failed to cancel POMaterialID {0}. Runtime error {1}", intPOMaterialID.ToString, ex.Message), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs)
        Session.RemoveAll()
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Session("POMaterialID") = xGrid.GetRowValues(index, "POMaterialID")
        ASPxPopupControl1.Height = 500
        ASPxPopupControl1.Width = 800
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

End Class
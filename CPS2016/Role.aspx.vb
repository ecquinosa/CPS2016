
Imports DevExpress.Web.ASPxGridView.Rendering

Public Class Role
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Role) Then
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

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblRole WHERE IsDeleted = 0") Then
            xGrid.DataSource = DAL.TableResult
        End If
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF ROLES ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs) Handles xGrid.CellEditorInitialize
        If e.Column.FieldName = "RoleID" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If xGrid.IsNewRowEditing Then
                txt.Text = "New"
                txt.Enabled = False
            End If
           
        End If
    End Sub

    Private Sub xGrid_ProcessOnClickRowFilter(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewOnClickRowFilterEventArgs) Handles xGrid.ProcessOnClickRowFilter
        BindGrid()
    End Sub

    Private Sub xGrid_RowDeleting(sender As Object, e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles xGrid.RowDeleting
        Dim DAL As New DAL
        If DAL.ExecuteQuery("UPDATE tblRole SET IsDeleted = 1 WHERE RoleID=" & e.Keys.Values(0).ToString.Trim) Then
            e.Cancel = True
            DAL.AddSystemLog(String.Format("{0} deleted RoleID {1}, Role {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), e.Keys.Values(0).ToString.Trim, e.Values(1).ToString.Trim), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            BindGrid()
        End If
    End Sub

    Private Sub xGrid_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles xGrid.RowInserting
        e.Cancel = True
        xGrid.CancelEdit()
        BindGrid()
    End Sub

    Private Sub xGrid_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles xGrid.RowUpdating
        e.Cancel = True
        xGrid.CancelEdit()
        BindGrid()
    End Sub

    Private Sub xGrid_RowValidating(sender As Object, e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles xGrid.RowValidating
        If e.NewValues("RoleDesc") = "" Then
            e.RowError = "Please enter Role"
        Else
            Dim DAL As New DAL

            If e.Keys.Values(0) = Nothing Then 'insert
                If DAL.AddRole(e.NewValues("RoleDesc").ToString.Trim) Then
                    e.RowError = DAL.ObjectResult.ToString
                Else
                    e.RowError = DAL.ErrorMessage
                End If
            Else 'edit
                If DAL.EditRole(e.OldValues("RoleID").ToString.Trim, e.NewValues("RoleDesc").ToString.Trim) Then
                    e.RowError = DAL.ObjectResult.ToString
                Else
                    e.RowError = DAL.ErrorMessage
                End If
            End If

            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_Roles"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("SYSTEM ROLES")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Session("RoleID") = xGrid.GetRowValues(index, "RoleID")
        Session("RoleDesc") = xGrid.GetRowValues(index, "RoleDesc")

        ASPxPopupControl1.HeaderText = "Role Module(s)"
        ASPxPopupControl1.ContentUrl = "~/RoleModule.aspx"

        ASPxPopupControl1.Height = 650
        ASPxPopupControl1.Width = 800
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub lbActivity_Click(sender As Object, e As EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Session("RoleID") = xGrid.GetRowValues(index, "RoleID")
        Session("RoleDesc") = xGrid.GetRowValues(index, "RoleDesc")

        ASPxPopupControl1.HeaderText = "Role Activity"
        ASPxPopupControl1.ContentUrl = "~/RoleActivity.aspx"

        ASPxPopupControl1.Height = 500
        ASPxPopupControl1.Width = 600
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

End Class
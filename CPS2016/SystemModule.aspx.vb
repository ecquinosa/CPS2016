
Imports DevExpress.Web.ASPxGridView.Rendering

Public Class SystemModule
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.SystemModule) Then
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
        'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblModule WHERE IsDeleted = 0") Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.dispose()
        DAl = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF MODULES ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs) Handles xGrid.CellEditorInitialize
        If e.Column.FieldName = "ModuleID" Then
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
        'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
        Dim DAL As New DAL
        If DAL.ExecuteQuery("UPDATE tblModule SET IsDeleted = 1 WHERE ModuleID=" & e.Keys.Values(0).ToString.Trim) Then
            e.Cancel = True
            DAL.AddSystemLog(String.Format("{0} deleted ModuleID {1}, Module {2}, Page {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), e.Keys.Values(0).ToString.Trim, e.Values(1).ToString.Trim, e.Values(2).ToString.Trim), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
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
        If e.NewValues("Module") = "" Then
            e.RowError = "Please enter Module"
        ElseIf e.NewValues("ModuleDesc") = "" Then
            e.RowError = "Please enter Description"
        ElseIf e.NewValues("Page") = "" Then
            e.RowError = "Please enter Page"
        Else
            'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
            Dim DAL As New DAL

            If e.Keys.Values(0) = Nothing Then 'insert
                If DAL.AddModule(e.NewValues("Module").ToString.Trim, e.NewValues("ModuleDesc").ToString.Trim, e.NewValues("Page").ToString.Trim) Then
                    e.RowError = DAL.ObjectResult.ToString
                Else
                    e.RowError = DAL.ErrorMessage
                End If
            Else 'edit
                If DAL.EditModule(e.OldValues("ModuleID").ToString.Trim, e.NewValues("Module").ToString.Trim, e.NewValues("ModuleDesc").ToString.Trim, e.NewValues("Page").ToString.Trim) Then
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
        Response.Redirect("Module.aspx")
    End Sub

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

End Class

Imports DevExpress.Web.ASPxGridView.Rendering

Public Class Material
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Material) Then
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
        If DAL.SelectQuery("SELECT * FROM tblMstrMaterial WHERE IsDeleted = 0") Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF MATERIALS ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs) Handles xGrid.CellEditorInitialize
        If e.Column.FieldName = "MaterialID" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If xGrid.IsNewRowEditing Then
                txt.Text = "New"
                txt.Enabled = False
            End If
        ElseIf e.Column.FieldName = "BegQty" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If xGrid.IsNewRowEditing Then txt.Text = 0

            'ElseIf e.Column.FieldName = "EditType" Then
            '    Dim cmb As DevExpress.Web.ASPxEditors.ASPxComboBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxComboBox)
            '    cmb.Items.Add("Edit Record", 1)
            '    cmb.Items.Add("Reset Password", 2)
            '    cmb.Items.Add("Deactivate account", 3)
            '    cmb.Items.Add("Activate account", 4)
            '    cmb.Items.Add("Change Username", 5)
            '    cmb.ValueType = GetType(String)
            '    cmb.SelectedIndex = 0
            '    If xGrid.IsNewRowEditing Then cmb.Enabled = False
        End If
    End Sub

    Private Sub xGrid_ProcessOnClickRowFilter(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewOnClickRowFilterEventArgs) Handles xGrid.ProcessOnClickRowFilter
        BindGrid()
    End Sub

    Private Sub xGrid_RowDeleting(sender As Object, e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles xGrid.RowDeleting
        'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
        Dim DAL As New DAL
        If DAL.ExecuteQuery("UPDATE tblMstrMaterial SET IsDeleted = 1 WHERE MaterialID=" & e.Keys.Values(0).ToString.Trim) Then
            e.Cancel = True
            DAL.AddSystemLog(String.Format("{0} deleted MaterialID {1}, Material {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), e.Keys.Values(0).ToString.Trim, e.Values(1).ToString.Trim), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
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
        If e.NewValues("Material") = "" Then
            e.RowError = "Please enter Material"
        ElseIf e.NewValues("BegQty") = 0 Then
            e.RowError = "Please enter beginning balance"
        Else
            'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
            Dim DAL As New DAL

            If e.Keys.Values(0) = Nothing Then 'insert
                If DAL.AddMstrMaterial(e.NewValues("Material").ToString.Trim, e.NewValues("BegQty").ToString, SharedFunction.UserID(Page.User.Identity.Name)) Then
                    e.RowError = DAL.ObjectResult.ToString
                Else
                    e.RowError = DAL.ErrorMessage
                End If
            Else 'edit
                If DAL.EditMstrMaterial(e.OldValues("MaterialID").ToString.Trim, e.NewValues("Material").ToString.Trim, e.NewValues("BegQty").ToString) Then
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
        Server.Transfer(IO.Path.GetFileName(Request.Url.AbsolutePath))
    End Sub

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        ASPxPopupControl1.Height = 500
        ASPxPopupControl1.Width = 500
        ASPxPopupControl1.ShowOnPageLoad = True

        Session("MaterialID") = xGrid.GetRowValues(index, "MaterialID")
        Session("Material") = xGrid.GetRowValues(index, "Material")
    End Sub

End Class
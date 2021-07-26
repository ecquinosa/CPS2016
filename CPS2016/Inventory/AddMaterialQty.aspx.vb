
Public Class AddMaterialQty
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

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            lblMaterial.Text = "Material: " & Session("Material").ToString

            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblRelMaterialAddtl WHERE MaterialID = " & Session("MaterialID").ToString) Then
            xGrid.DataSource = DAL.TableResult
        End If
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF ADDT'L ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_RowDeleting(sender As Object, e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles xGrid.RowDeleting
        Dim DAL As New DAL
        If DAL.ExecuteQuery("DELETE FROM tblRelMaterialAddtl WHERE AddtlMaterialID=" & e.Keys.Values(0).ToString.Trim) Then
            e.Cancel = True
            DAL.AddSystemLog(String.Format("{0} deleted AddtlMaterialID {1}, AddedQty {2} of Material {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), e.Keys.Values(0).ToString.Trim, CInt(e.Values(0).ToString).ToString("N0"), Session("Material").ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            BindGrid()
        End If
    End Sub

    Private Sub xGrid_RowValidating(sender As Object, e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles xGrid.RowValidating
        If e.NewValues("AddedQty") = 0 Then
            e.RowError = "Please enter Quantity"
        Else
            Dim DAL As New DAL
            If e.Keys.Values(0) = Nothing Then 'insert
                If Not DAL.AddRelMaterialAddtl(Session("MaterialID").ToString, CInt(e.NewValues("AddedQty").ToString), SharedFunction.UserID(Page.User.Identity.Name)) Then
                    e.RowError = DAL.ErrorMessage
                End If
            Else
                If Not DAL.UpdateRelMaterialAddtlByID(CInt(e.OldValues("AddtlMaterialID").ToString), CInt(e.NewValues("AddedQty").ToString), e.NewValues("DateTimePosted").ToString) Then
                    e.RowError = DAL.ErrorMessage
                Else
                    DAL.AddSystemLog(String.Format("{0} edited AddtlMaterialID {1}, AddedQty {2}, DateTimePosted {3}, Material {4}", SharedFunction.UserCompleteName(Page.User.Identity.Name), e.Keys.Values(0).ToString.Trim, CInt(e.NewValues("AddedQty").ToString).ToString("N0"), e.NewValues("DateTimePosted").ToString, Session("Material").ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                End If
            End If

            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Private Sub xGrid_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles xGrid.RowUpdating
        e.Cancel = True
        xGrid.CancelEdit()
        BindGrid()
    End Sub

    Private Sub xGrid_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs) Handles xGrid.CellEditorInitialize
        If e.Column.FieldName = "AddtlMaterialID" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If xGrid.IsNewRowEditing Then
                txt.Text = "New"
                txt.Enabled = False
            End If
            'ElseIf e.Column.FieldName = "DateTimePosted" Then
            '    Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            '    If xGrid.IsNewRowEditing Then
            '        txt.Text = "New"
            '        txt.Enabled = False
            '    End If
        End If
    End Sub

    Private Sub xGrid_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles xGrid.RowInserting
        e.Cancel = True
        xGrid.CancelEdit()
        BindGrid()
    End Sub


End Class
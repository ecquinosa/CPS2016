
Imports DevExpress.Web.ASPxGridView.Rendering

Public Class Users
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.User) Then
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
        Dim strWhereCriteria As String = " WHERE ISNULL(IsActive,0)=1"
        If chkAllUser.Checked Then strWhereCriteria = ""
        If DAL.SelectQuery("SELECT * FROM tblUser" & strWhereCriteria) Then
            xGrid.DataSource = DAL.TableResult
        End If
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF SYSTEM USERS ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGrid_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs) Handles xGrid.CellEditorInitialize
        If e.Column.FieldName = "UserID" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If xGrid.IsNewRowEditing Then
                txt.Text = "New"
                txt.Enabled = False
            End If
        ElseIf e.Column.FieldName = "LogInAttmptCntr" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If xGrid.IsNewRowEditing Then
                txt.Text = "0"
                txt.Enabled = False
            End If
        ElseIf e.Column.FieldName = "EditType" Then
            Dim cmb As DevExpress.Web.ASPxEditors.ASPxComboBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxComboBox)
            cmb.Items.Add("Edit Record", 1)
            cmb.Items.Add("Reset Password", 2)
            cmb.Items.Add("Deactivate account", 3)
            cmb.Items.Add("Activate account", 4)
            'cmb.Items.Add("Change Username", 5)
            cmb.Items.Add("Reset Login counter", 5)
            cmb.ValueType = GetType(String)
            cmb.SelectedIndex = 0
            If xGrid.IsNewRowEditing Then cmb.Enabled = False
            End If
    End Sub

    Private Sub xGrid_ProcessOnClickRowFilter(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewOnClickRowFilterEventArgs) Handles xGrid.ProcessOnClickRowFilter
        BindGrid()
    End Sub

    Private Sub xGrid_RowDeleting(sender As Object, e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles xGrid.RowDeleting
        'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
        Dim DAL As New DAL
        If DAL.ExecuteQuery("UPDATE tblUser SET IsDeleted = 1 WHERE UserID=" & e.Keys.Values(0).ToString.Trim) Then
            e.Cancel = True
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
        If e.NewValues("Username") = "" Then
            e.RowError = "Please enter Username"
        ElseIf e.NewValues("FName") = "" Then
            e.RowError = "Please enter First Name"
        ElseIf e.NewValues("LName") = "" Then
            e.RowError = "Please enter Last Name"
        Else
            'Dim DAL As New MySqlDAL(SharedFunction.UserID(Page.User.Identity.Name))
            Dim DAL As New DAL

            If e.Keys.Values(0) = Nothing Then 'insert
                Dim strMiddleName As String = ""
                If Not e.NewValues("MName") Is Nothing Then strMiddleName = e.NewValues("MName").ToString.Trim
                If DAL.AddUser(e.NewValues("Username").ToString.Trim, My.Settings.UserDefPsswrd, e.NewValues("FName").ToString.Trim, strMiddleName, e.NewValues("LName").ToString.Trim) Then
                    e.RowError = DAL.ObjectResult.ToString
                    DAL.AddSystemLog(String.Format("{0} new user has been added", e.NewValues("Username").ToString.Trim), "Login", SharedFunction.UserID(Page.User.Identity.Name))
                Else
                    e.RowError = DAL.ErrorMessage
                End If
            Else 'edit
                Select Case CInt(e.NewValues("EditType"))
                    Case 1
                        Dim strMName As String = ""
                        If Not e.NewValues("MName") Is Nothing Then strMName = e.NewValues("MName").ToString.Trim
                        If DAL.EditUser(e.OldValues("UserID").ToString.Trim, e.NewValues("Username").ToString.Trim, e.NewValues("FName").ToString.Trim, strMName, e.NewValues("LName").ToString.Trim) Then
                            e.RowError = DAL.ObjectResult.ToString
                            DAL.AddSystemLog(String.Format("{0} details is edited", e.OldValues("Username").ToString), "Login", SharedFunction.UserID(Page.User.Identity.Name))
                        Else
                            e.RowError = DAL.ErrorMessage
                        End If
                    Case 2
                        If Not DAL.ResetUserPassword(e.OldValues("UserID").ToString) Then
                            e.RowError = DAL.ErrorMessage
                        Else
                            DAL.AddSystemLog(String.Format("{0} password has been reset", e.OldValues("Username").ToString), "Login", SharedFunction.UserID(Page.User.Identity.Name))
                        End If
                    Case 3
                        If Not DAL.ExecuteQuery("UPDATE tblUser SET IsActive=0" & " WHERE UserID=" & e.OldValues("UserID").ToString) Then
                            e.RowError = DAL.ErrorMessage
                        Else
                            DAL.AddSystemLog(String.Format("{0} account is deactivated", e.OldValues("Username").ToString), "Login", SharedFunction.UserID(Page.User.Identity.Name))
                        End If
                    Case 4
                        If Not DAL.ExecuteQuery("UPDATE tblUser SET IsActive=1" & " WHERE UserID=" & e.OldValues("UserID").ToString) Then
                            e.RowError = DAL.ErrorMessage
                        Else
                            DAL.AddSystemLog(String.Format("{0} account is activated", e.OldValues("Username").ToString), "Login", SharedFunction.UserID(Page.User.Identity.Name))
                        End If
                    Case 5
                        If Not DAL.ExecuteQuery("UPDATE tblUser SET LogInAttmptCntr=0" & " WHERE UserID=" & e.OldValues("UserID").ToString) Then
                            e.RowError = DAL.ErrorMessage
                        Else
                            DAL.AddSystemLog(String.Format("{0} login counter has been reset", e.OldValues("Username").ToString), "Login", SharedFunction.UserID(Page.User.Identity.Name))
                        End If
                End Select
            End If

            DAL.Dispose()
            DAL = Nothing
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect("Users.aspx")
    End Sub

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllUser.CheckedChanged
        GridDataBind()
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Session("UserID") = xGrid.GetRowValues(index, "UserID")
        Session("Name") = String.Format("{0} {1}{2}", xGrid.GetRowValues(index, "FName"), IIf(xGrid.GetRowValues(index, "MName") = "", "", xGrid.GetRowValues(index, "MName") & " "), xGrid.GetRowValues(index, "LName"))

        ASPxPopupControl1.Height = 600
        ASPxPopupControl1.Width = 500
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

End Class
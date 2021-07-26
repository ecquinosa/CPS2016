
Public Class SystemParameter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.SystemParameter) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            Session.RemoveAll()
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblSystemParameter") Then
            Dim rw As DataRow = DAL.TableResult.Rows(0)
            If Not IsDBNull(rw("SystemStatus")) Then chkSystemStatus.Checked = rw("SystemStatus")
            If Not IsDBNull(rw("FileEncryptionKey")) Then txtFileEncKey.Text = rw("FileEncryptionKey")
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub btnSubmitCards_Click(sender As Object, e As EventArgs) Handles btnSubmitCards.Click
        Dim DAL As New DAL
        If DAL.ExecuteQuery(String.Format("UPDATE tblSystemParameter SET SystemStatus={0}, FileEncryptionKey='{1}'", IIf(chkSystemStatus.Checked, 1, 0), txtFileEncKey.Text)) Then
            DAL.AddSystemLog(String.Format("{0} updated system parameter SystemStatus={1}, FileEncryptionKey='{2}'", SharedFunction.UserCompleteName(Page.User.Identity.Name), chkSystemStatus.Checked, txtFileEncKey.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            lblStatus.Text = "Changes has been saved"
            lblStatus.ForeColor = SharedFunction.SuccessColor
        Else
            DAL.AddErrorLog(String.Format("{0} failed to update sysetm parameter. Returned error {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            lblStatus.Text = "Failed to save changes"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

End Class
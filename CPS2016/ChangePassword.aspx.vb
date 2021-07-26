
Public Class ChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not IsPostBack Then
            txtOldPassword.Text = ""
            txtNewPassword.Text = ""
            txtConfirm.Text = ""
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If txtOldPassword.Text = "" Then
            lblStatus.Text = "Please enter old password"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf txtNewPassword.Text = "" Then
            lblStatus.Text = "Please enter new password"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf txtConfirm.Text = "" Then
            lblStatus.Text = "Please confirm password"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf txtNewPassword.Text.Length < 6 Then
            lblStatus.Text = "Password should have at least 6 minimum characters"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            If txtNewPassword.Text <> txtConfirm.Text Then
                lblStatus.Text = "New and confirm password are not identical"
                lblStatus.ForeColor = SharedFunction.ErrorColor
            Else
                Dim DAL As New DAL
                If DAL.ChangeUserPassword(SharedFunction.UserID(Page.User.Identity.Name), txtOldPassword.Text, txtNewPassword.Text) Then
                    If DAL.ObjectResult.ToString = "" Then
                        DAL.AddSystemLog(String.Format("{0} changed his/ her password", SharedFunction.UserID(Page.User.Identity.Name)), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    Else
                        DAL.AddErrorLog(String.Format("Changepassword(): {0} failed to change password. Returned error {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), DAL.ObjectResult.ToString), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                        lblStatus.Text = DAL.ObjectResult.ToString
                        lblStatus.ForeColor = SharedFunction.ErrorColor
                    End If
                Else
                    DAL.AddErrorLog(String.Format("Changepassword(): {0} failed to change password. Returned error {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), DAL.ErrorMessage), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    lblStatus.Text = "Failed to change password. Please try again or contact Administrator"
                    lblStatus.ForeColor = SharedFunction.ErrorColor
                End If
            End If
        End If
    End Sub


    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

End Class
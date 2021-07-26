
Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim DAL As New DAL
            Dim intUserID As Integer = SharedFunction.UserID(Page.User.Identity.Name)
            If DAL.ExecuteQuery("DELETE FROM tblLoggedUsers WHERE UserID=" & intUserID.ToString) Then
                DAL.AddSystemLog(String.Format("{0} has logged out ", SharedFunction.UserCompleteName(Page.User.Identity.Name)), "Logout", intUserID)
            End If
            Session.RemoveAll()
            System.Web.Security.FormsAuthentication.SignOut()
            Response.Redirect("~/Login/Login.aspx")
        Catch ex As Exception
        End Try
    End Sub

End Class
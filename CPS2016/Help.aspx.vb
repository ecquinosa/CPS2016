
Public Class Help
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            If Not SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin) Then
                lb1.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Viewer)
                lb2.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Indigo)
                lb3.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Assembly)
                lb4.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.QualityControl)
                lb5.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Personalization)
                lb6.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.InventoryAdmin)
            End If
        End If
    End Sub

    Private Sub ShowPopup()
        ASPxPopupControl1.ContentUrl = "~/Rpt/PDFViewer.aspx"
        ASPxPopupControl1.Height = 600
        ASPxPopupControl1.Width = 800
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub lb1_Click(sender As Object, e As EventArgs) Handles lb1.Click
        ASPxPopupControl1.HeaderText = "Viewer Role"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "View Role.pdf")
        ShowPopup()
    End Sub

    Protected Sub lb2_Click(sender As Object, e As EventArgs) Handles lb2.Click
        ASPxPopupControl1.HeaderText = "Indigo Role"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "Indigo Role.pdf")
        ShowPopup()
    End Sub

    Protected Sub lb3_Click(sender As Object, e As EventArgs) Handles lb3.Click
        ASPxPopupControl1.HeaderText = "Assembly Role"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "Assembly Role.pdf")
        ShowPopup()
    End Sub

    Protected Sub lb4_Click(sender As Object, e As EventArgs) Handles lb4.Click
        ASPxPopupControl1.HeaderText = "Quality Control Role"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "Quality Control Role.pdf")
        ShowPopup()
    End Sub

    Protected Sub lb5_Click(sender As Object, e As EventArgs) Handles lb5.Click
        ASPxPopupControl1.HeaderText = "Personalization Role"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "Personalization Role.pdf")
        ShowPopup()
    End Sub

    Protected Sub lb6_Click(sender As Object, e As EventArgs) Handles lb6.Click
        ASPxPopupControl1.HeaderText = "Inventory Admin Role"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "Inventory Admin Role.pdf")
        ShowPopup()
    End Sub

    Protected Sub lb7_Click(sender As Object, e As EventArgs) Handles lb7.Click
        ASPxPopupControl1.HeaderText = "CPS Activity Sequence"
        Session("pdfFile") = String.Format("{0}\{1}", SharedFunction.SystemRepository, "CPS Activity Sequence.pdf")
        ShowPopup()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

End Class
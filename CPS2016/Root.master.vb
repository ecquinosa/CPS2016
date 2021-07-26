
Public Class RootMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ASPxSplitter1.GetPaneByName("Header").Size = If(DevExpress.Web.ASPxClasses.ASPxWebControl.GlobalTheme = "Moderno", 95, 83)
            ASPxSplitter1.GetPaneByName("Header").MinSize = If(DevExpress.Web.ASPxClasses.ASPxWebControl.GlobalTheme = "Moderno", 95, 83)
            ASPxLabel2.Text = String.Format("User: {0}", SharedFunction.UserCompleteName(Page.User.Identity.Name))
            ASPxLabel3.Text = String.Format("Role: {0}", SharedFunction.RoleDesc(Page.User.Identity.Name))
            ASPxLabel4.Text = String.Format("Log in time: {0}", SharedFunction.LoginTime(Page.User.Identity.Name))
            ASPxLabel5.Text = String.Format("Host: {0}", My.Settings.Server)
            aspxLblCopyright.Text = Date.Now.Year.ToString() + Server.HtmlDecode(" &copy; Copyright by [Allcard Technologies, Corp.]")

            ASPxMenu1.Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Home_CPS)
            ASPxMenu1.Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.CardControl)

            ASPxMenu1.Items(2).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.User)
            ASPxMenu1.Items(2).Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Role)
            ASPxMenu1.Items(2).Items(2).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.SystemModule)

            If ASPxMenu1.Items(2).Items(0).Visible = False And ASPxMenu1.Items(2).Items(0).Visible = False And ASPxMenu1.Items(2).Items(0).Visible = False Then
                ASPxMenu1.Items(2).Visible = False
            End If

            'CheckItemsAndHide(ASPxMenu1)
        End If
    End Sub

    'Private Sub CheckItemsAndHide(ByVal groupTab As DevExpress.Web.ASPxMenu.ASPxMen)
    '    Dim bln As Boolean = False
    '    groupTab.ite()
    '    For i As Short = 0 To groupTab.Items.Count - 1
    '        For i2 As Short = 0 To groupTab. .Count - 1
    '            If groupTab.Items(i2).Visible = True Then
    '                bln = True
    '                Exit For
    '            End If
    '        Next
    '    Next

    '    groupTab.Visible = bln
    'End Sub

    'Protected Sub lbLogout_Click(sender As Object, e As EventArgs) Handles lbLogout.Click
    '    Server.Transfer("Logout.aspx")
    'End Sub    

    'Protected Sub ASPxMenu1_ItemClick(source As Object, e As DevExpress.Web.ASPxMenu.MenuItemEventArgs) Handles ASPxMenu1.ItemClick
    '    Session.RemoveAll()
    '    Server.Transfer("ForUpload.aspx")
    'End Sub

End Class
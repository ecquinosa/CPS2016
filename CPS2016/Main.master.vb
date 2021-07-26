

Public Class MainMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'PROCESS
            ASPxNavBar1.Groups(0).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO)
            ASPxNavBar1.Groups(0).Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.IndigoExtract)
            ASPxNavBar1.Groups(0).Items(2).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.IndigoExtract)
            ASPxNavBar1.Groups(0).Items(3).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.PendingForProcessCard)
            ASPxNavBar1.Groups(0).Items(4).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.PendingForProcessSheet)
            ASPxNavBar1.Groups(0).Items(5).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.RejectCard)
            ASPxNavBar1.Groups(0).Items(6).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Download)
            ASPxNavBar1.Groups(0).Items(7).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.QAGoodReject)
            ASPxNavBar1.Groups(0).Items(8).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO)
            ASPxNavBar1.Groups(0).Items(9).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO)
            ASPxNavBar1.Groups(0).Items(10).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO)

            'TRACKING
            ASPxNavBar1.Groups(1).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Search_Track)

            'DATA PURGE
            ASPxNavBar1.Groups(2).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Process_DataPurge)

            'ADMINISTRATION
            ASPxNavBar1.Groups(3).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.LoggedUsers)
            ASPxNavBar1.Groups(3).Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.SystemParameter)

            'INVENTORY
            ASPxNavBar1.Groups(4).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Home_Inventory)
            ASPxNavBar1.Groups(4).Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Inventory)
            ASPxNavBar1.Groups(4).Items(2).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Processed_Inv)
            ASPxNavBar1.Groups(4).Items(3).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Material)

            'LOGS
            ASPxNavBar1.Groups(5).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.SystemLog)
            ASPxNavBar1.Groups(5).Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.ErrorLog)

            'REPORTS
            ASPxNavBar1.Groups(6).Items(0).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.POReports)
            ASPxNavBar1.Groups(6).Items(1).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.MiscReports)
            ASPxNavBar1.Groups(6).Items(2).Visible = SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.DeliveryReceipt)

            For i As Short = 0 To 6
                CheckItemsAndHide(ASPxNavBar1.Groups(i))
            Next
        End If
    End Sub

    Private Sub CheckItemsAndHide(ByVal groupTab As DevExpress.Web.ASPxNavBar.NavBarGroup)
        Dim bln As Boolean = False
        For i As Short = 0 To groupTab.Items.Count - 1
            If groupTab.Items(i).Visible = True Then
                bln = True
                Exit For
            End If
        Next

        groupTab.Visible = bln
        groupTab.Expanded = bln
    End Sub

End Class

Imports System.IO

Public Class Home_Inventory
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Home_Inventory) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            GridDataBind_Inventory()
            GridDataBind_ProcessedPO()
            GridDataBind_ForAllotment()
        End If
    End Sub

    Private Sub xGridInventory_DataBinding(sender As Object, e As System.EventArgs) Handles xGridInventory.DataBinding
        BindGrid_Inventory()
    End Sub

    Private Sub xGridProcessed_DataBinding(sender As Object, e As System.EventArgs) Handles xGridProcessed.DataBinding
        BindGrid_ProcessedPO()
    End Sub

    Private Sub xGridForAllotment_DataBinding(sender As Object, e As System.EventArgs) Handles xGridForAllotment.DataBinding
        BindGrid_ForAllotment()
    End Sub
   
    Private Sub GridDataBind_Inventory()
        xGridInventory.DataBind()
    End Sub

    Private Sub GridDataBind_ProcessedPO()
        xGridProcessed.DataBind()
    End Sub

    Private Sub GridDataBind_ForAllotment()
        xGridForAllotment.DataBind()
    End Sub

    Private Sub BindGrid_Inventory()
        Dim DAL As New DAL
        If DAL.SelectMaterialInventory Then
            xGridInventory.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub BindGrid_ProcessedPO()
        Dim DAL As New DAL
        If DAL.SelectPOMaterial() Then
            xGridProcessed.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub BindGrid_ForAllotment()
        Dim DAL As New DAL
        If DAL.SelectPOForAllotment Then
            xGridForAllotment.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

End Class
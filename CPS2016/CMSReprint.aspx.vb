
Imports System.IO

Public Class CMSReprint
    Inherits System.Web.UI.Page

    Private dtSource As DataTable

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO) Then
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

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
        Dim strPurchaseOrder As String = xGrid.GetRowValues(index, "PurchaseOrder")
        Dim strBatch As String = xGrid.GetRowValues(index, "Batch")

        'UploadData(strPurchaseOrder.Trim.Substring(0, 33), strBatch.Trim, strPurchaseOrder.Trim) ', ID)
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectCMSReprintData Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_CMSReprintData"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("CMS REPRINT DATA")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs)
        Try
            Dim currentButton As LinkButton = TryCast(sender, LinkButton)
            Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
            Dim strPurchaseOrder As String = xGrid.GetRowValues(index, "PurchaseOrder")
            Dim DAL As New DAL
            DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE PurchaseOrder='{0}'", strPurchaseOrder))
            DAL.Dispose()
            DAL = Nothing
        Catch ex As Exception
        End Try

        BindGrid()
    End Sub

    Protected Sub btnNewUpload_Click(sender As Object, e As EventArgs) Handles btnNewUpload.Click
        Response.Redirect("ForUpload_CMSReprint.aspx")
    End Sub

End Class
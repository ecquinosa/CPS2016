
Public Class AllotedMaterialsBreakdown
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

            If Not Session("MaterialID") Is Nothing Then
                TryCast(xGrid.Columns("Material"), DevExpress.Web.ASPxGridView.GridViewDataColumn).GroupBy()
                GridDataBind()
            End If
        End If
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectAllotedMaterialsBreakdown(Session("MaterialID").ToString) Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub xGrid_BeforeGetCallbackResult(sender As Object, e As System.EventArgs) Handles xGrid.BeforeGetCallbackResult
        TryCast(xGrid.Columns("Material"), DevExpress.Web.ASPxGridView.GridViewDataColumn).GroupBy()
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_AllotedMaterialsBreakdown"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("BREAKDOWN OF ALLOTED MATERIALS")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

End Class
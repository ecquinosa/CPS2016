
Public Class SubmittedQAGoodReject
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.QAGoodReject) Then
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
        Dim strWhereCriteria As String = ""
        If Not Session("SSSRejectsDateTimePosted") Is Nothing Then strWhereCriteria = String.Format(" WHERE CONVERT(char(20),dbo.tblSSSReject.DateTimePosted,13)='{0}'", Session("SSSRejectsDateTimePosted").ToString)

        Dim DAL As New DAL
        If DAL.SelectSSSRejects(strWhereCriteria) Then
            xGrid.DataSource = DAL.TableResult
        End If
        lblTotal.Text = "Total record(s): " & DAL.TableResult.DefaultView.Count.ToString

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Server.Transfer("ProcessedQAGoodReject.aspx")
    End Sub

    Private Function ReportName() As String
        Return "CustomerReturns_" & Now.ToString("yyyMMdd_hhmm")
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("CUSTOMER RETURNS")
        sb.AppendLine(Session("SSSRejectsDateTimePosted").ToString)

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

End Class

Imports System.IO

Public Class ForUpload_CMSReprint
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
            'If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            CreateTable()

            GridDataBind()
        End If
    End Sub

    Private Sub CreateTable()
        If dtSource Is Nothing Then
            dtSource = New DataTable
            dtSource.Columns.Add("SourceFile", Type.GetType("System.String"))
            dtSource.Columns.Add("DateTimePosted", Type.GetType("System.DateTime"))
        Else
            dtSource.Clear()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
        Dim strSourceFile As String = xGrid.GetRowValues(index, "SourceFile")

        UploadData(strSourceFile)
    End Sub

    Private Sub UploadData(ByVal strSourceFile As String)
        ASPxMemo1.Visible = False

        Dim strPrevPO As String = ""
        Dim strCurrentPO As String = ""
        Dim dtmPosted As DateTime = Now
        Dim intError As Short = 0
        Dim intTotalError As Short = 0
        Dim intCntr As Short = 0

        Dim sb As New StringBuilder

        sb.AppendLine(String.Format("Start of UploadData() process {0}<br>", Now.ToString))
        sb.AppendLine(String.Format("Source file {0}<br>", strSourceFile))

        Dim strFile As String = String.Format("{0}\{1}", SharedFunction.CMS_ReprintRepository, strSourceFile)

        Dim DAL As New DAL
        Dim strLines() As String = File.ReadAllLines(strFile)
        For Each strLine As String In strLines
            strCurrentPO = strLine.Split("|")(0)
            If strPrevPO = "" Then
                dtmPosted = Now
                sb.AppendLine(String.Format("Purchase Order {0}<br>", strCurrentPO))
            ElseIf strPrevPO <> strCurrentPO Then
                dtmPosted = Now
                sb.AppendLine(String.Format("Total: {0}, Error: {1}<br><br>", intCntr.ToString, intError.ToString))
                sb.AppendLine(String.Format("Purchase Order {0}<br>", strCurrentPO))
                intCntr = 0
                intError = 0
            End If

            If DAL.AddCMSReprintData(strCurrentPO, strLine.Split("|")(1), strLine.Split("|")(2), strLine.Split("|")(3), strLine.Split("|")(4), dtmPosted, strSourceFile) Then
                intCntr += 1
            Else
                intError += 1
                intTotalError += 1
                sb.AppendLine(String.Format("Purchase Order {0}, Barcode {1}<br>", strCurrentPO, strLine.Split("|")(1)))
            End If
            strPrevPO = strLine.Split("|")(0)
        Next

        sb.AppendLine(String.Format("Total: {0}, Error: {1}<br><br>", intCntr.ToString, intError.ToString))
        sb.AppendLine(String.Format("Total Error: {0}<br>", intTotalError.ToString))
        sb.AppendLine(String.Format("End process {0}<br>", Now.ToString) & vbNewLine)

        If intTotalError = 0 Then
            If Directory.Exists(SharedFunction.CMS_ReprintRepository & "\archive") Then File.Move(strFile, SharedFunction.CMS_ReprintRepository & "\archive\" & strSourceFile)
            If Not Session("dtSource") Is Nothing Then dtSource = CType(Session("dtSource"), DataTable)
            dtSource.Select("SourceFile='" & Path.GetFileName(strFile) & "'")(0).Delete()
            Session("dtSource") = dtSource
            DAL.AddSystemLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        Else
            DAL.AddErrorLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        End If

        DAL.Dispose()
        DAL = Nothing

        ASPxMemo1.Visible = True
        ASPxMemo1.Text = sb.ToString.Replace("<br>", "")

        GridDataBind()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        If Not Session("dtSource") Is Nothing Then dtSource = CType(Session("dtSource"), DataTable)

        Dim DAL As New DAL
        For Each strFile As String In Directory.GetFiles(SharedFunction.CMS_ReprintRepository)
            If dtSource.Select("SourceFile='" & Path.GetFileName(strFile) & "'").Length = 0 Then
                If DAL.ExecuteScalar("SELECT COUNT(SourceFile) FROM dbo.tblCMSReprintData WHERE SourceFile='" & Path.GetFileName(strFile) & "'") Then
                    If CInt(DAL.ObjectResult) = 0 Then
                        Dim rw As DataRow = dtSource.NewRow
                        rw("SourceFile") = Path.GetFileName(strFile)
                        rw("DateTimePosted") = New FileInfo(strFile).CreationTime
                        dtSource.Rows.Add(rw)
                    End If
                End If
            End If
        Next

        Session("dtSource") = dtSource
        xGrid.DataSource = dtSource

        DAL.Dispose()
        DAL = Nothing
    End Sub
 
    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_ForUpload_CMSReprintData"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("FOR UPLOAD - CMS REPRINT DATA")
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

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Session.RemoveAll()
        Response.Redirect("CMSReprint.aspx")
    End Sub

End Class
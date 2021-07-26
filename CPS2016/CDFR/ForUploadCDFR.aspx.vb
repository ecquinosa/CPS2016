
Imports System.IO

Public Class ForUploadCDFR
    Inherits System.Web.UI.Page

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
        Dim strReference As String = xGrid.GetRowValues(index, "GSUFilename")

        UploadData(strReference)
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs)
        Try
            Dim currentButton As LinkButton = TryCast(sender, LinkButton)
            Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
            Dim strReference As String = xGrid.GetRowValues(index, "Reference")
            Dim DAL As New DAL
            DAL.ExecuteQuery(String.Format("DELETE FROM tblForUpload WHERE Reference='{0}'", strReference))
            DAL.Dispose()
            DAL = Nothing
        Catch ex As Exception
        End Try

        BindGrid()
    End Sub

    Private Sub UploadData(ByVal strReference As String)
        ASPxMemo1.Visible = False
        Dim sb As New StringBuilder

        Dim cdfrFile As String = String.Format("{0}\{1}.txt", SharedFunction.ForUploadingCDFRRepository, strReference)
        Dim cdfr As New CDFR_Reader(cdfrFile)
        Dim intGenTypeCntr As Short = 0
        If cdfr.Read() Then
            Dim IsSuccess As Boolean
            SharedFunction.UploadDataCDFR(strReference, cdfr.CDFR_Table, IsSuccess, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name), sb)
        End If

        If cdfr.ErrorMessage <> "" Then
            sb.AppendLine()
            sb.AppendLine(cdfr.ErrorMessage)
        End If

        ASPxMemo1.Visible = True
        ASPxMemo1.Text = sb.ToString.Replace("<br>", "")

        GridDataBind()
    End Sub

    Private Sub CDFRvsUMID(ByVal strReference As String)
        ASPxMemo1.Visible = False
        Dim sb As New StringBuilder

        Dim cdfrFile As String = String.Format("{0}\{1}.txt", SharedFunction.ForUploadingRepository, strReference)

        ASPxMemo1.Visible = True
        ASPxMemo1.Text = sb.ToString.Replace("<br>", "")

        GridDataBind()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL

        Dim dtCDFR As New DataTable
        dtCDFR.Columns.Add("GSUFilename", GetType(String))
        dtCDFR.Columns.Add("Quantity", GetType(String))
        dtCDFR.Columns.Add("DateTimePosted", GetType(DateTime))

        For Each strFile In Directory.GetFiles(SharedFunction.ForUploadingCDFRRepository)
            Dim cdfr As String = Path.GetFileNameWithoutExtension(strFile)

            If DAL.ExecuteScalar("SELECT COUNT(GSUfilename) FROM tblCDFR WHERE GSUfilename = '" & cdfr & "'") Then
                If CInt(DAL.ObjectResult) = 0 Then
                    'If DAL.AddForUpload(cdfr, New FileInfo(strFile).CreationTime, File.ReadAllLines(strFile).Length, "CDFR") Then
                    'End If
                    Dim rw As DataRow = dtCDFR.NewRow
                    rw(0) = cdfr
                    rw(1) = File.ReadAllLines(strFile).Length
                    rw(2) = New FileInfo(strFile).CreationTime
                    dtCDFR.Rows.Add(rw)
                End If
            End If
        Next

        'If DAL.SelectQuery("SELECT * FROM tblForUpload") Then
        xGrid.DataSource = dtCDFR 'DAL.TableResult
        'End If

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Function IsPOExist(ByVal strPO As String, ByVal dt As DataTable) As Boolean
        If dt.Select("PurchaseOrder='" & strPO & "'").Length = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_ForUploadCDFR"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("FOR UPLOAD CDFR")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

End Class
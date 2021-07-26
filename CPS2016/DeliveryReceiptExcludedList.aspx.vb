
Public Class DeliveryReceiptExcludedList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.DeliveryReceipt) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            If Not Session("DR_Param") Is Nothing Then
                Dim param As String = Session("DR_Param")
                txtPO.Text = param.Split("|")(0)
            End If

            If Not Session("txtDRExcludedBarcodes") Is Nothing Then
                txtSource.Text = Session("txtDRExcludedBarcodes").ToString
            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        lblStatus.ForeColor = Drawing.Color.Black
        lblStatus.Text = ""

        If txtSource.Text = "" Then
            lblStatus.Text = "Please enter barcode/s"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            Dim dt As New DataTable
            dt.Columns.Add("Barcode", Type.GetType("System.String"))
            For Each strLine As String In txtSource.Text.Split(vbNewLine)
                Dim rw As DataRow = dt.NewRow
                rw(0) = strLine.Trim
                dt.Rows.Add(rw)
            Next

            Session("txtDRExcludedBarcodes") = txtSource.Text
            Session("dtDRExcludedBarcodes") = dt

            lblStatus.Text = "Saved!"
            lblStatus.ForeColor = SharedFunction.SuccessColor
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Sub ViewDownloadFile(ByVal strFile As String)
        HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
        Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
        HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
        Dim Dfile As New System.IO.FileInfo(strFile)
        HttpContext.Current.Response.WriteFile(Dfile.FullName)
        HttpContext.Current.Response.[End]()
    End Sub

    Private Function ReportName() As String
        Return "CPS_DeliveryReceipt"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine(txtPO.Text & " Delivery Receipt - Barcode Excluded")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        'xGridExporter.FileName = ReportName()
        'xGridExporter.PageHeader.Left = ReportHeader()

        'If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect(Session("OriginatingPage").ToString)
    End Sub

End Class
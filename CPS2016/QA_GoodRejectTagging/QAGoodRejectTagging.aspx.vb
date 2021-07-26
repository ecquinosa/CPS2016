
Public Class QAGoodRejectTagging
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

            CreateTable()
            PopulateRejectType()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Function ValidateParam() As Boolean
        Dim bln As Boolean = False

        If txtBarcode.Text = "" And txtCRN.Text = "" Then
            lblStatus.Text = "Please enter Barcode or CRN"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf txtBarcode.Text.Length <> 20 And txtCRN.Text = "" Then
            lblStatus.Text = "Please enter valid Barcode"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf txtCRN.Text.Length <> 14 And txtBarcode.Text = "" Then
            lblStatus.Text = "Please enter valid CRN"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf cboTag.Text = "-Select-" Then
            lblStatus.Text = "Please select Tag"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        ElseIf cboTag.Text = "Reject" And cboRejectType.Text = "-Select-" Then
            lblStatus.Text = "Please select Reject Type"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            Dim dt As DataTable = CType(Session("dtGrid"), DataTable)
            If dt.Select("Barcode='" & txtBarcode.Text & "'").Length > 0 Then
                lblStatus.Text = "Duplicate Barcode entry not allowed"
                lblStatus.ForeColor = SharedFunction.ErrorColor
            ElseIf dt.Select("CRN='" & txtCRN.Text & "'").Length > 0 Then
                lblStatus.Text = "Duplicate CRN entry not allowed"
                lblStatus.ForeColor = SharedFunction.ErrorColor
            Else
                bln = True
            End If
        End If

        GridDataBind()

        Return bln
    End Function

    Private Sub BindGrid()
        Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

        If Not Session("IsAdd") Is Nothing Then
            If Session("IsAdd") = 1 Then
                Dim rw As DataRow = dt.NewRow
                If txtPO.Text <> "" Then rw("PurchaseOrder") = txtPO.Text
                If txtBarcode.Text <> "" Then rw("Barcode") = txtBarcode.Text
                If txtCRN.Text <> "" Then rw("CRN") = txtCRN.Text
                rw("Tag") = cboTag.Text
                If cboTag.Text = "Reject" Then
                    rw("RejectTypeID") = cboRejectType.SelectedItem.Value
                    rw("RejectTypeDesc") = cboRejectType.SelectedItem.Text
                End If
                dt.Rows.Add(rw)
                dt.AcceptChanges()
                Session("dtGrid") = dt
                Session("IsAdd") = Nothing

                ResetPage()
            End If
        End If

        xGrid.DataSource = CType(Session("dtGrid"), DataTable)
        lblTotal.Text = "Total record(s): " & dt.DefaultView.Count.ToString
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub CreateTable()
        Dim dt As New DataTable
        Dim col As New DataColumn
        col.ColumnName = "QAGRID"
        col.AutoIncrement = True
        col.AutoIncrementSeed = 1
        col.AutoIncrementStep = 1
        dt.Columns.Add(col)
        dt.Columns.Add("Barcode", Type.GetType("System.String"))
        dt.Columns.Add("CRN", Type.GetType("System.String"))
        dt.Columns.Add("PurchaseOrder", Type.GetType("System.String"))
        dt.Columns.Add("Tag", Type.GetType("System.String"))
        dt.Columns.Add("RejectTypeID", Type.GetType("System.Int16"))
        dt.Columns.Add("RejectTypeDesc", Type.GetType("System.String"))
        dt.AcceptChanges()

        Session("dtGrid") = dt
    End Sub

    Protected Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        lblStatus.ForeColor = Drawing.Color.Black
        lblStatus.Text = ""

        Dim IsBarcodeBoxSelected As Boolean = True
        If txtCRN.Text <> "" Then IsBarcodeBoxSelected = False

        If ValidateParam() Then
            Session("IsAdd") = 1
            GridDataBind()

            If IsBarcodeBoxSelected Then txtBarcode.Focus() Else txtCRN.Focus()
        End If
    End Sub

    Private Sub ResetPage()
        'txtPO.Text = ""
        txtBarcode.Text = ""
        txtCRN.Text = ""
        'cboTag.SelectedIndex = 0
        'cboRejectType.SelectedIndex = 0
    End Sub

    Protected Sub cboTag_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTag.SelectedIndexChanged
        If cboTag.Text = "Good" Then
            cboRejectType.Enabled = False
        Else
            cboRejectType.Enabled = True
        End If
        GridDataBind()
    End Sub

    Private Sub PopulateRejectType()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblMstrRejectType") Then
            cboRejectType.Items.Add("-Select-", 0)
            cboRejectType.SelectedIndex = 0

            For Each rw As DataRow In DAL.TableResult.Rows
                cboRejectType.Items.Add(rw("RejectTypeDesc"), rw("RejectTypeID"))
            Next
        Else
            cboRejectType.Text = "Unable to populate data"
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim strIDs As String = GetSelectedItems_IDs()

        If strIDs <> "" Then
            Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

            For Each rw As DataRow In dt.Select("QAGRID IN (" & strIDs & ")")
                rw.Delete()
            Next
            dt.AcceptChanges()
            Session("dtGrid") = dt
        Else
            lblStatus.Text = "No data to delete"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        End If

        GridDataBind()
    End Sub

    Private Function GetSelectedItems_IDs() As String
        Dim sb As New StringBuilder
        For i As Short = 0 To xGrid.GetSelectedFieldValues("QAGRID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("QAGRID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("QAGRID")(i)))
            End If
        Next

        Return sb.ToString
    End Function

    Protected Sub btnSubmitCards_Click(sender As Object, e As EventArgs) Handles btnSubmitCards.Click
        Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

        If dt.DefaultView.Count > 0 Then
            Dim intError As Integer = 0

            Dim sbLog As New StringBuilder
            sbLog.AppendLine("Barcode|CRN|Result")

            Dim DAL As New DAL
            Dim dtmBatchDateTimePosted As Date = Now
            For Each rw As DataRow In dt.Rows
                If Not DAL.AddSSSReject(IIf(IsDBNull(rw("Barcode")), DBNull.Value, rw("Barcode")), IIf(IsDBNull(rw("CRN")), DBNull.Value, rw("CRN")), IIf(IsDBNull(rw("PurchaseOrder")), DBNull.Value, rw("PurchaseOrder")), rw("Tag"), IIf(IsDBNull(rw("RejectTypeID")), DBNull.Value, rw("RejectTypeID")), SharedFunction.UserID(Page.User.Identity.Name), dtmBatchDateTimePosted) Then
                    intError += 1
                    Dim strError As String = String.Format("{0} failed to add to sss reject Barcode {1} CRN {2} PO {3}. Runtime error {4}", SharedFunction.UserCompleteName(Page.User.Identity.Name), IIf(IsDBNull(rw("Barcode")), "", rw("Barcode")), IIf(IsDBNull(rw("CRN")), "", rw("CRN")), IIf(IsDBNull(rw("PurchaseOrder")), "", rw("PurchaseOrder")), DAL.ErrorMessage)
                    DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}", IIf(IsDBNull(rw("Barcode")), "", rw("Barcode")), IIf(IsDBNull(rw("CRN")), "", rw("CRN")), "Failed"))
                Else
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}", IIf(IsDBNull(rw("Barcode")), "", rw("Barcode")), IIf(IsDBNull(rw("CRN")), "", rw("CRN")), "Success"))
                End If
            Next

            DAL.Dispose()
            DAL = Nothing

            'SharedFunction.SaveToTextfile(String.Format("{0}\QAGoodReject_Qty{1}_{2}.txt", SharedFunction.QCRepository, dt.DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt")), sbLog.ToString, New StringBuilder)
            SharedFunction.SaveToTextfile(String.Format("{0}\QAGoodReject_Qty{1}_{2}_{3}.txt", SharedFunction.ProcessLogRepository, dt.DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt"), SharedFunction.UserCompleteName(Page.User.Identity.Name)), sbLog.ToString, New StringBuilder)

            Server.Transfer("ProcessedQAGoodReject.aspx")

            'ResetPage()

            'If intError = 0 Then
            '    lblStatus.Text = "Process is done"
            '    lblStatus.ForeColor = SharedFunction.SuccessColor
            'Else
            '    lblStatus.Text = String.Format("Process is done with {0} error(s) encountered", intError.ToString)
            '    lblStatus.ForeColor = SharedFunction.ErrorColor
            'End If

            'lblTotal.Text = "Total Record(s): 0"

            'GridDataBind()        '
        Else
            lblStatus.Text = "No data to delete"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            GridDataBind()
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Server.Transfer("ProcessedQAGoodReject.aspx")
    End Sub

End Class

Public Class StatusCardByPO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblStatus.Text = "Status: " & Session("ActivityDesc").ToString

            PopulateStatus()

            If Not Session("ResetMsg") Is Nothing Then
                lblStatus.Text = Session("ResetMsg").ToString
                If Session("ResetMsg").ToString = "Process is done" Then
                    lblStatus.ForeColor = SharedFunction.SuccessColor
                Else
                    lblStatus.ForeColor = SharedFunction.ErrorColor
                End If

                Session.RemoveAll()
            End If

            Select Case CType(Session("ActivityID"), DataKeysEnum.ActivityID)
                Case DataKeysEnum.ActivityID.IndigoDownload, DataKeysEnum.ActivityID.Done
                    btnProcess.Visible = False
                    lblResult.Visible = False
                Case DataKeysEnum.ActivityID.Assembly
                    If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin) Then
                    ElseIf SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Supervisor, DataKeysEnum.RoleID.StatusChangerAdmin) Then
                        btnProcess.Visible = SharedFunction.IsHaveAccessToActivity(SharedFunction.UserActivity(Page.User.Identity.Name), DataKeysEnum.ActivityID.Assembly)
                        lblResult.Visible = btnProcess.Visible
                    Else
                        btnProcess.Visible = False
                        lblResult.Visible = False
                    End If
                Case Else
                    btnProcess.Visible = SharedFunction.IsHaveAccessToActivity(SharedFunction.UserActivity(Page.User.Identity.Name), CType(Session("ActivityID"), DataKeysEnum.ActivityID))
                    lblResult.Visible = btnProcess.Visible
            End Select

            cboPO.Enabled = SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Supervisor, DataKeysEnum.RoleID.StatusChangerAdmin)

            GridDataBind()
        End If
    End Sub

    Private Sub PopulateStatus()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT ActivityID, ActivityDesc FROM dbo.tblMstrActivity WHERE ActivityID <> " & Session("ActivityID").ToString) Then
            For Each rw As DataRow In DAL.TableResult.Rows
                cboPO.Items.Add(rw("ActivityDesc"), rw("ActivityID"))
            Next

            For i As Short = 0 To cboPO.Items.Count - 1
                If cboPO.Items(i).Value.ToString = SharedFunction.GetNextActivity(Session("ActivityID")) Then cboPO.SelectedIndex = i
            Next
        Else
            cboPO.Text = "Unable to populate data"
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectPOPendingByStatus(Session("ActivityID")) Then
            Dim dt As DataTable = DAL.TableResult
            xGrid.DataSource = dt
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub ChangeCardStatus(ByVal POID As Integer, ByRef intError As Integer)
        Try
            'Dim intError As Integer = 0

            Dim sbLog As New StringBuilder
            sbLog.AppendLine("POID|CardID|Barcode|CRN|Result")

            Dim intActivityID As Integer = Session("ActivityID")

            Dim DAL As New DAL
            Dim dt As DataTable
            If DAL.SelectPOPendingBreakdown(POID, intActivityID) Then
                dt = DAL.TableResult
            End If

            For Each rw As DataRow In dt.Rows
                If DAL.UpdateCardStatusByCardID(rw("CardID"), CInt(cboPO.SelectedItem.Value)) Then
                    DAL.AddCardActivity(rw("POID"), rw("CRN"), rw("Barcode"), String.Format("{0} changed the status of record from {1} to {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(intActivityID), SharedFunction.GetActivityDesc(CInt(cboPO.SelectedItem.Value))), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", rw("POID"), ("CardID"), rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Success"))
                Else
                    intError += 1
                    Dim strError As String = String.Format("{0} failed to change the status of record from {1} to {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(intActivityID), SharedFunction.GetActivityDesc(CInt(cboPO.SelectedItem.Value)), DAL.ErrorMessage)
                    DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", rw("POID"), rw("CardID"), rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Failed"))
                End If
            Next

            DAL.Dispose()
            DAL = Nothing

            SharedFunction.SaveToTextfile(String.Format("{0}\ProcessCards_Qty{1}_{2}_{3}.txt", SharedFunction.ProcessLogRepository, dt.DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt"), SharedFunction.UserCompleteName(Page.User.Identity.Name)), sbLog.ToString, New StringBuilder)
        Catch ex As Exception
            lblResult.Text = "Failed to save changes"
            lblResult.ForeColor = SharedFunction.ErrorColor
        End Try
    End Sub

    Private Function GetSelectedItems_POID() As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGrid.GetSelectedFieldValues("POID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            End If
        Next

        Return sb.ToString
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        lblResult.Text = ""

        Dim strIDs As String = GetSelectedItems_POID()

        Dim intError As Integer = 0

        If strIDs = "" Then
            lblStatus.Text = "Please select item(s) to process"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            For Each strID As String In strIDs.Split(",")
                ChangeCardStatus(strID, intError)
            Next

            If intError = 0 Then
                Session("ResetMsg") = "Process is done"
            Else
                Session("ResetMsg") = String.Format("Process is done with {0} error(s) encountered", intError.ToString)
            End If

            Response.Redirect(Request.Url.AbsolutePath)

            lblResult.Text = "Changes has been saved"
            lblResult.ForeColor = SharedFunction.SuccessColor
        End If

        GridDataBind()
    End Sub
   
    Protected Sub btnProcess0_Click(sender As Object, e As EventArgs) Handles btnProcess0.Click
        lblResult.Text = ""

        'Dim strIDs As String = GetSelectedItems_POID()
        Dim strIDs As String = TextBox1.Text '"3363,3364,3365,3366,3367,3368,3369,3370,3371,3372,3373,3374,3375,3376,3377,3378,3380,3381,3382,3383,3384,3385,3386,3387,3388,3389,3390,3391,3392,3393,3394,3395,3396,3397,3398,3399,3400,3401,3403,3404,3405,3406"

        Dim intError As Integer = 0

        If strIDs = "" Then
            lblStatus.Text = "Please select item(s) to process"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            For Each strID As String In strIDs.Split(",")
                ChangeCardStatus(strID, intError)
            Next

            If intError = 0 Then
                Session("ResetMsg") = "Process is done"
            Else
                Session("ResetMsg") = String.Format("Process is done with {0} error(s) encountered", intError.ToString)
            End If

            Response.Redirect(Request.Url.AbsolutePath)

            lblResult.Text = "Changes has been saved"
            lblResult.ForeColor = SharedFunction.SuccessColor
        End If

        GridDataBind()
    End Sub
End Class
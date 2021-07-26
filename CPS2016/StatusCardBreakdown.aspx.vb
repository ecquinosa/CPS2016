
Public Class StatusCardBreakdown
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblStatus.Text = "Status: " & Session("ActivityDesc").ToString

            PopulatePurchaseOrder()

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
                    If Not SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.Supervisor) Then
                        btnProcess.Visible = False
                        lblResult.Visible = False
                    End If
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

            'If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin) Then
            '    Select Case CType(Session("ActivityID"), DataKeysEnum.ActivityID)
            '        Case DataKeysEnum.ActivityID.Done
            '            btnProcess.Visible = False
            '            lblResult.Visible = False
            '    End Select
            '    'ElseIf SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.Admin) Then
            '    '    Select Case CType(Session("ActivityID"), DataKeysEnum.ActivityID)
            '    '        Case DataKeysEnum.ActivityID.Done
            '    '            btnProcess.Visible = False
            '    '            lblResult.Visible = False
            '    '    End Select            
            'Else
            '    Select Case CType(Session("ActivityID"), DataKeysEnum.ActivityID)
            '        Case DataKeysEnum.ActivityID.IndigoDownload, DataKeysEnum.ActivityID.Assembly, DataKeysEnum.ActivityID.Done
            '            btnProcess.Visible = False
            '            lblResult.Visible = False
            '    End Select

            '    If Not SharedFunction.IsHaveAccessToActivity(SharedFunction.UserActivity(Page.User.Identity.Name), CType(Session("ActivityID"), DataKeysEnum.ActivityID)) Then
            '        btnProcess.Visible = False
            '        lblResult.Visible = False
            '    End If
            'End If

            'GridDataBind()
        End If
    End Sub

    Private Sub PopulatePurchaseOrder()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT DISTINCT dbo.tblPO.PurchaseOrder FROM dbo.tblRelPOData INNER JOIN dbo.tblPO ON dbo.tblRelPOData.POID = dbo.tblPO.POID WHERE dbo.tblRelPOData.ActivityID = " & Session("ActivityID").ToString) Then
            cboPO.Items.Add("-Select-", 0)
            cboPO.SelectedIndex = 0

            For Each rw As DataRow In DAL.TableResult.Rows
                cboPO.Items.Add(rw("PurchaseOrder"), rw("PurchaseOrder"))
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
        If cboPO.SelectedIndex > 0 Then
            If DAL.SelectDataByStatusAndPO(Session("ActivityID"), cboPO.Text) Then
                Dim dt As DataTable = DAL.TableResult
                xGrid.DataSource = dt
            End If
        Else
            If DAL.SelectDataByStatus(Session("ActivityID")) Then
                Dim dt As DataTable = DAL.TableResult

                xGrid.DataSource = dt
            End If
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    'Private Function GetPrintable(ByVal intTotal As Integer) As Integer
    '    'Dim intTotal As Integer = 0
    '    Dim intDivisibleBy21 As Integer = 0
    '    Dim strQuotient As String
    '    Dim intDrop As Integer = 0

    '    If intTotal > 0 Then
    '        If intTotal < 21 Then
    '            intDrop = intTotal
    '        Else
    '            strQuotient = (intTotal / 21).ToString
    '            If strQuotient.Contains(".") Then
    '                Dim intDivisor As Integer = strQuotient.Split(".")(0)
    '                intDivisibleBy21 = 21 * intDivisor
    '                intDrop = intTotal - intDivisibleBy21
    '            Else
    '                intDivisibleBy21 = intTotal
    '            End If
    '        End If
    '    End If

    '    Return intDivisibleBy21
    'End Function

    'Private Sub ReComputePageAndSeries(ByRef dt As DataTable, Optional ByVal intPrintable As Integer = 0)
    '    Try
    '        Dim intPage As Integer = 1
    '        Dim intSeries As Integer = 1
    '        Dim intPageCntr As Integer = 1

    '        For Each rw As DataRow In dt.Rows
    '            'rw("CurrentPage") = intPage
    '            'rw("CurrentSeries") = intSeries

    '            If intPrintable = 0 And dt.DefaultView.Count < SharedFunction.CardsPerSheet Then
    '                rw("Type") = "Drop"
    '            ElseIf intPrintable > 0 And dt.Columns.Contains("Type") Then
    '                If rw("CurrentSeries") > intPrintable Then
    '                    rw("Type") = "Drop"
    '                End If
    '            Else
    '                rw("Type") = "Printable"
    '            End If

    '            rw.AcceptChanges()

    '            intSeries += 1

    '            If intPageCntr <> SharedFunction.CardsPerSheet Then
    '                intPageCntr += 1
    '            Else
    '                intPage += 1
    '                intPageCntr = 1
    '            End If
    '        Next
    '    Catch ex As Exception
    '        'IO.File.WriteAllText(String.Format("{0}\status2.txt", SharedFunction.MainRepository), ex.Message)
    '    End Try
    'End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub SaveChanges()
        Try
            If xGrid.GetSelectedFieldValues("CardID").Count > 0 Then
                Dim DAL As New DAL

                Dim intError As Integer = 0

                Dim sbLog As New StringBuilder
                sbLog.AppendLine("POID|CardID|Barcode|CRN|Result")

                Dim intActivityID As Integer = Session("ActivityID")

                For i As Short = 0 To xGrid.GetSelectedFieldValues("CardID").Count - 1
                    'If DAL.UpdateCardStatusByCardID(xGrid.GetSelectedFieldValues("CardID")(i), SharedFunction.GetNextActivity(intActivityID)) Then
                    If DAL.UpdateCardStatusByCardID(xGrid.GetSelectedFieldValues("CardID")(i), GetNextActivity(intActivityID)) Then
                        'DAL.AddCardActivity(xGrid.GetSelectedFieldValues("POID")(i), xGrid.GetSelectedFieldValues("CRN")(i), xGrid.GetSelectedFieldValues("Barcode")(i), String.Format("{0} changed the status of record from {1} to {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(intActivityID), SharedFunction.GetActivityDesc(SharedFunction.GetNextActivity(intActivityID))), SharedFunction.UserID(Page.User.Identity.Name))
                        DAL.AddCardActivity(xGrid.GetSelectedFieldValues("POID")(i), xGrid.GetSelectedFieldValues("CRN")(i), xGrid.GetSelectedFieldValues("Barcode")(i), String.Format("{0} changed the status of record from {1} to {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(intActivityID), SharedFunction.GetActivityDesc(GetNextActivity(intActivityID))), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))
                        sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", xGrid.GetSelectedFieldValues("POID")(i), xGrid.GetSelectedFieldValues("CardID")(i), xGrid.GetSelectedFieldValues("Barcode")(i).ToString.Trim, xGrid.GetSelectedFieldValues("CRN")(i).ToString.Trim, "Success"))
                    Else
                        intError += 1
                        'Dim strError As String = String.Format("{0} failed to change the status of record from {1} to {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(intActivityID), SharedFunction.GetActivityDesc(SharedFunction.GetNextActivity(intActivityID)), DAL.ErrorMessage)
                        Dim strError As String = String.Format("{0} failed to change the status of record from {1} to {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(intActivityID), SharedFunction.GetActivityDesc(GetNextActivity(intActivityID)), DAL.ErrorMessage)
                        DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                        sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", xGrid.GetSelectedFieldValues("POID")(i), xGrid.GetSelectedFieldValues("CardID")(i), xGrid.GetSelectedFieldValues("Barcode")(i).ToString.Trim, xGrid.GetSelectedFieldValues("CRN")(i).ToString.Trim, "Failed"))
                    End If
                Next

                DAL.Dispose()
                DAL = Nothing

                SharedFunction.SaveToTextfile(String.Format("{0}\ProcessCards_Qty{1}_{2}_{3}.txt", SharedFunction.ProcessLogRepository, xGrid.VisibleRowCount.ToString, Now.ToString("MMddyy_hhmmtt"), SharedFunction.UserCompleteName(Page.User.Identity.Name)), sbLog.ToString, New StringBuilder)

                If intError = 0 Then
                    Session("ResetMsg") = "Process is done"
                Else
                    Session("ResetMsg") = String.Format("Process is done with {0} error(s) encountered", intError.ToString)
                End If

                Response.Redirect(Request.Url.AbsolutePath)

                lblResult.Text = "Changes has been saved"
                lblResult.ForeColor = SharedFunction.SuccessColor

            Else
                lblResult.Text = "No selected item to save"
                lblResult.ForeColor = SharedFunction.ErrorColor
            End If

            GridDataBind()
        Catch ex As Exception
            lblResult.Text = "Failed to save changes"
            lblResult.ForeColor = SharedFunction.ErrorColor
        End Try
    End Sub

    Private Function GetNextActivity(ByVal ActivityID As Short) As Short
        Select Case CType(ActivityID, DataKeysEnum.ActivityID)
            Case DataKeysEnum.ActivityID.Done
                Return DataKeysEnum.ActivityID.IndigoDownload
            Case Else
                Return SharedFunction.GetNextActivity(ActivityID)
        End Select
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        lblResult.Text = ""

        SaveChanges()
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If cboPO.SelectedIndex > 0 Then
            GridDataBind()
        End If
    End Sub

End Class
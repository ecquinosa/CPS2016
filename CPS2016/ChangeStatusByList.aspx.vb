
Public Class ChangeStatusByList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtSource.Attributes.Add("OnTextChanged", "ReplaceString();")

        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.IndigoExtract) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If txtSource.Text = "" Then
            lblStatus.Text = "No data to process"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            Return
        End If

        lblStatus.ForeColor = Drawing.Color.Black

        Dim DAL As New DAL
        Dim intSuccess As Integer = 0
        Dim intFailed As Integer = 0
        Dim sb As New StringBuilder
        For Each strLine As String In txtSource.Text.Replace("<br>", "").Split(vbNewLine)
            If strLine.Contains("CardID") Then
                If strLine.Contains("is dropped") Then
                    Dim CardID As String = strLine.Trim.Split(" ")(1)
                    Dim CRN As String = strLine.Trim.Split(" ")(5)
                    If DAL.ExecuteScalar("SELECT ISNULL(POID,0) FROM dbo.tblRelPOData WHERE CardID = " & CardID) Then
                        If IsDBNull(DAL.ObjectResult) Then
                            sb.Append(String.Format("CardID {0}, CRN {1} - no record found", CardID, CRN) & vbNewLine)
                            intFailed += 1
                        ElseIf DAL.ObjectResult Is Nothing Then
                            sb.Append(String.Format("CardID {0}, CRN {1} - no record found", CardID, CRN) & vbNewLine)
                            intFailed += 1
                        Else
                            UpdateStatus(DAL, DAL.ObjectResult, CardID, CRN, sb, intSuccess, intFailed)
                        End If
                    Else
                        sb.Append(String.Format("CardID {0}, CRN {1} - failed. {2}", CardID, CRN, DAL.ErrorMessage) & vbNewLine)
                        intFailed += 1
                    End If
                End If
            ElseIf strLine.Trim.Contains(",") Then
                Dim sbQuery As New StringBuilder
                sbQuery.Append("Select dbo.tblPO.POID, dbo.tblRelPOData.CardID, dbo.tblRelPOData.CRN, dbo.tblRelPOData.Barcode ")
                sbQuery.Append("From dbo.tblPO INNER Join dbo.tblRelPOData ON dbo.tblPO.POID = dbo.tblRelPOData.POID ")
                sbQuery.Append("Where (cast(dbo.tblPO.Batch as int) = " & CInt(strLine.Trim.Split(",")(0)) & ") ")
                Try
                    If strLine.Trim.Split(",")(1).Contains("-") Then
                        sbQuery.Append("AND (dbo.tblRelPOData.CRN = '" & strLine.Trim.Split(",")(1).Trim & "') ")
                    Else
                        sbQuery.Append("AND (dbo.tblRelPOData.Barcode = '" & strLine.Trim.Split(",")(1).Trim & "') ")
                    End If

                    If DAL.SelectQuery(sbQuery.ToString) Then
                        For Each rw As DataRow In DAL.TableResult.Rows
                            UpdateStatus(DAL, rw("POID"), rw("CardID"), rw("CRN"), sb, intSuccess, intFailed)
                        Next
                    End If
                Catch ex As Exception
                    sb.Append(String.Format("{0} - Invalid line", strLine) & vbNewLine)
                    intFailed += 1
                End Try
            End If
        Next

        DAL.AddSystemLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

        DAL.Dispose()
        DAL = Nothing

        lblStatus.Text = String.Format("Success: {0}, Failed: {1}", intSuccess.ToString("N0"), intFailed.ToString("N0"))
        txtSource.Text = sb.ToString
        btnSubmit.Enabled = False
    End Sub

    Private Sub UpdateStatus(ByVal DAL As DAL, ByVal POID As Integer, ByVal CardID As Integer, ByVal CRN As String, ByRef sb As StringBuilder, ByRef intSuccess As Integer, ByRef intFailed As Integer)
        If DAL.UpdateCardStatusByCardID(CardID, DataKeysEnum.ActivityID.IndigoDownload) Then
            DAL.AddCardActivity(POID, CRN, "", String.Format("{0} changed the status of record to {1}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(DataKeysEnum.ActivityID.IndigoDownload)), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))
            sb.Append(String.Format("CardID {0}, CRN {1} - success", CardID, CRN) & vbNewLine)
            intSuccess += 1
        Else
            Dim strError As String = String.Format("{0} failed to change the status of record to {1}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(DataKeysEnum.ActivityID.IndigoDownload), DAL.ErrorMessage)
            DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            sb.Append(String.Format("CardID {0}, CRN {1} - failed. {2}", CardID, CRN, DAL.ErrorMessage) & vbNewLine)
            intFailed += 1
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    'Protected Sub txtSource_TextChanged(sender As Object, e As EventArgs) 'Handles txtSource.TextChanged
    '    txtSource.Text = txtSource.Text.Replace("<br>", "")
    'End Sub

End Class
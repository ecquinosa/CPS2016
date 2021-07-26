
Imports System.IO

Public Class RejectCard
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.RejectCard) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            If Not Session("ResetMsg") Is Nothing Then
                lblStatus.Text = Session("ResetMsg").ToString
                If Session("ResetMsg").ToString = "Process is done" Then
                    lblStatus.ForeColor = SharedFunction.SuccessColor
                Else
                    lblStatus.ForeColor = SharedFunction.ErrorColor
                End If

                Session.RemoveAll()
            End If

            CreateTable()
        End If
    End Sub

    Private Sub ResetPage()
        'Dim strSessions As String = "dtGrid,dtDV,IsAddToDV"
        'For Each strSession As String In strSessions.Split(",")
        '    Session.Remove(strSession)
        'Next

        CreateTable()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub DVDataBind()
        xDataView.DataBind()
    End Sub

    Private Sub CreateTable()
        Dim DAL As New DAL
        If DAL.SelectRelPODataByBarcodev2("12345678901234567890") Then
            Dim dt As DataTable = DAL.TableResult.Clone
            dt.Columns.Add("RejectType", Type.GetType("System.String"))
            dt.AcceptChanges()

            If Session("dtGrid") Is Nothing Then Session("dtGrid") = dt
            If Session("dtDV") Is Nothing Then Session("dtDV") = DAL.TableResult.Clone
        End If
        DAL.Dispose()
    End Sub

    Public Function GetFullName(ByVal strFirst As String, ByVal objMiddle As Object, ByVal strLast As String) As String
        Return String.Format("{0} {1}{2}", strFirst, IIf(IsDBNull(objMiddle), "", objMiddle.ToString & " "), strLast)
    End Function

    Public Function GetPhotoImage(ByVal strBarcode As String) As Byte()
        'Return File.ReadAllBytes(SharedFunction.GetFrom_PSBImages(String.Format("{0}_Photo.jpg", strBarcode)))
        Return SharedFunction.GetPhotoImage(strBarcode)
    End Function

    Private Sub SelectRelPODataByByCRNorBarcode()
        If Session("IsAddToDV") = 0 Then Return

        Dim DAL As New DAL
        If DAL.SelectRelPODataByByCRNorBarcode(txtParam.Text) Then
            If DAL.TableResult.DefaultView.Count > 0 Then
                If Not Session("dtGrid") Is Nothing Then
                    Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

                    Select Case CType(DAL.TableResult.Rows(0)("ActivityID"), DataKeysEnum.ActivityID)
                        Case DataKeysEnum.ActivityID.IndigoDownload, DataKeysEnum.ActivityID.Done
                            Session("IsAddToDV") = 0
                            lblStatus.Text = String.Format("{0} status is {1}", txtParam.Text, SharedFunction.GetActivityDesc(DAL.TableResult.Rows(0)("ActivityID")))
                            lblStatus.ForeColor = SharedFunction.ErrorColor

                            RefreshGridAndDV()
                        Case Else
                            Dim dtTemp As DataTable = DAL.TableResult
                            dtTemp.Columns.Add("RejectType", Type.GetType("System.String"))
                            dtTemp.AcceptChanges()

                            For Each rwTemp As DataRow In dtTemp.Rows
                                rwTemp("RejectType") = IIf(chkQuality.Checked, "Quality", "Chip")
                                rwTemp.AcceptChanges()
                            Next

                            AddRowToDT(dtTemp.Rows(0), dt)

                            lblTotal.Text = "Total Record(s): " & dt.DefaultView.Count.ToString

                            Session("dtGrid") = dt

                            'xGrid.DataSource = dt

                            'DVDataBind()
                            RefreshGridAndDV()
                    End Select
                End If
            Else
                Session("IsAddToDV") = 0

                lblStatus.Text = "No data found"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                RefreshGridAndDV()
            End If
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub AddRowToDT(ByVal rwSource As DataRow, ByRef dtGrid As DataTable)
        Dim rw As DataRow = dtGrid.NewRow
        For i = 0 To dtGrid.Columns.Count - 1
            rw(i) = rwSource(i)
        Next
        dtGrid.Rows.Add(rw)
    End Sub

    Private Function ValidateParam() As Boolean
        If txtParam.Text = "" Then
            lblStatus.Text = "Please enter barcode or crn"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            RefreshGridAndDV()

            Return False
        ElseIf Not chkQuality.Checked And Not chkChip.Checked Then
            lblStatus.Text = "Please select defect reason"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            RefreshGridAndDV()

            Return False
        ElseIf chkQuality.Checked And chkChip.Checked Then
            lblStatus.Text = "Please select one defect reason only"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            RefreshGridAndDV()

            Return False
        Else
            If Not CheckDuplicate(CType(Session("dtGrid"), DataTable)) Then
                lblStatus.Text = "Duplicate entry not allowed"
                lblStatus.ForeColor = SharedFunction.ErrorColor
                RefreshGridAndDV()

                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Function CheckDuplicate(ByVal dt As DataTable) As Boolean
        Dim Script As String = String.Format("CRN='{0}'", txtParam.Text.Trim)
        If txtParam.Text.Trim.Length = 20 Then Script = String.Format("Barcode='{0}'", txtParam.Text.Trim)

        If dt.Select(Script).Length > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub BindGrid()
        If CheckDuplicate(CType(Session("dtGrid"), DataTable)) Then SelectRelPODataByByCRNorBarcode()
        'If Not CheckDuplicate(CType(Session("dtGrid"), DataTable)) Then Return
        'If Not Session("ActivityIsIndigoDownloadOrDone") Is Nothing Then
        '    Session("ActivityIsIndigoDownloadOrDone") = Nothing
        '    Return
        'End If

        'SelectRelPODataByByCRNorBarcode()
    End Sub

    Private Sub RefreshGridAndDV()
        xGrid.DataSource = CType(Session("dtGrid"), DataTable)
        xDataView.DataSource = CType(Session("dtDV"), DataTable)
        xGrid.DataBind()
        xDataView.DataBind()
    End Sub

    Private Sub BindDV()
        If Not CheckDuplicate(CType(Session("dtDV"), DataTable)) Then Return
        'If Not CheckDuplicate(CType(Session("dtDV"), DataTable)) Then Return
        'If Not Session("ActivityIsIndigoDownloadOrDone") Is Nothing Then
        '    Session("ActivityIsIndigoDownloadOrDone") = Nothing
        '    Return
        'End If

        If Not Session("dtDV") Is Nothing Then
            Dim dt As DataTable = CType(Session("dtDV"), DataTable)

            If Session("IsAddToDV") = 1 Then
                Dim dtGrid As DataTable = CType(Session("dtGrid"), DataTable)

                If dtGrid.DefaultView.Count > 0 Then
                    Dim rw As DataRow = dt.NewRow

                    For i = 0 To dt.Columns.Count - 1
                        rw(i) = dtGrid.Rows(dtGrid.DefaultView.Count - 1)(i)
                    Next

                    dt.Rows.InsertAt(rw, 0)

                    If dt.DefaultView.Count = 5 Then dt.Rows(dt.DefaultView.Count - 1).Delete()

                    Session("dtDV") = dt
                End If
            End If

            xDataView.DataSource = dt

            If dt.DefaultView.Count = 0 Then
                xDataView.Visible = False
            Else
                xDataView.Visible = True
            End If
        End If
    End Sub

    Private Sub xDataView_DataBinding(sender As Object, e As System.EventArgs) Handles xDataView.DataBinding
        BindDV()
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Protected Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        lblStatus.ForeColor = Drawing.Color.Black
        lblStatus.Text = ""

        If ValidateParam() Then
            Session("IsAddToDV") = 1
            GridDataBind()

            txtParam.Text = ""
            txtParam.Focus()
        End If
    End Sub

    Protected Sub btnSubmitCards_Click(sender As Object, e As EventArgs) Handles btnSubmitCards.Click
        Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

        If dt.DefaultView.Count > 0 Then
            Dim intError As Integer = 0

            Dim sbLog As New StringBuilder
            sbLog.AppendLine("POID|CardID|Barcode|CRN|Result")

            Dim DAL As New DAL
            For Each rw As DataRow In dt.Rows
                If DAL.UpdateCardStatusByCardID(rw("CardID"), DataKeysEnum.ActivityID.IndigoDownload) Then
                    DAL.AddCardActivity(rw("POID"), rw("CRN"), rw("Barcode"), String.Format("{0} changed the status of record from {1} to {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(rw("ActivityID")), SharedFunction.GetActivityDesc(DataKeysEnum.ActivityID.IndigoDownload)), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))
                    DAL.AddCardReject(rw("POID"), rw("PurchaseOrder"), rw("Barcode"), rw("CRN"), SharedFunction.UserID(Page.User.Identity.Name), IIf(rw("RejectType").ToString.Trim = "Quality", 1, 2))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", rw("POID"), rw("CardID"), rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Success"))
                Else
                    intError += 1
                    Dim strError As String = String.Format("{0} failed to change the status of record from {1} to {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(rw("ActivityID")), SharedFunction.GetActivityDesc(DataKeysEnum.ActivityID.IndigoDownload), DAL.ErrorMessage)
                    DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", rw("POID"), rw("CardID"), rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Failed"))
                End If
            Next

            DAL.Dispose()
            DAL = Nothing

            'SharedFunction.SaveToTextfile(String.Format("{0}\RejectedCards_Qty{1}_{2}.txt", SharedFunction.QCRepository, dt.DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt")), sbLog.ToString, New StringBuilder)
            SharedFunction.SaveToTextfile(String.Format("{0}\RejectedCards_Qty{1}_{2}_{3}.txt", SharedFunction.ProcessLogRepository, dt.DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt"), SharedFunction.UserCompleteName(Page.User.Identity.Name)), sbLog.ToString, New StringBuilder)

            'ResetPage()

            If intError = 0 Then
                Session("ResetMsg") = "Process is done"
                'lblStatus.Text = "Process is done"
                'lblStatus.ForeColor = SharedFunction.SuccessColor
            Else
                Session("ResetMsg") = String.Format("Process is done with {0} error(s) encountered", intError.ToString)
                'lblStatus.Text = String.Format("Process is done with {0} error(s) encountered", intError.ToString)
                'lblStatus.ForeColor = SharedFunction.ErrorColor
            End If

            Response.Redirect(Request.Url.AbsolutePath)

            'RefreshGridAndDV()

            'lblTotal.Text = "Total Record(s): 0"

            'GridDataBind()
            'DVDataBind()
        Else
            lblStatus.Text = "No data to process"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            RefreshGridAndDV()
        End If
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim strIDs As String = GetSelectedItems_CardID()

        If strIDs <> "" Then
            Session("IsAddToDV") = 0

            Dim dt As DataTable = CType(Session("dtGrid"), DataTable)
            Dim dtDV As DataTable = CType(Session("dtDV"), DataTable)

            For Each rw As DataRow In dt.Select("CardID IN (" & strIDs & ")")
                rw.Delete()
            Next

            For Each rw2 As DataRow In dtDV.Select("CardID IN (" & strIDs & ")")
                rw2.Delete()
            Next

            dt.AcceptChanges()
            dtDV.AcceptChanges()

            Session("dtGrid") = dt
            Session("dtDV") = dtDV

            xGrid.DataSource = dt

            lblTotal.Text = "Total Record(s): " & dt.DefaultView.Count.ToString

            GridDataBind()
            DVDataBind()
        Else
            lblStatus.Text = "No data to delete"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            RefreshGridAndDV()
        End If
    End Sub

    Private Function GetSelectedItems_CardID() As String
        Dim sb As New StringBuilder
        For i As Short = 0 To xGrid.GetSelectedFieldValues("CardID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("CardID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("CardID")(i)))
            End If
        Next

        Return sb.ToString
    End Function

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Protected Sub chkQuality_CheckedChanged(sender As Object, e As EventArgs) Handles chkQuality.CheckedChanged
        chkChip.Checked = Not chkQuality.Checked
    End Sub

    Protected Sub chkChip_CheckedChanged(sender As Object, e As EventArgs) Handles chkChip.CheckedChanged
        chkQuality.Checked = Not chkChip.Checked
    End Sub

End Class
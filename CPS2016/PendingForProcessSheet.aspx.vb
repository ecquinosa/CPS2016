
Imports System.IO

Public Class PendingForProcessSheet
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.PendingForProcessSheet) Then
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

                txtFrontSheet.Text = ""
                txtFrontSheet.Focus()
            End If

            CreateTable()
        End If
    End Sub

    Private Sub ResetPage()
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
            If Session("dtGrid") Is Nothing Then Session("dtGrid") = DAL.TableResult.Clone
            If Session("dtDV") Is Nothing Then Session("dtDV") = DAL.TableResult.Clone
        End If
        DAL.Dispose()
    End Sub

    Public Function GetPhotoImage(ByVal strBarcode As String) As Byte()
        'Return File.ReadAllBytes(SharedFunction.GetFrom_PSBImages(String.Format("{0}_Photo.jpg", strBarcode)))
        Return SharedFunction.GetPhotoImage(strBarcode)
    End Function

    Public Function GetFullName(ByVal strFirst As String, ByVal objMiddle As Object, ByVal strLast As String) As String
        Return String.Format("{0} {1}{2}", strFirst, IIf(IsDBNull(objMiddle), "", objMiddle.ToString & " "), strLast)
    End Function

    Private Sub SelectRelPODataByCRNorBarcodeWithoutBinding(ByVal CRN As String)
        If Session("IsAddToDV") = 0 Then Return

        Dim dtSource As DataTable = Nothing

        Dim DAL As New DAL
        Try
            If DAL.SelectRelPODataByByCRNorBarcode(CRN) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    dtSource = DAL.TableResult
                Else
                    Session("IsAddToDV") = 0

                    lblStatus.Text = "No data found"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    RefreshGridAndDV()

                    Return
                End If
            End If
        Catch ex As Exception
            Session("IsAddToDV") = 0

            Dim _error As String = String.Format("SelectRelPODataByByCRNorBarcode(): User {0} Param value {1}. Runtime error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), CRN, ex.Message)
            DAL.AddErrorLog(_error, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

            lblStatus.Text = "Failed to retrieve record"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            RefreshGridAndDV()

            Return
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try

        If Not Session("dtGrid") Is Nothing Then
            Dim intCardID As Integer = dtSource.Rows(0)("CardID")
            Dim intPOID As Integer = dtSource.Rows(0)("POID")
            Dim intSeries As Integer = dtSource.Rows(0)("CurrentSeries")

            Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

            Dim intActivityID As DataKeysEnum.ActivityID = dtSource.Rows(0)("ActivityID")

            Select Case intActivityID
                Case DataKeysEnum.ActivityID.IndigoDownload, DataKeysEnum.ActivityID.Done
                    Session("IsAddToDV") = 0

                    lblStatus.Text = String.Format("{0} status is {1}", CRN, SharedFunction.GetActivityDesc(intActivityID))
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    RefreshGridAndDV()

                Case Else
                    If Not SharedFunction.IsHaveAccessToActivity(SharedFunction.UserActivity(Page.User.Identity.Name), intActivityID) Then
                        Session("IsAddToDV") = 0

                        lblStatus.Text = String.Format("You have no access to view/ process card with {0} status", SharedFunction.GetActivityDesc(intActivityID))
                        lblStatus.ForeColor = SharedFunction.ErrorColor

                        RefreshGridAndDV()
                    Else
                        'if grid has no record
                        If dt.DefaultView.Count = 0 Then
                            AddRowToDT(dtSource.Rows(0), dt)
                        Else
                            'check if current series is divisible by 21
                            Dim strQuotient As String = (intSeries / 21).ToString

                            'current series is not divisible by 21
                            If strQuotient.Contains(".") Then
                                AddRowToDT(dtSource.Rows(0), dt)
                            Else
                                'current series is not divisible by 21

                                Dim intPreviousSeries As Integer = dt.Rows(dt.DefaultView.Count - 1)("CurrentSeries")
                                Dim intDiff As Integer = intSeries - 21

                                If intDiff < 0 And intSeries <> 1 Then
                                    AddRowToDT(dtSource.Rows(0), dt)
                                Else
                                    strQuotient = (intDiff / 21).ToString

                                    If strQuotient.Contains(".") Then
                                        AddRowToDT(dtSource.Rows(0), dt)
                                    Else
                                        Dim DAL2 As New DAL
                                        Try
                                            If DAL2.SelectRelPODataByCardIDandLast20(intCardID, intPOID) Then
                                                For i As Short = 1 To dtSource.DefaultView.Count - 1
                                                    AddRowToDT(dtSource.Rows(i), dt)
                                                Next
                                            End If
                                        Catch ex As Exception
                                            Session("IsAddToDV") = 0

                                            Dim _error As String = String.Format("SelectRelPODataByCardIDandLast20(): User {0} CardID {1} POID {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), intCardID, intPOID, ex.Message)
                                            DAL2.AddErrorLog(_error, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                                            lblStatus.Text = "Failed to retrieve records"
                                            lblStatus.ForeColor = SharedFunction.ErrorColor

                                            RefreshGridAndDV()

                                            Return
                                        Finally
                                            DAL2.Dispose()
                                            DAL2 = Nothing
                                        End Try
                                    End If
                                End If
                            End If
                        End If

                        lblTotal.Text = "Total Record(s): " & dt.DefaultView.Count.ToString

                        Session("dtGrid") = dt

                        xDataView.DataBind()
                    End If
            End Select
        End If
    End Sub

    Private Sub SelectRelPODataByByCRNorBarcodeWithBinding(ByVal CRN As String)
        If Session("IsAddToDV") = 0 Then Return

        Dim dtSource As DataTable = Nothing

        Dim DAL As New DAL
        Try
            If DAL.SelectRelPODataByByCRNorBarcode(CRN) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    dtSource = DAL.TableResult
                Else
                    Session("IsAddToDV") = 0

                    lblStatus.Text = "No data found"
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    RefreshGridAndDV()
                    Return
                End If
            End If
        Catch ex As Exception
            Session("IsAddToDV") = 0

            Dim _error As String = String.Format("SelectRelPODataByByCRNorBarcode(): User {0} Param value {1}. Runtime error {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), CRN, ex.Message)
            DAL.AddErrorLog(_error, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

            lblStatus.Text = "Failed to retrieve record"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            RefreshGridAndDV()

            Return
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try

        If Not Session("dtGrid") Is Nothing Then
            Dim intCardID As Integer = dtSource.Rows(0)("CardID")
            Dim intPOID As Integer = dtSource.Rows(0)("POID")
            Dim intSeries As Integer = dtSource.Rows(0)("CurrentSeries")

            Dim dt As DataTable = CType(Session("dtGrid"), DataTable)

            Dim intActivityID As DataKeysEnum.ActivityID = dtSource.Rows(0)("ActivityID")

            Select Case intActivityID
                Case DataKeysEnum.ActivityID.IndigoDownload, DataKeysEnum.ActivityID.Done
                    Session("IsAddToDV") = 0

                    lblStatus.Text = String.Format("{0} status is {1}", CRN, SharedFunction.GetActivityDesc(intActivityID))
                    lblStatus.ForeColor = SharedFunction.ErrorColor

                    RefreshGridAndDV()

                Case Else
                    If Not SharedFunction.IsHaveAccessToActivity(SharedFunction.UserActivity(Page.User.Identity.Name), intActivityID) Then
                        Session("IsAddToDV") = 0

                        lblStatus.Text = String.Format("You have no access to view/ process card with {0} status", SharedFunction.GetActivityDesc(intActivityID))
                        lblStatus.ForeColor = SharedFunction.ErrorColor

                        RefreshGridAndDV()
                    Else
                        'if grid has no record
                        If dt.DefaultView.Count = 0 Then
                            AddRowToDT(dtSource.Rows(0), dt)
                        Else
                            'check if current series is divisible by 21
                            Dim strQuotient As String = (intSeries / 21).ToString

                            'current series is not divisible by 21
                            If strQuotient.Contains(".") Then
                                AddRowToDT(dtSource.Rows(0), dt)
                            Else
                                'current series is not divisible by 21

                                Dim intPreviousSeries As Integer = dt.Rows(dt.DefaultView.Count - 1)("CurrentSeries")
                                Dim intDiff As Integer = intSeries - 21

                                If intDiff < 0 And intSeries <> 1 Then
                                    AddRowToDT(dtSource.Rows(0), dt)
                                Else
                                    strQuotient = (intDiff / 21).ToString

                                    If strQuotient.Contains(".") Then
                                        AddRowToDT(dtSource.Rows(0), dt)
                                    Else
                                        Dim DAL2 As New DAL
                                        Try
                                            If DAL2.SelectRelPODataByCardIDandLast20(intCardID, intPOID) Then
                                                For i As Short = 1 To DAL2.TableResult.DefaultView.Count - 1
                                                    AddRowToDT(DAL2.TableResult.Rows(i), dt)
                                                Next
                                            End If
                                        Catch ex As Exception
                                            Session("IsAddToDV") = 0

                                            Dim _error As String = String.Format("SelectRelPODataByCardIDandLast20(): User {0} CardID {1} POID {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), intCardID, intPOID, ex.Message)
                                            DAL2.AddErrorLog(_error, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                                            lblStatus.Text = "Failed to retrieve records"
                                            lblStatus.ForeColor = SharedFunction.ErrorColor

                                            RefreshGridAndDV()

                                            Return
                                        Finally
                                            DAL2.Dispose()
                                            DAL2 = Nothing
                                        End Try
                                    End If
                                End If
                            End If
                        End If

                        lblTotal.Text = "Total Record(s): " & dt.DefaultView.Count.ToString

                        Session("dtGrid") = dt

                        RefreshGridAndDV()
                    End If
            End Select
        End If
    End Sub

    Private Sub AddRowToDT(ByVal rwSource As DataRow, ByRef dtGrid As DataTable)
        Dim rw As DataRow = dtGrid.NewRow
        For i = 0 To dtGrid.Columns.Count - 1
            rw(i) = rwSource(i)
        Next
        dtGrid.Rows.Add(rw)
    End Sub

    Private Function ValidateParamInitial() As Boolean
        If txtFrontSheet.Text = "" Then
            lblStatus.Text = "Please enter/ scan front sheet"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            RefreshGridAndDV()

            Return False
        ElseIf txtBackSheet.Text = "" Then
            lblStatus.Text = "Please enter/ scan back sheet"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            RefreshGridAndDV()

            Return False
        Else
            Return True
        End If
    End Function

    Private Function GetCRN1andCRN2() As String
        Dim CRNs As String = ""
        Dim DAL As New DAL
        If DAL.SelectQuery(String.Format("SELECT CRN1, CRN2 FROM tblRelPODataSheetBarcode WHERE FrontSheetBarcode='{0}' AND BackSheetBarcode='{1}'", txtFrontSheet.Text, txtBackSheet.Text)) Then
            If DAL.TableResult.DefaultView.Count > 0 Then
                CRNs = DAL.TableResult.Rows(0)("CRN1").ToString.Trim & "," & DAL.TableResult.Rows(0)("CRN2").ToString.Trim
            End If
        End If
        DAL.Dispose()
        DAL = Nothing

        Return CRNs
    End Function

    Private Function ValidateParam() As Boolean
        If txtFrontSheet.Text <> txtBackSheet.Text Then
            Label1.Text = "NOT MATCHED"
            Label1.ForeColor = SharedFunction.ErrorColor

            txtBackSheet.Focus()

            RefreshGridAndDV()

            Return False
        Else
            Label1.Text = "MATCHED"
            Label1.ForeColor = SharedFunction.SuccessColor

            txtFrontSheet.Focus()

            Dim CRN1 As String = SharedFunction.CRNFormat(txtFrontSheet.Text.Substring(0, 12))
            Session("CRN1") = CRN1

            If Not CheckDuplicate(CType(Session("dtGrid"), DataTable), CRN1) Then
                lblStatus.Text = "Duplicate entry not allowed"
                lblStatus.ForeColor = SharedFunction.ErrorColor
                RefreshGridAndDV()

                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Function ValidateParamv2(ByRef CRNs As String) As Boolean
        If txtFrontSheet.Text = txtBackSheet.Text Then
            Label1.Text = "IDENTICAL"
            Label1.ForeColor = SharedFunction.ErrorColor

            txtBackSheet.Focus()

            RefreshGridAndDV()

            Return False
        Else
            CRNs = GetCRN1andCRN2()
            If CRNs = "" Then
                Label1.Text = "INVALID"
                Label1.ForeColor = SharedFunction.ErrorColor

                txtBackSheet.Focus()

                RefreshGridAndDV()

                Return False
            Else
                Label1.Text = "VALID"
                Label1.ForeColor = SharedFunction.SuccessColor

                txtFrontSheet.Focus()

                Dim CRN1 As String = CRNs.Split(",")(0) 'SharedFunction.CRNFormat(txtFrontSheet.Text.Substring(0, 12))
                Session("CRN1") = CRN1

                If Not CheckDuplicate(CType(Session("dtGrid"), DataTable), CRN1) Then
                    lblStatus.Text = "Duplicate entry not allowed"
                    lblStatus.ForeColor = SharedFunction.ErrorColor
                    RefreshGridAndDV()

                    Return False
                Else
                    Return True
                End If
            End If
        End If
    End Function

    Private Sub BindGrid()
        'SelectRelPODataByByCRNorBarcode()
        If Not Session("CRN2") Is Nothing Then
            If CheckDuplicate(CType(Session("dtGrid"), DataTable), Session("CRN2").ToString) Then _
            SelectRelPODataByByCRNorBarcodeWithBinding(Session("CRN2").ToString)
        End If
    End Sub

    Private Function CheckDuplicate(ByVal dt As DataTable, ByVal CRN As String) As Boolean
        'Dim CRN As String = Session("CRN1").ToString
        Dim Script As String = String.Format("CRN='{0}'", CRN.Trim)
        If CRN.Trim.Length = 20 Then Script = String.Format("Barcode='{0}'", CRN.Trim)

        If dt.Select(Script).Length > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub RefreshGridAndDV()
        xGrid.DataSource = CType(Session("dtGrid"), DataTable)
        xDataView.DataSource = CType(Session("dtDV"), DataTable)
        xGrid.DataBind()
        xDataView.DataBind()
    End Sub

    Private Sub BindDV()
        If Session("CRN2") Is Nothing Then Return
        If Not CheckDuplicate(CType(Session("dtDV"), DataTable), Session("CRN2").ToString) Then Return

        If Not Session("dtDV") Is Nothing Then
            Dim dt As DataTable = CType(Session("dtDV"), DataTable)

            If Session("IsAddToDV") = 1 Then
                Dim dtGrid As DataTable = CType(Session("dtGrid"), DataTable)

                If dtGrid.DefaultView.Count > 0 Then
                    Dim rw As DataRow = dt.NewRow

                    For i = 0 To dt.Columns.Count - 1
                        'If i <> 12 Then rw(i) = dtGrid.Rows(dtGrid.DefaultView.Count - 1)(i)
                        rw(i) = dtGrid.Rows(dtGrid.DefaultView.Count - 1)(i)
                    Next

                    dt.Rows.InsertAt(rw, 0)

                    If dt.DefaultView.Count = 5 Then dt.Rows(dt.DefaultView.Count - 1).Delete()

                    Session("dtDV") = dt
                End If
            End If

            xDataView.DataSource = CType(Session("dtDV"), DataTable)

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
        Label1.Text = ""

        If ValidateParamInitial() Then
            'If ValidateParam() Then
            Dim CRNs As String = ""
            If ValidateParamv2(CRNs) Then
                Dim CRN1 As String = CRNs.Split(",")(0) 'SharedFunction.CRNFormat(txtFrontSheet.Text.Substring(0, 12))
                Dim CRN2 As String = CRNs.Split(",")(1) 'SharedFunction.CRNFormat(txtFrontSheet.Text.Substring(12, 12))
                Session("CRN1") = CRN1
                Session("CRN2") = CRN2

                Session("IsAddToDV") = 1
                SelectRelPODataByCRNorBarcodeWithoutBinding(CRN1)
                GridDataBind()

                txtBackSheet.Text = ""
                txtFrontSheet.Text = ""
                txtFrontSheet.Focus()
            End If
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
                If DAL.UpdateCardStatusByCardID(rw("CardID"), SharedFunction.GetNextActivity(rw("ActivityID"))) Then
                    DAL.AddCardActivity(rw("POID"), rw("CRN"), rw("Barcode"), String.Format("{0} changed the status of record from {1} to {2}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(rw("ActivityID")), SharedFunction.GetActivityDesc(SharedFunction.GetNextActivity(rw("ActivityID")))), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", rw("POID"), rw("CardID"), rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Success"))
                Else
                    intError += 1
                    Dim strError As String = String.Format("{0} failed to change the status of record from {1} to {2}. Runtime error {3}", SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetActivityDesc(rw("ActivityID")), SharedFunction.GetActivityDesc(SharedFunction.GetNextActivity(rw("ActivityID"))), DAL.ErrorMessage)
                    DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                    sbLog.AppendLine(String.Format("{0}|{1}|{2}|{3}|{4}", rw("POID"), rw("CardID"), rw("Barcode").ToString.Trim, rw("CRN").ToString.Trim, "Failed"))
                End If
            Next

            DAL.Dispose()
            DAL = Nothing

            SharedFunction.SaveToTextfile(String.Format("{0}\ProcessCards_Qty{1}_{2}_{3}.txt", SharedFunction.ProcessLogRepository, dt.DefaultView.Count.ToString, Now.ToString("MMddyy_hhmmtt"), SharedFunction.UserCompleteName(Page.User.Identity.Name)), sbLog.ToString, New StringBuilder)

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
        Else
            Session("IsAddToDV") = 0

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

End Class
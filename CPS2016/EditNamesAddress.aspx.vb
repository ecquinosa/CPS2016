
Public Class EditNamesAddress
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.MiscReports) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then


        End If
    End Sub


    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        lblStatus.Text = ""

        Dim sbQuery As String = ""

        If txtCSNCRN.Text = "" Then
            lblStatus.Text = "Please enter barcode or crn"

InvalidCondition:
            lblStatus.ForeColor = SharedFunction.ErrorColor
            txtCSNCRN.Focus()
            Return
        Else
            If txtCSNCRN.Text.Contains("-") Then
                If txtCSNCRN.Text.Length <> 14 Then
                    lblStatus.Text = "Please re-check CRN"
                    GoTo InvalidCondition
                Else
                    sbQuery = "WHERE (dbo.tblRelPOData.CRN = '" & txtCSNCRN.Text & "')"
                End If
            Else
                If txtCSNCRN.Text.Length <> 20 Then
                    lblStatus.Text = "Please re-check Barcode"
                    GoTo InvalidCondition
                Else
                    sbQuery = "WHERE (dbo.tblRelPOData.Barcode = '" & txtCSNCRN.Text & "')"
                End If
            End If
        End If

        Dim DAL As New DAL
        If DAL.SelectDataForFNameLNameAddressModificationByCSNCRN(sbQuery) Then
            If DAL.TableResult.DefaultView.Count > 0 Then
                Dim rw As DataRow = DAL.TableResult.Rows(0)
                Session("CardID") = rw("CardID")
                txtPO.Text = rw("PurchaseOrder").ToString.Trim
                txtBackOCR.Text = rw("BackOCR").ToString.Trim
                txtFName.Text = rw("FName").ToString.Trim
                txtLName.Text = rw("LName").ToString.Trim
                txtSuffix.Text = rw("Suffix").ToString.Trim
                txtAddress.Text = rw("Address").ToString.Trim
                txtCSNCRN.Text = ""
                btnSave.Visible = True
            Else
                txtPO.Text = ""
                txtBackOCR.Text = ""
                txtFName.Text = ""
                txtLName.Text = ""
                txtSuffix.Text = ""
                txtAddress.Text = ""
            End If
        End If
        DAL.Dispose()
        DAL = Nothing

        CheckValues()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Session("CardID") Is Nothing Then
            lblStatus2.Text = "ID session expired. Close window and retry process."
            lblStatus2.ForeColor = SharedFunction.ErrorColor
            Return
        ElseIf txtFName.Text = "" Then
            lblStatus2.Text = "Please enter first name"
            lblStatus2.ForeColor = SharedFunction.ErrorColor
            Return
        ElseIf txtlName.Text = "" Then
            lblStatus2.Text = "Please enter first name"
            lblStatus2.ForeColor = SharedFunction.ErrorColor
            Return
        ElseIf txtAddress.Text = "" Then
            lblStatus2.Text = "Please enter first name"
            lblStatus2.ForeColor = SharedFunction.ErrorColor
            Return
        End If

        Dim DAL As New DAL
        If Not DAL.UpdateFNameLNameAddressByCardID(Session("CardID"), txtFName.Text, txtLName.Text, txtSuffix.Text, txtAddress.Text) Then
            lblStatus2.Text = "Failed to save changes. Error " & DAL.ErrorMessage
            lblStatus2.ForeColor = SharedFunction.ErrorColor
            Return
        Else
            lblStatus2.Text = "Changes has been saved"
            lblStatus2.ForeColor = SharedFunction.SuccessColor
            DAL.AddSystemLog(String.Format("{0} changed FName {1} LName {2} Suffix {3} Address {4}", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtFName.Text, txtLName.Text, txtSuffix.Text, txtAddress.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            btnSave.Visible = False
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub CheckValues()
        'Return
        Dim IsErrorFlag As Boolean

        Dim _FirstName() As String = PurchaseOrder.FormatDataWithCharLength(txtFName.Text, My.Settings.FName_Limit_Laser, IsErrorFlag).Split(vbNewLine)

        'If Not IsErrorFlag Then
        '    Dim FirstName1 As String = ""
        '    Dim FirstName2 As String = ""

        '    For i As Short = 0 To _FirstName.Length - 1
        '        Select Case Trim(_FirstName(i))
        '            Case "", " ", vbNewLine
        '            Case Else
        '                Select Case i
        '                    Case 0
        '                        FirstName1 = _FirstName(i)
        '                    Case 1
        '                        FirstName2 = _FirstName(i)
        '                End Select
        '        End Select
        '    Next
        'Else
        txtFName.ReadOnly = Not IsErrorFlag
        'End If

        Dim LName As String = txtLName.Text
        If txtSuffix.Text <> "" Then LName = LName & " " & txtSuffix.Text

        Dim _LastName() As String = PurchaseOrder.FormatDataWithCharLength(LName, My.Settings.LName_Limit_Laser, IsErrorFlag).Split(vbNewLine)

        'If Not IsErrorFlag Then
        '    Dim LastName1 As String = ""
        '    Dim LastName2 As String = ""

        '    For i As Short = 0 To _LastName.Length - 1
        '        Select Case Trim(_LastName(i))
        '            Case "", " ", vbNewLine
        '            Case Else
        '                Select Case i
        '                    Case 0
        '                        LastName1 = _LastName(i)
        '                    Case 1
        '                        LastName2 = _LastName(i)
        '                End Select
        '        End Select
        '    Next
        'Else
        txtLName.ReadOnly = Not IsErrorFlag
        txtSuffix.ReadOnly = Not IsErrorFlag
        'End If

        Dim _Address() As String = PurchaseOrder.FormatDataWithCharLength(txtAddress.Text, My.Settings.Address_Limit_Laser, IsErrorFlag).Split(vbNewLine)

        'If Not IsErrorFlag Then
        '    Dim Address1 As String = ""
        '    Dim Address2 As String = ""
        '    Dim Address3 As String = ""
        '    Dim Address4 As String = ""
        '    Dim Address5 As String = ""

        '    For i As Short = 0 To _Address.Length - 1
        '        Select Case Trim(_Address(i))
        '            Case "", " ", vbNewLine
        '            Case Else
        '                Select Case i
        '                    Case 0
        '                        Address1 = _Address(i)
        '                    Case 1
        '                        Address2 = _Address(i)
        '                    Case 2
        '                        Address3 = _Address(i)
        '                    Case 3
        '                        Address4 = _Address(i)
        '                    Case 4
        '                        Address5 = _Address(i)
        '                End Select
        '        End Select
        '    Next
        'Else
        txtAddress.ReadOnly = Not IsErrorFlag
        'End If

        If txtFName.ReadOnly And txtLName.ReadOnly And txtAddress.ReadOnly Then
            btnSave.Enabled = False
        Else
            btnSave.Enabled = True
        End If
    End Sub

End Class
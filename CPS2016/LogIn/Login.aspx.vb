
Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("ErrMsg") Is Nothing Then _
                Label1.Text = Request.QueryString("ErrMsg").ToString

            'Dim d As String = "12/01/2015"
            'txtUsername.Text = CDate(d).ToString("MMMM dd, yyyy")

            'Dim strPurchaseOrder As String = IO.File.ReadAllText(SharedFunction.ForUploadingRepository & "\ManualUpload.txt") '"01-20151124-P-032-9666-C-001-9415"
            'Dim strBatch As String = strPurchaseOrder.Split("-")(3)

            'Console.Write("TEST")

            If Not Session("guest_pass") Is Nothing Then txtPassword.Text = Session("guest_pass").ToString
        End If
    End Sub

    Protected Sub loginSubmit_Click(sender As Object, e As EventArgs) Handles loginSubmit.Click
        'SharedFunction.GenerateCertificateOfDeletion_PurchaseOrder(xGrid.GetRowValues(Index, "POID"), xGrid.GetRowValues(Index, "PurchaseOrder"), intPOReportID, Request.Url.AbsolutePath, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name))
        'TempoProcess()
        'Dim b As String = "0120180921C1ID012037"
        'Console.WriteLine(b.Substring(11, 2))
        'Return

        Try
            If Not Session("guest_pass") Is Nothing Then txtPassword.Text = Session("guest_pass").ToString

            If txtUsername.Text = "" And txtPassword.Text = "" Then
                txtUsername.IsValid = False
                txtPassword.IsValid = False
                Label1.Text = "Please enter valid username and password"
            ElseIf txtUsername.Text <> "" And txtPassword.Text = "" Then
                txtPassword.IsValid = False
                Label1.Text = "Please enter valid password"
            ElseIf txtUsername.Text = "" And txtPassword.Text <> "" Then
                txtUsername.IsValid = False
                Label1.Text = "Please enter valid username"
            Else

                Dim DAL As New DAL

                If Not DAL.IsConnectionOK() Then
                    Label1.Text = "Unable to connect to database" & vbNewLine & vbNewLine & ". Please try again or contact Administrator."
                Else
                    If Label1.Text = "You are still logged in" And chkEndSession.Checked Then
                        DAL.EndUserSession(txtUsername.Text)
                    End If

                    If DAL.ValidateLogIN(txtUsername.Text, txtPassword.Text) Then
                        Dim strResult As String = DAL.ObjectResult

                        If strResult = "1|You are still logged in" And Not chkEndSession.Checked Then
                            Session("guest_pass") = txtPassword.Text
                            Label1.Text = strResult.Split("|")(1)
                            chkEndSession.Visible = True
                        ElseIf strResult.Split("|")(0) <> 0 And Not chkEndSession.Visible Then
                            Label1.Text = strResult.Split("|")(1)
                        Else
                            Dim UserID As String = strResult.Split("|")(1)
                            Dim strUserCompleteName As String = strResult.Split("|")(2)

                            Dim dtmDatePosted As Date = Now

                            DAL.AddLoggedUsers(UserID, dtmDatePosted)
                            Dim intLogID As Integer = DAL.ObjectResult

                            Dim IsHaveSysAdminAccnt As Boolean = False

                            Dim sbRoleID As New StringBuilder
                            Dim sbRoleDesc As New StringBuilder
                            sbRoleID.Append("")
                            If DAL.SelectUserRoleByUserID(UserID) Then
                                For Each rw As DataRow In DAL.TableResult.Rows
                                    If rw("RoleID") = DataKeysEnum.RoleID.SystemAdmin Then IsHaveSysAdminAccnt = True

                                    If sbRoleID.ToString = "" Then
                                        sbRoleID.Append(rw("RoleID").ToString.Trim)
                                        sbRoleDesc.Append(rw("RoleDesc").ToString.Trim)
                                    Else
                                        sbRoleID.Append("," & rw("RoleID").ToString.Trim)
                                        sbRoleDesc.Append("," & rw("RoleDesc").ToString.Trim)
                                    End If
                                Next
                            End If

                            If Not IsHaveSysAdminAccnt Then
                                If DAL.ExecuteScalar("SELECT SystemStatus FROM tblSystemParameter") Then
                                    Try
                                        If Not CBool(DAL.ObjectResult) Then
                                            Label1.Text = "System is offline"
                                            Exit Sub
                                        End If
                                    Catch ex As Exception
                                    End Try
                                End If
                            End If

                            Dim sbRoleModules As New StringBuilder
                            sbRoleModules.Append("")
                            If DAL.SelectUserModules(UserID) Then
                                For Each rw As DataRow In DAL.TableResult.Rows
                                    If sbRoleModules.ToString = "" Then
                                        sbRoleModules.AppendLine(rw("ModuleID").ToString.Trim)
                                    Else
                                        sbRoleModules.AppendLine("," & rw("ModuleID").ToString.Trim)
                                    End If
                                Next
                            End If

                            Dim sbActivityID As New StringBuilder
                            If DAL.SelectUserActivity(UserID) Then
                                For Each rw As DataRow In DAL.TableResult.Rows
                                    If sbActivityID.ToString = "" Then
                                        sbActivityID.Append(rw("ActivityID"))
                                    Else
                                        sbActivityID.Append("," & rw("ActivityID"))
                                    End If
                                Next
                            End If

                            DAL.AddSystemLog(String.Format("{0} has logged in ", strUserCompleteName), "Login", UserID)

                            DAL.Dispose()
                            DAL = Nothing

                            Session("guest_pass") = Nothing

                            Session("CheckPass") = txtPassword.Text

                            Dim strUserCredentials As String = UserID & ":" & sbRoleID.ToString & ":" & sbRoleDesc.ToString & ":" & strUserCompleteName & ":" & intLogID & ":" & dtmDatePosted.ToString.Replace(":", "_") & ":" & sbRoleModules.ToString & ":" & sbActivityID.ToString

                            System.Web.Security.FormsAuthentication.SetAuthCookie(SharedFunction.EncryptData(strUserCredentials), False)
                            'System.Web.Security.FormsAuthentication.SetAuthCookie(strUserCredentials, False)

                            Response.Redirect("/Home_CPS.aspx", False)
                        End If
                    Else
                        DAL.AddErrorLog(DAL.ErrorMessage, "Login", 0)
                        Label1.Text = "An error occurred in login validation. Please try again or contact Administrator."
                    End If
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            'LogError(ex.Message & IIf(Textbox1.Text = "", "", " : User " & Textbox1.Text))
            ''lblMessage.Text = "An error occurred. Please contact Administrator."
            'Textbox1.ErrorText = "A runtime error occurred. Please try again or contact System Administrator."
            'Textbox2.ErrorText = ""
            'Textbox1.IsValid = False
            'Textbox2.IsValid = False
        End Try
    End Sub

    Private Sub TempoProcess()
        'If txtUsername.Text = "" Then Return
        'Dim bg As New BarcodeWeb
        'Dim bln As Boolean
        'Dim strLines() As String = IO.File.ReadAllText("D:\gsis_barcode.txt").Split(vbNewLine)
        'For Each strLine As String In strLines
        '    If strLine.Trim <> "" Then
        '        Dim barcode As String = strLine.Trim & txtUsername.Text
        '        bln = bg.GenerateBarcode2(barcode, String.Format("{0}\{1}", "C:\Allcard\UBPUMIDEMV_CPS\Perso\B1_MUHLBAUER", barcode & ".jpg"), txtUsername.Text)
        '    End If
        'Next
        'bg = Nothing
        'Return

        ''Dim strLines() As String = IO.File.ReadAllLines("D:\EDEL\ACC\SSS\SSS 2016\1stBatch_1492pcs.txt")
        'Dim strLines() As String = IO.File.ReadAllLines("D:\EDEL\ACC\SSS\SSS 2016\2ndBatch_506pcs.txt")
        ''Dim strLines() As String = IO.File.ReadAllLines("D:\EDEL\ACC\SSS\SSS 2016\3rdBatch_19pcs.txt")

        ''Dim strLinesDups() As String = IO.File.ReadAllLines("D:\EDEL\ACC\SSS\SSS 2016\duplicate.txt")

        ''Dim sb As New StringBuilder
        ''Dim strSource As String = "D:\EDEL\ACC\SSS\SSS 2016\COD\1st batch - 1492"
        ' ''Dim strDesti As String = "D:\EDEL\ACC\SSS\SSS 2016\COD\duplicate"


        ''Dim intCntr As Integer = 1
        ''For Each strLine As String In strLines
        ''    If strLine.Trim <> "" Then
        ''        Dim strFileSource As String = String.Format("{0}\{1}.pdf", strSource, strLine.Split("|")(0).Trim)
        ''        If IO.File.Exists(strFileSource) Then
        ''            sb.AppendLine(strLine.Split("|")(0).Trim & " - exist")
        ''        Else
        ''            Dim IsDuplicate As Boolean
        ''            For Each strLineDup As String In strLinesDups
        ''                If strLineDup.Trim <> "" Then
        ''                    If strLine.Split("|")(0).Trim = strLineDup.Trim Then
        ''                        sb.AppendLine(strLine.Split("|")(0).Trim & " - not exist (duplicate)")
        ''                        IsDuplicate = True
        ''                    End If
        ''                End If
        ''            Next

        ''            If Not IsDuplicate Then
        ''                sb.AppendLine(strLine.Split("|")(0).Trim & " - not exist")
        ''            End If
        ''        End If


        ''        'If IO.File.Exists(strFileSource) Then
        ''        '    Dim strFileDesti As String = String.Format("{0}\{1}.pdf", strDesti, strLine.Trim)
        ''        '    If Not IO.File.Exists(strFileDesti) Then
        ''        '        IO.File.Move(strFileSource, strFileDesti)
        ''        '    Else
        ''        '        IO.File.Move(strFileSource, strFileDesti.Replace(".pdf", "_" & Now.ToString("hhmmss") & ".pdf"))
        ''        '    End If
        ''        'End If
        ''    End If
        ''Next

        ''IO.File.WriteAllText("D:\EDEL\ACC\SSS\SSS 2016\result.txt", sb.ToString)

        ''Dim strSource As String = "D:\EDEL\ACC\SSS\SSS 2016\COD\1st batch - 1492"
        ''Dim strDesti As String = "D:\EDEL\ACC\SSS\SSS 2016\COD\duplicate"
        Dim strLines() As String = IO.File.ReadAllLines("D:\SSS\list.txt")
        Dim intCntr As Integer = 1
        For Each strLine As String In strLines
            If strLine.Trim <> "" Then
                SharedFunction.GenerateCertificateOfDeletion_Manual(strLine)
                IO.File.WriteAllText("D:\SSS\status.txt", intCntr.ToString)
                intCntr += 1
            End If
        Next

        Return

        'Dim sb As New StringBuilder
        'For Each strFile As String In System.IO.Directory.GetFiles("D:\SSS\COD")
        '    sb.AppendLine(System.IO.Path.GetFileNameWithoutExtension(strFile))
        'Next

        'Console.WriteLine(sb.ToString)

        'Dim strLine As String = ""
        'Dim MemberXML As New MemberXML("D:\0120161111CCID092005\0120161111CCID092005.xml")
        'If Not MemberXML.ExtractDataForMuhlbauer(strLine) Then
        '    'File.WriteAllText(strFile.Replace(".xml", ".txt"), strLine)
        '    'Else
        '    'intError += 1
        '    Console.Write("test")
        '    'sb.AppendLine(String.Format("MemberXML.ExtractDataForMuhlbauer(): Unable to extract data of Xml field of Barcode {0}", rw("Barcode")))
        'End If
        'MemberXML = Nothing

        'txtUsername.Text = "Done!"

        'Return
    End Sub

End Class
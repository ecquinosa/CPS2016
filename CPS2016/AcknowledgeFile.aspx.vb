
Public Class AcknowledgeFile
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
            PopulateCDFRs()

            If Session("ReportType") = DataKeysEnum.Report.AcknowledgeFile Then
                lblHeader.Text = "Generate Acknowledge File"
            Else
                lblHeader.Text = "Generate Response File"
            End If
        End If
    End Sub

    Private Sub PopulateCDFRs()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT GSUFileName FROM tblCDFR") Then
            cboCDFR.DataValueField = "GSUFileName"
            cboCDFR.DataTextField = "GSUFileName"
            cboCDFR.DataSource = DAL.TableResult
            cboCDFR.DataBind()
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        lblStatus.Text = ""

        Dim DAL As New DAL
        If Session("ReportType") = DataKeysEnum.Report.AcknowledgeFile Then
            If DAL.SelectCDFR(" WHERE dbo.tblCDFR.GSUfilename = '" & cboCDFR.Text.Trim & "'") Then
                Dim sbData As New StringBuilder
                Dim gsuFilename As String = cboCDFR.Text
                Dim gsuTotal As Integer = DAL.TableResult.DefaultView.Count
                For Each rw As DataRow In DAL.TableResult.Rows
                    Dim sbLine As New StringBuilder

                    'sbLine.Append(rw("Barcode").ToString.Trim & "|" & rw("SSSNo").ToString.Trim & vbNewLine)

                    'For Each col As DataColumn In DAL.TableResult.Columns
                    '    Select Case col.ColumnName
                    '        Case "RelCDFRID", "CDFRID", "OrigData"
                    '        Case Else
                    '            If sbLine.ToString = "" Then
                    '                sbLine.Append(rw(col).ToString.Trim)
                    '            Else
                    '                sbLine.Append("|" & rw(col).ToString.Trim)
                    '            End If
                    '    End Select
                    'Next

                    sbData.Append(rw("Barcode").ToString.Trim & "|" & rw("SSSNo").ToString.Trim & vbNewLine)
                Next

                Dim sbDataFinal As New StringBuilder
                sbDataFinal.Append(gsuFilename & "|" & gsuTotal & vbNewLine)
                sbDataFinal.Append(sbData.ToString)

                Dim destiFolder As String = SharedFunction.PersoRepository & "\" & gsuFilename & "_ACKN"
                If Not System.IO.Directory.Exists(destiFolder) Then System.IO.Directory.CreateDirectory(destiFolder)

                'ACKNyyyymmddhhss.txt
                Dim tempCDFR As String = String.Format("{0}\ACKN{1}.txt", destiFolder, Now.ToString("yyyyMMddhhss"))

                Try
                    SharedFunction.ExportToExcel(DAL.TableResult, tempCDFR.Replace(".txt", ".xls"), "data")
                Catch ex As Exception
                    'lblStatus.Text = "Failed to export to e. Error " & ex.Message
                    'lblStatus.ForeColor = SharedFunction.ErrorColor
                End Try

                If sbData.ToString <> "" Then
                    System.IO.File.WriteAllText(tempCDFR, sbDataFinal.ToString)
                    SharedFunction.ViewDownloadFile(tempCDFR)
                    lblStatus.Text = "Process is done"
                    lblStatus.ForeColor = SharedFunction.SuccessColor
                End If
            End If
        Else
            Dim sbRF As New StringBuilder
            Dim sbQuery As New StringBuilder
            sbQuery.Append("Select dbo.tblRelCDFRData.OrigData, dbo.tblRelCDFRData.RF_ReasonCode, dbo.tblRelCDFRData.RF_Date, dbo.tblCDFR.GSUfilename, dbo.tblRelCDFRData.PurchaseOrder ")
            sbQuery.Append("From dbo.tblRelCDFRData INNER Join ")
            sbQuery.Append("dbo.tblCDFR ON dbo.tblRelCDFRData.CDFRID = dbo.tblCDFR.CDFRID ")
            sbQuery.Append("WHERE dbo.tblCDFR.GSUfilename='@'")

            If DAL.SelectDataForRF(sbQuery.ToString.Replace("@", cboCDFR.Text.Trim)) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim IsBatchComplete As Boolean = True
                    If DAL.TableResult.Select("RF_ReasonCode<>'000'").Length > 0 Then IsBatchComplete = False

                    Dim strGSU As String = ""
                        Dim PurchaseOrder As String = ""
                        For Each rwRF As DataRow In DAL.TableResult.Rows
                            If strGSU = "" Then strGSU = IIf(rwRF("GSUfilename").ToString.Contains("."), rwRF("GSUfilename").ToString.Split(".")(0), rwRF("GSUfilename").ToString)
                            If PurchaseOrder = "" Then PurchaseOrder = rwRF("PurchaseOrder").ToString.Trim
                            Dim rfDate As String = "".PadRight(50, " ")
                            If Not IsDBNull(rwRF("RF_Date")) Then rfDate = CDate(rwRF("RF_Date")).ToString("yyyyMMdd").PadRight(50, " ")
                        sbRF.Append(String.Format("{0}{1}{2}", rwRF("OrigData").ToString.Trim.Substring(0, 451), rfDate, rwRF("RF_ReasonCode").ToString.Trim) & vbNewLine)
                    Next
                        'UBP RF
                        If sbRF.ToString <> "" Then
                            Dim ubpRF_FileOutput As String = SharedFunction.ResponseFile_FileNaming(PurchaseOrder, strGSU, IsBatchComplete)

                            'IO.File.WriteAllText(String.Format("{0}\{1}.txt", SharedFunction.ReportsUBPRFRepository, strGSU), sbRF.ToString)
                            IO.File.WriteAllText(ubpRF_FileOutput, sbRF.ToString)
                            lblStatus.Text = "Process is done"
                            lblStatus.ForeColor = SharedFunction.SuccessColor
                        End If
                    End If
                End If
        End If

        DAL.Dispose()
        DAL = Nothing
    End Sub
End Class

Public Class SelectPurchaseOrder
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
            'If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            GridDataBind()

            Select Case Session("DataExtraction").ToString
                Case "RejectList"
                    xGrid.Columns(2).Visible = False
                    xGrid.Columns(3).Visible = False
                    divOutput.Visible = False
                Case "CubaoBranchData"
                    divOutput.Visible = False
            End Select
        End If
    End Sub

    Private Function GetSelectedItems_POID(ByRef intTotal As Integer, ByRef sbPurchaseOrders As StringBuilder) As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGrid.GetSelectedFieldValues("POID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGrid.GetSelectedFieldValues("POID")(i)))
            End If
            sbPurchaseOrders.AppendLine(xGrid.GetSelectedFieldValues("PurchaseOrder")(i))
            intTotal += xGrid.GetSelectedFieldValues("Quantity")(i)
        Next

        Return sb.ToString
    End Function

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        Dim strQuery As String = Nothing

        If Session("DataExtraction") <> "RejectList" Then
            strQuery = "SELECT POID, PurchaseOrder, Quantity, DateTimePosted FROM dbo.tblPO"
        Else
            strQuery = "SELECT DISTINCT POID, PurchaseOrder, 0 As Quantity, GETDATE() As DateTimePosted FROM dbo.tblCardReject"
        End If

        If DAL.SelectQuery(strQuery) Then
            xGrid.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        LinkButton1.Visible = False

        Dim sbPurchaseOrder As New StringBuilder
        Dim intTotal As Integer = 0
        Dim POIDs As String = GetSelectedItems_POID(intTotal, sbPurchaseOrder)

        Dim sb As New StringBuilder
        Dim intError As Integer = 0
        Dim _PO As New PurchaseOrder
        Dim dt As DataTable = Nothing
        If Session("DataExtraction") = "Muhlbauer" Then
            '_PO.SaveToDownloadableFiles_ForMuhlbauer(POIDs, sbPurchaseOrder.ToString.Trim, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository, sb, intError)
            '_PO.SaveToDownloadableFiles_ForMuhlbauer(POIDs, txtOutput.Text, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository, sb, intError)
            _PO.SaveToDownloadableFiles_ForMuhlbauerv2(POIDs, txtOutput.Text, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository, sb, intError, True)
        ElseIf Session("DataExtraction") = "LaserEngraving" Then
            '_PO.SaveToDownloadableFiles_ForLaser(POIDs, txtOutput.Text, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & txtOutput.Text, sb, intError)
            _PO.SaveToDownloadableFiles_ForLaserv2(POIDs, txtOutput.Text, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & txtOutput.Text, sb, intError, True)
        ElseIf Session("DataExtraction") = "DeliveryReceipt2" Then
            _PO.GenerateDR2(POIDs, SharedFunction.PersoRepository & "\" & txtOutput.Text, SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.UserCompleteName(Page.User.Identity.Name), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), sb, intError)
            'ElseIf Session("DataExtraction") = "UBP_RF" Then
            '    Dim sbRF As New StringBuilder
            '    Dim sbQuery As New StringBuilder
            '    sbQuery.Append("Select Case dbo.tblRelCDFRData.OrigData, dbo.tblRelCDFRData.RF_ReasonCode, dbo.tblRelCDFRData.RF_Date, dbo.tblCDFR.GSUfilename ")
            '    sbQuery.Append("From dbo.tblRelCDFRData INNER Join ")
            '    sbQuery.Append("dbo.tblCDFR ON dbo.tblRelCDFRData.CDFRID = dbo.tblCDFR.CDFRID ")
            '    sbQuery.Append("WHERE dbo.tblRelCDFRData.PurchaseOrder='@'")
            '    Dim DAL As New DAL
            '    For Each strPO As String In sbPurchaseOrder.ToString
            '        sbRF.Clear()

            '        If DAL.SelectDataForRF(sbQuery.ToString.Replace("@", strPO)) Then
            '            Dim strGSU As String = ""
            '            For Each rwRF As DataRow In DAL.TableResult.Rows
            '                If strGSU = "" Then strGSU = IIf(rwRF("GSUfilename").ToString.Contains("."), rwRF("GSUfilename").ToString.Split(".")(0), rwRF("GSUfilename").ToString)
            '                Dim rfDate As String = "".PadRight(50, " ")
            '                If Not IsDBNull(rwRF("RF_Date")) Then rfDate = CDate(rwRF("RF_Date")).ToString("yyyyMMdd").PadRight(50, " ")
            '                sbRF.Append(String.Format("{0}{1}{2}", rwRF("OrigData").ToString.Trim.Substring(0, 451), rfDate, rwRF("RF_ReasonCode").ToString.Trim) & vbNewLine)
            '            Next
            '            'UBP RF
            '            If sbRF.ToString <> "" Then IO.File.WriteAllText(String.Format("{0}\{1}.txt", SharedFunction.ReportsUBPRFRepository, strGSU), sbRF.ToString)
            '        End If
            '    Next

            '    DAL.Dispose()
            '    DAL = Nothing
        ElseIf Session("DataExtraction") = "RejectList" Then
            Dim DAL As New DAL
            If DAL.SelectCardReject(String.Format(" WHERE dbo.tblCardReject.POID IN ({0})", POIDs)) Then
                dt = DAL.TableResult
                Session("dtRejectList") = dt
            End If
            DAL.Dispose()
            DAL = Nothing

            LinkButton1.Visible = True
        ElseIf Session("DataExtraction") = "ACML" Then
            Dim DAL As New DAL
            Dim sbACML As New StringBuilder

            For Each strPO As String In sbPurchaseOrder.ToString.Split(",")
                sbACML.Clear()

                strPO = strPO.Replace(vbCrLf, "")

                If DAL.SelectQuery("SELECT Barcode, PurchaseOrder, POSubFolder,RF_Date FROM dbo.vwCDFRList WHERE PurchaseOrder='" & strPO & "'") Then
                    For Each rwACML As DataRow In DAL.TableResult.Rows
                        sbACML.Clear()
                        Dim strSourcePath As String = SharedFunction.GetPath(rwACML("PurchaseOrder"), IIf(IsDBNull(rwACML("POSubFolder")), "", rwACML("POSubFolder").ToString.Trim), rwACML("Barcode"))
                        If strSourcePath <> "Error" Then
                            Dim MemberXML As New MemberXML(String.Format("{0}\{1}.xml", strSourcePath, rwACML("Barcode").ToString.Trim))
                            If Not MemberXML.GetDataAndPopulate() Then
                                'intError += 1
                                'sb.AppendLine(String.Format("MemberXML.ExtractDataForMuhlbauer(): Unable to extract data of Xml field of Barcode {0}", rw("Barcode")))
                            Else
                                Dim address() As String = MemberXML.AddressDelimited.Split("||")

                                'revised as requested by sir ferdie 03/06/2019
                                sbACML.Append(String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}",
                                                           FormatACMLFields(MemberXML.Barcode, 20), FormatACMLFields(rwACML("PurchaseOrder").ToString, 33), FormatACMLFields(MemberXML.Barcode, 20), FormatACMLFields(MemberXML.CRN.Replace("-", ""), 12),
                                                           FormatACMLFields(MemberXML.FirstName, 40), FormatACMLFields(MemberXML.MiddleName, 40), FormatACMLFields(MemberXML.LastName, 40), FormatACMLFields(MemberXML.Suffix, 10),
                                                           FormatACMLFields(address(0), 40), FormatACMLFields(address(2), 15), FormatACMLFields(address(4), 40), FormatACMLFields(address(6), 40), FormatACMLFields(address(8), 30),
                                                           FormatACMLFields(address(10), 30), FormatACMLFields(address(12), 30), FormatACMLFields(address(14), 3), FormatACMLFields(address(16), 6),
                                                           CDate(rwACML("RF_Date")).ToString("yyyyMMdd")) & vbNewLine)
                            End If
                        End If
                    Next

                    If sbACML.ToString <> "" Then IO.File.WriteAllText(String.Format("{0}\{1}\ACML_{1}_{2}.txt", SharedFunction.ReportsRepository, strPO, Now.ToString("MMdd")), sbACML.ToString, Encoding.Default)
                End If
            Next

            DAL.Dispose()
            DAL = Nothing

            LinkButton1.Visible = True
        ElseIf Session("DataExtraction") = "CubaoBranchData" Then
            Session.Remove("dtCubaoBranchList")
            LinkButton1.Visible = False

            Dim DAL As New DAL
            If DAL.SelectCubaoBranchList(String.Format(" AND dbo.tblPO.POID IN ({0})", POIDs)) Then
                dt = DAL.TableResult
                If dt.DefaultView.Count > 0 Then
                    Session("dtCubaoBranchList") = dt
                    LinkButton1.Visible = True
                End If
            End If
            DAL.Dispose()
            DAL = Nothing
        End If
        _PO = Nothing

        If intError = 0 Then
            lblStatus.Text = "Process is done"
            lblStatus.ForeColor = SharedFunction.SuccessColor
        Else
            WriteToErrorLog("Process is done with " & intError & " error/s (" & sb.ToString & ")")
            lblStatus.Text = "Process is done with " & intError & " error/s (" & sb.ToString & ")"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        End If

        GridDataBind()
    End Sub

    Private Function FormatACMLFields(ByVal value As String, ByVal DataLen As Integer) As String
        If value.Trim.Length = DataLen Then
            Return value
        ElseIf value.Trim.Length < DataLen Then
            Return value.PadRight(DataLen, " ")
        Else
            Return value.Substring(0, DataLen)
        End If
    End Function

    Private Sub WriteToErrorLog(ByVal strError As String)
        Dim DAL As New DAL
        DAL.AddErrorLog(strError, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub ExtractAndDownload()
        Dim dt As DataTable = CType(Session("dtRejectList"), DataTable)
        Dim sb As New StringBuilder
        For Each rw As DataRow In dt.Rows
            Dim strBarcode As String = IIf(IsDBNull(rw("Barcode")), "", rw("Barcode").ToString.Trim)
            Dim strCRN As String = IIf(IsDBNull(rw("CRN")), "", rw("CRN").ToString.Trim)
            Dim strPO As String = IIf(IsDBNull(rw("PurchaseOrder")), "", rw("PurchaseOrder").ToString.Trim)
            Dim strRejectType As String = IIf(IsDBNull(rw("RejectTypeDesc")), "", rw("RejectTypeDesc").ToString.Trim)
            sb.AppendLine(strBarcode & vbTab & strCRN & vbTab & strPO & vbTab & strRejectType & vbTab & CDate(rw("DateTimePosted")).ToString("MM/dd/yyyy"))
        Next

        Dim sbError As New StringBuilder
        Dim strFile As String = String.Format("{0}\{1}.csv", SharedFunction.QCRepository, "RejectList_" & Now.ToString("yyyyMMdd_hhmmtt"))
        IO.File.WriteAllText(strFile, sb.ToString)

        If sbError.ToString = "" Then
            If IO.File.Exists(strFile) Then
                HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
                Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
                HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
                Dim Dfile As New System.IO.FileInfo(strFile)
                HttpContext.Current.Response.WriteFile(Dfile.FullName)
                HttpContext.Current.Response.[End]()
            End If
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        If Session("DataExtraction") <> "CubaoBranchData" Then
            ExtractAndDownload()
        Else
            If Not Session("dtCubaoBranchList") Is Nothing Then
                Dim tempFile As String = Server.MapPath("~/Temp/" + String.Format("CubaoBranchData_{0}.xlsx", Now.ToString("yyyyMMdd_hhmmss")))

                If SharedFunction.ExportToExcel(Session("dtCubaoBranchList"), tempFile, "data") Then
                    SharedFunction.ViewDownloadFile(tempFile)
                End If
            End If
        End If
    End Sub

End Class
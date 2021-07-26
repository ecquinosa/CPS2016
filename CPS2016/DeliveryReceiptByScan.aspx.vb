
Public Class DeliveryReceiptByScan
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.DeliveryReceipt) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            'If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            PopulatePurchaseOrder()

            If Not Session("DR_Param") Is Nothing Then
                Dim param As String = Session("DR_Param") 'String.Format("{0}|{1}", cboPO.Text, dateEdit.Text)
                cboPO.Text = param.Split("|")(0)
                txtSource.Text = param.Split("|")(1)
                Search()
            End If

            If Not Session("ListType") Is Nothing Then
                Select Case Session("ListType").ToString
                    Case "Laser", "Muhlbauer"
                        chkCompleteDR.Visible = False
                        xGrid.Visible = False
                    Case Else
                        chkCompleteDR.Visible = True
                        xGrid.Visible = True
                End Select
            End If
        End If
    End Sub

    Private Sub PopulatePurchaseOrder()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT PurchaseOrder FROM tblPO ORDER BY CAST(SUBSTRING(PurchaseOrder,15,3) as int)") Then
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

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Search()
    End Sub

    Private Sub Search()
        lblStatus.ForeColor = Drawing.Color.Black
        lblStatus.Text = ""

        If txtSource.Text = "" Then
            lblStatus.Text = "Please enter barcode/s"
            lblStatus.ForeColor = SharedFunction.ErrorColor

            'GridDataBind()
        Else
            Dim intError As Integer
            Dim sb As New StringBuilder
            If Not Session("ListType") Is Nothing Then
                Select Case Session("ListType").ToString
                    Case "Laser", "Muhlbauer"
                        Dim _PO As New PurchaseOrder

                        Dim ListType As Short = 0

                        Dim bln As Boolean = True
                        Dim sbBarcodes As New StringBuilder
                        For Each strLine As String In txtSource.Text.Split(vbNewLine)


                            Select Case ListType
                                Case 0
                                    If strLine.Trim.Contains(".") Then ListType = 2 Else ListType = 1
                                Case 1
                                    If strLine.Trim.Contains(".") Then bln = False
                                Case 2
                                    If Not strLine.Trim.Contains(".") Then bln = False
                            End Select

                            If sbBarcodes.ToString = "" Then
                                sbBarcodes.Append(String.Format("'{0}'", strLine.Trim))
                            Else
                                sbBarcodes.Append("," & String.Format("'{0}'", strLine.Trim))
                            End If
                        Next

                        If bln Then
                            'revised code to allow backOCR 06/04/2019
                            'Dim strWhereQuery As String = " WHERE dbo.tblPO.PurchaseOrder='" & cboPO.Text & "' AND dbo.tblRelPOData.Barcode IN (" & sbBarcodes.ToString & ")"
                            Dim strWhereQuery As String = " WHERE dbo.tblPO.PurchaseOrder='" & cboPO.Text & "'"
                            If ListType = 1 Then
                                strWhereQuery += " AND dbo.tblRelPOData.Barcode IN (" & sbBarcodes.ToString & ")"
                            ElseIf ListType = 2 Then
                                strWhereQuery += " AND dbo.tblRelPOData.BackOCR IN (" & sbBarcodes.ToString & ")"
                            End If

                            If Session("ListType").ToString = "Laser" Then
                                _PO.SaveToDownloadableFiles_ForLaserv2(strWhereQuery, String.Format("{0}_Laser", cboPO.Text.Trim), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & String.Format("{0}_Laser", cboPO.Text.Trim), sb, intError, False)
                            Else
                                _PO.SaveToDownloadableFiles_ForMuhlbauerv2(strWhereQuery, String.Format("{0}_Muhl", cboPO.Text.Trim), SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository, sb, intError, False)
                            End If

                            lblStatus.Text = "Process is done"
                            lblStatus.ForeColor = SharedFunction.SuccessColor
                        Else
                            lblStatus.Text = "Please re-check list. List should be all barcode only or all back ocr only."
                            lblStatus.ForeColor = SharedFunction.ErrorColor
                        End If

                        _PO = Nothing
                    Case Else
                        Session("IsBtnSubmit") = 1
                        GridDataBind()
                End Select
            End If
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim dt As DataTable
        Dim dt2 As DataTable

        Dim dtRF As DataTable

        Dim dtDRExcludedBarcodes As DataTable
        If Not Session("dtDRExcludedBarcodes") Is Nothing Then dtDRExcludedBarcodes = CType(Session("dtDRExcludedBarcodes"), DataTable)

        If Not Session("IsBtnSubmit") Is Nothing Then
            Dim sb As New StringBuilder
            For Each strLine As String In txtSource.Text.Split(vbNewLine)
                Dim bln As Boolean = False

                If Not dtDRExcludedBarcodes Is Nothing Then _
                    If dtDRExcludedBarcodes.Select("Barcode='" & strLine.Trim & "'").Length > 0 Then bln = True

                If Not bln Then
                    If sb.ToString = "" Then
                        sb.Append(String.Format("'{0}'", strLine.Trim))
                    Else
                        sb.Append("," & String.Format("'{0}'", strLine.Trim))
                    End If
                End If
            Next

            Dim strWhereQuery As String = " WHERE dbo.tblPO.PurchaseOrder='" & cboPO.Text & "' AND dbo.tblRelPOData.Barcode IN (" & sb.ToString & ")"
            Dim strWhereQueryRF As String = " WHERE dbo.tblRelCDFRData.PurchaseOrder='" & cboPO.Text & "' AND dbo.tblRelCDFRData.Barcode IN (" & sb.ToString & ")"

            Dim DAL As New DAL
            If DAL.SelectDRListByBarcode(strWhereQuery) Then
                dt = DAL.TableResult
            End If
            If DAL.SelectUniqueCDFRByPOAndBarcode(strWhereQueryRF) Then
                dtRF = DAL.TableResult
            End If
            DAL.Dispose()
            DAL = Nothing

            Dim intBarcodeDuplicate As Integer = 0
            Dim dups = From row In dt.AsEnumerable()
                       Let Barcode = row.Field(Of String)("Barcode")
                       Group row By Barcode Into DupBarcode = Group
                       Where DupBarcode.Count() > 1
                       Select DupBarcode

            'Dim intCntr As Short = 0

            Dim dtBarcodeDuplicate As New DataTable
            dtBarcodeDuplicate.Columns.Add("Barcode", Type.GetType("System.String"))
            dtBarcodeDuplicate.Columns.Add("Count", Type.GetType("System.Int16"))

            For Each dupBarcodeRows In dups
                Dim intCntr As Short = 1
                For Each row In dupBarcodeRows
                    If dtBarcodeDuplicate.Select("Barcode='" & row.Field(Of String)("Barcode") & "'").Length = 0 Then
                        Dim rw As DataRow = dtBarcodeDuplicate.NewRow
                        rw("Barcode") = row.Field(Of String)("Barcode")
                        dtBarcodeDuplicate.Rows.Add(rw)
                        intCntr += 1
                    Else
                        dtBarcodeDuplicate.Select("Barcode='" & row.Field(Of String)("Barcode") & "'")(0)(1) = intCntr
                        intCntr += 1
                    End If
                Next
            Next

            For Each dupBarcodeRows In dups
                Dim intCntr As Short = 1
                For Each row In dupBarcodeRows
                    If dtBarcodeDuplicate.Select("Barcode='" & row.Field(Of String)("Barcode") & "'")(0)(1) <> intCntr Then
                        row("Barcode") = row.Field(Of String)("Barcode") & "^"
                        'intBarcodeDuplicate += 1
                        intCntr += 1
                    Else
                        row("Barcode") = row.Field(Of String)("Barcode") & "**"
                        intBarcodeDuplicate += 1
                        intCntr += 1
                    End If
                Next
            Next

            dt2 = dt.Select("Barcode NOT LIKE '%^'").CopyToDataTable
            xGrid.DataSource = dt2

            Session("IsBtnSubmit") = Nothing

            Session("dtGrid") = dt2
            Session("dtGrid_RF") = dtRF
        Else
            dt2 = Session("dtGrid")

            xGrid.DataSource = dt2
        End If

        lbViewDownloadTXT.Visible = False
        lbViewDownloadPDF.Visible = False
        If dt2.DefaultView.Count = 0 Then
            LinkButton1.Visible = False
        Else
            LinkButton1.Visible = True
        End If
        
        'Dim intBarcodeDistinct As Integer = dt.DefaultView.Count - intBarcodeDuplicate 'dt.DefaultView.ToTable(True, "Barcode").DefaultView.Count
        Dim intBarcodeWithSerial As Integer = dt2.Select("ChipSerialNo NOT IS NULL").Length ' - intBarcodeDuplicate

        'lblTotal.Text = String.Format("PO Quantity: {0},        With Chip Serial: {1},        Missing: {2},        Duplicate: {3}", intBarcodeDistinct.ToString, intBarcodeWithSerial.ToString, (intBarcodeDistinct - intBarcodeWithSerial).ToString, intBarcodeDuplicate.ToString)
        lblTotal.Text = String.Format("PO Quantity: {0},        With Chip Serial: {1}", dt2.DefaultView.Count.ToString, intBarcodeWithSerial.ToString)
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Sub ExtractAndDownload()
        Dim _rn1 As New Random
        Dim _rn2 As New Random

        Dim DAL As New DAL

        Dim intCntr As Integer = 0
        Dim sb As New StringBuilder
        Dim sbDR_Summary As New StringBuilder
        Dim dt As DataTable = CType(Session("dtGrid"), DataTable)
        Dim dtRF As DataTable = CType(Session("dtGrid_RF"), DataTable)

        Dim dtCardCarrier As New DataTable
        dtCardCarrier.TableName = "CardCarrier_PO"
        dtCardCarrier.Columns.Add("PurchaseOrder", GetType(String))
        dtCardCarrier.Columns.Add("PO_Barcode", GetType(Byte()))
        dtCardCarrier.Columns.Add("Barcode", GetType(String))
        dtCardCarrier.Columns.Add("PO_BarcodeCRN", GetType(Byte()))

        Dim bg As New BarcodeWeb
        'Dim bg As New BarcodeWeb2
        Dim blnCardCarrier As Boolean

        For Each rw As DataRow In dt.Rows
            If rw("Barcode").ToString.Trim <> rw("Barcode").ToString.Trim.Replace("*", "") & "*" Then
                If Not IsDBNull(rw("UID")) Then

                    Dim _barcode As String = rw("Barcode").ToString.Trim.Replace("*", "")
                    Dim _crn As String = rw("CRN").ToString.Trim.Replace("*", "").Replace("-", "")

                    'sb.AppendLine(String.Format("{0},{1},{2}", rw("Barcode").ToString.Trim, rw("ChipSerialNo").ToString.Trim, rw("CardDate").ToString.Trim))
                    sb.AppendLine(String.Format("{0},{1},{2}", _barcode, rw("UID").ToString.Trim.PadLeft(32, "0"), CDate(rw("CardDate")).ToString("ddMMyyyy")))
                    intCntr += 1

                    DAL.UpdateRelCDFRDataByPOAndBarcode(_barcode, cboPO.Text.Trim)

                    Dim PO_Barcode As String = String.Format("{0}\{1}_{2}.jpg", SharedFunction.BarcodeRepository, _barcode, Now.ToString("hhmmss"))
                    Dim PO_BarcodeCRN As String = String.Format("{0}\{1}_{2}.jpg", SharedFunction.BarcodeRepository, _crn, _barcode, Now.ToString("hhmmss"))

                    If Not System.IO.File.Exists(PO_Barcode) Then
                        'barcode
                        For i As Short = 1 To 3
                            'blnCardCarrier = bg.GenerateBarcodeCode39(_barcode, PO_Barcode)
                            blnCardCarrier = bg.GenerateBarcode(_barcode, PO_Barcode)
                            If blnCardCarrier Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                    End If

                    If Not System.IO.File.Exists(PO_BarcodeCRN) Then
                        'barcode
                        For i As Short = 1 To 3
                            blnCardCarrier = bg.GenerateBarcode(_crn.Replace("-", ""), PO_BarcodeCRN)
                            If blnCardCarrier Then Exit For
                            System.Threading.Thread.Sleep(3000)
                        Next
                    End If

                    Dim rwCardCarrier As DataRow = dtCardCarrier.NewRow
                    rwCardCarrier(0) = cboPO.Text.Trim
                    rwCardCarrier(1) = System.IO.File.ReadAllBytes(PO_Barcode)
                    rwCardCarrier(2) = _barcode
                    rwCardCarrier(3) = System.IO.File.ReadAllBytes(PO_BarcodeCRN)
                    'rwCardCarrier(3) = System.IO.File.ReadAllBytes(PO_Barcode)
                    dtCardCarrier.Rows.Add(rwCardCarrier)

                    Try
                        System.IO.File.Delete(PO_Barcode)
                        System.IO.File.Delete(PO_BarcodeCRN)
                    Catch ex As Exception
                    End Try

                    If sbDR_Summary.ToString = "" Then
                        sbDR_Summary.Append(String.Format("'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                    Else
                        sbDR_Summary.Append(String.Format(",'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                    End If
                Else
                    If chkCompleteDR.Checked Then
                        Dim chipserial As String = "0000000000000000" & _rn1.Next(12345678, 99999999).ToString.PadLeft(8, "0") & _rn2.Next(12345678, 99999999).ToString.PadLeft(8, "0")
                        sb.AppendLine(String.Format("{0},{1},{2}", rw("Barcode").ToString.Trim.Replace("*", ""), chipserial, Now.ToString("ddMMyyyy")))
                        intCntr += 1

                        If sbDR_Summary.ToString = "" Then
                            sbDR_Summary.Append(String.Format("'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                        Else
                            sbDR_Summary.Append(String.Format(",'{0}'", rw("Barcode").ToString.Trim.Replace("*", "")))
                        End If
                    End If
                End If
            End If
        Next

        bg = Nothing

        If Not IO.Directory.Exists(SharedFunction.ReportsRepository & "\" & cboPO.Text.Trim) Then IO.Directory.CreateDirectory(SharedFunction.ReportsRepository & "\" & cboPO.Text.Trim)

        Dim sbError As New StringBuilder
        Dim strFile As String = String.Format("{0}\{1}\{2}.txt", SharedFunction.ReportsRepository, cboPO.Text.Trim, cboPO.Text.Trim)
        IO.File.WriteAllText(strFile, sb.ToString)


        Dim sbRF As New StringBuilder
        For Each rw As DataRow In dtRF.Rows
            sbRF.Clear()
            If DAL.SelectDataForRF("SELECT OrigData, RF_ReasonCode, RF_Date FROM dbo.tblRelCDFRData WHERE CDFRID=" & rw("CDFRID").ToString) Then
                For Each rwRF As DataRow In DAL.TableResult.Rows
                    Dim rfDate As String = "".PadRight(50, " ")
                    If Not IsDBNull(rwRF("RF_Date")) Then rfDate = CDate(rwRF("RF_Date")).ToString("yyyyMMdd").PadRight(50, " ")
                    sbRF.Append(String.Format("{0}{1}{2}", rwRF("OrigData").ToString.Trim.Substring(0, 451), rfDate, rwRF("RF_ReasonCode").ToString.Trim) & vbNewLine)
                Next

                If sbRF.ToString <> "" Then IO.File.WriteAllText(String.Format("{0}\{1}\{2}.txt", SharedFunction.ReportsRepository, cboPO.Text.Trim, IIf(rw("GSUfilename").ToString.Contains("."), rw("GSUfilename").ToString.Split(".")(0), rw("GSUfilename").ToString)), sbRF.ToString)
            End If
        Next
        'DAL.Dispose()
        'DAL = Nothing

        Dim sbACML As New StringBuilder

        'If DAL.SelectQuery("SELECT Barcode, PurchaseOrder, POSubFolder,RF_Date FROM dbo.vwCDFRList WHERE RF_Date = convert(char(10),getdate(),101) AND PurchaseOrder='" & cboPO.Text.Trim & "'") Then
        If DAL.SelectQuery("SELECT Barcode, PurchaseOrder, POSubFolder,RF_Date FROM dbo.vwCDFRList WHERE PurchaseOrder='" & cboPO.Text.Trim & "'") Then
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
                        'sbACML.Append(String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}",
                        '                            FormatACMLFields(MemberXML.Barcode, 20), FormatACMLFields(rwACML("PurchaseOrder").ToString, 33), FormatACMLFields(MemberXML.CRN.Replace("-", ""), 12),
                        '                            FormatACMLFields(MemberXML.FirstName, 40), FormatACMLFields(MemberXML.MiddleName, 40), FormatACMLFields(MemberXML.LastName, 40), FormatACMLFields(MemberXML.Suffix, 10),
                        '                            FormatACMLFields(address(0), 40), FormatACMLFields(address(2), 15), FormatACMLFields(address(4), 40), FormatACMLFields(address(6), 40), FormatACMLFields(address(8), 30),
                        '                            FormatACMLFields(address(10), 30), FormatACMLFields(address(12), 30), FormatACMLFields(address(14), 3), FormatACMLFields(address(16), 6),
                        '                            CDate(rwACML("RF_Date")).ToString("yyyyMMdd"), 1) & vbNewLine)

                        'revised as requested by sir ferdie 03/06/2019
                        If Not IsDBNull(rwACML("RF_Date")) Then
                            sbACML.Append(String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}",
                                                   FormatACMLFields(MemberXML.Barcode, 20), FormatACMLFields(rwACML("PurchaseOrder").ToString, 33), FormatACMLFields(MemberXML.Barcode, 20), FormatACMLFields(MemberXML.CRN.Replace("-", ""), 12),
                                                   FormatACMLFields(MemberXML.FirstName, 40), FormatACMLFields(MemberXML.MiddleName, 40), FormatACMLFields(MemberXML.LastName, 40), FormatACMLFields(MemberXML.Suffix, 10),
                                                   FormatACMLFields(address(0), 40), FormatACMLFields(address(2), 15), FormatACMLFields(address(4), 40), FormatACMLFields(address(6), 40), FormatACMLFields(address(8), 30),
                                                   FormatACMLFields(address(10), 30), FormatACMLFields(address(12), 30), FormatACMLFields(address(14), 3), FormatACMLFields(address(16), 6),
                                                   CDate(rwACML("RF_Date")).ToString("yyyyMMdd")) & vbNewLine)
                        End If
                    End If
                End If
            Next

            If sbACML.ToString <> "" Then IO.File.WriteAllText(String.Format("{0}\{1}\ACML_{1}_{2}.txt", SharedFunction.ReportsRepository, cboPO.Text.Trim, Now.ToString("MMdd")), sbACML.ToString, Encoding.Default)
        End If

        DAL.Dispose()
        DAL = Nothing

        Dim rg As New RptGenerator
        Dim pdfError As String = ""
        Try
            Dim outputFile As String = "" '"D:\cardcarrier_ubp.pdf"

            If Not rg.GenerateReportCardCarrier(dtCardCarrier, outputFile, " WHERE vwCDFRListv2.PurchaseOrder='" & cboPO.Text & "' AND vwCDFRListv2.Barcode IN (" & sbDR_Summary.ToString & ")", cboPO.Text) Then
                pdfError = rg.ErrorMessage
            Else

            End If
        Catch ex As Exception
            Console.Write(ex.Message)
            pdfError = ex.Message
        Finally
            rg = Nothing
        End Try


        If sbError.ToString = "" Then
            If IO.File.Exists(strFile) Then
                Session("drTxtFile") = strFile
                'Dim DAL As New DAL
                'DAL.UpdateRelPOReportByPOAndReportTypeID(txtPO.Text, DataKeysEnum.Report.DeliveryReceipt, IO.File.ReadAllBytes(strFile))
                'DAL.AddSystemLog(String.Format("{0} generated PO {1} dr", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))

                Session("drParameters") = String.Format("{0}|{1}|{2}", cboPO.Text, intCntr.ToString, sbDR_Summary.ToString)

                'Dim result As String = GenerateDR_PDF(cboPO.Text, intCntr, sb.ToString)
                'If result = "Error" Then
                '    DAL.AddErrorLog(String.Format("ExtractAndDownload(): GenerateDR_PDF - Failed to generate dr pdf report of " & txtPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                'ElseIf result = "" Then
                '    DAL.AddErrorLog(String.Format("ExtractAndDownload(): GenerateDR_PDF - Failed to generate dr pdf report of " & txtPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                'Else
                'Session("drPdfFile") = result
                '    DAL.AddSystemLog(String.Format("{0} generated PO {1} dr", SharedFunction.UserCompleteName(Page.User.Identity.Name), txtPO.Text), SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
                'End If

                'DAL.Dispose()
                'DAL = Nothing

                'ViewDownloadFile(strFile)
                'ViewDownloadFile(result)
                'HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
                'Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
                'HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
                'Dim Dfile As New System.IO.FileInfo(strFile)
                'HttpContext.Current.Response.WriteFile(Dfile.FullName)
                'HttpContext.Current.Response.[End]()
            End If
        End If
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

    Private Sub ViewDownloadFile(ByVal strFile As String)
        HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM"
        Dim Header As [String] = "Attachment; Filename=" & IO.Path.GetFileName(strFile)
        HttpContext.Current.Response.AppendHeader("Content-Disposition", Header)
        Dim Dfile As New System.IO.FileInfo(strFile)
        HttpContext.Current.Response.WriteFile(Dfile.FullName)
        HttpContext.Current.Response.[End]()
    End Sub

    Private Function GenerateDR_PDF(ByVal strPurchaseOrder As String, ByVal intQty As Integer, ByVal strData As String) As String
        Dim rg As New RptGenerator
        Try
            Dim outputFile As String = ""
            If rg.GenerateReport(DataKeysEnum.Report.DeliveryReceipt_PDF, 1, outputFile, strPurchaseOrder, intQty, strData) Then
                Return outputFile
            Else
                Return "Error"
            End If
        Catch ex As Exception
            Return "Error"
        Finally
            rg = Nothing
        End Try
    End Function

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        If cboPO.Text = "" Then
            lblStatus.Text = "Please select purchase order"
            lblStatus.ForeColor = SharedFunction.ErrorColor
        Else
            lblStatus.Text = ""
            ExtractAndDownload()
            lbViewDownloadTXT.Visible = True
            lbViewGenerateSummaryPDF.Visible = True
            'lbViewDownloadPDF.Visible = True
        End If
    End Sub

    Protected Sub lbViewDownloadTXT_Click(sender As Object, e As EventArgs) Handles lbViewDownloadTXT.Click
        If Not Session("drTxtFile") Is Nothing Then ViewDownloadFile(Session("drTxtFile").ToString)
    End Sub

    Protected Sub lbViewDownloadPDF_Click(sender As Object, e As EventArgs) Handles lbViewDownloadPDF.Click
        If Not Session("drPdfFile") Is Nothing Then ViewDownloadFile(Session("drPdfFile").ToString)
    End Sub

    Private Function ReportName() As String
        Return "CPS_DeliveryReceipt"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine(cboPO.Text & " Delivery Receipt - Manual")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

    Protected Sub lbExcluded_Click(sender As Object, e As EventArgs) Handles lbExcluded.Click
        Session("DR_Param") = String.Format("{0}|{1}", cboPO.Text, txtSource.Text)
        Session("OriginatingPage") = Request.Url.AbsolutePath
        Response.Redirect("DeliveryReceiptExcludedList.aspx")
    End Sub

    Protected Sub lbViewGenerateSummaryPDF_Click(sender As Object, e As EventArgs) Handles lbViewGenerateSummaryPDF.Click
        ASPxPopupControl1.ContentUrl = "~/ReportD_DeliveryReceipt.aspx"
        ASPxPopupControl1.Height = 650
        ASPxPopupControl1.Width = 700
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

End Class
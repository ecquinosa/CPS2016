
Imports System.IO

Public Class Home_CPS
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Home_CPS) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            'GridDataBind_OpenBatch()
            'GridDataBind_ForDelivery()
            GridDataBind_StatusCounter()
        End If
    End Sub

    Private Sub xGridOpenBatch_DataBinding(sender As Object, e As System.EventArgs) Handles xGridOpenBatch.DataBinding
        BindGrid_OpenBatch()
    End Sub

    Private Sub xGridForDelivery_DataBinding(sender As Object, e As System.EventArgs) Handles xGridForDelivery.DataBinding
        BindGrid_ForDelivery()
    End Sub

    Private Sub xGridStatus_DataBinding(sender As Object, e As System.EventArgs) Handles xGridStatus.DataBinding
        BindGrid_StatusCounter()
    End Sub

    Private Sub GridDataBind_OpenBatch()
        xGridOpenBatch.DataBind()
    End Sub

    Private Sub GridDataBind_ForDelivery()
        xGridForDelivery.DataBind()
    End Sub

    Private Sub GridDataBind_StatusCounter()
        xGridStatus.DataBind()
    End Sub

    Private Sub BindGrid_OpenBatch()
        Dim DAL As New DAL

        If Session("AllOpenBatch") Is Nothing Then
            If DAL.SelectOpenBatchByCurrentDate Then
                xGridOpenBatch.DataSource = DAL.TableResult
            End If
        Else
            If DAL.SelectOpenBatch Then
                xGridOpenBatch.DataSource = DAL.TableResult
            End If
        End If
        
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub BindGrid_ForDelivery()
        Dim DAL As New DAL
        If DAL.SelectBatchForDelivery Then
            xGridForDelivery.DataSource = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub BindGrid_StatusCounter()
        Try
            Dim DAL As New DAL
            If DAL.SelectStatusCounter Then
                lbStatusCounterTimestamp.Text = DAL.TableResult.Rows(0)("LastUpdate")
                xGridStatus.DataSource = DAL.TableResult
            End If
            DAL.Dispose()
            DAL = Nothing
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    'Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
    '    If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    'End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim textFile As String = String.Format("{0}\cpsFile.txt", My.Settings.IndigoExtractRepository)
        Try
            File.WriteAllText(textFile, Now)
        Catch ex As Exception
            lbOpenBatchRefresh.Text = ex.Message
        End Try

        Return

        'Dim barcodeWeb2 As New BarcodeWeb
        'barcodeWeb2.GenerateCode39Barcode("01-20181227-P-262-0042-C-001-0040", "D:\testCode39.jpg")
        'barcodeWeb2 = Nothing

        'Return

        'Try
        '    Dim rg As New RptGenerator
        '    Dim outputFile As String = "~\Temp\PO_Barcode.jpg" '"D:\cardcarrier_ubp.pdf"

        '    If rg.GenerateReport(DataKeysEnum.Report.CardCarrier_UBP, 1, outputFile, " WHERE vwCDFRList.PurchaseOrder IS NOT NULL", "01-20181227-P-262-0042-C-001-0040") Then

        '    Else

        '    End If

        '    If rg.GenerateReport(DataKeysEnum.Report.CardCarrier_UBP, 3, outputFile, " WHERE vwCDFRList.PurchaseOrder IS NOT NULL", "01-20181227-P-262-0042-C-001-0040") Then

        '    Else

        '    End If
        'Catch ex As Exception
        '    Console.Write(ex.Message)
        'End Try

        'Return
        'Dim DAL As New DAL
        'Dim sb As New StringBuilder
        'Dim IsSuccess As Boolean

        ''added on 07/31/2017
        'If DAL.SelectQuery(String.Format("SELECT backocr, count(backocr) FROM dbo.tblRelPOData GROUP BY poid, backocr having (POID = {0}) and count(backocr)> 1", 4722)) Then
        '    For Each _rw As DataRow In DAL.TableResult.Rows
        '        sb.AppendLine(String.Format("Duplicate BackOCR {0}<br>", _rw("backocr").ToString.Trim))
        '        IsSuccess = False
        '    Next
        'End If

        'If DAL.SelectQuery(String.Format("SELECT crn, count(crn) FROM dbo.tblRelPOData GROUP BY poid, crn having (POID = {0}) and count(backocr)> 1", 4722)) Then
        '    For Each _rw As DataRow In DAL.TableResult.Rows
        '        sb.AppendLine(String.Format("Duplicate CRN {0}<br>", _rw("crn").ToString.Trim))
        '        IsSuccess = False
        '    Next
        'End If

        'If DAL.SelectQuery(String.Format("SELECT Barcode, count(Barcode) FROM dbo.tblRelPOData GROUP BY poid, Barcode having (POID = {0}) and count(backocr)> 1", 4722)) Then
        '    For Each _rw As DataRow In DAL.TableResult.Rows
        '        sb.AppendLine(String.Format("Duplicate Barcode {0}<br>", _rw("Barcode").ToString.Trim))
        '        IsSuccess = False
        '    Next
        'End If

        'Dim intBoxDivisibleBy210 As Integer = SharedFunction.GetTotalBoxDivisibleBy210(5283)

        'If DAL.SelectPO_PerBoxCntr(4708) Then
        '    For Each _rw As DataRow In DAL.TableResult.Rows
        '        If CInt(_rw("Box")) <> (intBoxDivisibleBy210 + 1) Then
        '            If CInt(_rw("Cntr")) <> 210 Then
        '                sb.AppendLine(String.Format("Box {0} have {1} count<br>", _rw("Box"), _rw("Cntr")))
        '                IsSuccess = False
        '            End If
        '        End If
        '    Next
        'End If

        'DAL.Dispose()
        'DAL = Nothing

        'Exit Sub
        'Dim rg As New RptGenerator
        'rg.GenerateReport(DataKeysEnum.Report.ElectronicReportOfDeliveredCardsPerPrintOrder, "01-20130122-P-023-4249-C-001-4053", 1)
        ''rg.GenerateReport(DataKeysEnum.Report.ElectronicReportOfDeliveredCardsPerPrintOrder, "01-20130122-P-023-4249-C-001-4053", 2)

        'rg.GenerateReport(DataKeysEnum.Report.ElectronicReportOfGoodCards, "01-20130122-P-023-4249-C-001-4053", 1)
        ''rg.GenerateReport(DataKeysEnum.Report.ElectronicReportOfGoodCards, "01-20130122-P-023-4249-C-001-4053", 2)
        'rg = Nothing


        'TextBox1.Text = "start: " & Now.ToString

        'Dim strFile As String = "C:\Allcard\SSS_CPS\ForUploading\01-20130124-P-025-4982-C-001-4755_zip.sss"
        'Dim outputFile As String = ""

        'If SharedFunction.EncryptDecryptFile(strFile, False, outputFile) Then
        '    Dim fc As New FileCompression
        '    If fc.ExtractZipFile(outputFile, "C:\Allcard\SSS_CPS\ForUploading\" & IO.Path.GetFileNameWithoutExtension(outputFile)) Then

        '    End If
        '    fc = Nothing
        '    'Dim strDestinationFile As String = String.Format("{0}\{1}", "C:\Allcard\SSS_CPS\ForUploading", Path.GetFileName(outputFile))
        '    'If File.Exists(strDestinationFile) Then
        '    '    File.Delete(outputFile)
        '    '    'SharedFunction.ShowInfoMessage("File already exist in destination...")
        '    'Else
        '    '    File.Move(outputFile, strDestinationFile)
        '    'End If
        'End If

        'TextBox1.Text += "       end: " & Now.ToString

        'Dim sb As New StringBuilder
        'Dim intError As Integer = 0
        'Dim _PO As New PurchaseOrder
        '_PO.SaveToDownloadableFiles_ForMuhlbauer(1, "01-20130122-P-023-4249-C-001-4053", "01-20130122-P-023-4249-C-001-4053", "PurchaseOrder", SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & "01-20130122-P-023-4249-C-001-4053", sb, intError)
        '_PO = Nothing
        'TextBox2.Text = String.Format("{0}-{1}-{2}", TextBox1.Text.Substring(0, 4), TextBox1.Text.Substring(4, 7), TextBox1.Text.Substring(11, 1))

        'Dim path As String = "\\ACITDG-02\Users\mldelacruz\New folder"

        'For Each strFile As String In IO.Directory.GetFiles(path)
        '    'Console.WriteLine(strFile)
        '    TextBox1.Text = strFile
        'Next

        'Dim _PO As New PurchaseOrder
        '_PO.SaveToDownloadableFiles_ForLaser(1, "01-20130122-P-023-4249-C-001-4053", "01-20130122-P-023-4249-C-001-4053", "PurchaseOrder", SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & "01-20130122-P-023-4249-C-001-4053", sb, intError)
        '_PO.SaveToDownloadableFiles_ForMuhlbauer(1, "01-20130122-P-023-4249-C-001-4053", "01-20130122-P-023-4249-C-001-4053", "PurchaseOrder", SharedFunction.UserID(Page.User.Identity.Name), SharedFunction.PersoRepository & "\" & "01-20130122-P-023-4249-C-001-4053", sb, intError)
        '_PO = Nothing

        'Dim _PO As New PurchaseOrder
        '_PO.GenerateDR2("3777,3797,3568", "01-20160323-P-294-0001-C-001-0001,01-20160516-P-464-0002-C-001-0002,01-20160115-P-043-0263-C-002-0003", "C:\Allcard\SSS_CPS", New StringBuilder, New Integer)
        '_PO = Nothing

        'Dim barcodeWeb As New BarcodeWeb
        ''barcodeWeb.GetBarcodeImage("0120151104M1ID161001")
        ''barcodeWeb.GenerateBarcode("0120151104M1ID161001")

        'For Each strBarcode As String In IO.File.ReadAllLines("C:\Allcard\SSS_CPS\Perso\gen_barcode.txt")
        '    barcodeWeb.GenerateBarcode(strBarcode.Trim, "C:\Allcard\SSS_CPS\Perso\gen_barcode2\" & strBarcode.Trim & ".jpg")
        'Next
        'barcodeWeb = Nothing

        'Dim cf As New Microsoft.VisualBasic.Devices.Computer
        'cf.FileSystem.CopyDirectory("C:\Allcard\SSS_CPS\ForUploading\01-20160104-P-003-0019-C-001-0019", "C:\Allcard\SSS_CPS\TEST", True)
        'cf = Nothing

        'Dim strBarcodes As String = "0120160107H2ID121017,0120151216H0ID079004,0120160111FJID229008"
        'Dim strDestination As String = "C:\Allcard\SSS_CPS\TEST2"

        'SharedFunction.CheckAndCreateDirectory(strDestination)

        'For Each strBarcode As String In strBarcodes.Split(",")
        '    Dim strSourcePath As String = SharedFunction.GetPath("01-20160118-P-048-8903-C-001-8676", strBarcode, strBarcode)

        '    If strSourcePath <> "Error" Then
        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & ".xml"), strDestination & "\" & strBarcode.Trim)

        '        Dim strLine As String = ""
        '        Dim MemberXML As New MemberXML(String.Format("{0}\{1}", SharedFunction.CheckAndCreateDirectory(strDestination & "\" & strBarcode.Trim), Path.GetFileName(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & ".xml"))))
        '        If Not MemberXML.ExtractDataForMuhlbauer(strLine) Then
        '            intError += 1
        '            sb.AppendLine(String.Format("MemberXML.ExtractDataForMuhlbauer(): Unable to extract data of Xml field of Barcode {0}", strBarcode.Trim))
        '        End If
        '        MemberXML = Nothing

        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Photo.jpg"), strDestination & "\" & strBarcode.Trim)
        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Signature.tiff"), strDestination & "\" & strBarcode.Trim)
        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Lprimary.ansi-fmr"), strDestination & "\" & strBarcode.Trim)
        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Lbackup.ansi-fmr"), strDestination & "\" & strBarcode.Trim)
        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Rprimary.ansi-fmr"), strDestination & "\" & strBarcode.Trim)
        '        SharedFunction.FileCopyImage(String.Format("{0}\{1}", strSourcePath, strBarcode.Trim & "_Rbackup.ansi-fmr"), strDestination & "\" & strBarcode.Trim)
        '    End If
        'Next

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Session("ActivityID") = xGridStatus.GetRowValues(index, "ActivityID")
        Session("ActivityDesc") = xGridStatus.GetRowValues(index, "ActivityDesc")

        ASPxPopupControl1.ContentUrl = "~/StatusCardByPO.aspx"

        ASPxPopupControl1.Height = 650
        ASPxPopupControl1.Width = 1200
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex

        Session("ActivityID") = xGridStatus.GetRowValues(index, "ActivityID")
        Session("ActivityDesc") = xGridStatus.GetRowValues(index, "ActivityDesc")

        ASPxPopupControl1.ContentUrl = "~/StatusCardBreakdown.aspx"

        ASPxPopupControl1.Height = 650
        ASPxPopupControl1.Width = 1200
        ASPxPopupControl1.ShowOnPageLoad = True
    End Sub

    Protected Sub lbOpenBatchRefresh_Click(sender As Object, e As EventArgs) Handles lbOpenBatchRefresh.Click
        Session("AllOpenBatch") = 1
        GridDataBind_OpenBatch()
    End Sub

    Protected Sub lbStatusCounter_Click(sender As Object, e As EventArgs) Handles lbStatusCounter.Click
        Dim DAL As New DAL
        DAL.RefreshStatusCounter()
        DAL.Dispose()
        DAL = Nothing
        GridDataBind_StatusCounter()
    End Sub

    Protected Sub lbForDelivery_Click(sender As Object, e As EventArgs) Handles lbForDelivery.Click
        GridDataBind_ForDelivery()
    End Sub

End Class
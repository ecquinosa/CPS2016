
Imports System.IO

Public Class ForUploadCMSReject
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.UploadPO) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim currentButton As LinkButton = TryCast(sender, LinkButton)
        Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
        Dim strReference As String = xGrid.GetRowValues(index, "FileName")

        UploadData(strReference)
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs)
        Try
            Dim currentButton As LinkButton = TryCast(sender, LinkButton)
            Dim index As Integer = TryCast(currentButton.NamingContainer, DevExpress.Web.ASPxGridView.GridViewDataRowTemplateContainer).VisibleIndex
            Dim strReference As String = xGrid.GetRowValues(index, "FileName")
            Dim DAL As New DAL
            DAL.ExecuteQuery(String.Format("DELETE FROM tblCMSReject WHERE FileName='{0}'", strReference))
            DAL.Dispose()
            DAL = Nothing
        Catch ex As Exception
        End Try

        BindGrid()
    End Sub

    Private Sub UploadData(ByVal strReference As String)
        ASPxMemo1.Visible = False
        Dim sbReport As New StringBuilder
        Dim sb As New StringBuilder

        sbReport.AppendLine()
        sbReport.AppendLine("Start of process " & Now.ToString)
        sbReport.AppendLine()
        sbReport.AppendLine("File: " & strReference)

        Dim intCntr As Integer = 0

        Dim DAL As New DAL

        Try
            Dim cmsFile As String = String.Format("{0}\{1}.txt", SharedFunction.ForUploadingCMSRepository, strReference)
            Dim PurchaseOrder As String = ""

            If strReference.Split("_").Length <> 4 Then
                sbReport.AppendLine()
                sbReport.AppendLine("Invalid file naming convention. Uploading process is cancelled.")

                ASPxMemo1.Visible = True
                ASPxMemo1.Text = sbReport.ToString.Replace("<br>", "")
                Return
            Else
                PurchaseOrder = strReference.Split("_")(1).Trim
                If DAL.ExecuteScalar("SELECT COUNT(PurchaseOrder) FROM tblRelCDFRData WHERE PurchaseOrder='" & PurchaseOrder & "'") Then
                    If CInt(DAL.ObjectResult) = 0 Then
                        sbReport.AppendLine()
                        sbReport.AppendLine("No PO " & PurchaseOrder & " found in cdfr table. Uploading process is cancelled.")

                        ASPxMemo1.Visible = True
                        ASPxMemo1.Text = sbReport.ToString.Replace("<br>", "")
                        Return
                    End If
                Else
                    sbReport.AppendLine()
                    sbReport.AppendLine("Failed to validate PO in cdfr table. Uploading process is cancelled. Error " & DAL.ErrorMessage)

                    ASPxMemo1.Visible = True
                    ASPxMemo1.Text = sbReport.ToString.Replace("<br>", "")
                    Return
                End If
            End If

            Dim intCMSRejectID As Integer = 0


            If DAL.AddCMSReject(strReference) Then
                intCMSRejectID = DAL.ObjectResult

                For Each strLine As String In IO.File.ReadAllLines(cmsFile)
                    If strLine.Trim <> "" Then
                        Dim arr() As String = strLine.Split(",")
                        If Not DAL.AddRelCMSRejectData(intCMSRejectID, arr(0), arr(1), arr(2)) Then
                            sb.AppendLine("Failed to insert " & strLine & ". Uploading process is cancelled. Error " & DAL.ErrorMessage)
                            Return
                        Else
                            intCntr += 1
                        End If
                    End If
                Next

                For Each strLine As String In IO.File.ReadAllLines(cmsFile)
                    If strLine.Trim <> "" Then
                        Dim arr() As String = strLine.Split(",")
                        If Not DAL.ResetRelCDFRDataByPOAndBarcode(arr(0), PurchaseOrder) Then
                            sb.AppendLine("Failed to reset PO " & PurchaseOrder & " Barcode " & arr(0) & " in cdfr table")
                        End If
                    End If
                Next

                DAL.ExecuteQuery(String.Format("UPDATE tblCMSReject SET Quantity={0} WHERE CMSRejectID={1}", intCntr, intCMSRejectID))
            Else
                sb.AppendLine("Failed to insert filename to cms reject. Uploading process is cancelled.")
                Return
            End If
        Catch ex As Exception
            sb.AppendLine("Runtime error catched " & ex.Message)
        Finally
            If sb.ToString <> "" Then
                sbReport.AppendLine()
                sbReport.AppendLine(sb.ToString)
            End If

            sbReport.AppendLine()
            sbReport.AppendLine("Total uploaded: " & intCntr.ToString("N0"))
            sbReport.AppendLine()
            sbReport.AppendLine("End of process " & Now.ToString)

            DAL.Dispose()
            DAL = Nothing

            ASPxMemo1.Visible = True
            ASPxMemo1.Text = sbReport.ToString.Replace("<br>", "")

            GridDataBind()
        End Try
    End Sub

    Private Sub CDFRvsUMID(ByVal strReference As String)
        ASPxMemo1.Visible = False
        Dim sb As New StringBuilder

        Dim cdfrFile As String = String.Format("{0}\{1}.txt", SharedFunction.ForUploadingRepository, strReference)

        ASPxMemo1.Visible = True
        ASPxMemo1.Text = sb.ToString.Replace("<br>", "")

        GridDataBind()
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL

        Dim dtCMS As New DataTable
        dtCMS.Columns.Add("FileName", GetType(String))
        dtCMS.Columns.Add("Quantity", GetType(String))
        dtCMS.Columns.Add("DateTimePosted", GetType(DateTime))

        For Each strFile In Directory.GetFiles(SharedFunction.ForUploadingCMSRepository)
            Dim cms As String = Path.GetFileNameWithoutExtension(strFile)
            If DAL.ExecuteScalar("SELECT COUNT(FileName) FROM tblCMSReject WHERE FileName = '" & cms & "'") Then
                If CInt(DAL.ObjectResult) = 0 Then
                    'If DAL.AddForUpload(cdfr, New FileInfo(strFile).CreationTime, File.ReadAllLines(strFile).Length, "CDFR") Then
                    'End If
                    Dim rw As DataRow = dtCMS.NewRow
                    rw(0) = cms
                    rw(1) = File.ReadAllLines(strFile).Length
                    rw(2) = New FileInfo(strFile).CreationTime
                    dtCMS.Rows.Add(rw)
                End If
            End If
        Next

        xGrid.DataSource = dtCMS

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_ForUploadCMS"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("FOR UPLOAD CMS")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub

End Class
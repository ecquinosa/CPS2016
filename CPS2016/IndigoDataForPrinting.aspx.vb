Public Class IndigoDataForPrinting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

            If Not Session("DataForPrinting_POID") Is Nothing Then GridDataBind()
        End If
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectReprocessAndNewUploadPO_Details(Session("DataForPrinting_POID").ToString) Then
            Dim dt As DataTable = DAL.TableResult

            SharedFunction.ReComputePageAndSeries(dt, CInt(Session("Printable")))

            'dt.Columns.Add("SheetBarcode", Type.GetType("System.String"))

            'GenerateSheetBarcode(CInt(Session("Printable")), dt)

            xGrid.DataSource = dt
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GenerateSheetBarcode(ByVal intPrintable As Integer, ByRef dt As DataTable)
        Dim IsGetCRN1 As Boolean = True
        Dim IsGetCRN2 As Boolean = True

        Dim intSeriesCntr As Integer = 1

        Dim strCRN1 As String = ""
        Dim strCRN2 As String = ""

        For Each rw As DataRow In dt.Rows
            If rw("CurrentSeries") > intPrintable Then
                strCRN2 = rw("CRN").ToString.Trim

                If strCRN1 <> "" And strCRN2 <> "" Then
                    Dim strBarcode As String = strCRN1 & strCRN2
                    dt.Select("CRN='" & strCRN1 & "'")(0)("SheetBarcode") = strBarcode.Replace("-", "")
                    strCRN1 = ""
                    strCRN2 = ""
                End If

                Exit For
            Else
                If intSeriesCntr = 1 Then
                    strCRN1 = rw("CRN").ToString.Trim
                    intSeriesCntr += 1
                ElseIf intSeriesCntr = 21 And rw("CurrentSeries") <> intPrintable Then
                    strCRN2 = rw("CRN").ToString.Trim
                    intSeriesCntr = 1
                ElseIf rw("CurrentSeries") = intPrintable Then
                    strCRN2 = rw("CRN").ToString.Trim
                    intSeriesCntr = 1
                Else
                    intSeriesCntr += 1
                End If

                If strCRN1 <> "" And strCRN2 <> "" Then
                    Dim strBarcode As String = strCRN1 & strCRN2
                    dt.Select("CRN='" & strCRN1 & "'")(0)("SheetBarcode") = strBarcode.Replace("-", "")
                    strCRN1 = ""
                    strCRN2 = ""
                End If
            End If
        Next
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub xGrid_HtmlDataCellPrepared(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs) Handles xGrid.HtmlDataCellPrepared
        If e.DataColumn.FieldName = "Type" Then
            If e.CellValue.ToString.Trim.Contains("drop") Then
                e.Cell.ForeColor = Drawing.Color.OrangeRed
            End If
        End If
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

    Private Function ReportName() As String
        Return "CPS_IndigoDataForPrinting"
    End Function

    Private Function ReportHeader() As String
        Dim sb As New StringBuilder
        sb.AppendLine("ALLCARD CARD PRODUCTION SYSTEM")
        sb.AppendLine("INDIGO DATA FOR PRINTING")
        sb.AppendLine(String.Format("As of {0}", Now.ToString))

        Return sb.ToString
    End Function

    Protected Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        xGridExporter.FileName = ReportName()
        xGridExporter.PageHeader.Left = ReportHeader()

        If xGrid.VisibleRowCount > 0 Then xGridExporter.WriteXlsxToResponse(True)
    End Sub
 
End Class
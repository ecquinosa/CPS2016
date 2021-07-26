
Public Class NewPOMatProcess
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Processed_Inv) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            GridDataBind_Material()
            GridDataBind_ForAllotment()

            If Not Session("POMaterialID") Is Nothing Then
                btnSubmit.Visible = False
                Label2.Text = "Materials Used"
                Label3.Text = "Purchase Order(s)"
                xGridMaterial.Columns(3).Visible = False
                xGridForAllotment.Columns(0).Visible = False

                If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.SystemAdmin, DataKeysEnum.RoleID.InventoryAdmin, DataKeysEnum.RoleID.InventorySpoiledEditor) Then
                Else
                    xGridMaterial.Columns(0).Visible = False
                End If
            End If
        End If
    End Sub

    Private Function IsAccessible() As Boolean
        If SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.InventoryAdmin) Then
        ElseIf SharedFunction.IsHaveAccessToRole(SharedFunction.RoleID(Page.User.Identity.Name), DataKeysEnum.RoleID.InventorySpoiledEditor) Then

        End If
    End Function

    Private Function GetSelectedItems_POID() As String
        Dim sb As New StringBuilder

        For i As Short = 0 To xGridForAllotment.GetSelectedFieldValues("POID").Count - 1
            If sb.ToString = "" Then
                sb.Append(String.Format("{0}", xGridForAllotment.GetSelectedFieldValues("POID")(i)))
            Else
                sb.Append(String.Format(",{0}", xGridForAllotment.GetSelectedFieldValues("POID")(i)))
            End If
        Next

        Return sb.ToString
    End Function

    Private Sub xGridMaterial_DataBinding(sender As Object, e As System.EventArgs) Handles xGridMaterial.DataBinding
        BindGrid_Material()
    End Sub

    Private Sub BindGrid_Material()
        Dim DAL As New DAL
        If Session("POMaterialID") Is Nothing Then
            If Session("dtGridMaterial") Is Nothing Then
                If DAL.SelectMaterialInventoryForNewProcess() Then
                    xGridMaterial.DataSource = DAL.TableResult
                    Session("dtGridMaterial") = DAL.TableResult
                End If
            Else
                xGridMaterial.DataSource = CType(Session("dtGridMaterial"), DataTable)
            End If
        Else
            If Session("dtGridMaterial") Is Nothing Then
                If DAL.SelectRelPOMatMaterialByPOMaterialID(Session("POMaterialID")) Then
                    xGridMaterial.DataSource = DAL.TableResult
                    Session("dtGridMaterial") = DAL.TableResult
                End If
            Else
                xGridMaterial.DataSource = CType(Session("dtGridMaterial"), DataTable)
            End If
        End if
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub GridDataBind_Material()
        xGridMaterial.DataBind()
        'xGridMaterial.Caption = "LIST OF MATERIALS ::: " & xGridMaterial.VisibleRowCount.ToString & " entries"
    End Sub

    Private Sub xGridMaterial_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs) Handles xGridMaterial.CellEditorInitialize
        If e.Column.FieldName = "MaterialID" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If Not xGridMaterial.IsNewRowEditing Then txt.ReadOnly = True
        ElseIf e.Column.FieldName = "Material" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxButtonEdit = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxButtonEdit)
            If Not xGridMaterial.IsNewRowEditing Then txt.ReadOnly = True
        ElseIf e.Column.FieldName = "FinalEndQty" Then
            Dim txt As DevExpress.Web.ASPxEditors.ASPxTextBox = TryCast(e.Editor, DevExpress.Web.ASPxEditors.ASPxTextBox)
            If Not xGridMaterial.IsNewRowEditing Then txt.ReadOnly = True
        End If
    End Sub

    Private Sub xGridForAllotment_DataBinding(sender As Object, e As System.EventArgs) Handles xGridForAllotment.DataBinding
        BindGrid_ForAllotment()
    End Sub

    Private Sub GridDataBind_ForAllotment()
        xGridForAllotment.DataBind()
    End Sub

    Private Sub BindGrid_ForAllotment()
        Dim DAL As New DAL
        If Session("POMaterialID") Is Nothing Then
            If Session("dtGridForAllotment") Is Nothing Then
                If DAL.SelectPOForAllotment Then
                    xGridForAllotment.DataSource = DAL.TableResult
                    Session("dtGridForAllotment") = DAL.TableResult
                End If
            Else
                xGridForAllotment.DataSource = CType(Session("dtGridForAllotment"), DataTable)
            End If
        Else
            If DAL.SelectRelPOMatPOByPOMaterialID(Session("POMaterialID")) Then
                xGridForAllotment.DataSource = DAL.TableResult

                Dim intTotal As Integer = 0
                For Each rw As DataRow In DAL.TableResult.Rows
                    intTotal += rw("Quantity")
                Next
                Label1.Text = String.Format("Total: {0}", intTotal.ToString)
            End If
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub xGridMaterial_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles xGridMaterial.RowUpdating
        e.Cancel = True
        xGridMaterial.CancelEdit()
        BindGrid_Material()
    End Sub

    Private Sub xGridMaterial_RowValidating(sender As Object, e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles xGridMaterial.RowValidating
        If e.NewValues("AlltdQty") = "0" And Session("IsNewProcess") = 1 Then
            e.RowError = "Please enter Alloted quantity"
            'ElseIf e.NewValues("UsdQty") = "0" And Session("IsNewProcess") Is Nothing Then
            '    e.RowError = "Please enter Used quantity"
            'ElseIf e.NewValues("SpoiledQty") = "0" And Session("IsNewProcess") Is Nothing Then
            '    e.RowError = "Please enter Spoiled quantity"
        ElseIf Session("IsNewProcess") = 1 And CInt(e.OldValues("FinalEndQty")) < CInt(e.NewValues("AlltdQty")) Then
            e.RowError = "Please recheck Alloted vs Balance quantity"
        ElseIf CInt(e.NewValues("AlltdQty")) < (CInt(e.NewValues("UsdQty")) + CInt(e.NewValues("SpoiledQty"))) Then
            e.RowError = "Please recheck Alloted vs Used + Spoiled quantity"
        Else
            Dim dt As DataTable = CType(Session("dtGridMaterial"), DataTable)

            Dim rw As DataRow = dt.Select("MaterialID=" & e.OldValues("MaterialID").ToString)(0)
            rw("AlltdQty") = e.NewValues("AlltdQty").ToString
            rw("UsdQty") = e.NewValues("UsdQty").ToString
            rw("SpoiledQty") = e.NewValues("SpoiledQty").ToString
            rw.AcceptChanges()
            dt.AcceptChanges()
            Session("dtGridMaterial") = dt


            If Not Session("POMaterialID") Is Nothing Then
                Dim DAL As New DAL
                DAL.ExecuteQuery(String.Format("UPDATE tblRelPOMatMaterial SET AlltdQty={0}, UsdQty={1}, SpoiledQty={2} WHERE PMMaterialID={3}", e.NewValues("AlltdQty").ToString, e.NewValues("UsdQty").ToString, e.NewValues("SpoiledQty").ToString, e.OldValues("MaterialID").ToString))
                DAL.Dispose()
                DAL = Nothing
            End If
        End If
    End Sub

    Private Sub Submit()
        Dim DAL As New DAL
        Dim sb As New StringBuilder
        Dim intError As Integer = 0
        Try
            Dim dt As DataTable = CType(Session("dtGridMaterial"), DataTable)

            sb.AppendLine(String.Format("Start of PO Material adding process {0}<br>", Now.ToString))

            Dim intPOMaterialID As Integer = 0
            If DAL.AddPOMaterial(SharedFunction.UserID(Page.User.Identity.Name)) Then
                intPOMaterialID = DAL.ObjectResult

                'add used material
                For Each rw As DataRow In dt.Rows
                    If CInt(rw("AlltdQty")) > 0 Then
                        If Not DAL.AddRelPOMatMaterial(intPOMaterialID, rw("MaterialID"), rw("AlltdQty")) Then
                            intError += 1
                            sb.AppendLine(String.Format("AddRelPOMatMaterial(): Failed to insert Material {0} Alloted qty {1}. Returned error {2}<br>", rw("Material"), rw("AlltdQty"), DAL.ErrorMessage))
                        End If
                    End If
                Next

                'add purchase order for allotment
                For i As Short = 0 To xGridForAllotment.GetSelectedFieldValues("POID").Count - 1
                    If Not DAL.AddRelPOMatPO(intPOMaterialID, xGridForAllotment.GetSelectedFieldValues("PurchaseOrder")(i), xGridForAllotment.GetSelectedFieldValues("Quantity")(i), xGridForAllotment.GetSelectedFieldValues("DateTimePosted")(i)) Then
                        intError += 1
                        sb.AppendLine(String.Format("AddRelPOMatPO(): Failed to insert PO {0}. Returned error {2}<br>", xGridForAllotment.GetSelectedFieldValues("PurchaseOrder")(i), DAL.ErrorMessage))
                    End If
                Next

                btnSubmit.Enabled = False
                lblResult.Text = "Process is done"
                lblResult.ForeColor = SharedFunction.SuccessColor
            Else
                intError += 1
                sb.AppendLine(String.Format("{0} failed to execute AddPOMaterial(). Returned error {1}<br>", SharedFunction.UserCompleteName(Page.User.Identity.Name), DAL.ErrorMessage))

                lblResult.Text = "Failed to execute POMaterial adding"
                lblResult.ForeColor = SharedFunction.ErrorColor
            End If
        Catch ex As Exception
            lblResult.Text = "Failed to save changes"
            lblResult.ForeColor = SharedFunction.ErrorColor
        Finally
            sb.AppendLine(String.Format("Error {0}<br>", intError.ToString) & vbNewLine)
            sb.AppendLine(String.Format("End of PO Material adding process {0}", Now.ToString))

            If intError = 0 Then
                DAL.AddSystemLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            Else
                DAL.AddErrorLog(sb.ToString, SharedFunction.GetCurrentPage(Request.Url.AbsolutePath), SharedFunction.UserID(Page.User.Identity.Name))
            End If

            DAL.Dispose()
            DAL = Nothing
            sb = Nothing

            Label2.Visible = False
            Label3.Visible = False

            GridDataBind_Material()
            GridDataBind_ForAllotment()
        End Try
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        lblResult.Text = ""

        Dim strIDs As String = GetSelectedItems_POID()

        If strIDs = "" Then
            lblResult.Text = "Please select Purchase Order(s)"
            lblResult.ForeColor = SharedFunction.ErrorColor

            'GridDataBind_Material()
            GridDataBind_ForAllotment()
            Exit Sub
        End If

        Dim dt As DataTable = CType(Session("dtGridMaterial"), DataTable)
        Dim IsGridMaterialValid As Boolean = False

        For Each rw As DataRow In dt.Rows
            If CInt(rw("AlltdQty")) > 0 Then
                IsGridMaterialValid = True
                Exit For
            End If
        Next

        If Not IsGridMaterialValid Then
            lblResult.Text = "No alloted material(s) detected"
            lblResult.ForeColor = SharedFunction.ErrorColor

            GridDataBind_Material()

            Exit Sub
        End If

        Session("IsNewProcess") = Nothing
        Submit()
    End Sub

End Class
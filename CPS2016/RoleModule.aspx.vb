Public Class RoleModule
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblRole.Text = "Role: " & Session("RoleDesc").ToString

            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblModule WHERE IsDeleted = 0") Then
            xGrid.DataSource = DAL.TableResult
        End If
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF MODULES ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Protected Sub xGrid_DataBound(sender As Object, e As EventArgs) Handles xGrid.DataBound
        Dim DAL As New DAL
        Dim dt As DataTable
        If DAL.SelectQuery("SELECT ModuleID FROM tblRoleModule WHERE RoleID=" & Session("RoleID").ToString) Then
            dt = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing

        Dim grid As DevExpress.Web.ASPxGridView.ASPxGridView = TryCast(sender, DevExpress.Web.ASPxGridView.ASPxGridView)
        For i As Integer = 0 To grid.VisibleRowCount - 1
            If dt.Select("ModuleID=" & xGrid.GetRowValues(i, "ModuleID")).Length > 0 Then
                grid.Selection.SelectRow(i)
            End If
        Next i
    End Sub

    Private Sub SaveChanges()
        Try
            If xGrid.GetSelectedFieldValues("ModuleID").Count > 0 Then
                Dim DAL As New DAL
                DAL.ExecuteQuery(String.Format("DELETE FROM tblRoleModule WHERE RoleID={0}", Session("RoleID").ToString))

                For i As Short = 0 To xGrid.GetSelectedFieldValues("RoleID").Count - 1
                    DAL.ExecuteQuery(String.Format("INSERT INTO tblRoleModule (RoleID, ModuleID, DateTimePosted) VALUES ({0},{1},GETDATE())", Session("RoleID").ToString, xGrid.GetSelectedFieldValues("ModuleID")(i)))
                Next

                DAL.Dispose()
                DAL = Nothing

                lblResult.Text = "Changes has been saved"
                lblResult.ForeColor = SharedFunction.SuccessColor

            Else
                lblResult.Text = "No selected item to save"
                lblResult.ForeColor = SharedFunction.ErrorColor
            End If

            GridDataBind()
        Catch ex As Exception
            lblResult.Text = "Failed to save changes"
            lblResult.ForeColor = SharedFunction.ErrorColor
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        lblResult.Text = ""

        SaveChanges()
    End Sub

End Class
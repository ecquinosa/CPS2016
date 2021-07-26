Public Class UserRole
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.User) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            lblUser.Text = "User: " & Session("Name").ToString

            GridDataBind()
        End If
    End Sub

    Private Sub xGrid_DataBinding(sender As Object, e As System.EventArgs) Handles xGrid.DataBinding
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim DAL As New DAL
        If DAL.SelectQuery("SELECT * FROM tblRole WHERE IsDeleted = 0") Then
            xGrid.DataSource = DAL.TableResult
        End If
    End Sub

    Private Sub GridDataBind()
        xGrid.DataBind()
        xGrid.Caption = "LIST OF ROLES ::: " & xGrid.VisibleRowCount.ToString & " entries"
    End Sub

    Protected Sub xGrid_DataBound(sender As Object, e As EventArgs) Handles xGrid.DataBound
        Dim DAL As New DAL
        Dim dt As DataTable
        If DAL.SelectQuery("SELECT RoleID FROM tblUserRole WHERE UserID=" & Session("UserID").ToString) Then
            dt = DAL.TableResult
        End If
        DAL.Dispose()
        DAL = Nothing

        Dim grid As DevExpress.Web.ASPxGridView.ASPxGridView = TryCast(sender, DevExpress.Web.ASPxGridView.ASPxGridView)
        For i As Integer = 0 To grid.VisibleRowCount - 1
            If dt.Select("RoleID=" & xGrid.GetRowValues(i, "RoleID")).Length > 0 Then
                grid.Selection.SelectRow(i)
            End If
        Next i
    End Sub

    Private Sub SaveChanges()
        Try
            If xGrid.GetSelectedFieldValues("RoleID").Count > 0 Then
                Dim DAL As New DAL
                DAL.ExecuteQuery(String.Format("DELETE FROM tblUserRole WHERE UserID={0}", Session("UserID").ToString))

                For i As Short = 0 To xGrid.GetSelectedFieldValues("RoleID").Count - 1
                    DAL.ExecuteQuery(String.Format("INSERT INTO tblUserRole (UserID, RoleID, DateTimePosted) VALUES ({0},{1},GETDATE())", Session("UserID").ToString, xGrid.GetSelectedFieldValues("RoleID")(i)))
                Next

                DAL.Dispose()
                DAL = Nothing

                lblResult.Text = "Changes has been saved"
                lblResult.ForeColor = SharedFunction.SuccessColor

                GridDataBind()
            End If
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
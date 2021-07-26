
Public Class TrackData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.Search_Track) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()
        End If
    End Sub

    Private Sub Search()
        Select Case cboSearch.Text
            Case "Purchase Order"
                xGridPO.Visible = True
                xGridBarcodeCRN.Visible = False
                xGridBarcodeCRN.DataSource = Nothing

                GridDataBind_PO()
            Case "Barcode", "CRN"
                xGridPO.Visible = False
                xGridPO.DataSource = Nothing
                xGridBarcodeCRN.Visible = True

                GridDataBind_BarcodeCRN()
        End Select
    End Sub

    Protected Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        lblStatus.Text = ""

        If txtValue.Text = "" Then
            lblStatus.Text = "Please enter value to search"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            xGridPO.Visible = False
            xGridBarcodeCRN.Visible = False
        Else
            lblStatus.ForeColor = SharedFunction.ErrorColor

            Select Case cboSearch.Text
                Case "Purchase Order"
                    If Not txtValue.Text.Contains("-") Then
                        lblStatus.Text = "Invalid Purchase Order format"
                        Exit Sub
                    End If
                Case "Barcode"
                    If txtValue.Text.Length <> 20 Then
                        lblStatus.Text = "Invalid Barcode data length"
                        Exit Sub
                    End If
                Case "CRN"
                    If txtValue.Text.Length <> 14 Then
                        lblStatus.Text = "Invalid CRN data length"
                        Exit Sub
                    ElseIf txtValue.Text.Split("-").Length <> 3 Then
                        lblStatus.Text = "Invalid CRN format"
                        Exit Sub
                    End If
            End Select

            Search()
        End If
    End Sub

    Private Sub xGridBarcodeCRN_DataBinding(sender As Object, e As System.EventArgs) Handles xGridBarcodeCRN.DataBinding
        BindGrid_BarcodeCRN()
    End Sub

    Private Sub xGridPO_DataBinding(sender As Object, e As System.EventArgs) Handles xGridPO.DataBinding
        BindGrid_PO()
    End Sub

    Private Sub BindGrid_PO()
        Dim DAL As New DAL

        If DAL.TrackData(cboSearch.Text.Trim, txtValue.Text) Then
            xGridPO.DataSource = DAL.TableResult
        End If

        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub BindGrid_BarcodeCRN()
         Dim DAL As New DAL

        If DAL.TrackData(cboSearch.Text.Trim, txtValue.Text) Then
            xGridBarcodeCRN.DataSource = DAL.TableResult
        End If

        DAL.Dispose()
        DAL = Nothing

        txtValue.Text = ""
        txtValue.Focus()
    End Sub

    Private Sub GridDataBind_PO()
        xGridPO.DataBind()
    End Sub

    Private Sub GridDataBind_BarcodeCRN()
        xGridBarcodeCRN.DataBind()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

End Class
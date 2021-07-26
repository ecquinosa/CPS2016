
Imports System.IO

Public Class CardControl
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.User.Identity.Name = "" Then
            Session.RemoveAll()
            Response.Redirect("Login.aspx?ErrMsg=Session expired")
        End If

        If Not SharedFunction.IsHaveAccessToPage(SharedFunction.RoleModules(Page.User.Identity.Name), DataKeysEnum.ModuleID.CardControl) Then
            Session("PageMsg") = "You have no access to page!"
            Server.Transfer("MsgPage.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("FromNavMenu") = 1 Then Session.RemoveAll()

            ResetPage()
        End If
    End Sub

    Private Sub ResetPage()
        Dim strSessions As String = "dtDV,IsAddToDV"
        For Each strSession As String In strSessions.Split(",")
            Session.Remove(strSession)
        Next

        CreateTable()
    End Sub

   Private Sub DVDataBind()
        xDataView.DataBind()
    End Sub

    Private Sub CreateTable()
        Dim DAL As New DAL
        If DAL.SelectRelPODataByBarcode_CardControlv2("12345678901234567890") Then
            If Session("dtDV") Is Nothing Then Session("dtDV") = DAL.TableResult.Clone
        End If
        DAL.Dispose()
    End Sub

    Public Function GetFullName(ByVal strFirst As String, ByVal objMiddle As Object, ByVal strLast As String) As String
        Return String.Format("{0} {1}{2}", strFirst, IIf(IsDBNull(objMiddle), "", objMiddle.ToString & " "), strLast)
    End Function

    Public Function VerifyBarcode(ByVal Barcode As String) As String
        'If Not Barcode.ToUpper.Contains("C01") Then
        If Not SharedFunction.IsCubaoData(Barcode) Then
            Return Barcode
        Else
            Return String.Format("{0} (CUBAO BRANCH)", Barcode)
        End If
    End Function

    Public Function CubaoBranchTag(ByVal Barcode As String) As String
        'If Not Barcode.ToUpper.Contains("C01") Then
        If Not SharedFunction.IsCubaoData(Barcode) Then
            Return ""
        Else
            Return " (CUBAO BRANCH)"
        End If
    End Function

    Public Function GetPhotoImage(ByVal strBarcode As String) As Byte()
        'Return File.ReadAllBytes(SharedFunction.GetFrom_PSBImages(String.Format("{0}_Photo.jpg", strBarcode)))
        Return SharedFunction.GetPhotoImage(strBarcode)
    End Function

    Public Function GetSignatureImage(ByVal strBarcode As String) As Byte()
        'Return File.ReadAllBytes(SharedFunction.GetFrom_PSBImages(String.Format("{0}_Signature.jpg", strBarcode)))
        Return SharedFunction.GetSignatureImage(strBarcode)
    End Function

    Public Function TIFFtoJPG(ByVal img() As Byte) As Byte()
        Try
            Dim ms As New MemoryStream()

            Using image As New System.Drawing.Bitmap(New MemoryStream(img))
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            End Using

            Return ms.ToArray
        Catch ex As Exception
            Return New MemoryStream().ToArray
        End Try
    End Function

    Public Function GetActivityDesc(ByVal intActivity As Integer) As String
        Return SharedFunction.GetActivityDesc(intActivity)
    End Function

    Private Sub SelectRelPODataByByCRNorBarcode()
        Dim DAL As New DAL
        If DAL.SelectRelPODataByByCRNorBarcode_CardControl(txtParam.Text) Then
            If DAL.TableResult.DefaultView.Count > 0 Then
                If Not Session("dtDV") Is Nothing Then
                    Dim dt As DataTable = CType(Session("dtDV"), DataTable)

                    AddRowToDT(DAL.TableResult.Rows(0), dt)

                    If dt.DefaultView.Count = 3 Then dt.Rows(dt.DefaultView.Count - 1).Delete()

                    Session("dtDV") = dt
                    xDataView.DataSource = dt
                End If
            Else
                lblStatus.Text = "No data found"
                lblStatus.ForeColor = SharedFunction.ErrorColor

                RefreshDV()
            End If
        End If
        DAL.Dispose()
        DAL = Nothing
    End Sub

    Private Sub AddRowToDT(ByVal rwSource As DataRow, ByRef dtGrid As DataTable)
        Dim rw As DataRow = dtGrid.NewRow
        For i = 0 To dtGrid.Columns.Count - 1
            rw(i) = rwSource(i)
        Next
        dtGrid.Rows.InsertAt(rw, 0)
    End Sub

    Private Sub RefreshDV()
        xDataView.DataSource = CType(Session("dtDV"), DataTable)
    End Sub

    Private Sub BindDV()
        If txtParam.Text = "" Then
            lblStatus.Text = "Please enter barcode or crn"
            lblStatus.ForeColor = SharedFunction.ErrorColor
            RefreshDV()
        Else
            SelectRelPODataByByCRNorBarcode()
        End If
    End Sub

    Private Sub xDataView_DataBinding(sender As Object, e As System.EventArgs) Handles xDataView.DataBinding
        BindDV()
    End Sub

    Protected Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        'Session("IsAddToDV") = 1
        DVDataBind()

        txtParam.Text = ""
        txtParam.Focus()
    End Sub

    Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect(String.Format("{0}?FromNavMenu=1", Request.Url.AbsolutePath))
    End Sub

End Class
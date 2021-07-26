
Public Class MsgPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not ("PageMsg") Is Nothing Then
                Label1.Text = Session("PageMsg").ToString
            End If
        End If
    End Sub

End Class
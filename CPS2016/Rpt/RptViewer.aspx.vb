
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.CrystalReports.Engine

Public Class RptViewer
    Inherits System.Web.UI.Page

    Private Sub RptViewer_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
        If Not Session("pdfFile") Is Nothing Then _
           If IO.File.Exists(Session("pdfFile").ToString) Then IO.File.Delete(Session("pdfFile").ToString)
    End Sub

    'Private crReportDocument As New ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("pdfFile") Is Nothing Then
                Dim filePath As String = Session("pdfFile").ToString
                Response.ContentType = "application/pdf"
                Response.WriteFile(filePath)
            End If
        End If
    End Sub

End Class
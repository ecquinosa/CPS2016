
Imports System.IO

Public Class CMS_Report

    Private ReadOnly CSN_Length As Integer = 20
    Private ReadOnly PRINTORDER_Length As Integer = 33
    Private ReadOnly CRN_Length As Integer = 12
    Private ReadOnly FIRSTNAME_Length As Integer = 40
    Private ReadOnly MIDDLENAME_Length As Integer = 40
    Private ReadOnly LASTNAME_Length As Integer = 40
    Private ReadOnly SUFFIX_Length As Integer = 10
    Private ReadOnly ADDRESS1_Length As Integer = 40
    Private ReadOnly ADDRESS2_Length As Integer = 15
    Private ReadOnly ADDRESS3_Length As Integer = 40
    Private ReadOnly ADDRESS4_Length As Integer = 40
    Private ReadOnly ADDRESS5_Length As Integer = 30
    Private ReadOnly ADDRESS6_Length As Integer = 30
    Private ReadOnly ADDRESS7_Length As Integer = 30
    Private ReadOnly COUNTRY_Length As Integer = 3
    Private ReadOnly POSTAL_Length As Integer = 6

    Private strErrorMessage As String

    Public ReadOnly Property ErrorMessage As String
        Get
            Return strErrorMessage
        End Get
    End Property

    Public Function GenerateReportEorReportG(ByVal strPO As String, ByVal ReportName As String, ByRef outputFile As String) As Boolean
        Try
            Dim strDateDelivered As String
            Dim dt As DataTable
            Dim DAL As New DAL
            If DAL.ReportE(strPO) Then
                dt = DAL.TableResult
            End If
            'If DAL.ExecuteScalar("SELECT DateDel FROM dbo.temp1 WHERE (PO = '" & strPO.Trim & "')") Then
            '    strDateDelivered = DAL.ObjectResult.ToString
            'End If

            DAL.Dispose()
            DAL = Nothing

            If Not dt Is Nothing Then
                Dim sb As New System.Text.StringBuilder
                For Each rw As DataRow In dt.Rows
                    Dim address() As String = rw("AddressDelimited").ToString.Split({"||"}, System.StringSplitOptions.None)

                    sb.AppendLine(FormatValue(rw("Barcode"), CSN_Length) & _
                                  FormatValue(rw("PurchaseOrder"), PRINTORDER_Length) & _
                                  FormatValue(rw("Barcode"), CSN_Length) & _
                                  FormatValue(rw("CRN"), CRN_Length) & _
                                  FormatValue(rw("FName"), FIRSTNAME_Length) & _
                                  FormatValue(rw("MName"), MIDDLENAME_Length) & _
                                  FormatValue(rw("LName"), LASTNAME_Length) & _
                                  FormatValue(rw("Suffix"), SUFFIX_Length) & _
                                  FormatValue(address(0), ADDRESS1_Length) & _
                                  FormatValue(address(1), ADDRESS2_Length) & _
                                  FormatValue(address(2), ADDRESS3_Length) & _
                                  FormatValue(address(3), ADDRESS4_Length) & _
                                  FormatValue(address(4), ADDRESS5_Length) & _
                                  FormatValue(address(5), ADDRESS6_Length) & _
                                  FormatValue(address(6), ADDRESS7_Length) & _
                                  FormatValue(address(7), COUNTRY_Length) & _
                                  FormatValue(address(8), POSTAL_Length) & _
                                  Now.ToString("yyyyMMdd"))
                    'CDate(strDateDelivered).ToString("yyyyMMdd"))
                Next

                'Now.ToString("yyyyMMdd"))

                Dim path As String = String.Format("{0}\{1}", SharedFunction.ReportsRepository, strPO)
                outputFile = String.Format("{0}\{1}_{2}.txt", path, strPO, ReportName)
                If Not IO.Directory.Exists(path) Then IO.Directory.CreateDirectory(path)
                IO.File.WriteAllText(outputFile, sb.ToString)

                Return True
            Else
                strErrorMessage = "Table is nothing"
                Return False
            End If
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Private Function FormatValue(ByVal value As Object, ByVal intLength As Short) As String
        If IsDBNull(value) Then
            Return "".PadRight(intLength, " ")
        ElseIf IsNothing(value) Then
            Return "".PadRight(intLength, " ")
        Else
            Return value.ToString.Trim.PadRight(intLength, " ")
        End If
    End Function


End Class

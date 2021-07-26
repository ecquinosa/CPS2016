
Public Class CDFR_Reader

    Private cdfrFile As String
    'Private _errMsg As String
    Private dt As DataTable

    Private _newCntr As Integer
    Private _replacementCntr As Integer
    Private _renewalCntr As Integer

    Public ReadOnly Property NewCounter As Integer
        Get
            Return _newCntr
        End Get
    End Property

    Public ReadOnly Property ErrorMessage As String
        Get
            Return sb.ToString
        End Get
    End Property

    Public ReadOnly Property ReplacementCounter As Integer
        Get
            Return _replacementCntr
        End Get
    End Property

    Public ReadOnly Property RenewalCounter As Integer
        Get
            Return _renewalCntr
        End Get
    End Property

    Public ReadOnly Property CDFR_Table As DataTable
        Get
            Return dt
        End Get
    End Property

    Public Sub New(ByVal cdfrFile As String)
        Me.cdfrFile = cdfrFile
    End Sub

    Private Sub CreateTable()
        If dt Is Nothing Then
            dt = New DataTable
            dt.Columns.Add("RefNo", Type.GetType("System.String"))
            dt.Columns.Add("CRN", Type.GetType("System.String"))
            dt.Columns.Add("GSISNo", Type.GetType("System.String"))
            dt.Columns.Add("BPNo", Type.GetType("System.String"))
            dt.Columns.Add("SSSNo", Type.GetType("System.String"))
            dt.Columns.Add("Barcode", Type.GetType("System.String"))
            dt.Columns.Add("ACNo", Type.GetType("System.String"))
            dt.Columns.Add("CNo", Type.GetType("System.String"))
            dt.Columns.Add("ExpDate", Type.GetType("System.String"))
            dt.Columns.Add("OrigData", Type.GetType("System.String"))
        Else
            dt.Clear()
        End If
    End Sub

    Dim sb As New StringBuilder

    Private Function CheckIfHasAlphabet(ByVal value As String) As Boolean
        If value.ToUpper = value.ToLower Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function Read() As Boolean
        Try
            CreateTable()

            Dim strLines() As String = System.IO.File.ReadAllLines(cdfrFile)
            For Each strLine As String In strLines
                If strLine.Trim <> "" Then
                    If strLine.Trim.Length = 618 Then
                        Dim CRN As String = strLine.Substring(0, 12).Trim
                        Dim ACNo As String = strLine.Substring(158, 12).Trim
                        Dim CNo As String = strLine.Substring(181, 20).Trim
                        Dim ExpDate As String = strLine.Substring(477, 5).Trim
                        Dim SSSNo As String = strLine.Substring(253, 10).Trim
                        Dim Barcode As String = strLine.Substring(274, 20).Trim

                        If ACNo = "" Then
                            sb.Append(String.Format("ACNo {0}, Empty", ACNo) & vbNewLine)
                        ElseIf CheckIfHasAlphabet(ACNo) Then
                            sb.Append(String.Format("ACNo {0}, Has alphabet", ACNo) & vbNewLine)
                        ElseIf CNo = "" Then
                            sb.Append(String.Format("CNo {0}, Empty", CNo) & vbNewLine)
                        ElseIf CheckIfHasAlphabet(CNo) Then
                            sb.Append(String.Format("CNo {0}, Has alphabet", CNo) & vbNewLine)
                        ElseIf CRN = "" Then
                            sb.Append(String.Format("CNo {0}, Empty", CRN) & vbNewLine)
                        ElseIf CheckIfHasAlphabet(CRN) Then
                            sb.Append(String.Format("CRN {0}, Has alphabet", CRN) & vbNewLine)
                        ElseIf ExpDate = "" Then
                            sb.Append(String.Format("ExpDate {0}, Empty", ExpDate) & vbNewLine)
                            'If strLine.Substring(253, 11).Trim = "" Then
                        ElseIf SSSNo = "" Then
                            'sb.Append(String.Format("GSISNo {0}, Empty", strLine.Trim.Substring(253, 11)) & vbNewLine)
                            sb.Append(String.Format("SSSNo {0}, Empty", SSSNo) & vbNewLine)
                            'ElseIf strLine.Substring(264, 10).Trim = "" Then
                        ElseIf CheckIfHasAlphabet(SSSNo) Then
                            sb.Append(String.Format("SSSNo {0}, Has alphabet", SSSNo) & vbNewLine)
                        ElseIf Barcode = "" Then
                            sb.Append(String.Format("Barcode {0}, Empty", Barcode) & vbNewLine)
                        Else
                            AddRow(strLine)
                        End If
                    Else
                        sb.Append(String.Format("CRN {0}, Invalid line format", strLine.Trim.Substring(0, 12)) & vbNewLine)
                    End If
                End If
            Next

            '_newCntr = dt.Select("GenType='811'").Length
            '_replacementCntr = dt.Select("GenType='812'").Length
            '_renewalCntr = dt.Select("GenType='813'").Length

            Return True
        Catch ex As Exception
            sb.Append(ex.Message & vbNewLine)
            Return False
        End Try
    End Function

    Private Sub AddRow(ByVal line As String)
        Dim rw As DataRow = dt.NewRow
        rw("RefNo") = line.Substring(0, 20)

        Dim CRN As String = ""
        If rw("RefNo").ToString.Trim.Contains("GS") Then
            CRN = rw("RefNo").Substring(8, 12)
        Else
            CRN = rw("RefNo").Substring(0, 12)
        End If

        If CRN.Trim.Length = 12 Then
            ''gsis format
            'CRN = CRN.Substring(0, 3) + "-" +
            '      CRN.Substring(3, 4) + "-" +
            '      CRN.Substring(7, 4) + "-" +
            '      CRN.Substring(11, 1)

            'sss format
            CRN = CRN.Substring(0, 4) + "-" +
                  CRN.Substring(4, 7) + "-" +
                  CRN.Substring(11, 1)
        End If

        rw("CRN") = CRN
        'rw("Name1") = line.Substring(20, 40)
        'rw("Name2") = line.Substring(60, 40)
        'rw("Name3") = line.Substring(100, 40)
        'rw("Name4") = line.Substring(140, 10)
        'rw("BDay") = line.Substring(150, 8)
        rw("ACNo") = line.Substring(158, 12)
        'rw("CType") = line.Substring(170, 3)
        'rw("CGroup") = line.Substring(173, 5)
        'rw("GenType") = line.Substring(178, 3)
        rw("CNo") = line.Substring(181, 20)
        'rw("FreeText1") = line.Substring(201, 50)
        'rw("FreeText2") = line.Substring(251, 50)
        'rw("FreeText3") = line.Substring(301, 50)
        'rw("FreeText4") = line.Substring(351, 50)
        'rw("FreeText5") = line.Substring(401, 50)
        'rw("CardPrintData") = line.Substring(451, 50)

        rw("GSISNo") = "" 'line.Substring(253, 11)
        rw("BPNo") = "" 'line.Substring(264, 10)
        rw("SSSNo") = line.Substring(253, 10)
        rw("Barcode") = line.Substring(274, 20)
        rw("ExpDate") = line.Substring(477, 5)
        'rw("Track1") = "B" & line.Substring(503, 75).Replace("L", "^").Replace("M", "^")
        'rw("Track2") = String.Format("{0}={1}", line.Substring(503, 16), line.Substring(597, 20))
        rw("OrigData") = line
        dt.Rows.Add(rw)
    End Sub

    Private Function ValidateValue(ByVal field As String) As Boolean
        If field.Trim = "" Then
            Return False
        Else
            Return True
        End If
    End Function

End Class

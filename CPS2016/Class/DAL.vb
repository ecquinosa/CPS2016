
Imports System.Data.SqlClient

Public Class DAL
    Implements IDisposable
    
    'Private ConStr As String = "Server=localhost;Database=dbcCPS2016;User=sa;Password=acc2015;"
    Public Shared ConStr As String = "Server=" & My.Settings.Server & ";Database=" & My.Settings.Database & ";User=" & My.Settings.User & ";Password=" & My.Settings.Password & ";"

    Private dtResult As DataTable
    Private dsResult As DataSet
    Private objResult As Object
    Private _readerResult As IDataReader
    Private strErrorMessage As String

    Private con As SqlConnection
    Private cmd As SqlCommand
    Private da As SqlDataAdapter
    Private _UserID As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal _UserID As String)
        Me._UserID = _UserID
    End Sub

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return strErrorMessage
        End Get
    End Property

    Public ReadOnly Property TableResult() As DataTable
        Get
            Return dtResult
        End Get
    End Property

    Public ReadOnly Property DatasetResult() As DataSet
        Get
            Return dsResult
        End Get
    End Property

    Public ReadOnly Property ObjectResult() As Object
        Get
            Return objResult
        End Get
    End Property

    Public ReadOnly Property ReaderResult() As IDataReader
        Get
            Return _readerResult
        End Get
    End Property

    Public Sub ClearAllPools()
        SqlConnection.ClearAllPools()
    End Sub

    Private Sub OpenConnection()
        If con Is Nothing Then con = New SqlConnection(ConStr)
    End Sub

    Private Sub CloseConnection()
        If Not cmd Is Nothing Then cmd.Dispose()
        If Not da Is Nothing Then da.Dispose()
        If Not _readerResult Is Nothing Then
            _readerResult.Close()
            _readerResult.Dispose()
        End If
        If Not con Is Nothing Then If con.State = ConnectionState.Open Then con.Close()
        ClearAllPools()
    End Sub

    Private Sub ExecuteNonQuery(ByVal cmdType As CommandType)
        cmd.CommandType = cmdType

        'If con.State = ConnectionState.Open Then con.Close()
        'con.Open()
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub

    Private Sub _ExecuteScalar(ByVal cmdType As CommandType)
        cmd.CommandType = cmdType

        'If con.State = ConnectionState.Open Then con.Close()
        'con.Open()
        If con.State = ConnectionState.Closed Then con.Open()
        Dim _obj As Object
        _obj = cmd.ExecuteScalar()
        con.Close()

        objResult = _obj
    End Sub

    Private Sub _ExecuteReader(ByVal cmdType As CommandType)
        cmd.CommandType = cmdType

        'If con.State = ConnectionState.Open Then con.Close()
        'con.Open()
        If con.State = ConnectionState.Closed Then con.Open()
        Dim reader As SqlDataReader = cmd.ExecuteReader

        _readerResult = reader
    End Sub

    Private Sub FillDataAdapter(ByVal cmdType As CommandType)
        cmd.CommandTimeout = 0
        cmd.CommandType = cmdType
        da = New SqlDataAdapter(cmd)
        Dim _dt As New DataTable
        da.Fill(_dt)
        dtResult = _dt
    End Sub

    Public Function IsConnectionOK(Optional ByVal strConString As String = "") As Boolean
        Try
            If strConString <> "" Then ConStr = strConString
            OpenConnection()

            con.Open()
            con.Close()

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ExecuteQuery(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddSSSReject(ByVal strBarcode As Object, ByVal strCRN As Object, ByVal strOld_PO As Object, _
                                    ByVal strTag As String, ByVal intRejectTypeID As Object, ByVal intUserID As Integer, _
                                    ByVal dtmBatchDateTimePosted As Date) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddSSSReject", con)

            cmd.Parameters.AddWithValue("Barcode", strBarcode)
            cmd.Parameters.AddWithValue("CRN", strCRN)
            cmd.Parameters.AddWithValue("Old_PO", strOld_PO)
            cmd.Parameters.AddWithValue("Tag", strTag)
            cmd.Parameters.AddWithValue("RejectTypeID", intRejectTypeID)
            cmd.Parameters.AddWithValue("UserID", intUserID)
            cmd.Parameters.AddWithValue("BatchDateTimePosted", dtmBatchDateTimePosted)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateRelPOReportByPOReportID(ByVal intPOReportID As Integer, _
                                                         ByVal byteReportImage() As Byte) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateRelPOReportByPOReportID", con)

            cmd.Parameters.AddWithValue("POReportID", intPOReportID)
            cmd.Parameters.AddWithValue("ReportImage", byteReportImage)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateRelPOReportByPOReportIDv2(ByVal intPOReportID As Integer, _
                                                         ByVal byteReportImage() As Byte, ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateRelPOReportByPOReportIDv2", con)

            cmd.Parameters.AddWithValue("POReportID", intPOReportID)
            cmd.Parameters.AddWithValue("ReportImage", byteReportImage)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateRelPOReportByPOAndReportTypeID(ByVal strPurchaseOrder As String, _
                                                         ByVal intReportTypeID As Integer, _
                                                         ByVal byteReportImage() As Byte) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateRelPOReportByPOAndReportTypeID", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("ReportTypeID", intReportTypeID)
            cmd.Parameters.AddWithValue("ReportImage", byteReportImage)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddPO(ByVal strPurchaseOrder As String, ByVal intQuantity As Integer, ByVal strBatch As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddPO", con)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("Quantity", intQuantity)
            cmd.Parameters.AddWithValue("Batch", strBatch)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddCDFR(ByVal GSUfilename As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddCDFR", con)
            cmd.Parameters.AddWithValue("GSUfilename", GSUfilename)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddCMSReject(ByVal filename As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddCMSReject", con)
            cmd.Parameters.AddWithValue("Filename", filename)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelPOData(ByVal intPOID As Integer, ByVal strCRN As String, ByVal strBarcode As String,
                                 ByVal strFName As String, ByVal strMName As String, ByVal strLName As String, ByVal strSuffix As String,
                                 ByVal strSex As String, ByVal strDateOfBirth As String, ByVal strAddress As String,
                                 ByVal intStatusID As Short, ByVal strBackOCR As String, ByVal intCurrentPage As Integer,
                                 ByVal intCurrentSeries As Integer, ByVal strPOSubFolder As String,
                                 ByVal strAddressDelimited As String,
                                 Optional IsUBP As Boolean = False) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOData", con)
            cmd.CommandTimeout = 0
            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("CRN", strCRN)
            cmd.Parameters.AddWithValue("Barcode", strBarcode)
            cmd.Parameters.AddWithValue("FName", strFName.Replace("�", "Ñ"))
            cmd.Parameters.AddWithValue("MName", strMName.Replace("�", "Ñ"))
            cmd.Parameters.AddWithValue("LName", strLName.Replace("�", "Ñ"))
            cmd.Parameters.AddWithValue("Suffix", strSuffix)
            cmd.Parameters.AddWithValue("Sex", strSex)
            cmd.Parameters.AddWithValue("DateOfBirth", strDateOfBirth)
            cmd.Parameters.AddWithValue("Address", strAddress.Replace("�", "Ñ"))
            cmd.Parameters.AddWithValue("ActivityID", intStatusID)
            cmd.Parameters.AddWithValue("BackOCR", strBackOCR)
            cmd.Parameters.AddWithValue("CurrentPage", intCurrentPage)
            cmd.Parameters.AddWithValue("CurrentSeries", intCurrentSeries)
            cmd.Parameters.AddWithValue("POSubFolder", strPOSubFolder)
            cmd.Parameters.AddWithValue("AddressDelimited", strAddressDelimited.Replace("�", "Ñ"))
            cmd.Parameters.AddWithValue("IsUBP", IsUBP)

            '_ExecuteReader(CommandType.StoredProcedure)
            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
            'Finally
            '    CloseConnection()
        End Try
    End Function

    Public Function AddSystemLog(ByVal strSystemLogDesc As String, ByVal strProcess As String, ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddSystemLog", con)
            cmd.Parameters.AddWithValue("SystemLogDesc", strSystemLogDesc)
            cmd.Parameters.AddWithValue("Process", strProcess)
            cmd.Parameters.AddWithValue("SubmittedBy_UserID", intUserID)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function EndUserSession(ByVal strUsername As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcEndUserSession", con)

            cmd.Parameters.AddWithValue("Username", strUsername)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateFNameLNameAddressByCardID(ByVal CardID As String, ByVal FName As String, ByVal LName As String,
                                                    ByVal Suffix As String, ByVal Address As String) As Boolean
        Try
            Dim sbQuery As New StringBuilder

            sbQuery.Append("UPDATE dbo.tblRelPOData SET FName=@FName,LName=@LName,Suffix=@Suffix,Address=@Address WHERE CardID=@CardID")

            OpenConnection()
            cmd = New SqlCommand(sbQuery.ToString, con)
            cmd.Parameters.AddWithValue("CardID", CardID)
            cmd.Parameters.AddWithValue("FName", FName)
            cmd.Parameters.AddWithValue("LName", LName)
            cmd.Parameters.AddWithValue("Suffix", Suffix)
            cmd.Parameters.AddWithValue("Address", Address)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddErrorLog(ByVal strErrorLogDesc As String, ByVal strProcess As String, ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddErrorLog", con)
            cmd.Parameters.AddWithValue("ErrorLogDesc", strErrorLogDesc)
            cmd.Parameters.AddWithValue("Process", strProcess)
            cmd.Parameters.AddWithValue("SubmittedBy_UserID", intUserID)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddCardActivity(ByVal intPOID As Integer, ByVal strCRN As String, ByVal strBarcode As String,
                                    ByVal strDescription As String, ByVal intUserID As Integer, ByVal strProcess As String) As Boolean
        Try
            OpenConnection()
            'cmd = New SqlCommand("prcAddCardActivity", con)
            cmd = New SqlCommand("prcAddCardActivityv2", con)

            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("CRN", strCRN)
            cmd.Parameters.AddWithValue("Barcode", strBarcode)
            cmd.Parameters.AddWithValue("Description", strDescription)
            cmd.Parameters.AddWithValue("UserID", intUserID)
            cmd.Parameters.AddWithValue("Process", strProcess)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelPODataSheetBarcode(ByVal CRN1 As String, ByVal CRN2 As String, ByVal FrontSheetBarcode As String, ByVal BackSheetBarcode As String) As Boolean
        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddRelPODataSheetBarcode", con)

            cmd.Parameters.AddWithValue("CRN1", CRN1)
            cmd.Parameters.AddWithValue("CRN2", CRN2)
            cmd.Parameters.AddWithValue("FrontSheetBarcode", FrontSheetBarcode)
            cmd.Parameters.AddWithValue("BackSheetBarcode", BackSheetBarcode)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddCMSReprintData(ByVal PurchaseOrder As String, ByVal Barcode As String, ByVal PO_Qty As Integer, _
                                    ByVal PO_Reprint As Integer, ByVal Description As String, ByVal DateTimePosted As DateTime, _
                                    ByVal SourceFile As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddCMSReprintData", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", PurchaseOrder)
            cmd.Parameters.AddWithValue("Barcode", Barcode)
            cmd.Parameters.AddWithValue("PO_Qty", PO_Qty)
            cmd.Parameters.AddWithValue("PO_Reprint", PO_Reprint)
            cmd.Parameters.AddWithValue("Description", Description)
            cmd.Parameters.AddWithValue("DateTimePosted", DateTimePosted)
            cmd.Parameters.AddWithValue("SourceFile", SourceFile)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddCardReject(ByVal intPOID As Integer, ByVal strPurchaseOrder As String, _
                                  ByVal strBarcode As String, ByVal strCRN As String, _
                                  ByVal intUserID As Integer, ByVal intRejectTypeID As Short) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddCardReject", con)

            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("Barcode", strBarcode)
            cmd.Parameters.AddWithValue("CRN", strCRN)
            cmd.Parameters.AddWithValue("RejectBy_UserID", intUserID)
            cmd.Parameters.AddWithValue("RejectTypeID", intRejectTypeID)


            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelMaterialAddtl(ByVal intMaterialID As Integer, ByVal intAddedQty As Integer, ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelMaterialAddtl", con)

            cmd.Parameters.AddWithValue("MaterialID", intMaterialID)
            cmd.Parameters.AddWithValue("AddedQty", intAddedQty)
            cmd.Parameters.AddWithValue("UserID", intUserID)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddMstrMaterial(ByVal strMaterial As String, ByVal intBegQty As Integer, ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddMstrMaterial", con)

            cmd.Parameters.AddWithValue("Material", strMaterial)
            cmd.Parameters.AddWithValue("BegQty", intBegQty)
            cmd.Parameters.AddWithValue("UserID", intUserID)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function EditMstrMaterial(ByVal intMaterialID As Integer, ByVal strMaterial As String, ByVal intBegQty As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcEditMstrMaterial", con)

            cmd.Parameters.AddWithValue("MaterialID", intMaterialID)
            cmd.Parameters.AddWithValue("Material", strMaterial)
            cmd.Parameters.AddWithValue("BegQty", intBegQty)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddPOMaterial(ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddPOMaterial", con)
          
            cmd.Parameters.AddWithValue("UserID", intUserID)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelPOReport(ByVal intPOID As Integer, ByVal strPurchaseOrder As String, _
                                   ByVal inReportTypeID As Integer, ByVal byteReportImage() As Byte) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOReport", con)

            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("ReportTypeID", inReportTypeID)
            If Not byteReportImage Is Nothing Then cmd.Parameters.AddWithValue("ReportImage", byteReportImage)
            cmd.Parameters.AddWithValue("IsGetResult", True)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelPOMatPO(ByVal intPOMaterialID As Integer, ByVal strPurchaseOrder As String, ByVal intPO_Quantity As Integer, _
                                    ByVal dtmDateTimePosted_PO As Date) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOMatPO", con)

            cmd.Parameters.AddWithValue("POMaterialID", intPOMaterialID)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("PO_Quantity", intPO_Quantity)
            cmd.Parameters.AddWithValue("DateTimePosted_PO", dtmDateTimePosted_PO)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelPOMatMaterial(ByVal intPOMaterialID As Integer, ByVal intMaterialID As Integer, _
                                  ByVal intAlltdQty As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOMatMaterial", con)

            cmd.Parameters.AddWithValue("POMaterialID", intPOMaterialID)
            cmd.Parameters.AddWithValue("MaterialID", intMaterialID)
            cmd.Parameters.AddWithValue("AlltdQty", intAlltdQty)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddUser(ByVal strUsername As String, ByVal strPassword As String, _
                            ByVal strFName As String, ByVal strMName As String, ByVal strLName As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddUser", con)
            cmd.Parameters.AddWithValue("Username", strUsername)
            cmd.Parameters.AddWithValue("Password", SharedFunction.EncryptData(strPassword))
            cmd.Parameters.AddWithValue("FName", strFName)
            cmd.Parameters.AddWithValue("MName", strMName)
            cmd.Parameters.AddWithValue("LName", strLName)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ChangeUserPassword(ByVal intUserID As Integer, ByVal strOldPassword As String, _
                           ByVal strNewPassword As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcChangeUserPassword", con)
            cmd.Parameters.AddWithValue("UserID", intUserID)
            cmd.Parameters.AddWithValue("OldPassword_in", SharedFunction.EncryptData(strOldPassword))
            cmd.Parameters.AddWithValue("NewPassword", SharedFunction.EncryptData(strNewPassword))

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function EditUser(ByVal intUserID As Integer, ByVal strUsername As String, _
                            ByVal strFName As String, ByVal strMName As String, ByVal strLName As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcEditUser", con)
            cmd.Parameters.AddWithValue("UserID", intUserID)
            cmd.Parameters.AddWithValue("Username", strUsername)
            cmd.Parameters.AddWithValue("FName", strFName)
            cmd.Parameters.AddWithValue("MName", strMName)
            cmd.Parameters.AddWithValue("LName", strLName)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function EditRole(ByVal intRoleID As Short, ByVal strRoleDesc As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcEditRole", con)
            cmd.Parameters.AddWithValue("RoleID", intRoleID)
            cmd.Parameters.AddWithValue("RoleDesc", strRoleDesc)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddLoggedUsers(ByVal intUserID As Integer, ByVal dtmDatePosted As Date) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddLoggedUsers", con)
            cmd.Parameters.AddWithValue("UserID", intUserID)
            cmd.Parameters.AddWithValue("DateTimePosted", dtmDatePosted)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ValidateLogIN(ByVal strUsername As String, ByVal strPassword As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcValidateLogIN2", con)
            cmd.Parameters.AddWithValue("Username", strUsername)
            cmd.Parameters.AddWithValue("Password", SharedFunction.EncryptData(strPassword))

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddModule(ByVal strModule As String, ByVal strModuleDesc As String, ByVal strPage As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddModule", con)
            cmd.Parameters.AddWithValue("Module", strModule)
            cmd.Parameters.AddWithValue("ModuleDesc", strModuleDesc)
            cmd.Parameters.AddWithValue("Page", strPage)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function EditModule(ByVal intModuleID As Integer, ByVal strModule As String, _
                               ByVal strModuleDesc As String, ByVal strPage As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcEditModule", con)
            cmd.Parameters.AddWithValue("ModuleID", intModuleID)
            cmd.Parameters.AddWithValue("Module", strModule)
            cmd.Parameters.AddWithValue("ModuleDesc", strModuleDesc)
            cmd.Parameters.AddWithValue("Page", strPage)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRole(ByVal strRoleDesc As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRole", con)

            cmd.Parameters.AddWithValue("RoleDesc", strRoleDesc)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddUserRole(ByVal intUserID As Integer, ByVal intRoleID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(String.Format("INSERT INTO tblUserRole (UserID, RoleID) VALUES ({0},{1})", intUserID, intRoleID), con)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ResetSSSRejectByPO(ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcResetSSSRejectByPO", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateSSSRejectByQAGRID(ByVal intQAGRID As Integer, ByVal strNewPO As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateSSSRejectByQAGRID", con)

            cmd.Parameters.AddWithValue("QAGRID", intQAGRID)
            cmd.Parameters.AddWithValue("NewPO", strNewPO)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelPOImage(ByVal strBarcode As String, _
                                 ByVal byteXML() As Byte, ByVal bytePersoXML() As Byte, _
                                 ByVal bytePhoto() As Byte, ByVal byteSignature() As Byte, _
                                 ByVal byteRprimaryANSI378() As Byte, ByVal byteRbackupANSI378() As Byte, _
                                 ByVal byteLprimaryANSI378() As Byte, ByVal byteLbackupANSI378() As Byte) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOImage", con)
            cmd.CommandTimeout = 0

            cmd.Parameters.Add("Barcode", SqlDbType.Char, 20).Value = strBarcode
            cmd.Parameters.Add("Xml", SqlDbType.VarBinary, 2000).Value = byteXML
            cmd.Parameters.Add("PersoXml", SqlDbType.VarBinary, 2000).Value = bytePersoXML
            cmd.Parameters.Add("Photo", SqlDbType.VarBinary, -1).Value = bytePhoto
            cmd.Parameters.Add("Signature", SqlDbType.VarBinary, 2000).Value = byteSignature
            cmd.Parameters.Add("RprimaryANSI378", SqlDbType.VarBinary, 2000).Value = byteRprimaryANSI378
            cmd.Parameters.Add("RbackupANSI378", SqlDbType.VarBinary, 2000).Value = byteRbackupANSI378
            cmd.Parameters.Add("LprimaryANSI378", SqlDbType.VarBinary, 2000).Value = byteLprimaryANSI378
            cmd.Parameters.Add("LbackupANSI378", SqlDbType.VarBinary, 2000).Value = byteLbackupANSI378

            'cmd.Parameters.AddWithValue("Xml", byteXML)
            'cmd.Parameters.AddWithValue("PersoXml", bytePersoXML)
            'cmd.Parameters.AddWithValue("Photo", bytePhoto)
            'cmd.Parameters.AddWithValue("Signature", byteSignature)
            'cmd.Parameters.AddWithValue("RprimaryANSI378", byteRprimaryANSI378)
            'cmd.Parameters.AddWithValue("RbackupANSI378", byteRbackupANSI378)
            'cmd.Parameters.AddWithValue("LprimaryANSI378", byteLprimaryANSI378)
            'cmd.Parameters.AddWithValue("LbackupANSI378", byteLbackupANSI378)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
            'Finally
            '    CloseConnection()
        End Try
    End Function

    Public Function AddRelPOImagev2(ByVal strBarcode As String, _
                                 ByVal byteXML() As Byte, ByVal bytePersoXML() As Byte, _
                                 ByVal bytePhoto() As Byte, ByVal byteSignature() As Byte, _
                                 ByVal byteRprimaryANSI378() As Byte, ByVal byteRbackupANSI378() As Byte, _
                                 ByVal byteLprimaryANSI378() As Byte, ByVal byteLbackupANSI378() As Byte, _
                                 ByVal intPOID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOImagev2", con)
            cmd.CommandTimeout = 0

            cmd.Parameters.Add("Barcode", SqlDbType.Char, 20).Value = strBarcode
            cmd.Parameters.Add("Xml", SqlDbType.VarBinary, 2000).Value = byteXML
            cmd.Parameters.Add("PersoXml", SqlDbType.VarBinary, 2000).Value = bytePersoXML
            cmd.Parameters.Add("Photo", SqlDbType.VarBinary, -1).Value = bytePhoto
            cmd.Parameters.Add("Signature", SqlDbType.VarBinary, 2000).Value = byteSignature
            cmd.Parameters.Add("RprimaryANSI378", SqlDbType.VarBinary, 2000).Value = byteRprimaryANSI378
            cmd.Parameters.Add("RbackupANSI378", SqlDbType.VarBinary, 2000).Value = byteRbackupANSI378
            cmd.Parameters.Add("LprimaryANSI378", SqlDbType.VarBinary, 2000).Value = byteLprimaryANSI378
            cmd.Parameters.Add("LbackupANSI378", SqlDbType.VarBinary, 2000).Value = byteLbackupANSI378
            cmd.Parameters.Add("POID", SqlDbType.Int).Value = intPOID

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
            'Finally
            '    CloseConnection()
        End Try
    End Function

    Public Function AddRelPOImage_PhotoAndSignature(ByVal strBarcode As String, _
                             ByVal bytePhoto() As Byte, ByVal byteSignature() As Byte, _
                             ByVal intPOID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelPOImage_PhotoAndSignature", con)
            cmd.CommandTimeout = 0

            cmd.Parameters.Add("Barcode", SqlDbType.Char, 20).Value = strBarcode
            cmd.Parameters.Add("Photo", SqlDbType.VarBinary, -1).Value = bytePhoto
            cmd.Parameters.Add("Signature", SqlDbType.VarBinary, 2000).Value = byteSignature
            cmd.Parameters.Add("POID", SqlDbType.Int).Value = intPOID

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
            'Finally
            '    CloseConnection()
        End Try
    End Function

    Public Function AddDownloadableFiles(ByVal strDescription As String, ByVal strFilePath As String, _
                                         ByVal intUserID As Integer, ByVal strType As String, ByVal strRefPOID As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddDownloadableFiles", con)
            cmd.Parameters.AddWithValue("Description", strDescription)
            cmd.Parameters.AddWithValue("FilePath", strFilePath)
            cmd.Parameters.AddWithValue("UserID", intUserID)
            cmd.Parameters.AddWithValue("Type", strType)
            cmd.Parameters.AddWithValue("RefPOID", strRefPOID)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddForUpload(ByVal strPurchaseOrder As String, ByVal strBatch As String, _
                                 ByVal strDateTime As String, ByVal intQuantity As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddForUpload", con)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("Batch", strBatch)
            cmd.Parameters.AddWithValue("DateTime", strDateTime)
            cmd.Parameters.AddWithValue("Quantity", intQuantity)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddDRList(ByVal strBarcode As String, ByVal strChipSerialNo As String, ByVal strCardDate As String,
                              ByVal strUID As String, ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddDRList", con)
            cmd.Parameters.AddWithValue("Barcode", strBarcode)
            cmd.Parameters.AddWithValue("ChipSerialNo", strChipSerialNo)
            cmd.Parameters.AddWithValue("CardDate", strCardDate)
            cmd.Parameters.AddWithValue("UID", strUID)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelCDFRData(ByVal CDFRID As Integer, ByVal CRN As String, ByVal Barcode As String, ByVal GSISNo As String,
                              ByVal BPNo As String, ByVal SSSNo As String,
                              ByVal ACNo As String, ByVal CNo As String, ByVal ExpDate As String,
                              ByVal OrigData As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelCDFRData", con)
            cmd.Parameters.AddWithValue("CDFRID", CDFRID)
            cmd.Parameters.AddWithValue("CRN", CRN)
            cmd.Parameters.AddWithValue("Barcode", Barcode)
            cmd.Parameters.AddWithValue("GSISNo", GSISNo)
            cmd.Parameters.AddWithValue("BPNo", BPNo)
            cmd.Parameters.AddWithValue("SSSNo", SSSNo)
            cmd.Parameters.AddWithValue("ACNo", ACNo)
            cmd.Parameters.AddWithValue("CNo", CNo)
            cmd.Parameters.AddWithValue("ExpDate", ExpDate)
            cmd.Parameters.AddWithValue("OrigData", OrigData)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddRelCMSRejectData(ByVal CMSRejectID As Integer, ByVal Barcode As String, ByVal Code As String, ByVal Description As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddRelCMSRejectData", con)
            cmd.Parameters.AddWithValue("CMSRejectID", CMSRejectID)
            cmd.Parameters.AddWithValue("Barcode", Barcode)
            cmd.Parameters.AddWithValue("Code", Code)
            cmd.Parameters.AddWithValue("Description", Description)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForRF(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            'strIDs = String.Format(" WHERE dbo.tblBatch.BatchID IN ({0})", strIDs)

            'cmd.Parameters.AddWithValue("IDs", strIDs)

            'FillDataAdapter(CommandType.StoredProcedure)
            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    'Public Function UpdateRelCDFRDataByCRN(ByVal CRN As String, ByVal PurchaseOrder As String, ByVal Barcode As String) As Boolean
    Public Function UpdateRelCDFRDataByCRN2(ByVal CRN As String, ByVal PurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateRelCDFRDataByCRN", con)
            cmd.Parameters.AddWithValue("CRN", CRN)
            cmd.Parameters.AddWithValue("PurchaseOrder", PurchaseOrder)
            'cmd.Parameters.AddWithValue("Barcode", Barcode)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateRelCDFRDataByBarcode(ByVal Barcode As String, ByVal PurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateRelCDFRDataByBarcode", con)
            'cmd.Parameters.AddWithValue("CRN", CRN)
            cmd.Parameters.AddWithValue("PurchaseOrder", PurchaseOrder)
            cmd.Parameters.AddWithValue("Barcode", Barcode)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateRelCDFRDataByPOAndBarcode(ByVal Barcode As String, ByVal PurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateRelCDFRDataByPOAndBarcode", con)

            cmd.Parameters.AddWithValue("Barcode", Barcode)
            cmd.Parameters.AddWithValue("PurchaseOrder", PurchaseOrder)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ResetRelCDFRDataByPOAndBarcode(ByVal Barcode As String, ByVal PurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcResetRelCDFRDataByPOAndBarcode", con)

            cmd.Parameters.AddWithValue("Barcode", Barcode)
            cmd.Parameters.AddWithValue("PurchaseOrder", PurchaseOrder)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ReturnPOStatusToNewUpload(ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcReturnPOStatusToNewUpload", con)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function RefreshStatusCounter() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcRefreshStatusCounter", con)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function IndigoExtract(ByVal intCardID As Integer, ByVal intStatusID As Short, _
                                             ByVal intCurrentPage As Integer, ByVal intCurrentSeries As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcIndigoExtract", con)
            cmd.Parameters.AddWithValue("CardID", intCardID)
            cmd.Parameters.AddWithValue("ActivityID", intStatusID)
            cmd.Parameters.AddWithValue("CurrentPage", intCurrentPage)
            cmd.Parameters.AddWithValue("CurrentSeries", intCurrentSeries)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateCardStatusByCardID(ByVal intCardID As Integer, ByVal intStatusID As Short) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcUpdateCardStatusByCardID", con)
            cmd.Parameters.AddWithValue("CardID", intCardID)
            cmd.Parameters.AddWithValue("ActivityID", intStatusID)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateRelMaterialAddtlByID(ByVal intAddtlMaterialID As Integer, ByVal intAddedQty As Integer, ByVal dtmDateTimePosted As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("UPDATE dbo.tblRelMaterialAddtl SET AddedQty=@AddedQty, DateTimePosted=@DateTimePosted WHERE AddtlMaterialID=@AddtlMaterialID", con)
            cmd.Parameters.AddWithValue("@AddedQty", intAddedQty)
            cmd.Parameters.AddWithValue("@DateTimePosted", dtmDateTimePosted)
            cmd.Parameters.AddWithValue("@AddtlMaterialID", intAddtlMaterialID)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function PurgePOData(ByVal intPOID As Integer, ByVal IsForReupload As Boolean) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcPurgePOData", con)
            cmd.CommandTimeout = 0
            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("IsForReupload", IsForReupload)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function PurgePODatav2(ByVal intPOID As Integer, ByVal IsForReupload As Boolean, ByVal intTable As Short) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcPurgePODatav2", con)
            cmd.CommandTimeout = 0
            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("IsForReupload", IsForReupload)
            cmd.Parameters.AddWithValue("Table", intTable)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function TagPOAsExtracted(ByVal strID As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("UPDATE tblPO SET DateTimeExtracted = GETDATE() WHERE POID IN (" & strID & ") AND DateTimeExtracted IS NULL", con)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception

            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ResetUserPassword(ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            'cmd = New SqlCommand("UPDATE  set fld_password='" & SharedFunction.EncryptData(My.Settings.UserDefPsswrd) & "' WHERE fld_userid='" & UserID & "'", con)

            cmd = New SqlCommand("UPDATE tblUser SET Password='" & SharedFunction.EncryptData(My.Settings.UserDefPsswrd) & "' WHERE UserID=" & intUserID, con)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception

            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ExecuteScalar(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetPurchaseOrderCMSReprint(ByVal strPO As String, ByVal strBarcodes As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("Select COUNT(Barcode) FROM dbo.tblCMSReprintData WHERE (PurchaseOrder = '" & strPO & "') AND (Barcode IN (" & strBarcodes & "))", con)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function InsertUser(ByVal CompanyID As String, _
                               ByVal UserName As String, _
                               ByVal Password As String, _
                               ByVal FullName As String, _
                               ByVal StatusID As Integer, _
                               ByVal RoleID As Integer, _
                               ByVal IH_CompanyID As String, _
                               ByVal SaturdayWorkSchedID As Short) As Boolean
        Try
            OpenConnection()
            'Dim strQuery As String = String.Format("INSERT INTO tbl_user (fld_userid,fld_username,fld_password,fld_fullname,fld_statusid,fld_RoleID,fld_DateAdded) VALUES ('{0}','{1}','{2}','{3}',{4},{5},{6})", CompanyID, UserName,assword, FullName, StatusID, RoleID, "(SELECT SYSDATE())")

            cmd = New SqlCommand("spSave_User", con)
            cmd.Parameters.AddWithValue("puserID", CompanyID)
            cmd.Parameters.AddWithValue("pusername", UserName)
            cmd.Parameters.AddWithValue("pPassword", Password)
            cmd.Parameters.AddWithValue("pfullname", FullName)
            cmd.Parameters.AddWithValue("pstatusID", StatusID)
            cmd.Parameters.AddWithValue("pRoleID", RoleID)
            cmd.Parameters.AddWithValue("pIH_userID", IH_CompanyID)
            cmd.Parameters.AddWithValue("pSaturdayWorkSchedID", SaturdayWorkSchedID)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectQuery(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByStatusID(ByVal intStatusID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByStatusID", con)

            cmd.Parameters.AddWithValue("StatusID", intStatusID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelMaterialAddtl() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelMaterialAddtl", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByByCRNorBarcode(ByVal param As String) As Boolean
        If param.Length = 20 Then
            Return SelectRelPODataByBarcodev2(param.Replace("-", "").Trim)
        Else
            Return SelectRelPODataByCRNv2(param.Trim)
        End If
    End Function

    Public Function SelectRelPODataByByCRNorBarcode_CardControl(ByVal param As String) As Boolean
        If param.Length = 20 Then
            Return SelectRelPODataByBarcode_CardControlv2(param.Replace("-", ""))
        Else
            Return SelectRelPODataByCRN_CardControlv2(param)
        End If
    End Function

    Public Function SelectRelPODataByBarcode(ByVal strBarcode As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByBarcode", con)

            cmd.Parameters.AddWithValue("Barcode", strBarcode)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByBarcodev2(ByVal strBarcode As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByBarcodev2", con)

            cmd.Parameters.AddWithValue("Barcode", strBarcode)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByBarcode_CardControl(ByVal strBarcode As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByBarcode_CardControl", con)

            cmd.Parameters.AddWithValue("Barcode", strBarcode)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByBarcode_CardControlv2(ByVal strBarcode As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByBarcode_CardControlv2", con)

            cmd.Parameters.AddWithValue("Barcode", strBarcode)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByCRN_CardControl(ByVal strCRN As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByCRN_CardControl", con)

            cmd.Parameters.AddWithValue("CRN", strCRN)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByCRN_CardControlv2(ByVal strCRN As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByCRN_CardControlv2", con)

            cmd.Parameters.AddWithValue("CRN", strCRN)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForPrinting(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForPrinting", con)

            'strIDs = String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForFNameLNameAddressModificationByCSNCRN(ByVal strWhereClause As String) As Boolean
        Try
            Dim sbQuery As New StringBuilder
            sbQuery.Append("SELECT TOP 1 dbo.tblRelPOData.CardID, dbo.tblRelPOData.CRN, dbo.tblRelPOData.Barcode, ")
            sbQuery.Append("dbo.tblRelPOData.FName, dbo.tblRelPOData.LName, dbo.tblRelPOData.Suffix, dbo.tblRelPOData.BackOCR, ")
            sbQuery.Append("dbo.tblRelPOData.Address, dbo.tblPO.PurchaseOrder FROM dbo.tblRelPOData INNER JOIN ")
            sbQuery.Append("dbo.tblPO ON dbo.tblRelPOData.POID = dbo.tblPO.POID " & strWhereClause)
            sbQuery.Append(" ORDER BY dbo.tblRelPOData.CardID DESC")

            OpenConnection()
            cmd = New SqlCommand(sbQuery.ToString, con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForPrintingForLaser(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()

            'cmd = New SqlCommand("prcSelectDataForPrintingForLaser", con)

            'revised 03/06/2019
            cmd = New SqlCommand("prcSelectDataForPrintingForLaserv2", con)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForDeliveryReceipt2(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForDeliveryReceipt2", con)

            'strIDs = String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectCardReject(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectCardReject", con)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectCubaoBranchList(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectCubaoBranchList", con)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForMailer(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForMailer", con)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectSSSRejects(ByVal strWhereCriteria As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectSSSRejects", con)

            cmd.Parameters.AddWithValue("WhereCriteria", strWhereCriteria)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForIndigoPrinting(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForIndigoPrinting", con)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForMuhlbauer(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            'cmd = New SqlCommand("prcSelectDataForMuhlbauer", con)

            'added for sss-ubp requirements 01/17/2019
            cmd = New SqlCommand("prcSelectDataForMuhlbauerv2", con)

            'strIDs = String.Format(" WHERE dbo.tblPO.POID IN ({0})", strIDs)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectMaterialInventory() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectMaterialInventory", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectOpenBatch() As Boolean
        Try
            OpenConnection()
            'cmd = New SqlCommand("prcSelectOpenBatch", con)

            'change on 01/15/2019 due to sss ubp requirements
            cmd = New SqlCommand("prcSelectOpenBatchv2", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectOpenBatchByCurrentDate() As Boolean
        Try
            OpenConnection()
            'cmd = New SqlCommand("prcSelectOpenBatchByCurrentDate", con)

            'change on 01/15/2019 due to sss ubp requirements
            cmd = New SqlCommand("prcSelectOpenBatchByCurrentDatev2", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOForAllotment() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPOForAllotment", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPurchaseOrderReports(ByVal WhereCriteria As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPurchaseOrderReports", con)

            cmd.Parameters.AddWithValue("WhereCriteria", WhereCriteria)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function TrackData(ByVal strTrackField As String, ByVal strValue As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcTrackData", con)

            cmd.Parameters.AddWithValue("TrackField", strTrackField.ToUpper)
            cmd.Parameters.AddWithValue("Value", strValue)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectBatchForDelivery() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectBatchForDelivery", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOMaterial() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPOMaterial", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectMaterialInventoryForNewProcess() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectMaterialInventoryForNewProcess", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPOMatPOByPOMaterialID(ByVal intPOMaterialID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPOMatPOByPOMaterialID", con)

            cmd.Parameters.AddWithValue("POMaterialID", intPOMaterialID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPOMatMaterialByPOMaterialID(ByVal intPOMaterialID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPOMatMaterialByPOMaterialID", con)

            cmd.Parameters.AddWithValue("POMaterialID", intPOMaterialID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectErrorLog() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectErrorLog", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDRListByActivePO(ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDRListByActivePO", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDRListByActivePOByDisplay(ByVal strPurchaseOrder As String, ByVal intDisplayType As Short) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDRListByActivePOByDisplay", con)
            cmd.CommandTimeout = 0

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("DisplayType", intDisplayType)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDRListByBarcode(ByVal strBarcodes As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDRListByBarcode", con)

            cmd.Parameters.AddWithValue("IDs", strBarcodes)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDRListByActivePOAndPersoDate(ByVal strPurchaseOrder As String, ByVal dtmPersoDate As Date) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDRListByActivePOAndPersoDate", con)
            cmd.CommandTimeout = 0

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("PersoDate", dtmPersoDate.ToString("yyyy/MM/dd"))

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDRListByActivePOAndPersoDateByDisplay(ByVal strPurchaseOrder As String, ByVal dtmPersoDate As Date, ByVal intDisplayType As Short) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDRListByActivePOAndPersoDateByDisplay", con)
            cmd.CommandTimeout = 0

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)
            cmd.Parameters.AddWithValue("PersoDate", dtmPersoDate.ToString("yyyy/MM/dd"))
            cmd.Parameters.AddWithValue("DisplayType", intDisplayType)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetLastBackOCR_ByPO(ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcGetLastBackOCR_ByPO", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectLoggedUsers() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("SELECT *, dbo.fnGetUserCompleteName(UserID) As UserCompleteName FROM dbo.tblLoggedUsers", con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectSystemLog() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectSystemLog", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectReprocessAndNewUploadPO_Details(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectReprocessAndNewUploadPO_Details", con)

            'strIDs = String.Format(" AND dbo.tblPO.POID IN ({0})", strIDs)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectAllotedMaterialsBreakdown(ByVal intMaterialID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectAllotedMaterialsBreakdown", con)

            cmd.Parameters.AddWithValue("MaterialID", intMaterialID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectCDFR(ByVal WhereCriteria As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectCDFR", con)

            cmd.Parameters.AddWithValue("WhereCriteria", WhereCriteria)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectUniqueCDFRByPOAndBarcode(ByVal WhereCriteria As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectUniqueCDFRByPOAndBarcode", con)

            cmd.Parameters.AddWithValue("IDs", WhereCriteria)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectCMSReprintData() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectCMSReprintData", con)

            'cmd.Parameters.AddWithValue("MaterialID", intMaterialID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOForPurging() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPOForPurging", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOForReupload() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPOForReupload", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectUserRoleByUserID(ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectUserRoleByUserID", con)

            cmd.Parameters.AddWithValue("UserID", intUserID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByCardIDandLast20(ByVal intCardID As Integer, ByVal intPOID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByCardIDandLast20", con)

            cmd.Parameters.AddWithValue("CardID", intCardID)
            cmd.Parameters.AddWithValue("POID", intPOID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForStatusUpdate(ByVal strIDs As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForStatusUpdate", con)

            strIDs = String.Format(" WHERE dbo.tblPO.POID IN ({0}) AND dbo.tblRelPOData.ActivityID = 1", strIDs)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForStatusUpdatev2(ByVal strIDs As String, ByVal DataType As Short) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForStatusUpdatev2", con)

            Dim strCDFRQuery As String = "(SELECT COUNT(vw.Barcode) FROM vwCDFRList vw WHERE vw.Barcode = dbo.tblRelPOData.Barcode AND vw.PurchaseOrder = dbo.tblPO.PurchaseOrder)"

            strIDs = String.Format(" WHERE dbo.tblPO.POID IN ({0}) AND dbo.tblRelPOData.ActivityID = 1 AND " & IIf(DataType = 1, strCDFRQuery & "=0", strCDFRQuery & ">0"), strIDs)

            cmd.Parameters.AddWithValue("IDs", strIDs)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectProcessedSSSReject() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectProcessedSSSReject", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectStatusCounter() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectStatusCounter", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByCRN(ByVal strCRN As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByCRN", con)

            cmd.Parameters.AddWithValue("CRN", strCRN)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectRelPODataByCRNv2(ByVal strCRN As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectRelPODataByCRNv2", con)

            cmd.Parameters.AddWithValue("CRN", strCRN)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataByStatus(ByVal intActivityID As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataByStatus", con)

            cmd.Parameters.AddWithValue("ActivityID", intActivityID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPO_PerBoxCntr(ByVal intPOID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPO_PerBoxCntr", con)

            cmd.Parameters.AddWithValue("POID", intPOID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataByStatusAndPO(ByVal intActivityID As String, ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataByStatusAndPO", con)

            cmd.Parameters.AddWithValue("ActivityID", intActivityID)
            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ReportE(ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcReportE", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ReportG(ByVal strPurchaseOrder As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcReportG", con)

            cmd.Parameters.AddWithValue("PurchaseOrder", strPurchaseOrder)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOPendingByStatus(ByVal intActivityID As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPOPendingByStatus", con)

            cmd.Parameters.AddWithValue("ActivityID", intActivityID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOPendingBreakdown(ByVal intPOID As String, ByVal intActivityID As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectPOPendingBreakdown", con)

            cmd.Parameters.AddWithValue("POID", intPOID)
            cmd.Parameters.AddWithValue("ActivityID", intActivityID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectSSSRejectPending() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectSSSRejectPending", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDownloadableFiles(ByVal strWhereCriteria As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDownloadableFiles", con)
            'WHERE DownloadCntr < 2
            cmd.Parameters.AddWithValue("WhereCriteria", strWhereCriteria)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectNewUploadPO() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectNewUploadPO", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectUserModules(ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectUserModules", con)

            cmd.Parameters.AddWithValue("UserID", intUserID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectUserActivity(ByVal intUserID As Integer) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectUserActivity", con)

            cmd.Parameters.AddWithValue("UserID", intUserID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectReprocessAndNewUploadPO(ByVal DataType As Short) As Boolean
        Try
            OpenConnection()

            'cmd = New SqlCommand("prcSelectReprocessAndNewUploadPO", con)

            'revised to handle sss-ubp requirements 01/14/2019
            cmd = New SqlCommand("prcSelectReprocessAndNewUploadPOv2", con)

            cmd.Parameters.AddWithValue("DataType", DataType)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectReprocessPO() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectReprocessPO", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectDataForBarcodeGeneration() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcSelectDataForBarcodeGeneration", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectPOArchive(ByVal strWhereCriteria As String) As Boolean
        Try
            Dim sb As New StringBuilder
            'CONVERT(char(10), DateTimePosted, 101) = 03/16/2016
            sb.Append("Select ArchivePOID, POID, PurchaseOrder, Quantity, Batch, DateTimePosted_Uploaded, DateTimeExtracted, DateTimePosted ")
            sb.Append("FROM dbo.tblPOArchive ")
            sb.Append(strWhereCriteria)

            OpenConnection()
            cmd = New SqlCommand(sb.ToString, con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GeneratePurchaseOrderDocuments() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(String.Format("Select PurchaseOrder FROM dbo.tblPO WHERE (IsHold = 0) And (Convert(Char(10), DateTimeExtracted, 101) = '{0}')", Now.ToString("MM/dd/yyyy")), con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GeneratePurchaseOrderDocuments2() As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcGeneratePurchaseOrderDocuments", con)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function CheckIfBatchIsComplete(ByVal parameter As String, Optional ByVal FilterByID As Boolean = True) As Boolean
        Try
            Dim sbQuery As New StringBuilder



            OpenConnection()
            If FilterByID Then
                sbQuery.Append("Select COUNT(dbo.tblRelCDFRData.PurchaseOrder) ")
                sbQuery.Append("From dbo.tblRelCDFRData  ")
                sbQuery.Append("Where (dbo.tblRelCDFRData.PurchaseOrder = @PurchaseOrder) And (dbo.tblRelCDFRData.RF_ReasonCode <> '000')  ")

                cmd = New SqlCommand(sbQuery.ToString, con)
            Else
                'sbQuery.Append("SELECT COUNT(dbo.tblRelBatchData.BatchID) ")
                'sbQuery.Append("FROM dbo.tblRelBatchData INNER JOIN ")
                'sbQuery.Append("dbo.tblBatch ON dbo.tblRelBatchData.BatchID = dbo.tblBatch.BatchID ")
                'sbQuery.Append("WHERE (dbo.tblRelBatchData.RF_ReasonCode <> '000') AND (dbo.tblBatch.Batch = @Batch) ")

                sbQuery.Append("Select COUNT(dbo.tblRelCDFRData.PurchaseOrder) ")
                sbQuery.Append("From dbo.tblRelCDFRData  ")
                sbQuery.Append("Where (dbo.tblRelCDFRData.PurchaseOrder = @PurchaseOrder) And (dbo.tblRelCDFRData.RF_ReasonCode <> '000')  ")

                cmd = New SqlCommand(sbQuery.ToString, con)
            End If

            cmd.CommandTimeout = 0

            If FilterByID Then
                cmd.Parameters.AddWithValue("PurchaseOrder", parameter)
            Else
                cmd.Parameters.AddWithValue("PurchaseOrder", parameter)
            End If


            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            CloseConnection()

        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

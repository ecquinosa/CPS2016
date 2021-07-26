Public Class MemberXML

    Private XML_Path As String = ""

    Private _crn As String = ""
    Private _barcode As String = ""
    Private _fname As String = ""
    Private _mname As String = ""
    Private _lname As String = ""
    Private _suffix As String = ""
    Private _sex As String = ""
    Private _dateofbirth As String = ""
    Private _address As String = ""
    Private _addressDelimited As String
    Private _errormessage As String = ""
    Private _IsSuccess As Boolean = False

    Private dt As DataTable

    Public ReadOnly Property CRN As String
        Get
            Return _crn
        End Get
    End Property

    Public ReadOnly Property Barcode As String
        Get
            Return _barcode
        End Get
    End Property

    Public ReadOnly Property FirstName As String
        Get
            Return _fname
        End Get
    End Property

    Public ReadOnly Property MiddleName As String
        Get
            Return _mname
        End Get
    End Property

    Public ReadOnly Property LastName As String
        Get
            Return _lname
        End Get
    End Property

    Public ReadOnly Property Suffix As String
        Get
            Return _suffix
        End Get
    End Property

    Public ReadOnly Property Sex As String
        Get
            Return _sex
        End Get
    End Property

    Public ReadOnly Property DateOfBirth As String
        Get
            Return _dateofbirth
        End Get
    End Property

    Public ReadOnly Property Address As String
        Get
            Return _address
        End Get
    End Property

    Public ReadOnly Property AddressDelimited As String
        Get
            Return _addressDelimited
        End Get
    End Property

    Public ReadOnly Property ErrorMessage As String
        Get
            Return _errormessage
        End Get
    End Property

    Public ReadOnly Property IsSuccess As String
        Get
            Return _IsSuccess
        End Get
    End Property


    Public Sub New(ByVal XML_Path As String)
        dt = New DataTable
        dt.Columns.Add("Field", Type.GetType("System.String"))
        dt.Columns.Add("Value", Type.GetType("System.String"))

        Me.XML_Path = XML_Path
    End Sub

    Private Sub AddRow(ByVal strField As String, ByVal strValue As String)
        Dim rw As DataRow = dt.NewRow
        rw(0) = strField
        rw(1) = strValue
        dt.Rows.Add(rw)
    End Sub

    Public Function GetDataAndPopulate() As Boolean
        Try
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(XML_Path)

            Do While xmlReader.Read
                If xmlReader.NodeType = System.Xml.XmlNodeType.Element And Not xmlReader.Name = "CARD_DATA" Then
                    Dim strField As String = xmlReader.Name
                    Dim strValue As String = ""

                    Select Case strField
                        Case "_10", "_29", "_2A", "_2B", "_2C"
                            xmlReader.Read()
                            strValue = xmlReader.Value
                        Case Else
                            xmlReader.Read()
                            Try
                                strValue = Base64ToASCII(System.Convert.FromBase64String(xmlReader.Value))
                            Catch
                                strValue = xmlReader.Value
                            End Try
                    End Select

                    'If strField = "_10" Then
                    '    xmlReader.Read()
                    '    'lvi.SubItems.Add(xmlReader.Value)
                    '    strValue = xmlReader.Value
                    'Else

                    'End If

                    AddRow(strField, strValue)
                End If
            Loop

            xmlReader.Close()

            _barcode = Trim(dt.Select("Field='_29'")(0)(1)) & Trim(dt.Select("Field='_2A'")(0)(1)) & Trim(dt.Select("Field='_2B'")(0)(1)) & Trim(dt.Select("Field='_2C'")(0)(1))

            _barcode = _barcode.Replace("-", "")

            _crn = Trim(dt.Select("Field='_10'")(0)(1))

            _crn = _crn.Substring(0, 4) + "-" + _
                   _crn.Substring(4, 7) + "-" + _
                   _crn.Substring(11, 1)

            _fname = Trim(dt.Select("Field='_11'")(0)(1))
            _mname = Trim(dt.Select("Field='_12'")(0)(1))
            _lname = Trim(dt.Select("Field='_13'")(0)(1)) ' & IIf(_suffix <> "", " " & _suffix, "")
            _suffix = Trim(dt.Select("Field='_14'")(0)(1))

            Dim Address(8) As String

            Address(0) = Trim(dt.Select("Field='_1D'")(0)(1))
            Address(1) = Trim(dt.Select("Field='_1C'")(0)(1))
            Address(2) = Trim(dt.Select("Field='_1B'")(0)(1))
            Address(3) = Trim(dt.Select("Field='_1A'")(0)(1))
            Address(4) = Trim(dt.Select("Field='_19'")(0)(1))
            Address(5) = Trim(dt.Select("Field='_18'")(0)(1))
            Address(6) = Trim(dt.Select("Field='_17'")(0)(1))
            Address(7) = Trim(dt.Select("Field='_16'")(0)(1))
            Address(8) = Trim(dt.Select("Field='_15'")(0)(1))

            Dim BuildAddressDelimited As String

            _address = BuildAddress(Address, BuildAddressDelimited)
            '_addressDelimited = BuildAddressDelimited
            _addressDelimited = String.Format("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}", Address(0), Address(1), Address(2), Address(3), Address(4), Address(5), Address(6), Address(7), Address(8))

            Select Case Trim(dt.Select("Field='_1F'")(0)(1))
                Case "F"
                    _sex = "FEMALE"
                Case "M"
                    _sex = "MALE"
                Case Else
                    _sex = Trim(dt.Select("Field='_1F'")(0)(1))
            End Select

            _dateofbirth = Trim(dt.Select("Field='_20'")(0)(1))

            If _dateofbirth.Length = 8 Then _dateofbirth = _dateofbirth.Substring(0, 4) + "/" + _dateofbirth.Substring(4, 2) + "/" + _dateofbirth.Substring(6, 2)

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function ExtractDataForMuhlbauer(ByRef sb As String, ByVal SSSNo As String) As Boolean
        Try
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(XML_Path)

            Do While xmlReader.Read
                If xmlReader.NodeType = System.Xml.XmlNodeType.Element And Not xmlReader.Name = "CARD_DATA" Then
                    Dim strField As String = xmlReader.Name
                    Dim strValue As String = ""

                    If strField = "_10" Then
                        xmlReader.Read()
                        'lvi.SubItems.Add(xmlReader.Value)
                        strValue = xmlReader.Value
                    Else
                        'If strField = "_27" Then
                        '    Console.WriteLine("TEST")
                        'ElseIf strField = "_28" Then
                        '    Console.WriteLine("TEST")
                        'End If

                        xmlReader.Read()
                        Try
                            strValue = Base64ToASCII(System.Convert.FromBase64String(xmlReader.Value))
                        Catch
                            strValue = xmlReader.Value
                        End Try
                    End If

                    AddRow(strField, strValue)
                End If
            Loop

            xmlReader.Close()
            xmlReader = Nothing

            Return ReplaceXMLValue(XML_Path, SSSNo)

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function PopulateXMLDataToTable() As Boolean
        Try
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(XML_Path)

            Do While xmlReader.Read
                If xmlReader.NodeType = System.Xml.XmlNodeType.Element And Not xmlReader.Name = "CARD_DATA" Then
                    Dim strField As String = xmlReader.Name
                    Dim strValue As String = ""

                    If strField = "_10" Then
                        xmlReader.Read()
                        'lvi.SubItems.Add(xmlReader.Value)
                        strValue = xmlReader.Value
                    Else
                        'If strField = "_27" Then
                        '    Console.WriteLine("TEST")
                        'ElseIf strField = "_28" Then
                        '    Console.WriteLine("TEST")
                        'End If

                        xmlReader.Read()
                        Try
                            strValue = Base64ToASCII(System.Convert.FromBase64String(xmlReader.Value))
                        Catch
                            strValue = xmlReader.Value
                        End Try
                    End If

                    AddRow(strField, strValue)
                End If
            Loop

            xmlReader.Close()
            xmlReader = Nothing
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function ExtractDataForMuhlbauer2(ByRef strLine As String) As Boolean
        Try
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(XML_Path)

            Do While xmlReader.Read
                If xmlReader.NodeType = System.Xml.XmlNodeType.Element And Not xmlReader.Name = "CARD_DATA" Then
                    Dim strField As String = xmlReader.Name
                    Dim strValue As String = ""

                    If strField = "_10" Then
                        xmlReader.Read()
                        'lvi.SubItems.Add(xmlReader.Value)
                        strValue = xmlReader.Value
                    Else
                        xmlReader.Read()
                        Try
                            strValue = Base64ToASCII(System.Convert.FromBase64String(xmlReader.Value))
                        Catch
                            strValue = xmlReader.Value
                        End Try
                    End If

                    AddRow(strField, strValue)
                End If
            Loop

            xmlReader.Close()

            Dim sb As New StringBuilder
            Dim sbBarcode As New StringBuilder
            For Each rw As DataRow In dt.Rows
                Select Case rw("Field").ToString.Trim
                    Case "_28"
                    Case "_29", "_2A", "_2B", "_2C"
                        sbBarcode.Append(rw("Value").ToString.Replace("-", "").Trim)

                        If rw("Field").ToString.Trim = "_2C" Then
                            sb.Append(sbBarcode.ToString)
                            sbBarcode.Clear()
                        End If
                    Case Else
                        If sb.ToString <> "" Then
                            sb.Append("|" & rw("Value").ToString.Trim)
                        End If
                End Select
            Next

            strLine = sb.ToString

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ReplaceXMLValue(ByVal file As String, ByVal SSSNo As String) As Boolean
        Try
            Dim MyXML As New System.Xml.XmlDocument()

            MyXML.Load(file)

            For Each rw As DataRow In dt.Rows
                Select Case rw("Field").ToString.Trim
                    Case "_29", "_2A", "_2B", "_2C", "_10"
                    Case Else
                        Dim MyXMLNode As System.Xml.XmlNode = MyXML.SelectSingleNode(String.Format("CARD_DATA/{0}", rw("Field").ToString.Trim))

                        If Not MyXMLNode Is Nothing Then
                            If Not MyXMLNode.ChildNodes(0) Is Nothing Then
                                MyXMLNode.ChildNodes(0).InnerText = dt.Select(String.Format("Field='{0}'", rw("Field").ToString.Trim))(0)("Value")
                                'MyXMLNode.ChildNodes(0).InnerText = dt.Select(String.Format("Field='{0}'", rw("Field").ToString.Trim))(0)("Value").ToString.Replace("&", "&amp;")
                            End If
                        Else
                            'Do whatever 
                        End If ' Save the Xml.
                End Select
            Next

            If SSSNo.Trim <> "" Then
                Dim SSSNo_Element As System.Xml.XmlElement = MyXML.CreateElement("_103")
                SSSNo_Element.InnerText = SSSNo.Trim
                MyXML.DocumentElement.AppendChild(SSSNo_Element)
            End If

            'Console.Write("test")

            MyXML.Save(file)

            Return True
        Catch ex As Exception

            Return False
        End Try

    End Function

    Private Function Hex2Str(ByVal strData As String) As String
        Dim CurrentHex As String = ""
        Dim i As Integer = strData.Length / 2

        Hex2Str = ""

        For index As Integer = 1 To i
            CurrentHex = Mid(strData, index * 2 - 1, 2)
            Hex2Str += ChrW(CInt("&H" + CurrentHex)).ToString
        Next
    End Function

    Private Function Base64ToASCII(ByVal pByteArray() As Byte) As String
        Return Hex2Str(ByteArrayToHexString(pByteArray))
    End Function

    Private Function ByteArrayToHexString(ByVal ByteArray As Byte()) As String
        Dim hStr As String = ""

        For i As Integer = 0 To ByteArray.Length - 1
            hStr += ByteArray(i).ToString("X2")
        Next

        Return hStr
    End Function

    Private Function BuildAddress(ByVal pAddress() As String, ByRef BuildAddressDelimited As String) As String
        Dim tAdd As String = ""
        'Dim tAddDelimited As String = ""

        For i As Integer = 0 To 8
            If pAddress(i).Trim = "" Then
                tAdd += pAddress(i)
                'tAddDelimited += pAddress(i)
            Else
                tAdd += pAddress(i) + " "
                'tAddDelimited += pAddress(i) + "||"
            End If
        Next

        'BuildAddressDelimited = tAddDelimited

        Return tAdd
    End Function

End Class

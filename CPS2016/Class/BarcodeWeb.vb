
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Bitmap
Imports System.Drawing.Graphics
Imports System.IO

Public Class BarcodeWeb

    Private strErrorMessage As String

    Public ReadOnly Property ErrorMessage As String
        Get
            Return strErrorMessage
        End Get
    End Property

    Public Function GenerateBarcode(ByVal strBarcode As String, ByVal outputFile As String) As Boolean
        Try
            'Dim strTempFile As String = String.Format("C:\Allcard\SSS_CPS\Temp1_{0}.jpg", strBarcode)
            Dim _image As System.Drawing.Image = GenCode128.Code128Rendering.MakeBarcodeImage(strBarcode, 2, True)
            _image.Save(outputFile, System.Drawing.Imaging.ImageFormat.Jpeg)
            _image.Dispose()
            _image = Nothing

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GenerateBarcode2(ByVal strBarcode As String, ByVal outputFile As String, ByVal BarWeight As String) As Boolean
        Try
            'Dim strTempFile As String = String.Format("C:\Allcard\SSS_CPS\Temp1_{0}.jpg", strBarcode)
            Dim _image As System.Drawing.Image = GenCode128.Code128Rendering.MakeBarcodeImage(strBarcode, CInt(BarWeight), True)
            _image.Save(outputFile, System.Drawing.Imaging.ImageFormat.Jpeg)
            _image.Dispose()
            _image = Nothing

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    'Public Sub GenerateBarcode2(ByVal valueToEncode As String, ByVal outputFile As String)
    '    'Create an instance of BarcodeProfessional class
    '    Using bcpInfo As New Neodynamic.WebControls.BarcodeProfessional.BarcodeProfessional
    '        'Set the desired barcode type or symbology
    '        bcpInfo.BarHeight = 0.8!
    '        bcpInfo.BarWidth = 0.02!

    '        'bcpInfo.DataMatrixModuleSize = 0.0!
    '        bcpInfo.ForeColor = System.Drawing.Color.Black
    '        bcpInfo.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code128

    '        'Set value to encode
    '        bcpInfo.Code = valueToEncode

    '        Dim strTempFile As String = String.Format("C:\Allcard\SSS_CPS\Temp1_{0}.jpg", valueToEncode)
    '        Dim strTempFile2 As String = String.Format("C:\Allcard\SSS_CPS\Temp2_{0}.jpg", valueToEncode)
    '        'Dim strOutputFile As String = String.Format("C:\Allcard\SSS_CPS\{0}.jpg", valueToEncode)

    '        bcpInfo.Save(strTempFile, System.Drawing.Imaging.ImageFormat.Bmp)

    '        'CheckPixels(strTempFile)

    '        If System.IO.File.Exists(strTempFile2) Then System.IO.File.Delete(strTempFile2)
    '        System.IO.File.Copy(strTempFile, strTempFile2)

    '        Dim bmp As New Bitmap(strTempFile2)
    '        If CropImage(bmp.Width, bmp.Height - 22, strTempFile2, outputFile) Then
    '            Try
    '                bmp.Dispose()
    '                bmp = Nothing

    '                'CheckPixels(strOutputFile)

    '                If System.IO.File.Exists(strTempFile) Then System.IO.File.Delete(strTempFile)
    '                If System.IO.File.Exists(strTempFile2) Then System.IO.File.Delete(strTempFile2)
    '            Catch ex As Exception
    '            End Try
    '        End If

    '        'Generate barcode image
    '        'Dim imgBuffer As Byte() = bcpInfo.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png)

    '        'Dim FS1 As New IO.FileStream("TEST", IO.FileMode.Create)
    '        'FS1.Write(imgBuffer, 0, imgBuffer.Length)
    '        'FS1.Close()
    '        'FS1 = Nothing

    '        'Write image buffer to Response obj
    '        'System.Web.HttpContext.Current.Response.ContentType = "image/png"
    '        'System.Web.HttpContext.Current.Response.BinaryWrite(imgBuffer)
    '    End Using
    'End Sub


    Public Shared Function CropImage(Width As Integer, Height As Integer, sourceFilePath As String, saveFilePath As String) As Boolean
        Try
            ' variable for percentage resize 
            Dim percentageResize As Single = 0
            Dim percentageResizeW As Single = 0
            Dim percentageResizeH As Single = 0

            ' variables for the dimension of source and cropped image 
            Dim sourceX As Integer = 0
            Dim sourceY As Integer = 0
            Dim destX As Integer = 0
            Dim destY As Integer = 0

            ' Create a bitmap object file from source file 
            Dim sourceImage As New Bitmap(sourceFilePath)

            ' Set the source dimension to the variables 
            Dim sourceWidth As Integer = sourceImage.Width
            Dim sourceHeight As Integer = sourceImage.Height

            ' Calculate the percentage resize 
            percentageResizeW = (CSng(Width) / CSng(sourceWidth))
            percentageResizeH = (CSng(Height) / CSng(sourceHeight))

            ' Checking the resize percentage 
            If percentageResizeH < percentageResizeW Then
                percentageResize = percentageResizeW
                destY = System.Convert.ToInt16((Height - (sourceHeight * percentageResize)) / 2)
            Else
                percentageResize = percentageResizeH
                destX = System.Convert.ToInt16((Width - (sourceWidth * percentageResize)) / 2)
            End If

            ' Set the new cropped percentage image
            Dim destWidth As Integer = CInt(Math.Round(sourceWidth * percentageResize))
            Dim destHeight As Integer = CInt(Math.Round(sourceHeight * percentageResize))

            ' Create the image object 
            Using objBitmap As New Bitmap(Width, Height)
                objBitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution)
                Using objGraphics As Graphics = Graphics.FromImage(objBitmap)
                    ' Set the graphic format for better result cropping 
                    objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
                    objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                    objGraphics.DrawImage(sourceImage, New Rectangle(destX, destY, destWidth, destHeight), New Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel)

                    ' Save the file path, note we use png format to support png file 
                    'objBitmap.Save(saveFilePath, Drawing.Imaging.ImageFormat.Png)
                    objBitmap.Save(saveFilePath, Drawing.Imaging.ImageFormat.Jpeg)
                End Using
            End Using

            sourceImage.Dispose()
            sourceImage = Nothing

            Return True
        Catch ex As Exception
            'strErrorMessage = ex.Message
            Return False
        End Try

    End Function

    Private _encoding As Hashtable = New Hashtable
    Private Const _wideBarWidth As Short = 5
    Private Const _narrowBarWidth As Short = 2
    Private Const _barHeight As Short = 100

    Sub BarcodeCode39()
        _encoding.Add("*", "bWbwBwBwb")
        _encoding.Add("-", "bWbwbwBwB")
        _encoding.Add("$", "bWbWbWbwb")
        _encoding.Add("%", "bwbWbWbWb")
        _encoding.Add(" ", "bWBwbwBwb")
        _encoding.Add(".", "BWbwbwBwb")
        _encoding.Add("/", "bWbWbwbWb")
        _encoding.Add("+", "bWbwbWbWb")
        _encoding.Add("0", "bwbWBwBwb")
        _encoding.Add("1", "BwbWbwbwB")
        _encoding.Add("2", "bwBWbwbwB")
        _encoding.Add("3", "BwBWbwbwb")
        _encoding.Add("4", "bwbWBwbwB")
        _encoding.Add("5", "BwbWBwbwb")
        _encoding.Add("6", "bwBWBwbwb")
        _encoding.Add("7", "bwbWbwBwB")
        _encoding.Add("8", "BwbWbwBwb")
        _encoding.Add("9", "bwBWbwBwb")
        _encoding.Add("A", "BwbwbWbwB")
        _encoding.Add("B", "bwBwbWbwB")
        _encoding.Add("C", "BwBwbWbwb")
        _encoding.Add("D", "bwbwBWbwB")
        _encoding.Add("E", "BwbwBWbwb")
        _encoding.Add("F", "bwBwBWbwb")
        _encoding.Add("G", "bwbwbWBwB")
        _encoding.Add("H", "BwbwbWBwb")
        _encoding.Add("I", "bwBwbWBwb")
        _encoding.Add("J", "bwbwBWBwb")
        _encoding.Add("K", "BwbwbwbWB")
        _encoding.Add("L", "bwBwbwbWB")
        _encoding.Add("M", "BwBwbwbWb")
        _encoding.Add("N", "bwbwBwbWB")
        _encoding.Add("O", "BwbwBwbWb")
        _encoding.Add("P", "bwBwBwbWb")
        _encoding.Add("Q", "bwbwbwBWB")
        _encoding.Add("R", "BwbwbwBWb")
        _encoding.Add("S", "bwBwbwBWb")
        _encoding.Add("T", "bwbwBwBWb")
        _encoding.Add("U", "BWbwbwbwB")
        _encoding.Add("V", "bWBwbwbwB")
        _encoding.Add("W", "BWBwbwbwb")
        _encoding.Add("X", "bWbwBwbwB")
        _encoding.Add("Y", "BWbwBwbwb")
        _encoding.Add("Z", "bWBwBwbwb")
    End Sub

    Public Function GenerateCode39Barcode(ByVal strBarcode As String, ByVal outputFile As String) As Boolean
        BarcodeCode39()
        'Dim barcode As String = String.Empty
        
        Return GenerateBarcodeImage(1038, 140, strBarcode, outputFile)
    End Function

    Protected Function getBCSymbolColor(ByVal symbol As String) As System.Drawing.Brush
        getBCSymbolColor = Brushes.Black
        If symbol = "W" Or symbol = "w" Then
            getBCSymbolColor = Brushes.White
        End If
    End Function

    Protected Function getBCSymbolWidth(ByVal symbol As String) As Short
        getBCSymbolWidth = _narrowBarWidth
        If symbol = "B" Or symbol = "W" Then
            getBCSymbolWidth = _wideBarWidth
        End If
    End Function

    Protected Overridable Function GenerateBarcodeImage(ByVal imageWidth As Short, ByVal imageHeight As Short, ByVal Code As String, ByVal outputFile As String) As Boolean
        Try
            'create a new bitmap
            Dim b As New Bitmap(imageWidth, imageHeight, Imaging.PixelFormat.Format32bppArgb)

            'create a canvas to paint on
            Dim canvas As New Rectangle(0, 0, imageWidth, imageHeight)

            'draw a white background
            Dim g As Graphics = Graphics.FromImage(b)
            g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight)

            'write the unaltered code at the bottom
            'TODO: truely center this text
            Dim textBrush As New SolidBrush(Color.Black)
            g.DrawString(Code, New Font("Courier New", 12), textBrush, 100, 110)
            'g.DrawString(Code, New Font("Courier New", 12), textBrush, 250, 140)

            'Code has to be surrounded by asterisks to make it a valid Code39 barcode
            Dim UseCode As String = String.Format("{0}{1}{0}", "*", Code)

            'Start drawing at 10, 10
            Dim XPosition As Short = 10
            Dim YPosition As Short = 10

            Dim invalidCharacter As Boolean = False
            Dim CurrentSymbol As String = String.Empty

            For j As Short = 0 To CShort(UseCode.Length - 1)
                CurrentSymbol = UseCode.Substring(j, 1)
                'check if symbol can be used
                If Not IsNothing(_encoding(CurrentSymbol)) Then
                    Dim EncodedSymbol As String = _encoding(CurrentSymbol).ToString

                    For i As Short = 0 To CShort(EncodedSymbol.Length - 1)
                        Dim CurrentCode As String = EncodedSymbol.Substring(i, 1)
                        g.FillRectangle(getBCSymbolColor(CurrentCode), XPosition, YPosition, getBCSymbolWidth(CurrentCode), _barHeight)
                        XPosition = XPosition + getBCSymbolWidth(CurrentCode)
                    Next

                    'After each written full symbol we need a whitespace (narrow width)
                    g.FillRectangle(getBCSymbolColor("w"), XPosition, YPosition, getBCSymbolWidth("w"), _barHeight)
                    XPosition = XPosition + getBCSymbolWidth("w")
                Else
                    invalidCharacter = True
                End If
            Next

            'errorhandling when an invalidcharacter is found
            If invalidCharacter Then
                g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight)
                g.DrawString("Invalid characters found,", New Font("Courier New", 8), textBrush, 0, 0)
                g.DrawString("no barcode generated", New Font("Courier New", 8), textBrush, 0, 10)
                g.DrawString("Input was: ", New Font("Courier New", 8), textBrush, 0, 30)
                g.DrawString(Code, New Font("Courier New", 8), textBrush, 0, 40)
            End If

            'write the image into a memorystream
            Dim ms As New MemoryStream

            Dim encodingParams As New EncoderParameters
            encodingParams.Param(0) = New EncoderParameter(Encoder.Quality, 100)

            Dim encodingInfo As ImageCodecInfo = FindCodecInfo("JPEG")

            b.Save(ms, encodingInfo, encodingParams)

            'Dim FS1 As New IO.FileStream(outputFile, IO.FileMode.Create)
            'FS1.Write(ms.ToArray, 0, ms.Length)
            'FS1.Close()
            'FS1 = Nothing

            Dim fs As FileStream = New FileStream(outputFile, FileMode.Create, FileAccess.Write)
            ms.WriteTo(fs)
            fs.Close()

            CropImage(410, 28, outputFile, outputFile.Replace(".jpg", "2.jpg"))

            'b.Save(outputFile, encodingInfo, encodingParams)

            'dispose of the object we won't need any more
            g.Dispose()
            b.Dispose()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Overridable Function FindCodecInfo(ByVal codec As String) As ImageCodecInfo
        Dim encoders As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders
        For Each e As ImageCodecInfo In encoders
            If e.FormatDescription.Equals(codec) Then Return e
        Next
        Return Nothing
    End Function



End Class

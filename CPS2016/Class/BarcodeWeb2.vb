
Imports Neodynamic.WebControls.BarcodeProfessional
Imports System.Drawing
Imports System.IO

Public Class BarcodeWeb2

    Private strErrorMessage As String

    Public ReadOnly Property ErrorMessage As String
        Get
            Return strErrorMessage
        End Get
    End Property

    Public Function GenerateBarcode(ByVal valueToEncode As String, ByVal outputFile As String) As Boolean
        Try
            'Create an instance of BarcodeProfessional class
            Using bcpInfo As New BarcodeProfessional()
                'Set the desired barcode type or symbology
                bcpInfo.BarHeight = 0.8!
                bcpInfo.BarWidth = 0.02!

                bcpInfo.ForeColor = System.Drawing.Color.Black
                bcpInfo.Symbology = Symbology.Code128

                'Set value to encode
                bcpInfo.Code = valueToEncode

                Dim strTempFile As String = String.Format("{0}\Temp1_{1}_{2}.jpg", SharedFunction.BarcodeRepository, valueToEncode, Now.ToString("hhmmss"))
                Dim strTempFile2 As String = String.Format("{0}\Temp2_{1}_{2}.jpg", SharedFunction.BarcodeRepository, valueToEncode, Now.ToString("hhmmss"))

                bcpInfo.Save(strTempFile, System.Drawing.Imaging.ImageFormat.Bmp)

                'CheckPixels(strTempFile)

                If File.Exists(strTempFile2) Then File.Delete(strTempFile2)
                File.Copy(strTempFile, strTempFile2)

                Dim bmp As New Bitmap(strTempFile2)
                If CropImage(bmp.Width, bmp.Height - 70, strTempFile2, outputFile) Then
                    Try
                        bmp.Dispose()
                        bmp = Nothing

                        Try
                            If File.Exists(strTempFile) Then File.Delete(strTempFile)
                            If File.Exists(strTempFile2) Then File.Delete(strTempFile2)
                            If File.Exists(strTempFile.Replace(".jpg", "2.jpg")) Then File.Delete(strTempFile.Replace(".jpg", "2.jpg"))
                        Catch ex3 As Exception
                        End Try
                    Catch ex2 As Exception
                    End Try
                End If
            End Using

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GenerateBarcodeCode39(ByVal valueToEncode As String, ByVal outputFile As String) As Boolean
        Try
            'Create an instance of BarcodeProfessional class
            Using bcpInfo As New BarcodeProfessional()
                'Set the desired barcode type or symbology
                bcpInfo.BarHeight = 0.8!
                bcpInfo.BarWidth = 0.02!

                bcpInfo.ForeColor = System.Drawing.Color.Black
                bcpInfo.Symbology = Symbology.Code39

                'Set value to encode
                bcpInfo.Code = valueToEncode

                Dim strTempFile As String = String.Format("{0}\Temp1_{1}_{2}.jpg", SharedFunction.BarcodeRepository, valueToEncode, Now.ToString("hhmmss"))
                Dim strTempFile2 As String = String.Format("{0}\Temp2_{1}_{2}.jpg", SharedFunction.BarcodeRepository, valueToEncode, Now.ToString("hhmmss"))

                bcpInfo.Save(strTempFile, System.Drawing.Imaging.ImageFormat.Bmp)

                'CheckPixels(strTempFile)

                If File.Exists(strTempFile2) Then File.Delete(strTempFile2)
                File.Copy(strTempFile, strTempFile2)

                Dim bmp As New Bitmap(strTempFile2)
                If CropImage(bmp.Width, bmp.Height - 70, strTempFile2, outputFile) Then
                    Try
                        bmp.Dispose()
                        bmp = Nothing

                        Try
                            If File.Exists(strTempFile) Then File.Delete(strTempFile)
                            If File.Exists(strTempFile2) Then File.Delete(strTempFile2)
                            If File.Exists(strTempFile.Replace(".jpg", "2.jpg")) Then File.Delete(strTempFile.Replace(".jpg", "2.jpg"))
                        Catch ex3 As Exception
                        End Try
                    Catch ex2 As Exception
                    End Try
                End If
            End Using

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Private Function CheckPixels(ByVal strFile As String) As Boolean
        Dim sb As New StringBuilder
        Dim x, y As Integer
        Using image1 As New Bitmap(strFile)
            For x = 0 To image1.Width - 1
                For y = 0 To image1.Height - 1
                    '161,41
                    '161,43
                    '162,41
                    '162,43
                    If x = 161 And y = 41 Then
                        image1.SetPixel(x, y, Color.White)
                    ElseIf x = 161 And y = 43 Then
                        image1.SetPixel(x, y, Color.White)
                    ElseIf x = 162 And y = 41 Then
                        image1.SetPixel(x, y, Color.White)
                    ElseIf x = 162 And y = 43 Then
                        image1.SetPixel(x, y, Color.White)
                    End If

                    'sb.AppendLine(image1.GetPixel(x, y).ToString)
                    image1.Save(strFile.Replace(".jpg", "2.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg)
                Next
            Next
        End Using
    End Function

    Private Function CropImage(Width As Integer, Height As Integer, sourceFilePath As String, saveFilePath As String) As Boolean
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
            '_errormessage = ex.Message
            Return False
        End Try

    End Function

End Class

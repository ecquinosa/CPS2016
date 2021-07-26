
Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core

Class FileCompression

    Private _ErrorMessage As String

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
    End Property

    Public Function Compress(directoryPath As String, ByRef strZipFile As String) As Boolean
        Try
            ' Depending on the directory this could be very large and would require more attention
            ' in a commercial package.
            Dim filenames As String() = Directory.GetFiles(directoryPath)

            strZipFile = directoryPath & Convert.ToString(".zip")

            ' 'using' statements guarantee the stream is closed properly which is a big source
            ' of problems otherwise.  Its exception safe as well which is great.
            Using s As New ZipOutputStream(File.Create(strZipFile))

                s.SetLevel(9)
                ' 0 - store only to 9 - means best compression
                Dim buffer As Byte() = New Byte(4095) {}

                For Each file__1 As String In filenames

                    ' Using GetFileName makes the result compatible with XP
                    ' as the resulting path is not absolute.
                    Dim entry As New ZipEntry(Path.GetFileName(file__1))

                    ' Setup the entry data as required.

                    ' Crc and size are handled by the library for seakable streams
                    ' so no need to do them here.

                    ' Could also use the last write time or similar for the file.
                    entry.DateTime = System.DateTime.Now
                    s.PutNextEntry(entry)

                    Using fs As FileStream = File.OpenRead(file__1)

                        ' Using a fixed size buffer here makes no noticeable difference for output
                        ' but keeps a lid on memory usage.
                        Dim sourceBytes As Integer
                        Do
                            sourceBytes = fs.Read(buffer, 0, buffer.Length)
                            s.Write(buffer, 0, sourceBytes)
                        Loop While sourceBytes > 0
                    End Using
                Next

                ' Finish/Close arent needed strictly as the using statement does this automatically

                ' Finish is important to ensure trailing information for a Zip file is appended.  Without this
                ' the created file would be invalid.
                s.Finish()

                ' Close is important to wrap things up and unlock the file.
                s.Close()

                Return True
            End Using
        Catch ex As System.Exception
            'Utilities.SaveToErrorLog(Utilities.TimeStamp() + String.Format("Failed to compress {0}", Utilities.SessionReference()))
            Return False
        End Try
    End Function

    Public Function addFolderToZip(ByVal f As ZipFile, ByVal root As String, ByVal folder As String) As Boolean
        Try
            Dim relative As String = folder.Substring(root.Length)
            If relative.Length > 0 Then
                f.AddDirectory(relative)
            End If
            For Each file As String In Directory.GetFiles(folder)
                relative = file.Substring(root.Length)
                f.Add(file, relative)
            Next
            'For Each subFolder As String In Directory.GetDirectories(folder)
            '    Me.addFolderToZip(f, root, subFolder)
            'Next

            Return True
        Catch ex As Exception
            _ErrorMessage = ex.Message
            Return False
        End Try
       
    End Function

    Public Function UpdateExistingZip(ByVal strZipFile As String, ByVal strDirectory As String, ByRef sb As StringBuilder) As Integer
        Dim intError As Integer = 0

        Dim zipFile As New ZipFile(strZipFile)

        ' Must call BeginUpdate to start, and CommitUpdate at the end.
        zipFile.BeginUpdate()

        'zipFile.Password = "whatever"
        ' Only if a password is wanted on the new entry
        ' The "Add()" method will add or overwrite as necessary.
        ' When the optional entryName parameter is omitted, the entry will be named
        ' with the full folder path and without the drive e.g. "temp/folder/test1.txt".
        '
        'zipFile.Add("c:\temp\folder\test1.txt")

        ' Specify the entryName parameter to modify the name as it appears in the zip.
        '
        Try
            For Each strFile As String In Directory.GetFiles(strDirectory)
                zipFile.Add(strFile, Path.GetFileName(strFile))
            Next
        Catch ex As Exception
            intError += 1
            sb.AppendLine(String.Format("UpdateExistingZip(): Runtime error encountered {0}", ex.Message))
        End Try

        ' Continue calling .Add until finished.

        ' Both CommitUpdate and Close must be called.
        zipFile.CommitUpdate()
        zipFile.Close()

        Return intError
    End Function

    Public Function ExtractZipFile(ByVal strZipFile As String, ByVal strDirectory As String) As Boolean
        Dim zf As ZipFile
        Dim fs As FileStream = File.OpenRead(strZipFile)
        Try
            zf = New ZipFile(fs)
            'If Not [String].IsNullOrEmpty(password) Then
            '    ' AES encrypted entries are handled automatically
            '    zf.Password = password
            'End If

            Dim intCntr As Integer = 1

            For Each zipEntry As ZipEntry In zf
                If Not zipEntry.IsFile Then
                    ' Ignore directories
                    Continue For
                End If
                Dim entryFileName As [String] = zipEntry.Name
                ' to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                ' Optionally match entrynames against a selection list here to skip as desired.
                ' The unpacked length is available in the zipEntry.Size property.

                'If intCntr = 1221 Then
                '    Console.WriteLine("TEST")
                'End If

                If entryFileName.Contains("0120160114H3ID168004") Then
                    Console.WriteLine("TEST")
                End If

                Dim buffer As Byte() = New Byte(4095) {}
                ' 4K is optimum
                Dim zipStream As Stream = zf.GetInputStream(zipEntry)

                ' Manipulate the output filename here as desired.
                Dim fullZipToPath As [String] = Path.Combine(strDirectory, entryFileName)
                Dim directoryName As String = Path.GetDirectoryName(fullZipToPath)
                If directoryName.Length > 0 Then
                    Directory.CreateDirectory(directoryName)
                End If

                ' Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                ' of the file, but does not waste memory.
                ' The "using" will close the stream even if an exception occurs.
                Using streamWriter As FileStream = File.Create(fullZipToPath)
                    StreamUtils.Copy(zipStream, streamWriter, buffer)
                    'System.Threading.Thread.Sleep(100)
                End Using

                intCntr += 1
            Next

            Return True
        Catch ex As Exception
            _ErrorMessage = ex.Message
            Return False
        Finally
            If zf IsNot Nothing Then
                zf.IsStreamOwner = True
                ' Makes close also shut the underlying stream
                ' Ensure we release resources
                'fs.Close()
                zf.Close()
                'fs = Nothing
                zf = Nothing
            End If
        End Try
    End Function

End Class


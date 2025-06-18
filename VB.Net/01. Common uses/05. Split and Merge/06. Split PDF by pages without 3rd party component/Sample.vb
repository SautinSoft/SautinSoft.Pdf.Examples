Option Infer On

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Diagnostics
Imports System.IO.Compression

Namespace Sample
    Friend Class Sample
        ''' <summary>
        ''' Split PDF by pages without 3rd party component in C# and .NET
        ''' </summary>
        ''' <remarks>
        ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/split-PDF-by-pages-without-3rd-party-component-csharp-dotnet.php
        ''' </remarks>
        Shared Sub Main(ByVal args() As String)
            ' C# example to split PDF document by pages without any 3rd party components.

            ' Advantages:
            ' + Completely C# code, no dependencies, no 3rd party references.

            ' Disadvantages:
            ' - Uses Regual expressions
            ' - May fault with complex PDF files (especially new PDF 1.7, PDF 2.0).

            ' If want to use 3rd party component, try our SautinSoft.Pdf:
            ' https://sautinsoft.com/products/pdf/help/net/developer-guide/split-pdf-files.php
            ' It's also completely FREE for Split and Merge operations.

            Dim inpFile As String = Path.GetFullPath("..\..\..\005.pdf")
            Dim outDir As New DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Pages"))
            If outDir.Exists Then
                outDir.Delete(True)
            End If

            outDir.Create()

            Dim pp As New ProjectPart()
            Try
                pp.Load(inpFile)
            Catch e As Exception
                Console.WriteLine($"Error! Can't load input file: {inpFile}")
                Console.WriteLine(e.Message)
                Return
            End Try
            Dim Pages() As Integer = pp.Pages
            For Each page As Integer In Pages
                Dim filename As String = Path.GetFileName(pp.path)
                filename = Regex.Replace(filename, ".pdf", "-" & (page + 1).ToString("00000") & ".pdf", RegexOptions.IgnoreCase)
                Dim w As FileStream = File.OpenWrite(Path.Combine(outDir.FullName, filename))
                Dim psm As New PdfSplitterMerger(w)
                Dim f As FileStream = File.OpenRead(pp.path)
                psm.Add(f, New Integer() {page})
                psm.Finish()
                f.Close()
                w.Close()
            Next page
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outDir.FullName) With {.UseShellExecute = True})
        End Sub
    End Class
    Friend Class ProjectPart
        Private Sub OnProgress(ByVal part As Integer, ByVal total As Integer)
        End Sub
        Friend Event ProgressEvent As ProgressDelegate
        Public path As String
        'INSTANT VB NOTE: The field pages was renamed since Visual Basic does not allow fields to have the same name as other class members:
        Friend pages_Conflict As String
        Public ReadOnly Property Pages() As Integer()
            Get
                Dim ps As New ArrayList()
                If Me.pages_Conflict Is Nothing OrElse pages_Conflict.Length = 0 Then
                    For index As Integer = 0 To Me.MaxPage - 1
                        ps.Add(index)
                    Next index
                Else
                    Dim ss() As String = Me.pages_Conflict.Split(New Char() {","c, " "c, ";"c})
                    For Each s As String In ss
                        If Regex.IsMatch(s, "\d+-\d+") Then
                            Dim start As Integer = Integer.Parse(s.Split(New Char() {"-"c})(0))
                            Dim [end] As Integer = Integer.Parse(s.Split(New Char() {"-"c})(1))
                            If start > [end] Then
                                Return New Integer() {0}
                            End If
                            Do While start <= [end]
                                ps.Add(start - 1)
                                start += 1
                            Loop
                        Else
                            ps.Add(Integer.Parse(s) - 1)
                        End If
                    Next s
                End If
                Return TryCast(ps.ToArray(GetType(Integer)), Integer())
            End Get
        End Property
        Public MaxPage As Integer
        Public Sub Load(ByVal path As String)
            Me.path = path
            Dim s As System.IO.Stream = File.OpenRead(path)
            Dim pf As New PdfFile(s)
            AddHandler pf.ProgressEvent, AddressOf pf_ProgressEvent
            pf.Load()
            Me.MaxPage = pf.PageCount
            s.Close()
        End Sub
        Public Sub LoadFromBytes(ByVal pdf() As Byte)

            Dim s As System.IO.Stream = New MemoryStream(pdf)
            Dim pf As New PdfFile(s)
            AddHandler pf.ProgressEvent, AddressOf pf_ProgressEvent
            pf.Load()
            Me.MaxPage = pf.PageCount
            s.Close()
        End Sub

        Public Sub New()
            Me.ProgressEventEvent = New ProgressDelegate(AddressOf Me.OnProgress)
        End Sub

        Private Sub pf_ProgressEvent(ByVal part As Integer, ByVal total As Integer)
            RaiseEvent ProgressEvent(part, total)
        End Sub
    End Class
    Friend Delegate Sub ProgressDelegate(ByVal part As Integer, ByVal total As Integer)
    Friend Class PdfSplitterMerger
        Private target As System.IO.Stream
        Private pos As Long = 15
        Private number As Integer = 3
        Private pageNumbers, xrefs As ArrayList
        Private Sub OnProgress(ByVal part As Integer, ByVal total As Integer)
        End Sub
        Public Event ProgressEvent As ProgressDelegate
        Public Sub New(ByVal OutputStream As System.IO.Stream)
            Me.ProgressEventEvent = New ProgressDelegate(AddressOf Me.OnProgress)
            Me.xrefs = New ArrayList()
            Me.pageNumbers = New ArrayList()
            Me.target = OutputStream
            Dim sw As New StreamWriter(Me.target)
            Dim versionstring As String = "PDF-1.4"
            sw.Write("%" & versionstring & vbCr)
            sw.Flush()
            Dim buffer(6) As Byte
            buffer(0) = &H25
            buffer(1) = &HE2
            buffer(2) = &HE3
            buffer(3) = &HCF
            buffer(4) = &HD3
            buffer(5) = &HD
            buffer(6) = &HA
            Me.target.Write(buffer, 0, buffer.Length)
            Me.target.Flush()
        End Sub
        Public Sub Add(ByVal PdfInputStream As System.IO.Stream, ByVal PageNumbers() As Integer)
            Dim pf As New PdfFile(PdfInputStream)
            AddHandler pf.ProgressEvent, AddressOf pf_ProgressEvent
            pf.Load()
            Dim ps As New PdfSplitter()
            AddHandler ps.ProgressEvent, AddressOf pf_ProgressEvent
            ps.Load(pf, PageNumbers, Me.number)
            Me.Add(ps)
        End Sub
        Private Sub Add(ByVal PdfSplitter As PdfSplitter)
            For Each pageNumber As Integer In PdfSplitter.pageNumbers
                Me.pageNumbers.Add(PdfSplitter.transHash(pageNumber))
            Next pageNumber
            Dim sortedObjects As New ArrayList()
            For Each pfo As PdfFileObject In PdfSplitter.sObjects.Values
                sortedObjects.Add(pfo)
            Next pfo
            sortedObjects.Sort(New PdfFileObjectNumberComparer())

            For Each pfo As PdfFileObject In sortedObjects
                Me.xrefs.Add(pos)
                Me.pos += pfo.WriteToStream(Me.target)
                Me.number += 1
            Next pfo
        End Sub
        Public Sub Finish()
            Dim sw As New StreamWriter(Me.target)

            Dim root As String = ""
            root = "1 0 obj" & vbCr
            root &= "<< " & vbCr & "/Type /Catalog " & vbCr
            root &= "/Pages 2 0 R " & vbCr
            root &= ">> " & vbCr
            root &= "endobj" & vbCr

            xrefs.Insert(0, pos)
            pos += root.Length
            sw.Write(root)

            Dim pages As String = ""
            pages &= "2 0 obj " & vbCr
            pages &= "<< " & vbCr
            pages &= "/Type /Pages " & vbCr
            pages &= "/Count " & pageNumbers.Count & " " & vbCr
            pages &= "/Kids [ "
            For Each pageIndex As Integer In pageNumbers
                pages &= pageIndex & " 0 R "
            Next pageIndex
            pages &= "] " & vbCr
            pages &= ">> " & vbCr
            pages &= "endobj" & vbCr

            xrefs.Insert(1, pos)
            pos += pages.Length
            sw.Write(pages)


            sw.Write("xref" & vbCr)
            sw.Write("0 " & (Me.number) & " " & vbCr)
            sw.Write("0000000000 65535 f " & vbCr)

            For Each xref As Long In Me.xrefs
                sw.Write((xref + 1).ToString("0000000000") & " 00000 n " & vbCr)
            Next xref
            sw.Write("trailer" & vbCr)
            sw.Write("<<" & vbCr)
            sw.Write("/Size " & (Me.number) & vbCr)
            sw.Write("/Root 1 0 R " & vbCr)
            sw.Write(">>" & vbCr)
            sw.Write("startxref" & vbCr)
            sw.Write((pos + 1) & vbCr)
            sw.Write("%%EOF" & vbCr)
            sw.Flush()
            sw.Close()
        End Sub

        Private Sub pf_ProgressEvent(ByVal part As Integer, ByVal total As Integer)
            RaiseEvent ProgressEvent(part, total)
        End Sub
    End Class
    Friend Class Project
        Public Name As String
        Public Parts As ArrayList
        Public ReadOnly Property TotalPages() As Integer
            Get
                Dim tot As Integer = 0
                For Each pp As ProjectPart In Me.Parts
                    If pp.Pages Is Nothing Then
                        tot += pp.MaxPage
                    Else
                        tot += pp.Pages.Length
                    End If
                Next pp
                Return tot
            End Get
        End Property
        Public Sub New(ByVal name As String)
            Me.Parts = New ArrayList()
            Me.Name = name
        End Sub
    End Class
    Friend Class PdfSplitter
        Friend sObjects As Hashtable
        Friend pageNumbers As ArrayList
        Friend transHash As Hashtable
        Friend PdfFile As PdfFile
        Private Sub OnProgress(ByVal part As Integer, ByVal total As Integer)
        End Sub
        Public Event ProgressEvent As ProgressDelegate
        Friend Sub New()
            Me.ProgressEventEvent = New ProgressDelegate(AddressOf Me.OnProgress)
        End Sub
        Friend Sub Load(ByVal PdfFile As PdfFile, ByVal PageNumbers() As Integer, ByVal startNumber As Integer)
            Me.PdfFile = PdfFile
            Me.pageNumbers = New ArrayList()
            Me.sObjects = New Hashtable()
            Dim part As Integer = 0
            Dim total As Integer = PageNumbers.Length
            For Each PageNumber As Integer In PageNumbers
                RaiseEvent ProgressEvent(part, total)
                Dim page As PdfFileObject = TryCast(PdfFile.PageList(PageNumber), PdfFileObject)
                page.PopulateRelatedObjects(PdfFile, Me.sObjects)
                Me.pageNumbers.Add(page.number)
                part += 1
            Next PageNumber
            Me.transHash = Me.CalcTransHash(startNumber)
            For Each pfo As PdfFileObject In Me.sObjects.Values
                pfo.Transform(transHash)
            Next pfo
        End Sub
        Private Function CalcTransHash(ByVal startNumber As Integer) As Hashtable
            Dim ht As New Hashtable()
            Dim al As New ArrayList()
            For Each pfo As PdfFileObject In Me.sObjects.Values
                al.Add(pfo)
            Next pfo
            al.Sort(New PdfFileObjectNumberComparer())
            Dim number As Integer = startNumber
            For Each pfo As PdfFileObject In al
                ht.Add(pfo.number, number)
                number += 1
            Next pfo
            Return ht
        End Function

    End Class
    Friend Enum PdfObjectType
        UnKnown
        Stream
        Page
        Other
    End Enum
    Friend Class PdfFileStreamObject
        Inherits PdfFileObject

        Private streamBuffer() As Byte
        Private streamStartOffset, streamLength As Integer
        Friend Sub New(ByVal obj As PdfFileObject)
            Me.address = obj.address
            Me.length = obj.length
            Me.text = obj.text
            Me.number = obj.number
            Me.PdfFile = obj.PdfFile
            Me.LoadStreamBuffer()
        End Sub

        Private Sub LoadStreamBuffer()
            Dim m1 As Match = Regex.Match(Me.text, "stream\s*")
            Me.streamStartOffset = m1.Index + m1.Value.Length
            Me.streamLength = Me.length - Me.streamStartOffset
            Me.streamBuffer = New Byte(Me.streamLength - 1) {}
            Me.PdfFile.memory.Seek(Me.address + Me.streamStartOffset, SeekOrigin.Begin)
            Me.PdfFile.memory.Read(Me.streamBuffer, 0, Me.streamLength)

            Me.PdfFile.memory.Seek(Me.address, SeekOrigin.Begin)
            Dim sr As New StreamReader(Me.PdfFile.memory)
            Dim startChars(Me.streamStartOffset - 1) As Char
            sr.ReadBlock(startChars, 0, Me.streamStartOffset)
            Dim sb As New StringBuilder()
            sb.Append(startChars)
            Me.text = sb.ToString()
        End Sub
        Friend Overrides Sub Transform(ByVal TransformationHash As System.Collections.Hashtable)
            MyBase.Transform(TransformationHash)
        End Sub
        Friend Overrides Function WriteToStream(ByVal Stream As System.IO.Stream) As Long
            Dim sw As New StreamWriter(Stream)
            sw.Write(Me.text)
            sw.Flush()
            Call (New MemoryStream(Me.streamBuffer)).WriteTo(Stream)
            sw.Flush()
            Return Me.streamLength + Me.text.Length
        End Function
    End Class

    Friend Class PdfFileObject
        Friend address As Long
        Friend number, length As Integer
        Friend text As String
        Friend PdfFile As PdfFile
        'INSTANT VB NOTE: The field filterEval was renamed since Visual Basic does not allow fields to have the same name as other class members:
        Private filterEval_Conflict As MatchEvaluator
        Friend Shared Function Create(ByVal PdfFile As PdfFile, ByVal number As Integer, ByVal address As Long) As PdfFileObject
            Dim pfo As New PdfFileObject()
            pfo.PdfFile = PdfFile
            pfo.number = number
            pfo.address = address
            pfo.GetLenght(PdfFile)
            pfo.LoadText()
            If pfo.Type = PdfObjectType.Stream Then
                pfo = New PdfFileStreamObject(pfo)
            End If
            pfo.filterEval_Conflict = New MatchEvaluator(AddressOf pfo.FilterEval)
            Return pfo
        End Function

        Friend Shared Function Create(ByVal PdfFile As PdfFile, ByVal number As Integer, ByVal initial As String) As PdfFileObject
            Dim pfo As New PdfFileObject()
            pfo.PdfFile = PdfFile
            pfo.number = number
            pfo.address = -1
            pfo.length = -1
            pfo.text = pfo.number & " 0 obj" & vbLf & initial & vbLf & "endobj" & vbLf
            If pfo.Type = PdfObjectType.Stream Then
                pfo = New PdfFileStreamObject(pfo)
            End If
            pfo.filterEval_Conflict = New MatchEvaluator(AddressOf pfo.FilterEval)
            Return pfo
        End Function


        Private Sub LoadText()
            Me.PdfFile.memory.Seek(Me.address, SeekOrigin.Begin)
            Dim sb As New StringBuilder()
            For index As Integer = 0 To Me.length - 1
                sb.Append(ChrW(Me.PdfFile.memory.ReadByte()))
            Next index
            Me.text = sb.ToString()
        End Sub
        Private Sub GetLenght(ByVal PdfFile As PdfFile)
            Dim stream As System.IO.Stream = PdfFile.memory
            stream.Seek(Me.address, SeekOrigin.Begin)
            Dim m As Match = Regex.Match("", "endobj\s*")
            Dim b As Integer = 0
            Me.length = 0
            Dim word As String = ""
            Do While b <> -1
                b = stream.ReadByte()
                Me.length += 1
                If b > 97 AndAlso b < 112 Then
                    Dim c As Char = ChrW(b)
                    word &= c
                    If word = "endobj" Then
                        b = -1
                    End If
                Else
                    word = ""
                End If
            Loop
            Dim c2 As Char = ChrW(stream.ReadByte())
            Do While Regex.IsMatch(c2.ToString(), "\s")
                Me.length += 1
                c2 = ChrW(stream.ReadByte())
            Loop
        End Sub
        'INSTANT VB NOTE: The field type was renamed since Visual Basic does not allow fields to have the same name as other class members:
        Protected type_Conflict As PdfObjectType
        Friend ReadOnly Property Type() As PdfObjectType
            Get
                If Me.type_Conflict = PdfObjectType.UnKnown Then
                    If Regex.IsMatch(Me.text, "/Page") And Not Regex.IsMatch(Me.text, "/Pages") Then
                        Me.type_Conflict = PdfObjectType.Page
                        Return Me.type_Conflict
                    End If
                    If Regex.IsMatch(Me.text, "stream") Then
                        Me.type_Conflict = PdfObjectType.Stream
                        Return Me.type_Conflict
                    End If
                    Me.type_Conflict = PdfObjectType.Other
                End If
                Return Me.type_Conflict
            End Get
        End Property

        Friend Function GetNumber(ByVal Name As String) As Integer
            Dim m As Match = Regex.Match(Me.text, "/" & Name & " (?'num'\d+)", RegexOptions.ExplicitCapture)
            Dim len As Integer = 0
            If m.Success Then
                len = Integer.Parse(m.Groups("num").Value)
            End If
            Return len
        End Function

        Friend Function GetArrayNumbers(ByVal arrayName As String) As Integer()
            Dim ids As New ArrayList()
            Dim pattern As String = "/" & arrayName & "\s*\[(\s*(?'id'\d+) 0 R\s*)*"
            Dim m As Match = Regex.Match(Me.text, pattern, RegexOptions.ExplicitCapture)
            For Each cap As Capture In m.Groups("id").Captures
                ids.Add(Integer.Parse(cap.Value))
            Next cap
            Return TryCast(ids.ToArray(GetType(Integer)), Integer())
        End Function
        Friend Function GetKids() As ArrayList
            Dim kids As New ArrayList()
            For Each id As Integer In Me.GetArrayNumbers("Kids")
                Dim pfo As PdfFileObject = PdfFile.LoadObject(id)
                If pfo.Type = PdfObjectType.Page Then
                    kids.Add(pfo)
                Else
                    kids.AddRange(pfo.GetKids())
                End If
            Next id
            Return kids
        End Function

        Friend Sub PopulateRelatedObjects(ByVal PdfFile As PdfFile, ByVal container As Hashtable)
            If Not container.ContainsKey(Me.number) Then
                container.Add(Me.number, Me)
                Dim m As Match = Regex.Match(Me.text, "(?'parent'(/Parent)*)\s*(?'id'\d+) 0 R[^G]", RegexOptions.ExplicitCapture)
                Do While m.Success
                    Dim num As Integer = Integer.Parse(m.Groups("id").Value)
                    Dim notparent As Boolean = m.Groups("parent").Length = 0
                    If notparent And Not container.Contains(num) Then
                        Dim pfo As PdfFileObject = PdfFile.LoadObject(num)
                        If pfo IsNot Nothing And Not container.Contains(pfo.number) Then
                            pfo.PopulateRelatedObjects(PdfFile, container)
                        End If
                    End If
                    m = m.NextMatch()
                Loop
            End If
        End Sub

        Private TransformationHash As Hashtable
        Private Function FilterEval(ByVal m As Match) As String
            Dim id As Integer = Integer.Parse(m.Groups("id").Value)
            Dim [end] As String = m.Groups("end").Value
            If Me.TransformationHash.ContainsKey(id) Then
                Dim rest As String = m.Groups("rest").Value
                Return DirectCast(TransformationHash(id), Integer).ToString() + rest & [end]
            End If
            Return [end]
        End Function
        Friend ReadOnly Property Parent() As PdfFileObject
            Get
                Return Me.PdfFile.LoadObject(Me.text, "Parent")
            End Get
        End Property
        Friend ReadOnly Property MediaBoxText() As String
            Get
                Dim pattern As String = "/MediaBox\s*\[\s*(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s*]"
                Dim mediabox As String = Regex.Match(Me.text, pattern).Value

                'link
                If mediabox = "" Then
                    pattern = "/MediaBox\s*\d+"
                    mediabox = Regex.Match(Me.text, pattern).Value
                    If mediabox <> "" Then
                        mediabox = mediabox.Remove(0, "/MediaBox".Length)
                        mediabox = mediabox.Trim()
                        Dim obj As Integer = Convert.ToInt32(mediabox, 10)

                        If obj > 0 AndAlso obj < Me.PdfFile.objects.Count Then
                            pattern = "\[\s*(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s*]"
                            Dim text As String = DirectCast(Me.PdfFile.objects(obj), PdfFileObject).text
                            mediabox = Regex.Match(text, pattern).Value
                            mediabox = "/MediaBox " & mediabox
                        End If
                    End If

                End If
                Return mediabox
            End Get
        End Property
        Friend Overridable Sub Transform(ByVal TransformationHash As Hashtable)
            If Me.Type = PdfObjectType.Page AndAlso Me.MediaBoxText = "" Then
                'INSTANT VB NOTE: The variable parent was renamed since Visual Basic does not handle local variables named the same as class members well:
                Dim parent_Conflict As PdfFileObject = Me.Parent
                Do While parent_Conflict IsNot Nothing
                    Dim mb As String = parent_Conflict.MediaBoxText
                    If mb = "" Then
                        parent_Conflict = parent_Conflict.Parent
                    Else
                        Me.text = Regex.Replace(Me.text, "/Type\s*/Page", "/Type /Page" & vbCr & mb)
                        parent_Conflict = Nothing
                    End If
                Loop
            End If
            Me.TransformationHash = TransformationHash
            Me.text = Regex.Replace(Me.text, "(?'id'\d+)(?'rest' 0 (obj|R))(?'end'[^G])", Me.filterEval_Conflict, RegexOptions.ExplicitCapture)
            Me.text = Regex.Replace(Me.text, "/Parent\s+(\d+ 0 R)*", "/Parent 2 0 R " & vbCr)
        End Sub
        Friend Overridable Function WriteToStream(ByVal Stream As System.IO.Stream) As Long

            Dim sw As New StreamWriter(Stream, Encoding.ASCII)
            sw.Write(Me.text)
            sw.Flush()
            Return Me.text.Length
        End Function


        Friend Function UnpackStream() As String
            Dim m As Match = Regex.Match(Me.text, "/Length (?'num'\d+)", RegexOptions.ExplicitCapture)
            Dim len As Integer = 0
            If m.Success Then
                len = Integer.Parse(m.Groups("num").Value)
            End If

            Me.PdfFile.memory.Seek(Me.address, SeekOrigin.Begin)
            Dim seek As Long = PdfFile.Seek(Me.PdfFile.memory, "stream" & vbCrLf)
            If seek <> -1 Then
                Me.PdfFile.memory.Seek(seek + 8, SeekOrigin.Begin)

                Dim buffer(len - 1) As Byte
                Me.PdfFile.memory.Read(buffer, 0, len)
                Dim unzipped() As Byte = PdfFile.UnZipStr(buffer)
                Return Encoding.UTF8.GetString(unzipped, 0, unzipped.Length)
            End If
            Return ""
        End Function
    End Class
    Friend Class PdfFileObjectNumberComparer
        Implements IComparer

#Region "IComparer Members"

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim a As PdfFileObject = TryCast(x, PdfFileObject)
            Dim b As PdfFileObject = TryCast(y, PdfFileObject)
            Return a.number.CompareTo(b.number)
        End Function

#End Region
    End Class
    Friend Class PdfFile
        Private Sub OnProgress(ByVal part As Integer, ByVal total As Integer)
        End Sub
        Friend Event ProgressEvent As ProgressDelegate
        Friend trailer As String
        Friend memory As System.IO.Stream
        Friend objects As Hashtable
        Public Sub New(ByVal InputStream As System.IO.Stream)
            Me.memory = InputStream
            Me.ProgressEventEvent = New ProgressDelegate(AddressOf Me.OnProgress)
        End Sub


        Public Sub Load()
            Dim startxref As Long = Me.GetStartxref()
            Me.trailer = Me.ParseTrailer(startxref)
            Dim adds() As Long = Me.GetAddresses(startxref)
            Me.LoadHash(adds)
        End Sub

        Private Sub LoadHash(ByVal addresses() As Long)
            Me.objects = New Hashtable()
            Dim part As Integer = 0
            Dim total As Integer = addresses.Length
            For Each add As Long In addresses
                If add < 0 Then
                    Continue For
                End If
                RaiseEvent ProgressEvent(part, total)
                Me.memory.Seek(add, SeekOrigin.Begin)
                Dim sr As New StreamReader(Me.memory)
                Dim line As String = sr.ReadLine()
                If line.Length < 2 Then
                    line = sr.ReadLine()
                End If
                Dim m As Match = Regex.Match(line, "(?'id'\d+) 0 obj", RegexOptions.ExplicitCapture)
                If m.Success Then
                    Dim num As Integer = Integer.Parse(m.Groups("id").Value)
                    If Not objects.ContainsKey(num) Then
                        objects.Add(num, PdfFileObject.Create(Me, num, add))
                    End If
                End If
                part += 1
            Next add
            For Each add As Long In addresses
                If add >= 0 Then
                    Continue For
                End If
                Dim realadd As Integer = -CInt(add)
                If Not objects.ContainsKey(realadd) Then
                    Continue For
                End If
                Dim obj As PdfFileObject = DirectCast(objects(realadd), PdfFileObject)
                Dim unpackedstream As String = obj.UnpackStream()
                Dim n As Integer = obj.GetNumber("N")
                Dim nums(n - 1) As Integer
                Dim poss(n - 1) As Integer
                Dim lens(n - 1) As Integer
                Dim splitted() As String = unpackedstream.Split(New Char() {" "c}, n * 2 + 1)
                Dim prevpos As Integer = 0
                For i As Integer = 0 To n - 1
                    nums(i) = Integer.Parse(splitted(i * 2))
                    poss(i) = Integer.Parse(splitted(i * 2 + 1))
                    If i > 0 Then
                        lens(i - 1) = poss(i) - poss(i - 1)
                    End If
                    prevpos = poss(i)
                Next i
                lens(n - 1) = splitted(splitted.Length - 1).Length - poss(n - 1)
                For i As Integer = 0 To n - 1
                    Dim objstr As String = splitted(splitted.Length - 1).Substring(poss(i), lens(i))
                    objects.Add(nums(i), PdfFileObject.Create(Me, nums(i), objstr))
                Next i
            Next add
        End Sub

        Friend Function LoadObject(ByVal text As String, ByVal key As String) As PdfFileObject
            Dim pattern As String = "/" & key & " (?'id'\d+)"
            Dim m As Match = Regex.Match(text, pattern, RegexOptions.ExplicitCapture)
            If m.Success Then
                Return Me.LoadObject(Integer.Parse(m.Groups("id").Value))
            End If
            Return Nothing
        End Function
        Friend Function LoadObject(ByVal number As Integer) As PdfFileObject
            Return TryCast(Me.objects(number), PdfFileObject)
        End Function
        Friend ReadOnly Property PageList() As ArrayList
            Get
                Dim root As PdfFileObject = Me.LoadObject(Me.trailer, "Root")
                Dim pages As PdfFileObject = Me.LoadObject(root.text, "Pages")
                Return pages.GetKids()
            End Get
        End Property
        Public ReadOnly Property PageCount() As Integer
            Get
                Return Me.PageList.Count
            End Get
        End Property
        Private Function GetAddresses(ByVal xref As Long) As Long()
            Dim al As New ArrayList()
            Dim tr As String = ParseTrailer(xref)
            If Regex.Match(tr, "/FlateDecode").Success Then
                Dim m As Match = Regex.Match(tr, "/Length (?'num'\d+)", RegexOptions.ExplicitCapture)
                Dim len As Integer = 0
                If m.Success Then
                    len = Integer.Parse(m.Groups("num").Value)
                End If

                Dim widths() As Integer = {1, 1, 1}
                Dim totalwidth As Integer = 1
                m = Regex.Match(tr, "/W\s*\[(?'nums'[\d\s]+?)\]", RegexOptions.ExplicitCapture)
                If m.Success Then
                    Dim split() As String = (m.Groups("nums").Value.Split(" "c))
                    widths = New Integer(split.Length - 1) {}
                    For i As Integer = 0 To split.Length - 1
                        'if (!int.TryParse(split[i], widths[i])) continue;
                        widths(i) = Integer.Parse(split(i))
                        totalwidth += widths(i)
                    Next i
                End If

                Me.memory.Seek(xref, SeekOrigin.Begin)
                Dim seekL As Long = Seek(Me.memory, "stream" & vbLf)

                If seekL <> -1 Then
                    Me.memory.Seek(seekL + 7, SeekOrigin.Begin)

                    Dim buffer(len - 1) As Byte
                    Me.memory.Read(buffer, 0, len)
                    Dim unzipped() As Byte = UnZipStr(buffer)

                    Dim prevrow(totalwidth - 2) As Byte
                    For row As Integer = 1 To unzipped.Length - 1 Step totalwidth
                        Dim currow(totalwidth - 2) As Byte
                        Dim col As Integer = 0
                        Do While col < totalwidth - 1
                            currow(col) = CByte((prevrow(col) + unzipped(row + col)) Mod 256)
                            col += 1
                        Loop
                        If currow(0) > 0 Then
                            Dim addr As Long = 0
                            col = widths(0)
                            Do While col < widths(0) + widths(1)
                                addr *= 256
                                addr += currow(col)
                                col += 1
                            Loop
                            If currow(0) = 1 Then
                                al.Add(addr)
                            ElseIf currow(0) = 2 AndAlso Not al.Contains(-addr) Then
                                al.Add(-addr)
                            End If
                        End If
                        '
                        prevrow = currow
                    Next row
                End If
            Else
                Me.memory.Seek(xref, SeekOrigin.Begin)
                Dim sr As New StreamReader(Me.memory)
                Dim line As String = ""
                Do
                    line = sr.ReadLine()
                    If line Is Nothing OrElse Regex.IsMatch(line, ">>") Then
                        Exit Do
                    End If
                    If Regex.IsMatch(line, "\d{10} 00000 n\s*") Then
                        al.Add(Long.Parse(line.Substring(0, 10)))
                    End If
                Loop
            End If

            Dim mtch As Match = Regex.Match(tr, "/Prev \d+")
            If mtch.Success Then
                al.AddRange(Me.GetAddresses(Long.Parse(mtch.Value.Substring(6))))
            End If
            Return TryCast(al.ToArray(GetType(Long)), Long())
        End Function
        Public Shared Function UnZipStr(ByVal bytes() As Byte) As Byte()
            Using returnStream As New MemoryStream()
                Using source As New MemoryStream(bytes)
                    source.Position = 0
                    Using decompressor = New DeflateStream(source, CompressionMode.Decompress)
                        decompressor.CopyTo(returnStream)
                        Return returnStream.ToArray()
                    End Using
                End Using
            End Using
        End Function

        Public Shared Function Seek(ByVal file As String, ByVal searchString As String) As Long
            'open filestream to perform a seek
            Using fs As System.IO.FileStream = System.IO.File.OpenRead(file)
                Return Seek(fs, searchString)
            End Using
        End Function

        Public Shared Function Seek(ByVal fs As Stream, ByVal searchString As String) As Long
            Dim search() As Char = searchString.ToCharArray()
            Dim result As Long = -1, position As Long = 0, stored As Long = -1, begin As Long = fs.Position
            Dim c As Integer

            c = fs.ReadByte()
            'INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
            'ORIGINAL LINE: while ((c = fs.ReadByte()) != -1)
            Do While c <> -1
                If ChrW(c) = search(position) Then
                    If stored = -1 AndAlso position > 0 AndAlso ChrW(c) = search(0) Then
                        stored = fs.Position
                    End If

                    If position + 1 = search.Length Then
                        result = fs.Position - search.Length
                        fs.Position = result
                        Exit Do
                    End If

                    position += 1
                ElseIf stored > -1 Then
                    fs.Position = stored + 1
                    position = 1
                    stored = -1 'reset stored position!
                Else
                    position = 0
                End If
                c = fs.ReadByte()
            Loop

            If result = -1 Then
                fs.Position = begin
            End If

            Return result
        End Function

        Private Function GetStartxref() As Long
            Dim sr As New StreamReader(Me.memory)
            Me.memory.Seek(Me.memory.Length - 100, SeekOrigin.Begin)
            Dim line As String = ""
            Do While Not line.StartsWith("startxref")
                line = sr.ReadLine()
            Loop
            Dim startxref As Long = Long.Parse(sr.ReadLine())
            If startxref = -1 Then
                Throw New Exception("Cannot find the startxref")
            End If
            Return startxref
        End Function
        Private Function ParseTrailer(ByVal xref As Long) As String
            Me.memory.Seek(xref, SeekOrigin.Begin)
            Dim sr As New StreamReader(Me.memory)
            Dim line As String
            Dim trailer As String = ""
            Dim istrailer As Boolean = False
            line = sr.ReadLine()
            'INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
            'ORIGINAL LINE: while ((line = sr.ReadLine()).IndexOf("startxref") == -1)
            Do While line.IndexOf("startxref") = -1
                line = line.Trim()
                If line.IndexOf("trailer") >= 0 Then
                    trailer = ""
                    istrailer = True
                End If
                If istrailer Then
                    trailer &= line & vbCr
                End If
                line = sr.ReadLine()
            Loop

            If trailer = "" Then
                Me.memory.Seek(xref, SeekOrigin.Begin)
                sr = New StreamReader(Me.memory)
                Dim parentheses As Integer = 0
                Dim temp As String = ""
                Dim started As Boolean = False
                line = sr.ReadLine()
                'INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
                'ORIGINAL LINE: while ((line = sr.ReadLine()) != null)
                Do While line IsNot Nothing
                    Dim pos As Integer = -1
                    Do
                        pos = line.IndexOf("<<")
                        Dim pos2 As Integer = line.IndexOf(">>")

                        If pos = -1 Then
                            pos = pos2
                        ElseIf pos2 <> -1 Then
                            pos = Math.Min(pos, pos2)
                        End If

                        If pos <> -1 Then
                            started = True
                            parentheses += If(line.Chars(pos) = ">"c, 1, -1)
                            temp &= line.Substring(0, pos + 2)
                            If parentheses = 0 Then
                                Exit Do
                            End If
                            line = line.Substring(pos + 2)
                        Else
                            temp &= line & vbCrLf
                        End If
                    Loop While pos <> -1
                    If parentheses = 0 AndAlso temp <> "" AndAlso started Then
                        Exit Do
                    End If
                    line = sr.ReadLine()
                Loop
                If parentheses = 0 Then ' && Regex.Match(temp, "TangibleTempVerbatimOpenTag/RootTangibleTempVerbatimCloseTag").Success)
                    trailer = temp
                End If
            End If
            If trailer = "" Then
                Throw New Exception("Cannot find trailer")
            End If
            Return trailer
        End Function

    End Class
End Namespace

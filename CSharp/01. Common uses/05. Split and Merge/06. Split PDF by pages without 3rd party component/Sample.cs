using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using System.IO.Compression;

namespace Sample
{
	class Sample
	{
		/// <summary>
		/// Split PDF by pages without 3rd party component in C# and .NET
		/// </summary>
		/// <remarks>
		/// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/split-PDF-by-pages-without-3rd-party-component-csharp-dotnet.php
		/// </remarks>
		static void Main(string[] args)
		{
			// C# example to split PDF document by pages without any 3rd party components.

			// Advantages:
			// + Completely C# code, no dependencies, no 3rd party references.

			// Disadvantages:
			// - Uses Regual expressions
			// - May fault with complex PDF files (especially new PDF 1.7, PDF 2.0).

			// If want to use 3rd party component, try our SautinSoft.Pdf:
			// https://sautinsoft.com/products/pdf/help/net/developer-guide/split-pdf-files.php
			// It's also completely FREE for Split and Merge operations.

			string inpFile = Path.GetFullPath(@"..\..\..\005.pdf");
			DirectoryInfo outDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Pages"));
			if (outDir.Exists)
				outDir.Delete(true);
			
			outDir.Create();

			ProjectPart pp = new ProjectPart();
			try
			{
				pp.Load(inpFile);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error! Can't load input file: {inpFile}");
                Console.WriteLine(e.Message);
                return;
			}
			int[] Pages = pp.Pages;
			foreach (int page in Pages)
			{
				string filename = Path.GetFileName(pp.path);
				filename = Regex.Replace(filename, @".pdf", "-" + (page + 1).ToString("00000") + ".pdf", RegexOptions.IgnoreCase);
				FileStream w = File.OpenWrite(Path.Combine(outDir.FullName, filename));
				PdfSplitterMerger psm = new PdfSplitterMerger(w);
				FileStream f = File.OpenRead(pp.path);
				psm.Add(f, new int[] { page });
				psm.Finish();
				f.Close();
				w.Close();
			}
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outDir.FullName) { UseShellExecute = true });
		}
	}
	internal class ProjectPart
	{
		private void OnProgress(int part, int total) { }
		internal event ProgressDelegate ProgressEvent;
		public string path;
		internal string pages;
		public int[] Pages
		{
			get
			{
				ArrayList ps = new ArrayList();
				if (this.pages == null || pages.Length == 0)
				{
					for (int index = 0; index < this.MaxPage; index++)
					{
						ps.Add(index);
					}
				}
				else
				{
					string[] ss = this.pages.Split(new char[] { ',', ' ', ';' });
					foreach (string s in ss)
						if (Regex.IsMatch(s, @"\d+-\d+"))
						{
							int start = int.Parse(s.Split(new char[] { '-' })[0]);
							int end = int.Parse(s.Split(new char[] { '-' })[1]);
							if (start > end)
								return new int[] { 0 };
							while (start <= end)
							{
								ps.Add(start - 1);
								start++;
							}
						}
						else
						{
							ps.Add(int.Parse(s) - 1);
						}
				}
				return ps.ToArray(typeof(int)) as int[];
			}
		}
		public int MaxPage;
		public void Load(string path)
		{
			this.path = path;
			System.IO.Stream s = File.OpenRead(path);
			PdfFile pf = new PdfFile(s);
			pf.ProgressEvent += new ProgressDelegate(pf_ProgressEvent);
			pf.Load();
			this.MaxPage = pf.PageCount;
			s.Close();
		}
		public void LoadFromBytes(byte[] pdf)
		{

			System.IO.Stream s = new MemoryStream(pdf);
			PdfFile pf = new PdfFile(s);
			pf.ProgressEvent += new ProgressDelegate(pf_ProgressEvent);
			pf.Load();
			this.MaxPage = pf.PageCount;
			s.Close();
		}

		public ProjectPart()
		{
			this.ProgressEvent = new ProgressDelegate(this.OnProgress);
		}

		private void pf_ProgressEvent(int part, int total)
		{
			this.ProgressEvent(part, total);
		}
	}
	internal delegate void ProgressDelegate(int part, int total);
	internal class PdfSplitterMerger
	{
		System.IO.Stream target;
		long pos = 15;
		private int number = 3;
		private ArrayList pageNumbers, xrefs;
		private void OnProgress(int part, int total) { }
		public event ProgressDelegate ProgressEvent;
		public PdfSplitterMerger(System.IO.Stream OutputStream)
		{
			this.ProgressEvent = new ProgressDelegate(this.OnProgress);
			this.xrefs = new ArrayList();
			this.pageNumbers = new ArrayList();
			this.target = OutputStream;
			StreamWriter sw = new StreamWriter(this.target);
			string versionstring = "PDF-1.4";
			sw.Write("%" + versionstring + "\r");
			sw.Flush();
			Byte[] buffer = new Byte[7];
			buffer[0] = 0x25;
			buffer[1] = 0xE2;
			buffer[2] = 0xE3;
			buffer[3] = 0xCF;
			buffer[4] = 0xD3;
			buffer[5] = 0x0D;
			buffer[6] = 0x0A;
			this.target.Write(buffer, 0, buffer.Length);
			this.target.Flush();
		}
		public void Add(System.IO.Stream PdfInputStream, int[] PageNumbers)
		{
			PdfFile pf = new PdfFile(PdfInputStream);
			pf.ProgressEvent += new ProgressDelegate(pf_ProgressEvent);
			pf.Load();
			PdfSplitter ps = new PdfSplitter();
			ps.ProgressEvent += new ProgressDelegate(pf_ProgressEvent);
			ps.Load(pf, PageNumbers, this.number);
			this.Add(ps);
		}
		private void Add(PdfSplitter PdfSplitter)
		{
			foreach (int pageNumber in PdfSplitter.pageNumbers)
			{
				this.pageNumbers.Add(PdfSplitter.transHash[pageNumber]);
			}
			ArrayList sortedObjects = new ArrayList();
			foreach (PdfFileObject pfo in PdfSplitter.sObjects.Values)
				sortedObjects.Add(pfo);
			sortedObjects.Sort(new PdfFileObjectNumberComparer());

			foreach (PdfFileObject pfo in sortedObjects)
			{
				this.xrefs.Add(pos);
				this.pos += pfo.WriteToStream(this.target);
				this.number++;
			}
		}
		public void Finish()
		{
			StreamWriter sw = new StreamWriter(this.target);

			string root = "";
			root = "1 0 obj\r";
			root += "<< \r/Type /Catalog \r";
			root += "/Pages 2 0 R \r";
			root += ">> \r";
			root += "endobj\r";

			xrefs.Insert(0, pos);
			pos += root.Length;
			sw.Write(root);

			string pages = "";
			pages += "2 0 obj \r";
			pages += "<< \r";
			pages += "/Type /Pages \r";
			pages += "/Count " + pageNumbers.Count + " \r";
			pages += "/Kids [ ";
			foreach (int pageIndex in pageNumbers)
			{
				pages += pageIndex + " 0 R ";
			}
			pages += "] \r";
			pages += ">> \r";
			pages += "endobj\r";

			xrefs.Insert(1, pos);
			pos += pages.Length;
			sw.Write(pages);


			sw.Write("xref\r");
			sw.Write("0 " + (this.number) + " \r");
			sw.Write("0000000000 65535 f \r");

			foreach (long xref in this.xrefs)
				sw.Write((xref + 1).ToString("0000000000") + " 00000 n \r");
			sw.Write("trailer\r");
			sw.Write("<<\r");
			sw.Write("/Size " + (this.number) + "\r");
			sw.Write("/Root 1 0 R \r");
			sw.Write(">>\r");
			sw.Write("startxref\r");
			sw.Write((pos + 1) + "\r");
			sw.Write("%%EOF\r");
			sw.Flush();
			sw.Close();
		}

		private void pf_ProgressEvent(int part, int total)
		{
			this.ProgressEvent(part, total);
		}
	}
	internal class Project
	{
		public string Name;
		public ArrayList Parts;
		public int TotalPages
		{
			get
			{
				int tot = 0;
				foreach (ProjectPart pp in this.Parts)
				{
					if (pp.Pages == null)
					{
						tot += pp.MaxPage;
					}
					else
					{
						tot += pp.Pages.Length;
					}
				}
				return tot;
			}
		}
		public Project(string name)
		{
			this.Parts = new ArrayList();
			this.Name = name;
		}
	}
	internal class PdfSplitter
	{
		internal Hashtable sObjects;
		internal ArrayList pageNumbers;
		internal Hashtable transHash;
		internal PdfFile PdfFile;
		private void OnProgress(int part, int total) { }
		public event ProgressDelegate ProgressEvent;
		internal PdfSplitter()
		{
			this.ProgressEvent = new ProgressDelegate(this.OnProgress);
		}
		internal void Load(PdfFile PdfFile, int[] PageNumbers, int startNumber)
		{
			this.PdfFile = PdfFile;
			this.pageNumbers = new ArrayList();
			this.sObjects = new Hashtable();
			int part = 0;
			int total = PageNumbers.Length;
			foreach (int PageNumber in PageNumbers)
			{
				this.ProgressEvent(part, total);
				PdfFileObject page = PdfFile.PageList[PageNumber] as PdfFileObject;
				page.PopulateRelatedObjects(PdfFile, this.sObjects);
				this.pageNumbers.Add(page.number);
				part++;
			}
			this.transHash = this.CalcTransHash(startNumber);
			foreach (PdfFileObject pfo in this.sObjects.Values)
			{
				pfo.Transform(transHash);
			}
		}
		private Hashtable CalcTransHash(int startNumber)
		{
			Hashtable ht = new Hashtable();
			ArrayList al = new ArrayList();
			foreach (PdfFileObject pfo in this.sObjects.Values)
			{
				al.Add(pfo);
			}
			al.Sort(new PdfFileObjectNumberComparer());
			int number = startNumber;
			foreach (PdfFileObject pfo in al)
			{
				ht.Add(pfo.number, number);
				number++;
			}
			return ht;
		}

	}
	enum PdfObjectType
	{
		UnKnown, Stream, Page, Other
	}
	internal class PdfFileStreamObject : PdfFileObject
	{
		private byte[] streamBuffer;
		private int streamStartOffset, streamLength;
		internal PdfFileStreamObject(PdfFileObject obj)
		{
			this.address = obj.address;
			this.length = obj.length;
			this.text = obj.text;
			this.number = obj.number;
			this.PdfFile = obj.PdfFile;
			this.LoadStreamBuffer();
		}

		private void LoadStreamBuffer()
		{
			Match m1 = Regex.Match(this.text, @"stream\s*");
			this.streamStartOffset = m1.Index + m1.Value.Length;
			this.streamLength = this.length - this.streamStartOffset;
			this.streamBuffer = new byte[this.streamLength];
			this.PdfFile.memory.Seek(this.address + this.streamStartOffset, SeekOrigin.Begin);
			this.PdfFile.memory.Read(this.streamBuffer, 0, this.streamLength);

			this.PdfFile.memory.Seek(this.address, SeekOrigin.Begin);
			StreamReader sr = new StreamReader(this.PdfFile.memory);
			char[] startChars = new char[this.streamStartOffset];
			sr.ReadBlock(startChars, 0, this.streamStartOffset);
			StringBuilder sb = new StringBuilder();
			sb.Append(startChars);
			this.text = sb.ToString();
		}
		internal override void Transform(System.Collections.Hashtable TransformationHash)
		{
			base.Transform(TransformationHash);
		}
		internal override long WriteToStream(System.IO.Stream Stream)
		{
			StreamWriter sw = new StreamWriter(Stream);
			sw.Write(this.text);
			sw.Flush();
			new MemoryStream(this.streamBuffer).WriteTo(Stream);
			sw.Flush();
			return this.streamLength + this.text.Length;
		}
	}

	internal class PdfFileObject
	{
		internal long address;
		internal int number, length;
		internal string text;
		internal PdfFile PdfFile;
		MatchEvaluator filterEval;
		internal static PdfFileObject Create(PdfFile PdfFile, int number, long address)
		{
			PdfFileObject pfo = new PdfFileObject();
			pfo.PdfFile = PdfFile;
			pfo.number = number;
			pfo.address = address;
			pfo.GetLenght(PdfFile);
			pfo.LoadText();
			if (pfo.Type == PdfObjectType.Stream)
			{
				pfo = new PdfFileStreamObject(pfo);
			}
			pfo.filterEval = new MatchEvaluator(pfo.FilterEval);
			return pfo;
		}

		internal static PdfFileObject Create(PdfFile PdfFile, int number, string initial)
		{
			PdfFileObject pfo = new PdfFileObject();
			pfo.PdfFile = PdfFile;
			pfo.number = number;
			pfo.address = -1;
			pfo.length = -1;
			pfo.text = pfo.number + " 0 obj\n" + initial + "\nendobj\n";
			if (pfo.Type == PdfObjectType.Stream)
			{
				pfo = new PdfFileStreamObject(pfo);
			}
			pfo.filterEval = new MatchEvaluator(pfo.FilterEval);
			return pfo;
		}


		private void LoadText()
		{
			this.PdfFile.memory.Seek(this.address, SeekOrigin.Begin);
			StringBuilder sb = new StringBuilder();
			for (int index = 0; index < this.length; index++)
			{
				sb.Append((char)this.PdfFile.memory.ReadByte());
			}
			this.text = sb.ToString();
		}
		private void GetLenght(PdfFile PdfFile)
		{
			System.IO.Stream stream = PdfFile.memory;
			stream.Seek(this.address, SeekOrigin.Begin);
			Match m = Regex.Match("", @"endobj\s*");
			int b = 0;
			this.length = 0;
			string word = "";
			while (b != -1)
			{
				b = stream.ReadByte();
				this.length++;
				if (b > 97 && b < 112)
				{
					char c = (char)b;
					word += c;
					if (word == "endobj")
						b = -1;
				}
				else
				{
					word = "";
				}
			}
			char c2 = (char)stream.ReadByte();
			while (Regex.IsMatch(c2.ToString(), @"\s"))
			{
				this.length++;
				c2 = (char)stream.ReadByte();
			}
		}
		protected PdfObjectType type;
		internal PdfObjectType Type
		{
			get
			{
				if (this.type == PdfObjectType.UnKnown)
				{
					if (Regex.IsMatch(this.text, @"/Page") & !Regex.IsMatch(this.text, @"/Pages"))
					{
						this.type = PdfObjectType.Page;
						return this.type;
					}
					if (Regex.IsMatch(this.text, @"stream"))
					{
						this.type = PdfObjectType.Stream;
						return this.type;
					}
					this.type = PdfObjectType.Other;
				}
				return this.type;
			}
		}

		internal int GetNumber(string Name)
		{
			Match m = Regex.Match(this.text, @"/" + Name + @" (?'num'\d+)", RegexOptions.ExplicitCapture);
			int len = 0;
			if (m.Success)
			{
				len = int.Parse(m.Groups["num"].Value);
			}
			return len;
		}

		internal int[] GetArrayNumbers(string arrayName)
		{
			ArrayList ids = new ArrayList();
			string pattern = @"/" + arrayName + @"\s*\[(\s*(?'id'\d+) 0 R\s*)*";
			Match m = Regex.Match(this.text, pattern, RegexOptions.ExplicitCapture);
			foreach (Capture cap in m.Groups["id"].Captures)
			{
				ids.Add(int.Parse(cap.Value));
			}
			return ids.ToArray(typeof(int)) as int[];
		}
		internal ArrayList GetKids()
		{
			ArrayList kids = new ArrayList();
			foreach (int id in this.GetArrayNumbers("Kids"))
			{
				PdfFileObject pfo = PdfFile.LoadObject(id);
				if (pfo.Type == PdfObjectType.Page)
				{
					kids.Add(pfo);
				}
				else
				{
					kids.AddRange(pfo.GetKids());
				}
			}
			return kids;
		}

		internal void PopulateRelatedObjects(PdfFile PdfFile, Hashtable container)
		{
			if (!container.ContainsKey(this.number))
			{
				container.Add(this.number, this);
				Match m = Regex.Match(this.text, @"(?'parent'(/Parent)*)\s*(?'id'\d+) 0 R[^G]", RegexOptions.ExplicitCapture);
				while (m.Success)
				{
					int num = int.Parse(m.Groups["id"].Value);
					bool notparent = m.Groups["parent"].Length == 0;
					if (notparent & !container.Contains(num))
					{
						PdfFileObject pfo = PdfFile.LoadObject(num);
						if (pfo != null & !container.Contains(pfo.number))
						{
							pfo.PopulateRelatedObjects(PdfFile, container);
						}
					}
					m = m.NextMatch();
				}
			}
		}

		private Hashtable TransformationHash;
		private string FilterEval(Match m)
		{
			int id = int.Parse(m.Groups["id"].Value);
			string end = m.Groups["end"].Value;
			if (this.TransformationHash.ContainsKey(id))
			{
				string rest = m.Groups["rest"].Value;
				return (int)TransformationHash[id] + rest + end;
			}
			return end;
		}
		internal PdfFileObject Parent
		{
			get
			{
				return this.PdfFile.LoadObject(this.text, "Parent");
			}
		}
		internal string MediaBoxText
		{
			get
			{
				string pattern = @"/MediaBox\s*\[\s*(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s*]";
				string mediabox = Regex.Match(this.text, pattern).Value;

				//link
				if (mediabox == "")
				{
					pattern = @"/MediaBox\s*\d+";
					mediabox = Regex.Match(this.text, pattern).Value;
					if (mediabox != "")
					{
						mediabox = mediabox.Remove(0, "/MediaBox".Length);
						mediabox = mediabox.Trim();
						int obj = Convert.ToInt32(mediabox, 10);

						if (obj > 0 && obj < this.PdfFile.objects.Count)
						{
							pattern = @"\[\s*(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s*]";
							string text = ((PdfFileObject)(this.PdfFile.objects[obj])).text;
							mediabox = Regex.Match(text, pattern).Value;
							mediabox = "/MediaBox " + mediabox;
						}
					}

				}
				return mediabox;
			}
		}
		internal virtual void Transform(Hashtable TransformationHash)
		{
			if (this.Type == PdfObjectType.Page && this.MediaBoxText == "")
			{
				PdfFileObject parent = this.Parent;
				while (parent != null)
				{
					string mb = parent.MediaBoxText;
					if (mb == "")
					{
						parent = parent.Parent;
					}
					else
					{
						this.text = Regex.Replace(this.text, @"/Type\s*/Page", "/Type /Page\r" + mb);
						parent = null;
					}
				}
			}
			this.TransformationHash = TransformationHash;
			this.text = Regex.Replace(this.text
			, @"(?'id'\d+)(?'rest' 0 (obj|R))(?'end'[^G])", this.filterEval, RegexOptions.ExplicitCapture);
			this.text = Regex.Replace(this.text, @"/Parent\s+(\d+ 0 R)*", "/Parent 2 0 R \r");
		}
		internal virtual long WriteToStream(System.IO.Stream Stream)
		{

			StreamWriter sw = new StreamWriter(Stream, Encoding.ASCII);
			sw.Write(this.text);
			sw.Flush();
			return this.text.Length;
		}


		internal string UnpackStream()
		{
			Match m = Regex.Match(this.text, @"/Length (?'num'\d+)", RegexOptions.ExplicitCapture);
			int len = 0;
			if (m.Success)
			{
				len = int.Parse(m.Groups["num"].Value);
			}

			this.PdfFile.memory.Seek(this.address, SeekOrigin.Begin);
			long seek = PdfFile.Seek(this.PdfFile.memory, "stream\r\n");
			if (seek != -1)
			{
				this.PdfFile.memory.Seek(seek + 8, SeekOrigin.Begin);

				byte[] buffer = new byte[len];
				this.PdfFile.memory.Read(buffer, 0, len);
				byte[] unzipped = PdfFile.UnZipStr(buffer);
				return Encoding.UTF8.GetString(unzipped, 0, unzipped.Length);
			}
			return "";
		}
	}
	internal class PdfFileObjectNumberComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			PdfFileObject a = x as PdfFileObject;
			PdfFileObject b = y as PdfFileObject;
			return a.number.CompareTo(b.number);
		}

		#endregion
	}
	internal class PdfFile
	{
		private void OnProgress(int part, int total) { }
		internal event ProgressDelegate ProgressEvent;
		internal string trailer;
		internal System.IO.Stream memory;
		internal Hashtable objects;
		public PdfFile(System.IO.Stream InputStream)
		{
			this.memory = InputStream;
			this.ProgressEvent = new ProgressDelegate(this.OnProgress);
		}


		public void Load()
		{
			long startxref = this.GetStartxref();
			this.trailer = this.ParseTrailer(startxref);
			long[] adds = this.GetAddresses(startxref);
			this.LoadHash(adds);
		}

		private void LoadHash(long[] addresses)
		{
			this.objects = new Hashtable();
			int part = 0;
			int total = addresses.Length;
			foreach (long add in addresses)
			{
				if (add < 0) continue;
				this.ProgressEvent(part, total);
				this.memory.Seek(add, SeekOrigin.Begin);
				StreamReader sr = new StreamReader(this.memory);
				string line = sr.ReadLine();
				if (line.Length < 2)
					line = sr.ReadLine();
				Match m = Regex.Match(line, @"(?'id'\d+) 0 obj", RegexOptions.ExplicitCapture);
				if (m.Success)
				{
					int num = int.Parse(m.Groups["id"].Value);
					if (!objects.ContainsKey(num))
					{
						objects.Add(num, PdfFileObject.Create(this, num, add));
					}
				}
				part++;
			}
			foreach (long add in addresses)
			{
				if (add >= 0) continue;
				int realadd = -(int)add;
				if (!objects.ContainsKey(realadd)) continue;
				PdfFileObject obj = (PdfFileObject)objects[realadd];
				string unpackedstream = obj.UnpackStream();
				int n = obj.GetNumber("N");
				int[] nums = new int[n];
				int[] poss = new int[n];
				int[] lens = new int[n];
				string[] splitted = unpackedstream.Split(new char[] { ' ' }, n * 2 + 1);
				int prevpos = 0;
				for (int i = 0; i < n; i++)
				{
					nums[i] = int.Parse(splitted[i * 2]);
					poss[i] = int.Parse(splitted[i * 2 + 1]);
					if (i > 0) lens[i - 1] = poss[i] - poss[i - 1];
					prevpos = poss[i];
				}
				lens[n - 1] = splitted[splitted.Length - 1].Length - poss[n - 1];
				for (int i = 0; i < n; i++)
				{
					string objstr = splitted[splitted.Length - 1].Substring(poss[i], lens[i]);
					objects.Add(nums[i], PdfFileObject.Create(this, nums[i], objstr));
				}
			}
		}

		internal PdfFileObject LoadObject(string text, string key)
		{
			string pattern = @"/" + key + @" (?'id'\d+)";
			Match m = Regex.Match(text, pattern, RegexOptions.ExplicitCapture);
			if (m.Success)
			{
				return this.LoadObject(int.Parse(m.Groups["id"].Value));
			}
			return null;
		}
		internal PdfFileObject LoadObject(int number)
		{
			return this.objects[number] as PdfFileObject;
		}
		internal ArrayList PageList
		{
			get
			{
				PdfFileObject root = this.LoadObject(this.trailer, "Root");
				PdfFileObject pages = this.LoadObject(root.text, "Pages");
				return pages.GetKids();
			}
		}
		public int PageCount
		{
			get
			{
				return this.PageList.Count;
			}
		}
		private long[] GetAddresses(long xref)
		{
			ArrayList al = new ArrayList();
			string tr = ParseTrailer(xref);
			if (Regex.Match(tr, @"/FlateDecode").Success)
			{
				Match m = Regex.Match(tr, @"/Length (?'num'\d+)", RegexOptions.ExplicitCapture);
				int len = 0;
				if (m.Success)
				{
					len = int.Parse(m.Groups["num"].Value);
				}

				int[] widths = new int[] { 1, 1, 1 };
				int totalwidth = 1;
				m = Regex.Match(tr, @"/W\s*\[(?'nums'[\d\s]+?)\]", RegexOptions.ExplicitCapture);
				if (m.Success)
				{
					string[] split = (m.Groups["nums"].Value.Split(' '));
					widths = new int[split.Length];
					for (int i = 0; i < split.Length; i++)
					{
						//if (!int.TryParse(split[i], widths[i])) continue;
						widths[i] = int.Parse(split[i]);
						totalwidth += widths[i];
					}
				}

				this.memory.Seek(xref, SeekOrigin.Begin);
				long seek = Seek(this.memory, "stream\n");

				if (seek != -1)
				{
					this.memory.Seek(seek + 7, SeekOrigin.Begin);

					byte[] buffer = new byte[len];
					this.memory.Read(buffer, 0, len);
					byte[] unzipped = UnZipStr(buffer);

					byte[] prevrow = new byte[totalwidth - 1];
					for (int row = 1; row < unzipped.Length; row += totalwidth)
					{
						byte[] currow = new byte[totalwidth - 1];
						for (int col = 0; col < totalwidth - 1; col++)
							currow[col] = (byte)((prevrow[col] + unzipped[row + col]) % 256);
						if (currow[0] > 0)
						{
							long addr = 0;
							for (int col = widths[0]; col < widths[0] + widths[1]; col++)
							{ addr *= 256; addr += currow[col]; }
							if (currow[0] == 1)
								al.Add(addr);
							else if (currow[0] == 2 && !al.Contains(-addr))
								al.Add(-addr);
						}
						//
						prevrow = currow;
					}
				}
			}
			else
			{
				this.memory.Seek(xref, SeekOrigin.Begin);
				StreamReader sr = new StreamReader(this.memory);
				string line = "";
				while (true)
				{
					line = sr.ReadLine();
					if (line == null || Regex.IsMatch(line, ">>"))
						break;
					if (Regex.IsMatch(line, @"\d{10} 00000 n\s*"))
					{
						al.Add(long.Parse(line.Substring(0, 10)));
					}
				}
			}

			Match mtch = Regex.Match(tr, @"/Prev \d+");
			if (mtch.Success)
				al.AddRange(this.GetAddresses(long.Parse(mtch.Value.Substring(6))));
			return al.ToArray(typeof(long)) as long[];
		}
		public static byte[] UnZipStr(byte[] bytes)
		{
			using (MemoryStream returnStream = new MemoryStream())
			{
				using (MemoryStream source = new MemoryStream(bytes))
				{
					source.Position = 0;
					using var decompressor = new DeflateStream(source, CompressionMode.Decompress);
					decompressor.CopyTo(returnStream);
					return returnStream.ToArray();
				}
			}
		}

		public static long Seek(string file, string searchString)
		{
			//open filestream to perform a seek
			using (System.IO.FileStream fs =
						System.IO.File.OpenRead(file))
			{
				return Seek(fs, searchString);
			}
		}

		public static long Seek(Stream fs,
								string searchString)
		{
			char[] search = searchString.ToCharArray();
			long result = -1, position = 0, stored = -1,
			begin = fs.Position;
			int c;

			while ((c = fs.ReadByte()) != -1)
			{
				if ((char)c == search[position])
				{
					if (stored == -1 && position > 0
						&& (char)c == search[0])
					{
						stored = fs.Position;
					}

					if (position + 1 == search.Length)
					{
						result = fs.Position - search.Length;
						fs.Position = result;
						break;
					}

					position++;
				}
				else if (stored > -1)
				{
					fs.Position = stored + 1;
					position = 1;
					stored = -1; //reset stored position!
				}
				else
				{
					position = 0;
				}
			}

			if (result == -1)
			{
				fs.Position = begin;
			}

			return result;
		}

		private long GetStartxref()
		{
			StreamReader sr = new StreamReader(this.memory);
			this.memory.Seek(this.memory.Length - 100, SeekOrigin.Begin);
			string line = "";
			while (!line.StartsWith("startxref"))
			{
				line = sr.ReadLine();
			}
			long startxref = long.Parse(sr.ReadLine());
			if (startxref == -1)
				throw new Exception("Cannot find the startxref");
			return startxref;
		}
		private string ParseTrailer(long xref)
		{
			this.memory.Seek(xref, SeekOrigin.Begin);
			StreamReader sr = new StreamReader(this.memory);
			string line;
			string trailer = "";
			bool istrailer = false;
			while ((line = sr.ReadLine()).IndexOf("startxref") == -1)
			{
				line = line.Trim();
				if (line.IndexOf("trailer") >= 0)
				{
					trailer = "";
					istrailer = true;
				}
				if (istrailer)
				{
					trailer += line + "\r";
				}
			}

			if (trailer == "")
			{
				this.memory.Seek(xref, SeekOrigin.Begin);
				sr = new StreamReader(this.memory);
				int parentheses = 0;
				string temp = "";
				bool started = false;
				while ((line = sr.ReadLine()) != null)
				{
					int pos = -1;
					do
					{
						pos = line.IndexOf("<<");
						int pos2 = line.IndexOf(">>");

						if (pos == -1) pos = pos2;
						else if (pos2 != -1) pos = Math.Min(pos, pos2);

						if (pos != -1)
						{
							started = true;
							parentheses += (line[pos] == '>') ? 1 : -1;
							temp += line.Substring(0, pos + 2);
							if (parentheses == 0)
								break;
							line = line.Substring(pos + 2);
						}
						else
							temp += line + "\r\n";
					} while (pos != -1);
					if (parentheses == 0 && temp != "" && started) break;
				}
				if (parentheses == 0)// && Regex.Match(temp, @"/Root").Success)
					trailer = temp;
			}
			if (trailer == "")
				throw new Exception("Cannot find trailer");
			return trailer;
		}

	}
}

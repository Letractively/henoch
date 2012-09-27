using System;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
namespace Telerik.QuickStart
{
	public enum ProgrammingLanguage
	{
		DEFAULT = 0,
		CS = 1,
		VB = 2,
		JS = 4,
		DELPHI = 8,
		CPP = 16
	};

	/// <summary>
	/// A helper class for the Code Viewer.
	/// </summary>
	public class CodeViewerHelper
	{
		#region templates & constants
		private static String[] VbKeywords = new String[] {
											   "Partial", "Handles","Private", "Protected", "Public", "End Namespace", "Namespace",
											   "End Class", "Exit", "Class", "Goto", "Try", "Catch", "End Try",
											   "For", "End If", "If", "Else", "ElseIf", "Next", "While", "And",
											   "Do", "Loop", "Dim", "As", "End Select", "Select", "Case", "Or",
											   "Imports", "Then", "Integer", "Long", "String", "Overloads", "True",
											   "Overrides", "End Property", "End Sub", "End Function", "Sub", "Me",
											   "Function", "End Get", "End Set", "Get", "Friend", "Inherits",
											   "Implements","Return", "Not", "New", "Shared", "Nothing", "Finally",
											   "False", "Me", "My", "MyBase" };
		private static String[] CsKeywords = new String[] {
											   "partial", "private", "protected", "public", "namespace", "class", "break",
											   "for", "if", "else", "while", "switch", "case", "using",
											   "return", "null", "void", "int", "bool", "string", "float",
											   "this", "new", "true", "false", "const", "static", "base",
											   "foreach", "in", "try", "catch", "finally", "get", "set", "char", "default"};
		private static String[] JsKeywords = new String[] {
											   "private", "protected", "public", "namespace", "class", "var",
											   "for", "if", "else", "while", "switch", "case", "using", "get",
											   "return", "null", "void", "int", "string", "float", "this", "set",
											   "new", "true", "false", "const", "static", "package", "function",
											   "internal", "extends", "super", "import", "default", "break", "try",
											   "catch", "finally" };

		private const string VbLangRegexFirstReplace = "(?i)\\b{0}\\b(?<!'.*)";
		private const string VbLangRegexSecondReplace = "(?<comment>'(?![^']*&quot;).*$)";
		private const string CLangRegexFirstReplace = "\\b{0}\\b(?<!//.*)";
		private const string CLangRegexSecondReplace = "(?<comment>//.*$)";
		private const string preFontTemplate = "font:{0} {1}";
		private const string colorMarkerTemplate = "<span style=\"color:{0};\">{1}</span>";
		private const string colorMarkerBTemplate = "<span style=\"background-color:{0};\">{1}</span>";
		private const string contentTemplateBegin = "<span style=\"color:{0};\">";
		private const string contentTemplateEnd = "</span>";
		private const string scriptTemplate = "<script type='text/javascript'>/*<![CDATA[*/{0}/*]]>*/</script>";
		private const string imageTemplate = "<p><img src=\"{0}\" style=\"border:none;\" alt=\"\"/><br/>{0}<br/></p>";
		private const string KeywordColor = "blue";
		private const string CommentColor = "green";
		private const string TagColor = "maroon";
		private const string AspDirectiveColor = "yellow";
		#endregion

		private static string processedFontTemplate = String.Empty;

		private static string PageVirtualPath = String.Empty;
		private static string PagePhysicalPath = String.Empty;
		private static string TabString = "&nbsp;";

		public static Unit FontSize = Unit.Point(8);
		public static String FontName = "Tahoma";
		public static int TabSpaces = 4;

		public static bool ReplaceTabs = true;

		public CodeViewerHelper()
		{
		}


		public static string RenderFile(string FileName)
		{
			if (Regex.IsMatch(Path.GetExtension(FileName), "htm", RegexOptions.IgnoreCase))
			{
				return RenderHtmlFile(FileName);
			}

			try
			{
				TabString = string.Empty;
				for (int i = 0; i < TabSpaces; i++)
				{
					TabString += "&nbsp;";
				}
				if (FileName != null && FileName != String.Empty)
				{
					StringBuilder StrBuilder = new StringBuilder();
					HtmlTextWriter hsr = GetContentWriter(StrBuilder);
					StreamReader sr = GetStreamReader(FileName);
					RenderContent(sr, hsr, FileName);
					sr.Close();
					return StrBuilder.ToString();
				}
			}
			catch (Exception)
			{
				return "Error displaying file: " + FileName;
			}
			return String.Empty;
		}

		public static string RenderHtmlFile(string FileName)
		{
			try
			{
				TabString = string.Empty;
				for (int i = 0; i < TabSpaces; i++)
				{
					TabString += "&nbsp;";
				}
				if (FileName != null && FileName != String.Empty)
				{
					StreamReader sr = Telerik.QuickStart.CodeViewerHelper.GetStreamReader(FileName);
					string Result = sr.ReadToEnd();
					sr.Close();
					return Result;
				}
			}
			catch (Exception)
			{
				return "Cannot Render Html file!";
			}
			return String.Empty;
		}

		internal static HtmlTextWriter GetContentWriter(StringBuilder contentBuilder)
		{
			StringWriter strWriter = new StringWriter(contentBuilder);
			return new HtmlTextWriter(strWriter, "    ");
		}

		internal static StreamReader GetStreamReader(string fileName)
		{
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			return new StreamReader(fs);
		}

		internal static void WriteCodeStart(HtmlTextWriter writer, string attributeContent)
		{
			writer.WriteBeginTag("code");
			writer.Write(HtmlTextWriter.TagRightChar);
		}

		internal static void WriteCodeEnd(HtmlTextWriter writer)
		{
			writer.WriteEndTag("code");
		}

		internal static void MergeArrays(string[] inputArray, System.Collections.ArrayList masterList)
		{
			if (inputArray != null && inputArray.Length > 0)
			{
				for (int i = 0; i < inputArray.Length; i++)
				{
					masterList.Add(inputArray[i]);
				}
			}
		}

		private static string RelativePath(string absolutePath)
		{
			string escapedDir = Regex.Escape(System.Web.HttpContext.Current.Server.MapPath(PageVirtualPath));
			return Regex.Replace(absolutePath, "(" + escapedDir + @")\\?", "", RegexOptions.IgnoreCase);
		}

		private static void RenderContent(StreamReader sr, HtmlTextWriter textBuffer, string fileName)
		{
			String sourceLine = String.Empty;

			textBuffer.Write("<div>");
			if (FontSize.Value > 12)
			{
				textBuffer.Write("<strong>");
			}

			ProgrammingLanguage DefaultLanguage = ProgrammingLanguage.DEFAULT;
			bool IsCodeBehindFile = false;
			if ((fileName.ToLower()).EndsWith(".cs"))
			{
				IsCodeBehindFile = true;
				DefaultLanguage = ProgrammingLanguage.CS;
			}
			else if ((fileName.ToLower()).EndsWith(".js"))
			{
				IsCodeBehindFile = true;
				DefaultLanguage = ProgrammingLanguage.JS;
			}
			else if ((fileName.ToLower()).EndsWith(".vb"))
			{
				IsCodeBehindFile = true;
				DefaultLanguage = ProgrammingLanguage.VB;
			}

			processedFontTemplate = String.Format(preFontTemplate, FontSize.ToString(), FontName);
			WriteCodeStart(textBuffer, processedFontTemplate);

			if (IsCodeBehindFile)
			{
				while ((sourceLine = sr.ReadLine()) != null)
				{
					textBuffer.Write(FixCodeLine(sourceLine, DefaultLanguage));
					textBuffer.Write("<br/>");
				}
			}
			else
			{
				String lang = "VB";
				bool isInScriptBlock = false;
				bool isInMultiLine = false;

				while ((sourceLine = sr.ReadLine()) != null)
				{
					// First we want to grab the global language
					// for this page by a Page directive.  Or
					// possibly from a script block.
					lang = GetLangFromLine(sourceLine, lang);
					if (IsScriptBlockTagStart(sourceLine))
					{
						textBuffer.Write(FixAspxLine(sourceLine));
						isInScriptBlock = true;
					}
					else if (IsScriptBlockTagEnd(sourceLine))
					{
						textBuffer.Write(FixAspxLine(sourceLine));
						isInScriptBlock = false;
					}
					else if (IsMultiLineTagStart(sourceLine) && !isInMultiLine)
					{
						isInMultiLine = true;
						textBuffer.Write(String.Format(contentTemplateBegin, KeywordColor) + "<b>");
						textBuffer.Write(HttpUtility.HtmlEncode(sourceLine));
					}
					else if (IsMultiLineTagEnd(sourceLine) && isInMultiLine)
					{
						isInMultiLine = false;
						textBuffer.Write(HttpUtility.HtmlEncode(sourceLine));
						textBuffer.Write("</b>" + contentTemplateEnd);
					}
					else if (isInMultiLine)
					{
						textBuffer.Write(HttpUtility.HtmlEncode(sourceLine));
					}
					else
					{
						if (isInScriptBlock == true)
						{
							ProgrammingLanguage DefaultInlineLanguage = ProgrammingLanguage.DEFAULT;
							if (lang.ToLower() == "jscript" || lang.ToLower() == "javascript")
							{
								DefaultInlineLanguage = ProgrammingLanguage.JS;
							}
							else if (lang.ToLower() == "c#")
							{
								DefaultInlineLanguage = ProgrammingLanguage.CS;
							}
							else if (lang.ToLower() == "vb")
							{
								DefaultInlineLanguage = ProgrammingLanguage.VB;
							}
							textBuffer.Write(FixCodeLine(sourceLine, DefaultInlineLanguage));
						}
						else
						{
							textBuffer.Write(FixAspxLine(sourceLine));
						}
					}
					textBuffer.Write("<br/>");
				}
			}
			WriteCodeEnd(textBuffer);

			if (FontSize.Value > 12)
			{
				textBuffer.Write("</strong>");
			}
			textBuffer.Write("</div>");
		}

		protected static string RenderImages(System.Collections.ArrayList exampleImages)
		{
			try
			{
				StringBuilder contents = new StringBuilder();
				for (int i = 0; i < exampleImages.Count; i++)
				{
					string imageFile = (string)exampleImages[i];
					string relativePath = RelativePath(imageFile);
					string relativeUrl = relativePath.Replace(@"\", "/");
					contents.Append(String.Format(imageTemplate, PageVirtualPath + "/" + relativeUrl));
				}

				return contents.ToString();
			}
			catch (Exception)
			{
				return "Error while rendering image tab!";
			}
		}

		private static string FixCodeLine(string sourceLine, ProgrammingLanguage languageType)
		{
			if (sourceLine == null)
			{
				return null;
			}

			sourceLine = HttpUtility.HtmlEncode(sourceLine);
			sourceLine = FixTabs(sourceLine);

			string[] LanguageReservedWords = null;
			string LangFirstReplace = String.Empty;
			string LangSecondReplace = String.Empty;
			switch (languageType)
			{
				case ProgrammingLanguage.VB://VB
					{
						LanguageReservedWords = VbKeywords;
						LangFirstReplace = VbLangRegexFirstReplace;
						LangSecondReplace = VbLangRegexSecondReplace;
						break;
					}
				case ProgrammingLanguage.CS://CS
					{
						LanguageReservedWords = CsKeywords;
						LangFirstReplace = CLangRegexFirstReplace;
						LangSecondReplace = CLangRegexSecondReplace;
						break;
					}
				case ProgrammingLanguage.JS://JS
					{
						LanguageReservedWords = JsKeywords;
						LangFirstReplace = CLangRegexFirstReplace;
						LangSecondReplace = CLangRegexSecondReplace;
						break;
					}
			}
			String CombinedKeywords = "(?<keyword>" + String.Join("|", LanguageReservedWords) + ")";

			sourceLine = Regex.Replace(sourceLine, String.Format(LangFirstReplace, CombinedKeywords), String.Format(colorMarkerTemplate, KeywordColor, "${keyword}"));
			sourceLine = Regex.Replace(sourceLine, LangSecondReplace, String.Format(colorMarkerTemplate, CommentColor, "${comment}"));
			return sourceLine;
		}

		private static string FixAspxLine(string sourceLine)
		{
			string searchExpr;      // search string
			string replaceExpr;     // replace string

			if ((sourceLine == null) || (sourceLine.Length == 0))
				return sourceLine;


			sourceLine = HttpUtility.HtmlEncode(sourceLine);
			sourceLine = FixTabs(sourceLine);


			// Single line comment or #include references. (Also <%-- comments)
			searchExpr = "(?i)(?<a>(^.*))(?<b>(&lt;[!%]--))(?<c>(.*))(?<d>(--%?&gt;))(?<e>(.*))";
			replaceExpr = "${a}" + String.Format(colorMarkerTemplate, CommentColor, "${b}${c}${d}") + "${e}";
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				return Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

            // Colorize <%@ <type>
			searchExpr = "(?i)" + "(?<a>(&lt;%@))" + "(?<b>(.*))" + "(?<c>(%&gt;))";
			replaceExpr = String.Format(colorMarkerBTemplate, AspDirectiveColor, "${a}${b}${c}"); //"<font style='background-color:yellow'></font>";//<b>${a}${b}${c}</b></font>";
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				sourceLine = Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

			// Colorize <%# <type>
			//TODO: not using the colorMarkerBTemplate?
			searchExpr = "(?i)" + "(?<a>(&lt;%#))" + "(?<b>(.*))" + "(?<c>(%&gt;))";
			replaceExpr = "${a}" + "<span style='color: red'><b>" + "${b}" + "</b></span>" + "${c}";
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				sourceLine = Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

			// Colorize tag <type>
			searchExpr = "(?i)" + "(?<a>(&lt;)(?!%)(?!/?template)(?!/?property)(?!/?ibuyspy:)(/|!)?)" + "(?<b>[^;\\s&]+)" + "(?<c>(\\s|&gt;|\\Z))";
			replaceExpr = "${a}" + String.Format(colorMarkerTemplate, TagColor, "${b}") + "${c}";
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				sourceLine = Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

			// Colorize asp:|template for runat=server tags <type>
			searchExpr = "(?i)(?<a>&lt;/?)(?<b>(asp:|template|property|IBuySpy:).*)(?<c>&gt;)?"; 
			replaceExpr = "${a}" + String.Format(colorMarkerTemplate, KeywordColor, "<b>${b}</b>") + "${c}";
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				sourceLine = Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

			//colorize begin of tag char(s) "<","</","<%"
			searchExpr = "(?i)(?<a>(&lt;)(/|!)?)";//|%
			replaceExpr = String.Format(colorMarkerTemplate, KeywordColor, "${a}");
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				sourceLine = Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

			// Colorize end of tag char(s) ">","/>", "%>"
			searchExpr = "(?i)(?<a>(/)?(&gt;))";//|%
			replaceExpr = String.Format(colorMarkerTemplate, KeywordColor, "${a}");
			if (Regex.IsMatch(sourceLine, searchExpr))
			{
				sourceLine = Regex.Replace(sourceLine, searchExpr, replaceExpr);
			}

			return sourceLine;
		}

		private static string FixTabs(string sourceLine)
		{
			if (!ReplaceTabs)
				return sourceLine;

			sourceLine = Regex.Replace(sourceLine, "(?i)(\\t)", TabString);
			StringBuilder padding = new StringBuilder();
			while (sourceLine.StartsWith(" "))
			{
				sourceLine = sourceLine.Remove(0, 1);
				padding.Append("&nbsp;");
			}
			padding.Append(sourceLine);
			return padding.ToString();
		}

		private static string GetLangFromLine(string sourceLine, string defaultLanguage)
		{
			if (sourceLine == null)
			{
				return defaultLanguage;
			}

			sourceLine = FixTabs(sourceLine);

			Match langMatch = Regex.Match(sourceLine, "(?i)<%@\\s*Page\\s*.*Language\\s*=\\s*\"(?<lang>[^\"]+)\"");
			if (langMatch.Success)
			{
				return langMatch.Groups["lang"].ToString();
			}

			langMatch = Regex.Match(sourceLine, "(?i)(?=.*runat\\s*=\\s*\"?server\"?)<script.*language\\s*=\\s*\"(?<lang>[^\"]+)\".*>");
			if (langMatch.Success)
			{
				return langMatch.Groups["lang"].ToString();
			}

			langMatch = Regex.Match(sourceLine, "(?i)<%@\\s*WebService\\s*.*Language\\s*=\\s*\"?(?<lang>[^\"]+)\"?");
			if (langMatch.Success)
			{
				return langMatch.Groups["lang"].ToString();
			}
			return defaultLanguage;
		}

		protected static bool IsScriptBlockTagStart(String source)
		{
			if (Regex.IsMatch(source, "<script.*runat=\"?server\"?.*>"))
			{
				return true;
			}
			if (Regex.IsMatch(source, "(?i)<%@\\s*WebService"))
			{
				return true;
			}
			return false;
		}

		protected static bool IsScriptBlockTagEnd(String source)
		{
			if (Regex.IsMatch(source, "</script.*>"))
			{
				return true;
			}
			return false;
		}

		protected static bool IsMultiLineTagStart(String source)
		{
			String searchExpr = "(?i)(?!.*&gt;)(?<a>&lt;/?)(?<b>(asp:|template|property|IBuySpy:).*)";

			source = HttpUtility.HtmlEncode(source);
			if (Regex.IsMatch(source, searchExpr))
			{
				return true;
			}
			return false;
		}

		protected static bool IsMultiLineTagEnd(String source)
		{
			String searchExpr = "(?i)&gt;";

			source = HttpUtility.HtmlEncode(source);
			if (Regex.IsMatch(source, searchExpr))
			{
				return true;
			}
			return false;
		}
	}
	public class CodeViewerCrawler
	{
		public NameValueCollection DefaultPageDirFiles
		{
			get
			{
				return _DefaultPageDirFiles;
			}
		}
		public NameValueCollection DefaultExampleFiles
		{
			get
			{
				return _DefaultExampleFiles;
			}
		}
		public ArrayList DefaultExampleImageFiles
		{
			get
			{
				return _DefaultExampleImageFiles;
			}
		}

		private NameValueCollection _DefaultPageDirFiles;
		private NameValueCollection _DefaultExampleFiles;
		private ArrayList _DefaultExampleImageFiles;

		public CodeViewerCrawler(string pagePhysicalPath)
		{
			_DefaultPageDirFiles = new NameValueCollection();
			_DefaultExampleFiles = new NameValueCollection();
			_DefaultExampleImageFiles = new ArrayList();
			ProcessExampleDirectory(pagePhysicalPath);
		}

		private void ProcessExampleDirectory(string targetDirectory)
		{
			if (targetDirectory != String.Empty)
			{
				// Process the list of files found in the directory.
				string[] fileEntries = Directory.GetFiles(targetDirectory);
				AddFilesByExtension(Directory.GetFiles(targetDirectory), DefaultPageDirFiles);
				// Recurse into subdirectories of this directory.
				string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
				foreach (string subdirectory in subdirectoryEntries)
				{
					ProcessExampleDirectory(subdirectory);
				}
			}
		}

		public string GetOneFileOf(string format)
		{
			string[] files = DefaultPageDirFiles.GetValues((string)format);
			string currentFile = string.Empty;

			if (format.ToLower().EndsWith("aspx"))
			{
				string page = FindCurrentAspxPage(files);
				if (page != string.Empty)
					return page;
			}

			if (files != null && files.Length > 0 && files[0] != null)
			{
				currentFile = files[0];
			}
			return currentFile;
		}

		private void AddFilesByExtension(string[] inputArray, NameValueCollection inputCollection)
		{
			for (int i = 0; i < inputArray.Length; i++)
			{
				string extension = Path.GetExtension(inputArray[i]);

				if ((extension + string.Empty != string.Empty))
				{
					extension = extension.Substring(1);
					inputCollection.Add(extension, inputArray[i]);
				}
			}
		}

		private string FindCurrentAspxPage(string[] files)
		{
			string page = Path.GetFileName(System.Web.HttpContext.Current.Request.CurrentExecutionFilePath);
			foreach (string file in files)
			{
				if (file.ToLower().EndsWith(page.ToLower()))
					return file;
			}

			return string.Empty;
		}
	}
}

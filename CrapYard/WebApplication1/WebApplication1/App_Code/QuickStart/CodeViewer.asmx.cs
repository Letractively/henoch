using System.IO;
using System.Web.Script.Services;
using System.Web;
using System.Web.Services;

namespace Telerik.QuickStart
{

	[WebService(Namespace = "http://www.telerik.com/webservices/")]
	[ScriptService]
	public class CodeViewerService : WebService
	{
		[WebMethod]
		public string GetFileContent(string path, string fileName)
		{
			string absolutePath = Server.MapPath(path);
			string currentAbsolutePath = Context.Request.PhysicalApplicationPath;

			if (string.IsNullOrEmpty(fileName) || !absolutePath.ToLower().StartsWith(currentAbsolutePath.ToLower()) || fileName.ToLowerInvariant().Contains("web.config"))
				throw new HttpException(403, "Unauthorized");

			return CodeViewerHelper.RenderFile(Path.Combine(absolutePath, fileName));
		}

		[WebMethod]
		public string GetDescriptionFileContent(string path, string fileName)
		{
			string absolutePath = Server.MapPath(path);
			string currentAbsolutePath = Context.Request.PhysicalApplicationPath;

			if (string.IsNullOrEmpty(fileName) || !absolutePath.ToLower().StartsWith(currentAbsolutePath.ToLower()) || fileName.ToLowerInvariant().Contains("web.config"))
				throw new HttpException(403, "Unauthorized");

			string filePath = Path.Combine(absolutePath, "default.html");
			if (!File.Exists(filePath))
			{
				filePath = Path.Combine(absolutePath, "default.htm");
			}
			string fullText = CodeViewerHelper.RenderFile(filePath);

			string searchedSectionID = string.Empty;
			string sectionText = string.Empty;
			switch (fileName)
			{
				case "Demo Instructions":
					{
						searchedSectionID = "instructions-section";
						string nextSectionID = "description-section";
						sectionText = GetSection(fullText, searchedSectionID, nextSectionID);
					}
					break;
				case "Description":
					{
						searchedSectionID = "description-section";
						sectionText = GetSection(fullText, searchedSectionID, "related-resources-section");
					}
					break;
				case "Related Resources":
					{
						searchedSectionID = "related-resources-section";
						sectionText = GetSection(fullText, searchedSectionID, null);
					}
					break;
				default:
					break;
			}

			return sectionText;

		}

		private string GetSection(string text, string id, string nextSectionID)
		{
			int textLength = text.Length;
			int indexOfSearchedSectionID = text.IndexOf(id);
			string beginText = text.Substring(0, indexOfSearchedSectionID);
			int searchedDivIndex = beginText.LastIndexOf("<div");

			string otherText = text.Substring(searchedDivIndex, textLength - searchedDivIndex);

			if (string.IsNullOrEmpty(nextSectionID))
			{
				return otherText;
			}

			int indexOfNextSectionID = otherText.IndexOf(nextSectionID);
			if (indexOfNextSectionID > -1)
			{
				string tempText = otherText.Substring(0, indexOfNextSectionID);
				int searchedDivEndTagIndex = tempText.LastIndexOf("<div");

				return otherText.Substring(0, searchedDivEndTagIndex);
			}
			else
			{
				return otherText;
			}
		}
	}
}
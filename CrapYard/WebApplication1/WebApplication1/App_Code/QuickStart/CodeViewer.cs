using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Collections.Generic;

namespace Telerik.QuickStart
{
	public partial class CodeViewer : UserControl
	{
		private RadToolBarItem currentItem;
		
		private readonly string[] knownItems = new string[] {
			"*.aspx",
			"*.ascx",
			"*.cs",
			"*.vb",
			"*.js",
			"*.css",
            "*.xml"
		};

		private static readonly string[] filesToRemoveCS = new string[] { ".vb", "vb.aspx", "vb.ascx" };
		private static readonly string[] filesToRemoveVB = new string[] { ".cs", "cs.aspx", "cs.ascx" };
		private static readonly char[] pathSeparators = new char[2] { '\\', '/' };
		protected RadToolBar FileSelection;
		protected HtmlGenericControl codeListings;

		public string[] AdditionalCodeViewerFiles { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsCallback && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
			{
				string examplePath = MapPathSecure(Page.TemplateSourceDirectory);
				PopulateCodeViewer(examplePath);
				if (FileSelection.Items.Count > 0)
				{
					currentItem = FileSelection.Items[0];
				}
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (currentItem == null)
				return;

			HtmlGenericControl codeViewerElement = new HtmlGenericControl("li");
			codeListings.Controls.Add(codeViewerElement);
			string examplePath = MapPathSecure(Page.TemplateSourceDirectory);
            string text = currentItem.Text;
			if (string.IsNullOrEmpty(text))
			{
				RadToolBarSplitButton button = currentItem as RadToolBarSplitButton;
				if (button != null)
				{
					text = button.Buttons[0].Text;
					button.Buttons[0].CssClass = "description";
				}
			}
			else
			{
				//set the class only when it is not empty string for validation purposes
				codeViewerElement.Attributes["class"] = text.Replace(".", "-");
			}
            codeViewerElement.InnerHtml = CodeViewerHelper.RenderFile(Path.Combine(examplePath, text));
		}

		private void PopulateCodeViewer(string examplePath)
		{
            bool isFirstItem = true;
			foreach (string itemFilter in knownItems)
			{
				List<string> filesOfKnownType = new List<string>(Directory.GetFiles(examplePath, itemFilter));

				//strip path from these
				for (int i = 0; i < filesOfKnownType.Count; i++)
				{
					filesOfKnownType[i] = filesOfKnownType[i].Remove(0, examplePath.Length).TrimStart(pathSeparators);
				}

				if (AdditionalCodeViewerFiles != null && AdditionalCodeViewerFiles.Length>0)
				{
					foreach (string path in AdditionalCodeViewerFiles)
					{
						if (path.EndsWith(itemFilter.Replace("*",""),StringComparison.InvariantCultureIgnoreCase))
						{
							//add with full path
							filesOfKnownType.Add(path);
						}
					}
				}

				if (filesOfKnownType.Count > 0)
				{
					filesOfKnownType.RemoveAll(HasOtherLanguage);
				}
				if (filesOfKnownType.Count < 1)
				{
					continue;
				}

				RadToolBarItem fileTypeItem = CreateCodeViewerItem(itemFilter, filesOfKnownType);

                if (isFirstItem)
				{
					fileTypeItem.CssClass = "description";
                    isFirstItem = false;
				}
				else
				{
					string cssClass = "file-type-" + itemFilter.Replace("*.", "");

					if (fileTypeItem is RadToolBarSplitButton)
					{
						foreach (RadToolBarButton button in (fileTypeItem as RadToolBarSplitButton).Buttons)
						{
							button.CssClass = cssClass;
						}
					}
					else
					{
						fileTypeItem.CssClass = cssClass;
					}
				}

				FileSelection.Items.Add(fileTypeItem);
			}
		}

		private static bool HasOtherLanguage(String s)
		{
			string pageLanguage = PageUtility.CurrentLanguage ?? "CS";
			string[] filesToRemove = pageLanguage.Equals("CS", StringComparison.InvariantCultureIgnoreCase) ? filesToRemoveCS : filesToRemoveVB;

			foreach (string pattern in filesToRemove)
			{
				if (s.EndsWith(pattern, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		private RadToolBarItem CreateCodeViewerItem(string knownItem, List<string> fileNames)
		{
			RadToolBarItem codeViewerItem;

			if (fileNames.Count == 1)
			{
				codeViewerItem = CreateSingleCodeViewerItem(knownItem, fileNames[0]);
			}
			else
			{
				RadToolBarSplitButton multipleFileCodeViewerItem = new RadToolBarSplitButton();

				multipleFileCodeViewerItem.CausesValidation = false;

				foreach (string fileName in fileNames)
				{
					RadToolBarItem singleCodeViewerItem = CreateSingleCodeViewerItem(knownItem, fileName);

					multipleFileCodeViewerItem.Buttons.Add(singleCodeViewerItem);
				}

				codeViewerItem = multipleFileCodeViewerItem;
			}

			return codeViewerItem;
		}

		private RadToolBarItem CreateSingleCodeViewerItem(string itemFilter, string fileName)
		{
			string filePath = string.Empty;
			int lastIndex = fileName.LastIndexOfAny(pathSeparators);
			if (lastIndex != -1)
			{
				filePath = fileName.Substring(0,lastIndex+1);
				fileName = fileName.Remove(0,lastIndex+1);
			}
			RadToolBarButton codeViewerItem = new RadToolBarButton(fileName);
			if (!string.IsNullOrEmpty(filePath))
			{
				codeViewerItem.Value = filePath;
			}

			codeViewerItem.CausesValidation = false;

			return codeViewerItem;
		}
	}
}
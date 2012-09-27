using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Telerik.QuickStart
{
    public partial class DescriptionViewer : UserControl
    {
        private RadToolBarItem currentItem;

        private readonly string[] knownItems = new string[] {
			"Default.htm*"
		};

        protected RadToolBar SectionSelection;
        protected HtmlGenericControl descriptionListings;

        public string[] AdditionalCodeViewerFiles { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsCallback && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                string examplePath = MapPathSecure(Page.TemplateSourceDirectory);
                PopulateCodeViewer(examplePath);
                if (SectionSelection.Items.Count > 0)
                {
                    currentItem = SectionSelection.Items[0];
                }
            }
        }

        private string GetSection(string text, string id)
        {
            int idIndex = text.IndexOf(id);
            if (idIndex > -1)
            {
                string beginText = text.Substring(0, text.IndexOf(id));
                int lastIndexOfDivElement = beginText.LastIndexOf("<div");
                return beginText.Substring(0, lastIndexOfDivElement);
            }
            else
            {
                return text;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (currentItem == null)
                return;

            HtmlGenericControl codeViewerElement = new HtmlGenericControl("li");
            descriptionListings.Controls.Add(codeViewerElement);
            string examplePath = MapPathSecure(Page.TemplateSourceDirectory);
            string path = Path.Combine(examplePath, "default.html");
            if (!File.Exists(path))
            {
                path = Path.Combine(examplePath, "default.htm");
            }
            string text = CodeViewerHelper.RenderFile(path);
            bool isHasInstructions = text.IndexOf("id=\"instructions-section\"") > -1;
            if (isHasInstructions)
            {
                currentItem.Text = "Demo Instructions";
                codeViewerElement.InnerHtml = GetSection(text, "description-section");
                codeViewerElement.Attributes.Add("class", "Demo_Instructions");
            }
            else
            {
                currentItem.Text = "Description";
                codeViewerElement.InnerHtml = GetSection(text, "related-resources");
                codeViewerElement.Attributes.Add("class", "Description");
            }
        }

        private void PopulateCodeViewer(string examplePath)
        {
            string path = Path.Combine(examplePath, "default.html");
            if (!File.Exists(path))
            {
                path = Path.Combine(examplePath, "default.htm");
            }

            string text = CodeViewerHelper.RenderFile(path);

            List<string> sections = new List<string>();

            if (text.IndexOf("instructions-section") > -1)
            {
                sections.Add("Demo_Instructions");
            }

            if (text.IndexOf("description-section") > -1)
            {
                sections.Add("Description");
            }

            if (text.IndexOf("related-resources-section") > -1)
            {
                sections.Add("Related_Resources");
            }
            
            if(sections.Count == 0)
            {
                RadToolBarItem fileTypeItem = CreateCodeViewerItem("Description");
                fileTypeItem.CssClass = "description";
                SectionSelection.Items.Add(fileTypeItem);
            }
            
            bool isFirst = true;
            foreach (string section in sections)
            {
                RadToolBarItem fileTypeItem = CreateCodeViewerItem(section);

                if (isFirst)
                {
                    fileTypeItem.CssClass = "description";
                    isFirst = false;
                }

                SectionSelection.Items.Add(fileTypeItem);
            }
        }

        private RadToolBarItem CreateCodeViewerItem(string sectionName)
        {
            RadToolBarItem codeViewerItem;
            codeViewerItem = CreateSingleCodeViewerItem(sectionName);

            return codeViewerItem;
        }

        private RadToolBarItem CreateSingleCodeViewerItem(string sectionName)
        {
            RadToolBarButton codeViewerItem = new RadToolBarButton(sectionName);
            codeViewerItem.Value = sectionName;
            codeViewerItem.Text = sectionName.Replace("_", " ");
            codeViewerItem.CausesValidation = false;

            return codeViewerItem;
        }
    }
}
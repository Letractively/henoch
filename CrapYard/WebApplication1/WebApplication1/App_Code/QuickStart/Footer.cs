using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Telerik.QuickStart
{
	public class Footer : UserControl
	{
		protected DescriptionViewer descriptionViewer;
		protected CodeViewer codeViewer;
		protected Panel infoChartPanel;
		protected PlaceHolder AnalyticsPlaceholder;
		protected PlaceHolder CdnAccessCdnDetectionScriptPlaceHolder;
		protected HyperLink CSharpLink;
		protected HyperLink VBLink;
		protected HyperLink HyperLinkWAI10;
		protected HyperLink HyperLinkWAI20;
		protected HyperLink HyperLinkXHTML11;

		[DefaultValue(true)]
		public bool ShowCSharpLink
		{
			get
			{
				return (bool)(ViewState["ShowCSharpLink"] ?? true);
			}
			set
			{
				ViewState["ShowCSharpLink"] = value;
			}
		}

		[DefaultValue(true)]
		public bool ShowVBLink
		{
			get
			{
				return (bool)(ViewState["ShowVBLink"] ?? true);
			}
			set
			{
				ViewState["ShowVBLink"] = value;
			}
		}

		public bool ShowCodeViewer
		{
			get
			{
				if (ViewState["ShowCodeViewer"] == null)
				{
					return true;
				}
				return (bool)ViewState["ShowCodeViewer"];
			}
			set
			{
				ViewState["ShowCodeViewer"] = value;
			}
		}

		[DefaultValue("")]
		[TypeConverter(typeof(CodeFilesCollectionConverter))]
		public string[] AdditionalCodeViewerFiles
		{
			get
			{
				if (ViewState["AdditionalCodeViewerFiles"] == null)
				{
					return new string[0];
				}
				return (string[])ViewState["AdditionalCodeViewerFiles"];
			}
			set
			{
				ViewState["AdditionalCodeViewerFiles"] = value;
				codeViewer.AdditionalCodeViewerFiles = value;
				descriptionViewer.AdditionalCodeViewerFiles = value;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			// RadAjax / MS AJAX optimization
			if (((HttpContext.Current.Request.Headers["x-microsoftajax"] != null) && (HttpContext.Current.Request.Headers["x-microsoftajax"].IndexOf("Delta=true") >= 0)) ||
				(HttpContext.Current.Request.Form["RadAJAXControlID"] != null))
			{
				codeViewer.Visible = false;
				descriptionViewer.Visible = false;
			}
			else
			{
				codeViewer.Visible = PageUtility.IsExamplePage(Page);
				codeViewer.AdditionalCodeViewerFiles = AdditionalCodeViewerFiles;
				descriptionViewer.Visible = PageUtility.IsExamplePage(Page);
				descriptionViewer.AdditionalCodeViewerFiles = AdditionalCodeViewerFiles;
			}
			//---

			if (!ShowCodeViewer)
			{
				codeViewer.Visible = false;
				descriptionViewer.Visible = false;
			}

			bool XhtmlCompliant = false;
			Telerik.QuickStart.Header header = (Telerik.QuickStart.Header)Parent.FindControl("Header1");
			if (header != null)
			{
				XhtmlCompliant = header.XhtmlCompliant;
			}

			if (PageUtility.CurrentLanguage == "CS")
			{
				VBLink.NavigateUrl = Regex.Replace(Page.Request.RawUrl, "cs\\.aspx", "vb.aspx", RegexOptions.IgnoreCase);
				CSharpLink.CssClass = "qsfCodeSelectedCS";
			}
			else
			{
				CSharpLink.NavigateUrl = Regex.Replace(Page.Request.RawUrl, "vb\\.aspx", "cs.aspx", RegexOptions.IgnoreCase);
				VBLink.CssClass = "qsfCodeSelectedVB";
			}

			CSharpLink.Visible = ShowCSharpLink;
			VBLink.Visible = ShowVBLink;

			if (!XhtmlCompliant)
			{
				HyperLinkWAI10.Visible = false;
				HyperLinkWAI20.Visible = false;
				HyperLinkXHTML11.Visible = false;
				return;
			}

			if (ProductInfo.ControlName == "Chart")
			{
				infoChartPanel.Visible = true;
			}

			// Disabled Google Analytics for everything but telerik.com
			AnalyticsPlaceholder.Visible = Request.Url.Host.ToLowerInvariant().Contains("telerik.com");

			if (QsfCdnConfigurator.QsfCdnIsEnabled && !QsfCdnConfigurator.IsCdnDisabledByQueryParam(Request))
				CdnAccessCdnDetectionScriptPlaceHolder.Visible = true;

			base.OnLoad(e);
		}
	}
}

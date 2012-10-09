using System.Web.UI;
using System.Xml;
using System.Web.UI.WebControls;
using System.Text;
using System;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

namespace Telerik.QuickStart
{
	public class HeadTag : UserControl
	{
		private string _title = string.Empty;
		private string _keywords = string.Empty;
		private string _description = string.Empty;
		private string _productId = string.Empty;

        protected HtmlGenericControl QsfStyleSheetsLink;
        protected HtmlGenericControl QsfScriptsLink;
        protected PlaceHolder ScriptTagPlaceHolder;
		protected PlaceHolder RadStyleSheetManagerPlaceHolder;

        private IHttpRequestInfo _requestInfo;

		private XmlDocument examplesDoc
		{
			get
			{
				XmlDocument exampleList = (XmlDocument)Cache["ExamplesXmlDocument"];
				if (exampleList == null)
				{
					exampleList = new XmlDocument();
					XmlDocument configDoc = new XmlDocument();
					configDoc.Load(Server.MapPath(Configuration.ConfigFile));
					Configuration configuration = new Configuration(configDoc);
					exampleList.Load(Server.MapPath(configuration.ExamplesDataFile));
					Cache["ExamplesXmlDocument"] = exampleList;
				}
				return exampleList;
			}
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _requestInfo = new HttpRequestInfo(Request);
        }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			//read out the title, keywords, description -- SEO optimization
			//defaulting to empty string if not set
			string exampleQuery = string.Format("//example[contains('{0}', concat('/', translate(@name, 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')))]", Request.Url.AbsolutePath.ToUpper());
			XmlNode exampleConfig = examplesDoc.SelectSingleNode(exampleQuery);
			if (exampleConfig != null)
			{
				XmlNode productElement = FindProductElement(exampleConfig);
				if (productElement != null)
				{
					StringBuilder titleBuilder = new StringBuilder();
					string controlNameForTitle = productElement.Attributes["text"].Value;
					titleBuilder.AppendFormat("ASP.NET {0} Demo", controlNameForTitle == "Overview" ? "Controls Overview" : controlNameForTitle);

					XmlNode category = exampleConfig.ParentNode;
					if (category != productElement)
					{
						titleBuilder.AppendFormat(" - {0}", category.Attributes["text"].Value);
					}
					titleBuilder.AppendFormat(" - {0}", exampleConfig.Attributes["text"].Value);

					_title = titleBuilder.ToString();
					_description = productElement.Attributes["description"] != null ? productElement.Attributes["description"].Value : string.Empty;
				}
				_keywords = exampleConfig.Attributes["keywords"] != null ? exampleConfig.Attributes["keywords"].Value : string.Empty;

				InitProductId(exampleConfig);
			}
		}

		XmlNode FindProductElement(XmlNode exampleElement)
		{
			XmlNode result = exampleElement;
			while (result.Attributes["productId"] == null && result != exampleElement.OwnerDocument.DocumentElement)
			{
				result = result.ParentNode;
			}

			return result;
		}
		protected override void OnPreRender(System.EventArgs e)
		{
			base.OnPreRender(e);

			if (EnableStyleSheetManager)
			{
				Telerik.Web.UI.RadStyleSheetManager RadStyleSheetManager1 = new Telerik.Web.UI.RadStyleSheetManager();
				RadStyleSheetManager1.ID = "RadStyleSheetManager1";
				RadStyleSheetManagerPlaceHolder.Controls.Add(RadStyleSheetManager1);
			}


			if (Page != null && Page.Header != null)
			{
				Page.Title = Title;
			}

            SetupQsfResourcesLinks();

            bool enableCdn = false;

            if (QsfCdnConfigurator.QsfCdnIsEnabled && 
                QsfCdnConfigurator.HasCanAccessCdnCookie(Request) &&
                QsfCdnConfigurator.GetCanAccessCdnFromCookie(Request) &&
                !QsfCdnConfigurator.IsCdnDisabledByQueryParam(Request))
            {
                enableCdn = true;
            }

            SetupCdnOnRadScriptManager(enableCdn);
            SetupCdnOnRadStyleSheetManager(enableCdn);
		}

		private void InitProductId(XmlNode example)
		{
			while (example.ParentNode != null && example.ParentNode.Name != "examples")
				example = example.ParentNode;

			if (example.ParentNode == null)
				return;

			if (example.Attributes["productId"] != null)
				_productId = example.Attributes["productId"].Value;
		}

		public string ProductID
		{
			get { return _productId; }
		}

		public string Title
		{
			get
			{
				if (_title != string.Empty)
					return _title;

				if (ViewState["Title"] == null)
				{

					return ProductInfo.RadControlName + " by Telerik";
				}
				return (string)ViewState["Title"];
			}
			set
			{
				ViewState["Title"] = value;
				_title = value;
			}
		}

		public string KeyWords
		{
			get
			{
				if (_keywords != string.Empty)
					return _keywords;

				return string.Empty;
			}

			set
			{
				_keywords = value;
			}
		}

		public string Description
		{
			get
			{
				if (_description != string.Empty)
					return _description;

				return Title;
			}

			set
			{
				_description = value;
			}
		}

		public bool EnableStyleSheetManager
		{
			get
			{
				return ViewState["EnableStyleSheetManager"] != null ?
					(bool)ViewState["EnableStyleSheetManager"] : true;
			}
			set
			{
				ViewState["EnableStyleSheetManager"] = value;
			}
		}

        private void SetupQsfResourcesLinks()
        {
            QsfScriptsLink.Attributes["href"] = Page.ResolveUrl(QsfScriptsLink.Attributes["href"]);
            QsfStyleSheetsLink.Attributes["href"] = Page.ResolveUrl(QsfStyleSheetsLink.Attributes["href"]);

			// Create a new link element as link tags in UC in the head are rendered as HtmlGenericControl.
			HtmlLink stylelink = new HtmlLink();
			stylelink.Attributes["href"] = QsfStyleSheetsLink.Attributes["href"];
			stylelink.Attributes["type"] = "text/css";
			stylelink.Attributes["rel"] = "stylesheet";

			// Add a script tag pointing to the scripts location.
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes["src"] = QsfScriptsLink.Attributes["href"];
            script.Attributes["type"] = "text/javascript";

			ScriptTagPlaceHolder.Controls.Add(stylelink);
			ScriptTagPlaceHolder.Controls.Add(script);

            // Hide the <link> tags.
			QsfStyleSheetsLink.Visible = false;
			QsfScriptsLink.Visible = false;
        }

        private void SetupCdnForQsfResources()
        {
            if (QsfCdnConfigurator.QsfCdnIsEnabled)
            {
                if (!string.IsNullOrEmpty(QsfCdnConfigurator.ScriptsCdnUrlAppSetting))
                    QsfScriptsLink.Attributes["href"] = QsfCdnConfigurator.ResolveScriptsFullCdnPath(_requestInfo);

                if (!string.IsNullOrEmpty(QsfCdnConfigurator.SkinsCdnUrlAppSetting))
                    QsfStyleSheetsLink.Attributes["href"] = QsfCdnConfigurator.ResolveSkinsFullCdnPath(_requestInfo);
            }
        }

        private void SetupCdnOnRadScriptManager(bool enable)
        {
            RadScriptManager scriptManager = RadScriptManager.GetCurrent(Page) as RadScriptManager;

            if (scriptManager != null)
                scriptManager.CdnSettings.TelerikCdn = enable ? TelerikCdnMode.Enabled : TelerikCdnMode.Disabled;
        }

        private void SetupCdnOnRadStyleSheetManager(bool enable)
        {
            RadStyleSheetManager styleSheetManager = RadStyleSheetManager.GetCurrent(Page) as RadStyleSheetManager;

            if (styleSheetManager != null)
                styleSheetManager.CdnSettings.TelerikCdn = enable ? TelerikCdnMode.Enabled : TelerikCdnMode.Disabled;
        }
    }
}

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Telerik.QuickStart
{
	public class ExamplesNavigation : UserControl
	{
		protected TreeNavigation treeNavigator;
		protected PlaceHolder exampleTables;

		protected string CollapsedImageUrl
		{
			get
			{
				return ResolveUrl(treeNavigator.CollapsedImageUrl);
			}
		}
		protected string ExpandedImageUrl
		{
			get
			{
				return ResolveUrl(treeNavigator.ExpandedImageUrl);
			}
		}
		private void Page_Load(object sender, EventArgs e)
		{
			XmlDocument configDoc = new XmlDocument();
			configDoc.Load(Server.MapPath(Configuration.ConfigFile));
			Configuration configuration = new Configuration(configDoc);
			treeNavigator.ContentFile = configuration.ExamplesDataFile;
		}

		[Description("Server-side language used to generate example links.")]
		[Browsable(true)]
		[DefaultValue("C#")]
		[NotifyParentProperty(true)]
		public string NavigationLanguage
		{
			get
			{
				return treeNavigator.NavigationLanguage;
			}
			set
			{
				treeNavigator.NavigationLanguage = value;
			}
		}

		#region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);

		}

		#endregion
	}
}
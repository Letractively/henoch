using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;


namespace Telerik.QuickStart
{
	public class FooterHome : UserControl
	{
		protected PlaceHolder AnalyticsPlaceholder;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Disabled Google Analytics for everything but telerik.com
			AnalyticsPlaceholder.Visible = Request.Url.Host.ToLowerInvariant().Contains("telerik.com");
		}
	}
}

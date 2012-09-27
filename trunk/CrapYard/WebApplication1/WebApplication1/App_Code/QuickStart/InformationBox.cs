using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for InformationBox
/// </summary>

namespace Telerik.QuickStart
{
	public class InformationBox : Panel
	{
		public InformationBox()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private string title;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				if (!String.IsNullOrEmpty(value))
					title = String.Format(@"<p class=""title"">{0}</p>", value);
				else
					title = "";
			}
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			string finalTitle = Title;
			if (!String.IsNullOrEmpty(Title) && Controls.Count == 1 && Controls[0] is LiteralControl && String.IsNullOrEmpty((Controls[0] as LiteralControl).Text.Trim()))
			{
				finalTitle = Title.Replace("class=\"title\"", "class=\"title\" style=\"margin-bottom:0\"");
			}
			writer.Write(string.Format(@"<div class=""infoPanel""><div class=""infoInner"">{0}", finalTitle));
			base.RenderContents(writer);
			writer.Write(string.Format("</div></div>"));
		}
	}
}
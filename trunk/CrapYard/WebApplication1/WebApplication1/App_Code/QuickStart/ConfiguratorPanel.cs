using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Telerik.QuickStart
{
	public enum ConfiguratorPanelOrientation
	{
		Horizontal,
		Vertical
	}

	public class ConfiguratorPanel : Panel, IPostBackDataHandler
	{
		protected override void RenderContents(HtmlTextWriter writer)
		{
			writer.Write(string.Format(
				@"<div class=""qsfConfig{4}"">
				<a class=""cfgHead qsfClear"" href=""javascript:slideConfig('{0}', '{1}');"" onfocus=""blur()"">
                    <span class=""cfgTitle"">{2}</span>
					<span class=""cfgButton cfg{1}""></span>
                </a>
                <div class=""cfgContent qsfClear"" style=""{3}"">",
				UniqueID,
				Expanded ? "Up" : "Down",
				Title,
				Expanded ? "" : "display:none",
				Orientation == ConfiguratorPanelOrientation.Vertical ? " cfgVertical" : "")
			);

			base.RenderContents(writer);

			writer.Write(string.Format(@"<input type=""hidden"" name=""{0}"" value=""{1}"" /></div></div>",
				UniqueID,
				Expanded)
				);
		}

		[DefaultValue("Example Configuration")]
		public string Title
		{
			get { return (string)(ViewState["Title"] ?? "Example Configuration"); }
			set { ViewState["Title"] = value; }
		}

		[DefaultValue(false)]
		public bool Expanded
		{
			get { return (bool)(ViewState["Expanded"] ?? false); }
			set { ViewState["Expanded"] = value; }
		}

		[DefaultValue(ConfiguratorPanelOrientation.Horizontal)]
		public ConfiguratorPanelOrientation Orientation
		{
			get { return (ConfiguratorPanelOrientation)(ViewState["Orientation"] ?? ConfiguratorPanelOrientation.Horizontal); }
			set { ViewState["Orientation"] = value; }
		}

		public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			if (!string.IsNullOrEmpty(postCollection[postDataKey]))
			{
				Expanded = Convert.ToBoolean(postCollection[postDataKey]);
			}
			return true;
		}

		public void RaisePostDataChangedEvent()
		{
		}
	}
}
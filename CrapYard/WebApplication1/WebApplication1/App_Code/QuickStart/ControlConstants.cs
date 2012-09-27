using System.Web;
using System.Xml;

namespace Telerik.QuickStart
{
	public class ProductInfo
	{
		private static readonly string[] controls;
		static ProductInfo()
		{
			//ADD New controls to this list or they will not be properly capitalized in the demo titles
			controls = new string[]{
"Ajax",
"AsyncUpload",
"BinaryImage",
"Button",
"Calendar",
"Captcha",
"Chart",
"ColorPicker",
"ComboBox",
"Controls",
"DataPager",
"Dock",
"Editor",
"FileExplorer",
"Filter",
"FormDecorator",
"Grid",
"ImageEditor",
"Input",
"ListBox",
"ListView",
"Menu",
"Notification",
"OrgChart",
"PanelBar",
"Rating",
"RibbonBar",
"Rotator",
"Scheduler",
"SiteMap",
"Slider",
"SocialShare",
"Spell",
"Splitter",
"TabStrip",
"TagCloud",
"ToolBar",
"ToolTip",
"TreeList",
"TreeView",
"Upload",
"Window",
"XmlHttpPanel"};
		}
		public static string ControlName
		{
			get
			{
				return (string)HttpContext.Current.Items["ControlName"] ?? "Controls";
			}
			set
			{
				string control = value;
				int i=0;
				for (i = 0; i < controls.Length && controls[i].ToLowerInvariant() != control.ToLowerInvariant(); i++) ;
				if (i < controls.Length) control = controls[i];
				HttpContext.Current.Items["ControlName"] = control;
			}
		}

		public static string RadControlName
		{
			get
			{
				return "Rad" + ControlName;
			}
		}
	}
}

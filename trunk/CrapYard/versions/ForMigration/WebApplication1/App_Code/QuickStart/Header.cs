using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Xml.Linq;
using Telerik.Web;

namespace Telerik.QuickStart
{
	public class QSFSkinManager : RadSkinManager, IPostBackEventHandler
	{
		//TODO: Update this list when new skins are added
		private readonly List<string> newSkins = new List<string>() {  };

		public string ControlName
		{
			get { return (string)ViewState["ControlName"] ?? ""; }
			set
			{
				ViewState["ControlName"] = value;
				//Reset skins
				this.FillSkins(this.GetSkinChooser());
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			//Take care of Black skin
			//if (this.Skin == "Black")
			//    this.Page.Form.Attributes.Add("class", "qsfDark");
			//else
			//    this.Page.Form.Attributes.Remove("class");

			switch (this.Skin)
			{
				case "Black":
					this.Page.Form.Attributes.Add("class", "qsfDark");
					break;
				case "Office2010Black":
					this.Page.Form.Attributes.Add("class", "qsfOfficeDark");
					break;
				case "Transparent":
					this.Page.Form.Attributes.Add("class", "qsfTransparent");
					break;
				default:
					this.Page.Form.Attributes.Remove("class");
					break;

			}

			this.ShowChooser = false;
			base.OnPreRender(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qsfSkinMgr");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qscLink");
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:openSkinChooser(event);");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			RenderTitle(writer);
			writer.RenderEndTag(); // qscLink

			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qscAnimContainer");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qscDropDown");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			RenderCorners(writer);

			RenderChooser(writer, GetSkinChooser());

			writer.RenderEndTag(); // qscDropDown
			writer.RenderEndTag(); // qscAnimationContainer
			writer.RenderEndTag(); // qsfSkinMgt
		}

		private void RenderTitle(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qscTitle");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);

			writer.Write(Skin);
			writer.Write(" skin <span>&#9660;</span>");

			writer.RenderEndTag();
		}

		protected void RenderCorners(HtmlTextWriter writer)
		{
			string[] cornerClasses = new string[] { "qscDDTL", "qscDDTR", "qscDDBL", "qscDDBR" };
			foreach (string cssClass in cornerClasses)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.RenderEndTag();
			}
		}

		protected void RenderChooser(HtmlTextWriter writer, RadComboBox chooser)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qscDropDownList");
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);
			foreach (RadComboBoxItem option in chooser.Items)
			{
				RenderItem(writer, option);
			}
			writer.RenderEndTag();
		}

		protected void RenderItem(HtmlTextWriter writer, RadComboBoxItem option)
		{
			string postBackReference = "javascript:" + Page.ClientScript.GetPostBackEventReference(this, option.Text);
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, postBackReference);

			writer.RenderBeginTag(HtmlTextWriterTag.Li);

			var classAttr = string.Empty;
			if (option.Text == Skin)
				classAttr = "qscItemSelected";
			if (newSkins.Contains(option.Text))
				classAttr += (classAttr.Length > 0 ? " " : string.Empty) + "qscItemNew";

			if (classAttr.Length > 0)
				writer.AddAttribute(HtmlTextWriterAttribute.Class, classAttr);

			writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");

			writer.RenderBeginTag(HtmlTextWriterTag.A);

			string imageUrl = GetImageUrl(option);
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, option.Text);
			writer.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.Write(option.Text);
			writer.RenderEndTag();

			writer.RenderEndTag(); // A
			writer.RenderEndTag(); // LI
		}

		private string GetImageUrl(RadComboBoxItem option)
		{
			string controlName = ControlName.Replace("Rad", "").Replace("AsyncUpload", "Upload"); // RadAsyncUpload shares skins with RadUpload.
			EmbeddedSkinAttribute attr = skinAttributes != null ? skinAttributes[option.Text] : null;
			Type skinType = attr != null ? attr.Type : typeof(EmbeddedSkinAttribute);
			string resourceName = string.Format("Telerik.Web.UI.Skins.{0}.{1}.{0}.gif", option.Text, controlName);
			string url = option.Page.ClientScript.GetWebResourceUrl(skinType, resourceName);

			return url;
		}

		public void RaisePostBackEvent(string eventArgument)
		{
			ChangeSkin(eventArgument);
		}
	}

	public class Header : UserControl
	{
		protected PlaceHolder leftNavigation;
		protected PlaceHolder content;
		protected PlaceHolder tabs;
		protected HtmlGenericControl qsfResetTimeout;
		protected Label ExampleLabel;
		protected Literal ExampleName;
		protected Literal ProductImage;
		protected HyperLink LinkButtonPrev;
		protected HyperLink LinkButtonNext;

		protected string ProductLink
		{
			get
			{
				StringBuilder link = new StringBuilder();
				link.Append("http://www.telerik.com/");
				return link.ToString();
			}
		}

		protected string Title
		{
			get
			{
				StringBuilder title = new StringBuilder();
				title.Append(ProductInfo.RadControlName);
				title.Append(" by Telerik");
				return title.ToString();
			}
		}

		[Description("Server-side language used to generate example links.")]
		[Browsable(true)]
		[DefaultValue("CS")]
		[NotifyParentProperty(true)]
		public string NavigationLanguage
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return PageUtility.CurrentLanguage;
				}
				return "CS";
			}
			set
			{
				if (value.ToLower() == "c#")
					value = "CS";

				if (HttpContext.Current != null)
				{
					PageUtility.CurrentLanguage = value;
				}
			}
		}

		[DefaultValue(true)]
		public bool ShowSkinChooser
		{
			get
			{
				return (bool)(ViewState["ShowSkinChooser"] ?? true);
			}
			set
			{
				ViewState["ShowSkinChooser"] = value;
			}
		}

		[Description("Determines if the DbReset timer should be visible")]
		[DefaultValue(false)]
		[NotifyParentProperty(true)]
		public bool ShowDbResetTimer
		{
			get
			{
				if (!Request.Url.Host.ToLowerInvariant().Contains("demos.telerik.com"))
				{
					return false;
				}

				if (ViewState["ShowDbResetTimer"] == null)
				{
					return false;
				}
				return (bool)ViewState["ShowDbResetTimer"];
			}
			set
			{
				ViewState["ShowDbResetTimer"] = value;
			}
		}

		[Description("Server-side path to the navigation user control.")]
		[Browsable(true)]
		[DefaultValue("~/Common/ExamplesNavigation.ascx")]
		[NotifyParentProperty(true)]
		public string NavigationControl
		{
			get
			{
				if (ViewState["NavigationControl"] == null)
				{
					return "~/Common/ExamplesNavigation.ascx";
				}
				return (string)ViewState["NavigationControl"];
			}
			set
			{
				ViewState["NavigationControl"] = value;
			}
		}

		[Description("The current page is XHTML 1.1 compliant.")]
		[Browsable(true)]
		[DefaultValue(true)]
		[NotifyParentProperty(true)]
		public bool XhtmlCompliant
		{
			get
			{
				if (ViewState["XhtmlCompliant"] == null)
				{
					return true;
				}
				return (bool)ViewState["XhtmlCompliant"];
			}
			set
			{
				ViewState["XhtmlCompliant"] = value;
			}
		}

		public string ProductVersion
		{
			get
			{
				Assembly controlAssembly = typeof(RadWebControl).Assembly;
				Version version = controlAssembly.GetName().Version;
				int quarter = version.Minor;
				int versionYear = version.Major;
				int year = versionYear;
				int date = version.Build;
				int month = date / 100;
				if (month > 12)
				{
					year++;
					month %= 12;
				}
				int day = date % 100;
				return string.Format("Q{0} {1} released {2:d2}/{3:d2}/{4}",
					quarter, versionYear, month, day, year);
			}
		}

		public int DBResetHours
		{
			get
			{
				string value = ConfigurationManager.AppSettings.Get("DBResetHours");
				if (string.IsNullOrEmpty(value))
				{
					return 2;
				}
				return int.Parse(value);
			}
		}

		public List<DateTime> DbResetTimes
		{
			get
			{
				if (Cache["DbResetTimes"] == null)
				{
					Cache.Insert("DbResetTimes", PopulateResetTimes(), null,
								 new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
								 TimeSpan.Zero);
				}

				return ((List<DateTime>)Cache["DbResetTimes"]);
			}
		}

		private List<DateTime> PopulateResetTimes()
		{
			List<DateTime> resetTimes = new List<DateTime>();
			DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DateTime t = currentDate.AddDays(1);
			resetTimes.Add(currentDate);
			while (currentDate < t)
			{
				currentDate = currentDate.AddHours(DBResetHours);
				resetTimes.Add(currentDate);
			}
			return resetTimes;
		}

		public DateTime CurrentTimeToDbReset
		{
			get
			{
				if (Cache["DBResetStartTime"] == null)
				{
					Cache.Insert("DBResetStartTime", DateTime.Now, null, DateTime.Now.AddHours(DBResetHours),
								 TimeSpan.Zero);
				}

				return ((DateTime)Cache["DBResetStartTime"]);
			}
		}
		private void InitDbReset()
		{
			DateTime timeToNextReset = DbResetTimes.Find(delegate(DateTime time) { return DateTime.Now < time; });

			TimeSpan timeSpan = (timeToNextReset - DateTime.Now);
			qsfResetTimeout.InnerText = string.Format(CultureInfo.InvariantCulture,
														"{0} hours, {1} minutes, {2} seconds", timeSpan.Hours,
														timeSpan.Minutes, timeSpan.Seconds);
		}

		private void InitCurrentProduct()
		{
			string path = PageUtility.LocationInWebApp(Page);

			string controlName = path.Split("/".ToCharArray())[0];

			if (!string.IsNullOrEmpty(Context.Request.QueryString["product"]))
			{
				controlName = Context.Request.QueryString["product"];
			}

			ProductInfo.ControlName = controlName;
		}

		private void InitImages()
		{
			if (ProductImage != null)
			{
				ProductImage.Text = String.Format(@"<input type=""button"" class=""Img{0}"" value="" ""/>", ProductInfo.ControlName);
			}
		}

		private void Page_Load(object sender, EventArgs e)
		{
			if (Request.Browser.IsBrowser("IE") && Request.Browser.MajorVersion == 8)
			{
				Page.PreRender += new EventHandler(Page_IE8BGFix);
			}
			Control dbResetPanel = FindControl("DbResetPanel");
			if (dbResetPanel != null)
			{
				dbResetPanel.Visible = ShowDbResetTimer;

				InitDbReset();
			}

			InitCurrentProduct();

			//Configure skin chooser
			RadMenu rateMenu = (RadMenu)FindControl("RateMenu");
			if (rateMenu != null)
			{
				RadMenuItem skinItem = rateMenu.FindItemByText("SkinManager");
				if (skinItem != null)
				{
					if (!ShowSkinChooser)
					{
						//skinItem.Visible = ShowSkinChooser;  // This is not working.
						if (rateMenu.Items[0].Visible == true)
							rateMenu.Items[1].Visible = false;
						else
							rateMenu.Visible = false;
					}
					else
					{
						QSFSkinManager skinManager = GetSkinManager();
						if (skinManager != null)
						{
							skinManager.Enabled = ShowSkinChooser;

							if (ShowSkinChooser)
							{
								skinManager.ControlName = ProductInfo.RadControlName;
								skinManager.SkinChanged += new RadSkinManager.SkinChangedDelegate(skinManager_SkinChanged);
							}
						}
					}
				}
			}


			// RadAjax / MS AJAX optimization
			if ((HttpContext.Current.Request.Headers["x-microsoftajax"] == null) &&
				(HttpContext.Current.Request.Form["RadAJAXControlID"] == null))
			{
				LoadSideNavigation();
			}

			if (LinkButtonNext != null)
			{
				LinkButtonNext.NavigateUrl = GenerateLinks(true);
			}
			if (LinkButtonNext != null)
			{
				LinkButtonPrev.NavigateUrl = GenerateLinks(false);
			}

			InitImages();

			ScriptReference sr1 = new ScriptReference("Telerik.Web.UI.Common.Core.js", "Telerik.Web.UI");
			ScriptReference sr2 = new ScriptReference("Telerik.Web.UI.Common.jQuery.js", "Telerik.Web.UI");
			ScriptManager.GetCurrent(Page).Scripts.Add(sr1);
			ScriptManager.GetCurrent(Page).Scripts.Add(sr2);
		}

		private void Page_IE8BGFix(object sender, EventArgs e)
		{
			if (Page.Controls.Count > 0)
			{
				LiteralControl ct = Page.Controls[0] as LiteralControl;
				if (ct != null)
				{
					ct.Text = ct.Text.Replace("<html ", "<html class=\"_Telerik_IE8\" ");
				}
			}
		}

		private string GenerateLinks(bool isNextLink)
		{
			string link = string.Empty;

			XDocument doc = XDocument.Load(Server.MapPath("~/Common/Examples.xml"));
			string url = Page.Request.RawUrl;
			string shortUrl = Regex.Replace(url, Page.Request.ApplicationPath + "/", "", RegexOptions.IgnoreCase);
			var lastIndex = shortUrl.LastIndexOf("/");
			string nodeNameStr = shortUrl.Substring(0, lastIndex);
			string suffix = Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1];

			foreach (XElement element in doc.Descendants("example"))
			{
				if (element.Attribute("name").Value.ToUpper() == nodeNameStr.ToUpper())
				{
					if (element.Attribute("product") != null && Page.Request.QueryString["product"] != null &&
						Page.Request.QueryString["product"] != element.Attribute("product").Value)
					{
						continue;
					}
					XElement current = null;
					if (isNextLink)
					{
						current = GetNextElementReqursively(element);
					}
					else
					{
						current = GetPrevElementReqursively(element);
					}

					if (current == null)
					{
						if (isNextLink)
						{
							LinkButtonNext.Visible = false;
						}
						else
						{
							LinkButtonPrev.Visible = false;
						}

						return "#";
					}

					string product = "";
					// If the user navigates to the Application section links
					if (current.Attribute("product") != null)
					{
						product = string.Format("?product={0}", current.Attribute("product").Value);
					}

					string value = current.Attribute("name").Value;
					return string.Format("~/{0}/{1}{2}", value, suffix, product);
				}
			}

			return link;
		}

		private XElement GetPrevElementReqursively(XElement current)
		{
			XElement prevElement = current.PreviousNode as XElement;
			if (current.Attribute("productId") != null)
			{
				prevElement = current;
			}

			if (prevElement == null)
			{
				prevElement = GetPrevElementReqursively(current.Parent);
			}
			else if (prevElement.Attribute("default") != null)
			{
				return null;
			}
			while (prevElement != null && (prevElement.Attribute("name") == null || prevElement.Attribute("productId") != null))
			{
				prevElement = prevElement.LastNode as XElement;
				if (prevElement != null && prevElement.Attribute("default") != null)
				{
					return null;
				}
			}

			return prevElement;
		}

		private XElement GetNextElementReqursively(XElement current)
		{
			XElement nextElement = current.NextNode as XElement;
			if (current.Attribute("productId") != null)
			{
				nextElement = current;
			}

			if (nextElement == null)
			{
				nextElement = GetNextElementReqursively(current.Parent);
			}

			while (nextElement != null && (nextElement.Attribute("name") == null || nextElement.Attribute("productId") != null))
			{
				nextElement = nextElement.FirstNode as XElement;
				if (nextElement.Attribute("default") != null)
				{
					return null;
				}
			}

			return nextElement;
		}

		public string CurrentSkin
		{
			get
			{
				QSFSkinManager manager = GetSkinManager();
				if (manager != null)
					return manager.Skin;

				return "Default";
			}
		}

		public QSFSkinManager GetSkinManager()
		{
			RadMenu rateMenu = (RadMenu)FindControl("RateMenu");
			if (rateMenu != null)
			{
				RadMenuItem skinItem = rateMenu.FindItemByText("SkinManager");
				if (skinItem != null)
				{
					return (QSFSkinManager)skinItem.FindControl("RadSkinManager1");
				}
			}

			return null;
		}

		public RadRating GetRating()
		{
			RadMenu rateMenu = (RadMenu)FindControl("RateMenu");
			if (rateMenu != null)
			{
				RadMenuItem skinItem = rateMenu.FindItemByText("Rating");
				if (skinItem != null)
				{
					return (RadRating)skinItem.FindControl("RadRating1");
				}
			}

			return null;
		}

		void skinManager_SkinChanged(object sender, SkinChangedEventArgs e)
		{
			if (SkinChanged != null)
			{
				SkinChanged(this, e);
			}
		}

		public event EventHandler<SkinChangedEventArgs> SkinChanged;


		protected override void Render(HtmlTextWriter writer)
		{
			if (ExampleName != null)
			{
				ExampleName.Text = (string)Context.Items["CurrentExample"];
			}
			base.Render(writer);
		}

		private void LoadSideNavigation()
		{
			Control navigationControl = LoadControl(NavigationControl);
			navigationControl.ID = "nav";

			Type navigationType = navigationControl.GetType();
			PropertyInfo languageProperty = navigationType.GetProperty("NavigationLanguage");
			if (languageProperty != null)
			{
				languageProperty.SetValue(navigationControl, NavigationLanguage, null);

			}

			leftNavigation.Controls.Add(navigationControl);
		}

		protected override void OnInit(EventArgs e)
		{
			this.Load += new EventHandler(Page_Load);
			if (Request.Url.Host.ToLowerInvariant().Contains("telerik.com"))
			{
				RadRating ratingControl = GetRating();
				if (ratingControl != null)
				{
					try
					{
						// Check whether the user has already rated the example.
						using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QSFRatingsConnectionString"].ConnectionString))
						{
							string demo = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();

							string getAverageRating =
								"SELECT AVG([DemoRating]) from " +
								"(SELECT MAX([Rating]) AS [DemoRating] FROM [Ratings] WHERE [Demo]=@demo AND [Rating] IS NOT NULL AND [Rating] > 0 GROUP BY [IP]) AS DemoRatings";

							SqlCommand getAverageCommand = new SqlCommand(getAverageRating, connection);
							getAverageCommand.Parameters.AddWithValue("@demo", demo);

							connection.Open();
							ratingControl.DbValue = getAverageCommand.ExecuteScalar();

							string selectCommandText = "SELECT MAX([Rating]) from [Ratings] WHERE [IP]=@ip AND [Demo]=@demo";
							SqlCommand selectCommand = new SqlCommand(selectCommandText, connection);
							selectCommand.Parameters.AddWithValue("@ip", HttpContext.Current.Request.UserHostAddress);
							selectCommand.Parameters.AddWithValue("@demo", demo);

							object qsfRating = selectCommand.ExecuteScalar();
							if (!DBNull.Value.Equals(qsfRating))
							{
								double rating = (double)qsfRating;
								if (rating > 0)
								{
									ratingControl.ReadOnly = true;
									ratingControl.ToolTip = "You have already rated this demo. Thank you for your feedback.";
								}
							}
						}
					}
					catch (Exception)
					{
						ratingControl.ReadOnly = true;
					}
				}
			}
			else
			{
				RadMenu rateMenu = (RadMenu)FindControl("RateMenu");
				if (rateMenu != null)
				{
					RadMenuItem rateItem = rateMenu.FindItemByText("Rating");
					if (rateItem != null)
					{
						rateMenu.Items[0].Visible = false;
					}
				}
			}

			base.OnInit(e);
		}

	}
}

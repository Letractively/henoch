using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Telerik.QuickStart
{
	public class PageUtility
	{
		public static string LocationInWebApp(Page page)
		{
			
			string appPath = page.Request.ApplicationPath;
			if (!appPath.EndsWith("/")) appPath+="/";
			
			return page.TemplateSourceDirectory.Remove(0, appPath.Length);
		}

		public static string PageFileName(Page page)
		{
			return Path.GetFileName(page.Request.PhysicalPath);
		}

		public static bool IsExamplePage(Page page)
		{
			return page.TemplateSourceDirectory.ToLower().IndexOf("examples") > 0;
		}

		public static string LanguageSpecificUrl(string rawUrl)
		{
			return Regex.Replace(rawUrl, 
				".aspx", CurrentLanguage + ".aspx", 
				RegexOptions.IgnoreCase);
		}

		public static string CurrentLanguage
		{
			get
			{
				HttpContext context = HttpContext.Current;
				string queryParameter = context.Request.QueryString["QSLanguage"];
				if (queryParameter != null && queryParameter.Trim() != string.Empty)
				{
					context.Session["NavigationLanguage"] = queryParameter.Trim();
					return queryParameter.Trim();
				}

				if (context.Session["NavigationLanguage"] == null)
				{	
					context.Session["NavigationLanguage"] = "CS";
				}
				return (string) HttpContext.Current.Session["NavigationLanguage"];
			}
			set
			{
				HttpContext.Current.Session["NavigationLanguage"] = value;
			}
		}
	}
}

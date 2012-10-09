using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Telerik.QuickStart
{
	public class DefaultPage : Page
	{
		protected override void OnInit(EventArgs e)
		{
			//try to find the control name (if we are in a /Control/Default.aspx file)
			string controlPath = this.Request.CurrentExecutionFilePath;
			int lastSlashIndex = controlPath.LastIndexOf("/")-1;
			int controlSlashIndex = lastSlashIndex>0 ? controlPath.Substring(0,lastSlashIndex).LastIndexOf("/")+1 : -1;
			string controlName = string.Empty;
			if (controlSlashIndex >0)
			{
				controlName = controlPath.Substring(controlSlashIndex, lastSlashIndex - controlSlashIndex + 1).ToLower();
			}
			
			string exampleUrl = GetDefaultExampleUrl(controlName);
			PermanentRedirect(exampleUrl);
		}

		private string GetDefaultExampleUrl(string controlName)
		{
			//open quickstart config to find the examples file
			System.Xml.XmlDocument configDoc = new XmlDocument();
			configDoc.Load(Server.MapPath(Configuration.ConfigFile));
			Configuration configuration = new Configuration(configDoc);

			//open the examples file and try to find the control's branch
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			xmlDoc.Load(Server.MapPath(configuration.ExamplesDataFile));
			System.Xml.XmlNode controlNode = xmlDoc.DocumentElement.SelectSingleNode(string.Format("/examples/category[translate(@text, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='rad{0}']", controlName));
			if (Object.Equals(controlNode,null))
			{
				//try without the Rad prefix
				controlNode = xmlDoc.DocumentElement.SelectSingleNode(string.Format("/examples/category[translate(@text, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='{0}']", controlName));
			}
			if (Object.Equals(controlNode,null))
			{
				//if no control branch, use the root 
				controlNode = xmlDoc.DocumentElement;
			}
			
			System.Xml.XmlNode exampleNode = controlNode.SelectSingleNode("example[@default='true']");
			if (exampleNode == null)
				exampleNode = controlNode.SelectSingleNode("category/example[@default='true']");
			if (exampleNode != null)
			{
				//use lowercase for the application path! solves telerik.com site problem
				string exampleUrl = Request.ApplicationPath.ToLower();
				if (!exampleUrl.EndsWith("/")) exampleUrl += "/";
				exampleUrl += exampleNode.Attributes["name"].Value + "/Default.aspx";
				return PageUtility.LanguageSpecificUrl(exampleUrl);
			}
			else
			{
				//no default example found
				return string.Empty;
			}
		}

		private void PermanentRedirect(string exampleUrl)
		{
			//add a permanent redirect header or display error message if no url is given
			System.Web.HttpContext.Current.Response.Clear();
			if (exampleUrl != null && exampleUrl != string.Empty)
			{
				System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
				System.Web.HttpContext.Current.Response.AddHeader("Location", exampleUrl);
			}
			else
			{
				System.Web.HttpContext.Current.Response.Write("Cannot redirect to default example..");
			}
			System.Web.HttpContext.Current.Response.Flush();
			System.Web.HttpContext.Current.Response.End();
		}
	}
}
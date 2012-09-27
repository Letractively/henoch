using System.Xml;

namespace Telerik.QuickStart
{
	public class Configuration
	{
		public const string ConfigFile = "~/QuickStart.config";
		private string _exampleNavigationPath = "~/Common/ExamplesNavigation.ascx";
		private string _examplesDataFile = "~/Common/Examples.xml";
		
		public Configuration(XmlDocument configDoc)
		{
			XmlNode exampleNavigation = configDoc.SelectSingleNode("//navigationControl");
			if (exampleNavigation != null)
			{
				_exampleNavigationPath = exampleNavigation.InnerText;
			}

			XmlNode dataFile = configDoc.SelectSingleNode("//dataFile");
			if (dataFile != null)
			{
				_examplesDataFile = dataFile.InnerText;
			}
		}

		
		public string ExamplesNavigationControlPath
		{
			get
			{
				return _exampleNavigationPath;
			}
		}

		public string ExamplesDataFile
		{
			get
			{
				return _examplesDataFile;
			}
		}
	}


}

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
using Telerik.Web.UI;

namespace Telerik.QuickStart
{
    public class InfoPanel : UserControl
    {
        private bool IsControlsTheCurrentProduct
        {
            get
            {
                return ProductInfo.ControlName.ToLower() == "controls";
            }
        }

        public string ProductVersion
        {
            get
            {
                Assembly controlAssembly = typeof(RadWebControl).Assembly;
                Version version = controlAssembly.GetName().Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            }
        }

        public string OnlineHelpUrl
        {
            get
            {
                if (IsControlsTheCurrentProduct)
                {
                    return "http://www.telerik.com/help/aspnet-ajax/introduction.html";
                }
                return string.Format("http://www.telerik.com/help/aspnet-ajax/{0}overview.html", ProductInfo.ControlName.ToLower());
            }
        }

        public string ForumUrl
        {
            get
            {
                if (IsControlsTheCurrentProduct)
                {
                    return "http://www.telerik.com/community/forums.aspx";
                }
                return string.Format("http://www.telerik.com/community/forums/aspnet-ajax/{0}.aspx", ProductInfo.ControlName.ToLower());
            }
        }

        public string KbUrl
        {
            get
            {
                if (IsControlsTheCurrentProduct)
                {
                    return "http://www.telerik.com/support/kb/aspnet-ajax.aspx";
                }
                return string.Format("http://www.telerik.com/support/kb/aspnet-ajax/{0}.aspx", ProductInfo.ControlName.ToLower());
            }
        }

        public string CodeLibraryUrl
        {
            get
            {
                if (IsControlsTheCurrentProduct)
                {
                    return "http://www.telerik.com/community/code-library.aspx";
                }
                return string.Format("http://www.telerik.com/community/code-library/aspnet-ajax/{0}.aspx", ProductInfo.ControlName.ToLower());
            }
        }

        public string BrowserSupportUrl
        {
            get
            {
                return "http://www.telerik.com/products/aspnet-ajax/resources/browser-support.aspx";
            }
        }

        public string AccessibilityUrl
        {
            get
            {
                return "http://www.telerik.com/products/aspnet-ajax/resources/accessibility-support.aspx";
            }
        }

        public string XhtmlUrl
        {
            get
            {
                return "http://www.telerik.com/products/aspnet-ajax/resources/xhtml-compliance.aspx";
            }
        }

        public string AtlasUrl
        {
            get
            {
                return "http://www.telerik.com/help/aspnet-ajax/generalaspnetajaxinfo.html";
            }
        }
    }
}

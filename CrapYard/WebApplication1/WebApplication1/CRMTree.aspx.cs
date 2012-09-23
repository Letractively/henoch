using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Caching;
using Repository;
using Telerik.Web.UI;

namespace WebApplication1
{
    public partial class CRMTree : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)             
            {

           
            }
         }

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid control = (RadGrid)sender;

            string xml = new ShareHolders().CreateXMLOrganoTreeView(control.SelectedValue.ToString());
            RadTreeView1.LoadXml(xml);
        }   
    }
}
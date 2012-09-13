using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Caching;
using Repository;

namespace WebApplication1
{
    public partial class CRMTree : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                new ShareHolders().Refresh();
            }
            else
            { 

            }
         }   
    }
}
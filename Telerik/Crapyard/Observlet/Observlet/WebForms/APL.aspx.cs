using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataResource;

namespace Observlet.WebForms
{
    public partial class APL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                datagrid1.DataSource = new MyAccess().GetDummyDataSet();
                datagrid1.DataBind();

                GridView1.DataSource = new MyAccess().GetDummyDataSet();
                GridView1.DataBind();
            }
        }
    }

}
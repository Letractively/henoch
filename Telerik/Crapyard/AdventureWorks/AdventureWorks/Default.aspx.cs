using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AdventureWorks
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var trans = Request.QueryString.Get("transaction_id");
            //TODO: if trans is not null transfer to payment service...

            if (!Page.IsPostBack)
            {
                List<ProductCategory> data = DataAccessLayer.Products.GetCategories();

                lbCategories.DataSource = data;
                lbCategories.DataBind();
            }
            else
            {
                if (lbCategories.SelectedIndex != -1)
                {
                    Thread.Sleep(1000);
                    string category = lbCategories.SelectedValue;
                    Response.Redirect("~/Products.aspx?id=" + category);

                    string curDir= MapPath(".");
                }

            }
        }

    }
}

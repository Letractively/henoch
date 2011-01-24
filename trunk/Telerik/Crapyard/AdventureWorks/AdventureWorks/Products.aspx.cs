using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AdventureWorks
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string categoryName;
                string categoryId = "5";

                if (Request["id"] != null)
                {
                    categoryId = Request["id"];
                }

                categoryName = DataAccessLayer.Products.GetCategoryName(int.Parse(categoryId));
                lblCategory.Text = categoryName;

                var data = DataAccessLayer.Products.GetProductsByCategory(int.Parse(categoryId));

                gvProducts.DataSource = data;
                gvProducts.DataBind();
            }


        }
    }
}
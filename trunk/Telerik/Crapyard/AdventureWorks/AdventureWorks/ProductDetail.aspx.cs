using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AdventureWorks
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string productId;

                if (Request["id"] != null)
                {
                    productId = Request["id"];

                    var data = DataAccessLayer.Products.GetProduct(int.Parse(productId));

                    lblName.Text = data.Name;
                    lblId.Text = data.ProductID.ToString();
                    lblColor.Text = data.Color;
                    lblSize.Text = data.Size;
                    lblWeight.Text = data.Weight.ToString();
                    lblListPrice.Text = data.ListPrice.ToString();
                    hdnProductId.Value = data.ProductID.ToString();
                    hdnProductName.Value = data.Name;
                    hdnListPrice.Value = data.ListPrice.ToString();
                }
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                List<clsShoppingCart> cart;
                int qty = 0;
                qty = int.Parse(txtQuantity.Text);

                if (Session["_cart"] == null)
                {
                    cart = new List<clsShoppingCart>();
                    Session["_cart"] = cart;
                }

                cart = (List<clsShoppingCart>)Session["_cart"];
                cart.Add(new clsShoppingCart { ProductId = int.Parse(hdnProductId.Value), Name = hdnProductName.Value, ListPrice = decimal.Parse(hdnListPrice.Value), Quantity = qty });
                Session["_cart"] = cart;
                Response.Redirect("~/ShoppingCart.aspx", true);
            }
            catch
            {
                lblError.Text = txtQuantity.Text + " is not a valid number";
            }
        }
    }
}
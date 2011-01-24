using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdventureWorks
{
    public partial class ShoppingCart1 : System.Web.UI.Page, ICallbackEventHandler
    {
        private string _callbackArgs;

        protected void Page_Load(object sender, EventArgs e)
        {
            //register the name of the client-side function that will
            // be called by the server
            string callbackRef = Page.ClientScript.GetCallbackEventReference(
            this, "args", "IdealProcessing", "");
            //define a function used by the client to call the server
            string callbackScript = string.Format("function MyServerCall(args) {{{0};}}", callbackRef);
            //register the client function with the page
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
            "MyServerCall", callbackScript, true);

            if (!Page.IsPostBack)
            {
                if (Session["_cart"] != null)
                {
                    List<clsShoppingCart> cart = (List<clsShoppingCart>) Session["_cart"];
                    gvCart.DataSource = cart;
                    gvCart.DataBind();
                }
                else
                {
                    lblMessage.Text = "Your cart is empty";
                    btnPlaceOrder.Visible = false;
                }
            }

            if (this.gvCart.Rows.Count > 0)
            {
                gvCart.UseAccessibleHeader = true;
                gvCart.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvCart.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", true);
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            Session["_cart"] = null;
            Response.Redirect("~/ThankYou.aspx", true);
        }

        #region Implementation of ICallbackEventHandler
        /// <summary>
        /// Client calls server here.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaiseCallbackEvent(string eventArgument)
        {
            Session["transaction_id"] = eventArgument;
            _callbackArgs = eventArgument;
        }
        /// <summary>
        /// Server calls client here.
        /// </summary>
        /// <returns></returns>
        public string GetCallbackResult()
        {
            return _callbackArgs; ;
        }

        #endregion
    }
}
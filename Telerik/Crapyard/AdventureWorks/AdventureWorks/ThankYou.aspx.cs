using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdventureWorks
{
    public partial class ThankYou : System.Web.UI.Page, ICallbackEventHandler
    {
        private string _callbackArgs;

        protected void Page_Load(object sender, EventArgs e)
        {
            //register the name of the client-side function that will
            // be called by the server
            string callbackRef = Page.ClientScript.GetCallbackEventReference(
            this, "args", "IDealCompleted", "");
            //define a function used by the client to call the server
            string callbackScript = string.Format("function MyServerCall(args) {{{0};}}", callbackRef);
            //register the client function with the page
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
            "MyServerCall", callbackScript, true);
        }

        #region Implementation of ICallbackEventHandler
        /// <summary>
        /// Client calls server here.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaiseCallbackEvent(string eventArgument)
        {
            _callbackArgs = "error";
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals("completed"))
            {
                _callbackArgs = Session["transaction_id"] as string;
                Session["transaction_id"] = null;
            }
            
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
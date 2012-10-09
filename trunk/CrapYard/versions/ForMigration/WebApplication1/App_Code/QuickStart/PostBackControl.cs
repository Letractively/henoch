using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Telerik.QuickStart
{
    public class PostBack : Control
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Button PostBackButton = new Button();
            PostBackButton.ID = "PostBackButton";
            PostBackButton.Text = "PostBack";
            PostBackButton.CssClass = "button";
            Controls.Add(PostBackButton);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(@"<div class=""bigModule""><div class=""bigModuleBottom"">Click PostBack button to see that the state is preserved : &nbsp;");
            base.Render(writer);
            writer.Write("</div></div>");
        }
    }
}

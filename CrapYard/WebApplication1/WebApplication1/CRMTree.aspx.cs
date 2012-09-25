using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Caching;
using Repository;
using Telerik.Web.UI;
using System.Text;

namespace WebApplication1
{
    public partial class CRMTree : System.Web.UI.Page
    {

        public string ZoekString 
        { 
            get
            {
                return Session["ZoekString"].ToString();
            }
            set
            {
                Session["ZoekString"] = value;
            } 
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ZoekString = RadTextBox1.Text;
            if (IsPostBack)             
            {

           
            }
         }

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid control = (RadGrid)sender;

            ZoekString = control.SelectedValue.ToString();
            string xml = new ShareHolders().CreateXMLOrganoTreeView(ZoekString);
            RadTreeView1.LoadXml(xml);
        }

        protected void RadGrid1_DataBound(object sender, EventArgs e)
        {

        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                // only access item if not header or footer cell
                if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    GridColumn column = RadGrid1.MasterTableView.GetColumn("custname");

                    foreach (TableCell cell in dataItem.Cells)
                    {
                        if (cell.Text.ToLower().IndexOf(ZoekString.ToLower()) != -1)
                            cell.CssClass = "wordfound";
                    }


                }
            }
            catch (Exception ex)
            {
                
            }
        }   
    }
}
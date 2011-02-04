using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using DataResource.Repository;
using Telerik.Web.UI;

namespace Observlet.WebForms
{
    public partial class NoViewstate : System.Web.UI.Page
    {

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable("Team");
            dataTable.Columns.Add("name", typeof(string));
            dataTable.Columns.Add("stadion", typeof(int));
            dataSet.Tables.Add(dataTable);

            String FilePath;
            FilePath = Server.MapPath(@"/App_Data/Repository.xml");

            dataSet.ReadXml(FilePath, XmlReadMode.IgnoreSchema);
            RadGrid1.Visible = true;
            RadGrid1.DataSource = BusinessDataStorage.GetCategories();
            RadGrid2.DataSource = dataSet;
            RadGrid2.Visible = false;

        }

        protected void RadGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            if (e.DetailTableView.Name == "RelatedItems")
            {
                int itemId = (int)e.DetailTableView.ParentItem.GetDataKeyValue("ID");
                e.DetailTableView.DataSource = BusinessDataStorage.GetHierarchicalData(itemId);
            }
            else if (e.DetailTableView.Name == "InnerMost")
            {
                int catId = (int)e.DetailTableView.ParentItem.GetDataKeyValue("CategoryID");
                e.DetailTableView.DataSource = BusinessDataStorage.GetData(catId);
            }
            else
            {
                int catId = (int)e.DetailTableView.ParentItem.GetDataKeyValue("ID");
                e.DetailTableView.DataSource = BusinessDataStorage.GetData(catId);
            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                //Attach to each GridTableView's Load event to save the expanded item state
                e.Item.OwnerTableView.Load += new EventHandler(GridTableView_Load);

                //Attach to each GridTableView's Load event to load the expanded item state
                e.Item.OwnerTableView.DataBound += new EventHandler(GridTableView_DataBound);
            }
        }

        void GridTableView_Load(object sender, EventArgs e)
        {
            GridTableView table = (GridTableView)sender;
            foreach (GridDataItem item in table.Items)
            {
                //build the item key that will be used to save the item as expanded
                string itemKey = BuildItemKey(item);
                if (item.Expanded)
                {
                    if (!ExpandedItemKeys.Contains(itemKey))
                    {
                        ExpandedItemKeys.Add(itemKey);
                    }
                }
                else
                {
                    if (ExpandedItemKeys.Contains(itemKey))
                    {
                        ExpandedItemKeys.Remove(itemKey);
                    }
                }
            }
        }

        void GridTableView_DataBound(object sender, EventArgs e)
        {
            GridTableView table = (GridTableView)sender;

            foreach (GridDataItem item in table.Items)
            {
                string itemKey = BuildItemKey(item);
                //if the item key is contained in the collection of saved items
                //this means the item should be expanded
                if (!item.Expanded && ExpandedItemKeys.Contains(itemKey))
                {
                    item.Expanded = true;
                }
            }

        }

        /// <summary>
        /// Build an item key that will uniquely identify a grid item
        /// among all the items in the RadGrid hierarchy. Use a combination 
        /// of the OwnerTableView.UniqueID and the set of all data values.
        /// </summary>
        protected string BuildItemKey(GridDataItem item)
        {
            string[] keyNames = item.OwnerTableView.DataKeyNames;
            if (keyNames.Length == 0) return item.ItemIndexHierarchical;

            string returnKey = item.OwnerTableView.UniqueID + "::";

            foreach (string keyName in keyNames)
            {
                returnKey += item.GetDataKeyValue(keyName).ToString();

                if (keyName != keyNames[keyNames.Length - 1])
                {
                    returnKey += "::";
                }
            }

            return returnKey;
        }

        /// <summary>
        /// Gets the expanded item state saved in the Session
        /// </summary>
        protected List<string> ExpandedItemKeys
        {
            get
            {
                if (Session["ExpandedItemKeys"] == null)
                {
                    Session["ExpandedItemKeys"] = new List<string>();
                }

                return (List<string>)Session["ExpandedItemKeys"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Text = "Not postback";
            }
            else
            {
                Label1.Text = "postback";
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            // Define the name and type of the client scripts on the page.
            StringBuilder sb = new StringBuilder();
            Type cstype = GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            String csname1 = "jQueryScript";

            #region jQuery

            sb.Append("$(document).ready(function() {");
            sb.Append(
                @"  $(""#.delbutton"").click(function() {
                        try{                            
                            
                            //Get the Id of the record to delete  
                            var record_id = $(this).attr(""id"");  

                            //Get the GridView Row reference  
                            var tr_id = $(this).parents(""#.record"");                                                        
                            
                            //add  GridView row 
                            //$(""#.gridview > tbody:nth-child(1)"").after('<TR><td>Hello World!</td><td></td><td></td><td></td><td></td></TR>'); 
                            //$('.gridview tr:last').after('<TR><td>WORLD!</td><td></td><td></td><td></td><td></td></TR>');   //<td>WORLD!</td><td></td><td></td>
                            var cloned = tr_id.clone(true);
                            tr_id.clone(true).insertAfter(tr_id);
                            
                            $(""#test tr:last"").after('<td>WORLD!</td><td></td><td></td>');   //<td>WORLD!</td><td></td><td></td> 
                            $('#test2 > tbody:last').after('<td>WORLD!</td><td></td><td></td>');   //<td>WORLD!</td><td></td><td></td>   

                            $('.label').hide(""slow"");  $('.button').hide(""slow""); 
                            //alert('done');
                        }
                        catch(Error){                                       
                            $(""#ObservableByErrorHandler"").append(""<div>"" + Error.description + ""</div>"");  
                        }
                    });

                    $(""#ObservableByErrorHandler"").click(function() {
                        $(this).hide(""slow"");            
                    });
                    $(""#Button1"").click(function() {
                        
                       $(this).hide(""slow"");   
                        alert('done');
           
                    });
                    ");
            sb.Append("});");

            #endregion

            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(cstype, csname1))
            {
                //ClientScript.RegisterClientScriptBlock(cstype, csname1, sb.ToString(), true);
            }
        }
    }
}
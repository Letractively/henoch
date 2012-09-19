using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class TrotterOverview : System.Web.UI.Page 
{
    private string gridMessage = null;

    public string OrderNo
    {
        get { return Session["OrdNo"] as string; }
        set { Session["OrdNo"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            OrderNo = "O201104322";
        }

    }
    protected void RadGrid1_DataBound(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(gridMessage))
        {
            DisplayMessage(gridMessage);
        }
    }

	protected void RadGrid1_ItemUpdated(object source, Telerik.Web.UI.GridUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.KeepInEditMode = true;
            e.ExceptionHandled = true;
            SetMessage("Update failed. Reason: " + e.Exception.Message);
        }
        else
        {
            SetMessage("Item updated!");
        }
    }

    protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            SetMessage("Insert failed! Reason: " + e.Exception.Message);
        }
        else
        {
            SetMessage("New product is inserted!");
        }
    }

    protected void RadGrid1_ItemDeleted(object source, GridDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            SetMessage("Delete failed! Reason: " + e.Exception.Message);
        }
        else
        {
            SetMessage("Item deleted!");
        }
    }

	private void DisplayMessage(string text)
    {
        RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
    }

    private void SetMessage(string message)
    {
        gridMessage = message;
    }
    //void GridTableView_Load(object sender, EventArgs e)
    //{
    //    GridTableView table = (GridTableView)sender;
    //    foreach (GridDataItem item in table.Items)
    //    {
    //        if (item.Expanded)
    //        {
    //            //build the item key that will be used to save the item as expanded
    //            string itemKey = BuildItemKey(item);
    //            if (!ExpandedItemKeys.Contains(itemKey))
    //            {
    //                ExpandedItemKeys.Add(itemKey);
    //            }
    //        }
    //    }
    //}

    //void GridTableView_DataBound(object sender, EventArgs e)
    //{
    //    GridTableView table = (GridTableView)sender;

    //    foreach (GridDataItem item in table.Items)
    //    {
    //        string itemKey = BuildItemKey(item);
    //        //if the item key is contained in the collection of saved items
    //        //this means the item should be expanded
    //        if (!item.Expanded && ExpandedItemKeys.Contains(itemKey))
    //        {
    //            item.Expanded = true;
    //            ExpandedItemKeys.Remove(itemKey);
    //        }
    //    }

    //}

    ///// <summary>
    ///// Build an item key that will uniquely identify a grid item
    ///// among all the items in the RadGrid hierarchy. Use a combination 
    ///// of the OwnerTableView.UniqueID and the set of all data values.
    ///// </summary>
    //protected string BuildItemKey(GridDataItem item)
    //{
    //    string[] keyNames = item.OwnerTableView.DataKeyNames;
    //    if (keyNames.Length == 0) return item.ItemIndexHierarchical;

    //    string returnKey = item.OwnerTableView.UniqueID + "::";

    //    foreach (string keyName in keyNames)
    //    {
    //        string val = item.GetDataKeyValue(keyName).ToString();
    //        OrderNo = val;
    //        returnKey += val;

    //        if (keyName != keyNames[keyNames.Length - 1])
    //        {
    //            returnKey += "::";
    //        }
    //    }

    //    return returnKey;
    //}

    ///// <summary>
    ///// Gets the expanded item state saved in the Session
    ///// </summary>
    //protected List<string> ExpandedItemKeys
    //{
    //    get
    //    {
    //        if (Session["ExpandedItemKeys"] == null)
    //        {
    //            Session["ExpandedItemKeys"] = new List<string>();
    //        }

    //        return (List<string>)Session["ExpandedItemKeys"];
    //    }
    //}

    protected void RadGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        OrderNo = (string)e.DetailTableView.ParentItem.GetDataKeyValue("ordNo");
        e.DetailTableView.DataSource = SqlDataSource1;
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }
}

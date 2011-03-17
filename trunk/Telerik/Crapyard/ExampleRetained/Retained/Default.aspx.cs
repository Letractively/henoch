using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using AsyncHandlers;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Web.UI;

public partial class _Default : Page
{
    private Hashtable _ordersExpandedState;
    private Hashtable _selectedState;
    private AsyncHandler _AsyncHandler;
    private const string ORDERS_EXPANDED_STATE = "_ordersExpandedState";

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //reset states
            this._ordersExpandedState = null;
            this.Session[ORDERS_EXPANDED_STATE] = null;
			this._selectedState = null;
            this.Session["_selectedState"] = null;

        }

        
    }

    private void DoAsync()
    {
        if (IsAsync)
        {
            Session["Halted"] = false;
            Timer1.Enabled = true;
            var asyncHandler = new AsyncHandler(Context);
            asyncHandler.NotifyHaltHandler += Halted;
            asyncHandler.NotifyLogger += Logger;
            BeginEventHandler bh = new BeginEventHandler(asyncHandler.BeginProcessRequest);
            EndEventHandler eh = new EndEventHandler(asyncHandler.EndProcessRequest);

            AddOnPreRenderCompleteAsync(bh, eh);

            // Initialize the WebRequest.
            string address = "http://localhost/";

            asyncHandler.WebRequest = System.Net.WebRequest.Create(address);
        }
    }

    private void Logger(object sender, NotifyObserverEventargs e)
    {
        Session["Logger"] = e.Message;
    }

    private void Halted(object sender, NotifyObserverEventargs e)
    {
        Session["Halted"] = true;
    }

    //Save/load expanded states Hash from the session
    //this can also be implemented in the ViewState
    private Hashtable ExpandedStates
    {
        get
        {
            if (this._ordersExpandedState == null)
            {
                _ordersExpandedState = this.Session[ORDERS_EXPANDED_STATE] as Hashtable;
                if (_ordersExpandedState == null)
                {
                    _ordersExpandedState = new Hashtable();
                    this.Session[ORDERS_EXPANDED_STATE] = _ordersExpandedState;
                }
            }

            return this._ordersExpandedState;
        }
    }

    //Clear the state for all expanded children if a parent item is collapsed
    private void ClearExpandedChildren(string parentHierarchicalIndex)
    {
        string[] indexes = new string[this.ExpandedStates.Keys.Count];
        this.ExpandedStates.Keys.CopyTo(indexes, 0);
        foreach (string index in indexes)
        {
            //all indexes of child items
            if (index.StartsWith(parentHierarchicalIndex + "_") ||
                index.StartsWith(parentHierarchicalIndex + ":"))
            {
                this.ExpandedStates.Remove(index);
            }
        }
    }
	
	private void ClearSelectedChildren(string parentHierarchicalIndex)
    {
        string[] indexes = new string[this.SelectedStates.Keys.Count];
        this.SelectedStates.Keys.CopyTo(indexes, 0);
        foreach (string index in indexes)
        {
            //all indexes of child items
            if (index.StartsWith(parentHierarchicalIndex + "_") ||
                index.StartsWith(parentHierarchicalIndex + ":"))
            {
                this.SelectedStates.Remove(index);
            }
        }
    }
	
    //Save/load selected states Hash from the session
    //this can also be implemented in the ViewState
    private Hashtable SelectedStates
    {
        get
        {
            if (this._selectedState == null)
            {
                _selectedState = this.Session["_selectedState"] as Hashtable;
                if (_selectedState == null)
                {
                    _selectedState = new Hashtable();
                    this.Session["_selectedState"] = _selectedState;
                }
            }

            return this._selectedState;
        }
    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        //save the expanded/selected state in the session
        if (e.CommandName == RadGrid.ExpandCollapseCommandName)
        {
            //Is the item about to be expanded or collapsed
            if (!e.Item.Expanded)
            {
                //Save its unique index among all the items in the hierarchy
                this.ExpandedStates[e.Item.ItemIndexHierarchical] = true;
            }
            else //collapsed
            {
                this.ExpandedStates.Remove(e.Item.ItemIndexHierarchical);
				this.ClearSelectedChildren(e.Item.ItemIndexHierarchical);
                this.ClearExpandedChildren(e.Item.ItemIndexHierarchical);
            }
        }
        //Is the item about to be selected 
        else if (e.CommandName == RadGrid.SelectCommandName)
        {
            //Save its unique index among all the items in the hierarchy
            this.SelectedStates[e.Item.ItemIndexHierarchical] = true;
        }
        //Is the item about to be deselected 
        else if (e.CommandName == RadGrid.DeselectCommandName)
        {
            this.SelectedStates.Remove(e.Item.ItemIndexHierarchical);
        }
    }
    protected void RadGrid1_DataBound(object sender, EventArgs e)
    {
        //Expand all items using our custom storage
        string[] indexes = new string[this.ExpandedStates.Keys.Count];
        this.ExpandedStates.Keys.CopyTo(indexes, 0);

        ArrayList arr = new ArrayList(indexes);
        //Sort so we can guarantee that a parent item is expanded before any of 
        //its children
        arr.Sort();

        foreach (string key in arr)
        {
            bool value = (bool)this.ExpandedStates[key];
            if (value)
            {
                RadGrid1.Items[key].Expanded = true;
            }
        }

        //Select all items using our custom storage
        indexes = new string[this.SelectedStates.Keys.Count];
        this.SelectedStates.Keys.CopyTo(indexes, 0);

        arr = new ArrayList(indexes);
        //Sort to ensure that a parent item is selected before any of its children
        arr.Sort();

        foreach (string key in arr)
        {
            bool value = (bool)this.SelectedStates[key];
            if (value)
            {
                RadGrid1.Items[key].Selected = true;
            }
        }
    }
    protected void grdRebind_Click(object sender, EventArgs e)
    {
        RadGrid1.Rebind();
        DoAsync();

    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (Session["Halted"] != null && (Session["Halted"] as Boolean?) == true)
            Timer1.Enabled = false;
        Label1.Text = Session["Logger"] as string;
    }

}
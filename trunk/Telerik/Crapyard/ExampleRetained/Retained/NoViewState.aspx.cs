using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Web.UI;

namespace Retained
{
    public partial class NoViewState : System.Web.UI.Page
    {
        private static CacheManager _ExpandedState = CacheFactory.GetCacheManager();
        private Hashtable _ordersExpandedState;
        private Hashtable _selectedState;
        private string _CacheIdentifier;
        private const string ORDERS_EXPANDED_STATE = "_ordersExpandedState";

        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //reset states
                this._ordersExpandedState = null;
                //this.Session[ORDERS_EXPANDED_STATE] = null;
                this._selectedState = null;
                this.Session["_selectedState"] = null;
            }
            else
            {
                if (!RadGrid1.EnableViewState)
                {
                    ////perform some actions here
                    //RadGrid1.DataSource = null;

                    ////call the Rebind() method after nullifying the data source
                    //RadGrid1.Rebind();
                }

            }
        }
        /// <summary>
        /// Uses cachemanager to retrieve/store state.
        /// </summary>
        protected Hashtable ExpandedState
        {
            get
            {
                _CacheIdentifier = "ExpandedState" + Context.Session.SessionID;
                if (_ExpandedState.Contains(_CacheIdentifier))
                {
                    _ordersExpandedState = _ExpandedState.GetData(_CacheIdentifier) as Hashtable;
                }
                else
                {
                    if (this._ordersExpandedState == null)
                    {
                        _ordersExpandedState = new Hashtable();
                        _ExpandedState.Add(_CacheIdentifier, _ordersExpandedState, CacheItemPriority.High, null, null);
                    }
                }
                return _ordersExpandedState;
            }
            set
            {
                _ExpandedState.Add(_CacheIdentifier, value, CacheItemPriority.High, null, null);
                _ordersExpandedState = value;
            }
        }
        //Save/load expanded states Hash from the session
        //this can also be implemented in the ViewState
        //private Hashtable ExpandedState
        //{
        //    get
        //    {
        //        if (this._ordersExpandedState == null)
        //        {
        //            _ordersExpandedState = ExpandedState;
        //            if (_ordersExpandedState == null)
        //            {
        //                _ordersExpandedState = new Hashtable();
        //                ExpandedState = _ordersExpandedState;
        //            }
        //        }

        //        return this._ordersExpandedState;
        //    }
        //}

        //Clear the state for all expanded children if a parent item is collapsed
        private void ClearExpandedChildren(string parentHierarchicalIndex)
        {
            string[] indexes = new string[this.ExpandedState.Keys.Count];
            this.ExpandedState.Keys.CopyTo(indexes, 0);
            foreach (string index in indexes)
            {
                //all indexes of child items
                if (index.StartsWith(parentHierarchicalIndex + "_") ||
                    index.StartsWith(parentHierarchicalIndex + ":"))
                {
                    this.ExpandedState.Remove(index);
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
                    this.ExpandedState[e.Item.ItemIndexHierarchical] = true;
                }
                else //collapsed
                {
                    this.ExpandedState.Remove(e.Item.ItemIndexHierarchical);
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
            string[] indexes = new string[this.ExpandedState.Keys.Count];
            this.ExpandedState.Keys.CopyTo(indexes, 0);

            ArrayList arr = new ArrayList(indexes);
            //Sort so we can guarantee that a parent item is expanded before any of 
            //its children
            arr.Sort();

            foreach (string key in arr)
            {
                bool value = (bool)this.ExpandedState[key];
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
        }
    }
}
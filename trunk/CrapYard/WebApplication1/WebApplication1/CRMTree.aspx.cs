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
using System.Diagnostics;
using System.Drawing;
using System.Data;
using Dictionary.BusinessObjects;

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

        public string ZoekString2
        {
            get
            {
                return Session["ZoekString2"].ToString();
            }
            set
            {
                Session["ZoekString2"] = value;
            }
        }
        protected void RadTreeView1_HandleDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            RadTreeNode sourceNode = e.SourceDragNode;
            RadTreeNode destNode = e.DestDragNode;
            RadTreeViewDropPosition dropPosition = e.DropPosition;

            if (destNode != null) //drag&drop is performed between trees
            {
                if (false) //dropped node will at the same level as a destination node
                {
                    if (sourceNode.TreeView.SelectedNodes.Count <= 1)
                    {
                        PerformDragAndDrop(dropPosition, sourceNode, destNode);
                    }
                    else if (sourceNode.TreeView.SelectedNodes.Count > 1)
                    {
                        foreach (RadTreeNode node in sourceNode.TreeView.SelectedNodes)
                        {
                            PerformDragAndDrop(dropPosition, node, destNode);
                        }
                    }
                }
                else //dropped node will be a sibling of the destination node
                {
                    if (sourceNode.TreeView.SelectedNodes.Count <= 1)
                    {
                        if (!sourceNode.IsAncestorOf(destNode))
                        {
                            sourceNode.Owner.Nodes.Remove(sourceNode);
                            destNode.Nodes.Add(sourceNode);
                        }
                    }
                    else if (sourceNode.TreeView.SelectedNodes.Count > 1)
                    {
                        foreach (RadTreeNode node in RadTreeView1.SelectedNodes)
                        {
                            if (!node.IsAncestorOf(destNode))
                            {
                                node.Owner.Nodes.Remove(node);
                                destNode.Nodes.Add(node);
                            }
                        }
                    }
                }

                destNode.Expanded = true;
                sourceNode.TreeView.UnselectAllNodes();
            }
            else if (e.HtmlElementID == RadGrid1.ClientID)
            {
                DataTable dt = (DataTable)Session["DataTable"];
                foreach (RadTreeNode node in e.DraggedNodes)
                {
                    AddRowToGrid(dt, node);
                }
            }
        }

        private void PopulateGrid()
        {
            string[] values = { "One", "Two", "Three" };

            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");
            dt.Columns.Add("Category");
            dt.Rows.Add(values);
            dt.Rows.Add(values);
            dt.Rows.Add(values);
            Session["DataTable"] = dt;

            RadGrid1.DataSource = dt;
            RadGrid1.DataBind();
        }

        private void AddRowToGrid(DataTable dt, RadTreeNode node)
        {
            string[] values = { node.Text, node.Value };
            dt.Rows.Add(values);

            RadGrid1.DataSource = dt;
            RadGrid1.DataBind();
        }

        private static void PerformDragAndDrop(RadTreeViewDropPosition dropPosition, RadTreeNode sourceNode,
                                               RadTreeNode destNode)
        {
            if (sourceNode.Equals(destNode) || sourceNode.IsAncestorOf(destNode))
            {
                return;
            }
            sourceNode.Owner.Nodes.Remove(sourceNode);

            switch (dropPosition)
            {
                case RadTreeViewDropPosition.Over:
                    // child
                    if (!sourceNode.IsAncestorOf(destNode))
                    {
                        destNode.Nodes.Add(sourceNode);
                    }
                    break;

                case RadTreeViewDropPosition.Above:
                    // sibling - above					
                    destNode.InsertBefore(sourceNode);
                    break;

                case RadTreeViewDropPosition.Below:
                    // sibling - below
                    destNode.InsertAfter(sourceNode);
                    break;
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
            
            string xml = new ShareHolders().CreateXMLOrganoTreeView(ZoekString, RelationView.Overview);
            RadTreeView1.LoadXml(xml);
            RadTreeView2.LoadXml(xml);

            var nodes = RadTreeView1.GetAllNodes();
            var nodes2 = RadTreeView2.GetAllNodes();

            if (nodes[0].Text.Equals(ZoekString))
                nodes[0].BackColor = Color.Gold;

            foreach (var node in nodes)
            {
                if(!string.IsNullOrEmpty( node.Value))
                    Debug.Assert (true);
            }
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
        protected void RadButton1_Click(object sender, EventArgs e)
        {
            ZoekString = RadTextBox2.Text;
            string xml = new ShareHolders().CreateXMLOrganoTreeView(ZoekString, RelationView.Overview);
            RadTreeView1.LoadXml(xml);
            var nodes = RadTreeView1.GetAllNodes();
            if (nodes.Count() > 0 && nodes[0].Text.Equals(ZoekString))
                nodes[0].BackColor = Color.Gold;
        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            ZoekString2 = RadTextBox3.Text;
            string xml2 = new ShareHolders().CreateXMLOrganoTreeView(ZoekString2, RelationView.Overview);
            RadTreeView2.LoadXml(xml2);
            var nodes2 = RadTreeView2.GetAllNodes();
            if (nodes2.Count() > 0 && nodes2[0].Text.Equals(ZoekString2))
                nodes2[0].BackColor = Color.Gold;
        }
    }
}
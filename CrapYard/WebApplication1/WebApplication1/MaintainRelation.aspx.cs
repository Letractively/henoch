using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.QuickStart;
using System.Data;
using Telerik.Web.UI;

namespace WebApplication1
{
    public partial class MaintainRelation : System.Web.UI.Page
    {
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

        }
    }
}
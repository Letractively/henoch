using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.QuickStart;
using System.Data;
using Telerik.Web.UI;
using Repository;
using System.Drawing;

namespace WebApplication1
{
    public partial class MaintainRelation : Page
    {
        static bool _IsUpdated;
        public string ZoekString
        {
            get
            {
                return (string.IsNullOrEmpty(Session["ZoekString"] as string) ? string.Empty : Session["ZoekString"].ToString());
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
                return (string.IsNullOrEmpty(Session["ZoekString2"] as string) ? string.Empty : Session["ZoekString2"].ToString());
            }
            set
            {
                Session["ZoekString2"] = value;
            }
        }
        public string XMLTreeView1
        {
            get
            {
                return Session["XMLTreeView1"].ToString();
            }
            set
            {
                Session["XMLTreeView1"] = value;
            }
        }
        public string XMLTreeView2
        {
            get
            {
                return Session["XMLTreeView2"].ToString();
            }
            set
            {
                Session["XMLTreeView2"] = value;
            }
        }
        protected void RadTreeView2_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            RadTreeView1_NodeDrop(sender, e);
        }
        protected void RadTreeView1_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            RadTreeNode sourceNode = e.SourceDragNode;
            RadTreeNode destNode = e.DestDragNode;
            RadTreeViewDropPosition dropPosition = e.DropPosition;

            if (destNode != null) //drag&drop is performed between trees
            {
                bool betweenNodes = true;
                if (betweenNodes) //dropped node will at the same level as a destination node
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

        }

        private void AddRowToGrid(DataTable dt, RadTreeNode node)
        {
            string[] values = { node.Text, node.Value };
            dt.Rows.Add(values);

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

                    UpdateRelation(destNode, sourceNode);

                    break;

                case RadTreeViewDropPosition.Above:
                    // sibling - above					
                    destNode.InsertBefore(sourceNode);

                    var parentNode = destNode.ParentNode;
                    UpdateRelation(parentNode, sourceNode);
                    
                    break;

                case RadTreeViewDropPosition.Below:
                    // sibling - below
                    destNode.InsertAfter(sourceNode);

                    UpdateRelation(destNode, sourceNode);
                    break;
            }
        }

        private static void UpdateRelation( RadTreeNode parentNode, RadTreeNode childNode)
        {
            string parent = parentNode.Text;
            string child = childNode.Text;

            var shareHolders = new ShareHolders();

            var list0 = new ShareHolders().Companies[parent];
            var newList = list0;
            if (newList == null)
            {
                newList = new List<string>();
            }

            newList.Add(childNode.Text);

            shareHolders.Companies.TryUpdate(parent,newList,list0);
            _IsUpdated = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {




            }
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            if (IsPostBack && _IsUpdated)
            {
                ///update treeviews
                BuildTreeView1();
                BuildTreeView2();
                _IsUpdated = false;
            }
        }
        protected void RadTreeView1_Load(object sender, EventArgs e)
        {
            RadTreeView treeView = (RadTreeView)sender;
            if (IsPostBack && !string.IsNullOrEmpty(ZoekString))
            { 
                //XMLTreeView1 = new ShareHolders().CreateXMLOrganoTreeView(ZoekString);
                //treeView.LoadXml(XMLTreeView1);
                //var nodes = RadTreeView1.GetAllNodes();
                //if (nodes.Count() > 0 && nodes[0].Text.Equals(ZoekString))
                //    nodes[0].BackColor = Color.Gold;
            }
        }

        protected void RadTreeView2_Load(object sender, EventArgs e)
        {
            RadTreeView treeView = (RadTreeView)sender;
            if (IsPostBack && !string.IsNullOrEmpty(ZoekString2))
            {
                XMLTreeView2 = new ShareHolders().CreateXMLOrganoTreeView("Stern Beheer B.V.");
                //treeView.LoadXml(XMLTreeView2);
                //var nodes2 = RadTreeView2.GetAllNodes();
                //if (nodes2.Count() > 0 && nodes2[0].Text.Equals("Stern Beheer B.V."))
                //    nodes2[0].BackColor = Color.Gold;
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            BuildTreeView1();
        }

        private void BuildTreeView1()
        {
            ZoekString = RadTextBox1.Text;
            XMLTreeView1 = new ShareHolders().CreateXMLOrganoTreeView(ZoekString);
            RadTreeView1.LoadXml(XMLTreeView1);
            var nodes = RadTreeView1.GetAllNodes();
            if (nodes.Count() > 0 && nodes[0].Text.Equals(ZoekString))
                nodes[0].BackColor = Color.Gold;
        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            BuildTreeView2();
        }

        private void BuildTreeView2()
        {
            ZoekString2 = RadTextBox2.Text;
            XMLTreeView2 = new ShareHolders().CreateXMLOrganoTreeView(ZoekString2);
            RadTreeView2.LoadXml(XMLTreeView2);
            var nodes2 = RadTreeView2.GetAllNodes();
            if (nodes2.Count() > 0 && nodes2[0].Text.Equals(ZoekString2))
                nodes2[0].BackColor = Color.Gold;
        }

        protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var treeViewTarget = RadTreeView2;
            var treeViewSource = (RadTreeView)sender;

            DisableNodesTarget(treeViewTarget, treeViewSource);

        }

        protected void RadTreeView2_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var treeViewTarget = RadTreeView1;
            var treeViewSource = (RadTreeView)sender;

            DisableNodesTarget(treeViewTarget, treeViewSource);
        }

        private static void DisableNodesTarget(RadTreeView treeViewTarget, RadTreeView sourceTreeView)
        {
            var nodeTreeView = sourceTreeView.SelectedNode;
            var nodesTarget = treeViewTarget.GetAllNodes().Where(n => n.Text.Equals(nodeTreeView.Text));
            var nodesDisabled = treeViewTarget.GetAllNodes().Where(n => n.Enabled == false);

            if (nodesDisabled.Count() > 0)
            {
                foreach (var node in nodesDisabled)
                {
                    node.Enabled = true;
                }

            }
            if (nodesTarget.Count() > 0)
            {
                foreach (var node in nodesTarget)
                {
                    node.Enabled = false;
                }

            }
        }


    }
}
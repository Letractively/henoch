﻿using System;
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
using System.Text.RegularExpressions;
using Dictionary.BusinessObjects;

namespace WebApplication1
{
    public partial class MaintainRelation : PageBase
    {

        #region Treeview
        protected void RadTreeView2_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            RadTreeView1_NodeDrop(sender, e);
        }
        protected void RadTreeView1_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            NodeDrop(sender, e);
        }

        private new static void PerformDragAndDrop(RadTreeViewDropPosition dropPosition, RadTreeNode sourceNode,
                                               RadTreeNode destNode)
        {
            PerformDragAndDrop(dropPosition, sourceNode, destNode);
        }

        #endregion Treeview

        #region ContextMenu
        protected const string unreadPattern = @"\(\d+\)";
protected void RadTreeView1_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            RadTreeNode clickedNode = e.Node;

            switch (e.MenuItem.Value)
            {
                case "NewRelation":
                    //RadTreeNode newRelation = new RadTreeNode(string.Format("Nieuwe Relatie {0}", clickedNode.Nodes.Count + 1));
                    //newRelation.Selected = true;
                    //newRelation.ImageUrl = clickedNode.ImageUrl;
                    //clickedNode.Nodes.Add(newRelation);
                    //clickedNode.Expanded = true;
                    ////update the number in the brackets
                    ////if (Regex.IsMatch(clickedNode.Text, unreadPattern))
                    ////    clickedNode.Text = Regex.Replace(clickedNode.Text, unreadPattern, "(" + clickedNode.Nodes.Count.ToString() + ")");
                    ////else
                    ////    clickedNode.Text += string.Format(" ({0})", clickedNode.Nodes.Count);
                    ////clickedNode.Font.Bold = true;
                    ////set node's value so we can find it in startNodeInEditMode
                    //newRelation.Value = newRelation.GetFullPath("/");
                    //startNodeInEditMode(newRelation.Value);
                    break;
                case "Verwijder":

                    if (clickedNode.ParentNode != null)
                    {
                        new ShareHolders().RemoveSubsidiary(clickedNode.ParentNode.Text, clickedNode.Text);
                        clickedNode.Remove();
                        IsUpdated = true;
                    }
                    break;
            }
        }

        private void startNodeInEditMode(string nodeValue)
        {
            //find the node by its Value and edit it when page loads
            string js = "Sys.Application.add_load(editNode); function editNode(){ ";
            js += "var tree = $find(\"" + RadTreeView1.ClientID + "\");";
            js += "var node = tree.findNodeByValue('" + nodeValue + "');";
            js += "if (node) node.startEdit();";
            js += "Sys.Application.remove_load(editNode);};";

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit", js, true);
        }

        protected void RadTreeView1_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
        {
            e.Node.Text = e.Text;
        }
        //this method is used by Mark All as Read and Empty this folder 
        protected void emptyFolder(RadTreeNode node, bool removeChildNodes)
        {
            node.Font.Bold = false;
            node.Text = Regex.Replace(node.Text, unreadPattern, "");

            if (removeChildNodes)
            {
                //Empty this folder is clicked
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                {
                    node.Nodes.RemoveAt(i);
                }
            }
            else
            {
                //Mark all as read is clicked
                foreach (RadTreeNode child in node.Nodes)
                {
                    emptyFolder(child, removeChildNodes);
                }
            }
        }    

        #endregion ContextMenu
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RadComboBox1.Filter = RadComboBoxFilter.Contains;
            RadComboBox2.Filter = RadComboBoxFilter.Contains;
            if (!IsPostBack)
            {

                ////set the expire timeout for the session 
                //Session.Timeout = 2;
                ////configure the notification to automatically show 1 min before session expiration
                //RadNotification1.ShowInterval = (Session.Timeout - 1) * 1000;
                ////set the redirect url as a value for an easier and faster extraction in on the client
                ////RadNotification1.Value = Page.ResolveClientUrl("Notification.aspx");
            }
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            if (IsPostBack && IsUpdated.HasValue && IsUpdated.Value)
            {
                ///update treeviews
                BuildTreeView1();
                BuildTreeView2();
                IsUpdated = false;
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
                //XMLTreeView2 = new ShareHolders().CreateXMLOrganoTreeView("Stern Beheer B.V.");
                //treeView.LoadXml(XMLTreeView2);
                //var nodes2 = RadTreeView2.GetAllNodes();
                //if (nodes2.Count() > 0 && nodes2[0].Text.Equals("Stern Beheer B.V."))
                //    nodes2[0].BackColor = Color.Gold;
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            try
            {
                BuildTreeView1();
                //throw new ApplicationException();
            }
            catch (Exception ex)
            {
                ////set the expire timeout for the session 
                //Session.Timeout = 2;
                ////configure the notification to automatically show 1 min before session expiration
                //RadNotification1.ShowInterval = (Session.Timeout - 1) * 1000;
                ////set the redirect url as a value for an easier and faster extraction in on the client
                //RadNotification1.Value = Page.ResolveClientUrl("Notification.aspx");
            }
        }

        private void BuildTreeView1()
        {
            ZoekString = RadComboBox1.SelectedValue;
            if(!btnToggle.Checked)
               BuildTreeView(ZoekString,RadTreeView1, RelationView.Overview);
            else
               BuildTreeView(ZoekString, RadTreeView1, RelationView.Dependencies);

        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            try
            {
                BuildTreeView2();
                throw new ApplicationException();
            }
            catch (Exception ex)
            {
                RadNotification2.Text = ex.Message;
                RadNotification2.VisibleOnPageLoad = true;
                RadNotification2.ShowInterval = 1000;
                ////set the expire timeout for the session 
                //Session.Timeout = 2;
                ////configure the notification to automatically show 1 min before session expiration
                //RadNotification1.ShowInterval = (Session.Timeout - 1) * 1000;
                ////set the redirect url as a value for an easier and faster extraction in on the client
                //RadNotification1.Value = Page.ResolveClientUrl("Notification.aspx");
            }
        }

        private void BuildTreeView2()
        {
            ZoekString2 = RadComboBox2.SelectedValue;
            BuildTreeView(ZoekString2, RadTreeView2, RelationView.Overview);
        }

        protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var treeViewTarget = RadTreeView2;
            var treeViewSource = (RadTreeView)sender;

            //DisableNodesTarget(treeViewTarget, treeViewSource);

        }

        protected void RadTreeView2_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var treeViewTarget = RadTreeView1;
            var treeViewSource = (RadTreeView)sender;

            //DisableNodesTarget(treeViewTarget, treeViewSource);
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

        protected void OnCallbackUpdate(object sender, RadNotificationEventArgs e)
        {
            e.Value = "hello";
        }


    }
}
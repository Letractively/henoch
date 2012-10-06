using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using Dictionary.BusinessObjects;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication1
{
    public class PageBase : Page
    {
        #region Session properties
        public bool? IsUpdated
        {
            get
            {
                return (Session["IsUpdated"] as bool? == null) ? false : Session["IsUpdated"] as bool?;
            }
            set
            {
                Session["IsUpdated"] = value;
            }
        }
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
        #endregion Seeion properties
        protected void NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            RadTreeNode sourceNode = e.SourceDragNode;
            RadTreeNode destNode = e.DestDragNode;
            RadTreeViewDropPosition dropPosition = e.DropPosition;
            if (!ShareHolders.ValidateAPriori(sourceNode.Text, destNode.Text))
            {
                return;
            }

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
                    throw new NotImplementedException();
                }
                destNode.Expanded = true;
                sourceNode.TreeView.UnselectAllNodes();
            }
        }

        public void PerformDragAndDrop(RadTreeViewDropPosition dropPosition, RadTreeNode sourceNode,
                                               RadTreeNode destNode)
        {
            if (sourceNode.Equals(destNode) || sourceNode.IsAncestorOf(destNode))
            {
                return;
            }
            //Do not remove relation here.
            //sourceNode.Owner.Nodes.Remove(sourceNode);

            var shareHolders = new ShareHolders();
            bool validation = false;
            switch (dropPosition)
            {
                case RadTreeViewDropPosition.Over:

                    validation = shareHolders.AddSubsidiary(destNode.Text, sourceNode.Text);
                    // child
                    if (!sourceNode.IsAncestorOf(destNode) && validation == true)
                    {
                        destNode.Nodes.Add(sourceNode);
                    }

                    IsUpdated = validation;
                    break;

                case RadTreeViewDropPosition.Above:

                    validation = shareHolders.AddSubsidiary(destNode.ParentNode.Text, sourceNode.Text);
                    // sibling - above	
                    if (validation == true)
                    {
                        destNode.InsertBefore(sourceNode);
                    }
                    IsUpdated = validation;
                    break;

                case RadTreeViewDropPosition.Below:

                    validation = shareHolders.AddSubsidiary(destNode.Text, sourceNode.Text);
                    // sibling - below
                    if (validation == true)
                    {
                        destNode.InsertAfter(sourceNode);
                    }

                    IsUpdated = validation;
                    break;
            }
        }

        protected void BuildTreeView(string zoekString, RadTreeView treeView, RelationView view)
        {
            string xml = new ShareHolders().CreateXMLOrganoTreeView(zoekString, view);
            XElement coloredXML = XElement.Parse(xml);
            ColorFoundNodes(zoekString, ref coloredXML);

            treeView.LoadXml(coloredXML.ToString());

            var nodes = treeView.GetAllNodes();

            if (nodes.Count() > 0 && nodes[0].Text.Equals(zoekString))
                nodes[0].BackColor = Color.Gold;

        }
        protected void ItemDataBound(object sender, GridItemEventArgs e)
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
        /// <summary>
        /// Colors the found nodes
        /// </summary>
        /// <param name="found"></param>
        /// <param name="xTree"></param>
        public static void ColorFoundNodes(string found, ref XElement xTree)
        {
            var foundList = (xTree.Descendants().Where(d => d.Attribute("Text").Value == found)).ToList();

            for (int i = 0; i < foundList.Count; i++)
            {
                XElement newNode = new XElement("Node",
                        new XAttribute("Text", found),
                        new XAttribute("CssClass", foundList[i].Attribute("CssClass").Value),
                        new XAttribute("Expanded", foundList[i].Attribute("Expanded").Value),
                        new XAttribute("BackColor", "Gold"));
                newNode.Add(foundList[i].Elements());
                foundList[i].ReplaceWith(newNode);
            }

            if (found.Equals(ShareHolders.VirtualRoot))
            {
                //collapse its children, if the root is equals to virtualroot
                var children = xTree.Elements().ToList();
                foreach (var child in children)
                {
                    child.Attribute("Expanded").Value = "False";
                }
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Repository;
using Telerik.Web.UI;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using Dictionary.BusinessObjects;

namespace WebApplication1
{
    public partial class CRMTree : PageBase
    {
        static bool _IsUpdated;

        protected void RadTreeView1_HandleDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            NodeDrop(sender, e);
        }

        protected new static void PerformDragAndDrop(RadTreeViewDropPosition dropPosition, RadTreeNode sourceNode,
                                               RadTreeNode destNode)
        {
            new PageBase().PerformDragAndDrop(dropPosition, sourceNode, destNode);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ZoekString = RadTextBox1.Text;
            if (IsPostBack)             
            {

                
            }
         }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            if (IsPostBack && _IsUpdated)
            {
                ///update treeviews
                BuildTreeView(ZoekString, RadTreeView1, RelationView.Overview);
            }
        }
        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid control = (RadGrid)sender;

            ZoekString = control.SelectedValue.ToString();

            _IsUpdated = true;
        }

        private void BuildTreeView2()
        {
            ZoekString2 = RadTextBox3.Text;
            BuildTreeView(ZoekString2, RadTreeView2, RelationView.Overview);
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            ItemDataBound(sender, e);
        }
        protected void RadButton1_Click(object sender, EventArgs e)
        {
            ZoekString = RadTextBox2.Text;
            _IsUpdated = true;
            BuildTreeView(ZoekString, RadTreeView1, RelationView.Overview);
        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            ZoekString2 = RadTextBox3.Text;
            _IsUpdated = true;
            BuildTreeView(ZoekString2, RadTreeView2, RelationView.Overview);
        }
    }
}
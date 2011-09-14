using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.Web.UI.WebControls;

namespace MetaData.Beheer.Views
{

    //[System.ComponentModel.DefaultBindingProperty("Details")]
    public partial class Trefwoorden : UserControl, IBusinessEntityView
    {
        private TrefwoordenPresenter m_Presenter;
        private BeheerContextEntity m_Master;
        private IList<BeheerContextEntity> m_Details;
        
        [CreateNew]
        public TrefwoordenPresenter Presenter
        {
            get { return m_Presenter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                m_Presenter = value;
                m_Presenter.View = this;
            }
        }

        #region ITrefwoordenView Members


        public IList<BeheerContextEntity> BusinessEntities
        {
            set { TrefwoordTableDataSource.DataSource = value; }
        }

        public IList<BeheerContextEntity> DetailsEntities
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsMasterView
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsInsertingInline
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Master
        {
            set
            {
                m_Master = value;
                if (m_Master != null) m_Details = m_Master.Details;
                TrefwoordTableDataSource.DataSource = m_Details;
            }
        }

        public bool IsSortable
        {
            set { GridView1.AllowSorting = value; }
        }

        public bool AllowCrud { set; private get; }

        public BeheerContextEntity Selected { set; private get; }
        public bool ShowFooter
        {
            set { throw new NotImplementedException(); }
        }

        public void ShowErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region LifeCycle page
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Trace.Write("Page_PreInit-------------------", (m_Details == null).ToString());
            if (m_Details != null)
            {
                Trace.Write("Page_PreInit-------------------", (m_Details[0].DataKeyValue));
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            Trace.Write("Page_Init-------------------", (m_Details == null).ToString());
            if (m_Details != null)
            {
                Trace.Write("Page_Init-------------------", (m_Details[0].DataKeyValue));
                m_Presenter.OnViewLoaded();
            }
        }

        protected void Page_InitComplete(object sender, EventArgs e)
        {
            if (m_Details != null)
            {
                Trace.Write("Page_InitComplete-------------------", (m_Details[0].DataKeyValue));
                m_Presenter.OnViewLoaded();
            }
        }
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            Trace.Write("Page_PreLoad-------------------", (m_Details == null).ToString());
            if (m_Details != null)
            {
                Trace.Write("Page_PreLoad-------------------", (m_Details[0].DataKeyValue));
                m_Presenter.OnViewLoaded();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("Trefwoord_Page_load-------------------", (m_Details ==null).ToString());
            if (!IsPostBack)
            {
                m_Presenter.OnViewInitialized();
                
            }
            if (m_Details != null)
            {
                Trace.Write("Trefwoord_Page_load-------------------", (m_Details[0].DataKeyValue));
                
            }


            GridView1.RowDataBound += GridView1_RowDataBound;
            m_Presenter.OnViewLoaded();
            

        }
        //protected void Page_PreRenderComplete(object sender, EventArgs e)
        //{
        //    ShowCrudButtons();
        //}
        protected void ShowCrudButtons()
        {
            if (AllowCrud)
            {
                int rowCount = GridView1.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    var row = GridView1.Rows[i];
                    var selectButton = row.FindControl("BtnSelect") as ImageButton;
                    var editButton = row.FindControl("BtnEdit") as ImageButton;
                    var deleteButton = row.FindControl("BtnDelete") as ImageButton;
                    var editBlur = row.FindControl("EditBlur") as Image;
                    var deleteBlur = row.FindControl("DeleteBlur") as Image;

                    if (selectButton != null) selectButton.Visible = AllowCrud;
                    if (editButton != null) editButton.Visible = AllowCrud;
                    if (deleteButton != null) deleteButton.Visible = AllowCrud;

                    if (editBlur != null) editBlur.Visible = !AllowCrud;
                    if (deleteBlur != null) deleteBlur.Visible = !AllowCrud;
                }
            }
        }
        public ImageButton InsertButton
        {
            get
            {
                return BtnInsertTrefwoord;
            }

            set
            {
                BtnInsertTrefwoord = value;
            }
        
        }
        private void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            Trace.Write("GridView2_RowDataBound", "--> Begin Current row: " + row.RowIndex);

            if (row.RowType == DataControlRowType.DataRow)
            {
                var editButton = row.FindControl("BtnEdit") as ImageButton;
                var deleteButton = row.FindControl("BtnDelete") as ImageButton;
                var editBlur = row.FindControl("EditBlur") as Image;
                var deleteBlur = row.FindControl("DeleteBlur") as Image;

                if (editButton != null) editButton.Visible = AllowCrud;
                if (deleteButton != null) deleteButton.Visible = AllowCrud;
                if (editBlur != null) editBlur.Visible = !AllowCrud;
                if (deleteBlur != null) deleteBlur.Visible = !AllowCrud;
            }
            Trace.Write("GridView1_RowDataBound", "--> End Current row..................");
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //		ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.wcsf.2007oct/wcsf/html/08da6294-8a4e-46b2-8bbe-ec94c06f1c30.html

        protected void TrefwoordTableDataSource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            BeheerContextEntity trefwoord = new BeheerContextEntity();
            trefwoord = e.Instance as BeheerContextEntity;
            if (trefwoord != null)
            {
                trefwoord.Tablename = "trefwoord";
                trefwoord.DataKeyName = "trefwoord";
                m_Presenter.OnBusinessEntityAdded(trefwoord);
            }
            AllowCrud = true;
        }

        protected void TrefwoordTableDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            m_Presenter.OnBusinessEntityUpdated(e.Instance as BeheerContextEntity);
            AllowCrud = true;
        }

        protected void TrefwoordTableDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            m_Presenter.OnBusinessEntityDeleted(e.Instance as BeheerContextEntity);
            AllowCrud = true;
        }

        protected void BtnInsertTrefwoord_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DetailsView2.Visible = true;
            BtnInsertTrefwoord.Visible = false;
        }
        protected void InsertImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DetailsView2.Visible = false;
            BtnInsertTrefwoord.Visible = true;
        }
        protected void CancelImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DetailsView2.Visible = false;
            BtnInsertTrefwoord.Visible = true;
        }
        //protected void BtnSortDown_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    string buttonName = "BtnSortUp";
        //    bool isVisible = true;

        //    SetVisibilitySortButtons(sender, isVisible, buttonName);
        //}
        //protected void BtnsortUp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    string buttonName = "BtnSortDown";
        //    bool isVisible = true;

        //    SetVisibilitySortButtons(sender, isVisible, buttonName);
        //}
        //private void SetVisibilitySortButtons(object sender, bool isVisible, string buttonName)
        //{
        //    ImageButton buttonPressed = sender as ImageButton;
        //    buttonPressed.Visible = !isVisible;

        //    GridViewRow row = GridView1.HeaderRow;
        //    ImageButton buttonUp = row.FindControl(buttonName) as ImageButton;
            
        //    buttonUp.Visible = isVisible;
        //}

        protected void CancelImage_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AllowCrud = true;
        }
        protected void GridView1_Sorted(object sender, EventArgs e)
        {
            AllowCrud = true;
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            AllowCrud = false;
        }
}
}
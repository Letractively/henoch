using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Views;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.Practices.Web.UI.WebControls.Utility;
using Page=Microsoft.Practices.CompositeWeb.Web.UI.Page;
using System.Globalization;

namespace MetaData.Shared
{
    public partial class Categorieen : Page,
                                       IBusinessEntityView
    {
        private IList<BeheerContextEntity> m_ListBeheerEntities;
        private CategorieenPresenter m_Presenter;
        private IList<BeheerContextEntity> m_ListDetailsEntities;
        private bool m_DeleteAlleenTrefwoord;
        private bool m_InsertAlleenTrefwoord;
        private string vorigeCategorie;

        [CreateNew]
        public CategorieenPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //		ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.wcsf.2007oct/wcsf/html/08da6294-8a4e-46b2-8bbe-ec94c06f1c30.html

        #region IBusinessEntityView Members

        public IList<BeheerContextEntity> BusinessEntities
        {
            set
            {
                m_ListBeheerEntities = value;
                CategorieTableDataSource.DataSource = value;//master
            }
        }

        public IList<BeheerContextEntity> DetailsEntities
        {
            set
            {
                m_ListDetailsEntities = value;
                TrefwoordenTableDatasource.DataSource = value;//details
            }
        }

        public bool IsMasterView { set; private get; }
        public bool IsInsertingInline
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Master
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsSortable
        {
            set { GridView2.AllowSorting = value; }
        }
        public bool AllowCrud { set; private get; }

        public BeheerContextEntity Selected { set; get; }
        public bool InsertButtonIsVisible { set; private get; }
        public bool ShowFooter
        {
            set
            {
                GridView3.ShowFooter = value;
            }
        }

        public void ShowErrorMessage(string errorMessage)
        {

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_Presenter.OnViewInitialized();
                
            }

            GridView2.RowDataBound += OnRowDataBoundMasterView;
            GridView3.RowDataBound += OnRowDataBoundDetailsView;
            m_Presenter.OnViewLoaded();
            if (m_Presenter.Showfooter != null && m_Presenter.Showfooter.Value!=null) 
                GridView3.ShowFooter = m_Presenter.Showfooter.Value;
        }

        private void OnRowDataBoundDetailsView(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            Trace.Write("OnRowDataBoundDetailsView", "--> Begin Current row: " + row.RowIndex);

            if (row.RowType == DataControlRowType.DataRow)
            {
                string lblCategorie = "LblCategorienaam";
                var lblCategorieFound = ((Label)row.FindControl(lblCategorie));

                string lblTrefwoord = "LblTrefwoord";
                var lblTrefwoordFound = ((Label) row.FindControl(lblTrefwoord));
                var editMaster = row.FindControl("BtnEdit") as ImageButton;
                var deleteMaster = row.FindControl("BtnDelete") as ImageButton;

                string categorie = "-------";
                if (lblCategorieFound!=null) categorie = lblCategorieFound.Text;

                if (!string.IsNullOrEmpty(vorigeCategorie) && vorigeCategorie.Equals(categorie))
                {
                    lblCategorieFound.Visible = false;

                    if (editMaster != null) editMaster.Visible = false;
                    if (deleteMaster != null) deleteMaster.Visible = false;
                }
                else
                {

                    if (editMaster != null) editMaster.Visible = true;
                    if (deleteMaster != null) deleteMaster.Visible = true;
                }
                vorigeCategorie = categorie;

                //itemtemplate-mode
                SetVisibilityCrudDetail(lblTrefwoordFound, row);

                string tbTrefwoord = "TrefwoordTextBox";
                var textControlFound = ((TextBox)row.FindControl(tbTrefwoord));
                //itemtemplate-mode
                SetVisibilityCrudDetail(textControlFound, row);
            }
            if (row.RowType==DataControlRowType.Footer)
            {
                var validator = row.FindControl("RfvEditCategorieFooter") as RequiredFieldValidator;
                validator.ErrorMessage = m_Presenter.ErrorMessage.Value;
                if (m_Presenter.ErrorMessage.Value!=null)
                {
                    var categorie = row.FindControl("tbCategorieFooter") as TextBox;
                    if (m_Presenter.DuplicateMaster != null)
                    {
                        //categorie.Text = m_Presenter.DuplicateMaster.Value.DataKeyValue;
                        categorie.Text = "";
                        validator.Validate();                      
                    }
                    m_Presenter.OnViewShowfooter(true);
                    m_Presenter.ErrorMessage = null;
                    //validator.ErrorMessage = "<b>Categorie is een verplicht veld</b>";
                }
                else
                    validator.ErrorMessage = "<b>Categorie is een verplicht veld</b>";
            }
            Trace.Write("OnRowDataBoundDetailsView", "--> End Current row..................");
        }

        private void SetVisibilityCrudDetail(TextBox controlFound, GridViewRow row)
        {
            if (controlFound != null)
            {                
                var update = row.FindControl("BtnUpdateDetail") as Image;
                var cancel = row.FindControl("BtnCancelDetail") as Image;
                var invisibleItem0 = row.FindControl("InvisibleSave") as Image;
                var invisibleItem1 = row.FindControl("InvisibleCancel") as Image;
                var invisibleItem2 = row.FindControl("InvisibleTextBoxEdit") as Image;

                var detailNaam = controlFound.Text;

                BeheerContextEntity detail = m_ListDetailsEntities.Where(
                    det => det.DataKeyValue == detailNaam).FirstOrDefault();

                if (detail.Id == -1 && !m_InsertAlleenTrefwoord)
                {
                    if (m_InsertAlleenTrefwoord)
                    {
                        
                    }
                    controlFound.Visible = false;
                    if (update != null) update.Visible = controlFound.Visible;
                    if (cancel != null) cancel.Visible = controlFound.Visible;
                    
                    if (invisibleItem0 != null) invisibleItem0.Visible = !controlFound.Visible;
                    if (invisibleItem1 != null) invisibleItem1.Visible = !controlFound.Visible;
                    if (invisibleItem2 != null) invisibleItem2.Visible = !controlFound.Visible;
                }
            }
        }

        private void SetVisibilityCrudDetail(Label controlFound, GridViewRow row)
        {

            if (controlFound != null)
            {
                var editMaster = row.FindControl("BtnEdit") as ImageButton;
                var deleteMaster = row.FindControl("BtnDelete") as ImageButton;
                var editDetail = row.FindControl("BtnEditDetail") as Image;
                var deleteDetail = row.FindControl("BtnDeleteDetail") as Image;
                var invisibleItemEdit = row.FindControl("InvisibleItemEdit") as Image;
                var invisibleItemDelete = row.FindControl("InvisibleItemDelete") as Image;

                var detailNaam = controlFound.Text;


                BeheerContextEntity detail = m_ListDetailsEntities.Where(
                    det => det.DataKeyValue == detailNaam).FirstOrDefault();

                if (detail.Id == -1)
                {
                    //if (editMaster != null) editMaster.Visible = false;
                    //if (deleteMaster != null) deleteMaster.Visible = false;

                    var insert = row.FindControl("BtnInsertTrefwoord") as ImageButton;
                    if (insert != null) insert.Visible = true;
                    if (editDetail != null) editDetail.Visible = false;
                    if (deleteDetail != null) deleteDetail.Visible = false;
                    if (invisibleItemEdit != null) invisibleItemEdit.Visible = false;
                    
                    //if (invisibleItemDelete != null) invisibleItemDelete.Visible = true;
                }
                if (controlFound.Text.Equals("leeg"))
                {
                    var insert = row.FindControl("BtnInsertTrefwoord") as Image;
                    if (insert != null) insert.Visible = false;
                    if (editMaster != null) editMaster.Visible = false;
                    if (deleteMaster != null) deleteMaster.Visible = false;
                    if (editDetail != null) editDetail.Visible = false;
                    if (deleteDetail != null) deleteDetail.Visible = false;
                    if (invisibleItemEdit != null) invisibleItemEdit.Visible = true;
                    if (invisibleItemDelete != null) invisibleItemDelete.Visible = true;
                }
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (GridView2.Visible) GridView2.SelectedIndex = GetSelectedIndex();
        }
        private int GetSelectedIndex()
        {
            int selIndex = -1;
            if(Selected!=null)
            {
                int rowCount = GridView2.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    var row = GridView2.Rows[i];
                    var lblNaam = row.FindControl("LblCategorienaam") as Label;
                    if (lblNaam != null)
                        if (lblNaam.Text.Equals(Selected.DataKeyValue))
                        {
                            selIndex = i;
                            break;
                        }

                }
            }
            return selIndex;
        }

        private void OnRowDataBoundMasterView(object sender, GridViewRowEventArgs e)
        {
            
            GridViewRow row = e.Row;
            Trace.Write("OnRowDataBoundMasterView", "--> Begin Current row: " + row.RowIndex);

            if (row.RowType == DataControlRowType.DataRow)
            {
                SetDetailsView(row);
                var editButton = row.FindControl("BtnEdit") as ImageButton;
                var deleteButton = row.FindControl("BtnDelete") as ImageButton;
                var editBlur = row.FindControl("EditBlur") as Image;
                var deleteBlur = row.FindControl("DeleteBlur") as Image;

                if (editButton != null) editButton.Visible = AllowCrud;
                if (deleteButton != null) deleteButton.Visible = AllowCrud;
                if (editBlur != null) editBlur.Visible = !AllowCrud;
                if (deleteBlur != null) deleteBlur.Visible = !AllowCrud;
            }

            Trace.Write("OnRowDataBoundMasterView", "--> End Current row..................");
        }
        private void SetDetailsView(GridViewRow row)
        {
            var categorieNaam = GetCategorieNaam(row);
            if (categorieNaam != null)
            {
                var query =
                    m_ListBeheerEntities.Where(beheerObject => beheerObject.DataKeyValue == categorieNaam.Text);
                var categorie = query.FirstOrDefault();
                var detailsViewer = row.FindControl("Trefwoorden1") as Trefwoorden;
                if (detailsViewer != null)
                {
                    detailsViewer.Master = categorie;
                    Trace.Write("OnRowDataBoundMasterView", categorie.DataKeyValue);
                }
            }
        }
        /// <summary>
        /// Categorienaam is een AK.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Label GetCategorieNaam(GridViewRow row)
        {
            var grid = row.FindControl("GridTrefwoord") as GridView;
            var insertDetail = row.FindControl("TrefwoordTableDataSource") as ObjectContainerDataSource;
            if (insertDetail != null) insertDetail.DataSource = m_ListBeheerEntities[row.RowIndex].Details;

            return row.FindControl("LblCategorienaam") as Label;
        }



        protected void CategorieTableDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            m_Presenter.OnBusinessEntityUpdated(e.Instance as BeheerContextEntity);
            
        }

        protected void CategorieTableDataSource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            BeheerContextEntity categorie = new BeheerContextEntity();
            categorie = e.Instance as BeheerContextEntity;
            if (categorie != null)
            {
                categorie.Tablename = "categorie";
                categorie.DataKeyName = "categorienaam";
                m_Presenter.OnBusinessEntityAdded(categorie);
            }
            m_Presenter.OnViewLoaded();
        }

        protected void CategorieTableDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            m_Presenter.OnBusinessEntityDeleted(e.Instance as BeheerContextEntity);
            
        }


        protected void btnShowTrefwoorden_Click(object sender, ImageClickEventArgs e)
        {
        }



        protected void Select(object sender, CommandEventArgs e)
        {
            //m_Presenter.OnSelectedEntity()
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var grid = (GridView)sender;
            Label categorie = grid.Rows[grid.SelectedIndex].FindControl("LblCategorienaam") as Label;
            Selected = m_ListBeheerEntities.Where(listelt => listelt.DataKeyValue.Equals(categorie.Text)).FirstOrDefault();
            Selected.SelectedIndex = grid.SelectedIndex;

            HandleCrudOfDetailsViewer(grid);
        }
        protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var grid = (GridView)sender;
            Label trefwoord = grid.Rows[grid.SelectedIndex].FindControl("LblTrefwoord") as Label;
            Selected = m_ListDetailsEntities.Where(listelt => listelt.DataKeyValue.Equals(trefwoord.Text)).FirstOrDefault();
            Selected.SelectedIndex = grid.SelectedIndex;

            HandleCrud(grid);
        }

        private void HandleCrudOfDetailsViewer(GridView grid)
        {            
            m_Presenter.OnSelectedEntity(Selected);
            
            var userControlSelected = grid.Rows[grid.SelectedIndex].FindControl("Trefwoorden1") as Trefwoorden;

            int rowCount = grid.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                Trefwoorden userControl = grid.Rows[i].FindControl("Trefwoorden1") as Trefwoorden;
                if (userControl.Equals(userControlSelected))
                {
                    SetVisibleAndSorting(i,grid, userControl, true);
                }
                else
                {
                    SetVisibleAndSorting(i, grid, userControl, false);
                }
                GridViewRow row = grid.Rows[i];
                SetDetailsView(row);
            }
        }
        private void HandleCrud(GridView grid)
        {
            m_Presenter.OnSelectedEntity(Selected);

            int rowCount = grid.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {

                if (grid.SelectedIndex==i)
                {
                    SetVisibleAndSorting(i, grid, null, true);
                }
                else
                {
                    SetVisibleAndSorting(i, grid, null, false);
                }
                GridViewRow row = grid.Rows[i];
                //SetDetailsView(row);
            }
        }
        /// <summary>
        /// Zet de commandbuttons visible en maak de usercontroler sorteerbaar.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="userControl"></param>
        /// <param name="enableVisibilityAndSorting"></param>
        private void SetVisibleAndSorting(int rowIndex,GridView grid, Trefwoorden userControl, bool enableVisibilityAndSorting)
        {
            if (userControl != null)
            {
                userControl.InsertButton.Visible = enableVisibilityAndSorting;
                userControl.AllowCrud = enableVisibilityAndSorting;
                
            }
            ImageButton edit = grid.Rows[rowIndex].FindControl("BtnEdit") as ImageButton;
            ImageButton delete = grid.Rows[rowIndex].FindControl("BtnDelete") as ImageButton;
            ImageButton cancel = grid.Rows[rowIndex].FindControl("BtnCancel") as ImageButton;

            if (edit != null && cancel != null && delete != null)
            {
                edit.Visible = enableVisibilityAndSorting;
                delete.Visible = enableVisibilityAndSorting;
                cancel.Visible = enableVisibilityAndSorting;
            }

            
        }


        protected void BtnSelect_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {

        }
        protected void LinkTrefwoord_Click(object sender, EventArgs e)
        {
            m_Presenter.OnViewLoaded();
            GridView2.Visible = false;
            GridView3.Visible = true;            
        }
        protected void OnMasterDetailView(object sender, ImageClickEventArgs e)
        {
            MasterDetailView.Visible = false;
            m_Presenter.OnViewLoaded();
            GridView2.Visible = true;
            GridView3.Visible = false; 
        }
        protected void OnDetailsView(object sender, ImageClickEventArgs e)
        {
            MasterDetailView.Visible = true;
            m_Presenter.OnViewLoaded();
            GridView2.Visible = false;
            GridView3.Visible = true; 
        }
        protected void TrefwoordenTableDatasource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {

        }
        protected void TrefwoordenTableDatasource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            BeheerContextEntity trefwoord = new BeheerContextEntity();
            trefwoord = e.Instance as BeheerContextEntity;

            if (trefwoord != null)
            {
                trefwoord.Tablename = "trefwoord";
                trefwoord.DataKeyName = "trefwoord";
                //var detail = m_ListDetailsEntities.Where(det => det.Id == trefwoord.Id).FirstOrDefault();
                var master = m_ListBeheerEntities.Where(mas => mas.Id == trefwoord.MasterId).FirstOrDefault();

                var updatedMaster = new BeheerContextEntity
                                        {
                                            Id = master.Id,
                                            DataKeyValue = trefwoord.Master//update de master.
                                        };
                trefwoord.Parent= new ParentKeyEntity
                                      {
                                          Id = master.Id,
                                          DataKeyValue = master.DataKeyValue
                                      };
                if (trefwoord.Id == -1)
                {
                    //-1 = geen trefwoord, -2 = lege tabel, -3 = nieuw trefwoord.
                    trefwoord.Id = -3;
                    updatedMaster.Details.Add(trefwoord);
                    m_Presenter.OnBusinessEntityUpdated(updatedMaster);                    
                }
                else
                    m_Presenter.OnDetailEntityUpdated(trefwoord);
            }
            m_Presenter.OnViewLoaded();
        }
        protected void TrefwoordenTableDatasource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            BeheerContextEntity trefwoord = new BeheerContextEntity();
            trefwoord = e.Instance as BeheerContextEntity;

            if (trefwoord != null)
            {
                if (m_DeleteAlleenTrefwoord)
                {
                    trefwoord.Tablename = "trefwoord";
                    trefwoord.DataKeyName = "trefwoord";
                    m_Presenter.OnDetailEntityDeleted(trefwoord);
                }
                else
                {
                    //Verwijder de master/categorie.
                    var detail = trefwoord;
                    BeheerContextEntity categorie;

                    //Haal gegevens op van de master.
                    string naam = detail.Master;
                    var master = m_ListBeheerEntities.Where(cat => cat.DataKeyValue == naam).FirstOrDefault();

                    categorie = new BeheerContextEntity
                                    {
                                        DataKeyValue = naam,
                                        Id = master.Id
                                    };
                    m_Presenter.OnBusinessEntityDeleted(categorie);
                }
            }
            m_Presenter.OnViewLoaded();
        }
        protected void CategorieTableDataSource_Updating(object sender, ObjectContainerDataSourceUpdatingEventArgs e)
        {

        }
        protected void TrefwoordenTableDatasource_Updating(object sender, ObjectContainerDataSourceUpdatingEventArgs e)
        {

        }

        protected void BtnInsertTrefwoord_Click(object sender, ImageClickEventArgs e)
        {
            m_Presenter.OnViewLoaded();
            m_Presenter.OnViewShowfooter(true);
             
        }
        protected void BtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            m_DeleteAlleenTrefwoord = true;
        }
        protected void TrefwoordenTableDatasource_Deleting(object sender, ObjectContainerDataSourceDeletingEventArgs e)
        {
            //var dataSourceView = sender as ObjectContainerDataSourceView;
            //BeheerContextEntity trefwoord = new BeheerContextEntity();

            //if (dataSourceView != null)
            //{
            //    var items = dataSourceView.Items;

            //    if (m_DeleteAlleenTrefwoord)
            //    {
            //        //Haal gegevens op van de master.
            //        var item = items[0] as BeheerContextEntity;
            //        //BeheerContextEntity master = new BeheerContextEntity();
            //        //if (item != null)
            //        //{
            //        //    string naam = item.Master;
            //        //    master = m_ListBeheerEntities.Where(cat => cat.DataKeyValue == naam).FirstOrDefault();
            //        //}

            //        trefwoord.Tablename = "trefwoord";
            //        trefwoord.DataKeyName = "trefwoord";
            //        if (item != null)
            //        {
            //            trefwoord.Id = item.Id;
            //            trefwoord.DataKeyValue = item.DataKeyValue;
            //            trefwoord.Master = item.Master;
            //        }                    
            //        m_Presenter.OnDetailEntityDeleted(trefwoord);
            //    }
            //    else
            //    {
            //        //Verwijder de master/categorie.
            //        var item = dataSourceView.Items[0] as BeheerContextEntity;
            //        BeheerContextEntity categorie = null;

            //        //Haal gegevens op van de master.
            //        if (item != null)
            //        {
            //            string naam = item.Master;
            //            var master = m_ListBeheerEntities.Where(cat => cat.DataKeyValue == naam).FirstOrDefault();

            //            categorie = new BeheerContextEntity
            //                            {
            //                                DataKeyValue = naam,
            //                                Id = master.Id
            //                            };
            //        }
            //        m_Presenter.OnBusinessEntityDeleted(categorie);
            //    }
            //}
        }

        protected void OnLinkSortTrefwoordAsc(object sender, EventArgs e)
        {
            var direction = GridView3.SortDirection;

            if (direction==SortDirection.Ascending)
            {
                GridView3.Sort("DataKeyValue", SortDirection.Descending);
            }
            else
            {
                GridView3.Sort("DataKeyValue", SortDirection.Ascending);
            }
            
        }
        protected void OnLinkSortCategorie(object sender, EventArgs e)
        {
            var direction = GridView3.SortDirection;

            if (direction == SortDirection.Ascending)
            {
                GridView3.Sort("Master", SortDirection.Descending);
            }
            else
            {
                GridView3.Sort("Master", SortDirection.Ascending);
            }
        }

        protected void OnInsertTrefwoordItemTemplate(object sender, ImageClickEventArgs e)
        {
            m_InsertAlleenTrefwoord = true;
        }
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            m_Presenter.OnViewShowfooter(false);
        }
        protected void TrefwoordenTableDatasource_Inserting(object sender, ObjectContainerDataSourceInsertingEventArgs e)
        {

        }
        
        protected void BtnSaveFooter_Click(object sender, ImageClickEventArgs e)
        {
            var footer = GridView3.FooterRow;
            var categorie = footer.FindControl("tbCategorieFooter") as TextBox;
            BeheerContextEntity master = new BeheerContextEntity
                                             {
                                                 DataKeyValue = categorie.Text
                                             };           

            var trefwoord = footer.FindControl("tbTrefwoordInsertFooter") as TextBox;
            BeheerContextEntity detail = null;
            if (trefwoord != null && !string.IsNullOrEmpty(trefwoord.Text))
            {
                detail = new BeheerContextEntity
                             {
                                 Id = -1,
                                 DataKeyValue = trefwoord.Text,
                                 Master = master.DataKeyValue
                             };
                master.Details.Add(detail);
                
            }
            var found = m_ListBeheerEntities.Where(cat => cat.DataKeyValue.Equals(master.DataKeyValue)).FirstOrDefault();

            if(found==null)
            {
                m_Presenter.OnBusinessEntityAdded(master);

                if (detail != null && trefwoord != null && !string.IsNullOrEmpty(trefwoord.Text))
                {
                    m_Presenter.OnDetailEntityUpdated(detail);
                }
                m_Presenter.OnViewLoaded();
            }
            else
            {
                m_Presenter.DuplicateMaster = new StateValue<BeheerContextEntity>(master);
                m_Presenter.OnViewShowErrorMessage("<b>" + master.DataKeyValue + " bestaat al. Dubbele categorieen zijn niet toegestaan.</b>");
                m_Presenter.OnViewShowfooter(true);
                m_Presenter.OnViewLoaded();//ondataboundchecks
            }

        }



    }
}
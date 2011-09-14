using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.Practices.Web.UI.WebControls.Utility;
using Page=Microsoft.Practices.CompositeWeb.Web.UI.Page;
using System.Globalization;
using System.Threading;

namespace MetaData.Beheer.Views
{
    public partial class Categorieen : Page,
                                       IBusinessEntityView
    {
        private IList<BeheerContextEntity> m_ListBeheerEntities;
        private CategorieenPresenter m_Presenter;
        private IList<BeheerContextEntity> m_ListDetailsEntities;
        /// <summary>
        /// Geeft aan of een detail compleet visueel mag zijn.
        /// </summary>
        private IList<BeheerContextEntity> m_ListVisualDetails;
        private bool m_DeleteAlleenTrefwoord;
        private bool m_IsCrudInline;
        private bool m_IsInsertingInline;
        private string vorigeCategorie;
        private bool m_IsAscending;
        private bool m_IsEditingDetail;
        private bool m_IsPreparingInline;
        private string m_TrefwoordTextBoxClientId;
        private List<string> m_ListRowClientIds = new List<string>();

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
                m_ListBeheerEntities = value;//master

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

        public BeheerContextEntity Master
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsSortable
        {
            set { throw new NotImplementedException(); }
        }

        public bool AllowCrud { set; private get; }

        public BeheerContextEntity Selected { set; get; }
        public bool InsertButtonIsVisible { set; private get; }
        public bool ShowFooter
        {
            set
            {
                TrefwoordView.ShowFooter = value;
            }
        }

        public bool IsInsertingInline
        {
            set { m_IsInsertingInline = value; }
        }

        public void ShowErrorMessage(string errorMessage)
        {

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BtnInsertCategorie.Enabled = true;


            if (!IsPostBack)
            {
                m_Presenter.OnViewInitialized();
                
            }
            
            TrefwoordView.RowDataBound += OnRowDataBoundDetailsView;
            TrefwoordView.RowCreated += OnRowCreated;
                        
            m_Presenter.OnViewLoaded();
            AccepteerTrefwoord();

            TrefwoordView.ShowFooter = false;
            BtnInsertCategorie.Visible = true;
        }



        #region SCRAP
        /// <summary>
        /// Dit geldt alleen voor inline insert van Trefwoord.
        /// </summary>
        protected void AccepteerTrefwoord()
        {
            int i;
            if (Int32.TryParse(Request.Form["RowId"], out i))
            {
                var detail = m_ListDetailsEntities[i - 1];// m_Presenter.m_ListDetails[i - 1];// m_ListDetailsEntities[i - 1]
                var updatedMaster = new BeheerContextEntity
                {
                    Id = detail.MasterId,
                    DataKeyValue = detail.Master
                };
                var trefwoord = new BeheerContextEntity
                {
                    Id = -3, //-1 = geen trefwoord, -2 = lege tabel, -3 = nieuw trefwoord.
                    DataKeyValue = Request.Form["InlineInsert"],
                    Master = detail.Master,
                    MasterId = detail.MasterId,
                    Parent = new ParentKeyEntity
                    {
                        Id = detail.MasterId,
                        DataKeyValue = detail.Master
                    }
                };

                updatedMaster.Details.Add(trefwoord);
                m_Presenter.OnBusinessEntityUpdated(updatedMaster);
                m_Presenter.OnViewLoaded();
            }
            else
            {                
                //TODO: this.ErrorMessages["TxtBox1"] = "Input data is not an integer";
            }
        }



        private void OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.ClientID != null) {m_ListRowClientIds.Add(row.ClientID.Replace('$','_'));}
            }
        }

        protected void OnPreRenderGridView(object sender, EventArgs e)
        {

            // You only need the following 2 lines of code if you are not 
            // using an ObjectDataSource of SqlDataSource
            //GridView1.DataSource = Sample.GetData();
            //GridView1.DataBind();

            if (TrefwoordView.Rows.Count > 0)
            {
                ////This replaces <td> with <th> and adds the scope attribute
                TrefwoordView.UseAccessibleHeader = true;

                ////This will add the <thead> and <tbody> elements
                TrefwoordView.HeaderRow.TableSection = TableRowSection.TableHeader;
                
                ////This adds the <tfoot> element. 
                ////Remove if you don't have a footer row
                //TrefwoordView.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {


            string controlId = TrefwoordView.ClientID;
            // Define the name and type of the client scripts on the page.
            StringBuilder sb = new StringBuilder();
            Type cstype = GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            String csname1 = "jQueryScript";

            #region jQuery

            sb.Append("$(document).ready(function() {");
            sb.Append(
                @"  $(""#.delbutton"").click(function() {
                        try{                            
                            
                            //Get the Id of the record to delete  
                            var record_id = $(this).attr(""id"");  

                            //Get the GridView Row reference  
                            var tr_id = $(this).parents(""#.record"");                                                        
                            
                            //add  GridView row 
                            //$(""#.gridview > tbody:nth-child(1)"").after('<TR><td>Hello World!</td><td></td><td></td><td></td><td></td></TR>'); 
                            //$('.gridview tr:last').after('<TR><td>WORLD!</td><td></td><td></td><td></td><td></td></TR>');   //<td>WORLD!</td><td></td><td></td>
                            var cloned = tr_id.clone(true);
                            tr_id.clone(true).insertAfter(tr_id);
                            
                            $(""#test tr:last"").after('<td>WORLD!</td><td></td><td></td>');   //<td>WORLD!</td><td></td><td></td> 
                            $('#test2 > tbody:last').after('<td>WORLD!</td><td></td><td></td>');   //<td>WORLD!</td><td></td><td></td>   

                            $('.label').hide(""slow"");  $('.button').hide(""slow""); 
                            //alert('done');
                        }
                        catch(Error){                                       
                            $(""#ObservableByErrorHandler"").append(""<div>"" + Error.description + ""</div>"");  
                        }
                    });

                    $(""#ObservableByErrorHandler"").click(function() {
                        $(this).hide(""slow"");            
                    });
                    $(""#Button1"").click(function() {
                        
                       $(this).hide(""slow"");   
                        alert('done');
           
                    });
                    ");
            sb.Append("});");

            #endregion

            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(cstype, csname1))
            {
                //ClientScript.RegisterClientScriptBlock(cstype, csname1, sb.ToString(), true);
            }
        }

        private void ShowClientControlId(string controlId)
        {
            String csname1 = "PopupScript";
            String csname2 = "ButtonClickScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('Hello World');";
                cs.RegisterStartupScript(cstype, csname1, cstext1);
            }

            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(cstype, csname2))
            {
                StringBuilder cstext2 = new StringBuilder();
                cstext2.Append("<script type=\"text/javascript\"> function DoClick() {");
                cstext2.Append(@"$(#ctl00_DefaultContent_TrefwoordView_ctl02_TrefwoordTextBox).hide(""slow"");} </"); //alert('" + controlId + "')
                cstext2.Append("script>");
                cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString());
            }

        }

        protected void TrefwoordView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //TrefwoordView.RowDataBound -= OnRowDataBoundDetailsView;
        }


        #endregion


        private void OnRowDataBoundDetailsView(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            Trace.Write("OnRowDataBoundDetailsView", "--> Begin Current row: " + row.RowIndex);

            if (row.RowType == DataControlRowType.DataRow)
            {
                string lblCategorie = "LblCategorienaam";
                var lblCategorieFound = ((Label)row.FindControl(lblCategorie));
                string tbCategorie = "tbCategorie";
                var tbCategorieFound = ((TextBox)row.FindControl(tbCategorie));

                string lblTrefwoord = "LblTrefwoord";
                var lblTrefwoordFound = ((Label) row.FindControl(lblTrefwoord));
                var insertMaster = row.FindControl("BtnInsert") as ImageButton;
                var editMaster = row.FindControl("BtnEdit") as ImageButton;
                var deleteMaster = row.FindControl("BtnDelete") as ImageButton;

                string categorieLabel = null;
                string categorieTextBox = null;
                if (lblCategorieFound!=null) categorieLabel = lblCategorieFound.Text;
                if (tbCategorieFound != null) categorieTextBox = tbCategorieFound.Text;

                bool isVisible = false;
                if (!string.IsNullOrEmpty(vorigeCategorie) &&
                    (vorigeCategorie.Equals(categorieLabel)|| vorigeCategorie.Equals(categorieTextBox)))
                {
                    #region DISABLE CRUD (visible)
                    if (insertMaster != null && editMaster != null && deleteMaster != null)
                    {
                        insertMaster.Visible = false;
                        editMaster.Visible = false;
                        deleteMaster.Visible = false;
                    }
                    if (lblCategorieFound != null) lblCategorieFound.Visible = false;
                    #endregion
                }
                else
                {
                    isVisible = true;
                    #region ENABLE INLINE CRUD (visible)
                    if (insertMaster!=null && editMaster != null && deleteMaster != null)
                    {
                        insertMaster.Visible = false;
                        editMaster.Visible = true;
                        deleteMaster.Visible = true;
                        
                        if (m_IsCrudInline)
                        {
                            insertMaster.Enabled = false;
                            editMaster.Enabled = false;
                            deleteMaster.Enabled = false;
                        }
                        else
                        {
                            insertMaster.Enabled = true;
                            editMaster.Enabled = true;
                            deleteMaster.Enabled = true;
                        }
                    }
                    #endregion

                }
                vorigeCategorie = (categorieLabel ?? categorieTextBox);

                //itemtemplate-mode
                SetVisibilityCrudDetail(lblTrefwoordFound, row);

                //edittemplate-mode
                SetVisibilityCrudDetail(row, isVisible);
            }
            if (row.RowType==DataControlRowType.Footer)
            {
                //var validator = row.FindControl("RfvEditCategorieFooter") as RequiredFieldValidator;
                //validator.ErrorMessage = m_Presenter.ErrorMessage.Value;
                //if (m_Presenter.ErrorMessage.Value!=null)
                //{
                //    var categorie = row.FindControl("tbCategorieFooter") as TextBox;
                //    if (m_Presenter.DuplicateMaster != null)
                //    {
                //        //categorie.Text = m_Presenter.DuplicateMaster.Value.DataKeyValue;
                //        categorie.Text = "";
                //        validator.Validate();                      
                //    }
                //    m_Presenter.OnViewShowfooter(true);
                //    m_Presenter.ErrorMessage = null;
                    ////validator.ErrorMessage = "<b>Categorie is een verplicht veld</b>";
                //}
                //else
                //    validator.ErrorMessage = "<b>Categorie is een verplicht veld</b>";
            }
            Trace.Write("OnRowDataBoundDetailsView", "--> End Current row..................");
        }
        /// <summary>
        /// For Inline Insert mode via edit mode 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="isVisible"></param>
        private void SetVisibilityCrudDetail(GridViewRow row, bool isVisible)
        {
            string trefwoordId = "TrefwoordTextBox";
            var trefwoordTexBox = ((TextBox)row.FindControl(trefwoordId));
            

            if (trefwoordTexBox != null)
            {

                m_TrefwoordTextBoxClientId = trefwoordTexBox.ClientID;
                //ShowClientControlId(m_TrefwoordTextBoxClientId);

                var tbCategorie = row.FindControl("tbCategorie") as TextBox;

                if (m_IsEditingDetail)
                {
                    //Maak de categorie/master controls onzichtbaar
                    MakeMasterInvisble(row, trefwoordTexBox, tbCategorie, isVisible);
                }
                else 
                {


                    if (m_IsPreparingInline)
                    {
                        //Maak de categorie/master controls onzichtbaar
                        MakeMasterInvisble(row, trefwoordTexBox, tbCategorie, isVisible);

                        if (!trefwoordTexBox.Text.Trim().Equals(string.Empty))
                        {
                            //Maak de textboxen zichtbaar
                            var tbTrefwoordInline = row.FindControl("TrefwoordTextBoxInline") as TextBox;
                            var lbTrefwoordInline = row.FindControl("LblTrefwoordInline") as Label;
                            if (lbTrefwoordInline != null) lbTrefwoordInline.Text = trefwoordTexBox.Text;
                            var invisibleInsertInliner = row.FindControl("InvisibleInsertInline") as Image;
                            var invisibleInsertInline2 = row.FindControl("InvisibleInsertInline2") as Image;

                            var btnUpdateDetail = row.FindControl("BtnUpdateDetail") as ImageButton;
                            var btnUpdateDetail2 = row.FindControl("BtnUpdateDetail2") as ImageButton;

                            //inline insert
                            trefwoordTexBox.Enabled = false;
                            trefwoordTexBox.Visible = false;
                            if (tbTrefwoordInline != null) tbTrefwoordInline.Visible = true;
                            if (lbTrefwoordInline != null) lbTrefwoordInline.Visible = true;
                            if (invisibleInsertInline2 != null) invisibleInsertInline2.Visible = true;
                            if (invisibleInsertInliner != null) invisibleInsertInliner.Visible = true;
                            if (btnUpdateDetail != null) btnUpdateDetail.Visible = false;
                            if (btnUpdateDetail2 != null) btnUpdateDetail2.Visible = true;
                            TrefwoordView.SelectedIndex = row.RowIndex;

                            //De presenter orkestreert de CRUD.
                            m_Presenter.OnInlineDetailInsert();
                        }
                    }
                }

            }
        }

        private void MakeMasterInvisble(GridViewRow row, TextBox trefwoordTexBox, TextBox tbCategorie,
            bool isVisible)
        {
            //Zet insert uit voor categorieen/masters.
            BtnInsertCategorie.Enabled = false;

            var editMaster = row.FindControl("BtnEdit") as ImageButton;
            var cancelMaster = row.FindControl("BtnCancelEdit") as ImageButton;

            if (editMaster != null) editMaster.Visible = isVisible;
            if (cancelMaster != null) cancelMaster.Visible = isVisible;

            trefwoordTexBox.Visible = true;
            if (tbCategorie != null) tbCategorie.Visible = isVisible;
        }

        /// <summary>
        /// For View mode
        /// </summary>
        /// <param name="controlFound"></param>
        /// <param name="row"></param>
        private void SetVisibilityCrudDetail(Label controlFound, GridViewRow row)
        {

            if (controlFound != null)
            {
                var insertMaster = row.FindControl("BtnInsert") as ImageButton;
                var editMaster = row.FindControl("BtnEdit") as ImageButton;
                var deleteMaster = row.FindControl("BtnDelete") as ImageButton;

                var insertDetail = row.FindControl("Inlineinsert") as Image;
                var editDetail = row.FindControl("BtnEditDetail") as ImageButton;
                var deleteDetail = row.FindControl("BtnDeleteDetail") as ImageButton;
                var invisibleItemEdit = row.FindControl("InvisibleItemEdit") as Image;
                var invisibleItemDelete = row.FindControl("InvisibleItemDelete") as Image;

                if (m_IsCrudInline)
                {
                    if (insertDetail != null) insertDetail.Enabled = false;
                    if (editDetail != null) editDetail.Enabled = false;
                    if (deleteDetail != null) deleteDetail.Enabled = false;
                }
                
                var detailNaam = controlFound.Text;

                BeheerContextEntity detail = m_ListDetailsEntities.Where(
                    det => det.DataKeyValue == detailNaam).FirstOrDefault();
              
                if (detail.Id == -1)
                {                    
                    if (insertDetail != null) insertDetail.Visible = false;

                    var insertDetailInline = row.FindControl("Inlineinsert") as Image;
                    if (insertDetailInline != null) insertDetailInline.Visible = true;

                    if (insertMaster != null) insertMaster.Visible = false;
                    //if (editMaster != null) editMaster.Visible = false;
                    //if (deleteMaster != null) deleteMaster.Visible = false;

                    var insertCategorie = row.FindControl("BtnInsertCategorie") as ImageButton;
                    if (insertCategorie != null) insertCategorie.Visible = true;
                    if (editDetail != null) editDetail.Visible = false;
                    if (deleteDetail != null) deleteDetail.Visible = false;
                    if (invisibleItemEdit != null) invisibleItemEdit.Visible = false;
                    
                    //if (invisibleItemDelete != null) invisibleItemDelete.Visible = true;
                }

                if (controlFound.Text.Equals("leeg"))
                {
                    
                    if (insertDetail != null) insertDetail.Visible = false;

                    if (insertMaster != null) insertMaster.Visible = false;
                    if (editMaster != null) editMaster.Visible = false;
                    if (deleteMaster != null) deleteMaster.Visible = false;
                    if (editDetail != null) editDetail.Visible = false;
                    if (deleteDetail != null) deleteDetail.Visible = false;
                    if (invisibleItemEdit != null) invisibleItemEdit.Visible = true;
                    if (invisibleItemDelete != null) invisibleItemDelete.Visible = true;
                }
            }
        }


        protected void TrefwoordenTableDatasource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {

        }
        protected void TrefwoordenTableDatasource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            BeheerContextEntity trefwoord = new BeheerContextEntity();
            trefwoord = e.Instance as BeheerContextEntity;

            if (!m_IsInsertingInline)
            {
                if (trefwoord != null)
                {
                    trefwoord.DataKeyValue = trefwoord.DataKeyValue.Trim();
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
                    if (trefwoord.Id == -1 && !trefwoord.DataKeyValue.Equals(string.Empty))
                    {
                        //-1 = geen trefwoord, -2 = lege tabel, -3 = nieuw trefwoord.
                        trefwoord.Id = -3;
                        updatedMaster.Details.Add(trefwoord);
                        m_Presenter.OnBusinessEntityUpdated(updatedMaster);                    
                    }
                    else
                        m_Presenter.OnDetailEntityUpdated(trefwoord);
                }
            }
            else
            {
                #region  inline insert trefwoord gebruikt een selectedrow

                int selectedIndex = TrefwoordView.SelectedIndex;
                GridViewRow row = TrefwoordView.Rows[selectedIndex];

                var tbCategorie = row.FindControl("tbCategorie") as TextBox;                
                var tbTrefwoordInline = TrefwoordView.Rows[selectedIndex].FindControl("TrefwoordTextBoxInline") as TextBox;
                
                var master = m_ListBeheerEntities.Where(mas => mas.DataKeyValue == tbCategorie.Text).FirstOrDefault();
                var updatedMaster = new BeheerContextEntity
                {
                    Id = master.Id,
                    DataKeyValue = master.DataKeyValue
                };
                trefwoord = new BeheerContextEntity
                                {                                   
                                    Id = -3, //-1 = geen trefwoord, -2 = lege tabel, -3 = nieuw trefwoord.
                                    DataKeyValue = tbTrefwoordInline.Text,
                                    Master = master.DataKeyValue,
                                    MasterId = master.Id,
                                    Parent = new ParentKeyEntity
                                    {
                                        Id = master.Id,
                                        DataKeyValue = master.DataKeyValue
                                    }
                                };

                updatedMaster.Details.Add(trefwoord);
                m_Presenter.OnBusinessEntityUpdated(updatedMaster);   
                #endregion


                TrefwoordView.SelectedIndex = -1;
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
        
        protected void TrefwoordenTableDatasource_Updating(object sender, ObjectContainerDataSourceUpdatingEventArgs e)
        {

        }

        protected void BtnInsertCategorie_Click(object sender, ImageClickEventArgs e)
        {
            m_Presenter.OnViewLoaded();
            m_Presenter.OnViewShowfooter(true);
            BtnInsertCategorie.Visible = false;
        }
        protected void BtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            m_DeleteAlleenTrefwoord = true;
        }
        protected void TrefwoordenTableDatasource_Deleting(object sender, ObjectContainerDataSourceDeletingEventArgs e)
        {
            
        }

        protected void OnLinkSortTrefwoordAsc(object sender, EventArgs e)
        {
            //SortDetailsView();
            #region sortering van details breekt Master-detail sortering
            if (TrefwoordView.SortDirection == SortDirection.Ascending)
            {
                TrefwoordView.Sort("DataKeyValue", SortDirection.Descending);                
            }
            else
            {
                TrefwoordView.Sort("DataKeyValue", SortDirection.Ascending);                
            }
            #endregion
        }

        private void SortDetailsView()
        {
            if (m_Presenter.IsSortedAscending.Value)
            {
                m_Presenter.OnSortDetailsAscending();
            }
            else
            {
                m_Presenter.OnSortAscendingDetails();
            }
        }

        protected void OnLinkSortCategorie(object sender, EventArgs e)
        {
            //SortView();
            #region sortering van Master breekt Master-detail sortering

            if (TrefwoordView.SortDirection == SortDirection.Ascending)
            {
                TrefwoordView.Sort("Master", SortDirection.Descending);                
            }
            else
            {
                TrefwoordView.Sort("Master", SortDirection.Ascending);                
            }
            #endregion
        }


        protected void TrefwoordenTableDatasource_Inserting(object sender, ObjectContainerDataSourceInsertingEventArgs e)
        {

        }
        
        protected void BtnSaveFooter_Click(object sender, ImageClickEventArgs e)
        {
            var footer = TrefwoordView.FooterRow;
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

        protected void OnInsert(object sender, ImageClickEventArgs e)
        {
            m_IsCrudInline = true;
        }
        protected void OnBtnEdit(object sender, ImageClickEventArgs e)
        {
            TrefwoordView.SelectedIndex = -1;
            m_IsCrudInline = true;
            m_Presenter.OnViewShowfooter(false);
        }
        protected void OnBtnEditDetail(object sender, ImageClickEventArgs e)
        {
            TrefwoordView.SelectedIndex = -1;
            m_IsCrudInline = true;
            m_IsEditingDetail = true;
            m_Presenter.OnViewShowfooter(false);
        }
        protected void TrefwoordView_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        protected void OnInsertTrefwoordItemTemplate(object sender, ImageClickEventArgs e)
        {
            TrefwoordView.SelectedIndex = -1;
            IsInsertingInline = true;
        }
        protected void OnSaveInsertInlineTrefwoord(object sender, ImageClickEventArgs e)
        {

        }
        protected void OnPrepareInlineInsert(object sender, ImageClickEventArgs e)
        {
            TrefwoordView.SelectedIndex = -1;
            m_IsPreparingInline = true;
        }
        protected void BtnCancelDetail_Click(object sender, ImageClickEventArgs e)
        {
            TrefwoordView.SelectedIndex = -1;
        }
        protected void BtnCancelEdit_Click(object sender, ImageClickEventArgs e)
        {
            TrefwoordView.SelectedIndex = -1;
        }
        protected void BtnUpdateDetail_Click(object sender, ImageClickEventArgs e)
        {
            //Thread.Sleep(500);
        }
}
}
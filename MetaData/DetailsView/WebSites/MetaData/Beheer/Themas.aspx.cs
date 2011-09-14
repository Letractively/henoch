using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;

using MetaData.Beheer.Views;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.Web.UI.WebControls;

namespace MetaData.Beheer
{
    public partial class Themas : Page,
                                  IBusinessEntityView
    {
        private ThemasPresenter m_Presenter;

        [CreateNew]
        public ThemasPresenter Presenter
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
        public void ShowErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }
        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //		ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.wcsf.2007oct/wcsf/html/08da6294-8a4e-46b2-8bbe-ec94c06f1c30.html

        public IList<Thema> ThemaTable
        {
            set { ThemaTableDataSource.DataSource = value; }
        }

        #region IBusinessEntityView Members

        public IList<BeheerContextEntity> BusinessEntities
        {
            set { ThemaTableDataSource.DataSource = value; }
        }

        public IList<BeheerContextEntity> DetailsEntities
        {
            set { throw new NotImplementedException(); }
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
        public bool InsertButtonIsVisible { set; private get; }


        public bool AllowCrud { set; private get; }

        public BeheerContextEntity Selected { set; private get; }
        public bool ShowFooter { set; private get; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_Presenter.OnViewInitialized();
            }
            m_Presenter.OnViewLoaded();
        }


        protected void ThemaTableDataSource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            BeheerContextEntity thema = new BeheerContextEntity();
            thema = e.Instance as BeheerContextEntity;
            if (thema != null)
            {
                thema.Tablename = "thema";
                thema.DataKeyName = "themanaam";
                m_Presenter.OnBusinessEntityAdded(thema);
            }
            m_Presenter.OnViewLoaded();
        }

        protected void ThemaTableDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            m_Presenter.OnBusinessEntityDeleted(e.Instance as BeheerContextEntity);
            m_Presenter.OnViewLoaded();
        }

        protected void ThemaTableDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            m_Presenter.OnBusinessEntityUpdated(e.Instance as BeheerContextEntity);
            m_Presenter.OnViewLoaded();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
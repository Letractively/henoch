using System;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.Shell.MasterPages
{
    public partial class DefaultMaster : MasterPage, IDefaultMasterView
    {
        private DefaultMasterPresenter _presenter;

        [CreateNew]
        public DefaultMasterPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _presenter = value;
                _presenter.View = this;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _presenter.OnViewInitialized();
            }
            _presenter.OnViewLoaded();
        }
    }
}
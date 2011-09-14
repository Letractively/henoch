using System;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.Beheer.Views
{
    public partial class BeheerDefault : Page, IDefaultView
    {
        private DefaultViewPresenter _presenter;

        [CreateNew]
        public DefaultViewPresenter Presenter
        {
            set
            {
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
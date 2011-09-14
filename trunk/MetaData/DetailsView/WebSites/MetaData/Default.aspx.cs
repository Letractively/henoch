using System;
using MetaData.Shell.Views;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;

public partial class ShellDefault : Page, IDefaultView
{
    private DefaultViewPresenter _presenter;

    [CreateNew]
    public DefaultViewPresenter Presenter
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
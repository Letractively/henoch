using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.BeheerThemas.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        private IBeheerThemasController _controller;

        public DefaultViewPresenter([CreateNew] IBeheerThemasController controller)
        {
            this._controller = controller;
        }
    }
}

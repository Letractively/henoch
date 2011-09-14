using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.Beheer.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        private IBeheerController _controller;

        public DefaultViewPresenter([CreateNew] IBeheerController controller)
        {
            this._controller = controller;
        }
    }
}

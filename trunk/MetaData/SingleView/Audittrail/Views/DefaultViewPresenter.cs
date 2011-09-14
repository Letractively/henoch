using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.Audittrail.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        private IAudittrailController _controller;

        public DefaultViewPresenter([CreateNew] IAudittrailController controller)
        {
            this._controller = controller;
        }
    }
}

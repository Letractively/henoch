using System;
using System.Collections.Generic;
using System.Text;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.Audittrail.Views
{
    public class AudittrailPresenter : Presenter<IAudittrailView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private IAudittrailController _controller;
        public AudittrailPresenter([CreateNew] IAudittrailController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.BusinessEntities = _controller.GetEntities();
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view
        public void OnBusinessEntityAdded(AuditItem audit)
        {
            _controller.AddBusinessEntity(audit);
        }
    }
}





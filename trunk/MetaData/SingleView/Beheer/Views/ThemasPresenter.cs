using System;
using System.Collections.Generic;
using System.Text;
using Beheer.BusinessObjects;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.Beheer.Views
{
    public class ThemasPresenter : Presenter<IBusinessEntityView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private IBeheerController _controller;
        public ThemasPresenter([CreateNew] IBeheerController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.BusinessEntities = _controller.GetEntities();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view
        public void OnBusinessEntityAdded(BeheerContextEntity beheerContextEntity)
        {
            _controller.AddBusinessEntity(beheerContextEntity);
        }

        public void OnBusinessEntityUpdated(BeheerContextEntity beheerContextEntity)
        {
            _controller.UpdateBusinessEntity(beheerContextEntity);
        }

        public void OnBusinessEntityDeleted(BeheerContextEntity beheerContextEntity)
        {
            _controller.DeleteBusinessEntity(beheerContextEntity);
        }
    }
}





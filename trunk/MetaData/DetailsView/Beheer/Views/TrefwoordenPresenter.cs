using System;
using System.Collections.Generic;
using System.Text;
using Beheer.BusinessObjects;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.Beheer.Views
{
    public class TrefwoordenPresenter : Presenter<IBusinessEntityView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private ITrefwoordController _controller;
        public StateValue<BeheerContextEntity> Master;
        public StateValue<bool> AllowCrud;

        public TrefwoordenPresenter([CreateNew] ITrefwoordController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.BusinessEntities = _controller.GetEntities();
            Master.Value = _controller.GetMaster();

            View.Master = Master.Value;
            //var selectedmaster = _controller.GetSelectedMaster();
            //if (selectedmaster != null)
            //    View.AllowCrud = (selectedmaster.Equals(Master.Value));
        }

        public override void OnViewInitialized()
        {            
            //View.BusinessEntities = _controller.GetEntities();
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view
        public void OnBusinessEntityAdded(BeheerContextEntity entity)
        {
            _controller.AddBusinessEntity(entity);            
        }

        public void OnBusinessEntityUpdated(BeheerContextEntity entity)
        {
            _controller.UpdateBusinessEntity(entity);
        }

        public void OnBusinessEntityDeleted(BeheerContextEntity entity)
        {
            _controller.DeleteBusinessEntity(entity);
        }
    }
}





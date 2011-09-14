using System;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Services;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using MetaData.BeheerThemas;

namespace MetaData.BeheerThemas.Views
{
    public class BeheerThemasPresenter : Presenter<IBeheerThemasView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private IBeheerThemasController _controller;
        public BeheerThemasPresenter([CreateNew] IBeheerThemasController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            // <pex>
            if (View == (IBeheerThemasController)null)
                throw new ArgumentNullException("View");
            // </pex>
            View.ThemaTable = _controller.GetThemaTable();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void OnThemasAdded(Thema thema)
        {
            _controller.AddThema(thema);
        }

        public void OnThemasUpdated(Thema thema)
        {
            _controller.UpdateThema(thema);
        }
        
        public void OnThemasDeleted(Thema thema)
        {
            _controller.DeleteThema(thema);
        }

        // TODO: Handle other view events and set state in the view
    }
}





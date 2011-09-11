//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add CAB Module" recipe.
//
// The Module class derives from the CAB base class ModuleInit and it will be instantiated
// when the module is loaded by the CAB infrastructure.
//
// 
//
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using OrderManagement.Infrastructure.Interface;

namespace OrderManagement.Infrastructure.Module
{
    public class Module : ModuleInit
    {
        private WorkItem _rootWorkItem;

        [InjectionConstructor]
        public Module([ServiceDependency] WorkItem rootWorkItem)
        {
            _rootWorkItem = rootWorkItem;
        }

        public override void Load()
        {
            base.Load();

            ControlledWorkItem<ModuleController> workItem = _rootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>();
            workItem.Controller.Run();
        }
    }
}

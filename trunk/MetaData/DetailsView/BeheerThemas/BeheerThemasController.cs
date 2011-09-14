using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Services;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.BeheerThemas
{
    public class BeheerThemasController : IBeheerThemasController
    {

        public BeheerThemasController()
        {
        }

        public virtual IList<Thema> ThemaTable { get; private set; }

        [InjectionConstructor]
        public BeheerThemasController([ServiceDependency]IBeheerThemasService beheerThemasService)
        {
            // <pex>
            if (beheerThemasService == (IBeheerThemasService)null)
                throw new ArgumentNullException("beheerThemasService");
            // </pex>
            BeheerThemasService = beheerThemasService;
        }

        public virtual IBeheerThemasService BeheerThemasService { get; private set; }

        public virtual IList<Thema> GetThemaTable()
        {
            
            ThemaTable = BeheerThemasService.GetThemaTable();
            return ThemaTable;
        }

        public virtual bool AddThemaCalled { get; set; }

        public virtual void AddThema(Thema thema)
        {
            AddThemaCalled = true;
            BeheerThemasService.AddThema(thema);
        }

        public virtual Thema UpdatedThema { get; set; }
        public virtual Thema DeletedThema { get; set; }


        public virtual bool DeleteThemaCalled { get; set; }

        public virtual void DeleteThema(Thema thema)
        {
            DeleteThemaCalled = true;
            BeheerThemasService.DeleteThema(thema);
        }

        public virtual void UpdateThema(Thema thema)
        {
            UpdateThemaCalled = true;
            BeheerThemasService.UpdateThema(thema);
        }

        public virtual bool UpdateThemaCalled { get; set; }
    }
}

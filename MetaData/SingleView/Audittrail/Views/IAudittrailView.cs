using System;
using System.Collections.Generic;
using System.Text;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Audittrail.Views
{
    public interface IAudittrailView
    {
        IList<AuditItem> BusinessEntities { set; }
    }
}





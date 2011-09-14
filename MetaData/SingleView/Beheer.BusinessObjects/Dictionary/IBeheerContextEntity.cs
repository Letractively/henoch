using System.Collections.Generic;

namespace Beheer.BusinessObjects.Dictionary
{
    /// <summary>
    /// Representeert alleen superkeys/unique identifiers van een entity.
    /// System-wide en business data worden hier gebruikt.
    /// </summary>
    public interface IBeheerContextEntity : IAlternateKey        
    {
        int SelectedIndex { get; set;}
        IList<BeheerContextEntity> Details { get; set; }

        int MasterId { get; set; }
        string Master { get; set; }
        //TODO: wijzig de context voor meer attributen en hun typen.
        //bijv. Datetime Created { get; set; }
        ParentKeyEntity Parent { get; set; }  

        //TODO: wijzig de context voor meer attributen en hun typen.
    }
}
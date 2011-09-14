using System.Collections.Generic;

namespace MetaData.Beheer.Interface.BusinessEntities
{
    /// <summary>
    /// Representeert alleen superkeys/unique identifiers van een entity.
    /// </summary>
    public interface IBeheerContextEntity : IAlternateKey
    {
        IList<BeheerContextEntity> Details { get; set; }
        //TODO: wijzig de context voor meer attributen en hun typen.
        //bijv. Datetime Created { get; set; }
        ParentKeyEntity Parent { get; set; }
    }
}
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;


namespace MetaData.Beheer.Views
{
    public interface IBusinessEntityView
    {
        /// <summary>
        /// Dit is default een lijst van masters.
        /// </summary>
        IList<BeheerContextEntity> BusinessEntities { set; }
        /// <summary>
        /// Dit is een lijst van details.
        /// </summary>
        IList<BeheerContextEntity> DetailsEntities { set; }
        bool IsMasterView { set; }
        bool IsInsertingInline { set;}
        BeheerContextEntity Master { set; }

        bool AllowCrud { set;}
        BeheerContextEntity Selected { set;}
        bool ShowFooter { set; }

        void ShowErrorMessage(string errorMessage);
    }

}





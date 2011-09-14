using System;
using System.Collections.Generic;

namespace Beheer.BusinessObjects.Dictionary
{
    public interface IAuditItem
    {
        DateTime DatumTijd { get; set; }
        string ActieNemer { get; set; }
        string Actie { get; set; }
        string Tabel { get; set; }
        string Veld { get; set; }
        string SleutelWaarde { get; set; }
        string WaardeVan { get; set; }
        string WaardeNaar { get; set; }

        #region nodig voor repository
        ParentKeyEntity Parent { get; set; }
        string DataKeyValue { get; set; }
        int Id { get; set; }
        IList<AuditItem> Details { get; set; }
        string Master { get; set; }
        #endregion
    }

    public class AuditItem : IAuditItem
    {
        // Default constructor
        public AuditItem()
        {
        }

        // Copy constructor
        public AuditItem(AuditItem other)
        {
            DatumTijd = other.DatumTijd;
            ActieNemer = other.ActieNemer;
            Actie = other.Actie;
            Tabel = other.Tabel;
            Veld = other.Veld;
            SleutelWaarde = other.SleutelWaarde;
            WaardeVan = other.WaardeVan;
            WaardeNaar = other.WaardeNaar;
        }

        public DateTime DatumTijd { get; set; }
        public string ActieNemer { get; set; }
        public string Actie { get; set; }
        public string Tabel { get; set; }
        public string Veld { get; set; }
        public string SleutelWaarde { get; set; }
        public string WaardeVan { get; set; }
        public string WaardeNaar { get; set; }

        #region Voor repository

        public string Tablename { get; set; }

        public int Id { get; set; }

        public string DataKeyValue { get; set; }

        public string DataKeyName { get; set; }
        public int SelectedIndex { get; set; }
        
        public IList<AuditItem> Details { get; set; }

        public int MasterId { get; set; }

        public string Master { get; set; }

        public ParentKeyEntity Parent { get; set; }

        #endregion

    }
}
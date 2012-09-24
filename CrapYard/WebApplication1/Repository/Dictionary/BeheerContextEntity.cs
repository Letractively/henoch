using System;
using System.Collections.Generic;

namespace Dictionary.BusinessObjects
{
    /// <summary>
    /// Representeert een business-entity voor Object-Relational Mapping.
    /// 
    /// Een business entity is een entity,
    /// waarvoor altijd een identificerende regel geldt (superkeys/unique identifiers).
    /// 
    /// Een business entity kenmerkt zich door verzameling van attributen en attribuutwaarden.
    /// </summary>
    public class BeheerContextEntity : IBeheerContextEntity
    {
        public BeheerContextEntity()
        {
            Attributes = new Dictionary<string, AttributeValue>();
            Details = new List<BeheerContextEntity>();
            Parent = new ParentKeyEntity();
            SelectedIndex = -1;//not selected
            MasterId = -1;
        }
        #region IBeheerContextEntity

        public string Tablename { get; set; }

        public int Id { get; set; }
        public string DataKeyValue { get; set; }
        public string DataKeyName { get; set; }

        public int SelectedIndex { get; set; }

        public IList<BeheerContextEntity> Details { get; set; }
        public string Master { get; set; }
        public int MasterId { get; set; }

        public ParentKeyEntity Parent { get; set; }

        //TODO: wijzig de properties eerst in IBeheerContextEntity voor andere Context.
        #endregion


        #region IAttributes: lijst van Attributen en hun waarde.

        public IDictionary<string, AttributeValue> Attributes { get; set; }
        #endregion


    }

    public class AttributeValue
    {
        public ValueType ValueType { get; set; }
        //String is een reference type
        public string ValueStringType { get; set; }

    }
}
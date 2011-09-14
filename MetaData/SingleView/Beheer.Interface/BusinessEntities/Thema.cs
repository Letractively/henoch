using System;
using System.Collections.Generic;

namespace MetaData.Beheer.Interface.BusinessEntities
{
    /// <summary>
    /// Obsolete: gebruik BeheerContextEntity ipv Thema.
    /// </summary>
    public class Thema : BeheerContextEntity, IThema
    {
        public string ThemaNaam
        {
            get
            {
                return DataKeyName;
            }
            set
            {
                DataKeyName = value;   
            }
        }

        public DateTime DateTime { get; set; }

        public ValueType Created { get; set; }
        public IDictionary<string, AttributeValue> Attributes { get; set; }
    }
}
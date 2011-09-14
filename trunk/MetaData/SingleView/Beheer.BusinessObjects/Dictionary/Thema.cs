using System;
using System.Collections.Generic;

namespace Beheer.BusinessObjects.Dictionary
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
                return DataKeyValue;
            }
            set
            {
                DataKeyValue = value;   
            }
        }

        public DateTime DateTime { get; set; }

        public ValueType Created { get; set; }
        public IDictionary<string, AttributeValue> Attributes { get; set; }
    }
}
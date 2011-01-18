using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DataResource.DesignPatterns
{
    /// <summary>
    /// Collection of [filename,filepath]
    /// </summary>
    public class CollectionBronPaden : KeyedCollection<string, BronBestand>
    {
        #region Overrides of KeyedCollection<string,BronBestand>

        protected override string GetKeyForItem(BronBestand item)
        {
           return item.Naam;
        }

        #endregion
    }    
    public class BronBestand
    {        
        /// <summary>
        /// Shortname file.
        /// </summary>
        public string Naam { get; set; }
        /// <summary>
        /// path
        /// </summary>
        public string Pad { get; set; }
    }
}

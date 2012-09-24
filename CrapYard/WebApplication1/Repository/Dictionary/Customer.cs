using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dictionary.BusinessObjects
{
    public class Customer : IBusinessObject<string, object>
    {
        public int Prio { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public string Name { get; set; }
        public string VoorNaam { get; set; }
        public string TussenVoegsel { get; set; }
        public string AchterNaam { get; set; }


    }
}

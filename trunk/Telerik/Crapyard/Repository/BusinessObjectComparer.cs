using System;
using System.Collections.Generic;
using System.Reflection;

namespace Repository
{
    public class BusinessObjectComparer : IComparer<BusinessObject>
    {
        private string _comparePropertyName;

        public BusinessObjectComparer()
            : this(String.Empty)
        {

        }

        public BusinessObjectComparer(string comparePropertyName)
        {
            _comparePropertyName = comparePropertyName;
        }

        #region IComparer<BusinessObject> Members

        public int Compare(BusinessObject x, BusinessObject y)
        {
            if (String.IsNullOrEmpty(_comparePropertyName))
            {
                return x.ID.CompareTo(y.ID);
            }
            PropertyInfo property = x.GetType().GetProperty(_comparePropertyName);
            return ((IComparable)property.GetValue(x, null)).CompareTo(property.GetValue(y, null));
        }

        #endregion
    }
}
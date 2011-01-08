using System;
using System.Collections.Generic;
using System.Reflection;

namespace Repository
{
    public class BusinessObjectEqualityComparer : IEqualityComparer<BusinessObject>
    {
        private string _comparePropertyName;

        public BusinessObjectEqualityComparer()
            : this(String.Empty)
        {

        }

        public BusinessObjectEqualityComparer(string comparePropertyName)
        {
            _comparePropertyName = comparePropertyName;
        }

        #region IEqualityComparer<BusinessObject> Members

        public bool Equals(BusinessObject x, BusinessObject y)
        {
            if (String.IsNullOrEmpty(_comparePropertyName))
            {
                return x.ID.Equals(y.ID);
            }

            PropertyInfo property = x.GetType().GetProperty(_comparePropertyName);
            return property.GetValue(x, null).Equals(property.GetValue(y, null));
        }

        public int GetHashCode(BusinessObject obj)
        {
            if (String.IsNullOrEmpty(_comparePropertyName))
            {
                return obj.ID.GetHashCode();
            }

            PropertyInfo property = obj.GetType().GetProperty(_comparePropertyName);

            return property.GetValue(obj, null).GetHashCode();
        }

        #endregion
    }
}
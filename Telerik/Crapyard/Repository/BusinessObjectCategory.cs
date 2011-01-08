using System;

namespace Repository
{
    public class BusinessObjectCategory : IComparable<BusinessObjectCategory>
    {
        private int _id;
        private string _name;
        private string _description;
        private BusinessObjectCollection _items;

        private static int _nextId = 1;

        public BusinessObjectCategory()
            : this(_nextId++)
        {

        }

        public BusinessObjectCategory(int id)
        {
            _id = id;
            _name = "Business Object Category: " + id.ToString();
            _description = "Description for Business Object Category: " + id.ToString();
            _items = new BusinessObjectCollection();
        }

        public int ID
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [System.Web.Script.Serialization.ScriptIgnore]
        public BusinessObjectCollection Items
        {
            get { return _items; }
        }

        #region IComparable<BusinessObjectCategory> Members

        public int CompareTo(BusinessObjectCategory other)
        {
            return this.ID.CompareTo(other.ID);
        }

        #endregion
    }
}
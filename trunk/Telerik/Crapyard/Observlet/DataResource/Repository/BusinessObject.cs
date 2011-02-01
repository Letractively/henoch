using System;

namespace DataResource.Repository
{
    #region The Repository

    public class BusinessObject : IComparable
    {
        private int _id;
        private string _name;
        private int? _categoryId;
        private BusinessObjectCategory _category;
        private DateTime _date;
        private int _quantity;
        private double _price;
        private bool _available;
        private int? _parentId;

        private static int _nextId;

        static Random rand = new Random();

        public BusinessObject()
            : this(_nextId++)
        {
        }

        public BusinessObject(int id)
            : this(id, new BusinessObjectCategory(0))
        {

        }

        public BusinessObject(int id, BusinessObjectCategory category)
        {
            _id = id;
            _name = "Business object ID: " + _id.ToString();
            _categoryId = category.ID;
            _category = category;
            _date = DateTime.Today.AddDays(rand.Next(20) - 10).AddHours(rand.Next(24) - 12).AddMinutes(rand.Next(60) - 30);
            _quantity = rand.Next(100);
            _price = Math.Round(rand.NextDouble() * 100, 2);
            _available = rand.Next(1000) % 2 == 0;
            _parentId = null;
        }

        public override bool Equals(object obj)
        {
            return ((BusinessObject)obj).ID.Equals(this.ID);
        }

        public int ID
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int? CategoryID
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public BusinessObjectCategory Category
        {
            get { return _category; }
            set
            {
                _category = value;
                if (value != null)
                {
                    _categoryId = value.ID;
                }
                else
                {
                    _categoryId = null;
                }
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }

        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }

        public bool Available
        {
            get
            {
                return _available;
            }
            set
            {
                _available = value;
            }
        }

        public int? ParentID
        {
            get
            {
                return _parentId;
            }
            set
            {
                _parentId = value;
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return ((BusinessObject)obj).ID.CompareTo(this.ID);
        }

        #endregion
    }

    #region Comparers

    #endregion
    #endregion
}

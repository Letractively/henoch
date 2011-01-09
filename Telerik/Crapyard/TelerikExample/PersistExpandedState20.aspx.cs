using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repository;
using Telerik.Web.UI;
using TelerikExample.Account;
using System.Reflection;
using System.Collections.Generic;

public partial class Tickets_327691_Default : System.Web.UI.Page
{

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid1.DataSource = BusinessDataStorage.GetCategories();
    }

    protected void RadGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        if (e.DetailTableView.Name == "RelatedItems")
        {
            int itemId = (int)e.DetailTableView.ParentItem.GetDataKeyValue("ID");
            e.DetailTableView.DataSource = BusinessDataStorage.GetHierarchicalData(itemId);
        }
        else if (e.DetailTableView.Name == "InnerMost")
        {
            int catId = (int)e.DetailTableView.ParentItem.GetDataKeyValue("CategoryID");
            e.DetailTableView.DataSource = BusinessDataStorage.GetData(catId);
        }
        else
        {
            int catId = (int)e.DetailTableView.ParentItem.GetDataKeyValue("ID");
            e.DetailTableView.DataSource = BusinessDataStorage.GetData(catId);
        }
    }

    protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            //Attach to each GridTableView's Load event to save the expanded item state
            e.Item.OwnerTableView.Load += new EventHandler(GridTableView_Load);

            //Attach to each GridTableView's Load event to load the expanded item state
            e.Item.OwnerTableView.DataBound += new EventHandler(GridTableView_DataBound);
        }
    }

    void GridTableView_Load(object sender, EventArgs e)
    {
        GridTableView table = (GridTableView)sender;
        foreach (GridDataItem item in table.Items)
        {
            if (item.Expanded)
            {
                //build the item key that will be used to save the item as expanded
                string itemKey = BuildItemKey(item);
                if (!ExpandedItemKeys.Contains(itemKey))
                {
                    ExpandedItemKeys.Add(itemKey);
                }
            }
        }
    }

    void GridTableView_DataBound(object sender, EventArgs e)
    {
        GridTableView table = (GridTableView)sender;
        
        foreach (GridDataItem item in table.Items)
        {
            string itemKey = BuildItemKey(item);
            //if the item key is contained in the collection of saved items
            //this means the item should be expanded
            if (!item.Expanded && ExpandedItemKeys.Contains(itemKey))
            {
                item.Expanded = true;
                ExpandedItemKeys.Remove(itemKey);
            }
        }
        
    }

    /// <summary>
    /// Build an item key that will uniquely identify a grid item
    /// among all the items in the RadGrid hierarchy. Use a combination 
    /// of the OwnerTableView.UniqueID and the set of all data values.
    /// </summary>
    protected string BuildItemKey(GridDataItem item)
    {
        string[] keyNames = item.OwnerTableView.DataKeyNames;
        if (keyNames.Length == 0) return item.ItemIndexHierarchical;

        string returnKey = item.OwnerTableView.UniqueID + "::";

        foreach (string keyName in keyNames)
        {
            returnKey += item.GetDataKeyValue(keyName).ToString();

            if (keyName != keyNames[keyNames.Length - 1])
            {
                returnKey += "::";
            }
        }

        return returnKey;
    }

    /// <summary>
    /// Gets the expanded item state saved in the Session
    /// </summary>
    protected List<string> ExpandedItemKeys
    {
        get
        {
            if (Session["ExpandedItemKeys"] == null)
            {
                Session["ExpandedItemKeys"] = new List<string>();
            }

            return (List<string>)Session["ExpandedItemKeys"];
        }
    }


    #region The Repository
    //public static class BusinessDataStorage
    //{
    //    private static BusinessObjectCategoryCollection _categories;
    //    private static BusinessObjectCollection _data;
    //    private const int DEFAULT_ITEM_COUNT = 30;
    //    private const int DEFAULT_CATEGORY_COUNT = 7;

    //    public static void Create()
    //    {
    //        _categories = new BusinessObjectCategoryCollection(DEFAULT_CATEGORY_COUNT);
    //        _data = new BusinessObjectCollection(DEFAULT_ITEM_COUNT);
    //    }

    //    public static void Create(int objectCount, int categoryCount)
    //    {
    //        _categories = new BusinessObjectCategoryCollection(categoryCount);
    //        _data = new BusinessObjectCollection(objectCount, _categories);
    //        BuildSelfHierarchy();
    //    }

    //    public static BusinessObjectCollection GetData()
    //    {
    //        if (_data == null)
    //        {
    //            Create(DEFAULT_ITEM_COUNT, DEFAULT_CATEGORY_COUNT);
    //        }

    //        return _data;
    //    }

    //    public static BusinessObjectCollection GetData(int categoryId)
    //    {
    //        List<BusinessObject> result = GetData().FindAll(delegate(BusinessObject obj) { return obj.CategoryID == categoryId; });
    //        BusinessObjectCollection ret = new BusinessObjectCollection();
    //        ret.Clear();
    //        ret.AddRange(result);

    //        return ret;
    //    }

    //    public static BusinessObjectCollection GetHierarchicalData(int? parentId)
    //    {
    //        List<BusinessObject> result = GetData().FindAll(delegate(BusinessObject obj) { return obj.ParentID == parentId; });
    //        BusinessObjectCollection ret = new BusinessObjectCollection();
    //        ret.Clear();
    //        ret.AddRange(result);

    //        return ret;
    //    }

    //    public static void BuildSelfHierarchy()
    //    {
    //        BuildSelfHierarchy(DEFAULT_ITEM_COUNT / 4);
    //    }

    //    public static void BuildSelfHierarchy(int masterItemCount)
    //    {
    //        List<BusinessObject> data = GetData();
    //        int zeroLevelItemCount = Math.Max(0, Math.Min(masterItemCount, data.Count));

    //        for (int i = zeroLevelItemCount; i < data.Count; i++)
    //        {
    //            data[i].ParentID = data.Count % i;
    //        }
    //    }

    //    public static int GetCount()
    //    {
    //        return GetData().Count;
    //    }

    //    public static int GetCategoryCount()
    //    {
    //        return GetCategories().Count;
    //    }

    //    public static BusinessObjectCategoryCollection GetCategories()
    //    {
    //        if (_categories == null)
    //        {
    //            _categories = new BusinessObjectCategoryCollection(DEFAULT_CATEGORY_COUNT, GetData());
    //        }

    //        return _categories;
    //    }

    //    public static int GetNextID()
    //    {
    //        return _data[_data.Count - 1].ID + 1;
    //    }

    //    public static BusinessObject Find(int id)
    //    {
    //        if (_data.Count > id && _data[id].ID == id)
    //        {
    //            return _data[id];
    //        }

    //        foreach (BusinessObject obj in _data)
    //        {
    //            if (obj.ID == id)
    //            {
    //                return obj;
    //            }
    //        }

    //        return null;
    //    }

    //    public static void Insert(BusinessObject obj)
    //    {
    //        BusinessObject insertedObj = new BusinessObject(GetNextID()); ;


    //        PropertyInfo[] properties = obj.GetType().GetProperties();
    //        foreach (PropertyInfo prop in properties)
    //        {
    //            if (prop.Name != "ID")
    //            {
    //                insertedObj.GetType().GetProperty(prop.Name).SetValue(insertedObj, prop.GetValue(obj, null), null);
    //            }
    //        }

    //        _data.Add(insertedObj);
    //    }

    //    public static void Update(BusinessObject obj)
    //    {
    //        if (Find(obj.ID) == null)
    //        {
    //            throw new NullReferenceException("Object with the specified ID does not exist in store");
    //        }

    //        _data[_data.BinarySearch(obj)] = obj;
    //    }

    //    public static void Delete(int id)
    //    {
    //        BusinessObject deletedObj = Find(id);
    //        if (deletedObj == null)
    //        {
    //            throw new NullReferenceException("Object with the specified ID does not exist in store");
    //        }

    //        _data.Remove(deletedObj);
    //    }
    //}

    //public class BusinessObjectCollection : List<BusinessObject>
    //{
    //    public BusinessObjectCollection()
    //        : this(0, null)
    //    {

    //    }

    //    public BusinessObjectCollection(int itemCount)
    //        : this(itemCount, null)
    //    {

    //    }

    //    public BusinessObjectCollection(int itemCount, BusinessObjectCategoryCollection categories)
    //    {
    //        if (categories != null)
    //        {
    //            Random rand = new Random();

    //            for (int i = 0; i < itemCount; i++)
    //            {
    //                BusinessObjectCategory category = categories[rand.Next(categories.Count)];
    //                BusinessObject obj = new BusinessObject(i, category);
    //                if (!category.Items.Contains(obj))
    //                {
    //                    category.Items.Add(obj);
    //                }

    //                this.Add(obj);
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < itemCount; i++)
    //            {
    //                this.Add(new BusinessObject(i));
    //            }
    //        }
    //    }
    //}

    //public class BusinessObjectCategoryCollection : List<BusinessObjectCategory>
    //{
    //    public BusinessObjectCategoryCollection()
    //        : this(0, null)
    //    {

    //    }

    //    public BusinessObjectCategoryCollection(int itemCount)
    //        : this(itemCount, null)
    //    {

    //    }

    //    public BusinessObjectCategoryCollection(int itemCount, BusinessObjectCollection dataInCategories)
    //    {
    //        CreateCollection(itemCount);

    //        if (dataInCategories != null)
    //        {
    //            Random rand = new Random();

    //            foreach (BusinessObject obj in dataInCategories)
    //            {
    //                BusinessObjectCategory category = this[rand.Next(this.Count)];
    //                obj.Category = category;
    //                obj.CategoryID = category.ID;

    //                if (!category.Items.Contains(obj))
    //                {
    //                    category.Items.Add(obj);
    //                }
    //            }
    //        }
    //    }

    //    private void CreateCollection(int count)
    //    {
    //        for (int i = 0; i < count; i++)
    //        {
    //            this.Add(new BusinessObjectCategory(i + 1));
    //        }
    //    }
    //}

    //public class BusinessObjectCategory : IComparable<BusinessObjectCategory>
    //{
    //    private int _id;
    //    private string _name;
    //    private string _description;
    //    private BusinessObjectCollection _items;

    //    private static int _nextId = 1;

    //    public BusinessObjectCategory()
    //        : this(_nextId++)
    //    {

    //    }

    //    public BusinessObjectCategory(int id)
    //    {
    //        _id = id;
    //        _name = "Business Object Category: " + id.ToString();
    //        _description = "Description for Business Object Category: " + id.ToString();
    //        _items = new BusinessObjectCollection();
    //    }

    //    public int ID
    //    {
    //        get { return _id; }
    //    }

    //    public string Name
    //    {
    //        get { return _name; }
    //        set { _name = value; }
    //    }

    //    public string Description
    //    {
    //        get { return _description; }
    //        set { _description = value; }
    //    }

    //    [System.Web.Script.Serialization.ScriptIgnore]
    //    public BusinessObjectCollection Items
    //    {
    //        get { return _items; }
    //    }

    //    #region IComparable<BusinessObjectCategory> Members

    //    public int CompareTo(BusinessObjectCategory other)
    //    {
    //        return this.ID.CompareTo(other.ID);
    //    }

    //    #endregion
    //}

    //public class BusinessObject : IComparable
    //{
    //    private int _id;
    //    private string _name;
    //    private int? _categoryId;
    //    private BusinessObjectCategory _category;
    //    private DateTime _date;
    //    private int _quantity;
    //    private double _price;
    //    private bool _available;
    //    private int? _parentId;

    //    private static int _nextId;

    //    static Random rand = new Random();

    //    public BusinessObject()
    //        : this(_nextId++)
    //    {
    //    }

    //    public BusinessObject(int id)
    //        : this(id, new BusinessObjectCategory(0))
    //    {

    //    }

    //    public BusinessObject(int id, BusinessObjectCategory category)
    //    {
    //        _id = id;
    //        _name = "Business object ID: " + _id.ToString();
    //        _categoryId = category.ID;
    //        _category = category;
    //        _date = DateTime.Today.AddDays(rand.Next(20) - 10).AddHours(rand.Next(24) - 12).AddMinutes(rand.Next(60) - 30);
    //        _quantity = rand.Next(100);
    //        _price = Math.Round(rand.NextDouble() * 100, 2);
    //        _available = rand.Next(1000) % 2 == 0;
    //        _parentId = null;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return ((BusinessObject)obj).ID.Equals(this.ID);
    //    }

    //    public int ID
    //    {
    //        get
    //        {
    //            return _id;
    //        }
    //    }

    //    public string Name
    //    {
    //        get
    //        {
    //            return _name;
    //        }
    //        set
    //        {
    //            _name = value;
    //        }
    //    }

    //    public int? CategoryID
    //    {
    //        get { return _categoryId; }
    //        set { _categoryId = value; }
    //    }

    //    public BusinessObjectCategory Category
    //    {
    //        get { return _category; }
    //        set
    //        {
    //            _category = value;
    //            if (value != null)
    //            {
    //                _categoryId = value.ID;
    //            }
    //            else
    //            {
    //                _categoryId = null;
    //            }
    //        }
    //    }

    //    public DateTime Date
    //    {
    //        get
    //        {
    //            return _date;
    //        }
    //        set
    //        {
    //            _date = value;
    //        }
    //    }

    //    public int Quantity
    //    {
    //        get
    //        {
    //            return _quantity;
    //        }
    //        set
    //        {
    //            _quantity = value;
    //        }
    //    }

    //    public double Price
    //    {
    //        get
    //        {
    //            return _price;
    //        }
    //        set
    //        {
    //            _price = value;
    //        }
    //    }

    //    public bool Available
    //    {
    //        get
    //        {
    //            return _available;
    //        }
    //        set
    //        {
    //            _available = value;
    //        }
    //    }

    //    public int? ParentID
    //    {
    //        get
    //        {
    //            return _parentId;
    //        }
    //        set
    //        {
    //            _parentId = value;
    //        }
    //    }

    //    #region IComparable Members

    //    public int CompareTo(object obj)
    //    {
    //        return ((BusinessObject)obj).ID.CompareTo(this.ID);
    //    }

    //    #endregion
    //}

    //#region Comparers

    //public class BusinessObjectEqualityComparer : IEqualityComparer<BusinessObject>
    //{
    //    private string _comparePropertyName;

    //    public BusinessObjectEqualityComparer()
    //        : this(String.Empty)
    //    {

    //    }

    //    public BusinessObjectEqualityComparer(string comparePropertyName)
    //    {
    //        _comparePropertyName = comparePropertyName;
    //    }

    //    #region IEqualityComparer<BusinessObject> Members

    //    public bool Equals(BusinessObject x, BusinessObject y)
    //    {
    //        if (String.IsNullOrEmpty(_comparePropertyName))
    //        {
    //            return x.ID.Equals(y.ID);
    //        }

    //        PropertyInfo property = x.GetType().GetProperty(_comparePropertyName);
    //        return property.GetValue(x, null).Equals(property.GetValue(y, null));
    //    }

    //    public int GetHashCode(BusinessObject obj)
    //    {
    //        if (String.IsNullOrEmpty(_comparePropertyName))
    //        {
    //            return obj.ID.GetHashCode();
    //        }

    //        PropertyInfo property = obj.GetType().GetProperty(_comparePropertyName);

    //        return property.GetValue(obj, null).GetHashCode();
    //    }

    //    #endregion
    //}

    //public class BusinessObjectComparer : IComparer<BusinessObject>
    //{
    //    private string _comparePropertyName;

    //    public BusinessObjectComparer()
    //        : this(String.Empty)
    //    {

    //    }

    //    public BusinessObjectComparer(string comparePropertyName)
    //    {
    //        _comparePropertyName = comparePropertyName;
    //    }

    //    #region IComparer<BusinessObject> Members

    //    public int Compare(BusinessObject x, BusinessObject y)
    //    {
    //        if (String.IsNullOrEmpty(_comparePropertyName))
    //        {
    //            return x.ID.CompareTo(y.ID);
    //        }
    //        PropertyInfo property = x.GetType().GetProperty(_comparePropertyName);
    //        return ((IComparable)property.GetValue(x, null)).CompareTo(property.GetValue(y, null));
    //    }

    //    #endregion
    //}

    //#endregion
    #endregion

}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Repository
{
    public static class BusinessDataStorage
    {
        private static BusinessObjectCategoryCollection _categories;
        private static BusinessObjectCollection _data;
        private const int DEFAULT_ITEM_COUNT = 30;
        private const int DEFAULT_CATEGORY_COUNT = 7;

        public static void Create()
        {
            _categories = new BusinessObjectCategoryCollection(DEFAULT_CATEGORY_COUNT);
            _data = new BusinessObjectCollection(DEFAULT_ITEM_COUNT);
        }

        public static void Create(int objectCount, int categoryCount)
        {
            _categories = new BusinessObjectCategoryCollection(categoryCount);
            _data = new BusinessObjectCollection(objectCount, _categories);
            BuildSelfHierarchy();
        }

        public static BusinessObjectCollection GetData()
        {
            if (_data == null)
            {
                Create(DEFAULT_ITEM_COUNT, DEFAULT_CATEGORY_COUNT);
            }

            return _data;
        }

        public static BusinessObjectCollection GetData(int categoryId)
        {
            List<BusinessObject> result = GetData().FindAll(delegate(BusinessObject obj) { return obj.CategoryID == categoryId; });
            BusinessObjectCollection ret = new BusinessObjectCollection();
            ret.Clear();
            ret.AddRange(result);

            return ret;
        }

        public static BusinessObjectCollection GetHierarchicalData(int? parentId)
        {
            List<BusinessObject> result = GetData().FindAll(delegate(BusinessObject obj) { return obj.ParentID == parentId; });
            BusinessObjectCollection ret = new BusinessObjectCollection();
            ret.Clear();
            ret.AddRange(result);

            return ret;
        }

        public static void BuildSelfHierarchy()
        {
            BuildSelfHierarchy(DEFAULT_ITEM_COUNT / 4);
        }

        public static void BuildSelfHierarchy(int masterItemCount)
        {
            List<BusinessObject> data = GetData();
            int zeroLevelItemCount = Math.Max(0, Math.Min(masterItemCount, data.Count));

            for (int i = zeroLevelItemCount; i < data.Count; i++)
            {
                data[i].ParentID = data.Count % i;
            }
        }

        public static int GetCount()
        {
            return GetData().Count;
        }

        public static int GetCategoryCount()
        {
            return GetCategories().Count;
        }

        public static BusinessObjectCategoryCollection GetCategories()
        {
            if (_categories == null)
            {
                _categories = new BusinessObjectCategoryCollection(DEFAULT_CATEGORY_COUNT, GetData());
            }

            return _categories;
        }

        public static int GetNextID()
        {
            return _data[_data.Count - 1].ID + 1;
        }

        public static BusinessObject Find(int id)
        {
            if (_data.Count > id && _data[id].ID == id)
            {
                return _data[id];
            }

            foreach (BusinessObject obj in _data)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }

            return null;
        }

        public static void Insert(BusinessObject obj)
        {
            BusinessObject insertedObj = new BusinessObject(GetNextID()); ;


            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.Name != "ID")
                {
                    insertedObj.GetType().GetProperty(prop.Name).SetValue(insertedObj, prop.GetValue(obj, null), null);
                }
            }

            _data.Add(insertedObj);
        }

        public static void Update(BusinessObject obj)
        {
            if (Find(obj.ID) == null)
            {
                throw new NullReferenceException("Object with the specified ID does not exist in store");
            }

            _data[_data.BinarySearch(obj)] = obj;
        }

        public static void Delete(int id)
        {
            BusinessObject deletedObj = Find(id);
            if (deletedObj == null)
            {
                throw new NullReferenceException("Object with the specified ID does not exist in store");
            }

            _data.Remove(deletedObj);
        }
    }
}
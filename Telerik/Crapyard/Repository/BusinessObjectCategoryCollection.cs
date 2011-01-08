using System;
using System.Collections.Generic;

namespace Repository
{
    public class BusinessObjectCategoryCollection : List<BusinessObjectCategory>
    {
        public BusinessObjectCategoryCollection()
            : this(0, null)
        {

        }

        public BusinessObjectCategoryCollection(int itemCount)
            : this(itemCount, null)
        {

        }

        public BusinessObjectCategoryCollection(int itemCount, BusinessObjectCollection dataInCategories)
        {
            CreateCollection(itemCount);

            if (dataInCategories != null)
            {
                Random rand = new Random();

                foreach (BusinessObject obj in dataInCategories)
                {
                    BusinessObjectCategory category = this[rand.Next(this.Count)];
                    obj.Category = category;
                    obj.CategoryID = category.ID;

                    if (!category.Items.Contains(obj))
                    {
                        category.Items.Add(obj);
                    }
                }
            }
        }

        private void CreateCollection(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Add(new BusinessObjectCategory(i + 1));
            }
        }
    }
}
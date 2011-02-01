using System;
using System.Collections.Generic;

namespace DataResource.Repository
{
    public class BusinessObjectCollection : List<BusinessObject>
    {
        public BusinessObjectCollection()
            : this(0, null)
        {

        }

        public BusinessObjectCollection(int itemCount)
            : this(itemCount, null)
        {

        }

        public BusinessObjectCollection(int itemCount, BusinessObjectCategoryCollection categories)
        {
            if (categories != null)
            {
                Random rand = new Random();

                for (int i = 0; i < itemCount; i++)
                {
                    BusinessObjectCategory category = categories[rand.Next(categories.Count)];
                    BusinessObject obj = new BusinessObject(i, category);
                    if (!category.Items.Contains(obj))
                    {
                        category.Items.Add(obj);
                    }

                    this.Add(obj);
                }
            }
            else
            {
                for (int i = 0; i < itemCount; i++)
                {
                    this.Add(new BusinessObject(i));
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AdventureWorks.DataAccessLayer
{
    public static class Products
    {
        public static List<ProductCategory>  GetCategories()
        {
            var db = new AdventureWorksEntities();
            var data = from o in db.ProductCategories orderby o.Name select o;
            return data.ToList();
         }

        public static string GetCategoryName(int ProductCategoryID)
        {
            var db = new AdventureWorksEntities();
            var data = from o in db.ProductCategories where o.ProductCategoryID == ProductCategoryID select o;
            if (data.Count() == 0) throw new Exception("Cannot find Product Category with ID " + ProductCategoryID.ToString());
            return data.FirstOrDefault().Name;
        }


        public static List<Product> GetProductsByCategory(int CategoryID)
        {
            

            var db = new AdventureWorksEntities();

            var data = from o in db.Products where o.ProductCategory.ProductCategoryID == CategoryID orderby o.Name select o;
            return data.ToList();
        }

        public static Product GetProduct(int ProductID)
        {
            var db = new AdventureWorksEntities();
            var data = from o in db.Products where o.ProductID == ProductID select o;
            if (data.Count() == 0) throw new Exception("Cannot find Product with ID " + ProductID.ToString());
            return data.FirstOrDefault();
        }
    }
}
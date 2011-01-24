using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace AdventureWorks
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ProductServiceSL
    {
        [OperationContract]
        public IEnumerable<ProductCategory> GetProductCategories()
        {

            return DataAccessLayer.Products.GetCategories();
        }

        [OperationContract]
        public IEnumerable<Product> GetProductsByCategory(int ProductCategoryID)
        {
            return DataAccessLayer.Products.GetProductsByCategory(ProductCategoryID);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks
{
    public class clsShoppingCart
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
    }
}
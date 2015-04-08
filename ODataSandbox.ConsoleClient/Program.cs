using Default;
using Microsoft.OData.Client;
using ODataSandbox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODataSandbox.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //var containerV3 = new NorthwindEntitiesV3.NorthwindEntities(new Uri("http://localhost:52799/odata"));
            var containerV4 = new Default.NorthwindEntities(new Uri("http://localhost.fiddler:53372/odata/"));

            var product = new Product();
            var category = new Category();

            product.ProductName = "Abdul Product";
            category.CategoryName = "Abdul Category";

            containerV4.AddObject("Products", product);
            containerV4.AddObject("Categories", category);
            containerV4.AddLink(category, "Products", product);

            containerV4.SaveChanges(SaveChangesOptions.BatchWithSingleChangeset);
            
            //containerV4.Products.ByKey(1).ExpensiveProducts().Execute();
            //containerV4.Products.ByKey(1).RetireProduct().GetValue();

            //var productV3 = containerV3.Products.First();
            //var productV4 = containerV4.Orders.OrderBy(p => p.OrderDate.Value).First();
        }
    }
}

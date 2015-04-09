using Default;
using Microsoft.OData.Client;
using ODataSandbox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using V3 = ODataSandbox.ConsoleClient.NorthwindEntitiesV3;
using V3Client = System.Data.Services.Client;

namespace ODataSandbox.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestV3Batch();
            TestV4Batch();
        }

        private static void TestV4Batch()
        {
            var containerV4 = new Default.NorthwindEntities(new Uri("http://localhost:53372/odata/"));

            var product = new Product();
            var category = new Category();

            product.ProductName = "Abdul Product";
            category.CategoryName = "Abdul Category";

            containerV4.AddObject("Products", product);
            containerV4.AddObject("Categories", category);
            containerV4.AddLink(category, "Products", product);

            try
            {
                containerV4.SaveChanges(SaveChangesOptions.BatchWithSingleChangeset);
            }
            catch
            {
                // eat it all
            }

            //containerV4.Products.ByKey(1).ExpensiveProducts().GetValue();
            //containerV4.Products.ByKey(1).RetireProduct().GetValue();            
            //var productV4 = containerV4.Orders.OrderBy(p => p.OrderDate.Value).First();
        }

        private static void TestV3Batch()
        {
            var containerV3 = new NorthwindEntitiesV3.NorthwindEntities(new Uri("http://localhost.fiddler:52799/odata"));

            var product = new V3.Product();
            var category = new V3.Category();

            product.ProductName = "Abdul Product";
            category.CategoryName = "Abdul Category";

            containerV3.AddObject("Products", product);
            containerV3.AddObject("Categories", category);
            containerV3.AddLink(category, "Products", product);
            containerV3.Format.UseJson();

            try
            {
                containerV3.SaveChanges(V3Client.SaveChangesOptions.Batch);
            }
            catch
            {
                // eat it all
            }

            //var productV3 = containerV3.Products.First();
        }
    }
}

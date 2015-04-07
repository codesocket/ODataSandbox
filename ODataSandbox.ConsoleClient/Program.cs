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
            var containerV3 = new NorthwindEntitiesV3.NorthwindEntities(new Uri("http://localhost:52799/odata"));
            var containerV4 = new Default.NorthwindEntities(new Uri("http://localhost:53372/odata/"));

            containerV4.Products.ByKey(1).RateProduct(10, DateTimeOffset.UtcNow).Execute();

            containerV4.CreateQuery<Product>("figure-it-out").ByKey(1).ExpensiveProducts().ExecuteAsync();

            var productV3 = containerV3.Products.First();
            var productV4 = containerV4.Orders.OrderBy(p => p.OrderDate.Value).First();
        }
    }
}

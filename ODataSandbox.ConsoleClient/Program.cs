using System;
using System.Collections.Generic;
using System.Linq;
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

            var productV3 = containerV3.Products.First();
            var productV4 = containerV4.Products.First();
        }
    }
}

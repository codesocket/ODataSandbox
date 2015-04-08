using ODataSandbox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;

namespace ODataSandbox.Data.Controllers
{
    public class OrdersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get(ODataQueryOptions<Order> options)
        {
            return Ok(_contxt.Orders.AsQueryable());
        }

        private NorthwindEntities _contxt = new NorthwindEntities();

        protected override void Dispose(bool disposing)
        {
            _contxt.Dispose();
            base.Dispose(disposing);
        }
    }
}
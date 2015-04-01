using ODataSandbox.DataV3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;

namespace ODataSandbox.Data.Controllers
{
    public class ProductsController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_contxt.Products.AsQueryable());
        }

        private NorthwindEntities _contxt = new NorthwindEntities();

        protected override void Dispose(bool disposing)
        {
            _contxt.Dispose();
            base.Dispose(disposing);
        }
    }
}
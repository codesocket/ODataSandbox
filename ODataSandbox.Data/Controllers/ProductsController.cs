using ODataSandbox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Batch;

namespace ODataSandbox.Data.Controllers
{
    public class ProductsController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_contxt.Products.AsQueryable());
        }
        [EnableQuery]
        public async Task<IHttpActionResult> Get([FromODataUri]int key)
        {
            var entity = await this._contxt.Products.FindAsync(key);
            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            this.Request.GetODataContentId();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IHttpActionResult> RateProduct([FromODataUri] int key, ODataActionParameters parameters)
        {
            return await Task.FromResult(Ok());
        }

        private NorthwindEntities _contxt = new NorthwindEntities();

        protected override void Dispose(bool disposing)
        {
            _contxt.Dispose();
            base.Dispose(disposing);
        }
    }
}
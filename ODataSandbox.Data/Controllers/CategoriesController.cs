using ODataSandbox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Batch;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;


namespace ODataSandbox.Data.Controllers
{
    public class CategoriesController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_contxt.Categories.AsQueryable());
        }
        [EnableQuery]
        public async Task<IHttpActionResult> Get([FromODataUri]int key)
        {
            var entity = await this._contxt.Categories.FindAsync(key);
            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            this.Request.GetODataContentId();

            return Ok(entity);
        }

        public async Task<HttpResponseMessage> Post([FromBody]Category entity)
        {
            var response = Request.CreateResponse(HttpStatusCode.Created);

            response.Headers.Location = new Uri(Url.CreateODataLink(
                new EntitySetPathSegment("Categories"),
                new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(1, ODataVersion.V4))));

            return await Task.FromResult(response);
        }


        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
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
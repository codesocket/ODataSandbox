﻿using ODataSandbox.Data.Entities;
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

        public async Task<HttpResponseMessage> Post([FromBody]Product entity)
        {
            var response = Request.CreateResponse(HttpStatusCode.Created);

            response.Headers.Location = new Uri(Url.CreateODataLink(
                new EntitySetPathSegment("Products"),
                new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(1, ODataVersion.V4))));

            return await Task.FromResult(response);
        }

        [HttpGet]
        
        public async Task<IHttpActionResult> RateProduct([FromODataUri] int key, [FromODataUri]int rating, [FromODataUri]DateTimeOffset? dateRated)
        {
            
            return await Task.FromResult(Ok(key));
        }

        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            return await Task.FromResult(Ok());
        }

        public async Task Delete([FromODataUri]int key)
        {
            var entity = await this._contxt.Products.FindAsync(key);
            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            this._contxt.Products.Remove(entity);
            if (!this.Request.IsBatchRequest())
            {
                await this._contxt.SaveChangesAsync();
            }
        }

        private NorthwindEntities _contxt = new NorthwindEntities();

        protected override void Dispose(bool disposing)
        {
            _contxt.Dispose();
            base.Dispose(disposing);
        }
    }
}
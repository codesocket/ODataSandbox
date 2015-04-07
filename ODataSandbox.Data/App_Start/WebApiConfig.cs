using ODataSandbox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ODataSandbox.Data
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects
            };

            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));
            */

            config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects
            };

            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.ContainerName = "NorthwindEntities";
            builder.EntitySet<Employee>("Employees");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Order_Detail>("Order_Details");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Customer>("Cutromers");
            builder.EntitySet<CustomerDemographic>("CustomerDemographics");
            builder.EntitySet<Region>("Regions");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<Territory>("Territories");
            builder.EntitySet<Shipper>("Shippers");

            var actionConfig = builder.EntityType<Product>().Action("RateProduct");
            actionConfig.Parameter<int>("rating");
            actionConfig.Parameter<DateTime>("dateRated");


            //builder.EntityType<Product>().Function("RateProduct").ReturnsEntityViaEntitySetPath<Product>("http://localhost:53372/odata/Products");
            builder.EntityType<Product>().Function("DiscountProduct").ReturnsCollection<Product>();
            builder.EntityType<Product>().Function("RetireProduct").ReturnsFromEntitySet<Product>("Products");
            builder.Function("RateProduct").Returns<Product>();
            builder.EntityType<Product>().Function("ExpensiveProducts").ReturnsCollection<Product>();

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        }
    }

    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }
    }
}

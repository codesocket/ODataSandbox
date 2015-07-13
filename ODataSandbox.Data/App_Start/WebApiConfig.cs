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

            //config.SetTimeZoneInfo(

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
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All
            };

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            config.SetTimeZoneInfo(timeZoneInfo);

            var builder = new ODataConventionModelBuilder();

            builder.ContainerName = "NorthwindEntities";




            builder.EntitySet<Employee>("Employees");
            //builder.EntityType<Employee>().Ignore(e => e.ReportsTo);
            //builder.EntityType<Employee>().Ignore(e => e.Employees1);
            //builder.EntityType<Employee>().ComplexProperty(e => e.Manager);
            builder.EntityType<Employee>().HasKey(e => e.EmployeeID);
            //builder.EntityType<Employee>().HasKey(e => e.ReportsTo);

            
                        
            

            builder.EntitySet<Product>("Products");
            builder.EntitySet<Order_Detail>("Order_Details");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<CustomerDemographic>("CustomerDemographics");
            builder.EntitySet<Region>("Regions");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<Territory>("Territories");
            builder.EntitySet<Shipper>("Shippers");

            foreach (var p in builder.EntityType<Order>().Properties)
            {
                if (p.Name == "OrderDate")
                {

                }
            }

            var actionConfig = builder.EntityType<Product>().Function("RateProduct");
            actionConfig.Parameter<int>("rating");
            actionConfig.Parameter<DateTimeOffset?>("dateRated");
            actionConfig.Returns<int>();

            builder.EntityType<Product>().Function("DiscountProduct").ReturnsCollection<Product>();
            builder.EntityType<Product>().Function("RetireProduct").Returns<Product>();
            builder.EntityType<Product>().Function("ExpensiveProducts").Returns<Customer>();

            builder.OnModelCreating = b =>
                {
                    b.EntityType<Employee>().HasKey(e => e.EmployeeID);
                    var prop = b.EntityType<Order>().Properties.First(p => p.Name == "EmployeeID");
                    
                };

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel(),
                batchHandler: new AtomicODataBatchHandler(GlobalConfiguration.DefaultServer));
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

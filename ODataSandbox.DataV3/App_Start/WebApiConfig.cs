using ODataSandbox.DataV3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

namespace ODataSandbox.DataV3
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
            /*
            builder.EntityType<Product>().Function("fff").ReturnsFromEntitySet<Product>("Products");
            builder.EntityType<Customer>()
                .Action("").re
            */
            
            config.Routes.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        }
    }
}

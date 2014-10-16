using System.Web.Http;
using BohFoundation.WebApi.Filters;
using Newtonsoft.Json.Serialization;

namespace BohFoundation.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Not Tested.
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new RequireHttpsAttribute());
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new CheckModelForNullAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Values",
                routeTemplate: "api/values",
                defaults: new { controller = "values" });

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}

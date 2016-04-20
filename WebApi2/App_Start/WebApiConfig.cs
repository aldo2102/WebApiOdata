using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi2.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using WebApi2.Models.Formulario;


namespace WebApi2
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
             
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<pedido>("Pedido");
            builder.EntitySet<evento>("evento"); 
            builder.EntitySet<formulario>("formulario");
            builder.EntitySet<categoria>("categoria");
            builder.EntitySet<roteador>("roteador");
            builder.EntitySet<Grupo>("Grupo");
            builder.EntitySet<GrupoUsuario>("GrupoUsuario");
            builder.EntitySet<Perfil>("Perfil");
            builder.EntitySet<PerfilUsuario>("PerfilUsuario");
            builder.Function("GetSalesTaxRate").Returns<double>().Parameter<int>("PostalCode");
            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            
            
        }
    }
}

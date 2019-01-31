using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BeerCouponApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Allow all origins
            config.EnableCors();

            //Config routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "beerCouponsApi",
                routeTemplate: "bcapi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

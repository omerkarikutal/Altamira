using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helper.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwaggerConfigToServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(_ => _.SwaggerDoc("users", new OpenApiInfo
            {
                Title = "Users",
                Version = "v1"
            }));
        }
    }
}

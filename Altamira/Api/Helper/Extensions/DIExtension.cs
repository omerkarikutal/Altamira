using Business.Services;
using Core.Business;
using Core.DataAccess;
using DataAccess.Repositories.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helper.Extensions
{
    public static class DIExtension
    {
        public static void AddDIConfigToServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}

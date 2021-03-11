using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Helper.Extensions
{
    public static class JwtExtension
    {
        public static void AddJwtConfigToServices(this IServiceCollection services, IConfiguration configuration)
        {

            var audience = configuration.GetSection("Audience");
            string aa = audience["Secret"];
            string iss = audience["Iss"];
            var signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(aa));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = iss,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = signinKey
                    };
                });


            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = signinKey,
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});
        }
    }
}

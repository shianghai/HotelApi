using HotelApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace HotelApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<ApiUser, IdentityRole>(b => b.User.RequireUniqueEmail = true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);

            builder.AddEntityFrameworkStores<HotelDbContext>();
            
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtKey = Environment.GetEnvironmentVariable("KEY");
            var jwtOptions = configuration.GetSection("jwt");

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(j =>
                j.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.GetSection("Issuer").Value,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                }
            ) ;
        }

        public static void ConfigureExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(handler =>
            {
                handler.Run(async m =>
                {
                    m.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    m.Response.ContentType = "application/json";

                    var feature = m.Features.Get<IExceptionHandlerFeature>();

                    if(feature is not null)
                    {
                        Log.Error($"Something went wrong: {feature.Error}");
                        await m.Response.WriteAsync(new Error
                        {
                            Message = "Something Went Wrong Please try again Later",
                            StatusCode = m.Response.StatusCode,
                            Exception = feature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}

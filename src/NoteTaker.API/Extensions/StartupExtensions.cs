using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NoteTaker.Core.Models;
using NoteTaker.DAL;

namespace NoteTaker.API.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var securityConfiguration = configuration.GetSection("Security");
            services.Configure<Security>(securityConfiguration);

            var security = securityConfiguration.Get<Security>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(security.JwtSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var userInMemoryDatabase = configuration.GetSection("UseInMemoryDatabase").Get<bool>();

            if (userInMemoryDatabase)
            {
                return services.AddDbContext<NoteTakerContext>(options => options.UseInMemoryDatabase("NoteTaker"));
            }
            else
            {
                return services.AddDbContext<NoteTakerContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("NoteTaker.DAL")));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplictionServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,
            IConfiguration configuration)
        {
           services.AddDbContext<DataContext> (opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ITokenServices,TokenService>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

            return services;
        }
        
    }
}
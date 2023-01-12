using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Data;
using API.Helper;
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
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
            services.Configure<CloudinarySetting>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService,PhotoService>();
            

            return services;
        }
        
    }
}
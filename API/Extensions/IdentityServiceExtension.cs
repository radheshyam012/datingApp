using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection EntityServiceExtension(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>(opt=>{
                opt.Password.RequireNonAlphanumeric = false;
            }).AddRoles<AppRoles>()
            .AddRoleManager<RoleManager<AppRoles>>()
            .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options =>
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false

                }
            );
            services.AddAuthorization(opt=>
            {
                opt.AddPolicy("RequiredAdminRole",Policy => Policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole",Policy => Policy.RequireRole("Admin","Moderator"));
            });
        
       
            return services;
        }
        
    }
}
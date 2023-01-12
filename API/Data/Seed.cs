using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Data
{
    public class Seed
    {//
        public static async Task SeedUser(UserManager<AppUser> userManager, RoleManager<AppRoles> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRoles>
            {
                new AppRoles{Name="Member"},
                new AppRoles{Name="Admin"},
                new AppRoles{Name="Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "Admin"
            };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
             //var a = new List<string>{"Admin","Moderator"};
            // foreach (var item in a)
            // {
            //     await userManager.AddToRoleAsync(admin, item);
            // }
            // await userManager.AddToRolesAsync(admin,"a");
             await userManager.AddToRolesAsync(admin,new string[]{"Admin","Moderator"});

            

        }
    }
    }
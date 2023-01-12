using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser,AppRoles,int,
        IdentityUserClaim<int>,AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options):base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasMany(ur=>ur.UserRoles)
            .WithOne(u=>u.User)
            .HasForeignKey(ur=>ur.UserId)
            .IsRequired();

            builder.Entity<AppRoles>()
            .HasMany(ur=>ur.UserRoles)
            .WithOne(u=>u.Roles)
            .HasForeignKey(ur=>ur.RoleId)
            .IsRequired();
            
        }


    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UserAccount.Core.Enum;
using UserAccount.Entities.Models;

namespace UserAccount.Entities.DataAccess
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<State> States { get; set; }
        //public DbSet<LGA> LGA { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedDefaultData(builder);
            base.OnModelCreating(builder);
        }

        private void SeedDefaultData(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var AdminRoleId = new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210").ToString();
            var AdminUserId = new Guid("b8633e2d-a33b-45e6-8329-1958b3252bbd").ToString();

            var CustomerRoleId = new Guid("FE8D7501-6CB1-4D99-4C9F-08D9B6BE7D9E").ToString();
            var CustomerId = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();

            var adminRole = AppRoles.AdminRole;
            var customerRole = AppRoles.Customer;
            var stateId= new Guid("5E31DB4E-6E79-487E-4C9E-08D9B22E7D9E").ToString();
            var state2= new Guid("5331DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();

            var lgai = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B22E7D9E").ToString();
            var lgaii = new Guid("5331DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = AdminRoleId,
                Name = adminRole,
                NormalizedName = adminRole.ToUpper()
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = CustomerRoleId,
                Name =customerRole,
                NormalizedName = customerRole.ToUpper()
            });
            //builder.Entity<LGA>().HasData(new LGA
            //{
            //    Id = lgai,
            //    Name="Somolu"

            //});
            //builder.Entity<LGA>().HasData(new LGA
            //{
            //    Id = lgaii,
            //    Name = "Lagos Mainland"

            //});
            builder.Entity<State>().HasData(new State
            {
                Id = stateId,
                Name = "Lagos",
                LGA = "LAGOS Mainland"

            });

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = AdminUserId,
                UserName = "Admin@alat.com",
                FirstName = "Admin",
                LastName = "Admin",
                NormalizedUserName = "Admin@alat.com",
                Email = "Admin@alat.com",
                NormalizedEmail = "Admin@alat.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@2022"),
                StateId=stateId,
                Activated = true,
                PhoneNumberConfirmed=true
            });

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = CustomerId,
                UserName = "customer@alat.com",
                FirstName = "Customer",
                LastName = "Customer",
                NormalizedUserName = "Customer@alat.com",
                Email = "customer@alat.com",
                NormalizedEmail = "customer@alat.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Customer@2022"),
                StateId = stateId,
                Activated = true,
                PhoneNumberConfirmed=true
            });

            builder.Entity<IdentityUserRole<string>>().HasData(
           new IdentityUserRole<string>
           {
               RoleId = AdminRoleId,
               UserId = AdminUserId,
           });

            builder.Entity<IdentityUserRole<string>>().HasData(
       new IdentityUserRole<string>
       {
           RoleId = CustomerRoleId,
           UserId = CustomerId,
       });
        }
    }
}

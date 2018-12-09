using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracker.Models;

namespace VehicleTracker.Data
{
    public class DataSeeder
    {
        private const string AdminRoleName = "admin";
        private VehicleTrackerContext _context;

        public DataSeeder(VehicleTrackerContext context)
        {
            _context = context;
        }

        public async void SeedAdminUser()
        {
            var user = new User
            {
                UserName = "email@email.com",
                NormalizedUserName = "email@email.com",
                Email = "Email@email.com",
                NormalizedEmail = "email@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == AdminRoleName))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = AdminRoleName, NormalizedName = AdminRoleName });
            }

            if (!_context.Roles.Any(r => r.Name == "NormalUser"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "NormalUser", NormalizedName = "normaluser" });
            }

            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                user.PasswordHash = password.HashPassword(user, "ThispasswordIsHellaStr0ng");
                var userStore = new UserStore<User>(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, AdminRoleName);
            }

            await _context.SaveChangesAsync();
        }
    }
}

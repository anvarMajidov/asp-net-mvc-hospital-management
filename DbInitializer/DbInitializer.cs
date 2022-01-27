using System;
using System.Linq;
using HospitalService.Data;
using HospitalService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalService.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, 
                        RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try{
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception){}
            if(_db.Roles.Any(r => r.Name == Helper.Helper.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Main Admin"
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.Helper.Admin).GetAwaiter().GetResult();
        }
    }
}

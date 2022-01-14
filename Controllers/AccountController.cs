using hospitalservice.Data;
using Microsoft.AspNetCore.Mvc;

namespace HospitalService.Models.ViewModels
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
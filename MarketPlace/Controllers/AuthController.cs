using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager; 
        private readonly AppDBContext _db;

 

        public AuthController(UserManager<User> userManager, AppDBContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {

            ViewBag.user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            ViewBag.id = _userManager.GetUserId(HttpContext.User);
            ViewBag.fullUser = HttpContext.User;


            return View();
        }
    }
}

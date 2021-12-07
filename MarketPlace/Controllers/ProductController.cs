using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
namespace MarketPlace.Controllers
{

   


   
    public class ProductController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDBContext _db;

        public ProductController(UserManager<User> userManager, AppDBContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Product()
        {
            ViewBag.user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            return View();
        }
        public async Task<IActionResult> Search()
        {
            ViewBag.user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            return View();
        }
        public async Task<IActionResult> Cart()
        {
            ViewBag.user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            return View();
        }
    }
}

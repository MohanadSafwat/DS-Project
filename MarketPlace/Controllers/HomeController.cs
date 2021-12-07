using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager; 
        private readonly AppDBContext _db;

        public HomeController(ILogger<HomeController> logger , UserManager<User> userManager, AppDBContext db)
        {
            _logger = logger;
            _userManager = userManager; 
            _db = db;

        }

        public async Task<IActionResult> IndexAsync()
        {
            //ViewBag.id =  _userManager.GetUserId(HttpContext.User);
            ViewBag.user  = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            
                            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

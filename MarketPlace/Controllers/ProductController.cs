using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
    }
}

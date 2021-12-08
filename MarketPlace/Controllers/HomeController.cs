using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
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
        private readonly IAssociatedRepository<AssociatedSell> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared> associatedSharedRepository;
        private readonly IProductRepository<Product> productRepository;
        private readonly IAssociatedRepository<AssociatedBought> associatedBoughtRepository;


        public HomeController(ILogger<HomeController> logger ,
            UserManager<User> userManager, AppDBContext db,
            IAssociatedRepository<AssociatedSell> associatedSellRepository,
            IAssociatedRepository<AssociatedShared> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought> associtaedBoughtRepository,
            IProductRepository<Product> productRepository

            )
        {
            _logger = logger;
            _userManager = userManager; 
            _db = db;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.productRepository = productRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //ViewBag.id =  _userManager.GetUserId(HttpContext.User);
            ViewBag.user  = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));

            var model = new ProductViewModel {
            productsIndex = productRepository.List()
            };
            return View(model);
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

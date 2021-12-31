using JWTAuthentication.Authentication;
using MarketPlace.Areas.Identity.Data;
using MarketPlace.Dtos;
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
        private readonly UserManager<User2> _userManager2;
        private readonly AppDBContext _db;
        private readonly AppDB2Context _db2;
        private readonly IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository;
        private readonly IProductRepository<Product> productRepository;
        private readonly IAssociatedRepository<AssociatedBought,ProductBoughtReadDto> associatedBoughtRepository;
        private readonly IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository;
        private readonly IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository;


        public HomeController(ILogger<HomeController> logger ,
            UserManager<User> userManager, AppDBContext db,
            IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository,
            IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepository,
             IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository,
            IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository,
            IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository,
            IProductRepository<Product> productRepository, UserManager<User2> userManager2, AppDB2Context db2

            )
        {
            _logger = logger;
            _userManager = userManager; 
            _db = db;
            _userManager2 = userManager2;
            _db2 = db2;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.productRepository = productRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;
            this.southAssociatedSellRepository = southAssociatedSellRepository;
            this.southAssociatedSharedRepository = southAssociatedSharedRepository;
            this.southAssociatedBoughtRepository = southAssociatedBoughtRepository;
        
    }

        public async Task<IActionResult> IndexAsync()
        {
            //ViewBag.id =  _userManager.GetUserId(HttpContext.User);

            ViewBag.user  = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User)) ;
           
            var model = new ProductViewModel
            {
           
            };
            if (ViewBag.user != null)
            {
                ViewBag.userLocation = "North";
                model = new ProductViewModel
                {
                    productsIndex = associatedSellRepository.List(),
                     associatedSharedRepository = associatedSharedRepository
                 };
                return View(model);

            }
            else
            {
                ViewBag.user = await _userManager2.FindByIdAsync(_userManager2.GetUserId(HttpContext.User));

                if (ViewBag.user != null)
                {
                    ViewBag.userLocation = "South";

                    model = new ProductViewModel
                    {
                        productsIndexSouth = southAssociatedSellRepository.List(),
                        southAssociatedSharedRepository = southAssociatedSharedRepository

                    };
                    return View(model);
                }
                else {
                    ViewBag.userLocation = "South";
                    model = new ProductViewModel
                    {
                        productsIndexSouth = southAssociatedSellRepository.List(),
                        southAssociatedSharedRepository = southAssociatedSharedRepository
                    };
                    return View(model);
                 }
            }

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

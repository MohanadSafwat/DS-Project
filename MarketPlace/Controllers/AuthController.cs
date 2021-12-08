using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
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
        private readonly IAssociatedRepository<AssociatedSell> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought> associatedBoughtRepository;

        public AuthController(UserManager<User> userManager, AppDBContext db,IAssociatedRepository<AssociatedSell> associatedSellRepository, IAssociatedRepository<AssociatedShared> associatedSharedRepository, IAssociatedRepository<AssociatedBought> associtaedBoughtRepository)
        {
            _userManager = userManager;
            _db = db;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;

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


            List<AssociatedSell> associatedSellList = new List<AssociatedSell>() { };
            List<AssociatedShared> associatedSharedList = new List<AssociatedShared>() { };
            List<AssociatedBought> associatedBoughtList = new List<AssociatedBought>() { };

            var sell = associatedSellRepository.FindProducts(ViewBag.id);
            var shared = associatedSharedRepository.FindProducts(ViewBag.id);
            var model = new ProductViewModel{};
          
            if (associatedSharedRepository.FindProducts(ViewBag.id) != null)
            {
                model.associatedShared = associatedSharedRepository.FindProducts(ViewBag.id);
            }
            if (associatedSellRepository.FindProducts(ViewBag.id) != null)
            {
                model.associatedSell = associatedSellRepository.FindProducts(ViewBag.id);
            }

            return View(model);
        }

     
    }
}

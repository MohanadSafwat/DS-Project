using AspNetCore.Reporting;
using Dapper;
using JWTAuthentication.Authentication;
using JWTAuthentication.Models;
using MarketPlace.Areas.Identity.Data;
using MarketPlace.Dtos;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager; 
        private readonly UserManager<User2> _userManager2; 
        private readonly AppDBContext _db;
        private readonly AppDB2Context _db2;
        private readonly IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared,ProductSharedReadDto> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought,ProductBoughtReadDto> associatedBoughtRepository;
        private readonly IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository;
        private readonly IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository<Product> _productRepository;

        private readonly string _connectionString;
        


        public AuthController(IOptions<AppDbConnection> config,IProductRepository<Product> productRepository,IWebHostEnvironment webHostEnvironment, UserManager<User> userManager, AppDBContext db,  UserManager<User2> userManager2, AppDB2Context db2, IAssociatedRepository<AssociatedSell,ProductSellerReadDto> associatedSellRepository, IAssociatedRepository<AssociatedShared,ProductSharedReadDto> associatedSharedRepository, IAssociatedRepository<AssociatedBought,ProductBoughtReadDto> associatedBoughtRepository, IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository, IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository, IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository)
        {
            _userManager = userManager;
            _db = db;
            _userManager2 = userManager2;
            _db2 = db2;
            _connectionString = config.Value.ConnectionString;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;
            this.southAssociatedSellRepository = southAssociatedSellRepository;
            this.southAssociatedSharedRepository = southAssociatedSharedRepository;
            this.southAssociatedBoughtRepository = southAssociatedBoughtRepository;
            this._webHostEnvironment = webHostEnvironment;
            this._productRepository = productRepository;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


        }

        /*public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                System.Diagnostics.Debug.WriteLine(model.Amount);
                user.Amount += model.Amount;
                _db.SaveChanges();
                return Redirect("/Auth/Dashboard");
            }
            catch { return Redirect("/Auth/Dashboard"); }
        }
        public async Task<IActionResult> Dashboard()
        {

            ViewBag.user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            ViewBag.id = _userManager.GetUserId(HttpContext.User);
            ViewBag.fullUser = HttpContext.User;
            var model = new ProductViewModel
            {

            };
            if (ViewBag.user != null)
            {
                ViewBag.userLocation = "North";


                model.associatedShared = associatedSharedRepository.FindProducts(ViewBag.id);
                
              
                    model.associatedSell = associatedSellRepository.FindProducts(ViewBag.id);
           
                    model.associatedBought = associatedBoughtRepository.FindProducts(ViewBag.id);
                

                return View(model);

            }
            else
            {
                ViewBag.user = await _userManager2.FindByIdAsync(_userManager2.GetUserId(HttpContext.User));

                if (ViewBag.user != null)
                {

                    ViewBag.userLocation = "South";
                    model.associatedSharedSouth = southAssociatedSharedRepository.FindProducts(ViewBag.id);


                    model.associatedSellSouth = southAssociatedSellRepository.FindProducts(ViewBag.id);

                    model.associatedBoughtSouth = southAssociatedBoughtRepository.FindProducts(ViewBag.id);


                    return View(model);
                }
                else
                {
                    return View();
                }
            }

      

          
         
        }

        public async Task< IActionResult> ProductReport()
        {
            string mimType = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\products.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // parameters.Add("rp1", "Welcome!!");
            var products = await GetProducts();
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", products);
            
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimType);
            return File(result.MainStream, "application/pdf");

        }

        public async Task<IActionResult> TransactionReport()
        {
            string mimType = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\trans.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // parameters.Add("rp1", "Welcome!!");
            var trans = await GetTransactions();
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet2", trans);

            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimType);
            return File(result.MainStream, "application/pdf");

        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Product>("select ProductId,ProductName,ProductPrice,ProductBrand,ProductDescription from Products", commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Transaction>("select Id,CustomerEmail,SellerEmail,ItemName,itemPrice from Transactions", commandType: CommandType.Text);
            }
        }
    }
}

using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly IProductRepository<Product> productRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly IAssociatedRepository<AssociatedSell,ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared,ProductSharedReadDto> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought,ProductBoughtReadDto> associatedBoughtRepository;
        private ApplicationDbContext db;
        [Obsolete]
        public ProductController(IAssociatedRepository<AssociatedSell,ProductSellerReadDto> associatedSellRepository,
            IAssociatedRepository<AssociatedShared,ProductSharedReadDto> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought,ProductBoughtReadDto> associatedBoughtRepositor,
            IProductRepository<Product> productRepository,
            IHostingEnvironment hosting,
            SignInManager<User> signInManager, ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
            this.productRepository = productRepository;
            this.hosting = hosting;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepositor;//this updated
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public ActionResult GetAllProducts()
        {

            var productsIndex = associatedSellRepository;


            var model = new
            {
                productsIndex = associatedSellRepository.List(),

            };

            return Ok(model);
        }
        public async Task<User> UserReturn(string id)
        {
            return await userManager.FindByIdAsync(id);

        }

        [HttpPost]
        [Route("createProduct")]
        public async Task<ActionResult> createProduct([FromForm] ProductViewModel model)
        {
            try
            {
                string fileNames = string.Empty;
                if (model.Files != null)
                {
                    /*                    var file = model.Files;
                    */
                    foreach (var file in model.Files)
                    {
                        string upload = Path.Combine(hosting.WebRootPath, "images");
                        fileNames += (file.FileName);
                        fileNames += "`";
                        string fullPath = Path.Combine(upload, file.FileName);
                        file.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }
                }

                Product product = new Product
                {
                    ProductBrand = model.ProductBrand,
                    ProductDescription = model.ProductDescription,
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    ProductImageUrls = fileNames,
                };

                productRepository.Add(product);

                int id = product.ProductId;

                Task<User> Seller = UserReturn(model.SellerId);
                User userData = await Seller;

                AssociatedSell associatedSell = new AssociatedSell
                {
                    productId = product,
                    SellerId = userData,
                    Sold = false
                };

                associatedSellRepository.Add(associatedSell);

                return Ok(product);


            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error" }); }
        }

        [HttpGet("GetProductById/{id}")]
        public ActionResult GetProductById(int id)
        {
            var product = associatedSellRepository.Find(id);
            return Ok(product);
        }



    }
}

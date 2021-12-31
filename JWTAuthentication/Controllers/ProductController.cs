using AutoMapper;
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
        private readonly UserManager<User2> userManager2;

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly ApplicationDb2Context _db2;

        private readonly SignInManager<User> _signInManager;
        private readonly IProductRepository<Product> productRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepository;
        private readonly IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository;
        private readonly IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository;

        [Obsolete]
        public ProductController(
            IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository,
            IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepositor,
            IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository,
            IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository,
            IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository,
            IProductRepository<Product> productRepository,
            IHostingEnvironment hosting,
            SignInManager<User> signInManager,
            ApplicationDbContext db,
            ApplicationDb2Context db2,
            UserManager<User> userManager,
            UserManager<User2> userManager2,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.userManager2 = userManager2;
            this.roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
            _db2 = db2;
            this.productRepository = productRepository;
            this.hosting = hosting;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepositor;//this updated
            this.southAssociatedSellRepository = southAssociatedSellRepository;
            this.southAssociatedSharedRepository = southAssociatedSharedRepository;
            this.southAssociatedBoughtRepository = southAssociatedBoughtRepository;
        }

        [HttpGet("GetAllProducts/{Location}")]
        public ActionResult GetAllProducts(string Location)
        {
            if (Location == "North")
            {
                var productsIndex = associatedSellRepository.ListDtos();
                return Ok(productsIndex);

            }
            else if (Location == "South")
            {
                var productsIndex = southAssociatedSellRepository.ListDtos();
                return Ok(productsIndex);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error" });
            }
        }
        public async Task<User> UserReturnNorth(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User2> UserReturnSouth(string id)
        {
            return await userManager2.FindByIdAsync(id);
        }
        
        [HttpPost]
        [Route("createProduct")]
        [Obsolete]
        public async Task<ActionResult> createProduct([FromForm] ProductViewModel model)
        {
            try
            {
                string fileNames = string.Empty;
                if (model.Files != null)
                {
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


                Task<User> SellerNorth = UserReturnNorth(model.SellerId);
                User userDataNorth = await SellerNorth;

                if (userDataNorth != null)
                {
                    productRepository.Add(product, "North");

                    int id = product.ProductId;
                    AssociatedSell associatedSell = new AssociatedSell
                    {
                        productId = product,
                        SellerId = userDataNorth,
                        Sold = false
                    };
                    associatedSellRepository.Add(associatedSell);
                    return Ok(product);
                }

                else if (userDataNorth == null)
                {
                    Task<User2> SellerSouth = UserReturnSouth(model.SellerId);
                    User2 userDataSouth = await SellerSouth;

                    if (userDataSouth != null)
                    {
                        productRepository.Add(product, "South");

                        int id = product.ProductId;
                        AssociatedSellSouth associatedSell = new AssociatedSellSouth
                        {
                            productId = product,
                            SellerId = userDataSouth,
                            Sold = false
                        };
                        southAssociatedSellRepository.Add(associatedSell);

                    }
                }


                return Ok(product);


            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error" }); }
        }

        [HttpGet("GetProductById/{Location}/{id}")]
        public ActionResult GetProductById(string Location, int id)
        {
            if (Location == "North")
            {
                var product = associatedSellRepository.FindProductByIdDtos(id);
                return Ok(product);
            }
            else if (Location == "South")
            {
                var product = southAssociatedSellRepository.FindProductByIdDtos(id);
                return Ok(product);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error" });

            }

        }

        [HttpGet("GetUnSoldProductsBySellerId/{id}")]
        public async Task<ActionResult> GetUnSoldProductsBySellerId(string id)
        {
            Task<User> account = UserReturnNorth(id);
            User accountData = await account;
            if (accountData != null)
            {
                var products = associatedSellRepository.FindUnSoldProductsDtos(id);
                if (products.Count == 0)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no unsold products" });

                return Ok(products);
            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(id);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var products = southAssociatedSellRepository.FindUnSoldProductsDtos(id);
                    if (products.Count == 0)
                        return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no unsold products" });

                    return Ok(products);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }


        }

        [HttpGet("GetSoldProductsBySellerId/{id}")]
        public async Task<ActionResult> GetSoldProductsBySellerId(string id)
        {
            Task<User> account = UserReturnNorth(id);
            User accountData = await account;
            if (accountData != null)
            {
                var products = associatedSellRepository.FindSoldProductsDtos(id);
                if (products.Count == 0)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no sold products" });

                return Ok(products);
            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(id);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var products = southAssociatedSellRepository.FindSoldProductsDtos(id);
                    if (products.Count == 0)
                        return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no sold products" });

                    return Ok(products);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }

        }

        [HttpGet("GetUnSoldProductsBySharerId/{id}")]
        public async Task<ActionResult> GetUnSoldProductsBySharerId(string id)
        {
            Task<User> account = UserReturnNorth(id);
            User accountData = await account;
            if (accountData != null)
            {
                var products = associatedSharedRepository.FindUnSoldProductsDtos(id);
                if (products.Count == 0)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no unsold shared products" });

                return Ok(products);
            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(id);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var products = southAssociatedSharedRepository.FindUnSoldProductsDtos(id);
                    if (products.Count == 0)
                        return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no unsold shared products" });

                    return Ok(products);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }

        }

        [HttpGet("GetSoldProductsBySharerId/{id}")]
        public async Task<ActionResult> GetSoldProductsBySharerId(string id)
        {
            Task<User> account = UserReturnNorth(id);
            User accountData = await account;
            if (accountData != null)
            {
                var products = associatedSharedRepository.FindSoldProductsDtos(id);
                if (products.Count == 0)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no sold shared products" });

                return Ok(products);
            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(id);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var products = southAssociatedSharedRepository.FindSoldProductsDtos(id);
                    if (products.Count == 0)
                        return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User has no sold shared products" });

                    return Ok(products);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }
        }

        [HttpGet("GetProductsByBuyerId/{id}")]
        public async Task<ActionResult> GetProductsByBuyerId(string id)
        {
            Task<User> account = UserReturnNorth(id);
            User accountData = await account;
            if (accountData != null)
            {
                var products = associatedBoughtRepository.FindSoldProductsDtos(id);
                if (products.Count == 0)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User hasn't bought products" });

                return Ok(products);
            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(id);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var products = southAssociatedBoughtRepository.FindUnSoldProductsDtos(id);
                    if (products.Count == 0)
                        return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "User hasn't bought products" });

                    return Ok(products);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }
        }
        [HttpGet("IsUserShareThis/{accountId}/{productId}")]
        public async Task<ActionResult> IsUserShareThis(string accountId, int productId)
        {
            Task<User> account = UserReturnNorth(accountId);
            User accountData = await account;
            if (accountData != null)
            {

                var product = associatedSellRepository.FindProductByIdDtos(productId);
                if (product == null)
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a Product" });

                var result = associatedSharedRepository.IsUserShareThis(accountId, productId);
                return Ok(result);

            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(accountId);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var product = associatedSellRepository.FindProductByIdDtos(productId);
                    if (product == null)
                        return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a Product" });

                    var result = southAssociatedSharedRepository.IsUserShareThis(accountId, productId);
                    return Ok(result);

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }



        }
        [HttpGet("IsUserBuyThis/{accountId}/{productId}")]
        public async Task<ActionResult> IsUserBuyThis(string accountId, int productId)
        {
            Task<User> account = UserReturnNorth(accountId);
            User accountData = await account;
            if (accountData != null)
            {

                var product = associatedSellRepository.FindProductByIdDtos(productId);
                if (product == null)
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a Product" });

                var result = associatedBoughtRepository.IsUserBuyThis(accountId, productId);
                return Ok(result);

            }
            else
            {
                Task<User2> accountSouth = UserReturnSouth(accountId);
                User2 accountDataSouth = await accountSouth;
                if (accountDataSouth != null)
                {
                    var product = associatedSellRepository.FindProductByIdDtos(productId);
                    if (product == null)
                        return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a Product" });

                    var result = southAssociatedBoughtRepository.IsUserBuyThis(accountId, productId);
                    return Ok(result);

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "404", Message = "No Suach a User" });

                }
            }


        }

        [HttpGet("Search/{Location}/{term}")]
        public ActionResult Search(string Location, string term)
        {
            if (Location == "North")
            {
                var result = associatedSellRepository.SearchDtos(term);
                if (result == null)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "204", Message = "No Suach a Products" });
                return Ok(result);
            }
            else if (Location == "South")
            {
                var result = southAssociatedSellRepository.SearchDtos(term);
                if (result == null)
                    return StatusCode(StatusCodes.Status204NoContent, new Response { Status = "204", Message = "No Suach a Products" });
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error" });
            }

        }

        [HttpPut("{Location}/{id}")]
        [Route("Edit")]
        [Obsolete]
        public ActionResult EditProduct(string Location, int id, [FromForm] ProductViewModel model)
        {
            var product = productRepository.Find(id, Location);
            if (product == null)
            {
                return NotFound();
            }


            try
            {
                string fileNames = string.Empty;
                if (model.Files != null)
                {
                    foreach (var file in model.Files)
                    {
                        string upload = Path.Combine(hosting.WebRootPath, "images");
                        fileNames += file.FileName;
                        fileNames += '`';
                        string fullPath = Path.Combine(upload, file.FileName);
                        file.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }
                }

                if (fileNames == "")
                {
                    fileNames = model.ProductImagesUrl;
                }
                Product newProduct = new Product
                {
                    ProductId = model.ProductId,
                    ProductBrand = model.ProductBrand,
                    ProductDescription = model.ProductDescription,
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    ProductImageUrls = fileNames
                };



                productRepository.Edit(id, newProduct, Location);


                return StatusCode(StatusCodes.Status201Created, new Response { Status = "Edited" });


            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error" }); }
        }


    }
}

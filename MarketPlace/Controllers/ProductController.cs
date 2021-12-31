using JWTAuthentication.Authentication;
using MarketPlace.Areas.Identity.Data;
using MarketPlace.Dtos;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository<Product> productRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<User> userManager;
        private readonly UserManager<User2> userManager2;

        private readonly IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepository;

        private readonly IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository;
        private readonly IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository;
        private AppDBContext db;
        [Obsolete]
        public ProductController(IProductRepository<Product> productRepository,
            IHostingEnvironment hosting,
            UserManager<User> userManager,
            UserManager<User2> userManager2,

            IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository,
            IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepository,

            IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository,
            IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository,
            IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository

            )
        {
            this.productRepository = productRepository;
            this.hosting = hosting;
            this.userManager = userManager;
            this.userManager2 = userManager2;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;

            this.southAssociatedSellRepository = southAssociatedSellRepository;
            this.southAssociatedSharedRepository = southAssociatedSharedRepository;
            this.southAssociatedBoughtRepository = southAssociatedBoughtRepository;
        }

        public async Task<User> UserReturnNorth(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User2> UserReturnSouth(string id)
        {
            return await userManager2.FindByIdAsync(id);
        }
        public IActionResult Create()
        {
            var id = userManager.GetUserId(HttpContext.User);
            if (id != null)
                ViewBag.id = id;
            else {
                var id2 = userManager2.GetUserId(HttpContext.User);
                if (id2 != null)
                    ViewBag.id = id2;
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(ProductViewModel model)
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
                    return Redirect("/Auth/Dashboard");
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


                return Redirect("/Auth/Dashboard");


            }
            catch { return View(); }


        }



        public async Task<IActionResult> Edit(int id, string sellerId)
        {
            var Location = "";

            Task<User> SellerNorth = UserReturnNorth(sellerId);
            User userDataNorth = await SellerNorth;
            if (userDataNorth != null)
                Location = "North";

            Task<User2> SellerSouth = UserReturnSouth(sellerId);
            User userDataSouth = await SellerNorth;
            if (userDataSouth != null)
                Location = "South";

            var product = productRepository.Find(id, Location);
            var model = new ProductViewModel
            {
                ProductBrand = product.ProductBrand,
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
                ProductId = product.ProductId,
                ProductImagesUrl = product.ProductImageUrls,
                ProductPrice = product.ProductPrice,

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var Location = "";

            Task<User> SellerNorth = UserReturnNorth(model.SellerId);
            User userDataNorth = await SellerNorth;
            if (userDataNorth != null)
                Location = "North";

            Task<User2> SellerSouth = UserReturnSouth(model.SellerId);
            User2 userDataSouth = await SellerSouth;
            if (userDataSouth != null)
                Location = "South";

            var product = productRepository.Find(model.ProductId, Location);
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



                productRepository.Edit(model.ProductId, newProduct, Location);


                return Redirect("/Auth/Dashboard");


            }
            catch { return View(); }
        }

    



        public async Task<IActionResult> Search(ProductViewModel model)
        {

        string id = this.userManager.GetUserId(HttpContext.User);

            var Location = "";

            Task<User> SellerNorth = UserReturnNorth(id);
            User userDataNorth = await SellerNorth;
            if (userDataNorth != null)
                Location = "North";

            Task<User2> SellerSouth = UserReturnSouth(id);
            User2 userDataSouth = await SellerSouth;
            if (userDataSouth != null)
                Location = "South";

            if (Location == "North")
            {
                var result = associatedSellRepository.SearchDtos(model.searchTerm);
              
                return View(result);
            }
            else if (Location == "South")
            {
                var result = southAssociatedSellRepository.SearchDtos(model.searchTerm);
            
                return View(result);
            }
            else
            {
                return NotFound();
            }
           
        }
        public async Task<IActionResult> Store(string sellerId)
        {

            ViewBag.user = await userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User));
            ViewBag.id = userManager.GetUserId(HttpContext.User);
            ViewBag.fullUser = HttpContext.User;

            var Location = "";

            Task<User> SellerNorth = UserReturnNorth(sellerId);
            User userDataNorth = await SellerNorth;
            if (userDataNorth != null)
                Location = "North";

            Task<User2> SellerSouth = UserReturnSouth(sellerId);
            User2 userDataSouth = await SellerSouth;
            if (userDataSouth != null)
                Location = "South";


            var vmodel = new ProductViewModel { };
            if (Location == "North")
            {

                vmodel.associatedShared = associatedSharedRepository.FindProducts(sellerId);

                vmodel.associatedSell = associatedSellRepository.FindProducts(sellerId);


                vmodel.associatedSharedRepository = associatedSharedRepository;
            }
            else if (Location == "South")
            {
                vmodel.associatedSharedSouth = southAssociatedSharedRepository.FindProducts(sellerId);

                vmodel.associatedSellSouth = southAssociatedSellRepository.FindProducts(sellerId);


                vmodel.associatedSharedRepository = associatedSharedRepository;
            }
            return View(vmodel);
        }

        public async Task<IActionResult> Product(int productId)
        {
            string id = userManager.GetUserId(HttpContext.User);

            var Location = "";
            var vmodel = new ProductViewModel { };

            Task<User> SellerNorth = UserReturnNorth(id);
            User userDataNorth = await SellerNorth;
            if (userDataNorth != null)
            {
                Location = "North";
                ViewBag.user = userDataNorth;
                vmodel = new ProductViewModel { productDeatails = associatedSellRepository.Find(productId) };
                return View(vmodel);


            }
            Task<User2> SellerSouth = UserReturnSouth(id);
            User2 userDataSouth = await SellerSouth;
            if (userDataSouth != null)
            {
                Location = "South";
                ViewBag.user = userDataSouth;
                 vmodel = new ProductViewModel { productDeatailsSouth = southAssociatedSellRepository.Find(productId) };
                return View(vmodel);

            }
            vmodel = new ProductViewModel { productDeatails = associatedSellRepository.Find(productId) };

            return View(vmodel);

        }
        public async Task<IActionResult> Delete(int id,string sellerId)
        {

            var Location = "";
            Task<User> SellerNorth = UserReturnNorth(sellerId);
            User userDataNorth = await SellerNorth;
            var model = new ProductViewModel
            {

               

            };
            if (userDataNorth != null)
            {
                Location = "North";
                var product = productRepository.Find(id, Location);

                var associatedProduct = associatedSellRepository.Find(id);
                 model = new ProductViewModel
                {

                    ProductBrand = product.ProductBrand,
                    SellerId = associatedProduct.SellerId.ToString(),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                    ProductId = product.ProductId,
                    ProductImagesUrl = product.ProductImageUrls,
                    ProductPrice = product.ProductPrice,

                };

            }
            Task<User2> SellerSouth = UserReturnSouth(sellerId);
            User2 userDataSouth = await SellerSouth;
            if (userDataSouth != null)
            {
                Location = "South";
                var associatedProduct = southAssociatedSellRepository.Find(id);
                var product = productRepository.Find(id, Location);

                 model = new ProductViewModel
                {

                    ProductBrand = product.ProductBrand,
                    SellerId = associatedProduct.SellerId.ToString(),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                    ProductId = product.ProductId,
                    ProductImagesUrl = product.ProductImageUrls,
                    ProductPrice = product.ProductPrice,

                };

            }

       
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            try
            {
                Task<User> SellerNorth = UserReturnNorth(model.SellerId);
                User userDataNorth = await SellerNorth;
                if (userDataNorth != null)
                {
                    associatedSellRepository.Delete(model.ProductId);
                    productRepository.Delete(model.ProductId,"North");
                }
                    Task<User2> SellerSouth = UserReturnSouth(model.SellerId);
                User2 userDataSouth = await SellerSouth;
                if (userDataSouth != null)
                {
                    southAssociatedSellRepository.Delete(model.ProductId);
                    productRepository.Delete(model.ProductId, "South");
                }
               
                return Redirect("/Auth/Dashboard");


            }
            catch
            {
                return View();
            }
        }
    }
}

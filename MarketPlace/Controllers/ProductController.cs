using MarketPlace.Areas.Identity.Data;
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
        private readonly IAssociatedRepository<AssociatedSell> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought> associatedBoughtRepository;

        [Obsolete]
        public ProductController(IProductRepository<Product> productRepository,
            IHostingEnvironment hosting,
            UserManager<User> userManager,
            IAssociatedRepository<AssociatedSell> associatedSellRepository,
            IAssociatedRepository<AssociatedShared> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought> associatedBoughtRepositor
            )
        {
            this.productRepository = productRepository;
            this.hosting = hosting;
            this.userManager = userManager;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;
        }
        public async Task<User> UserReturn(string id)
        {
            return await userManager.FindByIdAsync(id);

        }
        public IActionResult Create()
        {
            ViewBag.id = userManager.GetUserId(HttpContext.User);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            try {
                string fileNames = string.Empty;
                if (model.Files != null)
                {
                    foreach (var file in model.Files) {
                        string upload = Path.Combine(hosting.WebRootPath, "images");
                        fileNames+=(file.FileName);
                        fileNames += "`";
                        string fullPath = Path.Combine(upload, file.FileName);
                        file.CopyTo(new FileStream(fullPath,FileMode.Create));
                            }
                }

                Product product = new Product { 
                ProductBrand = model.ProductBrand,
                ProductDescription = model.ProductDescription,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductImageUrls =fileNames,
                };

                productRepository.Add(product);

                int id = product.ProductId;

                Task<User> Seller  = UserReturn(model.SellerId);
                User userData = await Seller;

                AssociatedSell associatedSell = new AssociatedSell { 
                productId=product,
                SellerId= userData,
                Sold=false
                };

                associatedSellRepository.Add(associatedSell);

                return Redirect("/Auth/Dashboard");


            }
            catch { return View(); }
        }
      
       
    
    public IActionResult Edit(int id)
    {
        var product = productRepository.Find(id);
            var model = new ProductViewModel
            {
                ProductBrand= product.ProductBrand,
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
            try
            {
                string fileNames = string.Empty;
                if (model.Files != null)
                {
                    foreach (var file in model.Files)
                    {
                        string upload = Path.Combine(hosting.WebRootPath, "images");
                        fileNames+=file.FileName;
                        fileNames += '`';
                        string fullPath = Path.Combine(upload, file.FileName);
                        file.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }
                }


                Product product = new Product
                {
                    ProductId = model.ProductId,
                    ProductBrand = model.ProductBrand,
                    ProductDescription = model.ProductDescription,
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    ProductImageUrls = fileNames
                };

                productRepository.Edit(model.ProductId,product);


                return Redirect("/Auth/Dashboard");


            }
            catch { return View(); }
        }


        public async Task<IActionResult> Search(ProductViewModel model)
        {
            string id = userManager.GetUserId(HttpContext.User);

            Task<User> Seller = UserReturn(id);
            User userData = await Seller;

            ViewBag.user = userData;

            var vmodel= new ProductViewModel {SearchedItems= associatedSellRepository.Search(model.searchTerm) };
            return View(vmodel);
        }
        public async Task<IActionResult> Product(int productId)
        {
            string id = userManager.GetUserId(HttpContext.User);

            Task<User> Seller = UserReturn(id);
            User userData = await Seller;

            ViewBag.user = userData;

            var vmodel = new ProductViewModel { productDeatails = associatedSellRepository.Find(productId) };
            return View(vmodel);
        }
        public IActionResult Delete(int id)
        {
            var product = productRepository.Find(id);
            var associatedProduct = associatedSellRepository.Find(id);

            var model = new ProductViewModel
            {
                
                ProductBrand = product.ProductBrand,
                SellerId=associatedProduct.SellerId.ToString(),
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
     
        public IActionResult Delete(int id,ProductViewModel model)
        {
            try
            {
                associatedSellRepository.Delete(model.ProductId);
                productRepository.Delete(model.ProductId);
                return Redirect("/Auth/Dashboard");


            }
            catch {
                return View(); 
            }
        }
    }
}

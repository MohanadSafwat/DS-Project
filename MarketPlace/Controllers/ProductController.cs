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
            IAssociatedRepository<AssociatedBought> associatedBoughtRepository
            )
        {
            this.productRepository = productRepository;
            this.hosting = hosting;
            this.userManager = userManager;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepository;
        }
        public IActionResult Create()
        {
            ViewBag.user = userManager.GetUserId(HttpContext.User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public IActionResult Create(ProductViewModel model)
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

                AssociatedSell associatedSell = new AssociatedSell { 
                productId=product,
                SellerId= model.SellerId,
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
        public IActionResult Edit(ProductViewModel model)
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

        public IActionResult Delete(int id)
        {
            var product = productRepository.Find(id);
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
        public IActionResult ConfirmDelete(int id)
        {
            try
            {
                productRepository.Delete(id);
                return Redirect("/Auth/Dashboard");


            }
            catch { return View(); }
        }
    }
}

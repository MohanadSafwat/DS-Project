using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Hosting;
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

        [Obsolete]
        public ProductController(IProductRepository<Product> productRepository,
            IHostingEnvironment hosting
            )
        {
            this.productRepository = productRepository;
            this.hosting = hosting;
        }
        public IActionResult Create()
        {
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
                Sold =false
             
                };
                productRepository.Add(product);
                

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

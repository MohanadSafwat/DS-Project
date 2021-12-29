using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarketPlace.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository<Product> productRepo;
        [System.Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<User> userManager;
        private readonly IAssociatedRepository<AssociatedSell> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought> associatedBoughtRepositor;

        [System.Obsolete]
        public ProductController(IProductRepository<Product> productRepo,
             IHostingEnvironment hosting,
            IAssociatedRepository<AssociatedSell> associatedSellRepository,
            IAssociatedRepository<AssociatedShared> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought> associatedBoughtRepositor
            )
        {
            this.productRepo = productRepo;
            this.hosting = hosting;
            this.userManager = userManager;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepositor = associatedBoughtRepositor;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public ActionResult GetHomeProducts()
        {
            var productsIndex = associatedSellRepository.List();


            var model = new 
            {
                productsIndex = associatedSellRepository.List(),

                associatedSharedRepository = associatedSharedRepository
            };
            return Ok(model);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

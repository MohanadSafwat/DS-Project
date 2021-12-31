using JWTAuthentication.Authentication;
using JWTAuthentication.Dtos;
using JWTAuthentication.Models;
using MarketPlace.Dtos;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository<Order> orderDbRepository;
        private readonly IOrderRepository<OrderItem> orderItemDbRepository;
        private readonly IProductRepository<Product> productRepository;
        private readonly UserManager<User> userManager;
        private readonly IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepository;
        private readonly IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository;
        private ApplicationDbContext db;
        private readonly ApplicationDb2Context db2;
        private readonly UserManager<User2> userManager2;
        private readonly string _connectionString;
        private readonly IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository;

        public OrderController(IOptions<AppDbConnection> config, IOrderRepository<Order> orderRepository,
            IOrderRepository<OrderItem> orderItemRepository,
            IProductRepository<Product> productRepository,

            UserManager<User> userManager,
            IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository,
            IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepositor,
             IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto> southAssociatedSellRepository,
            IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto> southAssociatedSharedRepository,
            IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto> southAssociatedBoughtRepository,
            ApplicationDbContext _db,
            UserManager<User2> userManager2,
            ApplicationDb2Context _db2
            )
        {
            this.orderDbRepository = orderRepository;
            this.orderItemDbRepository = orderItemRepository;
            this._connectionString = config.Value.ConnectionString;
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepositor;
            this.southAssociatedSellRepository = southAssociatedSellRepository;
            this.southAssociatedSharedRepository = southAssociatedSharedRepository;
            this.southAssociatedBoughtRepository = southAssociatedBoughtRepository;
            this.db = _db;
            this.db2 = _db2;
            this.userManager2 = userManager2;
        }
        // GET: OrderController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public async Task<User> UserReturnNorth(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User2> UserReturnSouth(string id)
        {
            return await userManager2.FindByIdAsync(id);
        }

        // POST: OrderController/createOrder
        [HttpPost]
        [Route("createOrder")]
        public async Task<IActionResult> createOrder([FromForm] OrderCreateDto orderCreateDto)
        {
            //get customer
            Task<User> customer = UserReturnNorth(orderCreateDto.CustomerId);
            User Customer = await customer;
            if (Customer != null)
            {
                //get product
                Product product = productRepository.Find(orderCreateDto.ProductId, Customer.Address);
                if (product == null)
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, "Invalid ProductId");
                }
                AssociatedSell oldSell = associatedSellRepository.Find(orderCreateDto.ProductId);
                if (PerformBuy(product.ProductPrice, Customer, oldSell.SellerId))
                {
                    //create new orderItem
                    OrderItem orderItem = new OrderItem
                    {
                        Product = product,
                        seller = oldSell.SellerId
                    };
                    orderItemDbRepository.Add(orderItem);
                    //create new order
                    Order order = new Order
                    {
                        OrderItem = orderItem,
                        Customer = Customer,
                    };
                    orderDbRepository.Add(order);
                    oldSell.Sold = true;
                    associatedSellRepository.Edit(oldSell);

                    //update AssociatedShared
                    List<AssociatedShared> oldShared = associatedSharedRepository.FindUsers(orderCreateDto.ProductId);
                    List<AssociatedShared> updatedSharedList = new List<AssociatedShared>();
                    foreach (var shared in oldShared)
                    {
                        shared.Sold = true;
                        updatedSharedList.Add(shared);
                    }
                    associatedSharedRepository.EditList(updatedSharedList);
                    //add buyer
                    AssociatedBought associatedBought = new AssociatedBought
                    {
                        Buyer = Customer,
                        product = product
                    };
                    associatedBoughtRepository.Add(associatedBought);
                    db.SaveChanges();
                    return NoContent();
                }
                else
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, "No Enough Amount");
                }

            }
            else
            {
                Task<User2> customerSouth = UserReturnSouth(orderCreateDto.CustomerId);
                User2 CustomerSouth = await customerSouth;
                if (CustomerSouth != null)
                {
                    //get product
                    Product product = productRepository.Find(orderCreateDto.ProductId, CustomerSouth.Address);
                    if (product == null)
                    {
                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, "Invalid ProductId");
                    }
                    AssociatedSellSouth oldSell = southAssociatedSellRepository.Find(orderCreateDto.ProductId);
                    if (PerformBuySouth(product.ProductPrice, CustomerSouth, oldSell.SellerId))
                    {
                        //create new orderItem
                        OrderItemSouth orderItem = new OrderItemSouth
                        {
                            Product = product,
                            seller = oldSell.SellerId
                        };
                        orderItemDbRepository.AddOrderItemSouth(orderItem);
                        //create new order
                        OrderSouth order = new OrderSouth
                        {
                            OrderItemSouth = orderItem,
                            Customer = CustomerSouth,
                        };
                        orderDbRepository.AddOrderSouth(order);
                        oldSell.Sold = true;
                        southAssociatedSellRepository.Edit(oldSell);

                        //update AssociatedShared
                        List<AssociatedSharedSouth> oldShared = southAssociatedSharedRepository.FindUsers(orderCreateDto.ProductId);
                        List<AssociatedSharedSouth> updatedSharedList = new List<AssociatedSharedSouth>();
                        foreach (var shared in oldShared)
                        {
                            shared.Sold = true;
                            updatedSharedList.Add(shared);
                        }
                        southAssociatedSharedRepository.EditList(updatedSharedList);
                        //add buyer
                        AssociatedBoughtSouth associatedBought = new AssociatedBoughtSouth
                        {
                            Buyer = CustomerSouth,
                            product = product
                        };
                        southAssociatedBoughtRepository.Add(associatedBought);
                        db2.SaveChanges();
                        return NoContent();
                    }
                    else
                    {
                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, "No Enough Amount");
                    }
                }
                else
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, "Invalid EmailId");
                }

            }
        }
        [HttpPost]
        [Route("Share")]
        public async Task<IActionResult> Share([FromForm] OrderCreateDto orderCreateDto)
        {
            //get customer
            Task<User> customer = UserReturnNorth(orderCreateDto.CustomerId);
            User Customer = await customer;
            if (Customer != null)
            {
                //get product

                Product product = productRepository.Find(orderCreateDto.ProductId, Customer.Address);
                if (product == null)
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, "Invalid ProductId");
                }


                //Add AssociatedShared
                AssociatedShared newShared = new AssociatedShared
                {
                    SharedId = Customer,
                    productId = product,
                    Sold = false
                };
                associatedSharedRepository.Add(newShared);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                Task<User2> customerSouth = UserReturnSouth(orderCreateDto.CustomerId);
                User2 CustomerSouth = await customerSouth;
                if (CustomerSouth != null)
                {
                    //get product

                    Product product = productRepository.Find(orderCreateDto.ProductId, CustomerSouth.Address);
                    if (product == null)
                    {
                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, "Invalid ProductId");
                    }


                    //Add AssociatedShared
                    AssociatedSharedSouth newShared = new AssociatedSharedSouth
                    {
                        SharedId = CustomerSouth,
                        productId = product,
                        Sold = false
                    };
                    southAssociatedSharedRepository.Add(newShared);
                    db.SaveChanges();
                    return Ok();
                }
                else
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, "Invalid EmailId");

            }


        }
        public async Task<User> UserReturn(string id)
        {
            return await userManager.FindByIdAsync(id);

        }

        private bool PerformBuy(int amount, User cusromer, User seller)
        {
            try
            {
                if (cusromer.Amount >= amount)
                {

                    cusromer.Amount -= amount;
                    seller.Amount += amount;

                    db.SaveChanges();

                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch { return false; }
        }
        private bool PerformBuySouth(int amount, User2 cusromer, User2 seller)
        {
            try
            {
                if (cusromer.Amount >= amount)
                {

                    cusromer.Amount -= amount;
                    seller.Amount += amount;

                    db.SaveChanges();

                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch { return false; }
        }
        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public void Delete(int id)
        {
            int orderItemId = orderDbRepository.Find(id).OrderItem.Id;
            orderDbRepository.Delete(id);
            orderItemDbRepository.Delete(orderItemId);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

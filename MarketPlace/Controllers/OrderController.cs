/*using JWTAuthentication.Authentication;
using JWTAuthentication.Models;
using MarketPlace.Areas.Identity.Data;
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
    public class OrderController : Controller
    {
        private readonly IOrderRepository<Order> orderDbRepository;
        private readonly IOrderRepository<OrderItem> orderItemDbRepository;
        private readonly IProductRepository<Product> productRepository;
        private readonly UserManager<User> userManager;
        private readonly IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepository;
        private AppDBContext db;
        private readonly string _connectionString;

        public OrderController(IOptions<AppDbConnection> config,IOrderRepository<Order> orderRepository,
            IOrderRepository<OrderItem> orderItemRepository,
            IProductRepository<Product> productRepository,

             UserManager<User> userManager,
             IAssociatedRepository<AssociatedSell, ProductSellerReadDto> associatedSellRepository,
             IAssociatedRepository<AssociatedShared, ProductSharedReadDto> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought, ProductBoughtReadDto> associatedBoughtRepositor,
            AppDBContext _db)
        {
            this.orderDbRepository = orderRepository;
            this.orderItemDbRepository = orderItemRepository;
            this._connectionString = config.Value.ConnectionString;
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepositor;
            this.db = _db;
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

        // GET: OrderController/Create
        public async Task<IActionResult> Create(int productId,string customerId )
        {
            //get product
            Product product = productRepository.Find(productId);
            
            //get customer
            Task<User> customer = UserReturn(customerId);
            User Customer = await customer;

            AssociatedSell oldSell = associatedSellRepository.Find(productId);
            if (PerformBuy(product.ProductPrice, Customer, oldSell.SellerId))
            {
                //create new orderItem
                OrderItem orderItem = new OrderItem
                {
                    Product = product,
                    seller = oldSell.SellerId
                };
                orderItemDbRepository.Add(orderItem);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    String query = "INSERT INTO dbo.Transactions (CustomerEmail,SellerEmail,ItemName,ItemPrice) VALUES (@CustomerEmail,@SellerEmail, @ItemName,@ItemPrice)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerEmail", Customer.Email);
                        command.Parameters.AddWithValue("@SellerEmail", oldSell.SellerId.Email);
                        command.Parameters.AddWithValue("@ItemName", orderItem.Product.ProductName);
                        command.Parameters.AddWithValue("@ItemPrice", orderItem.Product.ProductPrice);

                        // connection.ServerVersion = 'connection.ServerVersion' threw an exception of type 'System.InvalidOperationException'
                        connection.Open();

                        int result = command.ExecuteNonQuery();

                        // Check Error
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                }

                //create new order
                Order order = new Order
                {
                    OrderItem = orderItem,
                    Customer = Customer,
                };
                orderDbRepository.Add(order);

                //update AssociatedSell
                *//*  AssociatedSell updatedSell = new AssociatedSell
                  {
                      id = oldSell.id,
                      productId = product,
                      SellerId = oldSell.SellerId,
                      Sold = true
                  };*//*
                oldSell.Sold = true;
                associatedSellRepository.Edit(oldSell);

                //update AssociatedShared
                List<AssociatedShared> oldShared = associatedSharedRepository.FindUsers(productId);
                List<AssociatedShared> updatedSharedList = new List<AssociatedShared>();
                foreach (var shared in oldShared)
                {
                    *//* AssociatedShared updatedShared = new AssociatedShared
                     {
                         id = shared.id,
                         SharedId = shared.SharedId,
                         productId = shared.productId,
                         Sold = true
                     };*//*
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
                return Redirect("/Auth/Dashboard");
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        public async Task<IActionResult> Share(int productId, string customerId)
        {
            //get product
            Product product = productRepository.Find(productId);

            //get customer
            Task<User> customer = UserReturn(customerId);
            User Customer = await customer;
                //Add AssociatedShared
            AssociatedShared newShared = new AssociatedShared
                {
                    SharedId = Customer,
                    productId = product,
                    Sold = false
                };
            associatedSharedRepository.Add(newShared);
            db.SaveChanges();
            return Redirect("/Auth/Dashboard");
            
          
        }
        public async Task<User> UserReturn(string id)
        {
            return await userManager.FindByIdAsync(id);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool PerformBuy (int amount,User cusromer,User seller )
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
                    System.Diagnostics.Debug.WriteLine("no");
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
*/
using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderDbRepository orderDbRepository;
        private readonly OrderItemDbRepository orderItemDbRepository;
        private readonly IProductRepository<Product> productRepository;
        private readonly UserManager<User> userManager;
        private readonly IAssociatedRepository<AssociatedSell> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought> associatedBoughtRepository;
        private AppDBContext db;

        public OrderController(OrderDbRepository orderRepository,
            OrderItemDbRepository orderItemRepository,
            IProductRepository<Product> productRepository,
             UserManager<User> userManager,
             IAssociatedRepository<AssociatedSell> associatedSellRepository,
             IAssociatedRepository<AssociatedShared> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought> associatedBoughtRepositor,
            AppDBContext _db)
        {
            this.orderDbRepository = orderRepository;
            this.orderItemDbRepository = orderItemRepository;
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
            if(PerformBuy(product.ProductPrice, Customer, oldSell.SellerId))
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

                //update AssociatedSell
                AssociatedSell updatedSell = new AssociatedSell
                {
                    id = oldSell.id,
                    productId = product,
                    SellerId = oldSell.SellerId,
                    Sold = true
                };
                associatedSellRepository.Edit(updatedSell);

                //update AssociatedShared
                List<AssociatedShared> oldShared = associatedSharedRepository.FindUsers(productId);
                List<AssociatedShared> updatedSharedList = new List<AssociatedShared>();
                foreach (var shared in oldShared)
                {
                    AssociatedShared updatedShared = new AssociatedShared
                    {
                        id = shared.id,
                        SharedId = shared.SharedId,
                        productId = shared.productId,
                        Sold = true
                    };
                    updatedSharedList.Add(updatedShared);
                }
                associatedSharedRepository.EditList(updatedSharedList);
                //add buyer
                AssociatedBought associatedBought = new AssociatedBought
                {
                    Buyer = Customer,
                    product = product
                };
                associatedBoughtRepository.Add(associatedBought);
                return Redirect("/Auth/Dashboard");
            }else
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
                    return false;
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

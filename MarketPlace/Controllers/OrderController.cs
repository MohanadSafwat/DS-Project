using MarketPlace.Models;
using MarketPlace.Models.Repository;
using Microsoft.AspNetCore.Http;
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

        public OrderController(OrderDbRepository orderRepository,
            OrderItemDbRepository orderItemRepository)
        {
            this.orderDbRepository = orderRepository;
            this.orderItemDbRepository = orderItemRepository;    
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
        public void Create(int productId,int sellerId,int customerId )
        {
            OrderItem orderItem = new OrderItem
            {
                ProductId = productId,
                sellerId = sellerId
            };
            orderItemDbRepository.Add(orderItem);
            Order order = new Order
            {
                CustomerId=customerId,
                OrderItem= orderItem,
            };
            orderDbRepository.Add(order);
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

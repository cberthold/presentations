using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Services;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CartService cartService;
        private readonly OrderService orderService;

        public HomeController(ILogger<HomeController> logger, CartService cartService, OrderService orderService)
        {
            _logger = logger;
            this.cartService = cartService;
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await cartService.LoadCart();
            if (cart.Count == 0)
            {
                await cartService.AddCartItemAsync("Item 1", 47.35m);
                await cartService.AddCartItemAsync("Item 2", 12.59m);
                await cartService.AddCartItemAsync("Item 3", 7.11m);
                await cartService.AddCartItemAsync("Item 4", 9.11m);
                cart = await cartService.AddCartItemAsync("Item 5", 86753.09m);
            }

            return View(cart);
        }

        public async Task<IActionResult> RemoveItem(string id)
        {
            var cart = await cartService.RemoveCartItemAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PlaceOrder()
        {
            var cart = await cartService.LoadCart();
            var order = await orderService.PlaceOrder(cart.Id);
            return RedirectToAction(nameof(OrderPlaced), new { OrderDate = order.DatePartitionKey, OrderKey = order.OrderRowKey });
        }

        public async Task<IActionResult> OrderPlaced(string orderDate, string orderKey)
        {
            Order order = await orderService.GetOrder(orderDate, orderKey);
            return View(order);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

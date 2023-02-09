using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spa_core_final_project.Models;
using spa_core_final_project.ViewModels;
using System.Diagnostics;

namespace spa_core_final_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SellManagementDBContext db;

        public HomeController(ILogger<HomeController> logger, SellManagementDBContext db)
        {
            _logger = logger;
            this.db = db;
        }
        public IActionResult Index()
        {
            DataVM data = new DataVM();
            data.Categories = db.Categories.OrderByDescending(x => x.CategoryId).ToList();
            data.Customers = db.Customers.OrderByDescending(x => x.CustomerId).ToList();
            data.Products = db.Products.OrderByDescending(x => x.ProductId).ToList();
            data.Orders = db.Orders.OrderByDescending(x => x.OrderId).ToList();

            ViewBag.orders = db.Orders.ToList();
            ViewBag.products = db.Products.ToList();
            ViewBag.orderDetails = db.OrderDetails.ToList();
            return View(data);
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
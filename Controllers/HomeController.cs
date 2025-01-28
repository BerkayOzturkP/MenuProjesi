using System.Data;
using System.Diagnostics;
using Dapper;
using MenuProjesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MenuProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbConnection _connection;

        public HomeController(IDbConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            
            var categories = _connection.Query<Categories>("SELECT * FROM Categories WHERE IsActive = 1").ToList();

            
            foreach (var cat in categories)
            {
                var products = _connection.Query<Products>(
                    "SELECT * FROM Products WHERE CategoryId = @cId AND IsActive = 1",
                    new { cId = cat.Id }).ToList();

                
                cat.Products = products;
            }

            
            return View(categories);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}

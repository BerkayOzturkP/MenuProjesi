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
            var categories = _connection.Query<Categories>("SELECT * FROM Categories WHERE IsActive = 1 ");

            return View(categories);
        }

        

        

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}

using MenuProjesi.Models;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;

namespace MenuProjesi.Controllers
{
    public class EditorController : Controller
    {
        private readonly IDbConnection _connection;

        public EditorController(IDbConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Editor(int? editId)
        {
            var categories = _connection.Query<Categories>("SELECT * FROM Categories WHERE IsActive = 1");

            // editId boş değilse, düzenlenecek kategoriyi temsil eder
            if (editId.HasValue)
            {
                ViewData["EditingId"] = editId.Value;
            }

            return View(categories);
        }


        [HttpPost]
        public IActionResult AddCategory([FromForm] Categories model)
        {
            model.IsActive = true;
            model.CreatedDate = DateTime.Now;
            _connection.Execute("INSERT INTO Categories (CategoryName, ProductCount, IsActive, CreatedDate, UpdatedDate) VALUES (@CategoryName, @ProductCount, @IsActive, @CreatedDate, @UpdatedDate)", model);

            return RedirectToAction("Editor" , "Editor");
        }

        [HttpPost]
        public IActionResult Edit(int id, string CategoryName)
        {
            _connection.Execute("UPDATE Categories SET CategoryName = @CategoryName, UpdatedDate = @UpdatedDate WHERE Id = @Id",
                new { Id = id, CategoryName = CategoryName, UpdatedDate = DateTime.Now });

            return RedirectToAction("Editor", "Editor");
        }

    }
}

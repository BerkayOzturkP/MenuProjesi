using System.Data;
using Dapper;
using MenuProjesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MenuProjesi.Controllers
{
    public class ProductsController : Controller
    {

            private readonly IDbConnection _connection;

            public ProductsController(IDbConnection connection)
            {
                _connection = connection;
            }

            
            public IActionResult Manage(int id)           
            {
                var categoryName = _connection.QuerySingleOrDefault<string>("SELECT CategoryName FROM Categories WHERE Id = @Id", new { Id = id });

                var products = _connection.Query<Products>("SELECT * FROM Products WHERE CategoryId = @CategoryId AND IsActive = 1", new { CategoryId = id }).ToList();

                ViewBag.CategoryId = id;
                ViewBag.CategoryName = categoryName;

                return View(products);
            }

            [HttpPost]
            public IActionResult AddProduct(Products model, IFormFile? ImageFile)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    model.ImagePath = "/images/" + uniqueFileName;
                }

                model.IsActive = true;
                model.CreatedDate = DateTime.Now;


                _connection.Execute("INSERT INTO Products (CategoryId, Name, Description, Price, ImagePath, IsActive, CreatedDate) VALUES (@CategoryId, @Name, @Description, @Price, @ImagePath, @IsActive, @CreatedDate)", model);

                return RedirectToAction("Manage", new { id = model.CategoryId });
            }

            [HttpPost]
            public IActionResult DeleteProduct(int productId, int categoryId)
            {
                
                _connection.Execute("UPDATE Products SET IsActive=0 WHERE Id=@Id", new { Id = productId });

                return RedirectToAction("Manage", new { id = categoryId });
            }

    }
}


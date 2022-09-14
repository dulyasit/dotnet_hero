using dotnet_hero.Data;
using dotnet_hero.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace dotnet_hero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public ProductsController(DatabaseContext databaseContext) => this.databaseContext = databaseContext;
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return databaseContext.Products.OrderByDescending(p => p.ProductId).ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var result = databaseContext.Products.Find(id);
            if (result == null)
                return NotFound();
            return result;
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<Product>> SearchProducts([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
                name = "";
            var result = databaseContext.Products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToList();
            //if (result == null)
            //    return NotFound();
            return result;
        }
        [HttpPost]
        public ActionResult<Product> AddProduct([FromForm] Product model)
        {
            databaseContext.Products.Add(model);
            databaseContext.SaveChanges();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, [FromForm] Product model)
        {
            if (id != model.ProductId)
            {
                return BadRequest();
            }
            Product result = databaseContext.Products.Find(id);

            if (result == null)
            {
                return NotFound();
            }
            result.Name = model.Name;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.CategoryId = model.CategoryId; 

            databaseContext.Products.Update(result);
            databaseContext.SaveChanges();

            return StatusCode((int)HttpStatusCode.NoContent);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            Product result = databaseContext.Products.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            databaseContext.Remove(result);
            databaseContext.SaveChanges();

            return NoContent();
        }
    }
}

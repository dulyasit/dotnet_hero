using dotnet_hero.Data;
using dotnet_hero.DTOs.Product;
using dotnet_hero.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            return databaseContext.Products.Include(p => p.Category)
                .OrderByDescending(p => p.ProductId).Select(ProductResponse.FromProduct)
                .ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            var result = databaseContext.Products.Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == id);
            if (result == null)
                return NotFound();
            return ProductResponse.FromProduct(result);
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<ProductResponse>> SearchProducts([FromQuery] string name = "")
        {
            var result = databaseContext.Products.Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .Select(ProductResponse.FromProduct)
                .ToList();
            //if (result == null)
            //    return NotFound();
            return result;
        }
        [HttpPost]
        public ActionResult<Product> AddProduct([FromForm] ProductRequest productRequest)
        {
            Product product = productRequest.Adapt<Product>();
            
            databaseContext.Products.Add(product);
            databaseContext.SaveChanges();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, [FromForm] ProductRequest productRequest)
        {
            if (id != productRequest.ProductId)
            {
                return BadRequest();
            }
            Product result =  databaseContext.Products.Find(id);

            if (result == null)
            {
                return NotFound();
            }
            productRequest.Adapt(result);

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

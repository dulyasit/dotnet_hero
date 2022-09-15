using dotnet_hero.Data;
using dotnet_hero.DTOs.Product;
using dotnet_hero.Entities;
using dotnet_hero.Interfaces;
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
        private readonly IProductService productService;

        public ProductsController(IProductService productService) => this.productService = productService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            return (await productService.FindAll())
                .Select(ProductResponse.FromProduct).ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await productService.FindById(id);
            if (product == null)
                return NotFound();
            return product.Adapt<ProductResponse>();
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> SearchProducts([FromQuery] string name = "")
        {
            return (await productService.Search(name))
                .Select(ProductResponse.FromProduct)
                .ToList();
        }
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest)
        {
            Product product = productRequest.Adapt<Product>();
            await productService.Create(product);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromForm] ProductRequest productRequest)
        {
            if (id != productRequest.ProductId)
            {
                return BadRequest();
            }
            Product product = await productService.FindById(id);

            if (product == null)
            {
                return NotFound();
            }
            productRequest.Adapt(product);
            await productService.Update(product);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Product product = await productService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            await productService.Delete(product);
            return NoContent();
        }
    }
}

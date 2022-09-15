using dotnet_hero.Data;
using dotnet_hero.DTOs.Product;
using dotnet_hero.Entities;
using dotnet_hero.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace dotnet_hero.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext databaseContext;

        public ProductService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task Create(Product product)
        {
            databaseContext.Products.Add(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            databaseContext.Remove(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            return await databaseContext.Products.Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();
        }

        public async Task<Product> FindById(int id)
        {
            return await databaseContext.Products.Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            return await databaseContext.Products.Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task Update(Product product)
        {
            databaseContext.Products.Update(product);
            await databaseContext.SaveChangesAsync();
        }
    }
}

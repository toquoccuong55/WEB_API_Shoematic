using DatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseService.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public IQueryable<Product> GetAllProducts()
        {
            return shoematicContext.Set<Product>();
        }

        public Product GetProduct(int id) {
            Product product = shoematicContext.Products.FirstOrDefault(p => p.ID == id);
            return product;
        }
    }
}
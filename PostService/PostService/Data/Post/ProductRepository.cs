using PostService.Data.AccountMock;
using PostService.Entities;
using PostService.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.Post
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostDbContext _context;

        public ProductRepository(PostDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts(string productName = null)
        {       
            return _context.Products.Where(e => (productName == null || e.PostName == productName)).ToList();
        }

        public List<Product> GetProductsByAccountId(Guid id)
        {
            return _context.Products.Where(e => (e.AccountId == id)).ToList();
        }

        public Product GetProductById(Guid id)
        {
            return _context.Products.FirstOrDefault(e => e.PostId == id);
        }

        public void CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product oldProduct, Product newProduct)
        {
            throw new NotImplementedException();
        }
    }
}

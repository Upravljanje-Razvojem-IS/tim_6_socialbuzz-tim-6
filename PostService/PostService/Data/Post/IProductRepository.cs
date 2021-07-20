using PostService.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.Post
{
    public interface IProductRepository
    {
        List<Product> GetProducts(string productName = null);
        List<Product> GetProductsByAccountId(Guid id);
        Product GetProductById(Guid id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid id);
    }
}

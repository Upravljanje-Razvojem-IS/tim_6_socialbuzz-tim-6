using AutoMapper;
using PostService.Data.AccountMock;
using PostService.Data.PostHistories;
using PostService.Entities;
using PostService.Entities.Posts;
using PostService.Models.DTOs.PostHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.Post
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostDbContext _context;
        private readonly IPostHistoryRepository _postHistoryRepository;

        public ProductRepository(PostDbContext context, IPostHistoryRepository postHistoryRepository)
        {
            _context = context;
            _postHistoryRepository = postHistoryRepository;
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
            _context.Products.Add(product);

            PostHistory postHistory = new PostHistory();
            postHistory.Price = product.Price;
            postHistory.PostId = product.PostId;
            _postHistoryRepository.CreatePostHistory(postHistory);
        }

        public void DeleteProduct(Guid id)
        {
            var product = GetProductById(id);
            _context.Remove(product);
        }

        public void UpdateProduct(Product oldProduct, Product newProduct)
        {
            oldProduct.PostName = newProduct.PostName;
            oldProduct.PostImage = newProduct.PostImage;
            oldProduct.Description = newProduct.Description;
            if (oldProduct.Price != newProduct.Price)
            {
                oldProduct.Price = newProduct.Price;
                PostHistory postHistory = new PostHistory();
                postHistory.Price = newProduct.Price;
                postHistory.PostId = oldProduct.PostId;
                _postHistoryRepository.CreatePostHistory(postHistory);
            }
            oldProduct.Currency = newProduct.Currency;
            oldProduct.Category = newProduct.Category;
            oldProduct.AccountId = newProduct.AccountId;
            oldProduct.Weight = newProduct.Weight;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

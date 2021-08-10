using PostService.Data.PostHistories;
using PostService.Entities;
using PostService.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.Post
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly PostDbContext _context;
        private readonly IPostHistoryRepository _postHistoryRepository;

        public ServiceRepository(PostDbContext context, IPostHistoryRepository postHistoryRepository)
        {
            _context = context;
            _postHistoryRepository = postHistoryRepository;
        }

        public List<Service> GetServices(string serviceName = null)
        {
            return _context.Services.Where(e => (serviceName == null || e.PostName == serviceName)).ToList();
        }

        public List<Service> GetServicesByAccountId(Guid id)
        {
            return _context.Services.Where(e => (e.AccountId == id)).ToList();
        }

        public Service GetServiceById(Guid id)
        {
            return _context.Services.FirstOrDefault(e => e.PostId == id);
        }

        public void CreateService(Service service)
        {
            _context.Services.Add(service);

            PostHistory postHistory = new PostHistory();
            postHistory.Price = service.Price;
            postHistory.PostId = service.PostId;
            _postHistoryRepository.CreatePostHistory(postHistory);
        }

        public void DeleteService(Guid id)
        {
            var service = GetServiceById(id);
            _context.Remove(service);
        }

        public void UpdateService(Service oldService, Service newService)
        {
            oldService.PostName = newService.PostName;
            oldService.PostImage = newService.PostImage;
            oldService.Description = newService.Description;
            if (oldService.Price != newService.Price)
            {
                oldService.Price = newService.Price;
                PostHistory postHistory = new PostHistory();
                postHistory.Price = newService.Price;
                postHistory.PostId = oldService.PostId;
                _postHistoryRepository.CreatePostHistory(postHistory);
            }
            oldService.Currency = newService.Currency;
            oldService.Category = newService.Category;
            oldService.AccountId = newService.AccountId;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

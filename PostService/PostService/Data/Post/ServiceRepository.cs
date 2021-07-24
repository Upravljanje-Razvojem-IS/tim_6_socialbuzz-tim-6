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

        public ServiceRepository(PostDbContext context)
        {
            _context = context;
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
        }

        public void DeleteService(Guid id)
        {
            var service = GetServiceById(id);
            _context.Remove(service);
        }

        public void UpdateService(Service oldService, Service newService)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

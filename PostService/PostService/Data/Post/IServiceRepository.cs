using PostService.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.Post
{
    public interface IServiceRepository
    {
        List<Service> GetServices(string serviceName = null);
        List<Service> GetServicesByAccountId(Guid id);
        Service GetServiceById(Guid id);
        void CreateService(Service service);
        void UpdateService(Service service);
        void DeleteService(Guid id);
    }
}

using ReactionService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReactionService.ServiceCalls
{
    public interface IPostService
    {
        public Task<T> GetPostById<T>(HttpMethod method, Guid postId, string authorizationToken);
    }
}

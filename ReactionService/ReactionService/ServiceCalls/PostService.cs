using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReactionService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReactionService.ServiceCalls
{
    public class PostService : IPostService
    {
        private readonly IConfiguration configuration;

        public PostService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<T> GetPostById<T>(HttpMethod method, Guid postId, string jwtToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{ configuration["Services:PostService"] }api/posts/" + postId);
                HttpRequestMessage request = new HttpRequestMessage(method, url);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Authorization", jwtToken);

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content))
                    {
                        return default(T);
                    }

                    return JsonConvert.DeserializeObject<T>(content);
                }
                return default(T);
            }
        }
    }
}

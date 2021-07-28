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

        public async Task<T> GetPostById<T>(HttpMethod method, Guid postId, string jwtToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri("https://localhost:44377/api/posts/" + postId);
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
                //throw new Exception($"There was an error while communicating with PostService! Code: {response.StatusCode}, Message: {response.ReasonPhrase}");
            }
        }
    }
}

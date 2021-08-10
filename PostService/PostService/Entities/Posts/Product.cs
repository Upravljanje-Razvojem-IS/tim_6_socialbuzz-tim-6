using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities.Posts
{
    public class Product: Post
    {
        /// <summary>
        /// Weight of the product
        /// </summary>
        public String Weight { get; set; }
    }
}

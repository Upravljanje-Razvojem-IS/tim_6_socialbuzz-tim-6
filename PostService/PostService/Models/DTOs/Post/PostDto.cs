using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.Post
{
    public class PostDto
    {
        /// <summary>
        /// Unique identifier for the post
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Name of the post
        /// </summary>
        public String PostName { get; set; }

        /// <summary>
        /// Image of the post
        /// </summary>
        public String PostImage { get; set; }

        /// <summary>
        /// Description of the post
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Price of the post
        /// </summary>
        public Double Price { get; set; }

        /// <summary>
        /// Payment currency
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// Category to which the post belongs
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// Type of the post (product or service)
        /// </summary>
        public String PostType { get; set; }

        /// <summary>
        /// Date when the post was published
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Id of the user's account who posted the post
        /// </summary>
        public Guid AccountId { get; set; }
    }
}

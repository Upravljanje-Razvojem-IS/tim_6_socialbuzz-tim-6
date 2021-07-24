using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.Post
{
    public class ProductConfirmationDto
    {
        /// <summary>
        /// Unique identifier for the post(product)
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Name of the post(product)
        /// </summary>
        public String PostName { get; set; }

        /// <summary>
        /// Image of the post(product)
        /// </summary>
        public String PostImage { get; set; }

        /// <summary>
        /// Description of the post(product)
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Price of the post(product) with currency
        /// </summary>
        public String Price { get; set; }

        /// <summary>
        /// Category to which the post(product) belongs
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// Date when the post(product) was published
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Id of the user's account who posted the post
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Weight of the product
        /// </summary>
        public String Weight { get; set; }
    }
}

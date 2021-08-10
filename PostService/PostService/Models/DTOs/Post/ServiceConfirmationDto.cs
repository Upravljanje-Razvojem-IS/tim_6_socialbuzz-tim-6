using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.Post
{
    public class ServiceConfirmationDto
    {
        /// <summary>
        /// Unique identifier for the post(service)
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Name of the post(service)
        /// </summary>
        public String PostName { get; set; }

        /// <summary>
        /// Image of the post(service)
        /// </summary>
        public String PostImage { get; set; }

        /// <summary>
        /// Description of the post(service)
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Price of the post(service) with currency
        /// </summary>
        public String Price { get; set; }

        /// <summary>
        /// Category to which the post(service) belongs
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// Id of the user's account who posted the post
        /// </summary>
        public Guid AccountId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.Post
{
    public class ServiceCreationDto
    {
        /// <summary>
        /// Name of the post(service)
        /// </summary>
        [Required(ErrorMessage = "PostName is required.")]
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
        /// Price of the post(service)
        /// </summary>
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater then 0")]
        public Double Price { get; set; }

        /// <summary>
        /// Payment currency
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        public String Currency { get; set; }

        /// <summary>
        /// Category to which the post(service) belongs
        /// </summary>
        [Required(ErrorMessage = "Category is required.")]
        public String Category { get; set; }

        /// <summary>
        /// Id of the user's account who posted the post
        /// </summary>
        [Required(ErrorMessage = "AccountId is required.")]
        public Guid AccountId { get; set; }
    }
}

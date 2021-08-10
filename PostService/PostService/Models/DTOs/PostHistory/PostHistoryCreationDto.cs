using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.PostHistory
{
    public class PostHistoryCreationDto
    {
        /// <summary>
        /// Price for the post in a certain period of time
        /// </summary>
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater then 0")]
        public Double Price { get; set; }

        /// <summary>
        /// Id of the post to which a certain price applies
        /// </summary>
        [Required]
        public Guid PostId { get; set; }
    }
}

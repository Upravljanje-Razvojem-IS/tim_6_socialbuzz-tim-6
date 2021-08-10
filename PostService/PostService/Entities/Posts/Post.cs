using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities.Posts
{
    /// <summary>
    /// Entity class which models different kind of posts (products and services)
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Unique identifier for the post
        /// </summary>
        [Key]
        [Required]
        public Guid PostId { get; set; }

        /// <summary>
        /// Name of the post
        /// </summary>
        [StringLength(40)]
        [Required]
        public String PostName { get; set; }

        /// <summary>
        /// Image of the post
        /// </summary>
        public String PostImage { get; set; }


        /// <summary>
        /// Description of the post
        /// </summary>
        [StringLength(500)]
        [Required]
        public String Description { get; set; }

        /// <summary>
        /// Price of the post
        /// </summary>
        [Required]
        public Double Price { get; set; }

        /// <summary>
        /// Payment currency
        /// </summary>
        [StringLength(5)]
        [Required]
        public String Currency { get; set; }


        /// <summary>
        /// Category to which the post belongs
        /// </summary>
        [StringLength(50)]
        [Required]
        public String Category { get; set; }

        /// <summary>
        /// Date when the post was published
        /// </summary>
        [Required]
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Id of the user's account who posted the post
        /// </summary>
        public Guid AccountId { get; set; }

    }
}

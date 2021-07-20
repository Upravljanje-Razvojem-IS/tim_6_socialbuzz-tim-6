using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities
{
    public class PostHistory
    {
        /// <summary>
        /// Unique identifier for the post history
        /// </summary>
        [Key]
        [Required]
        public int PostHistoryId { get; set; }

        /// <summary>
        /// Price for the post in a certain period of time
        /// </summary>
        [Required]
        public Double Price { get; set; }

        /// <summary>
        /// Date from which a certain post's price applies
        /// </summary>
        [Required]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Date to which a certain post's price applies
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Id of the post to which a certain price applies
        /// </summary>
        [ForeignKey("PostId")]
        public Guid PostId { get; set; }
    }
}

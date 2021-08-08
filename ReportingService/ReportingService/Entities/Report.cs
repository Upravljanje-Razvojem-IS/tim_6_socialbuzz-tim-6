using System;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.Entities
{
    public class Report
    {
        /// <summary>
        /// Primary key Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key id of post
        /// </summary>
        [Required]
        public Guid PostId { get; set; }
        /// <summary>
        /// Number of likes per post
        /// </summary>
        [Required]
        public int NumberOfLikes { get; set; }
        /// <summary>
        /// Number of comments per post
        /// </summary>
        [Required]
        public int NumberOfComments { get; set; }
        /// <summary>
        /// Number of marks per post
        /// </summary>
        [Required]
        public int NumberOfMarks { get; set; }
    }
}

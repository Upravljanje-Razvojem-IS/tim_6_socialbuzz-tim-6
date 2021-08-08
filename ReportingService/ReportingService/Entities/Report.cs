using System;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.Entities
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public int NumberOfLikes { get; set; }
        [Required]
        public int NumberOfComments { get; set; }
        [Required]
        public int NumberOfMarks { get; set; }
    }
}

using System;

namespace ReportingService.DTOs
{
    public class ReportCreate
    {
        public Guid PostId { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfMarks { get; set; }
    }
}

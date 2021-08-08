using System;

namespace ReportingService.DTOs
{
    public class ReportConfirm
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfMarks { get; set; }
    }
}

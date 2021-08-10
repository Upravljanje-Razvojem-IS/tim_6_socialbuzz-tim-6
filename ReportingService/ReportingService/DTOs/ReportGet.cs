using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingService.DTOs
{
    public class ReportGet
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfMarks { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model
{
    /// <summary>
    /// DTO for comment modification
    /// </summary>
    public class CommentingUpdateDto
    {
        /// <summary>
        /// Comment ID
        /// </summary>
        /// <example>e48da32e-f8d3-4b2e-aaff-2a3a4827188b</example>
        public Guid CommentID { get; set; }

        /// <summary>
        /// Comment content
        /// </summary>
        /// <example>Good stuff.</example>
        [Required(ErrorMessage = "Comment text is required!")]
        public String CommentText { get; set; }
    }
}

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
        public Guid CommentID { get; set; }

        /// <summary>
        /// Comment content
        /// </summary>
        [Required(ErrorMessage = "Comment text is required!")]
        public String CommentText { get; set; }
    }
}
